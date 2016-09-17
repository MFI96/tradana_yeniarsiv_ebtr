using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;

namespace _300_Pantheon.Assistants
{
    public static class Logic
    {
        /// <summary>
        ///     This function will check if the given target is killable
        /// </summary>
        /// <param name="target">Must be a AIHeroClient</param>
        /// <param name="range">Must be a float value where to check the max range</param>
        /// <param name="key">Must be a Spellslot key for assign the damage</param>
        /// <returns></returns>
        public static bool IsKillableTarget(AIHeroClient target, float range, SpellSlot key)
        {
            var x = target;
            if (x.IsValidTarget(range) && !x.HasBuffOfType(BuffType.Invulnerability) &&
                x.TotalShieldHealth() <= Player.Instance.GetSpellDamage(x, key))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///     This function will check if the given target is killable
        /// </summary>
        /// <param name="target">Must be a OBJAIMinion</param>
        /// <param name="range">Must be a float value where to check the max range</param>
        /// <param name="key">Must be a Spellslot key for assign the damage</param>
        /// <returns></returns>
        public static bool IsKillableMinion(Obj_AI_Minion target, float range, SpellSlot key)
        {
            var x = target;
            if (x.IsValidTarget(range) && x.TotalShieldHealth() + 5 <= Player.Instance.GetSpellDamage(x, key))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///     This function will list all the closest enemies
        /// </summary>
        /// <param name="range">Limit the list to only range value</param>
        /// <param name="from">Limit the list according to the vector3 coordinations</param>
        /// <returns></returns>
        public static List<AIHeroClient> CloseEnemies(float range = 1500, Vector3 from = default(Vector3))
        {
            return EntityManager.Heroes.Enemies.Where(e => e.IsValidTarget(range, false, from)).ToList();
        }

        /// <summary>
        ///     This function will list all the closest allies
        /// </summary>
        /// <param name="range">Limit the list to only range value</param>
        /// <returns></returns>
        public static List<AIHeroClient> CloseAllies(float range = 1500)
        {
            return EntityManager.Heroes.Allies.Where(a => a.IsValidTarget(range) && !a.IsMe).ToList();
        }

        /// <summary>
        ///     This function will list all the closest minions
        /// </summary>
        /// <param name="team">Limit the list to the desired team</param>
        /// <param name="range">Limit the list to only range value</param>
        /// <param name="from">Limit the list according to the Vector3 coordinations</param>
        /// <returns></returns>
        public static List<Obj_AI_Minion> Minions(EntityManager.UnitTeam team, float range,
            Vector3 from = default(Vector3))
        {
            return EntityManager.MinionsAndMonsters.GetLaneMinions(team, from, range).ToList();
        }

        public static List<Obj_AI_Minion> Monsters(float range, Vector3 from = default(Vector3))
        {
            return EntityManager.MinionsAndMonsters.GetJungleMonsters().Where(x => x.IsInRange(from, range)).ToList();
        }
    }
}