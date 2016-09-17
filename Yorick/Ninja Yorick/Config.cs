using System.Collections.Generic;
using System.Linq;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

// ReSharper disable InconsistentNaming
// ReSharper disable MemberHidesStaticFromOuterClass

namespace Yorick
{
    public static class Config
    {
        private const string MenuName = "Ninja Yorick";

        private static readonly Menu Menu;

        static Config()
        {
            Menu = MainMenu.AddMenu(MenuName, MenuName.ToLower());
            Menu.AddGroupLabel("Yorick By Zpitty");
            Menu.AddLabel("Sorunları Lütfen Forumdan Bildirniz");
            Menu.AddLabel("Çeviri-TRAdana");


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
                private static readonly CheckBox _clone;
                private static readonly Slider _healthR;

                static ComboMenu()
                {
                    CMenu.AddGroupLabel("Kombo");
                    _useQ = CMenu.Add("comboUseQ", new CheckBox("Kullan Q"));
                    _useW = CMenu.Add("comboUseW", new CheckBox("Kullan W"));
                    _useE = CMenu.Add("comboUseE", new CheckBox("Kullan E"));
                    _useR = CMenu.Add("comboUseR", new CheckBox("Kullan R", false));
                    _clone = CMenu.Add("comboclone", new CheckBox("Hayeleti Hareket Ettir?"));
                    _healthR = CMenu.Add("combohealthR", new Slider("R için canım şu kadar %", 20));
                    CMenu.AddLabel("R için:");
                    if (EntityManager.Heroes.Allies.Count > 0)
                    {
                        var addedChamps = new List<string>();
                        foreach (
                            var ally in
                                EntityManager.Heroes.Allies.Where(ally => !addedChamps.Contains(ally.ChampionName)))
                        {
                            addedChamps.Add(ally.ChampionName);
                            CMenu.Add(ally.ChampionName, new CheckBox(string.Format("{0}", ally.ChampionName)));
                        }
                    }
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

                public static int HealthR
                {
                    get { return _healthR.CurrentValue; }
                }

                public static bool Clone
                {
                    get { return _clone.CurrentValue; }
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
                private static readonly CheckBox _useW;


                static FleeMenu()
                {
                    FMenu.AddGroupLabel("Flee");
                    _useW = FMenu.Add("fleeW", new CheckBox("Kullan W"));
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
                private static readonly CheckBox _useW;
                private static readonly CheckBox _useE;
                private static readonly Slider _manaW;
                private static readonly Slider _manaE;


                static HarassMenu()
                {
                    HMenu.AddGroupLabel("Dürtme");
                    _useW = HMenu.Add("harassW", new CheckBox("Kullan W"));
                    _useE = HMenu.Add("harassE", new CheckBox("Kullan E"));
                    _manaW = HMenu.Add("harassManaW", new Slider("W için gereken mana %", 40));
                    _manaE = HMenu.Add("harassManaE", new Slider("E için gereken mana %", 40));
                }

                public static bool UseW
                {
                    get { return _useW.CurrentValue; }
                }

                public static bool UseE
                {
                    get { return _useE.CurrentValue; }
                }

                public static int ManaW
                {
                    get { return _manaW.CurrentValue; }
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
                private static readonly Slider _manaQ;
                private static readonly Slider _manaE;
                private static readonly Slider _manaW;


                static JungleClearMenu()
                {
                    JMenu.AddGroupLabel("OrmanTemizleme");
                    _useQ = JMenu.Add("jungleUseQ", new CheckBox("Kullan Q"));
                    _useW = JMenu.Add("jungleUseW", new CheckBox("Kullan W"));
                    _useE = JMenu.Add("jungleUseE", new CheckBox("Kullan E"));
                    _manaQ = JMenu.Add("jungleManaQ", new Slider("Q için gereken mana %", 40));
                    _manaW = JMenu.Add("jungleManaW", new Slider("W için gereken mana %", 40));
                    _manaE = JMenu.Add("jungleManaE", new Slider("E için gereken mana %", 40));
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
                private static readonly CheckBox _useW;
                private static readonly CheckBox _useE;
                private static readonly Slider _manaQ;
                private static readonly Slider _manaW;
                private static readonly Slider _manaE;
                private static readonly Slider _minW;


                static LaneClearMenu()
                {
                    LMenu.AddGroupLabel("LaneTemizleme");
                    _useQ = LMenu.Add("laneUseQ", new CheckBox("Kullan Q"));
                    _useW = LMenu.Add("laneUseW", new CheckBox("Kullan W"));
                    _useE = LMenu.Add("laneUseE", new CheckBox("Kullan E"));
                    _minW = LMenu.Add("laneMinW", new Slider("W için minyon say", 3, 1, 4));
                    _manaQ = LMenu.Add("laneManaQ", new Slider("Q için gereken mana %", 40));
                    _manaW = LMenu.Add("laneManaW", new Slider("W için gereken mana %", 40));
                    _manaE = LMenu.Add("laneManaE", new Slider("E için gereken mana %", 40));
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

                public static int ManaQ
                {
                    get { return _manaQ.CurrentValue; }
                }

                public static int ManaW
                {
                    get { return _manaW.CurrentValue; }
                }

                public static int ManaE
                {
                    get { return _manaE.CurrentValue; }
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
                private static readonly CheckBox _useW;
                private static readonly CheckBox _useE;
                private static readonly Slider _manaW;
                private static readonly Slider _manaE;


                static LastHitMenu()
                {
                    LHMenu.AddGroupLabel("SonVuruş");
                    _useW = LHMenu.Add("lastHitW", new CheckBox("Kullan W"));
                    _useE = LHMenu.Add("lastHitE", new CheckBox("Kullan E"));
                    _manaW = LHMenu.Add("lastHitManaW", new Slider("W için gereken mana %", 40));
                    _manaE = LHMenu.Add("lastHitManaE", new Slider("E için gereken mana %", 40));
                }


                public static bool UseW
                {
                    get { return _useW.CurrentValue; }
                }

                public static bool UseE
                {
                    get { return _useE.CurrentValue; }
                }

                public static int ManaW
                {
                    get { return _manaW.CurrentValue; }
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
                private static readonly CheckBox _igniteks;
                private static readonly CheckBox _ksq;
                private static readonly CheckBox _ksw;
                private static readonly CheckBox _kse;
                private static readonly Slider _minHPPotion;
                private static readonly Slider _minMPPotion;


                static MiscMenu()
                {
                    MMenu.AddGroupLabel("Ek");
                    _igniteks = MMenu.Add("ksignite", new CheckBox("KS'de Tutuştur Kullan"));
                    _ksq = MMenu.Add("ksq", new CheckBox("KS'de Q Kullan"));
                    _ksw = MMenu.Add("ksw", new CheckBox("KS'de W Kullan"));
                    _kse = MMenu.Add("kse", new CheckBox("KS'de E Kullan"));
                    MMenu.AddGroupLabel("İksir Yardımcısı");
                    _enablePotion = MMenu.Add("Potion", new CheckBox("İksirleri Kullan"));
                    _minHPPotion = MMenu.Add("minHPPotion", new Slider("Canım Şundan Az %", 60));
                    _minMPPotion = MMenu.Add("minMPPotion", new Slider("Manam Şundan Az %", 20));
                }

                public static bool Igniteks
                {
                    get { return _igniteks.CurrentValue; }
                }

                public static bool Ksq
                {
                    get { return _ksq.CurrentValue; }
                }

                public static bool Ksw
                {
                    get { return _ksw.CurrentValue; }
                }

                public static bool Kse
                {
                    get { return _kse.CurrentValue; }
                }

                public static bool EnablePotion
                {
                    get { return _enablePotion.CurrentValue; }
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
                    _smiteToggle = SMenu.Add("EnableSmite",
                        new KeyBind("Çarp Canavarlar Üzerinde Aktif", false, KeyBind.BindTypes.PressToggle, 'M'));
                    _smiteEnemies = SMenu.Add("EnableSmiteEnemies",
                        new KeyBind("Mavi Çarp KS Tuşu", false, KeyBind.BindTypes.PressToggle, 'M'));
                    _smiteCombo = SMenu.Add("EnableSmiteCombo",
                        new KeyBind("Kırmızı Çarp Kombosu Tuşu", false, KeyBind.BindTypes.PressToggle, 'M'));
                    _redSmitePercent = SMenu.Add("SmiteRedPercent", new Slider("Kırmızı Çarp İçin düşmanın canı %", 60));
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
                public static readonly CheckBox _drawW;
                public static readonly CheckBox _drawE;
                public static readonly CheckBox _drawR;
                public static readonly CheckBox _drawSmite;

                static DrawMenu()
                {
                    DMenu.AddGroupLabel("Göstergeler");
                    DMenu.AddSeparator();
                    _drawW = DMenu.Add("WDraw", new CheckBox("Göster W"));
                    _drawE = DMenu.Add("EDraw", new CheckBox("Göster E"));
                    _drawR = DMenu.Add("RDraw", new CheckBox("Göster R"));
                    _drawSmite = DMenu.Add("SmiteDraw", new CheckBox("Göster Çarp (Eğer Aktifse)"));
                }

                public static Menu MainMenu
                {
                    get { return DMenu; }
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