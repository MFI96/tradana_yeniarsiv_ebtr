using System;
using EloBuddy;
using EloBuddy.SDK.Events;

namespace Maokai
{
    public static class Program
    {
        public const string ChampName = "Maokai";

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

            Chat.Print("Ninja Maokai Loaded - Have a Great Game!");
        }
    }
}