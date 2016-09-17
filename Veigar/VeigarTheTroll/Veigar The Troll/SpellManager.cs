﻿using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Spells;

namespace Veigar_The_Troll
{
    public static class SpellManager
    {

        public static Spell.Skillshot Q;
        public static Spell.Skillshot E;
        public static Spell.Skillshot W;
        public static Spell.Targeted R;
        public static SpellSlot Ignite { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 950, SkillShotType.Linear, 250, 2000, 70) { AllowedCollisionCount = 1 };
            W = new Spell.Skillshot(SpellSlot.W, 900, SkillShotType.Circular, 1350, int.MaxValue, 225);
            E = new Spell.Skillshot(SpellSlot.E, 1000, SkillShotType.Circular, 500, int.MaxValue, 80) { AllowedCollisionCount = int.MaxValue };
            R = new Spell.Targeted(SpellSlot.R, 650);
            Ignite = ObjectManager.Player.GetSpellSlotFromName("summonerdot");
        }
        
        public static void Initialize()
        {
         
        }
    }
}