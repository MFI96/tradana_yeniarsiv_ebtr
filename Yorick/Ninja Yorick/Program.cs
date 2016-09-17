using System;
using EloBuddy;
using EloBuddy.SDK.Events;

namespace Yorick
{
    public static class Program
    {
        public const string ChampName = "Yorick";

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


            Chat.Print("Ninja Yorick Loaded - Have a Great Game!");
        }
    }
}