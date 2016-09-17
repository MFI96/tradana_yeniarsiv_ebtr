using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;

namespace Vi
{
    public static class SpellManager
    {
        public static Spell.Chargeable Q { get; private set; }
        public static Spell.Active W { get; private set; }
        public static Spell.Active E { get; private set; }
        public static Spell.Skillshot E2 { get; private set; }
        public static Spell.Targeted R { get; private set; }
        public static Spell.Targeted Smite { get; private set; }
        public static Spell.Skillshot Flash { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Chargeable(SpellSlot.Q, 250, 875, 1250, 0, 1400, 55);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Active(SpellSlot.E, 600);
            E2 = new Spell.Skillshot(SpellSlot.E, 600, SkillShotType.Cone);
            R = new Spell.Targeted(SpellSlot.R, 800);
            var FlashSlot = ObjectManager.Player.GetSpellSlotFromName("summonerflash");
            Flash = new Spell.Skillshot(FlashSlot, 450, SkillShotType.Linear, 0, int.MaxValue, 55);
            
            Q.AllowedCollisionCount = int.MaxValue;
            Flash.AllowedCollisionCount = int.MaxValue;

            //VodkaSmite

            if (SmiteDamage.SmiteNames.ToList().Contains(Player.Instance.Spellbook.GetSpell(SpellSlot.Summoner1).Name))
            {
                Smite = new Spell.Targeted(SpellSlot.Summoner1, 570);
                return;
            }
            if (SmiteDamage.SmiteNames.ToList().Contains(Player.Instance.Spellbook.GetSpell(SpellSlot.Summoner2).Name))
            {
                Smite = new Spell.Targeted(SpellSlot.Summoner2, 570);
            }
        }

        public static void Initialize()
        {

        }
        public static bool HasSmite()
        {
            return Smite != null && Smite.IsLearned;
        }

        public static bool HasFlash()
        {
            return Flash != null && Flash.IsLearned;
        }
    }
}
