using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace AddonTemplate
{
    public static class SpellManager
    {

        public static Spell.Active Q { get; private set; }
        public static Spell.Skillshot W { get; private set; }
        public static Spell.Active E { get; private set; }
        public static Spell.Active R { get; private set; }
        public static Spell.Active Recall { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Skillshot(SpellSlot.W, 950, SkillShotType.Circular, 250, 1750, 275);
            E = new Spell.Active(SpellSlot.E, 1200);
            R = new Spell.Active(SpellSlot.R);
            Recall = new Spell.Active(SpellSlot.Recall);
            W.AllowedCollisionCount = Int32.MaxValue;
        }

        public static void Initialize()
        {
            
        }

    /*    public static HitChance PredW()
        {
            var mode = Config.Modes.Misc.PredW;
            switch (mode)
            {
                case 1:
                    return HitChance.Low;
                case 2:
                    return HitChance.Medium;
                case 3:
                    return HitChance.High;
            }
            return HitChance.Medium;
        }*/
    }
}
