using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace Black_Swan_Akali.Modes
{
    public static class Harass
    {
        public static bool ShouldBeExecuted()
        {
            return ModeController.OrbHarass;
        }

        public static void Execute()
        {
            var t = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Magical);

            if (Spells.Q.IsReady() && Return.UseQHarass)
            {
                if (t != null)
                    Spells.Q.Cast(t);
            }
        }
    }
}
