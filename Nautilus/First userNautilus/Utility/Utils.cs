using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Constants;
using SharpDX;

namespace Nautilus
{
    internal static class Utils
    {

        private static readonly YasuoWall _yasuoWall = new YasuoWall();
        private static readonly List<UnitIncomingDamage> IncomingDamageList = new List<UnitIncomingDamage>();

        public static int TickCount
        {
            get { return Environment.TickCount & int.MaxValue; }
        }

       
        public static Vector2[] CircleCircleIntersection(
            this Vector2 center1,
            Vector2 center2,
            float radius1,
            float radius2)
        {
            var d = center1.Distance(center2);

            if (d > radius1 + radius2 || (d <= Math.Abs(radius1 - radius2)))
            {
                return new Vector2[] { };
            }

            var a = ((radius1 * radius1) - (radius2 * radius2) + (d * d)) / (2 * d);
            var h = (float)Math.Sqrt((radius1 * radius1) - (a * a));
            var direction = (center2 - center1).Normalized();
            var pa = center1 + (a * direction);
            var s1 = pa + (h * direction.Perpendicular());
            var s2 = pa - (h * direction.Perpendicular());
            return new[] { s1, s2 };
        }
       

        public static Vector2 ToVector2(this Vector3 vector3)
        {
            return new Vector2(vector3.X, vector3.Y);
        }
        public static List<Vector2> ToVector2(this List<Vector3> path)
        {
            return path.Select(point => point.ToVector2()).ToList();
        }

        public static bool CanMove(Obj_AI_Base target)
        {
            if (target.HasBuffOfType(BuffType.Stun) || target.HasBuffOfType(BuffType.Snare) ||
                target.HasBuffOfType(BuffType.Knockup) ||
                target.HasBuffOfType(BuffType.Charm) || target.HasBuffOfType(BuffType.Fear) ||
                target.HasBuffOfType(BuffType.Knockback) ||
                target.HasBuffOfType(BuffType.Taunt) || target.HasBuffOfType(BuffType.Suppression) ||
                target.IsStunned || (target.IsChannelingImportantSpell() && !target.IsMoving))
            {
                return false;
            }
            return true;
        }

        public static float GetPassiveTime(Obj_AI_Base target, string buffName)
        {
            return
                target.Buffs.OrderByDescending(buff => buff.EndTime - Game.Time)
                    .Where(buff => buff.Name == buffName)
                    .Select(buff => buff.EndTime)
                    .FirstOrDefault() - Game.Time;
        }

        public static bool UnderTurret(this Obj_AI_Base unit)
        {
            return UnderTurret(unit.Position, true);
        }

        /// <summary>
        ///     Returns true if the unit is under turret range.
        /// </summary>
        public static bool UnderTurret(this Obj_AI_Base unit, bool enemyTurretsOnly)
        {
            return UnderTurret(unit.Position, enemyTurretsOnly);
        }

        public static int GetPriority(this AIHeroClient hero)
        {
            var championName = hero.ChampionName;
            string[] p1 =
            {
                "Alistar", "Amumu", "Bard", "Blitzcrank", "Braum", "Cho'Gath", "Dr. Mundo", "Garen", "Gnar",
                "Hecarim", "Janna", "Jarvan IV", "Leona", "Lulu", "Malphite", "Nami", "Nasus", "Nautilus", "Nunu",
                "Olaf", "Rammus", "Renekton", "Sejuani", "Shen", "Shyvana", "Singed", "Sion", "Skarner", "Sona",
                "Soraka", "Taric", "Thresh", "Volibear", "Warwick", "MonkeyKing", "Yorick", "Zac", "Zyra"
            };

            string[] p2 =
            {
                "Aatrox", "Darius", "Elise", "Evelynn", "Galio", "Gangplank", "Gragas", "Irelia", "Jax",
                "Lee Sin", "Maokai", "Morgana", "Nocturne", "Pantheon", "Poppy", "Rengar", "Rumble", "Ryze", "Swain",
                "Trundle", "Tryndamere", "Udyr", "Urgot", "Vi", "XinZhao", "RekSai"
            };

            string[] p3 =
            {
                "Akali", "Diana", "Ekko", "Fiddlesticks", "Fiora", "Fizz", "Heimerdinger", "Jayce", "Kassadin",
                "Kayle", "Kha'Zix", "Lissandra", "Mordekaiser", "Nidalee", "Riven", "Shaco", "Vladimir", "Yasuo",
                "Zilean"
            };

            string[] p4 =
            {
                "Ahri", "Anivia", "Annie", "Ashe", "Azir", "Brand", "Caitlyn", "Cassiopeia", "Corki", "Draven",
                "Ezreal", "Graves", "Jinx", "Kalista", "Karma", "Karthus", "Katarina", "Kennen", "KogMaw", "Leblanc",
                "Lucian", "Lux", "Malzahar", "MasterYi", "MissFortune", "Orianna", "Quinn", "Sivir", "Syndra", "Talon",
                "Teemo", "Tristana", "TwistedFate", "Twitch", "Varus", "Vayne", "Veigar", "VelKoz", "Viktor", "Xerath",
                "Zed", "Ziggs"
            };

            if (p1.Contains(championName))
            {
                return 1;
            }
            if (p2.Contains(championName))
            {
                return 2;
            }
            if (p3.Contains(championName))
            {
                return 3;
            }
            return p4.Contains(championName) ? 4 : 1;
        }

        public static bool UnderTurret(this Vector3 position, bool enemyTurretsOnly)
        {
            return
                ObjectManager.Get<Obj_AI_Turret>().Any(turret => turret.IsValidTarget(950, enemyTurretsOnly, position));
        }

        public static Vector3 ExtendVector3(this Vector3 vector, Vector3 direction, float distance)
        {
            if (vector.To2D().Distance(direction.To2D()) == 0)
            {
                return vector;
            }

            var edge = direction.To2D() - vector.To2D();
            edge.Normalize();

            var v = vector.To2D() + edge*distance;
            return new Vector3(v.X, v.Y, vector.Z);
        }

        public static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsEnemy || sender.IsMinion || args.SData.IsAutoAttack() ||
                sender.Type != GameObjectType.AIHeroClient ||
                ObjectManager.Player.Distance(sender.ServerPosition) > 2000)
                return;

            if (args.SData.Name == "YasuoWMovingWall")
            {
                _yasuoWall.CastTime = Game.Time;
                _yasuoWall.CastPosition = sender.Position.Extend(args.End, 400).To3D();
                _yasuoWall.YasuoPosition = sender.Position;
                _yasuoWall.WallLvl = sender.Spellbook.Spells[1].Level;
            }
        }

        public static bool CollisionYasuo(Vector3 from, Vector3 to)
        {
            if (Game.Time - _yasuoWall.CastTime > 4)
                return false;

            var level = _yasuoWall.WallLvl;
            var wallWidth = (350 + 50*level);
            var wallDirection =
                (_yasuoWall.CastPosition.To2D() - _yasuoWall.YasuoPosition.To2D()).Normalized().Perpendicular();
            var wallStart = _yasuoWall.CastPosition.To2D() + wallWidth/2f*wallDirection;
            var wallEnd = wallStart - wallWidth*wallDirection;

            if (wallStart.Intersection(wallEnd, to.To2D(), from.To2D()).Intersects)
            {
                return true;
            }
            return false;
        }

        public static double GetIncomingDamage(Obj_AI_Base target, float time = 0.5f, bool skillshots = true)
        {
            double totalDamage = 0;

            foreach (
                var damage in
                    IncomingDamageList.Where(
                        damage => damage.TargetNetworkId == target.NetworkId && Game.Time - time < damage.Time))
            {
                if (skillshots)
                {
                    totalDamage += damage.Damage;
                }
                else
                {
                    if (!damage.Skillshot)
                        totalDamage += damage.Damage;
                }
            }

            return totalDamage;
        }

        private class UnitIncomingDamage
        {
            public int TargetNetworkId { get; set; }
            public float Time { get; set; }
            public double Damage { get; set; }
            public bool Skillshot { get; set; }
        }

        private class YasuoWall
        {
            public YasuoWall()
            {
                CastTime = 0;
            }

            public Vector3 YasuoPosition { get; set; }
            public float CastTime { get; set; }
            public Vector3 CastPosition { get; set; }
            public float WallLvl { get; set; }
        }
    }
}