using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace JusticeTalon
{
    public static class Damages
    {
        #region Damages

        public static float GetDamage(SpellSlot slot, Obj_AI_Base target)
        {
            var lvl = Player.Instance.Spellbook.GetSpell(slot).Level - 1;
            var AD = Player.Instance.FlatPhysicalDamageMod;
            var TotalAD = Player.Instance.TotalAttackDamage;
            var AP = Player.Instance.FlatMagicDamageMod;
            var dmg = 0f;
            var ebendmg = 0f;
            var kaynindmg = 0f;
            var rektdmg = 0f;

            switch (slot)
            {
                case SpellSlot.Q:
                    if (SpellManager.Q.IsReady())
                    {
                        dmg += new float[] { 30, 60, 90, 120, 150 }[lvl] + 0.3f * AD;
                    }
                    break;
                case SpellSlot.W:
                    if (SpellManager.W.IsReady())
                    {
                        dmg += new float[] { 60, 110, 160, 210, 270 }[lvl] + 0.6f * AD;
                    }
                    break;
                case SpellSlot.E:
                    if (SpellManager.E.IsReady())
                    {
                        dmg += new float[] { 3, 6, 9, 12, 15 }[lvl];
                    }
                    break;
                case SpellSlot.R:
                    if (SpellManager.R.IsReady())
                    {
                        dmg += new float[] { 240, 340, 440 }[lvl] + 0.9f * AD;
                    }
                    break;
            }
            return Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical, dmg - 10);
        }
        //+ Player.Instance.GetAutoAttackDamage(target)
        public static float GetTotalDamage(Obj_AI_Base target)
        {
            var tomlamskilldmg = GetDamage(SpellSlot.Q, target) + GetDamage(SpellSlot.W, target) + GetDamage(SpellSlot.R, target) ;
            var ebendmg = tomlamskilldmg / 100;
            var kaynindmg = ebendmg*GetDamage(SpellSlot.E, target);
            var rektdmg = kaynindmg + tomlamskilldmg + Player.Instance.GetAutoAttackDamage(target) + Player.Instance.GetAutoAttackDamage(target);
            var dmg = rektdmg;
            return dmg;
        }

        #endregion Damages
    }
}