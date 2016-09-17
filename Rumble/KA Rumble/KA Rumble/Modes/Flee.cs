using EloBuddy;
using EloBuddy.SDK;

namespace KA_Rumble.Modes
{
    public sealed class Flee : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(E.Range, DamageType.Magical);
            if (target == null || target.IsZombie || target.HasUndyingBuff()) return;

            if (Player.Instance.HealthPercent <= 25 && target.IsValidTarget(E.Range))
            {
                E.Cast(target);
            }

            if (W.IsReady())
            {
                W.Cast();
            }
        }
    }
}
