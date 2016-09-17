using System;
using EloBuddy;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using SharpDX;

namespace AddonTemplate
{
    public static class Program
    {
        public const string ChampName = "Twitch";
        public static int SkinBase;

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

            DamageHelper.Initialize();
            StealthHelper.Initialize();

            Drawing.OnDraw += GameEvent.OnDraw;
            Config.Modes.Misc._stealthRecall.OnValueChange += GameEvent.StealthRecall_OnValueChanged;

            Config.Modes.Draw._useHax.OnValueChange += GameEvent.UseHax_OnValueChanged;
            Config.Modes.Draw._skinhax.OnValueChange += GameEvent.SkinHax_OnValueChanged;
        }

    }
}
