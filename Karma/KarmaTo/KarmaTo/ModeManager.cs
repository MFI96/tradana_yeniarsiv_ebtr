using System;
using System.Collections.Generic;
using KarmaTo.Modes;
using EloBuddy;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Utils;
using EloBuddy.SDK.Events;

namespace KarmaTo
{
    public static class ModeManager
    {
        private static List<ModeBase> Modes { get; set; }

        static ModeManager()
        {
            Modes = new List<ModeBase>();
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

            Game.OnTick += OnTick;
        }

        public static void Initialize()
        {
            Obj_AI_Base.OnProcessSpellCast += (Modes[0] as PermaActive).onSpellCast;
            Obj_AI_Base.OnBasicAttack += (Modes[0] as PermaActive).OnBasicAttack;
            Gapcloser.OnGapcloser += (Modes[0] as PermaActive).OnGapcloser;
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
