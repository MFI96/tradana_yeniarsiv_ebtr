using EloBuddy;
using EloBuddy.SDK;

namespace NinjaNunu
{
    internal class Damage
    {
        //Fluxy - Lee Sin
        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }
        public static double QDamage(Obj_AI_Base target)
        {
            if (!Player.GetSpell(SpellSlot.Q).IsLearned) return 0;
            return _Player.CalculateDamageOnUnit(target, DamageType.True,
                (float)(new double[] { 400, 550, 700, 850, 1000 }[SpellManager.Q.Level - 1]));
        }

        public static double EDamage(Obj_AI_Base target)
        {
            if (!Player.GetSpell(SpellSlot.E).IsLearned) return 0;
            return _Player.CalculateDamageOnUnit(target, DamageType.Magical,
                (float)new double[] { 85, 130, 175, 225, 275 }[SpellManager.E.Level - 1] + 1 * _Player.FlatMagicDamageMod);
        }

        public static float IgniteDmg(Obj_AI_Base target)
        {
            return Player.Instance.GetSummonerSpellDamage(target, DamageLibrary.SummonerSpells.Ignite);
        }

        public static void Initialize()
        {
        }
    }
}