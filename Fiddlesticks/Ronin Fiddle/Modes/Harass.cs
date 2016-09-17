using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using Color = System.Drawing.Color;
using System;
using System.Linq;
using Mario_s_Lib;
using static RoninFiddle.Menus;
using static RoninFiddle.SpellsManager;

namespace RoninFiddle.Modes
{
    /// <summary>
    /// This mode will run when the key of the orbwalker is pressed
    /// </summary>
    internal class Harass
    {
        /// <summary>
        /// Put in here what you want to do when the mode is running
        public static readonly AIHeroClient Player = ObjectManager.Player;
        public static void Execute()
        {
            var enemiese = EntityManager.Heroes.Enemies.OrderByDescending
                          (a => a.HealthPercent).Where(a => !a.IsMe && a.IsValidTarget() && a.Distance(Player) <= R.Range);
            var enemiesq = EntityManager.Heroes.Enemies.OrderByDescending
                (a => a.HealthPercent).Where(a => !a.IsMe && a.IsValidTarget() && a.Distance(Player) <= Q.Range);
            var target = TargetSelector.GetTarget(1900, DamageType.Magical);
            if (!target.IsValidTarget())
            {
                return;
            }

            if (E.IsReady() && target.IsValidTarget(E.Range))
            {
                foreach (var eenemies in enemiese)
                {
                    var useE = HarassMenu.GetCheckBoxValue("eUse");
                    if (useE)
                    {
                        E.Cast(eenemies);
                    }
                }
            }

            if (target.IsValidTarget(Q.Range))
            {
                foreach (var qenemies in enemiesq)
                {
                    var useQ = HarassMenu.GetCheckBoxValue("qUse");
                    if (useQ)
                    {
                        {
                            Q.Cast(qenemies);
                        }
                    }
                }
            }
        }
    }
}