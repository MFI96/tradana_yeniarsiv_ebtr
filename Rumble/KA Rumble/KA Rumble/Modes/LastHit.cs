using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

using Settings = KA_Rumble.Config.Modes.LastHit;

namespace KA_Rumble.Modes
{
    public sealed class LastHit : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit);
        }

        public override void Execute()
        {
            var minion =
                EntityManager.MinionsAndMonsters.GetLaneMinions()
                    .OrderByDescending(m => m.Health)
                    .FirstOrDefault(m => m.IsValidTarget(E.Range));

            if (minion == null) return;

            if (E.IsReady() && minion.IsValidTarget(E.Range) && Settings.UseE && Player.Instance.Mana <= 60)
            {
                var minionE =
                       EntityManager.MinionsAndMonsters.GetLaneMinions()
                           .OrderByDescending(m => m.Health)
                           .FirstOrDefault(
                               m => m.IsValidTarget(E.Range) && Prediction.Health.GetPrediction(m, E.CastDelay) <= SpellDamage.GetRealDamage(SpellSlot.E, m) &&
                               Prediction.Health.GetPrediction(m, E.CastDelay) > 10);

                if (minionE != null)
                {
                    E.Cast(minionE);
                }
                
            }
        }
    }
}
