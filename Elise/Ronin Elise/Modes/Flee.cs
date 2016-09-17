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
using static RoninElise.Menus;
using static RoninElise.SpellsManager;

//using Settings = RoninTune.Modes.Flee

namespace RoninElise.Modes
{
    internal class Flee
    {
        public static readonly AIHeroClient Player = ObjectManager.Player;
        public static void Execute()
        {
            var target = TargetSelector.GetTarget(1100, DamageType.Magical);
            if (E.IsReady())
            {
                E.Cast(target);
            }
            else if (E2.IsReady())
            {
                E2.Cast();
            }
        }
    }
}



