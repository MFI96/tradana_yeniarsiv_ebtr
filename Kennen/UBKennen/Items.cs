using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace UBKennen
{
    class Items
    {
        public static Item BilgewaterCutlass { get; private set; }
        public static Item BladeOfTheRuinedKing { get; private set; }
        public static Item ZhonyaHourglass { get; private set; }

        public static void InitItems()
        {
            BilgewaterCutlass = new Item(ItemId.Bilgewater_Cutlass);
            BladeOfTheRuinedKing = new Item(ItemId.Blade_of_the_Ruined_King);
            ZhonyaHourglass = new Item(ItemId.Zhonyas_Hourglass);


            Game.OnTick += OnTick;
        }

        private static void OnTick(EventArgs args)
        {
            if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) return;

            var target = Orbwalker.LastTarget as AIHeroClient;
            if (target == null) return;

            if (BilgewaterCutlass.IsOwned())
            {
                UseItem1(BilgewaterCutlass, target);
            }

            if (BladeOfTheRuinedKing.IsOwned())
            {
                UseItem2(BladeOfTheRuinedKing, target);
            }     
     
            if (ZhonyaHourglass.IsOwned())
            {
                UseItem3(ZhonyaHourglass, target);
            }
            if (ZhonyaHourglass.IsOwned())
            {
                Special(ZhonyaHourglass, target);
            }
        }

        private static void UseItem1(Item item, AIHeroClient target)
        {
            if (!target.IsValidTarget(550)
                || !Config.MiscMenu["item.1"].Cast<CheckBox>().CurrentValue
                || Config.MiscMenu["item.1MyHp"].Cast<Slider>().CurrentValue < Player.Instance.HealthPercent
                || Config.MiscMenu["item.1EnemyHP"].Cast<Slider>().CurrentValue < target.HealthPercent)
            {
                return;
            }

            var slot1 = Player.Instance.InventoryItems.FirstOrDefault(x => x.Id == item.Id);
            if (slot1 != null && Player.GetSpell(slot1.SpellSlot).IsReady)
            {
                Player.CastSpell(slot1.SpellSlot, target);
            }
        }
        private static void UseItem2(Item item, AIHeroClient target)
        {
            if (!target.IsValidTarget(550)
                || !Config.MiscMenu["item.2"].Cast<CheckBox>().CurrentValue
                || Config.MiscMenu["item.2MyHp"].Cast<Slider>().CurrentValue < Player.Instance.HealthPercent
                || Config.MiscMenu["item.2EnemyHP"].Cast<Slider>().CurrentValue < target.HealthPercent)
            {
                return;
            }

            var slot2 = Player.Instance.InventoryItems.FirstOrDefault(x => x.Id == item.Id);
            if (slot2 != null && Player.GetSpell(slot2.SpellSlot).IsReady)
            {
                Player.CastSpell(slot2.SpellSlot, target);
            }
        }
        private static void UseItem3(Item item, AIHeroClient target)
        {
            if (!target.IsValidTarget(550)
                || !Config.MiscMenu["item.3"].Cast<CheckBox>().CurrentValue
                || Config.MiscMenu["item.3MyHp"].Cast<Slider>().CurrentValue < Player.Instance.HealthPercent)               
            {
                return;
            }

            var slot3 = Player.Instance.InventoryItems.FirstOrDefault(x => x.Id == item.Id);
            if (slot3 != null && Player.GetSpell(slot3.SpellSlot).IsReady)
            {
                Player.CastSpell(slot3.SpellSlot);
            }
        }
        private static void Special(Item item, AIHeroClient target)
        {
            if (!target.IsValidTarget(550)
                || !Config.MiscMenu["item.4"].Cast<CheckBox>().CurrentValue
                || Config.MiscMenu["item.4mng"].Cast<Slider>().CurrentValue <= target.CountEnemiesInRange(400))
            {
                return;
            }

            var slot4 = Player.Instance.InventoryItems.FirstOrDefault(x => x.Id == item.Id);
            if (slot4 != null 
                && Spells.R.IsReady()
                && target.CountEnemiesInRange(450) >= Config.MiscMenu["item.4mng"].Cast<Slider>().CurrentValue 
                && Player.GetSpell(slot4.SpellSlot).IsReady)
            {
                Spells.R.Cast();
                Player.CastSpell(slot4.SpellSlot);
            }
        }
    }
}
