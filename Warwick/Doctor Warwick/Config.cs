using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

// ReSharper disable InconsistentNaming
// ReSharper disable MemberHidesStaticFromOuterClass

namespace Warwick
{
    public static class Config
    {
        private const string MenuName = "Warwick";

        private static readonly Menu Menu;

        static Config()
        {
            Menu = MainMenu.AddMenu(MenuName, MenuName.ToLower());
            Menu.AddGroupLabel("Doctor Warwick");
            Menu.AddLabel("Otomatik Q");
            Menu.AddLabel("İyi Şanslar.");
            ModesMenu.Initialize();
            PredictionMenu.Initialize();
            ManaManagerMenu.Initialize();
            MiscMenu.Initialize();
            DrawingMenu.Initialize();
            DebugMenu.Initialize();
        }

        public static void Initialize()
        {
        }

        public static class ModesMenu
        {
            public static readonly Menu MenuModes;

            static ModesMenu()
            {
                MenuModes = Config.Menu.AddSubMenu("Modes");

                Combo.Initialize();
                MenuModes.AddSeparator();

                Harass.Initialize();
                MenuModes.AddSeparator();

                LaneClear.Initialize();
                MenuModes.AddSeparator();

                JungleClear.Initialize();
                MenuModes.AddSeparator();

                LastHit.Initialize();
                MenuModes.AddSeparator();

                Flee.Initialize();
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
                private static readonly CheckBox _useSmite;
                private static readonly CheckBox _useItems;
                private static readonly Slider _maxBOTRKHPEnemy;
                private static readonly Slider _maxBOTRKHPPlayer;

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

                public static bool UseSmite
                {
                    get { return _useSmite.CurrentValue; }
                }

                public static bool UseItems
                {
                    get { return _useItems.CurrentValue; }
                }


                public static int MaxBOTRKHPPlayer
                {
                    get { return _maxBOTRKHPPlayer.CurrentValue; }
                }

                public static int MaxBOTRKHPEnemy
                {
                    get { return _maxBOTRKHPEnemy.CurrentValue; }
                }

                static Combo()
                {
                    MenuModes.AddGroupLabel("Combo");
                    _useQ = MenuModes.Add("comboUseQ", new CheckBox("Kullan Q"));
                    _useW = MenuModes.Add("comboUseW", new CheckBox("Kullan W"));
                    _useE = MenuModes.Add("comboUseE", new CheckBox("Kullan E"));
                    _useR = MenuModes.Add("comboUseR", new CheckBox("Kullan R"));
                    _useSmite = MenuModes.Add("comboUseSmite", new CheckBox("Kullan Çarp"));
                    _useItems = MenuModes.Add("comboUseItems", new CheckBox("Kullan Cutlass/Mahvolmuş/Youmuu"));
                    _maxBOTRKHPPlayer = MenuModes.Add("comboMaxBotrkHpPlayer",
                        new Slider("Mahvolmuş için  can", 80, 0, 100));
                    _maxBOTRKHPEnemy = MenuModes.Add("comboMaxBotrkHpEnemy",
                        new Slider("Mahvolmuş için rakip can", 100, 0, 100));
                }

                public static void Initialize()
                {
                }
            }

            public static class Harass
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;
                private static readonly CheckBox _useAutoQ;
                private static readonly Slider _minAutoQMana;

                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }

                public static bool UseW
                {
                    get { return _useW.CurrentValue; }
                }

                public static bool UseAutoQ
                {
                    get { return _useW.CurrentValue; }
                }

                public static int MinAutoQMana
                {
                    get { return _minAutoQMana.CurrentValue; }
                }

                static Harass()
                {
                    MenuModes.AddGroupLabel("Harass");
                    _useQ = MenuModes.Add("harassUseQ", new CheckBox("Kullan Q"));
                    _useW = MenuModes.Add("harassUseW", new CheckBox("Kullan W"));
                    _useAutoQ = MenuModes.Add("harassUseAutoQ", new CheckBox("Kullan Otomatik Q"));
                    _minAutoQMana = MenuModes.Add("minQMana", new Slider("Otomatik Q için MAna", 60, 0, 100));
                    foreach (var enemy in EntityManager.Heroes.Enemies)
                    {
                        MenuModes.Add("blacklist" + enemy.ChampionName, new CheckBox("Otomatik Q Kullanma " + enemy.ChampionName, false));
                    }
                }

                public static void Initialize()
                {
                }
            }

            public static class LaneClear
            {

                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;

                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }

                public static bool UseW
                {
                    get { return _useW.CurrentValue; }
                }

                static LaneClear()
                {
                    MenuModes.AddGroupLabel("LaneClear");
                    _useQ = MenuModes.Add("laneUseQ", new CheckBox("Kullan Q"));
                    _useW = MenuModes.Add("laneUseW", new CheckBox("Kullan W"));
                }

                public static void Initialize()
                {
                }
            }

            public static class JungleClear
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;

                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }

                public static bool UseW
                {
                    get { return _useW.CurrentValue; }
                }

                static JungleClear()
                {
                    MenuModes.AddGroupLabel("JungleClear");
                    _useQ = MenuModes.Add("jungleUseQ", new CheckBox("Kullan Q"));
                    _useW = MenuModes.Add("jungleUseW", new CheckBox("Kullan W"));
                }

                public static void Initialize()
                {
                }
            }

            public static class LastHit
            {
                private static readonly CheckBox _useQ;

                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }


                static LastHit()
                {
                    MenuModes.AddGroupLabel("LastHit");
                    _useQ = MenuModes.Add("lastHitUseQ", new CheckBox("Kullan Q"));
                }

                public static void Initialize()
                {
                }
            }

            public static class Flee
            {
                private static readonly CheckBox _useE;

                public static bool UseE
                {
                    get { return _useE.CurrentValue; }
                }

                static Flee()
                {
                    MenuModes.AddGroupLabel("Flee");
                    _useE = MenuModes.Add("fleeUseE", new CheckBox("Kullan E"));
                }

                public static void Initialize()
                {
                }
            }
        }

        public static class MiscMenu
        {
            private static readonly Menu MenuMisc;
            private static readonly CheckBox _interruptR;
            private static readonly CheckBox _potion;
            private static readonly CheckBox _ksQ;
            private static readonly CheckBox _ksIgnite;
            private static readonly CheckBox _ksSmite;
            private static readonly Slider _potionMinHP;
            private static readonly Slider _potionMinMP;

            public static bool InterruptR
            {
                get { return _interruptR.CurrentValue; }
            }
            public static bool KsQ
            {
                get { return _ksQ.CurrentValue; }
            }
            public static bool KsIgnite
            {
                get { return _ksIgnite.CurrentValue; }
            }
            public static bool KsSmite
            {
                get { return _ksSmite.CurrentValue; }
            }
            public static bool Potion
            {
                get { return _potion.CurrentValue; }
            }
            public static int potionMinHP
            {
                get { return _potionMinHP.CurrentValue; }
            }
            public static int potionMinMP
            {
                get { return _potionMinMP.CurrentValue; }
            }

            static MiscMenu()
            {
                MenuMisc = Config.Menu.AddSubMenu("Misc");
                MenuMisc.AddGroupLabel("AntiGapcloser");
                MenuMisc.AddGroupLabel("Interrupter");
                _interruptR = MenuMisc.Add("interruptR", new CheckBox("Önemli İnterrupt büyülerde R Kullan", false));
                MenuMisc.AddGroupLabel("KillSteal");
                _ksQ = MenuMisc.Add("ksQ", new CheckBox("KillSteal Q", false));
                _ksIgnite = MenuMisc.Add("ksIgnite", new CheckBox("Tutuşturla kill çal"));
                _ksSmite = MenuMisc.Add("ksSmite", new CheckBox("Çaprla kill çal"));
                MenuMisc.AddGroupLabel("Auto pot usage");
                _potion = MenuMisc.Add("potion", new CheckBox("Pot kullan"));
                _potionMinHP = MenuMisc.Add("potionminHP", new Slider("HP potu için min can", 70));
                _potionMinMP = MenuMisc.Add("potionMinMP", new Slider("Mana potu için min mana", 20));
                MenuMisc.AddGroupLabel("Diğer");
            }

            public static void Initialize()
            {
            }
        }

        public static class ManaManagerMenu
        {
            private static readonly Menu MenuManaManager;
            private static readonly Slider _minQMana;
            private static readonly Slider _minWMana;
            private static readonly Slider _minRMana;

            public static int MinQMana
            {
                get { return _minQMana.CurrentValue; }
            }
            public static int MinWMana
            {
                get { return _minWMana.CurrentValue; }
            }
            public static int MinRMana
            {
                get { return _minRMana.CurrentValue; }
            }

            static ManaManagerMenu()
            {
                MenuManaManager = Config.Menu.AddSubMenu("Mana Manager");
                _minQMana = MenuManaManager.Add("minQMana", new Slider("Q için gereken mana %", 25, 0, 100));
                _minWMana = MenuManaManager.Add("minWMana", new Slider("W için gereken mana %", 0, 0, 100));
                _minRMana = MenuManaManager.Add("minRMana", new Slider("R için gereken mana %", 0, 0, 100));
            }

            public static void Initialize()
            {
            }
        }

        public static class PredictionMenu
        {
            private static readonly Menu MenuPrediction;

            static PredictionMenu()
            {
                MenuPrediction = Config.Menu.AddSubMenu("Prediction");
                MenuPrediction.AddLabel("İsabet oranını Kontrol edebilirsin");
                MenuPrediction.AddSeparator();
                MenuPrediction.AddLabel("V bsıdlıyken isabet oranı dikkate alma");
            }

            public static void Initialize()
            {

            }
        }

        public static class DrawingMenu
        {
            private static readonly Menu MenuDrawing;
            private static readonly CheckBox _drawQ;
            private static readonly CheckBox _drawE;
            private static readonly CheckBox _drawR;
            private static readonly CheckBox _drawIgnite;
            private static readonly CheckBox _drawSmite;
            private static readonly CheckBox _drawOnlyReady;
            private static readonly CheckBox _drawLasthittable;

            public static bool DrawQ
            {
                get { return _drawQ.CurrentValue; }
            }
            public static bool DrawE
            {
                get { return _drawE.CurrentValue; }
            }
            public static bool DrawR
            {
                get { return _drawR.CurrentValue; }
            }
            public static bool DrawIgnite
            {
                get { return _drawIgnite.CurrentValue; }
            }
            public static bool DrawSmite
            {
                get { return _drawSmite.CurrentValue; }
            }
            public static bool DrawOnlyReady
            {
                get { return _drawOnlyReady.CurrentValue; }
            }
            public static bool DrawLasthittable
            {
                get { return _drawLasthittable.CurrentValue; }
            }

            static DrawingMenu()
            {
                MenuDrawing = Config.Menu.AddSubMenu("Drawing");
                _drawQ = MenuDrawing.Add("drawQ", new CheckBox("Göster Q"));
                _drawE = MenuDrawing.Add("drawE", new CheckBox("Göster E"));
                _drawR = MenuDrawing.Add("drawR", new CheckBox("Göster R"));
                _drawIgnite = MenuDrawing.Add("drawIgnite", new CheckBox("Göster Tutuştur"));
                _drawSmite = MenuDrawing.Add("drawSmite", new CheckBox("Göster Çarp"));
                _drawOnlyReady = MenuDrawing.Add("drawOnlyReady", new CheckBox("Sadece hazır büyüler"));
                _drawLasthittable = MenuDrawing.Add("drawLasthittable", new CheckBox("Q ile son vuruş yapılacak minyonlar"));
            }

            public static void Initialize()
            {
            }
        }

        public static class DebugMenu
        {
            private static readonly Menu MenuDebug;
            private static readonly CheckBox _debugChat;
            private static readonly CheckBox _debugConsole;

            public static bool DebugChat
            {
                get { return _debugChat.CurrentValue; }
            }
            public static bool DebugConsole
            {
                get { return _debugConsole.CurrentValue; }
            }

            static DebugMenu()
            {
                MenuDebug = Config.Menu.AddSubMenu("Debug");
                MenuDebug.AddLabel("Burası hata olduğunda çalışır");
                _debugChat = MenuDebug.Add("debugChat", new CheckBox("Hata mesajlarını chatte göster", false));
                _debugConsole = MenuDebug.Add("debugConsole", new CheckBox("Hata mesajlarını konsolda göster", false));
            }

            public static void Initialize()
            {

            }
        }
    }
}
