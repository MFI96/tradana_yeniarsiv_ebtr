using System.Linq;
using EloBuddy.SDK;
using Settings = KA_Shyvanna.Config.Modes.LaneClear;

namespace KA_Shyvanna.Modes
{
    public sealed class JungleClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear);
        }

        public override void Execute()
        {
            var jgminionQ =
                EntityManager.MinionsAndMonsters.GetJungleMonsters()
                    .OrderByDescending(j => j.Health)
                    .FirstOrDefault(j => j.IsValidTarget(Q.Range));

            var jgminionW =
                EntityManager.MinionsAndMonsters.GetJungleMonsters()
                    .OrderByDescending(j => j.Health)
                    .FirstOrDefault(j => j.IsValidTarget(W.Range));

            var jgminionE =
                EntityManager.MinionsAndMonsters.GetJungleMonsters()
                    .OrderByDescending(j => j.Health)
                    .FirstOrDefault(j => j.IsValidTarget(E.Range));

            if (Q.IsReady() && jgminionQ.IsValidTarget(Q.Range) && Settings.UseQ && EventsManager.CanQ)
            {
                Q.Cast();
            }

            if (W.IsReady() && Settings.UseW && jgminionW.IsValidTarget(W.Range))
            {
                W.Cast();
            }

            if (E.IsReady() && jgminionE.IsValidTarget(E.Range) && Settings.UseE)
            {
                E.Cast(E.GetPrediction(jgminionE).CastPosition);
            }
        }
    }
}
