using System;
using EloBuddy;
using EloBuddy.SDK;

namespace PandaTeemoReborn.Modes
{
    public sealed class Combo : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {
            if (Extensions.MenuValues.Combo.UseW && W.IsReady())
            {
                if (Extensions.MenuValues.Combo.ManaW >= Player.Instance.ManaPercent)
                {
                    return;
                }

                var countEnemies = Player.Instance.CountEnemiesInRange(Player.Instance.GetAutoAttackRange());

                if (Extensions.MenuValues.Combo.WRange && countEnemies > 0)
                {
                    W.Cast();
                }
                else if (!Extensions.MenuValues.Combo.WRange)
                {
                    W.Cast();
                }
            }

            if (Extensions.MenuValues.Combo.UseR && R.IsReady())
            {
                if (!Extensions.HasShroomLanded)
                {
                    return;
                }

                if (Environment.TickCount - Extensions.LastR < Extensions.MenuValues.Combo.RDelay)
                {
                    return;
                }

                if (Extensions.MenuValues.Combo.ManaR >= Player.Instance.ManaPercent ||
                    Player.Instance.Spellbook.GetSpell(SpellSlot.R).Ammo < Extensions.MenuValues.Combo.RCharge)
                {
                    return;
                }

                var target = TargetSelector.GetTarget(R.Range, DamageType.Magical);

                if (target != null)
                {
                    if (Extensions.MenuValues.Combo.RPoison && target.HasBuffOfType(BuffType.Poison))
                    {
                        return;
                    }

                    if (!Extensions.MenuValues.Combo.RPoison || Extensions.MenuValues.Combo.RPoison && !target.HasBuffOfType(BuffType.Poison))
                    {
                        R.Cast(target);
                    }
                }

                if (Extensions.MenuValues.Combo.DoubleShroom)
                {
                    var prediction = Extensions.TeemoShroomPrediction.GetPrediction();

                    if (prediction.HitCount > 0)
                    {
                        R.Cast(prediction.CastPosition);
                    }
                }
            }
        }
    }
}