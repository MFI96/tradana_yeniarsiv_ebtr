using System;
using EloBuddy.SDK;
using EloBuddy;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto_Carry_Vayne.Manager
{
    class MenuManager
    {
        private static Menu VMenu,
            Hotkeymenu,
    ComboMenu,
    CondemnMenu,
    HarassMenu,
    LaneClearMenu,
    JungleClearMenu,
    FleeMenu,
    MiscMenu,
    ItemMenu,
    DrawingMenu;

        public static void Load()
        {
            Mainmenu();
            Hotkeys();
            PackageLoader();
            if (Variables.Combo == true) Combomenu();
            if (Variables.Condemn == true) Condemnmenu();
            if (Variables.Harass == true) Harassmenu();
            if (Variables.LC == true) LaneClearmenu();
            if (Variables.JC == true) JungleClearmenu();
            if (Variables.Flee == true) Fleemenu();
            if (Variables.Misc == true) Miscmenu();
            if (Variables.Activator == true) Activator();
            if (Variables.Draw == true) Drawingmenu();
        }

        private static void Mainmenu()
        {
            VMenu = MainMenu.AddMenu("Auto Carry Vayne", "akavayne");
            VMenu.AddGroupLabel("Made by Aka.");
            VMenu.AddSeparator();
        }

        private static void PackageLoader()
        {
            VMenu.AddGroupLabel("PackageLoader");
            VMenu.AddLabel("'Let me modify the Options myself plz'");
            VMenu.AddLabel("-Press F5 after ticking!-");
            VMenu.Add("Combo", new CheckBox("Daha fazla kombo ayarı", false));
            VMenu.Add("Condemn", new CheckBox("Daha fazla E(condemn) ayarı", false));
            VMenu.Add("Harass", new CheckBox("Daha fazla Dürtme Ayarı", false));
            VMenu.Add("Flee", new CheckBox("Daha fazla Kaçış ayarı", false));
            VMenu.Add("LC", new CheckBox("Daha fazla lanetemizleme ayarı", false));
            VMenu.Add("JC", new CheckBox("Daha fazla Ormantemizleme ayarı", false));
            VMenu.Add("Misc", new CheckBox("Daha fazla Ek Ayarı", false));
            VMenu.Add("Activator", new CheckBox("Daha fazla Aktivatör Ayarları", false));
            VMenu.Add("Drawing", new CheckBox("Daha fazla Gösterge Ayarı", false));
            #region CheckMenu

            //Combo
            if (VMenu["Combo"].Cast<CheckBox>().CurrentValue)
            {
                Variables.Combo = true;
            }
            //Condemn
            if (VMenu["Condemn"].Cast<CheckBox>().CurrentValue)
            {
                Variables.Condemn = true;
            }
            //Harass
            if (VMenu["Harass"].Cast<CheckBox>().CurrentValue)
            {
                Variables.Harass = true;
            }
            //Flee
            if (VMenu["Flee"].Cast<CheckBox>().CurrentValue)
            {
                Variables.Flee = true;
            }
            //LC
            if (VMenu["LC"].Cast<CheckBox>().CurrentValue)
            {
                Variables.LC = true;
            }
            //JC
            if (VMenu["JC"].Cast<CheckBox>().CurrentValue)
            {
                Variables.JC = true;
            }
            //Misc
            if (VMenu["Misc"].Cast<CheckBox>().CurrentValue)
            {
                Variables.Misc = true;
            }
            //Activator
            if (VMenu["Activator"].Cast<CheckBox>().CurrentValue)
            {
                Variables.Activator = true;
            }
            //Drawing
            if (VMenu["Drawing"].Cast<CheckBox>().CurrentValue)
            {
                Variables.Draw = true;
            }

            #endregion CheckMenu
        }

        private static void Hotkeys()
        {
            Hotkeymenu = VMenu.AddSubMenu("Hotkeys", "Hotkeys");
            Hotkeymenu.Add("flashe", new KeyBind("Flash Condemn!", false, KeyBind.BindTypes.HoldActive, 'Y'));
            Hotkeymenu.Add("insece", new KeyBind("Flash Insec!", false, KeyBind.BindTypes.HoldActive, 'Z'));
            Hotkeymenu.Add("rote", new KeyBind("Zz'Rot Condemn!", false, KeyBind.BindTypes.HoldActive, 'N'));
            Hotkeymenu.Add("insecmodes", new ComboBox("Insec Mode", 0, "To Allys", "To Tower", "To Mouse"));
        }

        private static void Combomenu()
        {
            ComboMenu = VMenu.AddSubMenu("Combo", "Combo");
            ComboMenu.Add("UseQ", new CheckBox("Kullan Q"));
            ComboMenu.Add("UseQE", new CheckBox("QE dene"));
            ComboMenu.Add("UseQWall", new CheckBox("Duvara Q Kullan"));
            ComboMenu.Add("UseQEnemies", new Slider("Duvara Q için Düşman sayısı", 3, 5, 0));
            ComboMenu.Add("UseQStacks", new CheckBox("Q için 2W yükü", false));
            ComboMenu.Add("UseQMode", new ComboBox("Q Mode", 1, "Side", "Safe Position"));
            ComboMenu.Add("UseW", new CheckBox("W odaklan", false));
            ComboMenu.Add("UseE", new CheckBox("Kullan E"));
            ComboMenu.Add("UseEKill", new CheckBox("Ölecekse hedefe E?"));
            ComboMenu.Add("UseR", new CheckBox("Kullan R", false));
            ComboMenu.Add("UseRif", new Slider("R için gereken Düşman sayısı", 2, 1, 5));
            ComboMenu.Add("RnoAA", new CheckBox("Gizliyken AA yapma", false));
            ComboMenu.Add("RnoAAif", new Slider("Şu kadar düşman menzildeyse gizliyken AA yapma", 2, 0, 5));

        }

        private static void Condemnmenu()
        {
            CondemnMenu = VMenu.AddSubMenu("Condemn", "Condemn");
            CondemnMenu.Add("UseEAuto", new CheckBox("Otomatik E"));
            CondemnMenu.Add("UseETarget", new CheckBox("Sadece sabitleyecekse?", false));
            CondemnMenu.Add("UseEHitchance", new Slider("Condemn(E) İsabet oranı", 2, 1, 4));
            CondemnMenu.Add("UseEPush", new Slider("Condemn İtme mesafesi(atma)", 420, 350, 470));
            CondemnMenu.Add("UseEAA", new Slider("Eğer hedef AA ile ölecekse E Kullanma", 0, 0, 4));
            CondemnMenu.Add("AutoTrinket", new CheckBox("Çime totem ?"));
            CondemnMenu.Add("J4Flag", new CheckBox("Jarvanı it(ulti atarken veya ultisi varsa)?"));
        }

        private static void Harassmenu()
        {
            HarassMenu = VMenu.AddSubMenu("Harass", "Harass");
            HarassMenu.Add("HarassCombo", new CheckBox("Dürtme Kombosu"));
            HarassMenu.Add("HarassMana", new Slider("Dürtme Kombosu en az mana", 40));
        }

        private static void LaneClearmenu()
        {
            LaneClearMenu = VMenu.AddSubMenu("LaneClear", "LaneClear");
            LaneClearMenu.Add("UseQ", new CheckBox("Kullan Q"));
            LaneClearMenu.Add("UseQMana", new Slider("En az mana ({0}%)", 40));
        }

        private static void JungleClearmenu()
        {
            JungleClearMenu = VMenu.AddSubMenu("JungleClear", "JungleClear");
            JungleClearMenu.Add("UseQ", new CheckBox("Kullan Q"));
            JungleClearMenu.Add("UseE", new CheckBox("Kullan E"));
        }

        private static void Fleemenu()
        {
            FleeMenu = VMenu.AddSubMenu("Flee", "Flee");
            FleeMenu.Add("UseQ", new CheckBox("Kullan Q"));
            FleeMenu.Add("UseE", new CheckBox("Kullan E"));
        }

        private static void Miscmenu()
        {
            MiscMenu = VMenu.AddSubMenu("Misc", "Misc");
            MiscMenu.AddGroupLabel("Ek ");
            MiscMenu.Add("GapcloseQ", new CheckBox("Gapclose Q"));
            MiscMenu.Add("GapcloseE", new CheckBox("Gapclose E"));
            MiscMenu.Add("InterruptE", new CheckBox("Interrupt E"));
            MiscMenu.Add("LowLifeE", new CheckBox("canım düşükse rakibe E", false));
            MiscMenu.Add("LowLifeES", new Slider("Canım şundan düşükse % =>", 20));
            MiscMenu.AddGroupLabel("Ek Özellikler");
            MiscMenu.Add("Skinhack", new CheckBox("Aktif skin hilesi", false));
            MiscMenu.Add("SkinId", new ComboBox("Skin Hack", 0, "Default", "Vindicator", "Aristocrat", "Dragonslayer", "Heartseeker", "SKT T1", "Arclight", "Vayne Chroma Green", "Vayne Chroma Red", "Vayne Chroma Grey"));
            MiscMenu.Add("Autolvl", new CheckBox("Aktif Otomatik Level"));
            MiscMenu.Add("AutolvlS", new ComboBox("Level Mode", 0, "Max W", "Max Q(my style)"));
            MiscMenu.Add("Autobuy", new CheckBox("Başlangıçta otomatik satın al"));
            MiscMenu.Add("Autobuyt", new CheckBox("Totemi otomatik al(9.lvlde değiştirme)", false));
        }

        private static void Activator()
        {
            ItemMenu = VMenu.AddSubMenu("Activator", "Activator");
            ItemMenu.AddGroupLabel("İtemler");
            ItemMenu.AddLabel("Eğer daha fazlasına ihtiyaç olursa bana söyleyin.");
            ItemMenu.Add("Botrk", new CheckBox("Mahvolmuş ve Bilgewater Kullan"));
            ItemMenu.Add("AutoPotion", new CheckBox("Otomatik Can Potu"));
            ItemMenu.Add("AutoPotionHp", new Slider("Can botu için canım şundan az <=", 60));
            ItemMenu.Add("AutoBiscuit", new CheckBox("Otomatik bisküvi"));
            ItemMenu.Add("AutoBiscuitHp", new Slider("Biskivü için can <=", 60));
            ItemMenu.AddGroupLabel("Sihirdar büyüleri");
            ItemMenu.AddLabel("Daha fazlasına ihtiyacın olursa bana söyleyebilirsin.");
            ItemMenu.Add("Heal", new CheckBox("İyileştirme"));
            ItemMenu.Add("HealHp", new Slider("İyileştirme için benim canım şundan az <=", 20, 0, 100));
            ItemMenu.Add("HealAlly", new CheckBox("Dostlara İyileştirme yap"));
            ItemMenu.Add("HealAllyHp", new Slider("Dostların canı <=", 20, 0, 100));
            ItemMenu.AddGroupLabel("Qss");
            ItemMenu.Add("Qss", new CheckBox("Kullan Qss"));
            ItemMenu.Add("QssDelay", new Slider("Gecikme", 100, 0, 2000));
            ItemMenu.Add("Blind",
                new CheckBox("Kör", false));
            ItemMenu.Add("Charm",
                new CheckBox("Çekicilik(ahri)"));
            ItemMenu.Add("Fear",
                new CheckBox("Korkmuş"));
            ItemMenu.Add("Polymorph",
                new CheckBox("Polymorph"));
            ItemMenu.Add("Stun",
                new CheckBox("Sabitlenmiş"));
            ItemMenu.Add("Snare",
                new CheckBox("Yavaşlamış"));
            ItemMenu.Add("Silence",
                new CheckBox("Sessiz", false));
            ItemMenu.Add("Taunt",
                new CheckBox("Aley edilen"));
            ItemMenu.Add("Suppression",
                new CheckBox("Önlenen"));

        }

        private static void Drawingmenu()
        {
            DrawingMenu = VMenu.AddSubMenu("Drawings", "Drawings");
            DrawingMenu.Add("DrawQ", new CheckBox("Göster Q", false));
            DrawingMenu.Add("DrawE", new CheckBox("Göster E", false));
            DrawingMenu.Add("DrawCondemn", new CheckBox("Göster Condemn"));
            DrawingMenu.Add("DrawOnlyReady", new CheckBox("Göster Sadece büyüler hazırsa"));
        }

        #region checkvalues
        #region checkvalues:hotkeys
        public static bool FlashE
        {
            get { return (Hotkeymenu["flashe"].Cast<KeyBind>().CurrentValue); }
        }
        public static bool InsecE
        {
            get { return (Hotkeymenu["insece"].Cast<KeyBind>().CurrentValue); }
        }
        public static bool RotE
        {
            get { return (Hotkeymenu["rote"].Cast<KeyBind>().CurrentValue); }
        }
        public static int InsecPositions
        {
            get { return (Hotkeymenu["insecmodes"].Cast<ComboBox>().CurrentValue); }
        }
        #endregion checkvalues:hotkeys
        #region checkvalues:Combo
        public static bool UseQ
        {
            get { return (VMenu["Combo"].Cast<CheckBox>().CurrentValue ? ComboMenu["UseQ"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static int UseQEnemies
        {
            get { return (VMenu["Combo"].Cast<CheckBox>().CurrentValue ? ComboMenu["UseQEnemies"].Cast<Slider>().CurrentValue : 3); }
        }

        public static bool UseQWall
        {
            get { return (VMenu["Combo"].Cast<CheckBox>().CurrentValue ? ComboMenu["UseQWall"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static int UseQMode
        {
            get { return (VMenu["Combo"].Cast<CheckBox>().CurrentValue ? ComboMenu["UseQMode"].Cast<ComboBox>().CurrentValue : 1); }
        }

        public static bool UseQE
        {
            get { return (VMenu["Combo"].Cast<CheckBox>().CurrentValue ? ComboMenu["UseQE"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static bool UseQStacks
        {
            get { return (VMenu["Combo"].Cast<CheckBox>().CurrentValue ? ComboMenu["UseQStacks"].Cast<CheckBox>().CurrentValue : false); }
        }

        public static bool FocusW
        {
            get { return (VMenu["Combo"].Cast<CheckBox>().CurrentValue ? ComboMenu["UseW"].Cast<CheckBox>().CurrentValue : false); }
        }

        public static bool UseE
        {
            get { return (VMenu["Combo"].Cast<CheckBox>().CurrentValue ? ComboMenu["UseE"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static bool UseEKill
        {
            get { return (VMenu["Combo"].Cast<CheckBox>().CurrentValue ? ComboMenu["UseEKill"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static bool UseR
        {
            get { return (VMenu["Combo"].Cast<CheckBox>().CurrentValue ? ComboMenu["UseR"].Cast<CheckBox>().CurrentValue : false); }
        }

        public static int UseRSlider
        {
            get { return (VMenu["Combo"].Cast<CheckBox>().CurrentValue ? ComboMenu["UseRif"].Cast<Slider>().CurrentValue : 2); }
        }

        public static bool RNoAA
        {
            get { return (VMenu["Combo"].Cast<CheckBox>().CurrentValue ? ComboMenu["RnoAA"].Cast<CheckBox>().CurrentValue : false); }
        }

        public static int RNoAASlider
        {
            get { return (VMenu["Combo"].Cast<CheckBox>().CurrentValue ? ComboMenu["RnoAAif"].Cast<Slider>().CurrentValue : 3); }
        }

        //Condemn
        #endregion checkvalues:Combo
        #region checkvalues:Condemn

        public static bool AutoE
        {
            get { return (VMenu["Condemn"].Cast<CheckBox>().CurrentValue ? CondemnMenu["UseEAuto"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static bool OnlyStunCurrentTarget
        {
            get { return (VMenu["Condemn"].Cast<CheckBox>().CurrentValue ? CondemnMenu["UseETarget"].Cast<CheckBox>().CurrentValue : false); }
        }

        public static int CondemnHitchance
        {
            get { return (VMenu["Condemn"].Cast<CheckBox>().CurrentValue ? CondemnMenu["UseEHitchance"].Cast<Slider>().CurrentValue : 2); }
        }

        public static int CondemnPushDistance
        {
            get { return (VMenu["Condemn"].Cast<CheckBox>().CurrentValue ? CondemnMenu["UseEPush"].Cast<Slider>().CurrentValue : 420); }
        }

        public static int CondemnBlock
        {
            get { return (VMenu["Condemn"].Cast<CheckBox>().CurrentValue ? CondemnMenu["UseEAA"].Cast<Slider>().CurrentValue : 1); }
        }

        public static bool AutoTrinket
        {
            get { return (VMenu["Condemn"].Cast<CheckBox>().CurrentValue ? CondemnMenu["AutoTrinket"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static bool J4Flag
        {
            get { return (VMenu["Condemn"].Cast<CheckBox>().CurrentValue ? CondemnMenu["J4Flag"].Cast<CheckBox>().CurrentValue : true); }
        }

        #endregion checkvalues:Condemn
        #region checkvalues:Harass

        public static bool HarassCombo
        {
            get { return (VMenu["Harass"].Cast<CheckBox>().CurrentValue ? HarassMenu["HarassCombo"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static int HarassMana
        {
            get { return (VMenu["Harass"].Cast<CheckBox>().CurrentValue ? HarassMenu["HarassMana"].Cast<Slider>().CurrentValue : 0); }
        }


        #endregion checkvalues:Harass
        #region checkvalues:LC

        public static bool UseQLC
        {
            get { return (VMenu["LC"].Cast<CheckBox>().CurrentValue ? LaneClearMenu["UseQ"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static int UseQLCMana
        {
            get { return (VMenu["LC"].Cast<CheckBox>().CurrentValue ? LaneClearMenu["UseQMana"].Cast<Slider>().CurrentValue : 40); }
        }


        #endregion checkvalues:LC
        #region checkvalues:JC

        public static bool UseQJC
        {
            get { return (VMenu["JC"].Cast<CheckBox>().CurrentValue ? JungleClearMenu["UseQ"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static bool UseEJC
        {
            get { return (VMenu["JC"].Cast<CheckBox>().CurrentValue ? JungleClearMenu["UseE"].Cast<CheckBox>().CurrentValue : true); }
        }

        #endregion checkvalues:JC
        #region checkvalues:Flee
        public static bool UseQFlee
        {
            get { return (VMenu["Flee"].Cast<CheckBox>().CurrentValue ? FleeMenu["UseQ"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static bool UseEFlee
        {
            get { return (VMenu["Flee"].Cast<CheckBox>().CurrentValue ? FleeMenu["UseE"].Cast<CheckBox>().CurrentValue : true); }
        }

        #endregion checkvalues:Flee
        #region checkvalues:Misc

        public static bool GapcloseQ
        {
            get { return (VMenu["Misc"].Cast<CheckBox>().CurrentValue ? MiscMenu["GapcloseQ"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static bool GapcloseE
        {
            get { return (VMenu["Misc"].Cast<CheckBox>().CurrentValue ? MiscMenu["GapcloseE"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static bool InterruptE
        {
            get { return (VMenu["Misc"].Cast<CheckBox>().CurrentValue ? MiscMenu["InterruptE"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static bool LowLifeE
        {
            get { return (VMenu["Misc"].Cast<CheckBox>().CurrentValue ? MiscMenu["LowLifeE"].Cast<CheckBox>().CurrentValue : false); }
        }

        public static int LowLifeESlider
        {
            get { return (VMenu["Misc"].Cast<CheckBox>().CurrentValue ? MiscMenu["LowLifeES"].Cast<Slider>().CurrentValue : 20); }
        }

        public static bool Skinhack
        {
            get { return (VMenu["Misc"].Cast<CheckBox>().CurrentValue ? MiscMenu["Skinhack"].Cast<CheckBox>().CurrentValue : false); }
        }

        public static int SkinId
        {
            get { return (VMenu["Misc"].Cast<CheckBox>().CurrentValue ? MiscMenu["SkinId"].Cast<ComboBox>().CurrentValue : 0); }
        }

        public static bool Autolvl
        {
            get { return (VMenu["Misc"].Cast<CheckBox>().CurrentValue ? MiscMenu["Autolvl"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static int AutolvlSlider
        {
            get { return (VMenu["Misc"].Cast<CheckBox>().CurrentValue ? MiscMenu["AutolvlS"].Cast<ComboBox>().CurrentValue : 0); }
        }

        public static bool AutobuyStarters
        {
            get { return (VMenu["Misc"].Cast<CheckBox>().CurrentValue ? MiscMenu["Autobuy"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static bool AutobuyTrinkets
        {
            get { return (VMenu["Misc"].Cast<CheckBox>().CurrentValue ? MiscMenu["Autobuyt"].Cast<CheckBox>().CurrentValue : false); }
        }


        #endregion checkvalues:Misc
        #region checkvalues:Activator

        public static bool Botrk
        {
            get { return (VMenu["Activator"].Cast<CheckBox>().CurrentValue ? ItemMenu["Botrk"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static bool AutoPotion
        {
            get { return (VMenu["Activator"].Cast<CheckBox>().CurrentValue ? ItemMenu["AutoPotion"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static bool AutoBiscuit
        {
            get { return (VMenu["Activator"].Cast<CheckBox>().CurrentValue ? ItemMenu["AutoBiscuit"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static int AutoBiscuitHp
        {
            get { return (VMenu["Activator"].Cast<CheckBox>().CurrentValue ? ItemMenu["AutoBiscuitHp"].Cast<Slider>().CurrentValue : 60); }
        }

        public static int AutoPotionHp
        {
            get { return (VMenu["Activator"].Cast<CheckBox>().CurrentValue ? ItemMenu["AutoPotionHp"].Cast<Slider>().CurrentValue : 60); }
        }

        public static bool Heal
        {
            get { return (VMenu["Activator"].Cast<CheckBox>().CurrentValue ? ItemMenu["Heal"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static int HealHp
        {
            get { return (VMenu["Activator"].Cast<CheckBox>().CurrentValue ? ItemMenu["HealHp"].Cast<Slider>().CurrentValue : 20); }
        }

        public static bool HealAlly
        {
            get { return (VMenu["Activator"].Cast<CheckBox>().CurrentValue ? ItemMenu["HealAlly"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static int HealAllyHp
        {
            get { return (VMenu["Activator"].Cast<CheckBox>().CurrentValue ? ItemMenu["HealAllyHp"].Cast<Slider>().CurrentValue : 20); }
        }

        public static bool Qss
        {
            get { return (VMenu["Activator"].Cast<CheckBox>().CurrentValue ? ItemMenu["Qss"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static int QssDelay
        {
            get { return (VMenu["Activator"].Cast<CheckBox>().CurrentValue ? ItemMenu["QssDelay"].Cast<Slider>().CurrentValue : 0); }
        }

        public static bool QssBlind
        {
            get { return (VMenu["Activator"].Cast<CheckBox>().CurrentValue ? ItemMenu["Blind"].Cast<CheckBox>().CurrentValue : false); }
        }

        public static bool QssCharm
        {
            get { return (VMenu["Activator"].Cast<CheckBox>().CurrentValue ? ItemMenu["Charm"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static bool QssFear
        {
            get { return (VMenu["Activator"].Cast<CheckBox>().CurrentValue ? ItemMenu["Fear"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static bool QssPolymorph
        {
            get { return (VMenu["Activator"].Cast<CheckBox>().CurrentValue ? ItemMenu["Polymorph"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static bool QssStun
        {
            get { return (VMenu["Activator"].Cast<CheckBox>().CurrentValue ? ItemMenu["Stun"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static bool QssSnare
        {
            get { return (VMenu["Activator"].Cast<CheckBox>().CurrentValue ? ItemMenu["Snare"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static bool QssSilence
        {
            get { return (VMenu["Activator"].Cast<CheckBox>().CurrentValue ? ItemMenu["Silence"].Cast<CheckBox>().CurrentValue : false); }
        }

        public static bool QssTaunt
        {
            get { return (VMenu["Activator"].Cast<CheckBox>().CurrentValue ? ItemMenu["Taunt"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static bool QssSupression
        {
            get { return (VMenu["Activator"].Cast<CheckBox>().CurrentValue ? ItemMenu["Suppression"].Cast<CheckBox>().CurrentValue : true); }
        }

        #endregion checkvalues:Activator
        #region checkvalues:Drawing
        public static bool DrawQ
        {
            get { return (VMenu["Drawing"].Cast<CheckBox>().CurrentValue ? DrawingMenu["DrawQ"].Cast<CheckBox>().CurrentValue : false); }
        }

        public static bool DrawE
        {
            get { return (VMenu["Drawing"].Cast<CheckBox>().CurrentValue ? DrawingMenu["DrawE"].Cast<CheckBox>().CurrentValue : false); }
        }

        public static bool DrawCondemn
        {
            get { return (VMenu["Drawing"].Cast<CheckBox>().CurrentValue ? DrawingMenu["DrawCondemn"].Cast<CheckBox>().CurrentValue : true); }
        }

        public static bool DrawOnlyRdy
        {
            get { return (VMenu["Drawing"].Cast<CheckBox>().CurrentValue ? DrawingMenu["DrawOnlyReady"].Cast<CheckBox>().CurrentValue : false); }
        }
        #endregion checkvalues:Drawing
        #endregion checkvalues
    }
}
