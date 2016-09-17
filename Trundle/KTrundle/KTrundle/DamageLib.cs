using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace KTrundle
{
    internal class DamageLib
    {//damageb
        private static readonly AIHeroClient _Player = ObjectManager.Player;
        public static float QCalc(Obj_AI_Base target)
        {
            return _Player.CalculateDamageOnUnit(target, DamageType.Physical,
                (float)(new[] { 0, 20, 40, 60, 80, 100 }[Program.Q.Level] + 1.2f * _Player.FlatPhysicalDamageMod + 1f * _Player.GetAutoAttackDamage(target)
                    ));
        }
        public static float DmgCalc(AIHeroClient target)
        {
            var damage = 0f;
            if (Program.Q.IsReady() && target.IsValidTarget(Program.Q.Range))
                damage += QCalc(target);
            damage += _Player.GetAutoAttackDamage(target, true) * 2;
            return damage;
        }
    }
}
