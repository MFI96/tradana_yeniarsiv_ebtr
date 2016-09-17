using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using Settings = Farofakids_Galio.Config.Modes.Harass;
using SettingsC = Farofakids_Galio.Config;


namespace Farofakids_Galio.Modes
{
    

    public sealed class PermaActive : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return true;
        }

        public override void Execute()
        {
            if (SettingsC.Modes.Misc.autoshield && W.IsReady() && !EloBuddy.Player.Instance.IsRecalling())
            {
                W.Cast(Player.Instance);
            }

            if (CheckAutoW())
            {
                W.Cast(Player.Instance);
            }
            if (Program.rActive /*|| justR*/)
            {
                Orbwalker.DisableAttacking = true;
                Orbwalker.DisableMovement = true;
                return;
            }
            else
            {
                Orbwalker.DisableAttacking = false;
                Orbwalker.DisableMovement = false;
            }
            if (Player.Instance.ManaPercent < Settings.ManaAuto)
            {
                return;
            }
            if (Settings.UseQauto && Q.IsReady())
            {
                var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
                if (target != null)
                { 
                Q.Cast(target);
                }
            }
            if (Settings.UseEauto && E.IsReady())
            {
                var target = TargetSelector.GetTarget(E.Range, DamageType.Magical);
                if (target != null)
                {
                    E.Cast(target);
                }
            }
        }

        public static bool CheckAutoW()
        {
            return SettingsC.Modes.Misc.UseWman < Player.Instance.ManaPercent &&
                   SettingsC.Modes.Misc.UseWhea > Player.Instance.HealthPercent;
        }

    }
}
