using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Spells;
using KappAIO.Common;

namespace KappAIO.Utility.Activator.Spells
{
    internal class Summoners
    {
        private static Menu SummMenu;

        public static void Init()
        {
            SummMenu = Load.MenuIni.AddSubMenu("SummonerSpells");
            SummMenu.AddGroupLabel("Allies");
            SummMenu.CreateCheckBox("ally", "Use Heal For Allies");
            SummMenu.CreateSlider("allyhp", "Ally HP% {0}% To Use Heal", 30);
            SummMenu.AddSeparator(0);
            SummMenu.AddGroupLabel("Self");
            SummMenu.CreateCheckBox("ignite", "Use Ignite");
            SummMenu.CreateCheckBox("me", "Use Heal For Self");
            SummMenu.CreateSlider("hp", "Self HP% {0}% To Use Heal", 30);

            Events.OnIncomingDamage += Events_OnIncomingDamage;
            Game.OnTick += Game_OnTick;
        }

        private static void Game_OnTick(System.EventArgs args)
        {
            var target = TargetSelector.GetTarget(600, DamageType.True);
            if (target != null && target.IsKillable(600) && !target.IsValidTarget(Player.Instance.GetAutoAttackRange()) && target.CountEnemiesInRange(1000) >= target.CountAlliesInRange(1000) && Player.Instance.GetSummonerSpellDamage(target, DamageLibrary.SummonerSpells.Ignite) >= target.TotalShieldHealth() && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) && SummonerSpells.Ignite.IsReady() && SummMenu.CheckBoxValue("ignite"))
            {
                SummonerSpells.Ignite.Cast(target);
            }
        }

        private static void Events_OnIncomingDamage(Events.InComingDamageEventArgs args)
        {
            if(args.Target == null || !args.Target.IsKillable() || args.InComingDamage < 1 || args.Target.Distance(Player.Instance) > 800 || !SummonerSpells.Heal.IsReady()) return;
            
            var damagepercent = args.InComingDamage / args.Target.TotalShieldHealth() * 100;
            var death = args.InComingDamage >= args.Target.Health && args.Target.HealthPercent < 99;

            if (SummMenu.CheckBoxValue("ally") && args.Target.IsAlly && !args.Target.IsMe && (SummMenu.SliderValue("allyhp") >= args.Target.HealthPercent || death))
            {
                SummonerSpells.Heal.Cast();
            }

            if (SummMenu.CheckBoxValue("me") && args.Target.IsMe && (SummMenu.SliderValue("hp") >= args.Target.HealthPercent || death))
            {
                SummonerSpells.Heal.Cast();
            }
        }
    }
}
