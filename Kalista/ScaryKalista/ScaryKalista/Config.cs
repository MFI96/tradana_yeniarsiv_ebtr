using System.Linq;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace ScaryKalista
{
    public class Config
    {
        public static Menu Menu { get; private set; }
        public static Menu ComboMenu { get; private set; }
        public static Menu HarassMenu { get; private set; }
        public static Menu LaneMenu { get; private set; }
        public static Menu JungleMenu { get; private set; }
        public static Menu FleeMenu { get; private set; }
        public static Menu SentinelMenu { get; private set; }
        public static Menu MiscMenu { get; private set; }
        public static Menu DrawMenu { get; private set; }
        public static Menu BalistaMenu { get; private set; }
        public static Menu ItemMenu { get; private set; }

        public static void Initialize()
        {
            var blitzcrank = EntityManager.Heroes.Allies.Any(x => x.ChampionName == "Blitzcrank");

            //Initialize the menu
            Menu = MainMenu.AddMenu("Scary Kalista", "ScaryKalista");
            Menu.AddGroupLabel("Scary Kalistaya hoşgeldin!");
            Menu.AddLabel("Çeviri TRAdana");

            //Combo
            ComboMenu = Menu.AddSubMenu("Kombo");
            {
                ComboMenu.Add("combo.useQ", new CheckBox("Kullan Q"));
                ComboMenu.Add("combo.minManaQ", new Slider("Q için en az mana", 40));

                ComboMenu.Add("combo.sep1", new Separator());
                ComboMenu.Add("combo.useE", new CheckBox("E ile otomatik hedef öldür"));
                ComboMenu.Add("combo.gapClose", new CheckBox("Minyonlara orman moblarına vura vura git"));

                ComboMenu.Add("combo.sep2", new Separator());
                ComboMenu.Add("combo.harassEnemyE", new CheckBox("Minyonları öldürerek hedefi dürt", false));
            }

            //Harass
            HarassMenu = Menu.AddSubMenu("Harass");
            {
                HarassMenu.Add("harass.useQ", new CheckBox("Kullan Q"));
                HarassMenu.Add("harass.minManaQ", new Slider("Q için en az mana", 60));

                HarassMenu.Add("harass.sep1", new Separator());
                HarassMenu.Add("harass.harassEnemyE", new CheckBox("Minyonları öldürerek hedefi dürt"));
            }

            //LaneClear
            LaneMenu = Menu.AddSubMenu("LaneClear");
            {
                LaneMenu.Add("laneclear.useQ", new CheckBox("Kullan Q"));
                LaneMenu.Add("laneclear.minQ", new Slider("Q için minyon say", 3, 2, 10));
                LaneMenu.Add("laneclear.minManaQ", new Slider("Q için en az mana", 30));

                LaneMenu.Add("laneclear.sep1", new Separator());
                LaneMenu.Add("laneclear.useE", new CheckBox("Kullan E"));
                LaneMenu.Add("laneclear.minE", new Slider("E için minyon say", 3, 2, 10));
                LaneMenu.Add("laneclear.minManaE", new Slider("E için en az mana", 30));

                LaneMenu.Add("laneclear.sep2", new Separator());
                LaneMenu.Add("laneclear.harassEnemyE", new CheckBox("Minyonları E ile öldürerek hedefleri dürt"));
            }

            //JungleClear
            JungleMenu = Menu.AddSubMenu("JungleClear");
            {
                JungleMenu.Add("jungleclear.useE", new CheckBox("E ile mobları çal"));
                JungleMenu.Add("jungleclear.miniE", new CheckBox("E ile küçük mobları çal", false));
            }
            
            //Flee
            FleeMenu = Menu.AddSubMenu("Flee");
            {
                FleeMenu.Add("flee.attack", new CheckBox("Minyonlara vura vura Kaç(Canavar,şamp,minyon)"));
                FleeMenu.Add("flee.useJump", new CheckBox("Zıplama noktalarına geldiğim anda Q kullanarak atla"));
            }

            //Sentinel
            SentinelMenu = Menu.AddSubMenu("Sentinel (W)");
            {
                SentinelMenu.Add("sentinel.castDragon", new KeyBind("Ejdere Gözcü Yolla", false, KeyBind.BindTypes.HoldActive, 'U'));
                SentinelMenu.Add("sentinel.castBaron", new KeyBind("Barona gözcü yolla", false, KeyBind.BindTypes.HoldActive, 'I'));

                SentinelMenu.Add("sentinel.sep1", new Separator());
                SentinelMenu.Add("sentinel.enable", new CheckBox("Gözcüyü otomatik gönder", false));
                SentinelMenu.Add("sentinel.noMode", new CheckBox("Sadece hiçbir modda değilken (örn:komboda değilken)"));
                SentinelMenu.Add("sentinel.alert", new CheckBox("Gözcü Hasar aldığında uyar"));
                SentinelMenu.Add("sentinel.mana", new Slider("Gözcü yollamak için en az mana", 40));

                SentinelMenu.Add("sentinel.sep2", new Separator());
                SentinelMenu.Add("sentinel.locationLabel", new Label("Gözcü kullan şunlara:"));
                (SentinelMenu.Add("sentinel.baron", new CheckBox("Baron / Baronun Kız Kardeşi"))).OnValueChange += SentinelLocationsChanged;
                (SentinelMenu.Add("sentinel.dragon", new CheckBox("Ejder"))).OnValueChange += SentinelLocationsChanged;
                (SentinelMenu.Add("sentinel.mid", new CheckBox("Mide Yolla"))).OnValueChange += SentinelLocationsChanged;
                (SentinelMenu.Add("sentinel.blue", new CheckBox("Mavi"))).OnValueChange += SentinelLocationsChanged;
                (SentinelMenu.Add("sentinel.red", new CheckBox("Kırmızı"))).OnValueChange += SentinelLocationsChanged;
                Sentinel.RecalculateOpenLocations();
            }

            //Misc
            MiscMenu = Menu.AddSubMenu("Misc");
            {
                MiscMenu.Add("misc.labelSteal", new Label("Çalma: Hiçbir butona basma"));
                MiscMenu.Add("misc.killstealE", new CheckBox("E için Killçalma"));
                MiscMenu.Add("misc.junglestealE", new CheckBox("E için ormançalma"));

                MiscMenu.Add("misc.sep1", new Separator());
                MiscMenu.Add("misc.autoE", new CheckBox("Otomatik Kullan E"));
                MiscMenu.Add("misc.autoEHealth", new Slider("Canım şundan az olursa otomatik E çek", 10, 5, 25));

                MiscMenu.Add("misc.sep2", new Separator());
                MiscMenu.Add("misc.dmgReductionE", new Slider("Menzilden çıkan rakibin canını yüzde kaç azalacaksa E kullansın {0}%", 10, 0, 25));

                MiscMenu.Add("misc.sep3", new Separator());
                MiscMenu.Add("misc.unkillableE", new CheckBox("Öldürülemicek minyonlarda E kullan"));

                MiscMenu.Add("misc.sep4", new Separator());
                MiscMenu.Add("misc.useR", new CheckBox("R Dostlar için kullan"));
                MiscMenu.Add("misc.healthR", new Slider("{0}% Dostumun canı", 15, 5, 25));
            }

            //Items
            ItemMenu = Menu.AddSubMenu("Items");
            {
                var cutlass = Items.BilgewaterCutlass;
                ItemMenu.Add("item." + cutlass.ItemInfo.Name, new CheckBox("Kullan " + cutlass.ItemInfo.Name));
                ItemMenu.Add("item." + cutlass.ItemInfo.Name + "MyHp", new Slider("Senin canın şundan azsa {0}%", 80));
                ItemMenu.Add("item." + cutlass.ItemInfo.Name + "EnemyHp", new Slider("Düşmanın cnaı şundan azsa {0}%", 80));
                ItemMenu.Add("item.sep", new Separator());

                var bork = Items.BladeOfTheRuinedKing;
                ItemMenu.Add("item." + bork.ItemInfo.Name, new CheckBox("Kullan " + bork.ItemInfo.Name));
                ItemMenu.Add("item." + bork.ItemInfo.Name + "MyHp", new Slider("Senin canın şundan azsa {0}%", 80));
                ItemMenu.Add("item." + bork.ItemInfo.Name + "EnemyHp", new Slider("Düşmanın cnaı şundan azsa {0}%", 80));
            }

            //Balista
            if (blitzcrank)
            {
                BalistaMenu = Menu.AddSubMenu("Balista");
                {
                    BalistaMenu.Add("balista.use", new CheckBox("Kullan Balista"));

                    BalistaMenu.Add("balista.sep1", new Separator());
                    BalistaMenu.Add("balista.comboOnly", new CheckBox("Sadece kombo modunda balista kullan(spacebasılı"));
                    BalistaMenu.Add("balista.distance", new Slider("Blitzcharkla en az mesafem: {0}", 400, 0, 1200));

                    BalistaMenu.Add("balista.sep2", new Separator());
                    BalistaMenu.Add("balista.label", new Label("Balista için:"));
                    foreach (var enemy in EntityManager.Heroes.Enemies)
                    {
                        BalistaMenu.Add("balista." + enemy.ChampionName, new CheckBox(enemy.ChampionName));
                    }
                }
            }

            //Drawings
            DrawMenu = Menu.AddSubMenu("Drawings");
            {
                DrawMenu.Add("draw.Q", new CheckBox("Göster Q Menzili"));
                DrawMenu.Add("draw.W", new CheckBox("Göster W Menzili", false));
                DrawMenu.Add("draw.E", new CheckBox("Göster E Menzili"));
                DrawMenu.Add("draw.R", new CheckBox("Göster R Menzili"));
                DrawMenu.Add("draw.enemyE", new CheckBox("Düşmanın can barında E nin vereceği hasarı göster"));
                DrawMenu.Add("draw.percentage", new CheckBox("Düşmana vereceğin hasarı yüzdeyle göster"));
                DrawMenu.Add("draw.jungleE", new CheckBox("Ormanmoblarının canbarında Enin verebileceği hasarı göster"));
                DrawMenu.Add("draw.killableMinions", new CheckBox("E ile öldürülebilecek minyonları göster"));
                DrawMenu.Add("draw.stacks", new CheckBox("Düşman üzerinde kaç yük var göster", false));
                DrawMenu.Add("draw.jumpSpots", new CheckBox("Zıplama noktalarını sürekli göster"));
                if (blitzcrank) DrawMenu.Add("draw.balista", new CheckBox("Balista menzilini göster"));
            }
        }

        private static void SentinelLocationsChanged(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
        {
            Sentinel.RecalculateOpenLocations();
        }
    }
}
