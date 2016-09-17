using EloBuddy.SDK;
using KaPoppy;
using System.Linq;
namespace Modes
{
    using Menu = Settings.LaneclearSettings;
    class Laneclear : Helper
    {
        public static void Execute()
        {
            var targets = EntityManager.MinionsAndMonsters.EnemyMinions.Where(x => !x.IsDead && x.IsValidTarget(Lib.Q.Range));
            if (!(targets.Count() > 0)) return;
            var target = EntityManager.MinionsAndMonsters.GetLineFarmLocation(targets, Lib.Q.Width, (int)Lib.Q.Range);

            if (Menu.UseQ)
            {
                if (Lib.Q.IsReady())
                {
                    if (target.HitNumber >= Menu.Qmin)
                    {
                        Lib.Q.Cast(target.CastPosition);
                    }
                }
            }
        }
    }    
}