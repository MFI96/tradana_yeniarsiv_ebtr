using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;


namespace RekSai
{

    public static class Config
    {
        private const string MenuName = "RekSai";

        private static readonly Menu Menu;

        static Config()
        {

            Menu = MainMenu.AddMenu(MenuName, MenuName.ToLower());
            Menu.AddGroupLabel("RekSai By Zpitty");
            Menu.AddLabel("Sorunları Forumdan Bildiriniz");
            Menu.AddLabel("Çeviri-TRAdana");


            Combo.Initialize();
            Flee.Initialize();
            Harass.Initialize();
            JungleClear.Initialize();
            LaneClear.Initialize();
            //LastHit.Initialize();
            Misc.Initialize();
            Smite.Initialize();
            Draw.Initialize();
            
        }

        public static void Initialize()
        {
        }

        public static class Combo
        {
            private static readonly Menu CMBMenu;

            static Combo()
            {
                CMBMenu = Config.Menu.AddSubMenu("Combo");

                ComboMenu.Initialize();
            }

            public static void Initialize()
            {
            }

            public static class ComboMenu
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useQ2;
                private static readonly CheckBox _useW;
                private static readonly CheckBox _useE;
                private static readonly CheckBox _useE2;
                private static readonly CheckBox _itemUsage;
                private static readonly Slider _e2Distance;

                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }

                public static bool UseQ2
                {
                    get { return _useQ2.CurrentValue; }
                }
                public static bool UseW
                {
                    get { return _useW.CurrentValue; }
                }
                public static bool UseE
                {
                    get { return _useE.CurrentValue; }
                }
                public static bool UseE2
                {
                    get { return _useE2.CurrentValue; }
                }

                public static bool ItemUsage
                {
                    get { return _itemUsage.CurrentValue; }
                }

                public static int E2Distance
                {
                    get { return _e2Distance.CurrentValue; }
                }

                static ComboMenu()
                {

                    CMBMenu.AddGroupLabel("Kombo");
                    _useQ = CMBMenu.Add("comboUseQ", new CheckBox("Q Kullan (YerÜstünde)"));
                    _useQ2 = CMBMenu.Add("comboUseQ2", new CheckBox("Q2 Kullan (Yeraltında)"));
                    _useW = CMBMenu.Add("comboUseW", new CheckBox("Kullan W"));
                    _useE = CMBMenu.Add("comboUseE", new CheckBox("Kullan E (YerÜstünde)"));
                    _useE2 = CMBMenu.Add("comboUseE2", new CheckBox("Kullan E (Yeraltında)"));
                    _itemUsage = CMBMenu.Add("comboUseItems", new CheckBox("İtemleri Kullan"));
                    _e2Distance = CMBMenu.Add("comboE2Distance", new Slider("Kullan E (Yeraltında) Menzilde 0 düşman varken:", 550, 100, 750));
                }

                public static void Initialize()
                {
                }
            }
        }

        public static class Flee
        {
            private static readonly Menu FMenu;

            static Flee()
            {
                FMenu = Config.Menu.AddSubMenu("Flee");

                FleeMenu.Initialize();
            }

            public static void Initialize()
            {
            }
            public static class FleeMenu
            {
                private static readonly CheckBox _useE2;

                public static bool UseE2
                {
                    get { return _useE2.CurrentValue; }
                }


                static FleeMenu()
                {
                    FMenu.AddGroupLabel("Flee Ayarları");
                    _useE2 = FMenu.Add("flee", new CheckBox("Kullan Flee"));
                }

                public static void Initialize()
                {
                }
            }
        }

        public static class Harass
        {
            private static readonly Menu HMenu;

            static Harass()
            {
                HMenu = Config.Menu.AddSubMenu("Harass");

                HarassMenu.Initialize();
            }

            public static void Initialize()
            {
            }
            public static class HarassMenu
            {
                private static readonly CheckBox _useQ2;

                public static bool UseQ2
                {
                    get { return _useQ2.CurrentValue; }
                }


                static HarassMenu()
                {
                    HMenu.AddGroupLabel("Dürtme Ayarları");
                    _useQ2 = HMenu.Add("harassQ", new CheckBox("Kullan Q (Yeraltında)"));
                }

                public static void Initialize()
                {
                }
            }
        }

        public static class JungleClear
        {
            private static readonly Menu JMenu;

            static JungleClear()
            {
                JMenu = Config.Menu.AddSubMenu("JungleClear");

                JungleClearMenu.Initialize();
            }

            public static void Initialize()
            {
            }

            public static class JungleClearMenu
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useQ2;
                private static readonly CheckBox _useW;
                private static readonly CheckBox _useE;


                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }

                public static bool UseQ2
                {
                    get { return _useQ2.CurrentValue; }
                }
                public static bool UseW
                {
                    get { return _useW.CurrentValue; }
                }
                public static bool UseE
                {
                    get { return _useE.CurrentValue; }
                }


                static JungleClearMenu()
                {

                    JMenu.AddGroupLabel("OrmanTemizleme");
                    _useQ = JMenu.Add("jungleUseQ", new CheckBox("Kullan Q (Yerüstünde)"));
                    _useQ2 = JMenu.Add("jungleUseQ2", new CheckBox("Kullan Q (Yeraltında)"));
                    _useW = JMenu.Add("jungleUseW", new CheckBox("Kullan W"));
                    _useE = JMenu.Add("jungleUseE", new CheckBox("Kullan E (Yerüstünde)"));


                }

                public static void Initialize()
                {
                }
            }
        }

        public static class LaneClear
        {
            private static readonly Menu LMenu;

            static LaneClear()
            {
                LMenu = Config.Menu.AddSubMenu("LaneClear");

                LaneClearMenu.Initialize();
            }

            public static void Initialize()
            {
            }

            public static class LaneClearMenu
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useQ2;
                private static readonly CheckBox _useW;
                private static readonly CheckBox _useE;


                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }

                public static bool UseQ2
                {
                    get { return _useQ2.CurrentValue; }
                }
                public static bool UseW
                {
                    get { return _useW.CurrentValue; }
                }
                public static bool UseE
                {
                    get { return _useE.CurrentValue; }
                }


                static LaneClearMenu()
                {

                    LMenu.AddGroupLabel("LaneTemizleme");
                    _useQ = LMenu.Add("laneUseQ", new CheckBox("Kullan Q (Yerüstünde)"));
                    _useQ2 = LMenu.Add("laneUseQ2", new CheckBox("Kullan Q (Yeraltında)"));
                    _useW = LMenu.Add("laneUseW", new CheckBox("Kullan W"));
                    _useE = LMenu.Add("laneUseE", new CheckBox("Kullan E (Yerüstünde)"));


                }

                public static void Initialize()
                {
                }
            }
        }

        //public static class LastHit
        //{
        //    private static readonly Menu LHMenu;

        //    static LastHit()
        //    {
        //        LHMenu = Config.Menu.AddSubMenu("Last Hit");

        //        LastHitMenu.Initialize();
        //    }

        //    public static void Initialize()
        //    {
        //    }
        //    public static class LastHitMenu
        //    {
        //        private static readonly CheckBox _useQ2;
        //        private static readonly CheckBox _useW;


        //        public static bool UseQ2
        //        {
        //            get { return _useQ2.CurrentValue; }
        //        }

        //        public static bool UseW
        //        {
        //            get { return _useW.CurrentValue; }
        //        }


        //        static LastHitMenu()
        //        {
        //            LHMenu.AddGroupLabel("LastHit Options");
        //            _useQ2 = LHMenu.Add("lastHitQ2", new CheckBox("Use Q2 (Burrowed)"));
        //            _useW = LHMenu.Add("lastHitW", new CheckBox("Use W"));
        //        }

        //        public static void Initialize()
        //        {
        //        }
        //    }
        //}

        public static class Misc
        {
            private static readonly Menu MMenu;

            static Misc()
            {
                MMenu = Config.Menu.AddSubMenu("Misc");

                MiscMenu.Initialize();
            }

            public static void Initialize()
            {
            }
            public static class MiscMenu
            {
                private static readonly CheckBox _enablePotion;
                private static readonly CheckBox _enableKS;
                private static readonly Slider _minHPPotion;
                private static readonly Slider _minMPPotion;
                private static readonly CheckBox _itemUsage;

                public static bool EnablePotion
                {
                    get { return _enablePotion.CurrentValue; }
                }
                public static bool EnableKS
                {
                    get { return _enableKS.CurrentValue; }
                }

                public static int MinHPPotion
                {
                    get { return _minHPPotion.CurrentValue; }
                }

                public static int MinMPPotion
                {
                    get { return _minMPPotion.CurrentValue; }
                }

                public static bool ItemUsage
                {
                    get { return _itemUsage.CurrentValue; }
                }


                static MiscMenu()
                {
                    MMenu.AddGroupLabel("Ek Ayarlar");
                    _enableKS = MMenu.Add("KSEnabled", new CheckBox("Kullan KS"));
                    _itemUsage = MMenu.Add("miscUseItems", new CheckBox("İtemleri Kullan OrmanTemizleme/LaneTemizleme"));
                    MMenu.AddGroupLabel("Potion Manager");
                    _enablePotion = MMenu.Add("Potion", new CheckBox("İksirleri Kullan"));
                    _minHPPotion = MMenu.Add("minHPPotion", new Slider("Canım Şundan Az %", 60));
                    _minMPPotion = MMenu.Add("minMPPotion", new Slider("Manam Şundan Az %", 20));
                }

                public static void Initialize()
                {
                }
            }
        }


        public static class Smite
        {
            public static readonly Menu SMenu;
            static Smite()
            {

                SMenu = Config.Menu.AddSubMenu("Smite Menu");

                SmiteMenu.Initialize();
            }
            public static void Initialize()
            {
            }

            public static class SmiteMenu
            {
                public static readonly KeyBind _smiteEnemies;
                public static readonly KeyBind _smiteCombo;
                private static readonly KeyBind _smiteToggle;
                private static readonly Slider _redSmitePercent;

                public static Menu MainMenu
                {
                    get { return SMenu; }
                }


                public static bool SmiteToggle
                {
                    get { return _smiteToggle.CurrentValue; }
                }

                public static bool SmiteEnemies
                {
                    get { return _smiteEnemies.CurrentValue; }
                }

                public static bool SmiteCombo
                {
                    get { return _smiteCombo.CurrentValue; }
                }

                public static int RedSmitePercent
                {
                    get { return _redSmitePercent.CurrentValue; }
                }

                static SmiteMenu()
                {
                    SMenu.AddGroupLabel("Çarp  Ayarları");
                    SMenu.AddSeparator();
                    _smiteToggle = SMenu.Add("EnableSmite", new KeyBind("Enable Smite Monsters (Toggle)", false, KeyBind.BindTypes.PressToggle, 'M'));
                    _smiteEnemies = SMenu.Add("EnableSmiteEnemies", new KeyBind("Blue Smite KS (Toggle)", false, KeyBind.BindTypes.PressToggle, 'M'));
                    _smiteCombo = SMenu.Add("EnableSmiteCombo", new KeyBind("Red Smite Combo (Toggle)", false, KeyBind.BindTypes.PressToggle, 'M'));
                    _redSmitePercent = SMenu.Add("SmiteRedPercent", new Slider("Red Smite Enemy % HP", 60));
                    SMenu.AddSeparator();
                    SMenu.AddGroupLabel("Çarp Kullanılabilecek Canavarlar");
                    SMenu.Add("SRU_Baron", new CheckBox("Baron"));
                    SMenu.Add("SRU_Dragon", new CheckBox("Ejder"));
                    SMenu.Add("SRU_Red", new CheckBox("Kırmızı"));
                    SMenu.Add("SRU_Blue", new CheckBox("Mavi"));
                    SMenu.Add("SRU_Gromp", new CheckBox("Kurbağa"));
                    SMenu.Add("SRU_Murkwolf", new CheckBox("AlacaKurt"));
                    SMenu.Add("SRU_Krug", new CheckBox("Golem"));
                    SMenu.Add("SRU_Razorbeak", new CheckBox("SivriGagalar"));
                    SMenu.Add("Sru_Crab", new CheckBox("Yampiri Yengeç"));
                    SMenu.Add("SRU_RiftHerald", new CheckBox("Baronun Kız Kardeşi", false));
                }

                public static void Initialize()
                {
                }

            }
        }

        public static class Draw
        {
            public static readonly Menu DMenu;
            static Draw()
            {

                DMenu = Config.Menu.AddSubMenu("Draw Menu");

                DrawMenu.Initialize();
            }
            public static void Initialize()
            {
            }

            public static class DrawMenu
            {
                public static readonly CheckBox _drawQ;
                public static readonly CheckBox _drawQ2;
                public static readonly CheckBox _drawE;
                public static readonly CheckBox _drawE2;
                public static readonly CheckBox _drawSmite;

                public static Menu MainMenu
                {
                    get { return DMenu; }
                }


                public static bool DrawQ
                {
                    get { return _drawQ.CurrentValue; }
                }

                public static bool DrawQ2
                {
                    get { return _drawQ2.CurrentValue; }
                }

                public static bool DrawE
                {
                    get { return _drawE.CurrentValue; }
                }


                public static bool DrawE2
                {
                    get { return _drawE2.CurrentValue; }
                }

                public static bool DrawSmite
                {
                    get { return _drawSmite.CurrentValue; }
                }

                static DrawMenu()
                {
                    DMenu.AddGroupLabel("Gösterge");
                    DMenu.AddSeparator();
                    _drawQ = DMenu.Add("QDraw", new CheckBox("Göster Q (Yerüstünde)"));
                    _drawQ2 = DMenu.Add("Q2Draw", new CheckBox("Göster Q (Yeraltında)"));
                    _drawE = DMenu.Add("EDraw", new CheckBox("Göster E (Yerüstünde)"));
                    _drawE2 = DMenu.Add("E2Draw", new CheckBox("Göster E (Yeraltında)"));
                    _drawSmite = DMenu.Add("SmiteDraw", new CheckBox("Göster Çarp"));
                }

                public static void Initialize()
                {
                }
            }
        }
    }
}