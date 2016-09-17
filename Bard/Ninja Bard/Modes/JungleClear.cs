using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using Settings = Bard.Config.Modes.JungleClear;

namespace Bard.Modes
{
    public sealed class JungleClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear);
        }

        public override void Execute()
        {
            if (!Settings.UseQ || !Q.IsReady() || !(Player.Instance.ManaPercent >= Settings.ManaQ)) return;
            {
                var monster =
                    EntityManager.MinionsAndMonsters.GetJungleMonsters()
                        .Where(a => a.IsValidTarget(Q.Range))
                        .OrderByDescending(a => a.MaxHealth);

                foreach (var m in monster.Where(m => m.Health > Player.Instance.GetAutoAttackDamage(m)*2))
                {
                    Q.Cast(m);
                    return;
                }
            }
        }
    }
}
