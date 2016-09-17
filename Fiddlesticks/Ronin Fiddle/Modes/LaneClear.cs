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
    
    internal class LaneClear
    {
        /// <summary>
        /// Put in here what you want to do when the mode is running
        public static readonly AIHeroClient Player = ObjectManager.Player;
        public static void Execute()
        {
            var count =
              EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.ServerPosition,
                  Player.AttackRange, false).Count();
            var source =
                            EntityManager.MinionsAndMonsters.GetLaneMinions().OrderBy(a => a.MaxHealth).FirstOrDefault(a => a.IsValidTarget(Q.Range));
            if (count == 0) return;
            if (E.IsReady() && LaneClearMenu.GetCheckBoxValue("eUse") && LaneClearMenu.GetCheckBoxValue("eUse"))
            {
                E.Cast(source);
            }

            if (Q.IsReady())
            {
                if (LaneClearMenu.GetCheckBoxValue("qUse") && source.IsValidTarget(Q.Range) &&
                Player.GetSpellDamage(source, SpellSlot.Q) >= source.Health && !source.IsDead)
                {
                    Q.Cast(source);
                }

                if (Q.IsReady() && LaneClearMenu.GetCheckBoxValue("qUse") && source.IsValidTarget(Q.Range))
                {
                    Q.Cast(source);
                }
            }

            if (W.IsReady() && LaneClearMenu.GetCheckBoxValue("wUse"))
            {
                W.Cast(source);
                Orbwalker.DisableAttacking = true;
                Orbwalker.DisableMovement = true;
            }
        }
    }
}