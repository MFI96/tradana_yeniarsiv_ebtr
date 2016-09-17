using System;
using EloBuddy;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using SharpDX;

namespace RekSai
{
    public static class Program
    {
        public const string ChampName = "RekSai";

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
            ItemsManager.Initialize();
            SmiteDamage.Initialize();

            Chat.Print("Ninja RekSai Loaded - Have a Great Game!");

        }

        private static void OnDraw(EventArgs args)
        {
            // Draw range circles of our spells
            Circle.Draw(Color.Red, SpellManager.Q.Range, Player.Instance.Position);
            // TODO: Uncomment if you want those enabled aswell, but remember to enable them
            // TODO: in the SpellManager aswell, otherwise you will get a NullReferenceException
            //Circle.Draw(Color.Red, SpellManager.W.Range, Player.Instance.Position);
            //Circle.Draw(Color.Red, SpellManager.E.Range, Player.Instance.Position);
            //Circle.Draw(Color.Red, SpellManager.R.Range, Player.Instance.Position);
        }
    }
}
