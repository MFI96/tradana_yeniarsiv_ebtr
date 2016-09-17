using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

using Settings = KarmaTo.Config.Modes.Combo;

//TODO :
// Draw W Range when is active


namespace KarmaTo.Modes
{
    public sealed class Combo : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {
            Orbwalker.DisableAttacking = false;
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            if (W.IsReady() && Settings.UseW && target.IsValidTarget(W.Range))
            {
                if (R.IsReady() && Utils.getPlayer().HealthPercent < Settings.comboUseRW)
                    R.Cast();
                W.Cast(target);


            }
            if (Settings.UseQ)
                SpellManager.castQ(target, Settings.UseR, Settings.predictionHit);
        }
    }
}
