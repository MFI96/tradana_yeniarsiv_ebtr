using EloBuddy;
using EloBuddy.SDK;

//using Settings = RekSai.Config.LastHit.LastHitMenu;

namespace RekSai.Modes
{
    public sealed class LastHit : ModeBase
    {


        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit);
        }

        public override void Execute()
        {
     
            //if(!burrowed)
            //{       
            //    if (Settings.UseW && W.IsReady() && Q2.IsReady())
            //    {
            //        var minionsQ2 = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.Position, Q2.Range).OrderByDescending(a => a.MaxHealth).FirstOrDefault();
                    
            //        if (minionsQ2.HealthPercent<= 50)
            //        {
            //            W.Cast();
            //            return;
            //        }
            //    }

            //}

            //if (burrowed)
            //{
            //    if(Settings.UseQ2 && Q2.IsReady())
            //    {
            //        var minionsQ2L = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.Position, Q2.Range).OrderByDescending(a => a.MaxHealth).FirstOrDefault();
            //        if (minionsQ2L.Health <= SmiteDamage.Q2DamageLastHit(minionsQ2L))
            //        {
            //            Q2.Cast(minionsQ2L);
            //        }
            //    }

            //    if (Settings.UseW && W.IsReady())
            //    {
            //        if (Q2.IsOnCooldown)
            //        {
            //            W.Cast();
            //            return;
            //        }
            //    }

            //}
        }
    }
}
