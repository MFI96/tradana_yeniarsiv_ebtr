using EloBuddy;
using EloBuddy.SDK;
using Settings = JusticeTalon.Config.Modes.Harass;

namespace JusticeTalon.Modes
{
    public sealed class Harass : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
        }

        public override void Execute()
        {
            if (Player.Instance.ManaPercent < Config.Modes.Harass.ManaUsage)
                return;
            var harassdusman = TargetSelector.GetTarget(W.Range, DamageType.Physical);
            if (harassdusman != null)
            {
                if (Config.Modes.Harass.UseW && W.IsReady() && harassdusman.IsValidTarget(W.Range))
                {
                    W.Cast(harassdusman);
                }
                if (Config.Modes.ItemUsage.itemusage)
                {
                    ModeManager.Useitems();
                }
            }
        }
    }
}