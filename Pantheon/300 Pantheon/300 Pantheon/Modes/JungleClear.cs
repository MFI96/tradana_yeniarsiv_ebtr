using System.Linq;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using _300_Pantheon.Base;

namespace _300_Pantheon.Modes
{
    internal class JungleClear
    {
        public static void Execute()
        {
            if (Pantheon.Q.IsReady() && MenuDesigner.ClearUi.Get<CheckBox>("ClearJungleQ").CurrentValue)
            {
                var monster =
                    EntityManager.MinionsAndMonsters.Monsters.Where(
                        m => m.IsValidTarget(Pantheon.Q.Range) && !m.IsDead && m.IsEnemy)
                        .OrderBy(m => m.Health)
                        .Reverse()
                        .FirstOrDefault();

                if (monster == null) return;
                Pantheon.Q.Cast(monster);
            }

            else if (Pantheon.W.IsReady() && MenuDesigner.ClearUi.Get<CheckBox>("ClearJungleW").CurrentValue)
            {
                var monster =
                    EntityManager.MinionsAndMonsters.Monsters.Where(
                        m => m.IsValidTarget(Pantheon.W.Range) && !m.IsDead && m.IsEnemy)
                        .OrderBy(m => m.Health)
                        .Reverse()
                        .FirstOrDefault();

                if (monster == null) return;
                Pantheon.W.Cast(monster);
            }

            else if (Pantheon.E.IsReady() && MenuDesigner.ClearUi.Get<CheckBox>("ClearJungleE").CurrentValue)
            {
                var monster =
                    EntityManager.MinionsAndMonsters.Monsters.Where(
                        m => m.IsValidTarget(Pantheon.E.Range) && !m.IsDead && m.IsEnemy)
                        .OrderBy(m => m.Health)
                        .Reverse()
                        .FirstOrDefault();

                if (monster == null) return;
                Pantheon.E.Cast(monster.ServerPosition);
            }
        }
    }
}