using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using Settings = KA_Rumble.Config.Modes.Combo;

namespace KA_Rumble.Modes
{
    public sealed class Combo : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {
            
            var target = TargetSelector.GetTarget(E.Range, DamageType.Magical);
            if (target != null && !target.IsZombie && !target.HasUndyingBuff())
            {
                if (E.IsReady() && target.IsValidTarget(E.Range) && Settings.UseE &&
                (Functions.ShouldOverload(SpellSlot.E) || Player.Instance.Mana < 80))
                {
                    var pred = E.GetPrediction(target);
                    if (pred.HitChance >= HitChance.Medium)
                    {
                        E.Cast(E.GetPrediction(target).CastPosition);
                    }
                }

                if (Player.Instance.Spellbook.GetSpell(SpellSlot.E).ToggleState == 1)
                {
                    var pred = E.GetPrediction(target);
                    if (pred.HitChance >= HitChance.Medium)
                    {
                        E.Cast(E.GetPrediction(target).CastPosition);
                    }
                }

                if (Q.IsReady() && target.IsValidTarget(Q.Range) && Settings.UseQ && Player.Instance.IsFacing(target) &&
                    (Functions.ShouldOverload(SpellSlot.Q) || Player.Instance.Mana < 80))
                {
                    Q.Cast();
                }
            }
            
            
            var targetR = TargetSelector.GetTarget(R.Range, DamageType.Magical);
            if (targetR == null || targetR.IsZombie || targetR.HasUndyingBuff()) return;

            if (R.IsReady() && targetR.IsValidTarget(R.Range) && Settings.UseR &&
                !targetR.IsInRange(Player.Instance, E.Range) && !targetR.IsFacing(Player.Instance))
            {
                if (Settings.UseRKillable &&
                    Prediction.Health.GetPrediction(targetR, R.CastDelay) <=
                    SpellDamage.GetRealDamage(SpellSlot.R, targetR))
                {
                    Functions.CastR(targetR, Settings.MinR);
                }
                else
                {
                    Functions.CastR(targetR, Settings.MinR);
                }
            }
        }
    }
}
