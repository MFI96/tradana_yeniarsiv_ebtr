using System;
using EloBuddy;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using SharpDX;
using EloBuddy.SDK;
namespace Vi
{
    public static class Program
    {
        public const string ChampName = "Vi";

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
            Damage.Initialize();

            Chat.Print("Ninja Vi Loaded - Have a Great Game!");
            
        }
    }
}
