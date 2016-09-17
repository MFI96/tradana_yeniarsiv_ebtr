using EloBuddy;
using EloBuddy.SDK;

namespace RekSai
{
    class SmiteDamage
    {
        //VodkaSmite
        public readonly static string[] MonstersNames =
        {
            "SRU_Blue", "SRU_Gromp", "SRU_Murkwolf", "SRU_Razorbeak",
            "SRU_Red", "SRU_Krug", "SRU_Dragon", "Sru_Crab", "SRU_Baron", "SRU_RiftHerald"
        };

        public static readonly string[] SmiteNames =
        {
            "summonersmite", "s5_summonersmiteplayerganker", "s5_summonersmiteduel"
        };

        private static readonly int[] SmiteRed = { 3715, 1415, 1414, 1413, 1412 };
        private static readonly int[] SmiteBlue = { 3706, 1403, 1402, 1401, 1400 };

        public static float SmiteDmgMonster(Obj_AI_Base target)
        {
            return Player.Instance.GetSummonerSpellDamage(target, DamageLibrary.SummonerSpells.Smite);
        }

        public static float SmiteDmgHero(AIHeroClient target)
        {
            return Player.Instance.CalculateDamageOnUnit(target, DamageType.True,
                20.0f + Player.Instance.Level * 8.0f);
        }

        public static double EDamage(Obj_AI_Base target)
        {
            if (!Player.GetSpell(SpellSlot.E).IsLearned) return 0;
            var damage = Player.Instance.CalculateDamageOnUnit(target, DamageType.True, (float) new double[] { 0.8f, 0.9f, 1, 1.1f, 1.2f }[SpellManager.E.Level - 1] * Player.Instance.TotalAttackDamage);
            return damage * (1 + (Player.Instance.Mana / Player.Instance.MaxMana));
        }

        public static double Q2DamageKS(AIHeroClient target)
        {
            if (!Player.GetSpell(SpellSlot.Q).IsLearned) return 0;
            var damage = Player.Instance.CalculateDamageOnUnit(target, DamageType.True, (float)new double[] { 80, 90, 120, 150, 180 }[SpellManager.Q.Level - 1]  + .7f * Player.Instance.TotalMagicalDamage );
            return damage;           
        }

        public static double Q2DamageLastHit(Obj_AI_Base target)
        {
            if (!Player.GetSpell(SpellSlot.Q).IsLearned) return 0;
            var damage = Player.Instance.CalculateDamageOnUnit(target, DamageType.True, (float)new double[] { 80, 90, 120, 150, 180 }[SpellManager.Q.Level - 1] + .7f * Player.Instance.TotalMagicalDamage);
            return damage;
        }
        public static void Initialize()
        {
        }
    }
}