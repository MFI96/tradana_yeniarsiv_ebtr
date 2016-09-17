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
    /// This mode will run when the key of the orbwalker is pressed
    /// </summary>
    internal class Combo
    {
        /// <summary>
        /// Put in here what you want to do when the mode is running
        /// </summary>
        public static void Execute()
        {
            var target = TargetSelector.GetTarget(1100, DamageType.Magical);
            // SPIDER COMBO
            if (SpellsManager.IsSpider)
            {
                if (ComboMenu.GetCheckBoxValue("q2Use") && Q2.IsReady() && target.IsValidTarget(SpellsManager.Q2.Range))
                {
                    Q2.Cast(target);
                }
                if (ComboMenu.GetCheckBoxValue("w2Use") && W2.IsReady())
                {
                    W2.Cast();
                }
                if (ComboMenu.GetCheckBoxValue("e2Use") && E2.IsReady() && target.IsValidTarget(E2.Range))
                {
                    E2.Cast(target);
                }
                if (ComboMenu.GetCheckBoxValue("rUse") && R.IsReady() && E.IsReady() && W.IsReady())
                {
                    R.Cast();
                }
            }
            else
            { //HUMAN
                if (ComboMenu.GetCheckBoxValue("eUse") && E.IsReady() && target.IsValidTarget(SpellsManager.E.Range) && E.GetPrediction(target).HitChance >= HitChance.Medium)
                {
                    E.Cast(target);
                }
                if (ComboMenu.GetCheckBoxValue("qUse") && Q.IsReady() && target.IsValidTarget(SpellsManager.Q.Range))
                {
                    Q.Cast(target);
                }
                if (ComboMenu.GetCheckBoxValue("WUse") && W.IsReady() && target.IsValidTarget(W.Range))
                {
                    W.Cast(target);
                }
                if (ComboMenu.GetCheckBoxValue("rUse") && R.IsReady())
                {
                    R.Cast();
                }
            }


                    if (ComboMenu.GetKeyBindValue("gankcombokey"))
                    {
                        if (ComboMenu.GetCheckBoxValue("eUse") && E.IsReady() && target.IsValidTarget(SpellsManager.E.Range) && E.GetPrediction(target).HitChance >= HitChance.Medium)
                        {
                            E.Cast(target);
                        }
                        if (ComboMenu.GetCheckBoxValue("qUse") && Q.IsReady() && target.IsValidTarget(SpellsManager.Q.Range))
                        {
                            Q.Cast(target);
                        }
                        if (ComboMenu.GetCheckBoxValue("WUse") && W.IsReady() && target.IsValidTarget(W.Range))
                        {
                            W.Cast(target);
                        }
                        if (ComboMenu.GetCheckBoxValue("rUse") && R.IsReady() && Q2.IsReady())
                         {
                            R.Cast();
                         }

                else if (SpellsManager.IsSpider)
                {
                    if (ComboMenu.GetCheckBoxValue("q2Use") && Q2.IsReady() && target.IsValidTarget(SpellsManager.Q2.Range))
                    {
                        Q2.Cast(target);
                    }
                    if (ComboMenu.GetCheckBoxValue("w2Use") && W2.IsReady())
                    {
                        W2.Cast();
                    }
                    if (ComboMenu.GetCheckBoxValue("e2Use") && E2.IsReady() && target.IsValidTarget(E2.Range))
                    {
                        E2.Cast(target);
                    }
                    if (ComboMenu.GetCheckBoxValue("rUse") && R.IsReady() && E.IsReady() && W.IsReady())
                    {
                        R.Cast();
                    }
                }


                    }
                }

            }
        }

