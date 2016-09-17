using EloBuddy;
using EloBuddy.SDK;
using Settings = Yorick.Config.Harass.HarassMenu;

namespace Yorick.Modes
{
    public sealed class Harass : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
        }

        public override void Execute()
        {
            if (Settings.UseE && E.IsReady() && Mana >= Settings.ManaE)
            {
                var targete = TargetSelector.GetTarget(E.Range, DamageType.Physical);

                if (targete != null)
                {
                    E.Cast(targete);
                    return;
                }
            }

            if (Settings.UseW && W.IsReady() && Mana >= Settings.ManaW)
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