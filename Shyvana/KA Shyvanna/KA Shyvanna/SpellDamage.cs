using EloBuddy;
using EloBuddy.SDK;

namespace KA_Shyvanna
{
    public static class SpellDamage
    {
        public static float GetTotalDamage(AIHeroClient target)
        {
            // Auto attack
            var damage = Player.Instance.GetAutoAttackDamage(target);

            // Q
            if (SpellManager.Q.IsReady())
            {
                damage += SpellManager.Q.GetRealDamage(target);
            }

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

                    damage = Player.Instance.GetAutoAttackDamage(target) +  new [] { 0.80f, 0.85f, 0.90f, 0.95f, 1f }[spellLevel] * Player.Instance.FlatPhysicalDamageMod;
                    break;

                case SpellSlot.W:

                    damage = new float[] { 0, 0, 0, 0, 0 }[spellLevel] + 0.0f * Player.Instance.FlatMagicDamageMod;
                    break;

                case SpellSlot.E:

                    damage = new float[] { 60, 100, 140, 180, 220 }[spellLevel] + 0.6f * Player.Instance.FlatMagicDamageMod;
                    break;

                case SpellSlot.R:

                    damage = new float[] { 175, 300, 425 }[spellLevel] + 0.7f * Player.Instance.FlatMagicDamageMod;
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