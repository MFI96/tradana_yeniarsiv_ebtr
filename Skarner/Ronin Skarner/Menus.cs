using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using EloBuddy;
using System;
using System.IO;
using System.Media;
using System.Net;
using EloBuddy.SDK;
using EloBuddy.SDK.Constants;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Menu;
using Mario_s_Lib;

namespace RoninSkarner
{
    internal class Menus
    {
        public const string ComboMenuID = "combomenuid";
        public const string HarassMenuID = "harassmenuid";
        public const string AutoHarassMenuID = "autoharassmenuid";
        public const string LaneClearMenuID = "laneclearmenuid";
        //public const string LastHitMenuID = "lasthitmenuid";
        public const string JungleClearMenuID = "jungleclearmenuid";
        public const string KillStealMenuID = "killstealmenuid";
        public const string DrawingsMenuID = "drawingsmenuid";
        public const string MiscMenuID = "miscmenuid";
        public static Menu FirstMenu;
        public static Menu ComboMenu;
        public static Menu HarassMenu;
        public static Menu AutoHarassMenu;
        public static Menu LaneClearMenu;
        public static Menu LasthitMenu;
        public static Menu JungleClearMenu;
        public static Menu KillStealMenu;
        public static Menu DrawingsMenu;
        public static Menu MiscMenu;

        //These colorslider are from Mario`s Lib
        public static ColorSlide QColorSlide;
        public static ColorSlide WColorSlide;
        public static ColorSlide EColorSlide;
        public static ColorSlide RColorSlide;
        public static ColorSlide DamageIndicatorColorSlide;

        public static void CreateMenu()
        {
            FirstMenu = MainMenu.AddMenu("Ronin`s " + Player.Instance.ChampionName, Player.Instance.ChampionName.ToLower() + "hue");
            FirstMenu.AddGroupLabel("Addon by Taazuma / Kullandığın için teşekkürler");
            FirstMenu.AddLabel("Sorun olursa forumdaki konuma bildirin");
            FirstMenu.AddLabel("İyi oyunlar");
            FirstMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");
            ComboMenu = FirstMenu.AddSubMenu("• Combo", ComboMenuID);
            HarassMenu = FirstMenu.AddSubMenu("• Harass", HarassMenuID);
            AutoHarassMenu = FirstMenu.AddSubMenu("• AutoHarass", AutoHarassMenuID);
            LaneClearMenu = FirstMenu.AddSubMenu("• LaneClear", LaneClearMenuID);
            //LasthitMenu = FirstMenu.AddSubMenu("• LastHit", LastHitMenuID);
            JungleClearMenu = FirstMenu.AddSubMenu("• JungleClear", JungleClearMenuID);
            //KillStealMenu = FirstMenu.AddSubMenu("• KillSteal", KillStealMenuID);
            MiscMenu = FirstMenu.AddSubMenu("• Misc", MiscMenuID);
            DrawingsMenu = FirstMenu.AddSubMenu("• Drawings", DrawingsMenuID);

            ComboMenu.AddGroupLabel("Combo");
            ComboMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");
            ComboMenu.Add("gankc", new KeyBind("Gank Combo", false, KeyBind.BindTypes.HoldActive, 'T'));
            ComboMenu.CreateCheckBox(" - Use Q", "qUse");
            ComboMenu.AddLabel("W Spells");
            ComboMenu.CreateCheckBox(" - Her zaman W Kullan", "wUse", true);
            ComboMenu.CreateCheckBox(" - W sadece düşman menzildeyse ", "wrUse", false);
            ComboMenu.CreateCheckBox(" - E KUllan", "eUse");
            ComboMenu.CreateCheckBox(" - Kullan R", "rUse");
            ComboMenu.AddLabel("Use ultimate on");
            foreach (var enemies in EntityManager.Heroes.Enemies.Where(i => !i.IsMe))
            {
                ComboMenu.Add("r.ult" + enemies.ChampionName, new CheckBox("" + enemies.ChampionName, false));
            }
            ComboMenu.AddSeparator(10);
            ComboMenu.Add("manu.ult", new CheckBox("Use R Manual", false));
            ComboMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");

            HarassMenu.AddGroupLabel("Dürtme");
            HarassMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");
            HarassMenu.CreateCheckBox(" - Kullan E", "eUse");
            HarassMenu.AddGroupLabel("Ayarları");
            HarassMenu.CreateSlider("Dürtme büyüleri için gereken mana", "manaSlider", 30);
            HarassMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");

            AutoHarassMenu.AddGroupLabel("Otomatik Dürtme");
            AutoHarassMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");
            AutoHarassMenu.CreateCheckBox(" - Kullan E", "eUse", false);
            AutoHarassMenu.AddGroupLabel("Ayarları");
            AutoHarassMenu.CreateSlider("Dürtme için gereken mana", "manaSlider", 80);
            AutoHarassMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");

            LaneClearMenu.AddGroupLabel("Lanetemizleme");
            LaneClearMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");
            LaneClearMenu.CreateCheckBox(" - Kullan Q", "qUse");
            LaneClearMenu.CreateCheckBox(" - Kullan E", "eUse");
            LaneClearMenu.AddGroupLabel("Ayarları");
            LaneClearMenu.CreateSlider("Gereken mana", "manaSlider", 30);
            LaneClearMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");

            JungleClearMenu.AddGroupLabel("JungleClear");
            JungleClearMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");
            JungleClearMenu.CreateCheckBox(" - Kullan Q", "qUse");
            JungleClearMenu.CreateCheckBox(" - Kullan W", "wUse");
            JungleClearMenu.CreateCheckBox(" - Kullan E", "eUse");
            JungleClearMenu.AddGroupLabel("Ayarları");
            JungleClearMenu.CreateSlider("Gereken mana", "manaSlider", 20);
            JungleClearMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");

            //KillStealMenu.AddGroupLabel("KilLSteal");
            //KillStealMenu.CreateCheckBox(" - Use Q", "qUse");
            //KillStealMenu.CreateCheckBox(" - Use E", "eUse");
            //KillStealMenu.AddGroupLabel("Settings");
            //KillStealMenu.CreateSlider("Mana must be lower than [{0}%] to use Killsteal spells", "manaSlider", 50);

            MiscMenu.AddGroupLabel("Skin Değiştrici");

            var skinList = Mario_s_Lib.DataBases.Skins.SkinsDB.FirstOrDefault(list => list.Champ == Player.Instance.Hero);
            if (skinList != null)
            {
                MiscMenu.CreateComboBox("Skinini seç", "skinComboBox", skinList.Skins);
                MiscMenu.Get<ComboBox>("skinComboBox").OnValueChange += delegate (ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
                {
                    Player.Instance.SetSkinId(sender.CurrentValue);
                };
            }


            MiscMenu.AddGroupLabel("Otomatik level arttırma");
            MiscMenu.CreateCheckBox("Aktif oto level", "activateAutoLVL", false);
            MiscMenu.AddLabel("Otomatik level her zaman R ye öncelik verir ondan sonrası aşağıda");
            MiscMenu.CreateComboBox("1.ci büyü", "firstFocus", new List<string> { "Q", "W", "E" });
            MiscMenu.CreateComboBox("2.ci büyü", "secondFocus", new List<string> { "Q", "W", "E" }, 1);
            MiscMenu.CreateComboBox("3.cü büyü", "thirdFocus", new List<string> { "Q", "W", "E" }, 2);
            MiscMenu.CreateSlider("Gecikme Ayarı", "delaySlider", 200, 150, 500);

            MiscMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");
            MiscMenu.CreateCheckBox("- W ile efsanevi kalkan logic", "wLogic", false);
            MiscMenu.AddSeparator(10);

            DrawingsMenu.AddGroupLabel("Gösterge Ayarları");
            DrawingsMenu.CreateCheckBox(" - Sadece hazırsa göster.", "readyDraw");
            DrawingsMenu.CreateCheckBox(" - Hasar tespitçisi göster.", "damageDraw");
            DrawingsMenu.CreateCheckBox(" - Hasar tespitini yüzde olarak göster.", "perDraw");
            DrawingsMenu.CreateCheckBox(" - Hasar istatistiklerini göster.", "statDraw", false);
            DrawingsMenu.AddGroupLabel("Göstergeler");
            DrawingsMenu.CreateCheckBox(" - Göster Q.", "qDraw");
            DrawingsMenu.CreateCheckBox(" - Göster W.", "wDraw");
            DrawingsMenu.CreateCheckBox(" - Göster E.", "eDraw");
            DrawingsMenu.CreateCheckBox(" - Göster R.", "rDraw");
            DrawingsMenu.AddGroupLabel("Gösterge Rengi");
            QColorSlide = new ColorSlide(DrawingsMenu, "qColor", Color.Red, "Q Rengi:");
            WColorSlide = new ColorSlide(DrawingsMenu, "wColor", Color.Purple, "W Rengi:");
            EColorSlide = new ColorSlide(DrawingsMenu, "eColor", Color.Orange, "E Rengi:");
            RColorSlide = new ColorSlide(DrawingsMenu, "rColor", Color.DeepPink, "R Rengi:");
            DamageIndicatorColorSlide = new ColorSlide(DrawingsMenu, "healthColor", Color.YellowGreen, "Hasar Tespitçisi rengi:");
        }
        public static bool BlockSpells
        {
            get { return MiscMenu.GetCheckBoxValue("wLogic"); }
        }
    }
}