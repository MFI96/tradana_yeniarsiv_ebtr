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
            _myMenu.AddLabel(" Melodag taraf�ndan yap�ld�");
            _myMenu.AddLabel("tradana taraf�ndan �evrildi");
        }

        private static void DrawMeNuPage()
        {
            DrawMeNu = _myMenu.AddSubMenu("Draw  settings", "Draw");
            DrawMeNu.AddGroupLabel("G�sterge Ayarlar�:");
            DrawMeNu.Add("nodraw",
                new CheckBox("Hi�bir�ey G�sterme", false));
            DrawMeNu.AddSeparator();
            DrawMeNu.Add("draw.Q",
                new CheckBox("Q G�ster"));
            DrawMeNu.Add("draw.W",
                new CheckBox("W G�ster"));
            DrawMeNu.Add("draw.E",
                new CheckBox("E G�ster"));
            DrawMeNu.Add("draw.R",
                new CheckBox("R G�ster"));
        }

        private static void ComboMenuPage()
        {
            ComboMenu = _myMenu.AddSubMenu("Combo settings", "Combo");
            ComboMenu.AddGroupLabel("Kombo Ayarlar�:");
            ComboMenu.Add("useQCombo", new CheckBox("Q Kullan"));
            ComboMenu.Add("useECombo", new CheckBox("E Kullan"));
            ComboMenu.Add("useWCombo", new CheckBox("W Kullan"));
            ComboMenu.Add("useRCombo", new CheckBox("R Kullan"));
            ComboMenu.Add("Rcount", new Slider("R i�in d��man say ", 2, 1, 5));
            ComboMenu.AddLabel("Kombo Ek Ayarlar�:");
            ComboMenu.Add("combo.CC",
                new CheckBox("Mant�kl� Hedefe E Kullan"));
            ComboMenu.Add("combo.CCQ",
                new CheckBox("Mant�kl� hedefe Q Kullan"));
            ComboMenu.Add("Rlogic", new CheckBox("Ak�ll� R 1vs1"));
        }


        private static void FarmMeNuPage()
        {
            FarmMeNu = _myMenu.AddSubMenu("Farm Settings", "FarmSettings");
            FarmMeNu.AddGroupLabel("Lanetemizleme Ayarlar�");
            FarmMeNu.Add("qFarmAlways", new CheckBox("Her zaman Q At"));
            FarmMeNu.Add("qFarm", new CheckBox("T�m Modlarda Q ile sonvuru�"));
            FarmMeNu.AddLabel("OrmanTemizleme Ayarlar�");
            FarmMeNu.Add("useQJungle", new CheckBox("Q Kullan"));
        }

        private static void HarassMeNuPage()
        {
            HarassMeNu = _myMenu.AddSubMenu("Harass", "Harass");
            HarassMeNu.AddGroupLabel("D�rtme Ayarlar�");
            HarassMeNu.Add("useQHarass", new CheckBox("Q Kullan"));
            HarassMeNu.Add("useEHarass", new CheckBox("E Kullan"));
            HarassMeNu.AddLabel("Kill�alma Ayarlar�:");
            HarassMeNu.Add("ksQ",
                new CheckBox("Q Kullan", false));
                 HarassMeNu.Add("ksE",
                new CheckBox("E Kullan", false));
        }

        private static void ActivatorPage()
        {
            Activator = _myMenu.AddSubMenu("Activator Settings", "Items");
            Activator.AddGroupLabel("Zhonyas Ayarlar�");
            Activator.Add("Zhonyas", new CheckBox("Zhonya Kullan"));
            Activator.Add("ZhonyasHp", new Slider("Zhonya i�in can�m �undan azsa Kullan%", 20, 0, 100));
            Activator.AddLabel("�ksir Ayarlar�");
            Activator.Add("spells.Potions.Check",
                new CheckBox("�ksir Kullan"));
            Activator.Add("spells.Potions.HP",
                new Slider("Can�m �undan azsa iksir Kullan {0}(%)", 60, 1));
            Activator.Add("spells.Potions.Mana",
                new Slider("Manam �undan azsa �ksir Kullan {0}(%)", 60, 1));
            Activator.AddLabel("�yile�tirme Ayarlar�:");
            Activator.Add("spells.Heal.Hp",
                new Slider("�yile�tirme kullanmak i�in can�m �undan az {0}(%)", 30, 1));
            Activator.AddLabel("Tutu�tur Ayarlar�:");
            Activator.Add("spells.Ignite.Focus",
                new Slider("Tutu�tur i�in gereken d��man can� �undan az {0}(%)", 10, 1));
        }

        private static void MiscMeNuPage()
        {
            MiscMeNu = _myMenu.AddSubMenu("Misc Menu", "othermenu");
            MiscMeNu.AddGroupLabel("Rakibe Yakla�ma/Uzakla�ma ve B�y�lerini Engelleme");
            MiscMeNu.Add("gapcloser.E",
                new CheckBox("Rakibe Yakla�ma/Uzakla�ma i�in E"));
            MiscMeNu.Add("interupt.E",
                new CheckBox("Tehlikeli yetene�i bozmak i�in E kullan"));
            MiscMeNu.AddLabel("Kost�m Ayarlar�");
            MiscMeNu.Add("checkSkin",
                new CheckBox("Kost�m Hilesi Kullan:", false));
            MiscMeNu.Add("skin.Id",
                new Slider("Kost�m No", 5, 0, 10));
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