using EloBuddy;
using EloBuddy.SDK;
using System.Collections.Generic;
using Settings = KarmaTo.Config.Modes.LaneClear;
//AA minions in order to have multiple minion to one shot with Q
//Use Q to touch the most of minion killable 
namespace KarmaTo.Modes
{
    public sealed class LaneClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear);
        }

        public override void Execute()
        {
            Orbwalker.DisableAttacking = false;
            autoClearReworked();
        }
        private void autoClearReworked()
        {
            var minions = Orbwalker.LastHitMinionsList.FindAll(m => m.IsValidTarget(Q.Range));

            if (minions.Count <= 1)
            {
                return;
            }

            if (Q.IsReady())
            {
                if (Utils.getPlayer().ManaPercent >= Settings.Mana)
                {
                    var minion = minions[0];
                    var targeted = 0;
                    foreach (var m in minions)
                    {
                        var lTargeted = minions.FindAll(lm => lm != m && lm.Distance(m) < 200f).Count;
                        if (lTargeted > targeted)
                        {
                            var pred = Q.GetPrediction(m);

                            if (!pred.Collision)
                            {
                                minion = m;
                                targeted = lTargeted;
                            }
                        }
                    }

                    if (minion != null && targeted >= 1)
                        SpellManager.castQ(minion, Settings.UseR, Config.Modes.Combo.predictionHit);
                    return;
                }
            }
        }
    }
}
