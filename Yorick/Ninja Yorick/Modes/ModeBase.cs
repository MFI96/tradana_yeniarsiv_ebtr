using EloBuddy;
using EloBuddy.SDK;

namespace Yorick.Modes
{
    public abstract class ModeBase
    {
        protected Spell.Active Q
        {
            get { return SpellManager.Q; }
        }

        protected Spell.Skillshot W
        {
            get { return SpellManager.W; }
        }

        protected Spell.Targeted E
        {
            get { return SpellManager.E; }
        }

        protected Spell.Targeted R
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

        protected bool HasIgnite
        {
            get { return SpellManager.HasIgnite(); }
        }

        protected int Mana
        {
            get { return (int) Player.Instance.ManaPercent; }
        }

        protected bool PotionRunning()
        {
            return Player.Instance.HasBuff("RegenerationPotion") || Player.Instance.HasBuff("ItemCrystalFlaskJungle") ||
                   Player.Instance.HasBuff("ItemMiniRegenPotion") || Player.Instance.HasBuff("ItemCrystalFlask") ||
                   Player.Instance.HasBuff("ItemDarkCrystalFlask");
        }

        public abstract bool ShouldBeExecuted();

        public abstract void Execute();
    }
}