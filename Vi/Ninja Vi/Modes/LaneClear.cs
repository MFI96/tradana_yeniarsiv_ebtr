using EloBuddy.SDK;
using EloBuddy;
using System;
using System.Linq;
using Setting = Vi.Config.Modes.LaneClear;

namespace Vi.Modes
{
    public sealed class LaneClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear);
        }

        public override void Execute()
        {
            if (Setting.UseQ && Q.IsReady() && Player.Instance.ManaPercent >= Setting.MinMana || Q.IsCharging)
            {
                var MinionsQ = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy).Where(a => a.IsInRange(Player.Instance.ServerPosition, Q.MaximumRange));                           
                
                
                    var Qfarm = EntityManager.MinionsAndMonsters.GetLineFarmLocation(MinionsQ, 100, (int)Q.MaximumRange);
                    if (Q.IsCharging && Q.IsFullyCharged && Qfarm.CastPosition.IsValid())
                    {
                        Q.Cast(Qfarm.CastPosition);
                        return;
                    }

                    else if (Qfarm.HitNumber > 3)
                    {
                        Q.StartCharging();
                        return;
                    }
                            
            }
        }
    }
}
