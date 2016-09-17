using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;
using Settings = DrMundo.Config.ModesMenu.Flee;
using SettingsPrediction = DrMundo.Config.PredictionMenu;
using SettingsHealth = DrMundo.Config.HealthManagerMenu;

namespace DrMundo.Modes
{
    public sealed class Flee : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee);
        }

        public override void Execute()
        {
            if (Settings.UseQ && Q.IsReady() && SettingsHealth.MinQHealth < PlayerHealth)
            {
                var enemy =
                    EntityManager.Heroes.Enemies.Where(e => e.IsValidTarget(Q.Range))
                        .OrderBy(e => e.Distance(Player.Instance))
                        .FirstOrDefault();
                if (enemy != null)
                {
                    var pred = Q.GetPrediction(enemy);
                    if (pred.HitChance >= SettingsPrediction.MinQHCFlee)
                    {
                        Q.Cast(pred.CastPosition);
                        Debug.WriteChat("Casting Q in Flee on {0}", enemy.ChampionName);
                    }
                }
            }
        }
    }
}
