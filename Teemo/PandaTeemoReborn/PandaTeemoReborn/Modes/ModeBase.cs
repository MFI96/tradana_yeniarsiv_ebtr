using EloBuddy.SDK;

namespace PandaTeemoReborn.Modes
{
    public abstract class ModeBase
    {
        protected static Spell.Targeted Q => SpellManager.Q;

        protected static Spell.Active W => SpellManager.W;

        protected static Spell.Skillshot R => SpellManager.R;

        public abstract bool ShouldBeExecuted();

        public abstract void Execute();
    }
}
