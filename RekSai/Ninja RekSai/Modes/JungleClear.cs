using EloBuddy;
using EloBuddy.SDK;
using System.Linq;

using Settings = RekSai.Config.JungleClear.JungleClearMenu;

namespace RekSai.Modes
{
    public sealed class JungleClear : ModeBase
    {
             
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear);
        }

        public override void Execute()
        {

            var minionsE = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.Position, E.Range).Where(a => a.IsValidTarget()).OrderByDescending(a => a.MaxHealth).FirstOrDefault();
            var minionsQ2 = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.Position, 800).Where(a => a.IsValidTarget()).OrderByDescending(a => a.MaxHealth).FirstOrDefault();
            var minionsW = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.Position, Player.Instance.BoundingRadius).Where(a => a.IsValidTarget()).OrderByDescending(a => a.MaxHealth).FirstOrDefault();

            if (Events.burrowed)
            {
                if (Q2.IsOnCooldown && W.IsReady() && Settings.UseW && minionsW != null)
                {
                    W.Cast();
                    return;
                }

                if (Settings.UseQ2 && Q2.IsReady() && minionsQ2 != null)
                {
                    Q2.Cast(minionsQ2);
                    return;
                }

                
            }

            if (!Events.burrowed)
            {
                if (Settings.UseE && E.IsReady())
                {
                    if (minionsE != null && minionsE.Health <= SmiteDamage.EDamage(minionsE))
                    {
                        E.Cast(minionsE);
                        return;
                    }
                    if (Player.Instance.Mana == 100)
                    {
                        E.Cast(minionsE);
                        return;
                    }
                }

                if (W.IsReady() && Settings.UseW && EntityManager.MinionsAndMonsters.Combined.Where(a => a.IsValidTarget(300)).Count() == 0)
                {
                    W.Cast();
                    return;
                }

                if (W.IsReady() && Settings.UseW && Q2.IsReady() && minionsQ2 != null && !minionsW.HasBuff("reksaiknockupimmune") && !Player.Instance.HasBuff("RekSaiQ"))
                {
                    W.Cast();
                    return;
                }
            }
        }
    }
}
