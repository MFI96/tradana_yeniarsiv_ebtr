using EloBuddy;
using EloBuddy.SDK;
using Settings = PartyJanna.Config.Settings.Flee;

namespace PartyJanna.Modes
{
    public sealed class Flee : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee);
        }

        public override void Execute()
        {
            Q.Range = (uint)Settings.QUseRange;

            var target = GetTarget(W, DamageType.Magical);

            if (target != null && Settings.UseW)
            {
                W.Cast(target);
            }

            target = GetTarget(Q, DamageType.Magical);

            if (target != null && Settings.UseQ)
            {
                Q.Cast(Q.GetPrediction(target).CastPosition);
            }
        }
    }
}
