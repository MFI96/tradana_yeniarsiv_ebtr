using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using KA_Rumble.DMGHandler;
using Color = System.Drawing.Color;

// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable InconsistentNaming
// ReSharper disable MemberHidesStaticFromOuterClass

namespace KA_Rumble
{
    public static class Config
    {
        private static readonly string MenuName = "KA " + Player.Instance.ChampionName;

        private static readonly Menu Menu;

        static Config()
        {
            Menu = MainMenu.AddMenu(MenuName, MenuName.ToLower());
            Menu.AddGroupLabel("KA " + Player.Instance.ChampionName);
            Modes.Initialize();
        }

        public static void Initialize()
        {
        }

        public static class Modes
        {
            private static readonly Menu SpellsMenu, FarmMenu, MiscMenu, DrawMenu;
            public static readonly Menu SettingsMenu;

            static Modes()
            {
                SpellsMenu = Menu.AddSubMenu("::Büyü Menüsü::");
                Combo.Initialize();
                Harass.Initialize();

                FarmMenu = Menu.AddSubMenu("::FarmMenu::");
                LaneClear.Initialize();
                LastHit.Initialize();

                MiscMenu = Menu.AddSubMenu("::Ek::");
                Misc.Initialize();

                DrawMenu = Menu.AddSubMenu("::Göstergeler::");
                Draw.Initialize();

                SettingsMenu = Menu.AddSubMenu("::Ayarlar::");
                Settings.Initialize();
            }

            public static void Initialize()
            {
            }

            public static class Combo
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useE;
                private static readonly CheckBox _useR;

                private static readonly Slider _minR;
                private static readonly CheckBox _useRKillable;

                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }
                public static bool UseE
                {
                    get { return _useE.CurrentValue; }
                }
                public static bool UseR
                {
                    get { return _useR.CurrentValue; }
                }

                public static int MinR
                {
                    get { return _minR.CurrentValue; }
                }
                public static bool UseRKillable
                {
                    get { return _useRKillable.CurrentValue; }
                }

                static Combo()
                {
                    // Initialize the menu values
                    SpellsMenu.AddGroupLabel("Kombo Büyüleri:");
                    _useQ = SpellsMenu.Add("comboQ", new CheckBox("Kullan Q ?"));
                    _useE = SpellsMenu.Add("comboE", new CheckBox("Kullan E  ?"));
                    _useR = SpellsMenu.Add("comboR", new CheckBox("Kullan R  ?"));
                    SpellsMenu.AddLabel("R Ayarları:");
                    _minR = SpellsMenu.Add("minR", new Slider("R için gereken düşman  ?",2,0,5));
                    _useRKillable  = SpellsMenu.Add("comboRKill", new CheckBox("Kill alacaksam sayıyı önemsemeden R kullan ?",false));
                }

                public static void Initialize()
                {
                }
            }

            public static class Harass
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useE;

                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }

                public static bool UseE
                {
                    get { return _useE.CurrentValue; }
                }

                static Harass()
                {
                    SpellsMenu.AddGroupLabel("Dürtme Büyüleri:");
                    _useQ = SpellsMenu.Add("harassQ", new CheckBox("Kullan Q ?"));
                    _useE = SpellsMenu.Add("harassE", new CheckBox("Kullan E  ?"));
                }

                public static void Initialize()
                {
                }
            }

            public static class LaneClear
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useE;

                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }

                public static bool UseE
                {
                    get { return _useE.CurrentValue; }
                }

                static LaneClear()
                {
                    FarmMenu.AddGroupLabel("Lanetemizleme Büyüleri:");
                    _useQ = FarmMenu.Add("laneclearQ", new CheckBox("Q Kullan ?"));
                    _useE = FarmMenu.Add("laneclearE", new CheckBox("E Kullan ?"));
                }

                public static void Initialize()
                {
                }
            }

            public static class LastHit
            {
                private static readonly CheckBox _useE;

                public static bool UseE
                {
                    get { return _useE.CurrentValue; }
                }

                static LastHit()
                {
                    FarmMenu.AddGroupLabel("Sonvuruş Büyüleri:");
                    _useE = FarmMenu.Add("lasthitE", new CheckBox("E Kullan ?"));
                }

                public static void Initialize()
                {
                }
            }

            public static class Misc
            {
                private static readonly CheckBox _antiGapCloserSpell;

                public static bool AntiGapCloser
                {
                    get { return _antiGapCloserSpell.CurrentValue; }
                }
                static Misc()
                {
                    // Initialize the menu values
                    MiscMenu.AddGroupLabel("Ek");
                    _antiGapCloserSpell = MiscMenu.Add("gapcloserE", new CheckBox("Antigapcloser için E Kullan ?"));
                }

                public static void Initialize()
                {
                }
            }

            public static class Draw
            {
                private static readonly CheckBox _drawReady;
                private static readonly CheckBox _drawHealth;
                private static readonly CheckBox _drawPercent;
                private static readonly CheckBox _drawStatiscs;
                private static readonly CheckBox _drawQ;
                private static readonly CheckBox _drawW;
                private static readonly CheckBox _drawE;
                private static readonly CheckBox _drawR;
                //Color Config
                private static readonly ColorConfig _qColor;
                private static readonly ColorConfig _wColor;
                private static readonly ColorConfig _eColor;
                private static readonly ColorConfig _rColor;
                private static readonly ColorConfig _healthColor;

                //CheckBoxes
                public static bool DrawReady
                {
                    get { return _drawReady.CurrentValue; }
                }

                public static bool DrawHealth
                {
                    get { return _drawHealth.CurrentValue; }
                }

                public static bool DrawPercent
                {
                    get { return _drawPercent.CurrentValue; }
                }

                public static bool DrawStatistics
                {
                    get { return _drawStatiscs.CurrentValue; }
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
                //Colors
                public static Color HealthColor
                {
                    get { return _healthColor.GetSystemColor(); }
                }

                public static SharpDX.Color QColor
                {
                    get { return _qColor.GetSharpColor(); }
                }

                public static SharpDX.Color WColor
                {
                    get { return _wColor.GetSharpColor(); }
                }

                public static SharpDX.Color EColor
                {
                    get { return _eColor.GetSharpColor(); }
                }
                public static SharpDX.Color RColor
                {
                    get { return _rColor.GetSharpColor(); }
                }

                static Draw()
                {
                    DrawMenu.AddGroupLabel("Gösterge :");
                    _drawReady = DrawMenu.Add("drawOnlyWhenReady", new CheckBox("Büyü Hazırsa Göster ?"));
                    _drawHealth = DrawMenu.Add("damageIndicatorDraw", new CheckBox("Göster Hasar Tespiti ?"));
                    _drawPercent = DrawMenu.Add("percentageIndicatorDraw", new CheckBox("Hasarı Yüzde Olarak Göster ?"));
                    _drawStatiscs = DrawMenu.Add("statiscsIndicatorDraw", new CheckBox("Hasar İstatistiklerini Göster ?"));
                    DrawMenu.AddSeparator(1);
                    _drawQ = DrawMenu.Add("qDraw", new CheckBox("Göster Q  Menzili ?"));
                    _drawW = DrawMenu.Add("wDraw", new CheckBox("Göster W  Menzili ?"));
                    _drawE = DrawMenu.Add("eDraw", new CheckBox("Göster E  Menzili ?"));
                    _drawR = DrawMenu.Add("rDraw", new CheckBox("Göster R  Menzili ?"));

                    _healthColor = new ColorConfig(DrawMenu, "healthColorConfig", Color.Orange, "Renkli Hasar Tespiti:");
                    _qColor = new ColorConfig(DrawMenu, "qColorConfig", Color.Blue, "Renk Q:");
                    _wColor = new ColorConfig(DrawMenu, "wColorConfig", Color.Red, "Renk W:");
                    _eColor = new ColorConfig(DrawMenu, "eColorConfig", Color.DeepPink, "Renk E:");
                    _rColor = new ColorConfig(DrawMenu, "rColorConfig", Color.Yellow, "Renk R:");
                }

                public static void Initialize()
                {
                }
            }

            public static class Settings
            {
                //Danger
                private static readonly Slider EnemyRange;
                private static readonly Slider EnemySlider;
                private static readonly CheckBox Spells;
                private static readonly CheckBox Skillshots;
                private static readonly CheckBox AAs;


                public static int RangeEnemy
                {
                    get { return EnemyRange.CurrentValue; }
                }

                public static int EnemyCount
                {
                    get { return EnemySlider.CurrentValue; }
                }

                public static bool ConsiderSpells
                {
                    get { return Spells.CurrentValue; }
                }

                public static bool ConsiderSkillshots
                {
                    get { return Skillshots.CurrentValue; }
                }

                public static bool ConsiderAttacks
                {
                    get { return AAs.CurrentValue; }
                }

                static Settings()
                {
                    SettingsMenu.AddGroupLabel("Danger W Settings");
                    EnemySlider = SettingsMenu.Add("minenemiesinrange", new Slider("Min enemies in the range determined below", 1, 1, 5));
                    EnemyRange = SettingsMenu.Add("minrangeenemy", new Slider("Enemies must be in ({0}) range to be in danger", 1000, 600, 2500));
                    Spells = SettingsMenu.Add("considerspells", new CheckBox("Consider spells ?"));
                    Skillshots = SettingsMenu.Add("considerskilshots", new CheckBox("Consider SkillShots ?"));
                    AAs = SettingsMenu.Add("consideraas", new CheckBox("Consider Auto Attacks ?"));
                    SettingsMenu.AddSeparator();
                    SettingsMenu.AddGroupLabel("Dangerous Spells");
                    foreach (var spell in DangerousSpells.Spells.Where(x => EntityManager.Heroes.Enemies.Any(b => b.Hero == x.Hero)))
                    {
                        SettingsMenu.Add(spell.Hero.ToString() + spell.Slot, new CheckBox(spell.Hero + " - " + spell.Slot + ".", spell.DefaultValue));
                    }
                }

                public static void Initialize()
                {
                }
            }
        }
    }
}