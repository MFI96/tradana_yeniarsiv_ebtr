using EloBuddy;
using EloBuddy.SDK;

namespace RekSai.Modes
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

        protected Spell.Skillshot Q2
        {
            get { return SpellManager.Q2; }
        }

        protected Spell.Skillshot E2
        {
            get { return SpellManager.E2; }
        }


        protected Spell.Targeted Smite
        {
            get { return SpellManager.Smite; }
        }

        protected bool HasSmite
        {
            get { return SpellManager.HasSmite(); }
        }

        protected bool PotionRunning()
        {
            return Player.Instance.HasBuff("RegenerationPotion") || Player.Instance.HasBuff("ItemCrystalFlaskJungle") || Player.Instance.HasBuff("ItemMiniRegenPotion") || Player.Instance.HasBuff("ItemCrystalFlask") || Player.Instance.HasBuff("ItemDarkCrystalFlask");
        }

        public abstract bool ShouldBeExecuted();

        public abstract void Execute();
    }
}
