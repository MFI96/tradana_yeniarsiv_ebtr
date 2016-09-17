using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using YasuoBuddy.EvadePlus;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy.SDK.Enumerations;
using SharpDX;

namespace YasuoBuddy
{
    internal class Yasuo
    {
        public static Menu Menu, ComboMenu, HarassMenu, FarmMenu, FleeMenu, DrawMenu, MiscSettings;
        public static Spell.Targeted E;
        public static Spell.Skillshot E2;
        private static int _cleanUpTime;

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.Hero != Champion.Yasuo) return;

            Menu = MainMenu.AddMenu("TheNewYasuo", "yasuobuddyfluxy");

            ComboMenu = Menu.AddSubMenu("Combo", "yasuCombo");
            ComboMenu.AddGroupLabel("Combo");
            ComboMenu.Add("combo.Q", new CheckBox("Kullan Q"));
            ComboMenu.Add("combo.E", new CheckBox("Kullan E"));
            ComboMenu.Add("combo.EUnderTower", new CheckBox("Kule altında E", false));
            ComboMenu.Add("combo.stack", new CheckBox("Yük kas Q"));
            ComboMenu.Add("combo.leftclickRape", new CheckBox("Sol tuş odaklan"));
            ComboMenu.AddSeparator();
            ComboMenu.AddLabel("Ultimate");
            ComboMenu.Add("combo.R", new CheckBox("Kullan R"));
            ComboMenu.Add("combo.RTarget", new CheckBox("Ryi sadece seçili hedef için uygunsa kullan"));
            ComboMenu.Add("combo.RKillable", new CheckBox("Kullan R KS"));
            ComboMenu.Add("combo.MinTargetsR", new Slider("Kullan R için en az düşman", 2, 1, 5));

            HarassMenu = Menu.AddSubMenu("Harass", "yasuHarass");
            HarassMenu.AddGroupLabel("Dürtme");
            HarassMenu.Add("harass.Q", new CheckBox("Kullan Q"));
            HarassMenu.Add("harass.E", new CheckBox("Kullan E"));
            HarassMenu.Add("harass.stack", new CheckBox("Yük kas Q"));

            FarmMenu = Menu.AddSubMenu("Farming", "yasuoFarm");
            FarmMenu.AddGroupLabel("Farm");
            FarmMenu.AddLabel("Son Vuruş");
            FarmMenu.Add("LH.Q", new CheckBox("Kullan Q"));
            FarmMenu.Add("LH.E", new CheckBox("Kullan E"));
            FarmMenu.Add("LH.EUnderTower", new CheckBox("Kule altında E", false));

            FarmMenu.AddLabel("Lanetemizleme");
            FarmMenu.Add("WC.Q", new CheckBox("Kullan Q"));
            FarmMenu.Add("WC.E", new CheckBox("Kullan E"));
            FarmMenu.Add("WC.EUnderTower", new CheckBox("Kule altında E", false));

            FarmMenu.AddLabel("Orman");
            FarmMenu.Add("JNG.Q", new CheckBox("Kullan Q"));
            FarmMenu.Add("JNG.E", new CheckBox("Kullan E"));

            FleeMenu = Menu.AddSubMenu("Flee/Evade", "yasuoFlee");
            FleeMenu.AddGroupLabel("Flee");
            FleeMenu.Add("Flee.E", new CheckBox("Kullan E"));
            FleeMenu.Add("Flee.stack", new CheckBox("Yük kas  Q"));
            FleeMenu.AddGroupLabel("Evade");
            FleeMenu.Add("Evade.E", new CheckBox("Kaçmada E Kullan"));
            FleeMenu.Add("Evade.W", new CheckBox("Kaçarken W Kullan"));
            FleeMenu.Add("Evade.WDelay", new Slider("İnsancıl Gecikme (ms)", 0, 0, 1000));
            //
            FleeMenu.AddGroupLabel("WallJump");
            FleeMenu.Add("WJ", new KeyBind("Walljump Tuşu:", false, KeyBind.BindTypes.HoldActive, 'G'));
            FleeMenu.Add("DrawSpots", new CheckBox("Göster Walljump Yerleri"));
            //
            MiscSettings = Menu.AddSubMenu("Diversas/Misc");
            MiscSettings.AddGroupLabel("KS");
            MiscSettings.Add("KS.Q", new CheckBox("Kullan Q"));
            MiscSettings.Add("KS.E", new CheckBox("Kullan E"));
            MiscSettings.AddGroupLabel("Otomatik Q");
            MiscSettings.Add("Auto.Q3", new CheckBox("Kullan Q3"));
            MiscSettings.Add("Auto.Active", new KeyBind("Otomatik Q3 düşmana", true, KeyBind.BindTypes.PressToggle, 'M'));

            Program.Main(null);

            DrawMenu = Menu.AddSubMenu("Draw", "yasuoDraw");
            DrawMenu.AddGroupLabel("Gösterge Ayarları");

            DrawMenu.Add("Draw.Q", new CheckBox("Göster Q", true));
            DrawMenu.AddColourItem("Draw.Q.Colour");
            DrawMenu.AddSeparator();

            DrawMenu.Add("Draw.E", new CheckBox("Göster E", false));
            DrawMenu.AddColourItem("Draw.E.Colour");
            DrawMenu.AddSeparator();

            DrawMenu.Add("Draw.R", new CheckBox("Göster R", false));
            DrawMenu.AddColourItem("Draw.R.Colour");
            DrawMenu.AddSeparator();

            DrawMenu.AddLabel("Gerisayım rengi:", 4);
            DrawMenu.AddColourItem("Draw.Down", 7);
            
            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += Drawing_OnDraw;
            EEvader.Init();

            //

            E = new Spell.Targeted(SpellSlot.E, 475);
            E2 = new Spell.Skillshot(SpellSlot.E, 475, EloBuddy.SDK.Enumerations.SkillShotType.Linear);


            //
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (DrawMenu["Draw.Q"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(
                    SpellManager.Q.IsReady() ? DrawMenu.GetColour("Draw.Q.Colour") : DrawMenu.GetColour("Draw.Down"),
                    SpellManager.Q.Range, Player.Instance.Position);
            }
            if (DrawMenu["Draw.R"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(
                    SpellManager.R.IsReady() ? DrawMenu.GetColour("Draw.R.Colour") : DrawMenu.GetColour("Draw.Down"),
                    SpellManager.R.Range, Player.Instance.Position);
            }
            if (DrawMenu["Draw.E"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(
                    SpellManager.E.IsReady() ? DrawMenu.GetColour("Draw.E.Colour") : DrawMenu.GetColour("Draw.Down"),
                    SpellManager.E.Range, Player.Instance.Position);
            }
            if (FleeMenu["DrawSpots"].Cast<CheckBox>().CurrentValue)
            {
                Drawing.DrawCircle(YasuoWall.spotA.To3D(), 50, System.Drawing.Color.HotPink);
                Drawing.DrawCircle(YasuoWall.spotB.To3D(), 50, System.Drawing.Color.HotPink);
                Drawing.DrawCircle(YasuoWall.spotC.To3D(), 50, System.Drawing.Color.HotPink);
                Drawing.DrawCircle(YasuoWall.spotD.To3D(), 50, System.Drawing.Color.HotPink);
                Drawing.DrawCircle(YasuoWall.spotE.To3D(), 50, System.Drawing.Color.HotPink);
                //Drawing.DrawCircle(YasuoWall.spotF.To3D(), 50, System.Drawing.Color.HotPink);//Blue
                Drawing.DrawCircle(YasuoWall.spotG.To3D(), 50, System.Drawing.Color.HotPink);
                Drawing.DrawCircle(YasuoWall.spotH.To3D(), 50, System.Drawing.Color.HotPink);
                Drawing.DrawCircle(YasuoWall.spotI.To3D(), 50, System.Drawing.Color.HotPink);
                Drawing.DrawCircle(YasuoWall.spotJ.To3D(), 50, System.Drawing.Color.HotPink);
                Drawing.DrawCircle(YasuoWall.spotK.To3D(), 50, System.Drawing.Color.HotPink);
                Drawing.DrawCircle(YasuoWall.spotL.To3D(), 50, System.Drawing.Color.HotPink);
                Drawing.DrawCircle(YasuoWall.spotM.To3D(), 50, System.Drawing.Color.HotPink);
                Drawing.DrawCircle(YasuoWall.spotN.To3D(), 50, System.Drawing.Color.HotPink);
                Drawing.DrawCircle(YasuoWall.spotK.To3D(), 50, System.Drawing.Color.HotPink);
                Drawing.DrawCircle(YasuoWall.spotO.To3D(), 50, System.Drawing.Color.HotPink);
            }
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (_cleanUpTime < Environment.TickCount)
            {
                GC.Collect();
                _cleanUpTime = Environment.TickCount + 1000000;
            }
            StateManager.KillSteal();
            if (MiscSettings["Auto.Active"].Cast<KeyBind>().CurrentValue)
            {
                StateManager.AutoQ();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                StateManager.Flee();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                StateManager.Harass();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                StateManager.Combo();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                StateManager.LastHit();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                StateManager.WaveClear();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                StateManager.Jungle();
            }
            if (FleeMenu["WJ"].Cast<KeyBind>().CurrentValue)
            {
                YasuoWall.WallDash();
            }
        }
    }
}