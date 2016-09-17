using EloBuddy.SDK;
using System.Linq;
using Settings = Warwick.Config.ModesMenu.LaneClear;
using SettingsMana = Warwick.Config.ManaManagerMenu;

namespace Warwick.Modes
{
    public sealed class LaneClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear);
        }

        public override void Execute()
        {
            if (Settings.UseQ && Q.IsReady() && PlayerMana >= SettingsMana.MinQMana)
            {
                var target =
                    EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, _PlayerPos, (Q.Range + 50))
                        .OrderByDescending(e => e.MaxHealth).ThenBy(e => e.Health).FirstOrDefault();
                if (target != null && target.IsValidTarget())
                {
                    Q.Cast(target);
                    Debug.WriteChat("Casting Q in LaneClear on {0}", target.BaseSkinName);
                }
            }
        }
    }
}
