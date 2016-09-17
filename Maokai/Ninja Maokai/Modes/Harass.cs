using EloBuddy;
using EloBuddy.SDK;
using Settings = Maokai.Config.Harass.HarassMenu;

namespace Maokai.Modes
{
    public sealed class Harass : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(E.Range, DamageType.Magical);

            if (Settings.UseE && E.IsReady() && ManaPercent >= Settings.ManaE)
            {
                if (target != null && target.IsValidTarget(E.Range))
                {
                    E.Cast(target);
                    return;
                }
            }

            if (Settings.UseQ && Q.IsReady() && ManaPercent >= Settings.ManaQ)
            {
                if (target != null && target.IsValidTarget(Q.Range))
                {
                    Q.Cast(target);
                }
            }
        }
    }
}