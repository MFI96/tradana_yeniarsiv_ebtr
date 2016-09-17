#region

using EloBuddy;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

#endregion

namespace SimplisticTemplate.Champion.Fizz.Utils
{
    internal static class GameMenu
    {
        private static Menu _menu;

        public static Menu ComboMenu,
            HarassMenu,
            MiscMenu,
            DrawMenu;

        private static AIHeroClient Me
        {
            get { return ObjectManager.Player; }
        }

        public static void Initialize()
        {
            _menu = MainMenu.AddMenu("Simplistic " + Me.ChampionName, Me.ChampionName.ToLower());
            _menu.AddLabel("Simplistic Fizz");
            _menu.AddLabel("by nonm");

            ComboMenu = _menu.AddSubMenu("Combo", "combo");
            ComboMenu.AddLabel("Kombo Ayarları");
            ComboMenu.Add("qrcombo", new KeyBind("Q - R Kombo Tuşu", false, KeyBind.BindTypes.HoldActive, 'A'));
            ComboMenu.Add("useQ", new CheckBox("Kullan Q"));
            ComboMenu.Add("useW", new CheckBox("Kullan W"));
            ComboMenu.Add("useE", new CheckBox("Kullan E"));
            ComboMenu.Add("useR", new CheckBox("Kullan R"));
            ComboMenu.Add("useEGap", new CheckBox("Gapclose için E Kullan Ve ölecekse Q kullan?"));
            ComboMenu.Add("useRGap", new CheckBox("R Kullan ve Eğer Ölecekse Gapclose için E Kullan?"));

            HarassMenu = _menu.AddSubMenu("Harass", "harass");
            HarassMenu.AddLabel("Dürtme Ayarları");
            HarassMenu.Add("useQ", new CheckBox("Kullan Q"));
            HarassMenu.Add("useW", new CheckBox("Kullan W"));
            HarassMenu.Add("useE", new CheckBox("Kullan E"));
            HarassMenu.AddSeparator();
            HarassMenu.AddLabel("E Modes: (1) Geriye Doğru (2) Düşmana Doğru");
            HarassMenu.Add("useEMode", new Slider("E Modu", 0, 0, 1));

            MiscMenu = _menu.AddSubMenu("Misc", "misc");
            MiscMenu.AddLabel("ek Ayarlar");
            MiscMenu.AddLabel("Kullan W : (1) Önce Q (2) Düşmana");
            MiscMenu.Add("useWMode", new Slider("Kullan W", 0, 0, 1));
            MiscMenu.AddSeparator();
            MiscMenu.Add("useETower", new CheckBox("Kule vuruşunu E ile dodgele"));

            DrawMenu = _menu.AddSubMenu("Drawings", "drawings");
            DrawMenu.AddLabel("Gösterge Ayarları");
            DrawMenu.Add("disable", new CheckBox("Tüm göstergeleri kapat", false));
            DrawMenu.Add("drawDamage", new CheckBox("Göster Hasarı"));
            DrawMenu.Add("drawQ", new CheckBox("Göster Q Menzili"));
            DrawMenu.Add("drawW", new CheckBox("Göster W Menzili"));
            DrawMenu.Add("drawE", new CheckBox("Göster E Menzili"));
            DrawMenu.Add("drawR", new CheckBox("Göster R Menzili"));
            DrawMenu.Add("drawRPred", new CheckBox("Göster R Menzili"));
        }
    }
}