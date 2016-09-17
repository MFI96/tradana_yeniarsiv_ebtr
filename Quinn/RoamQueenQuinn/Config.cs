﻿using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK;

// ReSharper disable InconsistentNaming
// ReSharper disable MemberHidesStaticFromOuterClass
namespace RoamQueenQuinn
{
    // I can't really help you with my layout of a good config class
    // since everyone does it the way they like it most, go checkout my
    // config classes I make on my GitHub if you wanna take over the
    // complex way that I use
    public static class Config
    {
        private const string MenuName = "RoamQueenQuinn";

        private static readonly Menu Menu;

        static Config()
        {
            // Initialize the menu
            Menu = MainMenu.AddMenu(MenuName, MenuName.ToLower());
            Menu.AddGroupLabel("Welcome to RoamQueenQuinn by TopGunner");

            // Initialize the modes
            Modes.Initialize();
        }

        public static void Initialize()
        {
        }


        public static class Misc
        {

            private static readonly Menu Menu;
            public static readonly CheckBox _drawQ;
            public static readonly CheckBox _drawW;
            public static readonly CheckBox _drawE;
            private static readonly CheckBox _ksQ;
            private static readonly CheckBox _useHarrier;
            private static readonly CheckBox _interruptE;
            private static readonly CheckBox _useHeal;
            private static readonly CheckBox _useQSS;
            private static readonly CheckBox _castROnBase;
            private static readonly CheckBox _autoBuyStartingItems;
            private static readonly CheckBox _autolevelskills;
            private static readonly Slider _skinId;
            public static readonly CheckBox _useSkinHack;
            private static readonly CheckBox[] _useHealOn = { new CheckBox("", false), new CheckBox("", false), new CheckBox("", false), new CheckBox("", false), new CheckBox("", false) };

            public static bool useHealOnI(int i)
            {
                return _useHealOn[i].CurrentValue;
            }
            public static bool ksQ
            {
                get { return _ksQ.CurrentValue; }
            }
            public static bool useHarrier
            {
                get { return _useHarrier.CurrentValue; }
            }
            public static bool interruptE
            {
                get { return _interruptE.CurrentValue; }
            }
            public static bool useHeal
            {
                get { return _useHeal.CurrentValue; }
            }
            public static bool useQSS
            {
                get { return _useQSS.CurrentValue; }
            }
            public static bool autoBuyStartingItems
            {
                get { return _autoBuyStartingItems.CurrentValue; }
            }
            public static bool autolevelskills
            {
                get { return _autolevelskills.CurrentValue; }
            }
            public static bool castROnBase
            {
                get { return _castROnBase.CurrentValue; }
            }
            public static int skinId
            {
                get { return _skinId.CurrentValue; }
            }
            public static bool UseSkinHack
            {
                get { return _useSkinHack.CurrentValue; }
            }


           static Misc()
            {
                // Initialize the menu values
                Menu = Config.Menu.AddSubMenu("Misc");
                _drawQ = Menu.Add("drawQ", new CheckBox("Göster Q"));
                _drawW = Menu.Add("drawW", new CheckBox("Göster W"));
                _drawE = Menu.Add("drawE", new CheckBox("Göster E"));
                Menu.AddSeparator();
                _useHarrier = Menu.Add("Harrier", new CheckBox("Kullan Bariyer"));
                _castROnBase = Menu.Add("castROnBase", new CheckBox("R ile base yakala"));
                _interruptE = Menu.Add("InterruptE", new CheckBox("interrupt için E"));
                Menu.AddSeparator();
                _ksQ = Menu.Add("ksQ", new CheckBox("Akıllı KS Q ile"));
                Menu.AddSeparator();
                _useHeal = Menu.Add("useHeal", new CheckBox("Akıllı CAn"));
                _useQSS = Menu.Add("useQSS", new CheckBox("Kullan QSS"));
                Menu.AddSeparator();
                for (int i = 0; i < EntityManager.Heroes.Allies.Count; i++)
                {
                    _useHealOn[i] = Menu.Add("useHeal" + i, new CheckBox("Kullan can korumak için " + EntityManager.Heroes.Allies[i].ChampionName));
                }
                Menu.AddSeparator();
                _autolevelskills = Menu.Add("autolevelskills", new CheckBox("Otomatik seviye arttırma"));
                _autoBuyStartingItems = Menu.Add("autoBuyStartingItems", new CheckBox("Başlangıçta otomatik bişey alma"));
                Menu.AddSeparator();
                _useSkinHack = Menu.Add("useSkinHack", new CheckBox("Skin değiştriici"));
                _skinId = Menu.Add("skinId", new Slider("Skin Numarası", 2, 1, 4));
            }

            public static void Initialize()
            {
            }

        }

        public static class Modes
        {
            private static readonly Menu Menu;

            static Modes()
            {
                // Initialize the menu
                Menu = Config.Menu.AddSubMenu("Modes");

                // Initialize all modes
                // Combo
                Combo.Initialize();
                Menu.AddSeparator();

                // Harass
                Harass.Initialize();
                LaneClear.Initialize();
                JungleClear.Initialize();
            }

            public static void Initialize()
            {
            }

            public static class Combo
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;
                private static readonly CheckBox _useE;
                private static readonly CheckBox _useBOTRK;
                private static readonly CheckBox _useYOUMOUS;
                private static readonly CheckBox _useTrinketVision;

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
                public static bool useBOTRK
                {
                    get { return _useBOTRK.CurrentValue; }
                }
                public static bool useYOUMOUS
                {
                    get { return _useYOUMOUS.CurrentValue; }
                }
                public static bool useTrinketVision
                {
                    get { return _useTrinketVision.CurrentValue; }
                }

                static Combo()
                {
                    // Initialize the menu values
                    Menu.AddGroupLabel("Combo");
                    _useQ = Menu.Add("comboUseQ", new CheckBox("Kullan Q"));
                    _useW = Menu.Add("comboUseW", new CheckBox("Akıllı W Kullan"));
                    _useE = Menu.Add("comboUseE", new CheckBox("E Kullan"));
                    Menu.AddSeparator();
                    _useBOTRK = Menu.Add("useBotrk", new CheckBox("Mahvolmuş Kullan"));
                    _useYOUMOUS = Menu.Add("useYoumous", new CheckBox("Youmous Kullan"));
                    _useTrinketVision = Menu.Add("useTrinketVision", new CheckBox("Gösterici totem kullan"));
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
                public static bool UseR
                {
                    get { return Menu["harassUseR"].Cast<CheckBox>().CurrentValue; }
                }
                public static int Mana
                {
                    get { return Menu["harassMana"].Cast<Slider>().CurrentValue; }
                }

                static Harass()
                {
                    // Here is another option on how to use the menu, but I prefer the
                    // way that I used in the combo class
                    Menu.AddGroupLabel("Harass");
                    Menu.Add("harassUseQ", new CheckBox("Kullan Q"));
                    Menu.Add("harassUseE", new CheckBox("Kullan E", false)); // Default false

                    // Adding a slider, we have a little more options with them, using {0} {1} and {2}
                    // in the display name will replace it with 0=current 1=min and 2=max value
                    Menu.Add("harassMana", new Slider("Gereken mana ({0}%)", 60));
                }
                public static void Initialize()
                {
                }

            }

            public static class LaneClear
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useE;
                private static readonly Slider _mana;

                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }
                public static bool UseE
                {
                    get { return _useE.CurrentValue; }
                }
                public static int mana
                {
                    get { return _mana.CurrentValue; }
                }

                static LaneClear()
                {
                    // Initialize the menu values
                    Menu.AddGroupLabel("Lane Clear");
                    _useQ = Menu.Add("clearUseQ", new CheckBox("Kullan Q"));
                    _useE = Menu.Add("clearUseE", new CheckBox("Kullan E"));
                    _mana = Menu.Add("clearMana", new Slider("Gereken mana ({0}%)", 40));
                }

                public static void Initialize()
                {
                }
            }
            public static class JungleClear
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useE;
                private static readonly Slider _mana;

                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }
                public static bool UseE
                {
                    get { return _useE.CurrentValue; }
                }
                public static int mana
                {
                    get { return _mana.CurrentValue; }
                }

                static JungleClear()
                {
                    // Initialize the menu values
                    Menu.AddGroupLabel("Jungle Clear");
                    _useQ = Menu.Add("jglUseQ", new CheckBox("Kullan Q"));
                    _useE = Menu.Add("jglUseE", new CheckBox("Kullan E"));
                    _mana = Menu.Add("jglMana", new Slider("Gereken mana ({0}%)", 40));
                }

                public static void Initialize()
                {
                }
            }
        }
    }
}
