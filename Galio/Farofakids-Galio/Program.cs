using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK;
using SharpDX;

namespace Farofakids_Galio
{
    public static class Program
    {
        public const string ChampName = "Galio";
        public static bool RavenForm;

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

            DamageIndicator.Initialize(Damages.GetTotalDamage);
            DamageIndicator.DrawingColor = System.Drawing.Color.Aqua;
            Drawing.OnDraw += OnDraw;
            Interrupter.OnInterruptableSpell += OnInterruptableSpell;
            Gapcloser.OnGapcloser += Gapcloser_OnGap;
        }

        private static void OnDraw(EventArgs args)
        {
            foreach (var spell in SpellManager.Spells)
            {
                switch (spell.Slot)
                {
                    case SpellSlot.Q:
                        if (!Config.Modes.Drawing.DrawQ)
                        {
                            continue;
                        }
                        break;
                    case SpellSlot.W:
                        if (!Config.Modes.Drawing.DrawW)
                        {
                            continue;
                        }
                        break;
                    case SpellSlot.E:
                        if (!Config.Modes.Drawing.DrawE)
                        {
                            continue;
                        }
                        break;
                    case SpellSlot.R:
                        if (!Config.Modes.Drawing.DrawR)
                        {
                            continue;
                        }
                        break;
                }

                Circle.Draw(spell.GetColor(), spell.Range, Player.Instance);

                DamageIndicator.HealthbarEnabled = Config.Modes.Drawing.IndicatorHealthbar;
            }

        }

        public static bool rActive
        {
            get { return Player.Instance.Buffs.Any(buff => buff.Name == "GalioIdolOfDurand"); }
        }

        private static void OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            if (sender.IsEnemy && args.DangerLevel == DangerLevel.High && Config.Modes.Misc.UseWint && SpellManager.W.IsReady() && SpellManager.W.IsInRange(sender))
            {
                SpellManager.W.Cast(Player.Instance);
            }
        }

        private static void Gapcloser_OnGap(AIHeroClient Sender, Gapcloser.GapcloserEventArgs args)
        {
            var agapcloser = Config.Modes.Misc.UseWint;
            var antigapc = SpellManager.W.IsReady() && agapcloser;
            if (antigapc)
            {
                if (Sender.IsMe)
                {
                    var gap = args.Sender;
                    if (gap.IsValidTarget(4000))
                    {
                       SpellManager.W.Cast(Player.Instance);
                    }
                }
            }
        }

    }
}
