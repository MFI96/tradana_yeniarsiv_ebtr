using EloBuddy;
using EloBuddy.SDK;

namespace PandaTeemoReborn.Modes
{
    public sealed class Harass : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
        }

        public override void Execute()
        {
            if (Extensions.MenuValues.Harass.UseW && W.IsReady())
            {
                if (Extensions.MenuValues.Harass.ManaW >= Player.Instance.ManaPercent)
                {
                    return;
                }

                var countEnemies = Player.Instance.CountEnemiesInRange(Player.Instance.GetAutoAttackRange());

                if (Extensions.MenuValues.Harass.WRange && countEnemies > 0)
                {
                    W.Cast();
                }
                else if (!Extensions.MenuValues.Harass.WRange)
                {
                    W.Cast();
                }
            }
        }
    }
}