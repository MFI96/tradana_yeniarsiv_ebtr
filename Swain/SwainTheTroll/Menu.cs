using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace SwainTheTroll
{
    internal static class SwainTheTrollMeNu
    {
        private static Menu _myMenu;
        public static Menu ComboMenu, DrawMeNu, HarassMeNu, Activator, FarmMeNu, MiscMeNu;

        public static void LoadMenu()
        {
            SwainTheTrollPage();
            DrawMeNuPage();
            ComboMenuPage();
            FarmMeNuPage();
            HarassMeNuPage();
            ActivatorPage();
            MiscMeNuPage();
        }

        private static void SwainTheTrollPage()
        {
            _myMenu = MainMenu.AddMenu("Swain  The Troll", "main");
            _myMenu.AddLabel(" Swain The Troll " + Program.Version);
            _myMenu.AddLabel("MeLoDag Tarafýndan yapýldý");
            _myMenu.AddLabel("tradana tarafýndan çevrilmiþtir.");
        }

        private static void DrawMeNuPage()
        {
            DrawMeNu = _myMenu.AddSubMenu("Draw  settings", "Draw");
            DrawMeNu.AddGroupLabel("Gösterge Ayarlarý:");
            DrawMeNu.Add("nodraw",
                new CheckBox("Hiçbirþey Gösterme", false));
            DrawMeNu.AddSeparator();
            DrawMeNu.Add("draw.Q",
                new CheckBox("Göster Q"));
            DrawMeNu.Add("draw.W",
                new CheckBox("Göster W"));
            DrawMeNu.Add("draw.E",
                new CheckBox("Göster E"));
            DrawMeNu.Add("draw.R",
                new CheckBox("Göster R"));
            DrawMeNu.AddLabel("Hasar Tespitçisi");
            DrawMeNu.Add("healthbar", new CheckBox("Can barýnda verilebilecek hasarý göster"));
            DrawMeNu.Add("percent", new CheckBox("Hasarý yüzde Olarak Göster"));
        }

        private static void ComboMenuPage()
        {
            ComboMenu = _myMenu.AddSubMenu("Combo settings", "Combo");
            ComboMenu.AddGroupLabel("Kombo Ayarlarý:");
            ComboMenu.Add("useQCombo", new CheckBox("Q Kullan"));
            ComboMenu.Add("useECombo", new CheckBox("E Kullan"));
            ComboMenu.Add("useWCombo", new CheckBox("W Kullan"));
            ComboMenu.AddLabel("R Ayarlarý:");
            ComboMenu.Add("useRCombo", new CheckBox("Use R Combo"));
            ComboMenu.Add("useRCombo1", new CheckBox("Auto Cancel Ulty"));
            ComboMenu.Add("Rcount", new Slider("Use R If Hit Enemy ", 3, 1, 5));
            ComboMenu.Add("Rlogic", new CheckBox("Smart R 1vs1"));
            ComboMenu.AddLabel("Kombo EK:");
            ComboMenu.Add("combo.CC",
                new CheckBox("Mantýklý Rakibe W Kullan"));
            ComboMenu.Add("combo.CCQ",
                new CheckBox("Mantýklý  Hedefe Q Kullan"));
           }


        private static void FarmMeNuPage()
        {
            FarmMeNu = _myMenu.AddSubMenu("Farm Settings", "FarmSettings");
            FarmMeNu.AddGroupLabel("Lane Temizleme Ayarlarý");
            FarmMeNu.Add("qFarmAlways", new CheckBox("Q Kullan"));
            FarmMeNu.Add("wFarm", new CheckBox("W Kullan"));
            FarmMeNu.Add("eFarm", new CheckBox("E Kullan"));
            FarmMeNu.Add("LaneMana", new Slider("Büyüler için en azmana", 70, 0, 100));
            FarmMeNu.AddLabel("Orman Temizleme Ayarlarý");
            FarmMeNu.Add("useQJungle", new CheckBox("Q Kullan"));
            FarmMeNu.Add("useWJungle", new CheckBox("W Kullan"));
            FarmMeNu.Add("useEJungle", new CheckBox("E Kullan"));
            FarmMeNu.Add("JungleMana", new Slider("Büyüler için en azmana", 70, 0, 100));
        }

        private static void HarassMeNuPage()
        {
            HarassMeNu = _myMenu.AddSubMenu("Harass", "Harass");
            HarassMeNu.AddGroupLabel("Dürtme Ayarlarý");
            HarassMeNu.Add("useQHarass", new CheckBox("Kullan Q"));
            HarassMeNu.Add("useEHarass", new CheckBox("Kullan E"));
            HarassMeNu.Add("HarassMana", new Slider("Büyüler için en azmana", 70, 0, 100));
            HarassMeNu.AddLabel("Killçalma Ayarlarý:");
            HarassMeNu.Add("ksE",
                new CheckBox("Use E", false));
        }

        private static void ActivatorPage()
        {
            Activator = _myMenu.AddSubMenu("Activator Settings", "Items");
            Activator.AddGroupLabel("Zhonyas Ayarlarý");
            Activator.Add("Zhonyas", new CheckBox("Zhanya Kullan"));
            Activator.Add("ZhonyasHp", new Slider("Zhonya Kullanmak için caným þundan az%", 20, 0, 100));
            Activator.AddLabel("Ýksir Ayarlarý");
            Activator.Add("spells.Potions.Check",
                new CheckBox("Ýksirleri Kullan"));
            Activator.Add("spells.Potions.HP",
                new Slider("Ýksir Kullanmak için caným þundan az {0}(%)", 60, 1));
            Activator.Add("spells.Potions.Mana",
                new Slider("Ýksir Kullanmak için manam þundan az {0}(%)", 60, 1));
            Activator.AddLabel("Ýyileþtirme Ayarlarý:");
            Activator.Add("spells.Heal.Hp",
                new Slider("Ýyileþtirme Kullanmak için caným þundan az {0}(%)", 30, 1));
            Activator.AddLabel("Tutuþtur Ayarlarý:");
            Activator.Add("spells.Ignite.Focus",
                new Slider("Tutuþtur kullanmka için  hedefin caný þundan az {0}(%)", 10, 1));
        }

        private static void MiscMeNuPage()
        {
            MiscMeNu = _myMenu.AddSubMenu("Misc Menu", "othermenu");
            MiscMeNu.AddGroupLabel("Rakibe Yaklaþma/Uzaklaþma");
            MiscMeNu.Add("gapcloser.W",new CheckBox("W ile Rakibe Yaklaþma/Uzaklaþma"));
            MiscMeNu.AddLabel("Tehlikeli yeteneði bozmak Ayarlarý:");
            MiscMeNu.Add("interrupter", new CheckBox("Tehlikeli yeteneði bozmak için W kullan"));
            MiscMeNu.Add("interrupt.value", new ComboBox("Tehlikeli yeteneði bozma Tehlike Seviyesi", 0, "High", "Medium", "Low"));
            MiscMeNu.AddLabel("Kostüm Ayarlarý");
            MiscMeNu.Add("checkSkin",
                new CheckBox("Kostüm Deðiþtirici Kullan:", false));
            MiscMeNu.Add("skin.Id",
                new Slider("Kostüm Numarasý", 5, 0, 10));
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