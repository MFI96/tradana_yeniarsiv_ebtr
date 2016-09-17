using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System.Reflection;
namespace KappaLeBlanc
{
    class LBMenu : Helper
    {
        public static Menu Menu, ComboM, LCM, KSM, AntiGapcloserM, HSM,DrawM, FLM, Misc;
        static readonly string[] Modes = new string[] { "Move to nearest ally tower", "Move to mouse", "Move to enemy" };
        public static void StartMenu()
        {
            Menu = MainMenu.AddMenu("Kappa Leblanc", "menu");
            Menu.AddGroupLabel("Kappa Leblanc Reworked");
            Menu.AddLabel("By Capitão Addon");
            Menu.AddSeparator();
            Menu.AddSeparator();
            Menu.AddSeparator();
            Menu.AddLabel("Current Version: " + Assembly.GetExecutingAssembly().GetName().Version);
            Menu.AddLabel("Çeviri-TRAdana");

            DrawingsMenu();
            ComboMenu();
            LaneClearMenu();
            KillstealMenu();
            AntiGapMenu();
            Flee();
            MiscMenu();
        }
        private static void DrawingsMenu()
        {
            DrawM = Menu.AddSubMenu("Gösterge", "draw");
            DrawM.Add("Q", new CheckBox("Göster Q v"));
            DrawM.Add("W", new CheckBox("Göster W Menzili", false));
            DrawM.Add("WPos", new CheckBox("Göster W Pozisyonu", false));
            DrawM.Add("E", new CheckBox("Göster E Menzili", false));
            DrawM.AddSeparator();
            DrawM.Add("line", new CheckBox("Öldürülebilir hedef çizgisini göster"));
            DrawM.Add("dist", new Slider("en fazla çizgi mesafesi", 1000, 500, 3000));   

        }
        private static void ComboMenu()
        {
            ComboM = Menu.AddSubMenu("Combo", "combo");
            ComboM.Add("Q", new CheckBox("Kullan Q"));
            ComboM.Add("W", new CheckBox("Kullan W"));
            ComboM.Add("extW", new CheckBox("Gelişmiş W (W to gapclose)", false));
            ComboM.Add("E", new CheckBox("Kullan E"));
            ComboM.Add("R", new CheckBox("Kullan R"));
            ComboM.Add("RQ", new CheckBox("Kullan R (Q)"));
            ComboM.Add("RW", new CheckBox("Kullan R (W)", false));
            ComboM.Add("RE", new CheckBox("Kullan R (E)"));

        }
        private static void LaneClearMenu()
        {
            LCM = Menu.AddSubMenu("Laneclear", "laneclear");
            LCM.Add("Q", new CheckBox("Kullan Q", false));
            LCM.Add("QMana", new Slider("en az mana yüzde Q", 20, 0, 100));
            LCM.Add("W", new CheckBox("Kullan W"));         
            LCM.Add("WMana", new Slider("en az mana yüzde W", 20, 0, 100));
            LCM.Add("W2", new CheckBox("Otomatik W2"));
            LCM.Add("WMin", new Slider("en az mana yüzde W", 4, 1, 10));
            LCM.AddSeparator();
            LCM.Add("R", new CheckBox("Kullan R (W)", false));
            LCM.Add("RMin", new Slider("en az mana yüzde R (W)", 6, 1, 10));

            //Who uses E in laneclear -.-

        }
        private static void KillstealMenu()
        {
            KSM = Menu.AddSubMenu("Killsteal", "ks");
            KSM.Add("Q", new CheckBox("Kullan Q KS"));
            KSM.Add("W", new CheckBox("Kullan W KS"));
            KSM.Add("extW", new CheckBox("Gelişmiş W Kullan (veya R) Ks için (W + Q or E)"));
            KSM.Add("wr", new CheckBox("Kullan W+R + Ks için Q/E"));
            KSM.Add("E", new CheckBox("Kullan E KS"));
            KSM.Add("R", new CheckBox("Kullan R KS"));

        }
        private static void AntiGapMenu()
        {
            AntiGapcloserM = Menu.AddSubMenu("Anti Gapcloser", "antigap");
            AntiGapcloserM.Add("E", new CheckBox("AntiGapcloser için E"));
            AntiGapcloserM.Add("RE", new CheckBox("R (E) anti-gapclose", false));

            HarassMenu();
        }
        private static void HarassMenu()
        {
            HSM = Menu.AddSubMenu("Harass");

            HSM.Add("Q", new CheckBox("Kullan Q"));
            HSM.Add("QMana", new Slider("en az mana yüzde Q", 40, 0, 100));
            HSM.AddSeparator();
            HSM.Add("W", new CheckBox("Kullan W"));
            HSM.Add("extW", new CheckBox("Gelişmiş W (Gapcloser için W)", false));
            HSM.Add("AutoW", new CheckBox("Dürtmeden sonra otomatik W2"));
            HSM.Add("WMana", new Slider("en az mana yüzde W", 40, 0, 100));
            HSM.AddSeparator();
            HSM.Add("E", new CheckBox("Kullan E", false));
            HSM.Add("EMana", new Slider("en az mana yüzde E", 40, 0, 100));
            HSM.AddSeparator();
            HSM.Add("Auto", new KeyBind("Otomatik Dürtme", false, KeyBind.BindTypes.PressToggle, 'N'));

        }
        private static void Flee()
        {
            FLM = Menu.AddSubMenu("Flee");

            FLM.Add("E", new CheckBox("Kullan E"));
            FLM.Add("W", new CheckBox("W kullan imleç pozisyonu"));
            FLM.Add("R", new CheckBox("R kullan imleç pozisyonu", false));
        }
        private static void MiscMenu()
        {
            Misc = Menu.AddSubMenu("Misc");

            Misc.Add("AutoW", new Slider("Canım Şundan azsa W2 kullan", 20, 0, 100));
            Misc.Add("Clone", new CheckBox("Control Klonu"));
            var a = Misc.Add("CloneMode", new Slider("", 1, 0, 2));
            a.DisplayName = Modes[a.CurrentValue];
            a.OnValueChange += delegate
                (ValueBase<int> sender, ValueBase<int>.ValueChangeArgs Args)
            {
                sender.DisplayName = Modes[Args.NewValue];
            };
        }
    }
}