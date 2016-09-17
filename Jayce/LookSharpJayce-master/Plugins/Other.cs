using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using Color = System.Drawing.Color;

namespace LookSharp.Plugins
{
    class Other : PluginBase
    {
        public Other()
            : base()
        {
            ModeMenu = PluginMenu.AddSubMenu("Modes", "Modes");
            ModeMenu.AddGroupLabel("Combo");
            ModeMenu.Add("Qcombo", new CheckBox("Use Q"));
            ModeMenu.Add("Wcombo", new CheckBox("Use W"));
            ModeMenu.Add("Ecombo", new CheckBox("Use E"));
            ModeMenu.Add("Rcombo", new CheckBox("Use R"));

            MiscMenu = PluginMenu.AddSubMenu("Misc", "Misc");
            MiscMenu.AddGroupLabel("Key Binds");
            MiscMenu.Add("Special", new KeyBind("Special", false, KeyBind.BindTypes.HoldActive, 'A'));
            MiscMenu.Add("Insec", new KeyBind("Insec", false, KeyBind.BindTypes.HoldActive, 'G'));
            MiscMenu.Add("FlashInsec", new CheckBox("->Use Flash Insec"));

            MiscMenu.AddGroupLabel("Settings");
            MiscMenu.Add("Gapcloser", new CheckBox("Gapcloser"));
            MiscMenu.Add("Interrupt", new CheckBox("Interrupt"));

            MiscMenu.AddGroupLabel("Kill Steal");
            MiscMenu.Add("Eks", new CheckBox("E Killsteal"));

            DrawMenu = PluginMenu.AddSubMenu("Drawing", "Drawing");
            DrawMenu.AddGroupLabel("Ability Ranges");
            DrawMenu.Add("Q", new CheckBox("Draw Q"));
            DrawMenu.Add("W", new CheckBox("Draw W"));
            DrawMenu.Add("E", new CheckBox("Draw E"));
            DrawMenu.Add("R", new CheckBox("Draw R"));

            Gapcloser.OnGapcloser += OnGapCloser;
            Interrupter.OnInterruptableSpell += OnInterruptableSpell;
        }

        public override void OnTick()
        {
            // code
        }

        public override void OnPaint()
        {
            if (DrawMenu["Q"].Cast<CheckBox>().CurrentValue)
                Circle.Draw(Q.IsReady() ? SharpDX.Color.Green : SharpDX.Color.Red, Q.Range, Hero.Position);
            if (DrawMenu["W"].Cast<CheckBox>().CurrentValue)
                Circle.Draw(W.IsReady() ? SharpDX.Color.Green : SharpDX.Color.Red, W.Range, Hero.Position);
            if (DrawMenu["E"].Cast<CheckBox>().CurrentValue)
                Circle.Draw(E.IsReady() ? SharpDX.Color.Green : SharpDX.Color.Red, E.Range, Hero.Position);
            if (DrawMenu["R"].Cast<CheckBox>().CurrentValue)
                Circle.Draw(R.IsReady() ? SharpDX.Color.Green : SharpDX.Color.Red, R.Range, Hero.Position);
            
        }

        private void OnGapCloser(AIHeroClient target, Gapcloser.GapcloserEventArgs spell)
        {
            if (MiscMenu["Gapcloser"].Cast<CheckBox>().CurrentValue && target.IsEnemy)
            {
                // code
            }
        }

        private void OnInterruptableSpell(Obj_AI_Base target, Interrupter.InterruptableSpellEventArgs spell)
        {
            if (MiscMenu["Interrupt"].Cast<CheckBox>().CurrentValue && target.IsEnemy)
            {
                // code
            }
        }

        public override void Combo()
        {
            // code
        }
    }
}
