﻿using System;
using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Utils;
using NotBrand.Modes;

namespace NotBrand
{
    public static class ModeManager
    {
        private static List<ModeBase> Modes { get; set; }

        static ModeManager()
        {
            Modes = new List<ModeBase>();

            Modes.AddRange(new ModeBase[]
            {
                new LaneClear(),
                new Combo(), 
                new Harass(),
                new Ignite(), 
            });

            Game.OnTick += OnTick;
            Drawing.OnDraw += OnDraw;
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
                    if (mode.IsReady() && mode.ShouldBeExecuted())
                    {
                        mode.SetDelay();
                        mode.Execute();
                    }
                }
                catch (Exception e)
                {
                    Logger.Log(LogLevel.Error, "Error executing mode '{0}'\n{1}", mode.GetType().Name, e);
                }
            });
        }

        private static void OnDraw(EventArgs args)
        {
            Modes.ForEach(mode =>
            {
                try
                {
                    mode.Draw();
                }
                catch (Exception e)
                {
                    Logger.Log(LogLevel.Error, "Error executing mode '{0}'\n{1}", mode.GetType().Name, e);
                }
            });
        }
    }
}
