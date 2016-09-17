using System;
using EloBuddy.SDK.Events;

namespace KA_Rumble
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            Rumble.Initialize();
        }
    }
}
