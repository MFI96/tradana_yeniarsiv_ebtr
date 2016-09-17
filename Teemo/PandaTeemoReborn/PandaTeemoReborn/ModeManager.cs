using System;
using System.Collections.Generic;
using PandaTeemoReborn.Modes;
using EloBuddy;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Utils;

namespace PandaTeemoReborn
{
    class ModeManager
    {
        private static List<ModeBase> Modes { get; set; }

        static ModeManager()
        {
            Modes = new List<ModeBase>();

            Modes.AddRange(new ModeBase[]
            {
                new PermaActive(),
                new Combo(),
                new Flee(), 
                new Harass(),
                new LaneClear(),
                new JungleClear()
            });
            
            Game.OnTick += OnTick;
        }

        public static void Initialize()
        {
        }

        private static void OnTick(EventArgs args)
        {
            Modes.ForEach(mode =>
            {
                try
                {
                    if (mode.ShouldBeExecuted())
                    {
                        mode.Execute();
                    }
                }
                catch (Exception e)
                {
                    Logger.Log(LogLevel.Error, "Error executing mode '{0}'\n{1}", mode.GetType().Name, e);
                }
            });
        }
    }
}