using System.Linq;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace Fapturne
{
    internal static class FapturneMenu
    {
        private static Menu MyMenu;
        public static Menu MyCombo, MyDraw, MyHarass, MyActivator, MyFarm, MyOtherFunctions;

        public static void LoadMenu()
        {
            MyFapturnePage();
            MyDrawPage();
            MyComboPage();
            MyFarmPage();
            MyHarassPage();
            MyActivatorPage();
            MyOtherFunctionsPage();
        }

        private static void MyFapturnePage()
        {
            MyMenu = MainMenu.AddMenu("Fapturne", "main");
            MyMenu.AddGroupLabel("About this script:");
            MyMenu.AddLabel(" Fapturne - " + Program.Version);
            MyMenu.AddLabel(" Made by -iRaxe");
            MyMenu.AddSeparator();
            MyMenu.AddGroupLabel("Hotkeys");
            MyMenu.AddLabel(" - Kombo i�in space");
            MyMenu.AddLabel(" - V �unlar i�in LaneClear/JungleClear");
            MyMenu.AddLabel(" - T ka�mak i�in");
        }

        private static void MyDrawPage()
        {
            MyDraw = MyMenu.AddSubMenu("Draw  settings", "Draw");
            MyDraw.AddGroupLabel("G�ster Ayarlar�:");
            MyDraw.Add("nodraw", 
                new CheckBox("G�sterme hi�bir�ey", false));
            MyDraw.Add("onlyReady", 
                new CheckBox("Haz�rsa g�ster"));
            MyDraw.AddSeparator();
            MyDraw.Add("draw.Q", 
                new CheckBox("Q G�ster"));
            MyDraw.Add("draw.W", 
                new CheckBox("W G�ster"));
            MyDraw.Add("draw.E", 
                new CheckBox("E G�ster"));
            MyDraw.Add("draw.R", 
                new CheckBox("R G�ster"));
            MyDraw.Add("draw.T",
                new CheckBox("Ward at�lan yerleri g�ster"));
            MyDraw.AddSeparator();
            MyDraw.AddGroupLabel("Pro Tips");
            MyDraw.AddLabel(" - Uncheck the boxeses if you wish to dont see a specific draw");
        }

        private static void MyComboPage()
        {
            MyCombo = MyMenu.AddSubMenu("Combo settings", "Combo");
            MyCombo.AddGroupLabel("Combo Ayarlar�:");
            MyCombo.AddLabel("Q Kullan");
            foreach (var enemies in EntityManager.Heroes.Enemies.Where(i => !i.IsMe))
            {
                MyCombo.Add("combo.q" + enemies.ChampionName, new CheckBox("" + enemies.ChampionName));
            }
            MyCombo.Add("combo.qh", new Slider("Set your prediction value", 70));

            MyCombo.Add("combo.w",
                new CheckBox("W  Kullan"));
            MyCombo.AddLabel("E Kullan");
            foreach (var enemies in EntityManager.Heroes.Enemies.Where(i => !i.IsMe))
            {
                MyCombo.Add("combo.e" + enemies.ChampionName, new CheckBox("" + enemies.ChampionName));
            }
            MyCombo.AddLabel("R Kullan");
            foreach (var enemies in EntityManager.Heroes.Enemies.Where(i => !i.IsMe))
            {
                MyCombo.Add("combo.r" + enemies.ChampionName, new CheckBox("" + enemies.ChampionName));
            }
            MyCombo.AddSeparator();
            MyCombo.AddGroupLabel("Combo �zellikleri:");
            MyCombo.Add("combo.CC", 
                new CheckBox("CC �zerinde Q kullan"));
            MyCombo.Add("combo.CC1",
                new CheckBox("CC �zerinde E Kullan"));
            MyCombo.Add("combo.Q1", 
                new Slider("H�zl� �lecek hedefe Q kullan", 60, 0, 500));
            MyCombo.Add("combo.R1", 
                new Slider("�ld�r�lebilecek hedefe R Kullan", 50, 0, 500));
            MyCombo.AddSeparator();
            MyCombo.AddGroupLabel("Pro Tips");
            MyCombo.AddLabel(
                " -Uncheck the boxes if you wish to dont use a specific spell while you are pressing the Combo Key");
        }

        private static void MyFarmPage()
        {
            MyFarm = MyMenu.AddSubMenu("Lane Clear Settings", "laneclear");
            MyFarm.AddGroupLabel("LaneTemizleme Ayarlar�:");
            MyFarm.Add("lc.Q", 
                new CheckBox("Q Kullan"));
            MyFarm.Add("lc.Q2",
                new CheckBox("Q2 Kullan", false));
            MyFarm.Add("lc.Q1",
                new Slider("Q1 i�in en az d��man", 3, 1, 10));
            MyFarm.AddSeparator();
            MyFarm.AddSeparator();
            MyFarm.Add("lc.E", 
                new CheckBox("E Kullan", false));
            MyFarm.Add("lc.M", 
                new Slider("Kullanmak i�in en az mana %", 30));
            MyFarm.AddSeparator();
            MyFarm.AddGroupLabel("OrmanTemizleme Ayarlar�");
            MyFarm.Add("jungle.Q", 
                new CheckBox("Q Kullan"));
            MyFarm.Add("jungle.E", 
                new CheckBox("E Kullan"));
            MyFarm.AddSeparator();
            MyFarm.AddGroupLabel("Pro Tips");
            MyFarm.AddLabel(
                " -Uncheck the boxes if you wish to dont use a specific spell while you are pressing the Jungle/LaneClear Key");
        }

        private static void MyHarassPage()
        {
            MyHarass = MyMenu.AddSubMenu("Harass/Killsteal Settings", "hksettings");
            MyHarass.AddGroupLabel("D�rtme Ayarlar�:");
            MyHarass.AddSeparator();
            MyHarass.AddLabel("Q Kullan");
            foreach (var enemies in EntityManager.Heroes.Enemies.Where(i => !i.IsMe))
            {
                MyHarass.Add("harass.Q" + enemies.ChampionName, new CheckBox("" + enemies.ChampionName));
            }
            MyHarass.AddLabel("E Kullan");
            foreach (var enemies in EntityManager.Heroes.Enemies.Where(i => !i.IsMe))
            {
                MyHarass.Add("harass.E" + enemies.ChampionName, new CheckBox("" + enemies.ChampionName));
            }
            MyHarass.Add("harass.QE", 
                new Slider("D�rtme i�in en az mana %", 35));
            MyHarass.AddSeparator();
            MyHarass.AddGroupLabel("KillSteal Ayarlar�:");
            MyHarass.Add("killsteal.Q", 
                new CheckBox("Q Kullan", false));
                MyHarass.Add("killsteal.E",
                new CheckBox("E Kullan", false));
            MyHarass.Add("killsteal.R", 
                new CheckBox("R Kullan"));
            MyHarass.AddSeparator();
            MyHarass.AddGroupLabel("Pro Tips");
            MyHarass.AddLabel(" -Remember to play safe and don't be a teemo");
        }

        private static void MyActivatorPage()
        {
            MyActivator = MyMenu.AddSubMenu("Activator Settings", "Items");
            MyActivator.AddGroupLabel("Auto Shield/QSS if :");
            MyActivator.Add("Blind",
                new CheckBox("K�r", false));
            MyActivator.Add("Charm",
                new CheckBox("�ekicilik(ahri)"));
            MyActivator.Add("Fear",
                new CheckBox("Korku"));
            MyActivator.Add("Polymorph",
                new CheckBox("Polymorph"));
            MyActivator.Add("Stun",
                new CheckBox("Sabitleme"));
            MyActivator.Add("Snare",
                new CheckBox("Tuza�a d��me"));
            MyActivator.Add("Silence",
                new CheckBox("Sessiz", false));
            MyActivator.Add("Taunt",
                new CheckBox("Alay Etme"));
            MyActivator.Add("Suppression",
                new CheckBox("Suppression"));
            MyActivator.AddGroupLabel("Ultiler");
            MyActivator.Add("ZedUlt",
                new CheckBox("Zed Ult"));
            MyActivator.Add("VladUlt",
                new CheckBox("Vlad Ult"));
            MyActivator.Add("FizzUlt",
                new CheckBox("Fizz Ult"));
            MyActivator.Add("MordUlt",
                new CheckBox("Mordekaiser Ult"));
            MyActivator.Add("PoppyUlt",
                new CheckBox("Poppy Ult"));
            MyActivator.AddGroupLabel("Items Kullan�m�:");
            MyActivator.AddSeparator();
                        MyActivator.Add("checkward",
                new CheckBox("Otomatik ward", false));
            MyActivator.Add("pinkvision",
                new CheckBox("Pembe totem", false));
            MyActivator.Add("greatherstealthtotem",
                new CheckBox("G�r�nmez hedefe totem", false));
            MyActivator.Add("greatervisiontotem",
                new CheckBox("G�sterici totem", false));
            MyActivator.Add("wardingtotem",
                new CheckBox("Totem kullan", false));
            MyActivator.Add("farsightalteration",
                new CheckBox("Use Farsight Alteration", false));
            MyActivator.AddSeparator();
            MyActivator.Add("bilgewater", 
                new CheckBox("Kullan Bilgewater Palas�"));
            MyActivator.Add("bilgewater.HP", 
                new Slider("Kullan e�er can �undan d���kse {0}(%)", 60));
            MyActivator.AddSeparator();
            MyActivator.Add("botrk", 
                new CheckBox("Kullan Mahvolmu� k�l��"));
            MyActivator.Add("botrk.HP",
                new Slider("Kullan Mhavolmu� k�l�� e�er can �undan d���kse {0}(%)", 60));
            MyActivator.AddSeparator();
            MyActivator.Add("youmus", 
                new CheckBox("Kullan Youmus"));
            MyActivator.Add("items.Youmuss.HP",
                new Slider("Youumu i�in can �undan d���kse {0}(%)", 60, 1));
            MyActivator.Add("youmus.Enemies",
                new Slider("Youmu i�in menzilde gereken d��man", 3, 1, 5));
            MyActivator.Add("hydra",
               new CheckBox("Kullan Hydra"));
            MyActivator.Add("hydra.HP",
               new Slider("Kullan Can �undan az {0}(%)", 60, 1));
            MyActivator.Add("tiamat",
                new CheckBox("Kullan Tiamat"));
            MyActivator.Add("tiamat.HP",
               new Slider("Kullan can� �undan az {0}(%)", 60, 1));
            MyActivator.Add("hextech",
                new CheckBox("Kullan hextech k�l�c�"));
            MyActivator.Add("hextech.HP",
               new Slider("Kullanmak i�in can �undan az {0}(%)", 60, 1));
            MyActivator.AddSeparator();
            MyActivator.AddGroupLabel("�ksir Ayarlar�");
            MyActivator.Add("spells.Potions.Check", 
                new CheckBox("Kullan �ksir"));
            MyActivator.Add("spells.Potions.HP", 
                new Slider("�ksir i�in gerekli can {0}(%)", 60, 1));
            MyActivator.Add("spells.Potions.Mana", 
                new Slider("�ksir i�in gerekli mana {0}(%)", 60, 1));
            MyActivator.AddSeparator();
            MyActivator.AddGroupLabel("Spells settings:");
            MyActivator.AddGroupLabel("Smite settings");
            MyActivator.Add("SRU_Red", new CheckBox("K�rm�z�"));
            MyActivator.Add("SRU_Blue", new CheckBox("MAvi"));
            MyActivator.Add("SRU_Dragon", new CheckBox("Ejder"));
            MyActivator.Add("SRU_Baron", new CheckBox("Baron"));
            MyActivator.Add("SRU_Gromp", new CheckBox("Gromp"));
            MyActivator.Add("SRU_Murkwolf", new CheckBox("Kurtlar"));
            MyActivator.Add("SRU_Razorbeak", new CheckBox("Sivrigagalar"));
            MyActivator.Add("SRU_Krug", new CheckBox("Golem"));
            MyActivator.Add("Sru_Crab", new CheckBox("Yampiri yenge�"));
            MyActivator.AddSeparator();
            MyActivator.AddGroupLabel("Barrier settings:");
            MyActivator.Add("spells.Barrier.Hp", 
                new Slider("Use Barrier when HP is lower than {0}(%)", 30, 1));
            MyActivator.AddGroupLabel("Can Ayarlar�:");
            MyActivator.Add("spells.Heal.Hp", 
                new Slider("Can kullanmak i�in can�m �undan az {0}(%)", 30, 1));
            MyActivator.AddGroupLabel("Tutu�tur Ayarlar�:");
            MyActivator.Add("spells.Ignite.Focus", 
                new Slider("Hedefin can� �undna azsa tutu�tur kullan {0}(%)", 10, 1));
        }

        private static void MyOtherFunctionsPage()
        {
            MyOtherFunctions = MyMenu.AddSubMenu("Misc Menu", "othermenu");
            MyOtherFunctions.AddGroupLabel("Ayarlar i�in GapCloser/Interrupter");
            MyOtherFunctions.Add("interrupt.E",
                new CheckBox("E i�in Interrupt"));
            MyOtherFunctions.Add("gapcloser.E",
                new CheckBox("E i�in Anti-Gap"));
            MyOtherFunctions.Add("gapcloser.Q",
                new CheckBox("Q i�in Anti-Gap"));
            MyOtherFunctions.AddSeparator();
            MyOtherFunctions.AddGroupLabel("Ka�ma ayarlar�");
            MyOtherFunctions.Add("flee.M", 
                new Slider("Q kullan mana �undan y�ksekse {0}(%)", 10, 1));
            MyOtherFunctions.AddSeparator();
            MyOtherFunctions.AddGroupLabel("Level Up Ayarlar�");
            MyOtherFunctions.Add("lvlup", 
                new CheckBox("Otomatik level y�kseltme:", false));
            MyOtherFunctions.AddSeparator();
            MyOtherFunctions.AddGroupLabel("Skin Ayarlar�");
            MyOtherFunctions.Add("checkSkin",
                new CheckBox("Kullan skin De�i�tirici:"));
            MyOtherFunctions.Add("skin.Id", 
                new Slider("Skin Numaras�", 3, 0, 6));
        }

        public static bool Nodraw()
        {
            return MyDraw["nodraw"].Cast<CheckBox>().CurrentValue;
        }

        public static bool OnlyReady()
        {
            return MyDraw["onlyReady"].Cast<CheckBox>().CurrentValue;
        }

        public static bool DrawingsQ()
        {
            return MyDraw["draw.Q"].Cast<CheckBox>().CurrentValue;
        }

        public static bool DrawingsW()
        {
            return MyDraw["draw.W"].Cast<CheckBox>().CurrentValue;
        }

        public static bool DrawingsE()
        {
            return MyDraw["draw.E"].Cast<CheckBox>().CurrentValue;
        }

        public static bool DrawingsR()
        {
            return MyDraw["draw.R"].Cast<CheckBox>().CurrentValue;
        }
        public static bool DrawingsT()
        {
            return MyDraw["draw.T"].Cast<CheckBox>().CurrentValue;
        }

        public static float ComboQ1()
        {
            return MyCombo["combo.Q1"].Cast<Slider>().CurrentValue;
        }
        public static bool ComboW()
        {
            return MyCombo["combo.w"].Cast<CheckBox>().CurrentValue;
        }

        public static bool ComboCC()
        {
            return MyCombo["combo.CC"].Cast<CheckBox>().CurrentValue;
        }

        public static bool ComboCC1()
        {
            return MyCombo["combo.CC1"].Cast<CheckBox>().CurrentValue;
        }

        public static float ComboQH()
        {
            return MyCombo["combo.qh"].Cast<Slider>().CurrentValue;
        }

        public static float ComboR1()
        {
            return MyCombo["combo.R1"].Cast<Slider>().CurrentValue;
        }

        public static bool LcQ()
        {
            return MyFarm["lc.Q"].Cast<CheckBox>().CurrentValue;
        }

        public static float LcQ1()
        {
            return MyFarm["lc.Q1"].Cast<Slider>().CurrentValue;
        }

        public static bool LcQ2()
        {
            return MyFarm["lc.Q2"].Cast<CheckBox>().CurrentValue;
        }

        public static bool LcE()
        {
            return MyFarm["lc.E"].Cast<CheckBox>().CurrentValue;
        }

        public static float LcM()
        {
            return MyFarm["lc.M"].Cast<Slider>().CurrentValue;
        }

        public static bool JungleQ()
        {
            return MyFarm["jungle.Q"].Cast<CheckBox>().CurrentValue;
        }

        public static bool JungleE()
        {
            return MyFarm["jungle.E"].Cast<CheckBox>().CurrentValue;
        }

        public static float HarassQE()
        {
            return MyHarass["harass.QE"].Cast<Slider>().CurrentValue;
        }

        public static bool KillstealQ()
        {
            return MyHarass["killsteal.Q"].Cast<CheckBox>().CurrentValue;
        }

        public static bool KillstealE()
        {
            return MyHarass["killsteal.E"].Cast<CheckBox>().CurrentValue;
        }

        public static bool KillstealR()
        {
            return MyHarass["killsteal.R"].Cast<CheckBox>().CurrentValue;
        }

        public static bool Bilgewater()
        {
            return MyActivator["bilgewater"].Cast<CheckBox>().CurrentValue;
        }

        public static float BilgewaterHp()
        {
            return MyActivator["bilgewater.HP"].Cast<Slider>().CurrentValue;
        }

        public static bool Botrk()
        {
            return MyActivator["botrk"].Cast<CheckBox>().CurrentValue;
        }

        public static float BotrkHp()
        {
            return MyActivator["botrk.HP"].Cast<Slider>().CurrentValue;
        }

        public static bool Youmus()
        {
            return MyActivator["youmus"].Cast<CheckBox>().CurrentValue;
        }
        public static bool checkWard()
        {
            return MyActivator["checkward"].Cast<CheckBox>().CurrentValue;
        }
        public static bool pinkWard()
        {
            return MyActivator["pinkvision"].Cast<CheckBox>().CurrentValue;
        }
        public static bool greaterStealthTotem()
        {
            return MyActivator["greaterstealthtotem"].Cast<CheckBox>().CurrentValue;
        }
        public static bool greaterVisionTotem()
        {
            return MyActivator["greatervisiontotem"].Cast<CheckBox>().CurrentValue;
        }
        public static bool farsightAlteration()
        {
            return MyActivator["farsightalteration"].Cast<CheckBox>().CurrentValue;
        }
        public static bool wardingTotem()
        {
            return MyActivator["wardingtotem"].Cast<CheckBox>().CurrentValue;
        }

        public static float YoumusEnemies()
        {
            return MyActivator["youmus.Enemies"].Cast<Slider>().CurrentValue;
        }

        public static float ItemsYoumuShp()
        {
            return MyActivator["items.Youmuss.HP"].Cast<Slider>().CurrentValue;
        }
        public static bool SpellsPotionsCheck()
        {
            return MyActivator["spells.Potions.Check"].Cast<CheckBox>().CurrentValue;
        }
        public static float SpellsPotionsHP()
        {
            return MyActivator["spells.Potions.HP"].Cast<Slider>().CurrentValue;
        }
        public static float SpellsPotionsM()
        {
            return MyActivator["spells.Potions.Mana"].Cast<Slider>().CurrentValue;
        }
        public static float SpellsHealHp()
        {
            return MyActivator["spells.Heal.HP"].Cast<Slider>().CurrentValue;
        }

        public static float SpellsIgniteFocus()
        {
            return MyActivator["spells.Ignite.Focus"].Cast<Slider>().CurrentValue;
        }

        public static float spellsBarrierHP()
        {
            return MyActivator["spells.Barrier.Hp"].Cast<Slider>().CurrentValue;
        }

        public static bool Blind()
        {
            return MyActivator["Blind"].Cast<CheckBox>().CurrentValue;
        }

        public static bool Charm()
        {
            return MyActivator["Charm"].Cast<CheckBox>().CurrentValue;
        }

        public static bool Fear()
        {
            return MyActivator["Fear"].Cast<CheckBox>().CurrentValue;
        }

        public static bool Polymorph()
        {
            return MyActivator["Polymorph"].Cast<CheckBox>().CurrentValue;
        }

        public static bool Stun()
        {
            return MyActivator["Stun"].Cast<CheckBox>().CurrentValue;
        }

        public static bool Snare()
        {
            return MyActivator["Snare"].Cast<CheckBox>().CurrentValue;
        }

        public static bool Silence()
        {
            return MyActivator["Silence"].Cast<CheckBox>().CurrentValue;
        }

        public static bool Taunt()
        {
            return MyActivator["Taunt"].Cast<CheckBox>().CurrentValue;
        }
        public static bool Suppression()
        {
            return MyActivator["Suppression"].Cast<CheckBox>().CurrentValue;
        }

        public static bool ZedUlt()
        {
            return MyActivator["ZedUlt"].Cast<CheckBox>().CurrentValue;
        }

        public static bool VladUlt()
        {
            return MyActivator["VladUlt"].Cast<CheckBox>().CurrentValue;
        }

        public static bool FizzUlt()
        {
            return MyActivator["FizzUlt"].Cast<CheckBox>().CurrentValue;
        }

        public static bool MordUlt()
        {
            return MyActivator["MordUlt"].Cast<CheckBox>().CurrentValue;
        }

        public static bool PoppyUlt()
        {
            return MyActivator["PoppyUlt"].Cast<CheckBox>().CurrentValue;
        }

        public static bool tiamat()
        {
            return MyActivator["tiamat"].Cast<CheckBox>().CurrentValue;
        }

        public static bool hydra()
        {
            return MyActivator["hydra"].Cast<CheckBox>().CurrentValue;
        }

        public static bool gunblade()
        {
            return MyActivator["hextech"].Cast<CheckBox>().CurrentValue;
        }

        public static float tiamatHP()
        {
            return MyActivator["tiamat.Hp"].Cast<Slider>().CurrentValue;
        }

        public static float hydraHP()
        {
            return MyActivator["hydra.Hp"].Cast<Slider>().CurrentValue;
        }

        public static float gunbladeHP()
        {
            return MyActivator["hextech.Hp"].Cast<Slider>().CurrentValue;
        }

        public static int SkinId()
        {
            return MyOtherFunctions["skin.Id"].Cast<Slider>().CurrentValue;
        }

        public static bool Lvlup()
        {
            return MyOtherFunctions["lvlup"].Cast<CheckBox>().CurrentValue;
        }
        public static float FleeM()
        {
            return MyOtherFunctions["flee.M"].Cast<Slider>().CurrentValue;
        }
        public static bool checkSkin()
        {
            return MyOtherFunctions["checkSkin"].Cast<CheckBox>().CurrentValue;
        }
        public static bool interruptE()
        {
            return MyOtherFunctions["interrupt.E"].Cast<CheckBox>().CurrentValue;
        }
        public static bool gapcloserE()
        {
            return MyOtherFunctions["gapcloser.E"].Cast<CheckBox>().CurrentValue;
        }
        public static bool gapcloserQ()
        {
            return MyOtherFunctions["gapcloser.Q"].Cast<CheckBox>().CurrentValue;
        }
    


    }
}
