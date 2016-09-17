using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using Settings = Farofakids_Galio.Config.Modes.LaneClear;

namespace Farofakids_Galio.Modes
{
    public sealed class JungleClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            // Only execute this mode when the orbwalker is on jungleclear mode
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear);
        }

        public override void Execute()
        {
            if (Player.Instance.ManaPercent < Settings.Mana)
            {
                return;
            }

        }
    }
}
