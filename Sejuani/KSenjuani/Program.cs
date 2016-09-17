using System;
using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using Color = System.Drawing.Color;
using SharpDX;



namespace KSejuani
{
    internal class Program
    {



        public const string ChampionName = "Sejuani";
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





        public static Spell.Skillshot Q;
        public static Spell.Active W;
        public static Spell.Active E;
        public static Spell.Skillshot R;
        public static Spell.Targeted Ignite;



        static void Main(string[] args)
        {


            Loading.OnLoadingComplete += Game_OnStart;
            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Game_OnDraw;
            Orbwalker.OnPostAttack += Reset;
            Interrupter.OnInterruptableSpell += KInterrupter;
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
                Chat.Print("KSejuani Addon Loading Success", Color.Green);

                Q = new Spell.Skillshot(SpellSlot.Q, 650, SkillShotType.Linear, 0, 1600, 70);
                W = new Spell.Active(SpellSlot.W, 350);
                E = new Spell.Active(SpellSlot.E, 1000);
                R = new Spell.Skillshot(SpellSlot.R, 1175, SkillShotType.Linear, 250, 1600, 110);



                Menu = MainMenu.AddMenu("KSejuani", "sejuani");
                Menu.AddLabel("Criado por Bruno105");
                Menu.AddLabel("Çeviri TRAdana-");


                //------------//
                //-Mode Menu-//
                //-----------//


                ModesMenu1 = Menu.AddSubMenu("Kombo/Dürtme", "Modes1Sejuani");
                ModesMenu1.AddSeparator();
                ModesMenu1.AddLabel("Kombo Ayarları");
                ModesMenu1.Add("ComboQ", new CheckBox("Komboda Q Kullan", true));
                ModesMenu1.Add("ComboW", new CheckBox("Komboda W Kullan", true));
                ModesMenu1.Add("ComboE", new CheckBox("Komboda E Kullan", true));
                ModesMenu1.Add("ComboR", new CheckBox("Komboda R Kullan", true));
                ModesMenu1.Add("MinR", new Slider("R için gerekli şampiyon:", 2, 1, 5));
                ModesMenu1.AddSeparator();
                ModesMenu1.AddLabel("Dürtme Ayarları");
                ModesMenu1.Add("ManaH", new Slider("Manam şundan azsa büyü kullanma <=", 40));
                ModesMenu1.Add("HarassQ", new CheckBox("Dürtmede Q Kullan", true));
                ModesMenu1.Add("HarassW", new CheckBox("Dürtmede W Kullan", true));
                ModesMenu1.Add("HarassE", new CheckBox("Dürtmede E Kullan", true));
                ModesMenu2 = Menu.AddSubMenu("Lane/Sonvuruş", "Modes2Sejuani");
                ModesMenu2.AddLabel("Sonvuruş Ayarları");
                ModesMenu2.Add("ManaL", new Slider("Manam şundan azsa büyü kullanma <=", 40));
                ModesMenu2.Add("LastQ", new CheckBox("Son vuruşta Q Kullan", true));
                ModesMenu2.Add("LastW", new CheckBox("Son vuruşta W Kullan", true));
                ModesMenu2.Add("LastE", new CheckBox("Son vuruşta E Kullan", true));
                ModesMenu2.AddLabel("Lane/Orman Temizleme Ayarları");
                ModesMenu2.Add("ManaF", new Slider("Manam şundan azsa büyü kullanma <=", 40));
                ModesMenu2.Add("FarmQ", new CheckBox("LaneTemizlemede Q Kullan", true));
                ModesMenu2.Add("FarmW", new CheckBox("LaneTemizlemede W Kullan", true));
                ModesMenu2.Add("FarmE", new CheckBox("LaneTemizlemede E KUllan", true));
                ModesMenu2.Add("MinionE", new Slider("E için gereken minyon sayısı :", 3, 1, 5));
                //------------//
                //-Draw Menu-//
                //----------//
                DrawMenu = Menu.AddSubMenu("Göstergeler", "DrawKassadin");
                DrawMenu.Add("drawAA", new CheckBox("AA Menzilini Göster", true));
                DrawMenu.Add("drawQ", new CheckBox(" Q Menzilini Göster", true));
                DrawMenu.Add("drawW", new CheckBox(" W menzilini göster", true));
                DrawMenu.Add("drawE", new CheckBox(" E menzilini göster", true));
                DrawMenu.Add("drawR", new CheckBox(" R menzilini göster", true));
                //------------//
                //-Misc Menu-//
                //----------//

                Misc = Menu.AddSubMenu("MiscMenu", "Misc");
                Misc.Add("aarest", new CheckBox("W ile AA sıfırla"));
                //Misc.Add("useQGapCloser", new CheckBox("Q on GapCloser", true));
                Misc.Add("eInterrupt", new CheckBox("Interrupt için E Kullan", true));

            }

            catch (Exception e)
            {
                Chat.Print("KSejuani: Exception occured while Initializing Addon. Error: " + e.Message);
            }
        }


        static void Game_OnDraw(EventArgs args)
        {



            if (DrawMenu["drawAA"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = Color.White, Radius = _Player.GetAutoAttackRange(), BorderWidth = 2f }.Draw(_Player.Position);
            }
            if (DrawMenu["drawQ"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = Color.Aqua, Radius = 650, BorderWidth = 2f }.Draw(_Player.Position);
            }
            if (DrawMenu["drawW"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = Color.Green, Radius = 350, BorderWidth = 2f }.Draw(_Player.Position);
            }
            if (DrawMenu["drawE"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = Color.Red, Radius = 1000, BorderWidth = 2f }.Draw(_Player.Position);
            }
            if (DrawMenu["drawR"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = Color.Blue, Radius = 1175, BorderWidth = 2f }.Draw(_Player.Position);
            }

        }
        static void KInterrupter(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {

            if (args.DangerLevel == DangerLevel.High && sender.IsEnemy && sender is AIHeroClient && sender.Distance(_Player) < Q.Range && Q.IsReady() && Misc["useQGapCloser"].Cast<CheckBox>().CurrentValue)
            {
                Q.Cast(sender);
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

          private static void Reset(AttackableUnit target, EventArgs args)
        {
            if (!Misc["aareset"].Cast<CheckBox>().CurrentValue) return;
                if (target != null && target.IsEnemy && !target.IsInvulnerable && !target.IsDead && target is AIHeroClient &&target.Distance(ObjectManager.Player) <= W.Range)
                if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) && (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) &&  (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit)))) return;
                var e = target as Obj_AI_Base;
                if (!ModesMenu1["ComboW"].Cast<CheckBox>().CurrentValue || !e.IsEnemy) return;
                if (target == null) return;
                if (e.IsValidTarget() && W.IsReady())
                {
                    W.Cast();
                }
        



        }
    }
}
