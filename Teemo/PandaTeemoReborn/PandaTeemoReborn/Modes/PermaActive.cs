using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;

namespace PandaTeemoReborn.Modes
{
    public sealed class PermaActive : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return !Player.Instance.IsRecalling();
        }

        public override void Execute()
        {
            SpellManager.R = new Spell.Skillshot(SpellSlot.R, new uint[] {0, 400, 650, 900}[R.Level],
                SkillShotType.Circular, 0, 1000, 135);

            AutoQ();
            AutoW();
            KillSteal();
            AutoShroom();
        }

        /// <summary>
        /// Auto Q
        /// </summary>
        private static void AutoQ()
        {
            if (!Extensions.MenuValues.Misc.AutoQ)
            {
                return;
            }

            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            var allMinionsQ =
                EntityManager.MinionsAndMonsters.EnemyMinions.Where(t => Q.IsInRange(t)).OrderBy(t => t.Health);

            if (target == null)
            {
                return;
            }

            if (Q.IsReady() && allMinionsQ.Any())
            {
                foreach (
                    var minion in
                        allMinionsQ.Where(
                            minion =>
                                minion.Health <= Extensions.DamageLibrary.CalculateDamage(minion, true, false) &&
                                Q.IsInRange(minion)))
                {
                    Q.Cast(minion);
                }
            }
            else if (Q.IsReady() && target.IsValidTarget(Q.Range) && Player.Instance.ManaPercent >= 25)
            {
                Q.Cast(target);
            }
        }

        /// <summary>
        /// Auto W
        /// </summary>
        private static void AutoW()
        {
            if (!Extensions.MenuValues.Misc.AutoW)
            {
                return;
            }

            if (!W.IsReady())
            {
                return;
            }

            if (W.IsReady())
            {
                W.Cast();
            }
        }

        public static void AutoShroom()
        {
            if (!Extensions.HasShroomLanded)
            {
                return;
            }

            if (!Extensions.MenuValues.AutoShroom.UseR || !R.IsReady()) return;

            if (Extensions.MenuValues.AutoShroom.ManaR >= Player.Instance.ManaPercent ||
                Player.Instance.Spellbook.GetSpell(SpellSlot.R).Ammo < Extensions.MenuValues.AutoShroom.RCharge)
            {
                return;
            }

            var positions =
                PandaTeemoReborn.AutoShroom.ShroomPosition.Where(pos => pos.IsInRange(Player.Instance, R.Range))
                    .ToArray();

            if (!positions.Any())
            {
                return;
            }

            var place = positions.FirstOrDefault(pos => !Extensions.IsShroomed(pos));
            var castPos = new Vector3(place.X + new Random().Next(0, 100), place.Y + new Random().Next(0, 100),
                place.Z + new Random().Next(0, 100));

            R.Cast(castPos);
        }

        public static void KillSteal()
        {
            if (Extensions.MenuValues.KillSteal.UseQ && Q.IsReady())
            {
                if (Extensions.MenuValues.KillSteal.ManaQ >= Player.Instance.ManaPercent)
                {
                    return;
                }

                var targets =
                    EntityManager.Heroes.Enemies.Where(
                        t =>
                            t.IsValidTarget(Q.Range) &&
                            Extensions.DamageLibrary.CalculateDamage(t, true, false) >= t.Health);
                var target = TargetSelector.GetTarget(targets, DamageType.Magical);

                if (target != null)
                {
                    Q.Cast(target);
                }
            }

            if (Extensions.MenuValues.KillSteal.UseR && R.IsReady())
            {
                if (!Extensions.HasShroomLanded)
                {
                    return;
                }

                if (Environment.TickCount - Extensions.LastR < Extensions.MenuValues.KillSteal.RDelay)
                {
                    return;
                }

                if (Extensions.MenuValues.KillSteal.ManaR >= Player.Instance.ManaPercent)
                {
                    return;
                }

                var targets =
                    EntityManager.Heroes.Enemies.Where(
                        t =>
                            t.IsValidTarget(R.Range) &&
                            Extensions.DamageLibrary.CalculateDamage(t, false, true) >= t.Health);
                var target = TargetSelector.GetTarget(targets, DamageType.Magical);

                if (target != null)
                {
                    R.Cast(target);
                }

                if (!Extensions.MenuValues.KillSteal.DoubleShroom) return;

                var prediction = Extensions.TeemoShroomPrediction.GetPrediction();

                if (prediction.HitCount <= 0) return;

                var targets2 =
                    EntityManager.Heroes.Enemies.Where(
                        t =>
                            t.IsValidTarget(R.Radius, false, prediction.BouncePosition) &&
                            Extensions.DamageLibrary.CalculateDamage(t, false, true) >= t.Health);
                var target2 = TargetSelector.GetTarget(targets2, DamageType.Magical);

                if (target2 == null)
                {
                    return;
                }

                R.Cast(prediction.CastPosition);
            }
        }
    }
}