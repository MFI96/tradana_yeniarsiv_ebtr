using System;
using EloBuddy;
using EloBuddy.SDK.Events;
using RoninSkarner.Modes;

namespace RoninSkarner
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
            //Put the name of the champion here
            if (Player.Instance.ChampionName != "Skarner") return;
            Chat.Print("Welcome to the Ronin´s BETA ;)");
            SpellsManager.InitializeSpells();
            Menus.CreateMenu();
            ModeManager.InitializeModes();
            _W.Initialize();
            _W_Advance.Initialize();
            DrawingsManager.InitializeDrawings();
        }
    }
}