using EloBuddy.SDK.Menu;

namespace AlchemistSinged
{
    internal class MenuManager
    {
        // Create Main Segments
        public static Menu AutoCannonMenu, SettingsMenu;

        public static void Initialize()
        {
            // Addon Menu
            AutoCannonMenu = MainMenu.AddMenu("AutoCannon", "AutoCannon");
            AutoCannonMenu.AddGroupLabel("AutoCannon");
            AutoCannonMenu.AddLabel("Created by Counter");
            AutoCannonMenu.AddLabel("This addon will shoot Mark/Poros. :) <3");
            AutoCannonMenu.AddLabel("Also has a KS feature for priority kills!! ;) xD");

            SettingsMenu = AutoCannonMenu.AddSubMenu("Settings", "Settings");
            SettingsMenu.AddGroupLabel("Settings");
            SettingsMenu.AddLabel("Keep in mind that the script won't dash to target. This must be done manually.");
            SettingsMenu.AddSeparator();
            SettingsMenu.AddLabel("Press F5 to update these numbers for Drawing.");
        }
    }
}