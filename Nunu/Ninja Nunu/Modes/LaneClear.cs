using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using Settings = NinjaNunu.Config.Modes.LaneClear;

namespace NinjaNunu.Modes
{
    public sealed class LaneClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear);
        }

        public override void Execute()
        {           
            if (Settings.UseE && E.IsReady() && Player.Instance.ManaPercent >= Settings.MinMana || Player.Instance.HasBuff("Visions") && Settings.UseE && E.IsReady())
            {
                var Lmonsters = EntityManager.MinionsAndMonsters.GetLaneMinions().OrderByDescending(a => a.MaxHealth).FirstOrDefault(b => b.Distance(Player.Instance) < E.Range);
                if (Lmonsters != null)
                {
                    E.Cast(Lmonsters);
                    return;
                }
            } 
          
            if (Settings.UseW && W.IsReady() && Player.Instance.ManaPercent >= Settings.MinMana)
            {
                var ally = EntityManager.Heroes.Allies.OrderByDescending(a => a.TotalAttackDamage).FirstOrDefault(b => b.Distance(Player.Instance) < 1000);
                if (ally != null && Player.Instance.CountEnemiesInRange(1500) > 0)
                {
                    W.Cast(ally);
                    return;
                }
                if (Settings.UseW && W.IsReady() && Player.Instance.CountEnemiesInRange(1500) > 0)
                {
                    W.Cast(Player.Instance);
                    return;
                }
            }

            if (Settings.UseQ && Q.IsReady() && Player.Instance.ManaPercent >= Settings.MinMana)
            {
                var Lmonsters = EntityManager.MinionsAndMonsters.GetLaneMinions().OrderByDescending(a => a.MaxHealth).FirstOrDefault(b => b.Distance(Player.Instance) < 220);
                if (Lmonsters != null)
                {
                    Q.Cast(Lmonsters);
                    return;
                }
            }
        }
    }
}
