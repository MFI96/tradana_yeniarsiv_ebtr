using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KaPoppy
{
    class Lib
    {
        public static Spell.Skillshot Q;
        public static Spell.Active W;
        public static Spell.Targeted E;
        public static Spell.Chargeable R;
        public static Spell.Targeted Flash;
        public static GameObject Passive = null;
        static Lib()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 430, SkillShotType.Linear, 250, null, 100);
            Q.AllowedCollisionCount = int.MaxValue;
            W = new Spell.Active(SpellSlot.W, 400);
            E = new Spell.Targeted(SpellSlot.E, 525);
            R = new Spell.Chargeable(SpellSlot.R, 500, 1200, 4000, 250, int.MaxValue, 90);
            Flash = null;
        }
        public static List<Vector2> PointsAroundTheTarget(AttackableUnit target)
        {
            if (target == null)
            {
                return new List<Vector2>();
            }
            List<Vector2> list = new List<Vector2>();
            foreach (var point in new Geometry.Polygon.Circle(target.Position, 400).Points.Where(x => !x.IsWall()))
            {
                list.Add(point);
            }
            foreach (var point in new Geometry.Polygon.Circle(target.Position, 325).Points.Where(x => !x.IsWall()))
            {
                list.Add(point);
            }
            foreach (var point in new Geometry.Polygon.Circle(target.Position, 225).Points.Where(x => !x.IsWall()))
            {
                list.Add(point);
            }



            return list;
        }

        public static bool CanStun(AIHeroClient unit, bool ECheck = false)
        {
            if (unit.HasBuffOfType(BuffType.SpellImmunity) || unit.HasBuffOfType(BuffType.SpellShield) || Player.Instance.IsDashing()) return false;
            if (ECheck && !E.IsReady()) return false;

            var prediction = Prediction.Position.PredictUnitPosition(unit, 400);
            var predictionsList = new List<Vector3>
                        {
                            unit.ServerPosition,
                            unit.Position,
                            prediction.To3D(),
                        };

            var wallsFound = 0;
            foreach (var position in predictionsList)
            {
                for (var i = 0; i < 300; i += (int) unit.BoundingRadius)
                {
                    var cPos = Player.Instance.Position.Extend(position, Player.Instance.Distance(position) + i).To3D();
                    if (cPos.IsWall())
                    {
                        wallsFound++;
                        break;
                    }
                }
            }
            if ((wallsFound / predictionsList.Count) >= Settings.MiscSettings.StunPercent / 100f)
            {
                return true;
            }

            return false;
        }
        public static bool CanStun(AIHeroClient unit, Vector2 pos)
        {
            if (unit.HasBuffOfType(BuffType.SpellImmunity) || unit.HasBuffOfType(BuffType.SpellShield) || Player.Instance.IsDashing()) return false;
            var prediction = Prediction.Position.PredictUnitPosition(unit, 400);
            var predictionsList = new List<Vector3>
                        {
                            unit.ServerPosition,
                            unit.Position,
                            prediction.To3D(),
                        };

            var wallsFound = 0;
            foreach (var position in predictionsList)
            {
                for (var i = 0; i < 300; i += (int) unit.BoundingRadius)
                {
                    var cPos = pos.Extend(position, pos.Distance(position) + i).To3D();
                    if (cPos.IsWall())
                    {
                        wallsFound++;
                        break;
                    }
                }
            }
            if ((wallsFound / predictionsList.Count) >= Settings.MiscSettings.StunPercent / 100f)
            {
                return true;
            }

            return false;
        }
        public static float GetPassiveDamage(Obj_AI_Base unit)
        {
            return Player.Instance.CalculateDamageOnUnit(unit, DamageType.Magical, 9 * Player.Instance.Level + 20);
        }
    }
}