using EloBuddy;
using EloBuddy.SDK;

namespace Black_Swan_Akali
{
    public static class ModeController
    {
        static ModeController()
        {
            Game.OnTick += OnTick;
        }

        private static void OnTick(System.EventArgs args)
        {
            if (OrbCombo)
                Modes.Combo.Execute();

            if (OrbHarass)
                Modes.Combo.Execute();

            if (OrbLaneClear)
                Modes.Clear.Execute();

            if (OrbJungleClear)
                Modes.Jungle.Execute();

            if (OrbLastHit)
                Modes.Lasthit.Execute();

            if (OrbFlee)
                Modes.Flee.Execute();
        }

        // Return Orbwalker Modes
        public static bool OrbCombo
        {
            get
            {
                return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
            }
        }

        public static bool OrbHarass
        {
            get
            {
                return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
            }
        }

        public static bool OrbLaneClear
        {
            get
            {
                return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear);
            }
        }

        public static bool OrbJungleClear
        {
            get
            {
                return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear);
            }
        }

        public static bool OrbLastHit
        {
            get
            {
                return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit);
            }
        }

        public static bool OrbFlee
        {
            get
            {
                return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee);
            }
        }


        public static void Initialize()
        {
        }
    }
}
