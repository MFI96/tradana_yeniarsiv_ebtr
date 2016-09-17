using System.IO;
using System.Linq;
using System.Media;
using System.Drawing.Text;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using Color = System.Drawing.Color;
using System.Drawing;
using Mario_s_Lib;
using static RoninFiddle.Menus;
using static RoninFiddle.SpellsManager;

//using Settings = RoninTune.Modes.Flee

namespace RoninFiddle.Modes
{
    /// <summary>
    /// This mode will run when the key of the orbwalker is pressed

    internal class Flee
    {
        public static readonly AIHeroClient Player = ObjectManager.Player;

        public static void Execute()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            if (Q.IsReady())
            {
                Q.Cast(target);
            }
        }
    }
}



