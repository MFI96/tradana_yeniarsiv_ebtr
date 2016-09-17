using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace KA_Rumble
{
    public static class SpellManager
    {
        public static Spell.Active Q { get; private set; }
        public static Spell.Active W { get; private set; }
        public static Spell.Skillshot E { get; private set; }
        public static Spell.Skillshot R { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Active(SpellSlot.Q, 600);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Skillshot(SpellSlot.E, 840, SkillShotType.Linear, 250, 2000, 70);
            R = new Spell.Skillshot(SpellSlot.R, 1700, SkillShotType.Linear, 400, 2500, 120)
            {
                AllowedCollisionCount = int.MaxValue
            };
        }

        public static void Initialize()
        {
        }
    }
}
