using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using LookSharp.Plugins;

namespace LookSharp
{
    class Program
    {
        protected static AIHeroClient Hero { get { return Player.Instance; } }
        protected static PluginBase Plugin;

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += (eventArgs) =>
            {
                switch (Player.Instance.Hero)
                {
                    case Champion.Anivia:
                        Plugin = new Anivia();
                        break;
                    case Champion.Draven:
                        Plugin = null;
                        break;
                    case Champion.Jayce:
                        Plugin = new Jayce();
                        break;
                    case Champion.LeeSin:
                        Plugin = null;
                        break;
                    case Champion.TwistedFate:
                        Plugin = null;
                        break;
                    case Champion.Viktor:
                        Plugin = new Viktor();
                        break;
                    default:
                        Plugin = null;
                        break;
                }

                if (Plugin == null)
                {
                    Chat.Print("LookSharp => No implementation for Champion " + Hero.ChampionName);
                }
            };
        }
    }
    /*
     * utilities to implement
     * path tracker
     * shared experience
     */
}
