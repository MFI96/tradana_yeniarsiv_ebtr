using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;

namespace LeeSin
{
    public static class MenuManager
    {
        public static Menu AddonMenu;
        public static Dictionary<string, Menu> SubMenu = new Dictionary<string, Menu>();
        public static void Init()
        {
            var AddonName = Champion.AddonName;
            var Author = Champion.Author;
            AddonMenu = MainMenu.AddMenu(AddonName, AddonName + " by " + Author + " v6.4.0");
            AddonMenu.AddLabel(AddonName + " made by " + Author);

            SubMenu["Prediction"] = AddonMenu.AddSubMenu("Prediction", "Prediction3");
            SubMenu["Prediction"].AddGroupLabel("Q Ayarları");
            SubMenu["Prediction"].Add("QCombo", new Slider("Kombo İsabet Oranı", 65));
            SubMenu["Prediction"].Add("QHarass", new Slider("Dürtme İsabet Oranı", 70));

            //Combo
            SubMenu["Combo"] = AddonMenu.AddSubMenu("Combo", "Combo");
            SubMenu["Combo"].Add("Q", new CheckBox("Kullan Q", true));
            SubMenu["Combo"].Add("W", new CheckBox("Kullan W to GapClose", true));
            SubMenu["Combo"].Add("E", new CheckBox("Kullan E", true));
            SubMenu["Combo"].Add("Smite", new CheckBox("Çarp Kullan", false));
            SubMenu["Combo"].Add("Items", new CheckBox("Ofansif İtemleri Kullan", true));
            var switcher = SubMenu["Combo"].Add("Switcher", new KeyBind("Kombo Mod Değiştirme Tuşu", false, KeyBind.BindTypes.HoldActive, (uint)'K'));
            switcher.OnValueChange += delegate (ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
            {
                if (args.NewValue)
                {
                    var cast = GetSubMenu("Combo")["Mode"].Cast<Slider>();
                    if (cast.CurrentValue == cast.MaxValue)
                    {
                        cast.CurrentValue = 0;
                    }
                    else
                    {
                        cast.CurrentValue++;
                    }
                }
            };
            SubMenu["Combo"].AddStringList("Mode", "Combo Mode", new[] { "Normal Combo", "Star Combo", "Gank Combo" }, 0);
            SubMenu["Combo"]["Mode"].Cast<Slider>().CurrentValue = 0; //E L I M I N A R

            SubMenu["Combo"].AddGroupLabel("Normal Combo");
            SubMenu["Combo"].Add("Normal.R", new CheckBox("Hedefe R Kullan", false));
            SubMenu["Combo"].Add("Normal.Ward", new CheckBox("Totem Kullan", false));
            SubMenu["Combo"].Add("Normal.Stack", new Slider("Pasiften önce diğer büyüleri kullan", 1, 0, 2));
            SubMenu["Combo"].Add("Normal.W", new Slider("Eğer canım şundan azsa W Kullan", 25, 0, 100));
            SubMenu["Combo"].Add("Normal.R.Hit", new Slider("Şu kadar veya fazla birime çarpacaksa R Kullan >= ", 3, 1, 5));

            SubMenu["Combo"].AddSeparator();

            SubMenu["Combo"].AddGroupLabel("Star Combo");
            SubMenu["Combo"].Add("Star.Ward", new CheckBox("Totem Kullan", true));
            SubMenu["Combo"].Add("Star.Stack", new Slider("Pasiften önce diğer büyüleri Kullan", 0, 0, 2));
            SubMenu["Combo"].AddStringList("Star.Mode", "Star Combo Mode", new[] { "Q1 R Q2", "R Q1 Q2" }, 0);

            SubMenu["Combo"].AddSeparator();

            SubMenu["Combo"].AddGroupLabel("Gank Combo");
            SubMenu["Combo"].Add("Gank.R", new CheckBox("R Kullan", true));
            SubMenu["Combo"].Add("Gank.Ward", new CheckBox("Totem Kullan", true));
            SubMenu["Combo"].Add("Gank.Stack", new Slider("Pasiften önce diğer büyüleri Kullan", 1, 0, 2));

            //Insec
            SubMenu["Insec"] = AddonMenu.AddSubMenu("Insec", "Insec");
            SubMenu["Insec"].Add("Key", new KeyBind("Insec Tuşu (Bu tuşu Kendine göre ayarlamalısın)", false, KeyBind.BindTypes.HoldActive, (uint)'R'));
            SubMenu["Insec"].Add("Object", new CheckBox("Eğer hedefe çarpmayacaksa Düşman minyona Q kullan", true));
            SubMenu["Insec"].AddSeparator(0);
            SubMenu["Insec"].Add("Flash.Return", new CheckBox("Flash Kullanıp Dön", false));
            SubMenu["Insec"].AddStringList("Priority", "Priority", new[] { "WardJump > Flash", "Flash > WardJump" }, 1);
            SubMenu["Insec"].AddStringList("Flash.Priority", "Flash Priority", new[] { "Sadece R -> Flash", "Sadece Flash -> R", "R -> den sonra Flash veya Flash -> dan sonra R" }, 0);
            SubMenu["Insec"].AddStringList("Position", "Insec End Position", new[] { "Dostlar Seçildi > Pozisyon Seçildi > Kule > Dost Yakını > Mevcut Pozisyon", "Fare Pozisyonu", "Mevcut Pozisyon" }, 0);
            SubMenu["Insec"].Add("DistanceBetweenPercent", new Slider("Totem ve hedef arasında mesafe yüzde (çevirmen notu elleme)", 20, 0, 100));
            SubMenu["Insec"].AddGroupLabel("Tipler");
            SubMenu["Insec"].AddLabel("Dostlara ise dostu sol tıkla tıklıyacaksın.");
            SubMenu["Insec"].AddLabel("Hedef Düşmansa Hedefi sol tıkla belirliceksin");
            SubMenu["Insec"].AddLabel("To select a position just use left click on that position.");

            SubMenu["Harass"] = AddonMenu.AddSubMenu("Harass", "Harass");
            SubMenu["Harass"].Add("Q", new CheckBox("Kullan Q", true));
            SubMenu["Harass"].Add("W", new CheckBox("Kullan W Kaçarken", true));
            SubMenu["Harass"].Add("E", new CheckBox("Kullan E", true));

            SubMenu["Smite"] = AddonMenu.AddSubMenu("Çarp", "Smite");
            SubMenu["Smite"].Add("Q.Combo", new CheckBox("Q yu Çarp'la Birlikte Kullan Komboda", true));
            SubMenu["Smite"].Add("Q.Harass", new CheckBox("Q ile Dürterken Çarp Kullan", false));
            SubMenu["Smite"].Add("Q.Insec", new CheckBox("Q ile insec yaparken çarp kullan", true));
            SubMenu["Smite"].Add("DragonSteal", new CheckBox("Çarp Kullan Baronda ve Ejderde", true));
            //SubMenu["Smite"].Add("KillSteal", new CheckBox("Use Smite to KillSteal", true));

            SubMenu["JungleClear"] = AddonMenu.AddSubMenu("JungleClear", "JungleClear");
            SubMenu["JungleClear"].Add("Q", new CheckBox("Kullan Q", true));
            SubMenu["JungleClear"].Add("W", new CheckBox("Kullan W", true));
            SubMenu["JungleClear"].Add("E", new CheckBox("Kullan E", true));
            SubMenu["JungleClear"].Add("Smite", new CheckBox("Ejder Baronda Çarp Kullan", true));

            SubMenu["KillSteal"] = AddonMenu.AddSubMenu("KillSteal", "KillSteal");
            SubMenu["KillSteal"].Add("Ward", new CheckBox("Gapclose'da totem kullan", false));
            SubMenu["KillSteal"].Add("Q", new CheckBox("Kullan Q", false));
            SubMenu["KillSteal"].Add("W", new CheckBox("Gapclose'ta W Kullan", true));
            SubMenu["KillSteal"].Add("E", new CheckBox("Kullan E", true));
            SubMenu["KillSteal"].Add("R", new CheckBox("Kullan R", false));
            SubMenu["KillSteal"].Add("Ignite", new CheckBox("Kullan Ignite", true));
            SubMenu["KillSteal"].Add("Smite", new CheckBox("Kullan Smite", true));

            SubMenu["Drawings"] = AddonMenu.AddSubMenu("Drawings", "Drawings");
            SubMenu["Drawings"].Add("Disable", new CheckBox("Tüm Göstergeleri Kapat", false));
            SubMenu["Drawings"].Add("Q", new CheckBox("Göster Q Menzili", true));
            SubMenu["Drawings"].Add("W", new CheckBox("Göster W Menzili", true));
            SubMenu["Drawings"].Add("E", new CheckBox("Göster E Menzili", false));
            SubMenu["Drawings"].Add("R", new CheckBox("Göster R Menzili", false));
            SubMenu["Drawings"].Add("Combo.Mode", new CheckBox("Mevcut Modu Göster", true));
            SubMenu["Drawings"].Add("Insec.Line", new CheckBox("İnsec Çizgisini Göster", true));
            SubMenu["Drawings"].Add("Target", new CheckBox("Hedefi Daireyle Göster", true));

            SubMenu["Flee"] = AddonMenu.AddSubMenu("Flee/WardJump", "Flee");
            SubMenu["Flee"].Add("WardJump", new CheckBox("Kullan WardJump", true));
            SubMenu["Flee"].Add("W", new CheckBox("Mouse Yakınında W Kullan", true));

            SubMenu["Misc"] = AddonMenu.AddSubMenu("Misc", "Misc");
            SubMenu["Misc"].Add("Interrupter", new CheckBox("Use R to interrupt channeling spells", true));
            SubMenu["Misc"].Add("Overkill", new Slider("Overkill % for damage prediction", 10, 0, 100));
            SubMenu["Misc"].Add("R.Hit", new Slider("R kullan eğer şu kadar çarpacaksa >=", 3, 1, 5));

        }
        
        public static int GetSliderValue(this Menu m, string s)
        {
            if (m != null)
                return m[s].Cast<Slider>().CurrentValue;
            return -1;
        }
        public static bool GetCheckBoxValue(this Menu m, string s)
        {
            if (m != null)
                return m[s].Cast<CheckBox>().CurrentValue;
            return false;
        }
        public static bool GetKeyBindValue(this Menu m, string s)
        {
            if (m != null)
                return m[s].Cast<KeyBind>().CurrentValue;
            return false;
        }
        public static void AddStringList(this Menu m, string uniqueId, string displayName, string[] values, int defaultValue = 0)
        {
            var mode = m.Add(uniqueId, new Slider(displayName, defaultValue, 0, values.Length - 1));
            mode.DisplayName = displayName + ": " + values[mode.CurrentValue];
            mode.OnValueChange += delegate (ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
            {
                sender.DisplayName = displayName + ": " + values[args.NewValue];
            };
        }
        public static Menu GetSubMenu(string s)
        {
            foreach (KeyValuePair<string, Menu> t in SubMenu)
            {
                if (t.Key.Equals(s))
                {
                    return t.Value;
                }
            }
            return null;
        }
        public static Menu MiscMenu
        {
            get
            {
                return GetSubMenu("Misc");
            }
        }
        public static Menu PredictionMenu
        {
            get
            {
                return GetSubMenu("Prediction");
            }
        }
        public static Menu DrawingsMenu
        {
            get
            {
                return GetSubMenu("Drawings");
            }
        }
        public static Menu SmiteMenu
        {
            get
            {
                return GetSubMenu("Smite");
            }
        }
    }
}
