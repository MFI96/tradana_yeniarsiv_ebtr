using EloBuddy.SDK;
using EloBuddy;

namespace Rammus.Modes
{
    public abstract class ModeBase
    {
        protected Spell.Active Q
        {
            get { return SpellManager.Q; }
        }
        protected Spell.Active W
        {
            get { return SpellManager.W; }
        }
        protected Spell.Targeted E
        {
            get { return SpellManager.E; }
        }
        protected Spell.Active R
        {
            get { return SpellManager.R; }
        }

        protected Spell.Targeted Smite
        {
            get { return SpellManager.Smite; }
        }

        protected bool HasSmite
        {
            get { return SpellManager.HasSmite(); }
        }

        protected bool Spinning()
        {
            return Player.Instance.HasBuff("Powerball");
        }

        protected bool PotionRunning()
        {
            return Player.Instance.HasBuff("RegenerationPotion") || Player.Instance.HasBuff("ItemCrystalFlaskJungle") || Player.Instance.HasBuff("ItemMiniRegenPotion") || Player.Instance.HasBuff("ItemCrystalFlask") || Player.Instance.HasBuff("ItemDarkCrystalFlask");
        }

        public static void GankButton()
        {
            
            EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
            var target = TargetSelector.GetTarget(2000, DamageType.Magical);
            if (Player.Instance.HasBuff("Powerball"))
            {
                return;
            }

            if (target != null)
            {
                SpellManager.Q.Cast();
                return;
            }
        }

        public abstract bool ShouldBeExecuted();

        public abstract void Execute();
    }
}
