using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace _300_Pantheon.Assistants
{
    public static class MenuDesigner
    {
        public const string MenuName = "300 Pantheon";

        public static readonly Menu PantheonUi, ComboUi, HarassUi, ClearUi, KsUi, MiscUi;

        static MenuDesigner()
        {
            // 300 Pantheon :: Main Menu
            PantheonUi = MainMenu.AddMenu(MenuName, MenuName.ToLower());
            PantheonUi.AddGroupLabel("THIS IS SCRIPTED !!!");
            PantheonUi.AddSeparator();
            PantheonUi.AddLabel("Developer    :   Enelx");
            PantheonUi.AddLabel("Version          :   2.0.0.0");

            // 300 Pantheon :: Combo Menu
            ComboUi = PantheonUi.AddSubMenu("Combo");
            ComboUi.AddGroupLabel("Combo :: Spells");
            ComboUi.Add("ComboQ", new CheckBox("Use Q"));
            ComboUi.Add("ComboW", new CheckBox("Use W"));
            ComboUi.Add("ComboE", new CheckBox("Use E"));

            // 300 Pantheon :: Harass Menu
            HarassUi = PantheonUi.AddSubMenu("Harass");
            HarassUi.AddGroupLabel("Harass :: Spells");
            HarassUi.Add("HarassQ", new CheckBox("Use Q"));
            HarassUi.AddSeparator();
            HarassUi.AddGroupLabel("Harass :: Settings");
            HarassUi.Add("ToggleHarass",
                new KeyBind("Toggle Harass", false, KeyBind.BindTypes.PressToggle, "T".ToCharArray()[0]));
            HarassUi.AddSeparator();
            HarassUi.Add("HarassMana", new Slider("Mana Min. %", 40));

            // 300 Pantheon :: Clear Menu
            ClearUi = PantheonUi.AddSubMenu("Clear");
            ClearUi.AddGroupLabel("Last Hit :: Spells");
            ClearUi.Add("LastQ", new CheckBox("Use Q"));
            ClearUi.AddSeparator();
            ClearUi.AddGroupLabel("Lane Clear :: Spells");
            ClearUi.Add("ClearQ", new CheckBox("Use Q"));
            ClearUi.Add("ClearW", new CheckBox("Use W"));
            ClearUi.Add("ClearE", new CheckBox("Use E"));
            ClearUi.AddSeparator();
            ClearUi.Add("ClearMana", new Slider("Mana Min. %", 50));
            ClearUi.AddSeparator();
            ClearUi.AddGroupLabel("Jungle Clear :: Spells");
            ClearUi.Add("JungleQ", new CheckBox("Use Q"));
            ClearUi.Add("JungleW", new CheckBox("Use W"));
            ClearUi.Add("JungleE", new CheckBox("Use E"));

            // 300 Pantheon :: Killsteal Menu
            KsUi = PantheonUi.AddSubMenu("Killsteal");
            KsUi.AddGroupLabel("Killsteal :: Spells");
            KsUi.Add("KsQ", new CheckBox("Use Q"));
            KsUi.Add("KsW", new CheckBox("Use W"));

            // 300 Pantheon :: Misc Menu
            MiscUi = PantheonUi.AddSubMenu("Misc");
            MiscUi.AddGroupLabel("Misc :: Settings");
            MiscUi.Add("InterW", new CheckBox("Use W for Interrupt"));
            MiscUi.Add("GapW", new CheckBox("Use W for Anti Gapclose"));
            MiscUi.AddSeparator();
            MiscUi.AddGroupLabel("Misc :: Items");
            MiscUi.Add("UseItems", new CheckBox("Use Agressive Items"));
            MiscUi.AddSeparator();
            MiscUi.AddGroupLabel("Misc :: Draw");
            MiscUi.Add("DrawSpells", new CheckBox("Draw Q W E"));
        }

        public static void Initialize()
        {
        }
    }
}