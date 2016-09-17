namespace KappaKindred
{
    using EloBuddy;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    internal class Menu
    {
        private static EloBuddy.SDK.Menu.Menu menuIni;

        public static EloBuddy.SDK.Menu.Menu ComboMenu { get; private set; }

        public static EloBuddy.SDK.Menu.Menu DrawMenu { get; private set; }

        public static EloBuddy.SDK.Menu.Menu HarassMenu { get; private set; }

        public static EloBuddy.SDK.Menu.Menu JungleMenu { get; private set; }

        public static EloBuddy.SDK.Menu.Menu LaneMenu { get; private set; }

        public static EloBuddy.SDK.Menu.Menu ManaMenu { get; private set; }

        public static EloBuddy.SDK.Menu.Menu FleeMenu { get; private set; }

        public static EloBuddy.SDK.Menu.Menu UltMenu { get; private set; }

        public static void Load()
        {
            menuIni = MainMenu.AddMenu("Kindred", "Kindred");
            menuIni.AddGroupLabel("Welcome to the Worst Kindred addon!");

            UltMenu = menuIni.AddSubMenu("Ultimate");
            UltMenu.AddGroupLabel("Ultimate Settings");
            UltMenu.Add("Rally", new CheckBox("R Save Ally / Self"));
            UltMenu.Add("Rallyh", new Slider("R Ally Health %", 20, 0, 100));
            UltMenu.AddGroupLabel("Don't Use Ult On: ");
            foreach (var ally in ObjectManager.Get<AIHeroClient>())
            {
                CheckBox cb = new CheckBox(ally.BaseSkinName) { CurrentValue = false };
                if (ally.Team == ObjectManager.Player.Team)
                {
                    UltMenu.Add("DontUlt" + ally.BaseSkinName, cb);
                }
            }

            ComboMenu = menuIni.AddSubMenu("Combo");
            ComboMenu.AddGroupLabel("Kombo Ayarları");
            ComboMenu.Add("Q", new CheckBox("Kullan Q"));
            ComboMenu.Add("W", new CheckBox("Kullan W"));
            ComboMenu.Add("E", new CheckBox("Kullan E"));
            ComboMenu.AddGroupLabel("Ek Ayarlar");
            ComboMenu.Add("Qmode", new ComboBox("Q Mode", 0, "To Target", "To Mouse"));
            ComboMenu.Add("QW", new CheckBox("Q yu sadece w aktifse kullan", false));
            ComboMenu.Add("QAA", new CheckBox("AA menzilinde hedefe Q kullanma", false));
            ComboMenu.Add("Emark", new CheckBox("Odaklanmış hedefe E işaretle"));
            ComboMenu.Add("Pmark", new CheckBox("Odaklanmış hedefe pasif işaretle"));
            ComboMenu.Add("Pspells", new CheckBox("Hedefe ulti kullanmak için canı yüzde 15ten az olsun", false));

            HarassMenu = menuIni.AddSubMenu("Harass");
            HarassMenu.AddGroupLabel("Dürtme Ayarları");
            HarassMenu.Add("Q", new CheckBox("Kullan Q"));
            HarassMenu.Add("W", new CheckBox("Kullan W", false));
            HarassMenu.Add("E", new CheckBox("Kullan E"));

            LaneMenu = menuIni.AddSubMenu("Lane Clear");
            LaneMenu.AddGroupLabel("LaneTemizleme Ayarları");
            LaneMenu.Add("Q", new CheckBox("Kullan Q"));
            LaneMenu.Add("W", new CheckBox("Kullan W", false));
            LaneMenu.Add("E", new CheckBox("Kullan E", false));

            JungleMenu = menuIni.AddSubMenu("Jungle Clear");
            JungleMenu.AddGroupLabel("OrmanTemizleme Ayarları");
            JungleMenu.Add("Q", new CheckBox("Kullan Q"));
            JungleMenu.Add("W", new CheckBox("Kullan W", false));
            JungleMenu.Add("E", new CheckBox("Kullan E", false));

            FleeMenu = menuIni.AddSubMenu("Flee");
            FleeMenu.AddGroupLabel("Flee Ayarları");
            FleeMenu.Add("Q", new CheckBox("Kullan Q"));
            FleeMenu.Add("Qgap", new CheckBox("Gapclose Q Kullan"));

            ManaMenu = menuIni.AddSubMenu("Mana Yardımcısı");
            ManaMenu.AddGroupLabel("Dürtme");
            ManaMenu.Add("harassmana", new Slider("Dürtme mana %", 75, 0, 100));
            ManaMenu.AddGroupLabel("Lanetemizleme");
            ManaMenu.Add("lanemana", new Slider("Lanetemizleme mana %", 60, 0, 100));

            DrawMenu = menuIni.AddSubMenu("Drawings");
            DrawMenu.AddGroupLabel("Gösterge Ayarları");
            DrawMenu.Add("Q", new CheckBox("Göster Q"));
            DrawMenu.Add("W", new CheckBox("Göster W"));
            DrawMenu.Add("E", new CheckBox("Göster E"));
            DrawMenu.Add("R", new CheckBox("Göster R"));
            DrawMenu.Add("debug", new CheckBox("Hataayıklama", false));
        }
    }
}