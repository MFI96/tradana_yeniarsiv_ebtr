using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace Black_Swan_Akali
{
    public static class Items
    {
        // Items
        public static Item bilge = new Item(ItemId.Bilgewater_Cutlass, 550);
        public static Item Botrk = new Item(ItemId.Blade_of_the_Ruined_King, 550);
        public static Item Hextech = new Item(ItemId.Hextech_Gunblade, 700);
        public static Item Tiamat = new Item(ItemId.Tiamat_Melee_Only, 400);
        public static Item Hydra = new Item(ItemId.Ravenous_Hydra_Melee_Only, 400);
        public static Item Titanic = new Item(ItemId.Titanic_Hydra, 125);

        public static List<Item>ItemList = new List<Item>
        {
            Tiamat,
            Hydra,
            bilge,
            Botrk,
            Hextech,
            Titanic
        };

        public static void CastItems(AIHeroClient target)
        {
            foreach (var item in ItemList.Where(i => i.IsReady() && target.IsValidTarget(i.Range)))
                item.Cast(target);
        }
    }
}
