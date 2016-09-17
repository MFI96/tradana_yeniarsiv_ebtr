using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using Settings = Zac.Config.Jump.JumpMenu;
using Settings2 = Zac.Config.Combo.ComboMenu;

namespace Zac.Modes
{
    public sealed class Jump : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee);
        }

        public override void Execute()
        {
            var target2 =
                EntityManager.Heroes.Enemies.Where(
                    a =>
                        a.IsValidTarget(SpellManager.EMaxRanges[E.Level - 1] + 400) &&
                        a.IsInRange(Game.ActiveCursorPos, Settings2.CurDistance) && !a.IsDead &&
                        !a.IsZombie).OrderByDescending(TargetSelector.GetPriority).FirstOrDefault();
            if (target2 == null)
            {
                return;
            }
            var ePred = E.GetPrediction(target2);


            if (E.IsReady() && Settings.UseE)
            {
                if (!Events.ChannelingE)
                {
                    if (target2.Distance(Player.Instance.ServerPosition) <
                        SpellManager.EMaxRanges[E.Level - 1] - Settings.EDistanceIn &&
                        target2.Distance(Game.ActiveCursorPos) <= Settings2.CurDistance &&
                        target2.Distance(Player.Instance.ServerPosition) > Settings.EDistanceOut)
                    {
                        Player.CastSpell(SpellSlot.E, target2);
                        Console.WriteLine("Started Charging E (jump)");
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
                        Console.WriteLine("Casted E to Target in Range (jump)");
                        return;
                    }

                    if (E.IsFullyCharged &&
                        ePred.CastPosition.Distance(Player.Instance.ServerPosition) >
                        SpellManager.EMaxRanges[E.Level - 1] && target2.Distance(Player.Instance.ServerPosition) <
                        SpellManager.EMaxRanges[E.Level - 1] + 300)
                    {
                        Player.CastSpell(SpellSlot.E, ePred.CastPosition);
                        Console.WriteLine("Casted E to target out of range(jump)");
                        return;
                    }
                }
            }
        }
    }
}