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

namespace RoninTune.Modes
{
    internal class KS
    {
        public static readonly AIHeroClient Player = ObjectManager.Player;

        public static void Execute()
        {
            foreach (
              var target in
                  EntityManager.Heroes.Enemies.Where(
                      hero =>
                          hero.IsValidTarget(R.Range) && !hero.IsDead && !hero.IsZombie && hero.HealthPercent <= 25))
            {

                if (KillStealMenu.GetCheckBoxValue("rUse") && R.IsReady() &&
                    target.Health + target.AttackShield < Player.GetSpellDamage(target, SpellSlot.R))
                {
                    R.Cast(target.Position);
                }

                if (KillStealMenu.GetCheckBoxValue("wUse") &&
                    target.Health + target.AttackShield <
                    Player.GetSpellDamage(target, SpellSlot.W) && Player.Mana >= 100)
                {
                    if (W.IsReady() && target.IsValidTarget(W.Range))
                    {
                        W.Cast(target);
                    }
                }

                if (KillStealMenu.GetCheckBoxValue("eUse") && E.IsReady() &&
                    target.Health + target.AttackShield <
                    Player.GetSpellDamage(target, SpellSlot.E))
                {
                    E.Cast(target);
                }
            }
        }
    }
}



