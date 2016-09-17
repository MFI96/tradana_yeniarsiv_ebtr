using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System.Reflection;
namespace KaPoppy
{
    class Settings : Helper
    {
        public static Menu Menu, Combo, Harass, Laneclear, Jungleclear, Flee, Misc, WSettings;
        public static void Init()
        {
            Menu = MainMenu.AddMenu("KaPoppy", "menu");
            Menu.AddGroupLabel("KaPoppy by Capitão Addon");
            Menu.AddSeparator();
            Menu.AddSeparator();
            Menu.AddSeparator();
            Menu.AddLabel("Current Version: " + Assembly.GetExecutingAssembly().GetName().Version);

            ComboMenu();
        }
        private static void ComboMenu()
        {
            Combo = Menu.AddSubMenu("Combo");
            Combo.Add("Passive", new CheckBox("Otomatik Saldır eğer minnyon ölecekse & passive is up"));
            Combo.Add("Q", new CheckBox("Kullan Q"));
            Combo.Add("W", new CheckBox("Kullan W", false));
            Combo.Add("Ws", new CheckBox("Can yüzde 40ın altındaysa W kullan", false));
            Combo.Add("E", new CheckBox("Kullan E"));
            Combo.Add("EStun", new CheckBox("Sabitleme İçin E Kullan"));
            Combo.Add("EInsec", new CheckBox("Eğer insec yapabileceksen E kullan"));
            Combo.Add("EPassive", new CheckBox("E pasif kalkanı kullan", false));
            Combo.Add("FEs", new KeyBind("Flash+E ile Hedefi Sabitle", false, KeyBind.BindTypes.PressToggle, 'J'));
            Combo.Add("R", new CheckBox("R ile hedefi uçur"));
            Combo.Add("Rm", new Slider("R için gereken şampiyon", 2, 1, 5));
            HarassMenu();
        }
        private static void HarassMenu()
        {
            Harass = Menu.AddSubMenu("Dürtme");
            Harass.Add("Q", new CheckBox("Kullan Q"));
            Harass.Add("Qm", new Slider("Q için en az mana %", 40, 0, 100));
            Harass.Add("W", new CheckBox("Kullan W", false));
            Harass.Add("Ws", new CheckBox("Canım Yüzde 40ın altındaysa W Kullan", false));
            Harass.Add("Wm", new Slider("W için en az mana %", 40, 0, 100));
            Harass.Add("E", new CheckBox("Kullan E"));
            Harass.Add("EStun", new CheckBox("Eğer E sabitleyebilecekse"));
            Harass.Add("EInsec", new CheckBox("İnsec yapabileceksen E Kullan"));
            Harass.Add("EPassive", new CheckBox("Use E to catch passive shield"));
            Harass.Add("EAlways", new CheckBox("Always use E", false));
            LaneclearMenu();
        }
        private static void LaneclearMenu()
        {
            Laneclear = Menu.AddSubMenu("LaneTemizleme");
            Laneclear.Add("Q", new CheckBox("Kullan Q"));
            Laneclear.Add("Qs", new Slider("Use Q if will hit {0} minions", 3, 1, 10));
            Laneclear.Add("Qm", new Slider("Q için en az mana %", 40, 0, 100));

            JungleclearMenu();
        }
        private static void JungleclearMenu()
        {
            Jungleclear = Menu.AddSubMenu("OrmanTemizleme");
            Jungleclear.Add("Q", new CheckBox("Kullan Q"));
            Jungleclear.Add("Qm", new Slider("Q için en az mana", 20, 0, 100));
            Jungleclear.Add("W", new CheckBox("Kullan W", false));
            Jungleclear.Add("Ws", new CheckBox("W için canım yüzde 40dan az", false));
            Jungleclear.Add("Wm", new Slider("W için en az mana", 40, 0, 100));
            Jungleclear.Add("E", new CheckBox("Kullan E"));

            FleeMenu();
        }
        private static void FleeMenu()
        {
            Flee = Menu.AddSubMenu("Flee");
            Flee.Add("E", new CheckBox("Fare Pozisyonunun en yakınındaki minyona E kullan"));
            Flee.Add("W", new CheckBox("Kullan W", false));
            Flee.Add("Ws", new CheckBox("Canım yüzde 40tan az ise W Kullan", false));
            Flee.Add("R", new CheckBox("R kullan senin düşmanlarını uçurmak için"));
            MiscMenu();
        }
        private static void MiscMenu()
        {
            Misc = Menu.AddSubMenu("Ek");
            Misc.AddGroupLabel("Misc");
            Misc.Add("MinimumDistanceToFlash", new Slider("Flash+Sabitleme için mininum mesafe", 250, 0, 500));
            Misc.Add("percent", new Slider("Sabitleme İsabet Şansı", 40, 1, 100));
            Misc.Add("stun", new KeyBind("Seçili Hedefi Sabitlemek için hazır olma tuşu", false, KeyBind.BindTypes.HoldActive, 'K'));
            Misc.AddLabel("Nasıl Çalışır: Sana en yakın hedefe göre çalışır orbwalker");
            Misc.AddLabel("Seçili Hedefi Sabitle");
            Misc.AddLabel("(Eğer flash+ aktifse , Flash+E olacak(kullanacak diyor sanırım)");
            Misc.AddSeparator(0);
            Misc.Add("semiR", new KeyBind("Yarı Otomatik R (Hedefi Sol Tuşla Seç)", false, KeyBind.BindTypes.HoldActive, 'G'));
            Misc.AddSeparator();
            Misc.AddGroupLabel("Göstergeler");
            Misc.Add("dQ", new CheckBox("Göster Q"));
            Misc.Add("dE", new CheckBox("Göster E"));
            Misc.Add("dR", new CheckBox("Göster R"));
            Misc.Add("DrawStunPos", new CheckBox("Sabitleme pozisyonunu göster"));
            Misc.AddGroupLabel("Kill Çalma");
            Misc.Add("Q", new CheckBox("Q Kullan"));
            Misc.Add("E", new CheckBox("E Kullan"));
            Misc.Add("R", new CheckBox("R Kullan"));

            WSettingsMenu();
        }
        public static void WSettingsMenu()
        {
            WSettings = Menu.AddSubMenu("AntiGapcloser", "wmenu");

            WSettings.Add("W", new CheckBox("Kullan W"));
            WSettings.AddGroupLabel("Kullan W içinde: ");

            foreach (AIHeroClient enemy in EntityManager.Heroes.Enemies)
            {
                foreach (var dash in DashSpells.DashSpells.Dashes)
                {
                    if (enemy.Hero == dash.champ)
                    {
                        if (dash.spellname == string.Empty)
                            WSettings.Add("w" + dash.champname + dash.spellKey, new CheckBox(dash.champname + " " + dash.spellKey, dash.enabled));
                        else
                            WSettings.Add("w" + dash.champname + dash.spellname, new CheckBox(dash.champname + " " + dash.spellKey, dash.enabled));
                    }
                    if (enemy.Hero == Champion.Rengar)
                    {
                        WSettings.Add("AntiRengar", new CheckBox("Anti rengar"));
                        //Chat.Print("Anti rengo loaded");
                        Chat.Print("Popy Wsi Rengarı Engellemez beni suclama, ritoyu sucla,tradana iyi oyunlar diler");
                    }
                }
            }
        }
        public static class ComboSettings
        {
            public static bool UsePassive
            {
                get { return CastCheckbox(Combo, "Passive"); }
            }
            public static bool UseQ
            {
                get { return CastCheckbox(Combo, "Q"); }
            }
            public static bool UseW
            {
                get { return CastCheckbox(Combo, "W"); }
            }
            public static bool Whealth
            {
                get { return CastCheckbox(Combo, "Ws"); }
            }
            public static bool UseE
            {
                get { return CastCheckbox(Combo, "E"); }
            }
            public static bool UseEStun
            {
                get { return CastCheckbox(Combo, "EStun"); }
            }
            public static bool UseEInsec
            {
                get { return CastCheckbox(Combo, "EInsec"); }
            }
            public static bool UseEPassive
            {
                get { return CastCheckbox(Combo, "EPassive"); }
            }
            public static bool UseFlashE
            {
                get { return CastKeybind(Combo, "FEs"); }
            }
            public static bool UseR
            {
                get { return CastCheckbox(Combo, "R"); }
            }
            public static int RMin
            {
                get { return CastSlider(Combo, "Rm"); }
            }
        }
        public static class HarassSettings
        {
            public static bool UseQ
            {
                get { return CastCheckbox(Harass, "Q") &&
                        CastSlider(Harass, "Qm") <= myHero.ManaPercent;
                }
            }
            public static bool UseW
            {
                get { return CastCheckbox(Harass, "W")  &&
                        CastSlider(Harass, "Wm") <= myHero.ManaPercent; }
            }
            public static bool Whealth
            {
                get { return CastCheckbox(Harass, "Ws"); }
            }
            public static bool UseE
            {
                get { return CastCheckbox(Harass, "E"); }
            }
            public static bool UseEStun
            {
                get { return CastCheckbox(Harass, "EStun"); }
            }
            public static bool UseEInsec
            {
                get { return CastCheckbox(Harass, "EInsec"); }
            }
            public static bool UseEPassive
            {
                get { return CastCheckbox(Harass, "EPassive"); }
            }
            public static bool UseEAlways
            {
                get { return CastCheckbox(Harass, "EAlways"); }
            }
        }
        public static class LaneclearSettings
        {
            public static bool UseQ
            {
                get { return CastCheckbox(Laneclear, "Q") && 
                        CastSlider(Laneclear, "Qm") <= myHero.ManaPercent; }
            }
            public static int Qmin
            {
                get { return CastSlider(Laneclear, "Qs"); }
            }
        }
        public static class JungleclearSettings
        {
            public static bool UseQ
            {
                get {
                    return CastCheckbox(Jungleclear, "Q") &&
                      CastSlider(Jungleclear, "Qm") <= myHero.ManaPercent;
                }
            }
            public static bool UseW
            {
                get {
                    return CastCheckbox(Jungleclear, "W") &&
                      CastSlider(Jungleclear, "Wm") <= myHero.ManaPercent;
                }
            }
            public static bool Whealth
            {
                get { return CastCheckbox(Jungleclear, "Ws"); }
            }
            public static bool UseE
            {
                get { return CastCheckbox(Jungleclear, "E"); }
            }
        }
        public static class FleeSettings
        {
            public static bool UseE
            {
                get { return CastCheckbox(Flee, "E"); }
            }
            public static bool UseW
            {
                get { return CastCheckbox(Flee, "W"); }
            }
            public static bool Whealth
            {
                get { return CastCheckbox(Flee, "Ws"); }
            }
            public static bool UseR
            {
                get { return CastCheckbox(Flee, "R"); }
            }
        }
        public static class MiscSettings
        {
            public static bool drawQ
            {
                get { return CastCheckbox(Misc, "dQ"); }
            }
            public static bool drawE
            {
                get { return CastCheckbox(Misc, "dE"); }
            }
            public static bool drawR
            {
                get { return CastCheckbox(Misc, "dR"); }
            }
            public static bool DrawStunPos
            {
                get { return CastCheckbox(Misc, "DrawStunPos"); }
            }
            public static bool UseQ
            {
                get { return CastCheckbox(Misc, "Q"); }
            }
            public static bool UseE
            {
                get { return CastCheckbox(Misc, "E"); }
            }
            public static bool UseR
            {
                get { return CastCheckbox(Misc, "R"); }
            }
            public static bool AntiGapcloser
            {
                get { return CastCheckbox(WSettings, "W"); }
            }
            public static int StunPercent
            {
                get { return CastSlider(Misc, "percent"); }
            }
            public static int MinimumDistanceToFlash
            {
                get { return CastSlider(Misc, "MinimumDistanceToFlash"); }
            }
            public static bool WEnabled(Champion champ, SpellSlot slot)
            {
                foreach (var dash in DashSpells.DashSpells.Dashes)
                {
                    if (dash.champ == champ)
                    {
                        if (dash.spellKey == slot)
                        {
                            return CastCheckbox(WSettings, "w" + dash.champname + dash.spellKey);
                        }
                    }
                }
                return false;
            }
            public static bool WEnabled(Champion champ, string spellName)
            {
                foreach (var dash in DashSpells.DashSpells.Dashes)
                {
                    if (dash.champ == champ)
                    {
                        if (dash.spellname == spellName)
                        {
                            return CastCheckbox(WSettings, "w" + dash.champname + dash.spellname);
                        }
                    }
                }
                return false;
            }
            public static bool AntiRengo
            {
                get { return CastCheckbox(WSettings, "AntiRengar"); }
            }
            public static bool StunTarget
            {
                get { return CastKeybind(Misc, "stun"); }
            }
            public static bool SemiAutoR
            {
                get { return CastKeybind(Misc, "semiR"); }
            }
        }
    }
}