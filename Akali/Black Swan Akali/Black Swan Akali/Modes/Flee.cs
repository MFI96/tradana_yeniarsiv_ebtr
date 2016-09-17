using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace Black_Swan_Akali.Modes
{
    public static class Flee
    {
        public static bool ShouldBeExecuted()
        {
            return ModeController.OrbFlee;
        }

        public static void Execute()
        {
            if (Spells.W.IsReady() && Return.UseWFlee)
                Spells.W.Cast();
        }
    }
}
