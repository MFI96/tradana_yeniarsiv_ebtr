using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using SharpDX;
using Settings = Zac.Config.Draw.DrawMenu;

namespace Zac
{
    internal class Events
    {
        public static bool ChannelingE;

        static Events()
        {
            Drawing.OnDraw += OnDraw;
            Obj_AI_Base.OnBuffLose += Player_OnBuffLose;
            Obj_AI_Base.OnBuffGain += Player_OnBuffGain;
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
        }

        private static void Player_OnBuffGain(Obj_AI_Base sender, Obj_AI_BaseBuffGainEventArgs args)
        {
            if (!sender.IsMe) return;
            if (sender.IsMe && args.Buff.Name == "ZacE")
            {
                ChannelingE = true;
                Orbwalker.DisableAttacking = true;
                Orbwalker.DisableMovement = true;
            }

            if (sender.IsMe && args.Buff.Name == "ZacR")
            {
                Orbwalker.DisableAttacking = true;
            }
        }

        private static void Player_OnBuffLose(Obj_AI_Base sender, Obj_AI_BaseBuffLoseEventArgs args)
        {
            if (!sender.IsMe) return;
            if (sender.IsMe && args.Buff.Name == "ZacE")
            {
                ChannelingE = false;
                Orbwalker.DisableAttacking = false;
                Orbwalker.DisableMovement = false;
            }

            if (sender.IsMe && args.Buff.Name == "ZacR")
            {
                Orbwalker.DisableAttacking = false;
            }
        }

        private static void OnDraw(EventArgs args)
        {
            if (Settings.DrawCursor)
            {
                Circle.Draw(Color.Green, Config.Combo.ComboMenu.CurDistance, Game.CursorPos);
            }

            if (Settings.DrawEDistance)
            {
                Circle.Draw(Color.Purple, Config.Combo.ComboMenu.EDistanceOut, Player.Instance.Position);
            }

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
                Circle.Draw(Color.Green, new[] {1150, 1300, 1450, 1600, 1750}[SpellManager.E.Level - 1],
                    Player.Instance.Position);
            }

            if (Settings.DrawR && SpellManager.R.IsLearned)
            {
                Circle.Draw(Color.Red, SpellManager.R.Range, Player.Instance.Position);
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

        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender,
            Interrupter.InterruptableSpellEventArgs e)
        {
            if (sender == null || sender.IsAlly || !Config.Misc.MiscMenu.RInterrupt)
            {
                return;
            }

            if (SpellManager.R.IsReady() && SpellManager.R.IsInRange(sender) && e.DangerLevel == DangerLevel.High)
            {
                SpellManager.R.Cast();
            }
        }

        public static void Initialize()
        {
        }
    }
}