namespace AutoSteal
{
    using System;

    using Genesis.Library;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    public class Program
    {
        public static Menu MenuIni;

        public static Menu Special;

        public static Menu KillStealMenu;

        public static Menu DrawMenu;

        public static Menu JungleStealMenu;

        public static AIHeroClient player = ObjectManager.Player;

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoad;
        }

        public static void OnLoad(EventArgs args)
        {
            var champion = ObjectManager.Player.ChampionName;
            MenuIni = MainMenu.AddMenu("Auto Steal ", "Auto Steal");

            KillStealMenu = MenuIni.AddSubMenu("Kill Steal ", "Kill Steal");
            KillStealMenu.AddGroupLabel("Kill Çalma Ayarları");
            KillStealMenu.Add(
                champion + "EnableKST",
                new KeyBind("Kill çalma aktif tuşu", true, KeyBind.BindTypes.PressToggle, 'M'));
            KillStealMenu.Add(
                champion + "EnableKSA",
                new KeyBind("Kill çalma aktif", false, KeyBind.BindTypes.HoldActive));
            KillStealMenu.AddSeparator();
            KillStealMenu.AddGroupLabel(champion + " Kill Çalma Büyüleri");
            KillStealMenu.Add(champion + "AAC", new CheckBox("Kullan AA "));
            KillStealMenu.Add(champion + "QC", new CheckBox("Kullan Q "));
            KillStealMenu.Add(champion + "WC", new CheckBox("Kullan W "));
            KillStealMenu.Add(champion + "EC", new CheckBox("Kullan E "));
            KillStealMenu.Add(champion + "RC", new CheckBox("Kullan R "));
            KillStealMenu.AddSeparator();
            KillStealMenu.AddGroupLabel("Şampiyon Seç");
            foreach (var enemy in ObjectManager.Get<AIHeroClient>())
            {
                var cb = new CheckBox(enemy.BaseSkinName) { CurrentValue = true };
                if (enemy.Team != Player.Instance.Team)
                {
                    KillStealMenu.Add(champion + "Steal" + enemy.BaseSkinName, cb);
                }
            }
            KillStealMenu.AddGroupLabel(champion + " Ek Ayarlar");
            KillStealMenu.AddSeparator();
            KillStealMenu.Add(champion + "all", new CheckBox("Tüm hazır büyülerin hasarını hesapla", false));

            JungleStealMenu = MenuIni.AddSubMenu("Jungle Steal ", "Jungle Steal");
            JungleStealMenu.AddGroupLabel("Orman Çalma Ayarları");
            JungleStealMenu.Add(
                champion + "EnableJST",
                new KeyBind("Orman Çalma Aktif Tuşu", true, KeyBind.BindTypes.PressToggle, 'M'));
            JungleStealMenu.Add(
                champion + "EnableJSA",
                new KeyBind("Orman Çalma Aktif", false, KeyBind.BindTypes.HoldActive));
            JungleStealMenu.AddSeparator();
            JungleStealMenu.AddGroupLabel(champion + " Orman Çalma Büyüleri");
            JungleStealMenu.Add(champion + "AAJ", new CheckBox("Kullan AA "));
            JungleStealMenu.Add(champion + "QJ", new CheckBox("Kullan Q "));
            JungleStealMenu.Add(champion + "WJ", new CheckBox("Kullan W "));
            JungleStealMenu.Add(champion + "EJ", new CheckBox("Kullan E "));
            JungleStealMenu.Add(champion + "RJ", new CheckBox("Kullan R "));
            JungleStealMenu.AddGroupLabel(champion + " Ek Ayarlar");
            JungleStealMenu.AddSeparator();
            JungleStealMenu.Add(champion + "all", new CheckBox("Tüm hazır büyülerin hasarını hesapla", false));
            JungleStealMenu.AddSeparator();
            JungleStealMenu.AddGroupLabel("Orman Canavalarını seç");
            JungleStealMenu.Add(champion + "blue", new CheckBox("Çal Mavi "));
            JungleStealMenu.Add(champion + "red", new CheckBox("Çal Kırmızı "));
            JungleStealMenu.Add(champion + "baron", new CheckBox("Çal Baron "));
            JungleStealMenu.Add(champion + "drake", new CheckBox("Çal Ejder "));
            JungleStealMenu.Add(champion + "gromp", new CheckBox("Çal Kurbağa "));
            JungleStealMenu.Add(champion + "krug", new CheckBox("Çal Golem "));
            JungleStealMenu.Add(champion + "razorbeak", new CheckBox("Çal Sivrigagalar "));
            JungleStealMenu.Add(champion + "crab", new CheckBox("Çal YampiriYengeç "));
            JungleStealMenu.Add(champion + "murkwolf", new CheckBox("Çal Alacakurtlar "));

            Special = MenuIni.AddSubMenu("Special Stealer ", "Special Stealer");
            Special.AddGroupLabel("Özel çalma ayarları");
            var asc = Special.Add(champion + "Ascension", new CheckBox("Yükseliş mobları çalma(baska harita)"));
            if (Game.Type != GameType.Ascension)
            {
                Special.AddLabel("Bu oyun modu özel :(");
                asc.IsVisible = false;
            }

            SpellManager.Initialize();
            SpellLibrary.Initialize();
            Game.OnUpdate += OnUpdate;
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (DrawMenu[Player.Instance.ChampionName + "debug"].Cast<CheckBox>().CurrentValue)
            {
                Modes.Draw.DebugKs();
                Modes.Draw.DebugJs();
            }
        }

        private static void OnUpdate(EventArgs args)
        {

            if (Player.Instance.IsRecalling()) return;
            var champion = ObjectManager.Player.ChampionName;
            if (KillStealMenu[champion + "EnableKST"].Cast<KeyBind>().CurrentValue
                || KillStealMenu[champion + "EnableKSA"].Cast<KeyBind>().CurrentValue)
            {
                Modes.KillSteal.Ks();
            }

            if (JungleStealMenu[champion + "EnableJST"].Cast<KeyBind>().CurrentValue
                || JungleStealMenu[champion + "EnableJSA"].Cast<KeyBind>().CurrentValue)
            {
                if (Game.Type == GameType.Ascension || Game.Type == GameType.Normal)
                {
                    Modes.JungleSteal.Js();
                }
            }
        }
    }
}