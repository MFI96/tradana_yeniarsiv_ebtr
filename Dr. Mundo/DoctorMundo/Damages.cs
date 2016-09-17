using EloBuddy;
using EloBuddy.SDK;

namespace DrMundo
{
    class Damages
    {
       public static float QRawDamage(Obj_AI_Base target)
       {
           var minimumDmg = new float[] {80.0f, 130.0f, 180.0f, 230.0f, 280.0f}[SpellManager.Q.Level - 1];
           var percentDmg = new float[] { 0.15f, 0.18f, 0.21f, 0.23f, 0.25f }[SpellManager.Q.Level - 1] * target.Health;
           return percentDmg < minimumDmg ? minimumDmg : percentDmg;
        }

        public static float QDamage(Obj_AI_Base target)
        {
            var calculated = Player.Instance.CalculateDamageOnUnit(target, DamageType.Magical, QRawDamage(target)) *
                   (Player.Instance.HasBuff("SummonerExhaustSlow") ? 0.6f : 1);
            if (target is Obj_AI_Minion)
            {
                var maximumDmg = new float[] { 80.0f, 130.0f, 180.0f, 230.0f, 280.0f }[SpellManager.Q.Level - 1];
                return calculated > maximumDmg ? maximumDmg : calculated;
            }
            return calculated;
        }

        public static float IgniteDmg(Obj_AI_Base target)
        {
            return Player.Instance.GetSummonerSpellDamage(target, DamageLibrary.SummonerSpells.Ignite);
        }
    }
}