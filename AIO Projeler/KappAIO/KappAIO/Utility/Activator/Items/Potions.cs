using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using KappAIO.Common;
using static KappAIO.Utility.Activator.Database;

namespace KappAIO.Utility.Activator.Items
{
    internal class Potions
    {
        private static readonly List<Item> Pots = new List<Item> { HealthPotion, Biscuit, CorruptingPotion, RefillablePotion };

        private static readonly List<string> PotBuffs = new List<string> { "Health Potion", "ItemCrystalFlask", "ItemDarkCrystalFlask", "ItemMiniRegenPotion" };

        private static Menu PotionsMenu;

        public static void Init()
        {
            try
            {
                PotionsMenu = Load.MenuIni.AddSubMenu("Potions");
                Pots.ForEach(
                    p =>
                        {
                            PotionsMenu.CreateCheckBox(p.Id.ToString(), "Use " + p.ItemInfo.Name);
                            PotionsMenu.CreateSlider(p.Id + "hp", p.ItemInfo.Name + " HP% {0}%", 60);
                            PotionsMenu.AddSeparator(0);
                        });
                Events.OnIncomingDamage += Events_OnIncomingDamage;
            }
            catch (Exception ex)
            {
                Logger.Send("Activator Potions Error While Init", ex, Logger.LogLevel.Error);
            }
        }

        private static void Events_OnIncomingDamage(Events.InComingDamageEventArgs args)
        {
            if (args.Target == null || args.InComingDamage < 1 || args.Target.IsInFountainRange() || !args.Target.IsKillable() || !args.Target.IsMe)
                return;

            foreach (var pot in Pots.Where(p => p.ItemReady(PotionsMenu) && PotionsMenu.SliderValue(p.Id + "hp") >= Player.Instance.HealthPercent))
            {
                if (!Player.Instance.Buffs.Any(a => PotBuffs.Any(b => a.DisplayName.Equals(b))))
                {
                    pot.Cast();
                    return;
                }
            }
        }
    }
}
