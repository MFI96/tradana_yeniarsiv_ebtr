using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace Bard
{
    public static class SpellManager
    {
        public static Spell.Skillshot Q { get; private set; }
        public static Spell.Skillshot Q2 { get; private set; }
        public static Spell.Skillshot W { get; private set; }
        public static Spell.Skillshot E { get; private set; }
        public static Spell.Skillshot R { get; private set; }

        public static Spell.Targeted Ignite { get; private set; }
        public static Spell.Targeted Smite { get; private set; }


        static SpellManager()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 860, SkillShotType.Linear, 250, 1600, 65);
            Q2 = new Spell.Skillshot(SpellSlot.Q, 1310, SkillShotType.Linear, 250, 1600, 65);
            W = new Spell.Skillshot(SpellSlot.W, 800, SkillShotType.Circular);
            E = new Spell.Skillshot(SpellSlot.E, int.MaxValue, SkillShotType.Linear);
            R = new Spell.Skillshot(SpellSlot.R, 3400, SkillShotType.Circular, 250, int.MaxValue, 650);

            Q.AllowedCollisionCount = 1;


            if (Player.Instance.Spellbook.GetSpell(SpellSlot.Summoner1).Name.Equals("summonerdot", StringComparison.CurrentCultureIgnoreCase))
            {
                Ignite = new Spell.Targeted(SpellSlot.Summoner1, 600);
            }
            if (Player.Instance.Spellbook.GetSpell(SpellSlot.Summoner2).Name.Equals("summonerdot", StringComparison.CurrentCultureIgnoreCase))
            {
                Ignite = new Spell.Targeted(SpellSlot.Summoner2, 600);
            }

            if (Utility.SmiteNames.ToList().Contains(Player.Instance.Spellbook.GetSpell(SpellSlot.Summoner1).Name))
            {
                Smite = new Spell.Targeted(SpellSlot.Summoner1, 570);
            }
            if (Utility.SmiteNames.ToList().Contains(Player.Instance.Spellbook.GetSpell(SpellSlot.Summoner2).Name))
            {
                Smite = new Spell.Targeted(SpellSlot.Summoner2, 570);
            }
        }

        public static void Initialize()
        {
        }
        public static bool HasIgnite()
        {
            return Ignite != null;
        }

        public static bool HasSmite()
        {
            return Smite != null && Smite.IsLearned;
        }
    }
}
