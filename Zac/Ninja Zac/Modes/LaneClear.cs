using System.Linq;
using EloBuddy.SDK;
using Settings = Zac.Config.LaneClear.LaneClearMenu;

namespace Zac.Modes
{
    public sealed class LaneClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear);
        }

        public override void Execute()
        {
            if (Q.IsReady() && Settings.UseQ)
            {
                var minQ = EntityManager.MinionsAndMonsters.GetLaneMinions().Where(a => a.IsValidTarget(Q.Range));
                var qfarm = EntityManager.MinionsAndMonsters.GetLineFarmLocation(minQ, Q.Width, (int) Q.Range);

                if (qfarm.HitNumber >= Settings.MinQ)
                {
                    Q.Cast(qfarm.CastPosition);
                    return;
                }
            }
            if (W.IsReady() && Settings.UseW)
            {
                var minW = EntityManager.MinionsAndMonsters.GetLaneMinions().Where(a => a.IsValidTarget(W.Range));

                if (minW.Count() >= Settings.MinW)
                {
                    W.Cast();
                }
            }
        }
    }
}