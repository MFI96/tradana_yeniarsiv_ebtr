using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using Settings = DrMundo.Config.ModesMenu.Harass;
using SettingsCombo = DrMundo.Config.ModesMenu.Combo;
using SettingsPrediction = DrMundo.Config.PredictionMenu;
using SettingsHealth = DrMundo.Config.HealthManagerMenu;

namespace DrMundo.Modes
{
    public sealed class Harass : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
        }

        public override void Execute()
        {
            if (Settings.UseQ && Q.IsReady() && PlayerHealth >= SettingsHealth.MinQHealth)
            {
                var target = TargetSelector.GetTarget(SettingsCombo.MaxQDistance, DamageType.Magical);
                if (target != null)
                {
                    var pred = Q.GetPrediction(target);
                    if (pred.HitChance >= SettingsPrediction.MinQHCHarass)
                    {
                        Q.Cast(pred.CastPosition);
                        Debug.WriteChat("Casting Q in Harass, Target: {0}, Distance: {1}, HitChance: {2}", target.ChampionName,
                            "" + Player.Instance.Distance(target), pred.HitChance.ToString());
                    }
                }
            }
            if (Settings.UseW && W.IsReady() && !WActive && PlayerHealth >= SettingsHealth.MinWHealth)
            {
                var enemy =
                    EntityManager.Heroes.Enemies
                        .FirstOrDefault(e => !e.IsDead && e.Health > 0 && e.IsVisible && e.IsValidTarget() && _Player.Distance(e) < W.Range);
                if (enemy != null)
                {
                    W.Cast();
                    Debug.WriteChat("Casting W in Combo");
                    return;
                }
            }
            if (Settings.UseW && W.IsReady() && !WActive)
            {
                var enemies = EntityManager.Heroes.Enemies
                        .Count(e => !e.IsDead && e.Health > 0 && e.IsVisible && e.IsValidTarget() && _Player.Distance(e) < W.Range + 200);
                W.Cast();
                Debug.WriteChat("Turning W off in Harass, because enemy moved out of range.");
            }
        }
    }
}
