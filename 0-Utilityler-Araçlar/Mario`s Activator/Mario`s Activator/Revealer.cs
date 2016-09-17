using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Rendering;
using SharpDX;

namespace Mario_s_Activator
{
    internal class Revealer
    {
        public static void Init()
        {
            Drawing.OnDraw += Drawing_OnDraw;
        }

        private static void Drawing_OnDraw(System.EventArgs args)
        {
            var target = TargetSelector.GetTarget(1000, DamageType.Mixed);
            var closestGrass =
                ObjectManager.Get<GrassObject>()
                    .OrderBy(g => g.Distance(target))
                    .ThenBy(g => g.Distance(Player.Instance))
                    .FirstOrDefault(g => g.IsInRange(Player.Instance, 650));

            Circle.Draw(Color.Purple, 20f, 40f, closestGrass);
        }

        public static void OnTick()
        {
            BushRevealerOnTick();
        }

        private static void BushRevealerOnTick()
        {
            var item = WardsAndTrinkets.WardsAndTrinketsItems.FirstOrDefault(w => w.IsReady() && w.IsOwned());
            var target = TargetSelector.GetTarget(1000, DamageType.Mixed);

            if (item != null && target != null && !target.IsDead && !target.IsHPBarRendered &&
                target.Position.ToNavMeshCell().CollFlags.HasFlag(CollisionFlags.Grass))
            {
                var closestGrass =
                    ObjectManager.Get<GrassObject>()
                        .OrderBy(g => g.Distance(target))
                        .ThenBy(g => g.Distance(Player.Instance))
                        .FirstOrDefault(g => g.IsInRange(Player.Instance, item.Range));

                if (closestGrass != null)
                {
                    item.Cast(closestGrass.Position);
                }
            }
        }
    }
}
