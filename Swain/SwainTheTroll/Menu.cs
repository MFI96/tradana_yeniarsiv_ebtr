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
            _myMenu.AddLabel("MeLoDag Taraf�ndan yap�ld�");
            _myMenu.AddLabel("tradana taraf�ndan �evrilmi�tir.");
        }

        private static void DrawMeNuPage()
        {
            DrawMeNu = _myMenu.AddSubMenu("Draw  settings", "Draw");
            DrawMeNu.AddGroupLabel("G�sterge Ayarlar�:");
            DrawMeNu.Add("nodraw",
                new CheckBox("Hi�bir�ey G�sterme", false));
            DrawMeNu.AddSeparator();
            DrawMeNu.Add("draw.Q",
                new CheckBox("G�ster Q"));
            DrawMeNu.Add("draw.W",
                new CheckBox("G�ster W"));
            DrawMeNu.Add("draw.E",
                new CheckBox("G�ster E"));
            DrawMeNu.Add("draw.R",
                new CheckBox("G�ster R"));
            DrawMeNu.AddLabel("Hasar Tespit�isi");
            DrawMeNu.Add("healthbar", new CheckBox("Can bar�nda verilebilecek hasar� g�ster"));
            DrawMeNu.Add("percent", new CheckBox("Hasar� y�zde Olarak G�ster"));
        }

        private static void ComboMenuPage()
        {
            ComboMenu = _myMenu.AddSubMenu("Combo settings", "Combo");
            ComboMenu.AddGroupLabel("Kombo Ayarlar�:");
            ComboMenu.Add("useQCombo", new CheckBox("Q Kullan"));
            ComboMenu.Add("useECombo", new CheckBox("E Kullan"));
            ComboMenu.Add("useWCombo", new CheckBox("W Kullan"));
            ComboMenu.AddLabel("R Ayarlar�:");
            ComboMenu.Add("useRCombo", new CheckBox("Use R Combo"));
            ComboMenu.Add("useRCombo1", new CheckBox("Auto Cancel Ulty"));
            ComboMenu.Add("Rcount", new Slider("Use R If Hit Enemy ", 3, 1, 5));
            ComboMenu.Add("Rlogic", new CheckBox("Smart R 1vs1"));
            ComboMenu.AddLabel("Kombo EK:");
            ComboMenu.Add("combo.CC",
                new CheckBox("Mant�kl� Rakibe W Kullan"));
            ComboMenu.Add("combo.CCQ",
                new CheckBox("Mant�kl�  Hedefe Q Kullan"));
           }


        private static void FarmMeNuPage()
        {
            FarmMeNu = _myMenu.AddSubMenu("Farm Settings", "FarmSettings");
            FarmMeNu.AddGroupLabel("Lane Temizleme Ayarlar�");
            FarmMeNu.Add("qFarmAlways", new CheckBox("Q Kullan"));
            FarmMeNu.Add("wFarm", new CheckBox("W Kullan"));
            FarmMeNu.Add("eFarm", new CheckBox("E Kullan"));
            FarmMeNu.Add("LaneMana", new Slider("B�y�ler i�in en azmana", 70, 0, 100));
            FarmMeNu.AddLabel("Orman Temizleme Ayarlar�");
            FarmMeNu.Add("useQJungle", new CheckBox("Q Kullan"));
            FarmMeNu.Add("useWJungle", new CheckBox("W Kullan"));
            FarmMeNu.Add("useEJungle", new CheckBox("E Kullan"));
            FarmMeNu.Add("JungleMana", new Slider("B�y�ler i�in en azmana", 70, 0, 100));
        }

        private static void HarassMeNuPage()
        {
            HarassMeNu = _myMenu.AddSubMenu("Harass", "Harass");
            HarassMeNu.AddGroupLabel("D�rtme Ayarlar�");
            HarassMeNu.Add("useQHarass", new CheckBox("Kullan Q"));
            HarassMeNu.Add("useEHarass", new CheckBox("Kullan E"));
            HarassMeNu.Add("HarassMana", new Slider("B�y�ler i�in en azmana", 70, 0, 100));
            HarassMeNu.AddLabel("Kill�alma Ayarlar�:");
            HarassMeNu.Add("ksE",
                new CheckBox("Use E", false));
        }

        private static void ActivatorPage()
        {
            Activator = _myMenu.AddSubMenu("Activator Settings", "Items");
            Activator.AddGroupLabel("Zhonyas Ayarlar�");
            Activator.Add("Zhonyas", new CheckBox("Zhanya Kullan"));
            Activator.Add("ZhonyasHp", new Slider("Zhonya Kullanmak i�in can�m �undan az%", 20, 0, 100));
            Activator.AddLabel("�ksir Ayarlar�");
            Activator.Add("spells.Potions.Check",
                new CheckBox("�ksirleri Kullan"));
            Activator.Add("spells.Potions.HP",
                new Slider("�ksir Kullanmak i�in can�m �undan az {0}(%)", 60, 1));
            Activator.Add("spells.Potions.Mana",
                new Slider("�ksir Kullanmak i�in manam �undan az {0}(%)", 60, 1));
            Activator.AddLabel("�yile�tirme Ayarlar�:");
            Activator.Add("spells.Heal.Hp",
                new Slider("�yile�tirme Kullanmak i�in can�m �undan az {0}(%)", 30, 1));
            Activator.AddLabel("Tutu�tur Ayarlar�:");
            Activator.Add("spells.Ignite.Focus",
                new Slider("Tutu�tur kullanmka i�in  hedefin can� �undan az {0}(%)", 10, 1));
        }

        private static void MiscMeNuPage()
        {
            MiscMeNu = _myMenu.AddSubMenu("Misc Menu", "othermenu");
            MiscMeNu.AddGroupLabel("Rakibe Yakla�ma/Uzakla�ma");
            MiscMeNu.Add("gapcloser.W",new CheckBox("W ile Rakibe Yakla�ma/Uzakla�ma"));
            MiscMeNu.AddLabel("Tehlikeli yetene�i bozmak Ayarlar�:");
            MiscMeNu.Add("interrupter", new CheckBox("Tehlikeli yetene�i bozmak i�in W kullan"));
            MiscMeNu.Add("interrupt.value", new ComboBox("Tehlikeli yetene�i bozma Tehlike Seviyesi", 0, "High", "Medium", "Low"));
            MiscMeNu.AddLabel("Kost�m Ayarlar�");
            MiscMeNu.Add("checkSkin",
                new CheckBox("Kost�m De�i�tirici Kullan:", false));
            MiscMeNu.Add("skin.Id",
                new Slider("Kost�m Numaras�", 5, 0, 10));
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