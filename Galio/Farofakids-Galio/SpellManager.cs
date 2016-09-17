using System.Linq;
using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;

namespace Farofakids_Galio
{
    public static class SpellManager
    {

        public static Spell.Skillshot Q { get; private set; }
        public static Spell.Targeted W { get; private set; }
        public static Spell.Skillshot E { get; private set; }
        public static Spell.Active R { get; private set; }
        public static Spell.SpellBase[] Spells { get; private set; }
        public static Dictionary<SpellSlot, Color> ColorTranslation { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 940, SkillShotType.Circular, 500, 1300, 120);
            W = new Spell.Targeted(SpellSlot.W, 800);
            E = new Spell.Skillshot(SpellSlot.E, 1180, SkillShotType.Linear, 500, 1200, 140);
            R = new Spell.Active(SpellSlot.R, 575);
            Spells = (new Spell.SpellBase[] { Q, W, E, R }).OrderByDescending(o => o.Range).ToArray();
            ColorTranslation = new Dictionary<SpellSlot, Color>
            {
                { SpellSlot.Q, Color.IndianRed.ToArgb(150) },
                { SpellSlot.W, Color.PaleVioletRed.ToArgb(150) },
                { SpellSlot.E, Color.IndianRed.ToArgb(150) },
                { SpellSlot.R, Color.DarkRed.ToArgb(150) }
            };
        }

        public static Color GetColor(this Spell.SpellBase spell)
        {
            return ColorTranslation.ContainsKey(spell.Slot) ? ColorTranslation[spell.Slot] : Color.Wheat;
        }

        private static Color ToArgb(this Color color, byte a)
        {
            return new ColorBGRA(color.R, color.G, color.B, a);
        }


        public static void Initialize()
        {
            // Let the static initializer do the job, this way we avoid multiple init calls aswell
        }
    }
}
