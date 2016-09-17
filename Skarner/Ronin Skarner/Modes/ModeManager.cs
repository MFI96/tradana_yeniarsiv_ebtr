using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Constants;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using Mario_s_Lib;
using static RoninSkarner.SpellsManager;
using static RoninSkarner.Menus;

namespace RoninSkarner.Modes
{
    internal class ModeManager
    {
        /// <summary>
        /// Create the event on tick
        /// </summary>
        public static void InitializeModes()
        {
            Game.OnTick += Game_OnTick;
        }

        /// <summary>
        /// This event is triggered every tick of the game
        /// </summary>
        /// <param name="args"></param>
        private static void Game_OnTick(EventArgs args)
        {
            var orbMode = Orbwalker.ActiveModesFlags;
            var playerMana = Player.Instance.ManaPercent;

            Active.Execute();

            if (orbMode.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Combo.Execute();
            }

            if (orbMode.HasFlag(Orbwalker.ActiveModes.Harass) && playerMana > HarassMenu.GetSliderValue("manaSlider"))
            {
                Harass.Execute();
            }

            if (orbMode.HasFlag(Orbwalker.ActiveModes.LastHit) && playerMana > LasthitMenu.GetSliderValue("manaSlider"))
            {
                LastHit.Execute();
            }

            if (orbMode.HasFlag(Orbwalker.ActiveModes.LaneClear) && playerMana > LaneClearMenu.GetSliderValue("manaSlider"))
            {
                LaneClear.Execute();
            }

            if (orbMode.HasFlag(Orbwalker.ActiveModes.JungleClear) && playerMana > JungleClearMenu.GetSliderValue("manaSlider"))
            {
                JungleClear.Execute();
            }

            if (orbMode.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                Flee.Execute();
            }
        }
    }
}