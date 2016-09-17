using System;
using System.Linq;
using System.Runtime.InteropServices;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Rendering;
using SharpDX;

namespace AddonTemplate
{
    public class StealthHelper
    {
        public static void Initialize()
        {
            Drawing.OnEndScene += OnEndScene;
        }

        private static void OnEndScene(EventArgs args)
        {
            if (Config.Modes.Draw.StealthDistance)
            {
                Circle.Draw(Color.YellowGreen, GetAmbushTime() * Player.Instance.MoveSpeed, Player.Instance.Position);
            }
            if (Config.Modes.Draw.MinimapStealthDistance)
            {
                DrawCircleOnMinimap(Player.Instance.Position.X, Player.Instance.Position.Y, GetAmbushTime() * Player.Instance.MoveSpeed);
            }
        }

        private static void DrawCircleOnMinimap(float positionX, float positionY, float range)
        {
            float step = 2 * (float)Math.PI / 70;
            float h = positionX;
            float k = positionY;
            float r = range;
            float x_save = 0;
            float y_save = 0;

            for (float theta = 0; theta < 2 * Math.PI + 20; theta += step)
            {
                float x = h + r * (float)Math.Cos(theta);
                float y = k - r * (float)Math.Sin(theta);
                if (theta == 0)
                {
                    x_save = x;
                    y_save = y;
                }
                Vector2 saveVector = new Vector2(x_save, y_save);
                Vector2 newVector = new Vector2(x, y);
                if (x > 0 && x < 14500 && y > 0 && y < 14500 && x_save > 0 && x_save < 14500 && y_save > 0 && y_save < 14500) // Map limits (to fix bugged drawing)
                Drawing.DrawLine(Drawing.WorldToMinimap(saveVector.To3D()), Drawing.WorldToMinimap(newVector.To3D()), 1,
                    System.Drawing.Color.YellowGreen);
                x_save = x;
                y_save = y;
            }
        }

        public static float GetAmbushTime()
        {
            var ambush = Player.Instance.GetBuff("twitchhideinshadows");

            if (ambush != null)
            {
                if (ambush.StartTime - Game.Time < -0.3) // Fix to visual flickering
                return (ambush.EndTime - Game.Time);
            }
            return 0f;
        }
    }

}