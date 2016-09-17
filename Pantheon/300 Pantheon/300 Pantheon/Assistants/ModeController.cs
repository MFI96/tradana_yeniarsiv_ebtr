using System;
using EloBuddy;
using EloBuddy.SDK;
using _300_Pantheon.Modes;

namespace _300_Pantheon.Assistants
{
    public static class ModeController
    {
        static ModeController()
        {
            Game.OnTick += OnTick;
        }

        private static void OnTick(EventArgs args)
        {
            if (Return.Activemode(Orbwalker.ActiveModes.Combo))
            {
                Combo.Execute();
            }

            if (Return.Activemode(Orbwalker.ActiveModes.Harass) || Return.HarassToggle)
            {
                Harass.Execute();
            }

            if (Return.Activemode(Orbwalker.ActiveModes.LaneClear))
            {
                Clear.Execute();
            }

            if (Return.Activemode(Orbwalker.ActiveModes.JungleClear))
            {
                Jungle.Execute();
            }

            if (Return.Activemode(Orbwalker.ActiveModes.LastHit))
            {
                Lasthit.Execute();
            }

            PermaActive.Execute();
        }

        public static void Initialize()
        {
        }
    }
}