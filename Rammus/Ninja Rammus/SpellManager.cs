using EloBuddy;
using EloBuddy.SDK;
using System.Linq;
using System;

namespace Rammus
{
    public static class SpellManager
    {
        public static Spell.Active Q { get; private set; }
        public static Spell.Active W { get; private set; }
        public static Spell.Targeted E { get; private set; }
        public static Spell.Active R { get; private set; }

        public static Spell.Targeted Smite { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Active(SpellSlot.Q, 200);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Targeted(SpellSlot.E, 325);
            R = new Spell.Active(SpellSlot.R, 300);

            //VodkaSmite

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
