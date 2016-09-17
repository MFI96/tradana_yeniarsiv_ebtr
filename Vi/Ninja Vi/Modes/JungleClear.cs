using EloBuddy.SDK;
using EloBuddy;
using System.Linq;
using Settings = Vi.Config.Modes.JungleClear;
namespace Vi.Modes
{
    public sealed class JungleClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear);
        }

        public override void Execute()
        {
            if (Settings.UseQ && Q.IsReady() && Player.Instance.ManaPercent >= Settings.MinMana || Q.IsCharging)
            {
                var dragon = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.ServerPosition, Q.MaximumRange + 200)
                    .Where(e => !e.IsDead && e.Health > 0 && e.IsVisible && SmiteDamage.IMportantMonsters.Contains(e.BaseSkinName));
                var dragonline = EntityManager.MinionsAndMonsters.GetLineFarmLocation(dragon, Q.Width, (int)Q.MaximumRange);
                if (Q.IsCharging && Q.IsFullyCharged && dragonline.CastPosition.IsValid())
                {
                    Q.Cast(dragonline.CastPosition);
                    return;
                }

                else if (dragon.Count() > 0)
                {
                    Q.StartCharging();
                    return;
                }

            }

            if (Settings.UseQ && Q.IsReady() && Player.Instance.ManaPercent >= Settings.MinMana || Q.IsCharging)
            {
                var MinionsQ = EntityManager.MinionsAndMonsters.GetJungleMonsters().Where(a => a.IsInRange(Player.Instance.ServerPosition, Q.MaximumRange + 200)).OrderByDescending(e => e.MaxHealth);
                var Qfarm = EntityManager.MinionsAndMonsters.GetLineFarmLocation(MinionsQ, Q.Width, (int)Q.MaximumRange);
                

                if (Q.IsCharging && Q.IsFullyCharged && Qfarm.CastPosition.IsValid())
                {
                    Q.Cast(Qfarm.CastPosition);
                    return;
                }

                else if (MinionsQ.Count() > 1)
                {
                    Q.StartCharging();
                    return;
                }
  
            }
        }
    }
}
