using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using Settings = KarmaTo.Config.Modes.Flee;

//TODO
//Use E on ally behind me 

namespace KarmaTo.Modes
{
    public sealed class Flee : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee);
        }

        public override void Execute()
        {
            Orbwalker.DisableAttacking = true;
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            if (Settings.UseQ)
            {
                SpellManager.castQ(target,false,Config.Modes.Combo.predictionHit);
            }
            if (Settings.UseE)
            {
                SpellManager.castE(Utils.getPlayer(), Settings.UseR);
            }
        }
    }
}
