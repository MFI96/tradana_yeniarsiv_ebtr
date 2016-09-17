using System;
using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Utils;
using JusticeTalon.Modes;

namespace JusticeTalon
{
    public static class ModeManager
    {
        static ModeManager()
        {
            // Initialize properties
            Modes = new List<ModeBase>();
            // Load all modes manually since we are in a sandbox which does not allow reflection
            // Order matter here! You would want something like PermaActive being called first
            Modes.AddRange(new ModeBase[]
            {
                new PermaActive(),
                new Combo(),
                new Harass(),
                new LaneClear(),
                new JungleClear(),
                new LastHit(),
                new Flee()
            });
            // Listen to events we need
            Game.OnTick += OnTick;
        }

        private static List<ModeBase> Modes { get; set; }

        public static void Initialize()
        {
            // Let the static initializer do the job, this way we avoid multiple init calls aswell
        }

        private static void OnTick(EventArgs args)
        {
            // Execute all modes
            Modes.ForEach(mode =>
            {
                try
                {
                    // Precheck if the mode should be executed
                    if (mode.ShouldBeExecuted())
                    {
                        // Execute the mode
                        mode.Execute();
                    }
                }
                catch (Exception e)
                {
                    // Please enable the debug window to see and solve the exceptions that might occur!
                    Logger.Log(LogLevel.Error, "Error executing mode '{0}'\n{1}", mode.GetType().Name, e);
                }
            });
        }

        public static void Useitems()
        {
            Tiamath();
            Hydra();
        }

        public static void Tiamath()
        {
            var tiamat = new Item(ItemId.Tiamat_Melee_Only, 400);
            if (tiamat.IsOwned() && tiamat.IsReady())
            {
                tiamat.Cast();
            }
        }

        public static void Hydra()
        {
            var hydra = new Item(ItemId.Ravenous_Hydra_Melee_Only, 400);
            if (hydra.IsOwned() && hydra.IsReady())
            {
                hydra.Cast();
            }
        }

        public static void Youmu()
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                var youmu = new Item(ItemId.Youmuus_Ghostblade);
                if (youmu.IsOwned() && youmu.IsReady())
                {
                    youmu.Cast();
                }
            }
        }
    }
}