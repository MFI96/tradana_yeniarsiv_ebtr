using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace PandaTeemoReborn.Modes
{
    public sealed class LaneClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear);
        }

        public override void Execute()
        {
            if (Extensions.MenuValues.LaneClear.UseR && R.IsReady())
            {
                if (!Extensions.HasShroomLanded)
                {
                    return;
                }

                if (Environment.TickCount - Extensions.LastR < Extensions.MenuValues.LaneClear.RDelay)
                {
                    return;
                }

                if (Extensions.MenuValues.LaneClear.ManaR >= Player.Instance.ManaPercent ||
                    Player.Instance.Spellbook.GetSpell(SpellSlot.R).Ammo < Extensions.MenuValues.LaneClear.RCharge)
                {
                    return;
                }

                IEnumerable<Obj_AI_Minion> minions = null;

                if (Extensions.MenuValues.LaneClear.ROverkill)
                {
                    minions =
                        EntityManager.MinionsAndMonsters.GetLaneMinions()
                            .Where(
                                m =>
                                    m.IsValidTarget(R.Range) &&
                                    Extensions.DamageLibrary.CalculateDamage(m, false, true) >= m.Health);
                }

                if (!Extensions.MenuValues.LaneClear.ROverkill)
                {
                    minions =
                        EntityManager.MinionsAndMonsters.GetLaneMinions()
                            .Where(m => m.IsValidTarget(R.Range));
                }

                if (minions == null) return;

                if (Extensions.MenuValues.LaneClear.RPoison)
                {
                    minions = minions.Where(m => !m.HasBuffOfType(BuffType.Poison));
                }

                var prediction = EntityManager.MinionsAndMonsters.GetCircularFarmLocation(minions,
                    R.Width, (int)R.Range);

                if (prediction.HitNumber >= Extensions.MenuValues.LaneClear.MinionR)
                {
                    R.Cast(prediction.CastPosition);
                }
            }
        }
    }
}