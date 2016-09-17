using EloBuddy;
using EloBuddy.SDK;
using Settings = Maokai.Config.LaneClear.LaneClearMenu;

namespace Maokai.Modes
{
    public sealed class LaneClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear);
        }

        public override void Execute()
        {
            if (Settings.UseE && E.IsReady() && ManaPercent >= Settings.ManaE)
            {
                var laneminionsE = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                    Player.Instance.Position, E.Range);
                var efarm = EntityManager.MinionsAndMonsters.GetCircularFarmLocation(laneminionsE, E.Width,
                    (int) E.Range);

                if (efarm.HitNumber >= Settings.MinE)
                {
                    E.Cast(efarm.CastPosition);
                    return;
                }
            }

            if (Settings.UseQ && Q.IsReady() && ManaPercent >= Settings.ManaQ)
            {
                var lanemionionsQ = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                    Player.Instance.Position, Q.Range);
                var qfarm = EntityManager.MinionsAndMonsters.GetLineFarmLocation(lanemionionsQ, Q.Width, (int) Q.Range);

                if (qfarm.HitNumber >= Settings.MinQ)
                {
                    Q.Cast(qfarm.CastPosition);
                }
            }
        }
    }
}