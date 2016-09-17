using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace Bloodimir_Renekton
{
    internal static class LaneJungleClearA
    {
        public enum AttackSpell
        {
            Q,
            E,
            W,
            Tiamat,
            Hydra
        };

        private static AIHeroClient Renekton
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
            var echeck = Program.LaneJungleClear["LCE"].Cast<CheckBox>().CurrentValue;
            var eready = Program.E.IsReady();
            var qcheck = Program.LaneJungleClear["LCQ"].Cast<CheckBox>().CurrentValue;
            var wcheck = Program.LaneJungleClear["LCW"].Cast<CheckBox>().CurrentValue;
            var qready = Program.Q.IsReady();
            var wready = Program.W.IsReady();

            if (!echeck || !eready) return;
            {
                var aenemy = (Obj_AI_Minion) GetEnemy(Program.E.Range, GameObjectType.obj_AI_Minion);

                if (aenemy != null)
                    Program.E.Cast(aenemy.ServerPosition);
            }
            if (!qcheck || !qready) return;
            {
                var qenemy = (Obj_AI_Minion) GetEnemy(Program.Q.Range, GameObjectType.obj_AI_Minion);

                if (qenemy != null)
                    Program.Q.Cast();
            }
            if (!wcheck || !wready) return;
            {
                var wenemy =
                    (Obj_AI_Minion) GetEnemy(Player.Instance.GetAutoAttackRange(), GameObjectType.obj_AI_Minion);

                if (wenemy != null && Renekton.GetSpellDamage(wenemy, SpellSlot.Q) >= wenemy.Health)
                    Program.W.Cast();
            }
            if (!Orbwalker.CanAutoAttack) return;
            var cenemy = (Obj_AI_Minion) GetEnemy(Renekton.GetAutoAttackRange(), GameObjectType.obj_AI_Minion);

            if (cenemy != null)
                Orbwalker.ForcedTarget = cenemy;
        }

        public static
            void Items()
        {
            var ienemy =
                (Obj_AI_Minion) GetEnemy(Player.Instance.GetAutoAttackRange() + 335, GameObjectType.obj_AI_Minion);

            if (ienemy == null) return;
            if (!ienemy.IsValid || ienemy.IsZombie) return;
            if (Program.LaneJungleClear["LCI"].Cast<CheckBox>().CurrentValue)
            {
                if (Program.Hydra.IsOwned() && Program.Hydra.IsReady() &&
                    Program.Hydra.IsInRange(ienemy))
                    Program.Hydra.Cast();
            }
            if (!Program.LaneJungleClear["LCI"].Cast<CheckBox>().CurrentValue) return;
            if (Program.Tiamat.IsOwned() && Program.Tiamat.IsReady() &&
                Program.Tiamat.IsInRange(ienemy))
                Program.Tiamat.Cast();
        }
    }
}