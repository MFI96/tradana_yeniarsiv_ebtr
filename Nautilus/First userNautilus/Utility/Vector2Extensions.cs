using System;
using System.Collections.Generic;
using EloBuddy.SDK;
using SharpDX;

namespace Nautilus
{
    public static class Vector2Extensions
    {
        public static float GetPathLength(this List<Vector2> path)
        {
            var distance = 0f;

            for (var i = 0; i < path.Count - 1; i++)
            {
                distance += path[i].Distance(path[i + 1]);
            }

            return distance;
        }

        public static Vector2[] CircleCircleIntersection(
            this Vector2 center1,
            Vector2 center2,
            float radius1,
            float radius2)
        {
            var d = center1.Distance(center2);

            if (d > radius1 + radius2 || (d <= Math.Abs(radius1 - radius2)))
            {
                return new Vector2[] {};
            }

            var a = ((radius1*radius1) - (radius2*radius2) + (d*d))/(2*d);
            var h = (float) Math.Sqrt((radius1*radius1) - (a*a));
            var direction = (center2 - center1).Normalized();
            var pa = center1 + (a*direction);
            var s1 = pa + (h*direction.Perpendicular());
            var s2 = pa - (h*direction.Perpendicular());
            return new[] {s1, s2};
        }
    }
}