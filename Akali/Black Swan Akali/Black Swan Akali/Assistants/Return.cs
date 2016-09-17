using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace Black_Swan_Akali
{
    public static class Return
    {
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

        // Combo Q
        public static bool UseQCombo
        {
            get { return GetCheckbox(MenuDesigner.ComboUI, "ComboQ"); }
        }

        // Combo W
        public static bool UseWCombo
        {
            get { return GetCheckbox(MenuDesigner.ComboUI, "ComboW"); }
        }

        // Combo E
        public static bool UseECombo
        {
            get { return GetCheckbox(MenuDesigner.ComboUI, "ComboE"); }
        }

        // Combo R
        public static bool UseRCombo
        {
            get { return GetCheckbox(MenuDesigner.ComboUI, "ComboR"); }
        }

        // Harass Q
        public static bool UseQHarass
        {
            get { return GetCheckbox(MenuDesigner.HarassUI, "HarassQ"); }
        }

        // Lasthit Q
        public static bool UseQLast
        {
            get { return GetCheckbox(MenuDesigner.ClearUI, "LastQ"); }
        }

        // Lasthit E
        public static bool UseELast
        {
            get { return GetCheckbox(MenuDesigner.ClearUI, "LastE"); }
        }

        // Clear Q
        public static bool UseQClear
        {
            get { return GetCheckbox(MenuDesigner.ClearUI, "ClearQ"); }
        }

        // Clear E
        public static bool UseEClear
        {
            get { return GetCheckbox(MenuDesigner.ClearUI, "ClearE"); }
        }

        // Clear E Min. Minions
        public static int ClearMinionMin
        {
            get { return GetSlider(MenuDesigner.ClearUI, "ClearEmin"); }
        }

        // Clear Energy
        public static int ClearEnergyMin
        {
            get { return GetSlider(MenuDesigner.ClearUI, "ClearMana"); }
        }

        // Jungle Q
        public static bool UseQJungle
        {
            get { return GetCheckbox(MenuDesigner.ClearUI, "JungleQ"); }
        }

        // Jungle E
        public static bool UseEJungle
        {
            get { return GetCheckbox(MenuDesigner.ClearUI, "JungleE"); }
        }

        // Jungle Energy
        public static int JungleEnergyMin
        {
            get { return GetSlider(MenuDesigner.ClearUI, "JungleMana"); }
        }

        // KS Q
        public static bool UseQKs
        {
            get { return GetCheckbox(MenuDesigner.KsUI, "KsQ"); }
        }

        // KS R
        public static bool UseRKs
        {
            get { return GetCheckbox(MenuDesigner.KsUI, "KsR"); }
        }

        // Misc R Anti Gapclose
        public static bool UseRGapclose
        {
            get { return GetCheckbox(MenuDesigner.MiscUI, "GapR"); }
        }

        // Misc FLee
        public static bool UseWFlee
        {
            get { return GetCheckbox(MenuDesigner.MiscUI, "FleeW"); }
        }

        // Misc Draw Q
        public static bool DrawQRange
        {
            get { return GetCheckbox(MenuDesigner.MiscUI, "DrawQ"); }
        }

        // Misc Draw R
        public static bool DrawRRange
        {
            get { return GetCheckbox(MenuDesigner.MiscUI, "DrawR"); }
        }

        // Misc Use Items
        public static bool UseAgressiveItems
        {
            get { return GetCheckbox(MenuDesigner.MiscUI, "UseItems"); }
        }
    }
}
