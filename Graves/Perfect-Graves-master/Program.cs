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
using System.Drawing;


namespace PerfectGraves
{

    class Program
    {

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }
        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        public static Menu menu, ComboMenu, DrawingsMenu, FarmMenu, HarassMenu, UpdateMenu, KSMenu, MiscMenu;
        public static Spell.Skillshot Q;
        public static Spell.Skillshot W;
        public static Spell.Skillshot E;
        public static Spell.Skillshot R;
        public static Spell.Skillshot R1;
        private static Vector3 mousePos { get { return Game.CursorPos; } }
        public static float Etime { get; set; }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.ChampionName != "Graves")
            {
                return;
            }
            Bootstrap.Init(null);
            Q = new Spell.Skillshot(SpellSlot.Q, 950, SkillShotType.Linear, 250, 2000, 60);
            W = new Spell.Skillshot(SpellSlot.W, 850, SkillShotType.Circular, 250, 1650, 150);
            E = new Spell.Skillshot(SpellSlot.E, 425, SkillShotType.Linear);
            R = new Spell.Skillshot(SpellSlot.R, 1000, SkillShotType.Linear, 250, 2100, 100);
            R1 = new Spell.Skillshot(SpellSlot.R, 1400, SkillShotType.Cone, 250, 2100, 120);

            menu = MainMenu.AddMenu("Perfect Graves", "PerfectGraves");

            ComboMenu = menu.AddSubMenu("Combo Settings", "Combo");
            ComboMenu.Add("disableAA", new CheckBox("Devredışı AA"));
            ComboMenu.Add("useQCombo", new CheckBox("Kullan Q"));
            ComboMenu.Add("useWCombo", new CheckBox("Kullan W"));
            ComboMenu.Add("useECombo", new CheckBox("Kullan E"));
            ComboMenu.Add("useRCombo", new CheckBox("Hızlı R Kombosu"));
            ComboMenu.Add("useItems", new CheckBox("Kullan İtemler")); 
            ComboMenu.Add("useEreload", new CheckBox("E ile şarjör doldur"));
            ComboMenu.AddLabel("Mahvolmuş-pala ayarları");
            ComboMenu.Add("botrkHP", new Slider("Benim Canı < %", 60, 0, 100));
            ComboMenu.Add("botrkenemyHP", new Slider("Düşmanın Canı < %", 60, 0, 100));

            KSMenu = menu.AddSubMenu("KS Settings", "KSSettings");
            KSMenu.Add("useQKS", new CheckBox("KSde Q Kullan"));
            KSMenu.Add("useRKS", new CheckBox("KSde R Kullan"));

            HarassMenu = menu.AddSubMenu("Harass Settings", "Harass");
            HarassMenu.Add("useQHarass", new CheckBox("Kullan Q"));
            HarassMenu.Add("useItems", new CheckBox("İtemleri Kullan"));

            FarmMenu = menu.AddSubMenu("Lane/Jungle Clear Settings", "Farm");
            FarmMenu.AddLabel("Lane Temizleme");
            FarmMenu.Add("useQ", new CheckBox("Q Kullan ."));

            FarmMenu.AddLabel("Jungle Clear");
            FarmMenu.Add("Qjungle", new CheckBox("Use Q"));
            FarmMenu.Add("QjungleMana", new Slider("Mana < %", 45, 0, 100));
            FarmMenu.Add("Ejungle", new CheckBox("Use E"));
            FarmMenu.Add("EjungleMana", new Slider("Mana < %", 45, 0, 100));

            MiscMenu = menu.AddSubMenu("More Settings", "MoreSettings");
            MiscMenu.Add("gapcloserE", new CheckBox("Use E Gapcloser"));
            MiscMenu.Add("gapcloserW", new CheckBox("Use W Gapcloser"));

            DrawingsMenu = menu.AddSubMenu("Draw Settings", "Drawings");
            DrawingsMenu.Add("DrawQ", new CheckBox("Göster Q"));
            DrawingsMenu.Add("DrawW", new CheckBox("Göster W"));
            DrawingsMenu.Add("DrawE", new CheckBox("Göster E"));
            DrawingsMenu.Add("DrawR", new CheckBox("Göster R"));
            DrawingsMenu.Add("DrawR1", new CheckBox("Göster Gelişmiş R"));

            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        private static void Game_OnTick(EventArgs args)
        {

            Orbwalker.ForcedTarget = null;
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Combo();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                LaneClear();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                JungleClear();
            }
            KillSteal();

        }

        public static void OnPostAttack(AttackableUnit target, EventArgs args)
        {
            if (_Player.HasBuff("GravesBasicAttackAmmo2") || !E.IsReady() ||
                !ComboMenu["useEreload"].Cast<CheckBox>().CurrentValue ||
                (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) ||
                !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)))
                return;
            var direction = (Game.CursorPos - _Player.ServerPosition).To2D().Normalized();

            for (var step = 0f; step < 360; step += 30)
            {
                for (var a = 450; a > 0; a -= 50)
                {
                    var currentAngle = step * (float)Math.PI / 90;
                    var currentCheckPoint = _Player.ServerPosition.To2D() +
                                            a * direction.Rotated(currentAngle);

                    if (NavMesh.GetCollisionFlags(currentCheckPoint).HasFlag(CollisionFlags.Wall) ||
                        NavMesh.GetCollisionFlags(currentCheckPoint).HasFlag(CollisionFlags.Building))
                        continue;
                    {
                        E.Cast((Vector3)currentCheckPoint);
                    }
                }
            }
        }

        public static void OnGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (!sender.IsValidTarget() || sender.IsAlly) return;

            if (MiscMenu["gapcloserE"].Cast<CheckBox>().CurrentValue)
            {
                var direction = (e.End + _Player.ServerPosition).To2D().Normalized();

                for (var step = 0f; step < 360; step += 30)
                {
                    for (var a = 200; a < 450; a += 50)
                    {
                        var currentAngle = step * (float)Math.PI / 90;
                        var currentCheckPoint = _Player.ServerPosition.To2D() +
                                                a * direction.Rotated(currentAngle);

                        if (!IsSafePosition((Vector3)currentCheckPoint) ||
                            NavMesh.GetCollisionFlags(currentCheckPoint).HasFlag(CollisionFlags.Wall) ||
                            NavMesh.GetCollisionFlags(currentCheckPoint).HasFlag(CollisionFlags.Building))
                            continue;
                        {
                            E.Cast((Vector3)currentCheckPoint);
                        }
                    }
                }
            }

            if (MiscMenu["gapcloserW"].Cast<CheckBox>().CurrentValue &&
                W.IsReady() && _Player.Distance(e.End) <= W.Range)
                W.Cast(e.End);
        }
        public static bool UnderAllyTurret(Vector3 pos)
        {
            return ObjectManager.Get<Obj_AI_Turret>().Any(t => t.IsAlly && !t.IsDead && pos.Distance(t) <= 900);
        }

        public static bool UnderEnemyTurret(Vector3 pos)
        {
            return ObjectManager.Get<Obj_AI_Turret>().Any(t => t.IsEnemy && !t.IsDead && pos.Distance(t) <= 900);
        }

        public static bool InSpawnPoint(Vector3 pos)
        {
            return ObjectManager.Get<Obj_SpawnPoint>().Any(x => pos.Distance(x) < 800 && x.IsEnemy);
        }

        public static int CountAlliesInRange(Vector3 pos)
        {
            var allies = EntityManager.Heroes.Allies.Count(
                allied => !allied.IsDead && allied.Distance(pos) <= 800);
            return allies;
        }
        public static List<AIHeroClient> GetLowaiAiHeroClients(Vector3 position, float range)
        {
            return
                EntityManager.Heroes.Enemies.Where(
                    hero => hero.IsValidTarget() && (hero.Distance(position) <= range) && hero.HealthPercent <= 15)
                    .ToList();
        }

        public static void OnPreAttack(GameObject target, EventArgs args)
        {
            if (_Player.HasBuff("GravesBasicAttackAmmo2") || _Player.HasBuff("GravesBasicAttackAmmo1") ||
                !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) ||
                target.Type != GameObjectType.AIHeroClient || !ComboMenu["disableAA"].Cast<CheckBox>().CurrentValue)
                return;

            if (Prediction.Position.Collision.LinearMissileCollision(
                target as Obj_AI_Base, _Player.ServerPosition.To2D(), target.Position.To2D(), int.MaxValue,
                (int)target.BoundingRadius, 0))
            {
                Orbwalker.DisableAttacking = true;
            }
        }

        public static void OnSpellCast(GameObject sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsDead || !sender.IsMe) return;

            if (args.Slot == SpellSlot.Q)
                Orbwalker.ResetAutoAttack();

            if (args.Slot == SpellSlot.E)
                Orbwalker.ResetAutoAttack();

            if (args.Slot == SpellSlot.R)
                Orbwalker.ResetAutoAttack();
        }

        public static Vector3 WallQpos(AIHeroClient target)
        {
            if (target == null)
            {
                return new Vector3();
            }

            for (var i = 100; i < 900; i += 100)
            {
                var qPos = new Vector3(
                    Prediction.Position.PredictUnitPosition(target, 250).To3D().X + i,
                    Prediction.Position.PredictUnitPosition(target, 250).To3D().Y + i,
                    Prediction.Position.PredictUnitPosition(target, 250).To3D().Z);
                if (
                    NavMesh.GetCollisionFlags(qPos.To2D()
                        .RotateAroundPoint((Vector2)qPos, 20)).HasFlag(CollisionFlags.Building) ||
                    NavMesh.GetCollisionFlags(qPos.To2D()
                        .RotateAroundPoint((Vector2)qPos, 20)).HasFlag(CollisionFlags.Wall))
                    return qPos;
            }
            return new Vector3();
        }

        public static Vector3 DashtoQpos(AIHeroClient target)
        {
            if (target == null)
            {
                return new Vector3();
            }

            if (!WallQpos(target).IsValid()) return new Vector3();

            for (var p = 300; p < 900; p += 100)
            {
                var ePos = WallQpos(target).Extend(target.Position, target.Distance(WallQpos(target)) + p).To3D();
                if (ePos.Distance(_Player.ServerPosition) <= 300 &&
                    !(NavMesh.GetCollisionFlags(ePos).HasFlag(CollisionFlags.Building) ||
                      NavMesh.GetCollisionFlags(ePos).HasFlag(CollisionFlags.Wall)))
                {
                    return ePos;
                }
            }
            return new Vector3();
        }

        public static bool IsInGrass(AIHeroClient unit)
        {
            return unit.IsValidTarget(900) &&
                   (NavMesh.GetCollisionFlags(unit.ServerPosition).HasFlag(CollisionFlags.Grass) ||
                    NavMesh.GetCollisionFlags(Prediction.Position.PredictUnitPosition(unit, 500))
                        .HasFlag(CollisionFlags.Grass));
        }
        public static bool IsSafePosition(Vector3 position)
        {
            var enemies = position.CountEnemiesInRange(800);
            var allies = CountAlliesInRange(position);
            var turrets = EntityManager.Turrets.Allies.Count(x => _Player.Distance(x) < 800 && x.IsValid && !x.IsDead);
            var lowEnemies = GetLowaiAiHeroClients(position, 800).Count();

            if (UnderEnemyTurret(position)) return false;

            if (enemies == 1)
            {
                return true;
            }
            return allies + turrets > enemies - lowEnemies;
        }

        public static void CastCollisionQ(AIHeroClient target)
        {
            for (var i = 50; i < Q.Range - _Player.Distance(target.ServerPosition); i += 50)
            {
                var predPosQ = Prediction.Position.PredictLinearMissile(target, Q.Range - 100, Q.Width, Q.CastDelay,
                    Q.Speed, int.MaxValue, null);
                var predPosT = predPosQ.CastPosition
                    .Extend(_Player.ServerPosition.To2D(), -i).To3D();

                if (!NavMesh.GetCollisionFlags(predPosT).HasFlag(CollisionFlags.Wall) &&
                    !NavMesh.GetCollisionFlags(predPosT).HasFlag(CollisionFlags.Building) ||
                    _Player.Distance(predPosT) > Q.Range)
                    continue;

                Q.Cast((Vector3)_Player.ServerPosition.Extend(predPosT, Q.Range));
            }
        }

        public static void CastQkill(AIHeroClient target)
        {
            var predPos = Prediction.Position.PredictLinearMissile(target, Q.Range - 100, Q.Width, Q.CastDelay, Q.Speed,
                int.MaxValue, null);

            if (predPos.HitChance >= HitChance.Medium && Game.Time > Etime + 250 &&
                target.Health + 20 < _Player.GetSpellDamage(target, SpellSlot.Q))
            {
                Q.Cast(predPos.CastPosition);
            }
        }

        public static void CastQ(AIHeroClient target)
        {
            var predPos = Prediction.Position.PredictLinearMissile(target, Q.Range - 100, Q.Width, Q.CastDelay, Q.Speed,
                int.MaxValue, null);

            if (predPos.HitChance >= HitChance.Medium && Game.Time > Etime + 250)
            {
                Q.Cast(predPos.CastPosition);
            }
        }

        public static void KillSteal()
        {
            if (Orbwalker.IsAutoAttacking) return;
            var useR = KSMenu["useRKS"].Cast<CheckBox>().CurrentValue;
            var useQ = KSMenu["useQKS"].Cast<CheckBox>().CurrentValue;
            var targetQ = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            var targetR = TargetSelector.GetTarget(R.Range, DamageType.Physical);
            var targetR1 = TargetSelector.GetTarget(R1.Range, DamageType.Physical);
            if (useR && R.IsReady() && targetR.IsValidTarget(R.Range) && targetR.Health < RDamage(targetR) && R.GetPrediction(targetR).HitChance >= HitChance.Medium)
            {
                R.Cast(R.GetPrediction(targetR).CastPosition);
            }
            if (useR && R.IsReady() && targetR1.IsValidTarget(R1.Range) && targetR1.Health < R1Damage(targetR1) && R.GetPrediction(targetR1).HitChance >= HitChance.Medium)
            {
                R.Cast(R.GetPrediction(targetR1).CastPosition);
            }
            if (useQ && Q.IsReady() && targetQ.IsValidTarget(Q.Range) && targetQ.Health < QDamage(targetQ) && Q.GetPrediction(targetQ).HitChance >= HitChance.Medium)
            {
                CastQkill(targetQ);
            }
        }

        public static void CastWslow(AIHeroClient target)
        {
            var predPos = Prediction.Position.PredictCircularMissile(target, W.Range, W.Width, W.CastDelay, W.Speed,
                null, true);

            if (predPos.HitChance >= HitChance.Medium &&
                target.HealthPercent < 20)
            {
                Q.Cast(predPos.CastPosition);
            }
        }

        public static float QDamage(Obj_AI_Base target)
        {
            return _Player.CalculateDamageOnUnit(target, DamageType.Physical,
                (float)(new[] { 60, 80, 100, 120, 140 }[Program.Q.Level] + 0.75 * _Player.FlatPhysicalDamageMod));
        }
        public static float RDamage(Obj_AI_Base target)
        {
            return _Player.CalculateDamageOnUnit(target, DamageType.Physical,
                (float)(new[] { 250, 400, 550 }[Program.R.Level] + 1.4 * _Player.FlatPhysicalDamageMod));
        }
        public static float R1Damage(Obj_AI_Base target)
        {
            return _Player.CalculateDamageOnUnit(target, DamageType.Physical,
                (float)(new[] { 200, 320, 440 }[Program.R.Level] + 1.1 * _Player.FlatPhysicalDamageMod));
        }
        internal static void HandleItems()
        {
            var botrktarget = TargetSelector.GetTarget(550, DamageType.Physical);
            var target = TargetSelector.GetTarget(400, DamageType.Physical);
            var useItem = ComboMenu["useItems"].Cast<CheckBox>().CurrentValue;
            var useBotrkHP = ComboMenu["botrkHP"].Cast<Slider>().CurrentValue;
            var useBotrkEnemyHP = ComboMenu["botrkenemyHP"].Cast<Slider>().CurrentValue;
            //HYDRA
            if (useItem && Item.HasItem(3077) && Item.CanUseItem(3077))
                Item.UseItem(3077);

            //TİAMAT
            if (useItem && Item.HasItem(3074) && Item.CanUseItem(3074))
                Item.UseItem(3074);

            //NEW ITEM
            if (useItem && Item.HasItem(3748) && Item.CanUseItem(3748))
                Item.UseItem(3748);

            //BİLGEWATER CUTLASS
            if (useItem && Item.HasItem(3144) && Item.CanUseItem(3144) && botrktarget.HealthPercent <= useBotrkEnemyHP && _Player.HealthPercent <= useBotrkHP)
                Item.UseItem(3144, botrktarget);

            //BOTRK
            if (useItem && Item.HasItem(3153) && Item.CanUseItem(3153) && botrktarget.HealthPercent <= useBotrkEnemyHP && _Player.HealthPercent <= useBotrkHP)
                Item.UseItem(3153, botrktarget);

            //YOUMU
            if (useItem && Item.HasItem(3142) && Item.CanUseItem(3142))
                Item.UseItem(3142);

            //QSS
            if (useItem && Item.HasItem(3140) && Item.CanUseItem(3140) && (_Player.HasBuffOfType(BuffType.Charm) || _Player.HasBuffOfType(BuffType.Blind) || _Player.HasBuffOfType(BuffType.Fear) || _Player.HasBuffOfType(BuffType.Polymorph) || _Player.HasBuffOfType(BuffType.Silence) || _Player.HasBuffOfType(BuffType.Sleep) || _Player.HasBuffOfType(BuffType.Snare) || _Player.HasBuffOfType(BuffType.Stun) || _Player.HasBuffOfType(BuffType.Suppression) || _Player.HasBuffOfType(BuffType.Taunt)))
            {
                Item.UseItem(3140);
            }
        }
        public static void Harass()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            var UseItems = HarassMenu["useItems"].Cast<CheckBox>().CurrentValue;
            var useQ = HarassMenu["useQHarass"].Cast<CheckBox>().CurrentValue;
            if (useQ && Q.IsReady() && Q.GetPrediction(target).HitChance >= HitChance.Medium)
            {
                Q.Cast(Q.GetPrediction(target).CastPosition);
            }
        }
        public static void Combo()
        {
            var UseItems = ComboMenu["useItems"].Cast<CheckBox>().CurrentValue;
            var useQ = ComboMenu["useQCombo"].Cast<CheckBox>().CurrentValue;
            var useW = ComboMenu["useWCombo"].Cast<CheckBox>().CurrentValue;
            var useE = ComboMenu["useECombo"].Cast<CheckBox>().CurrentValue;
            var useR = ComboMenu["useRCombo"].Cast<CheckBox>().CurrentValue;
            var targetE = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            var targetR = TargetSelector.GetTarget(R.Range, DamageType.Physical);
            var targetR1 = TargetSelector.GetTarget(R1.Range, DamageType.Physical);
            var targetQ = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            var tHp = targetQ.Health + 20;
            if (!Player.HasBuff("gravesbasicattackammo2") && !Orbwalker.IsAutoAttacking)
            {
                if (useE && E.IsReady() && targetE.IsValidTarget(Q.Range))
                {
                    E.Cast(mousePos);
                }
            }
            if (useR && R.IsReady() && targetR.IsValidTarget(R.Range) && targetR.Health < RDamage(targetR) && R.GetPrediction(targetR).HitChance >= HitChance.Medium)
            {
                R.Cast(R.GetPrediction(targetR).CastPosition);
            }
            CastQ(targetQ);
            CastCollisionQ(targetQ);
            
            foreach (var target in EntityManager.Heroes.Enemies.Where(o => o.IsValidTarget(1300) && !o.IsDead && !o.IsZombie))
            {
                if (useW && W.IsReady() && W.GetPrediction(target).HitChance >= HitChance.Medium && target.IsValidTarget(W.Range))
                {
                    W.Cast(target);
                }
                if (UseItems)
                {
                    HandleItems();
                }
            }
        }
        public static void LaneClear()
        {

        }

        private static void JungleClear()
        {
            if (Orbwalker.IsAutoAttacking) return;
            var useQ = FarmMenu["Qjungle"].Cast<CheckBox>().CurrentValue;
            var useQMana = FarmMenu["QjungleMana"].Cast<Slider>().CurrentValue;
            var useE = FarmMenu["Ejungle"].Cast<CheckBox>().CurrentValue;
            var useEMana = FarmMenu["EjungleMana"].Cast<Slider>().CurrentValue;
            foreach (var monster in EntityManager.MinionsAndMonsters.Monsters)
            {
                if (useQ && Q.IsReady() && Player.Instance.ManaPercent > useQMana)
                {
                    Q.Cast(monster);
                }
                if (useE && E.IsReady() && Player.Instance.HealthPercent > useEMana)
                {
                    E.Cast(mousePos);
                }

                HandleItems();
            }
        }


        private static void Drawing_OnDraw(EventArgs args)
        {
            if (DrawingsMenu["DrawQ"].Cast<CheckBox>().CurrentValue)
            {
                Drawing.DrawCircle(_Player.Position, Q.Range, System.Drawing.Color.Red);
            }

            if (DrawingsMenu["DrawW"].Cast<CheckBox>().CurrentValue)
            {
                Drawing.DrawCircle(_Player.Position, W.Range, System.Drawing.Color.Red);
            }

            if (DrawingsMenu["DrawE"].Cast<CheckBox>().CurrentValue)
            {
                Drawing.DrawCircle(_Player.Position, E.Range, System.Drawing.Color.Red);
            }

            if (DrawingsMenu["DrawR"].Cast<CheckBox>().CurrentValue)
            {
                Drawing.DrawCircle(_Player.Position, R.Range, System.Drawing.Color.Red);
            }

            if (DrawingsMenu["DrawR1"].Cast<CheckBox>().CurrentValue)
            {
                Drawing.DrawCircle(_Player.Position, R1.Range, System.Drawing.Color.Red);
            }

        }
    }
}

