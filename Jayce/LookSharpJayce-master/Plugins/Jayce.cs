using System;
using System.Collections.Generic;
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
    class Jayce : PluginBase
    {
        private bool isMelee { get { return !Hero.HasBuff("jaycestancegun"); } }
        private Spell.SpellBase Qcharge;
        private Vector3 gatePos;
        private float LastCast;

        Dictionary<int, Tuple<int, PredictionResult>> predictedPositions = new Dictionary<int, Tuple<int, PredictionResult>>();

        public Jayce()
            : base()
        {
            

            Qcharge = new Spell.Skillshot(SpellSlot.Q, 1650, SkillShotType.Linear, 25, 1600, 70);
            Q = new Spell.Targeted(SpellSlot.Q, 600);
            W = new Spell.Active(SpellSlot.W, 285);
            E = new Spell.Targeted(SpellSlot.E, 240);
            Q2 = new Spell.Skillshot(SpellSlot.Q, 1030, SkillShotType.Linear, 25, 1200, 70)
            {
                MinimumHitChance = HitChance.High,
            };
            W2 = new Spell.Active(SpellSlot.W);
            E2 = new Spell.Skillshot(SpellSlot.E, 650, SkillShotType.Circular, 1, int.MaxValue, 120);
            R = new Spell.Active(SpellSlot.R);

            ModeMenu = PluginMenu.AddSubMenu("Modes", "Modes");
            ModeMenu.AddGroupLabel("Combo");
            ModeMenu.Add("Qcombo", new CheckBox("Kullan Q Çekic"));
            ModeMenu.Add("Q2combo", new CheckBox("Kullan Q Top"));
            ModeMenu.Add("Wcombo", new CheckBox("Kullan W Çekic"));
            ModeMenu.Add("W2combo", new CheckBox("Kullan W Top"));
            ModeMenu.Add("Ecombo", new CheckBox("Kullan E Çekic"));
            ModeMenu.Add("QEcombo", new CheckBox("Kullan QE Top"));
            ModeMenu.Add("Rcombo", new CheckBox("Formu Değiştir(R)"));

            ModeMenu.AddGroupLabel("Harass");
            ModeMenu.Add("Q2harass", new CheckBox("Kullan Q Top"));
            ModeMenu.Add("W2harass", new CheckBox("Kullan W Top"));
            ModeMenu.Add("QEharass", new CheckBox("Kullan QE Top"));

            MiscMenu = PluginMenu.AddSubMenu("Misc", "Misc");
            MiscMenu.AddGroupLabel("Key Binds");
            MiscMenu.Add("Quickscope", new KeyBind("Quickscope", false, KeyBind.BindTypes.HoldActive, 'A'));
            MiscMenu.Add("Insec", new KeyBind("Insec", false, KeyBind.BindTypes.HoldActive, 'G'));
            MiscMenu.Add("FlashInsec", new CheckBox("->Flash insec"));

            MiscMenu.AddGroupLabel("Ayarlar");
            MiscMenu.Add("Gapcloser", new CheckBox("Kullan E  Gapcloser"));
            MiscMenu.Add("Interrupt", new CheckBox("Kullan E  Interrupt"));
            MiscMenu.Add("GateMode", new ComboBox("QE Mode", 0, "Vertical Fast", "Horizontal Fast", "Horizontal Slow"));
            MiscMenu.Add("GateDistance", new Slider("E mesafesi", 60, 60, 100));

            MiscMenu.AddGroupLabel("Kill Steal");
            MiscMenu.Add("QEks", new CheckBox("QE Killçalma"));

            DrawMenu = PluginMenu.AddSubMenu("Drawing", "Drawing");
            DrawMenu.AddGroupLabel("Büyü Menzilleri");
            DrawMenu.Add("Q", new CheckBox("Göster Q Çekic"));
            DrawMenu.Add("Q2", new CheckBox("Göster Q Top"));
            DrawMenu.Add("Qcharge", new CheckBox("Göster QE Top"));
            
            DrawMenu.AddGroupLabel("Diğer");
            DrawMenu.Add("Drawcds", new CheckBox("Göster Bekleme Süreleri"));

            
            Gapcloser.OnGapcloser += OnGapCloser;
            Interrupter.OnInterruptableSpell += OnInterruptableSpell;
        }

        public override void OnTick()
        {
            if (!Hero.IsDead)
            {
                ShouldE();
                UpdateCooldowns(isMelee);
                ShouldE();
                KillSteal();
                ShouldE();
                if (MiscMenu["Quickscope"].Cast<KeyBind>().CurrentValue)
                {
                    EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                    if (!isMelee && Q.IsReady() && E.IsReady())
                    {
                        CastQE(Game.CursorPos);
                    }
                }
                if (MiscMenu["Insec"].Cast<KeyBind>().CurrentValue)
                {
                    Insec();
                }
            }
        }

        public override void OnPaint()
        {
            if (!Hero.IsDead)
            {
                /* Debug
                {
                    AIHeroClient target = TargetSelector.SelectedTarget;
                    if (target != null && !target.IsDead)
                    {
                        PredictionResult QchargePred = ((Spell.Skillshot)Qcharge).GetPrediction(target);
                        if (QchargePred.HitChance >= HitChance.Low)
                        {
                            Circle.Draw(SharpDX.Color.Orange, target.BoundingRadius, QchargePred.UnitPosition);
                            Line.DrawLine(System.Drawing.Color.GreenYellow, Hero.Position, QchargePred.CastPosition);
                        }
                        else if (QchargePred.HitChance == HitChance.Collision)
                        {
                            Obj_AI_Base collision = QchargePred.CollisionObjects.OrderBy(x => x.Distance(Hero.Position)).First();

                            Circle.Draw(SharpDX.Color.Orange, collision.BoundingRadius, collision.Position);
                            Vector3 blast = extend(collision.Position, QchargePred.UnitPosition, collision.BoundingRadius, false);
                            Circle.Draw(SharpDX.Color.Red, 180, blast);
                        }
                    }
                }*/
                
                if (DrawMenu["Q"].Cast<CheckBox>().CurrentValue)
                    Circle.Draw(CD[0] == 0 ? SharpDX.Color.Green : SharpDX.Color.Red, Q.Range, Hero.Position);
                if (DrawMenu["Q2"].Cast<CheckBox>().CurrentValue)
                    Circle.Draw(CD[3] == 0 ? SharpDX.Color.Green : SharpDX.Color.Red, Q2.Range, Hero.Position);
                if (DrawMenu["Qcharge"].Cast<CheckBox>().CurrentValue)
                    Circle.Draw((CD[3] == 0 && CD[5] == 0) ? SharpDX.Color.Green : SharpDX.Color.Red, Qcharge.Range, Hero.Position);


                if (DrawMenu["Drawcds"].Cast<CheckBox>().CurrentValue)
                {
                    Vector2 wts = Drawing.WorldToScreen(Hero.Position);
                    wts[0] -= 40;
                    wts[1] += 20;
                    if (!isMelee)
                        for (int i = 0; i < 3; ++i)
                            if (CD[i] == 0)
                                Drawing.DrawText(wts[0] + (i * 30), wts[1], Color.Lime, "UP");
                            else
                                Drawing.DrawText(wts[0] + (i * 30), wts[1], Color.Orange, CD[i].ToString("0.0"));
                    else
                        for (int i = 3; i < 6; ++i)
                            if (CD[i] == 0)
                                Drawing.DrawText(wts[0] + ((i - 3) * 30), wts[1], Color.Lime, "UP");
                            else
                                Drawing.DrawText(wts[0] + ((i - 3) * 30), wts[1], Color.Orange, CD[i].ToString("0.0"));
                }

                if (MiscMenu["Insec"].Cast<KeyBind>().CurrentValue)
                {
                    AIHeroClient target = TargetSelector.GetTarget(Qcharge.Range, DamageType.Physical);
                    if (IsValidTarget(target))
                    {
                        Vector3 insecPos = extend(target.Position, Game.CursorPos, 150, false);
                        Vector2 wtsx = Drawing.WorldToScreen(Game.CursorPos);
                        Vector2 wts = Drawing.WorldToScreen(target.Position);
                        Drawing.DrawLine(wts[0], wts[1], wtsx[0], wtsx[1], 2, System.Drawing.Color.Red);
                        Circle.Draw(SharpDX.Color.Red, 110, insecPos);
                    }
                    
                }
            }
        }

        private void OnGapCloser(AIHeroClient target, Gapcloser.GapcloserEventArgs spell)
        {
            if (MiscMenu["Gapcloser"].Cast<CheckBox>().CurrentValue && target.IsEnemy && CD[2] == 0 && E.IsInRange(target) && (isMelee || ((!isMelee && R.IsReady() && R.Cast()))))
            {
                E.Cast(target);
            }
        }
        private void OnInterruptableSpell(Obj_AI_Base target, Interrupter.InterruptableSpellEventArgs spell)
        {
            if (MiscMenu["Interrupt"].Cast<CheckBox>().CurrentValue && target.IsEnemy && CD[2] == 0 && E.IsInRange(target) && (isMelee || ((!isMelee && R.IsReady() && R.Cast()))))
            {
                E.Cast(target);
            }
        }
        public override void Combo()
        {
            AIHeroClient target = TargetSelector.GetTarget(Qcharge.Range, DamageType.Physical);
            if (IsValidTarget(target))
            {
                if (!isMelee)
                {
                    ShouldE();
                    if (ModeMenu["QEcombo"].Cast<CheckBox>().CurrentValue && Q.IsReady() && E.IsReady() && E.IsLearned)
                    {
                        CastQE(target);
                    }
                    if (ModeMenu["Q2combo"].Cast<CheckBox>().CurrentValue && Q2.IsReady() && !E.IsReady())
                    {
                        Q2.Cast(target);
                    }
                    if (ModeMenu["W2combo"].Cast<CheckBox>().CurrentValue && W2.IsReady() && Hero.Distance(target.Position) <= Hero.AttackRange + 25)
                    {
                        W2.Cast();
                    }
                    if (ModeMenu["Rcombo"].Cast<CheckBox>().CurrentValue && Q.IsInRange(target) && !Q2.IsReady() && !W2.IsReady() && R.IsReady())
                    {
                        R.Cast();
                    }
                }
                else
                {
                    if (ModeMenu["Qcombo"].Cast<CheckBox>().CurrentValue && Q.IsReady() && Q.IsInRange(target))
                    {
                        Q.Cast(target);
                    }
                    if (ModeMenu["Wcombo"].Cast<CheckBox>().CurrentValue && W.IsReady() && W.IsInRange(target))
                    {
                        W.Cast();
                    }
                    if (ModeMenu["Rcombo"].Cast<CheckBox>().CurrentValue && !Q.IsReady() && !W.IsReady() && R.IsReady())
                    {
                        if (ModeMenu["Ecombo"].Cast<CheckBox>().CurrentValue && E.IsReady())
                        {
                            E.Cast(target);
                        }
                        R.Cast();
                    }
                }
            }
        }
        public override void Harass()
        {
            AIHeroClient target = TargetSelector.GetTarget(Qcharge.Range, DamageType.Physical);
            if (IsValidTarget(target))
            {
                if (!isMelee || ((isMelee && R.IsReady() && R.Cast())))
                {
                    if (ModeMenu["QEharass"].Cast<CheckBox>().CurrentValue && Q.IsReady() && E.IsReady() && E.IsLearned)
                    {
                        CastQE(target);
                    }
                    if (ModeMenu["Q2harass"].Cast<CheckBox>().CurrentValue && Q2.IsReady() && !E.IsReady())
                    {
                        Q2.Cast(target);
                    }
                    if (ModeMenu["W2harass"].Cast<CheckBox>().CurrentValue && W2.IsReady() && Hero.Distance(target.Position) <= Hero.AttackRange + 25)
                    {
                        W2.Cast();
                    }
                }
            }
        }
        public override void Flee()
        {
            EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
            if (isMelee)
            {
                if (Q.IsReady())
                {
                    AIHeroClient bestChampion = EntityManager.Heroes.Enemies.OrderBy(x => x.Distance(Game.CursorPos))
                    .Where(x => Q.IsInRange(x) && x.Distance(Game.CursorPos) + 200 < Hero.Distance(Game.CursorPos)).FirstOrDefault();
                    if (bestChampion != null)
                    {
                        Q.Cast(bestChampion);
                    }
                    else
                    {
                        Obj_AI_Minion bestMinion = EntityManager.MinionsAndMonsters.CombinedAttackable.OrderBy(x => x.Distance(Game.CursorPos))
                        .Where(x => Q.IsInRange(x) && x.Distance(Game.CursorPos) + 200 < Hero.Distance(Game.CursorPos)).FirstOrDefault();
                        if (bestMinion != null)
                        {
                            Q.Cast(bestMinion);
                        }
                    }
                }
            }
            else
            {
                if (E.IsReady())
                {
                    E2.Cast((Vector3)Hero.Position.Extend(Game.CursorPos, 70));
                }
            }
            if (R.IsReady()) R.Cast();
        }
        public void Insec()
        {
            AIHeroClient target = TargetSelector.GetTarget(Qcharge.Range, DamageType.Physical);
            if (IsValidTarget(target))
            {
                Vector3 insecPos = extend(target.Position, Game.CursorPos, 150, false);
                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, insecPos);

                if ((isMelee || ((!isMelee && R.IsReady() && R.Cast()))) && E.IsReady() && Hero.Distance(target.Position) <= Q.Range + 10)
                {
                    if (Hero.Distance(insecPos) < 110)
                    {
                        E.Cast(target);
                        return;
                    }
                    if (Hero.Distance(target.Position) + 10 < Hero.Distance(insecPos))
                    {
                        if (Hero.Distance(target.Position) <= Q.Range && Q.IsReady())
                        {
                            Q.Cast(target);
                        }
                        if (MiscMenu["FlashInsec"].Cast<CheckBox>().CurrentValue && Hero.Distance(insecPos) < 410 && Hero.Distance(insecPos) >= 150)
                        {
                            SpellDataInst spell = Hero.Spellbook.Spells.FirstOrDefault(a => a.Name.ToLower().Contains("summonerflash"));
                            if (spell != null && spell.IsReady)
                            {
                                Hero.Spellbook.CastSpell(spell.Slot, insecPos);
                            }
                        }
                    }
                }
            }
        }

        private void CastQE(AIHeroClient target)
        {
            PredictionResult QchargePred = ((Spell.Skillshot)Qcharge).GetPrediction(target);
            if (QchargePred.HitChance >= HitChance.High)
            {
                CastQE(QchargePred.CastPosition);
            }
            else if (QchargePred.HitChance == HitChance.Collision) // check for collision
            {
                Obj_AI_Base collision = QchargePred.CollisionObjects.OrderBy(x => x.Distance(Hero.Position)).First();
                if (extend(collision.Position, QchargePred.UnitPosition, collision.BoundingRadius, false).Distance(QchargePred.UnitPosition) < 180)
                {
                    CastQE(QchargePred.CastPosition);
                }
            }
        }

        private void CastQE(Vector3 position)
        {
            LastCast = Game.Time;
            gatePos = extend(Hero.Position, position, MiscMenu["GateDistance"].Cast<Slider>().CurrentValue, true); // in front, horizontal
            switch (MiscMenu["GateMode"].Cast<ComboBox>().CurrentValue)
            {
                case 0: // vertical fast
                    Qcharge.Cast(position);
                    gatePos = new Vector3(Hero.Position.Y + Hero.Position.X - gatePos.Y, Hero.Position.Y - Hero.Position.X + gatePos.X, Hero.Position.Z);
                    break;
                case 2: // horizontal slow
                    E2.Cast(gatePos);
                    break;
            }
            Qcharge.Cast(position);
        }

        private void ShouldE()
        {
            if (CD[5] == 0 && Game.Time - LastCast < 0.20)
                E2.Cast(gatePos);
        }

        private void KillSteal()
        {
            foreach (AIHeroClient target in EntityManager.Heroes.Enemies.OrderBy(x => x.Health).Where(x => x.IsValidTarget(Qcharge.Range) && x.Health < Hero.Level * 150 && IsValidTarget(x)))
            {
                if ((QcannonDMG(target) * 1.4f) > target.Health && CD[3] == 0 && CD[5] == 0 &&
                    ((Spell.Skillshot)Qcharge).GetPrediction(target).HitChance >= HitChance.High &&
                    (!isMelee || ((isMelee && R.IsReady() && R.Cast()))))
                {
                    CastQE(target);
                    return;
                }

                if ((QcannonDMG(target)) > target.Health && CD[3] == 0 && CD[5] != 0 &&
                    ((Spell.Skillshot)Q2).GetPrediction(target).HitChance >= HitChance.High &&
                    (!isMelee || ((isMelee && R.IsReady() && R.Cast()))))
                {
                    Q2.Cast(target);
                    return;
                }

                if ((EhammerDMG(target)) > target.Health && CD[2] == 0 && E.IsInRange(target) &&
                    (isMelee || ((!isMelee && R.IsReady() && R.Cast()))))
                {
                    E.Cast(target);
                    return;
                }
            }
        }

        private float QhammerDMG(Obj_AI_Base target)
        {
            return Hero.CalculateDamageOnUnit(target, DamageType.Physical, new float[] { 0, 40, 80, 120, 160, 200, 240 }[Q.Level] + 1.2f * Hero.FlatPhysicalDamageMod);
        }

        private float EhammerDMG(Obj_AI_Base target)
        {
            return Hero.CalculateDamageOnUnit(target, DamageType.Magical, new float[] { 0, 8f, 10.4f, 12.8f, 15.2f, 17.6f, 20f }[E.Level] * (target.MaxHealth / 100) + Hero.FlatPhysicalDamageMod);
        }

        private float QcannonDMG(Obj_AI_Base target)
        {
            return Hero.CalculateDamageOnUnit(target, DamageType.Physical, new float[] { 0, 70, 120, 170, 220, 270, 320 }[Q.Level] + 1.2f * Hero.FlatPhysicalDamageMod);
        }
    }
}