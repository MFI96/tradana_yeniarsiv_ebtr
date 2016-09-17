using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace LightAmumu.MenuList
{
    public static class Drawing
    {
        private static readonly Menu Menu;
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

        static Drawing()
        {
            Menu = DrawingMenu.Menu.AddSubMenu("Drawing");
            _drawQ = Menu.Add("DrawQ", new CheckBox("Q"));
            _drawW = Menu.Add("DrawW", new CheckBox("W"));
            _drawE = Menu.Add("DrawE", new CheckBox("E"));
            _drawR = Menu.Add("DrawR", new CheckBox("R"));
        }

        public static void Initialize()
        {
        }
    }
}