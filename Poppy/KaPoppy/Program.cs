using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using SharpDX;
using System;
using Color = System.Drawing.Color;

namespace KaPoppy
{
    using EloBuddy.SDK.Rendering;
    using System.Linq;
    using Config = Settings.MiscSettings;
    class Program : Helper
    {
        private static void Main(string[] args) { Loading.OnLoadingComplete += Game_OnStart; }
        static Vector3 start = new Vector3();
        static Vector3 end = new Vector3();
        private static void Game_OnStart(EventArgs args)
        {
            if (myHero.Hero != Champion.Poppy) return;

            CheckForUpdates();
            Settings.Init();
            DamageIndicator.Initialize(Extensions.GetComboDamage);
            ItemManager.Init();

            var flash = myHero.Spellbook.Spells.Where(x => x.Name.ToLower().Contains("summonerflash"));
            SpellDataInst Flash = flash.Any() ? flash.First() : null;
            if (Flash != null)
            {
                Lib.Flash = new Spell.Targeted(Flash.Slot, 425);
            }

            Obj_AI_Base.OnProcessSpellCast += Modes.Misc.SpellCast;
            Obj_AI_Base.OnSpellCast += RectangleManager;
            
            Drawing.OnDraw += Drawings;

            GameObject.OnCreate += Modes.Misc.AntiRengar;

            GameObject.OnCreate += GameObject_OnCreate;
            GameObject.OnDelete += GameObject_OnDelete;

            Obj_AI_Base.OnBuffGain += Obj_AI_Base_OnBuffGain;

            Game.OnUpdate += Game_OnUpdate;
        }

        private static void Obj_AI_Base_OnBuffGain(Obj_AI_Base sender, Obj_AI_BaseBuffGainEventArgs args)
        {
            if (sender.IsMe)
            {
                if (args.Buff.Name == "poppypassiveshield")
                {
                    Lib.Passive = null;
                }
            }
        }

        private static void GameObject_OnCreate(GameObject sender, EventArgs args)
        {
            if (sender.IsAlly)
            {
                if (sender.Name.ToLower() == "shield")
                {
                    Lib.Passive = sender;
                }
            }
        }
        private static void GameObject_OnDelete(GameObject sender, EventArgs args)
        {
            if (sender.IsAlly)
            {
                if (sender.Name.ToLower() == "shield")
                {
                    Lib.Passive = null;
                }
            }
        }
        private static void RectangleManager(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe)
            {
                if (args.Slot == SpellSlot.Q)
                {
                    start = args.Start;
                    end = args.Start.Extend(args.End, Lib.Q.Range).To3D();
                    Core.DelayAction(() => start = new Vector3(), 1000);
                    Core.DelayAction(() => end = new Vector3(), 1000);
                }
            }
        }
        public static Geometry.Polygon.Rectangle rectangle = null;
        private static void Drawings(EventArgs args)
        {
            var target = TargetSelector.SelectedTarget?? TargetSelector.GetTarget(Lib.R.MaximumRange - 250, DamageType.Physical);
            var starget = TargetSelector.SelectedTarget;
            if (Config.DrawStunPos && starget.IsValidTarget())
            {
                var stunpos = Drawing.WorldToScreen(Lib.PointsAroundTheTarget(starget).Where(x => Lib.CanStun(starget, x)).OrderBy(x => x.Distance(myHero)).FirstOrDefault().To3D());
                if (stunpos.IsValid(true))
                {
                    Line.DrawLine(Color.Red, 2, myHero.Position.WorldToScreen(), stunpos);
                }
            }
            if (Config.StunTarget || Config.SemiAutoR)
            {
                if (target == null || target.IsDead || !target.IsValidTarget())
                {
                    Drawing.DrawText(Drawing.WorldToScreen(myHero.ServerPosition) - new Vector2(125, 30),
                         Color.Red, "Select a target" + Environment.NewLine + "with left click!", 9);
                }
                else
                {
                    Drawing.DrawText(Drawing.WorldToScreen(myHero.ServerPosition) - new Vector2(125, 30),
                         Color.White, "Target is:" + Environment.NewLine + target.ChampionName, 9);
                }
            }
            if (Config.drawQ)
            {
                Circle.Draw(Lib.Q.IsReady() ? Green : Red, Lib.Q.Range, myHero.Position);
            }
            if (Config.drawE)
            {
                Circle.Draw(Lib.E.IsReady() ? Green : Red, Lib.E.Range, myHero.Position);
            }
            if (Config.drawR)
            {
                Circle.Draw(Lib.R.IsReady() ? Green : Red, Lib.R.MaximumRange, myHero.Position);
            }
            if (Settings.ComboSettings.UseFlashE)
            {
                var pos = Drawing.WorldToScreen(myHero.ServerPosition) - new Vector2(-45, 15);
                Drawing.DrawText(pos, Color.White, "Flash stun!", 9);
            }
            if (Lib.Passive != null)
            {
                var pos1 = myHero.Position;
                var pos2 = Lib.Passive.Position;
                Line.DrawLine(Color.White, pos1, pos2);
            }

        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (myHero.IsDead) return;
            
            if (start.IsValid() && end.IsValid())
            {
                rectangle = new Geometry.Polygon.Rectangle(start, end, Lib.Q.Width);
                rectangle.Draw(Color.Red, 2);
            }
            else
                rectangle = null;

            if (Config.StunTarget)
                Modes.Stun.Execute();

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                Modes.Combo.Execute();
            
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
                Modes.Laneclear.Execute();

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
                Modes.Flee.Execute();

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
                Modes.Harass.Execute();

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
                Modes.Jungleclear.Execute();
            
            Modes.Misc.Execute();
        }
    }    
}