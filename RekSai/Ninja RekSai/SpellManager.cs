using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace RekSai
{
    public static class SpellManager
    {
        public static Spell.Active Q { get; private set; }
        public static Spell.Active W { get; private set; }
        public static Spell.Targeted E { get; private set; }
        public static Spell.Skillshot Q2 { get; private set; }

        public static Spell.Skillshot E2 { get; private set; }

        public static Spell.Targeted Smite { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Active(SpellSlot.Q, 325);          
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Targeted(SpellSlot.E, 250);            
            Q2 = new Spell.Skillshot(SpellSlot.Q, 1450, SkillShotType.Linear, 500, 1950, 60);
            E2 = new Spell.Skillshot(SpellSlot.E, 750, SkillShotType.Linear);

            Q2.AllowedCollisionCount = 1;
            E2.AllowedCollisionCount = int.MaxValue;

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
    }
}
