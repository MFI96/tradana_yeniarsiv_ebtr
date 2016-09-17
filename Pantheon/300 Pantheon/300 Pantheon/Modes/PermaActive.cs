using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using _300_Pantheon.Base;

namespace _300_Pantheon.Modes
{
    internal class PermaActive
    {
        public static void Execute()
        {
            if (Pantheon.Q.IsReady() && MenuDesigner.KsUi.Get<CheckBox>("KsQ").CurrentValue)
            {
                var target = TargetSelector.GetTarget(Pantheon.Q.Range, DamageType.Physical);
                if (target == null || target.IsInvulnerable) return;

                if (target.TotalShieldHealth() <= Player.Instance.GetSpellDamage(target, SpellSlot.Q))
                {
                    Pantheon.Q.Cast(target);
                }
            }

            if (Pantheon.W.IsReady() && MenuDesigner.KsUi.Get<CheckBox>("KsW").CurrentValue)
            {
                var target = TargetSelector.GetTarget(Pantheon.W.Range, DamageType.Physical);
                if (target == null || target.IsInvulnerable) return;

                if (target.TotalShieldHealth() <= Player.Instance.GetSpellDamage(target, SpellSlot.W))
                {
                    Pantheon.W.Cast(target);
                }
            }
        }
    }
}