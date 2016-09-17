using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Menu.Values;
using SharpDX;

using Settings = Rammus.Config.Draw.DrawMenu;

namespace Rammus
{
    class Events
    {
        static Events()
        {
            Gapcloser.OnGapcloser += OnGapCloser;
            Drawing.OnDraw += OnDraw;
        }

        public static void Initialize()
        {

        }

        private static void OnDraw(EventArgs args)
        {
            if (Settings.DrawQ && SpellManager.Q.IsLearned)
            {
                Circle.Draw(Color.Green, SpellManager.Q.Range, Player.Instance.Position);
            }

            if (Settings.DrawW && SpellManager.W.IsLearned)
            {
                Circle.Draw(Color.Red, SpellManager.W.Range, Player.Instance.Position);
            }

            if (Settings.DrawE && SpellManager.E.IsLearned)
            {
                Circle.Draw(Color.LightBlue, SpellManager.E.Range, Player.Instance.Position);
            }

            if (Settings.DrawR && SpellManager.R.IsLearned)
            {
                Circle.Draw(Color.DarkBlue, SpellManager.R.Range, Player.Instance.Position);
            }

            if (SpellManager.HasSmite())
            {
                if (Settings.DrawSmite && Config.Smite.SmiteMenu.SmiteToggle
                    || Settings.DrawSmite && Config.Smite.SmiteMenu.SmiteCombo
                    || Settings.DrawSmite && Config.Smite.SmiteMenu.SmiteEnemies)
                {
                    Circle.Draw(Color.Blue, SpellManager.Smite.Range, Player.Instance.Position);
                }
            }
        }

        public static void OnGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs a)
        {
            if (sender == null || sender.IsAlly || !Config.Modes.MiscMenu.GapCloseE)
            {
                return;
            }

            if ((sender.IsAttackingPlayer || a.End.Distance(Player.Instance) <= 70))
            {
                SpellManager.E.Cast(sender);
            }
        }


    }
}
