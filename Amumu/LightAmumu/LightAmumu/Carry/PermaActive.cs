using EloBuddy;
using EloBuddy.SDK;
using System.Linq;

namespace LightAmumu.Carry
{
    public sealed class PermaActive : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return true;
        }

        public override void Execute()
        {
            //W autodisable
            if (MenuList.Misc.smartW && Damage.WStatus())
            {
                int monsters = EntityManager.MinionsAndMonsters.CombinedAttackable.Where(monster => monster.IsValidTarget(W.Range * 2)).Count();
                int enemies = EntityManager.Heroes.Enemies.Where(enemy => enemy.IsValidTarget(W.Range * 2)).Count();
                if (monsters == 0 && enemies == 0)
                    Damage.WDisable();
            }
            ////
        }
    }
}