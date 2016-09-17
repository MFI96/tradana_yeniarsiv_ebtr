using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

// ReSharper disable InconsistentNaming
// ReSharper disable MemberHidesStaticFromOuterClass

namespace Gnaaar
{
    public static class Config
    {
        public const string MenuName = "Gnaaar";
        public const string LongMenuName = MenuName + " - The Hamster of Doom";
        private static readonly Menu Menu;

        static Config()
        {
            // Initialize menu
            Menu = MainMenu.AddMenu(MenuName, MenuName + "_hellsing", LongMenuName);

            // Initialize sub menus
            Modes.Initialize();
        }

        public static void Initialize()
        {
        }

        public static class Modes
        {
            public const string MenuName = "Modes";
            private static readonly Menu Menu;

            static Modes()
            {
                // Initialize menu
                Menu = Config.Menu.AddSubMenu(MenuName);

                // Initialize sub groups
                Combo.Initialize();
                Menu.AddSeparator();
                Harass.Initialize();
                Menu.AddSeparator();
                LaneClear.Initialize();
                Menu.AddSeparator();
                JungleClear.Initialize();
                Menu.AddSeparator();
                Flee.Initialize();
            }

            public static void Initialize()
            {
            }

            public static class Combo
            {
                public const string GroupName = "Combo";

                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useE;
                private static readonly CheckBox _useQMega;
                private static readonly CheckBox _useWMega;
                private static readonly CheckBox _useEMega;
                private static readonly CheckBox _useRMega;

                private static readonly CheckBox _useItems;
                private static readonly CheckBox _useIgnite;

                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }
                public static bool UseE
                {
                    get { return _useE.CurrentValue; }
                }
                public static bool UseQMega
                {
                    get { return _useQMega.CurrentValue; }
                }
                public static bool UseWMega
                {
                    get { return _useWMega.CurrentValue; }
                }
                public static bool UseEMega
                {
                    get { return _useEMega.CurrentValue; }
                }
                public static bool UseRMega
                {
                    get { return _useRMega.CurrentValue; }
                }

                public static bool UseItems
                {
                    get { return _useItems.CurrentValue; }
                }
                public static bool UseIgnite
                {
                    get { return _useIgnite.CurrentValue; }
                }

                static Combo()
                {
                    // Initialize group
                    Menu.AddGroupLabel(GroupName);

                    Menu.AddLabel("Küçükken");
                    _useQ = Menu.Add("useQ", new CheckBox("Kullan Q"));
                    _useE = Menu.Add("useE", new CheckBox("Kullan E"));
                    Menu.AddLabel("Kocamaaaan");
                    _useQMega = Menu.Add("useQMega", new CheckBox("Kullan Q"));
                    _useWMega = Menu.Add("useWMega", new CheckBox("Kullan W"));
                    _useEMega = Menu.Add("useEMega", new CheckBox("Kullan E"));
                    _useRMega = Menu.Add("useRMega", new CheckBox("Kullan R"));

                    Menu.AddLabel("Gelişmiş Seçenekler:");

                    _useItems = Menu.Add("comboUseItems", new CheckBox("İtemleri Kullan"));
                    _useIgnite = Menu.Add("comboUseIgnite", new CheckBox("Tutuştur Kullan"));
                }

                public static void Initialize()
                {
                }
            }

            public static class Harass
            {
                public const string GroupName = "Harass";

                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useQMega;
                private static readonly CheckBox _useWMega;

                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }
                public static bool UseQMega
                {
                    get { return _useQMega.CurrentValue; }
                }
                public static bool UseWMega
                {
                    get { return _useWMega.CurrentValue; }
                }

                static Harass()
                {
                    // Initialize group
                    Menu.AddGroupLabel(GroupName);

                    Menu.AddLabel("Küçükken");
                    _useQ = Menu.Add("harassUseQ", new CheckBox("Kullan Q"));
                    Menu.AddLabel("Kocamaaaan");
                    _useQMega = Menu.Add("harassUseQMega", new CheckBox("Kullan Q"));
                    _useWMega = Menu.Add("harassUseWMega", new CheckBox("Kullan W"));
                }

                public static void Initialize()
                {
                }
            }

            public static class LaneClear
            {
                public const string GroupName = "LaneClear";

                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useQMega;
                private static readonly CheckBox _useWMega;
                private static readonly CheckBox _useEMega;

                private static readonly CheckBox _useItems;

                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }
                public static bool UseQMega
                {
                    get { return _useQMega.CurrentValue; }
                }
                public static bool UseWMega
                {
                    get { return _useWMega.CurrentValue; }
                }
                public static bool UseEMega
                {
                    get { return _useEMega.CurrentValue; }
                }

                public static bool UseItems
                {
                    get { return _useItems.CurrentValue; }
                }

                static LaneClear()
                {
                    // Initialize group
                    Menu.AddGroupLabel(GroupName);

                    Menu.AddLabel("Küçükken");
                    _useQ = Menu.Add("waveUseQ", new CheckBox("Kullan Q"));
                    Menu.AddLabel("Kocamaaaan");
                    _useQMega = Menu.Add("waveUseQMega", new CheckBox("Kullan Q"));
                    _useWMega = Menu.Add("waveUseWMega", new CheckBox("Kullan W"));
                    _useEMega = Menu.Add("waveUseEMega", new CheckBox("Kullan E"));

                    Menu.AddLabel("Gelişmiş Seçenekler:");

                    _useItems = Menu.Add("waveUseItems", new CheckBox("İtemleri Kullan"));
                }

                public static void Initialize()
                {
                }
            }

            public static class JungleClear
            {
                public const string GroupName = "JungleClear";

                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useQMega;
                private static readonly CheckBox _useWMega;
                private static readonly CheckBox _useEMega;

                private static readonly CheckBox _useItems;

                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }
                public static bool UseQMega
                {
                    get { return _useQMega.CurrentValue; }
                }
                public static bool UseWMega
                {
                    get { return _useWMega.CurrentValue; }
                }
                public static bool UseEMega
                {
                    get { return _useEMega.CurrentValue; }
                }

                public static bool UseItems
                {
                    get { return _useItems.CurrentValue; }
                }

                static JungleClear()
                {
                    // Initialize group
                    Menu.AddGroupLabel(GroupName);

                    Menu.AddLabel("Küçükken");
                    _useQ = Menu.Add("jungleUseQ", new CheckBox("Kullan Q"));
                    Menu.AddLabel("Kocamaaaan");
                    _useQMega = Menu.Add("jungleUseQMega", new CheckBox("Kullan Q"));
                    _useWMega = Menu.Add("jungleUseWMega", new CheckBox("Kullan W"));
                    _useEMega = Menu.Add("jungleUseEMega", new CheckBox("Kullan E"));

                    Menu.AddLabel("Gelişmiş Seçenekler:");

                    _useItems = Menu.Add("jungleUseItems", new CheckBox("İtemleri Kullan"));
                }

                public static void Initialize()
                {
                }
            }

            public static class Flee
            {
                public const string GroupName = "Flee";

                static Flee()
                {
                    // Initialize group TODO
                    //Menu.AddGroupLabel(GroupName);
                }

                public static void Initialize()
                {
                }
            }
        }

        public static class Items
        {
            public const string MenuName = "Items";
            private static readonly Menu Menu;

            private static readonly CheckBox _useTiamat;
            private static readonly CheckBox _useHydra;
            private static readonly CheckBox _useCutlass;
            private static readonly CheckBox _useBotrk;
            private static readonly CheckBox _useYoumuu;
            private static readonly CheckBox _useRanduin;
            private static readonly CheckBox _useFace;

            public static bool UseTiamat
            {
                get { return _useTiamat.CurrentValue; }
            }
            public static bool UseHydra
            {
                get { return _useHydra.CurrentValue; }
            }
            public static bool UseCutlass
            {
                get { return _useCutlass.CurrentValue; }
            }
            public static bool UseBotrk
            {
                get { return _useBotrk.CurrentValue; }
            }
            public static bool UseYoumuu
            {
                get { return _useYoumuu.CurrentValue; }
            }
            public static bool UseRanduin
            {
                get { return _useRanduin.CurrentValue; }
            }
            public static bool UseFaceOfTheMountain
            {
                get { return _useFace.CurrentValue; }
            }

            static Items()
            {
                // Initialize menu
                Menu = Config.Menu.AddSubMenu(MenuName);

                _useTiamat = Menu.Add("itemsTiamat", new CheckBox("Kullan Tiamat"));
                _useHydra = Menu.Add("itemsHydra", new CheckBox("Kullan  Hydra"));
                _useCutlass = Menu.Add("itemsCutlass", new CheckBox("Kullan Bilgewater Palası"));
                _useBotrk = Menu.Add("itemsBotrk", new CheckBox("Kullan Mahvolmuş Kılıcı"));
                _useYoumuu = Menu.Add("itemsYoumuu", new CheckBox("Kullan Youmuu"));
                _useRanduin = Menu.Add("itemsRanduin", new CheckBox("Kullan Randuin"));
                _useFace = Menu.Add("itemsFace", new CheckBox("Kullan Dağın Sureti"));
            }

            public static void Initialize()
            {
            }
        }

        public static class Drawings
        {
            public const string MenuName = "Drawings";
            private static readonly Menu Menu;

            private static readonly CheckBox _drawQ;
            private static readonly CheckBox _drawE;
            private static readonly CheckBox _drawQMega;
            private static readonly CheckBox _drawWMega;
            private static readonly CheckBox _drawEMega;
            private static readonly CheckBox _drawRMega;

            private static readonly CheckBox _healthbar;
            private static readonly CheckBox _percent;

            public static bool DrawQ
            {
                get { return _drawQ.CurrentValue; }
            }
            public static bool DrawE
            {
                get { return _drawE.CurrentValue; }
            }
            public static bool DrawQMega
            {
                get { return _drawQMega.CurrentValue; }
            }
            public static bool DrawWMega
            {
                get { return _drawWMega.CurrentValue; }
            }
            public static bool DrawEMega
            {
                get { return _drawEMega.CurrentValue; }
            }
            public static bool DrawRMega
            {
                get { return _drawRMega.CurrentValue; }
            }

            public static bool IndicatorHealthbar
            {
                get { return _healthbar.CurrentValue; }
            }
            public static bool IndicatorPercent
            {
                get { return _percent.CurrentValue; }
            }

            static Drawings()
            {
                // Initialize menu
                Menu = Config.Menu.AddSubMenu(MenuName);

                Menu.AddGroupLabel("Büyü Menzilleri");
                Menu.AddLabel("Küçükken");
                _drawQ = Menu.Add("drawQ", new CheckBox("Q Menzili"));
                _drawE = Menu.Add("drawE", new CheckBox("E Menzili"));
                Menu.AddLabel("Kocaman");
                _drawQMega = Menu.Add("drawQMega", new CheckBox("Q Menzili"));
                _drawWMega = Menu.Add("drawWMega", new CheckBox("W Menzili"));
                _drawEMega = Menu.Add("drawEMega", new CheckBox("E Menzili"));
                _drawRMega = Menu.Add("drawRMega", new CheckBox("R Menzili", false));

                Menu.AddGroupLabel("Hasar Tespitçisi");
                _healthbar = Menu.Add("healthbar", new CheckBox("Canbarı Görünümü"));
                _percent = Menu.Add("percent", new CheckBox("Hasar bilgisi yüzde"));
            }

            public static void Initialize()
            {
            }
        }
    }
}
