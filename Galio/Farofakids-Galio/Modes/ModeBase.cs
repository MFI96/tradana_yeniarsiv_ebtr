﻿using EloBuddy;
using EloBuddy.SDK;

namespace Farofakids_Galio.Modes
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
        public abstract bool ShouldBeExecuted();

        public abstract void Execute();
    }
}
