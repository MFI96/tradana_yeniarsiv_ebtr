using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using Settings = Zac.Config.Combo.ComboMenu;

namespace Zac.Modes
{
    public sealed class Combo : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {
            #region R

            if (R.IsReady() && Settings.UseR)
            {
                var targetR =
                    EntityManager.Heroes.Enemies.Where(
                        a =>
                            a.IsValidTarget(R.Range - 25) && !a.IsDead && !a.IsZombie &&
                            !a.HasBuffOfType(BuffType.Knockup) && !a.HasBuffOfType(BuffType.Knockback) &&
                            !a.HasBuffOfType(BuffType.Stun));

                if (targetR.Count() >= Settings.RMin)
                {
                    R.Cast();
                    return;
                }
            }

            #endregion

            #region Q

            if (Q.IsReady() && Settings.UseQ)
            {
                var targetQ = TargetSelector.GetTarget(Q.Range, DamageType.Magical);

                if (targetQ != null && targetQ.IsValid && !targetQ.IsZombie && !targetQ.IsDead)
                {
                    var qPred = Q.GetPrediction(targetQ);
                    Q.Cast(qPred.CastPosition);
                    return;
                }
            }

            #endregion

            #region W

            if (W.IsReady() && Settings.UseW)
            {
                var targetW = TargetSelector.GetTarget(W.Range - 25, DamageType.Magical);

                if (targetW != null && targetW.IsValid && !targetW.IsDead && !targetW.IsZombie)
                {
                    W.Cast();
                    return;
                }
            }

            #endregion

            #region E

            if (E.IsReady() && Settings.UseE)
            {
                var target2 =
                    EntityManager.Heroes.Enemies.Where(
                        a =>
                            a.IsValidTarget(SpellManager.EMaxRanges[E.Level - 1] + 400) &&
                            a.IsInRange(Game.ActiveCursorPos, Settings.CurDistance) && !a.IsDead &&
                            !a.IsZombie).OrderByDescending(TargetSelector.GetPriority).FirstOrDefault();
                if (target2 == null)
                {
                    return;
                }
                var ePred = E.GetPrediction(target2);

                if (!Events.ChannelingE)
                {
                    if (target2.Distance(Player.Instance.ServerPosition) <
                        SpellManager.EMaxRanges[E.Level - 1] - Settings.EDistanceIn &&
                        target2.Distance(Game.ActiveCursorPos) <= Settings.CurDistance &&
                        target2.Distance(Player.Instance.ServerPosition) > Settings.EDistanceOut)
                    {
                        Player.CastSpell(SpellSlot.E, target2);
                        Console.WriteLine("Started Charging E (combo)");
                        return;
                    }
                }
                if (Events.ChannelingE)
                {
                    if (ePred.CastPosition.Distance(Player.Instance.ServerPosition) <=
                        SpellManager.EMaxRanges[E.Level - 1] &&
                        ePred.HitChance >= HitChance.High)
                    {
                        Player.CastSpell(SpellSlot.E, ePred.CastPosition);
                        Console.WriteLine("Casted E to Target in Range (combo)");
                        return;
                    }

                    if (E.IsFullyCharged &&
                        ePred.CastPosition.Distance(Player.Instance.ServerPosition) >
                        SpellManager.EMaxRanges[E.Level - 1] && target2.Distance(Player.Instance.ServerPosition) <
                        SpellManager.EMaxRanges[E.Level - 1] + 300)
                    {
                        Player.CastSpell(SpellSlot.E, ePred.CastPosition);
                        Console.WriteLine("Casted E to target out of range(combo)");
                        return;
                    }
                }
            }

            #endregion
        }
    }
}