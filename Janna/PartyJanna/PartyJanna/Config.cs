using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System.Collections.Generic;

namespace PartyJanna
{
    public static class Config
    {
        private const string MenuName = "PartyJanna";

        private static readonly Menu Menu;

        static Config()
        {
            Menu = MainMenu.AddMenu(MenuName, MenuName.ToLower());
            Menu.AddGroupLabel("PartyJanna Menüsüne hoşgeldin!");
            Menu.AddLabel("Çeviri TRAdana");

            Settings.Initialize();
        }

        public static void Initialize() { }

        public static class Settings
        {
            private static readonly Menu Menu;

            static Settings()
            {
                Menu = Config.Menu.AddSubMenu("Settings");

                Draw.Initialize();
                Menu.AddSeparator(13);

                AntiGapcloser.Initialize();
                Menu.AddSeparator(13);

                Interrupter.Initialize();
                Menu.AddSeparator(13);

                Items.Initialize();
                Menu.AddSeparator(13);

                AutoShield.Initialize();
                Menu.AddSeparator(13);

                Combo.Initialize();
                Menu.AddSeparator(13);

                Flee.Initialize();
                Menu.AddSeparator(13);

                Harass.Initialize();
                Menu.AddSeparator(13);
            }

            public static void Initialize()
            {
            }

            public static class Draw
            {
                private static readonly CheckBox _drawQ;
                private static readonly CheckBox _drawW;
                private static readonly CheckBox _drawE;
                private static readonly CheckBox _drawR;

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

                static Draw()
                {
                    Menu.AddGroupLabel("Göster");

                    _drawQ = Menu.Add("drawQ", new CheckBox("Göster Q Menzili"));
                    _drawW = Menu.Add("drawW", new CheckBox("Göster W Menzili"));
                    _drawE = Menu.Add("drawE", new CheckBox("Göster E Menzili"));
                    _drawR = Menu.Add("drawR", new CheckBox("Göster R Menzili"));
                }

                public static void Initialize() { }
            }

            public static class Items
            {
                private static readonly CheckBox _useItems;

                public static bool UseItems
                {
                    get { return _useItems.CurrentValue; }
                }

                static Items()
                {
                    Menu.AddGroupLabel("İtemler");

                    _useItems = Menu.Add("useItems", new CheckBox("İtemleri Kullan"));
                }

                public static void Initialize() { }
            }

            public static class AntiGapcloser
            {
                private static readonly CheckBox _antiGapcloser;

                public static bool AntiGap
                {
                    get { return _antiGapcloser.CurrentValue; }
                }

                static AntiGapcloser()
                {
                    Menu.AddGroupLabel("Anti-Gapcloser");

                    _antiGapcloser = Menu.Add("antiGapcloser", new CheckBox("Anti-Gapcloser"));
                }

                public static void Initialize() { }
            }

            public static class Interrupter
            {
                private static readonly CheckBox _qInterrupt;
                private static readonly CheckBox _qInterruptDangerous;
                private static readonly CheckBox _rInterruptDangerous;

                public static bool QInterrupt
                {
                    get { return _qInterrupt.CurrentValue; }
                }
                public static bool QInterruptDangerous
                {
                    get { return _qInterruptDangerous.CurrentValue; }
                }
                public static bool RInterruptDangerous
                {
                    get { return _rInterruptDangerous.CurrentValue; }
                }

                static Interrupter()
                {
                    Menu.AddGroupLabel("Interrupter");

                    _qInterrupt = Menu.Add("qInterrupt", new CheckBox("Interrupt için Q orta ve yüksek seviye tehlikeli büyülerde"));
                    Menu.AddSeparator(13);

                    _qInterruptDangerous = Menu.Add("rInterrupt", new CheckBox("Interrupt için Q yüksek seviye tehlikeli büyülerde"));
                    Menu.AddSeparator(13);

                    _rInterruptDangerous = Menu.Add("rInterruptDangerous", new CheckBox("Interrupt R için yüksek tehlike"));
                }

                public static void Initialize() { }
            }

            public static class AutoShield
            {
                private static readonly CheckBox _boostAD;
                private static readonly ComboBox _priorMode;
                private static readonly List<Slider> _sliders;
                private static readonly List<AIHeroClient> _heros;

                public static bool BoostAD
                {
                    get { return _boostAD.CurrentValue; }
                }
                public static int PriorMode
                {
                    get { return _priorMode.SelectedIndex; }
                }
                public static List<Slider> Sliders
                {
                    get { return _sliders; }
                }
                public static List<AIHeroClient> Heros
                {
                    get { return _heros; }
                }

                static AutoShield()
                {
                    Menu.AddGroupLabel("OtomatikKalkan");

                    _boostAD = Menu.Add("autoShieldBoostAd", new CheckBox("Ad dostlara E Kullan"));
                    Menu.AddSeparator(13);

                    _priorMode = Menu.Add("autoShieldPriorMode", new ComboBox("OtoKalkan Öncelik Modu:", 0, new string[] { "Lowest Health", "Priority Level" }));
                    Menu.AddSeparator(13);

                    _sliders = new List<Slider>();
                    _heros = new List<AIHeroClient>();

                    foreach (var ally in EntityManager.Heroes.Allies)
                    {
                        if (ally.ChampionName != Program.ChampName)
                        {
                            Slider PrioritySlider = Menu.Add<Slider>(ally.ChampionName, new Slider(string.Format("{0} ({1})", ally.ChampionName, ally.Name), 1, 1, EntityManager.Heroes.Allies.Count - 1));

                            Menu.AddSeparator(13);

                            _sliders.Add(PrioritySlider);

                            _heros.Add(ally);
                        }
                        else
                        {
                            _sliders.Add(new Slider("Janna"));
                            _heros.Add(ally);
                        }
                    }
                }

                public static void Initialize() { }
            }

            public static class Combo
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;
                private static readonly Slider _qUseRange;

                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }
                public static bool UseW
                {
                    get { return _useW.CurrentValue; }
                }
                public static int QUseRange
                {
                    get { return _qUseRange.CurrentValue; }
                }

                static Combo()
                {
                    Menu.AddGroupLabel("Kombo");

                    _useQ = Menu.Add("comboUseQ", new CheckBox("Kullan Q"));
                    _useW = Menu.Add("comboUseW", new CheckBox("Kullan W"));
                    Menu.AddSeparator();

                    _qUseRange = Menu.Add<Slider>("qUseRangeCombo", new Slider("Q için menzil:", 1100, 1100, 1700));
                }

                public static void Initialize() { }
            }

            public static class Flee
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;
                //private static readonly CheckBox _useR;
                private static readonly Slider _qUseRange;

                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }
                public static bool UseW
                {
                    get { return _useW.CurrentValue; }
                }
                /*public static bool UseR
                {
                    get { return _useR.CurrentValue; }
                }*/
                public static int QUseRange
                {
                    get { return _qUseRange.CurrentValue; }
                }

                static Flee()
                {
                    Menu.AddGroupLabel("Flee(Kaçma)");

                    _useQ = Menu.Add("fleeUseQ", new CheckBox("Kullan Q"));
                    _useW = Menu.Add("fleeUseW", new CheckBox("Kullan W"));
                    //_useR = Menu.Add("comboUseR", new CheckBox("Use R", false));
                    Menu.AddSeparator();

                    _qUseRange = Menu.Add<Slider>("qUseRangeFlee", new Slider("Q kullanma menzili:", 1100, 1100, 1700));
                }

                public static void Initialize() { }
            }

            public static class Harass
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;
                private static readonly CheckBox _autoHarass;
                private static readonly Slider _autoHarassManaPercent;
                private static readonly Slider _qUseRange;

                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }
                public static bool UseW
                {
                    get { return _useW.CurrentValue; }
                }
                public static bool AutoHarass
                {
                    get { return _autoHarass.CurrentValue; }
                }
                public static int AutoHarassManaPercent
                {
                    get { return _autoHarassManaPercent.CurrentValue; }
                }
                public static int QUseRange
                {
                    get { return _qUseRange.CurrentValue; }
                }

                static Harass()
                {
                    Menu.AddGroupLabel("Dürtme");

                    _useQ = Menu.Add("harassUseQ", new CheckBox("Kullan Q"));
                    Menu.AddSeparator(13);

                    _qUseRange = Menu.Add<Slider>("qUseRangeHarass", new Slider("Q için menzil:", 1100, 1100, 1700));
                    Menu.AddSeparator();

                    _useW = Menu.Add("harassUseW", new CheckBox("Kullan W"));
                    Menu.AddSeparator();

                    _autoHarass = Menu.Add("autoHarass", new CheckBox("W otomatik dürtmesi için gereken mana %"));
                    Menu.AddSeparator(13);

                    _autoHarassManaPercent = Menu.Add<Slider>("autoHarassManaPercent", new Slider("Otomatik dürtme için gereken mana %:", 75));
                }

                public static void Initialize() { }
            }
        }
    }
}
