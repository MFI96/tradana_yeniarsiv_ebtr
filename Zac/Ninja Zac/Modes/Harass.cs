using EloBuddy;
using EloBuddy.SDK;
using Settings = Zac.Config.Harass.HarassMenu;

namespace Zac.Modes
{
    public sealed class Harass : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
        }

        public override void Execute()
        {
            if (Q.IsReady() && Settings.UseQ)
            {
                var targetQ = TargetSelector.GetTarget(Q.Range, DamageType.Magical);

                if (targetQ != null && targetQ.IsValid)
                {
                    Q.Cast(targetQ);
                    return;
                }
            }
            if (W.IsReady() && Settings.UseW)
            {
                var targetW = TargetSelector.GetTarget(W.Range -25, DamageType.Magical);

                if (targetW != null && targetW.IsValid)
                {
                    W.Cast();
                }
            }
        }
    }
}