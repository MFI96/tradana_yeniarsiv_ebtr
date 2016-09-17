using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using Settings = Yorick.Config.LastHit.LastHitMenu;

namespace Yorick.Modes
{
    public sealed class LastHit : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit);
        }

        public override void Execute()
        {
            if (Settings.UseW && W.IsReady() && Mana >= Settings.ManaW)
            {
                var lhW =
                    EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                        Player.Instance.Position, W.Range)
                        .OrderByDescending(a => a.MaxHealth)
                        .FirstOrDefault(a => a.Health <= Player.Instance.GetSpellDamage(a, SpellSlot.W));

                if (lhW != null && lhW.Distance(Player.Instance.ServerPosition) > Player.Instance.GetAutoAttackRange())
                {
                    W.Cast(lhW);
                    return;
                }
            }

            if (Settings.UseE && E.IsReady() && Mana >= Settings.ManaE)
            {
                var lhe =
                    EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                        Player.Instance.Position, E.Range)
                        .OrderByDescending(a => a.MaxHealth)
                        .FirstOrDefault(a => a.Health <= Player.Instance.GetSpellDamage(a, SpellSlot.E));

                if (lhe != null && lhe.Distance(Player.Instance.ServerPosition) > Player.Instance.GetAutoAttackRange())
                {
                    E.Cast(lhe);
                }
            }
        }
    }
}