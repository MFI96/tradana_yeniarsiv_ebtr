using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using Nautilus.Utility;
using HitChance = Nautilus.Utility.HitChance;
using Prediction = Nautilus.Utility.Prediction;
using EloBuddy.SDK.Rendering;

namespace Nautilus
{
    internal class Program
    {
        private static readonly AIHeroClient Player = ObjectManager.Player;

        public static Spell.Targeted R;

        public static Spell.Skillshot Q;

        public static Spell.Active E, W;

        public static Menu ComboMenu { get; private set; }

        public static Menu DrawMenu { get; private set; }

        public static Menu MiscMenu { get; private set; }

        public static Menu KSMenu { get; private set; }

        public static Menu LaneMenu { get; private set; }

        public static Menu JungleMenu { get; private set; }

        public static Menu SmiteMenu { get; private set; }

        public static Menu FleeMenu { get; private set; }

        public static Menu PredictionMenu { get; private set; }

        public static AIHeroClient Target = null;

        private static Menu nautmenu;

        public static double TotalDamage = 0;

        public static Spell.Targeted SmiteSpell;

        public static Obj_AI_Base Monster;


        public static readonly string[] SmiteableUnits =
        {
            "SRU_Red", "SRU_Blue", "SRU_Dragon", "SRU_Baron",
            "SRU_Gromp", "SRU_Murkwolf", "SRU_Razorbeak",
            "SRU_Krug", "Sru_Crab"
        };

        private static readonly int[] SmiteRed = { 3715, 1415, 1414, 1413, 1412 };
        private static readonly int[] SmiteBlue = { 3706, 1403, 1402, 1401, 1400 };

        public static List<AIHeroClient> Enemies = new List<AIHeroClient>(), Allies = new List<AIHeroClient>();

        public static bool QCombo
        {
            get { return (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)); }
        }

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {

            if (ObjectManager.Player.BaseSkinName != "Nautilus")
            {
                return;
            }
            foreach (var hero in ObjectManager.Get<AIHeroClient>())
            {
                if (hero.IsEnemy)
                {
                    Enemies.Add(hero);
                }
                if (hero.IsAlly)
                    Allies.Add(hero);
            }



            Q = new Spell.Skillshot(SpellSlot.Q, 1100, SkillShotType.Linear, (int)0.5f, (int?)1900f, 90);
            Q.AllowedCollisionCount = 0;
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Active(SpellSlot.E, 300);
            R = new Spell.Targeted(SpellSlot.R, (uint)ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).SData.CastRange);

            nautmenu = MainMenu.AddMenu("Nautilus", "Nautilus");
            nautmenu.AddGroupLabel("Nautilus!");
            PredictionMenu = nautmenu.AddSubMenu("Prediction", "prediction");
            StringList(PredictionMenu, "Qpred", "Q Prediction", new[] { "Low", "Medium", "High", "Very High" }, 3);

            ComboMenu = nautmenu.AddSubMenu("Combo");
            ComboMenu.AddGroupLabel("Combo Ayarları");
            ComboMenu.Add("ts", new CheckBox("EB hedef seçici kullan"));
            ComboMenu.Add("ts1", new CheckBox("Sadece 1 hedefe odaklan", false));
            ComboMenu.Add("minGrab", new Slider("Çekmek için en az menzil", 250, 125, (int)Q.Range));
            ComboMenu.Add("maxGrab", new Slider("Çekmek için en fazla menzil", (int)Q.Range, 125, (int)Q.Range));
            ComboMenu.AddLabel("Çekme:");
            foreach (var enemy in ObjectManager.Get<AIHeroClient>().Where(enemy => enemy.Team != Player.Team))
                ComboMenu.Add("grab" + enemy.ChampionName, new CheckBox(enemy.ChampionName));
            ComboMenu.AddSeparator();
            ComboMenu.Add("useW", new CheckBox("Kullan W"));
            ComboMenu.Add("useE", new CheckBox("Kullan E"));
            ComboMenu.Add("useR", new CheckBox("Kullan R"));
            ComboMenu.AddGroupLabel("Ultiyi şu düşmanlara kullanma");
            foreach (var enemy in ObjectManager.Get<AIHeroClient>())
            {
                CheckBox cb = new CheckBox(enemy.BaseSkinName) { CurrentValue = false };
                if (enemy.Team != ObjectManager.Player.Team)
                {
                    ComboMenu.Add("DontUltenemy" + enemy.BaseSkinName, cb);
                }
            }

            KSMenu = nautmenu.AddSubMenu("KS");
            KSMenu.AddGroupLabel("KS Ayarları");
            KSMenu.Add("ksQ", new CheckBox("Kullan Q"));
            KSMenu.Add("ksE", new CheckBox("Kullan E"));

            LaneMenu = nautmenu.AddSubMenu("LaneCLear");
            LaneMenu.AddGroupLabel("Lanetemizleme Ayarları");
            LaneMenu.Add("UseWlc", new CheckBox("Kullan W"));
            LaneMenu.Add("UseElc", new CheckBox("Kullan E"));
            LaneMenu.AddSeparator();
            LaneMenu.Add("lccount", new Slider("E için en az minyon", 3, 1, 5));
            LaneMenu.Add("lanem", new Slider("Minimum mana %", 20, 0, 100));

            JungleMenu = nautmenu.AddSubMenu("Jungleclear");
            JungleMenu.AddGroupLabel("Ormantemizleme Ayarları");
            JungleMenu.Add("UseQjg", new CheckBox("Kullan Q"));
            JungleMenu.Add("UseWjg", new CheckBox("Kullan W"));
            JungleMenu.Add("UseEjg", new CheckBox("Kullan E"));
            JungleMenu.Add("jgMana", new Slider("en az mana %", 20, 0, 100));
            JungleMenu.AddSeparator();

            SmiteMenu = nautmenu.AddSubMenu("Smite", "Smite");
            SmiteMenu.AddSeparator();
            SmiteMenu.Add("smiteActive",
                new KeyBind("Çarp Aktif (Tuşu)", true, KeyBind.BindTypes.PressToggle, 'H'));
            SmiteMenu.AddSeparator();
            SmiteMenu.Add("useSlowSmite", new CheckBox("Mavi çarpla canavar çal"));
            SmiteMenu.Add("comboWithDuelSmite", new CheckBox("Kırmızı Çarp Komboda"));
            SmiteMenu.AddSeparator();
            SmiteMenu.AddGroupLabel("Kamplar");
            SmiteMenu.AddLabel("Epics");
            SmiteMenu.Add("SRU_Baron", new CheckBox("Baron"));
            SmiteMenu.Add("SRU_Dragon", new CheckBox("Ejder"));
            SmiteMenu.AddLabel("BUFFLAR");
            SmiteMenu.Add("SRU_Blue", new CheckBox("Mavi"));
            SmiteMenu.Add("SRU_Red", new CheckBox("Kırmızı"));
            SmiteMenu.AddLabel("Küçük Kamplar");
            SmiteMenu.Add("SRU_Gromp", new CheckBox("Kurbağa", false));
            SmiteMenu.Add("SRU_Murkwolf", new CheckBox("Alacakurtlar", false));
            SmiteMenu.Add("SRU_Krug", new CheckBox("Golemler", false));
            SmiteMenu.Add("SRU_Razorbeak", new CheckBox("Sivrigagalar", false));
            SmiteMenu.Add("Sru_Crab", new CheckBox("Yampiriyengeç", false));

            FleeMenu = nautmenu.AddSubMenu("Flee");
            FleeMenu.AddGroupLabel("Flee(kaçma) Ayarları");
            FleeMenu.Add("fleeuseQ", new CheckBox("Kullan Q"));
            FleeMenu.Add("fleeuseW", new CheckBox("Kullan W"));

            MiscMenu = nautmenu.AddSubMenu("Misc");
            MiscMenu.AddGroupLabel("Ek Ayarlar");
            MiscMenu.Add("antiG", new CheckBox("Kullan E - Antigapcloser"));
            MiscMenu.Add("interruptq", new CheckBox("Kullan Q - interrupter"));
            MiscMenu.Add("interruptr", new CheckBox("Kullan R - interrupter"));


            DrawMenu = nautmenu.AddSubMenu("Draw");
            DrawMenu.AddGroupLabel("Gösterge");
            DrawMenu.Add("drawq", new CheckBox("Göster Q"));
            DrawMenu.Add("drawe", new CheckBox("Göster E"));
            DrawMenu.Add("drawr", new CheckBox("Göster R"));

            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
            Game.OnUpdate += Game_OnUpdate;
            Gapcloser.OnGapcloser += Gapcloser_OnGapcloser;
            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnUpdate += SmiteEvent;
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (DrawMenu["drawq"].Cast<CheckBox>().CurrentValue)
            {
                Drawing.DrawCircle(Player.Position, Q.Range, System.Drawing.Color.CadetBlue);
            }

            if (DrawMenu["drawe"].Cast<CheckBox>().CurrentValue)
            {
                Drawing.DrawCircle(Player.Position, E.Range + 250, System.Drawing.Color.IndianRed);
            }

            if (DrawMenu["drawr"].Cast<CheckBox>().CurrentValue)
            {
                Drawing.DrawCircle(Player.Position, R.Range, System.Drawing.Color.YellowGreen);
            }
        }

        public static void StringList(Menu menu, string uniqueId, string displayName, string[] values, int defaultValue)
        {
            var mode = menu.Add(uniqueId, new Slider(displayName, defaultValue, 0, values.Length - 1));
            mode.DisplayName = displayName + ": " + values[mode.CurrentValue];
            mode.OnValueChange +=
                delegate (ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
                {
                    sender.DisplayName = displayName + ": " + values[args.NewValue];
                };
        }


        public static void Interrupter_OnInterruptableSpell(Obj_AI_Base unit, Interrupter.InterruptableSpellEventArgs spell)
        {
            if (MiscMenu["interruptq"].Cast<CheckBox>().CurrentValue && Q.IsReady() && Q.GetPrediction(Target).HitChance >= EloBuddy.SDK.Enumerations.HitChance.High)
            {
                if (unit.Distance(Player.ServerPosition, true) <= Q.Range && Q.GetPrediction(Target).HitChance >= EloBuddy.SDK.Enumerations.HitChance.High)
                {
                    Q.Cast(Target);
                }
            }
            if (MiscMenu["interruptr"].Cast<CheckBox>().CurrentValue && R.IsReady())
            {
                if (unit.Distance(Player.ServerPosition, true) <= R.Range)
                {
                    R.Cast();
                }
            }
        }

        private static void Gapcloser_OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {

            if (MiscMenu["antiG"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                if (e.Sender.IsValidTarget(E.Range))
                {
                    E.Cast();
                }
            }
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                Flee();
            }


            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) Combo();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)) LaneClear();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)) JungleClear();

            Killsteal();
            ;
        }

        private static float ComboDamage(Obj_AI_Base hero)
        {
            double damage = 0;

            if (Q.IsReady())
            {
                damage += Player.GetSpellDamage(hero, SpellSlot.Q);
            }
            if (E.IsReady())
            {
                damage += Player.GetSpellDamage(hero, SpellSlot.E);
            }
            if (R.IsReady())
            {
                damage += Player.GetSpellDamage(hero, SpellSlot.R);
            }

            var ignitedmg = Player.GetSummonerSpellDamage(hero, DamageLibrary.SummonerSpells.Ignite);
            if (Player.Spellbook.CanUseSpell(Player.GetSpellSlotFromName("summonerdot")) == SpellState.Ready &&
                hero.Health < damage + ignitedmg)
            {
                damage += ignitedmg;
            }

            return (float)damage;
        }




        private static void Combo()
        {


            bool vW = W.IsReady() && ComboMenu["useW"].Cast<CheckBox>().CurrentValue;
            bool vE = E.IsReady() && ComboMenu["useE"].Cast<CheckBox>().CurrentValue;
            bool vR = R.IsReady() && ComboMenu["useR"].Cast<CheckBox>().CurrentValue;

            var tsQ = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            var tsR = TargetSelector.GetTarget(R.Range, DamageType.Magical);

            if (tsQ == null || tsR == null)
                return;

            if (vR && tsR.IsValidTarget(R.Range) && tsR.Health > ComboDamage(tsR))
            {
                var useR = (!ComboMenu["DontUltenemy" + tsR.BaseSkinName].Cast<CheckBox>().CurrentValue);
                if (useR)
                {
                    R.Cast(tsR);
                }
            }


            if (QCombo && ComboMenu["ts"].Cast<CheckBox>().CurrentValue)
            {
                var t = TargetSelector.GetTarget(ComboMenu["maxGrab"].Cast<Slider>().CurrentValue, DamageType.Magical);

                if (t.IsValidTarget(ComboMenu["maxGrab"].Cast<Slider>().CurrentValue) && !t.HasBuffOfType(BuffType.SpellImmunity) &&
                    !t.HasBuffOfType(BuffType.SpellShield) && ComboMenu["grab" + t.ChampionName].Cast<CheckBox>().CurrentValue && Player.Distance(t.ServerPosition) > ComboMenu["minGrab"].Cast<Slider>().CurrentValue)
                    CastSpell(Q, t, predQ(), ComboMenu["maxGrab"].Cast<Slider>().CurrentValue);
            }


            foreach (var t in Enemies.Where(t => t.IsValidTarget(ComboMenu["maxGrab"].Cast<Slider>().CurrentValue) && ComboMenu["grab" + t.ChampionName].Cast<CheckBox>().CurrentValue))
            {
                if (!t.HasBuffOfType(BuffType.SpellImmunity) && !t.HasBuffOfType(BuffType.SpellShield) &&
                    Player.Distance(t.ServerPosition) > ComboMenu["minGrab"].Cast<Slider>().CurrentValue)
                {
                    if (QCombo && !ComboMenu["ts"].Cast<CheckBox>().CurrentValue)
                        CastSpell(Q, t, predQ(), ComboMenu["maxGrab"].Cast<Slider>().CurrentValue);

                    if (vW && tsQ.IsValidTarget(W.Range))
                        W.Cast();

                    if (vE && tsQ.IsValidTarget(E.Range))
                        E.Cast();

                }
            }
        }

        private static HitChance predQ()
        {
            switch (PredictionMenu["Qpred"].Cast<Slider>().CurrentValue)
            {
                case 0:
                    return HitChance.Low;
                case 1:
                    return HitChance.Medium;
                case 2:
                    return HitChance.High;
                case 3:
                    return HitChance.VeryHigh;
            }
            return HitChance.Medium;
        }

        private static bool collision;
        private static void Qcoltick(EventArgs args)
        {
            if (Orbwalker.LastTarget != null && Orbwalker.LastTarget is AIHeroClient)
            {
                var target = Orbwalker.LastTarget as AIHeroClient;
                var pred = Q.GetPrediction(target);
                if (pred.CollisionObjects.Any())
                {
                    collision = true;
                }
                else
                {
                    collision = false;
                }
            }
        }

        internal static bool IsAutoAttacking;
        private static void CastSpell(Spell.Skillshot QWER, Obj_AI_Base target, HitChance hitchance, int MaxRange)
        {
            var coreType2 = SkillshotType.SkillshotLine;
            var aoe2 = false;
            if ((int)QWER.Type == (int)SkillshotType.SkillshotCircle)
            {
                coreType2 = SkillshotType.SkillshotCircle;
                aoe2 = true;
            }
            if (QWER.Width > 80 && QWER.AllowedCollisionCount < 100)
                aoe2 = true;
            var predInput2 = new PredictionInput
            {
                Aoe = aoe2,
                Collision = QWER.AllowedCollisionCount < 100,
                Speed = QWER.Speed,
                Delay = QWER.CastDelay,
                Range = MaxRange,
                From = Player.ServerPosition,
                Radius = QWER.Radius,
                Unit = target,
                Type = coreType2
            };
            var poutput2 = Prediction.GetPrediction(predInput2);
            if (QWER.Speed < float.MaxValue && Utils.CollisionYasuo(Player.ServerPosition, poutput2.CastPosition))
                return;

            if (hitchance == HitChance.VeryHigh)
            {
                if (poutput2.Hitchance >= HitChance.VeryHigh)
                    QWER.Cast(poutput2.CastPosition);
                else if (predInput2.Aoe && poutput2.AoeTargetsHitCount > 1 &&
                         poutput2.Hitchance >= HitChance.High)
                {
                    QWER.Cast(poutput2.CastPosition);
                }
            }
            else if (hitchance == HitChance.High)
            {
                if (poutput2.Hitchance >= HitChance.High)
                    QWER.Cast(poutput2.CastPosition);
            }
            else if (hitchance == HitChance.Medium)
            {
                if (poutput2.Hitchance >= HitChance.Medium)
                    QWER.Cast(poutput2.CastPosition);
            }
            else if (hitchance == HitChance.Low)
            {
                if (poutput2.Hitchance >= HitChance.Low)
                    QWER.Cast(poutput2.CastPosition);
            }
        }
        private static void Killsteal()
        {
            foreach (AIHeroClient target in
                ObjectManager.Get<AIHeroClient>()
                    .Where(
                        hero =>
                            hero.IsValidTarget(Q.Range) && !hero.HasBuffOfType(BuffType.Invulnerability) && hero.IsEnemy)
                )
            {
                var qDmg = Player.GetSpellDamage(target, SpellSlot.Q);
                if (KSMenu["ksQ"].Cast<CheckBox>().CurrentValue && target.IsValidTarget(Q.Range) && target.Health <= qDmg)
                {
                    var qpred = Q.GetPrediction(target);
                    if (qpred.HitChance >= EloBuddy.SDK.Enumerations.HitChance.High && qpred.CollisionObjects.Count(h => h.IsEnemy && !h.IsDead && h is Obj_AI_Minion) < 2)
                        Q.Cast(qpred.CastPosition);
                }
                var eDmg = Player.GetSpellDamage(target, SpellSlot.E);
                if (KSMenu["ksE"].Cast<CheckBox>().CurrentValue && target.IsValidTarget(E.Range) && target.Health <= eDmg)
                {
                    E.Cast();
                }
            }
        }

        public static void LaneClear()
        {

            Orbwalker.ForcedTarget = null;
            var count = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.ServerPosition, E.Range, false).Count();
            if (count == 0) return;
            if (E.IsReady() && LaneMenu["UseElc"].Cast<CheckBox>().CurrentValue && LaneMenu["lccount"].Cast<Slider>().CurrentValue <= count)
            {
                E.Cast();
                return;
            }

            if (W.IsReady() && Player.HealthPercent <= 99 && LaneMenu["UseWlc"].Cast<CheckBox>().CurrentValue)
            {
                W.Cast();
            }

            return;

        }

        private static void JungleClear()

        {
            var minion =
                EntityManager.MinionsAndMonsters.GetJungleMonsters()
                    .Where(x => x.IsValidTarget(W.Range))
                    .OrderByDescending(x => x.MaxHealth)
                    .FirstOrDefault(x => x != null);
            if (minion == null)
            {
                return;
            }


            if (Q.IsReady() && minion.IsValidTarget(Q.Range) && JungleMenu["UseQjg"].Cast<CheckBox>().CurrentValue)
            {
                Q.Cast(minion);
            }

            if (E.IsReady() && minion.IsValidTarget(E.Range) && JungleMenu["UseEjg"].Cast<CheckBox>().CurrentValue)
            {
                E.Cast();
            }


            if (W.IsReady() && minion.IsValidTarget(E.Range) && JungleMenu["UseWjg"].Cast<CheckBox>().CurrentValue)
            {
                W.Cast();
            }

        }


        public static Vector2[] CircleCircleIntersection(Vector2 center1, Vector2 center2, float radius1, float radius2)
        {
            var d = center1.Distance(center2);
            if (d > radius1 + radius2 || (d <= Math.Abs(radius1 - radius2)))
            {
                return new Vector2[] { };
            }

            var a = (radius1 * radius1 - radius2 * radius2 + d * d) / (2 * d);
            var h = (float)Math.Sqrt(radius1 * radius1 - a * a);
            var direction = (center2 - center1).Normalized();
            var pa = center1 + a * direction;
            var s1 = pa + h * direction.Perpendicular();
            var s2 = pa - h * direction.Perpendicular();
            return new[] { s1, s2 };
        }

        private static void Flee()
        {
            EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos, false);

            bool vQ = Q.IsReady() && FleeMenu["fleeuseQ"].Cast<CheckBox>().CurrentValue;
            bool vW = W.IsReady() && FleeMenu["fleeuseW"].Cast<CheckBox>().CurrentValue;

            var minions = ObjectManager.Get<Obj_AI_Minion>().Where(minion => minion.IsValidTarget(Q.Range)).ToList(); // Hopefully this is enough...
            var step = Q.Range / 2; // Or whatever step value...
            for (var i = step; i <= Q.Range; i += step)
            {
                if (ObjectManager.Player.Position.Extend(Game.CursorPos, i).IsWall() && Player.Distance(Game.CursorPos) >= Q.Range / 2 && vQ)
                {
                    Q.Cast(Game.CursorPos);
                }

            }

            if (vW && ObjectManager.Get<Obj_AI_Base>().Any(x => x.IsEnemy && x.Distance(Player.Position) <= Q.Range && Player.IsTargetable))
            {
                W.Cast();
            }
        }



        public static void SetSmiteSlot()
        {
            SpellSlot smiteSlot;
            if (SmiteBlue.Any(x => Player.InventoryItems.FirstOrDefault(a => a.Id == (ItemId)x) != null))
                smiteSlot = Player.GetSpellSlotFromName("s5_summonersmiteplayerganker");
            else if (
                SmiteRed.Any(
                    x => Player.InventoryItems.FirstOrDefault(a => a.Id == (ItemId)x) != null))
                smiteSlot = Player.GetSpellSlotFromName("s5_summonersmiteduel");
            else
                smiteSlot = Player.GetSpellSlotFromName("summonersmite");
            SmiteSpell = new Spell.Targeted(smiteSlot, 500);
        }

        public static int GetSmiteDamage()
        {
            var level = Player.Level;
            int[] smitedamage =
            {
                20*level + 370,
                30*level + 330,
                40*level + 240,
                50*level + 100
            };
            return smitedamage.Max();
        }

        private static void SmiteEvent(EventArgs args)
        {
            SetSmiteSlot();
            if (!SmiteSpell.IsReady() || Player.IsDead) return;
            if (SmiteMenu["smiteActive"].Cast<KeyBind>().CurrentValue)
            {
                var unit =
                    EntityManager.MinionsAndMonsters.Monsters
                        .Where(
                            a =>
                                SmiteableUnits.Contains(a.BaseSkinName) && a.Health < GetSmiteDamage() &&
                                SmiteMenu[a.BaseSkinName].Cast<CheckBox>().CurrentValue)
                        .OrderByDescending(a => a.MaxHealth)
                        .FirstOrDefault();

                if (unit != null)
                {
                    SmiteSpell.Cast(unit);
                    return;
                }
            }
            if (SmiteMenu["useSlowSmite"].Cast<CheckBox>().CurrentValue &&
                SmiteSpell.Handle.Name == "s5_summonersmiteplayerganker")
            {
                foreach (
                    var target in
                        EntityManager.Heroes.Enemies
                            .Where(h => h.IsValidTarget(SmiteSpell.Range) && h.Health <= 20 + 8 * Player.Level))
                {
                    SmiteSpell.Cast(target);
                    return;
                }
            }
            if (SmiteMenu["comboWithDuelSmite"].Cast<CheckBox>().CurrentValue &&
                SmiteSpell.Handle.Name == "s5_summonersmiteduel" &&
                Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                foreach (
                    var target in
                        EntityManager.Heroes.Enemies
                            .Where(h => h.IsValidTarget(SmiteSpell.Range)).OrderByDescending(TargetSelector.GetPriority)
                    )
                {
                    SmiteSpell.Cast(target);
                    return;
                }
            }
        }
    }
}