using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using SharpDX;

namespace PandaTeemoReborn
{
    internal class Extensions
    {
        /// <summary>
        /// Stores the Last Time Shroom has Landed
        /// </summary>
        public static bool HasShroomLanded { get; set; } = true;
        
        /// <summary>
        /// Stores the Last Time R was used.
        /// </summary>
        public static int LastR { get; set; }

        /// <summary>
        /// Array of ADC's
        /// </summary>
        public static readonly Champion[] Marksman =
        {
            Champion.Ashe, Champion.Caitlyn, Champion.Corki, Champion.Draven, Champion.Ezreal, Champion.Graves,
            Champion.Jhin, Champion.Jinx, Champion.Kalista, Champion.Kindred, Champion.KogMaw, Champion.Lucian,
            Champion.MissFortune, Champion.Quinn, Champion.Sivir, Champion.Teemo, Champion.Tristana, Champion.Twitch,
            Champion.Urgot, Champion.Varus, Champion.Vayne
        };

        /// <summary>
        /// Jungle Mob List 
        /// </summary>
        public static readonly string[] JungleMobsList =
        {
            "SRU_Red", "SRU_Blue", "SRU_Dragon", "SRU_Baron", "SRU_Gromp",
            "SRU_Murkwolf", "SRU_Razorbeak", "SRU_Krug", "Sru_Crab", "TT_NWraith1.1", "TT_NWraith4.1",
            "TT_NGolem2.1", "TT_NGolem5.1", "TT_NWolf3.1", "TT_NWolf6.1", "TT_Spiderboss8.1"
        };

        /// <summary>
        /// Checks if there is shroom in location
        /// </summary>
        /// <param name="position">The location of check</param>
        /// <returns>If that location has a shroom.</returns>
        public static bool IsShroomed(Vector3 position)
        {
            return
                ObjectManager.Get<Obj_AI_Base>()
                    .Where(obj => obj.BaseSkinName == "TeemoMushroom")
                    .Any(obj => position.Distance(obj.Position) <= 250);
        }

        public static class TeemoShroomPrediction
        {
            /// <summary>
            /// Result of Prediction
            /// </summary>
            public struct Result
            {
                public Vector3 BouncePosition;
                public Vector3 CastPosition;
                public int HitCount;
            }

            /// <summary>
            /// Get's Prediction of Teemo's Double Shroom
            /// </summary>
            /// <param name="shroom"></param>
            /// <returns></returns>
            public static Result GetPrediction(Obj_AI_Base shroom = null)
            {
                if (shroom == null)
                {
                    shroom =
                        ObjectManager.Get<Obj_AI_Base>()
                            .FirstOrDefault(
                                obj =>
                                    obj.BaseSkinName == "TeemoMushroom" &&
                                    Player.Instance.IsInRange(obj, SpellManager.R.Range));
                }

                if (shroom == null)
                {
                    return new Result
                    {
                        BouncePosition = Vector3.Zero,
                        CastPosition = Vector3.Zero,
                        HitCount = 0
                    };
                }

                var castPos = Vector2.Zero;
                var bouncePos = Vector3.Zero;
                var hitCount = 0;

                for (var x = 0; x < 24; x++)
                {
                    for (var y = 0; y < 24; y++)
                    {
                        var newOffset = new Vector2(x, y);
                        var newCastPos = shroom.Position.To2D() + newOffset;
                        var newBouncePos = TeemoShroomBouncePosition(castPos);
                        var newHitCount =
                            EntityManager.Heroes.Enemies.Count(
                                t =>
                                    newBouncePos.IsInRange(
                                        Prediction.Position.PredictUnitPosition(t,
                                            CalculateTravelTime(newCastPos, newBouncePos)), SpellManager.R.Radius));

                        if (newHitCount > hitCount)
                        {
                            castPos = newCastPos;
                            bouncePos = newBouncePos;
                            hitCount = newHitCount;
                        }
                    }
                }

                for (var x = 0; x < 24; x++)
                {
                    for (var y = 0; y < 24; y++)
                    {
                        var newOffset = new Vector2(x, y);
                        var newCastPos = shroom.Position.To2D() - newOffset;
                        var newBouncePos = TeemoShroomBouncePosition(castPos);
                        var newHitCount =
                            EntityManager.Heroes.Enemies.Count(
                                t =>
                                    newBouncePos.IsInRange(
                                        Prediction.Position.PredictUnitPosition(t,
                                            CalculateTravelTime(newCastPos, newBouncePos)), SpellManager.R.Radius));

                        if (newHitCount > hitCount)
                        {
                            castPos = newCastPos;
                            bouncePos = newBouncePos;
                            hitCount = newHitCount;
                        }
                    }
                }

                if (castPos == Vector2.Zero && bouncePos == Vector3.Zero && hitCount == 0)
                {
                    castPos = shroom.Position.To2D();
                    bouncePos = TeemoShroomBouncePosition(castPos);
                    hitCount =
                        EntityManager.Heroes.Enemies.Count(
                            t =>
                                bouncePos.IsInRange(
                                    Prediction.Position.PredictUnitPosition(t,
                                        CalculateTravelTime(castPos, bouncePos)), SpellManager.R.Radius));
                }

                return new Result
                {
                    BouncePosition = bouncePos,
                    CastPosition = new Vector3(castPos, NavMesh.GetHeightForPosition(castPos.X, castPos.Y)),
                    HitCount = hitCount
                };
            }

            /// <summary>
            /// Returns the position of the shroom where it bounced off.
            /// </summary>
            /// <param name="shroomPosition"></param>
            /// <returns></returns>
            private static Vector3 TeemoShroomBouncePosition(Vector2 shroomPosition)
            {
                var extendedPosition = shroomPosition.Extend(Player.Instance,
                    -Player.Instance.Spellbook.GetSpell(SpellSlot.R).SData.BounceRadius);
                return new Vector3(extendedPosition,
                    NavMesh.GetHeightForPosition(extendedPosition.X, extendedPosition.Y));
            }

            /// <summary>
            /// Calculates R Travel Time given the cast position and bounce position
            /// </summary>
            /// <param name="castPos"></param>
            /// <param name="bouncePos"></param>
            /// <returns></returns>
            public static int CalculateTravelTime(Vector2 castPos, Vector3 bouncePos)
            {
                var distance = Player.Instance.Distance(castPos);
                var missileSpeed = SpellManager.R.Speed;

                if (bouncePos == Vector3.Zero)
                {
                    return (int) Math.Round(distance/missileSpeed) * 1000;
                }
                return (int) Math.Round((distance*2)/missileSpeed) * 1000;
            }
        }

        public static class MenuValues
        {
            public class Combo
            {
                public static bool UseQ => Config.ComboMenu["useQ"].Cast<CheckBox>().CurrentValue;

                public static bool UseW => Config.ComboMenu["useW"].Cast<CheckBox>().CurrentValue;

                public static bool UseR => Config.ComboMenu["useR"].Cast<CheckBox>().CurrentValue;

                public static bool RPoison => Config.ComboMenu["rPoison"].Cast<CheckBox>().CurrentValue;

                public static bool UseQMarksman => Config.ComboMenu["adc"].Cast<CheckBox>().CurrentValue;

                public static bool WRange => Config.ComboMenu["wRange"].Cast<CheckBox>().CurrentValue;

                public static bool DoubleShroom => Config.ComboMenu["doubleShroom"].Cast<CheckBox>().CurrentValue;

                public static int ManaQ => Config.ComboMenu["manaQ"].Cast<Slider>().CurrentValue;

                public static int ManaW => Config.ComboMenu["manaW"].Cast<Slider>().CurrentValue;

                public static int ManaR => Config.ComboMenu["manaR"].Cast<Slider>().CurrentValue;

                public static int CheckAutoAttack => Config.ComboMenu["checkAA"].Cast<Slider>().CurrentValue;

                public static int RCharge => Config.ComboMenu["rCharge"].Cast<Slider>().CurrentValue;

                public static int RDelay => Config.ComboMenu["rDelay"].Cast<Slider>().CurrentValue;
            }

            public class Harass
            {
                public static bool UseQ => Config.HarassMenu["useQ"].Cast<CheckBox>().CurrentValue;

                public static bool UseW => Config.HarassMenu["useW"].Cast<CheckBox>().CurrentValue;

                public static bool UseQMarksman => Config.HarassMenu["adc"].Cast<CheckBox>().CurrentValue;

                public static bool WRange => Config.HarassMenu["wRange"].Cast<CheckBox>().CurrentValue;

                public static int ManaQ => Config.HarassMenu["manaQ"].Cast<Slider>().CurrentValue;

                public static int ManaW => Config.HarassMenu["manaW"].Cast<Slider>().CurrentValue;

                public static int CheckAutoAttack => Config.HarassMenu["checkAA"].Cast<Slider>().CurrentValue;
            }

            public class LaneClear
            {
                public static bool UseQ => Config.LaneClearMenu["useQ"].Cast<CheckBox>().CurrentValue;

                public static bool UseR => Config.LaneClearMenu["useR"].Cast<CheckBox>().CurrentValue;

                public static int ManaQ => Config.LaneClearMenu["manaQ"].Cast<Slider>().CurrentValue;

                public static int ManaR => Config.LaneClearMenu["manaR"].Cast<Slider>().CurrentValue;

                public static bool HarassLogic => Config.LaneClearMenu["harass"].Cast<CheckBox>().CurrentValue;

                public static bool DisableLaneClear => Config.LaneClearMenu["disableLC"].Cast<CheckBox>().CurrentValue;

                public static bool ROverkill => Config.LaneClearMenu["rKillable"].Cast<CheckBox>().CurrentValue;

                public static bool RPoison => Config.LaneClearMenu["rPoison"].Cast<CheckBox>().CurrentValue;

                public static int RCharge => Config.LaneClearMenu["rCharge"].Cast<Slider>().CurrentValue;

                public static int RDelay => Config.LaneClearMenu["rDelay"].Cast<Slider>().CurrentValue;

                public static int MinionR => Config.LaneClearMenu["minionR"].Cast<Slider>().CurrentValue;
            }

            public class JungleClear
            {
                public static bool UseQ => Config.JungleClearMenu["useQ"].Cast<CheckBox>().CurrentValue;

                public static bool UseR => Config.JungleClearMenu["useR"].Cast<CheckBox>().CurrentValue;

                public static int ManaQ => Config.JungleClearMenu["manaQ"].Cast<Slider>().CurrentValue;

                public static int ManaR => Config.JungleClearMenu["manaR"].Cast<Slider>().CurrentValue;

                public static bool BigMob => Config.JungleClearMenu["bMob"].Cast<CheckBox>().CurrentValue;

                public static bool ROverkill => Config.JungleClearMenu["rKillable"].Cast<CheckBox>().CurrentValue;

                public static bool RPoison => Config.JungleClearMenu["rPoison"].Cast<CheckBox>().CurrentValue;

                public static int RCharge => Config.JungleClearMenu["rCharge"].Cast<Slider>().CurrentValue;

                public static int RDelay => Config.JungleClearMenu["rDelay"].Cast<Slider>().CurrentValue;

                public static int MobR => Config.JungleClearMenu["mobR"].Cast<Slider>().CurrentValue;
            }

            public class KillSteal
            {
                public static bool UseQ => Config.KillStealMenu["useQ"].Cast<CheckBox>().CurrentValue;

                public static bool UseR => Config.KillStealMenu["useR"].Cast<CheckBox>().CurrentValue;

                public static int ManaQ => Config.KillStealMenu["manaQ"].Cast<Slider>().CurrentValue;

                public static int ManaR => Config.KillStealMenu["manaR"].Cast<Slider>().CurrentValue;

                public static int RDelay => Config.KillStealMenu["rDelay"].Cast<Slider>().CurrentValue;

                public static bool DoubleShroom => Config.KillStealMenu["doubleShroom"].Cast<CheckBox>().CurrentValue;
            }

            public class Flee
            {
                public static bool UseW => Config.FleeMenu["useW"].Cast<CheckBox>().CurrentValue;

                public static bool UseR => Config.FleeMenu["useR"].Cast<CheckBox>().CurrentValue;

                public static int RDelay => Config.FleeMenu["rDelay"].Cast<Slider>().CurrentValue;

                public static int RCharge => Config.FleeMenu["rCharge"].Cast<Slider>().CurrentValue;
            }

            public class Drawing
            {
                public static bool DrawQ => Config.DrawingMenu["drawQ"].Cast<CheckBox>().CurrentValue;

                public static bool DrawR => Config.DrawingMenu["drawR"].Cast<CheckBox>().CurrentValue;

                public static bool DrawAutoR => Config.DrawingMenu["drawautoR"].Cast<CheckBox>().CurrentValue;

                public static bool DrawDoubleShroom => Config.DrawingMenu["drawdoubleR"].Cast<CheckBox>().CurrentValue;
            }

            public class AutoShroom
            {
                public static bool UseR => Config.AutoShroomMenu["useR"].Cast<CheckBox>().CurrentValue;

                public static int ManaR => Config.AutoShroomMenu["manaR"].Cast<Slider>().CurrentValue;

                public static int RCharge => Config.AutoShroomMenu["rCharge"].Cast<Slider>().CurrentValue;

                public static bool EnableShroom => Config.AutoShroomMenu["enableShroom"].Cast<CheckBox>().CurrentValue;

                public static bool DebugMode => Config.AutoShroomMenu["enableDebug"].Cast<CheckBox>().CurrentValue;

                public static bool SaveButton => Config.AutoShroomMenu["saveButton"].Cast<KeyBind>().CurrentValue;

                public static ComboBox PositionMode => Config.AutoShroomMenu["posMode"].Cast<ComboBox>();

                public static bool NewPosition => Config.AutoShroomMenu["newposButton"].Cast<KeyBind>().CurrentValue;

                public static bool DeletePosition => Config.AutoShroomMenu["delposButton"].Cast<KeyBind>().CurrentValue;

                public static bool UseDefaultPosition => Config.AutoShroomMenu["enableDefaultLocations"].Cast<CheckBox>().CurrentValue;
            }

            public class Misc
            {
                public static bool AutoQ => Config.MiscMenu["autoQ"].Cast<CheckBox>().CurrentValue;

                public static bool AutoW => Config.MiscMenu["autoW"].Cast<CheckBox>().CurrentValue;

                public static bool InterruptQ => Config.MiscMenu["intq"].Cast<CheckBox>().CurrentValue;

                public static bool GapcloserR => Config.MiscMenu["gapR"].Cast<CheckBox>().CurrentValue;
            }
        }

        /// <summary>
        /// DamageLibrary Class for Teemo Spells.
        /// </summary>
        public static class DamageLibrary
        {
            /// <summary>
            /// Calculates and returns damage totally done to the target
            /// </summary>
            /// <param name="target">The Target</param>
            /// <param name="useQ">Include Q in Calculations?</param>
            /// <param name="useR">Include R in Calculations?</param>
            /// <returns>The total damage done to target.</returns>
            public static float CalculateDamage(Obj_AI_Base target, bool useQ, bool useR)
            {
                if (target == null)
                {
                    return 0;
                }

                var totaldamage = 0f;

                if (useQ && SpellManager.Q.IsReady())
                {
                    totaldamage = totaldamage + QDamage(target);
                }

                if (useR && SpellManager.R.IsReady())
                {
                    totaldamage = totaldamage + RDamage(target);
                }

                return totaldamage;
            }

            /// <summary>
            /// Calculates the Damage done with Q
            /// </summary>
            /// <param name="target">The Target</param>
            /// <returns>Returns the Damage done with Q</returns>
            private static float QDamage(Obj_AI_Base target)
            {
                return target.CalculateDamageOnUnit(target, DamageType.Magical,
                    new[] {0, 80, 125, 170, 215, 260}[SpellManager.Q.Level] + (Player.Instance.TotalMagicalDamage*0.8f));
            }

            /// <summary>
            /// Calculates the Damage Done with R
            /// </summary>
            /// <param name="target">The Target</param>
            /// <returns>Returns the Damage done with R</returns>
            private static float RDamage(Obj_AI_Base target)
            {
                return target.CalculateDamageOnUnit(target, DamageType.Magical,
                    (float)
                        (new[] {0, 50, 81.25, 112.5}[SpellManager.R.Level] + (Player.Instance.TotalMagicalDamage*0.125f)));
            }
        }
    }
}