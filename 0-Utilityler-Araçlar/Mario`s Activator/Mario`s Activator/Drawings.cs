using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Rendering;
using Mario_s_Lib;
using static Mario_s_Activator.SummonerSpells;
using static Mario_s_Activator.MyMenu;
using Color = System.Drawing.Color;

namespace Mario_s_Activator
{
    internal class Drawings
    {
        public static void InitializeDrawings()
        {
            Drawing.OnDraw += Drawing_OnDraw;
            Drawing.OnEndScene += Drawing_OnEndScene;
        }

        #region DrawLine

        internal static int Height;
        internal static int Width;
        internal static int OffsetX;
        internal static int OffsetY;

        private static void Drawing_OnEndScene(EventArgs args)
        {
            if (PlayerHasSmite && Smite.IsReady() && SummonerMenu.GetCheckBoxValue("drawSmiteDamage"))
            {
                foreach (
                    var jg in
                        EntityManager.MinionsAndMonsters.GetJungleMonsters()
                            .Where(m => m.IsValidTarget(Smite.Range + 400) && m.IsHPBarRendered && !m.Name.Contains("Mini")))
                {
                    if (jg.Name.Contains("Blue"))
                    {
                        Height = 9;
                        Width = 142;
                        OffsetX = -4;
                        OffsetY = 7;
                    }

                    if (jg.Name.Contains("Red"))
                    {
                        Height = 9;
                        Width = 142;
                        OffsetX = -4;
                        OffsetY = 7;
                    }

                    if (jg.Name.Contains("Dragon"))
                    {
                        Height = 10;
                        Width = 143;
                        OffsetX = -4;
                        OffsetY = 8;
                    }

                    if (jg.Name.Contains("Baron"))
                    {
                        Height = 12;
                        Width = 191;
                        OffsetX = -29;
                        OffsetY = 6;
                    }

                    if (jg.Name.Contains("Herald"))
                    {
                        Height = 10;
                        Width = 142;
                        OffsetX = -4;
                        OffsetY = 7;
                    }

                    if (jg.Name.Contains("Razorbeak"))
                    {
                        Height = 3;
                        Width = 74;
                        OffsetX = 30;
                        OffsetY = 7;
                    }

                    if (jg.Name.Contains("Murkwolf"))
                    {
                        Height = 3;
                        Width = 74;
                        OffsetX = 30;
                        OffsetY = 7;
                    }

                    if (jg.Name.Contains("Krug"))
                    {
                        Height = 3;
                        Width = 80;
                        OffsetX = 27;
                        OffsetY = 7;
                    }

                    if (jg.Name.Contains("Gromp"))
                    {
                        Height = 3;
                        Width = 86;
                        OffsetX = 24;
                        OffsetY = 6;
                    }

                    if (jg.Name.Contains("Crab"))
                    {
                        Height = 2;
                        Width = 61;
                        OffsetX = 36;
                        OffsetY = 21;
                    }

                    DrawLine(jg);
                }
            }
        }

        private static void DrawLine(Obj_AI_Base unit)
        {
            var barPos = unit.HPBarPosition;
            var percentHealthAfterDamage = Math.Max(0, unit.Health - SmiteDamage())/
                                           (unit.MaxHealth + unit.AllShield + unit.AttackShield + unit.MagicShield);
            var currentHealthPercentage = unit.Health/(unit.MaxHealth + unit.AllShield + unit.AttackShield + unit.MagicShield);
            var startPoint = barPos.X + OffsetX + (percentHealthAfterDamage*Width);
            var endPoint = barPos.X + OffsetX + (currentHealthPercentage*Width);
            var yPos = barPos.Y + OffsetY;
            var canKill = SmiteDamage() - 10 > unit.Health;

            Drawing.DrawLine(startPoint, yPos, endPoint, yPos, Height, canKill ? Color.BlueViolet : Color.Beige);
        }

        #endregion DrawLine

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (DrawingMenu.GetCheckBoxValue("disableDrawings")) return;

            if (PlayerHasSmite)
            {
                if (Smite.IsReady() && !SummonerMenu.GetKeyBindValue("smiteKeybind") && SummonerMenu.GetCheckBoxValue("drawSmiteRange"))
                {
                    Circle.Draw(SharpDX.Color.Yellow, Smite.Range, Player.Instance);
                }
            }

            foreach (
                var item in
                    Offensive.OffensiveItems.Where(i => i.IsReady() && i.Range > 0)
                        .Where(item => DrawingMenu.GetCheckBoxValue("draw" + (int) item.Id)))
            {
                Circle.Draw(SharpDX.Color.Orange, item.Range, Player.Instance);
            }

            foreach (
                var item in
                    Defensive.DefensiveItems.Where(i => i.IsReady() && i.Range > 0)
                        .Where(item => DrawingMenu.GetCheckBoxValue("draw" + (int) item.Id)))
            {
                Circle.Draw(SharpDX.Color.BlueViolet, item.Range, Player.Instance);
            }

            if (SettingsMenu.GetCheckBoxValue("dev"))
            {
                foreach (var m in DangerHandler.Missiles)
                {
                    Circle.Draw(SharpDX.Color.Purple, 20f, 5f, m);
                }

                Circle.Draw(SharpDX.Color.Blue, Player.Instance.BoundingRadius + Initializer.SettingsMenu.GetSliderValue("saferange"), Player.Instance);
            }
        }
    }
}
