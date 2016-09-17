using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace BloodimirVladimir
{
    internal class LaneClearA
    {
        public enum AttackSpell
        {
            E,
            Q
        };

        public static AIHeroClient Vladimir
        {
            get { return ObjectManager.Player; }
        }

        public static Obj_AI_Base GetEnemy(float range, GameObjectType t)
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
            var echeck = Program.LaneClear["LCE"].Cast<CheckBox>().CurrentValue;
            var eready = Program.E.IsReady();
            var qcheck = Program.LaneClear["LCQ"].Cast<CheckBox>().CurrentValue;
            var qready = Program.Q.IsReady();
            if (!qcheck || !qready) return;
            {
                var enemy = (Obj_AI_Minion) GetEnemy(Program.Q.Range, GameObjectType.obj_AI_Minion);

                if (enemy != null)
                    Program.Q.Cast(enemy);
            }

            if (!echeck || !eready) return;
            {
                var enemy = (Obj_AI_Minion) GetEnemy(Program.E.Range, GameObjectType.obj_AI_Minion);

                if (enemy != null)
                    Program.E.Cast();
            }
        }
    }
}