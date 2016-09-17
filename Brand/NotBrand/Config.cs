using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace NotBrand
{
    static class Config
    {
        public static Menu Menu { get; set; }
        public static Menu HarassMenu { get; set; }
        public static Menu LastHitMenu { get; set; }
        public static Menu LaneClearMenu { get; set; }
        public static Menu DrawMenu { get; set; }

        static Config()
        {
            Menu = MainMenu.AddMenu("Not Brand", "NotBrand");

            Menu.AddGroupLabel("Kombo");
            Menu.Add("comboQ", new CheckBox("Kullan Q"));
            Menu.Add("comboW", new CheckBox("Kullan W"));
            Menu.Add("comboE", new CheckBox("Kullan E"));
            Menu.Add("comboR", new CheckBox("Kullan R"));
            Menu.Add("minEnemiesR", new Slider("R için en az düşman", 1, 1, 6));
            Menu.AddSeparator();
            Menu.AddGroupLabel("Kill Çalma");
            Menu.Add("ksQ", new CheckBox("Kullan Q"));
            Menu.Add("ksW", new CheckBox("Kullan W"));
            Menu.Add("ksE", new CheckBox("Kullan E"));
            Menu.Add("ksR", new CheckBox("Kullan R"));
            Menu.Add("ksIgnite", new CheckBox("Tutuştur Kullan"));

            HarassMenu = Menu.AddSubMenu("Dürtme");
            HarassMenu.AddGroupLabel("Büyüler");
            HarassMenu.Add("harassQ", new CheckBox("Kullan Q"));
            HarassMenu.Add("harassQmana", new Slider("Mininum mana % to use Q", 15, 0, 100));
            HarassMenu.AddSeparator();
            HarassMenu.Add("harassW", new CheckBox("Kullan W"));
            HarassMenu.Add("harassWmana", new Slider("Mininum mana % to use W", 15, 0, 100));
            HarassMenu.AddSeparator();
            HarassMenu.Add("harassE", new CheckBox("Kullan E"));
            HarassMenu.Add("harassEmana", new Slider("Mininum mana % to use E", 15, 0, 100));

            //LastHitMenu = Menu.AddSubMenu("Last Hit");
            //LastHitMenu.AddGroupLabel("Spells");
            //LastHitMenu.Add("lastHitQ", new CheckBox("Use Q"));
            //LastHitMenu.Add("minManaQ", new Slider("Mininum mana % to use Q", 15, 0, 100));
            //LastHitMenu.Add("lastHitE", new CheckBox("Use E"));
            //LastHitMenu.Add("minManaE", new Slider("Mininum mana % to use E", 15, 0, 100));

            LaneClearMenu = Menu.AddSubMenu("LaneTemizleme");
            LaneClearMenu.AddGroupLabel("Büyüler");
            LaneClearMenu.Add("laneClearW", new CheckBox("Kullan W"));
            LaneClearMenu.Add("minMinionsW", new Slider("W için en az minyon", 2, 1, 6));
            LaneClearMenu.Add("minManaW", new Slider("W için en az mana", 25, 0, 100));
            LaneClearMenu.AddSeparator();
            LaneClearMenu.Add("laneClearE", new CheckBox("Kullan E"));
            LaneClearMenu.Add("minMinionsE", new Slider("E için en az minyon", 2, 1, 6));
            LaneClearMenu.Add("minManaE", new Slider("E için en az mana", 25, 0, 100));

            DrawMenu = Menu.AddSubMenu("Göstergeler");
            DrawMenu.Add("drawQ", new CheckBox("Göster Q"));
            DrawMenu.Add("drawW", new CheckBox("Göster W"));
            DrawMenu.Add("drawE", new CheckBox("Göster E"));
            DrawMenu.Add("drawR", new CheckBox("Göster R"));
        }

        public static void Initialize()
        {
        }
    }
}
