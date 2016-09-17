using EloBuddy;
using EloBuddy.SDK;
using KaPoppy;
using System.Linq;

namespace Modes
{
    internal class Stun : Helper
    {
        internal static void Execute()
        {
            var target = TargetSelector.SelectedTarget;
            if (target == null)
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                return;
            }


            if (!Lib.E.IsReady())
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                return;
            }
            var pos = Lib.PointsAroundTheTarget(target).Where(x => Lib.CanStun(target, x)).OrderBy(x => x.Distance(myHero)).FirstOrDefault();
            if (!pos.IsValid(true))
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                return;
            }

            Player.IssueOrder(GameObjectOrder.MoveTo, pos.To3D());
            if (Lib.Flash != null && Settings.ComboSettings.UseFlashE && !Lib.CanStun(target))
            {
                if (Lib.Flash.IsReady() && Lib.Flash.IsInRange(pos.To3D()) && myHero.Distance(pos) >= Settings.MiscSettings.MinimumDistanceToFlash)
                {
                    Lib.Flash.Cast(pos.To3D());
                }
            }
        }
    }
}