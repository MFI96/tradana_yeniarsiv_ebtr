using EloBuddy;
using EloBuddy.SDK;
using KA_Shyvanna;
using Settings = KA_Shyvanna.Config.Modes.Harass;

namespace KA_Shyvanna.Modes
{
    public sealed class Harass : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(E.Range, DamageType.Magical);
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
        }
    }
}
