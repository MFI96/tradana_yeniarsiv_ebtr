
using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;

namespace YasuoBuddy
{
    class Variables
    {
        public static YasWall wall = new YasWall();

        public static bool IsDashing = false;

        public static bool wallCasted;

        public static BuffType[] buffs;

        public static AIHeroClient _Player { get { return ObjectManager.Player; } }


        internal class YasWall
        {
            public MissileClient pointL;
            public MissileClient pointR;
            public float endtime = 0;
            public YasWall()
            {

            }

            public YasWall(MissileClient L, MissileClient R)
            {
                pointL = L;
                pointR = R;
                endtime = Game.Time + 4;
            }

            public void setR(MissileClient R)
            {
                pointR = R;
                endtime = Game.Time + 4;
            }

            public void setL(MissileClient L)
            {
                pointL = L;
                endtime = Game.Time + 4;
            }

            public bool isValid(int time = 0)
            {
                return pointL != null && pointR != null && endtime - (time / 1000) > Game.Time;
            }
        }

        public static bool CanCastE(Obj_AI_Base target)
        {
            return !target.HasBuff("YasuoDashWrapper");
        }


        public static Vector3 PosAfterE(Obj_AI_Base target)
        {
            return (Vector3)_Player.ServerPosition.Extend(
                target.ServerPosition,
                _Player.Distance(target) < 410 ? Yasuo.E.Range : _Player.Distance(target) + 65);
        }

        public static bool AlliesNearTarget(Obj_AI_Base target, float range)
        {
            return EntityManager.Heroes.Allies.Where(tar => tar.Distance(target) < range).Any(tar => tar != null);
        }


        public static bool enemyIsJumpable(Obj_AI_Base enemy, List<AIHeroClient> ignore = null)
        {
            if (enemy.IsValid && enemy.IsEnemy && !enemy.IsInvulnerable && !enemy.MagicImmune && !enemy.IsDead &&
                !(enemy is FollowerObject))
            {
                if (ignore != null)
                    foreach (AIHeroClient ign in ignore)
                    {
                        if (ign.NetworkId == enemy.NetworkId)
                            return false;
                    }
                foreach (BuffInstance buff in enemy.Buffs)
                {
                    if (buff.Name == "YasuoDashWrapper")
                        return false;
                }
                return true;
            }
            return false;
        }

        public static bool isDashing
        {
            get
            {
                if (Yasuo.E.State == SpellState.Surpressed
                    && !Variables._Player.HasBuffOfType(BuffType.Suppression))
                    return true;
                else
                    return false;
            }
        }
    }
}
