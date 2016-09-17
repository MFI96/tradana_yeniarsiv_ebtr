using EloBuddy.SDK.Menu;
using LightAmumu.MenuList;

namespace LightAmumu
{
    public static class DrawingMenu
    {
        private const string MenuName = "LightAmumu";
        public static readonly Menu Menu;

        static DrawingMenu()
        {
            Menu = MainMenu.AddMenu(MenuName, MenuName);
            Menu.AddGroupLabel("Hoşgeldin LightAmumu!");
            Modes.Initialize();
        }

        public static void Initialize()
        {
        }

        public static class Modes
        {
            static Modes()
            {
                //Menu drawing!
                Combo.Initialize();
                Drawing.Initialize();
                Farm.Initialize();
                Misc.Initialize();
            }

            public static void Initialize()
            {
            }
        }
    }
}