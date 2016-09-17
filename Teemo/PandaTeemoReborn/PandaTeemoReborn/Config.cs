using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using Color = System.Drawing.Color;

namespace PandaTeemoReborn
{
    internal class Config
    {        
        public static Menu PandaTeemoReborn,
            ComboMenu,
            HarassMenu,
            LaneClearMenu,
            JungleClearMenu,
            KillStealMenu,
            FleeMenu,
            DrawingMenu,
            MiscMenu,
            AutoShroomMenu;

        static Config()
        {
            PandaTeemoReborn = MainMenu.AddMenu("PandaTeemoReborn", "PTR");
            PandaTeemoReborn.AddGroupLabel("Bu addon KarmaPanda Tarafından Geliştirilmiştir.");
            PandaTeemoReborn.AddGroupLabel(
                "Kimseden yardım alınmamıştır.");
            PandaTeemoReborn.AddGroupLabel("Addonumu Kullandığınız için Teşekkür Ederim!");
            PandaTeemoReborn.AddGroupLabel("Çeviri TRAdana");

            ComboMenu = PandaTeemoReborn.AddSubMenu("Combo", "Combo");
            ComboMenu.AddLabel("Büyü Ayarları");
            ComboMenu.Add("useQ", new CheckBox("Q Kullan"));
            ComboMenu.Add("useW", new CheckBox("W Kullan"));
            ComboMenu.Add("useR", new CheckBox("R Kullan"));
            ComboMenu.AddLabel("ManaYardımcısı");
            ComboMenu.Add("manaQ", new Slider("Q dan önce hesapla"));
            ComboMenu.Add("manaW", new Slider("Wden önce hesapla"));
            ComboMenu.Add("manaR", new Slider("R den önce hesapla"));
            ComboMenu.AddLabel("Q Ayarları");
            ComboMenu.Add("checkAA", new Slider("Q için menzil: {0}", 0, 0, 180));
            ComboMenu.AddLabel("R Ayarları");
            ComboMenu.Add("doubleShroom", new CheckBox("Çift mantar mantığı kullan"));
            ComboMenu.Add("rPoison", new CheckBox("Hedef sadece zehirlenmemişse R Kullan"));
            ComboMenu.Add("rCharge", new Slider("R kullanmak için mevcut R yükü: {0}", 2, 1, 3));
            ComboMenu.Add("rDelay", new Slider("R kullanma gecikmesi: {0}", 1000, 0, 5000));
            ComboMenu.AddLabel("Ek Ayarları");
            ComboMenu.Add("adc", new CheckBox("Qyu sadece adc de kullan", false));
            ComboMenu.Add("wRange", new CheckBox("W yi sadece hedef menzildeyse kullan"));

            HarassMenu = PandaTeemoReborn.AddSubMenu("Harass", "Harass");
            HarassMenu.AddGroupLabel("Büyü Ayarları");
            HarassMenu.Add("useQ", new CheckBox("Q Kullan"));
            HarassMenu.Add("useW", new CheckBox("W Kullan", false));
            HarassMenu.AddLabel("ManaYardımcısı");
            HarassMenu.Add("manaQ", new Slider("Q dan önce hesapla"));
            HarassMenu.Add("manaW", new Slider("W den önce hesapla"));
            HarassMenu.AddLabel("Q Ayarları");
            HarassMenu.Add("checkAA", new Slider("Q için menzil: {0}", 0, 0, 180));
            HarassMenu.AddLabel("Misc Ayarları");
            HarassMenu.Add("adc", new CheckBox("Qyu sadece adc de kullan", false));
            HarassMenu.Add("wRange", new CheckBox("W yi sadece hedef menzildeyse kullan"));

            LaneClearMenu = PandaTeemoReborn.AddSubMenu("LaneClear", "LaneClear");
            LaneClearMenu.AddLabel("Büyü Ayarları");
            LaneClearMenu.Add("useQ", new CheckBox("Q Kullan"));
            LaneClearMenu.Add("useR", new CheckBox("R Kullan"));
            LaneClearMenu.AddLabel("ManaYardımcısı");
            LaneClearMenu.Add("manaQ", new Slider("Q için en az mana", 50));
            LaneClearMenu.Add("manaR", new Slider("R için en az mana", 50));
            LaneClearMenu.AddLabel("Q Ayarları");
            LaneClearMenu.Add("harass", new CheckBox("Dürtme mantığı Kullan"));
            LaneClearMenu.Add("disableLC", new CheckBox("Laneclearda mantığı devredışı bırak"));
            LaneClearMenu.AddLabel("R Ayarları");
            LaneClearMenu.Add("rKillable", new CheckBox("Sadece minyonlar ölecekse R fırlat"));
            LaneClearMenu.Add("rPoison", new CheckBox("Sadece minyonlar zehirlenmemişse at"));
            LaneClearMenu.Add("rCharge", new Slider("R kullanmak için mevcut R yükü: {0}", 2, 1, 3));
            LaneClearMenu.Add("rDelay", new Slider("R için gecikme: {0}", 1000, 0, 5000));
            LaneClearMenu.Add("minionR", new Slider("Şu kadar minyon varsa R Kullan: {0}", 3, 1, 4));

            JungleClearMenu = PandaTeemoReborn.AddSubMenu("JungleClear", "JungleClear");
            JungleClearMenu.AddGroupLabel("Büyü Ayarları");
            JungleClearMenu.Add("useQ", new CheckBox("Q Kullan"));
            JungleClearMenu.Add("useR", new CheckBox("R Kullan"));
            JungleClearMenu.AddLabel("ManaYardımcısı");
            JungleClearMenu.Add("manaQ", new Slider("Q için en az mana", 25));
            JungleClearMenu.Add("manaR", new Slider("R için en az mana", 25));
            JungleClearMenu.AddLabel("R Ayarları");
            JungleClearMenu.Add("rKillable", new CheckBox("Sadece canavar ölecekse R Kullan", false));
            JungleClearMenu.Add("rPoison", new CheckBox("Sadece zehirlenmemişse R Kullan"));
            JungleClearMenu.Add("rCharge", new Slider("R kullanmak için mevcut R yükü: {0}", 2, 1, 3));
            JungleClearMenu.Add("rDelay", new Slider("R gecikmesi: {0}", 1000, 0, 5000));
            JungleClearMenu.Add("mobR", new Slider("R için gereken canavar sayısı: {0}", 1, 1, 4));
            JungleClearMenu.AddLabel("Ek Ayarları");
            JungleClearMenu.Add("bMob", new CheckBox("Küçük canavarlara büyü kullanmayı önle"));

            KillStealMenu = PandaTeemoReborn.AddSubMenu("Kill Steal", "Kill Steal");
            KillStealMenu.AddGroupLabel("Büyü Ayarları");
            KillStealMenu.Add("useQ", new CheckBox("Q Kullan"));
            KillStealMenu.Add("useR", new CheckBox("R Kullan", false));
            KillStealMenu.AddLabel("ManaYardımcısı");
            KillStealMenu.Add("manaQ", new Slider("Q için en az mana", 25));
            KillStealMenu.Add("manaR", new Slider("R için en az mana", 25));
            KillStealMenu.AddLabel("R Ayarları");
            KillStealMenu.Add("rDelay", new Slider("R gecikmesi: {0}", 1000, 0, 5000));
            KillStealMenu.Add("doubleShroom", new CheckBox("Çift mantar mantığını kullan"));

            FleeMenu = PandaTeemoReborn.AddSubMenu("Flee Menu", "Flee");
            FleeMenu.AddGroupLabel("Flee Ayarları");
            FleeMenu.Add("useW", new CheckBox("W Kullan"));
            FleeMenu.Add("useR", new CheckBox("R Kullan"));
            FleeMenu.AddLabel("R Ayarları");
            FleeMenu.Add("rDelay", new Slider("R gecikmesi: {0}", 1000, 0, 5000));
            FleeMenu.Add("rCharge", new Slider("R kullanmak için mevcut R yükü: {0}", 2, 1, 3));

            AutoShroomMenu = PandaTeemoReborn.AddSubMenu("Auto Shroom", "Auto Shroom");
            AutoShroomMenu.AddGroupLabel("Otomatik Mantar Ayarları");
            AutoShroomMenu.Add("useR", new CheckBox("R kullan"));
            AutoShroomMenu.Add("manaR", new Slider("R için en az mana", 25));
            AutoShroomMenu.Add("rCharge", new Slider("R kullanmak için mevcut R yükü: {0}", 2, 1, 3));
            AutoShroomMenu.Add("enableShroom", new CheckBox("Otomatik Mantar Yükle (F5 basman gerek)"));
            AutoShroomMenu.Add("enableDefaultLocations", new CheckBox("Varsayılan konumları kullan(mantar atma konumları) (F5 basman gerek)"));
            AutoShroomMenu.AddLabel("Hata Ayıklama Modu");
            var enable = AutoShroomMenu.Add("enableDebug", new CheckBox("Aktif hata ayıklama", false));
            enable.OnValueChange += delegate(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
            {
                if (!args.NewValue)
                {
                    Chat.Print("PandaTeemo | Hata ayiklama devredisi", System.Drawing.Color.LawnGreen);
                }
                else
                {
                    Chat.Print("PandaTeemo | Hata Ayiklama aktif", System.Drawing.Color.Red);
                }
            };
            var save = AutoShroomMenu.Add("saveButton", new KeyBind("Ayarları kaydet", false, KeyBind.BindTypes.HoldActive, 'K'));
            save.OnValueChange += delegate(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
            {
                if (!args.NewValue)
                {
                    return;
                }

                if (Extensions.MenuValues.AutoShroom.DebugMode)
                {
                    save.CurrentValue = false;
                    AutoShroom.SavePositions();
                }
            };
            AutoShroomMenu.AddLabel("Mantar atma konumları");
            AutoShroomMenu.Add("posMode", new ComboBox("Position Mode", 0, "Save Mouse", "Save Player Position"));
            var add = AutoShroomMenu.Add("newposButton", new KeyBind("Konumu kaydet", false, KeyBind.BindTypes.HoldActive, 'L'));
            add.OnValueChange += delegate(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
            {
                if (!args.NewValue)
                {
                    return;
                }

                if (Extensions.MenuValues.AutoShroom.DebugMode)
                {
                    add.CurrentValue = false;

                    Vector3 newPosition = Vector3.Zero;

                    switch (Extensions.MenuValues.AutoShroom.PositionMode.CurrentValue)
                    {
                        case 0:
                            newPosition = Game.CursorPos;
                            break;
                        case 1:
                            newPosition = Player.Instance.Position;
                            break;
                    }

                    if (newPosition != Vector3.Zero && !AutoShroom.ShroomPosition.Contains(newPosition))
                    {
                        AutoShroom.AddShroomLocation(newPosition);
                        AutoShroom.SavePositions();
                    }
                }
            };
            var remove = AutoShroomMenu.Add("delposButton", new KeyBind("Konumu Sil", false, KeyBind.BindTypes.HoldActive, 'U'));
            remove.OnValueChange += delegate(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
            {
                if (!args.NewValue)
                {
                    return;
                }

                if (Extensions.MenuValues.AutoShroom.DebugMode)
                {
                    remove.CurrentValue = false;
                }

                Vector3 newPosition = Vector3.Zero;

                switch (Extensions.MenuValues.AutoShroom.PositionMode.CurrentValue)
                {
                    case 0:
                        newPosition = Game.CursorPos;
                        break;
                    case 1:
                        newPosition = Player.Instance.Position;
                        break;
                }

                if (newPosition == Vector3.Zero) return;

                var nearbyShrooms = AutoShroom.PlayerAssignedShroomPosition.Where(pos => pos.IsInRange(newPosition, SpellManager.R.Radius)).ToList();

                if (!nearbyShrooms.Any())
                {
                    return;
                }

                AutoShroom.RemoveShroomLocations(nearbyShrooms);
                AutoShroom.SavePositions();
            };

            DrawingMenu = PandaTeemoReborn.AddSubMenu("Drawing", "Drawing");
            DrawingMenu.AddGroupLabel("Gösterge Ayarları");
            DrawingMenu.Add("drawQ", new CheckBox("Göster Q Menzili"));
            DrawingMenu.Add("drawR", new CheckBox("Göster R Menzili"));
            DrawingMenu.Add("drawautoR", new CheckBox("Göster Otomatik mantar atma yerleri"));
            DrawingMenu.Add("drawdoubleR", new CheckBox("Göster Çift mantar mantığını", false));

            MiscMenu = PandaTeemoReborn.AddSubMenu("Misc", "Misc");
            MiscMenu.AddGroupLabel("Büyü Ayarları");
            MiscMenu.Add("autoQ", new CheckBox("Otomatik Q", false));
            MiscMenu.Add("autoW", new CheckBox("Otomatik W", false));
            MiscMenu.Add("intq", new CheckBox("Interrupt ile Q"));
            MiscMenu.Add("gapR", new CheckBox("Gapcloser ile R"));
        }

        public static void Initialize()
        {
        }
    }
}