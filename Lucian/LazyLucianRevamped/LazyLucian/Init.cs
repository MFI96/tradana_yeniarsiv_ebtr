using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace LazyLucian
{
    internal class Init
    {
        public static Menu Menu, ComboMenu, HarassMenu, FarmMenu, MiscMenu, DrawMenu;

        public static void LoadMenu()
        {
            Bootstrap.Init(null);

            Menu = MainMenu.AddMenu("Lazy Lucian", "LazyLucian");
            Menu.AddGroupLabel("Lazy Lucian");
            Menu.AddLabel("by DamnedNooB");
            Menu.AddLabel("Transtlate Turkish-Çeviri TRAdana");
            Menu.AddSeparator();

            //-------------------------------------------------------------------------------------------------------------------
            /*
            *       _____                _             __  __                  
            *      / ____|              | |           |  \/  |                 
            *     | |     ___  _ __ ___ | |__   ___   | \  / | ___ _ __  _   _ 
            *     | |    / _ \| '_ ` _ \| '_ \ / _ \  | |\/| |/ _ \ '_ \| | | |
            *     | |___| (_) | | | | | | |_) | (_) | | |  | |  __/ | | | |_| |
            *      \_____\___/|_| |_| |_|_.__/ \___/  |_|  |_|\___|_| |_|\__,_|
            *                                                                  
            *                                                                  
            */

            ComboMenu = Menu.AddSubMenu("Kombo", "Combo");
            ComboMenu.AddGroupLabel("Kombo Ayarları");
            ComboMenu.AddLabel("Q - Işık Atışı");
            ComboMenu.Add("useQcombo", new CheckBox("Komboda Kullan"));
            ComboMenu.Add("useQextended", new CheckBox("Komboda Uzun Q Kullan(Minyon Arkasındakine vurur)"));
            ComboMenu.Add("qMana", new Slider("En az mana: ", 20, 1));
            ComboMenu.AddSeparator();

            ComboMenu.AddLabel("W - İntikam Ateşi");
            ComboMenu.Add("useWaaRange", new CheckBox("AA da kullan - Menzili"));
            ComboMenu.Add("useWalways", new CheckBox("AA dışında Kullan  - Menzili"));
            ComboMenu.Add("wMana", new Slider("en az mana: ", 20, 1));
            ComboMenu.AddSeparator();

            ComboMenu.AddLabel("E - Amansız Takip");
            ComboMenu.Add("useEcombo", new CheckBox("E mantığı Kullan"));
            ComboMenu.Add("useEmouse", new CheckBox("E yi Mouse yönünde Kullan"));
            ComboMenu.Add("eMana", new Slider("en az mana: ", 20, 1));
            ComboMenu.AddSeparator();

            ComboMenu.AddLabel("R - İnfaz");
            ComboMenu.Add("useRkillable", new CheckBox("Hedef Öldürülebilir Olduğunda"));
            ComboMenu.Add("useRlock", new CheckBox("Hedefe Kilitlen"));
            //ComboMenu.Add("rMana", new Slider("Min Mana to use: ", 20, 1));
            ComboMenu.AddSeparator();

            ComboMenu.AddLabel("Ek  Ayarlar (Kombo)");
            ComboMenu.Add("spellWeaving", new CheckBox("Pasifi Kullan (Işık Silahşoru)"));
            ComboMenu.Add("useYoumuu", new CheckBox("Kullan Youmuu's GhostBlade for The Culling"));

            //-------------------------------------------------------------------------------------------------------------------
            /*
            *      _    _                           __  __                  
            *     | |  | |                         |  \/  |                 
            *     | |__| | __ _ _ __ __ _ ___ ___  | \  / | ___ _ __  _   _ 
            *     |  __  |/ _` | '__/ _` / __/ __| | |\/| |/ _ \ '_ \| | | |
            *     | |  | | (_| | | | (_| \__ \__ \ | |  | |  __/ | | | |_| |
            *     |_|  |_|\__,_|_|  \__,_|___/___/ |_|  |_|\___|_| |_|\__,_|
            *                                                               
            *                                                               
            */

            HarassMenu = Menu.AddSubMenu("Dürtme", "Harass");
            HarassMenu.AddGroupLabel("Dürtme Ayarları");
            HarassMenu.AddLabel("Q - Işık Atışı");
            HarassMenu.Add("useQharass", new CheckBox("Dürtmede Kullan"));
            HarassMenu.Add("useQextended", new CheckBox("Dürtmede Uzun Q Kullan(Minyon Arkasındakine vurur)"));
            HarassMenu.Add("qMana", new Slider("en az mana: ", 20, 1));
            HarassMenu.AddSeparator();

            HarassMenu.AddLabel("W - İntikam Ateşi");
            HarassMenu.Add("useWaaRange", new CheckBox("AA'da Kullanma - Menzil"));
            HarassMenu.Add("useWalways", new CheckBox("AA dışında Kullanma - Menzili"));
            HarassMenu.Add("wMana", new Slider("en az mana: ", 20, 1));
            HarassMenu.AddSeparator();

            HarassMenu.AddLabel("Ek Ayarlar (Dürtme)");
            //HarassMenu.Add("manaCheck", new CheckBox("")); // soon(TM)
            HarassMenu.Add("spellWeaving", new CheckBox("Pasifi Kullan (Işık Silahşoru)"));

            //-------------------------------------------------------------------------------------------------------------------
            /*
            *      ______                     __  __                  
            *     |  ____|                   |  \/  |                 
            *     | |__ __ _ _ __ _ __ ___   | \  / | ___ _ __  _   _ 
            *     |  __/ _` | '__| '_ ` _ \  | |\/| |/ _ \ '_ \| | | |
            *     | | | (_| | |  | | | | | | | |  | |  __/ | | | |_| |
            *     |_|  \__,_|_|  |_| |_| |_| |_|  |_|\___|_| |_|\__,_|
            *                                                         
            *                                                         
            */

            FarmMenu = Menu.AddSubMenu("Farm", "Farm");
            FarmMenu.AddGroupLabel("Farm Ayarları");
            FarmMenu.AddLabel("Q - Işık Atışı");
            FarmMenu.Add("useQfarm", new CheckBox("Lane Temizlerken Kullan"));
            FarmMenu.Add("qManaLane", new Slider("Lanetemizleme için gerekli mana % : ", 20, 1));
            FarmMenu.Add("qMinionsLane", new Slider("Laneclear için gereken minyon: ", 3, 1, 5));
            FarmMenu.AddSeparator();
            FarmMenu.Add("useQjungle", new CheckBox("Orman Temizlemede Kullan"));
            FarmMenu.Add("qManaJungle", new Slider("Orman Temizleme için gereken mana: ", 20, 1));
            FarmMenu.AddSeparator();

            FarmMenu.AddLabel("W - İntikam Ateşi");
            FarmMenu.Add("useWfarm", new CheckBox("Lane Temizlerken Kullan"));
            FarmMenu.Add("wManaLane", new Slider("Lanetemizleme için gerekli mana: ", 20, 1));
            FarmMenu.AddSeparator();
            FarmMenu.Add("useWjungle", new CheckBox("Orman Temizlemede Kullan"));
            FarmMenu.Add("wManaJungle", new Slider("Orman Temizleme için gereken mana: ", 20, 1));
            FarmMenu.AddSeparator();

            FarmMenu.AddLabel("Ek Ayarlar (Farm)");
            FarmMenu.Add("spellWeaving", new CheckBox("Pasif Kullan (Işık Silahşoru)"));

            //-------------------------------------------------------------------------------------------------------------------
            /*
            *      __  __ _            __  __                  
            *     |  \/  (_)          |  \/  |                 
            *     | \  / |_ ___  ___  | \  / | ___ _ __  _   _ 
            *     | |\/| | / __|/ __| | |\/| |/ _ \ '_ \| | | |
            *     | |  | | \__ \ (__  | |  | |  __/ | | | |_| |
            *     |_|  |_|_|___/\___| |_|  |_|\___|_| |_|\__,_|
            *                                                  
            *                                                  
            */

            MiscMenu = Menu.AddSubMenu("Ek", "Misc");
            MiscMenu.AddGroupLabel("Ek Ayarlar");
            MiscMenu.AddLabel("Anti Gapcloser Ayarları");
            MiscMenu.Add("gapcloser", new CheckBox("E ile hedefi önleme"));
            MiscMenu.Add("gapcloserT", new CheckBox("E ile hedefi önle"));
            MiscMenu.AddSeparator();
            MiscMenu.AddGroupLabel("Diğer Ayarlar");
            MiscMenu.Add("useKs", new CheckBox("Kill Güvenliği -  Mnatığı"));

            //-------------------------------------------------------------------------------------------------------------------
            /*
            *      _____                       __  __                  
            *     |  __ \                     |  \/  |                 
            *     | |  | |_ __ __ ___      __ | \  / | ___ _ __  _   _ 
            *     | |  | | '__/ _` \ \ /\ / / | |\/| |/ _ \ '_ \| | | |
            *     | |__| | | | (_| |\ V  V /  | |  | |  __/ | | | |_| |
            *     |_____/|_|  \__,_| \_/\_/   |_|  |_|\___|_| |_|\__,_|
            *                                                          
            *                                                          
            */

            DrawMenu = Menu.AddSubMenu("Göster", "Draw");
            DrawMenu.AddGroupLabel("Gösterge Ayarları");
            DrawMenu.AddLabel("Büyü Menzili");
            DrawMenu.Add("drawQ", new CheckBox("Göster Q Menzili"));
            DrawMenu.Add("drawQextended", new CheckBox("Göster Uzun Q Menzili"));
            DrawMenu.Add("drawW", new CheckBox("Göster W Menzili"));
            DrawMenu.Add("drawE", new CheckBox("Göster E Menzili"));
            DrawMenu.Add("drawR", new CheckBox("Göster R Menzili"));

            //-------------------------------------------------------------------------------------------------------------------
            /*
            *      ______               _       
            *     |  ____|             | |      
            *     | |____   _____ _ __ | |_ ___ 
            *     |  __\ \ / / _ \ '_ \| __/ __|
            *     | |___\ V /  __/ | | | |_\__ \
            *     |______\_/ \___|_| |_|\__|___/
            *                                   
            *                                   
            */

            Game.OnUpdate += Events.OnUpdate;
            Gapcloser.OnGapcloser += Events.OnGapCloser;
            Obj_AI_Base.OnProcessSpellCast += Events.OnProcessSpellCast;
            Obj_AI_Base.OnSpellCast += Events.OnCastSpell;
            Drawing.OnDraw += Events.OnDraw;
            //Orbwalker.OnPostAttack += Events.OnAfterAttack;
        }
    }
}