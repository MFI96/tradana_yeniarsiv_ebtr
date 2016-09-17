using EloBuddy.SDK;
using EloBuddy;
using System.Linq;
using Settings = Rammus.Config.Modes.JungleClear;

namespace Rammus.Modes
{
    public sealed class JungleClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear);
        }

        public override void Execute()
        {
            if (Spinning())
            {
                return;
            }
           if (Settings.UseQ && Q.IsReady())
           {
               if (EntityManager.MinionsAndMonsters.GetJungleMonsters().Count(a => a.IsValidTarget(1000)) > 0)
               {
                   Q.Cast();
                   return;
               }
           }

            if (Settings.UseW && W.IsReady())
            {
                if (EntityManager.MinionsAndMonsters.GetJungleMonsters().Count(a => a.IsValidTarget(300)) > 0)
                {
                    W.Cast();
                    return;
                }
            }
        }
    }
}
