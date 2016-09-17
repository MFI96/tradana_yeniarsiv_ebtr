using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace PandaTeemoReborn
{
    internal class SpellManager
    {
        public static Spell.Targeted Q { get; set; }

        public static Spell.Active W { get; set; }

        public static Spell.Skillshot R { get; set; }

        static SpellManager()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 680);
            W = new Spell.Active(SpellSlot.W);
            R = new Spell.Skillshot(SpellSlot.R, 0, SkillShotType.Circular, 0, 1000, 135);
        }

        public static void Initialize()
        {
        }
    }
}