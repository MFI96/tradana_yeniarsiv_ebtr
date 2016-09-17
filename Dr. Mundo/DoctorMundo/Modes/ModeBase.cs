using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;

namespace DrMundo.Modes
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
        protected Spell.Targeted Ignite
        {
            get { return SpellManager.Ignite; }
        }

        protected bool HasIgnite
        {
            get { return SpellManager.HasIgnite();  }
        }

        protected bool WActive
        {
            get { return Player.Instance.Spellbook.GetSpell(SpellSlot.W).ToggleState == 2; }
        }
        protected float PlayerHealth
        {
            get { return Player.Instance.HealthPercent; }
        }

        protected AIHeroClient _Player
        {
            get { return Player.Instance; }
        }

        protected Vector3 _PlayerPos
        {
            get { return Player.Instance.Position; }
        }

        protected bool PlayerIsUnderEnemyTurret()
        {
            var player = Player.Instance;
            return ObjectManager.Get<Obj_AI_Turret>().Any(turret => turret.Team != _Player.Team && turret.Health > 0 && turret.Distance(_Player) < 1000);
        }

        public abstract bool ShouldBeExecuted();

        public abstract void Execute();
    }
}
