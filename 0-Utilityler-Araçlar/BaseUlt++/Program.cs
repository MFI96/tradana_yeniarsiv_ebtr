using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace BaseUltPlusPlus
{
    public class Program
    {
        public static Menu BaseUltMenu { get; set; }

        public static void Main(string[] args)
        {
            // Wait till the name has fully loaded
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            //Menu
            BaseUltMenu = MainMenu.AddMenu("BaseUlt++", "BUP");
            BaseUltMenu.AddGroupLabel("BaseUlt++ Genel Ayarlar");
            BaseUltMenu.AddSeparator();
            BaseUltMenu.Add("baseult", new CheckBox("BaseUlt"));
            BaseUltMenu.Add("showrecalls", new CheckBox("Geridönüş göster"));
            BaseUltMenu.Add("showallies", new CheckBox("Dostları"));
            BaseUltMenu.Add("showenemies", new CheckBox("Düşmanları"));
            BaseUltMenu.Add("checkcollision", new CheckBox("Kontrol sağla"));
            BaseUltMenu.AddSeparator();
            BaseUltMenu.Add("timeLimit", new Slider("FOW zaman limiti (SEC)", 0, 0, 120));
            BaseUltMenu.AddSeparator();
            BaseUltMenu.Add("nobaseult", new KeyBind("Base ulti şurda kullanma", false, KeyBind.BindTypes.HoldActive, 32));
            BaseUltMenu.AddSeparator();
            BaseUltMenu.Add("x", new Slider("Offset X", 0, -500, 500));
            BaseUltMenu.Add("y", new Slider("Offset Y", 0, -500, 500));
            BaseUltMenu.AddGroupLabel("BaseUlt++ Hedefleri");
            foreach (var unit in EntityManager.Heroes.Enemies)
            {
                BaseUltMenu.Add("target" + unit.ChampionName,
                    new CheckBox(string.Format("{0} ({1})", unit.ChampionName, unit.Name)));
            }

            BaseUltMenu.AddGroupLabel("BaseUlt++ Yapanlar");
            BaseUltMenu.AddLabel("By: LunarBlue (Fixed by: Roach_)");
            BaseUltMenu.AddLabel("Deneyen: FinnDev, MrOwl");

            // Initialize the Addon
            OfficialAddon.Initialize();

            // Listen to the two main events for the Addon
            Game.OnUpdate += args1 => OfficialAddon.Game_OnUpdate();
            Drawing.OnEndScene += args1 => OfficialAddon.Drawing_OnEndScene();
            Teleport.OnTeleport += OfficialAddon.Teleport_OnTeleport;
        }
    }
}