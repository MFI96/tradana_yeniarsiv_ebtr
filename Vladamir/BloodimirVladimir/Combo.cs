using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace BloodimirVladimir
{
    internal class Combo
    {
        public enum AttackSpell
        {
            Q,
            E
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

        public static void VladCombo()
        {
            var qcheck = Program.ComboMenu["usecomboq"].Cast<CheckBox>().CurrentValue;
            var echeck = Program.ComboMenu["usecomboe"].Cast<CheckBox>().CurrentValue;
            var qready = Program.Q.IsReady();
            var eready = Program.E.IsReady();

            if (!echeck || !eready) return;
            {
                var eenemy = TargetSelector.GetTarget(Program.E.Range, DamageType.Magical);

                if (eenemy != null)
                    Program.E.Cast();
            }

            if (!qcheck || !qready) return;
            {
                var qenemy = (AIHeroClient) GetEnemy(Program.Q.Range, GameObjectType.AIHeroClient);

                if (qenemy != null)
                    Program.Q.Cast(qenemy);
            }
        }
    }
}