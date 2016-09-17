using System.Linq;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace MissFortuneTheTroll
{
    internal static class MissFortuneTheTrollMenu
    {
        private static Menu _myMenu;
        public static Menu ComboMenu, DrawMeNu, HarassMeNu, Activator, FarmMeNu, MiscMeNu, FleeMenu;

        public static void LoadMenu()
        {
            MissFortuneTheTrollPage();
            ComboMenuPage();
            FarmMeNuPage();
            HarassMeNuPage();
            ActivatorPage();
            MiscMeNuPage();
            FleeMenuPage();
            DrawMeNuPage();
        }

        private static void MissFortuneTheTrollPage()
        {
            _myMenu = MainMenu.AddMenu("MissFortune The Troll", "main");
            _myMenu.AddLabel(" MissFortune The Troll " + Program.Version);
            _myMenu.AddLabel(" Melodag Tarafýndan yapýlmýþ");
            _myMenu.AddLabel("tradana tarafindan çevrilmiþtir.")
        }

        private static void DrawMeNuPage()
        {
            DrawMeNu = _myMenu.AddSubMenu("Draw  settings", "Draw");
            DrawMeNu.AddGroupLabel("Gösterge Ayarlarý:");
            DrawMeNu.Add("nodraw",
                new CheckBox("Hiçbir Þey Gösterme", false));
            DrawMeNu.AddSeparator();
            DrawMeNu.Add("draw.Q",
                new CheckBox("Göster Q"));
            DrawMeNu.Add("draw.W",
                new CheckBox("Göster W"));
            DrawMeNu.Add("draw.E",
                new CheckBox("Göster E"));
            DrawMeNu.Add("draw.R",
                new CheckBox("Göster R"));
            DrawMeNu.AddLabel("Hasar Tespitçisi");
            DrawMeNu.Add("healthbar", new CheckBox("Canbarýnda verilecek hasarý Göster"));
            DrawMeNu.Add("percent", new CheckBox("Hasarý Yüzde olarak Göster"));
        }

        private static void ComboMenuPage()
        {
            ComboMenu = _myMenu.AddSubMenu("Combo settings", "Combo");
            ComboMenu.AddGroupLabel("Kombo Ayarlarý:");
            ComboMenu.AddLabel("Q Ayarlarý:");
            ComboMenu.Add("combo.Q", new CheckBox("Kullan Q"));
            ComboMenu.Add("combo.QExtend", new CheckBox("Geliþmiþ Q Kullan"));
            ComboMenu.AddLabel("W Ayarlarý:");
            ComboMenu.Add("combo.w", new CheckBox("Kullan W"));
            ComboMenu.Add("combo.wenemies", new Slider("Eðer þu kadar düþman varsa W kullan", 2, 1, 5));
            ComboMenu.AddLabel("E Ayarlarý:");
            ComboMenu.Add("combo.E", new CheckBox("Kullan E"));
            ComboMenu.Add("combo.CCQ", new CheckBox("Mantýklý Hedefe Doðru E"));
            ComboMenu.AddLabel("R Ayarlarý :");
            ComboMenu.Add("combo.R", new CheckBox("Kullan R"));
            ComboMenu.Add("combo.REnemies", new Slider("R için en az düþman", 1, 1, 5));
            ComboMenu.Add("combo.R1", new CheckBox("Mantýklý Hedefe Doðru R Kullan"));
            
        }


        private static void FarmMeNuPage()
        {
            FarmMeNu = _myMenu.AddSubMenu("Lane Jungle Settings", "laneclear");
            FarmMeNu.AddGroupLabel("Lane Temizleme Ayarlarý:");
            FarmMeNu.Add("Lane.Q",
                new CheckBox("Kullan Q"));
            FarmMeNu.Add("Lane.W",
                new CheckBox("Kullan W"));
            FarmMeNu.Add("Lane.E",
                new CheckBox("Kullan E"));
            FarmMeNu.Add("LaneMana",
                new Slider("Lanetemizleme için en az mana %", 60));
            FarmMeNu.AddLabel("Orman Ayarlarý");
            FarmMeNu.Add("jungle.Q",
                new CheckBox("Kullan Q"));
            FarmMeNu.Add("jungle.W",
                new CheckBox("Kullan W"));
            FarmMeNu.Add("jungle.E",
                new CheckBox("Kullan E"));
        }

        private static void HarassMeNuPage()
        {
            HarassMeNu = _myMenu.AddSubMenu("Harass/Killsteal Settings", "hksettings");
            HarassMeNu.AddGroupLabel("Dürtme Ayarlarý:");
            HarassMeNu.Add("UseQextendharass", new CheckBox("Q ile Zorla"));
            HarassMeNu.AddLabel("E yi þu durumda Kullan");
            foreach (var enemies in EntityManager.Heroes.Enemies.Where(i => !i.IsMe))
            {
                HarassMeNu.Add("Harass.E" + enemies.ChampionName, new CheckBox("" + enemies.ChampionName));
            }
            HarassMeNu.Add("harass.QE",
                new Slider("Dürtme büyüleri için en az mana %", 55));
            HarassMeNu.AddLabel("Otomatik Dürtme Ayarlarý:");
            HarassMeNu.Add("AutoQextendharass", new CheckBox("Otomatik Q ile Hedefi Zorla"));
            HarassMeNu.Add("AutoHarassmana",
                new Slider("Otomatik Dürtme için en azmana %", 55));
            HarassMeNu.AddLabel("Killçalma Ayarlarý:");
            HarassMeNu.Add("killsteal.Q",
                new CheckBox("Kullan Q", true));
        }

        private static void ActivatorPage()
        {
            Activator = _myMenu.AddSubMenu("Activator Settings", "Items");
            Activator.AddGroupLabel("Auto QSS if :");
            Activator.Add("Blind",
                new CheckBox("Körse", false));
            Activator.Add("Charm",
                new CheckBox("Çekilmiþse(ahri)"));
            Activator.Add("Fear",
                new CheckBox("Korkmuþsa"));
            Activator.Add("Polymorph",
                new CheckBox("Polymorph(Lulu W)"));
            Activator.Add("Stun",
                new CheckBox("Sersemlemiþse"));
            Activator.Add("Snare",
                new CheckBox("Tuzaða Düþmüþse(cait)"));
            Activator.Add("Silence",
                new CheckBox("Susturulmuþsa", false));
            Activator.Add("Taunt",
                new CheckBox("Alay Ediliyorsa"));
            Activator.Add("Suppression",
                new CheckBox("WW,Urgot R(Suppression)"));
            Activator.Add("delay", new Slider("Gecikme Kullan", 100, 0, 500));
            Activator.AddLabel("Ýtem Kullanýmlarý:");
            Activator.Add("bilgewater",
                new CheckBox("Bilgewater palasý Kullan"));
            Activator.Add("bilgewater.HP",
                new Slider("Bilgewater Palasý Kullanmak için can þunun altýnda {0}(%)", 60));
            Activator.Add("botrk",
                new CheckBox("Mahvolmuþ Kýlýç Kullan"));
            Activator.Add("botrk.HP",
                new Slider("Mahvolmuþ Kýlýç kullanmak için can þunun altýnda {0}(%)", 60));
            Activator.Add("youmus",
                new CheckBox("Youmu'nun Kýlýcý Kullan"));
            Activator.Add("items.Youmuss.HP",
                new Slider("Youumu'nun Kýlýcý Kullanmak için caným þundan altta {0}(%)", 60, 1));
            Activator.Add("youmus.Enemies",
                new Slider("Youumu'nun Kýlýcý için menzilde {0} Kadar düþman olsun", 3, 1, 5));
            Activator.AddLabel("Ýksir Ayarlarý");
            Activator.Add("spells.Potions.Check",
                new CheckBox("Ýksirleri Kullan"));
            Activator.Add("spells.Potions.HP",
                new Slider("Ýksir kullanmak için caným þunun altýnda {0}(%)", 60, 1));
            Activator.Add("spells.Potions.Mana",
                new Slider("Ýksir Kullanmak için Manam þunun altýnda {0}(%)", 60, 1));
            Activator.AddLabel("Büyü Ayarlarý:");
            Activator.AddLabel("Ýyileþtirme Ayarlarý:");
            Activator.Add("spells.Heal.Hp",
                new Slider("Ýyileþtirme için caným þunun altýnda {0}(%)", 30, 1));
            Activator.AddLabel("Tutuþtur Ayarlarý:");
            Activator.Add("spells.Ignite.Focus",
                new Slider("Tutuþtur için hedefin caný þunun altýnda {0}(%)", 10, 1));
        }
        private static void FleeMenuPage()
        {
            FleeMenu = _myMenu.AddSubMenu("Flee Settings", "FleeSettings");
            FleeMenu.AddGroupLabel("Kaçýþ(Flee) Ayarlarý");
            FleeMenu.Add("FleeE", new CheckBox("Kaçarken E Kullan"));
            FleeMenu.Add("FleeW", new CheckBox("Kaçarken W Kullan"));
        }

        private static void MiscMeNuPage()
        {
            MiscMeNu = _myMenu.AddSubMenu("Misc Menu", "othermenu");
            MiscMeNu.AddGroupLabel("Tehlikeli Durumlarý Önleyici Kullan");
            MiscMeNu.Add("gapcloser.E",
                new CheckBox("Rakibe Yaklaþma/Uzaklaþma için E Kullan(Gapclose)"));
            MiscMeNu.AddLabel("Kostüm Ayarlarý");
            MiscMeNu.Add("checkSkin",
                new CheckBox("Kostümünü Seç:", false));
            MiscMeNu.Add("skin.Id",
                new Slider("Kostüm Numarasý", 5, 0, 10));
        }

        public static bool Nodraw()
        {
            return DrawMeNu["nodraw"].Cast<CheckBox>().CurrentValue;
        }

        public static bool DrawingsQ()
        {
            return DrawMeNu["draw.Q"].Cast<CheckBox>().CurrentValue;
        }

        public static bool DrawingsW()
        {
            return DrawMeNu["draw.W"].Cast<CheckBox>().CurrentValue;
        }

        public static bool DrawingsE()
        {
            return DrawMeNu["draw.E"].Cast<CheckBox>().CurrentValue;
        }

        public static bool DrawingsR()
        {
            return DrawMeNu["draw.R"].Cast<CheckBox>().CurrentValue;
        }

        public static bool DrawingsT()
        {
            return DrawMeNu["draw.T"].Cast<CheckBox>().CurrentValue;
        }

        public static bool Fleee()
        {
            return FleeMenu["FleeE"].Cast<CheckBox>().CurrentValue;
        }

        public static bool Fleew()
        {
            return FleeMenu["FleeW"].Cast<CheckBox>().CurrentValue;
        }
        public static float ComboREnemies()
        {
            return ComboMenu["combo.REnemies"].Cast<Slider>().CurrentValue;
        }

        public static bool ComboR()
        {
            return ComboMenu["combo.R"].Cast<CheckBox>().CurrentValue;
        }

        public static bool ComboRcc()
        {
            return ComboMenu["combo.R1"].Cast<CheckBox>().CurrentValue;
        }

        public static bool ComboE()
        {
            return ComboMenu["combo.E"].Cast<CheckBox>().CurrentValue;
        }

        public static bool ComboQ()
        {
            return ComboMenu["combo.Q"].Cast<CheckBox>().CurrentValue;
        }

        public static bool ComboQextend()
        {
            return ComboMenu["combo.QExtend"].Cast<CheckBox>().CurrentValue;
        }

        public static bool ComboW()
        {
            return ComboMenu["combo.W"].Cast<CheckBox>().CurrentValue;
        }

        public static float Combowenemies()
        {
            return ComboMenu["combo.wenemies"].Cast<Slider>().CurrentValue;
        }

        public static float LaneMana()
        {
            return FarmMeNu["LaneMana"].Cast<Slider>().CurrentValue;
        }

        public static bool LaneQ()
        {
            return FarmMeNu["Lane.Q"].Cast<CheckBox>().CurrentValue;
        }

        public static bool LaneE()
        {
            return FarmMeNu["Lane.E"].Cast<CheckBox>().CurrentValue;
        }

        public static bool LaneW()
        {
            return FarmMeNu["Lane.W"].Cast<CheckBox>().CurrentValue;
        }

        public static bool JungleQ()
        {
            return FarmMeNu["jungle.Q"].Cast<CheckBox>().CurrentValue;
        }

        public static bool JungleE()
        {
            return FarmMeNu["jungle.E"].Cast<CheckBox>().CurrentValue;
        }

        public static bool JungleW()
        {
            return FarmMeNu["jungle.W"].Cast<CheckBox>().CurrentValue;
        }

        public static float HarassQe()
        {
            return HarassMeNu["harass.QE"].Cast<Slider>().CurrentValue;
        }
        public static float AutoHarassmana()
        {
            return HarassMeNu["AutoHarassmana"].Cast<Slider>().CurrentValue;
        }

        public static bool AutoQextendharass()
        {
            return HarassMeNu["AutoQextendharass"].Cast<CheckBox>().CurrentValue;
        }
        
        public static bool KillstealQ()
        {
            return HarassMeNu["killsteal.Q"].Cast<CheckBox>().CurrentValue;
        }

        public static bool UseQextendharass()
        {
            return HarassMeNu["UseQextendharass"].Cast<CheckBox>().CurrentValue;
        }

        public static bool Bilgewater()
        {
            return Activator["bilgewater"].Cast<CheckBox>().CurrentValue;
        }

        public static float BilgewaterHp()
        {
            return Activator["bilgewater.HP"].Cast<Slider>().CurrentValue;
        }

        public static bool Botrk()
        {
            return Activator["botrk"].Cast<CheckBox>().CurrentValue;
        }

        public static float BotrkHp()
        {
            return Activator["botrk.HP"].Cast<Slider>().CurrentValue;
        }

        public static bool Youmus()
        {
            return Activator["youmus"].Cast<CheckBox>().CurrentValue;
        }

        public static float YoumusEnemies()
        {
            return Activator["youmus.Enemies"].Cast<Slider>().CurrentValue;
        }

        public static float SpellsIgniteFocus()
        {
            return Activator["spells.Ignite.Focus"].Cast<Slider>().CurrentValue;
        }

        public static float ItemsYoumuShp()
        {
            return Activator["items.Youmuss.HP"].Cast<Slider>().CurrentValue;
        }

        public static bool SpellsPotionsCheck()
        {
            return Activator["spells.Potions.Check"].Cast<CheckBox>().CurrentValue;
        }

        public static float SpellsPotionsHp()
        {
            return Activator["spells.Potions.HP"].Cast<Slider>().CurrentValue;
        }

        public static float SpellsPotionsM()
        {
            return Activator["spells.Potions.Mana"].Cast<Slider>().CurrentValue;
        }

        public static float SpellsHealHp()
        {
            return Activator["spells.Heal.HP"].Cast<Slider>().CurrentValue;
        }

        public static float SpellsBarrierHp()
        {
            return Activator["spells.Barrier.Hp"].Cast<Slider>().CurrentValue;
        }

        public static bool Blind()
        {
            return Activator["Blind"].Cast<CheckBox>().CurrentValue;
        }

        public static bool Charm()
        {
            return Activator["Charm"].Cast<CheckBox>().CurrentValue;
        }

        public static bool Fear()
        {
            return Activator["Fear"].Cast<CheckBox>().CurrentValue;
        }

        public static bool Polymorph()
        {
            return Activator["Polymorph"].Cast<CheckBox>().CurrentValue;
        }

        public static bool Stun()
        {
            return Activator["Stun"].Cast<CheckBox>().CurrentValue;
        }

        public static bool Snare()
        {
            return Activator["Snare"].Cast<CheckBox>().CurrentValue;
        }

        public static bool Silence()
        {
            return Activator["Silence"].Cast<CheckBox>().CurrentValue;
        }

        public static bool Taunt()
        {
            return Activator["Taunt"].Cast<CheckBox>().CurrentValue;
        }

        public static bool Suppression()
        {
            return Activator["Suppression"].Cast<CheckBox>().CurrentValue;
        }


        public static int SkinId()
        {
            return MiscMeNu["skin.Id"].Cast<Slider>().CurrentValue;
        }

        public static bool GapcloserQ()
        {
            return MiscMeNu["gapcloser.Q"].Cast<CheckBox>().CurrentValue;
        }

        public static bool GapcloserE()
        {
            return MiscMeNu["gapcloser.E"].Cast<CheckBox>().CurrentValue;
        }

        public static bool InterupteQ()
        {
            return MiscMeNu["interupt.Q"].Cast<CheckBox>().CurrentValue;
        }

        public static bool SkinChanger()
        {
            return MiscMeNu["SkinChanger"].Cast<CheckBox>().CurrentValue;
        }

        public static bool CheckSkin()
        {
            return MiscMeNu["checkSkin"].Cast<CheckBox>().CurrentValue;
        }
    }
}