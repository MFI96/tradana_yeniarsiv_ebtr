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
            _myMenu.AddLabel(" Melodag Taraf�ndan yap�lm��");
            _myMenu.AddLabel("tradana tarafindan �evrilmi�tir.")
        }

        private static void DrawMeNuPage()
        {
            DrawMeNu = _myMenu.AddSubMenu("Draw  settings", "Draw");
            DrawMeNu.AddGroupLabel("G�sterge Ayarlar�:");
            DrawMeNu.Add("nodraw",
                new CheckBox("Hi�bir �ey G�sterme", false));
            DrawMeNu.AddSeparator();
            DrawMeNu.Add("draw.Q",
                new CheckBox("G�ster Q"));
            DrawMeNu.Add("draw.W",
                new CheckBox("G�ster W"));
            DrawMeNu.Add("draw.E",
                new CheckBox("G�ster E"));
            DrawMeNu.Add("draw.R",
                new CheckBox("G�ster R"));
            DrawMeNu.AddLabel("Hasar Tespit�isi");
            DrawMeNu.Add("healthbar", new CheckBox("Canbar�nda verilecek hasar� G�ster"));
            DrawMeNu.Add("percent", new CheckBox("Hasar� Y�zde olarak G�ster"));
        }

        private static void ComboMenuPage()
        {
            ComboMenu = _myMenu.AddSubMenu("Combo settings", "Combo");
            ComboMenu.AddGroupLabel("Kombo Ayarlar�:");
            ComboMenu.AddLabel("Q Ayarlar�:");
            ComboMenu.Add("combo.Q", new CheckBox("Kullan Q"));
            ComboMenu.Add("combo.QExtend", new CheckBox("Geli�mi� Q Kullan"));
            ComboMenu.AddLabel("W Ayarlar�:");
            ComboMenu.Add("combo.w", new CheckBox("Kullan W"));
            ComboMenu.Add("combo.wenemies", new Slider("E�er �u kadar d��man varsa W kullan", 2, 1, 5));
            ComboMenu.AddLabel("E Ayarlar�:");
            ComboMenu.Add("combo.E", new CheckBox("Kullan E"));
            ComboMenu.Add("combo.CCQ", new CheckBox("Mant�kl� Hedefe Do�ru E"));
            ComboMenu.AddLabel("R Ayarlar� :");
            ComboMenu.Add("combo.R", new CheckBox("Kullan R"));
            ComboMenu.Add("combo.REnemies", new Slider("R i�in en az d��man", 1, 1, 5));
            ComboMenu.Add("combo.R1", new CheckBox("Mant�kl� Hedefe Do�ru R Kullan"));
            
        }


        private static void FarmMeNuPage()
        {
            FarmMeNu = _myMenu.AddSubMenu("Lane Jungle Settings", "laneclear");
            FarmMeNu.AddGroupLabel("Lane Temizleme Ayarlar�:");
            FarmMeNu.Add("Lane.Q",
                new CheckBox("Kullan Q"));
            FarmMeNu.Add("Lane.W",
                new CheckBox("Kullan W"));
            FarmMeNu.Add("Lane.E",
                new CheckBox("Kullan E"));
            FarmMeNu.Add("LaneMana",
                new Slider("Lanetemizleme i�in en az mana %", 60));
            FarmMeNu.AddLabel("Orman Ayarlar�");
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
            HarassMeNu.AddGroupLabel("D�rtme Ayarlar�:");
            HarassMeNu.Add("UseQextendharass", new CheckBox("Q ile Zorla"));
            HarassMeNu.AddLabel("E yi �u durumda Kullan");
            foreach (var enemies in EntityManager.Heroes.Enemies.Where(i => !i.IsMe))
            {
                HarassMeNu.Add("Harass.E" + enemies.ChampionName, new CheckBox("" + enemies.ChampionName));
            }
            HarassMeNu.Add("harass.QE",
                new Slider("D�rtme b�y�leri i�in en az mana %", 55));
            HarassMeNu.AddLabel("Otomatik D�rtme Ayarlar�:");
            HarassMeNu.Add("AutoQextendharass", new CheckBox("Otomatik Q ile Hedefi Zorla"));
            HarassMeNu.Add("AutoHarassmana",
                new Slider("Otomatik D�rtme i�in en azmana %", 55));
            HarassMeNu.AddLabel("Kill�alma Ayarlar�:");
            HarassMeNu.Add("killsteal.Q",
                new CheckBox("Kullan Q", true));
        }

        private static void ActivatorPage()
        {
            Activator = _myMenu.AddSubMenu("Activator Settings", "Items");
            Activator.AddGroupLabel("Auto QSS if :");
            Activator.Add("Blind",
                new CheckBox("K�rse", false));
            Activator.Add("Charm",
                new CheckBox("�ekilmi�se(ahri)"));
            Activator.Add("Fear",
                new CheckBox("Korkmu�sa"));
            Activator.Add("Polymorph",
                new CheckBox("Polymorph(Lulu W)"));
            Activator.Add("Stun",
                new CheckBox("Sersemlemi�se"));
            Activator.Add("Snare",
                new CheckBox("Tuza�a D��m��se(cait)"));
            Activator.Add("Silence",
                new CheckBox("Susturulmu�sa", false));
            Activator.Add("Taunt",
                new CheckBox("Alay Ediliyorsa"));
            Activator.Add("Suppression",
                new CheckBox("WW,Urgot R(Suppression)"));
            Activator.Add("delay", new Slider("Gecikme Kullan", 100, 0, 500));
            Activator.AddLabel("�tem Kullan�mlar�:");
            Activator.Add("bilgewater",
                new CheckBox("Bilgewater palas� Kullan"));
            Activator.Add("bilgewater.HP",
                new Slider("Bilgewater Palas� Kullanmak i�in can �unun alt�nda {0}(%)", 60));
            Activator.Add("botrk",
                new CheckBox("Mahvolmu� K�l�� Kullan"));
            Activator.Add("botrk.HP",
                new Slider("Mahvolmu� K�l�� kullanmak i�in can �unun alt�nda {0}(%)", 60));
            Activator.Add("youmus",
                new CheckBox("Youmu'nun K�l�c� Kullan"));
            Activator.Add("items.Youmuss.HP",
                new Slider("Youumu'nun K�l�c� Kullanmak i�in can�m �undan altta {0}(%)", 60, 1));
            Activator.Add("youmus.Enemies",
                new Slider("Youumu'nun K�l�c� i�in menzilde {0} Kadar d��man olsun", 3, 1, 5));
            Activator.AddLabel("�ksir Ayarlar�");
            Activator.Add("spells.Potions.Check",
                new CheckBox("�ksirleri Kullan"));
            Activator.Add("spells.Potions.HP",
                new Slider("�ksir kullanmak i�in can�m �unun alt�nda {0}(%)", 60, 1));
            Activator.Add("spells.Potions.Mana",
                new Slider("�ksir Kullanmak i�in Manam �unun alt�nda {0}(%)", 60, 1));
            Activator.AddLabel("B�y� Ayarlar�:");
            Activator.AddLabel("�yile�tirme Ayarlar�:");
            Activator.Add("spells.Heal.Hp",
                new Slider("�yile�tirme i�in can�m �unun alt�nda {0}(%)", 30, 1));
            Activator.AddLabel("Tutu�tur Ayarlar�:");
            Activator.Add("spells.Ignite.Focus",
                new Slider("Tutu�tur i�in hedefin can� �unun alt�nda {0}(%)", 10, 1));
        }
        private static void FleeMenuPage()
        {
            FleeMenu = _myMenu.AddSubMenu("Flee Settings", "FleeSettings");
            FleeMenu.AddGroupLabel("Ka���(Flee) Ayarlar�");
            FleeMenu.Add("FleeE", new CheckBox("Ka�arken E Kullan"));
            FleeMenu.Add("FleeW", new CheckBox("Ka�arken W Kullan"));
        }

        private static void MiscMeNuPage()
        {
            MiscMeNu = _myMenu.AddSubMenu("Misc Menu", "othermenu");
            MiscMeNu.AddGroupLabel("Tehlikeli Durumlar� �nleyici Kullan");
            MiscMeNu.Add("gapcloser.E",
                new CheckBox("Rakibe Yakla�ma/Uzakla�ma i�in E Kullan(Gapclose)"));
            MiscMeNu.AddLabel("Kost�m Ayarlar�");
            MiscMeNu.Add("checkSkin",
                new CheckBox("Kost�m�n� Se�:", false));
            MiscMeNu.Add("skin.Id",
                new Slider("Kost�m Numaras�", 5, 0, 10));
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