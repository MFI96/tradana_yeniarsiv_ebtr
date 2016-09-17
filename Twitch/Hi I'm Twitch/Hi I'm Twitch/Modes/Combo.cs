using System;
using EloBuddy;
using EloBuddy.SDK;

using Settings = AddonTemplate.Config.Modes.Combo;

namespace AddonTemplate.Modes
{
    public sealed class Combo : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        private void ItemUsage()
        {
            var target = TargetSelector.GetTarget(550, DamageType.Physical); // 550 = Botrk.Range
            if (Settings.UseYoumuu && Config.Youmuu.IsOwned() && Config.Youmuu.IsReady())
            {
                Config.Youmuu.Cast();
            }
            if (target != null)
            {
                if (Settings.useBotrk && Item.HasItem(Config.Cutlass.Id) && Item.CanUseItem(Config.Cutlass.Id) &&
                    Player.Instance.HealthPercent < Settings.MinHPBotrk &&
                    target.HealthPercent < Settings.EnemyMinHPBotrk)
                {
                    Item.UseItem(Config.Cutlass.Id, target);
                }
                if (Settings.useBotrk && Item.HasItem(Config.Botrk.Id) && Item.CanUseItem(Config.Botrk.Id) &&
                    Player.Instance.HealthPercent < Settings.MinHPBotrk &&
                    target.HealthPercent < Settings.EnemyMinHPBotrk)
                {
                    Config.Botrk.Cast(target);
                }
            }
        }

        public override void Execute()
        {
            ItemUsage();
            if (Settings.UseQ && Q.IsReady())
            {
               Q.Cast();
            }
            if (Settings.UseW && W.IsReady())
            {
                var target = TargetSelector.GetTarget(W.Range - 50, DamageType.Physical);
                if (target != null && target.IsValidTarget())
                {
                    if ((Settings.UseW && !Player.Instance.HasBuff("TwitchFullAutomatic")) || (Settings.UseWUlt && Player.Instance.HasBuff("TwitchFullAutomatic")))
                    {
                        var predW = W.GetPrediction(target);
                        W.Cast(predW.CastPosition);
                    }
                }
            }
            if (Settings.UseE && E.IsReady())
            {
                var target = TargetSelector.GetTarget(E.Range, DamageType.Physical);
                if (target != null && target.IsValidTarget() && target.Health < DamageHelper.GetEDamage(target))
                {
                    E.Cast();
                }
            }
            if (Settings.UseR && R.IsReady() && Player.Instance.CountEnemiesInRange(850) >= Settings.NumberR)
            {
               R.Cast();
            }
        }
    }
}
