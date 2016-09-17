using System;
using EloBuddy;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using SharpDX;
using EloBuddy.SDK.Menu.Values;

namespace NinjaNunu
{
    public static class Program
    {
        public const string ChampName = "Nunu";

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
            SmiteDamage.Initialize();
            Damage.Initialize();
            Events.Initialize();

            Chat.Print("Ninja Nunu Loaded - Have a Great Game!");
        }
    }
}
