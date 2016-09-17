using System;
using EloBuddy;
using EloBuddy.SDK;

namespace PandaTeemoReborn.Modes
{
    public sealed class Flee : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee);
        }

        public override void Execute()
        {
            if (Extensions.MenuValues.Flee.UseW && W.IsReady())
            {
                W.Cast();
            }

            if (Extensions.MenuValues.Flee.UseR && R.IsReady())
            {
                if (!Extensions.HasShroomLanded)
                {
                    return;
                }

                if (Environment.TickCount - Extensions.LastR < Extensions.MenuValues.Flee.RDelay)
                {
                    return;
                }

                if (Player.Instance.Spellbook.GetSpell(SpellSlot.R).Ammo < Extensions.MenuValues.Flee.RCharge)
                {
                    return;
                }

                if (Player.Instance.CountEnemiesInRange(Player.Instance.GetAutoAttackRange()) == 0)
                {
                    return;
                }

                if (Extensions.IsShroomed(Player.Instance.Position))
                {
                    return;
                }

                R.Cast(Player.Instance.Position);
            }
        }
    }
}