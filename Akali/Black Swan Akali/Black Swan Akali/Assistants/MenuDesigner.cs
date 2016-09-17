using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace Black_Swan_Akali
{
    public static class MenuDesigner
    {
        public const string MenuName = "Black Swan Akali";

        public static readonly Menu AkaliUI, ComboUI, HarassUI, ClearUI, KsUI, MiscUI;

        static MenuDesigner()
        {
            // Black Swan Akali :: Main Menu
            AkaliUI = MainMenu.AddMenu(MenuName, MenuName.ToLower());
            AkaliUI.AddGroupLabel("As balance dictates.");
            AkaliUI.AddSeparator();
            AkaliUI.AddLabel("Geliştirici    :   Enelx");
            AkaliUI.AddLabel("Versiyon          :   1.1.0.0");
            AkaliUI.AddLabel("Çeviri TRAdana");

            // Black Swan Akali :: Combo Menu
            ComboUI = AkaliUI.AddSubMenu("Kombo");
            ComboUI.AddGroupLabel("Kombo :: Büyüleri");
            ComboUI.Add("ComboQ", new CheckBox("Kullan Q"));
            ComboUI.Add("ComboW", new CheckBox("Kullan W"));
            ComboUI.Add("ComboE", new CheckBox("Kullan E"));
            ComboUI.Add("ComboR", new CheckBox("Kullan R"));

            // Black Swan Akali :: Harass Menu
            HarassUI = AkaliUI.AddSubMenu("Harass");
            HarassUI.AddGroupLabel("Dürtme :: Büyüleri");
            HarassUI.Add("HarassQ", new CheckBox("Kullan Q"));

            // Black Swan Akali :: Clear Menu
            ClearUI = AkaliUI.AddSubMenu("Clear");
            ClearUI.AddGroupLabel("SonVuruş :: Büyüleri");
            ClearUI.Add("LastQ", new CheckBox("Kullan Q"));
            ClearUI.Add("LastE", new CheckBox("Kullan E", false));
            ClearUI.AddSeparator();
            ClearUI.AddGroupLabel("LaneTemizleme :: Büyüleri / Ayarları");
            ClearUI.Add("ClearQ", new CheckBox("Kullan Q"));
            ClearUI.Add("ClearE", new CheckBox("Kullan E"));
            ClearUI.Add("ClearEmin", new Slider("E için en az minyon", 2, 1, 6));
            ClearUI.Add("ClearMana", new Slider("en az enerji  %", 25, 0, 100));
            ClearUI.AddSeparator();
            ClearUI.AddGroupLabel("Jungle Clear :: Spells / Settings");
            ClearUI.Add("JungleQ", new CheckBox("Kullan Q"));
            ClearUI.Add("JungleE", new CheckBox("Kullan E"));
            ClearUI.Add("JungleMana", new Slider("en az enerji. %", 10, 0, 100));

            // Black Swan Akali :: Killsteal Menu
            KsUI = AkaliUI.AddSubMenu("Kill Çalma");
            KsUI.AddGroupLabel("Kill Çalma :: Büyüleri");
            KsUI.Add("KsQ", new CheckBox("Kullan Q"));
            KsUI.Add("KsR", new CheckBox("Kullan R"));

            // Black Swan Akali :: Misc Menu
            MiscUI = AkaliUI.AddSubMenu("Misc");
            MiscUI.AddGroupLabel("Ek :: Ayarlar");
            MiscUI.Add("GapR", new CheckBox("AntiGapcloser için R"));
            MiscUI.Add("FleeW", new CheckBox("Kaçarken W Kullan"));
            MiscUI.AddSeparator();
            MiscUI.AddGroupLabel("Ek :: İtemler");
            MiscUI.Add("UseItems", new CheckBox("Use Agressive Items"));
            MiscUI.AddSeparator();
            MiscUI.AddGroupLabel("Ek :: Gösterge");
            MiscUI.Add("DrawQ", new CheckBox("Göster Q"));
            MiscUI.Add("DrawR", new CheckBox("Göster R"));
        }

        public static void Initialize()
        {
        }
    }
}
