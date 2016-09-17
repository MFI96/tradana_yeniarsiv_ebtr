using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System;
using System.IO;
using System.Media;
using System.Net;
using EloBuddy.SDK;
using EloBuddy.SDK.Constants;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using Mario_s_Lib;

namespace Ronin
{
    internal class Menus
    {
        public const string ComboMenuID = "combomenuid";
        public const string HarassMenuID = "harassmenuid";
        public const string AutoHarassMenuID = "autoharassmenuid";
        public const string LaneClearMenuID = "laneclearmenuid";
        public const string LastHitMenuID = "lasthitmenuid";
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
            FirstMenu.AddGroupLabel("Tarafından Taazuma / Kullandığın için teşekkürler");
            FirstMenu.AddLabel("Sorun varsa forumdan bana yazın");
            FirstMenu.AddLabel("İyi oyunlar");
            FirstMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");
            ComboMenu = FirstMenu.AddSubMenu("• Combo", ComboMenuID);
            HarassMenu = FirstMenu.AddSubMenu("• Harass", HarassMenuID);
            //AutoHarassMenu = FirstMenu.AddSubMenu("• AutoHarass", AutoHarassMenuID);
            LaneClearMenu = FirstMenu.AddSubMenu("• LaneClear", LaneClearMenuID);
            LasthitMenu = FirstMenu.AddSubMenu("• LastHit", LastHitMenuID);
            JungleClearMenu = FirstMenu.AddSubMenu("• JungleClear", JungleClearMenuID);
            KillStealMenu = FirstMenu.AddSubMenu("• KillSteal", KillStealMenuID);
            MiscMenu = FirstMenu.AddSubMenu("• Misc", MiscMenuID);
            DrawingsMenu = FirstMenu.AddSubMenu("• Drawings", DrawingsMenuID);

            ComboMenu.AddGroupLabel("Kombo");
			ComboMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");
            ComboMenu.CreateCheckBox(" - Kullan Q", "qUse");
            ComboMenu.CreateCheckBox(" - Kullan W", "wUse");
            ComboMenu.CreateCheckBox(" - Kullan E", "eUse");
            ComboMenu.CreateCheckBox(" - Kullan R", "rUse");
            ComboMenu.Add("hpR", new Slider("R kullanmak için % canım", 30));
            ComboMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");

            HarassMenu.AddGroupLabel("Dürtme");
			HarassMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");
            HarassMenu.CreateCheckBox(" - Kullan W", "wUse");
            HarassMenu.CreateCheckBox(" - Kullan E", "eUse");
            HarassMenu.AddGroupLabel("Ayarları");
            HarassMenu.CreateSlider("Manam şundan azsa kullanma %", "manaSlider", 30);
			HarassMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");

            LaneClearMenu.AddGroupLabel("Lanetemizleme");
			LaneClearMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");
            LaneClearMenu.CreateCheckBox(" - Kullan Q", "qUse", false);
            LaneClearMenu.CreateCheckBox(" - Kullan E", "eUse");
            LaneClearMenu.AddGroupLabel("Ayarları");
            LaneClearMenu.CreateSlider("Manam şundan azsa kullanma %", "manaSlider", 20);
			LaneClearMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");

            LasthitMenu.AddGroupLabel("Sonvuruş");
            LasthitMenu.CreateCheckBox(" - Kullan Q", "qUse", true);
            LasthitMenu.AddGroupLabel("Ayarları");
            LasthitMenu.CreateSlider("Manam şundan azsa kullanma %", "manaSlider", 30);

            JungleClearMenu.AddGroupLabel("OrmanTemizleme");
			JungleClearMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");
            JungleClearMenu.CreateCheckBox(" - Kullan Q", "qUse");
            JungleClearMenu.CreateCheckBox(" - Kullan W", "wUse");
            JungleClearMenu.CreateCheckBox(" - Kullan E", "eUse");
            JungleClearMenu.AddGroupLabel("Ayarları");
            JungleClearMenu.CreateSlider("Manam şundan azsa kullanma %", "manaSlider", 15);
			JungleClearMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");

            MiscMenu.AddGroupLabel("Skin Değiştrici");
            
            var skinList = Mario_s_Lib.DataBases.Skins.SkinsDB.FirstOrDefault(list => list.Champ == Player.Instance.Hero);
            if (skinList != null)
            {
                MiscMenu.CreateComboBox("Skinini Seç", "skinComboBox", skinList.Skins);
                MiscMenu.Get<ComboBox>("skinComboBox").OnValueChange += delegate(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
                {
                    Player.Instance.SetSkinId(sender.CurrentValue);
                };
            }

            MiscMenu.AddGroupLabel("Otomatik Level Arttırma");
            MiscMenu.CreateCheckBox("Aktif Otomatik Level Arttırma", "activateAutoLVL", false);
            MiscMenu.AddLabel("her zaman R öncelik verir gerisini ayarla");
            MiscMenu.CreateComboBox("1.ci öncelik büyü", "firstFocus", new List<string> {"Q", "W", "E"});
            MiscMenu.CreateComboBox("2.ci öncelik büyü", "secondFocus", new List<string> {"Q", "W", "E"}, 1);
            MiscMenu.CreateComboBox("3.cü öncelik büyü", "thirdFocus", new List<string> {"Q", "W", "E"}, 2);
            MiscMenu.CreateSlider("Gecikme Ayarı ms", "delaySlider", 200, 150, 500);

            DrawingsMenu.AddGroupLabel("Ayarlar");
            DrawingsMenu.CreateCheckBox(" - Sadece hazır büyüleri göster.", "readyDraw");
            DrawingsMenu.CreateCheckBox(" - Hasar tespitçisi göster.", "damageDraw");
            DrawingsMenu.CreateCheckBox(" - Hasarı yüzde olarak göster.", "perDraw");
            DrawingsMenu.CreateCheckBox(" - Hasar istatistiklerini göster.", "statDraw", false);
            DrawingsMenu.AddGroupLabel("Büyüler");
            DrawingsMenu.CreateCheckBox(" - göster Q.", "qDraw");
            DrawingsMenu.CreateCheckBox(" - göster W.", "wDraw");
            DrawingsMenu.CreateCheckBox(" - göster E.", "eDraw");
            DrawingsMenu.CreateCheckBox(" - göster R.", "rDraw");
            DrawingsMenu.AddGroupLabel("Gösterge Rengi");
            QColorSlide = new ColorSlide(DrawingsMenu, "qColor", Color.Red, "Q Rengi:");
            WColorSlide = new ColorSlide(DrawingsMenu, "wColor", Color.Purple, "W Rengi:");
            EColorSlide = new ColorSlide(DrawingsMenu, "eColor", Color.Orange, "E Rengi:");
            RColorSlide = new ColorSlide(DrawingsMenu, "rColor", Color.DeepPink, "R Rengi:");
            DamageIndicatorColorSlide = new ColorSlide(DrawingsMenu, "healthColor", Color.YellowGreen, "Hasartespitçisi Rengi:");
        }
    }
}