using EloBuddy.SDK;
using EloBuddy;
using Settings = Rammus.Config.Modes.Flee;

namespace Rammus.Modes
{
    public sealed class Flee : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee);
        }

        public override void Execute()
        {
            if (Spinning())
            {
                return;
            }

            if (Settings.UseQ && Q.IsReady())
            {
                Q.Cast();
                return;
            }
        }
    }
}
