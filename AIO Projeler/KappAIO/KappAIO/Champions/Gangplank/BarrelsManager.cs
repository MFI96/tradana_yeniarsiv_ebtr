using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace KappAIO.Champions.Gangplank
{
    internal class BarrelsManager
    {
        internal static readonly List<Barrels> BarrelsList = new List<Barrels>();

        internal static void Init()
        {
            Game.OnTick += Game_OnTick;
        }

        private static float BarrelDamage(Obj_AI_Base target)
        {
            var Elvl = Gangplank.E.Level - 1;
            var floats = new float[] { 0, 0, 0, 0, 0 };
            if (target is AIHeroClient)
            {
                floats = new float[] { 60f, 90f, 120f, 150f, 180f };
            }
            return Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical, Player.Instance.TotalAttackDamage + floats[Elvl]);
        }

        internal static bool BarrelKill(AIHeroClient target)
        {
            //Chat.Print(BarrelDamage(target));
            return BarrelDamage(target) >= Prediction.Health.GetPrediction(target, Game.Ping);
        }

        internal static bool BarrelKill(Obj_AI_Base target)
        {
            //Chat.Print(BarrelDamage(target));
            return BarrelDamage(target) >= Prediction.Health.GetPrediction(target, Game.Ping);
        }

        private static void Game_OnTick(EventArgs args)
        {
            foreach (var barrel in ObjectManager.Get<Obj_AI_Minion>().Where(o => o.Buffs.Any(b => b.DisplayName.Equals("GangplankEBarrelActive") && b.Caster.IsMe)))
            {
                if (BarrelsList.All(b => b.Barrel.NetworkId != barrel.NetworkId))
                {
                    var newbarrel = new Barrels(barrel, Core.GameTickCount);
                    BarrelsList.Add(newbarrel);
                }
            }
            BarrelsList.RemoveAll(b => b?.Barrel == null || b.Barrel.IsDead || !b.Barrel.IsValid || b.Barrel.Health <= 0);
        }

        internal class Barrels
        {
            public Obj_AI_Minion Barrel;
            public float StartTick;

            public Barrels(Obj_AI_Minion barrel, float tick)
            {
                this.Barrel = barrel;
                this.StartTick = tick;
            }
        }

        internal static Obj_AI_Minion KillableBarrel(Barrels b)
        {
            if (b == null)
            {
                return null;
            }
            if (Prediction.Health.GetPrediction(b.Barrel, (int)QTravelTime(b.Barrel)) < 2)
            {
                return b.Barrel;
            }

            //Chat.Print(Core.GameTickCount - b.StartTick);
            if (!b.Barrel.IsValidTarget(Player.Instance.GetAutoAttackRange() + 25) && b.Barrel.IsValidTarget(Gangplank.Q.Range))
            {
                if (Core.GameTickCount - (b.StartTick + HPTiming() - QTravelTime(b.Barrel)) >= 0)
                {
                    return b.Barrel;
                }
            }
            return null;
        }

        internal static int HPTiming()
        {
            if (Player.Instance.Level < 7)
            {
                return 4000;
            }

            return Player.Instance.Level < 13 ? 2000 : 1000;
        }

        internal static float QTravelTime(Obj_AI_Base Target)
        {
            return Player.Instance.Distance(Target) / (Player.Instance.Crit < 0.05f ? 2600f : 3000f) * 1000 + 250 + Game.Ping / 2f;
        }

        internal static Obj_AI_Minion AABarrel(Obj_AI_Base target)
        {
            foreach (var A in BarrelsList)
            {
                if (KillableBarrel(A) != null && KillableBarrel(A).IsValidTarget(Player.Instance.GetAutoAttackRange()))
                {
                    if (target.IsInRange(KillableBarrel(A), Gangplank.E.Width))
                    {
                        return KillableBarrel(A);
                    }

                    var Secondbarrel = BarrelsList.FirstOrDefault(b => b.Barrel.NetworkId != KillableBarrel(A).NetworkId && b.Barrel.Distance(KillableBarrel(A)) <= Gangplank.ConnectionRange && b.Barrel.Distance(target) <= Gangplank.E.Width);
                    if (Secondbarrel != null)
                    {
                        return BarrelsList.Any(b => b.Barrel.NetworkId != Secondbarrel.Barrel.NetworkId && b.Barrel.Distance(Secondbarrel.Barrel) <= Gangplank.ConnectionRange && b.Barrel.CountEnemiesInRange(Gangplank.E.Width) > 0) ? KillableBarrel(A) : KillableBarrel(A);
                    }

                    if (BarrelsList.Any(b => b.Barrel.NetworkId != KillableBarrel(A).NetworkId && b.Barrel.Distance(KillableBarrel(A)) <= Gangplank.ConnectionRange && b.Barrel.CountEnemiesInRange(Gangplank.E.Width) > 0))
                    {
                        return KillableBarrel(A);
                    }
                }
            }
            return BarrelsList.FirstOrDefault(b => KillableBarrel(b) != null && b.Barrel.IsValidTarget(Player.Instance.GetAutoAttackRange()) && target.IsInRange(KillableBarrel(b), Gangplank.E.Width))?.Barrel;
        }
    }
}
