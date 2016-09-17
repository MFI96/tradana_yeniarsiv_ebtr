using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace Maokai
{
    public static class SpellManager
    {
        static SpellManager()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 600, SkillShotType.Linear, 500, 1200, 110);
            W = new Spell.Targeted(SpellSlot.W, 525);
            E = new Spell.Skillshot(SpellSlot.E, 1075, SkillShotType.Circular, 1000, 1500, 225);
            R = new Spell.Active(SpellSlot.R, 475);
            Q.AllowedCollisionCount = int.MaxValue;
            E.AllowedCollisionCount = int.MaxValue;


            if (Utility.SmiteNames.ToList().Contains(Player.Instance.Spellbook.GetSpell(SpellSlot.Summoner1).Name))
            {
                Smite = new Spell.Targeted(SpellSlot.Summoner1, 570);
                return;
            }
            if (Utility.SmiteNames.ToList().Contains(Player.Instance.Spellbook.GetSpell(SpellSlot.Summoner2).Name))
            {
                Smite = new Spell.Targeted(SpellSlot.Summoner2, 570);
            }
        }

        public static Spell.Skillshot Q { get; private set; }
        public static Spell.Targeted W { get; private set; }
        public static Spell.Skillshot E { get; private set; }
        public static Spell.Active R { get; private set; }
        public static Spell.Targeted Smite { get; private set; }

        public static void Initialize()
        {
        }

        public static bool HasSmite()
        {
            return Smite != null && Smite.IsLearned;
        }
    }
}