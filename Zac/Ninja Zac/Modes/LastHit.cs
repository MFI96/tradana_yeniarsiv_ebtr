using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using Settings = Zac.Config.LastHit.LastHitMenu;

namespace Zac.Modes
{
    public sealed class LastHit : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit);
        }

        public override void Execute()
        {
            if (Q.IsReady() && Settings.UseQ)
            {
                var minionsQ =
                    EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                        Player.Instance.Position, Q.Range).OrderByDescending(a => a.MaxHealth).FirstOrDefault();

                if (minionsQ != null && minionsQ.Health <= Player.Instance.GetSpellDamage(minionsQ, SpellSlot.Q) &&
                    !minionsQ.IsInAutoAttackRange(Player.Instance))
                {
                    Q.Cast(minionsQ);
                    return;
                }
            }

            if (W.IsReady() && Settings.UseW)
            {
                var minionsW =
                    EntityManager.MinionsAndMonsters.GetLaneMinions()
                        .Where(a => a.IsValidTarget(W.Range))
                        .OrderByDescending(a => a.MaxHealth)
                        .FirstOrDefault();

                if (minionsW != null && minionsW.Health <= Player.Instance.GetSpellDamage(minionsW, SpellSlot.W))
                {
                    W.Cast();
                }
            }
        }
    }
}