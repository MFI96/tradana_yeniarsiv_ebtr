using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace LightAmumu.MenuList
{
    public static class Combo
    {
        private static readonly Menu Menu;
        private static readonly CheckBox _withQ;
        private static readonly CheckBox _withW;
        private static readonly CheckBox _withE;
        private static readonly CheckBox _withR;
        private static readonly CheckBox _withIgnite;
        private static readonly Slider _countEnemiesInR;

        public static bool WithQ
        {
            get { return _withQ.CurrentValue; }
        }

        public static bool WithW
        {
            get { return _withW.CurrentValue; }
        }

        public static bool WithE
        {
            get { return _withE.CurrentValue; }
        }

        public static bool WithR
        {
            get { return _withR.CurrentValue; }
        }

        public static bool UseIgnite
        {
            get { return _withIgnite.CurrentValue; }
        }

        public static int CountEnemiesInR
        {
            get { return _countEnemiesInR.CurrentValue; }
        }

        static Combo()
        {
            Menu = DrawingMenu.Menu.AddSubMenu("Combo");
            _withQ = Menu.Add("useQ", new CheckBox("Q"));
            _withW = Menu.Add("useW", new CheckBox("W"));
            _withE = Menu.Add("useE", new CheckBox("E"));
            _withR = Menu.Add("useR", new CheckBox("R"));
            _withIgnite = Menu.Add("useIgnite", new CheckBox("Tutuştur"));
            Menu.AddSeparator();
            _countEnemiesInR = Menu.Add("countEnimiesInR", new Slider("R için düşman say", 3, 1, 5));
        }

        public static void Initialize()
        {
        }
    }
}