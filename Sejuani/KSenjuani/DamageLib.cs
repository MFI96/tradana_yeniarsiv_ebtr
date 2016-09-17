using EloBuddy;
using EloBuddy.SDK;

namespace KSejuani
{
  internal class DamageLib
    {
     
            private static readonly AIHeroClient _Player = ObjectManager.Player;
        public static float QCalc(Obj_AI_Base target)
        {
            return _Player.CalculateDamageOnUnit(target, DamageType.Magical,
                (float)(new[] { 0, 80, 125, 170, 215, 260 }[Program.Q.Level] + 0.4f * _Player.FlatMagicDamageMod
                    ));
        }

        public static float WCalc(Obj_AI_Base target)
        {
         var onePercent = (target.HealthPercent/100);
            return _Player.CalculateDamageOnUnit(target, DamageType.Magical,
                (float)(new[] { 0, (4* onePercent), (4.5* onePercent) , (5* onePercent), (5.5* onePercent), (6* onePercent) }[Program.W.Level] ));
        }

        public static float ECalc(Obj_AI_Base target)
        {
            return _Player.CalculateDamageOnUnit(target, DamageType.Magical,
                (float)(new[] { 0, 80, 105, 130, 155, 180 }[Program.E.Level] + 0.5f * _Player.FlatMagicDamageMod
                    ));
        }

        public static float RCalc(Obj_AI_Base target)
        {
            return _Player.CalculateDamageOnUnit(target, DamageType.Magical,
                (float)(new[] { 0, 150, 250, 350 }[Program.R.Level] + 0.8f * _Player.FlatMagicDamageMod
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

