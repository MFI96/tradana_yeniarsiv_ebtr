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
    internal class Combo
    {
        /// <summary>
        /// Put in here what you want to do when the mode is running
        /// </summary>
        public static readonly AIHeroClient Player = ObjectManager.Player;
        public static void Execute()
        {
            var enemiesq = EntityManager.Heroes.Enemies.OrderByDescending
                 (a => a.HealthPercent).Where(a => !a.IsMe && a.IsValidTarget() && a.Distance(Player) <= Q.Range);
            var enemiesr = EntityManager.Heroes.Enemies.OrderByDescending
                (a => a.HealthPercent).Where(a => !a.IsMe && a.IsValidTarget() && a.Distance(Player) <= R.Range);
            var enemiesw = EntityManager.Heroes.Enemies.OrderByDescending
                (a => a.HealthPercent).Where(a => !a.IsMe && a.IsValidTarget() && a.Distance(Player) <= W.Range);
            var enemies = EntityManager.Heroes.Enemies.OrderByDescending
                (a => a.HealthPercent).Where(a => !a.IsMe && a.IsValidTarget() && a.Distance(Player) <= E.Range);
            var target = TargetSelector.GetTarget(1900, DamageType.Magical);


            if (R.IsReady() && target.IsValidTarget(R.Range) && !target.IsInvulnerable)
                foreach (var ultenemies in enemiesr)
                {
                    var useR = ComboMenu.GetCheckBoxValue("rUse");
                    if (useR)
                    {
                        R.Cast(ultenemies.Position);
                    }
                }

            if (E.IsReady() && target.IsValidTarget(E.Range))
                foreach (var eenemies in enemies)
                {
                    var useE = ComboMenu.GetCheckBoxValue("eUse");
                    if (useE)
                    {
                        E.Cast(eenemies);
                    }
                }

            if (Q.IsReady() && target.IsValidTarget(Q.Range))
            {
                foreach (var qenemies in enemiesq)
                {
                    var useQ = ComboMenu.GetCheckBoxValue("qUse");
                    if (useQ)
                    {
                        Q.Cast(qenemies);
                    }
                }
            }

            if (W.IsReady() && target.IsValidTarget(W.Range))
                foreach (var wenemies in enemies)
                {
                    var usew = ComboMenu.GetCheckBoxValue("wUse");
                    if (usew)
                    {
                        W.Cast(wenemies);
                        
                    }
                    var usedaw = ComboMenu.GetCheckBoxValue("stopatt");
                    if (usedaw)
                    {
                        Orbwalker.DisableAttacking = true;
                        
                    }
                    var usedmw = ComboMenu.GetCheckBoxValue("stopmov");
                    if (usedmw)
                    {
                        Orbwalker.DisableMovement = true;
                        
                    }
                }

        }
    }
}