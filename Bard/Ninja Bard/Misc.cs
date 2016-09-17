using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;

namespace Bard
{
    public static class Misc
    {
        private static List<Vector2> _points = new List<Vector2>();

        private static AIHeroClient BardPitt
        {
            get { return ObjectManager.Player; }
        }

        //fluxy
        public static bool WallBangable(this AIHeroClient hero, Vector2 pos = new Vector2())
        {
            if (hero.HasBuffOfType(BuffType.SpellImmunity) || hero.HasBuffOfType(BuffType.SpellShield)) return false;
            var qprediction = SpellManager.Q.GetPrediction(hero);
            var qpredlist = pos.IsValid() ? new List<Vector3> { pos.To3D() } : new List<Vector3>
                        {
                            hero.ServerPosition,
                            hero.Position,
                            qprediction.CastPosition,
                            qprediction.UnitPosition
                        };

            var bangableWalls = 0;
            _points = new List<Vector2>();
            foreach (var position in qpredlist)
            {
                for (var i = 0; i < Config.Modes.Combo.QBindDistance; i += (int)hero.BoundingRadius)
                {
                    var cPos = BardPitt.Position.Extend(position, BardPitt.Distance(position) + i).To3D();
                    _points.Add(cPos.To2D());
                    if (NavMesh.GetCollisionFlags(cPos).HasFlag(CollisionFlags.Wall) || NavMesh.GetCollisionFlags(cPos).HasFlag(CollisionFlags.Building))
                    {
                        bangableWalls++;
                        break;
                    }
                }
            }
            if (bangableWalls / qpredlist.Count >= Config.Modes.Combo.QAccuracyPercent / 100f)
            {
                return true;
            }

            return false;
        }

        

        public static void Initialize()
        {

        }
    }
}
