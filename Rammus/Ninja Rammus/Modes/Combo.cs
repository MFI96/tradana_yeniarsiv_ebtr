using EloBuddy;
using EloBuddy.SDK;
using System.Linq;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using Settings = Rammus.Config.Modes.Combo;

namespace Rammus.Modes
{
    public sealed class Combo : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {

            if (Spinning())
            {
                return;
            }

            if (Settings.UseR && R.IsReady())
            {
                if (EntityManager.Heroes.Enemies.Count(a => a.IsValidTarget(R.Range)) >= Settings.MinR)
                {
                    R.Cast();
                    return;
                }
            }

            if (Settings.UseQ && Q.IsReady())
            {
                var target = TargetSelector.GetTarget(2000, DamageType.Magical);
                if (target != null && !target.HasBuffOfType(BuffType.Taunt))
                {
                    Q.Cast();
                    return;
                }
            }

            if (Settings.UseW && W.IsReady() && E.IsOnCooldown)
            {
                if (EntityManager.Heroes.Enemies.Count(a => a.IsValidTarget(500)) > 0)
                {
                    W.Cast();
                    return;
                }
            }

            if (Settings.UseE && E.IsReady())
            {
                foreach (var enemy in EntityManager.Heroes.Enemies.Where(enemy => Settings.MainMenu[enemy.ChampionName].Cast<CheckBox>().CurrentValue &&
                                                                                 enemy.IsValidTarget(E.Range + 150) &&
                                                                                 !enemy.HasBuffOfType(BuffType.SpellShield)).OrderByDescending(TargetSelector.GetPriority))
                {
                    
                    if (enemy != null)
                    {
                        E.Cast(enemy);
                        return;
                    }
                }
            }
        }
    }
}
