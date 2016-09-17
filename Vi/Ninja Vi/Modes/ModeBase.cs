using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Constants;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;

namespace Vi.Modes
{
    public abstract class ModeBase
    {

        protected Spell.Chargeable Q
        {
            get { return SpellManager.Q; }
        }
        protected Spell.Active W
        {
            get { return SpellManager.W; }
        }
        protected Spell.Active E
        {
            get { return SpellManager.E; }
        }

        protected Spell.Skillshot E2
        {
            get { return SpellManager.E2; }
        }

        protected Spell.Targeted R
        {
            get { return SpellManager.R; }
        }

        protected Spell.Targeted Smite
        {
            get { return SpellManager.Smite; }
        }

        protected Spell.Skillshot Flash
        {
            get { return SpellManager.Flash; }
        }

        protected bool HasSmite
        {
            get { return SpellManager.HasSmite(); }
        }

        protected bool HasFlash
        {
            get { return SpellManager.HasFlash(); }
        }
        protected bool ChargingQ()
        {
            return Player.Instance.Spellbook.IsCharging || Player.Instance.HasBuff("Vault Breaker");
        }

        protected bool PotionRunning()
        {
            return Player.Instance.HasBuff("RegenerationPotion") || Player.Instance.HasBuff("ItemCrystalFlaskJungle") || Player.Instance.HasBuff("ItemMiniRegenPotion") || Player.Instance.HasBuff("ItemCrystalFlask") || Player.Instance.HasBuff("ItemDarkCrystalFlask");
        }

        public static void FlashQ()
        {
            EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
            if (SpellManager.Q.IsReady() && SpellManager.Flash.IsReady())
            {
                var target2 = TargetSelector.SelectedTarget;
                var range = SpellManager.Q.MaximumRange + SpellManager.Flash.Range;


                if (target2 != null && target2.IsValidTarget(SpellManager.Q.MaximumRange + SpellManager.Flash.Range))
                {
                    SpellManager.Q.StartCharging();

                    Core.DelayAction(delegate()
                    {
                        if (SpellManager.Q.IsCharging && SpellManager.Q.IsFullyCharged && target2.IsValidTarget(SpellManager.Q.MaximumRange)) SpellManager.Q.Cast(target2.Position);
                    }, 1250);


                    Core.DelayAction(delegate()
                    {
                        if (target2.IsValidTarget(SpellManager.Q.MaximumRange)) SpellManager.Flash.Cast(target2.Position);
                    }, 1251);
                    

                }
            }
            return;
        }


        public static void ForceR()
        {
            foreach (var enemy in EntityManager.Heroes.Enemies.Where(enemy => enemy.IsValidTarget(SpellManager.R.Range) && !enemy.HasBuffOfType(BuffType.SpellShield) && !enemy.HasBuffOfType(BuffType.Invulnerability)).OrderByDescending(TargetSelector.GetPriority))
            {
                if (enemy != null)
                {
                    SpellManager.R.Cast(enemy);
                    return;
                }
            }
        }

        public static void GankButton()
        {
            EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);

            var target = TargetSelector.GetTarget(SpellManager.Q.MaximumRange + 300, DamageType.Physical);
            if (SpellManager.Q.IsReady() && target != null)
            {
                if (SpellManager.Q.IsCharging)
                {
                    SpellManager.Q.Cast(target);
                    return;
                }
                else
                {
                    SpellManager.Q.StartCharging();
                    return;
                }
            }
        }
            

        

        public abstract bool ShouldBeExecuted();

        public abstract void Execute();
    }
}
