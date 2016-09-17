using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

using Settings = RekSai.Config.Combo.ComboMenu;

namespace RekSai.Modes
{
    public sealed class Combo : ModeBase
    {

        public override bool ShouldBeExecuted()
        {

            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {

            if (Player.Instance.CountEnemiesInRange(800) > 0)
            {

                RekSai.ItemsManager.Yomuus();
            }



            if (Events.burrowed)
            {
                var targetW = TargetSelector.GetTarget(Player.Instance.BoundingRadius + 175, DamageType.Physical);
                var targetQ2 = TargetSelector.GetTarget(850, DamageType.Magical);
                
                var targetE = TargetSelector.GetTarget(550, DamageType.Physical);
                var targetE2 = TargetSelector.GetTarget(E2.Range, DamageType.Physical);
                var predE2 = E2.GetPrediction(targetE2);

                if (Settings.UseW && W.IsReady())
                {
                    
                    if (targetW != null && targetW.IsValidTarget())
                    {
                        W.Cast();
                        return;
                    }
                }

                if (Settings.UseQ2 && Q2.IsReady())
                {
                    
                    if (targetQ2 != null && targetQ2.IsValidTarget())
                    {
                        var predictionQ2 = Q2.GetPrediction(targetQ2);
                        if (predictionQ2.HitChance >= HitChance.Medium)
                        {
                            Q2.Cast(predictionQ2.CastPosition);
                            return;
                        }
                    }

                }

                if (Settings.UseE2 && E2.IsReady())
                {
                    
                    if (Player.Instance.CountEnemiesInRange(Settings.E2Distance) < 1)
                    {
                        
                        if (targetE2.IsValidTarget())
                        {
                            E2.Cast(targetE2.Position + 200);
                            return;

                        }
                    }
                }
            }

                if (!Events.burrowed)
                {
                    var targetE = TargetSelector.GetTarget(E.Range, DamageType.Physical);
                    var lastTarget = Orbwalker.LastTarget;
                    var target = TargetSelector.GetTarget(300, DamageType.Physical);

                    if (Settings.UseE && E.IsReady())
                    {
                        
                        if (targetE != null && targetE.IsValidTarget())
                        {
                            E.Cast(targetE);
                            return;
                        }
                    }

                    if (Settings.UseW && W.IsReady())
                    {
                        if (Player.Instance.CountEnemiesInRange(400) == 0)
                        {
                            W.Cast();
                            return;
                        }
                    }

                    if (Settings.UseW && W.IsReady())
                    {
                        if (lastTarget.IsValidTarget(Player.Instance.BoundingRadius + 250) && !target.HasBuff("reksaiknockupimmune"))
                        {
                            W.Cast();
                            return;
                        }
                    }
                }
            }
 
    }
}
