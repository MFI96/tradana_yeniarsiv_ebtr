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
            _myMenu.AddLabel("MeLoDag Taraf�ndan kodland�");
            _myMenu.AddLabel("tradana tarafindan �evrildi.");
        }

        private static void DrawMeNuPage()
        {
            DrawMeNu = _myMenu.AddSubMenu("Draw  settings", "Draw");
            DrawMeNu.AddGroupLabel("Draw Settings:");
            DrawMeNu.Add("nodraw",
                new CheckBox("Hi�bir �ey g�sterme", false));
          DrawMeNu.AddSeparator();
            DrawMeNu.Add("draw.Q",
                new CheckBox("G�ster Q"));
            DrawMeNu.Add("draw.W",
                new CheckBox("G�ster W"));
            DrawMeNu.Add("draw.E",
                new CheckBox("G�ster E"));
            DrawMeNu.Add("draw.R",
                new CheckBox("G�ster R"));
          }

        private static void ComboMenuPage()
        {
            ComboMenu = _myMenu.AddSubMenu("Combo settings", "Combo");
            ComboMenu.AddGroupLabel("Combo Ayarlar�:");
            ComboMenu.AddLabel("Q Kullanma Ayarlar�");
            foreach (var enemies in EntityManager.Heroes.Enemies.Where(i => !i.IsMe))
            {
                ComboMenu.Add("combo.Q" + enemies.ChampionName, new CheckBox("" + enemies.ChampionName));
            }
            ComboMenu.Add("combo.CCQ",
                new CheckBox("Kullan Q"));
            ComboMenu.AddLabel("E Ayarlar�:");
            ComboMenu.Add("combo.E",
                new CheckBox("Kullan E"));
            ComboMenu.Add("combo.CC",
             new CheckBox("Kullan E"));
            ComboMenu.AddLabel("W Ayarlar�:");
            ComboMenu.Add("combo.w",
                new CheckBox("Kullan W"));
            ComboMenu.AddLabel("W kullanmadan Zhonya,MF ulti, teleport hat�rla :)");
            ComboMenu.Add("combo.CCW",
            new CheckBox("Kullan W CC"));
            ComboMenu.AddLabel("R Ayarlar�:");
            ComboMenu.Add("combo.R",
                new CheckBox("Kullan Ak�ll� R"));
            ComboMenu.AddSeparator();
            ComboMenu.AddLabel("Kombo Ayarlar�:");
           ComboMenu.Add("comboEQbind",
                new KeyBind("Kullan E > Q > AA", false, KeyBind.BindTypes.HoldActive, 'Z'));
         
         
           }


        private static void FarmMeNuPage()
        {
            FarmMeNu = _myMenu.AddSubMenu("Lane Clear Settings", "laneclear");
            FarmMeNu.AddGroupLabel("Lane Temizleme Ayarlar�:");
            FarmMeNu.Add("Lane.Q",
                new CheckBox("Kullan Q"));
            FarmMeNu.Add("LaneMana",
                new Slider("Lantemizlemek i�in en az mana %", 60));
            FarmMeNu.AddSeparator();
            FarmMeNu.AddLabel("Orman Ayarlar�");
            FarmMeNu.Add("jungle.Q",
                new CheckBox("Kullan Q"));
        }

        private static void HarassMeNuPage()
        {
            HarassMeNu = _myMenu.AddSubMenu("Harass/Killsteal Settings", "hksettings");
            HarassMeNu.AddGroupLabel("D�rtme Ayarlar�:");
            HarassMeNu.AddSeparator();
            HarassMeNu.AddLabel("Q �unlarda kullan");
            foreach (var enemies in EntityManager.Heroes.Enemies.Where(i => !i.IsMe))
            {
                HarassMeNu.Add("Harass.Q" + enemies.ChampionName, new CheckBox("" + enemies.ChampionName));
            }
            HarassMeNu.AddSeparator();
            HarassMeNu.Add("UseWharass", new CheckBox("Use W"));
            HarassMeNu.Add("harass.QE",
                new Slider("en az �u kadar manam varsa %", 55));
            HarassMeNu.AddSeparator();
            HarassMeNu.AddLabel("Kill �alma AYarlar�:");
            HarassMeNu.Add("killsteal.Q",
                new CheckBox("Kullan Q", false));
        }

        private static void ActivatorPage()
        {
            Activator = _myMenu.AddSubMenu("Activator Settings", "Items");
            Activator.AddGroupLabel("Otomatik QSS Kullan E�er :");
            Activator.Add("Blind",
                new CheckBox("K�rse", false));
            Activator.Add("Charm",
                new CheckBox("�ekilmi�se(Ahri)"));
            Activator.Add("Fear",
                new CheckBox("Kokrmu�sa"));
            Activator.Add("Polymorph",
                new CheckBox("Polymorph(Lulu W)"));
            Activator.Add("Stun",
                new CheckBox("Sersemlemi�se"));
            Activator.Add("Snare",
                new CheckBox("Snare"));
            Activator.Add("Silence",
                new CheckBox("Susturulmu�sa", false));
            Activator.Add("Taunt",
                new CheckBox("Alay ediliyorsa"));
            Activator.Add("Suppression",
                new CheckBox("WW,Urgot RS(Suppression)"));
            Activator.AddLabel("�tem Kullan�m�:");
         Activator.Add("bilgewater",
                new CheckBox("Bilgewater palas� kullan"));
            Activator.Add("bilgewater.HP",
                new Slider("Bilgewater palas� i�in gereken can {0}(%)", 60));
            Activator.AddSeparator();
            Activator.Add("botrk",
                new CheckBox("Mahvolmu� k�l�� kullan"));
            Activator.Add("botrk.HP",
                new Slider("Mahvolmu� k�l�� i�in gereken can {0}(%)", 60));
            Activator.AddSeparator();
            Activator.Add("youmus",
                new CheckBox("Kullan Youmus K�l�c�"));
            Activator.Add("items.Youmuss.HP",
                new Slider("Youmuss i�in gereken can {0}(%)", 60, 1));
            Activator.Add("youmus.Enemies",
                new Slider("E�er menzilde �u kadar {0} d��man varsa Kullan", 3, 1, 5));
           Activator.AddLabel("�ksir Ayarlar�");
            Activator.Add("spells.Potions.Check",
                new CheckBox("Kullan �ksirler"));
            Activator.Add("spells.Potions.HP",
                new Slider("Can�m �undan azsa {0}(%)", 60, 1));
            Activator.Add("spells.Potions.Mana",
                new Slider("Manam �undan azsa {0}(%)", 60, 1));
            Activator.AddLabel("B�y� Ayarlar�:");
           Activator.AddLabel("�yile�tirme ayarlar�:");
            Activator.Add("spells.Heal.Hp",
                new Slider("E�er can�m �undan d���kse kullan {0}(%)", 30, 1));
            Activator.AddLabel("Tutu�tur Ayarlar�:");
            Activator.Add("spells.Ignite.Focus",
                new Slider("Tutu�tur kullanmak i�in hedefin can� �undan az {0}(%)", 10, 1));
        }

        private static void MiscMeNuPage()
        {
            MiscMeNu = _myMenu.AddSubMenu("Misc Menu", "othermenu");
            MiscMeNu.AddGroupLabel("Ka�ma ayarlar�");
            MiscMeNu.Add("useEmouse",
                new KeyBind("Kullan E Fareye do�ru", false, KeyBind.BindTypes.HoldActive, "T".ToCharArray()[0]));
            MiscMeNu.AddLabel("Anti Tehlikeli Yak�nla�ma/Tehlikeli Yetenek");
            MiscMeNu.Add("gapcloser.E",
                new CheckBox("Tehlikeli yak�nla�may� engellemek i�in E kullan"));
            MiscMeNu.Add("gapcloser.W",
                new CheckBox("Tehlikeli yak�nla�may� engellemek i�in W kullan"));
            MiscMeNu.Add("interupt.W",
              new CheckBox("Tehlikeli yetene�i bozmak i�in W kullan"));
            MiscMeNu.AddLabel("Kost�m Ayarlar�");
            MiscMeNu.Add("checkSkin",
                new CheckBox("Kost�m De�i�tirici Kullan:", false));
            MiscMeNu.Add("skin.Id",
                new Slider("Kost�m Say�s�", 5, 0, 10));
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