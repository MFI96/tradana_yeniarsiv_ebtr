using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Utils;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

using SharpDX;
using Color = System.Drawing.Color;

namespace GodSpeedRengar
{
    public class Config
    {
        public static Menu Menu, Modes, Draw, Magnet, Targetting;
        private static int _lastSwitchTick;
        public static void Initialize()
        {
            Menu = MainMenu.AddMenu(Player.Instance.ChampionName, "GodSpeedRengar");
            Menu.AddGroupLabel("Açıklama");
            Menu.AddLabel("Korkutucu Hareketler Yapma :D!");
            Menu.AddLabel("Çeviri TRAdana");

            
            // Modes
            Modes = Menu.AddSubMenu("Modlar", "modes");

            // Combo
            Modes.AddGroupLabel("Kombo");
            Variables.ComboSmite =  Modes.Add("comboUseSmite", new CheckBox("Tutuştur Kullan"));
            Variables.ComboYoumuu = Modes.Add("comboUseYoumuu", new CheckBox("Kullan Youmuu"));
            Modes.AddLabel("Modlar (1-Snare;2- OneShoot;3- Snare on Jump;4- Q always)");
            Variables.ComboMode = Modes.Add("comboMode", new Slider("Modlar",2,1,4));
            Variables.ComboSwitchKey = Modes.Add("comboSwitch", new KeyBind("Modu Değiştirme Tuşu", false, KeyBind.BindTypes.HoldActive, 'T'));

            Modes.AddSeparator();

            //Harass
            Modes.AddGroupLabel("Dürtme");
            Variables.HarassW = Modes.Add("harassUseW", new CheckBox("Kullan W"));
            Variables.HarassE = Modes.Add("harassUseE", new CheckBox("Kullan E"));

            Modes.AddSeparator();

            //LaneClear
            Modes.AddGroupLabel("LaneTemizleme");
            Variables.LaneQ = Modes.Add("laneUseQ", new CheckBox("Kullan Q"));
            Variables.LaneW = Modes.Add("laneUseW", new CheckBox("Kullan W"));
            Variables.LaneE = Modes.Add("laneUseE", new CheckBox("Kullan E"));
            Variables.LaneTiamat = Modes.Add("laneUseTiamat", new CheckBox("Kullan Tiamat/Hydra"));
            Variables.LaneSave = Modes.Add("laneSave", new CheckBox("5 yükü Sakla", false));

            Modes.AddSeparator();

            //JungleClear
            Modes.AddGroupLabel("OrmanTemizleme");
            Variables.JungQ = Modes.Add("jungUseQ", new CheckBox("Kullan Q"));
            Variables.JungW = Modes.Add("jungUseW", new CheckBox("Kullan W"));
            Variables.JungE = Modes.Add("jungUseE", new CheckBox("Kullan E"));
            Variables.JungTiamat = Modes.Add("jungUseTiamat", new CheckBox("Kullan Tiamat/Hydra"));
            Variables.JungSave = Modes.Add("jungSave", new CheckBox("5 yükü Sakla", false));

            Modes.AddSeparator();

            //Auto
            Modes.AddGroupLabel("Otomatik");
            Variables.AutoWHeal = Modes.Add("autoWHeal", new Slider("W için canım şundan az <",20,0,100));
            Variables.AutoEInterrupt = Modes.Add("autoEInterrupt", new CheckBox("Interrupt with E"));
            Variables.AutoSmiteKS = Modes.Add("autoSmiteKS", new CheckBox("Çarpla Ks at (blue/red)"));
            Variables.AutoESK = Modes.Add("autoEKS", new CheckBox("E Ks Kullan"));
            Variables.AutoWKS = Modes.Add("autoWKS", new CheckBox("W Ks Kullan"));
            Variables.AutoSmiteSteal = Modes.Add("autoSteal", new CheckBox("Çarpla Çal Ejder/Baron"));

            //drawing
            Draw = Menu.AddSubMenu("Gösterge","drawing");

            Variables.DrawMode = Draw.Add("drawMode", new CheckBox("Gösterge Modu"));
            //Variables.DrawSelectedTarget = Draw.Add("drawSelected", new CheckBox("Notify Selected Target While Steath"));

            Magnet = Menu.AddSubMenu("Magnet", "magnet");
            Magnet.AddLabel("Seçili hedefe doğru odaklan");
            Variables.MagnetEnable = Magnet.Add("magnetEnable", new CheckBox("Aktif", false));
            Variables.MagnetRange = Magnet.Add("magnetRange", new Slider("Magnet(Mıktatıs) Menzili", 300, 150, 500));

            Targetting = Menu.AddSubMenu("Hedefleme", "targetting");

            Targetting.AddGroupLabel("Ulti Zıplama Hedefleme");
            Variables.UltSelected = Targetting.Add("ultiSelected", new CheckBox("Hedef Önceliği Seç"));
            foreach (var hero in EntityManager.Heroes.Enemies)
            {
                Targetting.Add("ulti" + hero.NetworkId, 
                    new CheckBox(hero.ChampionName + "(" + hero.Name + ")"));
            }

            Targetting.AddGroupLabel("Çalıdan Hedefe Zıplama");
            Variables.BushSelected = Targetting.Add("bushSelected", new CheckBox("Hedef Önceliği Seç"));
            foreach (var hero in EntityManager.Heroes.Enemies)
            {
                Targetting.Add("bush" + hero.NetworkId,
                    new Slider(hero.ChampionName + "(" + hero.Name + ")",TargetSelector.GetPriority(hero),1,5));
            }

            Game.OnTick += Game_OnTick;
        }

        private static void Game_OnTick(EventArgs args)
        {
            ComboModeSwitch();
        }

        private static void ComboModeSwitch()
        {
            var lasttime = Environment.TickCount - _lastSwitchTick;
            if (!Variables.ComboSwitchKey.CurrentValue ||
                lasttime <= Game.Ping)
            {
                return;
            }

            switch (Variables.ComboMode.CurrentValue)
            {
                case 1:
                    Variables.ComboMode.CurrentValue = 2;
                    _lastSwitchTick = Environment.TickCount + 300;
                    break;
                case 2:
                    Variables.ComboMode.CurrentValue = 3;
                    _lastSwitchTick = Environment.TickCount + 300;
                    break;
                case 3:
                    Variables.ComboMode.CurrentValue = 4;
                    _lastSwitchTick = Environment.TickCount + 300;
                    break;
                case 4:
                    Variables.ComboMode.CurrentValue = 1;
                    _lastSwitchTick = Environment.TickCount + 300;
                    break;
            }
        }
    }
}
