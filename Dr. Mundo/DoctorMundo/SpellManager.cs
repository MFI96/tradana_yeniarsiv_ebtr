using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace DrMundo
{
    public static class SpellManager
    {
        public static Spell.Skillshot Q { get; private set; }
        public static Spell.Active W { get; private set; }
        public static Spell.Active E { get; private set; }
        public static Spell.Active R { get; private set; }
        public static Spell.Targeted Ignite { get; private set; }

        static SpellManager()
        {
            // Initialize spells
            Q = new Spell.Skillshot(SpellSlot.Q, 1000, SkillShotType.Linear, 250, 2000, 60);
            W = new Spell.Active(SpellSlot.W, 315);
            E = new Spell.Active(SpellSlot.E);
            R = new Spell.Active(SpellSlot.R);
            
            
            if (Player.Instance.Spellbook.GetSpell(SpellSlot.Summoner1).Name.Equals("summonerdot", StringComparison.CurrentCultureIgnoreCase))
            {
                Ignite = new Spell.Targeted(SpellSlot.Summoner1, 600);
            }
            else if ((Player.Instance.Spellbook.GetSpell(SpellSlot.Summoner2).Name.Equals("summonerdot", StringComparison.CurrentCultureIgnoreCase)))
            {
                Ignite = new Spell.Targeted(SpellSlot.Summoner2, 600);
            }
        }

        public static void Initialize()
        {

        }

        public static bool HasIgnite()
        {
            return Ignite != null;
        }
    }
}
