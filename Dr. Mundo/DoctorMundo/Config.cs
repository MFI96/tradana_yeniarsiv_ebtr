using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

// ReSharper disable InconsistentNaming
// ReSharper disable MemberHidesStaticFromOuterClass

namespace DrMundo
{
    public static class Config
    {
        private const string MenuName = "DrMundo";

        private static readonly Menu Menu;

        static Config()
        {
            Menu = MainMenu.AddMenu(MenuName, MenuName.ToLower());
            Menu.AddGroupLabel("DoctorMundo");
            Menu.AddLabel("Akıllı Q");
            Menu.AddLabel("Ootmatik W.");
            ModesMenu.Initialize();
            PredictionMenu.Initialize();
            HealthManagerMenu.Initialize();
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
                private static readonly Slider _maxQDistance;


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

                public static int MaxQDistance
                {
                    get { return _maxQDistance.CurrentValue; }
                }

                static Combo()
                {
                    MenuModes.AddGroupLabel("Combo");
                    _useQ = MenuModes.Add("comboUseQ", new CheckBox("Kullan Q"));
                    _useW = MenuModes.Add("comboUseW", new CheckBox("Kullan W"));
                    _useE = MenuModes.Add("comboUseE", new CheckBox("Kullan E"));
                    _maxQDistance = MenuModes.Add("comboMaxQDistance",
                        new Slider("en fazla Q menzili", 800, 0, 1000));

                }

                public static void Initialize()
                {
                }
            }

            public static class Harass
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;
                private static readonly CheckBox _useE;
                private static readonly CheckBox _autoQ;
                private static readonly Slider _minAutoQHealth;


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

                public static bool AutoQ
                {
                    get { return _autoQ.CurrentValue; }
                }

                public static int MinAutoQHealth
                {
                    get { return _minAutoQHealth.CurrentValue; }
                }
                
                static Harass()
                {
                    MenuModes.AddGroupLabel("Harass");
                    _useQ = MenuModes.Add("harassUseQ", new CheckBox("Kullan Q"));
                    _autoQ = MenuModes.Add("harassAutoQ", new CheckBox("Q yu otomatik kullan"));
                    _useW = MenuModes.Add("harassUseW", new CheckBox("Kullan W"));
                    _useE = MenuModes.Add("harassUseE", new CheckBox("Kullan E"));
                    _minAutoQHealth = MenuModes.Add("minAutoQHealth", new Slider("otomatik Q için can", 65, 0, 100));
                    foreach (var enemy in EntityManager.Heroes.Enemies)
                    {
                        MenuModes.Add("blacklist" + enemy.ChampionName, new CheckBox("Otomatik Q kullanma " + enemy.ChampionName, false));
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
                private static readonly CheckBox _useE;

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

                static LaneClear()
                {
                    MenuModes.AddGroupLabel("LaneClear");
                    _useQ = MenuModes.Add("laneUseQ", new CheckBox("Kullan Q"));
                    _useW = MenuModes.Add("laneUseW", new CheckBox("Kullan W"));
                    _useE = MenuModes.Add("laneUseE", new CheckBox("Kullan E"));
                }

                public static void Initialize()
                {
                }
            }

            public static class JungleClear
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;
                private static readonly CheckBox _useE;

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

                static JungleClear()
                {
                    MenuModes.AddGroupLabel("JungleClear");
                    _useQ = MenuModes.Add("jungleUseQ", new CheckBox("Kullan Q"));
                    _useW = MenuModes.Add("jungleUseW", new CheckBox("Kullan W"));
                    _useE = MenuModes.Add("jungleUseE", new CheckBox("Kullan E"));
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
                    MenuModes.AddGroupLabel("LastHit minions");
                    _useQ = MenuModes.Add("lastHitUseQ", new CheckBox("Kullan Q"));
                }

                public static void Initialize()
                {
                }
            }

            public static class Flee
            {
                private static readonly CheckBox _useQ;
                
                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }

                static Flee()
                {
                    MenuModes.AddGroupLabel("Flee");
                    _useQ = MenuModes.Add("fleeUseQ", new CheckBox("Kullan Q", false));
                }

                public static void Initialize()
                {
                }
            }
        }

        public static class MiscMenu
        {
            private static readonly Menu MenuMisc;
            private static readonly CheckBox _gapcloserQ;
            private static readonly CheckBox _autoR;
            private static readonly CheckBox _potion;
            private static readonly CheckBox _ksQ;
            private static readonly CheckBox _ksIgnite;
            private static readonly CheckBox _autoWOff;
            private static readonly Slider _potionMinHP;
            private static readonly Slider _autoRMinHP;
            private static readonly Slider _autoRMinEnemies;
            
            public static bool GapcloserQ
            {
                get { return _gapcloserQ.CurrentValue; }
            }
            public static bool AutoR
            {
                get { return _autoR.CurrentValue; }
            }
            public static bool KsQ
            {
                get { return _ksQ.CurrentValue; }
            }
            public static bool KsIgnite
            {
                get { return _ksIgnite.CurrentValue; }
            }
            public static bool AutoWOff
            {
                get { return _autoWOff.CurrentValue; }
            }
            public static int AutoRMinHP
            {
                get { return _autoRMinHP.CurrentValue; }
            }
            public static int AutoRMinEnemies
            {
                get { return _autoRMinEnemies.CurrentValue; }
            }
            public static bool Potion
            {
                get { return _potion.CurrentValue; }
            }
            public static int potionMinHP
            {
                get { return _potionMinHP.CurrentValue; }
            }

            static MiscMenu()
            {
                MenuMisc = Config.Menu.AddSubMenu("Misc");
                MenuMisc.AddGroupLabel("AntiGapcloser");
                _gapcloserQ = MenuMisc.Add("gapcloserQ", new CheckBox("gapclosers için Q"));
                MenuMisc.AddGroupLabel("Kill Çalma");
                _ksQ = MenuMisc.Add("ksE", new CheckBox("Kill Çalma Q"));
                _ksIgnite = MenuMisc.Add("ksIgnite", new CheckBox("Kill Çalma Tutuştur"));
                MenuMisc.AddGroupLabel("otomatik r kullanımı");
                _autoR = MenuMisc.Add("autoR", new CheckBox("otomatik R kullan"));
                _autoRMinHP = MenuMisc.Add("autoRMinHP", new Slider("Ulti için cnaım şundan az", 15));
                _autoRMinEnemies = MenuMisc.Add("autoRMinEnemies", new Slider("Ulti için en az menzilimde şu düşman", 1, 0, 5));
                MenuMisc.AddGroupLabel("Otomatik İksir");
                _potion = MenuMisc.Add("potion", new CheckBox("İksirleri KUllan"));
                _potionMinHP = MenuMisc.Add("potionminHP", new Slider("can iksiri için en az can", 50));
                MenuMisc.AddGroupLabel("Diğer");
                _autoWOff = MenuMisc.Add("otherAutoWOff", new CheckBox("DÜşmana etki etmicekse W yi oto kapat"));
            }

            public static void Initialize()
            {
            }
        }

        public static class HealthManagerMenu
        {
            private static readonly Menu MenuHealthManager;
            private static readonly Slider _minQHealth;
            private static readonly Slider _minWHealth;
            private static readonly Slider _minEQHealth;

            public static int MinQHealth
            {
                get { return _minQHealth.CurrentValue; }
            }
            public static int MinWHealth
            {
                get { return _minWHealth.CurrentValue; }
            }
            public static int MinEHealth
            {
                get { return _minWHealth.CurrentValue; }
            }

            static HealthManagerMenu()
            {
                MenuHealthManager = Config.Menu.AddSubMenu("Health Manager");
                _minQHealth = MenuHealthManager.Add("minQHealth", new Slider("Q için en az can", 0, 0, 100));
                _minWHealth = MenuHealthManager.Add("minWHealth", new Slider("W için en az can", 50, 0, 100));
                _minEQHealth = MenuHealthManager.Add("minEHealth", new Slider("E için en az can", 20, 0, 100));
            }

            public static void Initialize()
            {
            }
        }

        public static class DrawingMenu
        {
            private static readonly Menu MenuDrawing;
            private static readonly CheckBox _drawQ;
            private static readonly CheckBox _drawW;
            private static readonly CheckBox _drawIgnite;
            private static readonly CheckBox _drawOnlyReady;
            private static readonly CheckBox _drawLastHitable;

            public static bool DrawQ
            {
                get { return _drawQ.CurrentValue; }
            }
            public static bool DrawW
            {
                get { return _drawW.CurrentValue; }
            }
            public static bool DrawOnlyReady
            {
                get { return _drawOnlyReady.CurrentValue; }
            }
            public static bool DrawIgnite
            {
                get { return _drawIgnite.CurrentValue; }
            }
            public static bool DrawLastHitable
            {
                get { return _drawIgnite.CurrentValue; }
            }

            static DrawingMenu()
            {
                MenuDrawing = Config.Menu.AddSubMenu("Drawing");
                _drawQ = MenuDrawing.Add("drawQ", new CheckBox("Göster Q"));
                _drawW = MenuDrawing.Add("drawW", new CheckBox("Göster W", false));
                _drawIgnite = MenuDrawing.Add("drawIgnite", new CheckBox("Göster Tutuştur"));
                _drawOnlyReady = MenuDrawing.Add("drawOnlyReady", new CheckBox("Sadece hazır büyüler", false));
                _drawLastHitable = MenuDrawing.Add("drawLastHitable", new CheckBox("Q ile son vuruş yapılabilecekler"));
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
                MenuDebug.AddLabel("Burası hata varsa çalışır");
                _debugChat = MenuDebug.Add("debugChat", new CheckBox("Chatte hata mesajını göster", false));
                _debugConsole = MenuDebug.Add("debugConsole", new CheckBox("Konsolda hata mesajını göster", false));
            }

            public static void Initialize()
            {

            }
        }

        public static class PredictionMenu
        {
            private static readonly Menu MenuPrediction;
            private static readonly Slider _minQHCCombo;
            private static readonly Slider _minQHCHarass;
            private static readonly Slider _minQHCAutoHarass;
            private static readonly Slider _minQHCLastHit;
            private static readonly Slider _minQHCLaneClear;
            private static readonly Slider _minQHCJungleClear;
            private static readonly Slider _minQHCKillSteal;
            private static readonly Slider _minQHCFlee;

            public static HitChance MinQHCCombo
            {
                get { return Util.GetHCSliderHitChance(_minQHCCombo); }
            }

            public static HitChance MinQHCHarass
            {
                get { return Util.GetHCSliderHitChance(_minQHCHarass); }
            }

            public static HitChance MinQHCAutoHarass
            {
                get { return Util.GetHCSliderHitChance(_minQHCAutoHarass); }
            }

            public static HitChance MinQHCLastHit
            {
                get { return Util.GetHCSliderHitChance(_minQHCLastHit); }
            }

            public static HitChance MinQHCLaneClear
            {
                get { return Util.GetHCSliderHitChance(_minQHCLaneClear); }
            }

            public static HitChance MinQHCJungleClear
            {
                get { return Util.GetHCSliderHitChance(_minQHCJungleClear); }
            }

            public static HitChance MinQHCKillSteal
            {
                get { return Util.GetHCSliderHitChance(_minQHCKillSteal); }
            }

            public static HitChance MinQHCFlee
            {
                get { return Util.GetHCSliderHitChance(_minQHCFlee); }
            }

            static PredictionMenu()
            {
                MenuPrediction = Config.Menu.AddSubMenu("Q Prediction");
                MenuPrediction.AddGroupLabel("Q İsabet Oranı");
                MenuPrediction.AddLabel("En az isabet oranını buradan ayarlayabilrisin");
                MenuPrediction.AddGroupLabel("Combo");
                _minQHCCombo = Util.CreateHCSlider("comboMinQHitChance", "Combo", HitChance.High, MenuPrediction);
                MenuPrediction.AddGroupLabel("Harass");
                _minQHCHarass = Util.CreateHCSlider("harassMinQHitChance", "Harass", HitChance.High, MenuPrediction);
                MenuPrediction.AddGroupLabel("Auto Harass");
                _minQHCAutoHarass = Util.CreateHCSlider("autoHarassMinQHitChance", "Auto Harass", HitChance.Medium, MenuPrediction);
                MenuPrediction.AddGroupLabel("Lane Clear");
                _minQHCLaneClear = Util.CreateHCSlider("laneClearMinQHitChance", "Lane Clear", HitChance.Low, MenuPrediction);
                MenuPrediction.AddGroupLabel("Jungle Clear");
                _minQHCJungleClear = Util.CreateHCSlider("jungleClearMinQHitChance", "Jungle Clear", HitChance.Collision, MenuPrediction);
                MenuPrediction.AddGroupLabel("Last Hit");
                _minQHCLastHit = Util.CreateHCSlider("lastHitMinQHitChance", "Last Hit", HitChance.Medium, MenuPrediction);
                MenuPrediction.AddGroupLabel("Kill Steal");
                _minQHCKillSteal = Util.CreateHCSlider("killStealMinQHitChance", "Kill Steal", HitChance.Medium, MenuPrediction);
                MenuPrediction.AddGroupLabel("Flee");
                _minQHCFlee = Util.CreateHCSlider("fleeMinQHitChance", "Flee", HitChance.Low, MenuPrediction);
            }

            public static void Initialize()
            {

            }
        }
    }
}
