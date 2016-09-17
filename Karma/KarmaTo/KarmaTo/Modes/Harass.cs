using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using Settings = KarmaTo.Config.Modes.Harass;

//TODO : 
//DISABLE LAST HIT MINION

namespace KarmaTo.Modes
{
    public sealed class Harass : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
        }

        public override void Execute()
        {
            Orbwalker.DisableAttacking = false;
            if (Settings.UseQ && Player.Instance.ManaPercent > Settings.Mana)
            {
                var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
                SpellManager.castQ(target, Settings.UseR, Config.Modes.Combo.predictionHit);
            }
        }
    }
}
