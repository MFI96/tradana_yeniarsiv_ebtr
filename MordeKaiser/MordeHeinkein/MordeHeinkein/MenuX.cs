using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace MordeHeinkein
{
    internal class MenuX
    {
        public static Menu Mordekaiser, Combo, Harass, LaneClear, JungleClear, Drawing, Misc;
        public static Slider skinSelect;
        // Init
        public static void GetMenu()
        {
            Mordekaiser = MainMenu.AddMenu("Mordekaiser", "Mordekaiser");

            Combo = Mordekaiser.AddSubMenu("Kombo", "Combo");
            Combo.AddGroupLabel("Kombo Ayarları");
            Combo.AddSeparator();
            Combo.Add("useQC", new CheckBox("Kullan Q"));
            Combo.Add("useEC", new CheckBox("Kullan E"));
            Combo.Add("useWC", new CheckBox("Kullan W"));
            Combo.Add("useRC", new CheckBox("Kullan R"));

            Harass = Mordekaiser.AddSubMenu("Dürtme", "Harass");
            Harass.AddGroupLabel("Dürtme Ayarları");
            Harass.AddSeparator();
            Harass.Add("useEH", new CheckBox("Kullan E"));
            Harass.Add("useQH", new CheckBox("Kullan Q"));
            Harass.Add("useWH", new CheckBox("Kullan W"));
            Harass.Add("HPSliderH", new Slider("HP % > Dürtme için", 20));

            LaneClear = Mordekaiser.AddSubMenu("LaneClear", "LaneClear");
            LaneClear.AddGroupLabel("Lane Temizleme Ayarları");
            LaneClear.AddSeparator();
            LaneClear.Add("UseEL", new CheckBox("Kullan E"));
            LaneClear.Add("UseQL", new CheckBox("Kullan Q"));

            JungleClear = Mordekaiser.AddSubMenu("JungleClear", "JungleClear");
            JungleClear.AddGroupLabel("Orman Temizleme Ayarları");
            JungleClear.AddSeparator();
            JungleClear.Add("UseEJ", new CheckBox("Kullan E"));
            JungleClear.Add("UseWJ", new CheckBox("Kullan W"));
            JungleClear.Add("UseQJ", new CheckBox("Kullan Q"));

            Misc = Mordekaiser.AddSubMenu("Ek", "misc");
            Misc.AddGroupLabel("Ek Ayarlar");
            Misc.AddSeparator();
            Misc.Add("UsePot", new CheckBox("Kullan İksirler"));
            Misc.Add("AutoPilot", new CheckBox("Otomatik Pilot Hayelet"));
            Misc.AddSeparator();
            skinSelect = Misc.Add("ChangeSkin", new Slider("Skin Değiştirici [Numarası]", 2, 0, 5));

            Drawing = Mordekaiser.AddSubMenu("Göstergeler", "Drawings");
            Drawing.AddGroupLabel("Gösterge Ayarları");
            Drawing.AddSeparator();
            Drawing.Add("drawQ", new CheckBox("Göster Q"));
            Drawing.Add("drawW", new CheckBox("Göster W"));
            Drawing.Add("drawE", new CheckBox("Göster E"));
            Drawing.Add("drawR", new CheckBox("Göster R"));
        }
    }
}