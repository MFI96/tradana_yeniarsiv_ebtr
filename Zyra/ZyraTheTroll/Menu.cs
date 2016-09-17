using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace ZyraTheTroll
{
    internal static class ZyraTheTrollMeNu
    {
        private static Menu _myMenu;
        public static Menu ComboMenu, DrawMeNu, HarassMeNu, Activator, FarmMeNu, MiscMeNu;

        public static void LoadMenu()
        {
            MyZyraTheTrollPage();
            DrawMeNuPage();
            ComboMenuPage();
            FarmMeNuPage();
            HarassMeNuPage();
            ActivatorPage();
            MiscMeNuPage();
        }

        private static void MyZyraTheTrollPage()
        {
            _myMenu = MainMenu.AddMenu("Zyra The Troll", "main");
            _myMenu.AddLabel(" Zyra The Troll " + Program.Version);
            _myMenu.AddLabel(" Melodag tarafýndan yapýldý");
            _myMenu.AddLabel("tradana tarafýndan çevrildi");
        }

        private static void DrawMeNuPage()
        {
            DrawMeNu = _myMenu.AddSubMenu("Draw  settings", "Draw");
            DrawMeNu.AddGroupLabel("Gösterge Ayarlarý:");
            DrawMeNu.Add("nodraw",
                new CheckBox("Hiçbirþey Gösterme", false));
            DrawMeNu.AddSeparator();
            DrawMeNu.Add("draw.Q",
                new CheckBox("Q Göster"));
            DrawMeNu.Add("draw.W",
                new CheckBox("W Göster"));
            DrawMeNu.Add("draw.E",
                new CheckBox("E Göster"));
            DrawMeNu.Add("draw.R",
                new CheckBox("R Göster"));
        }

        private static void ComboMenuPage()
        {
            ComboMenu = _myMenu.AddSubMenu("Combo settings", "Combo");
            ComboMenu.AddGroupLabel("Kombo Ayarlarý:");
            ComboMenu.Add("useQCombo", new CheckBox("Q Kullan"));
            ComboMenu.Add("useECombo", new CheckBox("E Kullan"));
            ComboMenu.Add("useWCombo", new CheckBox("W Kullan"));
            ComboMenu.Add("useRCombo", new CheckBox("R Kullan"));
            ComboMenu.Add("Rcount", new Slider("R için düþman say ", 2, 1, 5));
            ComboMenu.AddLabel("Kombo Ek Ayarlarý:");
            ComboMenu.Add("combo.CC",
                new CheckBox("Mantýklý Hedefe E Kullan"));
            ComboMenu.Add("combo.CCQ",
                new CheckBox("Mantýklý hedefe Q Kullan"));
            ComboMenu.Add("Rlogic", new CheckBox("Akýllý R 1vs1"));
        }


        private static void FarmMeNuPage()
        {
            FarmMeNu = _myMenu.AddSubMenu("Farm Settings", "FarmSettings");
            FarmMeNu.AddGroupLabel("Lanetemizleme Ayarlarý");
            FarmMeNu.Add("qFarmAlways", new CheckBox("Her zaman Q At"));
            FarmMeNu.Add("qFarm", new CheckBox("Tüm Modlarda Q ile sonvuruþ"));
            FarmMeNu.AddLabel("OrmanTemizleme Ayarlarý");
            FarmMeNu.Add("useQJungle", new CheckBox("Q Kullan"));
        }

        private static void HarassMeNuPage()
        {
            HarassMeNu = _myMenu.AddSubMenu("Harass", "Harass");
            HarassMeNu.AddGroupLabel("Dürtme Ayarlarý");
            HarassMeNu.Add("useQHarass", new CheckBox("Q Kullan"));
            HarassMeNu.Add("useEHarass", new CheckBox("E Kullan"));
            HarassMeNu.AddLabel("KillÇalma Ayarlarý:");
            HarassMeNu.Add("ksQ",
                new CheckBox("Q Kullan", false));
                 HarassMeNu.Add("ksE",
                new CheckBox("E Kullan", false));
        }

        private static void ActivatorPage()
        {
            Activator = _myMenu.AddSubMenu("Activator Settings", "Items");
            Activator.AddGroupLabel("Zhonyas Ayarlarý");
            Activator.Add("Zhonyas", new CheckBox("Zhonya Kullan"));
            Activator.Add("ZhonyasHp", new Slider("Zhonya için caným þundan azsa Kullan%", 20, 0, 100));
            Activator.AddLabel("Ýksir Ayarlarý");
            Activator.Add("spells.Potions.Check",
                new CheckBox("Ýksir Kullan"));
            Activator.Add("spells.Potions.HP",
                new Slider("Caným þundan azsa iksir Kullan {0}(%)", 60, 1));
            Activator.Add("spells.Potions.Mana",
                new Slider("Manam þundan azsa Ýksir Kullan {0}(%)", 60, 1));
            Activator.AddLabel("Ýyileþtirme Ayarlarý:");
            Activator.Add("spells.Heal.Hp",
                new Slider("Ýyileþtirme kullanmak için caným þundan az {0}(%)", 30, 1));
            Activator.AddLabel("Tutuþtur Ayarlarý:");
            Activator.Add("spells.Ignite.Focus",
                new Slider("Tutuþtur için gereken düþman caný þundan az {0}(%)", 10, 1));
        }

        private static void MiscMeNuPage()
        {
            MiscMeNu = _myMenu.AddSubMenu("Misc Menu", "othermenu");
            MiscMeNu.AddGroupLabel("Rakibe Yaklaþma/Uzaklaþma ve Büyülerini Engelleme");
            MiscMeNu.Add("gapcloser.E",
                new CheckBox("Rakibe Yaklaþma/Uzaklaþma için E"));
            MiscMeNu.Add("interupt.E",
                new CheckBox("Tehlikeli yeteneði bozmak için E kullan"));
            MiscMeNu.AddLabel("Kostüm Ayarlarý");
            MiscMeNu.Add("checkSkin",
                new CheckBox("Kostüm Hilesi Kullan:", false));
            MiscMeNu.Add("skin.Id",
                new Slider("Kostüm No", 5, 0, 10));
        }

        public static bool Nodraw()
        {
            return DrawMeNu["nodraw"].Cast<CheckBox>().CurrentValue;
        }

        public static bool DrawingsQ()
        {
            return DrawMeNu["draw.Q"].Cast<CheckBox>().CurrentValue;
        }

        public static bool DrawingsW()
        {
            return DrawMeNu["draw.W"].Cast<CheckBox>().CurrentValue;
        }

        public static bool DrawingsE()
        {
            return DrawMeNu["draw.E"].Cast<CheckBox>().CurrentValue;
        }

        public static bool DrawingsR()
        {
            return DrawMeNu["draw.R"].Cast<CheckBox>().CurrentValue;
        }

        public static bool DrawingsT()
        {
            return DrawMeNu["draw.T"].Cast<CheckBox>().CurrentValue;
        }

        public static bool SpellsPotionsCheck()
        {
            return Activator["spells.Potions.Check"].Cast<CheckBox>().CurrentValue;
        }

        public static float SpellsPotionsHp()
        {
            return Activator["spells.Potions.HP"].Cast<Slider>().CurrentValue;
        }

        public static float SpellsPotionsM()
        {
            return Activator["spells.Potions.Mana"].Cast<Slider>().CurrentValue;
        }

        public static float SpellsHealHp()
        {
            return Activator["spells.Heal.HP"].Cast<Slider>().CurrentValue;
        }

        public static float SpellsIgniteFocus()
        {
            return Activator["spells.Ignite.Focus"].Cast<Slider>().CurrentValue;
        }

        public static int SkinId()
        {
            return MiscMeNu["skin.Id"].Cast<Slider>().CurrentValue;
        }


        public static bool SkinChanger()
        {
            return MiscMeNu["SkinChanger"].Cast<CheckBox>().CurrentValue;
        }

        public static bool CheckSkin()
        {
            return MiscMeNu["checkSkin"].Cast<CheckBox>().CurrentValue;
        }
    }
}