using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace Black_Swan_Akali.Modes
{
    public static class Lasthit
    {
        public static bool ShouldBeExecuted()
        {
            return ModeController.OrbLastHit;
        }

        public static void Execute()
        {
            var QMinions = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, Spells.Q.Range).OrderByDescending(a => a.MaxHealth).FirstOrDefault(a => a.IsValidTarget(Spells.Q.Range) && a.Health < Player.Instance.GetSpellDamage(a, SpellSlot.Q));
            var EMinions = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, Spells.E.Range).OrderByDescending(a => a.MaxHealth).FirstOrDefault(a => a.IsValidTarget(Spells.E.Range) && a.Health < Player.Instance.GetSpellDamage(a, SpellSlot.E));

            if (Spells.Q.IsReady() && Return.UseQLast)
            {
                if (QMinions != null)
                    Spells.Q.Cast(QMinions);
            }

            if (Spells.E.IsReady() && Return.UseELast)
            {
                if (EMinions != null)
                    Spells.E.Cast();
            }
        }
    }
}
