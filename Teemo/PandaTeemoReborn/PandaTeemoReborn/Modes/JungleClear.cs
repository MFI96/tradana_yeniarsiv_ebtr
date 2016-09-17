using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace PandaTeemoReborn.Modes
{
    public sealed class JungleClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear);
        }

        public override void Execute()
        {
            if (Extensions.MenuValues.JungleClear.UseR && R.IsReady())
            {
                if (!Extensions.HasShroomLanded)
                {
                    return;
                }

                if (Environment.TickCount - Extensions.LastR < Extensions.MenuValues.JungleClear.RDelay)
                {
                    return;
                }

                if (Extensions.MenuValues.JungleClear.ManaR >= Player.Instance.ManaPercent ||
                    Player.Instance.Spellbook.GetSpell(SpellSlot.R).Ammo < Extensions.MenuValues.JungleClear.RCharge)
                {
                    return;
                }

                IEnumerable<Obj_AI_Minion> mobs = null;

                if (Extensions.MenuValues.JungleClear.ROverkill)
                {
                    mobs =
                        EntityManager.MinionsAndMonsters.GetJungleMonsters()
                            .Where(
                                m =>
                                    m.IsValidTarget(R.Range) &&
                                    Extensions.DamageLibrary.CalculateDamage(m, false, true) >= m.Health);
                }

                if (!Extensions.MenuValues.JungleClear.ROverkill)
                {
                    mobs =
                        EntityManager.MinionsAndMonsters.GetJungleMonsters()
                            .Where(m => m.IsValidTarget(R.Range));
                }

                if (mobs == null) return;

                if (Extensions.MenuValues.JungleClear.RPoison)
                {
                    mobs = mobs.Where(m => !m.HasBuffOfType(BuffType.Poison));
                }

                if (Extensions.MenuValues.JungleClear.BigMob)
                {
                    mobs = mobs.Where(m => Extensions.JungleMobsList.Contains(m.BaseSkinName));
                }

                var prediction = EntityManager.MinionsAndMonsters.GetCircularFarmLocation(mobs,
                    R.Width, (int) R.Range);

                if (prediction.HitNumber >= Extensions.MenuValues.JungleClear.MobR)
                {
                    R.Cast(prediction.CastPosition);
                }
            }
        }
    }
}