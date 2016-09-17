using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using Settings = KA_Rumble.Config.Modes.Harass;

namespace KA_Rumble.Modes
{
    public sealed class Harass : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(E.Range, DamageType.Physical);
            if (target == null || target.IsZombie) return;

            if (E.IsReady() && target.IsValidTarget(E.Range) && Settings.UseE && Player.Instance.Mana < 70)
            {
                var pred = E.GetPrediction(target);
                if (pred.HitChance >= HitChance.Medium)
                {
                    E.Cast(E.GetPrediction(target).CastPosition);
                }
            }

            if (Q.IsReady() && target.IsValidTarget(Q.Range) && Settings.UseQ && Player.Instance.IsFacing(target) && Player.Instance.Mana < 70)
            {
                Q.Cast();
            }
        }
    }
}
