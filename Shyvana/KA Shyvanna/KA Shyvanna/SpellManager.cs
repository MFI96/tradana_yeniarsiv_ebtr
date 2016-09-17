using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace KA_Shyvanna
{
    public static class SpellManager
    {
        public static Spell.Active Q { get; private set; }
        public static Spell.Active W { get; private set; }
        public static Spell.Skillshot E { get; private set; }
        public static Spell.Skillshot R { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Active(SpellSlot.Q, (uint)Player.Instance.GetAutoAttackRange());
            W = new Spell.Active(SpellSlot.W, 425);
            E = new Spell.Skillshot(SpellSlot.E, 925, SkillShotType.Linear, 250, 1500, 60)
            {
                AllowedCollisionCount = int.MaxValue
            };
            R = new Spell.Skillshot(SpellSlot.R, 1000, SkillShotType.Circular, 250, 1500, 150)
            {
                AllowedCollisionCount = int.MaxValue
            };
        }

        public static void Initialize()
        {
        }
    }
}
