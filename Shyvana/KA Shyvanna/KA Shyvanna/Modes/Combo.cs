using EloBuddy;
using EloBuddy.SDK;
using Settings = KA_Shyvanna.Config.Modes.Combo;

namespace KA_Shyvanna.Modes
{
    public sealed class Combo : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(R.Range, DamageType.Magical);
            if (target == null || target.IsZombie || target.HasUndyingBuff()) return;

            if (Q.IsReady() && target.IsValidTarget(Q.Range) && Settings.UseQ && EventsManager.CanQ)
            {
                Q.Cast();
            }

            if (W.IsReady() && Settings.UseW && target.IsValidTarget(W.Range))
            {
                W.Cast();
            }

            if (E.IsReady() && target.IsValidTarget(E.Range) && Settings.UseE)
            {
                E.Cast(E.GetPrediction(target).CastPosition);
            }

            if (R.IsReady() && target.IsValidTarget(R.Range) && Settings.UseR &&
                target.HealthPercent + 15 <= Player.Instance.HealthPercent && target.CountEnemiesInRange(R.Range) <= 2)
            {
                R.Cast(E.GetPrediction(target).CastPosition);
            }
        }
    }
}
