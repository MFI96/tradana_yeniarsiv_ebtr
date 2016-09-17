using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace ProtectorLeona
{
    internal class SpellManager
    {
        public static Spell.Active Q { get; set; }
        public static Spell.Active W { get; set; }
        public static Spell.Skillshot E { get; set; }
        public static Spell.Skillshot R { get; set; }

        // Clone Character Object
        public static AIHeroClient Champion = Program.Champion;

        public static void Initialize()
        {
            // Initialize spells
            Q = new Spell.Active(SpellSlot.Q, (uint) (Champion.GetAutoAttackRange() + 30));
            W = new Spell.Active(SpellSlot.W, 275);
            E = new Spell.Skillshot(SpellSlot.E, 875, SkillShotType.Linear, 250, 2000, 70)
            {
                MinimumHitChance = HitChance.High,
                AllowedCollisionCount = int.MaxValue
            };
            R = new Spell.Skillshot(SpellSlot.R, 1200, SkillShotType.Circular, 1000, int.MaxValue, 250)
            {
                MinimumHitChance = HitChance.High,
                AllowedCollisionCount = int.MaxValue
            };
        }

        public static void ConfigSpells(EventArgs args)
        {
            Q = new Spell.Active(SpellSlot.Q, (uint)(Champion.GetAutoAttackRange() + 30));
        }

        // Champion Specified Abilities
        public static float QDamage()
        {
            return new float[] {0, 40, 70, 100, 130, 160}[Q.Level]
                   + 0.3f*Champion.FlatMagicDamageMod;
        }

        public static float QBonus(Obj_AI_Base target)
        {
            return QDamage() + Champion.GetAutoAttackDamage(target);
        }

        public static float WDamage()
        {
            return new float[] { 0, 60, 110, 160, 210, 260 }[W.Level]
                + 0.4f * Champion.FlatMagicDamageMod;
        }

        public static float EDamage()
        {
            return new float[] { 0, 60, 100, 140, 180, 220 }[E.Level]
                + 0.4f * Champion.FlatMagicDamageMod;
        }

        public static float RDamage()
        {
            return new float[] { 0, 150, 250, 350 }[R.Level]
                + 0.8f * Champion.FlatMagicDamageMod;
        }

        // Cast Methods
        public static void CastQ(Obj_AI_Base target)
        {
            if (target == null) return;
            if (Q.IsReady())
                Q.Cast();
        }

        public static void CastW(Obj_AI_Base target)
        {
            if (target == null) return;
            if (W.IsReady())
                W.Cast();
        }

        public static void CastE(Obj_AI_Base target)
        {
            if (target == null) return;
            if (E.IsReady())
                E.Cast(target);
        }

        public static void CastR(Obj_AI_Base target)
        {
            if (target == null) return;
            if (R.IsReady())
                R.Cast(target);
        }
    }
}
