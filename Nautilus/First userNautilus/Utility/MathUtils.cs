using System;
using System.Collections.Generic;
using System.Linq;
using ClipperLib;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;
using Color = System.Drawing.Color;

namespace Nautilus
{
    public static class MathUtils
    {
        public static Polygon getPolygonOn(Vector2 source, Vector2 target, float w, float l)
        {
            var points = new List<Vector2>();

            var rTpos = target;
            var startP = source;
            var endP = startP.Extend(rTpos, l);

            var p = (rTpos - startP);
            var per = p.Perpendicular().Normalized()*(w/2);
            points.Add(startP + per);
            points.Add(startP - per);
            points.Add(endP - per);
            points.Add(endP + per);

            return new Polygon(points);
        }

        /// <summary>
        ///     Waypoint Tracker data container.
        /// </summary>
        internal static class WaypointTracker
        {
            #region Static Fields

            /// <summary>
            ///     Stored Paths.
            /// </summary>
            public static readonly Dictionary<int, List<Vector2>> StoredPaths = new Dictionary<int, List<Vector2>>();

            /// <summary>
            ///     Stored Ticks.
            /// </summary>
            public static readonly Dictionary<int, int> StoredTick = new Dictionary<int, int>();

            #endregion
        }

        public class Polygon
        {
            public List<Vector2> Points = new List<Vector2>();

            public Polygon()
            {
            }

            public Polygon(List<Vector2> p)
            {
                Points = p;
            }

            public void Add(Vector2 vec)
            {
                Points.Add(vec);
            }

            public int Count()
            {
                return Points.Count;
            }

            public Vector2 GetProjOnPolygon(Vector2 vec)
            {
                var closest = new Vector2(-1000, -1000);
                var start = Points[Count() - 1];
                foreach (var vecPol in Points)
                {
                    var proj = ProjOnLine(start, vecPol, vec);
                    closest = ClosestVec(proj, closest, vec);
                    start = vecPol;
                }
                return closest;
            }

            public Vector2 ClosestVec(Vector2 vec1, Vector2 vec2, Vector2 to)
            {
                var dist1 = Vector2.DistanceSquared(vec1, to); //133
                var dist2 = Vector2.DistanceSquared(vec2, to); //12
                return (dist1 > dist2) ? vec2 : vec1;
            }

            public void Draw(Color color, int width = 1)
            {
                for (var i = 0; i <= Points.Count - 1; i++)
                {
                    if (Points[i].Distance(ObjectManager.Player.Position) < 1500)
                    {
                        var nextIndex = (Points.Count - 1 == i) ? 0 : (i + 1);
                        var from = Drawing.WorldToScreen(Points[i].To3D());
                        var to = Drawing.WorldToScreen(Points[nextIndex].To3D());
                        Drawing.DrawLine(from[0], from[1], to[0], to[1], width, color);
                    }
                }
            }

            private Vector2 ProjOnLine(Vector2 v, Vector2 w, Vector2 p)
            {
                var nullVec = new Vector2(-1, -1);
                // Return minimum distance between line segment vw and point p
                var l2 = Vector2.DistanceSquared(v, w); // i.e. |w-v|^2 -  avoid a sqrt
                if (l2 == 0.0)
                    return nullVec; // v == w case
                // Consider the line extending the segment, parameterized as v + t (w - v).
                // We find projection of point p onto the line. 
                // It falls where t = [(p-v) . (w-v)] / |w-v|^2
                var t = Vector2.Dot(p - v, w - v)/l2;
                if (t < 0.0)
                    return nullVec; // Beyond the 'v' end of the segment
                if (t > 1.0)
                    return nullVec; // Beyond the 'w' end of the segment
                var projection = v + t*(w - v); // Projection falls on the segment
                return projection;
            }

            public bool PointInside(Vector2 testPoint)
            {
                var result = false;
                var j = Count() - 1;
                for (var i = 0; i < Count(); i++)
                {
                    if (Points[i].Y < testPoint.Y && Points[j].Y >= testPoint.Y ||
                        Points[j].Y < testPoint.Y && Points[i].Y >= testPoint.Y)
                    {
                        if (Points[i].X +
                            (testPoint.Y - Points[i].Y)/(Points[j].Y - Points[i].Y)*(Points[j].X - Points[i].X) <
                            testPoint.X)
                        {
                            result = !result;
                        }
                    }
                    j = i;
                }
                return result;
            }
        }

        #region Public Methods and Operators

        /// <summary>
        ///     Creates a double list of the list of <see cref="Geometry.Polygon" /> broken up into <see cref="IntPoint" />'s.
        /// </summary>
        /// <param name="polygons">List of <see cref="Geometry.Polygon" />s</param>
        /// <returns>Double list of <see cref="IntPoint" />, each list clipping a polygon that was broken up.</returns>
        public static List<List<IntPoint>> ClipPolygons(List<Geometry.Polygon> polygons)
        {
            var subj = new List<List<IntPoint>>(polygons.Count);
            var clip = new List<List<IntPoint>>(polygons.Count);

            foreach (var polygon in polygons)
            {
                subj.Add(polygon.ToClipperPath());
                clip.Add(polygon.ToClipperPath());
            }

            var solution = new List<List<IntPoint>>();
            var c = new Clipper();
            c.AddPaths(subj, PolyType.ptSubject, true);
            c.AddPaths(clip, PolyType.ptClip, true);
            c.Execute(ClipType.ctUnion, solution, PolyFillType.pftPositive, PolyFillType.pftEvenOdd);

            return solution;
        }

        /// <summary>
        ///     Removes vectors past a distance from a list.
        /// </summary>
        /// <param name="path">The Path</param>
        /// <param name="distance">The Distance</param>
        /// <returns>The paths in range</returns>
        public static List<Vector2> CutPath(this List<Vector2> path, float distance)
        {
            var result = new List<Vector2>();
            for (var i = 0; i < path.Count - 1; i++)
            {
                var dist = path[i].Distance(path[i + 1]);
                if (dist > distance)
                {
                    result.Add(path[i] + (distance*(path[i + 1] - path[i]).Normalized()));

                    for (var j = i + 1; j < path.Count; j++)
                    {
                        result.Add(path[j]);
                    }

                    break;
                }

                distance -= dist;
            }

            return result.Count > 0 ? result : new List<Vector2> {path.Last()};
        }

        /// <summary>
        ///     Returns the path of the unit appending the ServerPosition at the start, works even if the unit just entered fog of
        ///     war.
        /// </summary>
        /// <param name="unit">Unit to get the waypoints for</param>
        /// <returns>List of waypoints(<see cref="Vector2" />)</returns>
        public static List<Vector2> GetWaypoints(this Obj_AI_Base unit)
        {
            var result = new List<Vector2>();

            if (unit.IsVisible)
            {
                result.Add(unit.ServerPosition.To2D());
                result.AddRange(unit.Path.Select(point => point.To2D()));
            }
            else
            {
                List<Vector2> value;
                if (WaypointTracker.StoredPaths.TryGetValue(unit.NetworkId, out value))
                {
                    var path = value;
                    var timePassed = ((Game.Time*1000) - WaypointTracker.StoredTick[unit.NetworkId])/1000f;
                    if (path.GetPathLength() >= unit.MoveSpeed*timePassed)
                    {
                        result = CutPath(path, (int) (unit.MoveSpeed*timePassed));
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///     Gets the position after a set time, speed, and delay.
        /// </summary>
        /// <param name="self">List of <see cref="Vector2" />'s.</param>
        /// <param name="time">The Time</param>
        /// <param name="speed">The Speed</param>
        /// <param name="delay">The Delay</param>
        /// <returns>The position after calculations</returns>
        public static Vector2 PositionAfter(this List<Vector2> self, int time, int speed, int delay = 0)
        {
            var distance = Math.Max(0, time - delay)*speed/1000;
            for (var i = 0; i <= self.Count - 2; i++)
            {
                var from = self[i];
                var to = self[i + 1];
                var d = (int) to.Distance(from);
                if (d > distance)
                {
                    return from + (distance*(to - from).Normalized());
                }

                distance -= d;
            }

            return self[self.Count - 1];
        }

        /// <summary>
        ///     Converts a list of <see cref="IntPoint" />s to a <see cref="Geometry.Polygon" />
        /// </summary>
        /// <param name="list">List of <see cref="Geometry.Polygon" /></param>
        /// <returns>Polygon made up of <see cref="IntPoint" />s</returns>
        public static Geometry.Polygon ToPolygon(this List<IntPoint> list)
        {
            var polygon = new Geometry.Polygon();
            foreach (var point in list)
            {
                polygon.Add(new Vector2(point.X, point.Y));
            }

            return polygon;
        }

        /// <summary>
        ///     Converts a list of lists of <see cref="IntPoint" /> to a polygon.
        /// </summary>
        /// <param name="v">List of <see cref="IntPoint" />.</param>
        /// <returns>List of polygons.</returns>
        public static List<Geometry.Polygon> ToPolygons(this List<List<IntPoint>> v)
        {
            return v.Select(path => path.ToPolygon()).ToList();
        }

        #endregion
    }
}