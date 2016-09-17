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
using EloBuddy.SDK.Menu;
using static RoninSkarner.Menus;
using static RoninSkarner.SpellsManager;

//using Settings = RoninTune.Modes.Flee

namespace RoninSkarner.Modes
{
    internal class Flee
    {
        public static readonly AIHeroClient Player = ObjectManager.Player;
        public static void Execute()
        {
            if (W.IsReady() && Q.IsReady())
            {
                Q.Cast();
                W.Cast();
            }
        }
    }
}



