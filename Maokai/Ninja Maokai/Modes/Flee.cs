using EloBuddy;
using EloBuddy.SDK;
using Settings = Maokai.Config.Flee.FleeMenu;

namespace Maokai.Modes
{
    public sealed class Flee : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            if (Settings.UseQ && Q.IsReady())
            {
                if (target != null && target.IsValidTarget(Q.Range))
                {
                    Q.Cast(target);
                }
            }
        }
    }
}