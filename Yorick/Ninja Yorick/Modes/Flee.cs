using EloBuddy;
using EloBuddy.SDK;
using Settings = Yorick.Config.Flee.FleeMenu;

namespace Yorick.Modes
{
    public sealed class Flee : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee);
        }

        public override void Execute()
        {
            if (Settings.UseW && W.IsReady())
            {
                var targetw = TargetSelector.GetTarget(W.Range, DamageType.Physical);

                if (targetw != null)
                {
                    W.Cast(targetw);
                }
            }
        }
    }
}