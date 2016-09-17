using System.Drawing;
using EloBuddy.SDK;
using EloBuddy.SDK.Notifications;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace UBRyze
{
    class Config
    {
        public static Menu Menu;
        public static Menu ComboMenu;
        public static Menu HarassMenu;
        public static Menu LaneClear;
        public static Menu JungleClear;
        public static Menu LasthitMenu;
        public static Menu AutoMenu;
        public static Menu MiscMenu;
        public static Menu DrawMenu;
        public static ComboBox AutoBox;

        public static void Dattenosa()
        {
            // Menu
            Menu = MainMenu.AddMenu("UB Ryze", "UBRyze");
            Menu.AddGroupLabel("Uzumaki Boruto tarafindan yapılmıştır");
            Menu.AddGroupLabel("tradana tarafindan çevrilmiştir.");
            Menu.AddLabel("Dattenosa");


            ComboMenu = Menu.AddSubMenu("Combo");
            {
                ComboMenu.Add("useQcb", new CheckBox("Q Kullan"));
                ComboMenu.Add("useWcb", new CheckBox("W Kullan"));
                ComboMenu.Add("useEcb", new CheckBox("E Kullan"));
                ComboMenu.Add("useRcb", new CheckBox("R Kullan", false));
                ComboMenu.AddSeparator();
                ComboMenu.Add("combostyle", new ComboBox("Kombo Stili", 2, "Full Damage", "Shield & Movement speed", "Smart"));
                ComboMenu.Add("hpcbsmart", new Slider("Eğer canım şunun altındaysa {0}% Q kullanarak Kalkan Olsun (Akıllı)", 30));
                ComboMenu.Add("disatt", new CheckBox("Otomatik Atak Devredışı", false));
                ComboMenu.Add("logicdisatt", new CheckBox("Düz Vuruş Devredışı"));
                ComboMenu.AddSeparator();
                ComboMenu.Add("useflee", new CheckBox("Kaçarken Komboya İzin Ver"));
                ComboMenu.AddLabel("Normalde Kaçarken Sadece Q Kullanır");
                ComboMenu.AddLabel("Aktif Ederseniz Sadece Kaçar");

            }

            HarassMenu = Menu.AddSubMenu("Harass");
            {
                HarassMenu.Add("useQhr", new CheckBox("Q Kullan"));
                HarassMenu.Add("useWhr", new CheckBox("W Kullan"));
                HarassMenu.Add("useEhr", new CheckBox("E Kullan"));
                //HarassMenu.Add("useQEhr", new CheckBox("Use E on minion that colision in Q Width"));
                HarassMenu.Add("hrmanage", new Slider("Eğer canım şunun altındaysa {0}% Dürtmeyi durdur", 50));
                var HarassKey = HarassMenu.Add("keyharass", new KeyBind("Otomatik Dürtme", false, KeyBind.BindTypes.PressToggle, 'Z'));
                HarassKey.OnValueChange += delegate
                {
                    var On = new SimpleNotification("Otomatik Dürtme Durumu:", "Aktif. ");
                    var Off = new SimpleNotification("Otomatik Dürtme Durumu:", "Pasif. ");

                    Notifications.Show(HarassKey.CurrentValue ? On : Off, 2000);
                };
                HarassMenu.Add("autohrmng", new Slider("Dürtmeyi durdur eğer canım şunun altındaysa {0}%", 80));
            }

            LaneClear = Menu.AddSubMenu("LaneClear");
            {
                LaneClear.Add("useQlc", new CheckBox("Q Kullan", false));
                LaneClear.Add("useWlc", new CheckBox("W Kullan", false));
                LaneClear.Add("useElc", new CheckBox("E Kullan", false));
                LaneClear.Add("lcmanage", new Slider("Eğer manam şunun altındaysa Büyü Kullanma", 50));
                LaneClear.Add("logiclc", new CheckBox("Akıllı Lanetemizleme Kullan[Deneme]", false));
                LaneClear.AddGroupLabel("AkıllıLanetemizleme");
                LaneClear.Add("Qlc", new Slider("Sadece şu kadar minyona E Kullan", 5, 1, 15));
                LaneClear.Add("Elc", new CheckBox("E atma Mantığı Aktif"));
            }

            JungleClear = Menu.AddSubMenu("JungClear");
            {
                JungleClear.Add("useQjc", new CheckBox("Q Kullan"));
                JungleClear.Add("onlyQjc", new CheckBox("Sadece Q Kullan"));
                JungleClear.Add("useWjc", new CheckBox("W Kullan"));
                JungleClear.Add("useEjc", new CheckBox("E Kullan"));
                JungleClear.Add("jcmanage", new Slider("Eğer manam şunun altında düşerse büyü Kullanma", 50));
            }

            LasthitMenu = Menu.AddSubMenu("Lasthit");
            {
                LasthitMenu.AddLabel("Bu büyüler sadece minyon ölecekse Kullanılır");
                LasthitMenu.Add("useQlh", new CheckBox("Q Kullan"));
                LasthitMenu.Add("useWlh", new CheckBox("W Kullan"));
                LasthitMenu.Add("useElh", new CheckBox("E Kullan"));
                LasthitMenu.Add("lhmanage", new Slider("Manam şunun altındayken Büyü Kullanma", 50));
                LasthitMenu.AddGroupLabel("Ölmeyecek minyonlara Büyü Kullan");
                LasthitMenu.Add("Qlh", new CheckBox("Q Kullan"));
                LasthitMenu.Add("Wlh", new CheckBox("W Kullan"));
                LasthitMenu.Add("Elh", new CheckBox("E Kullan"));
                LasthitMenu.Add("unkillmanage", new Slider("Manam şunun altındaysa KUllanma {0}%", 15));
            }

            AutoMenu = Menu.AddSubMenu("AutoMenu");
            {
                AutoBox = AutoMenu.Add("autofl", new ComboBox("Flashtan Sonra Otomatik W", 1, "None", "W", "E + W"));
                AutoBox.OnValueChange += AutoBox_OnValueChange;
                //AutoMenu.AddSeparator();
                //AutoMenu.Add("Rzh", new Slider("Auto use Zhonya & R to your nearesr Nexus if around you >= {0}", 5, 1, 6));
                //AutoMenu.Add("Rzhe", new Slider("Get enemy around you {0}0 distance", 50, 0, 150));
                //AutoMenu.Add("Rzha", new Slider("Get ally around you {0}0 distance", 100, 0, 150));
                //AutoMenu.AddLabel("This mean if x enemy around you and no ally around, you will R into nearest Turret");
            }

            MiscMenu = Menu.AddSubMenu("Ek");
            {
                MiscMenu.AddGroupLabel("Killçalma");
                MiscMenu.Add("Qks", new CheckBox("Q ile Çal"));
                MiscMenu.Add("Wks", new CheckBox("W ile Çal"));
                MiscMenu.Add("Eks", new CheckBox("E ile çal"));
                MiscMenu.AddGroupLabel("Hasar Tespitçisi");
                MiscMenu.Add("dmg", new ComboBox("Nasıl Hesaplansın?", 0, "Basic Combo(QWE)", "Highest Damage you can take"));
                MiscMenu.Add("gapcloser", new CheckBox("Rakibe Yaklaşma/Uzaklaşma için W"));

            }

            DrawMenu = Menu.AddSubMenu("Drawing");
            {
                DrawMenu.Add("draw", new CheckBox("Göstergeler Aktif"));
                DrawMenu.Add("drQ", new CheckBox("Q Göster"));
                DrawMenu.Add("drW", new CheckBox("W+E Göster"));
                DrawMenu.Add("drR", new CheckBox("R Göster"));
                DrawMenu.Add("drdamage", new CheckBox("Hasar Tespitçisi"));
                DrawMenu.Add("Color", new ColorPicker("Hasar Tespitçisi Rengi", Color.FromArgb(255, 255, 236, 0)));
            }
        }

        static void AutoBox_OnValueChange(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
        {
            switch (args.NewValue)
            {
                case 0:
                    {
                        AutoBox.DisplayName = "You turn off flash auto";
                    }
                    break;
                case 1:
                    {
                        AutoBox.DisplayName = "Auto use W after flash";
                    }
                    break;
                case 2:
                    {
                        AutoBox.DisplayName = "Auto use E + W after flash";
                    }
                    break;
            }
        }

    }
}
