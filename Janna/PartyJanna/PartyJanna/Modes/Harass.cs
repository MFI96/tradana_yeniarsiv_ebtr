using EloBuddy;
using EloBuddy.SDK;
using Settings = PartyJanna.Config.Settings.Harass;

namespace PartyJanna.Modes
{
    public sealed class Harass : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            if (Settings.AutoHarass && Player.Instance.ManaPercent >= Settings.AutoHarassManaPercent)
            {
                var target = GetTarget(W, DamageType.Magical);

                if (target != null)
                {
                    W.Cast(target);
                }
            }

            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
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
