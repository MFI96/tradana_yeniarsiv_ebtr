using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Utils;
using System.Linq;

namespace KTrundle
{
    internal class ModesManager : Program
    {
        public static void Combo()
        {
            //combo
            var alvo = TargetSelector.GetTarget(E.Range, DamageType.Physical);
            var useQ = ModesMenu1["ComboQ"].Cast<CheckBox>().CurrentValue;
            var useW = ModesMenu1["ComboW"].Cast<CheckBox>().CurrentValue;
            var useE = ModesMenu1["ComboE"].Cast<CheckBox>().CurrentValue;
            var useR = ModesMenu1["ComboR"].Cast<CheckBox>().CurrentValue;
            if (!alvo.IsValid()) return;
            if (ModesMenu1["useI"].Cast<CheckBox>().CurrentValue)
            {
                Itens.UseItens();
            }


            if (W.IsInRange(alvo) && W.IsReady() && useW)
            {
                W.Cast(alvo);
            }
            if (E.IsInRange(alvo) && E.IsReady() && useE)
            {
                E.Cast(alvo);

            }
            if (Q.IsInRange(alvo) && Q.IsReady() && useQ)
            {
                Q.Cast();
            }
            if (R.IsInRange(alvo) && R.IsReady() && useR)
            {
                R.Cast(alvo);
            }
        }

        public static void Harass()
        {

            var alvo = TargetSelector.GetTarget(E.Range, DamageType.Physical);
            var useQ = ModesMenu1["HarassQ"].Cast<CheckBox>().CurrentValue;
            var useW = ModesMenu1["HarassW"].Cast<CheckBox>().CurrentValue;
            if (!alvo.IsValid()) return;

            if (W.IsInRange(alvo) && W.IsReady() && useW)
            {
                W.Cast(alvo);
            }
            if (Q.IsInRange(alvo) && Q.IsReady() && useQ)
            {
                Q.Cast();
            }

        }
        public static void LaneClear()
        {

            var useQ = ModesMenu2["FarmQ"].Cast<CheckBox>().CurrentValue;
            var useW = ModesMenu2["FarmW"].Cast<CheckBox>().CurrentValue;
            var minions = EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(m => m.IsValidTarget(W.Range));
            var minion = EntityManager.MinionsAndMonsters.EnemyMinions.Where(t => t.IsInRange(Player.Instance.Position, W.Range) && !t.IsDead && t.IsValid && !t.IsInvulnerable).Count();
            if (minions == null) return;
            if ((_Player.ManaPercent <= Program.ModesMenu2["ManaF"].Cast<Slider>().CurrentValue))
            {
                return;
            }
            if (useQ && Q.IsReady() && W.IsInRange(minions))
            {
                Q.Cast();
            }
            if (useW && W.IsReady() && W.IsInRange(minions) && (minion >= Program.ModesMenu2["MinionW"].Cast<Slider>().CurrentValue))
            {
                W.Cast(minions);
            }


        }
        public static void JungleClear()
        {

            var useQ = ModesMenu2["JungQ"].Cast<CheckBox>().CurrentValue;
            var useW = ModesMenu2["JungW"].Cast<CheckBox>().CurrentValue;
            var jungleMonsters = EntityManager.MinionsAndMonsters.GetJungleMonsters().OrderByDescending(j => j.Health).FirstOrDefault(j => j.IsValidTarget(Program.W.Range));
            var minioon = EntityManager.MinionsAndMonsters.EnemyMinions.Where(t => t.IsInRange(Player.Instance.Position, Program.W.Range) && !t.IsDead && t.IsValid && !t.IsInvulnerable).Count();
            if ((Program._Player.ManaPercent <= Program.ModesMenu2["ManaJ"].Cast<Slider>().CurrentValue))
            {
                return;
            }
            if (jungleMonsters == null) return;
            if (useQ && Q.IsReady() && Q.IsInRange(jungleMonsters))
            {
                Q.Cast();
            }
            if (useW && W.IsReady() && W.IsInRange(jungleMonsters))
            {
                W.Cast(jungleMonsters);
            }
        }
        public static void LastHit()
        {

            var useQ = Program.ModesMenu2["LastQ"].Cast<CheckBox>().CurrentValue;
            var qminions = EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(m => m.IsValidTarget((Program.W.Range)) && (DamageLib.QCalc(m) > m.Health));
            if (qminions == null) return;
            if ((Program._Player.ManaPercent <= Program.ModesMenu2["ManaL"].Cast<Slider>().CurrentValue))
            {
                return;
            }

            if (Q.IsReady() && (Program._Player.Distance(qminions) <= Program._Player.GetAutoAttackRange()) && useQ && qminions.Health < DamageLib.QCalc(qminions))
            {
                Q.Cast();
            }

        }

        public static void Flee()
        {
            var alvo = TargetSelector.GetTarget(E.Range, DamageType.Physical);

            if (W.IsReady())
            {
                W.Cast(Game.CursorPos);

            }
            if (E.IsReady() && E.IsInRange(alvo))
            {
                E.Cast(alvo);
            }

        }

        public static void KillSteal()
        {


            foreach (var enemy in EntityManager.Heroes.Enemies.Where(a => !a.IsDead && !a.IsZombie && a.Health > 0))
            {
                if (enemy.IsValidTarget(R.Range) && enemy.HealthPercent <= 40)
                {

                    if (DamageLib.QCalc(enemy) >= enemy.Health)
                    {
                        if (Q.IsReady() && Q.IsInRange(enemy) && Program.ModesMenu1["KQ"].Cast<CheckBox>().CurrentValue)
                        {
                            E.Cast(enemy);
                        }
                    }
                }
            }
        }
    }
}