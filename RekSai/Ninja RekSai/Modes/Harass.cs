using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;


using Settings = RekSai.Config.Harass.HarassMenu;

namespace RekSai.Modes
{
    public sealed class Harass : ModeBase
    {
        public override bool ShouldBeExecuted()
        {

            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
        }

        public override void Execute()
        {
            if (Settings.UseQ2 && Q2.IsReady())
            {
                var targetQ2 = TargetSelector.GetTarget(Q2.Range, DamageType.Magical);
                
                if (Events.burrowed && targetQ2 != null && targetQ2.IsValidTarget())
                {
                    var predictionQ2 = Q2.GetPrediction(targetQ2);
                    if (predictionQ2.HitChance >= HitChance.Medium)
                    {
                        Q2.Cast(predictionQ2.CastPosition);
                        return;
                    }
                }
                else if (!Events.burrowed)
                {
                    W.Cast();
                    return;
                }
            }
            
        }
    }
}
