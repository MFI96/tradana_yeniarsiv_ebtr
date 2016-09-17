using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;

namespace KTrundle
{
    internal class Program
    {
        public const string ChampionName = "Trundle";
        public static Menu Menu, ModesMenu1, ModesMenu2, DrawMenu, Misc;
        public static AIHeroClient PlayerInstance
        {
            get { return Player.Instance; }
        }
        private static float HealthPercent()
        {
            return (PlayerInstance.Health / PlayerInstance.MaxHealth) * 100;
        }

        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }



        public static Spell.Active Q;
        public static Spell.Skillshot W;
        public static Spell.Skillshot E;
        public static Spell.Targeted R;


        static void Main(string[] args)
        {
            //gamee
            Loading.OnLoadingComplete += Game_OnStart;
            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Game_OnDraw;
            //GameObject.OnCreate += Game_ObjectCreate;
            //GameObject.OnDelete += Game_OnDelete;
            //Orbwalker.OnPostAttack += Reset;
            Game.OnTick += Game_OnTick;
            Interrupter.OnInterruptableSpell += KInterrupter;
            Gapcloser.OnGapcloser += KGapCloser;

        }
        static void Game_OnStart(EventArgs args)
        {

            try
            {
                if (ChampionName != PlayerInstance.BaseSkinName)
                {
                    return;
                }

                Bootstrap.Init(null);
                Chat.Print("KTrundle Başarıyla yüklendi", Color.Green);

                Q = new Spell.Active(SpellSlot.Q, 125);
                W = new Spell.Skillshot(SpellSlot.W, 900, SkillShotType.Circular, 1, 2000, 900);
                W.AllowedCollisionCount = int.MaxValue;
                E = new Spell.Skillshot(SpellSlot.E, 1000, SkillShotType.Circular, 1, 1600, 188);
                E.AllowedCollisionCount = int.MaxValue;
                R = new Spell.Targeted(SpellSlot.R, 700);

                Menu = MainMenu.AddMenu("KTrundle", "Trundle");
                Menu.AddSeparator();
                Menu.AddLabel("Bruno105 Tarafından");
                Menu.AddLabel("Çeviri TRAdana");


                //------------//
                //-Mode Menu-//
                //-----------//

                var Enemies = EntityManager.Heroes.Enemies.Where(a => !a.IsMe).OrderBy(a => a.BaseSkinName);
                ModesMenu1 = Menu.AddSubMenu("Combo/Harass/KS", "Modes1Trundle");
                ModesMenu1.AddSeparator();
                ModesMenu1.AddLabel("Combo Ayarları");
                ModesMenu1.Add("ComboQ", new CheckBox("Q kullan", true));
                ModesMenu1.Add("ComboW", new CheckBox("W kullan", true));
                ModesMenu1.Add("ComboE", new CheckBox("E kullan", true));
                ModesMenu1.Add("ComboR", new CheckBox("R kullan", true));
                ModesMenu1.AddSeparator();
                ModesMenu1.AddLabel("R kullan şurda:");
                foreach (var a in Enemies)
                {
                    ModesMenu1.Add("Ult_" + a.BaseSkinName, new CheckBox(a.BaseSkinName));
                }
                ModesMenu1.Add("useI", new CheckBox("Komboda itemler kullan", true));
                ModesMenu1.AddSeparator();
                ModesMenu1.AddLabel("Dürtme Ayarları");
                ModesMenu1.Add("ManaH", new Slider("Manam şundan azsa kullanma <=", 40));
                ModesMenu1.Add("HarassQ", new CheckBox("Q kullan", true));
                ModesMenu1.Add("HarassW", new CheckBox("W kullan", true));
                ModesMenu1.AddSeparator();
                ModesMenu1.AddLabel("Kill çakma Ayarları");
                ModesMenu1.Add("KQ", new CheckBox("Q kullan", true));

                ModesMenu2 = Menu.AddSubMenu("Lane/Jungle/Last", "Modes2Trundle");
                ModesMenu2.AddLabel("Sonvuruş Ayarları");
                ModesMenu2.Add("ManaL", new Slider("Mana şundan azsa büyü kullanma <= ", 40));
                ModesMenu2.Add("LastQ", new CheckBox("Q kullan", true));
                ModesMenu2.AddLabel("Lanetemizleme Ayarları");
                ModesMenu2.Add("ManaF", new Slider("Mana şundan azsa büyü kullanma <=", 40));
                ModesMenu2.Add("FarmQ", new CheckBox("Q kullan", true));
                ModesMenu2.Add("FarmW", new CheckBox("W kullan", true));
                ModesMenu2.Add("MinionW", new Slider("W için şu kadar minyon :", 3, 1, 5));
                ModesMenu2.AddLabel("Orman Temizleme Ayarları");
                ModesMenu2.Add("ManaJ", new Slider("Mana şundan azsa büyü kullanma <=", 40));
                ModesMenu2.Add("JungQ", new CheckBox("Q kullan", true));
                ModesMenu2.Add("JungW", new CheckBox("W kullan", true));



                //------------//
                //-Draw Menu-//
                //----------//
                DrawMenu = Menu.AddSubMenu("Draws", "DrawTrundle");
                DrawMenu.Add("drawAA", new CheckBox("Göster  AA", true));
                DrawMenu.Add("drawQ", new CheckBox(" Göster  Q", true));
                DrawMenu.Add("drawE", new CheckBox(" Göster  E", true));
                DrawMenu.Add("drawR", new CheckBox(" Göster  R", true));
                //------------//
                //-Misc Menu-//
                //----------//
                Misc = Menu.AddSubMenu("MiscMenu", "Misc");
                Misc.Add("useEGapCloser", new CheckBox("GapCloser için E", true));
                Misc.Add("useEInterrupter", new CheckBox("Interrupt için E", true));
                Misc.Add("resetAA", new CheckBox("AA sıfırla"));
            }

            catch (Exception e)
            {
                Chat.Print("KTrundle: Exception occured while Initializing Addon. Error: " + e.Message);

            }

        }
        private static void Game_OnDraw(EventArgs args)
        {

            Circle.Draw(Color.Red, _Player.GetAutoAttackRange(), Player.Instance.Position);
            if (Q.IsReady() && Q.IsLearned)
            {
                Circle.Draw(Color.White, Q.Range, Player.Instance.Position);
            }
            if (W.IsReady() && W.IsLearned)
            {
                Circle.Draw(Color.Green, W.Range, Player.Instance.Position);
            }
            if (E.IsReady() && E.IsLearned)
            {
                Circle.Draw(Color.Aqua, E.Range, Player.Instance.Position);
            }
            if (R.IsReady() && R.IsLearned)
            {
                Circle.Draw(Color.Black, R.Range, Player.Instance.Position);
            }
        }
        static void Game_OnUpdate(EventArgs args)
        {


            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                ModesManager.Combo();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                ModesManager.Harass();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {

                ModesManager.LaneClear();

            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {

                ModesManager.JungleClear();
            }



            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                ModesManager.LastHit();

            }



        }
        public static void Game_OnTick(EventArgs args)
        {
            ModesManager.KillSteal();

        }
        static void KInterrupter(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {

            if (args.DangerLevel == DangerLevel.High && sender.IsEnemy && sender is AIHeroClient && sender.Distance(_Player) < E.Range && E.IsReady() && Misc["useEInterrupter"].Cast<CheckBox>().CurrentValue)
            {
                E.Cast(sender);
            }


        }
        static void KGapCloser(Obj_AI_Base sender, Gapcloser.GapcloserEventArgs args)
        {


            if (sender.IsEnemy && sender is AIHeroClient && sender.Distance(_Player) < E.Range && E.IsReady() && Misc["useEGapCloser"].Cast<CheckBox>().CurrentValue)
            {
                E.Cast(sender);
            }
        }
    }
}
