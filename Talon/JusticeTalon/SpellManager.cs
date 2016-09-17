using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace JusticeTalon
{
    public static class SpellManager
    {
        static SpellManager()
        {
            Q = new Spell.Active(SpellSlot.Q, 125);
            W = new Spell.Skillshot(SpellSlot.W, 600, SkillShotType.Cone, 1, 2300, 75)
            {
                AllowedCollisionCount = int.MaxValue
            };
            E = new Spell.Targeted(SpellSlot.E, 700);
            R = new Spell.Active(SpellSlot.R, 500);
        }

        public static Spell.Active Q { get; private set; }
        public static Spell.Skillshot W { get; private set; }
        public static Spell.Targeted E { get; private set; }
        public static Spell.Active R { get; private set; }
        
        public static void Initialize()
        {
        }
    }
}