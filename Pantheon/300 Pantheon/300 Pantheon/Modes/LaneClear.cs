using System.Linq;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using _300_Pantheon.Base;

namespace _300_Pantheon.Modes
{
    internal class LaneClear
    {
        public static void Execute()
        {
            if (Pantheon.Q.IsReady() && MenuDesigner.ClearUi.Get<CheckBox>("ClearLaneQ").CurrentValue)
            {
                var minion =
                    EntityManager.MinionsAndMonsters.Minions.Where(
                        m => m.IsValidTarget(Pantheon.Q.Range) && !m.IsDead && m.IsEnemy)
                        .OrderBy(m => m.Health)
                        .Reverse()
                        .FirstOrDefault();

                if (minion == null) return;
                Pantheon.Q.Cast(minion);
            }

            else if (Pantheon.W.IsReady() && MenuDesigner.ClearUi.Get<CheckBox>("ClearLaneW").CurrentValue)
            {
                var minion =
                    EntityManager.MinionsAndMonsters.Minions.Where(
                        m => m.IsValidTarget(Pantheon.W.Range) && !m.IsDead && m.IsEnemy)
                        .OrderBy(m => m.Health)
                        .Reverse()
                        .FirstOrDefault();

                if (minion == null) return;
                Pantheon.W.Cast(minion);
            }

            else if (Pantheon.E.IsReady() && MenuDesigner.ClearUi.Get<CheckBox>("ClearLaneE").CurrentValue)
            {
                var minion =
                    EntityManager.MinionsAndMonsters.Minions.Where(
                        m => m.IsValidTarget(Pantheon.E.Range) && !m.IsDead && m.IsEnemy)
                        .OrderBy(m => m.Health)
                        .Reverse()
                        .FirstOrDefault();

                if (minion == null) return;
                Pantheon.E.Cast(minion.ServerPosition);
            }
        }
    }
}