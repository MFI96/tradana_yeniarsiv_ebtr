using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace LightAmumu
{
    public static class SpellManager
    {
        public static Spell.Skillshot Q { get; private set; }
        public static Spell.Active W { get; private set; }
        public static Spell.Active E { get; private set; }
        public static Spell.Active R { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1100, SkillShotType.Linear, 250, 2000, 50);
            W = new Spell.Active(SpellSlot.W, 300);
            E = new Spell.Active(SpellSlot.E, 350);
            R = new Spell.Active(SpellSlot.R, 550);
        }

        public static void Initialize()
        {
        }
    }
}