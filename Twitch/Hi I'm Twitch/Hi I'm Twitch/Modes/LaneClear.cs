using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

using Settings = AddonTemplate.Config.Modes.Clear;

namespace AddonTemplate.Modes
{
    public sealed class LaneClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear);
        }

        public override void Execute()
        {
            var minion = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                Player.Instance.ServerPosition, E.Range, false);
            if (Settings.WLaneClear && W.IsReady() && Player.Instance.ManaPercent > Settings.LaneClearMana)
            {
                foreach (var hitableminion in minion)
                {
                    var collision = new List<Obj_AI_Minion>();
                    var startPos = Player.Instance.Position.To2D();
                    var endPos = hitableminion.Position.To2D();
                    collision.Clear();
                    foreach (
                        var colliMinion in
                            EntityManager.MinionsAndMonsters.EnemyMinions.Where(
                                colliMinion =>
                                    colliMinion.IsInRange(hitableminion, W.Range) && colliMinion.IsValidTarget(W.Range))
                        )
                    {
                        if (Prediction.Position.Collision.CircularMissileCollision(colliMinion, startPos, endPos,
                            SpellManager.W.Speed, SpellManager.W.Width, SpellManager.W.CastDelay))
                        {
                            collision.Add(colliMinion);
                        }
                        if (collision.Count >= Settings.WNumber)
                        {
                            W.Cast(hitableminion);
                        }
                    }
                }
            }
            if (Settings.ELaneClear && E.IsReady() && Player.Instance.ManaPercent > Settings.LaneClearMana)
            {
                var kill = 0;
                foreach (var killableminion in minion)
                {
                    if (kill >= Settings.ENumber)
                    {
                        E.Cast();
                        break;
                    }
                    if (killableminion.Health < DamageHelper.GetEDamage(killableminion))
                    {
                        kill++;
                    }
                }
            }
        }
    }
}