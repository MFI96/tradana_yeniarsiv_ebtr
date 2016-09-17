using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace Farofakids_Heimer
{
    public static class SpellManager
    {
        public static Spell.Skillshot Q { get; private set; }
        public static Spell.Skillshot W { get; private set; }
        public static Spell.Skillshot E { get; private set; }
        public static Spell.Active R { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 350, SkillShotType.Linear, (int)0.5f, 1450, (int)40f);
            W = new Spell.Skillshot(SpellSlot.W, 1325, SkillShotType.Cone, (int)0.5f, 902, 200);
            E = new Spell.Skillshot(SpellSlot.E, 970, SkillShotType.Circular, (int)0.5f, 2500, 120);
            R = new Spell.Active(SpellSlot.R, 350);
        }

        public static void Initialize()
        {
        }
    }
}
