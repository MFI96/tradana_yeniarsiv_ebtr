namespace TwistedBuddy
{
    using System;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;
    using EloBuddy.SDK.Rendering;

    using SharpDX;

    internal class Program
    {
        /// <summary>
        /// Q
        /// </summary>
        public static Spell.Skillshot Q;

        /// <summary>
        /// W
        /// </summary>
        public static Spell.Active W;

        /// <summary>
        /// E
        /// </summary>
        public static Spell.Active E;

        /// <summary>
        /// R
        /// </summary>
        public static Spell.Active R;

        /// <summary>
        /// Twisted Fate's Name
        /// </summary>
        public const string ChampionName = "TwistedFate";

        /// <summary>
        /// Called when program starts
        /// </summary>
        private static void Main()
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        /// <summary>
        /// Called when the game finishes loading.
        /// </summary>
        /// <param name="args">The Args.</param>
        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.BaseSkinName != ChampionName)
            {
                return;
            }

            Bootstrap.Init(null);

            Q = new Spell.Skillshot(SpellSlot.Q, 1450, SkillShotType.Linear, 0, 1000, 40);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Active(SpellSlot.E);
            R = new Spell.Active(SpellSlot.R, 5500);

            // Menu
            Essentials.MainMenu = MainMenu.AddMenu("Twisted Fate", "TwistedFate");

            // Card Selector Menu
            Essentials.CardSelectorMenu = Essentials.MainMenu.AddSubMenu("Card Seçme Menüsü", "csMenu");
            Essentials.CardSelectorMenu.AddGroupLabel("Card Seçme Ayarları");
            Essentials.CardSelectorMenu.Add("useY", new KeyBind("Sarı Kart Kullan", false, KeyBind.BindTypes.HoldActive, "W".ToCharArray()[0]));
            Essentials.CardSelectorMenu.Add("useB", new KeyBind("Mavi Kart Kullan", false, KeyBind.BindTypes.HoldActive, "E".ToCharArray()[0]));
            Essentials.CardSelectorMenu.Add("useR", new KeyBind("Kırmızı Kart Kullan", false, KeyBind.BindTypes.HoldActive, "T".ToCharArray()[0]));

            // Combo
            Essentials.ComboMenu = Essentials.MainMenu.AddSubMenu("Combo Menu", "comboMenu");
            Essentials.ComboMenu.AddGroupLabel("Kombo Ayarları");
            Essentials.ComboMenu.Add("useQ", new CheckBox("Komboda Q kullan"));
            Essentials.ComboMenu.Add("useCard", new CheckBox("Komboda W kullan"));
            Essentials.ComboMenu.Add("useQStun", new CheckBox("Sadece Stunluysa Q kullan", false));
            Essentials.ComboMenu.Add("qPred", new Slider("Q İsabet Oranı %", 75));
            Essentials.ComboMenu.Add("wSlider", new Slider("Range from enemy before picking card (Not including the additional range)", 300, 0, 10000));
            Essentials.ComboMenu.Add("manaManagerQ", new Slider("How much mana before using Q", 25));
            Essentials.ComboMenu.AddGroupLabel("Card Seçici Ayarları");
            Essentials.ComboMenu.Add("chooser", new ComboBox("Kart Seçme Modu", new[] { "Akıllıca", "Mavi", "Kırmızı", "Sarı" }));
            Essentials.ComboMenu.Add("enemyW", new Slider("Kırmızı Kart Seçmek İçin Kaç Düşman Gerekli? (Akıllıca)", 4, 1, 5));
            Essentials.ComboMenu.Add("manaW", new Slider("Mavi Kardı Ne Kadar manam kalırsa Seçeyim? (Akıllıca)", 25));

            // Harass Menu
            Essentials.HarassMenu = Essentials.MainMenu.AddSubMenu("Dürtme", "harassMenu");
            Essentials.HarassMenu.AddGroupLabel("Dürtme Ayarları");
            Essentials.HarassMenu.Add("useQ", new CheckBox("Dürtmede Q kullan"));
            Essentials.HarassMenu.Add("useCard", new CheckBox("Dürtmede W Kullan"));
            Essentials.HarassMenu.Add("qPred", new Slider("Q İsabet Oranı %", 75));
            Essentials.HarassMenu.Add("wSlider", new Slider("Kart Seçmeden Önce Düşman Uzaklığı (Değiştirilen Çeviri:Dokunmayın)", 300, 0, 10000));
            Essentials.HarassMenu.Add("manaManagerQ", new Slider("Q dan önce ne kadar manam olsun", 25));
            Essentials.HarassMenu.AddGroupLabel("Card Seçici Ayarlar");
            Essentials.HarassMenu.Add("chooser", new ComboBox("Card Seçme Modu", new[] { "Akıllıca", "Mavi", "Kırmızı", "Sarı" }));
            Essentials.HarassMenu.Add("enemyW", new Slider("Kırmızı Kart Seçmek İçin Kaç Düşman Gerekli? (Akıllıca)", 3, 1, 5));
            Essentials.HarassMenu.Add("manaW", new Slider("Mavi Kardı Ne Kadar manam kalırsa Seçeyim? (Akıllıca)", 25));

            // Lane Clear Menu
            Essentials.LaneClearMenu = Essentials.MainMenu.AddSubMenu("Lane Clear", "laneclearMenu");
            Essentials.LaneClearMenu.AddGroupLabel("LaneTemizleme Ayarları");
            Essentials.LaneClearMenu.Add("useQ", new CheckBox("LaneTemizlemede Q Kullan", false));
            Essentials.LaneClearMenu.Add("useCard", new CheckBox("LaneTemizlerken W Kullan"));
            Essentials.LaneClearMenu.Add("qPred", new Slider("Q için gereken minyon", 3, 1, 5));
            Essentials.LaneClearMenu.Add("manaManagerQ", new Slider("Q dan önce ne kadar manam olsun %", 50));
            Essentials.LaneClearMenu.AddGroupLabel("Card Seçici Menü");
            Essentials.LaneClearMenu.Add("chooser", new ComboBox("Card Seçme Modu", new[] { "Akıllıca", "Mavi", "Kırmızı", "Sarı" }));
            Essentials.LaneClearMenu.Add("enemyW", new Slider("Kırmızı Kart Seçmek İçin Kaç Düşman Gerekli? (Akıllıca)", 2, 1, 5));
            Essentials.LaneClearMenu.Add("manaW", new Slider("Mavi Kardı Ne Kadar manam kalırsa Seçeyim? (Akıllıca)", 25));

            // Jungle Clear Menu
            Essentials.JungleClearMenu = Essentials.MainMenu.AddSubMenu("Orman Temizleme", "jgMenu");
            Essentials.JungleClearMenu.AddGroupLabel("Orman Temizleme Ayarları");
            Essentials.JungleClearMenu.Add("useQ", new CheckBox("Orman Temizlerken Q Kullan", false));
            Essentials.JungleClearMenu.Add("useCard", new CheckBox("Orman Temizlerken W kullan"));
            Essentials.JungleClearMenu.Add("qPred", new Slider("Q İsabet Oranı %", 75));
            Essentials.JungleClearMenu.Add("manaManagerQ", new Slider("Q atmadan önce ne kadar manam olsun", 50));
            Essentials.JungleClearMenu.AddGroupLabel("Card Seçici Mod");
            Essentials.JungleClearMenu.Add("chooser", new ComboBox("Card Seçme Modu", new[] { "Akıllıca", "Mavi", "Kırmızı", "Sarı" }));
            Essentials.JungleClearMenu.Add("enemyW", new Slider("Kırmızı Kart Seçmek İçin Kaç Düşman Gerekli? (Akıllıca)", 2, 1, 5));
            Essentials.JungleClearMenu.Add("manaW", new Slider("Mavi Kardı Ne Kadar manam kalırsa Seçeyim? (Akıllıca)", 25));

            // Kill Steal Menu
            Essentials.KillStealMenu = Essentials.MainMenu.AddSubMenu("Kill Çalma Menüsü", "ksMenu");
            Essentials.KillStealMenu.AddGroupLabel("Kill Çalma Ayarları");
            Essentials.KillStealMenu.Add("useQ", new CheckBox("KS'de Q kullan"));
            Essentials.KillStealMenu.Add("qPred", new Slider("Q İsabet Oranı %", 75));
            Essentials.KillStealMenu.Add("manaManagerQ", new Slider("Q atmadan önce gereken mana%", 15));
            Essentials.KillStealMenu.AddSeparator();

            // Drawing Menu
            Essentials.DrawingMenu = Essentials.MainMenu.AddSubMenu("Gösterge Menüsü", "drawMenu");
            Essentials.DrawingMenu.AddGroupLabel("Gösterge Ayarları");
            Essentials.DrawingMenu.Add("drawQ", new CheckBox("Göster Q Menzili"));
            Essentials.DrawingMenu.Add("drawR", new CheckBox("Göster R Menzili"));
            Essentials.DrawingMenu.AddSeparator();

            // Misc Menu
            Essentials.MiscMenu = Essentials.MainMenu.AddSubMenu("Ek Menu", "miscMenu");
            Essentials.MiscMenu.AddGroupLabel("Ek Ayarlar");
            Essentials.MiscMenu.Add("autoQ", new CheckBox("Hedef Sabitlendiğinde Otomatik Q kullan"));
            Essentials.MiscMenu.Add("qPred", new Slider("Q İsabet Oranı %", 75));
            Essentials.MiscMenu.Add("autoY", new CheckBox("R Kullandıktan sonra Otomatik Sarı Kart Seç"));
            Essentials.MiscMenu.Add("delay", new Slider("Kart Seçme Gecikmesi", 175, 175, 345));

            Chat.Print("TwistedBuddy 2.2.0.3 - By KarmaPanda-Ceviri-TRAdana iyi oyunlar diler", System.Drawing.Color.Green);

            // Events
            Game.OnUpdate += Game_OnUpdate;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
            Obj_AI_Base.OnSpellCast += Obj_AI_Base_OnSpellCast;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        /// <summary>
        /// Called after Spell Cast
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void Obj_AI_Base_OnSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            var target = args.Target as AIHeroClient;

            if (target == null || args.SData.Name != "goldcardpreattack" || !Q.IsReady() || !Q.IsInRange(target))
            {
                return;
            }

            if (Essentials.MiscMenu["autoQ"].Cast<CheckBox>().CurrentValue)
            {
                var pred = Q.GetPrediction(target);

                if (pred != null && pred.HitChancePercent >= Essentials.MiscMenu["qPred"].Cast<Slider>().CurrentValue)
                {
                    Q.Cast(pred.CastPosition);
                }
                else
                {
                    Essentials.UseStunQ = true;
                    Essentials.StunnedTarget = target;
                }
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) &&
                Essentials.ComboMenu["useQStun"].Cast<CheckBox>().CurrentValue)
            {
                var pred = Q.GetPrediction(target);

                if (pred != null && pred.HitChancePercent >= Essentials.ComboMenu["qPred"].Cast<Slider>().CurrentValue)
                {
                    Q.Cast(pred.CastPosition);
                }
                else
                {
                    Essentials.UseStunQ = true;
                    Essentials.StunnedTarget = target;
                }
            }
        }

        /// <summary>
        /// Called on Spell Cast
        /// </summary>
        /// <param name="sender">The Person who casted a spell</param>
        /// <param name="args">The Args</param>
        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            if (args.SData.Name.ToLower() == "gate" && Essentials.MiscMenu["autoY"].Cast<CheckBox>().CurrentValue)
            {
                CardSelector.StartSelecting(Cards.Yellow);
            }
        }

        /// <summary>
        /// Called when game draws.
        /// </summary>
        /// <param name="args">The Args.</param>
        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Essentials.DrawingMenu["drawQ"].Cast<CheckBox>().CurrentValue)
            {
                if (Player.Instance != null)
                {
                    Circle.Draw(Q.IsReady() ? Color.Green : Color.Red, Q.Range, Player.Instance.Position);
                }
            }

            if (!Essentials.DrawingMenu["drawR"].Cast<CheckBox>().CurrentValue)
            {
                return;
            }

            if (Player.Instance != null)
            {
                Circle.Draw(R.IsReady() ? Color.Green : Color.Red, R.Range, Player.Instance.Position);
            }
        }

        /// <summary>
        /// Called when game updates.
        /// </summary>
        /// <param name="args">The Args.</param>
        private static void Game_OnUpdate(EventArgs args)
        {
            if (!Player.Instance.IsRecalling() && !Player.Instance.IsInShopRange())
            {
                var useY = Essentials.CardSelectorMenu["useY"].Cast<KeyBind>().CurrentValue;
                var useB = Essentials.CardSelectorMenu["useB"].Cast<KeyBind>().CurrentValue;
                var useR = Essentials.CardSelectorMenu["useR"].Cast<KeyBind>().CurrentValue;

                if (useY)
                {
                    CardSelector.StartSelecting(Cards.Yellow);
                }

                if (useB)
                {
                    CardSelector.StartSelecting(Cards.Blue);
                }

                if (useR)
                {
                    CardSelector.StartSelecting(Cards.Red);
                }

                StateManager.AutoQ();
            }

            if (Essentials.KillStealMenu["useQ"].Cast<CheckBox>().CurrentValue)
            {
                StateManager.KillSteal();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                StateManager.Combo();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                StateManager.LaneClear();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                StateManager.JungleClear();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                StateManager.Harass();
            }
        }
    }
}
