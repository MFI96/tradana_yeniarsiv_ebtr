using EloBuddy;
using EloBuddy.SDK;

namespace LightAmumu.Carry
{
    internal class Combo : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical, Player.Instance.Position);
            if (target.IsValidTarget(Q.Range) && MenuList.Combo.WithQ && target.IsEnemy)
            {
                var qPred = Q.GetPrediction(target);
                if (qPred.HitChancePercent >= MenuList.Misc.predQ && Q.IsInRange(target) && qPred.Collision == false)
                    Q.Cast(qPred.CastPosition);
            }

            if (R.IsReady() && MenuList.Combo.WithR)
                if (Player.Instance.CountEnemiesInRange(R.Range) >= MenuList.Combo.CountEnemiesInR)
                    R.Cast();

            if (target.IsValidTarget(W.Range * 2) && W.IsReady() && MenuList.Combo.WithW)
                Damage.WEnable();

            if (target.IsValidTarget(E.Range) && MenuList.Combo.WithE)
                E.Cast();
        }
    }
}