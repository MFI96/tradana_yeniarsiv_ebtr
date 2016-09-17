using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using SharpDX;

using Settings = Bard.Config.Modes.Misc;
using Settings2 = Bard.Config.Draw.DrawMenu;

namespace Bard
{
    public static class Events
    {


        static Events()
        {
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
            Gapcloser.OnGapcloser += OnGapCloser;
            Orbwalker.OnPreAttack += Orbwalker_OnPreAttack;
            Drawing.OnDraw += OnDraw;
        }

        public static void Initialize()
        {

        }



        private static void OnDraw(EventArgs args)
        {
            if (Settings2.DrawQ && SpellManager.Q.IsLearned)
            {
                Circle.Draw(Color.Green, SpellManager.Q.Range, Player.Instance.Position);
            }

            if (Settings2.DrawW && SpellManager.W.IsLearned)
            {
                Circle.Draw(Color.Red, SpellManager.W.Range, Player.Instance.Position);
            }

            if (Settings2.DrawR && SpellManager.R.IsLearned)
            {
                Circle.Draw(Color.DarkBlue, SpellManager.R.Range, Player.Instance.Position);
            }

            if (SpellManager.HasSmite())
            {
                if (Settings2.DrawSmite && Config.Smite.SmiteMenu.SmiteToggle
                    || Settings2.DrawSmite && Config.Smite.SmiteMenu.SmiteCombo
                    || Settings2.DrawSmite && Config.Smite.SmiteMenu.SmiteEnemies)
                {
                    Circle.Draw(Color.Blue, SpellManager.Smite.Range, Player.Instance.Position);
                }
            }
        }

        private static void OnGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (sender == null || sender.IsAlly || !Settings.QGapcloser)
            {
                return;
            }
            var gapclosepred = SpellManager.Q.GetPrediction(sender);
            if (SpellManager.Q.IsReady() && SpellManager.Q.IsInRange(sender) && e.End.Distance(Player.Instance) <= 300)
            {
                SpellManager.Q.Cast(gapclosepred.CastPosition);
            }
        }

        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs e)
        {
            if (sender == null || sender.IsAlly || !Settings.RInterrupt)
            {
                return;
            }

            if (SpellManager.R.IsReady() && SpellManager.R.IsInRange(sender) && e.DangerLevel == DangerLevel.High)
            {
                Core.DelayAction(delegate
                {
                    SpellManager.R.Cast(sender);
                }, Settings.RInterruptDelay);
            }
        }


        private static void Orbwalker_OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            var a = target as Obj_AI_Minion;
            var allys = EntityManager.Heroes.Allies.Count(c => Player.Instance.Distance(c) <= Player.Instance.AttackRange);

            if (a == null)
            {
                return;
            }

            if (allys < 1)
            {
                return;
            }

            if (Settings.DisableMAA)
            {
                args.Process = false;
            }
        }

        
    }
}
