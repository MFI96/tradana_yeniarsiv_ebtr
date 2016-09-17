using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Enumerations;
using SharpDX;
using RekSai.Modes;

using Settings = RekSai.Config.Combo.ComboMenu;
using Settings2 = RekSai.Config.JungleClear.JungleClearMenu;
using Settings3 = RekSai.Config.Misc.MiscMenu;
using Settings4 = RekSai.Config.Draw.DrawMenu;

namespace RekSai
{
    class Events
    {

        public static bool burrowed = false;

        static Events()
        {
            
            
            Orbwalker.OnPostAttack += OnAfterAttack;
            Drawing.OnDraw += OnDraw;
            Player.OnBuffLose += Player_OnBuffLose;
            Player.OnBuffGain += Player_OnBuffGain;
        }

        private static void Player_OnBuffGain(Obj_AI_Base sender, Obj_AI_BaseBuffGainEventArgs args)
        {
            if (!sender.IsMe) return;
            if (sender.IsMe && args.Buff.Name == "RekSaiW")
            {
                burrowed = true;
                Orbwalker.DisableAttacking = true;
            }
        }

        private static void Player_OnBuffLose(Obj_AI_Base sender, Obj_AI_BaseBuffLoseEventArgs args)
        {
            if (!sender.IsMe) return;
            if (sender.IsMe && args.Buff.Name == "RekSaiW")
            {
                burrowed = false;
                Orbwalker.DisableAttacking = false;
            }
        }


        public static void OnAfterAttack(AttackableUnit target, EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                if (Settings.ItemUsage && Item.HasItem(3074) && Item.CanUseItem(3074) || Settings.ItemUsage && Item.HasItem(3077) && Item.CanUseItem(3077) || Settings.ItemUsage && Item.HasItem(3748) && Item.CanUseItem(3748))
                {
                    ItemsManager.HailHydra();
                    if (Settings.UseQ && SpellManager.Q.IsReady())
                    {
                        SpellManager.Q.Cast();
                        return;
                    }
                }

                else if (Settings.UseQ && SpellManager.Q.IsReady())
                {
                    SpellManager.Q.Cast();
                    return;
                }
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {

                if (Settings3.ItemUsage && Item.HasItem(3074) && Item.CanUseItem(3074) || Settings3.ItemUsage && Item.HasItem(3077) && Item.CanUseItem(3077) || Settings3.ItemUsage && Item.HasItem(3748) && Item.CanUseItem(3748))
                {
                    ItemsManager.HailHydra();
                    if (Settings2.UseQ && SpellManager.Q.IsReady())
                    {
                        SpellManager.Q.Cast();
                        return;
                    }
                }
                
                else if (Settings2.UseQ && SpellManager.Q.IsReady())
                {
                    SpellManager.Q.Cast();
                    return;
                }
            }
            
        }

        public static void KS()
        {
            var targetKSQ2 = TargetSelector.GetTarget(SpellManager.Q2.Range, DamageType.Magical);
            
            if (targetKSQ2 != null && burrowed && Settings3.EnableKS && SpellManager.Q2.IsReady())
            {
                var predQ2 = SpellManager.Q2.GetPrediction(targetKSQ2);
                if (predQ2.HitChance >= HitChance.High && targetKSQ2.Health < Player.Instance.GetSpellDamage(targetKSQ2, SpellSlot.Q))
                {
                    SpellManager.Q2.Cast(predQ2.CastPosition);
                    return;
                }
            }
        }


        

        private static void OnDraw(EventArgs args)
        {
            if (Settings4.DrawQ && SpellManager.Q.IsLearned)
            {
                Circle.Draw(Color.Green, SpellManager.Q.Range, Player.Instance.Position);
            }

            if (Settings4.DrawQ2 && SpellManager.Q.IsLearned)
            {
                Circle.Draw(Color.Green, SpellManager.Q2.Range, Player.Instance.Position);
            }

            if (Settings4.DrawE && SpellManager.E.IsLearned)
            {
                Circle.Draw(Color.Green, SpellManager.E.Range, Player.Instance.Position);
            }

            if (Settings4.DrawE2 && SpellManager.E.IsLearned)
            {
                Circle.Draw(Color.Green, SpellManager.E2.Range, Player.Instance.Position);
            }

            if (SpellManager.HasSmite())
            {
                if (Settings4.DrawSmite && Config.Smite.SmiteMenu.SmiteToggle
                    || Settings4.DrawSmite && Config.Smite.SmiteMenu.SmiteCombo
                    || Settings4.DrawSmite && Config.Smite.SmiteMenu.SmiteEnemies)
                {
                    Circle.Draw(Color.Blue, SpellManager.Smite.Range, Player.Instance.Position);
                }
            }
        }
        public static void Initialize()
        {

        }

    }
}
