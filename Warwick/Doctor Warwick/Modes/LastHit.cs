using EloBuddy.SDK;
using System.Linq;
using Settings = Warwick.Config.ModesMenu.LastHit;
using SettingsMana = Warwick.Config.ManaManagerMenu;

namespace Warwick.Modes
{
    public sealed class LastHit : ModeBase
    {

        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit);
        }

        public override void Execute()
        {
            if (Settings.UseQ && Q.IsReady() && PlayerMana >= SettingsMana.MinQMana)
            {
                var target =
                    EntityManager.MinionsAndMonsters.CombinedAttackable.Where(e => e.IsValidTarget(Q.Range))
                        .OrderBy(e => e.Health).FirstOrDefault(e => e.Health <= Damages.QDamage(e));
                if (target != null)
                {
                    Q.Cast(target);
                    Debug.WriteChat("Casting Q in LastHit on {0}", target.BaseSkinName);
                }
            }
        }
    }
}
