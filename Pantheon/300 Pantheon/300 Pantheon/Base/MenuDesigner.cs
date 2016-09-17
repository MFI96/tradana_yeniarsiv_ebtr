using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace _300_Pantheon.Base
{
    internal class MenuDesigner
    {
        public static Menu PantheonUi;
        public static Menu ComboUi;
        public static Menu HarassUi;
        public static Menu ClearUi;
        public static Menu KsUi;
        public static Menu MiscUi;

        internal static void Initialize()
        {
            PantheonUi = MainMenu.AddMenu("300 Pantheon", "panth", "Enelx 300 Pantheon");
            PantheonUi.AddGroupLabel("This is SCRIPTED !");

            ComboUi = PantheonUi.AddSubMenu("Kombo");
            ComboUi.AddGroupLabel("Kombo :: Büyüleri");
            ComboUi.Add("ComboQ", new CheckBox("Kullan Q"));
            ComboUi.Add("ComboW", new CheckBox("Kullan W"));
            ComboUi.Add("ComboE", new CheckBox("Kullan E"));

            HarassUi = PantheonUi.AddSubMenu("Dürtme");
            HarassUi.AddGroupLabel("Dürtme :: Büyüleri");
            HarassUi.Add("HarassQ", new CheckBox("Kullan Q"));
            HarassUi.AddSeparator();
            HarassUi.AddGroupLabel("Dürtme :: Ayarları");
            HarassUi.Add("ToggleHarass",
                new KeyBind("Dürtme Tuşu", false, KeyBind.BindTypes.PressToggle, "T".ToCharArray()[0]));
            HarassUi.AddSeparator();
            HarassUi.Add("HarassMana", new Slider("En az mana %", 40));

            ClearUi = PantheonUi.AddSubMenu("Clear");
            ClearUi.AddGroupLabel("Son Vuruş :: Büyüleri");
            ClearUi.Add("ClearLastQ", new CheckBox("Kullan Q"));
            ClearUi.AddSeparator();
            ClearUi.AddGroupLabel("Lane Temizleme :: Büyüleri");
            ClearUi.Add("ClearLaneQ", new CheckBox("Kullan Q"));
            ClearUi.Add("ClearLaneW", new CheckBox("Kullan W"));
            ClearUi.Add("ClearLaneE", new CheckBox("Kullan E"));
            ClearUi.AddSeparator();
            ClearUi.Add("ClearLanaMana", new Slider("En az. Mana %", 50));
            ClearUi.AddSeparator();
            ClearUi.AddGroupLabel("Orman Temizleme :: Büyüleri");
            ClearUi.Add("ClearJungleQ", new CheckBox("Kullan Q"));
            ClearUi.Add("ClearJungleW", new CheckBox("Kullan Q"));
            ClearUi.Add("ClearJungleE", new CheckBox("Kullan Q"));

            KsUi = PantheonUi.AddSubMenu("Killsteal");
            KsUi.Add("KsQ", new CheckBox("Kullan Q"));
            KsUi.Add("KsW", new CheckBox("Kullan W"));

            MiscUi = PantheonUi.AddSubMenu("Misc");
            MiscUi.AddGroupLabel("Misc :: Config");
            MiscUi.Add("InterW", new CheckBox("Interrupt W"));
            MiscUi.Add("GapW", new CheckBox("Gapcloser W"));
            MiscUi.Add("MiscItems", new CheckBox("Kullan iTEMLER"));
            MiscUi.AddSeparator();
            MiscUi.Add("MiscDrawQW", new CheckBox("Göster Q W"));
        }
    }
}