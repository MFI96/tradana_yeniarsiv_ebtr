using System;
using EloBuddy;
using EloBuddy.SDK;
using Settings = JusticeTalon.Config.Modes;

namespace JusticeTalon.Modes
{
    public sealed class PermaActive : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return true;
        }

        public override void Execute()
        {
            /*Orbwalker.OnPreAttack += Orbwalker_Pre;
            Orbwalker.OnPostAttack += Orbwalker_Post;*/
        }

      /*  private void Orbwalker_Pre(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))  ||
                Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) ||
                Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear) ||
                Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)) 
            {
                ModeManager.Useitems();
                Orbwalker.ResetAutoAttack();
            }
        }

        private void Orbwalker_Post(AttackableUnit target, EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))  ||
                Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) ||
                Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear) ||
                Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                if (Settings.Combo.UseQ && SpellManager.Q.IsReady())
                {
                    SpellManager.Q.Cast();
                    Orbwalker.ResetAutoAttack();
                }
            }
        }*/
    }
}