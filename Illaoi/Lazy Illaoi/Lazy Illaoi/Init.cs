using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace Lazy_Illaoi
{
    internal class Init
    {
        public static Menu Menu, ComboMenu, HarassMenu, FarmMenu, MiscMenu, DrawMenu;

        public static void LoadMenu()
        {
            Bootstrap.Init(null);

            Menu = MainMenu.AddMenu("Lazy Illaoi", "lazy illaoi");
            Menu.AddGroupLabel("Lazy Illaoi");
            Menu.AddLabel("by DamnedNooB");
            Menu.AddLabel("Ceviri Tradana");
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
            ComboMenu.AddLabel("Q - DOKUNAÇ DARBESİ");
            ComboMenu.Add("useQ", new CheckBox("Q Mantıklı Kullan"));
            ComboMenu.Add("useEQ", new CheckBox("Mümkünse İlk E Kullan"));
            ComboMenu.AddSeparator();

            ComboMenu.AddLabel("W - SİLLE TOKAT");
            ComboMenu.Add("useW", new CheckBox("W Mantıklı Kullan"));
            ComboMenu.AddSeparator();

            ComboMenu.AddLabel("E - RUH SINAVI");
            ComboMenu.Add("useE", new CheckBox("E Mantıklı Kullan"));
            ComboMenu.AddSeparator();

            ComboMenu.AddLabel("R - İLAHİ DERS");
            ComboMenu.Add("useR", new CheckBox("R Mantıklı Kullan"));
            ComboMenu.Add("useR#", new Slider("Eğer Düşman Menzildeyse (veya 1v1 hayaletse)", 2, 0, 5));
            ComboMenu.AddSeparator();

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

            HarassMenu.AddLabel("Q - DOKUNAÇ DARBESİ");
            HarassMenu.Add("useQ", new CheckBox("Q mantıklı Kullan"));
            HarassMenu.Add("qMana", new Slider("En az mana: ", 20, 1));
            HarassMenu.AddSeparator();

            HarassMenu.AddLabel("W - SİLLE TOKAT");
            HarassMenu.Add("useW", new CheckBox("W mantıklı Kullan"));
            HarassMenu.Add("wMana", new Slider("En az mana: ", 20, 1));
            HarassMenu.AddSeparator();

            HarassMenu.AddLabel("E - RUH SINAVI");
            HarassMenu.Add("useE", new CheckBox("E mantıklı Kullan"));
            HarassMenu.Add("eMana", new Slider("En az mana: ", 20, 1));
            HarassMenu.AddSeparator();


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

            FarmMenu.AddLabel("Q - DOKUNAÇ DARBESİ");
            FarmMenu.Add("useQlane", new CheckBox("LaneTemizlerken Kullan"));
            FarmMenu.Add("qManaLane", new Slider("LaneTemizleme için en az mana: ", 20, 1));
            FarmMenu.Add("qMinionsLane", new Slider("LaneTemizleme için en az minyon: ", 3, 1, 6));
            FarmMenu.AddSeparator();

            FarmMenu.Add("useQjungle", new CheckBox("Orman Temizlemede Kullan"));
            FarmMenu.AddSeparator();

            FarmMenu.AddLabel("W - SİLLE TOKAT");
            FarmMenu.Add("useWlane", new CheckBox("LaneTemizlemede Kullan"));
            FarmMenu.Add("wManaLane", new Slider("LaneTemizleme için en az mana : ", 20, 1));
            FarmMenu.AddSeparator();

            FarmMenu.Add("useWjungle", new CheckBox("Orman Temizleme için Kullan"));
            FarmMenu.AddSeparator();

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
            MiscMenu.Add("gapcloserQ", new CheckBox("Use Q - DOKUNAÇ DARBESİ ni gapcloser için kullan"));
            MiscMenu.Add("gapcloserW", new CheckBox("Use W - SİLLE TOKAT 'ı gapcloser için kullan"));
            MiscMenu.AddSeparator();


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
            DrawMenu.AddLabel("Spell Ranges");
            DrawMenu.Add("drawQ", new CheckBox("Göster Q Menzili"));
            DrawMenu.Add("drawW", new CheckBox("Göster W Menzili"));
            DrawMenu.Add("drawE", new CheckBox("Göster E Menzili"));
            DrawMenu.Add("drawR", new CheckBox("Göster R Menzili"));
            DrawMenu.Add("drawT", new CheckBox("Göster Dokunaç Menzili"));

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
            Orbwalker.OnPostAttack += Events.OnPostAttack;
            //Orbwalker.OnPreAttack += Events.OnPreAttack;
            Obj_AI_Base.OnSpellCast += Events.OnSpellCast;
            Gapcloser.OnGapcloser += Events.OnGapCloser;
            GameObject.OnCreate += Events.OnCreateObj;
            GameObject.OnDelete += Events.OnDeleteObj;
            Drawing.OnDraw += Events.OnDraw;
        }
    }
}