using System;
using EloBuddy;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using SharpDX;
using Settings = Maokai.Config.Draw.DrawMenu;

namespace Maokai
{
    public class Events
    {
        static Events()
        {
            Drawing.OnDraw += OnDraw;
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
            Gapcloser.OnGapcloser += Gapcloser_OnGapcloser;
        }

        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender,
            Interrupter.InterruptableSpellEventArgs e)
        {
            if (sender == null || sender.IsAlly || !Config.Misc.MiscMenu.QInterrupt)
            {
                return;
            }

            if (SpellManager.Q.IsReady() && SpellManager.Q.IsInRange(sender) && e.DangerLevel == DangerLevel.High)
            {
                SpellManager.Q.Cast(sender);
            }
        }

        private static void Gapcloser_OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (sender == null || sender.IsAlly || !Config.Misc.MiscMenu.QGapclose)
            {
                return;
            }

            if (SpellManager.Q.IsReady() && SpellManager.Q.IsInRange(sender))
            {
                SpellManager.Q.Cast(sender);
            }
        }


        private static void OnDraw(EventArgs args)
        {
            if (Settings.DrawQ && SpellManager.Q.IsLearned)
            {
                Circle.Draw(Color.Green, SpellManager.Q.Range, Player.Instance.Position);
            }

            if (Settings.DrawW && SpellManager.W.IsLearned)
            {
                Circle.Draw(Color.Green, SpellManager.W.Range, Player.Instance.Position);
            }

            if (Settings.DrawE && SpellManager.E.IsLearned)
            {
                Circle.Draw(Color.Green, SpellManager.E.Range, Player.Instance.Position);
            }

            if (Settings.DrawR && SpellManager.R.IsLearned)
            {
                Circle.Draw(Color.Green, SpellManager.R.Range, Player.Instance.Position);
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

        public static void Initialize()
        {
        }
    }
}