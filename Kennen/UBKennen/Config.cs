using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;


namespace UBKennen
{
    public class Config
    {     
        public static Menu Menu { get; private set; }
        public static Menu ComboMenu { get; private set; }
        public static Menu HarassMenu { get; private set; }
        public static Menu LaneClear { get; private set; }
        public static Menu JungleClear { get; private set; }
        public static Menu LasthitMenu { get; private set; }
        public static Menu MiscMenu { get; private set; }
        public static Menu DrawMenu { get; private set; }     

        public static void Dattenosa()
        {
            // Menu
            Menu = MainMenu.AddMenu("UB Kennen", "UBKennen");          
            Menu.AddGroupLabel("Made by Uzumaki Boruto");
            Menu.AddLabel("Dattenosa");

            //ComboMenu
            ComboMenu = Menu.AddSubMenu("Combo");
            {
                ComboMenu.AddGroupLabel("Combo Ayarları");
                ComboMenu.Add("useQCombo", new CheckBox("Kullan Q"));
                ComboMenu.Add("useWCombo", new CheckBox("Kullan W"));
                ComboMenu.Add("WHitCombo", new Slider("W kullanmak için gereken düşman", 1, 1, 5));
                ComboMenu.Add("useECombo", new CheckBox("Kullan E", false));
                ComboMenu.Add("useRCombo", new CheckBox("Kullan R"));
                ComboMenu.Add("RHitCombo", new Slider("R Kullanalabilecek düşman sayısı", 2, 1, 5));
                ComboMenu.Add("sep", new Separator());
                ComboMenu.AddLabel("Büyü Kullan");
                ComboMenu.Add("useIg", new CheckBox("Tutuştur"));
                ComboMenu.Add("useheal", new CheckBox("Can"));
                ComboMenu.Add("manageheal", new Slider("Canım şu kadarsa kullan", 15, 1, 80));
                ComboMenu.Add("usehealally", new CheckBox("Dostlara can kullan"));
                ComboMenu.Add("managehealally", new Slider("Can kullanmak için dostlarımın canı :)", 10, 1, 80));
            }

            //HarassMenu
            HarassMenu = Menu.AddSubMenu("Harass");
            {
                HarassMenu.AddGroupLabel("Dürtme Ayarları");
                HarassMenu.Add("useQ", new CheckBox("Kullan Q"));
                HarassMenu.Add("useW", new CheckBox("Kullan W"));
                HarassMenu.Add("HrEnergyManager", new Slider("Dürtmeyi durdurmak için enerji", 0, 0, 200));
            }

            //LaneJungleClear Menu
            LaneClear = Menu.AddSubMenu("LaneClear");
            {
                LaneClear.AddGroupLabel("Laneclear Ayarları");
                LaneClear.Add("useQLc", new CheckBox("Q Kullan", false));
                LaneClear.Add("useWLc", new CheckBox("W Kullan", false));
                LaneClear.Add("WHitLc", new Slider("W şu kadar minyona çarpacaksa", 5, 1, 30));
                LaneClear.Add("useELc", new CheckBox("E Kullan", false));
                LaneClear.Add("sep4", new Separator());
                LaneClear.Add("EnergyManager", new Slider("Büyü kullanmak için enerjim şu kadar olsun", 0, 0, 200));
                LaneClear.Add("sep5", new Separator(40));
            }
            //JungleClear Menu
            JungleClear =Menu.AddSubMenu("JungleClear");
            {
                JungleClear.AddGroupLabel("OrmanTemizleme Ayarları");
                JungleClear.Add("useQJc", new CheckBox("Q Kullan"));
                JungleClear.Add("useWJc", new CheckBox("W Kullan"));
                JungleClear.Add("WHitJc", new Slider("W şu kadar canavara çarpacaksa", 2, 1, 4));
                JungleClear.Add("useEJc", new CheckBox("E Kullan", false));
                JungleClear.Add("JcEnergyManager", new Slider("Büyü kullanmak için enerjim şu kadar olsun", 0, 0, 200));
            }

            //LasthitMenu
            LasthitMenu = Menu.AddSubMenu("Lasthit");
            {
                LasthitMenu.Add("useQLh", new CheckBox("Q Kullan"));
                LasthitMenu.Add("useWLh", new CheckBox("W Kullan"));
            }

            //DrawMenu
            DrawMenu = Menu.AddSubMenu("Drawings");
            {
                DrawMenu.Add("drawQ", new CheckBox("Göster Q", false));
                DrawMenu.Add("drawW", new CheckBox("Göster W", false));
                DrawMenu.Add("drawR", new CheckBox("Göster R", false));
            }

            //MiscMenu          
            MiscMenu = Menu.AddSubMenu("MiscMenu");
            {
                MiscMenu.AddGroupLabel("Ek Ayarları");
                MiscMenu.AddLabel("Anti Gapcloser");
                MiscMenu.Add("useQAG", new CheckBox("anti GapCloser Q"));
                MiscMenu.Add("useWAG", new CheckBox("anti GapCloser W"));
                MiscMenu.Add("useEAG", new CheckBox("anti GapCloser E"));

                MiscMenu.AddLabel("Killçalma Ayarları");
                MiscMenu.Add("useQKS", new CheckBox("Q Kullan"));
                MiscMenu.Add("useWKS", new CheckBox("W Kullan"));

                MiscMenu.AddLabel("İtem Aktifleştirici");
                MiscMenu.Add("item.1", new CheckBox("Otomatik Kullan bilgewater palası"));
                MiscMenu.Add("item.1MyHp", new Slider("Benim canım şundan az {0}%", 95));
                MiscMenu.Add("item.1EnemyHp", new Slider("Düşman canı şundan az {0}%", 70));
                MiscMenu.Add("item.sep", new Separator());

                MiscMenu.Add("item.2", new CheckBox("Otomatik Kullan Mahvolmuş"));
                MiscMenu.Add("item.2MyHp", new Slider("benim canım şundan az {0}%", 80));
                MiscMenu.Add("item.2EnemyHp", new Slider("Düimanın canı şundan az {0}%", 70));
                MiscMenu.Add("sep7", new Separator());

                MiscMenu.Add("item.3", new CheckBox("Otomatik Zhonya"));
                MiscMenu.Add("item.3MyHp", new Slider("Benim Canım {0}%", 50));
                MiscMenu.Add("sep8", new Separator());

                MiscMenu.Add("item.4", new CheckBox("R den sonra hemen Zhonya Kullan"));
                MiscMenu.Add("item.4mng", new Slider("Şu kadar düşmana vuracaksa", 3, 1, 5));
                MiscMenu.Add("sep9", new Separator());

                MiscMenu.AddLabel("Mod Skin");
                MiscMenu.Add("Modskin", new CheckBox("Aktif mod skin"));
                MiscMenu.Add("Modskinid", new Slider("Mod Skin", 6, 0, 6));
            }         
        }                     
    }
}     
