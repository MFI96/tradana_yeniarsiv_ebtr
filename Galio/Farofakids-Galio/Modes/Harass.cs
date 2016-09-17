using EloBuddy;
using EloBuddy.SDK;
using Settings = Farofakids_Galio.Config.Modes.Harass;

namespace Farofakids_Galio.Modes
{
    public sealed class Harass : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
        }

        public override void Execute()
        {
            if (Player.Instance.ManaPercent < Settings.Mana)
            {
                return;
            }
            var target = TargetSelector.GetTarget(800, DamageType.Magical);
            if (target != null)
            {
                if (Settings.UseE && E.IsReady())
                {
                    E.Cast(target);
                }
                if (Settings.UseQ && Q.IsReady())
                {
                    Q.Cast(target);
                }

            }
        }
    }
}
