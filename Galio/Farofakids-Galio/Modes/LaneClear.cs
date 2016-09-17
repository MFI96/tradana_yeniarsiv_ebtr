using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using Settings = Farofakids_Galio.Config.Modes.LaneClear;

namespace Farofakids_Galio.Modes
{
    public sealed class LaneClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear);
        }

        public override void Execute()
        {
            if (Player.Instance.ManaPercent < Settings.Mana)
            {
                return;
            }
            var minion = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.Position, Q.Range, false);
            if (minion == null)
            {
                return;
            }
            if (Settings.UseQE)
            { 
            var predictResult =
            Prediction.Position.PredictCircularMissileAoe(minion.Cast<Obj_AI_Base>().ToArray(), Q.Range, Q.Radius, Q.CastDelay, Q.Speed)
            .OrderByDescending(r => r.GetCollisionObjects<Obj_AI_Minion>().Length).FirstOrDefault();
            var predictResultE =
            Prediction.Position.PredictCircularMissileAoe(minion.Cast<Obj_AI_Base>().ToArray(), E.Range, E.Radius, E.CastDelay, E.Speed)
            .OrderByDescending(r => r.GetCollisionObjects<Obj_AI_Minion>().Length).FirstOrDefault();
            if (predictResult != null && predictResult.CollisionObjects.Length >= Settings.MinNumberQE)
            {
                Q.Cast(predictResult.CastPosition);
                E.Cast(predictResultE.CastPosition);
                return;
            }
            }

        }
    }
}
