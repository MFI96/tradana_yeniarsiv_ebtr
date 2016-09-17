using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace Yorick
{
    public static class SpellManager
    {
        static SpellManager()
        {
            Q = new Spell.Active(SpellSlot.Q, 125);
            W = new Spell.Skillshot(SpellSlot.W, 585, SkillShotType.Circular, 250, int.MaxValue, 200);
            E = new Spell.Targeted(SpellSlot.E, 540);
            R = new Spell.Targeted(SpellSlot.R, 835);

            if (Utility.SmiteNames.ToList().Contains(Player.Instance.Spellbook.GetSpell(SpellSlot.Summoner1).Name))
            {
                Smite = new Spell.Targeted(SpellSlot.Summoner1, 570);
            }
            if (Utility.SmiteNames.ToList().Contains(Player.Instance.Spellbook.GetSpell(SpellSlot.Summoner2).Name))
            {
                Smite = new Spell.Targeted(SpellSlot.Summoner2, 570);
            }
            if (Player.Instance.Spellbook.GetSpell(SpellSlot.Summoner1)
                .Name.Equals("summonerdot", StringComparison.CurrentCultureIgnoreCase))
            {
                Ignite = new Spell.Targeted(SpellSlot.Summoner1, 600);
            }
            if (Player.Instance.Spellbook.GetSpell(SpellSlot.Summoner2)
                .Name.Equals("summonerdot", StringComparison.CurrentCultureIgnoreCase))
            {
                Ignite = new Spell.Targeted(SpellSlot.Summoner2, 600);
            }
        }

        public static Spell.Active Q { get; private set; }
        public static Spell.Skillshot W { get; private set; }
        public static Spell.Targeted E { get; private set; }
        public static Spell.Targeted R { get; private set; }
        public static Spell.Targeted Smite { get; private set; }
        public static Spell.Targeted Ignite { get; private set; }

        public static void Initialize()
        {
        }

        public static bool HasSmite()
        {
            return Smite != null && Smite.IsLearned;
        }

        public static bool HasIgnite()
        {
            return Ignite != null;
        }
    }
}