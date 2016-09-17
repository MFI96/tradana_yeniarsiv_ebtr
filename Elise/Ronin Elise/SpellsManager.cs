using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mario_s_Lib;
using static RoninElise.Menus;

namespace RoninElise
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

        //Remenber of putting the correct type of the spell here
        public static Spell.Targeted Q;
        public static Spell.Skillshot W;
        public static Spell.Skillshot E;
        public static Spell.Active R;
        public static Spell.Targeted Q2;
        public static Spell.Active W2;
        public static Spell.Targeted E2;

        //        new Spell.Targeted(SpellSlot.Q, 625),
        //        new Spell.Skillshot(SpellSlot.W, 950, SkillShotType.Circular),
        //        new Spell.Skillshot(SpellSlot.E, 1075, SkillShotType.Linear, 250, 1600, 80) { AllowedCollisionCount = 0},
        //        new Spell.Active(SpellSlot.R),
        //        new Spell.Targeted(SpellSlot.Q, 475),
        //        new Spell.Active(SpellSlot.W),
        //        new Spell.Skillshot(SpellSlot.E, 750, SkillShotType.Circular),
        public static void InitializeSpells()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 625);
            W = new Spell.Skillshot(SpellSlot.W, 950, SkillShotType.Linear, 250, 1000, 100);
            E = new Spell.Skillshot(SpellSlot.E, 1075, SkillShotType.Linear, 250, 1300, 55) { AllowedCollisionCount = 0 };
            R = new Spell.Active(SpellSlot.R);
            Q2 = new Spell.Targeted(SpellSlot.Q, 950);
            W2 = new Spell.Active(SpellSlot.W);
            E2 = new Spell.Targeted(SpellSlot.E, 750);


            Obj_AI_Base.OnLevelUp += Obj_AI_Base_OnLevelUp;
        }

        public static string HumanQ
        {
            get { return "Q"; }
        }

        public static string SpiderQ
        {
            get { return "Q2"; }
        }

        public static string HumanW
        {
            get { return "W"; }
        }

        public static string SpiderW
        {
            get { return "W2"; }
        }

        public static string HumanE
        {
            get { return "E"; }
        }

        public static string SpiderE
        {
            get { return "E2"; }
        }

        public static bool IsSpider
        {
            get
            {
                return Player.GetSpell(SpellSlot.Q).Name == SpiderQ ||
                       Player.GetSpell(SpellSlot.W).Name == SpiderW ||
                       Player.GetSpell(SpellSlot.E).Name == SpiderE;
            }
        }

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
        private static void Obj_AI_Base_OnLevelUp(Obj_AI_Base sender, Obj_AI_BaseLevelUpEventArgs args)
        {
            if (MiscMenu.GetCheckBoxValue("activateAutoLVL") && sender.IsMe)
            {
                var delay = MiscMenu.GetSliderValue("delaySlider");
                Core.DelayAction(LevelUPSpells, delay);

            }
        }

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