using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace Bloodimir_Ziggs_v2
{
    internal static class LaneJungleClearA
    {
        public static AIHeroClient Ziggs
        {
            get { return ObjectManager.Player; }
        }

        private static Obj_AI_Base GetEnemy(float range, GameObjectType t)
        {
            switch (t)
            {
                case GameObjectType.AIHeroClient:
                    return EntityManager.Heroes.Enemies.OrderBy(a => a.Health).FirstOrDefault(
                        a => a.Distance(Player.Instance) < range && !a.IsDead && !a.IsInvulnerable);
                default:
                    return EntityManager.MinionsAndMonsters.EnemyMinions.OrderBy(a => a.Health).FirstOrDefault(
                        a => a.Distance(Player.Instance) < range && !a.IsDead && !a.IsInvulnerable);
            }
        }

        public static void LaneClear()
        {
            var qcheck = Program.LaneJungleClear["LCQ"].Cast<CheckBox>().CurrentValue;
            var qready = Program.Q.IsReady();
            var echeck = Program.LaneJungleClear["LCE"].Cast<CheckBox>().CurrentValue;
            var eready = Program.E.IsReady();

            if (!qcheck || !qready) return;
            var qenemy = (Obj_AI_Minion) GetEnemy(Program.Q.Range, GameObjectType.obj_AI_Minion);

            if (qenemy != null)
            {
                var predQ = Program.Q.GetPrediction(qenemy).CastPosition;
                Program.Q.Cast(predQ);
            }
            if (!echeck || !eready) return;
            var eminion = (Obj_AI_Minion) GetEnemy(Program.E.Range, GameObjectType.obj_AI_Minion);
            if (eminion == null) return;
            var predE = Program.E.GetPrediction(eminion).CastPosition;
            Program.E.Cast(predE);
        }
    }
}
