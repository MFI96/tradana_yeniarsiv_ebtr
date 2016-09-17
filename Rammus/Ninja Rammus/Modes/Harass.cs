using EloBuddy;
using EloBuddy.SDK;

using Settings = Rammus.Modes.Harass;

namespace Rammus.Modes
{
    public sealed class Harass : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
        }

        public override void Execute()
        {
        }
    }
}
