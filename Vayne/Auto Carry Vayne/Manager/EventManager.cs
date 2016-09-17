﻿using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;

namespace Auto_Carry_Vayne.Manager
{
    class EventManager
    {
        public static void Load()
        {
            Gapcloser.OnGapcloser += Gapcloser_OnGapCloser;
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
            Obj_AI_Base.OnBuffGain += Obj_AI_Base_OnBuffGain;
            Obj_AI_Base.OnSpellCast += Obj_AI_Base_OnSpellCast;
            Obj_AI_Base.OnBasicAttack += Obj_AI_Base_OnBasicAttack;
            Spellbook.OnStopCast += Spellbook_OnStopCast;
            Game.OnUpdate += Game_OnTick;
            Drawing.OnDraw += OnDraw;
            Logic.Mechanics.LoadFlash();
            Turrets.Load();
        }

        private static void Game_OnTick(EventArgs args)
        {
            //Summoners
            Features.Utility.Summoners.Healme();
            Features.Utility.Summoners.Healally();
            //Items
            Features.Utility.Items.AutoPotion();
            Features.Utility.Items.AutoBiscuit();
            //Misc
            Features.Utility.Misc.AutobuyTrinkets();
            Features.Utility.Misc.AutoLevelUp();
            Features.Utility.Misc.Skinhack();
            Features.Utility.Misc.AutobuyStartes();
            Features.Utility.Misc.QKs();
            Features.Utility.Misc.LowlifeE();
            Features.Utility.Misc.FocusW();
            Features.Utility.Misc.AutoE();
            //Mechanics
            Logic.Mechanics.FlashE();
            Logic.Mechanics.Insec();

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)) Features.Modes.Harass.HarassCombo();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)) Features.Modes.JungleClear.Load();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee)) Features.Modes.Flee.Load();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) Features.Modes.Combo.Load();
        }

        private static void Gapcloser_OnGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            Features.Utility.Misc.Gapclose(sender, e);
        }

        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender,
            Interrupter.InterruptableSpellEventArgs e)
        {
            Features.Utility.Misc.Interrupt(sender, e);
        }

        private static void Obj_AI_Base_OnBuffGain(Obj_AI_Base sender, Obj_AI_BaseBuffGainEventArgs args)
        {
            Features.Utility.Items.BuffGain(sender, args);
        }

        private static void Obj_AI_Base_OnSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            Features.Modes.LaneClear.SpellCast(sender, args);
        }

        private static void OnDraw(EventArgs args)
        {
            Features.Utility.drawing.OnDraw();
        }

        private static void Obj_AI_Base_OnBasicAttack(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe)
            {
                Variables.lastaa = Game.Time * 1000;
            }
        }

        private static void Spellbook_OnStopCast(Obj_AI_Base sender, SpellbookStopCastEventArgs args)
        {
            if (sender.IsMe && (Game.Time * 1000) - Variables.lastaa < ObjectManager.Player.AttackCastDelay * 1000 + 50f && !args.ForceStop)
            {
                Variables.lastaa = 0f;
            }
        }
    }
}
