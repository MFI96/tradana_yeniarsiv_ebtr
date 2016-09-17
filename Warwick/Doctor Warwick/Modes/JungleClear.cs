using EloBuddy.SDK;
using System.Linq;
using Settings = Warwick.Config.ModesMenu.JungleClear;
using SettingsMana = Warwick.Config.ManaManagerMenu;

namespace Warwick.Modes
{
    public sealed class JungleClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear);
        }

        public override void Execute()
        {
            if (Settings.UseQ && Q.IsReady() && PlayerMana >= SettingsMana.MinQMana)
            {
                var target =
                    EntityManager.MinionsAndMonsters.GetJungleMonsters(_PlayerPos, Q.Range + 100)
                        .OrderByDescending(e => e.MaxHealth)
                        .FirstOrDefault();
                if (target != null && target.IsValidTarget())
                {
                    Q.Cast(target);
                    Debug.WriteChat("Casting Q in JungleClear on {0}", target.BaseSkinName);
                }
            }
        }
    }
}

