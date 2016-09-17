using EloBuddy;
using EloBuddy.SDK;

namespace Maokai.Modes
{
    public abstract class ModeBase
    {
        protected Spell.Skillshot Q
        {
            get { return SpellManager.Q; }
        }

        protected Spell.Targeted W
        {
            get { return SpellManager.W; }
        }

        protected Spell.Skillshot E
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

        protected int ManaPercent
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