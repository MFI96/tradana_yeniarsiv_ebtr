using EloBuddy.SDK;
using EloBuddy;
using System.Linq;

namespace Vi.Modes
{
    public sealed class Flee : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee);
        }

        public override void Execute()
        {

        }
    }
}
