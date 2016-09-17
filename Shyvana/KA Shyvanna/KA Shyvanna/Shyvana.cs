using System;
using EloBuddy;
using EloBuddy.SDK.Rendering;
using Settings = KA_Shyvanna.Config.Modes.Draw;

namespace KA_Shyvanna
{
    internal class Shyvana
    {
        public static void Initialize()
        {
            Config.Initialize();
            SpellManager.Initialize();
            ModeManager.Initialize();
            DamageIndicator.Initialize(SpellDamage.GetTotalDamage);
            EventsManager.Initialize();

            Drawing.OnDraw += OnDraw;
        }

        private static void OnDraw(EventArgs args)
        {
            if (Settings.DrawQ && Settings.DrawReady ? SpellManager.Q.IsReady() : Settings.DrawQ)
            {
                Circle.Draw(Settings.QColor, SpellManager.Q.Range, 1f, Player.Instance);
            }

            if (Settings.DrawW && Settings.DrawReady ? SpellManager.W.IsReady() : Settings.DrawW)
            {
                Circle.Draw(Settings.WColor, SpellManager.W.Range, 1f, Player.Instance);
            }

            if (Settings.DrawE && Settings.DrawReady ? SpellManager.E.IsReady() : Settings.DrawE)
            {
                Circle.Draw(Settings.EColor, SpellManager.E.Range, 1f, Player.Instance);
            }

            if (Settings.DrawR && Settings.DrawReady ? SpellManager.R.IsReady() : Settings.DrawR)
            {
                Circle.Draw(Settings.RColor, SpellManager.R.Range, 1f, Player.Instance);
            }
        }
    }
}
