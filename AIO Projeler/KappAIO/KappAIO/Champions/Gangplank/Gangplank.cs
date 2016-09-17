using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using KappAIO.Common;
using SharpDX;
using static KappAIO.Champions.Gangplank.BarrelsManager;

namespace KappAIO.Champions.Gangplank
{
    internal class Gangplank : Base
    {
        internal static int ConnectionRange = 675;

        public static Spell.Targeted Q { get; }
        public static Spell.Active W { get; }
        public static Spell.Skillshot E { get; }
        public static Spell.Skillshot R { get; }

        private static float Rdamage(Obj_AI_Base target)
        {
            return user.HasBuff("GangplankRUpgrade2") ? user.GetSpellDamage(target, SpellSlot.R) * 3F : 0;
        }

        static Gangplank()
        {
            Init();

            Q = new Spell.Targeted(SpellSlot.Q, 625);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Skillshot(SpellSlot.E, 1000, SkillShotType.Circular, 500, int.MaxValue, 325);
            R = new Spell.Skillshot(SpellSlot.R, int.MaxValue, SkillShotType.Circular, 500, int.MaxValue, 600);

            MenuIni = MainMenu.AddMenu(MenuName, MenuName);
            AutoMenu = MenuIni.AddSubMenu("Otomatik");
            ComboMenu = MenuIni.AddSubMenu("Kombo");
            //HarassMenu = MenuIni.AddSubMenu("Dürtme");
            JungleClearMenu = MenuIni.AddSubMenu("OrmanTemizleme");
            LaneClearMenu = MenuIni.AddSubMenu("LaneTemizleme");
            KillStealMenu = MenuIni.AddSubMenu("KillÇal");
            DrawMenu = MenuIni.AddSubMenu("Göstergeler");
            SpellList.Add(Q);
            SpellList.Add(E);
            SpellList.Add(R);

            SpellList.ForEach(
                i =>
                {
                    ComboMenu.CreateCheckBox(i.Slot, "Kullan " + i.Slot);
                    if (i != R)
                    {
                        //HarassMenu.CreateCheckBox(i.Slot, "Kullan " + i.Slot);
                        //HarassMenu.AddSeparator(0);
                        LaneClearMenu.CreateCheckBox(i.Slot, "Kullan " + i.Slot);
                        LaneClearMenu.AddSeparator(0);
                        JungleClearMenu.CreateCheckBox(i.Slot, "Kullan " + i.Slot);
                        JungleClearMenu.AddSeparator(0);
                        DrawMenu.CreateCheckBox(i.Slot, "Göster " + i.Slot);
                        if (i != E)
                        {
                            //HarassMenu.CreateSlider(i.Slot + "mana", i.Slot + " Mana Yardımcısı {0}%", 60);
                            LaneClearMenu.CreateSlider(i.Slot + "mana", i.Slot + " Mana Yardımcısı {0}%", 60);
                            JungleClearMenu.CreateSlider(i.Slot + "mana", i.Slot + " Mana Yardımcısı {0}%", 60);
                        }
                    }
                    KillStealMenu.CreateCheckBox(i.Slot, i.Slot + " KillÇal");
                });

            MenuIni.AddGroupLabel("W ile Arındırma için Activator Kontrol Et > Qss");
            //AutoMenu.CreateCheckBox("CC", "Auto W CC Buffs");
            AutoMenu.CreateCheckBox("AutoQ", "Varillere Otomatik Q", false);
            AutoMenu.CreateCheckBox("Qunk", "Ölmeyecek Minyona Q");
            AutoMenu.CreateKeyBind("EQMOUSE", "E > Q Fareye Doğru", false, KeyBind.BindTypes.HoldActive, 'S');

            ComboMenu.CreateCheckBox("FB", "Önce Varil Yerleştir");
            ComboMenu.CreateSlider("RAOE", "R alanı için düşman sayısı {0}", 3, 1, 6);

            KillStealMenu.CreateCheckBox("RSwitch", "Hasar Yükseltmeyi Otomatik Kullan");
            KillStealMenu.CreateSlider("Rdmg", "R hasarını çoğalt X{0}", 3, 1, 12);

            LaneClearMenu.CreateCheckBox("QLH", "Q ile Son vuruş");
            LaneClearMenu.CreateSlider("EKill", "E için ölecek minyon Sayısı {0}", 2, 0, 10);
            LaneClearMenu.CreateSlider("EHits", "E kaç Minyona Çarpması gerekir {0}", 3, 0, 10);

            DrawMenu.CreateCheckBox("Barrels", "Varil Göstergeleri Aktif");

            Orbwalker.OnUnkillableMinion += Orbwalker_OnUnkillableMinion;
            Spellbook.OnCastSpell += Spellbook_OnCastSpell;
        }

        private static void Orbwalker_OnUnkillableMinion(Obj_AI_Base target, Orbwalker.UnkillableMinionArgs args)
        {
            if (target.IsKillable(Q.Range) && Q.IsReady() && Q.WillKill(target) && AutoMenu.CheckBoxValue("Qunk") && !(Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) || AutoMenu.KeyBindValue("EQMOUSE")))
            {
                Q.Cast(target);
            }
        }

        private static void Spellbook_OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            if (sender.Owner.IsMe && (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) || AutoMenu.KeyBindValue("EQMOUSE")))
            {
                if(BarrelsList.All(b => b?.Barrel.NetworkId != args.Target?.NetworkId)) return;
                
                var target =
                    EntityManager.Heroes.Enemies.FirstOrDefault(e => e.IsKillable() &&
                    BarrelsList.Any(b => b.Barrel.IsValidTarget(Q.Range) && (KillableBarrel(b)?.Distance(e) <= E.Width || BarrelsList.Any(a => KillableBarrel(b)?.Distance(a.Barrel) <= ConnectionRange && e.Distance(b.Barrel) <= E.Width))))
                    ?? TargetSelector.GetTarget(E.Range, DamageType.Physical);
                var position = Vector3.Zero;
                var startposition = Vector3.Zero;
                if (args.Slot == SpellSlot.Q && E.IsReady())
                {
                    var barrel = BarrelsList.FirstOrDefault(b => b.Barrel.NetworkId == args.Target.NetworkId);
                    var Secondbarrel = BarrelsList.FirstOrDefault(b => b.Barrel.NetworkId != barrel?.Barrel.NetworkId && b.Barrel.Distance(args.Target) <= ConnectionRange);
                    if (barrel != null)
                    {
                        startposition = Secondbarrel?.Barrel.ServerPosition ?? barrel.Barrel.ServerPosition;
                    }
                    if (startposition != Vector3.Zero)
                    {
                        if (target != null && target.IsKillable(E.Range + E.Radius))
                        {
                            if (target.Distance(startposition) <= ConnectionRange + E.Radius && target.Distance(startposition) > E.Width - 75)
                            {
                                var extended = startposition.Extend(E.GetPrediction(target).CastPosition, ConnectionRange).To3D();
                                position = !E.IsInRange(extended) ? E.GetPrediction(target).CastPosition : extended;
                            }
                        }
                        else
                        {
                            target = EntityManager.Heroes.Enemies.OrderBy(e => e.Distance(Game.CursorPos)).FirstOrDefault(e => e.IsKillable(E.Range));
                            if (target != null)
                            {
                                var extended = startposition.Extend(E.GetPrediction(target).CastPosition, ConnectionRange).To3D();
                                position = !E.IsInRange(extended) ? E.GetPrediction(target).CastPosition : extended;
                            }
                        }
                        if (position != Vector3.Zero)
                        {
                            if (BarrelsList.Count(b => b.Barrel.Distance(position) <= E.Width) < 1)
                            {
                                E.Cast(position);
                            }
                        }
                    }
                }
            }
        }

        public override void Active()
        {
            if (AutoMenu.KeyBindValue("EQMOUSE"))
            {
                var target = EntityManager.Heroes.Enemies.OrderBy(e => e.Distance(Game.CursorPos)).FirstOrDefault(e => e.IsKillable(E.Range));
                if (target != null)
                {
                    var targetedbarrel = BarrelsList.OrderBy(b => b.Barrel.Distance(user)).FirstOrDefault(b => KillableBarrel(b) != null && KillableBarrel(b).IsValidTarget(Q.Range));
                    if (targetedbarrel != null && Q.IsReady() && E.IsReady())
                    {
                        var castpos = target.IsInRange(targetedbarrel.Barrel, ConnectionRange)
                                          ? E.GetPrediction(target).CastPosition
                                          : targetedbarrel.Barrel.ServerPosition.Extend(E.GetPrediction(target).CastPosition, ConnectionRange).To3D();
                        if (castpos.IsInRange(target, E.Width) && E.IsInRange(castpos))
                        {
                            Q.Cast(targetedbarrel.Barrel);
                        }
                    }
                }
            }

            if (AutoMenu.CheckBoxValue("AutoQ") && Q.IsReady())
            {
                var target =
                    EntityManager.Heroes.Enemies.OrderByDescending(TargetSelector.GetPriority).FirstOrDefault(e => e.IsKillable() &&
                    BarrelsList.Any(b => b.Barrel.IsValidTarget(Q.Range) && (KillableBarrel(b)?.Distance(e) <= E.Width || BarrelsList.Any(a => KillableBarrel(b)?.Distance(a.Barrel) <= ConnectionRange && e.Distance(b.Barrel) <= E.Width))))
                    ?? TargetSelector.GetTarget(E.Range, DamageType.Physical);
                if(target == null) return;
                foreach (var A in BarrelsList.OrderBy(b => b.Barrel.Distance(target)))
                {
                    if (KillableBarrel(A) != null && KillableBarrel(A).IsValidTarget(Q.Range))
                    {
                        if (target.IsInRange(KillableBarrel(A), E.Width))
                        {
                            Q.Cast(KillableBarrel(A));
                        }

                        var Secondbarrel = BarrelsList.OrderBy(b => b.Barrel.Distance(target)).FirstOrDefault(b => b.Barrel.NetworkId != KillableBarrel(A).NetworkId && b.Barrel.Distance(KillableBarrel(A)) <= ConnectionRange);
                        if (Secondbarrel != null)
                        {
                            if (target.IsInRange(Secondbarrel.Barrel, E.Width))
                            {
                                Q.Cast(KillableBarrel(A));
                            }
                            if (BarrelsList.OrderBy(b => b.Barrel.Distance(target)).Any(b => b.Barrel.NetworkId != Secondbarrel.Barrel.NetworkId && b.Barrel.Distance(Secondbarrel.Barrel) <= ConnectionRange && b.Barrel.CountEnemiesInRange(E.Width) > 0))
                            {
                                Q.Cast(KillableBarrel(A));
                            }
                        }
                    }
                }
            }
        }

        public override void Combo()
        {
            Orbwalker.ForcedTarget = null;
            if (R.IsReady() && ComboMenu.CheckBoxValue(SpellSlot.R))
            {
                R.CastAOE(ComboMenu.SliderValue("RAOE"), 3000);
            }

            var target = 
                EntityManager.Heroes.Enemies.OrderByDescending(TargetSelector.GetPriority).FirstOrDefault(e => e.IsKillable() &&
                BarrelsList.Any(b => b.Barrel.IsValidTarget(Q.Range) && (KillableBarrel(b)?.Distance(e) <= E.Width || BarrelsList.Any(a => KillableBarrel(b)?.Distance(a.Barrel) <= ConnectionRange && e.Distance(b.Barrel) <= E.Width))))
                ?? TargetSelector.GetTarget(E.Range, DamageType.Physical);
            if(target == null || !target.IsKillable()) return;

            var pred = target.PrediectPosition((int)QTravelTime(target));
            var castpos = E.GetPrediction(target).CastPosition;

            if (AABarrel(target) != null)
            {
                var extended = AABarrel(target).ServerPosition.Extend(pred, ConnectionRange).To3D();
                castpos = !E.IsInRange(extended) ? pred : extended;
                Orbwalker.ForcedTarget = AABarrel(target);
                if (E.IsReady() && ComboMenu.CheckBoxValue(SpellSlot.E))
                {
                    if (BarrelsList.Count(b => b.Barrel.Distance(user) <= Q.Range) > 0 && BarrelsList.Count(b => b.Barrel.Distance(castpos) <= E.Width) < 1)
                    {
                        E.Cast(castpos);
                    }
                }
                Player.IssueOrder(GameObjectOrder.AttackUnit, AABarrel(target));
                return;
            }

            if (Q.IsReady())
            {
                if (ComboMenu.CheckBoxValue(SpellSlot.Q))
                {
                    if (((BarrelsList.Count(b => b.Barrel.IsInRange(target, E.Radius + ConnectionRange)) < 1 && (!E.IsReady() || E.Handle.Ammo < 1)) || Q.WillKill(target)) && target.IsKillable(Q.Range))
                    {
                        Q.Cast(target);
                    }

                    foreach (var A in BarrelsList.OrderBy(b => b.Barrel.Distance(target)))
                    {
                        if (KillableBarrel(A) != null && KillableBarrel(A).IsValidTarget(Q.Range))
                        {
                            if (pred.IsInRange(KillableBarrel(A), E.Width))
                            {
                                Q.Cast(KillableBarrel(A));
                            }

                            var Secondbarrel = BarrelsList.OrderBy(b => b.Barrel.Distance(target)).FirstOrDefault(b => b.Barrel.NetworkId != KillableBarrel(A).NetworkId && b.Barrel.Distance(KillableBarrel(A)) <= ConnectionRange);
                            if (Secondbarrel != null)
                            {
                                if (pred.IsInRange(Secondbarrel.Barrel, E.Width))
                                {
                                    Q.Cast(KillableBarrel(A));
                                }
                                if (BarrelsList.OrderBy(b => b.Barrel.Distance(target)).Any(b => b.Barrel.NetworkId != Secondbarrel.Barrel.NetworkId && b.Barrel.Distance(Secondbarrel.Barrel) <= ConnectionRange && b.Barrel.CountEnemiesInRange(E.Width) > 0))
                                {
                                    Q.Cast(KillableBarrel(A));
                                }
                            }
                            else
                            {
                                if (BarrelsList.OrderBy(b => b.Barrel.Distance(target)).Any(b => b.Barrel.NetworkId != KillableBarrel(A).NetworkId && b.Barrel.Distance(KillableBarrel(A)) <= ConnectionRange && b.Barrel.CountEnemiesInRange(E.Width) > 0))
                                {
                                    Q.Cast(KillableBarrel(A));
                                }
                            }
                        }
                    }
                }
                if (E.IsReady() && ComboMenu.CheckBoxValue(SpellSlot.E))
                {
                    if (BarrelsList.OrderBy(b => b.Barrel.Distance(target)).Count(b => b.Barrel.IsInRange(target, E.Width)) < 1)
                    {
                        if (BarrelsList.OrderBy(b => b.Barrel.Distance(target)).Count(b => b.Barrel.IsInRange(target, E.Radius + ConnectionRange)) > 0)
                        {
                            var targetbarrel = BarrelsList.OrderBy(b => b.Barrel.Distance(target)).FirstOrDefault(b => KillableBarrel(b) != null && (b.Barrel.IsValidTarget(Q.Range) || b.Barrel.IsValidTarget(user.GetAutoAttackRange())) && b.Barrel.IsInRange(target, E.Radius + ConnectionRange));
                            if (KillableBarrel(targetbarrel) != null)
                            {
                                var Secondbarrel = BarrelsList.OrderBy(b => b.Barrel.Distance(target)).FirstOrDefault(b => b.Barrel.NetworkId != targetbarrel?.Barrel.NetworkId && b.Barrel.Distance(targetbarrel?.Barrel) <= ConnectionRange);

                                if (Secondbarrel != null)
                                {
                                    var extended = Secondbarrel.Barrel.ServerPosition.Extend(pred, ConnectionRange).To3D();
                                    castpos = !E.IsInRange(extended) ? pred : extended;
                                }
                                if ((castpos.Distance(KillableBarrel(targetbarrel)) <= ConnectionRange || Secondbarrel?.Barrel.Distance(castpos) <= ConnectionRange) && E.IsInRange(castpos))
                                {
                                    E.Cast(castpos);
                                }
                            }
                        }
                        else
                        {
                            if (E.Handle.Ammo > 1 && ComboMenu.CheckBoxValue("FB"))
                            {
                                if (Q.IsInRange(castpos))
                                {
                                    if (HPTiming() <= 1000 || target.IsCC())
                                    {
                                        E.Cast(castpos);
                                    }
                                }
                                else
                                {
                                    if (E.IsInRange(castpos))
                                    {
                                        E.Cast(castpos.Extend(user, ConnectionRange - 300).To3D());
                                    }
                                }

                                var circle = new Geometry.Polygon.Circle(castpos, ConnectionRange);
                                foreach (var point in circle.Points)
                                {
                                    circle = new Geometry.Polygon.Circle(point, E.Width);
                                    var grass = circle.Points.OrderBy(p => p.Distance(castpos)).FirstOrDefault(p => p.IsGrass() && Q.IsInRange(p.To3D()) && p.Distance(castpos) <= ConnectionRange);
                                    if (grass != null)
                                    {
                                        E.Cast(grass.To3D());
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public override void Flee()
        {
        }

        public override void Harass()
        {
        }

        public override void LastHit()
        {
            if (LaneClearMenu.CheckBoxValue("QLH") && Q.IsReady())
            {
                var barrel = BarrelsList.OrderByDescending(b => b.Barrel.CountEnemyMinionsInRange(E.Width)).FirstOrDefault(m => KillableBarrel(m) != null && m.Barrel.CountEnemyMinionsInRange(E.Width) > 0 && (KillableBarrel(m).IsValidTarget(Q.Range) || KillableBarrel(m).IsInRange(user, user.GetAutoAttackRange())));
                if (barrel != null)
                {
                    var EkillMinions = EntityManager.MinionsAndMonsters.EnemyMinions.Count(m => BarrelKill(m) && BarrelsList.Any(b => b.Barrel.IsInRange(m, E.Width)) && m.IsValidTarget())
                                       >= LaneClearMenu.SliderValue("EKill");
                    if (KillableBarrel(barrel).IsValidTarget(user.GetAutoAttackRange()))
                    {
                        Orbwalker.ForcedTarget = KillableBarrel(barrel);
                    }
                    else
                    {
                        if (KillableBarrel(barrel).IsValidTarget(Q.Range) && EkillMinions)
                        {
                            Q.Cast(barrel.Barrel);
                        }
                    }
                }
                else
                {
                    if (LaneClearMenu.CompareSlider("Qmana", user.ManaPercent))
                    {
                        foreach (var minion in EntityManager.MinionsAndMonsters.EnemyMinions.OrderByDescending(m => m.Distance(user)).Where(m => m.IsKillable(Q.Range) && Q.WillKill(m) && !BarrelsList.Any(b => b.Barrel.Distance(m) <= E.Width)))
                        {
                            Q.Cast(minion);
                        }
                    }
                }
            }
        }

        public override void LaneClear()
        {
            Orbwalker.ForcedTarget = null;
            if (Q.IsReady())
            {
                if (E.IsReady() && LaneClearMenu.CheckBoxValue(SpellSlot.E))
                {
                    foreach (var minion in EntityManager.MinionsAndMonsters.EnemyMinions.OrderBy(m => m.Health).Where(m => m.IsKillable(E.Range)))
                    {
                        var pred = E.GetPrediction(minion);
                        if (EntityManager.MinionsAndMonsters.EnemyMinions.Count(e => e.Distance(pred.CastPosition) <= E.Width && BarrelKill(e)) >= LaneClearMenu.SliderValue("EKill"))
                        {
                            if (BarrelsList.Count(b => b.Barrel.IsInRange(pred.CastPosition, E.Width)) < 1
                                || (BarrelsList.Count(b => b.Barrel.IsInRange(pred.CastPosition, ConnectionRange)) > 0 && BarrelsList.Count(b => b.Barrel.IsInRange(pred.CastPosition, E.Width)) < 1))
                            {
                                E.Cast(pred.CastPosition);
                                return;
                            }
                        }
                    }
                }
                if (LaneClearMenu.CheckBoxValue(SpellSlot.Q))
                {
                    var barrel = BarrelsList.OrderByDescending(b => b.Barrel.CountEnemyMinionsInRange(E.Width)).FirstOrDefault(m => KillableBarrel(m) != null && m.Barrel.CountEnemyMinionsInRange(E.Width) > 0 && (KillableBarrel(m).IsValidTarget(Q.Range) || KillableBarrel(m).IsInRange(user, user.GetAutoAttackRange())));
                    if (barrel != null)
                    {
                        var EkillMinions = EntityManager.MinionsAndMonsters.EnemyMinions.Count(m => BarrelKill(m) && BarrelsList.Any(b => b.Barrel.IsInRange(m, E.Width)) && m.IsValidTarget())
                                           >= LaneClearMenu.SliderValue("EKill");
                        var EHitMinions = EntityManager.MinionsAndMonsters.EnemyMinions.Count(m => BarrelsList.Any(b => b.Barrel.IsInRange(m, E.Width)) && m.IsValidTarget())
                                           >= LaneClearMenu.SliderValue("EHits");
                        if (KillableBarrel(barrel).IsValidTarget(user.GetAutoAttackRange()))
                        {
                            Orbwalker.ForcedTarget = KillableBarrel(barrel);
                        }
                        else
                        {
                            if (KillableBarrel(barrel).IsValidTarget(Q.Range) && (EkillMinions || EHitMinions))
                            {
                                Q.Cast(barrel.Barrel);
                            }
                        }
                    }
                    else
                    {
                        if (LaneClearMenu.CompareSlider("Qmana", user.ManaPercent))
                        {
                            foreach (var minion in EntityManager.MinionsAndMonsters.EnemyMinions.OrderByDescending(m => m.Distance(user)).Where(m => m.IsKillable(Q.Range) && Q.WillKill(m) && !BarrelsList.Any(b => b.Barrel.Distance(m) <= E.Width)))
                            {
                                Q.Cast(minion);
                            }
                        }
                    }
                }
            }
        }

        public override void JungleClear()
        {
            foreach (var target in EntityManager.MinionsAndMonsters.GetJungleMonsters().OrderBy(m => m.MaxHealth).Where(m => m.IsKillable(Q.Range) && !m.IsMoving))
            {
                if (target != null)
                {
                    if (Q.IsReady() && JungleClearMenu.CheckBoxValue(SpellSlot.Q) && JungleClearMenu.CompareSlider("Qmana", user.ManaPercent))
                    {
                        var targetbarrel = BarrelsList.FirstOrDefault(b => KillableBarrel(b) != null && b.Barrel.IsInRange(target, E.Width));
                        Q.Cast(targetbarrel != null ? KillableBarrel(targetbarrel) : target);
                    }
                    if (E.IsReady() && JungleClearMenu.CheckBoxValue(SpellSlot.E) && BarrelsList.Count(b => b.Barrel.IsInRange(target, E.Width)) < 1)
                    {
                        E.Cast(target);
                    }
                }
            }
        }

        public override void KillSteal()
        {
            foreach (var enemy in EntityManager.Heroes.Enemies.Where(e => e.IsKillable()))
            {
                if (Q.IsReady() && Q.WillKill(enemy) && enemy.IsKillable(Q.Range) && KillStealMenu.CheckBoxValue(SpellSlot.Q))
                {
                    Q.Cast(enemy);
                }
                if (R.IsReady() && enemy.CountEnemiesInRange(1000) >= enemy.CountAlliesInRange(1000) && enemy.Distance(user) >= Q.Range + 1000 && KillStealMenu.CheckBoxValue(SpellSlot.R))
                {
                    if (KillStealMenu.CheckBoxValue("RSwitch") && Rdamage(enemy) > 0 ? Rdamage(enemy) >= enemy.TotalShieldHealth() : R.WillKill(enemy, KillStealMenu.SliderValue("Rdmg"), Rdamage(enemy)))
                    {
                        if (KillStealMenu.CheckBoxValue("RSwitch") && Rdamage(enemy) > 0)
                        {
                            Player.CastSpell(SpellSlot.R, enemy.PrediectPosition(R.CastDelay + Game.Ping));
                        }
                        else
                        {
                            R.CastAOE(1, R.Range);
                        }
                    }
                }
                if (KillStealMenu.CheckBoxValue(SpellSlot.E))
                {
                    foreach (var a in BarrelsList)
                    {
                        if (BarrelKill(enemy))
                        {
                            if (KillableBarrel(a) != null)
                            {
                                if (KillableBarrel(a)?.Distance(enemy) <= E.Width)
                                {
                                    Q.Cast(KillableBarrel(a));
                                }
                                if (BarrelsList.Any(b => b.Barrel.Distance(KillableBarrel(a)) <= ConnectionRange && enemy.Distance(b.Barrel) <= E.Width))
                                {
                                    Q.Cast(KillableBarrel(a));
                                }
                            }
                        }
                    }
                }
            }
        }

        public override void Draw()
        {
            if (AutoMenu.KeyBindValue("EQMOUSE") && BarrelsList.Count(b => b.Barrel.IsValidTarget(Q.Range)) < 1)
            {
                Drawing.DrawText(Game.CursorPos.WorldToScreen().X + 50, Game.CursorPos.WorldToScreen().Y, System.Drawing.Color.AliceBlue, "THERE ARE NO BARRELS NEARBY TRY TO CREATE ONE", 15);
            }

            if (DrawMenu.CheckBoxValue("Barrels"))
            {
                foreach (var A in BarrelsList)
                {
                    foreach (var B in BarrelsList.Where(b => b.Barrel.NetworkId != A.Barrel.NetworkId))
                    {
                        if (B.Barrel.Distance(A.Barrel) <= ConnectionRange)
                        {
                            Drawing.DrawLine(new Vector2(A.Barrel.ServerPosition.WorldToScreen().X, A.Barrel.ServerPosition.WorldToScreen().Y), new Vector2(B.Barrel.ServerPosition.WorldToScreen().X, B.Barrel.ServerPosition.WorldToScreen().Y), 2, System.Drawing.Color.Red);
                        }
                    }
                    if (KillableBarrel(A) != null)
                    {
                        Circle.Draw(EntityManager.Heroes.Enemies.Any(e => e.Distance(A.Barrel) <= E.Width && e.IsKillable()) ? Color.Red : Color.AliceBlue, 325, KillableBarrel(A));
                    }
                }
            }

            foreach (var spell in SpellList.Where(s => s != R && DrawMenu.CheckBoxValue(s.Slot)))
            {
                Circle.Draw(spell.IsReady() ? Color.Chartreuse : Color.OrangeRed, spell.Range, user);
            }
        }
    }
}
