using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using EloBuddy;
using EloBuddy.SDK;

using Settings = AddonTemplate.Config.Modes.Clear;

namespace AddonTemplate.Modes
{
    public sealed class JungleClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear);
        }

        public override void Execute()
        {
            if (Settings.eBigJungle)
            {
                var monstersBuff = new List<String>();
                monstersBuff.Add("SRU_Gromp");
                monstersBuff.Add("SRU_Krug");
                monstersBuff.Add("SRU_Murkwolf");
                monstersBuff.Add("SRU_Razorbeak");
                monstersBuff.Add("Sru_Crab");
                monstersBuff.Add("SRU_Red");
                monstersBuff.Add("SRU_Blue");

                List<Obj_AI_Minion> jungleBuffs = EntityManager.MinionsAndMonsters.GetJungleMonsters(null, Int32.MaxValue, true).ToList();

                foreach (Obj_AI_Minion mob in jungleBuffs)
                {
                    foreach (string name in monstersBuff)
                    {
                        if (Regex.IsMatch(mob.Name, name + "[0-9.]*$") && mob.Health < DamageHelper.GetEDamage(mob))
                        {
                            E.Cast();
                        }
                    }
                }
            }
        }
    }
}
