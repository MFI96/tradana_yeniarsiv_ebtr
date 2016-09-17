using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Utils;
using System;

namespace LightAmumu
{
    internal class Damage : IDisposable
    {
        public enum SmiteType { Challenging, Chilling };

        public static double GetQDamage(Obj_AI_Base target)
        {
            if (SpellManager.Q.IsReady())
                return Player.Instance.GetSpellDamage(target, SpellSlot.Q, DamageLibrary.SpellStages.Default);
            return 0;
        }

        public static double GetWDamage(Obj_AI_Base target)
        {
            if (SpellManager.W.IsLearned)
                return Player.Instance.GetSpellDamage(target, SpellSlot.W, DamageLibrary.SpellStages.Default);
            return 0;
        }

        public static double GetEDamage(Obj_AI_Base target)
        {
            if (SpellManager.E.IsReady())
                return Player.Instance.GetSpellDamage(target, SpellSlot.E, DamageLibrary.SpellStages.Default);
            return 0;
        }

        public static double GetRDamage(Obj_AI_Base target)
        {
            if (SpellManager.R.IsReady())
                return Player.Instance.GetSpellDamage(target, SpellSlot.R, DamageLibrary.SpellStages.Default);
            return 0;
        }

        public void Dispose()
        {
        }

        public static float GetIgniteDamage()
        {
            return 50 + (20 * Player.Instance.Level);
        }

        public static float GetSmiteDamage(SmiteType smite)
        {
            if (smite == SmiteType.Challenging)
            {
                return 54 + (6 * Player.Instance.Level);
            }
            if (smite == SmiteType.Chilling)
            {
                return 20 + (8 * Player.Instance.Level);
            }
            return 0;
        }

        public static double GetMaxSpellDamage(Obj_AI_Base target)
        {
            double dmg = 0;
            dmg += GetQDamage(target);
            dmg += GetWDamage(target);
            dmg += GetEDamage(target);
            dmg += GetRDamage(target);
            return dmg;
        }

        public static int GetMaxSummonerSpellDamage(Obj_AI_Base target)
        {
            //TODO
            return 0;
        }

        public static bool WStatus()
        {
            if (Player.HasBuff("AuraofDespair"))
            {
                return true;
            }
            return false;
        }

        public static void WEnable()
        {
            if (!WStatus())
                SpellManager.W.Cast();
            return;
        }

        public static void WDisable()
        {
            if (WStatus())
                SpellManager.W.Cast();
            return;
        }
    }
}