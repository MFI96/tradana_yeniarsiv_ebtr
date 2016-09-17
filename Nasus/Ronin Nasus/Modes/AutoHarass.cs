using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Constants;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using Mario_s_Lib;
using static Ronin.Menus;
using static Ronin.SpellsManager;

namespace Ronin.Modes
{
    /// <summary>
    /// This mode will always run
    /// </summary>
    internal class AutoHarass
    {
        /// <summary>
        /// Put in here what you want to do when the mode is running
        /// </summary>
        public static void Execute()
        {
            var target = TargetSelector.GetTarget(1000, DamageType.Mixed);

            Q.TryToCast(target, AutoHarassMenu);
            W.TryToCast(target, AutoHarassMenu);
            E.TryToCast(target, AutoHarassMenu);
            R.TryToCast(target, AutoHarassMenu);
        }
    }
}