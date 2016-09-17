using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

// ReSharper disable InconsistentNaming
// ReSharper disable MemberHidesStaticFromOuterClass

namespace GenesisUrgot
{
    // I can't really help you with my layout of a good config class
    // since everyone does it the way they like it most, go checkout my
    // config classes I make on my GitHub if you wanna take over the
    // complex way that I use
    public static class Config
    {
        private const string MenuName = "GenesisUrgot";

        private static readonly Menu Menu;

        static Config()
        {
            // Initialize the menu
            Menu = MainMenu.AddMenu(MenuName, MenuName.ToLower());
            Menu.AddGroupLabel("GenesisUrgot'a Hoşgeldin, Bu gerçek Urgottur!");
            Menu.AddGroupLabel("Çeviri tradana");
            // Initialize the modes
            Modes.Initialize();
        }

        public static void Initialize()
        {
        }

        public static class Modes
        {
            private static readonly Menu ComboMenu;
            private static readonly Menu ShieldMenu;
            private static readonly Menu InterruptMenu;
            private static readonly Menu HarassMenu;
            private static readonly Menu MiscMenu;
            private static readonly Menu LCMenu;

            static Modes()
            {
                // Initialize the menu
                ComboMenu = Menu.AddSubMenu("Combo");
                Combo.Initialize();
                InterruptMenu = Menu.AddSubMenu("Interrupt");
                Interrupt.Initialize();
                ShieldMenu = Menu.AddSubMenu("Shield");
                ShieldManager.Initialize();
                HarassMenu = Menu.AddSubMenu("Harass");
                Harass.Initialize();
                LCMenu = Menu.AddSubMenu("LastHit");
                LC.Initialize();
                MiscMenu = Menu.AddSubMenu("Misc");
                PermaActive.Initialize();

                // Initialize all modes
                // Combo

                Menu.AddSeparator();

                // Harass
            }

            public static void Initialize()
            {
            }

            public static class PermaActive
            {
                private static readonly CheckBox _stackTearQ;
                private static readonly CheckBox _useR;

                public static bool StackTearQ
                {
                    get
                    {
                        return _stackTearQ.CurrentValue;
                    }
                }

                public static bool UseR
                {
                    get
                    {
                        return _useR.CurrentValue;
                    }
                }

                static PermaActive()
                {
                    MiscMenu.AddLabel("Sadece Şu durumda");
                    _useR = MiscMenu.Add("AlwaysUseR", new CheckBox("R ye basınca düşman Kule Altına çekilecekse"));

                    _stackTearQ = MiscMenu.Add("AlwaysUseWFlee", new CheckBox("Q ile Tanrıçanın Gözyaşı"));
                }

                public static void Initialize()
                {
                }
            }

            public static class ShieldManager
            {
                private static readonly CheckBox _useW;
                private static readonly CheckBox _useHumanizer;

                public static bool UseW
                {
                    get
                    {
                        return _useW.CurrentValue;
                    }
                }

                public static bool UseHumanizer
                {
                    get
                    {
                        return _useHumanizer.CurrentValue;
                    }
                }

                static ShieldManager()
                {
                    // Initialize the menu values
                    ShieldMenu.AddGroupLabel("Kalkan Ayarları");
                    _useW = ShieldMenu.Add("UseW", new CheckBox("Kullan W"));
                    _useHumanizer = ShieldMenu.Add("Humanizer", new CheckBox("İnsancıl Ayar Kullan", false));
                }

                public static void Initialize()
                {
                }
            }

            public static class Combo
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;
                private static readonly CheckBox _useE;
                private static readonly CheckBox _useR;

                public static bool UseQ
                {
                    get
                    {
                        return _useQ.CurrentValue;
                    }
                }

                public static bool UseW
                {
                    get
                    {
                        return _useW.CurrentValue;
                    }
                }

                public static bool UseE
                {
                    get
                    {
                        return _useE.CurrentValue;
                    }
                }

                public static bool UseR
                {
                    get
                    {
                        return _useR.CurrentValue;
                    }
                }

                static Combo()
                {
                    // Initialize the menu values
                    ComboMenu.AddGroupLabel("Kombo");
                    _useQ = ComboMenu.Add("comboUseQ", new CheckBox("Kullan Q"));
                    _useW = ComboMenu.Add("comboUseW", new CheckBox("Kullan W"));
                    _useE = ComboMenu.Add("comboUseE", new CheckBox("Kullan E"));
                    _useR = ComboMenu.Add("comboUseR", new CheckBox("Kullan R"));
                }

                public static void Initialize()
                {
                }
            }

            public static class Interrupt
            {
                private static readonly CheckBox _useR;

                public static bool UseR
                {
                    get
                    {
                        return _useR.CurrentValue;
                    }
                }

                static Interrupt()
                {
                    // Initialize the menu values
                    InterruptMenu.AddGroupLabel("Tehlikeli yeteneği bozmak için R kullan");
                    _useR = InterruptMenu.Add("InterruptUseQ", new CheckBox("Kullan R"));
                }

                public static void Initialize()
                {
                }
            }

            public static class Harass
            {
                public static bool UseQ
                {
                    get
                    {
                        return HarassMenu["harassUseQ"].Cast<CheckBox>().CurrentValue;
                    }
                }
                public static bool UseE
                {
                    get
                    {
                        return HarassMenu["harassUseE"].Cast<CheckBox>().CurrentValue;
                    }
                }

                static Harass()
                {
                    HarassMenu.AddGroupLabel("Dürtme");
                    HarassMenu.Add("harassUseQ", new CheckBox("Kullan Q"));
                    HarassMenu.Add("harassUseE", new CheckBox("Kullan E"));
                }

                public static void Initialize()
                {
                }
            }

            public static class LC
            {
                private static readonly CheckBox _useQ;

                public static bool UseQ
                {
                    get
                    {
                        return _useQ.CurrentValue;
                    }
                }
                public static int QMana
                {
                    get
                    {
                        return LCMenu["QMana"].Cast<Slider>().CurrentValue;
                    }
                }

                static LC()
                {
                    // Initialize the menu values
                    LCMenu.AddGroupLabel("SonVuruş");
                    _useQ = LCMenu.Add("UseQ", new CheckBox("Kullan Q"));
                    LCMenu.Add("QMana", new Slider("Q kullan eğer manam şundan az değilse% ({0}%)", 40));
                }

                public static void Initialize()
                {
                }
            }
        }
    }
}
