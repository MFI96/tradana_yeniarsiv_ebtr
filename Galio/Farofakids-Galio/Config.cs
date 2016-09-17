using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace Farofakids_Galio
{

    public static class Config
    {
        private const string MenuName = "Farofakids-Galio";

        private static readonly Menu Menu;

        static Config()
        {
            Menu = MainMenu.AddMenu(MenuName, MenuName.ToLower());
            Menu.AddGroupLabel("Hoşgeldin Farofakids-Galio!");

            Modes.Initialize();
        }

        public static void Initialize()
        {
        }

        public static class Modes
        {
            private static readonly Menu Menu, MenuL, MenuM;

            static Modes()
            {
                Menu = Config.Menu.AddSubMenu("Modes");
                MenuL = Config.Menu.AddSubMenu("Lane/Jungle Clear");
                MenuM = Config.Menu.AddSubMenu("Misc/Drawing");

                Combo.Initialize();
                Menu.AddSeparator();
                Harass.Initialize();
                LaneClear.Initialize();
                Misc.Initialize();
                Menu.AddSeparator();
                Drawing.Initialize();
            }

            public static void Initialize()
            {
            }

            public static class Combo
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;
                private static readonly CheckBox _useE;
                private static readonly CheckBox _useR;
                private static readonly Slider _useRmin;

                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }
                public static bool UseW
                {
                    get { return _useW.CurrentValue; }
                }
                public static bool UseE
                {
                    get { return _useE.CurrentValue; }
                }
                public static bool UseR
                {
                    get { return _useR.CurrentValue; }
                }
                public static int UseRmin
                {
                    get { return _useRmin.CurrentValue; }
                }
                


                static Combo()
                {
                    Menu.AddGroupLabel("Kombo");
                    _useQ = Menu.Add("comboUseQ", new CheckBox("Kullan Q"));
                    _useW = Menu.Add("comboUseW", new CheckBox("Kullan W"));
                    _useE = Menu.Add("comboUseE", new CheckBox("Kullan E"));
                    _useR = Menu.Add("comboUseR", new CheckBox("Kullan R", false));
                    _useRmin = Menu.Add("comboUseRmin", new Slider("Kullan R En az düşman", 2, 1, 5));
                }

                public static void Initialize()
                {
                }
            }

            public static class Harass
            {
                public static bool UseQ
                {
                    get { return Menu["harassUseQ"].Cast<CheckBox>().CurrentValue; }
                }
                public static bool UseE
                {
                    get { return Menu["harassUseE"].Cast<CheckBox>().CurrentValue; }
                }
                public static int Mana
                {
                    get { return Menu["harassMana"].Cast<Slider>().CurrentValue; }
                }
                public static bool UseQauto
                {
                    get { return Menu["H_AutoQ"].Cast<CheckBox>().CurrentValue; }
                }
                public static bool UseEauto
                {
                    get { return Menu["H_AutoE"].Cast<CheckBox>().CurrentValue; }
                }
                public static int ManaAuto
                {
                    get { return Menu["H_ESlinder"].Cast<Slider>().CurrentValue; }
                }
                static Harass()
                {
                    Menu.AddGroupLabel("Dürtme");
                    Menu.Add("harassUseQ", new CheckBox("Kullan Q"));
                    Menu.Add("harassUseE", new CheckBox("Kullan E"));
                    Menu.Add("harassMana", new Slider("Maximum mana usage in percent ({0}%)", 40));
                    Menu.Add("H_AutoQ", new CheckBox("Auto-Q enemies"));
                    Menu.Add("H_AutoE", new CheckBox("Auto-E enemies"));
                    Menu.Add("H_ESlinder", new Slider("Auto Q/E Minimum Mana", 70));
                }

                public static void Initialize()
                {
                }
            }

            public static class LaneClear
            {
                public static bool UseQE
                {
                    get { return MenuL["LaneClearUseQE"].Cast<CheckBox>().CurrentValue; }
                }
                public static int Mana
                {
                    get { return MenuL["LaneClearMana"].Cast<Slider>().CurrentValue; }
                }
                public static int MinNumberQE
                {
                    get { return MenuL["laneNumQE"].Cast<Slider>().CurrentValue; }
                }

                static LaneClear()
                {
                    MenuL.AddGroupLabel("LaneTemizleme/orman Temizleme");
                    MenuL.Add("LaneClearUseQE", new CheckBox("Kullan QE"));
                    MenuL.Add("laneNumQE", new Slider("QE için en az minyon", 3, 1, 10));
                    MenuL.Add("LaneClearMana", new Slider("en az mana ({0}%)", 70));
                }

                public static void Initialize()
                {
                }
            }

            public static class Misc
            {
                public static bool autoshield
                {
                    get { return MenuM["autoshield"].Cast<CheckBox>().CurrentValue; }
                }

                public static bool UseWint
                {
                    get { return MenuM["UseWint"].Cast<CheckBox>().CurrentValue; }
                }

                public static int UseWhea
                {
                    get { return MenuM["UseWhea"].Cast<Slider>().CurrentValue; }
                }

                public static int UseWman
                {
                    get { return MenuM["UseWman"].Cast<Slider>().CurrentValue; }
                }

                static Misc()
                {
                    MenuM.AddGroupLabel("Ek ayarlar");
                    MenuM.Add("autoshield", new CheckBox("URF de Otomatik Kalkan"));
                    MenuM.Add("UseWint", new CheckBox("Tehlikeli büyülerde W"));
                    MenuM.Add("UseWhea", new Slider("otomatik W için canım az %", 70, 1,100));
                    MenuM.Add("UseWman", new Slider("Otomatik W için manam şundan fazla %", 50, 1, 100));
                    
                }

                public static void Initialize()
                {
                }
            }

            public static class Drawing
            {
                private static readonly CheckBox _drawQ;
                private static readonly CheckBox _drawW;
                private static readonly CheckBox _drawE;
                private static readonly CheckBox _drawR;
                private static readonly CheckBox _healthbar;

                public static bool DrawQ
                {
                    get { return _drawQ.CurrentValue; }
                }
                public static bool DrawW
                {
                    get { return _drawW.CurrentValue; }
                }
                public static bool DrawE
                {
                    get { return _drawE.CurrentValue; }
                }
                public static bool DrawR
                {
                    get { return _drawR.CurrentValue; }
                }
                public static bool IndicatorHealthbar
                {
                    get { return _healthbar.CurrentValue; }
                }

                static Drawing()
                {
                    MenuM.AddGroupLabel("BÜyü Menzilleri");
                    _drawQ = MenuM.Add("drawQ", new CheckBox("Q Menzili", false));
                    _drawW = MenuM.Add("drawW", new CheckBox("W Menzili", false));
                    _drawE = MenuM.Add("drawE", new CheckBox("E Menzili", false));
                    _drawR = MenuM.Add("drawR", new CheckBox("R Menzili", false));
                    _healthbar = MenuM.Add("healthbar", new CheckBox("Canbarı Gösterimi"));


                }

                public static void Initialize()
                {
                    
                }
            }

        }


    }
}
