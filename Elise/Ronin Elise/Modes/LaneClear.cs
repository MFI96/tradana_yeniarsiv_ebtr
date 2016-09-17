using Mario_s_Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu.Values;
using static RoninElise.Menus;
using static RoninElise.SpellsManager;

namespace RoninElise.Modes
{
    /// <summary>
    /// This mode will run when the key of the orbwalker is pressed
    /// </summary>
    internal class LaneClear
    {
        /// <summary>
        /// Put in here what you want to do when the mode is running
        /// </summary>
        public static void Execute()
        {
            if (LaneClearMenu.GetCheckBoxValue("quse"))
            Q.TryToCast(Q.GetLastHitMinion(), LaneClearMenu);
        }
    }
}