﻿using System;
using System.Linq;

using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Rendering;

using SharpDX;

namespace Olaf
{
    internal class OlafAxe
    {
        public GameObject Object { get; set; }

        public float NetworkId { get; set; }

        public Vector3 AxePos { get; set; }

        public double ExpireTime { get; set; }
    }

    internal class Program
    {
        public static readonly Item Cutlass = new Item((int)ItemId.Bilgewater_Cutlass, 550);

        public static readonly Item Botrk = new Item((int)ItemId.Blade_of_the_Ruined_King, 550);

        public static readonly Item Youmuu = new Item((int)ItemId.Youmuus_Ghostblade);

        public const string ChampName = "Olaf";

        public const string Menuname = "Olaf";

        private static readonly OlafAxe olafAxe = new OlafAxe();

        public static int LastTickTime;

        private static GameObject _axeObj;

        public static Spell.Skillshot Q { get; private set; }

        public static Spell.Skillshot Q2 { get; private set; }

        public static Spell.Active W { get; private set; }

        public static Spell.Targeted E { get; private set; }

        public static Spell.Active R { get; private set; }

        private static readonly AIHeroClient player = ObjectManager.Player;

        public static Menu UltMenu { get; private set; }

        public static Menu ComboMenu { get; private set; }

        public static Menu HarassMenu { get; private set; }

        public static Menu LaneMenu { get; private set; }

        public static Menu KillStealMenu { get; private set; }

        public static Menu MiscMenu { get; private set; }

        public static Menu ItemsMenu { get; private set; }

        public static Menu DrawMenu { get; private set; }

        private static Menu menuIni;

        public static void Execute()
        {
            if (player.ChampionName != ChampName)
            {
                return;
            }

            //Ability Information - Range - Variables.
            Q = new Spell.Skillshot(SpellSlot.Q, 1000, SkillShotType.Linear, 250, 1550, 75)
                    { AllowedCollisionCount = int.MaxValue, MinimumHitChance = HitChance.High };
            Q2 = new Spell.Skillshot(SpellSlot.Q, 900, SkillShotType.Linear, 250, 1550, 75)
                     { AllowedCollisionCount = int.MaxValue, MinimumHitChance = HitChance.High };
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Targeted(SpellSlot.E, 325);
            R = new Spell.Active(SpellSlot.R);

            menuIni = MainMenu.AddMenu("Olaf", "Olaf");
            menuIni.AddGroupLabel("Worstun olaf addonuna hoşgeldin");
            menuIni.AddGroupLabel("Genel Ayarlar");
            menuIni.Add("Ult", new CheckBox("Kullan Ulti?"));
            menuIni.Add("Items", new CheckBox("Kullan İtmeler?"));
            menuIni.Add("Combo", new CheckBox("Kullan Kombo?"));
            menuIni.Add("Harass", new CheckBox("Kullan Dürtme?"));
            menuIni.Add("LaneClear", new CheckBox("Kullan LaneTemizleme?"));
            menuIni.Add("LastHit", new CheckBox("Kullan SonVuruş?"));
            menuIni.Add("JungleClear", new CheckBox("Kullan OrmanTemizleme?"));
            menuIni.Add("KillSteal", new CheckBox("Kullan Kill çalma?"));
            menuIni.Add("Misc", new CheckBox("Kullan Ek?"));
            menuIni.Add("Drawings", new CheckBox("Kullan Göstergeler?"));

            ItemsMenu = menuIni.AddSubMenu("Items");
            ItemsMenu.AddGroupLabel("Item Ayarları");
            ItemsMenu.Add("useGhostblade", new CheckBox("Kullan Youmu"));
            ItemsMenu.Add("UseBOTRK", new CheckBox("Kullan Mahvolmuş Kılıç"));
            ItemsMenu.Add("UseBilge", new CheckBox("Kullan Bilgewater Palası"));
            ItemsMenu.Add("eL", new Slider("Düşmanın canı şu kadarsa", 65, 0, 100));
            ItemsMenu.Add("oL", new Slider("Benim canım şu kadarsa", 65, 0, 100));

            UltMenu = menuIni.AddSubMenu("Ultimate [BETA]");
            UltMenu.AddGroupLabel("Ulti Ayarları");
            UltMenu.Add("UseR", new CheckBox("Kullan R"));
            UltMenu.AddLabel("R Kullanma Ayarları:");
            UltMenu.Add("blind", new CheckBox("Körse?", false));
            UltMenu.Add("charm", new CheckBox("Use On Charms?"));
            UltMenu.Add("disarm", new CheckBox("Silahsız haldeyse?", false));
            UltMenu.Add("fear", new CheckBox("Korkmuşsa?"));
            UltMenu.Add("frenzy", new CheckBox("Donmuşsa?", false));
            UltMenu.Add("silence", new CheckBox("Sessiz kaldıysa?", false));
            UltMenu.Add("snare", new CheckBox("Yavaşlamışsa?"));
            UltMenu.Add("sleep", new CheckBox("Uyumuşsa?"));
            UltMenu.Add("stun", new CheckBox("sabitlenmişse?"));
            UltMenu.Add("supperss", new CheckBox("Use On Supperss?"));
            UltMenu.Add("slow", new CheckBox("yavaşlamışsa?", false));
            UltMenu.Add("knockup", new CheckBox("Use On Knock Ups?"));
            UltMenu.Add("knockback", new CheckBox("Devrilmişse?"));
            UltMenu.Add("nearsight", new CheckBox("Yakın görüşteyse?", false));
            UltMenu.Add("root", new CheckBox("Kök tutmuşsa?"));
            UltMenu.Add("tunt", new CheckBox("Alay ediliyorsa?"));
            UltMenu.Add("poly", new CheckBox("Use On Polymorph?"));
            UltMenu.Add("poison", new CheckBox("Zehirlenmişse?", false));
            UltMenu.Add("hp", new Slider("R yi sadece şu kadar canım varken kullan %", 25, 0, 100));
            UltMenu.Add("human", new Slider("insancıl gecikme", 150, 0, 1500));
            UltMenu.Add("Rene", new Slider("R kullanmak için çevrede düşman s.", 1, 0, 5));
            UltMenu.Add("enemydetect", new Slider("düşmanları tespit etme mesafesi", 1000, 0, 2000));
            UltMenu.AddLabel("Ult logic: It will Cast if you have one of the selected debuffs, HP under selected and Nearby enemies.");

            ComboMenu = menuIni.AddSubMenu("Combo");
            ComboMenu.AddGroupLabel("Kombo Ayarları");
            ComboMenu.Add("UseQ", new CheckBox("Use Q"));
            ComboMenu.Add("UseW", new CheckBox("Use W"));
            ComboMenu.Add("UseE", new CheckBox("Use E"));

            HarassMenu = menuIni.AddSubMenu("Harass");
            HarassMenu.AddGroupLabel("Dürtme Ayarları");
            HarassMenu.Add("hQ", new CheckBox("Kullan Q"));
            HarassMenu.Add("hQ2", new CheckBox("Kısa menzilde Q2 Kullan"));
            HarassMenu.Add("hQA", new CheckBox("Use Auto Q", false));
            HarassMenu.Add("hW", new CheckBox("Kullan W", false));
            HarassMenu.Add("hE", new CheckBox("Kullan E"));
            HarassMenu.Add("harassmana", new Slider("Harass Mana Manager", 60, 0, 100));

            LaneMenu = menuIni.AddSubMenu("Farm");
            LaneMenu.AddGroupLabel("LaneTemizleme Ayarları");
            LaneMenu.Add("laneQ", new CheckBox("Kullan Q"));
            LaneMenu.Add("fE", new CheckBox("Kullan E Sonvuruş"));
            LaneMenu.Add("laneW", new CheckBox("Kullan W"));
            LaneMenu.Add("laneE", new CheckBox("Kullan E", false));
            LaneMenu.Add("femana", new Slider("Can (E) yardımcısı", 75, 0, 100));
            LaneMenu.Add("lanemana", new Slider("Farm mana yardımcısı", 80, 0, 100));
            LaneMenu.AddGroupLabel("OrmanTemizleme Ayarları");
            LaneMenu.Add("jungleQ", new CheckBox("Kullan Q"));
            LaneMenu.Add("jE", new CheckBox("Son vuruşta E Kullan"));
            LaneMenu.Add("jungleW", new CheckBox("Kullan W"));
            LaneMenu.Add("jungleE", new CheckBox("Kullan E", false));
            LaneMenu.Add("jemana", new Slider("Can için (E) yardımcısı", 75, 0, 100));
            LaneMenu.Add("junglemana", new Slider("Orman için mana", 80, 0, 100));

            KillStealMenu = menuIni.AddSubMenu("Kill Steal");
            KillStealMenu.AddGroupLabel("Kill Çalma Ayarları");
            KillStealMenu.Add("ksQ", new CheckBox("Kill Çalma Q"));
            KillStealMenu.Add("ksE", new CheckBox("Kill Çalma E"));

            MiscMenu = menuIni.AddSubMenu("Misc");
            MiscMenu.AddGroupLabel("Ek Ayarları");
            MiscMenu.Add("gapcloser", new CheckBox("Use Q On GapCloser"));
            MiscMenu.Add("gapclosermana", new Slider("Anti-GapCloser Mana", 25, 0, 100));

            DrawMenu = menuIni.AddSubMenu("Drawings");
            DrawMenu.AddGroupLabel("Gösterge Ayarları");
            DrawMenu.Add("Qdraw", new CheckBox("Göster Q"));
            DrawMenu.Add("Edraw", new CheckBox("Göster E"));
            DrawMenu.Add("Rdraw", new CheckBox("Göster R Tespit Menzili"));
            DrawMenu.Add("AxeDraw", new CheckBox("Göster Balta pozisyonu"));

            Drawing.OnDraw += OnDraw;
            Game.OnUpdate += Game_OnGameUpdate;
            Gapcloser.OnGapcloser += Gapcloser_OnGap;
            GameObject.OnCreate += GameObject_OnCreate;
            GameObject.OnDelete += GameObject_OnDelete;
        }

        private static void Gapcloser_OnGap(AIHeroClient Sender, Gapcloser.GapcloserEventArgs args)
        {
            if (!menuIni.Get<CheckBox>("Misc").CurrentValue || !MiscMenu.Get<CheckBox>("gapcloser").CurrentValue
                || ObjectManager.Player.ManaPercent < MiscMenu.Get<Slider>("gapclosermana").CurrentValue || Sender == null)
            {
                return;
            }
            var predq = Q.GetPrediction(Sender);
            if (Sender.IsValidTarget(Q.Range) && Q.IsReady() && !Sender.IsAlly && !Sender.IsMe)
            {
                Q.Cast(predq.CastPosition);
            }
        }

        private static void Ult()
        {
            if (R.IsReady() && UltMenu["UseR"].Cast<CheckBox>().CurrentValue)
            {
                var debuff = (UltMenu["charm"].Cast<CheckBox>().CurrentValue && player.IsCharmed)
                             || (UltMenu["root"].Cast<CheckBox>().CurrentValue && player.IsRooted)
                             || (UltMenu["tunt"].Cast<CheckBox>().CurrentValue && player.IsTaunted)
                             || (UltMenu["stun"].Cast<CheckBox>().CurrentValue && player.IsStunned)
                             || (UltMenu["fear"].Cast<CheckBox>().CurrentValue && player.HasBuffOfType(BuffType.Fear))
                             || (UltMenu["silence"].Cast<CheckBox>().CurrentValue && player.HasBuffOfType(BuffType.Silence))
                             || (UltMenu["snare"].Cast<CheckBox>().CurrentValue && player.HasBuffOfType(BuffType.Snare))
                             || (UltMenu["supperss"].Cast<CheckBox>().CurrentValue && player.HasBuffOfType(BuffType.Suppression))
                             || (UltMenu["sleep"].Cast<CheckBox>().CurrentValue && player.HasBuffOfType(BuffType.Sleep))
                             || (UltMenu["poly"].Cast<CheckBox>().CurrentValue && player.HasBuffOfType(BuffType.Polymorph))
                             || (UltMenu["frenzy"].Cast<CheckBox>().CurrentValue && player.HasBuffOfType(BuffType.Frenzy))
                             || (UltMenu["disarm"].Cast<CheckBox>().CurrentValue && player.HasBuffOfType(BuffType.Disarm))
                             || (UltMenu["nearsight"].Cast<CheckBox>().CurrentValue && player.HasBuffOfType(BuffType.NearSight))
                             || (UltMenu["knockback"].Cast<CheckBox>().CurrentValue && player.HasBuffOfType(BuffType.Knockback))
                             || (UltMenu["knockup"].Cast<CheckBox>().CurrentValue && player.HasBuffOfType(BuffType.Knockup))
                             || (UltMenu["slow"].Cast<CheckBox>().CurrentValue && player.HasBuffOfType(BuffType.Slow))
                             || (UltMenu["poison"].Cast<CheckBox>().CurrentValue && player.HasBuffOfType(BuffType.Poison))
                             || (UltMenu["blind"].Cast<CheckBox>().CurrentValue && player.HasBuffOfType(BuffType.Blind));
                var enemys = UltMenu["Rene"].Cast<Slider>().CurrentValue;
                var hp = UltMenu["hp"].Cast<Slider>().CurrentValue;
                var enemysrange = UltMenu["enemydetect"].Cast<Slider>().CurrentValue;
                if (debuff && ObjectManager.Player.HealthPercent <= hp && enemys >= ObjectManager.Player.Position.CountEnemiesInRange(enemysrange))
                {
                    Core.DelayAction(() => R.Cast(), UltMenu["human"].Cast<Slider>().CurrentValue);
                }
            }
        }

        private static void combo()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            if (target == null || !target.IsValidTarget())
            {
                return;
            }

            if (target.IsValidTarget() && ComboMenu["UseQ"].Cast<CheckBox>().CurrentValue && Q.IsReady()
                && player.Distance(target.ServerPosition) <= Q.Range)
            {
                var Qpredict = Q.GetPrediction(target);
                var hithere = Qpredict.CastPosition.Extend(ObjectManager.Player.Position, -100);
                if (player.Distance(target.ServerPosition) >= 350)
                {
                    Q.Cast((Vector3)hithere);
                }
                else
                {
                    Q.Cast(Qpredict.CastPosition);
                }
            }

            if (target.IsValidTarget() && ComboMenu["UseE"].Cast<CheckBox>().CurrentValue && E.IsReady()
                && player.Distance(target.ServerPosition) <= E.Range)
            {
                E.Cast(target);
            }

            if (target.IsValidTarget() && ComboMenu["UseW"].Cast<CheckBox>().CurrentValue && W.IsReady()
                && player.Distance(target.ServerPosition) <= 225f)
            {
                W.Cast();
            }

            if (menuIni["Items"].Cast<CheckBox>().CurrentValue)
            {
                items();
            }
        }

        private static void Killsteal()
        {
            if (KillStealMenu["ksQ"].Cast<CheckBox>().CurrentValue && Q.IsReady())
            {
                var target =
                    ObjectManager.Get<AIHeroClient>()
                        .FirstOrDefault(
                            enemy => enemy.IsEnemy && enemy.IsValidTarget(Q.Range) && enemy.Health < player.GetSpellDamage(enemy, SpellSlot.Q));
                if (target.IsValidTarget(Q.Range))
                {
                    if (target != null)
                    {
                        Q.Cast(target.Position);
                        Player.IssueOrder(GameObjectOrder.AttackUnit, target);
                    }
                }
            }

            if (KillStealMenu["ksE"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                var target =
                    ObjectManager.Get<AIHeroClient>()
                        .FirstOrDefault(
                            enemy => enemy.IsEnemy && enemy.IsValidTarget(E.Range) && enemy.Health < player.GetSpellDamage(enemy, SpellSlot.E));
                if (target.IsValidTarget(E.Range) && target != null)
                {
                    E.Cast(target);
                }
            }
        }

        private static void items()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            if (target == null || !target.IsValidTarget())
            {
                return;
            }

            if (Botrk.IsReady() && Botrk.IsOwned(player) && Botrk.IsInRange(target)
                && target.HealthPercent <= ItemsMenu["eL"].Cast<Slider>().CurrentValue && ItemsMenu["UseBOTRK"].Cast<CheckBox>().CurrentValue)
            {
                Botrk.Cast(target);
            }

            if (Botrk.IsReady() && Botrk.IsOwned(player) && Botrk.IsInRange(target)
                && target.HealthPercent <= ItemsMenu["oL"].Cast<Slider>().CurrentValue && ItemsMenu["UseBOTRK"].Cast<CheckBox>().CurrentValue)

            {
                Botrk.Cast(target);
            }

            if (Cutlass.IsReady() && Cutlass.IsOwned(player) && Cutlass.IsInRange(target)
                && target.HealthPercent <= ItemsMenu["eL"].Cast<Slider>().CurrentValue && ItemsMenu["UseBilge"].Cast<CheckBox>().CurrentValue)
            {
                Cutlass.Cast(target);
            }

            if (Youmuu.IsReady() && Youmuu.IsOwned(player) && target.IsValidTarget(Q.Range)
                && ItemsMenu["useGhostblade"].Cast<CheckBox>().CurrentValue)
            {
                Youmuu.Cast();
            }
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (player.IsDead || MenuGUI.IsChatOpen || player.IsRecalling())
            {
                return;
            }

            var flags = Orbwalker.ActiveModesFlags;
            if (flags.HasFlag(Orbwalker.ActiveModes.Combo) && menuIni.Get<CheckBox>("Combo").CurrentValue)
            {
                combo();
            }

            if (flags.HasFlag(Orbwalker.ActiveModes.LaneClear) && menuIni.Get<CheckBox>("LaneClear").CurrentValue)
            {
                Clear();
                Lasthit();
            }

            if (flags.HasFlag(Orbwalker.ActiveModes.JungleClear) && menuIni.Get<CheckBox>("JungleClear").CurrentValue)
            {
                JungleClear();
            }

            if (flags.HasFlag(Orbwalker.ActiveModes.Harass) && menuIni.Get<CheckBox>("Harass").CurrentValue)
            {
                harass();
            }

            if (flags.HasFlag(Orbwalker.ActiveModes.LastHit) && menuIni.Get<CheckBox>("LastHit").CurrentValue)
            {
                Lasthit();
            }

            if (menuIni["KillSteal"].Cast<CheckBox>().CurrentValue)
            {
                Killsteal();
            }

            if (menuIni["Harass"].Cast<CheckBox>().CurrentValue)
            {
                AutoHarass();
            }

            if (menuIni["Ult"].Cast<CheckBox>().CurrentValue)
            {
                Ult();
            }
        }

        private static void Lasthit()
        {
            var femana = LaneMenu["femana"].Cast<Slider>().CurrentValue;
            var minions = EntityManager.MinionsAndMonsters.EnemyMinions;
            if (minions == null)
            {
                return;
            }

            if (E.IsReady() && LaneMenu["fE"].Cast<CheckBox>().CurrentValue && player.HealthPercent >= femana)
            {
                var etarget = ObjectManager.Get<Obj_AI_Base>().OrderBy(m => m.Health).Where(m => m.IsMinion && m.IsEnemy && !m.IsDead);
                foreach (var minion in etarget)
                {
                    if (minion.Health <= player.GetSpellDamage(minion, SpellSlot.E) && minion != null)
                    {
                        E.Cast(minion);
                    }
                }
            }
        }

        private static void AutoHarass()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            var harassmana = HarassMenu["harassmana"].Cast<Slider>().CurrentValue;
            if (!Q.IsReady() || !menuIni["Harass"].Cast<CheckBox>().CurrentValue || !HarassMenu["hQA"].Cast<CheckBox>().CurrentValue
                || player.IsRecalling() || target == null || !target.IsValidTarget())
            {
                return;
            }

            if (Q.IsReady() && HarassMenu["hQA"].Cast<CheckBox>().CurrentValue && target.IsValidTarget(Q.Range) && player.ManaPercent >= harassmana)
            {
                var Qpredict = Q.GetPrediction(target);
                var hithere = Qpredict.CastPosition.Extend(ObjectManager.Player.Position, -100);
                if (player.Distance(target.ServerPosition) >= 350)
                {
                    Q.Cast((Vector3)hithere);
                }
                else
                {
                    Q.Cast(Qpredict.CastPosition);
                }
            }
        }

        private static void harass()
        {
            var target = TargetSelector.GetTarget(E.Range, DamageType.Physical);
            var harassmana = HarassMenu["harassmana"].Cast<Slider>().CurrentValue;
            if (target == null || !target.IsValidTarget())
            {
                return;
            }

            if (HarassMenu["hQ"].Cast<CheckBox>().CurrentValue && target.IsValidTarget(Q.Range) && player.ManaPercent >= harassmana)
            {
                var Qpredict = Q.GetPrediction(target);
                var hithere = Qpredict.CastPosition.Extend(ObjectManager.Player.Position, -100);
                if (player.Distance(target.ServerPosition) >= 350)
                {
                    Q.Cast((Vector3)hithere);
                }
                else
                {
                    Q.Cast(Qpredict.CastPosition);
                }
            }

            if (HarassMenu["hQ2"].Cast<CheckBox>().CurrentValue && target.IsValidTarget(Q.Range) && player.ManaPercent >= harassmana)
            {
                if (target.IsValidTarget() && Q.IsReady() && player.Distance(target.ServerPosition) <= Q2.Range)
                {
                    var q2Predict = Q2.GetPrediction(target);
                    var hithere = q2Predict.CastPosition.Extend(ObjectManager.Player.Position, -140);
                    if (q2Predict.HitChance >= HitChance.High)
                    {
                        Q2.Cast((Vector3)hithere);
                    }
                }
            }

            if (E.IsReady() && player.ManaPercent >= harassmana && HarassMenu["hE"].Cast<CheckBox>().CurrentValue)
            {
                E.Cast(target);
            }

            if (W.IsReady() && target.IsValidTarget(175) && player.ManaPercent >= harassmana && HarassMenu["hW"].Cast<CheckBox>().CurrentValue)
            {
                W.Cast();
            }
        }

        private static void JungleClear()
        {
            var junglemana = LaneMenu["junglemana"].Cast<Slider>().CurrentValue;
            var jemana = LaneMenu["femana"].Cast<Slider>().CurrentValue;
            var Qlane = LaneMenu["jungleQ"].Cast<CheckBox>().CurrentValue && Q.IsReady();
            var Elane = LaneMenu["jungleE"].Cast<CheckBox>().CurrentValue && E.IsReady();
            var Wlane = LaneMenu["jungleW"].Cast<CheckBox>().CurrentValue && W.IsReady();

            var jmobs = ObjectManager.Get<Obj_AI_Minion>().OrderBy(m => m.CampNumber).Where(m => m.IsMonster && m.IsEnemy && !m.IsDead);
            foreach (var jmob in jmobs)
            {
                if (junglemana <= Player.Instance.ManaPercent)
                {
                    if (Qlane && !jmob.IsValidTarget(player.AttackRange) && jmob.IsValidTarget(Q.Range) && jmobs.Count() > 1)
                    {
                        Q.Cast(jmob);
                    }

                    if (Wlane && jmob.IsValidTarget(player.AttackRange) && Player.Instance.HealthPercent < 40)
                    {
                        W.Cast();
                    }
                }

                if (Elane && E.IsReady() && Player.Instance.HealthPercent > jemana && jmob.Health <= player.GetSpellDamage(jmob, SpellSlot.E)
                    && !jmob.IsValidTarget(player.AttackRange))
                {
                    E.Cast(jmob);
                }
            }
        }

        private static void Clear()
        {
            var lanemana = LaneMenu["lanemana"].Cast<Slider>().CurrentValue;
            var femana = LaneMenu["femana"].Cast<Slider>().CurrentValue;
            var Qlane = LaneMenu["laneQ"].Cast<CheckBox>().CurrentValue && Q.IsReady();
            var Elane = LaneMenu["laneE"].Cast<CheckBox>().CurrentValue && E.IsReady();
            var Wlane = LaneMenu["laneW"].Cast<CheckBox>().CurrentValue && W.IsReady();

            var minions = ObjectManager.Get<Obj_AI_Minion>().OrderBy(m => m.Health).Where(m => m.IsMinion && m.IsEnemy && !m.IsDead);

            foreach (var minion in minions)
            {
                if (lanemana <= Player.Instance.ManaPercent)
                {
                    if (Qlane && !minion.IsValidTarget(player.AttackRange) && minion.IsValidTarget(Q.Range)
                        && minion.Health <= player.GetSpellDamage(minion, SpellSlot.Q) && minions.Count() > 1)
                    {
                        Q.Cast(minion);
                    }

                    if (Wlane && minion.IsValidTarget(player.AttackRange) && Player.Instance.HealthPercent < 40)
                    {
                        W.Cast();
                    }
                }

                if (Elane && E.IsReady() && Player.Instance.HealthPercent > femana && minion.Health <= player.GetSpellDamage(minion, SpellSlot.E)
                    && !minion.IsValidTarget(player.AttackRange))
                {
                    E.Cast(minion);
                }
            }
        }

        private static void GameObject_OnCreate(GameObject obj, EventArgs args)
        {
            if (obj.Name == "olaf_axe_totem_team_id_green.troy")
            {
                olafAxe.Object = obj;
                olafAxe.ExpireTime = Game.Time + 8;
                olafAxe.NetworkId = obj.NetworkId;
                olafAxe.AxePos = obj.Position;
                //_axeObj = obj;
                //LastTickTime = Environment.TickCount;
            }
        }

        private static void GameObject_OnDelete(GameObject obj, EventArgs args)
        {
            if (obj.Name == "olaf_axe_totem_team_id_green.troy")
            {
                olafAxe.Object = null;
                //_axeObj = null;
                LastTickTime = 0;
            }
        }

        private static void OnDraw(EventArgs args)
        {
            if (!menuIni["Drawings"].Cast<CheckBox>().CurrentValue)
            {
                return;
            }

            if (DrawMenu["Qdraw"].Cast<CheckBox>().CurrentValue && Q.IsReady())
            {
                Circle.Draw(Color.White, Q.Range, Player.Instance.Position);
            }
            if (DrawMenu["Edraw"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                Circle.Draw(Color.White, E.Range, Player.Instance.Position);
            }
            if (DrawMenu["Rdraw"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(Color.DarkOrange, UltMenu["enemydetect"].Cast<Slider>().CurrentValue, Player.Instance.Position);
            }

            if (DrawMenu["AxeDraw"].Cast<CheckBox>().CurrentValue && olafAxe.Object != null)
            {
                Drawing.DrawCircle(olafAxe.Object.Position, 200, System.Drawing.Color.White);
            }

            /*
            if (Target != null && DrawMenu["combodamage"].Cast<CheckBox>().CurrentValue && Q.IsInRange(Target))
            {
                float[] Positions = GetLength();
                Drawing.DrawLine
                    (

                        new Vector2(Target.HPBarPosition.X + Positions[0] * 104, Target.HPBarPosition.Y),
                        new Vector2(Target.HPBarPosition.X + Positions[1] * 104, Target.HPBarPosition.Y),
                        15);
            }
            */
        }
    }
}