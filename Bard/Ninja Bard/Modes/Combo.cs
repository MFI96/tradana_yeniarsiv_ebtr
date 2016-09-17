using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using Settings = Bard.Config.Modes.Combo;

namespace Bard.Modes
{
    public sealed class Combo : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {
            #region Q Logic

            if (!Settings.UseQ || !Q.IsReady()) return;
            {
                var qext = TargetSelector.GetTarget(Q2.Range, DamageType.Magical);
                if (qext == null)
                {
                    return;
                }
                var predQext = Q2.GetPrediction(qext);

                if (qext.IsValid &&
                    predQext.CollisionObjects.Count(
                        a => a.IsValidTarget(Q.Range) && a.Distance(qext) <= Settings.QBindDistanceM) == 1)
                {
                    Q.Cast(predQext.CastPosition);
                }

                var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
                if (target == null)
                {
                    return;
                }
                var predictionQ = Q.GetPrediction(target);

                if (target.IsValid && target.WallBangable() && !predictionQ.CollisionObjects.Any())
                {
                    Q.Cast(predictionQ.CastPosition);
                }
            }

            #endregion
        }
    }
}