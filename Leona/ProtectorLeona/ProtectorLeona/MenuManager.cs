using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace ProtectorLeona
{
    internal class MenuManager
    {
        // Create Main Segments
        public static Menu ProtectorLeonaMenu, ComboMenu, HarassMenu, DrawingMenu, SettingMenu;

        public static void Initialize()
        {
            // Addon Menu
            ProtectorLeonaMenu = MainMenu.AddMenu("ProtectorLeona", "ProtectorLeona");
            ProtectorLeonaMenu.AddGroupLabel("Protector Leona");
            ProtectorLeonaMenu.AddLabel("Çevirmen TRAdana");

            // Combo Menu
            ComboMenu = ProtectorLeonaMenu.AddSubMenu("Kombo", "ComboFeatures");
            ComboMenu.AddGroupLabel("Kombo Ayarları");
            ComboMenu.AddLabel("Büyüler");
            ComboMenu.Add("Qcombo", new CheckBox("Kullan Q"));
            ComboMenu.Add("Wcombo", new CheckBox("Kullan W"));
            ComboMenu.Add("Ecombo", new CheckBox("Kullan E"));
            ComboMenu.Add("Rcombo", new CheckBox("Kullan R"));
            ComboMenu.AddSeparator(1);
            ComboMenu.Add("Rlimit", new Slider("Düşmana R kullanmak için gerekli sayı", 2, 1, 5));

            // Harass Menu
            HarassMenu = ProtectorLeonaMenu.AddSubMenu("Dürtme", "HarassFeatures");
            HarassMenu.AddGroupLabel("Dürtme Ayarları");
            HarassMenu.AddLabel("Büyüler");
            HarassMenu.Add("Qharass", new CheckBox("Kullan Q"));
            HarassMenu.Add("Eharass", new CheckBox("Kullan E"));
            HarassMenu.AddSeparator(1);
            HarassMenu.Add("Mharass", new Slider("Dürtme için en az mana %", 25));

            // Drawing Menu
            DrawingMenu = ProtectorLeonaMenu.AddSubMenu("Gösterge", "DrawingFeatures");
            DrawingMenu.AddGroupLabel("Gösterge Ayarları");
            DrawingMenu.Add("Udraw", new CheckBox("Gösterme Modu"));
            DrawingMenu.AddSeparator(1);
            DrawingMenu.AddLabel("Büyüler");
            DrawingMenu.Add("Qdraw", new CheckBox("Göster Q", false));
            DrawingMenu.Add("Wdraw", new CheckBox("Göster W", false));
            DrawingMenu.Add("Edraw", new CheckBox("Göster E"));
            DrawingMenu.Add("Rdraw", new CheckBox("Göster R"));
            DrawingMenu.AddSeparator(1);
            DrawingMenu.AddLabel("Skin Değiştirici");
            DrawingMenu.Add("Udesign", new CheckBox("Skin Değiştirici", false));
            DrawingMenu.Add("Sdesign", new Slider("Skin Numarası: ", 4, 0, 8));

            // Setting Menu
            SettingMenu = ProtectorLeonaMenu.AddSubMenu("Ayarlar", "Settings");
            SettingMenu.AddGroupLabel("Ayarlar");
            SettingMenu.AddLabel("Otomatik Level Yükseltme");
            SettingMenu.Add("Ulevel", new CheckBox("Otomatik Level Yükseltme"));
            SettingMenu.AddSeparator(1);
            SettingMenu.AddLabel("Otomatik Atak Modu");
            SettingMenu.Add("Aattack", new CheckBox("AA Kullan"));
            SettingMenu.AddSeparator(1);
            SettingMenu.AddLabel("Interrupter");
            SettingMenu.Add("Uinterrupt", new CheckBox("Interrupt Mode"));
            SettingMenu.Add("Qinterrupt", new CheckBox("İnterrupt için Q"));
            SettingMenu.Add("Rinterrupt", new CheckBox("İnterrupt için R"));
            SettingMenu.AddLabel("Gap Closer");
            SettingMenu.Add("Ugapc", new CheckBox("Gap Closer Mode"));
            SettingMenu.Add("Qgapc", new CheckBox("Gapcloser için  Q"));
        }

        // Assign Global Checks
        public static bool ComboUseQ { get { return ComboMenu["Qcombo"].Cast<CheckBox>().CurrentValue; } }
        public static bool ComboUseW { get { return ComboMenu["Wcombo"].Cast<CheckBox>().CurrentValue; } }
        public static bool ComboUseE { get { return ComboMenu["Ecombo"].Cast<CheckBox>().CurrentValue; } }
        public static bool ComboUseR { get { return ComboMenu["Rcombo"].Cast<CheckBox>().CurrentValue; } }
        public static int ComboRLimiter { get { return ComboMenu["Rlimit"].Cast<Slider>().CurrentValue; } }

        public static bool HarassUseQ { get { return HarassMenu["Qharass"].Cast<CheckBox>().CurrentValue; } }
        public static bool HarassUseE { get { return HarassMenu["Eharass"].Cast<CheckBox>().CurrentValue; } }
        public static int HarassMana { get { return HarassMenu["Mharass"].Cast<Slider>().CurrentValue; } }

        public static bool DrawerMode { get { return DrawingMenu["Udraw"].Cast<CheckBox>().CurrentValue; } }
        public static bool DrawQ { get { return DrawingMenu["Qdraw"].Cast<CheckBox>().CurrentValue; } }
        public static bool DrawW { get { return DrawingMenu["Wdraw"].Cast<CheckBox>().CurrentValue; } }
        public static bool DrawE { get { return DrawingMenu["Edraw"].Cast<CheckBox>().CurrentValue; } }
        public static bool DrawR { get { return DrawingMenu["Rdraw"].Cast<CheckBox>().CurrentValue; } }
        public static bool DesignerMode { get { return DrawingMenu["Udesign"].Cast<CheckBox>().CurrentValue; } }
        public static int DesignerSkin { get { return DrawingMenu["Sdesign"].Cast<Slider>().CurrentValue; } }

        public static bool LevelerMode { get { return SettingMenu["Ulevel"].Cast<CheckBox>().CurrentValue; } }
        public static bool AutoAttack { get { return SettingMenu["Aattack"].Cast<CheckBox>().CurrentValue; } }

        public static bool InterrupterMode { get { return SettingMenu["Uinterrupt"].Cast<CheckBox>().CurrentValue; } }
        public static bool InterrupterUseQ { get { return SettingMenu["Qinterrupt"].Cast<CheckBox>().CurrentValue; } }
        public static bool InterrupterUseR { get { return SettingMenu["Rinterrupt"].Cast<CheckBox>().CurrentValue; } }
        public static bool GapCloserMode { get { return SettingMenu["Ugapc"].Cast<CheckBox>().CurrentValue; } }
        public static bool GapCloserUseQ { get { return SettingMenu["Qgapc"].Cast<CheckBox>().CurrentValue; } }
    }
}
