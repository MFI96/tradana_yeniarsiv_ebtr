using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace NidaleeBuddyEvolution
{
    internal class NidaleeMenu
    {
        /// <summary>
        /// Stores Menus
        /// </summary>
        public static Menu DefaultMenu,
            ComboMenu,
            LastHitMenu,
            HarassMenu,
            LaneClearMenu,
            JungleClearMenu,
            KillStealMenu,
            JungleStealMenu,
            DrawingMenu,
            MiscMenu;

        /// <summary>
        /// Creates the Menu.
        /// </summary>
        public static void Create()
        {
            DefaultMenu = MainMenu.AddMenu("NidaleeBuddy", "NidaleeBuddy");
            DefaultMenu.AddGroupLabel("Bu addon yapımcısı Karmapandadır.");
            DefaultMenu.AddGroupLabel(
                "Any unauthorized redistribution without credits will result in severe consequences.");
            DefaultMenu.AddGroupLabel("Bu addonu kullandığın için teşekkürler");
            DefaultMenu.AddGroupLabel("Çevirmen TRAdana");

            #region Combo

            ComboMenu = DefaultMenu.AddSubMenu("Combo", "Combo");
            ComboMenu.AddGroupLabel("Combo Settings");
            ComboMenu.Add("useQH", new CheckBox("İnsan Formunda Q At"));
            ComboMenu.Add("useWH", new CheckBox("İnsan Formunda W at"));
            ComboMenu.Add("useQC", new CheckBox("Puma Modunda Q at"));
            ComboMenu.Add("useWC", new CheckBox("Puma Modunda W at"));
            ComboMenu.Add("useEC", new CheckBox("Puma Modunda E Kullan"));
            ComboMenu.Add("useR", new CheckBox("Kombo Sırasında R kullan"));
            ComboMenu.AddLabel("İsabet Oranı Ayarları - İnsan Formunda");
            ComboMenu.Add("predQH", new Slider("Q İsabet Oranı", 75));
            ComboMenu.Add("predWH", new Slider("W İsabet Oranı", 75));
            ComboMenu.AddLabel("İsabet Oranı Ayarları - Puma Formunda");
            ComboMenu.Add("predWC", new Slider("W İsabet Oranı", 75));
            ComboMenu.Add("predEC", new Slider("E İsabet Oranı", 75));

            #endregion

            #region Last Hit

            LastHitMenu = DefaultMenu.AddSubMenu("Last Hit", "Last Hit");
            LastHitMenu.AddGroupLabel("SonVuruş Ayarları");
            LastHitMenu.Add("useQC", new CheckBox("Puma Formunda öldürülemeyecek minyona Q"));
            LastHitMenu.Add("useEC", new CheckBox("Puma Formunda öldürülemeyecek minyona E", false));
            LastHitMenu.Add("useR", new CheckBox("Menzil Dışıysa R"));

            #endregion

            #region Harass

            HarassMenu = DefaultMenu.AddSubMenu("Harass", "Harass");
            HarassMenu.AddGroupLabel("Dürtme Ayarları");
            HarassMenu.Add("useQH", new CheckBox("İnsan Formunda Q"));
            HarassMenu.Add("useR", new CheckBox("Zorlamak için İnsan Formunda R"));
            HarassMenu.AddLabel("İsabet Oranı Ayarları - İnsan Formu");
            HarassMenu.Add("predQH", new Slider("Q İsabet Oranı", 75));

            #endregion

            #region Kill Steal

            KillStealMenu = DefaultMenu.AddSubMenu("Kill Steal", "Kill Steal");
            KillStealMenu.AddGroupLabel("Kill Çalma Ayarları");
            KillStealMenu.Add("useQH", new CheckBox("Kill Çalma Q"));
            KillStealMenu.Add("predQH", new Slider("Q İsabet Oranı", 75));
            KillStealMenu.Add("useIgnite", new CheckBox("Tutuştur Kullan", false));

            #endregion

            #region Lane Clear

            LaneClearMenu = DefaultMenu.AddSubMenu("Lane Clear", "Lane Clear");
            LaneClearMenu.AddGroupLabel("LaneTemizleme Ayarları");
            LaneClearMenu.Add("useQC", new CheckBox("Puma Formunda Q"));
            LaneClearMenu.Add("useWC", new CheckBox("Puma Formunda W"));
            LaneClearMenu.Add("useEC", new CheckBox("Puma Formunda E"));
            LaneClearMenu.Add("useR", new CheckBox("Lanetemizleme için R", false));
            LaneClearMenu.AddLabel("Farm Ayarları - Puma Formunda");
            LaneClearMenu.Add("predWC", new Slider("W için gereken minyon sayısı", 1, 1, 7));
            LaneClearMenu.Add("predEC", new Slider("E tutma oranı", 1, 1, 7));

            #endregion

            #region Jungle Clear

            JungleClearMenu = DefaultMenu.AddSubMenu("Jungle Clear", "Jungle Clear");
            JungleClearMenu.AddGroupLabel("OrmanTemizleme Ayarları");
            JungleClearMenu.Add("useQH", new CheckBox("Q İnsan Formunda"));
            JungleClearMenu.Add("useQC", new CheckBox("Puma Formunda Q"));
            JungleClearMenu.Add("useWC", new CheckBox("W Puma Formunda"));
            JungleClearMenu.Add("useEC", new CheckBox("Puma Formunda E"));
            JungleClearMenu.Add("useR", new CheckBox("OrmanTemizleme sırasında R"));
            JungleClearMenu.AddLabel("İsabet Oranı Ayarları");
            JungleClearMenu.Add("predQH", new Slider("Q İsabet Oranı İnsan Formunda", 75));
            JungleClearMenu.Add("predWC", new Slider("W İsabet Oranı Puma Formunda", 75));
            JungleClearMenu.Add("predEC", new Slider("E İsabet Oranı Puma Formunda", 1, 1, 3));

            #endregion

            #region Jungle Steal

            JungleStealMenu = DefaultMenu.AddSubMenu("Jungle Steal", "Jungle Steal");
            JungleStealMenu.AddGroupLabel("Orman Çal");
            JungleStealMenu.Add("useQH", new CheckBox("Ormanı çalmak için Q at"));
            JungleStealMenu.Add("predQH", new Slider("Q İsabet Oranı", 75));
            JungleStealMenu.Add("useSmite", new CheckBox("Orman çalmak için Çarp at"));
            JungleStealMenu.Add("toggleK", new KeyBind("Çarp Tuşu", true, KeyBind.BindTypes.PressToggle, 'M'));
            JungleStealMenu.AddGroupLabel("Orman Kampları");
            switch (Game.MapId)
            {
                case GameMapId.SummonersRift:
                    JungleStealMenu.AddLabel("Epics");
                    JungleStealMenu.Add("SRU_Baron", new CheckBox("Baron"));
                    JungleStealMenu.Add("SRU_Dragon", new CheckBox("Ejder"));
                    JungleStealMenu.AddLabel("Buffs");
                    JungleStealMenu.Add("SRU_Blue", new CheckBox("MAvi"));
                    JungleStealMenu.Add("SRU_Red", new CheckBox("Kırmızı"));
                    JungleStealMenu.AddLabel("Küçük Kamplar");
                    JungleStealMenu.Add("SRU_Gromp", new CheckBox("Kurbağa", false));
                    JungleStealMenu.Add("SRU_Murkwolf", new CheckBox("AlacaKurt", false));
                    JungleStealMenu.Add("SRU_Krug", new CheckBox("Golem", false));
                    JungleStealMenu.Add("SRU_Razorbeak", new CheckBox("SivriGagalar", false));
                    JungleStealMenu.Add("Sru_Crab", new CheckBox("Yampiri Yengeç", false));
                    break;
                case GameMapId.TwistedTreeline:
                    JungleStealMenu.AddLabel("Epics");
                    JungleStealMenu.Add("TT_Spiderboss8.1", new CheckBox("Örümcek"));
                    JungleStealMenu.AddLabel("Camps");
                    JungleStealMenu.Add("TT_NWraith1.1", new CheckBox("Hayalet"));
                    JungleStealMenu.Add("TT_NWraith4.1", new CheckBox("Hayalet"));
                    JungleStealMenu.Add("TT_NGolem2.1", new CheckBox("Golem"));
                    JungleStealMenu.Add("TT_NGolem5.1", new CheckBox("Golem"));
                    JungleStealMenu.Add("TT_NWolf3.1", new CheckBox("Kurt"));
                    JungleStealMenu.Add("TT_NWolf6.1", new CheckBox("Kurt"));
                    break;
            }

            #endregion

            #region Drawing

            DrawingMenu = DefaultMenu.AddSubMenu("Drawing", "Drawing");
            DrawingMenu.AddGroupLabel("Gösterge Ayarları");
            DrawingMenu.Add("drawQH", new CheckBox("Göster Javelin Menzili"));
            DrawingMenu.Add("drawPred", new CheckBox("Göster Javelin İsabet Oranı"));
            DrawingMenu.AddLabel("HasarTespitçisi");
            DrawingMenu.Add("draw.Damage", new CheckBox("Göster hasarı"));
            DrawingMenu.Add("draw.Q", new CheckBox("Q hasarı hesapla"));
            DrawingMenu.Add("draw.W", new CheckBox("W hasarı hesapla"));
            DrawingMenu.Add("draw.E", new CheckBox("E hasarı hesapla"));
            DrawingMenu.Add("draw.R", new CheckBox("R hasarı hesapla", false));
            DrawingMenu.AddLabel("Hasar tespiti renkleri");
            DrawingMenu.Add("draw_Alpha", new Slider("Alpha: ", 255, 0, 255));
            DrawingMenu.Add("draw_Red", new Slider("Kırmızı: ", 255, 0, 255));
            DrawingMenu.Add("draw_Green", new Slider("Yeşil: ", 0, 0, 255));
            DrawingMenu.Add("draw_Blue", new Slider("Mavi: ", 0, 0, 255));

            #endregion

            #region Misc

            MiscMenu = DefaultMenu.AddSubMenu("Misc Menu", "Misc Menu");
            MiscMenu.AddGroupLabel("Auto Heal Ayarları");
            MiscMenu.Add("autoHeal", new CheckBox("Otomatik can dostlara ve bana"));
            MiscMenu.Add("autoHealPercent", new Slider("Otomatik can yüzdesi", 50));

            foreach (var a in EntityManager.Heroes.Allies.OrderBy(a => a.BaseSkinName))
            {
                MiscMenu.Add("autoHeal_" + a.BaseSkinName, new CheckBox("Otomatik Can " + a.BaseSkinName));
            }

            MiscMenu.AddGroupLabel("Büyü Ayarları");
            MiscMenu.AddLabel("Only choose one of them below.");
            MiscMenu.Add("useQC_AfterAttack", new CheckBox("Saldırıdan sonra puma formunda Q at"));
            MiscMenu.Add("useQC_BeforeAttack", new CheckBox("Saldırıdan önce puma formunda Q at", false));
            MiscMenu.Add("useQC_OnUpdate", new CheckBox("Cast Q in Cougar Form on Update", false));
            MiscMenu.AddGroupLabel("ManaYardımcısı");
            MiscMenu.Add("manaQ", new Slider("Kullan Q İnsan Formunda en az mana >= x", 25));
            MiscMenu.Add("manaW", new Slider("Kullan W İnsan Formunda en az mana >= x", 25));
            MiscMenu.Add("manaE", new Slider("Kullan E İnsan Formunda en az mana >= x", 25));
            MiscMenu.Add("disableMM", new CheckBox("Kombo Modunda mana yardımcısı Devredışı"));

            #endregion
        }
    }
}