using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using Settings = Yorick.Config.JungleClear.JungleClearMenu;

namespace Yorick.Modes
{
    public sealed class JungleClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear);
        }

        public override void Execute()
        {
            if (Settings.UseW && W.IsReady() && Mana >= Settings.ManaW)
            {
                var junglew =
                    EntityManager.MinionsAndMonsters.GetJungleMonsters()
                        .FirstOrDefault(
                            a => a.IsValidTarget(W.Range) && a.Health >= Player.Instance.GetSpellDamage(a, SpellSlot.W));

                if (junglew != null)
                {
                    W.Cast(junglew);
                    return;
                }
            }

            if (Settings.UseE && E.IsReady() && Mana >= Settings.ManaE)
            {
                var jungleE =
                    EntityManager.MinionsAndMonsters.GetJungleMonsters()
                        .FirstOrDefault(
                            a => a.IsValidTarget(E.Range) && a.Health >= Player.Instance.GetSpellDamage(a, SpellSlot.E));
                if (jungleE != null)
                {
                    E.Cast(jungleE);
                }
            }
        }
    }
}