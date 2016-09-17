using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace AddonTemplate.Modes
{
    public sealed class PermaActive : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return true;
        }

        public override void Execute()
        {

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
                return;
            if (!Player.Instance.IsRecalling())
            {
                EBigMinion();
                EBaronDragon();
                KsE();
                EFullStacks();
                EOutOfRange();
                EDying();
            }
        }

        private void EBigMinion()
        {
            if (Config.Modes.Clear.EBigMinion)
            {
                if (EntityManager.MinionsAndMonsters.EnemyMinions.Any(c => Player.Instance.Position.IsInRange(c.Position, SpellManager.E.Range)
                && (c.BaseSkinName.Contains("Siege") || c.BaseSkinName.Contains("Super"))
                && c.IsValid && c.Health < DamageHelper.GetEDamage(c)))
                {
                    E.Cast();
                }
            }
        }

        private void EBaronDragon()
        {
            if (Config.Modes.Clear.EBaronDragon)
            {
                if (EntityManager.MinionsAndMonsters.Monsters.Any(c => Player.Instance.Position.IsInRange(c.Position, SpellManager.E.Range)
                && (c.BaseSkinName.Contains("Dragon") || c.BaseSkinName.Contains("Baron") || c.BaseSkinName.Contains("Herald"))
                && c.Health < DamageHelper.GetEDamage(c)))
                {
                    E.Cast();
                }
            }
        }
        private void KsE()
        {
            if (Config.Modes.KillSteal.KsE)
            {
                if (EntityManager.Heroes.Enemies.Any(c => Player.Instance.Position.IsInRange(c.Position, SpellManager.E.Range)
                 && c.IsValidTarget() && c.Health < DamageHelper.GetEDamage(c)))
                {
                    E.Cast();
                }
            }
        }
        private void EFullStacks()
        {
            if (Config.Modes.KillSteal.EFullStacks)
            {
                if (EntityManager.Heroes.Enemies.Any(c => Player.Instance.Position.IsInRange(c.Position, SpellManager.E.Range) &&
                 c.IsValidTarget() && DamageHelper.getEStacks(c) >= Config.Modes.KillSteal.ENumberstacks))
                {
                    E.Cast();
                }
            }
        }
        private void EOutOfRange()
        {
            if (Config.Modes.KillSteal.EOutOfRange)
            {
                foreach (var enemy in EntityManager.Heroes.Enemies.Where(c => Player.Instance.Position.IsInRange(c.Position, SpellManager.E.Range) &&
                 c.IsValidTarget() && DamageHelper.getEStacks(c) >= Config.Modes.KillSteal.EOutOfRangeStacks))
                {
                    if (Player.Instance.Distance(enemy.Position) > SpellManager.E.Range - 150)
                        E.Cast();
                }
            }
        }
        private void EDying()
        {
            if (Config.Modes.KillSteal.EDying)
            {
                foreach (var enemy in EntityManager.Heroes.Enemies.Where(c => Player.Instance.Position.IsInRange(c.Position, SpellManager.E.Range)
                    && c.IsValidTarget() && DamageHelper.getEStacks(c) >= 1))
                {
                    if (Player.Instance.HealthPercent < 10)
                    {
                        E.Cast();
                    }
                }
            }
        }
    }
}
