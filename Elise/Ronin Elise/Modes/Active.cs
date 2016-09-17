using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu.Values;
using Mario_s_Lib;
using static RoninElise.Menus;
using static RoninElise.SpellsManager;
namespace RoninElise.Modes
{
    /// <summary>
    /// This mode will always run
    /// </summary>
    internal class Active
    {
        /// <summary>
        /// Put in here what you want to do when the mode is running
        /// </summary>
        public static void Execute()
        {

            var target = TargetSelector.GetTarget(1100, DamageType.Magical);

            if (MiscMenu.GetCheckBoxValue("eGap") && (Player.Instance.HealthPercent <= 60))
            {
                    E.Cast(target);
              }
            //{
            //    Spells[0].Cast(Player.Instance.Position);
            //}
        }
    }
    }
