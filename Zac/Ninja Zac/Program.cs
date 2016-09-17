using System;
using EloBuddy;
using EloBuddy.SDK.Events;

namespace Zac
{
    public static class Program
    {
        public const string ChampName = "Zac";

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
            Events.Initialize();
            SpellManager.Initialize();
            ModeManager.Initialize();
            Utility.Initialize();


            Chat.Print("Ninja Zac Loaded - Have a Great Game!");
        }
    }
}