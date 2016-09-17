using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using Settings = Maokai.Config.LastHit.LastHitMenu;

namespace Maokai.Modes
{
    public sealed class LastHit : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit);
        }

        public override void Execute()
        {
            if (Settings.UseQ && Q.IsReady() && ManaPercent >= Settings.ManaQ)
            {
                var minionQ =
                    EntityManager.MinionsAndMonsters.GetLaneMinions()
                        .Where(
                            a =>
                                a.IsValidTarget(Q.Range) && a.Health < Player.Instance.GetSpellDamage(a, SpellSlot.Q) &&
                                a.Distance(Player.Instance.ServerPosition) > Player.Instance.GetAutoAttackRange())
                        .OrderByDescending(a => a.MaxHealth)
                        .FirstOrDefault();

                if (minionQ != null)
                {
                    Q.Cast(minionQ);
                }
            }
        }
    }
}