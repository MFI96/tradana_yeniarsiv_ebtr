
using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using Color = System.Drawing.Color;

namespace LookSharp.Plugins
{
    class Viktor : PluginBase
    {
        private const int EMaxRange = 1225;
        public Viktor()
            : base()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 600);

            W = new Spell.Skillshot(SpellSlot.W, 700, SkillShotType.Circular, 500, int.MaxValue, 300)
            {
                MinimumHitChance = HitChance.High,
                AllowedCollisionCount = int.MaxValue
            };
            E = new Spell.Skillshot(SpellSlot.E, 525, SkillShotType.Linear, 250, int.MaxValue, 100)
            {
                MinimumHitChance = HitChance.High,
                AllowedCollisionCount = int.MaxValue
            };
            R = new Spell.Skillshot(SpellSlot.R, 700, SkillShotType.Circular, 250, int.MaxValue, 450)
            {
                MinimumHitChance = HitChance.High,
                AllowedCollisionCount = int.MaxValue
            };

            ModeMenu = PluginMenu.AddSubMenu("Modes", "Modes");
            ModeMenu.AddGroupLabel("Combo");
            ModeMenu.Add("Qcombo", new CheckBox("Use Q"));
            ModeMenu.Add("Wcombo", new CheckBox("Use W", false));
            ModeMenu.Add("Ecombo", new CheckBox("Use E"));
            ModeMenu.Add("Rcombo", new CheckBox("Use R"));
            ModeMenu.Add("Ignite", new CheckBox("Use Ignite"));
            
            ModeMenu.AddGroupLabel("Harass");
            ModeMenu.Add("Qharass", new CheckBox("Use Q"));
            ModeMenu.Add("Eharass", new CheckBox("Use E"));

            MiscMenu = PluginMenu.AddSubMenu("Misc", "Misc");
            MiscMenu.AddGroupLabel("KillSteal");
            MiscMenu.Add("KillSteal", new CheckBox("Enable KillSteal"));
            MiscMenu.AddGroupLabel("Settings");
            MiscMenu.Add("Rtick", new Slider("R tick count for dmg calc", 4, 1, 14));
            MiscMenu.Add("Wteam", new CheckBox("Use W in team fight"));
            MiscMenu.Add("Wcount", new Slider("->If will hit", 2, 2, 5));
            MiscMenu.Add("Rteam", new CheckBox("Use Ult in team fight"));
            MiscMenu.Add("Rcount", new Slider("->If will hit", 3, 2, 5));

            DrawMenu = PluginMenu.AddSubMenu("Drawing", "Drawing");
            DrawMenu.AddGroupLabel("Ability Ranges");
            DrawMenu.Add("Q", new CheckBox("Draw Q"));
            DrawMenu.Add("W", new CheckBox("Draw W", false));
            DrawMenu.Add("E", new CheckBox("Draw E"));
            DrawMenu.Add("R", new CheckBox("Draw R"));
        }


        public override void OnPaint()
        {
            AIHeroClient target = TargetSelector.GetTarget(EMaxRange, DamageType.Magical);
            if (target != null)
            {
                Vector3 startPos = Hero.Position + Vector3.Normalize(target.Position - Hero.Position) * E.Range;
                Vector3 endPos = ((Spell.Skillshot)E).GetPrediction(target).UnitPosition;
                Circle.Draw(SharpDX.Color.Green, 20, startPos);
                Circle.Draw(SharpDX.Color.Red, 20, endPos);
            }


            Circle.Draw(SharpDX.Color.Orange, E.Range, Hero.Position);
            

            if (DrawMenu["Q"].Cast<CheckBox>().CurrentValue)
                Circle.Draw(Q.IsReady() ? SharpDX.Color.Green : SharpDX.Color.Red, Q.Range, Hero.Position);
            if (DrawMenu["W"].Cast<CheckBox>().CurrentValue)
                Circle.Draw(W.IsReady() ? SharpDX.Color.Green : SharpDX.Color.Red, W.Range, Hero.Position);
            if (DrawMenu["E"].Cast<CheckBox>().CurrentValue)
                Circle.Draw(E.IsReady() ? SharpDX.Color.Green : SharpDX.Color.Red, EMaxRange, Hero.Position);
            if (DrawMenu["R"].Cast<CheckBox>().CurrentValue)
                Circle.Draw(R.IsReady() ? SharpDX.Color.Green : SharpDX.Color.Red, R.Range, Hero.Position);
        }

        public override void Combo()
        {
            AIHeroClient target = TargetSelector.GetTarget(EMaxRange, DamageType.Magical);
            if (ModeMenu["Ecombo"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                CastE(target);
            }
        }

        public override void Harass()
        {
            AIHeroClient target = TargetSelector.GetTarget(EMaxRange, DamageType.Magical);
        }

        private void CastE(AIHeroClient target)
        {
            if (E.IsInRange(target))
            {
                ((Spell.Skillshot)E).SourcePosition = target.ServerPosition;
                Player.CastSpell(SpellSlot.E,  ((Spell.Skillshot)E).GetPrediction(target).UnitPosition, target.ServerPosition);
            }
            else if (Hero.Distance(target) < EMaxRange)
            {
                Vector3 startPos = Hero.Position + Vector3.Normalize(target.Position - Hero.Position) * E.Range;
                ((Spell.Skillshot)E).SourcePosition = startPos;
                Player.CastSpell(SpellSlot.E, ((Spell.Skillshot)E).GetPrediction(target).UnitPosition, startPos);
            }
        }
        private float AAdamage(AIHeroClient target)
        {
            return Hero.CalculateDamageOnUnit(target, DamageType.Mixed, new float[] { 0, 20, 25, 30, 35, 40, 45, 50, 55, 60, 70, 80, 90, 110, 130, 150, 170, 190, 210 }[Hero.Level] + 0.5f * Hero.FlatMagicDamageMod + Hero.TotalAttackDamage);
        }
        private float Qdamage(AIHeroClient target)
        {
            return Hero.CalculateDamageOnUnit(target, DamageType.Magical, new float[] { 0, 40, 60, 80, 100, 120 }[Q.Level] + 0.2f * Hero.FlatMagicDamageMod);
        }
        private float Edamage(AIHeroClient target)
        {
            return Hero.CalculateDamageOnUnit(target, DamageType.Magical, new float[] { 0, 70, 110, 150, 190, 230 }[E.Level] + 0.7f * Hero.FlatMagicDamageMod);
        }
        private float Rdamage(AIHeroClient target)
        {
            return Hero.CalculateDamageOnUnit(target, DamageType.Magical, new float[] { 0, 150, 250, 350 }[R.Level] + 0.55f * Hero.FlatMagicDamageMod + new float[] { 0, 15, 30, 45 }[R.Level] * 0.1f * Hero.FlatMagicDamageMod * MiscMenu["Rtick"].Cast<Slider>().CurrentValue);
        }
    }
}