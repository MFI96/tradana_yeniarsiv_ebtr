using EloBuddy;
using EloBuddy.SDK.Events;
using System;
using System.Drawing;

namespace Warwick
{
    public static class Program
    {
        public const string ChampName = "Warwick";

        public static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.ChampionName != ChampName)
            {
                return;
            }
            Config.Initialize();
            SpellManager.Initialize();
            ModeManager.Initialize();
            Events.Initialize();
            WelcomeMsg();
        }

        private static void WelcomeMsg()
        {
            Chat.Print("Doctor{0} Loaded. Have a splendid game!", Color.Red, ChampName);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Doctor{0} Loaded. Have a splendid game!", ChampName);
            Console.ResetColor();
        }
    }
}
