using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

// ReSharper disable InconsistentNaming
// ReSharper disable MemberHidesStaticFromOuterClass

namespace Zac
{
    public static class Config
    {
        private const string MenuName = "Ninja Zac";

        private static readonly Menu Menu;

        static Config()
        {
            Menu = MainMenu.AddMenu(MenuName, MenuName.ToLower());
            Menu.AddGroupLabel("Zac By Zpitty");
            Menu.AddLabel("Lütfen Sorunları Forumdan Bildirin!");
            Menu.AddLabel("Çeviri TRAdana");


            Combo.Initialize();
            Jump.Initialize();
            Harass.Initialize();
            JungleClear.Initialize();
            LaneClear.Initialize();
            LastHit.Initialize();
            Misc.Initialize();
            Smite.Initialize();
            Draw.Initialize();
        }

        public static void Initialize()
        {
        }

        public static class Combo
        {
            private static readonly Menu CMenu;

            static Combo()
            {
                CMenu = Menu.AddSubMenu("Combo");


                ComboMenu.Initialize();
            }

            public static void Initialize()
            {
            }

            public static class ComboMenu
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;
                private static readonly CheckBox _useE;
                private static readonly CheckBox _useR;
                private static readonly Slider _rMin;
                private static readonly Slider _eDistanceOut;
                private static readonly Slider _eDistanceIn;
                private static readonly Slider _curDistance;


                static ComboMenu()
                {
                    CMenu.AddGroupLabel("Kombo Ayarları");
                    _useQ = CMenu.Add("comboUseQ", new CheckBox("Kullan Q"));
                    _useW = CMenu.Add("comboUseW", new CheckBox("Kullan W"));
                    _useE = CMenu.Add("comboUseE", new CheckBox("Kullan E (Düşman İmleçteyse göster)"));
                    _eDistanceIn = CMenu.Add("comboEDistanceIn",
                        new Slider("E Kullan düşman şu menzildeyse(elleme)", 100, 0, 1000));
                    _eDistanceOut = CMenu.Add("comboEDistanceOut",
                        new Slider("Elleme", 250, 0, 1000));
                    _curDistance = CMenu.Add("comboCurDistance",
                        new Slider("Elleme", 250, 100, 1000));
                    CMenu.AddSeparator();
                    _useR = CMenu.Add("comboUseR", new CheckBox("Kullan R", false));
                    _rMin = CMenu.Add("comboMinR", new Slider("R için gereken düşman sayısı", 2, 0, 5));
                }

                public static Menu MainMenu
                {
                    get { return CMenu; }
                }

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

                public static int RMin
                {
                    get { return _rMin.CurrentValue; }
                }

                public static int EDistanceOut
                {
                    get { return _eDistanceOut.CurrentValue; }
                }

                public static int EDistanceIn
                {
                    get { return _eDistanceIn.CurrentValue; }
                }

                public static int CurDistance
                {
                    get { return _curDistance.CurrentValue; }
                }

                public static void Initialize()
                {
                }
            }
        }

        public static class Jump
        {
            private static readonly Menu FMenu;


            static Jump()
            {
                FMenu = Menu.AddSubMenu("Jump");

                JumpMenu.Initialize();
            }

            public static void Initialize()
            {
            }

            public static class JumpMenu
            {
                private static readonly CheckBox _useE;
                private static readonly Slider _eDistanceIn;
                private static readonly Slider _eDistanceOut;


                static JumpMenu()
                {
                    FMenu.AddGroupLabel("Zıplama");
                    FMenu.AddGroupLabel(
                        "Bas T & İmlecin olduğu yerdeki düşmana zıpla");
                    FMenu.AddGroupLabel("Tuşu değiştirmek için  Flee  orbwalker menu");
                    _useE = FMenu.Add("jumpE", new CheckBox("Kullan E (enemy inside of cursor drawing)"));
                    _eDistanceIn = FMenu.Add("jumpEDistanceIn",
                        new Slider("Elleme", 100, 0, 1000));
                    _eDistanceOut = FMenu.Add("jumpEDistanceOut",
                        new Slider("Elleme", 100, 0, 1000));
                }


                public static bool UseE
                {
                    get { return _useE.CurrentValue; }
                }

                public static int EDistanceIn
                {
                    get { return _eDistanceIn.CurrentValue; }
                }

                public static int EDistanceOut
                {
                    get { return _eDistanceOut.CurrentValue; }
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
                HMenu = Menu.AddSubMenu("Harass");

                HarassMenu.Initialize();
            }

            public static void Initialize()
            {
            }

            public static class HarassMenu
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;


                static HarassMenu()
                {
                    HMenu.AddGroupLabel("Dürtme");
                    _useQ = HMenu.Add("harassQ", new CheckBox("Kullan Q"));
                    _useW = HMenu.Add("harassW", new CheckBox("Kullan W"));
                }

                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }

                public static bool UseW
                {
                    get { return _useW.CurrentValue; }
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
                JMenu = Menu.AddSubMenu("JungleClear");

                JungleClearMenu.Initialize();
            }

            public static void Initialize()
            {
            }

            public static class JungleClearMenu
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;


                static JungleClearMenu()
                {
                    JMenu.AddGroupLabel("OrmanTemizleme");
                    _useQ = JMenu.Add("jungleUseQ", new CheckBox("Kullan Q"));
                    _useW = JMenu.Add("jungleUseW", new CheckBox("Kullan W"));
                }


                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }

                public static bool UseW
                {
                    get { return _useW.CurrentValue; }
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
                LMenu = Menu.AddSubMenu("LaneClear");

                LaneClearMenu.Initialize();
            }

            public static void Initialize()
            {
            }

            public static class LaneClearMenu
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;
                private static readonly Slider _minQ;
                private static readonly Slider _minW;


                static LaneClearMenu()
                {
                    LMenu.AddGroupLabel("LaneTemizleme");
                    _useQ = LMenu.Add("laneUseQ", new CheckBox("Kullan Q"));
                    _useW = LMenu.Add("laneUseW", new CheckBox("Kullan W"));
                    _minQ = LMenu.Add("laneMinQ", new Slider("Q için minyon say", 3, 1, 7));
                    _minW = LMenu.Add("laneMinW", new Slider("W için minyon say", 3, 1, 7));
                }


                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }

                public static bool UseW
                {
                    get { return _useW.CurrentValue; }
                }

                public static int MinQ
                {
                    get { return _minQ.CurrentValue; }
                }

                public static int MinW
                {
                    get { return _minW.CurrentValue; }
                }

                public static void Initialize()
                {
                }
            }
        }

        public static class LastHit
        {
            private static readonly Menu LHMenu;

            static LastHit()
            {
                LHMenu = Menu.AddSubMenu("Last Hit");

                LastHitMenu.Initialize();
            }

            public static void Initialize()
            {
            }

            public static class LastHitMenu
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;


                static LastHitMenu()
                {
                    LHMenu.AddGroupLabel("SonVuruş");
                    _useQ = LHMenu.Add("lastHitQ", new CheckBox("Kullan Q"));
                    _useW = LHMenu.Add("lastHitW", new CheckBox("Kullan W"));
                }


                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }

                public static bool UseW
                {
                    get { return _useW.CurrentValue; }
                }

                public static void Initialize()
                {
                }
            }
        }

        public static class Misc
        {
            private static readonly Menu MMenu;

            static Misc()
            {
                MMenu = Menu.AddSubMenu("Misc");

                MiscMenu.Initialize();
            }

            public static void Initialize()
            {
            }

            public static class MiscMenu
            {
                private static readonly CheckBox _enablePotion;
                private static readonly CheckBox _enableKSQ;
                private static readonly CheckBox _enableKSW;
                private static readonly CheckBox _RInterrupt;
                private static readonly Slider _minHPPotion;
                private static readonly Slider _minMPPotion;


                static MiscMenu()
                {
                    MMenu.AddGroupLabel("Ek Options");
                    _RInterrupt = MMenu.Add("InterruptR", new CheckBox("İnterrupt için R"));
                    _enableKSQ = MMenu.Add("KSQ", new CheckBox("KS'de Q Kullan"));
                    _enableKSW = MMenu.Add("KSW", new CheckBox("KS'de W Kullan"));
                    MMenu.AddGroupLabel("Potion Manager");
                    _enablePotion = MMenu.Add("Potion", new CheckBox("İksirleri Kullan"));
                    _minHPPotion = MMenu.Add("minHPPotion", new Slider("Canım Şundan Az %", 60));
                    _minMPPotion = MMenu.Add("minMPPotion", new Slider("Manam Şundan Az %", 20));
                }

                public static bool EnablePotion
                {
                    get { return _enablePotion.CurrentValue; }
                }

                public static bool RInterrupt
                {
                    get { return _RInterrupt.CurrentValue; }
                }

                public static bool EnableKSQ
                {
                    get { return _enableKSQ.CurrentValue; }
                }

                public static bool EnableKSW
                {
                    get { return _enableKSW.CurrentValue; }
                }

                public static int MinHPPotion
                {
                    get { return _minHPPotion.CurrentValue; }
                }

                public static int MinMPPotion
                {
                    get { return _minMPPotion.CurrentValue; }
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
                SMenu = Menu.AddSubMenu("Smite Menu");

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

                static SmiteMenu()
                {
                    SMenu.AddGroupLabel("Çarp  Ayarları");
                    SMenu.AddSeparator();
                    _smiteToggle = SMenu.Add("EnableSmite",
                        new KeyBind("Çarp Canavarlar Üzerinde Aktif", false, KeyBind.BindTypes.PressToggle, 'M'));
                    _smiteEnemies = SMenu.Add("EnableSmiteEnemies",
                        new KeyBind("Mavi Çarp KS Tuşu", false, KeyBind.BindTypes.PressToggle, 'M'));
                    _smiteCombo = SMenu.Add("EnableSmiteCombo",
                        new KeyBind("Kırmızı Çarp Kombosu Tuşu", false, KeyBind.BindTypes.PressToggle, 'M'));
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
                DMenu = Menu.AddSubMenu("Draw Menu");

                DrawMenu.Initialize();
            }

            public static void Initialize()
            {
            }

            public static class DrawMenu
            {
                public static readonly CheckBox _drawQ;
                public static readonly CheckBox _drawW;
                public static readonly CheckBox _drawE;
                public static readonly CheckBox _drawR;
                public static readonly CheckBox _drawSmite;
                public static readonly CheckBox _drawCursor;
                public static readonly CheckBox _drawEdistance;

                static DrawMenu()
                {
                    DMenu.AddGroupLabel("Göstergeler");
                    DMenu.AddSeparator();
                    _drawQ = DMenu.Add("QDraw", new CheckBox("Göster Q"));
                    _drawW = DMenu.Add("WDraw", new CheckBox("Göster W"));
                    _drawE = DMenu.Add("EDraw", new CheckBox("Göster E"));
                    _drawR = DMenu.Add("RDraw", new CheckBox("Göster R"));
                    _drawSmite = DMenu.Add("SmiteDraw", new CheckBox("Göster Çarp"));
                    _drawCursor = DMenu.Add("CurDraw", new CheckBox("Draw Cursor Distance for E Charge"));
                    _drawEdistance = DMenu.Add("Edistance", new CheckBox("Draw Distance for E Charge"));
                }

                public static Menu MainMenu
                {
                    get { return DMenu; }
                }


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

                public static bool DrawSmite
                {
                    get { return _drawSmite.CurrentValue; }
                }

                public static bool DrawCursor
                {
                    get { return _drawCursor.CurrentValue; }
                }

                public static bool DrawEDistance
                {
                    get { return _drawEdistance.CurrentValue; }
                }

                public static void Initialize()
                {
                }
            }
        }
    }
}