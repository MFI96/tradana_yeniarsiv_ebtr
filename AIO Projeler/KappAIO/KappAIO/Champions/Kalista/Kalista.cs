using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using KappAIO.Common;
using SharpDX;
using static KappAIO.Champions.Kalista.Rend;
using static KappAIO.Common.Extentions;

namespace KappAIO.Champions.Kalista
{
    internal class Kalista : Base
    {
        private static readonly string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EloBuddy\\KappAIO\\temp\\";
        private static readonly bool Created;
        private static float LastE;
        private static AIHeroClient BoundHero;

        private static readonly List<BallistaHeros> Ballistaheros = new List<BallistaHeros>
        {
            new BallistaHeros(Champion.Blitzcrank, "rocketgrab2"),
            new BallistaHeros(Champion.TahmKench, "tahmkenchdevoured"),
            new BallistaHeros(Champion.Skarner, "SkarnerImpale")
        };

        private class BallistaHeros
        {
            public Champion Hero;
            public string BuffName;
            public BallistaHeros(Champion hero, string buffname)
            {
                this.Hero = hero;
                this.BuffName = buffname;
            }
        }

        public static Spell.Skillshot Q { get; }
        public static Spell.Skillshot W { get; }
        public static Spell.Active E { get; }
        public static Spell.Active R { get; }

        static Kalista()
        {
            if (!Directory.Exists(appdata))
            {
                Directory.CreateDirectory(appdata);
            }

            if (!File.Exists(appdata + Game.GameId + ".dat"))
            {
                File.Create(appdata + Game.GameId + ".dat");
                Created = true;
            }

            Edmg = new Text(string.Empty, new Font("Tahoma", 9, FontStyle.Bold)) { Color = System.Drawing.Color.White };
            Q = new Spell.Skillshot(SpellSlot.Q, 1150, SkillShotType.Linear, 250, 2100, 60) { AllowedCollisionCount = int.MaxValue };
            W = new Spell.Skillshot(SpellSlot.W, 5000, SkillShotType.Circular, 250, 2100, 80);
            E = new Spell.Active(SpellSlot.E, 1000);
            R = new Spell.Active(SpellSlot.R, 1100);
            SpellList.Add(Q);
            SpellList.Add(E);

            MenuIni = MainMenu.AddMenu(MenuName, MenuName);
            AutoMenu = MenuIni.AddSubMenu("Otomatik");
            ComboMenu = MenuIni.AddSubMenu("Kombo");
            HarassMenu = MenuIni.AddSubMenu("Dürtme");
            JungleClearMenu = MenuIni.AddSubMenu("OrmanTemizleme");
            LaneClearMenu = MenuIni.AddSubMenu("LaneTemizleme");
            KillStealMenu = MenuIni.AddSubMenu("KillÇal");
            DrawMenu = MenuIni.AddSubMenu("Göstergeler");

            SpellList.ForEach(
                i =>
                {
                    ComboMenu.CreateCheckBox(i.Slot, "Kullan " + i.Slot);
                    HarassMenu.CreateCheckBox(i.Slot, "Kullan " + i.Slot);
                    HarassMenu.CreateSlider(i.Slot + "mana", i.Slot + " Mana Yardımcısı {0}%", 60);
                    HarassMenu.AddSeparator(0);
                    LaneClearMenu.CreateCheckBox(i.Slot, "Kullan " + i.Slot);
                    LaneClearMenu.CreateSlider(i.Slot + "mana", i.Slot + " Mana Yardımcısı {0}%", 60);
                    LaneClearMenu.AddSeparator(0);
                    JungleClearMenu.CreateCheckBox(i.Slot, "Kullan " + i.Slot);
                    JungleClearMenu.CreateSlider(i.Slot + "mana", i.Slot + " Mana Yardımcısı {0}%", 60);
                    JungleClearMenu.AddSeparator(0);
                    KillStealMenu.CreateCheckBox(i.Slot, i.Slot + " KillÇal");
                    DrawMenu.CreateCheckBox(i.Slot, "Göster " + i.Slot);
                });

            //AutoMenu.CreateCheckBox("exploit", "Kalista Exploit Kullan (Ban Yiyebilirsin)", false);
            AutoMenu.CreateCheckBox("SoulBound", "Bağlı Olduğuna R sakla");
            AutoMenu.CreateCheckBox("AutoR", "Otomatik R");
            AutoMenu.CreateCheckBox("EDeath", "Ölmeden Önce E");
            AutoMenu.CreateCheckBox("AutoEJungle", "Orman Moblarını Otomatik çal (E)");
            AutoMenu.CreateCheckBox("AutoEBig", "Büyük Minyonlar için Otomatik E");
            AutoMenu.CreateCheckBox("AutoEUnKillable", "Ölmeyecek minyonlar için otomatik E", false);
            AutoMenu.CreateCheckBox("AutoE", "Hiçbir Mod Aktif Değilken Eyi yinede Kullan", false);
            AutoMenu.CreateSlider("AutoEcount", "{0} Otomatik R için yük say", 5, 1, 25);

            var balistahero = Ballistaheros.FirstOrDefault(a => EntityManager.Heroes.Allies.Any(b => a.Hero == b.Hero));

            if (balistahero != null)
            {
                AutoMenu.CreateCheckBox(balistahero.Hero.ToString(), "Ballista Kullan şununla " + balistahero.Hero);
                AutoMenu.CreateSlider(balistahero.Hero + "dis", "Ballista için arandaki mesafe", 600, 0, 1100);
            }

            ComboMenu.CreateCheckBox("Gapclose", "Minyona vura vura Hareket et");
            ComboMenu.CreateSlider("EKillCount", "E ile öldür {0}+ Sadece Düşman için", 1, 1, 6);

            HarassMenu.CreateCheckBox("Emin", "E ile minyon öldürerek Hedefi Dürt");
            HarassMenu.CreateSlider("Estacks", "{0} E için en az yük", 5, 1, 25);

            LaneClearMenu.CreateKeyBind("Etog", "Lanetemizlemede E Kullan/Kullanma", false, KeyBind.BindTypes.PressToggle);
            LaneClearMenu.CreateSlider("Qhits", "Q kaç Minyona Çarpsın {0}", 3, 1, 15);
            LaneClearMenu.CreateSlider("Ekills", "E ile lanetemizleme için en az minyon {0}", 2, 1, 10);

            JungleClearMenu.CreateCheckBox("Esmall", "Küçük moblara E");

            KillStealMenu.CreateCheckBox("ETransfer", "Kill Çalma için Q ya öncelik ver (Q > E)");
            DrawMenu.CreateCheckBox("EDMG", "E Hasarını Göster");
            
            Orbwalker.OnUnkillableMinion += Orbwalker_OnUnkillableMinion;
            Spellbook.OnCastSpell += Spellbook_OnCastSpell;
            Events.OnIncomingDamage += Events_OnIncomingDamage;
            Gapcloser.OnGapcloser += Gapcloser_OnGapcloser;
            Obj_AI_Base.OnBuffGain += Obj_AI_Base_OnBuffGain;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
            Events.OnGameEnd += Events_OnGameEnd;
        }

        private static void Events_OnGameEnd(EventArgs args)
        {
            if (File.Exists(appdata + Game.GameId + ".dat"))
            {
                File.Delete(appdata + Game.GameId + ".dat");
            }
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender != null && sender.IsMe && args.Target != null && args.Target.IsAlly && args.SData.Name.Equals("KalistaPSpellCast", StringComparison.CurrentCultureIgnoreCase))
            {
                File.WriteAllText(appdata + Game.GameId + ".dat", args.Target.NetworkId.ToString());
            }
        }

        private static void Obj_AI_Base_OnBuffGain(Obj_AI_Base sender, Obj_AI_BaseBuffGainEventArgs args)
        {
            var caster = sender as AIHeroClient;
            if (caster != null && R.IsReady() && caster.IsEnemy && BoundHero != null)
            {
                if (Ballistaheros.Any(b => b?.Hero == BoundHero?.Hero && AutoMenu.CheckBoxValue(b.Hero.ToString()) && args.Buff.Name == b.BuffName && AutoMenu.SliderValue(b.Hero + "dis") >= user.Distance(BoundHero)))
                {
                    R.Cast();
                }
            }
        }

        private static void Gapcloser_OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if(sender == null || !sender.IsEnemy || e.End.Distance(user) > 1000 || !R.IsReady()) return;

            if(user.HealthPercent <= 20 || user.CountEnemiesInRange(1000) > user.CountAlliesInRange(1000))
                R.Cast();
        }

        private static void Events_OnIncomingDamage(Events.InComingDamageEventArgs args)
        {
            if (AutoMenu.CheckBoxValue("EDeath") && args.Target.IsMe && args.InComingDamage >= user.TotalShieldHealth())
            {
                E.Cast();
            }

            if (args.Target?.NetworkId == BoundHero?.NetworkId && args.InComingDamage >= args.Target.TotalShieldHealth() && AutoMenu.CheckBoxValue("SoulBound") && R.IsReady())
            {
                R.Cast();
            }
        }

        private static void Spellbook_OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            if (sender.Owner.IsMe && args.Slot == SpellSlot.E)
            {
                if (Core.GameTickCount - LastE <= Game.Ping + 50)
                {
                    args.Process = false;
                }
                else
                {
                    LastE = Core.GameTickCount;
                }
            }
        }

        private static void Orbwalker_OnUnkillableMinion(Obj_AI_Base target, Orbwalker.UnkillableMinionArgs args)
        {
            if (target != null && target.IsKillable(E.Range) && E.IsReady() && target.EKill() && AutoMenu.CheckBoxValue("AutoEUnKillable"))
            {
                E.Cast();
            }
        }

        public override void Active()
        {
            if (BoundHero == null && Created)
            {
                if (File.Exists(appdata + Game.GameId + ".dat"))
                {
                    var read = File.ReadAllLines(appdata + Game.GameId + ".dat");   
                    BoundHero = EntityManager.Heroes.Allies.FirstOrDefault(a => read.Contains(a.NetworkId.ToString()));
                }
            }

            if(!E.IsReady()) return;
            if (AutoMenu.CheckBoxValue("AutoEJungle"))
            {
                foreach (var mob in SupportedJungleMobs.Where(m => m != null && m.IsKillable(E.Range) && m.EKill()))
                {
                    if(mob != null)
                        E.Cast();
                    return;
                }
            }
            if (AutoMenu.CheckBoxValue("AutoEBig"))
            {
                foreach (var mob in EntityManager.MinionsAndMonsters.EnemyMinions.Where(m => m != null && m.IsBigMinion() && m.IsKillable(E.Range) && m.EKill()))
                {
                    if (mob != null)
                        E.Cast();
                    return;
                }
            }
            if (AutoMenu.CheckBoxValue("AutoE") && NoModesActive)
            {
                foreach (var enemy in EntityManager.Heroes.Enemies.Where(e => e != null && e.IsKillable(E.Range) && AutoMenu.CompareSlider("AutoEcount", e.RendCount())))
                {
                    if (enemy != null)
                        E.Cast();
                    return;
                }
            }
            if (LaneClearMenu.KeyBindValue("Etog"))
            {
                if (EntityManager.MinionsAndMonsters.EnemyMinions.Count(m => m.IsKillable(E.Range) && m.EKill()) >= LaneClearMenu.SliderValue("Ekills"))
                {
                    E.Cast();
                }
            }
        }

        public override void Combo()
        {
            if (ComboMenu.CheckBoxValue("Gapclose"))
            {
                Gapclose();
            }
            if (ComboMenu.CompareSlider("EKillCount", EntityManager.Heroes.Enemies.Count(e => e.IsKillable(E.Range) && ComboMenu.CheckBoxValue(E.Slot) && e.EKill())) && E.IsReady())
            {
                E.Cast();
            }

            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            if(target == null || !target.IsKillable(Q.Range)) return;

            if (ComboMenu.CheckBoxValue(Q.Slot) && Q.IsReady())
            {
                QCast(target);
            }
        }

        public override void Flee()
        {
        }

        public override void Harass()
        {
            if(EntityManager.Heroes.Enemies.Any(e => e.RendCount() >= HarassMenu.SliderValue("Estacks") && e.IsKillable(E.Range)) && E.IsReady() && HarassMenu.CheckBoxValue(SpellSlot.E) && HarassMenu.CompareSlider("Emana", user.ManaPercent))
            {
                if (HarassMenu.CheckBoxValue("Emin"))
                {
                    if (EntityManager.MinionsAndMonsters.EnemyMinions.Any(e => e.EKill() && e.IsKillable(E.Range)) || EntityManager.MinionsAndMonsters.GetJungleMonsters().Any(e => e.EKill() && e.IsKillable(E.Range)))
                    {
                        E.Cast();
                    }
                }
                else
                {
                    E.Cast();
                }
            }

            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            if (target == null || !target.IsKillable(Q.Range)) return;

            if (HarassMenu.CheckBoxValue(Q.Slot) && Q.IsReady() && HarassMenu.CompareSlider("Qmana", user.ManaPercent))
            {
                QCast(target);
            }
        }

        public override void LastHit()
        {
        }

        public override void LaneClear()
        {
            if (E.IsReady() && LaneClearMenu.CheckBoxValue(SpellSlot.E) && LaneClearMenu.CompareSlider("Emana", user.ManaPercent) && LaneClearMenu.CompareSlider("Ekills", EntityManager.MinionsAndMonsters.EnemyMinions.Count(e => e.IsKillable(E.Range) && e.EKill())))
            {
                E.Cast();
            }

            if (Q.IsReady() && LaneClearMenu.CheckBoxValue(SpellSlot.Q) && LaneClearMenu.CompareSlider("Qmana", user.ManaPercent))
            {
                foreach (var mob in EntityManager.MinionsAndMonsters.EnemyMinions.Where(m => m.IsKillable(Q.Range)))
                {
                    QCast(mob, false, LaneClearMenu.SliderValue("Qhits"));
                }
            }
        }

        public override void JungleClear()
        {
            if (E.IsReady() && JungleClearMenu.CheckBoxValue(SpellSlot.E) && JungleClearMenu.CompareSlider("Emana", user.ManaPercent))
            {
                if (JungleClearMenu.CheckBoxValue("Esmall"))
                {
                    foreach (var mob in EntityManager.MinionsAndMonsters.GetJungleMonsters().Where(m => m != null && m.IsKillable(E.Range) && m.EKill()))
                    {
                        if (mob != null)
                            E.Cast();
                        return;
                    }
                }
                else
                {
                    foreach (var mob in SupportedJungleMobs.Where(m => m != null && m.IsKillable(E.Range) && m.EKill()))
                    {
                        if (mob != null)
                            E.Cast();
                        return;
                    }
                }
            }

            if (Q.IsReady() && JungleClearMenu.CheckBoxValue(SpellSlot.Q) && JungleClearMenu.CompareSlider("Qmana", user.ManaPercent))
            {
                foreach (var mob in SupportedJungleMobs.Where(m => m != null && m.IsKillable(Q.Range)))
                {
                    if (mob != null)
                        Q.Cast(mob);
                    return;
                }
            }
        }

        public override void KillSteal()
        {
            foreach (var enemy in EntityManager.Heroes.Enemies.Where(e => e != null && e.IsKillable()))
            {
                if (Q.IsReady() && E.IsReady())
                {
                    QCast(enemy, KillStealMenu.CheckBoxValue("ETransfer"));
                }

                if (KillStealMenu.CheckBoxValue(E.Slot) && E.IsReady() && enemy.IsKillable(E.Range) && enemy.EKill())
                {
                    E.Cast();
                    return;
                }
                
                if (KillStealMenu.CheckBoxValue(Q.Slot) && Q.IsReady() && enemy.IsKillable(Q.Range) && Q.WillKill(enemy))
                {
                    QCast(enemy);
                    return;
                }
            }
        }

        private static void Gapclose()
        {
            Orbwalker.ForcedTarget = user.CountEnemiesInRange(user.GetAutoAttackRange()) < 1 ?
                EntityManager.MinionsAndMonsters.CombinedAttackable.OrderBy(m => m.Distance(Game.CursorPos)).FirstOrDefault(m => !m.IsDead && m.IsEnemy && m.Health > 0 && m.IsKillable(user.GetAutoAttackRange())) : null;
        }

        public static Text Edmg;

        public override void Draw()
        {
            foreach (var obj in ObjectManager.Get<Obj_AI_Base>().Where(o => o.IsValidTarget() && o.RendCount() > 0 && DrawMenu.CheckBoxValue("EDMG")))
            {
                float x = obj.HPBarPosition.X;
                float y = obj.HPBarPosition.Y;
                Edmg.Color = System.Drawing.Color.White;
                if (obj is Obj_AI_Minion)
                {
                    x = obj.HPBarPosition.X + 110;
                    y = obj.HPBarPosition.Y - 20;
                }
                if (obj.EKill())
                {
                    Edmg.Color = System.Drawing.Color.Red;
                }
                Edmg.TextValue = (int)obj.EDamage(obj.RendCount()) + " / " + (int)obj.TotalShieldHealth();
                Edmg.Position = new Vector2(x, y);
                Edmg.Draw();
            }

            foreach (var spell in SpellList.Where(s => DrawMenu.CheckBoxValue(s.Slot)))
            {
                Circle.Draw(spell.IsReady() ? SharpDX.Color.Chartreuse : SharpDX.Color.OrangeRed, spell.Range, user);
            }
        }

        private static void QCast(Obj_AI_Base target, bool transfer = false, int HitCount = -1)
        {
            /*
            var pred = Prediction.Position.GetPrediction(
                new Prediction.Manager.PredictionInput
                    {
                        Range = Q.Range, Delay = Q.CastDelay, Radius = Q.Radius, Target = target, Type = SkillShotType.Linear, From = user.ServerPosition, Speed = Q.Speed,
                        CollisionTypes = new HashSet<CollisionType> { CollisionType.AiHeroClient, CollisionType.ObjAiMinion, CollisionType.YasuoWall }, RangeCheckFrom = user.ServerPosition
                    });*/
            var collidelist = new List<Obj_AI_Base>();
            collidelist.Clear();
            var pred = Q.GetPrediction(target);
            var CastPos = pred.CastPosition;
            var rect = new Geometry.Polygon.Rectangle(user.ServerPosition, CastPos, Q.Width);

            if(pred.HitChance < HitChance.Medium) return;

            collidelist.AddRange(EntityManager.MinionsAndMonsters.EnemyMinions.Where(m => new Geometry.Polygon.Circle(m.ServerPosition, m.BoundingRadius).Points.Any(p => rect.IsInside(p)) && !m.IsDead && m.IsValidTarget()));
            if (HitCount == -1)
            {
                collidelist.AddRange(EntityManager.MinionsAndMonsters.GetJungleMonsters().Where(m => new Geometry.Polygon.Circle(m.ServerPosition, m.BoundingRadius).Points.Any(p => rect.IsInside(p)) && !m.IsDead && m.IsValidTarget()));
                collidelist.AddRange(EntityManager.Heroes.Enemies.Where(m => new Geometry.Polygon.Circle(m.ServerPosition, m.BoundingRadius).Points.Any(p => rect.IsInside(p)) && m.NetworkId != target.NetworkId && !m.IsDead && m.IsValidTarget()));
                //Chat.Print(collidelist.Count(o => Q.WillKill(o)) - collidelist.Count);
                if (collidelist.Count(o => Q.WillKill(o)) - collidelist.Count == 0)
                {
                    if (transfer)
                    {
                        if (collidelist.Any(o => EKill(o, target)))
                        {
                            Q.Cast(CastPos);
                        }
                    }
                    else
                    {
                        Q.Cast(CastPos);
                    }
                }
            }
            else
            {
                if (collidelist.Count(o => Q.WillKill(o)) >= HitCount)
                {
                    Q.Cast(CastPos);
                }
            }
        }
    }
}
