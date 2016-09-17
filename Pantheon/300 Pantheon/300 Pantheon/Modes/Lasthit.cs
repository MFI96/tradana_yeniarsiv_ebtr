using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using _300_Pantheon.Base;

namespace _300_Pantheon.Modes
{
    internal class LastHit
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

                if (minion.Health <= Player.Instance.GetSpellDamage(minion, SpellSlot.Q))
                {
                    Pantheon.Q.Cast(minion);
                }
            }
        }
    }
}