using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using GuTenTak.Ezreal;
using SharpDX;

namespace GuTenTak.Ezreal
{
    internal class Program
    {
        public const string ChampionName = "Ezreal";
        public static Menu Menu, ModesMenu1, ModesMenu2, ModesMenu3, DrawMenu;
        public static int SkinBase;
        public static Item Youmuu = new Item(ItemId.Youmuus_Ghostblade);
        public static Item Botrk = new Item(ItemId.Blade_of_the_Ruined_King);
        public static Item Cutlass = new Item(ItemId.Bilgewater_Cutlass);
        public static Item Tear = new Item(ItemId.Tear_of_the_Goddess);
        public static Item Manamune = new Item(ItemId.Manamune);
        public static Item Qss = new Item(ItemId.Quicksilver_Sash);
        public static Item Simitar = new Item(ItemId.Mercurial_Scimitar);
        public static Item hextech = new Item(ItemId.Hextech_Gunblade, 700);

        /*public static AIHeroClient PlayerInstance
        {
            get { return Player.Instance; }
        }*/
        private static float HealthPercent()
        {
            return (Player.Instance.Health / Player.Instance.MaxHealth) * 100;
        }

        public static AIHeroClient _Player
        {
            get { return Player.Instance; }
        }

        public static bool AutoQ { get; protected set; }
        public static float Manaah { get; protected set; }
        public static object GameEvent { get; private set; }
        public static int LastTick = 0;
        public static IOrderedEnumerable<Obj_AI_Turret> enemyTurret=null;
        public static Spell.Skillshot Q;
        public static Spell.Skillshot W;
        public static Spell.Skillshot E;
        public static Spell.Skillshot R;

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Game_OnStart;
        }


        static void Game_OnStart(EventArgs args)
        {
            if (ChampionName != Player.Instance.BaseSkinName)
            {
                return;
            }
                
            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Game_OnDraw;
            Obj_AI_Base.OnBuffGain += Common.OnBuffGain;
            Gapcloser.OnGapcloser += Common.Gapcloser_OnGapCloser;
            Game.OnTick += OnTick;
            // Item
            SkinBase = Player.Instance.SkinId;
            try
            {
                Q = new Spell.Skillshot(SpellSlot.Q, 1150, SkillShotType.Linear, 250, 2000, 60);
                Q.AllowedCollisionCount = 0;
                W = new Spell.Skillshot(SpellSlot.W, 1000, SkillShotType.Linear, 250, 1600, 80);
                W.AllowedCollisionCount = int.MaxValue;
                E = new Spell.Skillshot(SpellSlot.E, 475, SkillShotType.Linear);
                E.AllowedCollisionCount = int.MaxValue;
                R = new Spell.Skillshot(SpellSlot.R, 3000, SkillShotType.Linear, 1000, 2000, 160);
                R.AllowedCollisionCount = int.MaxValue;



                Bootstrap.Init(null);
                Chat.Print("GuTenTak Addon Loading Success", Color.Green);


                Menu = MainMenu.AddMenu("GuTenTak Ezreal", "Ezreal");
                Menu.AddSeparator();
                Menu.AddLabel("GuTenTak Ezreal Addon");

                var Enemies = EntityManager.Heroes.Enemies.Where(a => !a.IsMe).OrderBy(a => a.BaseSkinName);
                ModesMenu1 = Menu.AddSubMenu("Menu", "Modes1Ezreal");
                ModesMenu1.AddSeparator();
                ModesMenu1.AddLabel("Kombo Ayarları");
                ModesMenu1.Add("ComboQ", new CheckBox("Q Kullan", true));
                ModesMenu1.Add("ComboA", new CheckBox("AA Kullan => Q Kombo", false));
                ModesMenu1.Add("ComboW", new CheckBox("W Kullan", true));
                ModesMenu1.Add("ComboR", new CheckBox("R Kullan", true));
                ModesMenu1.Add("ManaCW", new Slider("W için mana %", 30));
                ModesMenu1.Add("RCount", new Slider("R için düşman sayısı >=", 3, 2, 5));
                ModesMenu1.AddSeparator();
                ModesMenu1.AddSeparator();
                ModesMenu1.AddLabel("Otomatik dürtme Ayarları");
                ModesMenu1.Add("AutoHarass", new CheckBox("Otomatik dürtme Q", false));

                ModesMenu1.Add("ManaAuto", new Slider("Mana %", 80));
                ModesMenu1.AddLabel("Dürtme Ayarları");
                ModesMenu1.Add("HarassQ", new CheckBox("Q Kullan", true));
                ModesMenu1.Add("ManaHQ", new Slider("Mana %", 40));
                ModesMenu1.Add("HarassW", new CheckBox("W Kullan", true));
                ModesMenu1.Add("ManaHW", new Slider("Mana %", 60));
                ModesMenu1.AddSeparator();
                ModesMenu1.AddLabel("Kill Çalma Ayarları");
                ModesMenu1.Add("KS", new CheckBox("Use KillSteal", true));
                ModesMenu1.Add("KQ", new CheckBox("KS'de Q Kullan", true));
                ModesMenu1.Add("KW", new CheckBox("KS'de W Kullan", true));
                ModesMenu1.Add("KR", new CheckBox("KS'de R Kullan", true));

                ModesMenu2 = Menu.AddSubMenu("Farm", "Modes2Ezreal");
                ModesMenu2.AddLabel("SonVuruş Ayarları");
                ModesMenu2.Add("ManaF", new Slider("Mana %", 60));
                ModesMenu2.Add("LastQ", new CheckBox("Q Kullan", true));
                ModesMenu2.AddLabel("LaneTemizleme Ayarları");
                ModesMenu2.Add("ManaL", new Slider("Mana %", 40));
                ModesMenu2.Add("FarmQ", new CheckBox("Q Kullan", true));
                ModesMenu2.AddLabel("OrmanTemizleme Ayarları");
                ModesMenu2.Add("ManaJ", new Slider("Mana %", 40));
                ModesMenu2.Add("JungleQ", new CheckBox("Q Kullan", true));

                ModesMenu3 = Menu.AddSubMenu("Misc", "Modes3Ezreal");
                ModesMenu3.AddLabel("ek Ayarları");
                ModesMenu3.Add("AntiGap", new CheckBox("Use E for Anti-Gapcloser", true));
                ModesMenu3.Add("StackTear", new CheckBox("Auto StackTear in Shop ", true));
                ModesMenu3.AddLabel("Flee Ayarları");
                ModesMenu3.Add("FleeQ", new CheckBox("Kaçarken Q ", true));
                ModesMenu3.Add("FleeE", new CheckBox("E Kullan", true));
                ModesMenu3.Add("ManaFlQ", new Slider("Q Mana %", 35));

                ModesMenu3.AddLabel("Komboda kullnaılacak itemler");
                ModesMenu3.Add("useYoumuu", new CheckBox("Kullan Youmuu", true));
                ModesMenu3.Add("usehextech", new CheckBox("Kullan Hextech", true));
                ModesMenu3.Add("useBotrk", new CheckBox("Kullan Mahvolmuş", true));
                ModesMenu3.Add("useQss", new CheckBox("Use QuickSilver", true));
                ModesMenu3.Add("minHPBotrk", new Slider("Mahvolmuş için gereken can %", 80));
                ModesMenu3.Add("enemyMinHPBotrk", new Slider("Mahvolmuş için gereken düşman can %", 80));

                ModesMenu3.AddLabel("QSS Ayarları");
                ModesMenu3.Add("Qssmode", new ComboBox(" ", 0, "Auto", "Combo"));
                ModesMenu3.Add("Stun", new CheckBox("Sabit", true));
                ModesMenu3.Add("Blind", new CheckBox("Kör", true));
                ModesMenu3.Add("Charm", new CheckBox("Çekicilik(ahri)", true));
                ModesMenu3.Add("Suppression", new CheckBox("Suppression", true));
                ModesMenu3.Add("Polymorph", new CheckBox("Polymorph", true));
                ModesMenu3.Add("Fear", new CheckBox("Korku", true));
                ModesMenu3.Add("Taunt", new CheckBox("Alaycı", true));
                ModesMenu3.Add("Silence", new CheckBox("Sessiz", false));
                ModesMenu3.Add("QssDelay", new Slider("QSS gecikmesi(ms)", 250, 0, 1000));

                ModesMenu3.AddLabel("QSS Ult Ayarları");
                ModesMenu3.Add("ZedUlt", new CheckBox("Zed R", true));
                ModesMenu3.Add("VladUlt", new CheckBox("Vladimir R", true));
                ModesMenu3.Add("FizzUlt", new CheckBox("Fizz R", true));
                ModesMenu3.Add("MordUlt", new CheckBox("Mordekaiser R", true));
                ModesMenu3.Add("PoppyUlt", new CheckBox("Poppy R", true));
                ModesMenu3.Add("QssUltDelay", new Slider("Kullan QSS Gecikmesi(ms) Ulti için", 250, 0, 1000));

                ModesMenu3.AddLabel("Skin Hack");
                ModesMenu3.Add("skinhack", new CheckBox("Aktif Skin hack", false));
                ModesMenu3.Add("skinId", new ComboBox("Skin Mode", 0, "Default", "1", "2", "3", "4", "5", "6", "7", "8"));

                DrawMenu = Menu.AddSubMenu("Draws", "DrawEzreal");
                DrawMenu.Add("drawQ", new CheckBox(" Göster Q", true));
                DrawMenu.Add("drawW", new CheckBox(" Göster W", true));
                DrawMenu.Add("drawR", new CheckBox(" Göster R", false));
                DrawMenu.Add("drawXR", new CheckBox(" Göster R kullanmadında", true));
                DrawMenu.Add("drawXFleeQ", new CheckBox(" Göster kaçarken Q gösterme", false));

            }

            catch (Exception e)
            {

            }

        }
        private static void Game_OnDraw(EventArgs args)
        {

            try
            {
                if (DrawMenu["drawQ"].Cast<CheckBox>().CurrentValue)
                {
                    if (Q.IsReady() && Q.IsLearned)
                    {
                        Circle.Draw(Color.White, Q.Range, Player.Instance.Position);
                    }
                }
                if (DrawMenu["drawW"].Cast<CheckBox>().CurrentValue)
                {
                    if (W.IsReady() && W.IsLearned)
                    {
                        Circle.Draw(Color.White, W.Range, Player.Instance.Position);
                    }
                }
                if (DrawMenu["drawR"].Cast<CheckBox>().CurrentValue)
                {
                    if (R.IsReady() && R.IsLearned)
                    {
                        Circle.Draw(Color.White, R.Range, Player.Instance.Position);
                    }
                }
                if (DrawMenu["drawXR"].Cast<CheckBox>().CurrentValue)
                {
                    if (R.IsReady() && R.IsLearned)
                    {
                        Circle.Draw(Color.Red, 700, Player.Instance.Position);
                    }
                }
                if (DrawMenu["drawXFleeQ"].Cast<CheckBox>().CurrentValue)
                {
                    if (Q.IsReady() && Q.IsLearned)
                    {
                        Circle.Draw(Color.Red, 400, Player.Instance.Position);
                    }
                }
            }
            catch (Exception e)
            {

            }
        }
        static void Game_OnUpdate(EventArgs args)
        {
            try
            {
                var AutoHarass = ModesMenu1["AutoHarass"].Cast<CheckBox>().CurrentValue;
                var ManaAuto = ModesMenu1["ManaAuto"].Cast<Slider>().CurrentValue;

                Common.Skinhack();


                if (AutoHarass && ManaAuto <= Player.Instance.ManaPercent)
                {
                    Common.AutoQ();
                }

                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                {
                    Common.Combo();
                    Common.ItemUsage();
                }
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
                {
                    Common.Harass();
                }

                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
                {

                    Common.LaneClear();

                }

                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
                {

                    Common.JungleClear();
                }

                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
                {
                    Common.LastHit();

                }
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
                {
                    Common.Flee();

                }
            }
            catch (Exception e)
            {

            }
        }
        public static void OnTick(EventArgs args)
        {
            if (ModesMenu1["ComboA"].Cast<CheckBox>().CurrentValue && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Orbwalker.OnPostAttack += Common.Orbwalker_OnPostAttack;
            }
            else
            {
                Orbwalker.OnPostAttack -= Common.Orbwalker_OnPostAttack;
            }
            Common.KillSteal();
            Common.StackTear();
            /*if (ModesMenu3["BlockE"].Cast<CheckBox>().CurrentValue && Environment.TickCount - LastTick > 1500)
            {
                enemyTurret = ObjectManager.Get<Obj_AI_Turret>().Where(tur => tur.IsEnemy && tur.Health > 0)
                .OrderBy(tur => tur.Distance(Player.Instance.Position));
                LastTick = Environment.TickCount;
            }*/
        }
    }
}
