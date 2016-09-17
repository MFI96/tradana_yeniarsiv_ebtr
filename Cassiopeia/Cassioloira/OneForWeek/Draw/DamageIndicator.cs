﻿using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;

namespace OneForWeek.Draw
{

    public static class DamageIndicator
    {
        private const int BarWidth = 104;
        private const int LineThickness = 10;

        public delegate float DamageToUnitDelegate(AIHeroClient hero);

        private static DamageToUnitDelegate DamageToUnit { get; set; }

        private static readonly Vector2 BarOffset = new Vector2(0, 0);

        private static System.Drawing.Color _drawingColor;
        public static System.Drawing.Color DrawingColor
        {
            get { return _drawingColor; }
            set { _drawingColor = System.Drawing.Color.FromArgb(170, value); }
        }

        public static bool Enabled { get; set; }

        public static void Initialize(DamageToUnitDelegate damageToUnit)
        {
            // Apply needed field delegate for damage calculation
            DamageToUnit = damageToUnit;
            DrawingColor = System.Drawing.Color.DarkOrange;
            Enabled = true;

            // Register event handlers
            Drawing.OnEndScene += Drawing_OnDraw;
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Enabled)
            {
                foreach (var unit in EntityManager.Heroes.Enemies.Where(u => u.IsValidTarget() && u.IsHPBarRendered))
                {
                    // Get damage to unit
                    var damage = DamageToUnit(unit);

                    // Continue on 0 damage
                    if (damage <= 0)
                        continue;

                    // Get remaining HP after damage applied in percent and the current percent of health
                    var damagePercentage = ((unit.Health - damage) > 0 ? (unit.Health - damage) : 0) / unit.MaxHealth;
                    var currentHealthPercentage = unit.Health / unit.MaxHealth;

                    // Calculate start and end point of the bar indicator
                    var startPoint = new Vector2((int)(unit.HPBarPosition.X + BarOffset.X + damagePercentage * BarWidth), (int)(unit.HPBarPosition.Y + BarOffset.Y) - 5);
                    var endPoint = new Vector2((int)(unit.HPBarPosition.X + BarOffset.X + currentHealthPercentage * BarWidth) + 1, (int)(unit.HPBarPosition.Y + BarOffset.Y) - 5);

                    // Draw the line
                    Drawing.DrawLine(startPoint, endPoint, LineThickness, DrawingColor);
                }
            }
        }
    }
}
