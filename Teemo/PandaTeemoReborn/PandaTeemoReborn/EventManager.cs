using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using SharpDX;

namespace PandaTeemoReborn
{
    internal class EventManager
    {
        /// <summary>
        /// Called when the game finishes loading.
        /// </summary>
        /// <param name="args"></param>
        public static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Champion.Teemo != Player.Instance.Hero)
            {
                Chat.Print("PandaTeemo | Encountered Error: Incorrect Champion | Champion Not Loaded",
                    System.Drawing.Color.Red);
                return;
            }

            Config.Initialize();
            SpellManager.Initialize();
            ModeManager.Initialize();
            AutoShroom.Initialize();
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
            Gapcloser.OnGapcloser += Gapcloser_OnGapcloser;
            Orbwalker.OnPostAttack += Orbwalker_OnPostAttack;
            Orbwalker.OnPostAttack += Orbwalker_LaneClear_OnPostAttack;
            Orbwalker.OnPostAttack += Orbwalker_JungleClear_OnPostAttack;
            GameObject.OnCreate += GameObject_OnCreate;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
            Drawing.OnDraw += Drawing_OnDraw;

            Chat.Print("PandaTeemo | Successfully Loaded ", System.Drawing.Color.LawnGreen);
        }

        /// <summary>
        /// Called when a Object is Created
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void GameObject_OnCreate(GameObject sender, EventArgs args)
        {
            if (sender.Name == "Teemo_Base_R_GroundImpact_Dust.troy")
            {
                Extensions.HasShroomLanded = true;
            }
        }

        /// <summary>
        /// Called when a Spell is Interruptable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs e)
        {
            if (!Extensions.MenuValues.Misc.InterruptQ || !SpellManager.Q.IsReady())
            {
                return;
            }
            if (sender == null)
            {
                return;
            }
            if (e.DangerLevel == DangerLevel.High)
            {
                SpellManager.Q.Cast(sender);
            }
        }

        /// <summary>
        /// Called when a Gapcloser is avaliable.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Gapcloser_OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (!Extensions.MenuValues.Misc.GapcloserR || !sender.IsValidTarget())
            {
                return;
            }

            SpellManager.R.Cast(sender);
        }

        /// <summary>
        /// Called after Orbwalker attacks
        /// </summary>
        /// <param name="target"></param>
        /// <param name="args"></param>
        private static void Orbwalker_JungleClear_OnPostAttack(AttackableUnit target, EventArgs args)
        {
            // Jungle Clear Logic
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear) &&
                Extensions.MenuValues.JungleClear.UseQ)
            {
                if (Extensions.MenuValues.JungleClear.ManaQ >= Player.Instance.ManaPercent)
                {
                    return;
                }

                var m = target as Obj_AI_Minion;

                if (m == null || m.Team != GameObjectTeam.Neutral)
                {
                    return;
                }

                if (Extensions.MenuValues.JungleClear.BigMob)
                {
                    if (SpellManager.Q.IsInRange(m) && Extensions.JungleMobsList.Contains(m.BaseSkinName))
                    {
                        SpellManager.Q.Cast(m);
                    }
                }
                else if (SpellManager.Q.IsInRange(m))
                {
                    SpellManager.Q.Cast(m);
                }
            }
        }

        /// <summary>
        /// Called after Orbwalker attacks
        /// </summary>
        /// <param name="target"></param>
        /// <param name="args"></param>
        private static void Orbwalker_LaneClear_OnPostAttack(AttackableUnit target, EventArgs args)
        {
            // Lane Clear Logic
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) &&
                Extensions.MenuValues.LaneClear.UseQ && !Extensions.MenuValues.LaneClear.DisableLaneClear)
            {
                if (Extensions.MenuValues.LaneClear.ManaQ >= Player.Instance.ManaPercent)
                {
                    return;
                }

                var m = target as Obj_AI_Minion;

                if (m == null || !m.IsMinion)
                {
                    return;
                }

                if (SpellManager.Q.IsInRange(m) &&
                    Extensions.DamageLibrary.CalculateDamage(m, true, false) >= m.Health)
                {
                    SpellManager.Q.Cast(m);
                }
            }
        }

        /// <summary>
        /// Called after Orbwalker attacks
        /// </summary>
        /// <param name="target"></param>
        /// <param name="args"></param>
        private static void Orbwalker_OnPostAttack(AttackableUnit target, EventArgs args)
        {
            if (!SpellManager.Q.IsReady())
            {
                return;
            }

            // Combo Logic
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) && Extensions.MenuValues.Combo.UseQ)
            {
                var t = target as AIHeroClient;

                if (t == null)
                {
                    return;
                }

                if (Extensions.MenuValues.Combo.ManaQ >= Player.Instance.ManaPercent)
                {
                    return;
                }

                if (!Player.Instance.IsInRange(t, SpellManager.Q.Range - Extensions.MenuValues.Combo.CheckAutoAttack))
                {
                    return;
                }

                if (Extensions.MenuValues.Combo.UseQMarksman && Extensions.Marksman.Contains(t.Hero))
                {
                    SpellManager.Q.Cast(t);
                }
                else if (!Extensions.MenuValues.Combo.UseQMarksman)
                {
                    SpellManager.Q.Cast(t);
                }
            }

            // Harass Logic
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass) && Extensions.MenuValues.Harass.UseQ)
            {
                var t = target as AIHeroClient;

                if (t == null)
                {
                    return;
                }

                if (Extensions.MenuValues.Harass.ManaQ >= Player.Instance.ManaPercent)
                {
                    return;
                }

                if (!Player.Instance.IsInRange(t, SpellManager.Q.Range - Extensions.MenuValues.Harass.CheckAutoAttack))
                {
                    return;
                }

                if (Extensions.MenuValues.Harass.UseQMarksman && Extensions.Marksman.Contains(t.Hero))
                {
                    SpellManager.Q.Cast(t);
                }
                else if (!Extensions.MenuValues.Harass.UseQMarksman)
                {
                    SpellManager.Q.Cast(t);
                }
            }
            // Lane Clear Harass Logic
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) &&
                Extensions.MenuValues.LaneClear.UseQ && Extensions.MenuValues.LaneClear.HarassLogic)
            {
                var t = target as AIHeroClient;

                if (t == null)
                {
                    return;
                }

                if (Extensions.MenuValues.LaneClear.ManaQ >= Player.Instance.ManaPercent)
                {
                    return;
                }

                if (!Player.Instance.IsInRange(t, SpellManager.Q.Range - Extensions.MenuValues.Harass.CheckAutoAttack))
                {
                    return;
                }

                if (Extensions.MenuValues.Harass.UseQMarksman && Extensions.Marksman.Contains(t.Hero))
                {
                    SpellManager.Q.Cast(t);
                }
                else if (!Extensions.MenuValues.Harass.UseQMarksman)
                {
                    SpellManager.Q.Cast(t);
                }
            }
        }

        /// <summary>
        /// Called when a spell is casted.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            if (args.SData.Name.ToLower() == "teemorcast")
            {
                Extensions.HasShroomLanded = false;
                Extensions.LastR = Environment.TickCount;
            }
        }

        /// <summary>
        /// Called when the game draws.
        /// </summary>
        /// <param name="args"></param>
        private static void Drawing_OnDraw(EventArgs args)
        {
            //Chat.Print(Extensions.TeemoShroomPrediction.CalculateTravelTime(AutoShroom.ShroomPosition.FirstOrDefault(pos => pos.IsInRange(Player.Instance, SpellManager.R.Range)).To2D(), Vector3.Zero));

            if (Extensions.MenuValues.Drawing.DrawQ)
            {
                Circle.Draw(SpellManager.Q.IsReady() ? Color.YellowGreen : Color.Red, SpellManager.Q.Range, Player.Instance);
            }

            if (Extensions.MenuValues.Drawing.DrawR)
            {
                Circle.Draw(SpellManager.R.IsReady() ? Color.YellowGreen : Color.Red, SpellManager.R.Range, Player.Instance);
            }

            if (Extensions.MenuValues.Drawing.DrawAutoR)
            {
                foreach (var pos in AutoShroom.ShroomPosition)
                {
                    Circle.Draw(!Extensions.IsShroomed(pos) ? Color.YellowGreen : Color.Red, SpellManager.R.Radius, pos);
                }
            }

            if (Extensions.MenuValues.Drawing.DrawDoubleShroom)
            {
                var drawPos = Extensions.TeemoShroomPrediction.GetPrediction().BouncePosition;

                if (drawPos != Vector3.Zero)
                {
                    Circle.Draw(SpellManager.R.IsReady() ? Color.YellowGreen : Color.Red, SpellManager.R.Radius, drawPos);
                }
            }
        }
    }
}