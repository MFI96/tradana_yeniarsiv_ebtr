using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using Settings = NinjaNunu.Config.Modes.JungleClear;
using Settings2 = NinjaNunu.Config.Modes.MiscMenu;

namespace NinjaNunu.Modes
{
    public sealed class JungleClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            // Only execute this mode when the orbwalker is on jungleclear mode
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear);
        }

        public override void Execute()
        {
            if (Settings.UseQ && Q.IsReady())
            {
                var Jmonsters = EntityManager.MinionsAndMonsters.GetJungleMonsters().OrderByDescending(a => a.MaxHealth).FirstOrDefault(b => b.Distance(Player.Instance) <= 500);
                //if (Damage.QDamage(Jmonsters) > Jmonsters.Health)
                if (Jmonsters.Health <= Damage.QDamage(Jmonsters))
                {
                    Q.Cast(Jmonsters);
                    return;
                }
            }

            if (Settings.UseE && E.IsReady() && Player.Instance.ManaPercent >= Settings.MinMana || Player.Instance.HasBuff("Visions") && Settings.UseE && E.IsReady())
            {
                var Emonsters = EntityManager.MinionsAndMonsters.GetJungleMonsters().OrderByDescending(a => a.MaxHealth).FirstOrDefault(b => b.Distance(Player.Instance) <= 550);
                if (Emonsters != null)
                {
                    E.Cast(Emonsters);
                    return;
                }
            }

            if (Settings.UseW && W.IsReady() && Player.Instance.ManaPercent >= Settings.MinMana)
            {
                var ally = EntityManager.Heroes.Allies.OrderByDescending(a => a.TotalAttackDamage).FirstOrDefault(b => b.Distance(Player.Instance) < 1000);
                if (ally != null)
                {
                    W.Cast(ally);
                    return;
                }
                if (Settings.UseW && W.IsReady())
                {
                    W.Cast(Player.Instance);
                    return;
                }
            }
        }
    }
}
