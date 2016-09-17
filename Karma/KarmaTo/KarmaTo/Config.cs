using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace KarmaTo
{

    public static class Config
    {
        private const string MenuName = "KarmaTo";

        private static readonly Menu Menu;

        static Config()
        {
            Menu = MainMenu.AddMenu(MenuName, MenuName.ToLower());
            Menu.AddGroupLabel("Welcome to KarmaTo!");
            Menu.AddLabel("Bu benim ilk addonum");
            Menu.AddLabel("İyi eğlenceler !");
            Menu.AddLabel("Hellsinge teşekkürler");

            Modes.Initialize();
        }

        public static void Initialize()
        {
        }

        public static class Modes
        {
            static Modes()
            {
                Combo.Initialize(Menu.AddSubMenu("Combo"));
                Menu.AddSeparator();
                Harass.Initialize(Menu.AddSubMenu("Harass"));
                Menu.AddSeparator();
                Flee.Initialize(Menu.AddSubMenu("Flee"));
                Menu.AddSeparator();
                LaneClear.Initialize(Menu.AddSubMenu("LaneClear"));
                Menu.AddSeparator();
                Draw.Initialize(Menu.AddSubMenu("Draw"));
                Menu.AddSeparator();
                PermaActive.Initialize(Menu.AddSubMenu("Misc"));
            }

            public static void Initialize()
            {
            }

            public static class Combo
            {
                private static CheckBox _useQ;
                private static CheckBox _useW;
                private static CheckBox _useR;
                private static Menu myMenu;
                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }
                public static bool UseW
                {
                    get { return _useW.CurrentValue; }
                }
                public static bool UseR
                {
                    get { return _useR.CurrentValue; }
                }
                public static int comboUseRW
                {
                    get { return myMenu["comboUseRW"].Cast<Slider>().CurrentValue; }
                }
                public static Menu getMenu()
                {
                    return myMenu;
                }

                public static double predictionHit
                {
                    get { return myMenu["predictionHit"].Cast<Slider>().CurrentValue; }
                }

                static Combo()
                {
                    // Initialize the menu values



                }

                public static void Initialize(Menu menu)
                {
                    myMenu = menu;
                    myMenu.AddGroupLabel("Combo");
                    _useQ = myMenu.Add("comboUseQ", new CheckBox("Kullan Q", true));
                    _useW = myMenu.Add("comboUseW", new CheckBox("Kullan W", true));
                    _useR = myMenu.Add("comboUseR", new CheckBox("Kullan Kombo Q+R", true));
                    myMenu.Add("comboUseRW", new Slider("W+R kombo için can < ({0}%)", 40));
                    myMenu.Add("predictionHit", new Slider("Q isabet oranı", 70));
                }
            }

            public static class LaneClear
            {
                public static bool UseQ
                {
                    get { return myMenu["clearUseQ"].Cast<CheckBox>().CurrentValue; }
                }
                public static bool UseR
                {
                    get { return myMenu["clearUseR"].Cast<CheckBox>().CurrentValue; }
                }
                public static int Mana
                {
                    get { return myMenu["clearMana"].Cast<Slider>().CurrentValue; }
                }
                public static Menu getMenu()
                {
                    return myMenu;
                }
                private static Menu myMenu;
                static LaneClear()
                {

                }

                public static void Initialize(Menu menu)
                {
                    myMenu = menu;
                    myMenu.AddGroupLabel("LaneClear");
                    myMenu.Add("clearUseQ", new CheckBox("Kullan Q", false));
                    myMenu.Add("clearUseR", new CheckBox("Kullan combo Q+R", false));
                    myMenu.Add("clearMana", new Slider("Gereken mana ({0}%)", 40));
                }
            }

            public static class PermaActive
            {
                public static bool autoShieldTurret
                {
                    get { return myMenu["autoShieldTurret"].Cast<CheckBox>().CurrentValue; }
                }
                public static bool autoShieldSpell
                {
                    get { return myMenu["autoShieldSpell"].Cast<CheckBox>().CurrentValue; }
                }
                public static bool antiGapCloser
                {
                    get { return myMenu["antiGapCloser"].Cast<CheckBox>().CurrentValue; }
                }

                public static Menu getMenu()
                {
                    return myMenu;
                }
                private static Menu myMenu;
                static PermaActive()
                {

                }

                public static void Initialize(Menu menu)
                {
                    myMenu = menu;
                    myMenu.AddGroupLabel("PermaActive");
                    myMenu.Add("autoShieldTurret", new CheckBox("Kule altında otomatik kalkan", true));
                    myMenu.Add("autoShieldSpell", new CheckBox("Otomatik kalkan büyüsü", true));
                    myMenu.Add("antiGapCloser", new CheckBox("Anti GapCloser", true));
                }
            }

            public static class Harass
            {
                private static Menu myMenu;
                public static bool UseQ
                {
                    get { return myMenu["harassUseQ"].Cast<CheckBox>().CurrentValue; }
                }
                public static bool UseR
                {
                    get { return myMenu["harassUseR"].Cast<CheckBox>().CurrentValue; }
                }
                public static int Mana
                {
                    get { return myMenu["harassMana"].Cast<Slider>().CurrentValue; }
                }
                public static Menu getMenu()
                {
                    return myMenu;
                }

                static Harass()
                {

                }

                public static void Initialize(Menu menu)
                {
                    myMenu = menu;
                    myMenu.AddGroupLabel("Harass");
                    myMenu.Add("harassUseQ", new CheckBox("Kullan Q", true));
                    myMenu.Add("harassUseR", new CheckBox("Kullan combo R+Q", false));

                    myMenu.Add("harassMana", new Slider("Gereken mana ({0}%)", 40));
                }
            }

            public static class Draw
            {
                private static Menu myMenu;
                public static Menu getMenu()
                {
                    return myMenu;
                }
                public static bool DrawQ
                {
                    get { return myMenu["drawQ"].Cast<CheckBox>().CurrentValue; }
                }
                public static bool DrawW
                {
                    get { return myMenu["drawW"].Cast<CheckBox>().CurrentValue; }
                }

                static Draw()
                {

                }

                public static void Initialize(Menu menu)
                {
                    myMenu = menu;
                    myMenu.AddGroupLabel("Draw");
                    myMenu.Add("drawQ", new CheckBox("Göster Q", true));
                    myMenu.Add("drawW", new CheckBox("Göster W Aktifse", true));
                }
            }

            public static class Flee
            {
                private static Menu myMenu;
                public static bool UseQ
                {
                    get { return myMenu["fleeUseQ"].Cast<CheckBox>().CurrentValue; }
                }
                public static bool UseE
                {
                    get { return myMenu["fleeUseE"].Cast<CheckBox>().CurrentValue; }
                }
                public static bool UseR
                {
                    get { return myMenu["fleeUseR"].Cast<CheckBox>().CurrentValue; }
                }
                public static Menu getMenu()
                {
                    return myMenu;
                }

                static Flee()
                {

                }

                public static void Initialize(Menu menu)
                {
                    myMenu = menu;
                    myMenu.AddGroupLabel("Flee");
                    myMenu.Add("fleeUseQ", new CheckBox("Kullan Q", true));
                    myMenu.Add("fleeUseE", new CheckBox("Kullan E", true));
                    myMenu.Add("fleeUseR", new CheckBox("Kullan combo E+R", true));
                }
            }
        }
    }
}
