using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace LightAmumu.MenuList
{
    public static class Misc
    {
        private static readonly Menu Menu;
        private static readonly CheckBox _smartW;
        private static readonly CheckBox _HealthbarEnabled;
        private static readonly Slider _predQ;

        public static bool smartW
        {
            get { return _smartW.CurrentValue; }
        }

        public static int predQ
        {
            get { return _predQ.CurrentValue; }
        }

        public static bool HealthbarEnabled
        {
            get { return _HealthbarEnabled.CurrentValue; }
        }

        static Misc()
        {
            Menu = DrawingMenu.Menu.AddSubMenu("Misc");
            _smartW = Menu.Add("smartW", new CheckBox("Otomatik W Kapat (Akıllıca)"));
            Menu.AddSeparator();
            _predQ = Menu.Add("predQ", new Slider("Q İsabet Oranı", 90, 1, 100));
            Menu.AddSeparator();
            Menu.AddLabel("Hasar Tespitçisi");
            _HealthbarEnabled = Menu.Add("Healthbar", new CheckBox("Healthbar", true));
        }

        public static void Initialize()
        {
        }
    }
}