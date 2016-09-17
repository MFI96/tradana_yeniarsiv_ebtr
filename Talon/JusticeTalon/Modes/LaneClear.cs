using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using Settings = JusticeTalon.Config.Modes.LaneClear;

namespace JusticeTalon.Modes
{
    public sealed class LaneClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear);
        }

        public override void Execute()
        {
            if (Player.Instance.ManaPercent < Config.Modes.LaneClear.ManaUsage)
                return;
            var minions =
                EntityManager.MinionsAndMonsters.GetLaneMinions().Where(m => m.IsValidTarget(W.Range)).ToArray();
            if (minions.Length == 0)
                return;
            var pos = EntityManager.MinionsAndMonsters.GetLineFarmLocation(minions, W.Width, (int) W.Range);
            if (pos.HitNumber >= Config.Modes.LaneClear.HitNumberW && Config.Modes.LaneClear.UseW && W.IsReady())
            {
                W.Cast(pos.CastPosition);
                ModeManager.Useitems();
            }

        }

        internal static void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}