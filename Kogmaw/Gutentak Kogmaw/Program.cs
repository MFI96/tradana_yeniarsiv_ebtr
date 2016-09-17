using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using GuTenTak.KogMaw;
using SharpDX;

namespace GuTenTak.KogMaw
{
    internal class Program
    {
        public const string ChampionName = "KogMaw";
        public static Menu Menu, ModesMenu1, ModesMenu2, ModesMenu3, DrawMenu;
        public static int SkinBase;
        public static Item Youmuu = new Item(ItemId.Youmuus_Ghostblade);
        public static Item Botrk = new Item(ItemId.Blade_of_the_Ruined_King);
        public static Item Cutlass = new Item(ItemId.Bilgewater_Cutlass);
        public static Item Tear = new Item(ItemId.Tear_of_the_Goddess);
        public static Item Qss = new Item(ItemId.Quicksilver_Sash);
        public static Item Simitar = new Item(ItemId.Mercurial_Scimitar);
        public static Item hextech = new Item(ItemId.Hextech_Gunblade, 700);
        //private static bool IsZombie;
        //private static bool wActive;
        //private static int LastAATick;

        //public static AIHeroClient PlayerInstance { get { return Player.Instance; } }
        private static float HealthPercent() { return (Player.Instance.Health / Player.Instance.MaxHealth) * 100; }
        
        //public static bool AutoQ { get; protected set; }
        //public static float Manaah { get; protected set; }
        //public static object GameEvent { get; private set; }

        public static Spell.Skillshot Q;
        public static Spell.Active W;
        public static Spell.Skillshot E;
        public static Spell.Skillshot R;
        //private static bool siegecount;

        public static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }



        public static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.ChampionName != ChampionName)
            {
                return;
            }
            //IsZombie = PlayerInstance.HasBuff("kogmawicathiansurprise");
            //wActive = PlayerInstance.HasBuff("kogmawbioarcanebarrage");
            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Game_OnDraw;
            Obj_AI_Base.OnBuffGain += Common.OnBuffGain;
            Game.OnTick += (EventArgs) => Common.Skinhack();
            Gapcloser.OnGapcloser += Common.Gapcloser_OnGapCloser;
            Game.OnUpdate += Common.zigzag;
            Obj_AI_Base.OnLevelUp += OnLevelUp;
            SkinBase = Player.Instance.SkinId;
            // Item
            try
            {
                Q = new Spell.Skillshot(SpellSlot.Q, 1000, SkillShotType.Linear, 250, 1650, 70);
                Q.AllowedCollisionCount = 0;
                W = new Spell.Active(SpellSlot.W, 720);
                E = new Spell.Skillshot(SpellSlot.E, 1200, SkillShotType.Linear, 500, 1400, 120);
                E.AllowedCollisionCount = int.MaxValue;
                R = new Spell.Skillshot(SpellSlot.R, 1800, SkillShotType.Circular, 1200, int.MaxValue, 120);
                R.AllowedCollisionCount = int.MaxValue;



                Bootstrap.Init(null);
                Chat.Print("GuTenTak Addon Loading Success", Color.Green);


                Menu = MainMenu.AddMenu("GuTenTak KogMaw", "KogMaw");
                Menu.AddSeparator();
                Menu.AddLabel("GuTenTak KogMaw Addon");

                var Enemies = EntityManager.Heroes.Enemies.Where(a => !a.IsMe).OrderBy(a => a.BaseSkinName);
                ModesMenu1 = Menu.AddSubMenu("Menu", "Modes1KogMaw");
                ModesMenu1.AddSeparator();
                ModesMenu1.AddLabel("Combo Ayarları");
                ModesMenu1.Add("ComboQ", new CheckBox("Q Kullan", true));
                ModesMenu1.AddLabel("Q için mana >= 80");
                ModesMenu1.Add("ComboW", new CheckBox("W Kullan", true));
                ModesMenu1.Add("ComboE", new CheckBox("E Kullan", true));
                ModesMenu1.Add("ComboR", new CheckBox("R Kullan", true));
                ModesMenu1.Add("LogicRn", new ComboBox("R için düşmanın canı şundan az % <= ", 1, "100%", "55%", "30%"));
                ModesMenu1.Add("ManaCE", new Slider("E için mana %", 30));
                ModesMenu1.Add("ManaCR", new Slider("R için mana %", 80));
                ModesMenu1.Add("CRStack", new Slider("Komboda R için stack Limiti", 3, 1, 10));
                ModesMenu1.AddSeparator();
                ModesMenu1.AddLabel("Otomatik Dürtme Ayarları");
                ModesMenu1.Add("AutoHarass", new CheckBox("R kullan otomatik dürtmek için", false));
                ModesMenu1.Add("ARStack", new Slider("Otomatik R limiti", 2, 1, 6));
                ModesMenu1.Add("ManaAuto", new Slider("Manam şundan çok %", 70));

                ModesMenu1.AddLabel("Dürtme Ayarları");
                ModesMenu1.Add("HarassQ", new CheckBox("Q Kullan", true));
                ModesMenu1.Add("HarassE", new CheckBox("E Kullan", true));
                ModesMenu1.Add("HarassR", new CheckBox("R Kullan", true));
                ModesMenu1.Add("ManaHE", new Slider("E Mana %", 60));
                ModesMenu1.Add("ManaHR", new Slider("R Mana %", 60));
                ModesMenu1.Add("HRStack", new Slider("Dürterken R limiti", 1, 1, 6));
                ModesMenu1.AddSeparator();
                ModesMenu1.AddLabel("Kill Çalma Ayarları");
                ModesMenu1.Add("KS", new CheckBox("Kullan", true));
                ModesMenu1.Add("KQ", new CheckBox("Q Kullan", true));
                ModesMenu1.Add("KR", new CheckBox("R Kullan", true));

                ModesMenu2 = Menu.AddSubMenu("Farm", "Modes2KogMaw");
                ModesMenu2.AddLabel("Lane Clear Ayarları");
                ModesMenu2.Add("ManaL", new Slider("Mana %", 40));
                ModesMenu2.Add("FarmQ", new CheckBox("Q Kullan", true));
                ModesMenu2.Add("ManaLR", new Slider("Mana %", 40));
                ModesMenu2.Add("FarmR", new CheckBox("R Kullan", true));
                ModesMenu2.Add("FRStack", new Slider("Lanetemizliği R limiti", 1, 1, 6));
                ModesMenu2.AddLabel("Orman Temizleme Ayarları");
                ModesMenu2.Add("ManaJ", new Slider("Mana %", 40));
                ModesMenu2.Add("JungleQ", new CheckBox("Q Kullan", true));
                ModesMenu2.Add("ManaJR", new Slider("Mana %", 40));
                ModesMenu2.Add("JungleR", new CheckBox("R Kullan", true));
                ModesMenu2.Add("JRStack", new Slider("Ormantemizleme R limiti", 2, 1, 6));

                ModesMenu3 = Menu.AddSubMenu("Misc", "Modes3KogMaw");
                ModesMenu3.AddLabel("Flee Ayarları");
                ModesMenu3.Add("FleeR", new CheckBox("R Kullan", true));
                ModesMenu3.Add("FleeE", new CheckBox("E Kullan", true));
                ModesMenu3.Add("ManaFlR", new Slider("R Mana %", 35));
                ModesMenu3.Add("FlRStack", new Slider("Kaçarken R limiti", 2, 1, 6));

                ModesMenu3.AddLabel("Item Usage on Combo");
                ModesMenu3.Add("useYoumuu", new CheckBox("Kullan Youmuu", true));
                ModesMenu3.Add("usehextech", new CheckBox("Kullan Hextech", true));
                ModesMenu3.Add("useBotrk", new CheckBox("Kullan Mahvolmuş", true));
                ModesMenu3.Add("useQss", new CheckBox("Kullan Civayatağan", true));
                ModesMenu3.Add("minHPBotrk", new Slider("Mahvolmuş canım şundan az %", 80));
                ModesMenu3.Add("enemyMinHPBotrk", new Slider("Düşmanın canı şundan az %", 80));

                ModesMenu3.AddLabel("QSS Ayarları");
                ModesMenu3.Add("Qssmode", new ComboBox(" ", 0, "Auto", "Combo"));
                ModesMenu3.Add("Stun", new CheckBox("Sabit", true));
                ModesMenu3.Add("Blind", new CheckBox("Kör", true));
                ModesMenu3.Add("Charm", new CheckBox("Çekicilik(ahri)", true));
                ModesMenu3.Add("Suppression", new CheckBox("Önleme skilleri", true));
                ModesMenu3.Add("Polymorph", new CheckBox("Polimorf", true));
                ModesMenu3.Add("Fear", new CheckBox("Korkmuş", true));
                ModesMenu3.Add("Taunt", new CheckBox("Alay etme", true));
                ModesMenu3.Add("Silence", new CheckBox("Sessiz", false));
                ModesMenu3.Add("QssDelay", new Slider("Kullan QSS Gecikmesi(ms)", 250, 0, 1000));

                ModesMenu3.AddLabel("QSS Ult Ayarları");
                ModesMenu3.Add("ZedUlt", new CheckBox("Zed R", true));
                ModesMenu3.Add("VladUlt", new CheckBox("Vladimir R", true));
                ModesMenu3.Add("FizzUlt", new CheckBox("Fizz R", true));
                ModesMenu3.Add("MordUlt", new CheckBox("Mordekaiser R", true));
                ModesMenu3.Add("PoppyUlt", new CheckBox("Poppy R", true));
                ModesMenu3.Add("QssUltDelay", new Slider("Kullan QSS Gecikmesi(ms) Ulti için", 250, 0, 1000));

                ModesMenu3.AddLabel("Skin Değiştirici");
                ModesMenu3.Add("skinhack", new CheckBox("Aktif Skin hack", false));
                ModesMenu3.Add("skinId", new ComboBox("Skin Mode", 0, "Default", "1", "2", "3", "4", "5", "6", "7", "8"));

                DrawMenu = Menu.AddSubMenu("Draws", "DrawKogMaw");
                DrawMenu.Add("drawQ", new CheckBox(" Göster Q", true));
                DrawMenu.Add("drawW", new CheckBox(" Göster W", true));
                DrawMenu.Add("drawR", new CheckBox(" Göster R", false));
                DrawMenu.Add("drawXR", new CheckBox("Kaçarken R Göster", true));
                DrawMenu.Add("drawXFleeQ", new CheckBox("Kaçarken Q Göster", false));

            }

            catch (Exception)
            {

            }

        }
        private static void Game_OnDraw(EventArgs args)
        {

            try
            {
                if (DrawMenu["drawQ"].Cast<CheckBox>().CurrentValue)
                {
                    if (Q.IsReady() && Q.IsLearned)
                    {
                        Circle.Draw(Color.White, Q.Range, Player.Instance.Position);
                    }
                }
                if (DrawMenu["drawW"].Cast<CheckBox>().CurrentValue)
                {
                    if (W.IsReady() && W.IsLearned)
                    {
                        Circle.Draw(Color.White, W.Range, Player.Instance.Position);
                    }
                }
                if (DrawMenu["drawR"].Cast<CheckBox>().CurrentValue)
                {
                    if (R.IsReady() && R.IsLearned)
                    {
                        Circle.Draw(Color.White, R.Range, Player.Instance.Position);
                    }
                }
                if (DrawMenu["drawXR"].Cast<CheckBox>().CurrentValue)
                {
                    if (R.IsReady() && R.IsLearned)
                    {
                        Circle.Draw(Color.Red, 700, Player.Instance.Position);
                    }
                }
                if (DrawMenu["drawXFleeQ"].Cast<CheckBox>().CurrentValue)
                {
                    if (Q.IsReady() && Q.IsLearned)
                    {
                        Circle.Draw(Color.Red, 400, Player.Instance.Position);
                    }
                }
            }
            catch (Exception e)
            {

            }
        }
        static void Game_OnUpdate(EventArgs args)
        {
            try
            {
                var AutoHarass = ModesMenu1["AutoHarass"].Cast<CheckBox>().CurrentValue;
                var ManaAuto = ModesMenu1["ManaAuto"].Cast<Slider>().CurrentValue;
                Common.KillSteal();

                if (AutoHarass && ManaAuto <= ObjectManager.Player.ManaPercent)
                    {
                        Common.AutoR();
                    }
                    if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                    {
                        Common.Combo();
                        Common.ItemUsage();
                    }
                    if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
                    {
                        Common.Harass();
                    }

                    if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
                    {

                        Common.LaneClear();

                    }

                    if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
                    {

                        Common.JungleClear();
                    }

                    if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
                    {
                        Common.LastHit();

                    }
                    if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
                    {
                        Common.Flee();

                    }
            }
            catch (Exception e)
            {

            }
        }

        public static void OnLevelUp(Obj_AI_Base sender, Obj_AI_BaseLevelUpEventArgs args)
        {
            if (!sender.IsMe || args.Level != 1) return;
            Game.OnTick += SetSkillshot;
        }

        public static void SetSkillshot(EventArgs args)
        {
            if (Q.Level + W.Level + E.Level + R.Level == Player.Instance.Level)
            {
                W = new Spell.Active(SpellSlot.W, (uint)(565 + 60 + W.Level * 30 + 65));
                R = new Spell.Skillshot(SpellSlot.R, (uint)(900 + R.Level * 300), SkillShotType.Circular, 1500, int.MaxValue, 225);
                Game.OnTick -= SetSkillshot; //improve fps
            }
        }

    }
}
