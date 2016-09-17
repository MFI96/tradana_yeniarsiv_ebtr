using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using Settings = Bard.Config.Modes.Harass;

namespace Bard.Modes
{
    public sealed class Harass : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
        }

        public override void Execute()
        {
            if (!Settings.UseQ || !Q.IsReady() || !(Player.Instance.ManaPercent >= Settings.ManaQ)) return;
            {
                var targetq = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
                if (targetq == null)
                {
                    return;
                }
                var qprediction = Q.GetPrediction(targetq);
                if (qprediction.HitChance >= HitChance.High)
                {
                    Q.Cast(qprediction.CastPosition);
                }
            }
        }
    }
}
