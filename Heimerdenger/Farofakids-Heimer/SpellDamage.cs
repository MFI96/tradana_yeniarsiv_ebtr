using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;

namespace Farofakids_Heimer
{
    public static class SpellDamage
    {
        public static float GetTotalDamage(AIHeroClient target)
        {
            // Auto attack
            var damage = Player.Instance.GetAutoAttackDamage(target);

            // Q

            damage += SpellManager.Q.GetRealDamage(target);

            // W
            if (SpellManager.W.IsReady())
            {
                damage += SpellManager.W.GetRealDamage(target);
            }

            // E
            if (SpellManager.E.IsReady())
            {
                damage += SpellManager.E.GetRealDamage(target);
            }

            // R
            if (SpellManager.R.IsReady())
            {
                damage += SpellManager.R.GetRealDamage(target);
            }

            return damage;
        }

        public static float GetRealDamage(this Spell.SpellBase spell, Obj_AI_Base target)
        {
            return spell.Slot.GetRealDamage(target);
        }

        public static float GetRealDamage(this SpellSlot slot, Obj_AI_Base target)
        {
            // Helpers
            var spellLevel = Player.Instance.Spellbook.GetSpell(slot).Level;
            const DamageType damageType = DamageType.Magical;
            float damage = 0;

            // Validate spell level
            if (spellLevel == 0)
            {
                return 0;
            }
            spellLevel--;

            switch (slot)
            {
                case SpellSlot.Q:

                    damage = (new float[] { 12, 18, 23, 29, 34 }[spellLevel] + 0.15f * Player.Instance.FlatMagicDamageMod) * 3f + new float[] { 40, 60, 80, 105, 130 }[spellLevel] + 0.55f * Player.Instance.FlatMagicDamageMod;
                    break;

                case SpellSlot.W:

                    damage = new float[] { 60, 90, 120, 150, 180 }[spellLevel] + 0.45f * Player.Instance.FlatMagicDamageMod;
                    break;

                case SpellSlot.E:

                    damage = new float[] { 60, 100, 140, 180, 220 }[spellLevel] + 0.6f * Player.Instance.FlatMagicDamageMod;
                    break;

                case SpellSlot.R:

                    damage = new float[] { 0, 0, 0 }[spellLevel] + 0.0f * Player.Instance.FlatMagicDamageMod;
                    break;
            }

            if (damage <= 0)
            {
                return 0;
            }

            return Player.Instance.CalculateDamageOnUnit(target, damageType, damage) - 10;
        }
    }

}
