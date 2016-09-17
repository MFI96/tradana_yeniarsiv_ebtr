using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;

namespace Mario_s_Activator
{
    internal class Offensive
    {
        public static List<Item> OffensiveItems = new List<Item>
        {
            new Item(ItemId.Bilgewater_Cutlass, 550),
            new Item(ItemId.Blade_of_the_Ruined_King, 550),
            new Item(ItemId.Tiamat, 380),
            new Item(ItemId.Ravenous_Hydra, 380),
            new Item(ItemId.Titanic_Hydra, Player.Instance.GetAutoAttackRange()),
            new Item(ItemId.Youmuus_Ghostblade, 1000),
            new Item(ItemId.Hextech_Gunblade, 700),
            new Item(ItemId.Frost_Queens_Claim)
        };
    }

    internal class Defensive
    {
        public static List<Item> DefensiveItems = new List<Item>
        {
            new Item(ItemId.Zhonyas_Hourglass),
            new Item(ItemId.Seraphs_Embrace),
            new Item(ItemId.Face_of_the_Mountain, 1050),
            new Item(ItemId.Talisman_of_Ascension),
            new Item(ItemId.Locket_of_the_Iron_Solari, 600),
            new Item(ItemId.Randuins_Omen, 500),
            new Item(ItemId.Ohmwrecker, 850)
        };
    }

    internal class Cleansers
    {
        public static List<Item> CleansersItems = new List<Item>
        {
            new Item(ItemId.Mikaels_Crucible, 750),
            new Item(ItemId.Dervish_Blade),
            new Item(ItemId.Mercurial_Scimitar),
            new Item(ItemId.Quicksilver_Sash),
        };
    }

    internal class Consumables
    {
        public static List<Item> ComsumableItems = new List<Item>
        {
            new Item(ItemId.Health_Potion),
            //Biscuit
            new Item(ItemId.Total_Biscuit_of_Rejuvenation),
            new Item(ItemId.Hunters_Potion),
            new Item(ItemId.Corrupting_Potion),
            new Item(ItemId.Refillable_Potion),
            new Item(ItemId.Elixir_of_Iron),
            new Item(ItemId.Elixir_of_Wrath),
            new Item(ItemId.Elixir_of_Sorcery),
        };
    }

    internal class WardsAndTrinkets
    {
        public static List<Item> Detectors = new List<Item>
        {
            new Item(ItemId.Sweeping_Lens_Trinket, 650),
            new Item(ItemId.Oracle_Alteration, 650),
            new Item(ItemId.Vision_Ward, 650),
        };

        public static List<Item> WardsAndTrinketsItems = new List<Item>
        {
            new Item(ItemId.Greater_Stealth_Totem_Trinket, 650),
            new Item(ItemId.Warding_Totem_Trinket, 650),
            new Item(ItemId.Farsight_Alteration, 1000),
        };
    }
}
