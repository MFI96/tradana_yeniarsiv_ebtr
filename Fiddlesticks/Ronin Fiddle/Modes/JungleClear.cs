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
   

    internal class JungleClear
    {
        /// <summary>
        /// Put in here what you want to do when the mode is running
        public static readonly AIHeroClient Player = ObjectManager.Player;
        public static void Execute()
        {
            var source =
                           EntityManager.MinionsAndMonsters.GetJungleMonsters()
                               .OrderBy(a => a.MaxHealth)
                               .FirstOrDefault(a => a.IsValidTarget(Q.Range));

            if (source == null) return;
            if (Q.IsReady() && JungleClearMenu.GetCheckBoxValue("qUse") && source.Distance(Player) <= Q.Range)
            {
                Q.Cast(source);
            }

            if (W.IsReady() && JungleClearMenu.GetCheckBoxValue("wUse") && source.Distance(Player) <= W.Range)
            {
              foreach (var Mob in EntityManager.MinionsAndMonsters.Monsters.Where(x => x.IsValid && !x.IsDead))
                {
                W.Cast(source);
                Orbwalker.DisableAttacking = true;
                Orbwalker.DisableMovement = true;
                Healing = true;
            }
            }


            if (E.IsReady() && JungleClearMenu.GetCheckBoxValue("eUse") && source.Distance(Player) <= E.Range)
            {
                E.Cast(source);
            }
        }
    }
}