using System.Linq;
using EloBuddy.SDK;
using Settings = KA_Shyvanna.Config.Modes.LaneClear;

namespace KA_Shyvanna.Modes
{
    public sealed class LaneClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear);
        }

        public override void Execute()
        {
            var minionQ =
                EntityManager.MinionsAndMonsters.GetLaneMinions()
                    .OrderByDescending(m => m.Health)
                    .FirstOrDefault(m => m.IsValidTarget(Q.Range));
            var minionW =
                EntityManager.MinionsAndMonsters.GetLaneMinions()
                    .OrderByDescending(m => m.Health)
                    .FirstOrDefault(m => m.IsValidTarget(W.Range));
            var minionE =
                EntityManager.MinionsAndMonsters.GetLaneMinions()
                    .OrderByDescending(m => m.Health)
                    .FirstOrDefault(m => m.IsValidTarget(E.Range));

            if (Q.IsReady() && minionQ.IsValidTarget(Q.Range) && Settings.UseQ && EventsManager.CanQ)
            {
                Q.Cast();
            }

            if (W.IsReady() && Settings.UseW && minionW.IsValidTarget(W.Range))
            {
                W.Cast();
            }

            if (E.IsReady() && minionE.IsValidTarget(E.Range) && Settings.UseE)
            {
                E.Cast(E.GetPrediction(minionE).CastPosition);
            }
        }
    }
}
