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
    internal class JungleClear
    {
        /// <summary>
        /// Put in here what you want to do when the mode is running
        /// </summary>
        public static void Execute()
        {
            var target = EntityManager.MinionsAndMonsters.GetJungleMonsters().OrderByDescending(a => a.MaxHealth).FirstOrDefault(a => a.IsValidTarget(900));
            if (SpellsManager.IsSpider)
            {
                if (JungleClearMenu.GetCheckBoxValue("q2Use") && Q2.IsReady() && target.IsValidTarget(SpellsManager.Q2.Range))
                {
                    Q2.Cast(target);
                }
                if (JungleClearMenu.GetCheckBoxValue("w2Use") && W2.IsReady())
                {
                    W2.Cast();
                }
                if (JungleClearMenu.GetCheckBoxValue("rUse") && R.IsReady() && E.IsReady() && W.IsReady())
                {
                    R.Cast();
                }
            }
            else
            { //HUMAN
                if (JungleClearMenu.GetCheckBoxValue("eUse") && E.IsReady() && target.IsValidTarget(SpellsManager.E.Range) && E.GetPrediction(target).HitChance >= HitChance.Medium)
                {
                    E.Cast(target);
                }
                if (JungleClearMenu.GetCheckBoxValue("qUse") && Q.IsReady() && target.IsValidTarget(SpellsManager.Q.Range))
                {
                    Q.Cast(target);
                }
                if (JungleClearMenu.GetCheckBoxValue("WUse") && W.IsReady() && target.IsValidTarget(W.Range))
                {
                    W.Cast(target);
                }
                if (JungleClearMenu.GetCheckBoxValue("rUse") && R.IsReady())
                {
                    R.Cast();
                }
            }




        }
    }
}