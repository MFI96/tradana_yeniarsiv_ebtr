using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

// ReSharper disable InconsistentNaming
// ReSharper disable MemberHidesStaticFromOuterClass

namespace Maokai
{
    public static class Config
    {
        private const string MenuName = "Ninja Maokai";

        private static readonly Menu Menu;

        static Config()
        {
            Menu = MainMenu.AddMenu(MenuName, MenuName.ToLower());
            Menu.AddGroupLabel("Maokai By Zpitty");
            Menu.AddLabel("Sorunları lütfen forumdan bildiriniz!");
            Menu.AddLabel("Çevirmen TRAdana");


            Combo.Initialize();
            Flee.Initialize();
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
                private static readonly CheckBox _turnoffR;
                private static readonly Slider _MinR;
                private static readonly Slider _manaR;
                private static readonly Slider _manaE;

                static ComboMenu()
                {
                    CMenu.AddGroupLabel("Kombo");
                    _useQ = CMenu.Add("comboUseQ", new CheckBox("Kullan Q"));
                    _useW = CMenu.Add("comboUseW", new CheckBox("Kullan W"));
                    _useE = CMenu.Add("comboUseE", new CheckBox("Kullan E"));
                    _useR = CMenu.Add("comboUseR", new CheckBox("Kullan R", false));
                    _turnoffR = CMenu.Add("comboturnoffR", new CheckBox("Eğer R menzilinde düşman yoksa R kapat?"));
                    _MinR = CMenu.Add("comboMinR", new Slider("R için en az düşman", 2, 0, 5));
                    _manaR = CMenu.Add("comboManaR", new Slider("R için mana %", 15));
                    _manaE = CMenu.Add("comboManaE", new Slider("E için mana %", 30));
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

                public static bool TurnoffR
                {
                    get { return _turnoffR.CurrentValue; }
                }

                public static int MinR
                {
                    get { return _MinR.CurrentValue; }
                }

                public static int ManaR
                {
                    get { return _manaR.CurrentValue; }
                }

                public static int ManaE
                {
                    get { return _manaE.CurrentValue; }
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
                FMenu = Menu.AddSubMenu("Flee");

                FleeMenu.Initialize();
            }

            public static void Initialize()
            {
            }

            public static class FleeMenu
            {
                private static readonly CheckBox _useQ;


                static FleeMenu()
                {
                    FMenu.AddGroupLabel("Flee(kaçma)Ayarları");
                    _useQ = FMenu.Add("fleeQ", new CheckBox("Kullan Q"));
                }

                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
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
                private static readonly CheckBox _useE;
                private static readonly Slider _manaQ;
                private static readonly Slider _manaE;


                static HarassMenu()
                {
                    HMenu.AddGroupLabel("Dürtme Ayarları");
                    _useQ = HMenu.Add("harassQ", new CheckBox("Kullan Q"));
                    _useE = HMenu.Add("harassE", new CheckBox("Kullan E"));
                    _manaQ = HMenu.Add("harassManaQ", new Slider("Q için mana %", 40));
                    _manaE = HMenu.Add("harassManaE", new Slider("E için mana %", 40));
                }

                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }

                public static bool UseE
                {
                    get { return _useE.CurrentValue; }
                }

                public static int ManaQ
                {
                    get { return _manaQ.CurrentValue; }
                }

                public static int ManaE
                {
                    get { return _manaE.CurrentValue; }
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
                private static readonly CheckBox _useE;
                private static readonly Slider _minE;
                private static readonly Slider _manaQ;
                private static readonly Slider _manaE;
                private static readonly Slider _manaW;


                static JungleClearMenu()
                {
                    JMenu.AddGroupLabel("OrmanTemizleme Ayarları");
                    _useQ = JMenu.Add("jungleUseQ", new CheckBox("Kullan Q"));
                    _useW = JMenu.Add("jungleUseW", new CheckBox("Kullan W"));
                    _useE = JMenu.Add("jungleUseE", new CheckBox("Kullan E"));
                    _minE = JMenu.Add("jungleMinW", new Slider("E için minyon say", 2, 1, 4));
                    _manaQ = JMenu.Add("jungleManaQ", new Slider("Q için mana %", 40));
                    _manaW = JMenu.Add("jungleManaW", new Slider("W için mana %", 40));
                    _manaE = JMenu.Add("jungleManaE", new Slider("E için mana %", 40));
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


                public static int MinE
                {
                    get { return _minE.CurrentValue; }
                }

                public static int ManaQ
                {
                    get { return _manaQ.CurrentValue; }
                }

                public static int ManaE
                {
                    get { return _manaE.CurrentValue; }
                }

                public static int ManaW
                {
                    get { return _manaW.CurrentValue; }
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
                private static readonly CheckBox _useE;
                private static readonly Slider _minQ;
                private static readonly Slider _minE;
                private static readonly Slider _manaQ;
                private static readonly Slider _manaE;


                static LaneClearMenu()
                {
                    LMenu.AddGroupLabel("LaneTemizleme Ayarları");
                    _useQ = LMenu.Add("laneUseQ", new CheckBox("Kullan Q"));
                    _useE = LMenu.Add("laneUseE", new CheckBox("Kullan E"));
                    _minQ = LMenu.Add("laneMinQ", new Slider("Q için minyon say", 3, 1, 7));
                    _minE = LMenu.Add("laneMinE", new Slider("E için minyon say", 3, 1, 7));
                    _manaQ = LMenu.Add("laneManaQ", new Slider("Q için mana %", 40));
                    _manaE = LMenu.Add("laneManaE", new Slider("E için mana %", 40));
                }


                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }

                public static bool UseE
                {
                    get { return _useE.CurrentValue; }
                }

                public static int MinQ
                {
                    get { return _minQ.CurrentValue; }
                }

                public static int MinE
                {
                    get { return _minE.CurrentValue; }
                }

                public static int ManaQ
                {
                    get { return _manaQ.CurrentValue; }
                }

                public static int ManaE
                {
                    get { return _manaE.CurrentValue; }
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
                private static readonly Slider _manaQ;


                static LastHitMenu()
                {
                    LHMenu.AddGroupLabel("SonVuruş Ayarları");
                    _useQ = LHMenu.Add("lastHitQ", new CheckBox("Kullan Q"));
                    _manaQ = LHMenu.Add("lastHitManaQ", new Slider("Q için mana %", 40));
                }


                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }

                public static int ManaQ
                {
                    get { return _manaQ.CurrentValue; }
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
                private static readonly CheckBox _rKS;
                private static readonly CheckBox _qKS;
                private static readonly CheckBox _qInterrupt;
                private static readonly CheckBox _qGapclose;
                private static readonly CheckBox _turnoffR;
                private static readonly Slider _minHPPotion;
                private static readonly Slider _minMPPotion;


                static MiscMenu()
                {
                    MMenu.AddGroupLabel("Ek Ayarlar");
                    _rKS = MMenu.Add("ksR", new CheckBox("KSde R Kullan"));
                    _qKS = MMenu.Add("ksQ", new CheckBox("KSde Q Kullan"));
                    _qInterrupt = MMenu.Add("InterruptQ", new CheckBox("İnterrupt için Q"));
                    _qGapclose = MMenu.Add("GapcloseQ", new CheckBox("Gapcloser için Q"));
                    _turnoffR = MMenu.Add("miscturnoffR", new CheckBox("R menzilinde hedef yoksa kapat?"));
                    MMenu.AddGroupLabel("Potion Manager");
                    _enablePotion = MMenu.Add("Potion", new CheckBox("İksirleri Kullan"));
                    _minHPPotion = MMenu.Add("minHPPotion", new Slider("Canım %", 60));
                    _minMPPotion = MMenu.Add("minMPPotion", new Slider("Manam %", 20));
                }

                public static bool EnablePotion
                {
                    get { return _enablePotion.CurrentValue; }
                }

                public static bool RKS
                {
                    get { return _rKS.CurrentValue; }
                }

                public static bool QKS
                {
                    get { return _qKS.CurrentValue; }
                }

                public static bool QInterrupt
                {
                    get { return _qInterrupt.CurrentValue; }
                }

                public static bool QGapclose
                {
                    get { return _qGapclose.CurrentValue; }
                }

                public static bool TurnoffR
                {
                    get { return _turnoffR.CurrentValue; }
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
                public static readonly CheckBox _blueSlow;

                static SmiteMenu()
                {
                    SMenu.AddGroupLabel("Çarp  Ayarları");
                    _smiteToggle = SMenu.Add("EnableSmite",
                        new KeyBind("Canavarlara Çarp Tuşu", false, KeyBind.BindTypes.PressToggle, 'M'));
                    _smiteEnemies = SMenu.Add("EnableSmiteEnemies",
                        new KeyBind("Maviye Çarp Atma tuşu", false, KeyBind.BindTypes.PressToggle, 'M'));
                    _smiteCombo = SMenu.Add("EnableSmiteCombo",
                        new KeyBind("Kırmızı Çarp Kombosu", false, KeyBind.BindTypes.PressToggle, 'M'));
                    _blueSlow = SMenu.Add("BlueSlow", new CheckBox("Maviyi Çarpla Yavaşlat"));
                    SMenu.AddGroupLabel("Düşman menzilden çıkıyorsa canı azsa çarp+düzvuruş kullan");
                    _redSmitePercent = SMenu.Add("SmiteRedPercent", new Slider("Kırmızı Çarp düşmanın canı %", 60));
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

                public static bool BlueSlow
                {
                    get { return _blueSlow.CurrentValue; }
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

                static DrawMenu()
                {
                    DMenu.AddGroupLabel("Gösterge");
                    DMenu.AddSeparator();
                    _drawQ = DMenu.Add("QDraw", new CheckBox("Göster Q"));
                    _drawW = DMenu.Add("WDraw", new CheckBox("Göster W"));
                    _drawE = DMenu.Add("EDraw", new CheckBox("Göster E"));
                    _drawR = DMenu.Add("RDraw", new CheckBox("Göster R"));
                    _drawSmite = DMenu.Add("SmiteDraw", new CheckBox("Göster Çarp"));
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

                public static void Initialize()
                {
                }
            }
        }
    }
}