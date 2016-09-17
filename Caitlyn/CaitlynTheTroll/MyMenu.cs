using System.Linq;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace CaitlynTheTroll
{
    internal static class CaitlynTheTrollMeNu
    {
        private static Menu _myMenu;
        public static Menu ComboMenu, DrawMeNu, HarassMeNu, Activator, FarmMeNu, MiscMeNu;

        public static void LoadMenu()
        {
            MyCaitlynTheTrollPage();
            ComboMenuPage();
            FarmMeNuPage();
            HarassMeNuPage();
            ActivatorPage();
            MiscMeNuPage();
            DrawMeNuPage();
        }

        private static void MyCaitlynTheTrollPage()
        {
            _myMenu = MainMenu.AddMenu("Caitlyn The Troll", "main");
            _myMenu.AddLabel(" Caitlyn The Troll " + Program.Version);
            _myMenu.AddLabel("MeLoDag Tarafýndan kodlandý");
            _myMenu.AddLabel("tradana tarafindan çevrildi.");
        }

        private static void DrawMeNuPage()
        {
            DrawMeNu = _myMenu.AddSubMenu("Draw  settings", "Draw");
            DrawMeNu.AddGroupLabel("Draw Settings:");
            DrawMeNu.Add("nodraw",
                new CheckBox("Hiçbir þey gösterme", false));
          DrawMeNu.AddSeparator();
            DrawMeNu.Add("draw.Q",
                new CheckBox("Göster Q"));
            DrawMeNu.Add("draw.W",
                new CheckBox("Göster W"));
            DrawMeNu.Add("draw.E",
                new CheckBox("Göster E"));
            DrawMeNu.Add("draw.R",
                new CheckBox("Göster R"));
          }

        private static void ComboMenuPage()
        {
            ComboMenu = _myMenu.AddSubMenu("Combo settings", "Combo");
            ComboMenu.AddGroupLabel("Combo Ayarlarý:");
            ComboMenu.AddLabel("Q Kullanma Ayarlarý");
            foreach (var enemies in EntityManager.Heroes.Enemies.Where(i => !i.IsMe))
            {
                ComboMenu.Add("combo.Q" + enemies.ChampionName, new CheckBox("" + enemies.ChampionName));
            }
            ComboMenu.Add("combo.CCQ",
                new CheckBox("Kullan Q"));
            ComboMenu.AddLabel("E Ayarlarý:");
            ComboMenu.Add("combo.E",
                new CheckBox("Kullan E"));
            ComboMenu.Add("combo.CC",
             new CheckBox("Kullan E"));
            ComboMenu.AddLabel("W Ayarlarý:");
            ComboMenu.Add("combo.w",
                new CheckBox("Kullan W"));
            ComboMenu.AddLabel("W kullanmadan Zhonya,MF ulti, teleport hatýrla :)");
            ComboMenu.Add("combo.CCW",
            new CheckBox("Kullan W CC"));
            ComboMenu.AddLabel("R Ayarlarý:");
            ComboMenu.Add("combo.R",
                new CheckBox("Kullan Akýllý R"));
            ComboMenu.AddSeparator();
            ComboMenu.AddLabel("Kombo Ayarlarý:");
           ComboMenu.Add("comboEQbind",
                new KeyBind("Kullan E > Q > AA", false, KeyBind.BindTypes.HoldActive, 'Z'));
         
         
           }


        private static void FarmMeNuPage()
        {
            FarmMeNu = _myMenu.AddSubMenu("Lane Clear Settings", "laneclear");
            FarmMeNu.AddGroupLabel("Lane Temizleme Ayarlarý:");
            FarmMeNu.Add("Lane.Q",
                new CheckBox("Kullan Q"));
            FarmMeNu.Add("LaneMana",
                new Slider("Lantemizlemek için en az mana %", 60));
            FarmMeNu.AddSeparator();
            FarmMeNu.AddLabel("Orman Ayarlarý");
            FarmMeNu.Add("jungle.Q",
                new CheckBox("Kullan Q"));
        }

        private static void HarassMeNuPage()
        {
            HarassMeNu = _myMenu.AddSubMenu("Harass/Killsteal Settings", "hksettings");
            HarassMeNu.AddGroupLabel("Dürtme Ayarlarý:");
            HarassMeNu.AddSeparator();
            HarassMeNu.AddLabel("Q þunlarda kullan");
            foreach (var enemies in EntityManager.Heroes.Enemies.Where(i => !i.IsMe))
            {
                HarassMeNu.Add("Harass.Q" + enemies.ChampionName, new CheckBox("" + enemies.ChampionName));
            }
            HarassMeNu.AddSeparator();
            HarassMeNu.Add("UseWharass", new CheckBox("Use W"));
            HarassMeNu.Add("harass.QE",
                new Slider("en az þu kadar manam varsa %", 55));
            HarassMeNu.AddSeparator();
            HarassMeNu.AddLabel("Kill Çalma AYarlarý:");
            HarassMeNu.Add("killsteal.Q",
                new CheckBox("Kullan Q", false));
        }

        private static void ActivatorPage()
        {
            Activator = _myMenu.AddSubMenu("Activator Settings", "Items");
            Activator.AddGroupLabel("Otomatik QSS Kullan Eðer :");
            Activator.Add("Blind",
                new CheckBox("Körse", false));
            Activator.Add("Charm",
                new CheckBox("Çekilmiþse(Ahri)"));
            Activator.Add("Fear",
                new CheckBox("Kokrmuþsa"));
            Activator.Add("Polymorph",
                new CheckBox("Polymorph(Lulu W)"));
            Activator.Add("Stun",
                new CheckBox("Sersemlemiþse"));
            Activator.Add("Snare",
                new CheckBox("Snare"));
            Activator.Add("Silence",
                new CheckBox("Susturulmuþsa", false));
            Activator.Add("Taunt",
                new CheckBox("Alay ediliyorsa"));
            Activator.Add("Suppression",
                new CheckBox("WW,Urgot RS(Suppression)"));
            Activator.AddLabel("Ýtem Kullanýmý:");
         Activator.Add("bilgewater",
                new CheckBox("Bilgewater palasý kullan"));
            Activator.Add("bilgewater.HP",
                new Slider("Bilgewater palasý için gereken can {0}(%)", 60));
            Activator.AddSeparator();
            Activator.Add("botrk",
                new CheckBox("Mahvolmuþ kýlýç kullan"));
            Activator.Add("botrk.HP",
                new Slider("Mahvolmuþ kýlýç için gereken can {0}(%)", 60));
            Activator.AddSeparator();
            Activator.Add("youmus",
                new CheckBox("Kullan Youmus Kýlýcý"));
            Activator.Add("items.Youmuss.HP",
                new Slider("Youmuss için gereken can {0}(%)", 60, 1));
            Activator.Add("youmus.Enemies",
                new Slider("Eðer menzilde þu kadar {0} düþman varsa Kullan", 3, 1, 5));
           Activator.AddLabel("Ýksir Ayarlarý");
            Activator.Add("spells.Potions.Check",
                new CheckBox("Kullan Ýksirler"));
            Activator.Add("spells.Potions.HP",
                new Slider("Caným þundan azsa {0}(%)", 60, 1));
            Activator.Add("spells.Potions.Mana",
                new Slider("Manam þundan azsa {0}(%)", 60, 1));
            Activator.AddLabel("Büyü Ayarlarý:");
           Activator.AddLabel("Ýyileþtirme ayarlarý:");
            Activator.Add("spells.Heal.Hp",
                new Slider("Eðer caným þundan düþükse kullan {0}(%)", 30, 1));
            Activator.AddLabel("Tutuþtur Ayarlarý:");
            Activator.Add("spells.Ignite.Focus",
                new Slider("Tutuþtur kullanmak için hedefin caný þundan az {0}(%)", 10, 1));
        }

        private static void MiscMeNuPage()
        {
            MiscMeNu = _myMenu.AddSubMenu("Misc Menu", "othermenu");
            MiscMeNu.AddGroupLabel("Kaçma ayarlarý");
            MiscMeNu.Add("useEmouse",
                new KeyBind("Kullan E Fareye doðru", false, KeyBind.BindTypes.HoldActive, "T".ToCharArray()[0]));
            MiscMeNu.AddLabel("Anti Tehlikeli Yakýnlaþma/Tehlikeli Yetenek");
            MiscMeNu.Add("gapcloser.E",
                new CheckBox("Tehlikeli yakýnlaþmayý engellemek için E kullan"));
            MiscMeNu.Add("gapcloser.W",
                new CheckBox("Tehlikeli yakýnlaþmayý engellemek için W kullan"));
            MiscMeNu.Add("interupt.W",
              new CheckBox("Tehlikeli yeteneði bozmak için W kullan"));
            MiscMeNu.AddLabel("Kostüm Ayarlarý");
            MiscMeNu.Add("checkSkin",
                new CheckBox("Kostüm Deðiþtirici Kullan:", false));
            MiscMeNu.Add("skin.Id",
                new Slider("Kostüm Sayýsý", 5, 0, 10));
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

        public static bool ComboE()
        {
            return ComboMenu["combo.E"].Cast<CheckBox>().CurrentValue;
        }

        public static bool ComboW()
        {
            return ComboMenu["combo.W"].Cast<CheckBox>().CurrentValue;
        }
        public static float ComboREnemies()
        {
            return ComboMenu["combo.REnemies"].Cast<Slider>().CurrentValue;
        }
        public static bool ComboR()
        {
            return ComboMenu["combo.R"].Cast<CheckBox>().CurrentValue;
        }
        public static bool ComboEq()
        {
            return ComboMenu["comboEQbind"].Cast<KeyBind>().CurrentValue;
        }
       public static bool LaneQ()
        {
            return FarmMeNu["lane.Q"].Cast<CheckBox>().CurrentValue;
        }

        public static float LaneMana()
        {
            return FarmMeNu["LaneMana"].Cast<Slider>().CurrentValue;
        }

        public static bool JungleQ()
        {
            return FarmMeNu["jungle.Q"].Cast<CheckBox>().CurrentValue;
        }

        public static bool HarassQ()
        {
            return HarassMeNu["harass.Q"].Cast<CheckBox>().CurrentValue;
        }
        public static bool HarassW()
        {
            return HarassMeNu["UseWharass"].Cast<CheckBox>().CurrentValue;
        }
        public static float HarassQe()
        {
            return HarassMeNu["harass.QE"].Cast<Slider>().CurrentValue;
        }

        public static bool KillstealQ()
        {
            return HarassMeNu["killsteal.Q"].Cast<CheckBox>().CurrentValue;
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

        public static float SpellsIgniteFocus()
        {
            return Activator["spells.Ignite.Focus"].Cast<Slider>().CurrentValue;
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

        public static bool GapcloserE()
        {
            return MiscMeNu["gapcloser.E"].Cast<CheckBox>().CurrentValue;
        }

        public static bool GapcloserW()
        {
            return MiscMeNu["gapcloser.W"].Cast<CheckBox>().CurrentValue;
        }
        public static bool InterupteW()
        {
            return MiscMeNu["interupt.W"].Cast<CheckBox>().CurrentValue;
        }

        public static bool SkinChanger()
        {
            return MiscMeNu["SkinChanger"].Cast<CheckBox>().CurrentValue;
        }

        public static bool CheckSkin()
        {
            return MiscMeNu["checkSkin"].Cast<CheckBox>().CurrentValue;
        }

        public static bool UseEmouse()
        {
            return MiscMeNu["useEmouse"].Cast<KeyBind>().CurrentValue;
        }
    }
}