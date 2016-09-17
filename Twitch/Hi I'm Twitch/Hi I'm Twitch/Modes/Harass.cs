using EloBuddy;
using EloBuddy.SDK;

using Settings = AddonTemplate.Config.Modes.Harass;

namespace AddonTemplate.Modes
{
    public sealed class Harass : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
        }

        public override void Execute()
        {
            if (Settings.UseW && Player.Instance.ManaPercent > Settings.ManaW && W.IsReady())
            {
                var target = TargetSelector.GetTarget(W.Range, DamageType.Physical);
                if (target != null && target.IsValidTarget())
                {
                    var predW = W.GetPrediction(target);
                    W.Cast(predW.CastPosition);
                }
            }
            if (Settings.UseE && Player.Instance.ManaPercent > Settings.ManaE && E.IsReady())
            {
                var target = TargetSelector.GetTarget(W.Range, DamageType.Physical);
                if (target != null && target.IsValidTarget())
                {
                    if (target.getEStacks() >= Settings.HarassStacks)
                    {
                        E.Cast();
                    }
                }
            }
        }
    }
}
