using EloBuddy;
using EloBuddy.SDK;

namespace RekSai
{
    public static class ItemsManager
    {
        public static void HailHydra()
        {
            if (Item.HasItem(3074) && Item.CanUseItem(3074)) Item.UseItem(3074); //hydra
            if (Item.HasItem(3077) && Item.CanUseItem(3077)) Item.UseItem(3077); //tiamat
            if (Item.HasItem(3748) && Item.CanUseItem(3748)) Item.UseItem(3748); //titanic             
        }

        public static void Yomuus()
        {
            if (Item.HasItem(3142) && Item.CanUseItem(3142)) Item.UseItem(3142); Item.UseItem(3142);
        }


        public static void Initialize()
        {

        }
    }
}