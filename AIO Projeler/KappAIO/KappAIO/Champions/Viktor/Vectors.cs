using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Rendering;
using KappAIO.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace KappAIO.Champions.Viktor
{
    internal static class Vectors
    {
        public static void ECast(this AIHeroClient target, int HitCount = 1, bool Draw = false)
        {
            if (!Viktor.E.IsReady()) return;

            var rectlist = new List<Geometry.Polygon.Rectangle>();
            rectlist.Clear();
            var pred = Viktor.E.GetPrediction(target);
            
            if(pred.HitChance < HitChance.Low) return;

            Vector3 Start = pred.CastPosition.Distance(Player.Instance) > 525 ? Player.Instance.ServerPosition.Extend(pred.CastPosition, 525).To3D() : target.ServerPosition;
            Vector3 End = pred.CastPosition;

            foreach (var A in EntityManager.Heroes.Enemies.OrderBy(o => o.Health).Where(e => e.IsKillable(Viktor.E.Range) && e.NetworkId != target.NetworkId))
            {
                var predmobB = Viktor.E.GetPrediction(A);
                End = Start.Extend(predmobB.CastPosition, 600).To3D();
                rectlist.Add(new Geometry.Polygon.Rectangle(Start, End, Viktor.E.Width));
            }

            var bestpos = rectlist.OrderByDescending(r => EntityManager.Heroes.Enemies.OrderBy(o => o.Health).Count(m => r.IsInside(m) && m.IsKillable(Viktor.E.Range))).FirstOrDefault();

            if (bestpos != null)
            {
                Start = bestpos.Start.To3D();
                End = bestpos.End.To3D();

                if (!Draw)
                {
                    if (HitCount > 1)
                    {
                        if (EntityManager.Heroes.Enemies.OrderBy(o => o.Health).Count(m => bestpos.IsInside(m) && m.IsKillable(Viktor.E.Range)) >= HitCount)
                        {
                            Viktor.E.CastStartToEnd(End, Start);
                        }
                    }
                    else
                    {
                        Viktor.E.CastStartToEnd(End, Start);
                    }
                }
            }
            else
            {
                if (!Draw)
                    Viktor.E.CastStartToEnd(End, Start);
            }
            if (Draw)
            {
                bestpos?.Draw(Color.AliceBlue);
                Circle.Draw(SharpDX.Color.AliceBlue, 100, Start);
                Circle.Draw(SharpDX.Color.Red, 100, End);
            }
        }

        
        public static void ECast(bool Jungle = false, int HitCount = 1, bool Draw = false)
        {
            if(!Viktor.E.IsReady()) return;

            var rectlist = new List<Geometry.Polygon.Rectangle>();
            rectlist.Clear();
            Vector3 Start;
            Vector3 End;
            var mobs = EntityManager.MinionsAndMonsters.EnemyMinions.OrderBy(o => o.Health).Where(e => e.IsKillable(Viktor.E.Range));

            if (Jungle)
            {
                mobs = EntityManager.MinionsAndMonsters.GetJungleMonsters().OrderBy(o => o.MaxHealth).Where(e => e.IsKillable(Viktor.E.Range));
            }

            foreach (var A in mobs)
            {
                var predmob = Viktor.E.GetPrediction(A);
                Start = predmob.CastPosition.Distance(Player.Instance) > 525 ? Player.Instance.ServerPosition.Extend(predmob.CastPosition, 525).To3D() : A.ServerPosition;
                var mobs2 = EntityManager.MinionsAndMonsters.EnemyMinions.OrderBy(o => o.Health).Where(e => e.IsKillable(Viktor.E.Range) && e.NetworkId != A.NetworkId && e.IsInRange(A, 600));
                if (Jungle)
                {
                    mobs2 = EntityManager.MinionsAndMonsters.GetJungleMonsters().OrderBy(o => o.MaxHealth).Where(e => e.IsKillable(Viktor.E.Range) && e.NetworkId != A.NetworkId && e.IsInRange(A, 600));
                }
                foreach (var B in mobs2)
                {
                    var predmobB = Viktor.E.GetPrediction(B);
                    End = Start.Extend(predmobB.CastPosition, 600).To3D();
                    rectlist.Add(new Geometry.Polygon.Rectangle(Start, End, Viktor.E.Width));
                }
            }

            var bestpos = rectlist.OrderByDescending(r => EntityManager.MinionsAndMonsters.EnemyMinions.OrderBy(o => o.Health).Count(m => r.IsInside(m) && m.IsKillable(Viktor.E.Range))).FirstOrDefault();
            if (Jungle)
            {
                bestpos = rectlist.OrderByDescending(r => EntityManager.MinionsAndMonsters.GetJungleMonsters().OrderBy(o => o.MaxHealth).Count(m => r.IsInside(m) && m.IsKillable(Viktor.E.Range))).FirstOrDefault();
            }

            if (bestpos != null)
            {
                var mobs3 = EntityManager.MinionsAndMonsters.EnemyMinions.OrderBy(o => o.Health).Count(m => bestpos.IsInside(m) && m.IsKillable(Viktor.E.Range));
                if (Jungle)
                {
                    mobs3 = EntityManager.MinionsAndMonsters.GetJungleMonsters().OrderBy(o => o.MaxHealth).Count(m => bestpos.IsInside(m) && m.IsKillable(Viktor.E.Range));
                }
                if (mobs3 >= HitCount)
                {
                    Start = bestpos.Start.To3D();
                    End = bestpos.End.To3D();
                    if (Draw)
                    {
                        bestpos.Draw(Color.AliceBlue);
                        Circle.Draw(SharpDX.Color.AliceBlue, 100, Start);
                        Circle.Draw(SharpDX.Color.Red, 100, End);
                    }
                    else
                    {
                        Viktor.E.CastStartToEnd(End, Start);
                    }
                }
            }
        }
    }
}
