namespace Khappa_Zix.Load
{
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    internal class menu
    {
        public static Menu MenuIni, Combo, Harass, Misc, Clear, KillSteal, Mana, Jump, Draw;

        internal static void Load()
        {
            MenuIni = MainMenu.AddMenu("Khappa'Zix", "Khappa'Zix");

            Jump = MenuIni.AddSubMenu("JumpsHandler ", "JumpsHandler");
            Jump.AddGroupLabel("E Ayarları");
            Jump.Add("double", new CheckBox("Çift zıplamak için E", false));
            Jump.Add("block", new CheckBox("Block if will land on a wall"));
            Jump.Add("delay", new Slider("2.E gecikmesi {0}", 150, 0, 300));
            Jump.AddGroupLabel("1st Zıplama");
            Jump.Add("1jump", new ComboBox("1st Jump", 0, "To Base", "To Ally", "To Mouse", "To Next Target"));
            Jump.AddGroupLabel("2nd Zıplama");
            Jump.Add("2jump", new ComboBox("2nd Jump", 0, "To Base", "To Ally", "To Mouse", "To Next Target"));
            Jump.AddSeparator();
            Jump.AddGroupLabel("Ekstra Ayarları");
            Jump.AddLabel("Kaç Kuleler");
            Jump.Add("save", new CheckBox("Düşman kule menzilinden kaç"));
            Jump.Add("saveh", new Slider("Canım şundan azsa kaç", 15));

            Combo = MenuIni.AddSubMenu("Combo ", "Combo");
            Combo.AddGroupLabel("Combo Ayarları");
            Combo.Add("Q", new CheckBox("Kullan Q "));
            Combo.Add("W", new CheckBox("Kullan W "));
            Combo.Add("E", new CheckBox("Kullan E "));
            Combo.AddSeparator();
            Combo.AddGroupLabel("E Ayarları");
            Combo.Add("Edive", new CheckBox("E İle kuleye dal"));
            Combo.Add("safe", new Slider("Eğer hedefin yakınında şu kadar düşman varsa E at {0}", 3, 0, 5));
            Combo.Add("dis", new Slider("Hedefe menzilim şu kadarsa kullan {0}", 385, 0, 850));
            Combo.AddSeparator();
            Combo.AddGroupLabel("R Ayarları");
            Combo.Add("useR", new CheckBox("Kullan R"));
            Combo.Add("R", new CheckBox("Büyülerim Hazırsa R Kullan"));
            Combo.Add("NoAA", new CheckBox("R aktifken AA yapma"));
            Combo.Add("Rmode", new ComboBox("R Modu", 0, "GapClose For Combo", "Always"));
            Combo.Add("danger", new Slider("Şu kadar düşman bana yakınsa R {0}", 3, 1, 5));

            Harass = MenuIni.AddSubMenu("Harass ", "Harass");
            Harass.AddGroupLabel("Dürtme Ayarları");
            Harass.Add("Q", new CheckBox("Kullan Q "));
            Harass.Add("W", new CheckBox("Kullan W "));
            Harass.Add("E", new CheckBox("Kullan E "));
            Harass.Add("Edive", new CheckBox("E Dive Towers"));

            Clear = MenuIni.AddSubMenu("Clear ", "Clear");
            Clear.AddGroupLabel("LaneClear Ayarları");
            Clear.Add("Qc", new CheckBox("Kullan Q "));
            Clear.Add("Wc", new CheckBox("Kullan W "));
            Clear.Add("Ec", new CheckBox("Kullan E ", false));
            Clear.AddSeparator();
            Clear.AddGroupLabel("LastHit Ayarları");
            Clear.Add("Qh", new CheckBox("Kullan Q "));
            Clear.Add("Wh", new CheckBox("Kullan W "));
            Clear.Add("Eh", new CheckBox("Kullan E ", false));
            Clear.AddSeparator();
            Clear.AddGroupLabel("JungleClear Ayarları");
            Clear.Add("Qj", new CheckBox("Kullan Q "));
            Clear.Add("Wj", new CheckBox("Kullan W "));
            Clear.Add("Ej", new CheckBox("Kullan E ", false));

            Mana = MenuIni.AddSubMenu("ManaManager ", "ManaManager");
            Mana.AddGroupLabel("Harass Mana");
            Mana.Add("harass", new Slider("Manam şundan fazla", 60));
            Mana.AddSeparator();
            Mana.AddGroupLabel("LaneClear Mana");
            Mana.Add("lane", new Slider("Manam şundan fazla", 75));
            Mana.AddSeparator();
            Mana.AddGroupLabel("LastHit Mana");
            Mana.Add("last", new Slider("Manam şundan fazla", 50));
            Mana.AddSeparator();
            Mana.AddGroupLabel("JungleClear Mana");
            Mana.Add("jungle", new Slider("Manam şundan fazla", 30));

            KillSteal = MenuIni.AddSubMenu("KillSteal ", "KillSteal");
            KillSteal.AddGroupLabel("KillÇalma Ayarları");
            KillSteal.Add("Q", new CheckBox("Kullan Q "));
            KillSteal.Add("W", new CheckBox("Kullan W "));
            KillSteal.Add("E", new CheckBox("Kullan E "));

            Draw = MenuIni.AddSubMenu("Drawings ", "Drawings");
            Draw.AddGroupLabel("Gösterge Ayarları");
            Draw.Add("Q", new CheckBox("Göster Q "));
            Draw.Add("W", new CheckBox("Göster W "));
            Draw.Add("E", new CheckBox("Göster E "));

            Misc = MenuIni.AddSubMenu("Misc ", "Misc");
            Misc.AddGroupLabel("Büyüler İsabetOranı");
            Misc.Add("hitChance", new ComboBox("HitChance", 0, "High", "Medium", "Low"));
        }
    }
}