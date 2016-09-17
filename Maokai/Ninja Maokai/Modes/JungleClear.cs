using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using Settings = Maokai.Config.JungleClear.JungleClearMenu;

namespace Maokai.Modes
{
    public sealed class JungleClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear);
        }

        public override void Execute()
        {
            if (Settings.UseE && E.IsReady() && ManaPercent >= Settings.ManaE)
            {
                var junglemonsters = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.Position,
                    E.Range);
                var efarm = EntityManager.MinionsAndMonsters.GetCircularFarmLocation(junglemonsters, E.Width,
                    (int) E.Range);
                var importantmonstersE =
                    EntityManager.MinionsAndMonsters
                        .GetJungleMonsters()
                        .FirstOrDefault(
                            a =>
                                a.IsValidTarget(E.Range) && a.Name.Contains("SRU_Baron") ||
                                a.IsValidTarget(E.Range) && a.Name.Contains("SRU_Dragon") ||
                                a.IsValidTarget(E.Range) && a.Name.Contains("SRU_Gromp") ||
                                a.IsValidTarget(E.Range) && a.Name.Contains("SRU_RiftHerald"));

                if (importantmonstersE != null)
                {
                    E.Cast(importantmonstersE);
                    return;
                }

                if (efarm.HitNumber >= Settings.MinE)
                {
                    E.Cast(efarm.CastPosition);
                    return;
                }
            }

            if (Settings.UseW && W.IsReady() && ManaPercent >= Settings.ManaW)
            {
                var junglemonsterW =
                    EntityManager.MinionsAndMonsters.GetJungleMonsters()
                        .Where(
                            a =>
                                a.IsValidTarget(W.Range) && Utility.MonstersNames.Contains(a.BaseSkinName) &&
                                a.Health > Player.Instance.GetSpellDamage(a, SpellSlot.W))
                        .OrderByDescending(a => a.MaxHealth)
                        .FirstOrDefault();
                if (junglemonsterW != null)
                {
                    W.Cast(junglemonsterW);
                    return;
                }
            }

            if (Settings.UseQ && Q.IsReady() && ManaPercent >= Settings.ManaQ)
            {
                var junglemonstersQ =
                    EntityManager.MinionsAndMonsters.GetJungleMonsters()
                        .Where(
                            a =>
                                a.IsValidTarget(Q.Range) &&
                                a.Health <
                                Player.Instance.GetSpellDamage(a, SpellSlot.Q) + Player.Instance.GetAutoAttackDamage(a))
                        .OrderByDescending(a => a.MaxHealth)
                        .FirstOrDefault();

                var junglemonstersQ2 =
                    EntityManager.MinionsAndMonsters
                        .GetJungleMonsters(
                        )
                        .FirstOrDefault(
                            a => a.IsValidTarget(Q.Range) && Utility.MonstersNames.Contains(a.BaseSkinName) &&
                                 a.Health > Player.Instance.GetSpellDamage(a, SpellSlot.Q));


                if (junglemonstersQ2 != null)
                {
                    Q.Cast(junglemonstersQ2);
                    return;
                }
                if (junglemonstersQ != null)
                {
                    Q.Cast(junglemonstersQ);
                }
            }
        }
    }
}