using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

// ReSharper disable InconsistentNaming
// ReSharper disable MemberHidesStaticFromOuterClass

namespace Bard
{
    public static class Config
    {
        private const string MenuName = "Ninja Bard";

        private static readonly Menu Menu;

        static Config()
        {
            Menu = MainMenu.AddMenu(MenuName, MenuName.ToLower());
            Menu.AddGroupLabel("Zpitty'nin Bardı");
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
            private static readonly Menu Menu;

            static Modes()
            {
                Menu = Config.Menu.AddSubMenu("Modes");

                Combo.Initialize();
                Menu.AddSeparator();
                Harass.Initialize();
                Menu.AddSeparator();
                JungleClear.Initialize();
                Menu.AddSeparator();
                Misc.Initialize();
            }

            public static void Initialize()
            {
            }

            public static class Combo
            {
                private static readonly CheckBox _useQ;
                private static readonly Slider _qBindDistance;
                private static readonly Slider _qBindDistanceM;
                private static readonly Slider _qAccuracyPercent;

                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }

                public static int QBindDistance
                {
                    get { return _qBindDistance.CurrentValue; }
                }

                public static int QBindDistanceM
                {
                    get { return _qBindDistanceM.CurrentValue; }
                }

                public static int QAccuracyPercent
                {
                    get { return _qAccuracyPercent.CurrentValue; }
                }
                static Combo()
                {
                    Menu.AddGroupLabel("Kombo");
                    _useQ = Menu.Add("comboUseQ", new CheckBox("Q ile sabitle"));
                    _qBindDistance = Menu.Add("qBind", new Slider("Q yu duvardan(arkası gibi) tutturmak için gerekli mesafe", 325, 100, 450));
                    _qBindDistanceM = Menu.Add("qBindM", new Slider("Q kullanma mesafesi (minyonlar/şampiyonlar)", 425, 100, 450));
                    _qAccuracyPercent = Menu.Add("qAccuracy", new Slider("Q Accuracy % to Wall", 80));
                }

                public static void Initialize()
                {
                }
            }

            public static class Harass
            {
                private static readonly CheckBox _useQ;
                private static readonly Slider _manaQ;

                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }

                public static int ManaQ
                {
                    get { return _manaQ.CurrentValue; }
                }

                

                static Harass()
                {
                    Menu.AddGroupLabel("Dürtme");
                    _useQ = Menu.Add("harrasQ", new CheckBox("Q her zaman kullan"));
                    _manaQ = Menu.Add("QMana", new Slider("Q için manam şundan fazla", 40));

                }

                public static void Initialize()
                {
                }
            }

            public static class JungleClear
            {
                private static readonly CheckBox _useQ;
                private static readonly Slider _manaQ;

                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }

                public static int ManaQ
                {
                    get { return _manaQ.CurrentValue; }
                }



                static JungleClear()
                {
                    Menu.AddGroupLabel("OrmanTemizleme");
                    _useQ = Menu.Add("jungleQ", new CheckBox("Kullan Q"));
                    _manaQ = Menu.Add("jungleQMana", new Slider("Manam şundan çoksa", 40));

                }

                public static void Initialize()
                {
                }
            }

            public static class Misc
            {
                private static readonly CheckBox _enablePotion;
                private static readonly CheckBox _disableMAA;
                private static readonly CheckBox _useW;
                private static readonly CheckBox _useQKS;
                private static readonly CheckBox _RInterrupt;
                private static readonly CheckBox _qGapcloser;
                private static readonly Slider _rInterruptDelay;
                private static readonly Slider _wMana;
                private static readonly Slider _minHPPotion;
                private static readonly Slider _minMPPotion;
                private static readonly Slider _wHeal;
                private static readonly CheckBox _igniteKS;


                public static bool EnablePotion
                {
                    get { return _enablePotion.CurrentValue; }
                }

                public static bool UseW
                {
                    get { return _useW.CurrentValue; }
                }
                public static bool DisableMAA
                {
                    get { return _disableMAA.CurrentValue; }
                }
                public static bool UseQKS
                {
                    get { return _useQKS.CurrentValue; }
                }
                public static bool RInterrupt
                {
                    get { return _RInterrupt.CurrentValue; }
                }

                public static bool QGapcloser
                {
                    get { return _qGapcloser.CurrentValue; }
                }
                public static int WMana
                {
                    get { return _wMana.CurrentValue; }
                }

                public static int MinHPPotion
                {
                    get { return _minHPPotion.CurrentValue; }
                }

                public static int RInterruptDelay
                {
                    get { return _rInterruptDelay.CurrentValue; }
                }

                public static int WHeal
                {
                    get { return _wHeal.CurrentValue; }
                }

                public static int MinMPPotion
                {
                    get { return _minMPPotion.CurrentValue; }
                }
                public static bool IgniteKS
                {
                    get { return _igniteKS.CurrentValue; }
                }

                

                static Misc()
                {
                    Menu.AddGroupLabel("Ek");
                    _RInterrupt = Menu.Add("InterruptR", new CheckBox("İnterrupt için R"));
                    _qGapcloser = Menu.Add("GapcloseQ", new CheckBox("Gapcloser için Q"));
                    _rInterruptDelay = Menu.Add("InterruptRDelay", new Slider("İnterrupt R için gecikme (ms)", 250, 0, 1000));
                    Menu.AddSeparator();
                    _useQKS = Menu.Add("QKS", new CheckBox("Q Kullan"));
                    _useW = Menu.Add("WUse", new CheckBox("W dostlara otomatik kullan"));
                    _disableMAA = Menu.Add("disablemAA", new CheckBox("Minyonlara düz vurma"));
                    _wHeal = Menu.Add("healW", new Slider("W kullan eğer canım şundan azsa", 40));
                    _wMana = Menu.Add("manaW", new Slider("W Kullan manam şundan azsa", 20));
                    Menu.AddSeparator();
                    Menu.AddGroupLabel("Özelbölüm");
                    _igniteKS = Menu.Add("KSIgnite", new CheckBox("KS için Tutuştur kullan"));
                    _enablePotion = Menu.Add("Potion", new CheckBox("Can iksiri kullan"));
                    _minHPPotion = Menu.Add("minHPPotion", new Slider("Canım Şundan azsa", 60));
                    _minMPPotion = Menu.Add("minMPPotion", new Slider("Manam şundan azsa", 20));

                }

                public static void Initialize()
                {
                }
            }
        }

        public static class Smite
        {
            private static readonly Menu SMenu;
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
                private static readonly KeyBind _smiteEnemies;
                private static readonly KeyBind _smiteCombo;
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
                    SMenu.AddGroupLabel("Çarp Ayarları");
                    SMenu.AddSeparator();
                    _smiteToggle = SMenu.Add("EnableSmite", new KeyBind("Canavarlara Çarp kullanma tuşu", false, KeyBind.BindTypes.PressToggle, 'M'));
                    _smiteEnemies = SMenu.Add("EnableSmiteEnemies", new KeyBind("Maviye Çarp Kullan", false, KeyBind.BindTypes.PressToggle, 'M'));
                    _smiteCombo = SMenu.Add("EnableSmiteCombo", new KeyBind("Kırmızıya Çarp Kombo (Tuş)", false, KeyBind.BindTypes.PressToggle, 'M'));
                    _redSmitePercent = SMenu.Add("SmiteRedPercent", new Slider("Kırmızının Canı Şu kadarken at", 60));
                    SMenu.AddSeparator();
                    SMenu.AddGroupLabel("Çarp Atılacak Canavarlar");
                    SMenu.Add("SRU_Baron", new CheckBox("Baron"));
                    SMenu.Add("SRU_Dragon", new CheckBox("Ejder"));
                    SMenu.Add("SRU_Red", new CheckBox("Kırmız"));
                    SMenu.Add("SRU_Blue", new CheckBox("Mavi"));
                    SMenu.Add("SRU_Gromp", new CheckBox("Kurbağa"));
                    SMenu.Add("SRU_Murkwolf", new CheckBox("AlacaKurt"));
                    SMenu.Add("SRU_Krug", new CheckBox("Golem"));
                    SMenu.Add("SRU_Razorbeak", new CheckBox("SivriGagalar"));
                    SMenu.Add("Sru_Crab", new CheckBox("Yampiri Yengeç"));
                    SMenu.Add("SRU_RiftHerald", new CheckBox("Baronun Kız Kardeşi :D", false));
                }

                public static void Initialize()
                {
                }

            }
        }

        public static class Draw
        {
            private static readonly Menu DMenu;
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
                private static readonly CheckBox _drawQ;
                private static readonly CheckBox _drawW;
                private static readonly CheckBox _drawR;
                private static readonly CheckBox _drawSmite;

                public static bool DrawQ
                {
                    get { return _drawQ.CurrentValue; }
                }

                public static bool DrawW
                {
                    get { return _drawW.CurrentValue; }
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
