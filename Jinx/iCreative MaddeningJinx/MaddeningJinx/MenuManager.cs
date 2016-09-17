using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace MaddeningJinx
{
    public static class MenuManager
    {
        public static Menu Menu;
        public static Dictionary<string, Menu> SubMenu = new Dictionary<string, Menu>();

        public static Menu MiscMenu
        {
            get { return GetSubMenu("Misc"); }
        }

        public static Menu PredictionMenu
        {
            get { return GetSubMenu("Prediction"); }
        }

        public static Menu DrawingsMenu
        {
            get { return GetSubMenu("Drawings"); }
        }

        public static KeyBind TapKey;

        public static bool TapKeyPressed;

        public static Menu ImmobileSpellsMenu;
        
        public static readonly HashSet<string> ImmobileSpellsHashSet = new HashSet<string>();

        public static void Initialize()
        {
            Menu = MainMenu.AddMenu(Champion.AddonName, Champion.AddonName + " by " + Champion.Author);

            Menu.AddGroupLabel("Genel Ayarlar");
            Menu.Add("Prediction.W", new Slider("W İsabet Oranı", 80));
            TapKey = Menu.Add("R.Tap.Key", new KeyBind("Otomatik R Kullan şu tuşla", false, EloBuddy.SDK.Menu.Values.KeyBind.BindTypes.HoldActive, 'T'));
            TapKey.OnValueChange += OnValueChange;

            SubMenu["Combo"] = Menu.AddSubMenu("Combo");
            {
                SubMenu["Combo"].Add("Q", new CheckBox("Roket Kullan (Akıllı)"));
                SubMenu["Combo"].Add("Q.Aoe", new Slider("Roket Kullan isabet edecekse >= {0}", 2, 1, 5));
                SubMenu["Combo"].Add("W", new CheckBox("W Kullan"));
                SubMenu["Combo"].Add("E", new CheckBox("E Kullan (Akıllı)"));
                SubMenu["Combo"].Add("E.Aoe", new Slider("E Kullan isabet edecekse >= {0}", 3, 0, 5));
            }
            SubMenu["Automatic"] = Menu.AddSubMenu("Automatic");
            {
                SubMenu["Automatic"].Add("W.Spells", new CheckBox("Kaçan hedefe W kullan"));
                SubMenu["Automatic"].Add("E.CC", new CheckBox("Use E on enemies with CC"));
                SubMenu["Automatic"].Add("E.Antigapcloser", new CheckBox("E Kullan  antigapcloser"));
                SubMenu["Automatic"].Add("E.Spells", new CheckBox("Use E to immobile spells casted by enemy"));
                SubMenu["Automatic"].Add("R.JungleSteal", new CheckBox("Ejder Baron çalmak için R Kullan"));

                if (EntityManager.Heroes.Enemies.Count > 0)
                {

                    var hash = new HashSet<string>();
                    foreach (var h in EntityManager.Heroes.Enemies)
                    {
                        hash.Add(h.ChampionName);
                    }

                    foreach (var t in ImmobileTracker.ChampionImmobileSlots.Where(h => EntityManager.Heroes.Enemies.Any(h2 => h2.Hero == h.Key)))
                    {
                        if (ImmobileSpellsMenu == null)
                        {
                            ImmobileSpellsMenu = SubMenu["Automatic"];
                            ImmobileSpellsMenu.AddGroupLabel("Enemy spells to cast E");
                        }
                        var enemy = EntityManager.Heroes.Enemies.FirstOrDefault(h => t.Key == h.Hero);
                        if (enemy != null)
                        {
                            ImmobileSpellsMenu.AddLabel(enemy.ChampionName);
                            foreach (var slot in t.Value.Where(slot => !ImmobileSpellsHashSet.Contains(enemy.ChampionName + slot)))
                            {
                                ImmobileSpellsMenu.Add(enemy.ChampionName + slot, new CheckBox(slot.ToString()));
                                ImmobileSpellsHashSet.Add(enemy.ChampionName + slot);
                            }
                        }
                    }
                }
            }

            SubMenu["KillSteal"] = Menu.AddSubMenu("KillSteal");
            {
                SubMenu["KillSteal"].Add("Q", new CheckBox("Roket Kullan"));
                SubMenu["KillSteal"].Add("W", new CheckBox("W Kullan"));
                SubMenu["KillSteal"].Add("E", new CheckBox("E Kullan"));
                SubMenu["KillSteal"].Add("R", new CheckBox("R Kullan (Sadece ölecek varsa)"));
            }
            SubMenu["Clear"] = Menu.AddSubMenu("Clear");
            {
                SubMenu["Clear"].AddGroupLabel("SonVuruş");
                SubMenu["Clear"].Add("LastHit.Q", new Slider("Roket kullan .. isabet edecekse >= {0}", 2, 0, 10));
                SubMenu["Clear"].AddGroupLabel("LaneTemizleyici");
                SubMenu["Clear"].Add("LaneClear.Q", new Slider("Roket kullan .. isabet edecekse >= {0}", 3, 0, 10));
                SubMenu["Clear"].AddGroupLabel("Orman Temizleyici");
                SubMenu["Clear"].Add("JungleClear.Q", new Slider("Roket kullan ..isabet edecekse >= {0}", 2, 0, 10));
            }

            SubMenu["Drawings"] = Menu.AddSubMenu("Drawings");
            {
                SubMenu["Drawings"].Add("Disable", new CheckBox("Tüm Göstergeleri Kapat", false));
                SubMenu["Drawings"].Add("W", new CheckBox("Göster W Menzili"));
                SubMenu["Drawings"].Add("DamageIndicator", new CheckBox("R ile  vurulduğunda verilecek Hasar Tespiti "));
                SubMenu["Drawings"].Add("Target", new CheckBox("Hedefi Daireyle Göster"));
                SubMenu["Drawings"].Add("R.Killable", new CheckBox("Eğer öldürülebilecek varsa R ile göster"));
            }

        }

        private static void OnValueChange(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
        {
            if (args.NewValue && SpellSlot.R.IsReady() && KillSteal.RKillableBases.Any())
            {
                TapKeyPressed = true;
            }
        }

        public static int Slider(this Menu m, string s)
        {
            if (m != null)
                return m[s].Cast<Slider>().CurrentValue;
            return -1;
        }

        public static bool CheckBox(this Menu m, string s)
        {
            return m != null && m[s].Cast<CheckBox>().CurrentValue;
        }

        public static bool KeyBind(this Menu m, string s)
        {
            return m != null && m[s].Cast<KeyBind>().CurrentValue;
        }

        public static void AddStringList(this Menu m, string uniqueId, string displayName, string[] values,
            int defaultValue)
        {
            var mode = m.Add(uniqueId, new Slider(displayName, defaultValue, 0, values.Length - 1));
            mode.DisplayName = displayName + ": " + values[mode.CurrentValue];
            mode.OnValueChange +=
                delegate (ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
                {
                    sender.DisplayName = displayName + ": " + values[args.NewValue];
                };
        }

        public static Menu GetSubMenu(string s)
        {
            return (from t in SubMenu where t.Key.Equals(s) select t.Value).FirstOrDefault();
        }
    }
}