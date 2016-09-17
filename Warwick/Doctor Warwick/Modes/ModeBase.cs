using EloBuddy;
using EloBuddy.SDK;
using SharpDX;
using System.Linq;

namespace Warwick.Modes
{
    public abstract class ModeBase
    {
        protected Spell.Targeted Q
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
        protected Spell.Targeted R
        {
            get { return SpellManager.R; }
        }
        protected Spell.Targeted Ignite
        {
            get { return SpellManager.Ignite; }
        }
        protected Spell.Targeted Smite
        {
            get { return SpellManager.Smite; }
        }

        protected float PlayerHealth
        {
            get { return Player.Instance.HealthPercent; }
        }

        protected float PlayerMana
        {
            get { return Player.Instance.ManaPercent; }
        }

        protected float PlayerManaExact
        {
            get { return Player.Instance.Mana; }
        }

        protected bool HasIgnite
        {
            get { return SpellManager.HasIgnite(); }
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
