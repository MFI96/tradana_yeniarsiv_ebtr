using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using Color = System.Drawing.Color;
using System;
using System.Linq;
using Mario_s_Lib;
using static RoninFiddle.Menus;

namespace RoninFiddle
{
    public static class SpellsManager
    {
        /*
        Targeted spells are like Katarina`s Q
        Active spells are like Katarina`s W
        Skillshots are like Ezreal`s Q
        Circular Skillshots are like Lux`s E and Tristana`s W
        Cone Skillshots are like Annie`s W and ChoGath`s W
        */

        public static Spell.Targeted Q;
        public static Spell.Targeted W;
        public static Spell.Targeted E;
        public static Spell.Skillshot E1;
        public static Spell.Skillshot R;
        public static bool Healing;
        /// <summary>
        /// It sets the values to the spells
        /// </summary>
        public static void InitializeSpells()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 575);
            W = new Spell.Targeted(SpellSlot.W, 575);
            E = new Spell.Targeted(SpellSlot.E, 750);
            E1 = new Spell.Skillshot(SpellSlot.E, 1050, SkillShotType.Linear);
            R = new Spell.Skillshot(SpellSlot.R, 800, SkillShotType.Circular);

            Obj_AI_Base.OnLevelUp += Obj_AI_Base_OnLevelUp;
        }
        // THANKS TO IRAXE :)

        //private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        //{
        //    if (sender.IsMe && args.SData.Name == "Drain")
        //    {
        //        Orbwalker.DisableAttacking = true;
        //        Orbwalker.DisableMovement = true;
        //        Healing = true;
        //    }
        //}

        #region Damages

        /// <summary>
        /// It will return the damage but you need to set them before getting the damage
        /// </summary>
        /// <param name="target"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        public static float GetDamage(this Obj_AI_Base target, SpellSlot slot)
        {
            var damageType = DamageType.Magical;
            var AD = Player.Instance.FlatPhysicalDamageMod;
            var AP = Player.Instance.FlatMagicDamageMod;
            var sLevel = Player.GetSpell(slot).Level - 1;

            //You can get the damage information easily on wikia

            var dmg = 0f;

            switch (slot)
            {
                case SpellSlot.Q:
                    if (Q.IsReady())
                    {
                        //Information of Q damage
                        dmg += new float[] {20, 45, 70, 95, 120}[sLevel] + 1f*AD;
                    }
                    break;
                case SpellSlot.W:
                    if (W.IsReady())
                    {
                        //Information of W damage
                        dmg += new float[] {0, 0, 0, 0, 0}[sLevel] + 1f*AD;
                    }
                    break;
                case SpellSlot.E:
                    if (E.IsReady())
                    {
                        //Information of E damage
                        dmg += new float[] {80, 110, 140, 170, 200}[sLevel];
                    }
                    break;
                case SpellSlot.R:
                    if (R.IsReady())
                    {
                        //Information of R damage
                        dmg += new float[] {600, 840, 1080}[sLevel]*0.6f + 1.2f*AP;
                    }
                    break;
            }
            return Player.Instance.CalculateDamageOnUnit(target, damageType, dmg - 10);
        }

        #endregion Damages

        /// <summary>
        /// This event is triggered when a unit levels up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// 

        
        private static void Obj_AI_Base_OnLevelUp(Obj_AI_Base sender, Obj_AI_BaseLevelUpEventArgs args)
        {
            if (MiscMenu.GetCheckBoxValue("activateAutoLVL") && sender.IsMe)
            {
                var delay = MiscMenu.GetSliderValue("delaySlider");
                Core.DelayAction(LevelUPSpells, delay);

            }
        }

        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender,
          Interrupter.InterruptableSpellEventArgs e)
        {
            if (!sender.IsValidTarget(Q.Range) || e.DangerLevel != DangerLevel.High || e.Sender.Type != ObjectManager.Player.Type ||
                !e.Sender.IsEnemy)
            {
                return;
            }
            if (Q.IsReady() && Q.IsInRange(sender) && MiscMenu.GetCheckBoxValue("interrupt.Q"))
            {
                Q.Cast(sender);
            }
        }
        private static void AntiGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (!e.Sender.IsValidTarget() || e.Sender.Type != ObjectManager.Player.Type || !e.Sender.IsEnemy)
            {
                return;
            }

            if (E.IsReady() && E.IsInRange(sender) && MiscMenu.GetCheckBoxValue("gapcloser.E"))
            {
                E.Cast(sender);
            }
            if (Q.IsReady() && Q.IsInRange(sender) && MiscMenu.GetCheckBoxValue("gapcloser.Q"))
            {
                Q.Cast(sender);
            }
        }

        //private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        //{
        //    if (sender.IsMe && args.SData.Name == "Drain")
        //    {
        //        Orbwalker.DisableAttacking = true;
        //        Orbwalker.DisableMovement = true;
        //        Healing = true;
        //    }
        //}

        //END OF THE THANKS

        /// <summary>
        /// It will level up the spell using the values of the comboboxes on the menu as a priority
        /// </summary>
        private static void LevelUPSpells()
        {
            if (Player.Instance.Spellbook.CanSpellBeUpgraded(SpellSlot.R))
            {
                Player.Instance.Spellbook.LevelSpell(SpellSlot.R);
            }

            if (Player.Instance.Spellbook.CanSpellBeUpgraded(GetSlotFromComboBox(MiscMenu.GetComboBoxValue("firstFocus"))))
            {
                Player.Instance.Spellbook.LevelSpell(GetSlotFromComboBox(MiscMenu.GetComboBoxValue("firstFocus")));
            }

            if (Player.Instance.Spellbook.CanSpellBeUpgraded(GetSlotFromComboBox(MiscMenu.GetComboBoxValue("secondFocus"))))
            {
                Player.Instance.Spellbook.LevelSpell(GetSlotFromComboBox(MiscMenu.GetComboBoxValue("secondFocus")));
            }

            if (Player.Instance.Spellbook.CanSpellBeUpgraded(GetSlotFromComboBox(MiscMenu.GetComboBoxValue("thirdFocus"))))
            {
                Player.Instance.Spellbook.LevelSpell(GetSlotFromComboBox(MiscMenu.GetComboBoxValue("thirdFocus")));
            }
        }

        /// <summary>
        /// It will transform the value of the combobox into a SpellSlot
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static SpellSlot GetSlotFromComboBox(this int value)
        {
            switch (value)
            {
                case 0:
                    return SpellSlot.Q;
                case 1:
                    return SpellSlot.W;
                case 2:
                    return SpellSlot.E;
            }
            Chat.Print("Failed getting slot");
            return SpellSlot.Unknown;
        }
    }
}