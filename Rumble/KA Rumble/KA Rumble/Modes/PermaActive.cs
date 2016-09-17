using EloBuddy;
using KA_Rumble.DMGHandler;

namespace KA_Rumble.Modes
{
    public sealed class PermaActive : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return true;
        }

        public override void Execute()
        {
            if (Player.Instance.InDanger(95) && W.IsReady() && (Functions.ShouldOverload(SpellSlot.W) || Player.Instance.Mana < 80))
            {
                W.Cast();
            }
        }
    }
}
