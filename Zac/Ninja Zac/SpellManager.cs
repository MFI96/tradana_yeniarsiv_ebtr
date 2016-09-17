using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace Zac
{
    public static class SpellManager
    {
        public static int[] EMaxRanges = {1150, 1300, 1450, 1600, 1750};

        public static int[] EMaxChannelTimes = {900, 1000, 1100, 1200, 1300};

        static SpellManager()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 550, SkillShotType.Linear, 500, int.MaxValue, 120);
            W = new Spell.Active(SpellSlot.W, 350);
            E = new Spell.Chargeable(SpellSlot.E, 0, 1750, 1500, 500, 1500, 250);
            R = new Spell.Active(SpellSlot.R, 300);

            //E = new Spell.Chargeable(SpellSlot.E, 250, 1750, (int)EMaxChannelTimes[E.Level - 1], 0, 1500, 300);

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
        public static Spell.Active W { get; private set; }
        public static Spell.Chargeable E { get; private set; }
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