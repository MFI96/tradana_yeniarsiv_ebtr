using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using Settings = Yorick.Config.LaneClear.LaneClearMenu;

namespace Yorick.Modes
{
    public sealed class LaneClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear);
        }

        public override void Execute()
        {
            if (Settings.UseW && W.IsReady() && Mana >= Settings.ManaW)
            {
                var lanew = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                    Player.Instance.Position, W.Range);
                var wfarm = EntityManager.MinionsAndMonsters.GetCircularFarmLocation(lanew, W.Width, (int) W.Range);

                if (wfarm.HitNumber >= Settings.MinW)
                {
                    W.Cast(wfarm.CastPosition);
                    return;
                }
            }

            if (Settings.UseE && E.IsReady() && Mana >= Settings.ManaE)
            {
                var laneE = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                    Player.Instance.Position, E.Range)
                    .OrderByDescending(a => a.MaxHealth)
                    .FirstOrDefault(a => a.Health >= Player.Instance.GetSpellDamage(a, SpellSlot.E));

                if (laneE != null)
                {
                    E.Cast(laneE);
                }
            }
        }
    }
}