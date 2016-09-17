using EloBuddy;
using EloBuddy.SDK;
using System;
using System.Linq;

namespace Warwick
{
    public static class SpellManager
    {
        public static Spell.Targeted Q { get; private set; }
        public static Spell.Active W { get; private set; }
        public static Spell.Active E { get; private set; }
        public static Spell.Targeted R { get; private set; }
        public static Spell.Targeted Ignite { get; private set; }
        public static Spell.Targeted Smite { get; private set; }
        public static Spell.Active Recall { get; private set; }
        private static AIHeroClient _Player
        {
            get { return Player.Instance; }
        }

        static SpellManager()
        {
            // Initialize spells
            Q = new Spell.Targeted(SpellSlot.Q, 400);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Active(SpellSlot.E, 1500);
            R = new Spell.Targeted(SpellSlot.R, 700);

            Recall = new Spell.Active(SpellSlot.Recall);

            if (Player.Instance.Spellbook.GetSpell(SpellSlot.Summoner1).Name.Equals("summonerdot", StringComparison.CurrentCultureIgnoreCase))
            {
                Ignite = new Spell.Targeted(SpellSlot.Summoner1, 600);
            }
            else if ((Player.Instance.Spellbook.GetSpell(SpellSlot.Summoner2).Name.Equals("summonerdot", StringComparison.CurrentCultureIgnoreCase)))
            {
                Ignite = new Spell.Targeted(SpellSlot.Summoner2, 600);
            }
            if (Util.SmiteNames.ToList().Contains(Player.Instance.Spellbook.GetSpell(SpellSlot.Summoner1).Name))
            {
                Smite = new Spell.Targeted(SpellSlot.Summoner1, 570);
            }
            else if (Util.SmiteNames.ToList().Contains(Player.Instance.Spellbook.GetSpell(SpellSlot.Summoner2).Name))
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
            return Smite != null;
        }

        public static bool HasChillingSmite()
        {

            return Smite != null &&
                   Smite.Name.Equals("s5_summonersmiteplayerganker", StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool HasChallengingSmite()
        {
            return Smite != null &&
                   Smite.Name.Equals("s5_summonersmiteduel", StringComparison.CurrentCultureIgnoreCase);
        }

        public static float ERange()
        {
            return (new[] { 1500.0f, 2300.0f, 3100.0f, 3900.0f, 4700.0f }[SpellManager.E.Level - 1]);
        }
    }
}
