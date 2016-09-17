using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace Bloodimir_Ziggs_v2
{
    internal static class LastHitA
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
                        a => a.Distance(Player.Instance) < range && !a.IsDead && !a.IsInvulnerable &&
                             a.Health <= Calculations.Qcalc(a));
                default:
                    return EntityManager.MinionsAndMonsters.EnemyMinions.OrderBy(a => a.Health).FirstOrDefault(
                        a => a.Distance(Player.Instance) < range && !a.IsDead && !a.IsInvulnerable &&
                             a.Health <= Calculations.Qcalc(a));
            }
        }

        public static void LastHitB()
        {
            var qcheck = Program.LastHitMenu["LHQ"].Cast<CheckBox>().CurrentValue;
            var qready = Program.Q.IsReady();
            if (!qcheck || !qready) return;
            var qenemy = (Obj_AI_Minion) GetEnemy(Program.Q.Range, GameObjectType.obj_AI_Minion);
            if (qenemy == null) return;
            {
                var predQ = Program.Q.GetPrediction(qenemy).CastPosition;
                Program.Q.Cast(predQ);
            }
        }
    }
}
