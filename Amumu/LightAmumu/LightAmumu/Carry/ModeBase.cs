using EloBuddy.SDK;

namespace LightAmumu.Carry
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

        protected Spell.Active E
        {
            get { return SpellManager.E; }
        }

        protected Spell.Active R
        {
            get { return SpellManager.R; }
        }

        public abstract bool ShouldBeExecuted();

        public abstract void Execute();
    }
}