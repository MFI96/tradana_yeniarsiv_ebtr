using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using Settings = DrMundo.Config.ModesMenu.LastHit;
using SettingsCombo = DrMundo.Config.ModesMenu.Combo;
using SettingsPrediction = DrMundo.Config.PredictionMenu;
using SettingsHealth = DrMundo.Config.HealthManagerMenu;

namespace DrMundo.Modes
{
    public sealed class LastHit : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit);
        }

        public override void Execute()
        {
            if (Settings.UseQ && Q.IsReady() && PlayerHealth >= SettingsHealth.MinQHealth)
            {
                var minion =
                EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, _PlayerPos, Q.Range)
                    .FirstOrDefault(e => e.IsValidTarget(SettingsCombo.MaxQDistance) && e.Distance(_Player) > _Player.GetAutoAttackRange() && e.Health < Damages.QDamage(e) && Q.GetPrediction(e).HitChance >= HitChance.Medium);
                if (minion != null)
                {
                    var pred = Q.GetPrediction(minion);
                    if (pred.HitChance >= SettingsPrediction.MinQHCLastHit)
                    {
                        Q.Cast(pred.CastPosition);
                        Debug.WriteChat("Casting Q in LastHit");
                    }
                }

            }
        }
    }
}
