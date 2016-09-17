using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using Mario_s_Lib;
using Mario_s_Lib.DataBases;
using static Mario_s_Activator.SummonerSpells;

namespace Mario_s_Activator
{
    internal class MyMenu
    {
        public static Menu FirstMenu = MainMenu.AddMenu("Mario`s Activator", "marioactivatorr");
        public static Menu OffensiveMenu = FirstMenu.AddSubMenu("• Offensive Items", "activatorOffensive");
        public static Menu DefensiveMenu = FirstMenu.AddSubMenu("• Defensive Items", "activatordefensive");
        public static Menu CleansersMenu = FirstMenu.AddSubMenu("• Cleansers", "activatorcleansers");
        public static Menu ConsumablesMenu = FirstMenu.AddSubMenu("• Consumables", "activatorconsumables");
        public static Menu ProtectMenu = FirstMenu.AddSubMenu("• Protector", "activatorprotector");
        public static Menu SummonerMenu = FirstMenu.AddSubMenu("• Summoner Spells", "activatorSummonerspells");
        public static Menu MiscMenu = FirstMenu.AddSubMenu("• Misc", "activatormisc");
        public static Menu DrawingMenu = FirstMenu.AddSubMenu("• Drawing", "activatordrawing");
        public static Menu SettingsMenu = FirstMenu.AddSubMenu("• Settings", "activatorsettings");

        public static void InitializeMenu()
        {
            FirstMenu.AddGroupLabel("Bu addon MarioGK tarafından kodlanmıştır.");
            FirstMenu.AddGroupLabel("Hiçbir yetkisiz işlem yoktur.");
            FirstMenu.AddGroupLabel("Eğleneceğinizi umuyorum");
            FirstMenu.AddGroupLabel("Çeviri-TRAdana");

            #region OffensiveMenu

            OffensiveMenu.AddGroupLabel("Bilgewater Cutlass");
            OffensiveMenu.CreateCheckBox("Bilgewater palası kullan.", "check3144");
            OffensiveMenu.CreateSlider("Düşmanın canı şunun altındaysa ({0}%).", "slider3144", 90);
            OffensiveMenu.AddGroupLabel("Mahvolmuş Kılıç");
            OffensiveMenu.CreateCheckBox("Mahvolmuş Kılıç Kullan.", "check3153");
            OffensiveMenu.CreateSlider("Düşmanın canı şundan azsa ({0}%).", "slider3153", 60);
            OffensiveMenu.AddGroupLabel("Tiamat");
            OffensiveMenu.CreateCheckBox("Kullan tiamat.", "check3077");
            OffensiveMenu.CreateSlider("Düşmanın canı şundan azsa ({0}%).", "slider3077", 90);
            OffensiveMenu.AddGroupLabel("VahşiHydra");
            OffensiveMenu.CreateCheckBox("Kullan hydra.", "check3074");
            OffensiveMenu.CreateSlider("Düşmanın canı şundan azsa ({0}%).", "slider3074", 90);
            OffensiveMenu.AddGroupLabel("Haşmetli Hydra");
            OffensiveMenu.CreateCheckBox("Haşmetli Hydra Kullan.", "check"+ (int)ItemId.Titanic_Hydra);
            OffensiveMenu.CreateSlider("Düşmanın canı şundan azsa ({0}%).", "slider" + (int)ItemId.Titanic_Hydra, 90);
            OffensiveMenu.AddGroupLabel("Youmuus");
            OffensiveMenu.CreateCheckBox("Kullan youmuus.", "check3142");
            OffensiveMenu.CreateSlider("Düşmanın canı şundan azsa ({0}%).", "slider3142", 70);
            OffensiveMenu.AddGroupLabel("Hextech Kılıcı");
            OffensiveMenu.CreateCheckBox("Kullan Hextech Kılıcı.", "check3146");
            OffensiveMenu.CreateSlider("Düşmanın canı şundan azsa ({0}%).", "slider3146", 40);
            OffensiveMenu.AddGroupLabel("Buz Kraliçesinin Hakkı");
            OffensiveMenu.CreateCheckBox("Kullan buz kraliçesi.", "check3092");
            OffensiveMenu.CreateSlider("Düşmanın canı şundan azsa ({0}%).", "slider3092", 40);

            #endregion OffensiveMenu

            #region DefensiveMenu

            DefensiveMenu.AddGroupLabel("Zhonyas");
            DefensiveMenu.CreateCheckBox("Kullan Zhonyas.", "check" + (int)ItemId.Zhonyas_Hourglass);
            DefensiveMenu.CreateSlider("Cnaım şundan azsa ({0}%).", "slider" + (int)ItemId.Zhonyas_Hourglass, 25);
            DefensiveMenu.AddGroupLabel("Seraph");
            DefensiveMenu.CreateCheckBox("Seraphin Şefkati Kullan.", "check" + (int)ItemId.Seraphs_Embrace);
            DefensiveMenu.CreateSlider("Canım şundan azsa ({0}%).", "slider" + (int)ItemId.Seraphs_Embrace, 25);
            DefensiveMenu.AddGroupLabel("Dağın Sureti");
            DefensiveMenu.CreateCheckBox("Kullan Dağın Sureti.", "check" + "3401");
            DefensiveMenu.CreateSlider("Canım şundan azsa ({0}%).", "slider" + "3401", 10);
            DefensiveMenu.CreateSlider("Dostların Canı şundan azsa ({0}%).", "slider" + "3401" + "ally", 20);
            DefensiveMenu.AddGroupLabel("Uluların Tılsımı");
            DefensiveMenu.CreateCheckBox("Kullan Uluların Tılsımı.", "check" + "3069");
            DefensiveMenu.CreateSlider("Canım şundan azsa ({0}%).", "slider" + "3069", 20);
            DefensiveMenu.AddGroupLabel("Demir Solari'nin Broşu");
            DefensiveMenu.CreateCheckBox("Kullan Demir Solari'nin Broşu.", "check" + "3190");
            DefensiveMenu.CreateSlider("Canım şundan azsa ({0}%).", "slider" + "3190", 20);
            DefensiveMenu.AddGroupLabel("Randuins");
            DefensiveMenu.CreateCheckBox("Kullan Randuins.", "check" + "3143");
            DefensiveMenu.CreateSlider("Use it if there are ({0}) in range.", "slider" + "3143", 2, 0, 5);
            DefensiveMenu.AddGroupLabel("OHMYIKICI");
            DefensiveMenu.CreateCheckBox("Kullan OHMYIKICI.", "check" + "3056");
            DefensiveMenu.CreateSlider("Canım şundan azsa ({0}%).", "slider" + "3056", 60);

            #endregion DefensiveMenu

            #region CleansersMenu
            CleansersMenu.AddGroupLabel("Temizleme Ayarları(QSS)");
            CleansersMenu.CreateSlider("Temizleme için Gecikme", "delayCleanse", 50, 0, 500);
            CleansersMenu.AddGroupLabel("Hangi Durumlarda Kullansın ?");
            CleansersMenu.CreateCheckBox("Sabitlenme", "ccStun");
            CleansersMenu.CreateCheckBox("Kör Olma", "ccBlind", false);
            CleansersMenu.CreateCheckBox("Yavaşlama", "ccSlow", false);
            CleansersMenu.CreateCheckBox("Tuzakta", "ccSnare");
            CleansersMenu.CreateCheckBox("Kaçma", "ccFlee");
            CleansersMenu.CreateCheckBox("Önlemede", "ccSupression");
            CleansersMenu.CreateCheckBox("Alay etme", "ccTaunt");
            CleansersMenu.CreateCheckBox("Çekicilik(ahri)", "ccCharm");
            CleansersMenu.CreateCheckBox("Polymorph", "ccPolymorph");
            CleansersMenu.CreateCheckBox("Duskblade of Draktharr", "ccDusk");
            CleansersMenu.AddLabel("Özel büyüler (ultiler)");
            CleansersMenu.CreateCheckBox("Zed R", "ccZedR");
            CleansersMenu.CreateCheckBox("Vladmir R", "ccVladmirR");
            CleansersMenu.CreateCheckBox("Mordekaiser R", "ccMordekaiserR");
            CleansersMenu.CreateCheckBox("Trundle R", "ccTrundleR");
            CleansersMenu.CreateCheckBox("Fiora R", "ccFioraR");
            CleansersMenu.CreateCheckBox("Kalista E", "ccKalistaE");
            CleansersMenu.AddGroupLabel("İtemler");
            CleansersMenu.CreateCheckBox("Deviş Kılıcı Kullan", "check" + "3137");
            CleansersMenu.CreateCheckBox("Akıncı Palası Kullan.", "check" + "3139");
            CleansersMenu.CreateCheckBox("Civalı Kuşak Kullan.", "check" + "3140");
            if (PlayerHasCleanse)
            {
                CleansersMenu.AddLabel("Sihirdar Büyüleri");
                CleansersMenu.CreateCheckBox("Arındırma Kullan", "check" + "cleanse");
            }
            CleansersMenu.AddGroupLabel("Mikail Ayarları");
            CleansersMenu.CreateCheckBox("Mikail Kullan.", "check" + "3222");
            CleansersMenu.AddSeparator();
            foreach (var ally in EntityManager.Heroes.Allies.Where(a => !a.IsMe))
            {
                CleansersMenu.CreateCheckBox("Şunda Kullan "+ ally.ChampionName + "("+ally.Name+")", "check" + ally.ChampionName);
            }

            #endregion CleansersMenu

            #region ConsumableMenu
            ConsumablesMenu.AddGroupLabel("Can İksiri");
            ConsumablesMenu.CreateCheckBox("Kullan Can İksiri.", "check" + "2003");
            ConsumablesMenu.CreateSlider("Benim canım şundan azsa ({0}%).", "slider" + "2003" + "health", 45);
            ConsumablesMenu.AddGroupLabel("Biskivü");
            ConsumablesMenu.CreateCheckBox("Kullan Biskivü.", "check" + (int)ItemId.Total_Biscuit_of_Rejuvenation);
            ConsumablesMenu.CreateSlider("Benim canım şundan azsa ({0}%).", "slider" + (int)ItemId.Total_Biscuit_of_Rejuvenation + "health", 45);
            ConsumablesMenu.CreateSlider("Manam şundan azsa ({0}%).", "slider" + (int)ItemId.Total_Biscuit_of_Rejuvenation + "mana", 30);
            ConsumablesMenu.AddGroupLabel("Avcının İksiri");
            ConsumablesMenu.CreateCheckBox("Kullan Avcının İksiri", "check" + "2032");
            ConsumablesMenu.CreateSlider("Benim canım şundan azsa ({0}%).", "slider" + "2032" + "health", 30);
            ConsumablesMenu.CreateSlider("Manam şundan azsa ({0}%).", "slider" + "2032" + "mana", 30);
            ConsumablesMenu.CreateCheckBox("Kullan Musibet İksiri.", "check" + (int)ItemId.Corrupting_Potion);
            ConsumablesMenu.CreateSlider("Benim canım şundan azsa ({0}%).", "slider" + (int)ItemId.Corrupting_Potion + "health", 30);
            ConsumablesMenu.CreateSlider("Manam şundan azsa ({0}%).", "slider" + (int)ItemId.Corrupting_Potion + "mana", 30);
            ConsumablesMenu.CreateCheckBox("Kullan Doldurulabilir İksir.", "check" + (int)ItemId.Refillable_Potion);
            ConsumablesMenu.CreateSlider("Benim canım şundan azsa ({0}%).", "slider" + (int)ItemId.Refillable_Potion + "health", 30);
            ConsumablesMenu.AddGroupLabel("Elixirs");
            ConsumablesMenu.CreateCheckBox("Büyücülük Karışımı", "check" + (int) ItemId.Elixir_of_Sorcery);
            ConsumablesMenu.CreateCheckBox("Hiddet Karışımı", "check" + (int) ItemId.Elixir_of_Wrath);
            ConsumablesMenu.CreateCheckBox("Sağlamlık Karışımı", "check" + (int) ItemId.Elixir_of_Iron);
            #endregion ConsumablesMenu

            #region ProtectMenu
            var champS = ProtectSpells.Spells.FirstOrDefault(s => s.Champ == Player.Instance.Hero);
            if (champS != null)
            {
                ProtectMenu.AddGroupLabel("Ayarlar: ");
                ProtectMenu.CreateCheckBox("Koruyucu Aktif", "checkProtector");
                ProtectMenu.CreateSlider("Dostların canı şundan az {0}", "protectallyhealth", 20);
                ProtectMenu.AddGroupLabel("Büyüler: ");

                var spell = Player.GetSpell(champS.Slot);
                
                if (spell != null)
                {
                    var slot = spell.Slot.ToString()[spell.Slot.ToString().Length - 1];
                    ProtectMenu.CreateCheckBox("- Büyüleri Kullan " + slot, "canUseSpell" + spell.Slot);
                }

                ProtectMenu.AddGroupLabel("Beyazliste: ");
                foreach (var a in EntityManager.Heroes.Allies)
                {
                    ProtectMenu.CreateCheckBox("- Şu durumda kullan " + a.ChampionName + " (" + a.Name + ") ", "canUseSpellOn" + a.ChampionName);
                }
            }
            else
            {
                ProtectMenu.AddGroupLabel("Arkadaşlarını korumak için burayı kullanabilirsin");
            }

            #endregion ProtectMenu

            #region SummonerSpells

            if (PlayerHasSmite)
            {
                SummonerMenu.AddGroupLabel("Çarp");
                SummonerMenu.CreateKeyBind("Çarp Devredışı", "smiteKeybind", 'Z', 'U');
                SummonerMenu.CreateCheckBox("Çarp menzilini göster.", "drawSmiteRange");
                SummonerMenu.CreateCheckBox("Çarp ne kadar hasar verecek göster HP barında.", "drawSmiteDamage");
                
                var combo = SummonerMenu.Add("comboBox", new ComboBox("Çarp Modu", new List<string> {"Use Prediction", " Dont use prediction"}));
                var label = SummonerMenu.Add("comboBoxText", new Label("aaa"));
                switch (combo.CurrentValue)
                {
                    case 0:
                        label.CurrentValue = "It will try to predict the health of the jungle minion to have a fast Smite";
                        break;
                    case 1:
                        label.CurrentValue = "It will only use if the jungle minion health is lower than the smite damage";
                        break;
                }
                combo.OnValueChange += delegate (ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
                {
                    switch (sender.CurrentValue)
                    {
                        case 0:
                            label.CurrentValue = "It will try to predict the health of the jungle minion to have a fast Smite";
                            break;
                        case 1:
                            label.CurrentValue = "It will only use if the jungle minion health is lower than the smite damage";
                            break;
                    }
                };
                SummonerMenu.AddSeparator();
                SummonerMenu.CreateSlider("Çarp hasarını hesapla", "sliderDMGSmite", 15, 0, 50);
                SummonerMenu.AddSeparator();
                SummonerMenu.CreateCheckBox("Şampiyonlara çarp kullan", "smiteUseOnChampions");
                SummonerMenu.CreateSlider("Kaç kişi varsa çarp kullansın", "smiteKeep", 1, 0, 2);

                switch (Game.MapId)
                {
                    case GameMapId.TwistedTreeline:
                        SummonerMenu.AddLabel("Epic");
                        SummonerMenu.CreateCheckBox("Çarp Örümcek Bossu", "monster" + "TT_Spiderboss");
                        SummonerMenu.AddLabel("Normal");
                        SummonerMenu.CreateCheckBox("Çarp Golem", "monster" + "TTNGolem");
                        SummonerMenu.CreateCheckBox("Çarp Kurtlar", "monster" + "TTNWolf");
                        SummonerMenu.CreateCheckBox("Çarp Hayalet", "monster" + "TTNWraith", false);
                        break;
                    case GameMapId.SummonersRift:
                        SummonerMenu.AddLabel("Efsanevi");
                        SummonerMenu.CreateCheckBox("Çarp Baron", "monster" + "SRU_Baron");
                        SummonerMenu.CreateCheckBox("Çarp Ejder", "monster" + "SRU_Dragon");
                        SummonerMenu.CreateCheckBox("Çarp BaronunKızKardeşi", "monster" + "SRU_RiftHerald");
                        SummonerMenu.AddLabel("Normal");
                        SummonerMenu.CreateCheckBox("Çarp Mavi", "monster" + "SRU_Blue");
                        SummonerMenu.CreateCheckBox("Çarp Kırmızı", "monster" + "SRU_Red");
                        SummonerMenu.CreateCheckBox("Çarp YampiriYengeç", "monster" + "Sru_Crab", false);
                        SummonerMenu.AddLabel("Peh...");
                        SummonerMenu.CreateCheckBox("Çarp Kurbağa", "monster" + "SRU_Gromp", false);
                        SummonerMenu.CreateCheckBox("Çarp Alacakurtlar", "monster" + "SRU_Murkwolf", false);
                        SummonerMenu.CreateCheckBox("Çarp SivriGagalar", "monster" + "SRU_Razorbeak", false);
                        SummonerMenu.CreateCheckBox("Çarp Golem", "monster" + "SRU_Krug", false);
                        break;
                }
            }

            if (PlayerHasBarrier)
            {
                SummonerMenu.AddGroupLabel("Bariyer");
                SummonerMenu.CreateCheckBox("Kullan Barrier.", "check" + "barrier");
                SummonerMenu.CreateSlider("Use it if MY health is lower than ({0}%).", "slider" + "barrier", 20);
            }

            if (PlayerHasHeal)
            {
                SummonerMenu.AddGroupLabel("İyileştirme");
                SummonerMenu.CreateCheckBox("Kullan İyileştirme.", "check" + "heal");
                SummonerMenu.CreateSlider("Benim canım şundan azsa ({0}%).", "slider" + "heal" + "me", 20);
                SummonerMenu.CreateSlider("Dostlarımın canı şundan azsa ({0}%).", "slider" + "heal" + "ally", 10);
            }

            if (PlayerHasIgnite)
            {
                SummonerMenu.AddGroupLabel("Tutuştur");
                SummonerMenu.CreateCheckBox("Kullan Tutuştur.", "check" + "ignite");
                SummonerMenu.AddLabel("Gelişmiş seçenekleri ellememeniz gerektiği için açmadım");
                SummonerMenu.CreateSlider("Tutuştur için en az menzil", "minimunRangeIgnite", 480, 0, 600);
            }

            if (PlayerHasPoroThrower)
            {
                SummonerMenu.AddGroupLabel("KarTopu");
                SummonerMenu.CreateCheckBox("Kullan KarTopu.", "check" + "snowball");
            }

            if (PlayerHasCleanse)
            {
                SummonerMenu.AddGroupLabel("Bu aktivatör yapacağını otomatik tespit eder ve uygular eğer değiştrmek istersen kendine göre düzenle");
            }

            #endregion SummonerSpells

            #region Misc
            MiscMenu.AddGroupLabel("Kaşif(totemler)");
            MiscMenu.AddLabel("Pembe");
            MiscMenu.CreateCheckBox("Gizli Düşmanları ortaya çıkar(vayne gibi)", "revelInviEnemiesTrinket");
            MiscMenu.CreateCheckBox("Gizlenebilen düşman bir çalıya girerse ortaya çıkar", "revelBushEnemiesTrinket");
            MiscMenu.AddLabel("Normal Totem");
            MiscMenu.CreateCheckBox("Görünür düşmanlarda Kullan", "revelInviEnemiesWard");
            MiscMenu.CreateCheckBox("Görünebilen düşman çime girerse totem kullan ve ortaya çıkar", "revelBushEnemiesWard");
            #endregion Misc

            #region Drawings

            DrawingMenu.AddGroupLabel("Tüm Gösterge Ayarları");
            DrawingMenu.CreateCheckBox("Tüm Göstergeler Devredışı", "disableDrawings", false);

            DrawingMenu.AddGroupLabel("Saldırgan İtemler");
            DrawingMenu.CreateCheckBox("Bilgewater Palası", "draw" + (int)ItemId.Bilgewater_Cutlass);
            DrawingMenu.CreateCheckBox("Mahvolmuş Kılıç", "draw" + (int)ItemId.Blade_of_the_Ruined_King);
            DrawingMenu.CreateCheckBox("Tiamat", "draw" + (int)ItemId.Tiamat);
            DrawingMenu.CreateCheckBox("Vahşi Hydra", "draw" + (int)ItemId.Ravenous_Hydra);
            DrawingMenu.CreateCheckBox("Haşmetli Hydra", "draw" + (int)ItemId.Titanic_Hydra);
            DrawingMenu.CreateCheckBox("Youmuus Kılıcı", "draw" + (int)ItemId.Youmuus_Ghostblade);
            DrawingMenu.CreateCheckBox("Hextech Kılıcı", "draw" + (int)ItemId.Hextech_Gunblade);

            DrawingMenu.AddGroupLabel("Defansif İtemler");
            DrawingMenu.CreateCheckBox("Dağın Sureti", "draw" + (int)ItemId.Face_of_the_Mountain);
            DrawingMenu.CreateCheckBox("Solarinin Demir Broşu", "draw" + (int)ItemId.Locket_of_the_Iron_Solari);
            DrawingMenu.CreateCheckBox("Randuins", "draw" + (int)ItemId.Randuins_Omen);
            DrawingMenu.CreateCheckBox("OhmYıkıcı", "draw" + (int)ItemId.Ohmwrecker);

            #endregion Drawings

            #region Settings
            SettingsMenu.AddGroupLabel("Tehlike Ayarları");
            SettingsMenu.AddLabel("Lütfen Mario Lib Menu Kullanın");
            SettingsMenu.AddGroupLabel("Saldırdan item ayarları");
            SettingsMenu.CreateCheckBox("Saldırgan itemleri sadece kombo modunda kullan", "comboUseItems");
            SettingsMenu.AddGroupLabel("FPS drop önle");
            SettingsMenu.AddLabel("The higher the slider below is, the more fps you will get and the slower the tick will be");
            SettingsMenu.CreateSlider("Tick Limitleyici", "tickLimiter", 0, 0, 1000);
            SettingsMenu.AddSeparator();
            SettingsMenu.AddGroupLabel("HataAyıklama");
            SettingsMenu.CreateCheckBox("Geliştirici Hata Ayıklama Aktif.", "dev", false);

            #endregion Settings

            FirstMenu.InitiliazeDangerHandler();
        }
    }
}
