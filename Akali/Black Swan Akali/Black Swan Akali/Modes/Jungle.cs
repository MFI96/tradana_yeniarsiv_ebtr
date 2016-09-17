using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace Black_Swan_Akali.Modes
{
    public static class Jungle
    {
        public static bool ShouldBeExecuted()
        {
            return ModeController.OrbJungleClear;
        }

        public static void Execute()
        {
            if (Player.Instance.ManaPercent < Return.JungleEnergyMin) return;

            var Creeps = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.ServerPosition, Spells.Q.Range).OrderByDescending(a => a.MaxHealth).FirstOrDefault();

            if (Spells.Q.IsReady() && Return.UseQJungle)
            {
                if (Creeps.IsValidTarget(Spells.Q.Range))
                    Spells.Q.Cast(Creeps);
            }

            if (Spells.E.IsReady() && Return.UseEJungle)
            {
                if (Creeps.IsValidTarget(Spells.E.Range))
                    Spells.E.Cast();
            }
        }
    }

}