using System;
using EloBuddy;
using EloBuddy.SDK.Events;

namespace Bard
{
    public static class Program
    {
        private const string ChampName = "Bard";

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
            Misc.Initialize();
            ModeManager.Initialize();
            SpellManager.Initialize();

            Chat.Print("Ninja Bard Loaded - Have a Great Game!");
        }
    }
}
