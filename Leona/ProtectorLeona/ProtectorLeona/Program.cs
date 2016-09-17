using System;
using System.Drawing;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;

namespace ProtectorLeona
{
    // Created by Counter
    internal class Program
    {

        // Grab Player Attributes
        public static AIHeroClient Champion { get { return Player.Instance; } }
        public static int ChampionSkin;


        public static void Main()
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        public static void Loading_OnLoadingComplete(EventArgs args)
        {
            // Validate Player.Instace is Addon Champion
            if (Champion.ChampionName != "Leona") return;
            ChampionSkin = Champion.SkinId;

            // Initialize classes
            SpellManager.Initialize();
            MenuManager.Initialize();

            // Listen to Events
            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnUpdate += Game_OnUpdate;
            Game.OnTick += SpellManager.ConfigSpells;
            Game.OnTick += Game_OnTick;
            Interrupter.OnInterruptableSpell += ModeManager.InterruptMode;
            Gapcloser.OnGapcloser += ModeManager.GapCloserMode;
        }

        public static void Game_OnUpdate(EventArgs args)
        {
            Champion.SetSkinId(MenuManager.DesignerMode ? MenuManager.DesignerSkin : ChampionSkin);
        }

        public static void Drawing_OnDraw(EventArgs args)
        {
            // Wait for Game Load
            if (Game.Time < 10) return;

            // No Responce While Dead
            if (Champion.IsDead) return;

            Color color;

            switch (Champion.SkinId)
            {
                default:
                    color = Color.Transparent;
                    break;
                case 0:
                    color = Color.Goldenrod;
                    break;
                case 1:
                    color = Color.SandyBrown;
                    break;
                case 2:
                    color = Color.IndianRed;
                    break;
                case 3:
                    color = Color.LightSteelBlue;
                    break;
                case 4:
                    color = Color.OrangeRed;
                    break;
                case 5:
                    color = Color.HotPink;
                    break;
                case 6:
                    color = Color.LightSkyBlue;
                    break;
                case 7:
                    color = Color.Orange;
                    break;
                case 8:
                    color = Color.Yellow;
                    break;
            }
            if (!MenuManager.DrawerMode) return;
            if (MenuManager.DrawQ && SpellManager.Q.IsLearned)
                Drawing.DrawCircle(Champion.Position, SpellManager.Q.Range, color);
            if (MenuManager.DrawW && SpellManager.W.IsLearned)
                Drawing.DrawCircle(Champion.Position, SpellManager.W.Range, color);
            if (MenuManager.DrawE && SpellManager.E.IsLearned)
                Drawing.DrawCircle(Champion.Position, SpellManager.E.Range, color);
            if (MenuManager.DrawR && SpellManager.R.IsLearned)
                Drawing.DrawCircle(Champion.Position, SpellManager.R.Range, color);
        }

        public static void Game_OnTick(EventArgs args)
        {
            // Initialize Leveler
            if (MenuManager.LevelerMode && Champion.SpellTrainingPoints >= 1)
                LevelerManager.Initialize();
            // No Responce While Dead
            if (Champion.IsDead) return;

            // Mode Activation
            switch (Orbwalker.ActiveModesFlags)
            {
                case Orbwalker.ActiveModes.Combo:
                    ModeManager.ComboMode();
                    break;
                case Orbwalker.ActiveModes.Harass:
                    ModeManager.HarassMode();
                    break;
            }
        }
    }
}