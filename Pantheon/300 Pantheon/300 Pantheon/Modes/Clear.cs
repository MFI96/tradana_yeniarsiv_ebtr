using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using _300_Pantheon.Assistants;

namespace _300_Pantheon.Modes
{
    public static class Clear
    {
        public static bool ShouldBeExecuted()
        {
            return Return.Activemode(Orbwalker.ActiveModes.LaneClear);
        }

        public static void Execute()
        {
            if (Player.Instance.ManaPercent < Return.ClearManaMin) return;

            var minion = Logic.Minions(EntityManager.UnitTeam.Enemy, Spells.Q.Range, Player.Instance.ServerPosition)
                .FirstOrDefault();

            if (minion != null && minion.IsValidTarget(Spells.Q.Range))
            {
                if (Return.UseQClear && Spells.Q.IsReady())
                    Spells.Q.Cast(minion);
                else if (Return.UseEClear && Spells.E.IsReady())
                    Spells.E.Cast(minion);
                else if (Return.UseWClear && Spells.W.IsReady())
                    Spells.W.Cast(minion);
            }
        }
    }
}