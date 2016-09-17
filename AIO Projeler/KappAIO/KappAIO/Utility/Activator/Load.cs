using System;
using EloBuddy.SDK.Menu;
using KappAIO.Common;
using KappAIO.Utility.Activator.Spells;

namespace KappAIO.Utility.Activator
{
    internal class Load
    {
        internal static Menu MenuIni, DamageHandler;

        public static void Init()
        {
            try
            {
                MenuIni = MainMenu.AddMenu("KappActivator", "KappActivator");
                MenuIni.CreateCheckBox("Champ", "Load Only Activator", false);
                DamageHandler = MenuIni.AddSubMenu("DamageHandler");
                DamageHandler.CreateCheckBox("Minions", "Detect Minions Damage", false);
                DamageHandler.CreateCheckBox("Heros", "Detect Heros Damage");
                DamageHandler.CreateCheckBox("Turrets", "Detect Turrets Damage");
                DamageHandler.CreateCheckBox("Minion", "Detect Minions Damage");
                DamageHandler.CreateCheckBox("Skillshots", "Detect Skillshots Damage");
                DamageHandler.CreateCheckBox("Targetedspells", "Detect Targetedspells Damage");
                DamageHandler.CreateSlider("Mod", "InComing Damage Modifier {0}%", 100, 0, 200);

                Items.Potions.Init();
                Cleanse.Qss.Init();
                Summoners.Init();
                Items.Offence.Init();
                Items.Defence.Init();
            }
            catch (Exception ex)
            {
                Logger.Send("Activator Load Error While Init", ex, Logger.LogLevel.Error);
            }
        }
    }
}
