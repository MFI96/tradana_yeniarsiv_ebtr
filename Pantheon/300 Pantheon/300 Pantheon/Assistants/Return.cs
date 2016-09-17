using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace _300_Pantheon.Assistants
{
    public static class Return
    {
        /// <summary>
        ///     This function will make the life easy just checking the active mode instead of create 1000 bools
        /// </summary>
        /// <param name="id">Flag id of the orbwalker</param>
        /// <returns></returns>
        public static bool Activemode(Orbwalker.ActiveModes id)
        {
            return Orbwalker.ActiveModesFlags.HasFlag(id);
        }

        #region Combo

        // Combo Q
        public static bool UseQCombo
        {
            get { return GetCheckbox(MenuDesigner.ComboUi, "ComboQ"); }
        }

        // Combo W
        public static bool UseWCombo
        {
            get { return GetCheckbox(MenuDesigner.ComboUi, "ComboW"); }
        }

        // Combo E
        public static bool UseECombo
        {
            get { return GetCheckbox(MenuDesigner.ComboUi, "ComboE"); }
        }

        #endregion

        #region Harass

        // Harass Q
        public static bool UseQHarass
        {
            get { return GetCheckbox(MenuDesigner.HarassUi, "HarassQ"); }
        }

        // Harass Toggle
        public static bool HarassToggle
        {
            get { return GetKeybind(MenuDesigner.HarassUi, "ToggleHarass"); }
        }

        // Harass Min. Mana
        public static int HarassManaMin
        {
            get { return GetSlider(MenuDesigner.HarassUi, "HarassMana"); }
        }

        #endregion

        #region Clear

        // Lasthit Q
        public static bool UseQLast
        {
            get { return GetCheckbox(MenuDesigner.ClearUi, "LastQ"); }
        }

        // Clear Q
        public static bool UseQClear
        {
            get { return GetCheckbox(MenuDesigner.ClearUi, "ClearQ"); }
        }

        // Clear W
        public static bool UseWClear
        {
            get { return GetCheckbox(MenuDesigner.ClearUi, "ClearW"); }
        }

        // Clear E
        public static bool UseEClear
        {
            get { return GetCheckbox(MenuDesigner.ClearUi, "ClearE"); }
        }

        // Clear Min. Mana
        public static int ClearManaMin
        {
            get { return GetSlider(MenuDesigner.ClearUi, "ClearMana"); }
        }

        // Jungle Q
        public static bool UseQJungle
        {
            get { return GetCheckbox(MenuDesigner.ClearUi, "JungleQ"); }
        }

        // Jungle W
        public static bool UseWJungle
        {
            get { return GetCheckbox(MenuDesigner.ClearUi, "JungleW"); }
        }

        // Jungle E
        public static bool UseEJungle
        {
            get { return GetCheckbox(MenuDesigner.ClearUi, "JungleE"); }
        }

        #endregion

        #region Killsteal

        // KS Q
        public static bool UseQKs
        {
            get { return GetCheckbox(MenuDesigner.KsUi, "KsQ"); }
        }

        // KS W
        public static bool UseWKs
        {
            get { return GetCheckbox(MenuDesigner.KsUi, "KsW"); }
        }

        #endregion

        #region Misc

        // Misc W Interrupt
        public static bool UseWInterrupt
        {
            get { return GetCheckbox(MenuDesigner.MiscUi, "InterW"); }
        }

        // Misc W Anti Gapclose
        public static bool UseWGapclose
        {
            get { return GetCheckbox(MenuDesigner.MiscUi, "GapW"); }
        }

        // Misc Use Items
        public static bool UseAgressiveItems
        {
            get { return GetCheckbox(MenuDesigner.MiscUi, "UseItems"); }
        }

        // Misc Draw Q W E
        public static bool DrawQweRange
        {
            get { return GetCheckbox(MenuDesigner.MiscUi, "DrawSpells"); }
        }

        #endregion

        #region Menu Values

        // Return Checkbox Value
        public static bool GetCheckbox(Menu menu, string menuvalue)
        {
            return menu[menuvalue].Cast<CheckBox>().CurrentValue;
        }

        // Return Keybind Value
        public static bool GetKeybind(Menu menu, string menuvalue)
        {
            return menu[menuvalue].Cast<KeyBind>().CurrentValue;
        }

        // Return Slider Value
        public static int GetSlider(Menu menu, string menuvalue)
        {
            return menu[menuvalue].Cast<Slider>().CurrentValue;
        }

        #endregion
    }
}