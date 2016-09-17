using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;


// ReSharper disable InconsistentNaming
// ReSharper disable MemberHidesStaticFromOuterClass
namespace NinjaNunu
{
    public static class Config
    {
        private const string MenuName = "Ninja Nunu";

        public static readonly Menu Menu;

        static Config()
        {
            Menu = MainMenu.AddMenu(MenuName, MenuName.ToLower());
            Menu.AddGroupLabel("Nunu By Zpitty");
            Menu.AddLabel("Sorunları lütfen forumdan bildiriniz!");
            Menu.AddLabel("Çevirmen TRAdana");

            Modes.Initialize();
            Smite.Initialize();
            Draw.Initialize();        
        }

        public static void Initialize()
        {
        }

        public static class Modes
        {
            public static readonly Menu Menu;
            public static Spell.Targeted Smite;
            static Modes()
            {

                Menu = Config.Menu.AddSubMenu("Modes");

                Combo.Initialize();
                Menu.AddSeparator();
                Flee.Initialize();
                Menu.AddSeparator();
                Harass.Initialize();
                Menu.AddSeparator();
                JungleClear.Initialize();
                Menu.AddSeparator();
                LaneClear.Initialize();
                Menu.AddSeparator();
                LastHit.Initialize();
                Menu.AddSeparator();
                MiscMenu.Initialize();
            }

            public static void Initialize()
            {
            }

            public static class Combo
            {
                private static readonly CheckBox _useW;
                private static readonly CheckBox _useE;
                private static readonly CheckBox _useR;
                private static readonly Slider _manaW;
                private static readonly Slider _minR;

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

                public static int ManaW
                {
                    get { return _manaW.CurrentValue; }
                }

                public static int MinR
                {
                    get { return _minR.CurrentValue; }
                }

                static Combo()
                {
                    Menu.AddGroupLabel("Kombo");
                    _useW = Menu.Add("comboUseW", new CheckBox("Kullan W"));
                    _useE = Menu.Add("comboUseE", new CheckBox("Kullan E"));
                    _useR = Menu.Add("comboUseR", new CheckBox("Kullan R", false));
                    _manaW = Menu.Add("WMana", new Slider("W için gereken mana  %", 35));
                    _minR = Menu.Add("minnumberR", new Slider("R için en az düşman", 2, 0, 5));
                }

                public static void Initialize()
                {
                }
            }

            public static class Flee
            {
                private static readonly CheckBox _useW;
                private static readonly CheckBox _useE;
                public static bool UseW
                {
                    get { return _useW.CurrentValue; }
                }
                public static bool UseE
                {
                    get { return _useE.CurrentValue; }
                }

                static Flee()
                {
                    Menu.AddGroupLabel("Flee");
                    _useW = Menu.Add("fleeUseW", new CheckBox("Kullan W"));
                    _useE = Menu.Add("fleeUseE", new CheckBox("Kullan E"));
                }

                public static void Initialize()
                {
                }
            }

            public static class Harass
            {
                private static readonly CheckBox _useE;
                private static readonly Slider _minMana;
                public static bool UseE
                {
                    get { return _useE.CurrentValue; }
                }
                public static int MinMana
                {
                    get { return _minMana.CurrentValue; }
                }

                static Harass()
                {
                    Menu.AddGroupLabel("Dürtme");
                    _useE = Menu.Add("harassUseE", new CheckBox("Kullan E"));
                    _minMana = Menu.Add("harassMana", new Slider("E için gereken mana  %", 20));
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
                private static readonly Slider _minMana;

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
                public static int MinMana
                {
                    get { return _minMana.CurrentValue; }
                }

                static JungleClear()
                {
                    Menu.AddGroupLabel("OrmanTemizleme");
                    _useQ = Menu.Add("jungleUseQ", new CheckBox("Kullan Q"));
                    _useW = Menu.Add("jungleUseW", new CheckBox("Kullan W"));
                    _useE = Menu.Add("jungleUseE", new CheckBox("Kullan E"));
                    _minMana = Menu.Add("jungleMana", new Slider("W/E için gereken mana  %", 30));
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
                private static readonly Slider _minMana;
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
                public static int MinMana
                {
                    get { return _minMana.CurrentValue; }
                }
                static LaneClear()
                {
                    Menu.AddGroupLabel("LaneTemizleme");
                    _useQ = Menu.Add("laneUseQ", new CheckBox("Kullan Q"));
                    _useW = Menu.Add("laneUseW", new CheckBox("Kullan W"));
                    _useE = Menu.Add("laneUseE", new CheckBox("Kullan E"));
                    _minMana = Menu.Add("laneMana", new Slider("Q/W/E için gereken mana  %", 30));
                }
                public static void Initialize()
                {
                }
            }

            public static class LastHit
            {
                private static readonly CheckBox _useE;
                private static readonly Slider _manalasthit;

                public static bool UseE
                {
                    get { return _useE.CurrentValue; }
                }

                public static int ManaLastHit
                {
                    get { return _manalasthit.CurrentValue; }
                }

                static LastHit()
                {
                    Menu.AddGroupLabel("SonVuruş");
                    _useE = Menu.Add("lasthitUseE", new CheckBox("E Kullan"));
                    _manalasthit = Menu.Add("lasthitmana", new Slider("Son vuruş büyüsü için gereken mana  %", 20));
                }

                public static void Initialize()
                {
                }
            }

            

            public static class MiscMenu
            {
                private static readonly CheckBox _useautoQ;
                private static readonly CheckBox _gapcloseE;
                private static readonly CheckBox _igniteKS;
                private static readonly CheckBox _enablePotion;
                private static readonly Slider _autoQhealth;
                private static readonly Slider _minHPPotion;
                private static readonly Slider _minMPPotion;

                public static bool UseAutoQ
                {
                    get { return _useautoQ.CurrentValue; }
                }

                public static bool GapcloseE
                {
                    get { return _gapcloseE.CurrentValue; }
                }

                public static bool IgniteKS
                {
                    get { return _igniteKS.CurrentValue; }
                }

                public static bool EnablePotion
                {
                    get { return _enablePotion.CurrentValue; }
                }

                public static int AutoQHealth
                {
                    get { return _autoQhealth.CurrentValue; }
                }

                public static int MinHPPotion
                {
                    get { return _minHPPotion.CurrentValue; }
                }

                public static int MinMPPotion
                {
                    get { return _minMPPotion.CurrentValue; }
                }


                static MiscMenu()
                {
                    Menu.AddGroupLabel("Ek");
                    _useautoQ = Menu.Add("autouseQ", new CheckBox("Q Otomatik Kullan"));
                    _gapcloseE = Menu.Add("gapcloseE", new CheckBox("Gapcloser için  E"));
                    _igniteKS = Menu.Add("KSIgnite", new CheckBox("KS'de tutuştur kullan"));
                    _autoQhealth = Menu.Add("autoQhealth", new Slider("Otomatik Q Kullanımı için gerekli can", 35));
                    Menu.AddSeparator();
                    Menu.AddGroupLabel("Potion Manager");
                    _enablePotion = Menu.Add("Potion", new CheckBox("Use Potions"));
                    _minHPPotion = Menu.Add("minHPPotion", new Slider("Use at % Health", 60));
                    _minMPPotion = Menu.Add("minMPPotion", new Slider("Use at % Mana", 20));
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
                    _smiteToggle = SMenu.Add("EnableSmite", new KeyBind("Canavarlara çarp Aktif(tuşu)", false, KeyBind.BindTypes.PressToggle, 'M'));
                    _smiteEnemies = SMenu.Add("EnableSmiteEnemies", new KeyBind("KS'de mavi çarp tuşu", false, KeyBind.BindTypes.PressToggle, 'M'));
                    _smiteCombo = SMenu.Add("EnableSmiteCombo", new KeyBind("Komboda Kırmızı Çarp Tuşu", false, KeyBind.BindTypes.PressToggle, 'M'));
                    _redSmitePercent = SMenu.Add("SmiteRedPercent", new Slider("Kırmızı Çarp için Rakibin Canı %", 60));
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
                public static readonly CheckBox _drawW;
                public static readonly CheckBox _drawE;
                public static readonly CheckBox _drawR;
                public static readonly CheckBox _drawSmite;

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

                public static void Initialize()
                {
                }

            }
        }
    }
}

