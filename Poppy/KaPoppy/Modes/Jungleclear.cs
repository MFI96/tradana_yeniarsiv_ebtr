using EloBuddy.SDK;
using KaPoppy;
using System.Linq;

namespace Modes
{
    using Menu = Settings.JungleclearSettings;
    class Jungleclear : Helper
    {
        public static void Execute()
        {
            var target = EntityManager.MinionsAndMonsters.GetJungleMonsters().OrderByDescending(x => x.MaxHealth).FirstOrDefault(x => x.IsValidTarget(Lib.E.Range + 300));
            if (target == null) return;

            if (Menu.UseW && Lib.W.IsReady())
            {
                if (Menu.Whealth)
                {
                    if (myHero.HealthPercent <= 40)
                        Lib.W.Cast();
                }
                else
                {
                    Lib.W.Cast();
                }
            }

            if (Menu.UseQ && Lib.Q.IsReady())
            {
                if (target.IsValidTarget(Lib.Q.Range))
                {
                    Lib.Q.Cast(target);
                }
            }

            else if (Menu.UseE && Lib.E.IsReady())
            {
                if (target.IsValidTarget(Lib.E.Range))
                {
                    if (Program.rectangle != null)
                    {
                        if (!Program.rectangle.IsInside(target.Position))
                            Lib.E.Cast(target);
                    }
                    else
                        Lib.E.Cast(target);
                }
            }
        }
    }
}