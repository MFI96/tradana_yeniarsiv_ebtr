using System;
using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK.Events;
using KappAIO.Champions;
using KappAIO.Common;

namespace KappAIO
{
    internal class Program
    {
        private static readonly List<Champion> SupportedHeros = new List<Champion> { Champion.Gangplank, Champion.Kalista, Champion.Viktor };

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            Utility.Load.Init();
            if(Utility.Activator.Load.MenuIni.CheckBoxValue("Champ")) return;

            if (!SupportedHeros.Contains(Player.Instance.Hero)) return;
            var Instance = (Base)Activator.CreateInstance(null, "KappAIO.Champions." + Player.Instance.Hero + "." + Player.Instance.Hero).Unwrap();
            CheckVersion.Init();
        }
    }
}
