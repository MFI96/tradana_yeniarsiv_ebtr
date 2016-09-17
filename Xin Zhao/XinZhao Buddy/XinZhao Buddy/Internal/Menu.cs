using EloBuddy;
using EloBuddy.SDK.Menu.Values;

namespace XinZhao_Buddy.Internal
{
    internal class Menu
    {
        public static EloBuddy.SDK.Menu.Menu MainMenu;
        public static EloBuddy.SDK.Menu.Menu ComboMenu;
        public static EloBuddy.SDK.Menu.Menu HarassMenu;
        public static EloBuddy.SDK.Menu.Menu ClearMenu;
        public static EloBuddy.SDK.Menu.Menu MiscMenu;
        public static EloBuddy.SDK.Menu.Menu DrawMenu;

        public static void Initialize()
        {
            MainMenu = EloBuddy.SDK.Menu.MainMenu.AddMenu("XinZhao Buddy", "XinZhaoBuddy");
            MainMenu.AddGroupLabel("XinZhao Buddy");
            MainMenu.AddLabel("Versiyon: " + "1.0.0.4");
            MainMenu.AddSeparator();
            MainMenu.AddLabel("Yapan Kişi: " + "Roach");
            MainMenu.AddLabel("Çeviri TRAdana");
            Combo.Initialize();

            Harass.Initialize();

            Clear.Initialize();

            var smiteSpell = Player.Instance.Spellbook.GetSpell(Spells.Smite);
            if (smiteSpell != null)
            {
                Smite.Initialize();
            }

            Misc.Initialize();

            KillSteal.Initialize();

            Draw.Initialize();
        }

        public static class Combo
        {
            private static CheckBox _q;
            private static CheckBox _w;
            private static CheckBox _e;
            private static CheckBox _r;
            private static CheckBox _hydra;
            private static Slider _rHp;
            private static Slider _rCount;

            public static bool Q
            {
                get { return _q.CurrentValue; }
            }

            public static bool W
            {
                get { return _w.CurrentValue; }
            }

            public static bool E
            {
                get { return _e.CurrentValue; }
            }

            public static bool R
            {
                get { return _r.CurrentValue; }
            }

            public static bool Hydra
            {
                get { return _hydra.CurrentValue; }
            }

            public static int RHp
            {
                get { return _rHp.CurrentValue; }
            }

            public static int RCount
            {
                get { return _rCount.CurrentValue; }
            }

            public static void Initialize()
            {
                ComboMenu = MainMenu.AddSubMenu("Kombo", "Combo");
                ComboMenu.AddGroupLabel("Kombo Ayarları");
                _q = ComboMenu.Add("Combo.Q", new CheckBox("Kullan Q"));
                _w = ComboMenu.Add("Combo.W", new CheckBox("Kullan W"));
                _e = ComboMenu.Add("Combo.E", new CheckBox("Kullan E"));
                _r = ComboMenu.Add("Combo.R", new CheckBox("Kullan R"));
                _hydra = ComboMenu.Add("Combo.Hydra", new CheckBox("Kullan Tiamat/Hydra"));
                ComboMenu.AddGroupLabel("R Ayarları");
                _rHp = ComboMenu.Add("Combo.R.HP", new Slider("Eğer canım şundan azsa:", 50));
                _rCount = ComboMenu.Add("Combo.R.Count", new Slider("Şu kadar düşmandan sonra:", 2, 1, 5));
            }
        }

        public static class Harass
        {
            private static CheckBox _q;
            private static CheckBox _w;
            private static CheckBox _e;
            private static CheckBox _hydra;

            public static bool Q
            {
                get { return _q.CurrentValue; }
            }

            public static bool W
            {
                get { return _w.CurrentValue; }
            }

            public static bool E
            {
                get { return _e.CurrentValue; }
            }

            public static bool Hydra
            {
                get { return _hydra.CurrentValue; }
            }

            public static void Initialize()
            {
                HarassMenu = MainMenu.AddSubMenu("Dürtme", "Harass");
                HarassMenu.AddGroupLabel("Dürtme Ayarları");
                _q = HarassMenu.Add("Harass.Q", new CheckBox("Kullan Q"));
                _w = HarassMenu.Add("Harass.W", new CheckBox("Kullan W"));
                _e = HarassMenu.Add("Harass.E", new CheckBox("Kullan E"));
                _hydra = HarassMenu.Add("Harass.Hydra", new CheckBox("Kullan Tiamat/Hydra", false));
            }
        }

        public static class Clear
        {
            private static CheckBox _q;
            private static CheckBox _w;
            private static CheckBox _e;
            private static CheckBox _hydra;

            public static bool Q
            {
                get { return _q.CurrentValue; }
            }

            public static bool W
            {
                get { return _w.CurrentValue; }
            }

            public static bool E
            {
                get { return _e.CurrentValue; }
            }

            public static bool Hydra
            {
                get { return _hydra.CurrentValue; }
            }

            public static void Initialize()
            {
                ClearMenu = MainMenu.AddSubMenu("LaneTemizleme", "Clear");
                ClearMenu.AddGroupLabel("LaneTemizleme Ayarları");
                _q = ClearMenu.Add("Clear.Q", new CheckBox("Kullan Q"));
                _w = ClearMenu.Add("Clear.W", new CheckBox("Kullan W"));
                _e = ClearMenu.Add("Clear.E", new CheckBox("Kullan E"));
                _hydra = ClearMenu.Add("Clear.Hydra", new CheckBox("Kullan Tiamat/Hydra"));
            }
        }

        public static class Smite
        {
            private static CheckBox _enable;
            private static CheckBox _baron;
            private static CheckBox _dragon;
            private static CheckBox _red;
            private static CheckBox _blue;
            private static CheckBox _krug;
            private static CheckBox _gromp;
            private static CheckBox _raptor;
            private static CheckBox _wolf;

            public static bool? Enable
            {
                get { return _enable.CurrentValue; }
            }

            public static bool? Baron
            {
                get { return _baron.CurrentValue; }
            }

            public static bool? Dragon
            {
                get { return _dragon.CurrentValue; }
            }

            public static bool? Red
            {
                get { return _red.CurrentValue; }
            }

            public static bool? Blue
            {
                get { return _blue.CurrentValue; }
            }

            public static bool? Krug
            {
                get { return _krug.CurrentValue; }
            }

            public static bool? Gromp
            {
                get { return _gromp.CurrentValue; }
            }

            public static bool? Raptor
            {
                get { return _raptor.CurrentValue; }
            }

            public static bool? Wolf
            {
                get { return _wolf.CurrentValue; }
            }

            public static void Initialize()
            {
                ClearMenu.AddGroupLabel("Çarp Atılacak");
                _enable = ClearMenu.Add("Smite.Enable", new CheckBox("Kullan Çarp"));
                ClearMenu.AddSeparator();
                _baron = ClearMenu.Add("Smite.Baron", new CheckBox("Baron Nashor"));
                _dragon = ClearMenu.Add("Smite.Dragon", new CheckBox("Ejder"));
                _red = ClearMenu.Add("Smite.Red", new CheckBox("Red"));
                _blue = ClearMenu.Add("Smite.Blue", new CheckBox("Blue"));
                _krug = ClearMenu.Add("Smite.Krug", new CheckBox("Yampiri Yengeç"));
                _gromp = ClearMenu.Add("Smite.Gromp", new CheckBox("Kurbağa"));
                _raptor = ClearMenu.Add("Smite.Raptor", new CheckBox("Sivri Gagalar"));
                _wolf = ClearMenu.Add("Smite.Wolf", new CheckBox("AlacaKurtlar"));
            }
        }

        public static class Misc
        {
            private static CheckBox _interruptR;
            private static CheckBox _debugMode;

            public static bool InterruptR
            {
                get { return _interruptR.CurrentValue; }
            }

            public static bool DebugMode
            {
                get { return _debugMode.CurrentValue; }
            }

            public static void Initialize()
            {
                MiscMenu = MainMenu.AddSubMenu("Ek", "Misc");
                MiscMenu.AddGroupLabel("Ek Ayarlar");
                _interruptR = MiscMenu.Add("Misc.InterruptR", new CheckBox("Interrupt büyüleri için Otomatik R"));
                _debugMode = MiscMenu.Add("Misc.DebugMode", new CheckBox("Sıfırla Modu", false));
            }
        }

        public static class KillSteal
        {
            private static CheckBox _e;
            private static CheckBox _r;
            private static CheckBox _ignite;
            private static CheckBox _smite;

            public static bool E
            {
                get { return _e.CurrentValue; }
            }

            public static bool R
            {
                get { return _r.CurrentValue; }
            }

            public static bool Ignite
            {
                get { return _ignite.CurrentValue; }
            }

            public static bool Smite
            {
                get { return _smite.CurrentValue; }
            }

            public static void Initialize()
            {
                MiscMenu.AddGroupLabel("Kill Çalma");
                _e = MiscMenu.Add("KillSteal.E", new CheckBox("Kullan E"));
                _r = MiscMenu.Add("KillSteal.R", new CheckBox("Kullan R"));
                _ignite = MiscMenu.Add("KillSteal.Ignite", new CheckBox("Kullan Tutuştur"));
                _smite = MiscMenu.Add("KillSteal.Smite", new CheckBox("Kullan Çarp"));
            }
        }

        public static class Draw
        {
            private static CheckBox _e;
            private static CheckBox _r;
            private static CheckBox _smite;
            private static CheckBox _damageHpBar;

            public static bool E
            {
                get { return _e.CurrentValue; }
            }

            public static bool R
            {
                get { return _r.CurrentValue; }
            }

            public static bool Smite
            {
                get { return _smite.CurrentValue; }
            }

            public static bool DamageHpBar
            {
                get { return _damageHpBar.CurrentValue; }
            }

            public static void Initialize()
            {
                DrawMenu = MainMenu.AddSubMenu("Gösterge", "Draw");
                DrawMenu.AddGroupLabel("Gösterge Ayarları");
                _e = DrawMenu.Add("Draw.E", new CheckBox("E Menzili"));
                _r = DrawMenu.Add("Draw.R", new CheckBox("R Menzili"));
                _smite = DrawMenu.Add("Draw.Smite", new CheckBox("Çarp durumunu göster"));
                _damageHpBar = DrawMenu.Add("Draw.DamageHPBar", new CheckBox("Hasarı Hp barında göster"));
            }
        }
    }
}