using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;

namespace Vi
{
    class Damage
    {

        public static float GetDamage(Obj_AI_Base enemy)
        {

            var damage = 0d;

            if (SpellManager.Q.IsReady())
            {
                damage += Player.Instance.GetSpellDamage(enemy, SpellSlot.Q);
            }
            if (SpellManager.E.IsReady())
            {
                damage += Player.Instance.GetSpellDamage(enemy, SpellSlot.E) * SpellManager.E.Handle.Ammo +
                          (float)Player.Instance.GetAutoAttackDamage(enemy);
            }

            if (SpellManager.R.IsReady())
            {
                damage += Player.Instance.GetSpellDamage(enemy, SpellSlot.R);
            }

            return (float)damage;

        }

        public static void Initialize()
        {
        }
    }
}
