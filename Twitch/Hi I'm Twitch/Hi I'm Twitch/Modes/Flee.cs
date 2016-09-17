using EloBuddy.SDK;

using Settings = AddonTemplate.Config.Modes.Misc;

namespace AddonTemplate.Modes
{
    public sealed class Flee : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee);
        }

        public override void Execute()
        {
            if (Settings.UseQ && Q.IsReady())
            {
                Q.Cast();
            }
        }
    }
}
