using EloBuddy;
using EloBuddy.SDK;
using KaPoppy;
namespace Modes
{
    using System.Linq;
    using Menu = Settings.HarassSettings;
    class Harass : Helper
    {
        public static void Execute()
        {
            var target = TargetSelector.GetTarget(Lib.E.Range, DamageType.Physical);
            if (target == null) return;

            if (Menu.UseQ)
            {
                if (Lib.Q.IsReady())
                {
                    if (target.IsValidTarget(Lib.Q.Range))
                    {
                        var pred = Lib.Q.GetPrediction(target);
                        if (pred.HitChance >= EloBuddy.SDK.Enumerations.HitChance.Medium)
                            Lib.Q.Cast(pred.CastPosition);
                    }
                }
            }

            if (Menu.UseE)
            {
                if (target.IsValidTarget(Lib.E.Range))
                {
                    var ally = EntityManager.Heroes.Allies.Where(x => !x.IsMe && !x.IsDead && x.Health > 200 && x.Distance(myHero) < 1200).OrderBy(x => x.Distance(myHero));
                    var turret = EntityManager.Turrets.Allies.Where(x => !x.IsDead && x.Distance(myHero) < 1200).OrderBy(x => x.Distance(myHero));
                    var push = target.ServerPosition.Extend(myHero.ServerPosition, -300);
                    if (
                        (Menu.UseEStun && Lib.CanStun(target)) ||
                        (Menu.UseEPassive && Lib.Passive != null && push.Distance(Lib.Passive) < myHero.Distance(Lib.Passive)) ||
                        (Menu.UseEInsec && turret.Count() > 0 && push.Distance(turret.First()) < target.Distance(turret.First())) ||
                        (Menu.UseEInsec && ally.Count() > 0 && push.Distance(ally.First()) < target.Distance(ally.First()))
                        )
                    {
                        Lib.E.Cast(target);
                    }
                    else if (Menu.UseEAlways)
                    {
                        Lib.E.Cast(target);
                    }
                }
            }

            if (Menu.UseW)
            {
                if (Menu.Whealth)
                    if (myHero.HealthPercent >= 40)
                        return;

                Lib.W.Cast();
            }
        }
    }
}