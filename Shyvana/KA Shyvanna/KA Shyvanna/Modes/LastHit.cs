using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using Settings = KA_Shyvanna.Config.Modes.LastHit;

namespace KA_Shyvanna.Modes
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

            if (E.IsReady() && minion.IsValidTarget(E.Range) && Settings.UseE)
            {
                var minionE =
                       EntityManager.MinionsAndMonsters.GetLaneMinions()
                           .OrderByDescending(m => m.Health)
                           .FirstOrDefault(
                               m => m.IsValidTarget(E.Range) && m.Health <= SpellDamage.GetRealDamage(SpellSlot.E, m));

                if (minionE != null)
                {
                    E.Cast(minionE);
                }
                
            }
        }
    }
}
