using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using _300_Pantheon.Assistants;

namespace _300_Pantheon.Modes
{
    public static class Jungle
    {
        public static bool ShouldBeExecuted()
        {
            return Return.Activemode(Orbwalker.ActiveModes.JungleClear);
        }

        public static void Execute()
        {
            var monster = Logic.Monsters(Spells.Q.Range, Player.Instance.ServerPosition).FirstOrDefault();

            if (monster != null && monster.IsValidTarget(Spells.Q.Range))
            {
                if (Return.UseQJungle && Spells.Q.IsReady())
                    Spells.Q.Cast(monster);
                else if (Return.UseWJungle && Spells.W.IsReady())
                    Spells.W.Cast(monster);
                else if (Return.UseEJungle && Spells.E.IsReady())
                    Spells.E.Cast(monster);
            }
        }
    }
}