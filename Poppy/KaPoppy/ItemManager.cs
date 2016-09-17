using System;
using EloBuddy;
using EloBuddy.SDK;

namespace KaPoppy
{
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;
    using Config = ItemsSettings;
    internal class ItemManager
    {
        public static Menu Items;
        internal static void Init()
        {
            Items = Settings.Menu.AddSubMenu("Item manager");

            Items.AddGroupLabel("Combo");
            Items.Add("ComboHydra", new CheckBox("Use Hydra/Tiamat"));

            Items.AddGroupLabel("Harass");
            Items.Add("HarassHydra", new CheckBox("Use Hydra/Tiamat", false));

            Items.AddGroupLabel("Laneclear");
            Items.Add("LaneclearHydra", new CheckBox("Use Hydra/Tiamat"));

            Items.AddGroupLabel("Jungleclear");
            Items.Add("JungleclearHydra", new CheckBox("Use Hydra/Tiamat"));

            Orbwalker.OnPostAttack += Orbwalker_OnPostAttack;
        }

        private static void Orbwalker_OnPostAttack(AttackableUnit target, EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                if (Config.UseHydra("Combo") && Player.Instance.Distance(target.Position) < 250)
                {
                    if (HasHydra())
                    {
                        CastHydra();
                    }
                }
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                if (Config.UseHydra("Harass") && Player.Instance.Distance(target.Position) < 250)
                {
                    if (HasHydra())
                    {
                        CastHydra();
                    }
                }
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                if (Config.UseHydra("Laneclear") && Player.Instance.Distance(target.Position) < 250)
                {
                    if (HasHydra())
                    {
                        CastHydra();
                    }
                }
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                if (Config.UseHydra("Jungleclear") && Player.Instance.Distance(target.Position) < 250)
                {
                    if (HasHydra())
                    {
                        CastHydra();
                    }
                }
            }
        }

        public static bool HasHydra()
        {
            return Item.CanUseItem(3077) || Item.CanUseItem(3074) || Item.CanUseItem(3748);
        }
        public static void CastHydra()
        {
            Item.UseItem(3077);
            Item.UseItem(3074);
            Item.UseItem(3748);
        }
    }
    public class ItemsSettings : Helper
    {
        public static bool UseHydra(string mode)
        {
            return CastCheckbox(ItemManager.Items, mode + "Hydra");
        }
    }
}