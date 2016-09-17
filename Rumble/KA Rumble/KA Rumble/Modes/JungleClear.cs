using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

using Settings = KA_Rumble.Config.Modes.LaneClear;

namespace KA_Rumble.Modes
{
    public sealed class JungleClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear);
        }

        public override void Execute()
        {
            var jgminion =
                EntityManager.MinionsAndMonsters.GetJungleMonsters()
                    .OrderByDescending(j => j.Health)
                    .FirstOrDefault(j => j.IsValidTarget(E.Range));

            if (jgminion == null) return;

            var minionE =
                EntityManager.MinionsAndMonsters.GetLaneMinions()
                    .OrderByDescending(m => m.Health)
                    .FirstOrDefault(
                        m =>
                            m.IsValidTarget(E.Range) &&
                            Prediction.Health.GetPrediction(m, E.CastDelay) <= SpellDamage.GetRealDamage(SpellSlot.E, m) &&
                            Prediction.Health.GetPrediction(m, E.CastDelay) > 10);

            if (minionE != null)
            {
                E.Cast(minionE);
            }

            if (Q.IsReady() && jgminion.IsValidTarget(Q.Range) && Settings.UseQ && Player.Instance.IsFacing(jgminion) &&
                Player.Instance.Mana <= 60)
            {
                Q.Cast();
            }
        }
    }
}
