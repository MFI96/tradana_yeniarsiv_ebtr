using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using JusticeTalon;
using SharpDX;
using SharpDX.Direct3D9;
using Color = System.Drawing.Color;


namespace Mario_sGangplank.Ultilities
{
    internal class DamageIndicator
    {
        //Offsets
        private const float YOff = 9.8f;
        private const float XOff = 0;
        private const float Width = 107;
        private const float Thick = 9.82f;
        //Offsets
        private static Font _Font, _Font2;
        private static Color color = Color.Yellow;

        public static void Init()
        {
            Drawing.OnEndScene += Drawing_OnEndScene;

            _Font = new Font(
                Drawing.Direct3DDevice,
                new FontDescription
                {
                    FaceName = "Segoi UI",
                    Height = 16,
                    Weight = FontWeight.Bold,
                    OutputPrecision = FontPrecision.Default,
                    Quality = FontQuality.ClearType,
                });

            _Font2 = new Font(
                Drawing.Direct3DDevice,
                new FontDescription
                {
                    FaceName = "Segoi UI",
                    Height = 11,
                    Weight = FontWeight.Bold,
                    OutputPrecision = FontPrecision.Default,
                    Quality = FontQuality.ClearType,
                });
        }

        private static void Drawing_OnEndScene(EventArgs args)
        {
            foreach (
                var enemy in
                    EntityManager.Heroes.Enemies.Where(e => e.IsValid && e.IsHPBarRendered && e.TotalShieldHealth() > 10)
                )
            {
                var damage = Damages.GetTotalDamage(enemy);
                if (Config.Modes.DrawDmg.damageDraw)
                {
                    //Drawing Line Over Enemies Helth bar
                    var dmgPer = (enemy.TotalShieldHealth() - damage > 0 ? enemy.TotalShieldHealth() - damage : 0) /
                                 enemy.TotalShieldMaxHealth();
                    var currentHPPer = enemy.TotalShieldHealth() / enemy.TotalShieldMaxHealth();
                    var initPoint = new Vector2((int)(enemy.HPBarPosition.X + XOff + dmgPer * Width),
                        (int)enemy.HPBarPosition.Y + YOff);
                    var endPoint = new Vector2((int)(enemy.HPBarPosition.X + XOff + currentHPPer * Width) + 1,
                        (int)enemy.HPBarPosition.Y + YOff);

                    var colour = Color.FromArgb(180, color);
                    EloBuddy.SDK.Rendering.Line.DrawLine(colour, Thick, initPoint, endPoint);
                }

                if (Config.Modes.DrawDmg.statDraw)
                {
                    //Statistics
                    var posXStat = (int)enemy.HPBarPosition[0];
                    var posYStat = (int)enemy.HPBarPosition[1] - 7;
                    var mathStat = "-" + Math.Round(damage) + " / " +
                                   Math.Round(enemy.Health - damage);
                    _Font2.DrawText(null, mathStat, posXStat, posYStat, SharpDX.Color.Yellow);
                }

                if (Config.Modes.DrawDmg.perDraw)
                {
                    //Percent
                    var posXPer = (int)enemy.HPBarPosition[0] + 106;
                    var posYPer = (int)enemy.HPBarPosition[1] - 12;
                    _Font.DrawText(null, string.Concat(Math.Ceiling((int)damage / enemy.TotalShieldHealth() * 100), "%"),
                        posXPer, posYPer, SharpDX.Color.Yellow);
                }
            }
        }
    }
}