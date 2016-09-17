using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using _300_Pantheon.Base;

namespace _300_Pantheon
{
    internal class Pantheon
    {
        public static Spell.Targeted Q = new Spell.Targeted(SpellSlot.Q, 600);
        public static Spell.Targeted W = new Spell.Targeted(SpellSlot.W, 600);

        public static Spell.Skillshot E = new Spell.Skillshot(SpellSlot.E, 400, SkillShotType.Cone, 250, 800,
            (int) (35*Math.PI/180));

        internal static void Initialize()
        {
            MenuDesigner.Initialize();
            ModeController.Initialize();

            Drawing.OnDraw += OnDraw;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            Obj_AI_Base.OnBuffLose += OnBuffLose;
            Gapcloser.OnGapcloser += OnGapcloser;
            Interrupter.OnInterruptableSpell += OnInterruptableSpell;
        }

        private static void OnDraw(EventArgs args)
        {
            if (Player.Instance.IsDead || Shop.IsOpen || MainMenu.IsOpen ||
                !MenuDesigner.MiscUi.Get<CheckBox>("MiscDrawQW").CurrentValue) return;

            Circle.Draw(Color.Orange, Q.Range, Player.Instance);
        }

        private static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe || args.Slot != SpellSlot.E) return;
            Orbwalker.DisableAttacking = true;
            Orbwalker.DisableMovement = true;
        }

        private static void OnBuffLose(Obj_AI_Base sender, Obj_AI_BaseBuffLoseEventArgs args)
        {
            if (!sender.IsMe || args.Buff.Name != "pantheonesound") return;
            Orbwalker.DisableAttacking = false;
            Orbwalker.DisableMovement = false;
        }

        private static void OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (!sender.IsEnemy || sender.IsUnderEnemyturret() ||
                !MenuDesigner.MiscUi.Get<CheckBox>("GapW").CurrentValue || !W.IsReady() || !W.IsInRange(e.End)) return;

            W.Cast(sender);
        }

        private static void OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs e)
        {
            if (!sender.IsEnemy || e.DangerLevel != DangerLevel.High ||
                !MenuDesigner.MiscUi.Get<CheckBox>("InterW").CurrentValue || !W.IsReady() || !E.IsInRange(sender))
                return;

            W.Cast(sender);
        }
    }
}