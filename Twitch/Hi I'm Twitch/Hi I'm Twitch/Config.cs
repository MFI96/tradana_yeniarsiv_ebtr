using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

// ReSharper disable InconsistentNaming
// ReSharper disable MemberHidesStaticFromOuterClass
namespace AddonTemplate
{
    public static class Config
    {
        private const string MenuName = "Hi I'm Twitch";
        public static Item Youmuu = new Item(ItemId.Youmuus_Ghostblade);
        public static Item Botrk = new Item(ItemId.Blade_of_the_Ruined_King);
        public static Item Cutlass = new Item(ItemId.Bilgewater_Cutlass);

        private static readonly Menu Menu;

        static Config()
        {
            Menu = MainMenu.AddMenu(MenuName, MenuName.ToLower());
            Menu.AddGroupLabel("Welcome to Hi I'm Twitch addon!");
            Menu.AddLabel("Made by GinjiBan");

            Modes.Initialize();
        }

        public static void Initialize()
        {
        }

        public static class Modes
        {
            private static readonly Menu MenuCombo;
            private static readonly Menu MenuHarass;
            private static readonly Menu MenuDraw;
            private static readonly Menu MenuKillSteal;
            private static readonly Menu MenuClear;
            private static readonly Menu MenuMisc;

            static Modes()
            {
                MenuCombo = Config.Menu.AddSubMenu("Combo");
                MenuHarass = Config.Menu.AddSubMenu("Harass");
                MenuDraw = Config.Menu.AddSubMenu("Visual");
                MenuKillSteal = Config.Menu.AddSubMenu("Contaminate usage");
                MenuClear = Config.Menu.AddSubMenu("Clear");
                MenuMisc = Config.Menu.AddSubMenu("Misc");

                Combo.Initialize();
                Harass.Initialize();
                Draw.Initialize();
                KillSteal.Initialize();
                Clear.Initialize();
                Misc.Initialize();
            }

            public static void Initialize()
            {
            }

            public static class Combo
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;
                private static readonly CheckBox _useWUlt;
                private static readonly CheckBox _useE;
                private static readonly CheckBox _useR;
                private static readonly Slider _numberR;
                private static readonly Slider _minHPBotrk;
                private static readonly Slider _enemyMinHPBotrk;
                private static readonly CheckBox _useYoumuu;
                private static readonly CheckBox _useBotrk;


                public static bool UseYoumuu
                {
                    get { return _useYoumuu.CurrentValue; }
                }
                public static bool useBotrk
                {
                    get { return _useBotrk.CurrentValue; }
                }
                public static int MinHPBotrk
                {
                    get { return _minHPBotrk.CurrentValue; }
                }
                public static int EnemyMinHPBotrk
                {
                    get { return _enemyMinHPBotrk.CurrentValue; }
                }
                public static int NumberR
                {
                    get { return _numberR.CurrentValue; }
                }


                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }
                public static bool UseW
                {
                    get { return _useW.CurrentValue; }
                }
                public static bool UseWUlt
                {
                    get { return _useWUlt.CurrentValue; }
                }
                public static bool UseE
                {
                    get { return _useE.CurrentValue; }
                }
                public static bool UseR
                {
                    get { return _useR.CurrentValue; }
                }

                static Combo()
                {
                    MenuCombo.AddGroupLabel("Combo");
                    _useQ = MenuCombo.Add("comboUseQ", new CheckBox("Kullan Q"));
                    _useW = MenuCombo.Add("comboUseW", new CheckBox("Kullan W"));
                    _useWUlt = MenuCombo.Add("comboUseWUlt", new CheckBox("Use W during ult", false));
                    _useE = MenuCombo.Add("comboUseE", new CheckBox("Ölecek biri varsa E"));
                    _useR = MenuCombo.Add("comboUseR", new CheckBox("Menzilden şu kadar düşman varsa R", false));
                    _numberR = MenuCombo.Add("numberR", new Slider("R için düşman sayısı ({0})", 3, 1, 5));
                    MenuCombo.AddSeparator();
                    MenuCombo.AddGroupLabel("Item usage");
                    _useYoumuu = MenuCombo.Add("useYoumuu", new CheckBox("Yuumo kullan"));
                    _useBotrk = MenuCombo.Add("useBotrk", new CheckBox("Mahvolmuş kılıç"));
                    _minHPBotrk = MenuCombo.Add("minHPBotrk", new Slider("Mahvolmuş için benim canım ({0}%)", 80));
                    _enemyMinHPBotrk = MenuCombo.Add("enemyMinHPBotrk", new Slider("Mhavolmuş için düşmanın canı ({0}% ({0}%)", 80));
                }

                public static void Initialize()
                {
                }
            }

            public static class Harass
            {
                private static readonly CheckBox _useW;
                private static readonly CheckBox _useE;
                private static readonly Slider _manaW;
                private static readonly Slider _manaE;
                private static readonly Slider _harassStacks;

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
                public static int HarassStacks
                {
                    get { return _harassStacks.CurrentValue; }
                }

                static Harass()
                {
                    MenuHarass.AddGroupLabel("Harass");
                    _useW = MenuHarass.Add("harassUseW", new CheckBox("Kullan W"));
                    _useE = MenuHarass.Add("harassUseE", new CheckBox("Kullan E"));
                    MenuHarass.AddSeparator();
                    MenuHarass.AddGroupLabel("Mana Yardımcısı");
                    _manaW = MenuHarass.Add("harassManaW", new Slider("W en az mana ({0}%)", 40));
                    _manaE = MenuHarass.Add("harassManaE", new Slider("E en az mana ({0}%)", 40));
                    MenuHarass.AddSeparator();
                    MenuHarass.AddGroupLabel("Dürtme Yükü");
                    _harassStacks = MenuHarass.Add("harassStacks", new Slider("E için kaç yük biriksin ({0})", 6, 1, 6));
                }

                public static void Initialize()
                {
                }
            }
            public static class Draw
            {
                private static readonly CheckBox _dmgIndicator;
                private static readonly CheckBox _sleathDistance;
                private static readonly CheckBox _miniMapSleathDistance;
                private static readonly CheckBox _drawW;
                private static readonly CheckBox _drawE;
                private static readonly CheckBox _onlyRdy;
                public static readonly CheckBox _useHax;
                public static readonly Slider _skinhax;
                public static string[] skinName = { "Classic Twitch", "Kingpin Twitch", "Whistler Village Twitch", "Medieval Twitch", "Gangster Twitch", "Vandal Twitch", "Pickpocket Twitch", "SSW Twitch"};

                public static bool DrawW
                {
                    get { return _drawW.CurrentValue; }
                }

                public static bool DrawE
                {
                    get { return _drawE.CurrentValue; }
                }

                public static bool OnlyRdy
                {
                    get { return _onlyRdy.CurrentValue; }
                }
                public static bool DamageIndicator
                {
                    get { return _dmgIndicator.CurrentValue; }
                }
                public static bool StealthDistance
                {
                    get { return _sleathDistance.CurrentValue; }
                }
                public static bool MinimapStealthDistance
                {
                    get { return _miniMapSleathDistance.CurrentValue; }
                }
                public static bool UseHax
                {
                    get { return _useHax.CurrentValue; }
                }

                public static int SkinHax
                {
                    get { return _skinhax.CurrentValue; }
                }

                static Draw()
                {
                    MenuDraw.AddGroupLabel("Büyülerin Menzili");
                    _drawW = MenuDraw.Add("drawW", new CheckBox("Göster W"));
                    _drawE = MenuDraw.Add("drawE", new CheckBox("Göster E"));
                    _onlyRdy = MenuDraw.Add("onlyRdy", new CheckBox("Bekleme süresindekileri gösterme"));
                    MenuDraw.AddSeparator();
                    MenuDraw.AddLabel("HP Bar");
                    _dmgIndicator = MenuDraw.Add("damageIndicator", new CheckBox("Hasar Tespiti"));
                    MenuDraw.AddSeparator();
                    MenuDraw.AddLabel("Stealth");
                    _sleathDistance = MenuDraw.Add("stealthdistance", new CheckBox("Gizlilik mesafesi"));
                    _miniMapSleathDistance = MenuDraw.Add("minimapstealthdistance", new CheckBox("Haritada gizlilik mesafesi"));
                    MenuDraw.AddSeparator();
                    MenuDraw.AddGroupLabel("Skin Hilesi");
                    _useHax = MenuDraw.Add("UseHax", new CheckBox("Aktif", false));
                    _skinhax = MenuDraw.Add("skinhax", new Slider("Skin hack", 0, skinName.Length - 1, 0));
                }

                public static void Initialize()
                {
                }
            }

            public static class KillSteal
            {
                private static readonly CheckBox _eChamp;
                private static readonly CheckBox _eFullStacks;
                private static readonly Slider _eNumberStacks;
                private static readonly CheckBox _eOutOfRange;
                private static readonly Slider _eOutOfRangeStacks;
                private static readonly CheckBox _eDying;


                public static bool KsE
                {
                    get { return _eChamp.CurrentValue; }
                }
                public static bool EFullStacks
                {
                    get { return _eFullStacks.CurrentValue; }
                }
                public static bool EOutOfRange
                {
                    get { return _eOutOfRange.CurrentValue; }
                }
                public static bool EDying
                {
                    get { return _eDying.CurrentValue; }
                }
                public static int ENumberstacks
                {
                    get { return _eNumberStacks.CurrentValue; }
                }

                public static int EOutOfRangeStacks
                {
                    get { return _eOutOfRangeStacks.CurrentValue; }
                }


                static KillSteal()
                {
                    MenuKillSteal.AddGroupLabel("E kullanım");
                    _eChamp = MenuKillSteal.Add("echamp", new CheckBox("Kill çalmak için E Kullan"));
                    MenuKillSteal.AddSeparator();
                    _eFullStacks = MenuKillSteal.Add("efullstacks", new CheckBox("Düşmanda x kadar yük varsa E", false));
                    _eNumberStacks = MenuKillSteal.Add("enumberstacks", new Slider("E için kaç yük ({0})", 6, 1, 6));
                    MenuKillSteal.AddSeparator();
                    _eOutOfRange = MenuKillSteal.Add("eoutofrange", new CheckBox("Menzil dışındaki hedefe E", false));
                    _eOutOfRangeStacks = MenuKillSteal.Add("eoutofrangestacks", new Slider("Menzil dışındaki hedefe E için gereken yük ({0})", 6, 1, 6));
                    MenuKillSteal.AddSeparator();
                    _eDying = MenuKillSteal.Add("edying", new CheckBox("Eğer E ile ölecekse belirt"));
                }

                public static void Initialize()
                {
                }
            }

            public static class Clear
            {
                private static readonly CheckBox _eBigMinion;
                private static readonly CheckBox _eBaronDragon;
                private static readonly CheckBox _eBigJungle;
                private static readonly CheckBox _wLaneClear;
                private static readonly Slider _wNumber;
                private static readonly CheckBox _eLaneClear;
                private static readonly Slider _eNumber;
                private static readonly Slider _laneClearMana;

                public static bool EBigMinion
                {
                    get { return _eBigMinion.CurrentValue; }
                }

                public static bool EBaronDragon
                {
                    get { return _eBaronDragon.CurrentValue; }
                }
                public static bool eBigJungle
                {
                    get { return _eBigJungle.CurrentValue; }
                }
                public static bool WLaneClear
                {
                    get { return _wLaneClear.CurrentValue; }
                }
                public static int WNumber
                {
                    get { return _wNumber.CurrentValue; }
                }
                public static bool ELaneClear
                {
                    get { return _eLaneClear.CurrentValue; }
                }
                public static int ENumber
                {
                    get { return _eNumber.CurrentValue; }
                }
                public static int LaneClearMana
                {
                    get { return _laneClearMana.CurrentValue; }
                }
                static Clear()
                {
                    MenuClear.AddGroupLabel("Otomatik Temizleme");
                    _eBaronDragon = MenuClear.Add("ebarondragon", new CheckBox("baronu ejderi her zaman E ile koru"));
                    _eBigMinion = MenuClear.Add("ebigminion", new CheckBox("Büyük minyonu E ile koru"));
                    MenuClear.AddSeparator();
                    MenuClear.AddGroupLabel("Jungle clear");
                    _eBigJungle = MenuClear.Add("ebigjungle", new CheckBox("Kamplarda E kullan"));
                    MenuClear.AddSeparator();
                    MenuClear.AddGroupLabel("Lane clear");
                    _wLaneClear = MenuClear.Add("wlaneclear", new CheckBox("W Kullan"));
                    _wNumber = MenuClear.Add("wnumber", new Slider("W için gereken minyon ({0})", 4, 1, 15));
                    MenuClear.AddSeparator();
                    _eLaneClear = MenuClear.Add("elaneclear", new CheckBox("E Kullan"));
                    _eNumber = MenuClear.Add("enumber", new Slider("ŞU kadar minyon ölecekse ({0})", 4, 1, 15));
                    MenuClear.AddSeparator();
                    _laneClearMana = MenuClear.Add("laneclearmana", new Slider("Lanetemizleme için en az mana ({0}%)", 40, 1, 100));
                }

                public static void Initialize()
                {
                }
            }

            public static class Misc
            {
                public static readonly KeyBind _stealthRecall;
                public static readonly CheckBox _useQ;

                public static bool StealthRecall
                {
                    get { return _stealthRecall.CurrentValue; }
                }
                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }


                static Misc()
                {
                    MenuMisc.AddGroupLabel("Misc");
                    _stealthRecall = MenuMisc.Add("stealthrecall", new KeyBind("Görünmez B", true, KeyBind.BindTypes.PressToggle, 'B'));
                    _useQ = MenuMisc.Add("qflee", new CheckBox("Kaçarken Q"));
                }

                public static void Initialize()
                {
                }
            }
        }
    }
}
