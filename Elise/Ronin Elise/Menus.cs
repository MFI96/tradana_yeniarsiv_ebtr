using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using Mario_s_Lib;

namespace RoninElise
{
    internal class Menus
    {
        public const string ComboMenuID = "combomenuid";
        public const string HarassMenuID = "harassmenuid";
        //public const string AutoHarassMenuID = "autoharassmenuid";
        public const string LaneClearMenuID = "laneclearmenuid";
        //public const string LastHitMenuID = "lasthitmenuid";
        public const string JungleClearMenuID = "jungleclearmenuid";
        //public const string KillStealMenuID = "killstealmenuid";
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
            FirstMenu = MainMenu.AddMenu("Ronin " + Player.Instance.ChampionName, Player.Instance.ChampionName.ToLower() + "hue");
            ComboMenu = FirstMenu.AddSubMenu("• Combo", ComboMenuID);
            HarassMenu = FirstMenu.AddSubMenu("• Harass", HarassMenuID);
            //AutoHarassMenu = FirstMenu.AddSubMenu("• AutoHarass", AutoHarassMenuID);
            LaneClearMenu = FirstMenu.AddSubMenu("• LaneClear", LaneClearMenuID);
            //LasthitMenu = FirstMenu.AddSubMenu("• LastHit", LastHitMenuID);
            JungleClearMenu = FirstMenu.AddSubMenu("• JungleClear", JungleClearMenuID);
            //KillStealMenu = FirstMenu.AddSubMenu("• KillSteal", KillStealMenuID);
            MiscMenu = FirstMenu.AddSubMenu("• Misc", MiscMenuID);
            DrawingsMenu = FirstMenu.AddSubMenu("• Drawings", DrawingsMenuID);

            ComboMenu.AddGroupLabel("Combo");
            ComboMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");
            //ComboMenu.Add("gankc", new KeyBind("Gank Combo", false, KeyBind.BindTypes.HoldActive, 'T'));
            ComboMenu.CreateKeyBind("Enable/Disable GankCombo", "gankcombokey", 'T', 'Z');
            //Menu.Add("flashq", new KeyBind("FlashQ - Select Target (Hold)", false, KeyBind.BindTypes.HoldActive, 'T'));
            ComboMenu.AddSeparator(10);
            ComboMenu.AddLabel("İnsan Ayarları");
            ComboMenu.CreateCheckBox(" - Kullan Q", "qUse");
            ComboMenu.CreateCheckBox(" - Kullan W", "wUse");
            ComboMenu.CreateCheckBox(" - Kullan E", "eUse");
            ComboMenu.CreateCheckBox(" - Kullan R", "rUse");
            ComboMenu.AddLabel("Örümcek Ayarları");
            ComboMenu.CreateCheckBox(" - Ayarları Q2", "q2Use");
            ComboMenu.CreateCheckBox(" - Kullan W2", "w2Use");
            ComboMenu.CreateCheckBox(" - Kullan E2", "e2Use");
            ComboMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");

            HarassMenu.AddGroupLabel("Dürtme");
            HarassMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");
            HarassMenu.AddLabel("İnsan Ayarları");
            HarassMenu.CreateCheckBox(" - Kullan Q", "qUse");
            HarassMenu.CreateCheckBox(" - Kullan W", "wUse", false);
            HarassMenu.CreateCheckBox(" - Kullan E", "eUse");
            HarassMenu.AddLabel("Örümcek Ayarları");
            HarassMenu.CreateCheckBox(" - Kullan Q2", "q2Use");
            HarassMenu.CreateCheckBox(" - Kullan W2", "w2Use", false);
            HarassMenu.CreateCheckBox(" - Kullan E2", "e2Use", false);
            HarassMenu.AddGroupLabel("Ayarlar");
            HarassMenu.CreateSlider("Dürtme için gereken mana", "manaSlider", 30);
            HarassMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");

            LaneClearMenu.AddGroupLabel("Sonvuruş ve Lantemizleme");
            LaneClearMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");
            LaneClearMenu.CreateCheckBox(" - Kullan Q/Q2", "qUse");
            LaneClearMenu.AddGroupLabel("Ayarlar");
            LaneClearMenu.CreateSlider("Gereken mana", "manaSlider", 30);
            LaneClearMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");

            JungleClearMenu.AddGroupLabel("OrmanTemizleme");
            JungleClearMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");
            JungleClearMenu.AddLabel("İnsan Ayarları");
            JungleClearMenu.CreateCheckBox(" - Kullan Q", "qUse");
            JungleClearMenu.CreateCheckBox(" - Kullan W", "wUse");
            JungleClearMenu.CreateCheckBox(" - Kullan E", "eUse");
            JungleClearMenu.CreateCheckBox(" - Kullan R", "rUse");
            JungleClearMenu.AddLabel("Örümcek Ayarları");
            JungleClearMenu.CreateCheckBox(" - Kullan Q2", "q2Use");
            JungleClearMenu.CreateCheckBox(" - Kullan W2", "w2Use");
            JungleClearMenu.CreateCheckBox(" - Kullan E2", "e2Use");
            JungleClearMenu.AddGroupLabel("Ayarlar");
            JungleClearMenu.CreateSlider("Ormantemizleme için gereken mana", "manaSlider", 20);
            JungleClearMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");

            //KillStealMenu.AddGroupLabel("KS");
            //KillStealMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");
            //KillStealMenu.CreateCheckBox(" - Kullan Q", "qUse");
            //KillStealMenu.CreateCheckBox(" - Kullan W", "wUse");
            //KillStealMenu.CreateCheckBox(" - Kullan E", "eUse");
            //KillStealMenu.CreateCheckBox(" - Kullan R", "rUse");
            //KillStealMenu.AddGroupLabel("Ayarlar");
            //KillStealMenu.CreateSlider("Manam şundan fazla", "manaSlider", 30);
            //KillStealMenu.AddLabel("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬");

            MiscMenu.CreateCheckBox("- E kullan Antigapcloser için(betadır)", "eGap");
            MiscMenu.AddGroupLabel("Skin Değiştirici");
            
            var skinList = Mario_s_Lib.DataBases.Skins.SkinsDB.FirstOrDefault(list => list.Champ == Player.Instance.Hero);
            if (skinList != null)
            {
                MiscMenu.CreateComboBox("Kostümünüzü seçin", "skinComboBox", skinList.Skins);
                MiscMenu.Get<ComboBox>("skinComboBox").OnValueChange += delegate(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
                {
                    Player.Instance.SetSkinId(sender.CurrentValue);
                };
            }

            MiscMenu.AddGroupLabel("Otomatik level arttırma");
            MiscMenu.CreateCheckBox("Otomatik level", "activateAutoLVL", false);
            MiscMenu.AddLabel("Otomatik level herşeyden önce Rye öncelik verir gerisini ayarlayın");
            MiscMenu.CreateComboBox("1.ci büyü", "firstFocus", new List<string> {"Q", "W", "E"});
            MiscMenu.CreateComboBox("2.ci büyü", "secondFocus", new List<string> {"Q", "W", "E"}, 1);
            MiscMenu.CreateComboBox("3.cü büyü", "thirdFocus", new List<string> {"Q", "W", "E"}, 2);
            MiscMenu.CreateSlider("Gecikme Ayarı", "delaySlider", 200, 150, 500);

            DrawingsMenu.AddGroupLabel("Gösterge Ayarları");
            DrawingsMenu.CreateCheckBox(" - Sadece büyü hazırsa göster.", "readyDraw");
            DrawingsMenu.CreateCheckBox(" - Hasar tespitçisi göster.", "damageDraw");
            DrawingsMenu.CreateCheckBox(" - Yüzde olarak göster.", "perDraw");
            DrawingsMenu.CreateCheckBox(" - İstatistiklerini göster.", "statDraw", false);
            DrawingsMenu.AddGroupLabel("Büyülere gösterge");
            DrawingsMenu.CreateCheckBox(" - Göster Q.", "qDraw");
            DrawingsMenu.CreateCheckBox(" - Göster W.", "wDraw");
            DrawingsMenu.CreateCheckBox(" - Göster E.", "eDraw");
            DrawingsMenu.CreateCheckBox(" - Göster R.", "rDraw");
            DrawingsMenu.AddGroupLabel("Gösterge Rengi");
            QColorSlide = new ColorSlide(DrawingsMenu, "qColor", Color.Red, "Q Rengi:");
            WColorSlide = new ColorSlide(DrawingsMenu, "wColor", Color.Purple, "W Rengi:");
            EColorSlide = new ColorSlide(DrawingsMenu, "eColor", Color.Orange, "E Rengi:");
            RColorSlide = new ColorSlide(DrawingsMenu, "rColor", Color.DeepPink, "R Rengi:");
            DamageIndicatorColorSlide = new ColorSlide(DrawingsMenu, "healthColor", Color.YellowGreen, "Hasartespitçisi Rengi:");
        }
    }
}