using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using System.Collections.Generic;
using System.Linq;

namespace LightAmumu.Carry
{
    public sealed class JungleClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear);
        }

        public override void Execute()
        {
            int monsters = EntityManager.MinionsAndMonsters.Monsters.Where(monster => monster.IsValidTarget(W.Range * 2)).Count();
            if (monsters != 0)
            {
                if (MenuList.Farm.WithQ)
                {
                    var targetmonster = EntityManager.MinionsAndMonsters.Monsters.Where(monster => monster.IsValidTarget(Q.Range));
                    Q.Cast(targetmonster.FirstOrDefault());
                }
                if (MenuList.Farm.WithW)
                    Damage.WEnable();

                if (MenuList.Farm.WithE)
                {
                    var targetmonster = EntityManager.MinionsAndMonsters.Monsters;
                    foreach (var select in targetmonster)
                    {
                        if (select.IsValidTarget(E.Range))
                            E.Cast(); return;
                    }
                }
            }
        }
    }
}