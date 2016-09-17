using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace Black_Swan_Akali
{
    public static class Spells
    {
        // Define and Initialize Spells
        public static Spell.Targeted Q = new Spell.Targeted(SpellSlot.Q, 600);
        public static Spell.Active W = new Spell.Active(SpellSlot.W, 700);
        public static Spell.Active E = new Spell.Active(SpellSlot.E, 325);
        public static Spell.Targeted R = new Spell.Targeted(SpellSlot.R, 700);
    }
}
