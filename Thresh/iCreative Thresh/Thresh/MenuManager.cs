using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace Thresh
{
    public static class MenuManager
    {
        public static Menu AddonMenu;
        public static Dictionary<string, Menu> SubMenu = new Dictionary<string, Menu>();

        public static Menu MiscMenu
        {
            get { return GetSubMenu("Ek"); }
        }

        public static Menu PredictionMenu
        {
            get { return GetSubMenu("Tutturma Ayarlama"); }
        }

        public static Menu DrawingsMenu
        {
            get { return GetSubMenu("Göstergeler"); }
        }

        public static void Init(EventArgs args)
        {
            var addonName = Champion.AddonName;
            var author = Champion.Author;
            AddonMenu = MainMenu.AddMenu(addonName, addonName + " by " + author + " v1.0000-ceviri tradana");
            AddonMenu.AddLabel(addonName + " Tarafından " + author);

            SubMenu["Prediction"] = AddonMenu.AddSubMenu("Tutturma", "Prediction 2.0");
            SubMenu["Prediction"].AddGroupLabel("Q Ayarları");
            SubMenu["Prediction"].Add("QCombo", new Slider("Kombo İsabet Oranı", 70));
            SubMenu["Prediction"].Add("QHarass", new Slider("Dürtme İsabet Oranı", 75));
            SubMenu["Prediction"].AddGroupLabel("E Ayarları");
            SubMenu["Prediction"].Add("ECombo", new Slider("Kombo İsabet Oranı", 90));
            SubMenu["Prediction"].Add("EHarass", new Slider("Dürtme İsabet Oranı", 95));

            SubMenu["Combo"] = AddonMenu.AddSubMenu("Kombo", "Combo");
            SubMenu["Combo"].AddStringList("Q1", "Q1 Kullan", new[] {"Asla", "Sadece Hedef", "Herhangi bir düşman"}, 1);
            SubMenu["Combo"].AddStringList("Q2", "Q2 Kullan",
                new[] {"Asla", "Sadece Bağlanmış Hedef", "Eğer Bağlanmışsa Yakınındaki"}, 1);
            SubMenu["Combo"].AddStringList("W1", "W Kullan", new[] {"Asla", "Only if target got hooked", "Always"}, 1);
            SubMenu["Combo"].Add("W2", new Slider("W Kullan Dostların canı şu kadarken <= {0}", 20));
            SubMenu["Combo"].AddStringList("E1", "E Modu", new[] {"Asla", "Çek", "İt", "Based on team HealthPercent" }, 1);
            SubMenu["Combo"].AddStringList("E2", "E Kullan", new[] {"Never", "Only on target", "On any enemy"}, 3);
            SubMenu["Combo"].Add("R1", new Slider("Use R if HealthPercent <= {0}", 15));
            SubMenu["Combo"].Add("R2", new Slider("Use R if Enemies Inside >= {0}", 3, 0, 5));

            SubMenu["Harass"] = AddonMenu.AddSubMenu("Dürtme", "Harass");
            SubMenu["Harass"].AddStringList("Q1", "Q1 Kullan", new[] {"Never", "Only on target", "On any enemy"}, 1);
            SubMenu["Harass"].AddStringList("Q2", "Q2 Kullan",
                new[] {"Asla", "Eğer hedef tutulmuşsa hedefe", "Eğer Yakalanmışsa Yanındaki Hedefe"}, 1);
            SubMenu["Harass"].AddStringList("W1", "W Kullan", new[] {"Asla", "Eğer Hedef Yakalanmışsa", "Her zaman"}, 1);
            SubMenu["Harass"].Add("W2", new Slider("W kullan Dost Şampın Can oranı şunda azsa <= {0}", 35));
            SubMenu["Harass"].AddStringList("E1", "E Modu", new[] {"Asla", "Çek", "İt", "Based on team HealthPercent" }, 3);
            SubMenu["Harass"].AddStringList("E2", "E Kullan", new[] {"Asla", "Sadece Hedef", "Herhangi bir düşman" }, 3);
            SubMenu["Harass"].Add("Mana", new Slider("en az mana:", 20));

            SubMenu["JungleClear"] = AddonMenu.AddSubMenu("JungleTemizleme", "JungleClear");
            SubMenu["JungleClear"].Add("Q", new CheckBox("Q Kullan"));
            SubMenu["JungleClear"].Add("E", new CheckBox("E Kullan"));
            SubMenu["JungleClear"].Add("Mana", new Slider("en az mana %:", 20));

            SubMenu["KillSteal"] = AddonMenu.AddSubMenu("Kill Çalma", "KillSteal");
            SubMenu["KillSteal"].Add("Q", new CheckBox("Q Kullan", false));
            SubMenu["KillSteal"].Add("E", new CheckBox("E Kullan", false));
            SubMenu["KillSteal"].Add("R", new CheckBox("R Kullan", false));
            SubMenu["KillSteal"].Add("Ignite", new CheckBox("Tutuştur Kullan"));

            SubMenu["Flee"] = AddonMenu.AddSubMenu("Flee(Kaçma)", "Flee");
            SubMenu["Flee"].Add("W", new CheckBox("Dostlar için W kullan"));
            SubMenu["Flee"].Add("E", new CheckBox("Düşmanı E ile İt"));

            SubMenu["Drawings"] = AddonMenu.AddSubMenu("Göstergeler", "Drawings");
            SubMenu["Drawings"].Add("Disable", new CheckBox("Disable all drawings", false));
            SubMenu["Drawings"].AddSeparator();
            SubMenu["Drawings"].Add("Q", new CheckBox("Göster Q Menzili"));
            SubMenu["Drawings"].Add("W", new CheckBox("Göster W Menzili", false));
            SubMenu["Drawings"].Add("E", new CheckBox("Göster E Menzili"));
            SubMenu["Drawings"].Add("R", new CheckBox("Göster R Menzili", false));
            SubMenu["Drawings"].Add("Enemy.Target", new CheckBox("Düşman Hedefi Dairede Göster"));
            SubMenu["Drawings"].Add("Ally.Target", new CheckBox("Dostları Daireyle Göster"));

            SubMenu["Misc"] = AddonMenu.AddSubMenu("Ek", "Misc");
            SubMenu["Misc"].Add("W", new Slider("Düşmanın içindeki Dostlara W kullan >= {0}", 3, 0, 5));
            SubMenu["Misc"].Add("GapCloser.E", new CheckBox("Use E to Interrupt GapClosers (Push or Pull)"));
            SubMenu["Misc"].Add("GapCloser.Q", new CheckBox("Use Q to Interrupt Escapes"));
            SubMenu["Misc"].Add("Interrupter", new CheckBox("Use Q/E to Interrupt Channeling Spells"));
            SubMenu["Misc"].Add("Turret.Q", new CheckBox("Use Q for enemies in turret"));
            SubMenu["Misc"].Add("Turret.E", new CheckBox("Use E for enemies in turret"));
        }

        public static int GetSliderValue(this Menu m, string s)
        {
            if (m != null)
                return m[s].Cast<Slider>().CurrentValue;
            return -1;
        }

        public static bool GetCheckBoxValue(this Menu m, string s)
        {
            return m != null && m[s].Cast<CheckBox>().CurrentValue;
        }

        public static bool GetKeyBindValue(this Menu m, string s)
        {
            return m != null && m[s].Cast<KeyBind>().CurrentValue;
        }

        public static void AddStringList(this Menu m, string uniqueId, string displayName, string[] values,
            int defaultValue = 0)
        {
            var mode = m.Add(uniqueId, new Slider(displayName, defaultValue, 0, values.Length - 1));
            mode.DisplayName = displayName + ": " + values[mode.CurrentValue];
            mode.OnValueChange +=
                delegate(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
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