using System.Runtime.InteropServices;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using _300_Pantheon.Base;

namespace _300_Pantheon.Modes
{
    internal class Combo
    {
        public static void Execute()
        {
            if (Pantheon.Q.IsReady() && MenuDesigner.ComboUi.Get<CheckBox>("ComboQ").CurrentValue)
            {
                var target = TargetSelector.GetTarget(Pantheon.Q.Range, DamageType.Physical);
                if (target == null || target.IsInvulnerable) return;

                Pantheon.Q.Cast(target);
            }

            else if (Pantheon.W.IsReady() && MenuDesigner.ComboUi.Get<CheckBox>("ComboW").CurrentValue)
            {
                var target = TargetSelector.GetTarget(Pantheon.W.Range, DamageType.Physical);
                if (target == null || target.IsInvulnerable) return;

                Pantheon.W.Cast(target);
            }

            else if (Pantheon.E.IsReady() && MenuDesigner.ComboUi.Get<CheckBox>("ComboE").CurrentValue)
            {
                var target = TargetSelector.GetTarget(Pantheon.E.Range, DamageType.Physical);
                if (target == null || target.IsInvulnerable) return;

                var prediction = Pantheon.E.GetPrediction(target);
                Pantheon.E.Cast(prediction.CastPosition);
            }
        }
    }
}