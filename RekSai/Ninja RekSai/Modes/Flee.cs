using EloBuddy;
using EloBuddy.SDK;

using Settings = RekSai.Config.Flee.FleeMenu;

namespace RekSai.Modes
{
    public sealed class Flee : ModeBase
    {

        public override bool ShouldBeExecuted()
        {

            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee);
        }

        public override void Execute()
        {
            if (!Events.burrowed && W.IsReady() && Settings.UseE2)
            {
                W.Cast();
                return;
            }

            if (Events.burrowed && E2.IsReady() && Settings.UseE2)
            {
                Player.CastSpell(SpellSlot.E, Game.CursorPos);
                return;
            }
            
        }
    }
}
