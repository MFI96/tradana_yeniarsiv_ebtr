using System;
using EloBuddy;
using EloBuddy.SDK.Events;

namespace _300_Pantheon
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.Hero != Champion.Pantheon) return;
            Pantheon.Initialize();
        }
    }
}