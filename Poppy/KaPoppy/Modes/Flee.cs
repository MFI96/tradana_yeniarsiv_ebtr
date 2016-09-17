using EloBuddy;
using EloBuddy.SDK;
using KaPoppy;
namespace Modes
{
    using System.Linq;
    using Menu = Settings.FleeSettings;
    class Flee : Helper
    {

        public static void Execute()
        {
            var target = ObjectManager.Get<Obj_AI_Minion>().Where(x => myHero.ServerPosition.Extend(x, myHero.Distance(x) + 300).Distance(Game.CursorPos) < myHero.Distance(Game.CursorPos) &&
            x.IsValidTarget(Lib.E.Range)).OrderBy(x => x.Distance(Game.CursorPos));
            var flypls = EntityManager.Heroes.Enemies.Where(x => !x.IsDead && !x.HasBuffOfType(BuffType.SpellImmunity) && x.IsValidTarget(Lib.R.MaximumRange - 250)).OrderBy(x => x.Distance(myHero));
            if (Menu.UseR)
            {
                if (Lib.R.IsReady())
                {
                    if (flypls.Count() > 0)
                    {
                        if (!Lib.R.IsCharging)
                        {
                            Lib.R.StartCharging();
                            return;
                        }
                        var pred = Lib.R.GetPrediction(flypls.First());
                        if (pred.HitChance >= EloBuddy.SDK.Enumerations.HitChance.Medium)
                        {
                            Lib.R.Cast(pred.CastPosition);
                        }
                    }
                }
            }
            if (Menu.UseE)
            {
                if (target.Count() > 0)
                {
                    if (Lib.E.IsReady() && Lib.E.IsInRange(target.First()))
                    {
                        Lib.E.Cast(target.First());
                    }
                }
            }
            if (Menu.UseW)
            {
                if (myHero.CountEnemiesInRange(1000) > 0)
                {
                    Lib.W.Cast();
                }
            }
        }
    }
}

