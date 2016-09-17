using EloBuddy;
using EloBuddy.SDK;

namespace Warwick
{
    class Damages
    {
        private static AIHeroClient _Player
        {
            get { return Player.Instance; }
        }

        private static float PlayerAP
        {
            get { return Player.Instance.TotalMagicalDamage; }
        }

        private static float PlayerAD
        {
            get { return Player.Instance.TotalAttackDamage; }
        }

        private static float PlayerBonusAD
        {
            get { return Player.Instance.TotalAttackDamage - Player.Instance.BaseAttackDamage; }
        }

        public static float QRawDamage(Obj_AI_Base target)
        {
            var flatDmg = (new[] { 75.0f, 125.0f, 175.0f, 225.0f, 275.0f }[SpellManager.Q.Level - 1]) + PlayerAP;
            if (target is AIHeroClient)
            {
                var percentageDmg = (new[] { 0.08f, 0.1f, 0.12f, 0.14f, 0.16f }[SpellManager.Q.Level - 1]) *
                                    target.MaxHealth + PlayerAP;
                return percentageDmg > flatDmg ? percentageDmg : flatDmg;
            }
            return flatDmg;
        }

        public static float QDamage(Obj_AI_Base target)
        {
            return Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical, QRawDamage(target)) *
                   (Player.Instance.HasBuff("SummonerExhaustSlow") ? 0.6f : 1);
        }

        public static float RRawDamage(Obj_AI_Base target)
        {
            var baseDmg = (new[] { 150.0f, 250.0f, 350.0f }[SpellManager.R.Level - 1]) + 2 * PlayerBonusAD;
            // TODO Include on-hit effects in dmg calculations
            return baseDmg;
        }

        public static float RDamage(Obj_AI_Base target)
        {
            return Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical, RRawDamage(target)) *
                   (Player.Instance.HasBuff("SummonerExhaustSlow") ? 0.6f : 1);
        }

        public static float IgniteDmg(Obj_AI_Base target)
        {
            return Player.Instance.GetSummonerSpellDamage(target, DamageLibrary.SummonerSpells.Ignite);
        }

        public static float SmiteDmgHero(AIHeroClient target)
        {
            return Player.Instance.CalculateDamageOnUnit(target, DamageType.True,
                20.0f + Player.Instance.Level * 8.0f);
        }
    }
}