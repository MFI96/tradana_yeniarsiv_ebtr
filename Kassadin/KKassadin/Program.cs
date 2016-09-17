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

namespace KKassadin
{
    internal class Program
    {
        public const string ChampionName = "Kassadin";

        public static Menu Menu, ModesMenu1, ModesMenu2, DrawMenu, Misc;

        public static Spell.Targeted Q;
        public static Spell.Active W;
        public static Spell.Skillshot E;
        public static Spell.Skillshot R;
        public static Spell.Targeted Ignite;

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
        private static int EBuffCount
        {
            get { return _Player.GetBuffCount("forcepulsecounter"); }
        }
        public static float RMana
        {
            get { return _Player.Spellbook.GetSpell(SpellSlot.R).SData.Mana; }
        }

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Game_OnStart;
            Drawing.OnDraw += Game_OnDraw;
            Game.OnUpdate += Game_OnUpdate;
            Gapcloser.OnGapcloser += KGapCloser;
            Interrupter.OnInterruptableSpell += KInterrupter;
            Orbwalker.OnPostAttack += Reset;
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
                Chat.Print("KKassadin basariyla yuklendi", Color.Aqua);
                Q = new Spell.Targeted(SpellSlot.Q, 650);
                W = new Spell.Active(SpellSlot.W);
                E = new Spell.Skillshot(SpellSlot.E, 650, SkillShotType.Cone, (int)0.5f, int.MaxValue, 10);
                R = new Spell.Skillshot(SpellSlot.R, 500, SkillShotType.Circular, (int)0.5f, int.MaxValue, 190);
                if (_Player.GetSpellSlotFromName("summonerdot") != SpellSlot.Unknown)
                    Ignite = new Spell.Targeted(_Player.GetSpellSlotFromName("summonerdot"), 550);

                Menu = MainMenu.AddMenu("KKassadin", "kassadin");
                Menu.AddSeparator();
                Menu.AddLabel("Criado por Bruno105");
                //------------//
                //-Mode Menu-//
                //-----------//
                ModesMenu1 = Menu.AddSubMenu("Combo/Harass", "Modes1Kassadin");
                ModesMenu1.AddSeparator();
                ModesMenu1.AddLabel("Combo Ayarları");
                ModesMenu1.Add("ComboQ", new CheckBox("Q Kullan", true));
                ModesMenu1.Add("ComboW", new CheckBox("W Kullan", true));
                ModesMenu1.Add("ComboE", new CheckBox("E Kullan", true));
                ModesMenu1.Add("ComboR", new CheckBox("R Kullan", true));
                ModesMenu1.Add("MaxR", new Slider("R için gereken şampiyon :", 2, 1, 5));
                ModesMenu1.AddSeparator();
                ModesMenu1.AddLabel("Dürtme Ayarları");
                ModesMenu1.Add("ManaH", new Slider("mana şundan azsa kullanma <=", 40));
                ModesMenu1.Add("HarassQ", new CheckBox("Q Kullan", true));
                ModesMenu1.Add("HarassW", new CheckBox("W Kullan", true));
                ModesMenu1.Add("HarassE", new CheckBox("E Kullan", true));
                ModesMenu1.Add("HarassR", new CheckBox("R Kullan", true));
                ModesMenu2 = Menu.AddSubMenu("Lane/LastHit", "Modes2Kassadin");
                ModesMenu2.AddLabel("SonVuruş Ayarları");
                ModesMenu2.Add("ManaL", new Slider("mana şundan azsa kullanma <=", 40));
                ModesMenu2.Add("LastQ", new CheckBox("Q Kullan", true));
                ModesMenu2.Add("LastW", new CheckBox("W Kullan", true));
                ModesMenu2.Add("LastE", new CheckBox("E Kullan", true));
                ModesMenu2.AddLabel("LaneTemizleme Ayarları");
                ModesMenu2.Add("ManaF", new Slider("mana şundan azsa kullanma <=", 40));
                ModesMenu2.Add("FarmQ", new CheckBox("Q Kullan", true));
                ModesMenu2.Add("FarmW", new CheckBox("W Kullan", true));
                ModesMenu2.Add("FarmE", new CheckBox("E Kullan", true));
                ModesMenu2.Add("MinionE", new Slider("E için gereken mana :", 3, 1, 5));
                ModesMenu2.Add("FarmR", new CheckBox("R Kullan", true));
                //------------//
                //-Draw Menu-//
                //----------//
                DrawMenu = Menu.AddSubMenu("Draws", "DrawKassadin");
                DrawMenu.Add("drawAA", new CheckBox("Göster AA", true));
                DrawMenu.Add("drawQ", new CheckBox(" Göster Q", true));
                DrawMenu.Add("drawW", new CheckBox(" Göster W", true));
                DrawMenu.Add("drawE", new CheckBox(" Göster E", true));
                DrawMenu.Add("drawR", new CheckBox(" Göster R", true));
                //------------//
                //-Misc Menu-//
                //----------//

                Misc = Menu.AddSubMenu("MiscMenu", "Misc");
                Misc.Add("aarest", new CheckBox("AA sıfırlama için W"));
                Misc.Add("useQGapCloser", new CheckBox("GapCloser için Q ", true));
                Misc.Add("eInterrupt", new CheckBox("Interrupt için E", true));



            }

            catch (Exception e)
            {
                Chat.Print("KKassadin: Exception occured while Initializing Addon. Error: " + e.Message);
            }

        }


        static void KInterrupter(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {

            if (args.DangerLevel == DangerLevel.High && sender.IsEnemy && sender is AIHeroClient && sender.Distance(_Player) < Q.Range && Q.IsReady() && Misc["eInterrupt"].Cast<CheckBox>().CurrentValue)
            {
                Q.Cast(sender);
            }

        }


        static void KGapCloser(Obj_AI_Base sender, Gapcloser.GapcloserEventArgs args)
        {


            if (sender.IsEnemy && sender is AIHeroClient && sender.Distance(_Player) < E.Range && E.IsReady() && Misc["useQGapCloser"].Cast<CheckBox>().CurrentValue)
            {
                E.Cast(sender);
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
                new Circle() { Color = Color.Green, Radius = (_Player.GetAutoAttackRange() + 50), BorderWidth = 2f }.Draw(_Player.Position);
            }
            if (DrawMenu["drawE"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = Color.Red, Radius = 600, BorderWidth = 2f }.Draw(_Player.Position);
            }
            if (DrawMenu["drawR"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = Color.Blue, Radius = 500, BorderWidth = 2f }.Draw(_Player.Position);
            }

        }

        static void Game_OnUpdate(EventArgs args)
        {


            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                var alvo = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
                var rmax = EntityManager.Heroes.Enemies.Where(t => t.IsInRange(Player.Instance.Position, R.Range) && !t.IsDead && t.IsValid && !t.IsInvulnerable).Count();
                if (!alvo.IsValid()) return;
                if (Q.IsReady() && Q.IsInRange(alvo) && ModesMenu1["ComboQ"].Cast<CheckBox>().CurrentValue)
                {
                    Q.Cast(alvo);
                }
                if (W.IsReady() && _Player.Distance(alvo) <= _Player.GetAutoAttackRange() + 50 && ModesMenu1["ComboW"].Cast<CheckBox>().CurrentValue)
                {
                    W.Cast();

                }
                if (E.IsReady() && E.IsInRange(alvo) && ModesMenu1["ComboE"].Cast<CheckBox>().CurrentValue)
                {
                    E.Cast(alvo);

                }
                if (R.IsReady() && R.IsInRange(alvo) && ModesMenu1["ComboE"].Cast<CheckBox>().CurrentValue && !(rmax >= ModesMenu1["MaxR"].Cast<Slider>().CurrentValue))
                {
                    R.Cast(alvo);

                }


            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                var alvo = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
                if (!alvo.IsValid()) return;



                if ((_Player.ManaPercent <= ModesMenu1["ManaH"].Cast<Slider>().CurrentValue))
                {
                    return;
                }
                if (Q.IsReady() && Q.IsInRange(alvo) && ModesMenu1["HarassQ"].Cast<CheckBox>().CurrentValue)
                {
                    Q.Cast(alvo);
                }
                if (W.IsReady() && _Player.Distance(alvo) <= _Player.GetAutoAttackRange() + 50 && ModesMenu1["HarassW"].Cast<CheckBox>().CurrentValue)
                {
                    W.Cast();

                }
                if (E.IsReady() && E.IsInRange(alvo) && ModesMenu1["HarassE"].Cast<CheckBox>().CurrentValue)
                {
                    E.Cast(alvo);

                }
                if (R.IsReady() && R.IsInRange(alvo) && ModesMenu1["HarassR"].Cast<CheckBox>().CurrentValue)
                {
                    R.Cast(alvo);

                }


            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {



                var minions = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, _Player.Position, Q.Range).OrderByDescending(x => x.MaxHealth).FirstOrDefault();

                var minion = EntityManager.MinionsAndMonsters.EnemyMinions.Where(t => t.IsInRange(Player.Instance.Position, E.Range) && !t.IsDead && t.IsValid && !t.IsInvulnerable).Count();
       

                if ((_Player.ManaPercent <= ModesMenu2["ManaF"].Cast<Slider>().CurrentValue))
                {
                    return;
                }
                if (Q.IsReady() && Q.IsInRange(minions) && ModesMenu2["FarmQ"].Cast<CheckBox>().CurrentValue && minions.Health < DamageLib.QCalc(minions))
                {
                    Q.Cast(minions);
                }

                if (E.IsReady() && E.IsInRange(minions) && ModesMenu2["FarmE"].Cast<CheckBox>().CurrentValue && (minion >= ModesMenu2["MinionE"].Cast<Slider>().CurrentValue))
                {

                    E.Cast(minions);

                }
                if (R.IsReady() && R.IsInRange(minions) && ModesMenu2["FarmR"].Cast<CheckBox>().CurrentValue)
                {
                    R.Cast(minions);

                }
                if (W.IsReady() && _Player.Distance(minions) <= _Player.GetAutoAttackRange() + 50 && ModesMenu2["FarmW"].Cast<CheckBox>().CurrentValue)
                {
                    W.Cast();

                }
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                var minions = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, _Player.Position, Q.Range).OrderByDescending(x => x.MaxHealth).FirstOrDefault();

                var minion = EntityManager.MinionsAndMonsters.EnemyMinions.Where(t => t.IsInRange(Player.Instance.Position, E.Range) && !t.IsDead && t.IsValid && !t.IsInvulnerable).Count();
       
                if (Q.IsReady() && Q.IsInRange(minions) && ModesMenu2["LastQ"].Cast<CheckBox>().CurrentValue && minions.Health < DamageLib.QCalc(minions))
                {
                    Q.Cast(minions);
                }
                if (W.IsReady() && _Player.Distance(minions) <= _Player.GetAutoAttackRange() + 50 && ModesMenu2["LastW"].Cast<CheckBox>().CurrentValue && minions.Health < DamageLib.WCalc(minions))
                {
                    W.Cast();

                }
                if (E.IsReady() && E.IsInRange(minions) && ModesMenu2["LastE"].Cast<CheckBox>().CurrentValue && (minion >= ModesMenu2["MinionE"].Cast<Slider>().CurrentValue && minions.Health < DamageLib.ECalc(minions)))
                {

                    E.Cast(minions);





                }

                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
                {
                    var Mpos = Game.CursorPos;

                    if (R.IsReady())
                    {

                        R.Cast(Mpos);


                    }








                }








            }
        }

        private static void Reset(AttackableUnit target, EventArgs args)
        {
            if (!Misc["aareset"].Cast<CheckBox>().CurrentValue) return;
            if (target != null && target.IsEnemy && !target.IsInvulnerable && !target.IsDead && target is AIHeroClient && target.Distance(ObjectManager.Player) <= W.Range)
                if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) && (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) && (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit)))) return;
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

