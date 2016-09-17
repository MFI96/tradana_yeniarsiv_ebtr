using System.Linq;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace Sorakinha
{
    internal static class MenuX
    {
        private static Menu Soraka;
        public static Menu Combo, Harass, Healing, Drawing, Misc;
        private static string[] PredictionSliderValues = {"Low", "Medium", "High"};
        public static Slider SkinSelect;
        /*
        Create the Menu ^.^
        */

        public static void CallMeNigga()
        {
            // Main Menu
            Soraka = MainMenu.AddMenu("Soraka", "Soraka");
            Soraka.AddGroupLabel("Sorakinha ^.^");
            Soraka.AddSeparator();
            Soraka.AddLabel("Can vermeye başla Adamım!");
            Soraka.AddLabel("Yapan Kk2 (:");

            // Combo Menu
            Combo = Soraka.AddSubMenu("Kombo", "Combo");
            Combo.AddGroupLabel("Kombo Ayarları >.<");
            Combo.AddSeparator();
            Combo.Add("useQCombo", new CheckBox("Kullan Q"));
            Combo.Add("useECombo", new CheckBox("Kullan E"));
            Combo.Add("minMcombo", new Slider("Mana %", 20));

            // Harass Menu
            Harass = Soraka.AddSubMenu("Dürtme", "Harass");
            Harass.AddGroupLabel("Dürtme Ayarları ¬¬");
            Harass.AddSeparator();
            Harass.Add("useQHarass", new CheckBox("Kullan Q"));
            Harass.Add("useEHarass", new CheckBox("Kullan E"));
            Harass.Add("minMharass", new Slider("Dürtme için en az mana % ", 20));
            Harass.AddSeparator();
            var sliderValue = Harass.Add("predNeeded", new Slider("Prediction İsabet Oranı: ", 0, 0, 2));
            sliderValue.OnValueChange +=
                delegate
                {
                    sliderValue.DisplayName = "Prediction Hitchange: " + PredictionSliderValues[sliderValue.CurrentValue];
                };
            sliderValue.DisplayName = "Prediction Hitchange: " + PredictionSliderValues[sliderValue.CurrentValue];

            // Healing Menu
            Healing = Soraka.AddSubMenu("Can Ver", "Healing");
            Healing.AddGroupLabel("W Ayarları ~.~");
            Healing.AddSeparator();
            Healing.Add("useW", new CheckBox("Otomatik W"));
            Healing.Add("dontWF", new CheckBox("Dont W in Fountain"));
            Healing.AddSeparator();

            /** 
            The Magic ~ 
            **/
            foreach (var hero in EntityManager.Heroes.Allies.Where(x => !x.IsMe))
            {
                Healing.AddSeparator();
                Healing.Add("w" + hero.ChampionName, new CheckBox("Can " + hero.ChampionName));
                Healing.AddSeparator();
                Healing.Add("wpct" + hero.ChampionName, new Slider("Canı % " + hero.ChampionName, 45));
            }
            Healing.AddSeparator();
            Healing.AddGroupLabel("R Ayarları ^.~");
            Healing.AddSeparator();
            Healing.Add("useR", new CheckBox("Kullan R"));
            Healing.Add("useRslider", new Slider("Canlar Şu Kadar Olduğunda R Kullan", 10));
            /** 
            End of The Magic Kappa
            **/

            // Misc Menu
            Misc = Soraka.AddSubMenu("Ek", "Misc");
            Misc.AddGroupLabel("Ek Ayarlar 0.o");
            Misc.AddSeparator();
            Misc.Add("useQGapCloser", new CheckBox("GapCloser Q Kullan"));
            Misc.Add("useEGapCloser", new CheckBox("GapCloser E Kullan"));
            Misc.Add("eInterrupt", new CheckBox("Interrupt E Kullan"));
            Misc.Add("AttackMinions", new CheckBox("Minyonlara Saldır"));
            SkinSelect = Misc.Add("skinSelect", new Slider("Skin Değiştirici [Numarası]", 0, 0, 5));

            // Drawing Menu
            Drawing = Soraka.AddSubMenu("Gösterge", "Drawing");
            Drawing.AddGroupLabel("Gösterge Ayarları :~");
            Drawing.AddSeparator();
            Drawing.Add("drawQ", new CheckBox("Göster Q"));
            Drawing.Add("drawE", new CheckBox("Göster E"));
            Drawing.AddSeparator();
            Drawing.Add("drawH", new CheckBox("Göster Cana İhtiyacı OLan Hedefi"));
        }
    }
}