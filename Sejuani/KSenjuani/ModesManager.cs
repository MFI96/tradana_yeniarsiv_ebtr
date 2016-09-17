using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using System.Linq;

namespace KSejuani
{
    class ModesManager
    {

        public static void Combo()
        {
            var rmax = EntityManager.Heroes.Enemies.Where(t => t.IsInRange(Player.Instance.Position, Program.R.Range) && !t.IsDead && t.IsValid && !t.IsInvulnerable).Count();

            var alvo = TargetSelector.GetTarget(Program.Q.Range, DamageType.Magical);

            if (!alvo.IsValid()) return;
            var predPos = Prediction.Position.PredictLinearMissile(alvo, Program.R.Range, Program.R.Width, Program.R.CastDelay, Program.R.Speed, int.MaxValue, null, false);


            if (Program.Q.IsReady() && Program.Q.IsInRange(alvo) && Program.ModesMenu1["ComboQ"].Cast<CheckBox>().CurrentValue)
            {
                Program.Q.Cast(alvo);
            }
            if (Program.W.IsReady() && Program.W.IsInRange(alvo) && Program.ModesMenu1["ComboW"].Cast<CheckBox>().CurrentValue)
            {
                Program.W.Cast();

            }
            if (Program.E.IsReady() && Program.E.IsInRange(alvo) && Program.ModesMenu1["ComboE"].Cast<CheckBox>().CurrentValue)
            {
                Program.E.Cast();

            }
            if (Program.R.IsReady() && Program.R.IsInRange(alvo) && Program.ModesMenu1["ComboR"].Cast<CheckBox>().CurrentValue && (rmax >= Program.ModesMenu1["MinR"].Cast<Slider>().CurrentValue) && predPos.HitChance > HitChance.High)
            {
                Program.R.Cast(predPos.CastPosition);

            }
        }


        public static void Harass()
        {

            var alvo = TargetSelector.GetTarget(Program.Q.Range, DamageType.Magical);
                if (!alvo.IsValid()) return;



                if ((Program._Player.ManaPercent <= Program.ModesMenu1["ManaH"].Cast<Slider>().CurrentValue))
                {
                    return;
                }
                if (Program.Q.IsReady() && Program.Q.IsInRange(alvo) && Program.ModesMenu1["HarassQ"].Cast<CheckBox>().CurrentValue)
                {
                    Program.Q.Cast(alvo);
                }
                if (Program.W.IsReady() && Program.W.IsInRange(alvo) && Program.ModesMenu1["HarassW"].Cast<CheckBox>().CurrentValue)
                {
                    Program.W.Cast();

                }
                if (Program.E.IsReady() && Program.E.IsInRange(alvo) && Program.ModesMenu1["HarassE"].Cast<CheckBox>().CurrentValue)
                {
                    Program.E.Cast();

                }



}

        public static void LaneClear()
        {


            var minions = EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(m => m.IsValidTarget(Program.Q.Range));
            var minion = EntityManager.MinionsAndMonsters.EnemyMinions.Where(t => t.IsInRange(Player.Instance.Position, Program.E.Range) && !t.IsDead && t.IsValid && !t.IsInvulnerable).Count();
            if (minions == null) return;
            if ((Program._Player.ManaPercent <= Program.ModesMenu2["ManaF"].Cast<Slider>().CurrentValue))
            {
                return;
            }
            if (Program.Q.IsReady() && Program.Q.IsInRange(minions) && Program.ModesMenu2["FarmQ"].Cast<CheckBox>().CurrentValue)
            {
                Program.Q.Cast(minions);
            }

            if (Program.W.IsReady() && Program.W.IsInRange(minions) && Program.ModesMenu2["FarmW"].Cast<CheckBox>().CurrentValue)
            {
                Program.W.Cast();

            }
            if (Program.E.IsReady() && Program.E.IsInRange(minions) && Program.ModesMenu1["FarmE"].Cast<CheckBox>().CurrentValue && (minion >= Program.ModesMenu2["MinionE"].Cast<Slider>().CurrentValue) && minions.HasBuff("SejuaniFrost"))
            {
                Program.E.Cast();

            }




        }

        public static void LastHit()
        {



            var qminions = EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(m => m.IsValidTarget(Program.Q.Range) && (DamageLib.QCalc(m) > m.Health));
            var wminions = EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(m => m.IsValidTarget(Program.W.Range) && (DamageLib.WCalc(m) > m.Health));
            var eminions = EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(m => m.IsValidTarget(Program.E.Range) && (DamageLib.ECalc(m) > m.Health));
            var minion = EntityManager.MinionsAndMonsters.EnemyMinions.Where(t => t.IsInRange(Player.Instance.Position, Program.E.Range) && !t.IsDead && t.IsValid && !t.IsInvulnerable).Count();
            if (qminions == null) return;
            var prediction = Program.Q.GetPrediction(qminions);
            if (Program.Q.IsReady() && Program.Q.IsInRange(qminions) && Program.ModesMenu2["LastQ"].Cast<CheckBox>().CurrentValue && qminions.Health < DamageLib.QCalc(qminions))
            {
                Program.Q.Cast(qminions);
            }
            if (Program.W.IsReady() && Program.W.IsInRange(wminions) && Program.ModesMenu2["LastW"].Cast<CheckBox>().CurrentValue && wminions.Health < DamageLib.WCalc(wminions))
            {
                Program.W.Cast();
            }
            if (Program.E.IsReady() && Program.E.IsInRange(eminions) && Program.ModesMenu2["LastE"].Cast<CheckBox>().CurrentValue && eminions.Health < DamageLib.ECalc(eminions) && (minion >= Program.ModesMenu2["MinionE"].Cast<Slider>().CurrentValue) && eminions.HasBuff("SejuaniFrost"))
            {
                Program.E.Cast();
            }



        }

        public static void JungleClear()
        {


            var jungleMonsters =
                EntityManager.MinionsAndMonsters.GetJungleMonsters()
                    .OrderByDescending(j => j.Health)
                    .FirstOrDefault(j => j.IsValidTarget(Program.Q.Range));
            var minioon = EntityManager.MinionsAndMonsters.EnemyMinions.Where(t => t.IsInRange(Player.Instance.Position, Program.E.Range) && !t.IsDead && t.IsValid && !t.IsInvulnerable).Count();
            if (jungleMonsters == null) return;
            if ((Program._Player.ManaPercent <= Program.ModesMenu2["ManaF"].Cast<Slider>().CurrentValue))
            {
                return;
            }
            if (Program.Q.IsReady() && Program.Q.IsInRange(jungleMonsters) && Program.ModesMenu2["FarmQ"].Cast<CheckBox>().CurrentValue)
            {
                Program.Q.Cast(jungleMonsters);
            }

            if (Program.W.IsReady() && Program.W.IsInRange(jungleMonsters) && Program.ModesMenu2["FarmW"].Cast<CheckBox>().CurrentValue)
            {
                Program.W.Cast();

            }
            if (Program.E.IsReady() && Program.E.IsInRange(jungleMonsters) && Program.ModesMenu1["FarmE"].Cast<CheckBox>().CurrentValue && (minioon >= Program.ModesMenu2["MinionE"].Cast<Slider>().CurrentValue))
            {
                Program.E.Cast();

            }




        }

    }
}
