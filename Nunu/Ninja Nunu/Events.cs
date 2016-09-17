using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using Settings = NinjaNunu.Config.Draw.DrawMenu;

namespace NinjaNunu
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
                Circle.Draw(Color.Green, 195, Player.Instance.Position);
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

        public static void OnGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (Player.Instance.Spellbook.IsChanneling || Player.Instance.HasBuff("Absolute Zero"))
            {
                return;
            }

            if (sender == null || sender.IsAlly || !Config.Modes.MiscMenu.GapcloseE)
            {
                return;
            }

            if ((sender.IsAttackingPlayer || e.End.Distance(Player.Instance) <= 70))
            {
                SpellManager.E.Cast(sender);
            }
        }


    }
}
