using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Rendering;
using SharpDX;
using Settings = KA_Rumble.Config.Modes.Draw;
using ComboSet = KA_Rumble.Config.Modes.Combo;

namespace KA_Rumble
{
    internal class Rumble
    {
        public static void Initialize()
        {
            if(Player.Instance.ChampionName != "Rumble")return;

            SpellManager.Initialize();
            Config.Initialize();
            DMGHandler.DamageHandler.Initialize();
            ModeManager.Initialize();
            DamageIndicator.Initialize(SpellDamage.GetTotalDamage);
            EventsManager.Initialize();

            Drawing.OnDraw += OnDraw;
        }

        private static void OnDraw(EventArgs args)
        {
            var targetR = TargetSelector.GetTarget(SpellManager.R.Range, DamageType.Magical);
            if (targetR != null)
            {
                var enemies = EntityManager.Heroes.Enemies.Where(e => e.IsValidTarget()).Select(enemy => enemy.Position.To2D()).ToList();
                if (enemies.Any())
                {
                    var initPos = targetR.Position.To2D() - 200 * targetR.Direction.To2D().Perpendicular();
                    var endPos = Functions.GetBestEnPos(enemies, SpellManager.R.Width, 990, ComboSet.MinR, initPos);

                    if (endPos.X > 10)
                    {
                        Circle.Draw(SharpDX.Color.Yellow, 10, 20f, initPos.To3D());
                        Circle.Draw(SharpDX.Color.Orange, 10, 20f, endPos.To3D());
                        Drawing.DrawLine(initPos, endPos, 10f, System.Drawing.Color.DarkRed);
                    }
                }
            }


            if (Settings.DrawQ && Settings.DrawReady ? SpellManager.Q.IsReady() : Settings.DrawQ)
            {
                Circle.Draw(Settings.QColor, SpellManager.Q.Range, 1f, Player.Instance);
            }

            if (Settings.DrawW && Settings.DrawReady ? SpellManager.W.IsReady() : Settings.DrawW)
            {
                Circle.Draw(Settings.WColor, SpellManager.W.Range, 1f, Player.Instance);
            }

            if (Settings.DrawE && Settings.DrawReady ? SpellManager.E.IsReady() : Settings.DrawE)
            {
                Circle.Draw(Settings.EColor, SpellManager.E.Range, 1f, Player.Instance);
            }

            if (Settings.DrawR && Settings.DrawReady ? SpellManager.R.IsReady() : Settings.DrawR)
            {
                Circle.Draw(Settings.RColor, SpellManager.R.Range, 1f, Player.Instance);
            }
        }
    }
}
