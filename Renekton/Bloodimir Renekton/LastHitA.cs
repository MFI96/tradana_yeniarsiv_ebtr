using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace Bloodimir_Renekton
{
    internal static class LastHitA
    {
        private static AIHeroClient Renekton
        {
            get { return ObjectManager.Player; }
        }

        private static float Qcalc(Obj_AI_Base target)
        {
            return Renekton.CalculateDamageOnUnit(target, DamageType.Physical,
                (new float[] {0, 60, 90, 120, 150, 180}[Program.Q.Level] +
                 (0.80f*Renekton.FlatPhysicalDamageMod)));
        }

        private static Obj_AI_Base MinionLh(GameObjectType type, AttackSpell spell)
        {
            return ObjectManager.Get<Obj_AI_Base>().OrderBy(a => a.Health).FirstOrDefault(a => a.IsEnemy
                                                                                               && a.Type == type
                                                                                               &&
                                                                                               a.Distance(Renekton) <=
                                                                                               Program.Q.Range
                                                                                               && !a.IsDead
                                                                                               && !a.IsInvulnerable
                                                                                               &&
                                                                                               a.IsValidTarget(
                                                                                                   Program.Q.Range)
                                                                                               &&
                                                                                               a.Health <= Qcalc(a));
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

        public static void LastHitB()
        {
            var qcheck = Program.LastHit["LHQ"].Cast<CheckBox>().CurrentValue;
            var wcheck = Program.LastHit["LHW"].Cast<CheckBox>().CurrentValue;
            var qready = Program.Q.IsReady();
            var wready = Program.W.IsReady();

            if (!qcheck || !qready) return;
            var minion = (Obj_AI_Minion) MinionLh(GameObjectType.obj_AI_Minion, AttackSpell.Q);
            if (minion != null)
            {
                Program.Q.Cast();
            }
            if (!wcheck || !wready) return;
            var wminion = (Obj_AI_Minion) GetEnemy(Player.Instance.GetAutoAttackRange(), GameObjectType.obj_AI_Minion);
            if (wminion != null && Renekton.GetSpellDamage(wminion, SpellSlot.W) >= wminion.Health)
            {
                Program.W.Cast();
            }
        }

        public static
            void Items()
        {
            var ienemy =
                (Obj_AI_Minion) GetEnemy(Player.Instance.GetAutoAttackRange() + 335, GameObjectType.obj_AI_Minion);


            if (ienemy == null) return;
            if (!ienemy.IsValid || ienemy.IsZombie) return;
            if (Program.LastHit["LHI"].Cast<CheckBox>().CurrentValue)
            {
                if (Program.Hydra.IsOwned() && Program.Hydra.IsReady() &&
                    Program.Hydra.IsInRange(ienemy))
                    Program.Hydra.Cast();
            }
            if (!Program.LastHit["LHI"].Cast<CheckBox>().CurrentValue) return;
            if (Program.Tiamat.IsOwned() && Program.Tiamat.IsReady() &&
                Program.Tiamat.IsInRange(ienemy))
                Program.Tiamat.Cast();
        }

        private enum AttackSpell
        {
            Q
        };
    }
}