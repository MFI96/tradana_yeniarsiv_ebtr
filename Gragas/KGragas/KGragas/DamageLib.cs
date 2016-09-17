using EloBuddy;
using EloBuddy.SDK;

namespace KGragas
{
    internal class DamageLib
    {

        private static readonly AIHeroClient _Player = ObjectManager.Player;
        public static float QCalc(Obj_AI_Base target)
        {
            return _Player.CalculateDamageOnUnit(target, DamageType.Magical,
                (float)(new[] { 0, 80, 120, 160, 200, 240 }[Program.Q.Level] + 0.6f * _Player.FlatMagicDamageMod
                    ));
        }

        public static float WCalc(Obj_AI_Base target)
        {
            return _Player.CalculateDamageOnUnit(target, DamageType.Magical,
                (float)(new[] { 0, 20, 50, 80, 110, 140 }[Program.W.Level] + 0.3f * _Player.FlatMagicDamageMod + 0.08f * target.HealthPercent
                    ));
        }

        public static float ECalc(Obj_AI_Base target)
        {
            return _Player.CalculateDamageOnUnit(target, DamageType.Magical,
                (float)(new[] { 0, 80, 130, 180, 230, 280 }[Program.E.Level] + 0.6f * _Player.FlatMagicDamageMod
                    ));
        }

        public static float RCalc(Obj_AI_Base target)
        {
            return _Player.CalculateDamageOnUnit(target, DamageType.Magical,
                (float)(new[] { 0, 200, 300, 400 }[Program.R.Level]  + 0.8f * _Player.FlatMagicDamageMod
                    ));
        }
        public static float DmgCalc(AIHeroClient target)
        {
            var damage = 0f;
            if (Program.Q.IsReady() && target.IsValidTarget(Program.Q.Range))
                damage += QCalc(target);
            if (Program.W.IsReady())
                damage += WCalc(target);
            if (Program.E.IsReady() && target.IsValidTarget(Program.E.Range))
                damage += ECalc(target);
            if (Program.R.IsReady() && target.IsValidTarget(Program.R.Range))
                damage += RCalc(target);
            damage += _Player.GetAutoAttackDamage(target, true) * 2;
            return damage;
        }


    }
}