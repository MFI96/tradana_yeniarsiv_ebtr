using EloBuddy;
using EloBuddy.SDK;

namespace PartyJanna.Modes
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
        protected Spell.Targeted E
        {
            get { return SpellManager.E; }
        }
        protected Spell.Active R
        {
            get { return SpellManager.R; }
        }

        public abstract bool ShouldBeExecuted();

        public abstract void Execute();

        public static AIHeroClient GetTarget(Spell.SpellBase spell, DamageType damageType)
        {
            var target = TargetSelector.GetTarget(spell.Range, damageType, Player.Instance.Position);
            return TargetSelector.SelectedTarget != null && spell.IsInRange(target) ? TargetSelector.SelectedTarget : target != null && spell.IsInRange(target) ? target : null;
        }
    }
}
