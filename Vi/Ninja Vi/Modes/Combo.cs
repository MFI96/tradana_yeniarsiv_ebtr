using EloBuddy;
using EloBuddy.SDK;
using System.Linq;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Enumerations;

using Settings = Vi.Config.Modes.Combo;

namespace Vi.Modes
{
    public sealed class Combo : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(Q.MaximumRange + 300, DamageType.Physical);
            if (Settings.UseQ && Q.IsReady() && target != null)
            {
                if (Q.IsCharging)
                {
                    Q.Cast(target);
                    return;
                }
                else
                {
                    Q.StartCharging();
                    return;
                }     
            }

            if (Settings.UseR && R.IsReady())
            {
                foreach (var enemy in EntityManager.Heroes.Enemies.Where(enemy => Settings.MainMenu[enemy.ChampionName].Cast<CheckBox>().CurrentValue &&
                                                                                 enemy.IsValidTarget(R.Range) && enemy.Health <= Player.Instance.GetSpellDamage(enemy, SpellSlot.Q) * 2 + Damage.GetDamage(enemy) &&
                                                                                 !enemy.HasBuffOfType(BuffType.SpellShield) && !enemy.HasBuffOfType(BuffType.Invulnerability)).OrderByDescending(TargetSelector.GetPriority))
                {

                    if (enemy != null)
                    {
                        R.Cast(enemy);
                        return;
                    }
                }
            }
        }
    }
}
