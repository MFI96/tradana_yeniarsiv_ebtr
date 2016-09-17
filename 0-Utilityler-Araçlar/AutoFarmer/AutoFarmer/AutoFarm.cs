namespace AutoFarmer
{
    using System;

    using Modes;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;
    using EloBuddy.SDK.Rendering;

    using SharpDX;

    using Genesis.Library;
    using Genesis.Library.Spells;

    internal class AutoFarm
    {
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        protected static SpellBase Spells => SpellManager.CurrentSpells;

        public static Menu MenuIni, Lh, Lc, ManaMenu, DrawMenu;

        internal static readonly AIHeroClient Player = ObjectManager.Player;

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            OnLoad();
        }

        private static void OnLoad()
        {
            MenuIni = MainMenu.AddMenu("Auto Farm ", "Auto Farm");

            Lh = MenuIni.AddSubMenu("LastHit ", "LastHit");
            Lh.AddGroupLabel("Sonvuruş Ayarları");
            Lh.Add(
                Player.ChampionName + "Enable",
                new KeyBind("Son vuruş tuşu", true, KeyBind.BindTypes.PressToggle, 'M'));
            Lh.Add(
                Player.ChampionName + "Enableactive",
                new KeyBind("Son vuruş Aktif", false, KeyBind.BindTypes.HoldActive));
            Lh.Add(Player.ChampionName + "combo", new CheckBox("Komboda Kullanma"));
            Lh.Add(Player.ChampionName + "harass", new CheckBox("Dürtmede Kullanma"));
            Lh.AddSeparator();
            Lh.AddGroupLabel("Spells Settings");
            Lh.Add(Player.ChampionName + "Qmode", new ComboBox("Q Modu", 0, "UnKillable Minions", "LastHit"));
            Lh.Add(Player.ChampionName + "Q", new CheckBox("Kullan Q ", false));
            Lh.Add(Player.ChampionName + "Wmode", new ComboBox("W Modu", 0, "UnKillable Minions", "LastHit"));
            Lh.Add(Player.ChampionName + "W", new CheckBox("Kullan W ", false));
            Lh.Add(Player.ChampionName + "Emode", new ComboBox("E Modu", 0, "UnKillable Minions", "LastHit"));
            Lh.Add(Player.ChampionName + "E", new CheckBox("Kullan E ", false));
            Lh.Add(Player.ChampionName + "Rmode", new ComboBox("R Modu", 0, "UnKillable Minions", "LastHit"));
            Lh.Add(Player.ChampionName + "R", new CheckBox("Kullan R ", false));

            Lc = MenuIni.AddSubMenu("LaneClear ", "LaneClear");
            Lc.AddGroupLabel("Lanetemizleme Ayarları");
            Lc.Add(
                Player.ChampionName + "Enable",
                new KeyBind("Lanetemizleme aktif Tuşu", true, KeyBind.BindTypes.PressToggle, 'M'));
            Lc.Add(
                Player.ChampionName + "Enableactive",
                new KeyBind("Lanetemizleme aktif", false, KeyBind.BindTypes.HoldActive));
            Lc.Add(Player.ChampionName + "combo", new CheckBox("Kombo sırasında devredışı bırak "));
            Lc.Add(Player.ChampionName + "harass", new CheckBox("Dürtme sırasından devredışı bırak "));
            Lc.AddSeparator();
            Lc.AddGroupLabel("Büyü Ayarları");
            Lc.Add(Player.ChampionName + "Qmode", new ComboBox("Q Modu", 0, "OnAfterAttack", "Always"));
            Lc.Add(Player.ChampionName + "Q", new CheckBox("Kullan Q ", false));
            Lc.Add(Player.ChampionName + "Wmode", new ComboBox("W Modu", 0, "OnAfterAttack", "Always"));
            Lc.Add(Player.ChampionName + "W", new CheckBox("Kullan W ", false));
            Lc.Add(Player.ChampionName + "Emode", new ComboBox("E Modu", 0, "OnAfterAttack", "Always"));
            Lc.Add(Player.ChampionName + "E", new CheckBox("Kullan E ", false));
            Lc.Add(Player.ChampionName + "Rmode", new ComboBox("R Modu", 0, "OnAfterAttack", "Always"));
            Lc.Add(Player.ChampionName + "R", new CheckBox("Kullan R ", false));

            ManaMenu = MenuIni.AddSubMenu("Mana Manager ", "Mana Manager");
            ManaMenu.AddGroupLabel("Manam sadece şundan yüksekse >= %");
            ManaMenu.Add(Player.ChampionName + "Q", new Slider("Q Kullan"));
            ManaMenu.Add(Player.ChampionName + "W", new Slider("W Kullan"));
            ManaMenu.Add(Player.ChampionName + "E", new Slider("E Kullan"));
            ManaMenu.Add(Player.ChampionName + "R", new Slider("R Kullan"));

            DrawMenu = MenuIni.AddSubMenu("Drawings ", "Drawings");
            DrawMenu.AddGroupLabel("Gösterge Ayarları");
            DrawMenu.Add(Player.ChampionName + "Q", new CheckBox("Göster Q ", false));
            DrawMenu.Add(Player.ChampionName + "W", new CheckBox("Göster W ", false));
            DrawMenu.Add(Player.ChampionName + "E", new CheckBox("Göster E ", false));
            DrawMenu.Add(Player.ChampionName + "R", new CheckBox("Göster R ", false));

            SpellManager.Initialize();
            SpellLibrary.Initialize();
            Orbwalker.OnUnkillableMinion += LastHit.Orbwalker_OnUnkillableMinion;
            Drawing.OnDraw += Drawing_OnDraw;
            Orbwalker.OnPostAttack += LaneClear.Orbwalker_OnPostAttack;
            Game.OnTick += Game_OnTick;
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (Player.IsRecalling()) return;
            if (Lc[Player.ChampionName + "Enable"].Cast<KeyBind>().CurrentValue
                || Lc[Player.ChampionName + "Enableactive"].Cast<KeyBind>().CurrentValue)
            {
                if (Lc[Player.ChampionName + "combo"].Cast<CheckBox>().CurrentValue
                    && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                {
                    return;
                }

                if (Lc[Player.ChampionName + "harass"].Cast<CheckBox>().CurrentValue
                    && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
                {
                    return;
                }

                LaneClear.Clear();
            }

            if (Lh[Player.ChampionName + "Enable"].Cast<KeyBind>().CurrentValue
                || Lh[Player.ChampionName + "Enableactive"].Cast<KeyBind>().CurrentValue)
            {
                if (Lh[Player.ChampionName + "combo"].Cast<CheckBox>().CurrentValue
                    && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                {
                    return;
                }

                if (Lh[Player.ChampionName + "harass"].Cast<CheckBox>().CurrentValue
                    && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
                {
                    return;
                }

                LastHit.Last();
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (DrawMenu[Player.ChampionName + "Q"].Cast<CheckBox>().CurrentValue
                && (!Spells.QisToggle || !Spells.QisDash || !Spells.QisCc || Spells.Q.Range <= 20000 || Spells.Q != null))
            {
                Circle.Draw(Color.White, Spells.Q.Range, ObjectManager.Player.Position);
            }

            if (DrawMenu[Player.ChampionName + "W"].Cast<CheckBox>().CurrentValue
                && (!Spells.WisToggle || !Spells.WisDash || !Spells.WisCc || Spells.W.Range <= 20000 || Spells.W != null))
            {
                Circle.Draw(Color.White, Spells.W.Range, ObjectManager.Player.Position);
            }

            if (DrawMenu[Player.ChampionName + "E"].Cast<CheckBox>().CurrentValue
                && (!Spells.EisToggle || !Spells.EisDash || !Spells.EisCc || Spells.E.Range <= 20000 || Spells.E != null))
            {
                Circle.Draw(Color.White, Spells.E.Range, ObjectManager.Player.Position);
            }

            if (DrawMenu[Player.ChampionName + "R"].Cast<CheckBox>().CurrentValue
                && (!Spells.RisToggle || !Spells.RisDash || !Spells.RisCc || Spells.R.Range <= 20000 || Spells.R != null))
            {
                Circle.Draw(Color.White, Spells.R.Range, ObjectManager.Player.Position);
            }
        }
    }
}