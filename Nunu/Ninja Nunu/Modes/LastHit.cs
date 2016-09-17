using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using Settings = NinjaNunu.Config.Modes.LastHit;

namespace NinjaNunu.Modes
{
    public sealed class LastHit : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit);
        }
        public override void Execute()
        {
            if (Settings.UseE && E.IsReady() && Player.Instance.ManaPercent >= Settings.ManaLastHit)
            {
                var Lmonsters = EntityManager.MinionsAndMonsters.GetLaneMinions().OrderByDescending(a => a.MaxHealth).FirstOrDefault(b => b.Distance(Player.Instance) <= 1300);
                if (Lmonsters.Health <= Damage.EDamage(Lmonsters))
                {
                    E.Cast(Lmonsters);
                    return;
                }
            }
        }
    }
}
