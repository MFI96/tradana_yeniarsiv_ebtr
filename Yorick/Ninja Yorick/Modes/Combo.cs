using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using Settings = Yorick.Config.Combo.ComboMenu;

namespace Yorick.Modes
{
    public sealed class Combo : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {
            if (Player.GetSpell(SpellSlot.R).Name.Equals("yorickreviveallyguide") && Settings.Clone)
            {
                var targetr = TargetSelector.GetTarget(2200, DamageType.Physical);

                if (targetr != null && targetr.IsValid)
                {
                    Core.DelayAction(() => Player.IssueOrder(GameObjectOrder.MovePet, targetr), 500);
                    Console.WriteLine("clone attacked!");
                }
            }

            if (Settings.UseW && W.IsReady())
            {
                var targetw = TargetSelector.GetTarget(W.Range, DamageType.Physical);

                if (targetw != null && targetw.IsValid)
                {
                    W.Cast(targetw);
                    return;
                }
            }

            if (Settings.UseE && E.IsReady())
            {
                var targete = TargetSelector.GetTarget(E.Range, DamageType.Physical);

                if (targete != null && targete.IsValid)
                {
                    E.Cast(targete);
                    return;
                }
            }

            if (Settings.UseR && R.IsReady())
            {
                if (Player.GetSpell(SpellSlot.R).Name.Equals("YorickReviveAlly"))
                {
                    foreach (
                        var ally in
                            EntityManager.Heroes.Allies.Where(
                                ally =>
                                    Settings.MainMenu[ally.ChampionName].Cast<CheckBox>().CurrentValue &&
                                    ally.HealthPercent <= Settings.HealthR && ally.IsValidTarget(R.Range))
                                .OrderByDescending(ally => ally.TotalAttackDamage))
                    {
                        if (ally != null && ally.CountEnemiesInRange(1000) > 0 && R.IsInRange(ally))
                        {
                            R.Cast(ally);
                        }
                    }
                }
            }
        }
    }
}