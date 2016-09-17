using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace Black_Swan_Akali.Modes
{
    public static class Clear
    {
        public static bool ShouldBeExecuted()
        {
            return ModeController.OrbLaneClear;
        }

        public static void Execute()
        {
            if (Player.Instance.ManaPercent < Return.ClearEnergyMin) return;

            var Count = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, Spells.E.Range).Count();
            var EMinions = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, Spells.E.Range);
            var QMinions = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, Spells.Q.Range).OrderByDescending(a => a.MaxHealth).FirstOrDefault(a => a.IsValidTarget(Spells.Q.Range) && a.Health < Player.Instance.GetSpellDamage(a, SpellSlot.Q));

            if (Count < Return.ClearMinionMin) return;

            if (Spells.Q.IsReady() && Return.UseQClear)
            {
                if (QMinions != null)
                    Spells.Q.Cast(QMinions);
            }

            if (Spells.E.IsReady() && Return.UseEClear)
            {
                if (EMinions.Any(x => x.IsValidTarget(Spells.E.Range) && x.Health < 0.90 * Player.Instance.GetSpellDamage(x, SpellSlot.E)))
                    Spells.E.Cast();
            }
        }
    }
}
