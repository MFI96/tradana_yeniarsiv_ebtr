using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

using Settings = KA_Rumble.Config.Modes.LaneClear;

namespace KA_Rumble.Modes
{
    public sealed class LaneClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear);
        }

        public override void Execute()
        {
            var minion =
                EntityManager.MinionsAndMonsters.GetLaneMinions()
                    .OrderBy(m => m.Health)
                    .FirstOrDefault(m => m.IsValidTarget(E.Range));

            if (minion == null) return;

            if (E.IsReady() && minion.IsValidTarget(E.Range) && Settings.UseE && Player.Instance.Mana < 70)
            {
                E.Cast(minion);
            }

            if (Q.IsReady() && minion.IsValidTarget(Q.Range) && Settings.UseQ && Player.Instance.IsFacing(minion) && Player.Instance.Mana < 70)
            {
                Q.Cast();
            }
        }
    }
}
