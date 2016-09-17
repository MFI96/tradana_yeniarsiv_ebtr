using EloBuddy;
using EloBuddy.SDK;

namespace KA_Shyvanna.Modes
{
    public sealed class Flee : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(R.Range, DamageType.Magical);
            if (Player.Instance.HealthPercent <= 15 && target.IsValidTarget(E.Range))
            {
                R.Cast(Player.Instance.Position.Extend(Game.CursorPos, R.Range).To3D());
            }
        }
    }
}
