using EloBuddy;
using EloBuddy.SDK;
using Settings = Zac.Config.Combo.ComboMenu;
using Settings2 = Zac.Config.Misc.MiscMenu;

namespace Zac.Modes
{
    public abstract class ModeBase
    {
        protected Spell.Skillshot Q
        {
            get { return SpellManager.Q; }
        }

        protected Spell.Active W
        {
            get { return SpellManager.W; }
        }

        protected Spell.Chargeable E
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