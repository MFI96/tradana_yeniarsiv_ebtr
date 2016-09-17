using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using Color = System.Drawing.Color;
using System;
using System.Linq;
using RoninFiddle.Modes;

namespace RoninFiddle
{
    internal class Program
    {
        // ReSharper disable once UnusedParameter.Local
        /// <summary>
        /// The firs thing that runs on the template
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        /// <summary>
        /// This event is triggered when the game loads
        /// </summary>
        /// <param name="args"></param>
        private static void Loading_OnLoadingComplete(EventArgs args)
        {

           // if (Player.Instance.ChampionName != "Fiddlesticks") return;
            Chat.Print("Ronin Loaded - Fiddle the Sticks");
            SpellsManager.InitializeSpells();
            Menus.CreateMenu();
            ModeManager.InitializeModes();
            DrawingsManager.InitializeDrawings();
            //Gapcloser.OnGapcloser();
        }
    }
}