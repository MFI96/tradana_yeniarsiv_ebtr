using System;
using System.Linq;
using EloBuddy;


namespace UBKennen
{
    class Damages
    {
        // 75 / 115 / 155 / 195 / 235 (+ 75% AP)
        public static int QDamage(Obj_AI_Base target)
        {
            return
                (int)
                    (new int[] {0, 75, 115, 155, 195, 235 }[Spells.Q.Level] +
                     0.75 * (ObjectManager.Player.TotalMagicalDamage));
        }
        // 65 / 95 / 125 / 155 / 185 (+ 55% AP)
        public static int WDamage(Obj_AI_Base target)
        {
            return
                (int)
                    (new int[] {0, 65, 95, 125, 155, 185 }[Spells.W.Level] +
                     0.55 * (ObjectManager.Player.TotalMagicalDamage));
        }
        //85 / 125 / 165 / 205 / 245 (+ 60% AP)
        public static int EDamage(Obj_AI_Base target)
        {
            return
                (int)
                    (new int[] {0, 85, 125, 165, 205, 245 }[Spells.E.Level] +
                     0.6 * (ObjectManager.Player.TotalMagicalDamage));
        }
        // 300 / 544 / 788 (+1.5 ability power)
        public static int RDamage(Obj_AI_Base target)
        {
            return
                (int)
                    (new int[] {0, 300, 544, 788 }[Spells.R.Level] +
                     1.5 * (ObjectManager.Player.TotalMagicalDamage));
        }   
    }
}
