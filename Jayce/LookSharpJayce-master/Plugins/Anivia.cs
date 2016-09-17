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
    class Anivia : PluginBase
    {
        private MissileClient missile = null;
        private Obj_AI_Base castedQon = null;

        public Anivia()
            : base()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1075, EloBuddy.SDK.Enumerations.SkillShotType.Linear, 0, 850, 110)
            {
                MinimumHitChance = HitChance.High,
                AllowedCollisionCount = int.MaxValue
            };
            
            W = new Spell.Skillshot(SpellSlot.W, 1000, EloBuddy.SDK.Enumerations.SkillShotType.Circular, 0, int.MaxValue, 1);
            E = new Spell.Targeted(SpellSlot.E, 650);
            R = new Spell.Skillshot(SpellSlot.R, 625, EloBuddy.SDK.Enumerations.SkillShotType.Circular, 0, int.MaxValue, 400)
            {
                AllowedCollisionCount = int.MaxValue
            };

            ModeMenu = PluginMenu.AddSubMenu("Modes", "Modes");
            MiscMenu = PluginMenu.AddSubMenu("Misc", "Misc");
            MiscMenu.AddGroupLabel("Key Binds");
            MiscMenu.Add("Special", new KeyBind("Special", false, KeyBind.BindTypes.HoldActive, 'A'));

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

            GameObject.OnCreate += OnCreate;
            Gapcloser.OnGapcloser += OnGapCloser;
            Interrupter.OnInterruptableSpell += OnInterruptableSpell;
        }

        public override void OnTick()
        {
            if (Prediction.Position.Collision.LinearMissileCollision(castedQon, missile.StartPosition.To2D(), missile.StartPosition.Extend(missile.EndPosition, Q.Range), 850, 150, Q.CastDelay))
            {

            }
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

            Drawing.DrawText(100, 100, Color.Red, "Q: " + Hero.Spellbook.GetSpell(SpellSlot.Q).ToggleState.ToString());
            Drawing.DrawText(100, 120, Color.Red, "R: " + Hero.Spellbook.GetSpell(SpellSlot.R).ToggleState.ToString());

            Circle.Draw(SharpDX.Color.Green, 50, missile.Position);

        }

        private void OnCreate(GameObject spell, EventArgs args)
        {
            if (spell.GetType() == typeof(MissileClient))
            {
                MissileClient miss = spell as MissileClient;
                if (miss.IsValidMissile() && miss.SpellCaster.NetworkId == Player.Instance.NetworkId && miss.SData.Name == "FlashFrostSpell")
                {
                    Chat.Print("Casted Q");
                    missile = miss;
                }
            }
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
            AIHeroClient target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);

            if (Q.IsReady() && Q.IsInRange(target))
            {
                Q.Cast(target);
                castedQon = target;
            }

            /*
            if (R.IsReady() && Hero.Spellbook.GetSpell(SpellSlot.R).ToggleState != 2)
            {
                if (E.IsReady() && E.IsInRange(target))
                {
                    E.Cast(target);
                }
                R.Cast(target);
            }*/
        }
    }
}
