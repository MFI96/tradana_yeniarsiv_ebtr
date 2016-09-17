using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace SwagZilean
{
    internal class MenuX
    {
        public static Menu Zilean, Combo, Harass, Misc, UltMenu, LaneClear, Draw;
        public static Slider SkinSelect, ComboSlider, PredictionSlider;
        public static string[] CombosZileans = {"Fast Zilean", "Slow Zilean", "Focus AD Zilean"};
        public static string[] PredicOptions = {"Low", "Medium", "High"};

        public static void getMenu()
        {
            Zilean = MainMenu.AddMenu("SwagZilean", "SwagZilean");
            Zilean.AddGroupLabel("Swaaaaaaaaaaaaaaaaaaaaaaag Zilean");
            Zilean.AddSeparator();
            Zilean.AddLabel("İsteyen yorik100");
            Zilean.AddLabel("Yapan Kk2");
            Zilean.AddLabel("Çeviren-TRAdana");
            /*
            Combo Menu
            */
            Combo = Zilean.AddSubMenu("Kombo", "Combo");
            Combo.AddGroupLabel("Kombo Ayarları");
            Combo.AddSeparator();
            Combo.Add("comboQ", new CheckBox("Komboda Q Kullan"));
            Combo.Add("comboW", new CheckBox("Komboda W Kullan"));
            Combo.Add("comboE", new CheckBox("Komboda E Kullan"));
            Combo.AddSeparator();
            ComboSlider = Combo.Add("whatcombo", new Slider("Kombo Modunu Seç: ", 0, 0, 2));
            ComboSlider.OnValueChange +=
                delegate { ComboSlider.DisplayName = "Choose your Combo: " + CombosZileans[ComboSlider.CurrentValue]; };
            ComboSlider.DisplayName = "Choose your Combo: " + CombosZileans[ComboSlider.CurrentValue];
            Combo.AddSeparator();
            PredictionSlider = Combo.Add("dPrediction", new Slider("Büyü Tahmini: ", 2, 0, 2));
            PredictionSlider.OnValueChange +=
                delegate
                {
                    PredictionSlider.DisplayName = "Spell Prediction: " + PredicOptions[PredictionSlider.CurrentValue];
                };
            PredictionSlider.DisplayName = "Spell Prediction: " + PredicOptions[PredictionSlider.CurrentValue];

            /*
            Harass Menu
            */
            Harass = Zilean.AddSubMenu("Dürtme", "Harass");
            Harass.AddGroupLabel("Dürtme Ayarları");
            Harass.AddSeparator();
            Harass.Add("harassQ", new CheckBox("Dürtmede Q Kullan"));
            Harass.Add("harrasW", new CheckBox("Dürtmede W Kullan"));
            Harass.Add("harrasE", new CheckBox("Dürtmede E Kullan"));
            Harass.AddSeparator();
            Harass.Add("hManaSlider", new Slider("Dürtme için gereken mana % ", 20));

            /*
            LaneClear Menu
            */
            LaneClear = Zilean.AddSubMenu("LaneTemizleme", "LaneClear");
            LaneClear.AddGroupLabel("LaneTemizleme Ayarları");
            LaneClear.AddSeparator();
            LaneClear.Add("laneQ", new CheckBox("LaneTemizlemede Q Kullan"));
            LaneClear.Add("laneW", new CheckBox("LaneTemizlemede W Kullan"));
            LaneClear.AddSeparator();
            LaneClear.Add("lManaSlider", new Slider("LaneTemizleme için gereken mana % ", 20));

            /*
            Ult Menu
            */
            UltMenu = Zilean.AddSubMenu("Ulti", "UltMenu");
            UltMenu.AddGroupLabel("Ulti Ayarları");
            UltMenu.AddSeparator();
            foreach (var h in EntityManager.Heroes.Allies)
            {
                UltMenu.AddSeparator();
                UltMenu.Add("r" + h.ChampionName, new CheckBox("Ult Aktif " + h.ChampionName));
                UltMenu.AddSeparator();
                UltMenu.Add("rpct" + h.ChampionName, new Slider("Can % " + h.ChampionName, 10));
            }

            /*
            Misc Menu
            */
            Misc = Zilean.AddSubMenu("Ek", "Misc");
            Misc.AddGroupLabel("Ek Ayarlar");
            Misc.AddSeparator();
            Misc.Add("Support", new CheckBox("Destek Modu"));
            Misc.Add("gapCloser", new CheckBox("GapCloser E Kullan"));
            Misc.Add("Interrupt", new CheckBox("Interrupt Çift Q Atma Kullan"));
            Misc.AddSeparator();
            SkinSelect = Misc.Add("skinX", new Slider("Skin Değiştirici [Numarası]:", 5, 0, 5));

            /*
            Drawings Menu
            */
            Draw = Zilean.AddSubMenu("Göstergeler", "Drawings");
            Draw.AddGroupLabel("Gösterge Ayarları");
            Draw.AddSeparator();
            Draw.Add("drawQ", new CheckBox("Göster Q Menzili"));
            Draw.Add("drawE", new CheckBox("Göster E Menzili"));
            Draw.Add("drawR", new CheckBox("Göster R Menzili"));
            Draw.Add("cMode", new CheckBox("Göster Mevcut Kombo Modu"));
        }
    }
}