using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using _300_Pantheon.Modes;

namespace _300_Pantheon.Base
{
    internal class ModeController
    {
        internal static void Initialize()
        {
            Game.OnUpdate += OnUpdate;
        }

        private static void OnUpdate(EventArgs args)
        {
            if (Player.Instance.IsDead) return;

            PermaActive.Execute();

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Combo.Execute();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass) ||
                MenuDesigner.HarassUi.Get<KeyBind>("HarassToggle").CurrentValue)
            {
                Harass.Execute();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) &&
                Player.Instance.Mana > MenuDesigner.ClearUi.Get<Slider>("ClearLanaMana").CurrentValue)
            {
                LaneClear.Execute();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                LastHit.Execute();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                JungleClear.Execute();
            }
        }
    }
}