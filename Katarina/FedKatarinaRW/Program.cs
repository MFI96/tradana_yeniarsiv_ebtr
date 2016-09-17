using System;
using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using System.Linq;

namespace FedKatarinaV2
{
    internal class Program
    {
                //Spells
        public static Spell.Targeted Q;
        public static Spell.Active W;
        public static Spell.Targeted E;
        public static Spell.Active R;
        public static Menu menu, DrawingMenu, KillStealMenu;

        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        private static void Main(string[] args)
        {
            if (args != null)
            {
                try
                {
                    Loading.OnLoadingComplete += Load_OnLoad;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        private static void Load_OnLoad(EventArgs a)
        {

            if (Player.Instance.Hero != Champion.Katarina) return;

            menu = MainMenu.AddMenu("FedKatarinaV2", "FedSeries");
            menu.AddGroupLabel("Fed KatarinaV2");
            menu.AddLabel("Version: " + "1.0.0.0");
            menu.AddSeparator();
            menu.AddLabel("MostlyPride");
            menu.AddSeparator();
            menu.AddLabel("+Rep If you use this :)");
            menu.AddLabel("Çeviri TRAdana");

            DrawingMenu = menu.AddSubMenu("Gösterge", "FedSeriesDrawings");
            DrawingMenu.AddGroupLabel("Gösterge Ayarları");
            DrawingMenu.Add("dQ", new CheckBox("Göster Q", true));
            DrawingMenu.Add("dW", new CheckBox("Göster W", true));
            DrawingMenu.Add("dE", new CheckBox("Göster E", true));
            DrawingMenu.Add("dR", new CheckBox("Göster R", true));

            KillStealMenu = menu.AddSubMenu("Kill Çalma", "FedSeriesKillSteal");
            KillStealMenu.AddGroupLabel("Kill Çalma Ayarları");
            KillStealMenu.Add("kQ", new CheckBox("Kullan Q", true));
            KillStealMenu.Add("kW", new CheckBox("Kullan W", true));
            KillStealMenu.Add("kE", new CheckBox("Kullan E", true));
            KillStealMenu.Add("kR", new CheckBox("Kullan R", true));


            Q = new Spell.Targeted(SpellSlot.Q, 675);

            W = new Spell.Active(SpellSlot.W, 375);

            E = new Spell.Targeted(SpellSlot.E, 700);

            R = new Spell.Active(SpellSlot.R, 550);


            Drawing.OnDraw += Drawing_OnDraw;
            StateManager.Init();
            WardJumper.Init();
            DamageIndicator.DamageIndicator.Initialize(DamageIndicator.SpellDamage.GetTotalDamage);


            Chat.Print("FedKatarinaV2 Yuklendi!tradana iyi oyunlar diler", System.Drawing.Color.LightBlue);
        }


        private static void Drawing_OnDraw(EventArgs args)
        {
            if (DrawingMenu["dQ"].Cast<CheckBox>().CurrentValue)
            {
               Circle.Draw(Q.IsReady() ? Color.Green : Color.Red, Q.Range, Player.Instance.Position); 
            }
            if (DrawingMenu["dW"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(W.IsReady() ? Color.Green : Color.Red, W.Range, Player.Instance.Position);
            }
            if (DrawingMenu["dE"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(E.IsReady() ? Color.Green : Color.Red, E.Range, Player.Instance.Position);
            }
            if (DrawingMenu["dR"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(R.IsReady() ? Color.Green : Color.Red, R.Range, Player.Instance.Position);
            }
        }
    }
}