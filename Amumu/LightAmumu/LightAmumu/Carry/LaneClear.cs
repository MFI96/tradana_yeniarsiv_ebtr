using EloBuddy;
using EloBuddy.SDK;
using System.Linq;

namespace LightAmumu.Carry
{
    public sealed class LaneClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear);
        }

        public override void Execute()
        {
            int minions = EntityManager.MinionsAndMonsters.EnemyMinions.Where(minion => minion.IsValidTarget(W.Range)).Count();
            if (minions != 0)
            {
                if (MenuList.Farm.WithW)
                    Damage.WEnable();
                if (MenuList.Farm.WithE)
                {
                    var minion = EntityManager.MinionsAndMonsters.EnemyMinions;
                    foreach (var select in minion)
                    {
                        if (select.IsValidTarget(E.Range))
                            E.Cast();
                    }
                }
            }
        }
    }
}