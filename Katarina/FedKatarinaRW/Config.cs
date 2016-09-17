using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using Color = System.Drawing.Color;

// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable InconsistentNaming
// ReSharper disable MemberHidesStaticFromOuterClass

namespace DamageIndicator
{
    public static class Config
    {
        private const string MenuName = "Damage Draw";

        private static readonly Menu Menu;

        static Config()
        {
            Menu = MainMenu.AddMenu(MenuName, MenuName.ToLower());
            Menu.AddGroupLabel("Hasarı Göster");
            Modes.Initialize();
        }

        public static void Initialize()
        {
        }

        public static class Modes
        {
            private static readonly Menu DamageIndicatorMenu;

            static Modes()
            {

                DamageIndicatorMenu = Menu.AddSubMenu("::Hasartespitçisi::");
                Draw.Initialize();
            }

            public static void Initialize()
            {
            }

            public static class Draw
            {
                private static readonly CheckBox _drawHealth;
                private static readonly CheckBox _drawPercent;
                private static readonly CheckBox _drawStatiscs;
                //Color Config
                private static readonly ColorConfig _healthColor;

                //CheckBoxes

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


                //Colors
                public static Color HealthColor
                {
                    get { return _healthColor.GetSystemColor(); }
                }

                static Draw()
                {
                    DamageIndicatorMenu.AddGroupLabel("Büyü gösterme ayarları :");
                    _drawHealth = DamageIndicatorMenu.Add("damageIndicatorDraw", new CheckBox("Hasartespitçisi göster ?"));
                    _drawPercent = DamageIndicatorMenu.Add("percentageIndicatorDraw", new CheckBox("Hasarı yüzde göster ?"));
                    _drawStatiscs = DamageIndicatorMenu.Add("statiscsIndicatorDraw", new CheckBox("Hasar istatistikelrini göster ?"));

                    _healthColor = new ColorConfig(DamageIndicatorMenu, "healthColorConfig", Color.Yellow,
                        "Renk Hasar Tespitçisi:");
                }

                public static void Initialize()
                {
                }
            }
        }
    }
}