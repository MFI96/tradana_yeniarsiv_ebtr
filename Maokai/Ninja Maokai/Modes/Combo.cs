using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using Settings = Maokai.Config.Combo.ComboMenu;

namespace Maokai.Modes
{
    public sealed class Combo : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(E.Range, DamageType.Magical);

            if (Settings.UseE && E.IsReady() && ManaPercent >= Settings.ManaE)
            {
                if (target != null && target.IsValidTarget(E.Range))
                {
                    var epred = E.GetPrediction(target);
                    if (epred.HitChance >= HitChance.High)
                    {
                        E.Cast(target);
                        return;
                    }
                }
            }

            var targetW = TargetSelector.GetTarget(W.Range, DamageType.Magical);

            if (Settings.UseW && W.IsReady())
            {
                if (targetW != null && targetW.IsValidTarget(W.Range))
                {
                    W.Cast(targetW);
                    return;
                }
            }

            if (Settings.UseQ && Q.IsReady())
            {
                if (targetW != null && targetW.IsValidTarget(Q.Range))
                {
                    var qpred = Q.GetPrediction(targetW);
                    if (qpred.HitChance >= HitChance.High)
                    {
                        Q.Cast(targetW);
                        return;
                    }
                }
            }


            if (Settings.UseR && R.IsReady())
            {
                var targetsR = TargetSelector.GetTarget(R.Range, DamageType.Magical);
                var enough = Player.Instance.CountEnemiesInRange(R.Range - 50) >= Settings.MinR;
                var toggleState = R.Handle.ToggleState;
                var noenemies = Player.Instance.CountEnemiesInRange(R.Range - 50) == 0;

                if (targetsR != null && enough && ManaPercent >= Settings.ManaR)
                {
                    if (toggleState == 1)
                    {
                        R.Cast();
                        return;
                    }
                }

                if (Settings.TurnoffR && noenemies)
                {
                    if (toggleState == 2)
                    {
                        R.Cast();
                    }
                }
            }
        }
    }
}