using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using _300_Pantheon.Base;

namespace _300_Pantheon.Modes
{
    internal class Harass
    {
        public static void Execute()
        {
            if (!Pantheon.Q.IsReady() || !MenuDesigner.HarassUi.Get<CheckBox>("HarassQ").CurrentValue ||
                !(Player.Instance.ManaPercent > MenuDesigner.HarassUi.Get<Slider>("HarassMana").CurrentValue)) return;

            var target = TargetSelector.GetTarget(Pantheon.Q.Range, DamageType.Physical);
            if (target == null || target.IsInvulnerable) return;

            Pantheon.Q.Cast(target);
        }
    }
}