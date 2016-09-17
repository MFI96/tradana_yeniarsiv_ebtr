namespace KappAzir
{
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    using Utility;

    internal static class Menus
    {
        public static Menu Menuini, Auto, JumperMenu, ComboMenu, HarassMenu, LaneClearMenu, JungleClearMenu, KillstealMenu, DrawMenu, ColorMenu;

        public static void Execute()
        {
            Menuini = MainMenu.AddMenu("KappAzir", "KappAzir");
            Auto = Menuini.AddSubMenu("Auto Settings");
            JumperMenu = Menuini.AddSubMenu("Jumper Settings");
            ComboMenu = Menuini.AddSubMenu("Combo Settings");
            HarassMenu = Menuini.AddSubMenu("Harass Settings");
            LaneClearMenu = Menuini.AddSubMenu("LaneClear Settings");
            JungleClearMenu = Menuini.AddSubMenu("JungleClear Settings");
            KillstealMenu = Menuini.AddSubMenu("KillSteal Settings");
            DrawMenu = Menuini.AddSubMenu("Drawings Settings");
            ColorMenu = Menuini.AddSubMenu("ColorPicker");

            Auto.AddGroupLabel("Ayarlar");
            Auto.Add("gap", new CheckBox("Anti-GapCloser"));
            Auto.Add("int", new CheckBox("Interrupter"));
            Auto.Add("danger", new ComboBox("Interrupter DangerLevel", 1, "High", "Medium", "Low"));
            Auto.AddGroupLabel("Kule ayarları");
            Auto.Add("tower", new CheckBox("Kule kur"));
            Auto.Add("Tenemy", new Slider("Kule kurmak için yakındaki düşman sayısı", 3, 1, 6));
            Auto.AddGroupLabel("Anti GapCloser Büyüleri");
            foreach (var spell in
                from spell in Gapcloser.GapCloserList
                from enemy in EntityManager.Heroes.Enemies.Where(enemy => spell.ChampName == enemy.ChampionName)
                select spell)
            {
                Auto.Add(spell.SpellName, new CheckBox(spell.ChampName + " " + spell.SpellSlot));
            }

            if (EntityManager.Heroes.Enemies.Any(e => e.Hero == Champion.Rengar))
            {
                Auto.Add("rengar", new CheckBox("Rengar Sıçraması"));
            }

            JumperMenu.Add("jump", new KeyBind("WEQ Kaçma tuşu", false, KeyBind.BindTypes.HoldActive, 'A'));
            JumperMenu.Add("normal", new KeyBind("Normal Insec Tuşu", false, KeyBind.BindTypes.HoldActive, 'S'));
            JumperMenu.Add("new", new KeyBind("Yeni Insec Tuşu", false, KeyBind.BindTypes.HoldActive, 'Z'));
            JumperMenu.Add("flash", new CheckBox("Pasif Flash kombosu kullan"));
            JumperMenu.Add("delay", new Slider("Gecikme EQ", 200, 0, 500));
            JumperMenu.Add("range", new Slider("Asker menzilini kontrol et", 800, 0, 1000));

            ComboMenu.AddGroupLabel("Combo Ayarları");
            ComboMenu.Add("key", new KeyBind("Combo Tuşu", false, KeyBind.BindTypes.HoldActive, 32));
            ComboMenu.AddSeparator(0);
            ComboMenu.AddGroupLabel("Q Ayarları");
            ComboMenu.Add("Q", new CheckBox("Kullan Q"));
            ComboMenu.Add("WQ", new CheckBox("Kullan W > Q"));
            ComboMenu.Add("Qaoe", new CheckBox("Kullan Q Aoe", false));
            ComboMenu.Add("QS", new Slider("Askerlerle Q Kullan", 1, 1, 3));
            ComboMenu.AddSeparator(0);
            ComboMenu.AddGroupLabel("W Ayarları");
            ComboMenu.Add("W", new CheckBox("Kullan W"));
            ComboMenu.Add("Wsave", new CheckBox("1 yük sakla", false));
            ComboMenu.Add("WS", new Slider("Asker yaratma limitleyici", 3, 1, 3));
            ComboMenu.AddSeparator(0);
            ComboMenu.AddGroupLabel("E Ayarları");
            ComboMenu.Add("E", new CheckBox("Kullan E"));
            ComboMenu.Add("Ekill", new CheckBox("E Sadece hedef ölecekse"));
            ComboMenu.Add("Edive", new CheckBox("E Kuleye dalış", false));
            ComboMenu.Add("EHP", new Slider("Sadece benim canım şu kadar veya fazlaysa [{0}%]", 50));
            ComboMenu.Add("Esafe", new Slider("Şu kadar düşmanın arasına atlama E ile", 3, 1, 6));
            ComboMenu.AddSeparator(0);
            ComboMenu.AddGroupLabel("R Ayarları");
            ComboMenu.Add("R", new CheckBox("Kullan R"));
            ComboMenu.Add("Rkill", new CheckBox("R Bitirici"));
            ComboMenu.Add("insec", new CheckBox("İnsec kombosunda kullanmaya çalış"));
            ComboMenu.Add("Raoe", new Slider("R AoE Çarpacağı düşman sayısı", 3, 1, 6));
            ComboMenu.Add("Rsave", new CheckBox("R yi Sakla güvenliğim için"));
            ComboMenu.Add("RHP", new Slider("Hedefi itmek için benim kalan canım [{0}%]", 35));

            HarassMenu.AddGroupLabel("Dürtme Ayarları");
            HarassMenu.Add("key", new KeyBind("Dürtme tuşu", false, KeyBind.BindTypes.HoldActive, 'C'));
            HarassMenu.Add("toggle", new KeyBind("Otomatik dürtme", false, KeyBind.BindTypes.PressToggle, 'H'));
            HarassMenu.AddSeparator(0);
            HarassMenu.AddGroupLabel("Q Ayarları");
            HarassMenu.Add("Q", new CheckBox("Kullan Q"));
            HarassMenu.Add("WQ", new CheckBox("Kullan W > Q"));
            HarassMenu.Add("QS", new Slider("Askerler Q kullansın", 1, 1, 3));
            HarassMenu.Add("Qmana", new Slider("Q kullanması için gereken manam < [{0}%]", 65));
            HarassMenu.AddSeparator(0);
            HarassMenu.AddGroupLabel("W Ayarları");
            HarassMenu.Add("W", new CheckBox("Kullan W"));
            HarassMenu.Add("Wsave", new CheckBox("1 yük sakla"));
            HarassMenu.Add("WS", new Slider("Asker çıkarma limitleyici", 3, 1, 3));
            HarassMenu.Add("Wmana", new Slider("W kullanmayı durdur şu kadar mana varsa < [{0}%]", 65));
            HarassMenu.AddSeparator(0);
            HarassMenu.AddGroupLabel("E Ayarları");
            HarassMenu.Add("E", new CheckBox("Kullan E"));
            HarassMenu.Add("Edive", new CheckBox("E ile Kuleye dal", false));
            HarassMenu.Add("EHP", new Slider("Sadece benim canım şu kadar veya fazlaysa [{0}%]", 50));
            HarassMenu.Add("Esafe", new Slider("Şu kadar düşman arasına E KUllanma", 3, 1, 6));
            HarassMenu.Add("Emana", new Slider("E kullanmak için manam şundan çok [{0}%]", 65));

            LaneClearMenu.AddGroupLabel("Lanetemizleme Ayarları");
            LaneClearMenu.Add("key", new KeyBind("Lanetemizleme Tuşu", false, KeyBind.BindTypes.HoldActive, 'V'));
            LaneClearMenu.Add("Q", new CheckBox("Kullan Q"));
            LaneClearMenu.Add("Qmana", new Slider("Q kullanmayı durdur manam şundan azsa < [{0}%]", 65));
            LaneClearMenu.Add("W", new CheckBox("Kullan W"));
            LaneClearMenu.Add("Wsave", new CheckBox("1 yük sakla"));
            LaneClearMenu.Add("Wmana", new Slider("W için gereken manam  [{0}%]", 65));

            JungleClearMenu.AddGroupLabel("Ormantemizleme Kullan");
            JungleClearMenu.Add("key", new KeyBind("Ormantemizleme Tuşu", false, KeyBind.BindTypes.HoldActive, 'V'));
            JungleClearMenu.Add("Q", new CheckBox("Kullan Q"));
            JungleClearMenu.Add("Qmana", new Slider("Q kullanmak için gerekli mana  [{0}%]", 65));
            JungleClearMenu.Add("W", new CheckBox("Kullan W"));
            JungleClearMenu.Add("Wsave", new CheckBox("1 yük sakla"));
            JungleClearMenu.Add("Wmana", new Slider("W kullanmak için en az mana < [{0}%]", 65));

            KillstealMenu.AddGroupLabel("Killçalma ayarları");
            KillstealMenu.Add("Q", new CheckBox("Kullan Q"));
            KillstealMenu.Add("E", new CheckBox("Kullan E"));
            KillstealMenu.Add("R", new CheckBox("Kullan R"));

            foreach (var spell in Azir.SpellList)
            {
                DrawMenu.Add(spell.Slot.ToString(), new CheckBox(spell.Slot + " Range"));
                ColorMenu.Add(spell.Slot.ToString(), new ColorPicker(spell.Slot + " Color", System.Drawing.Color.Chartreuse));
            }

            DrawMenu.Add("insec", new CheckBox("Insec yardımcısını göster"));
        }

        public static int combobox(this Menu m, string id)
        {
            return m[id].Cast<ComboBox>().CurrentValue;
        }

        public static int slider(this Menu m, string id)
        {
            return m[id].Cast<Slider>().CurrentValue;
        }

        public static bool checkbox(this Menu m, string id)
        {
            return m[id].Cast<CheckBox>().CurrentValue;
        }

        public static bool keybind(this Menu m, string id)
        {
            return m[id].Cast<KeyBind>().CurrentValue;
        }

        public static System.Drawing.Color Color(this Menu m, string id)
        {
            return m[id].Cast<ColorPicker>().CurrentValue;
        }
    }
}