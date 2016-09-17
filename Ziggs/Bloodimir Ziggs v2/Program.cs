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

namespace Bloodimir_Ziggs_v2
{
    internal static class Program
    {
        public static Spell.Skillshot Q;
        private static Spell.Skillshot _q2;
        private static Spell.Skillshot _q3;
        public static Spell.Skillshot W;
        public static Spell.Skillshot E;
        public static Spell.Skillshot R;
        private static readonly AIHeroClient Ziggs = ObjectManager.Player;
        private static int _useSecondWTime;

        public static Menu ZiggsMenu,
            ComboMenu,
            LaneJungleClear,
            SkinMenu,
            LastHitMenu,
            MiscMenu,
            HarassMenu,
            DrawMenu,
            FleeMenu,
            PredMenu;

        private static AIHeroClient SelectedHero { get; set; }

        private static Vector3 MousePos
        {
            get { return Game.CursorPos; }
        }

        private static bool HasSpell(string s)
        {
            return Player.Spells.FirstOrDefault(o => o.SData.Name.Contains(s)) != null;
        }

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoaded;
        }

        private static void OnLoaded(EventArgs args)
        {
            if (Player.Instance.ChampionName != "Ziggs")
                return;
            Bootstrap.Init(null);
            Q = new Spell.Skillshot(SpellSlot.Q, 850, SkillShotType.Circular, 300, 1700, 130);
            _q2 = new Spell.Skillshot(SpellSlot.Q, 1125, SkillShotType.Circular, 250 + Q.CastDelay, 1700, 130);
            _q3 = new Spell.Skillshot(SpellSlot.Q, 1400, SkillShotType.Circular, 300 + _q2.CastDelay, 1700, 140);
            W = new Spell.Skillshot(SpellSlot.W, 1000, SkillShotType.Circular, 250, 1750, 275);
            E = new Spell.Skillshot(SpellSlot.E, 900, SkillShotType.Circular, 500, 1750, 100);
            R = new Spell.Skillshot(SpellSlot.R, 5300, SkillShotType.Circular, 2000, 1500, 500);
            ZiggsMenu = MainMenu.AddMenu("BloodimirZiggs", "bloodimirziggs");
            ZiggsMenu.AddGroupLabel("Bloodimir Ziggs v2.0.2.0");
            ZiggsMenu.AddSeparator();
            ZiggsMenu.AddLabel("Bloodimir Ziggs v2.0.2.0");

            ComboMenu = ZiggsMenu.AddSubMenu("Combo", "sbtw");
            ComboMenu.AddGroupLabel("Kombo Ayarları");
            ComboMenu.AddSeparator();
            ComboMenu.Add("usecomboq", new CheckBox("Q Kullan"));
            ComboMenu.Add("usecomboe", new CheckBox("E Kullan"));
            ComboMenu.Add("usecombow", new CheckBox("W Kullan"));
            ComboMenu.Add("usecombor", new CheckBox("R Kullan"));
            ComboMenu.AddSeparator();
            ComboMenu.Add("rslider", new Slider("R için gerekli kişi sayısı", 1, 0, 5));
            ComboMenu.Add("wslider", new Slider("W için Düşmanın Canının Oranı", 15));
            ComboMenu.Add("waitAA", new CheckBox("AA tamamlanmasını bekle", false));

            HarassMenu = ZiggsMenu.AddSubMenu("HarassMenu", "Harass");
            HarassMenu.Add("useQHarass", new CheckBox("Q Kullan"));
            HarassMenu.Add("waitAA", new CheckBox("AA tamamlanmasını bekle", false));

            LaneJungleClear = ZiggsMenu.AddSubMenu("Lane Jungle Clear", "lanejungleclear");
            LaneJungleClear.AddGroupLabel("Lane-OrmanTemizleme Ayarları");
            LaneJungleClear.Add("LCE", new CheckBox("E Kullan"));
            LaneJungleClear.Add("LCQ", new CheckBox("Q Kullan"));

            LastHitMenu = ZiggsMenu.AddSubMenu("Last Hit", "lasthit");
            LastHitMenu.AddGroupLabel("Son Vuruş Ayarları");
            LastHitMenu.Add("LHQ", new CheckBox("Q Kullan"));

            DrawMenu = ZiggsMenu.AddSubMenu("Drawings", "drawings");
            DrawMenu.AddGroupLabel("Gösterge");
            DrawMenu.AddSeparator();
            DrawMenu.Add("drawq", new CheckBox("Göster Q"));
            DrawMenu.Add("draww", new CheckBox("Göster W"));
            DrawMenu.Add("drawe", new CheckBox("Göster E"));
            DrawMenu.Add("drawaa", new CheckBox("Göster AA"));

            MiscMenu = ZiggsMenu.AddSubMenu("Misc Menu", "miscmenu");
            MiscMenu.AddGroupLabel("KS");
            MiscMenu.AddSeparator();
            MiscMenu.Add("ksq", new CheckBox("KS için Q"));
            MiscMenu.Add("int", new CheckBox("TRY to Interrupt spells"));
            MiscMenu.Add("gapw", new CheckBox("Anti Gapcloser W"));
            MiscMenu.Add("peel", new CheckBox("Peel From Melees")); ;

            FleeMenu = ZiggsMenu.AddSubMenu("Flee", "Flee");
            FleeMenu.Add("fleew", new CheckBox("Kaçarken Wyi mouse'un olduğu yere doğru kullan"));

            PredMenu = ZiggsMenu.AddSubMenu("Prediction", "pred");
            PredMenu.AddGroupLabel("Q İsabet Oranı");
            var qslider = PredMenu.Add("hQ", new Slider("Q İsabet Oranı", 2, 0, 2));
            var qMode = new[] {"Low (Fast Casting)", "Medium", "High (Slow Casting)"};
            qslider.DisplayName = qMode[qslider.CurrentValue];

            qslider.OnValueChange +=
                delegate(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs changeArgs)
                {
                    sender.DisplayName = qMode[changeArgs.NewValue];
                };
            PredMenu.AddGroupLabel("E İsabet Oranı");
            var eslider = PredMenu.Add("hE", new Slider("E İsabet Oranı", 2, 0, 2));
            var eMode = new[] {"Low (Fast Casting)", "Medium", "High (Slow Casting)"};
            eslider.DisplayName = eMode[eslider.CurrentValue];

            eslider.OnValueChange +=
                delegate(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs changeArgs)
                {
                    sender.DisplayName = eMode[changeArgs.NewValue];
                };
            PredMenu.AddGroupLabel("W İsabet Oranı");
            var wslider = PredMenu.Add("hW", new Slider("W İsabet Oranı", 1, 0, 2));
            var wMode = new[] {"Low (Fast Casting)", "Medium", "High (Slow Casting)"};
            wslider.DisplayName = wMode[wslider.CurrentValue];

            wslider.OnValueChange +=
                delegate(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs changeArgs)
                {
                    sender.DisplayName = wMode[changeArgs.NewValue];
                };
            SkinMenu = ZiggsMenu.AddSubMenu("Skin Changer", "skin");
            SkinMenu.AddGroupLabel("İstediğiniz görünümü seçin");

            var skinchange = SkinMenu.Add("sID", new Slider("Skin", 4, 0, 5));
            var sid = new[] {"Default", "Mad Scientist", "Major", "Pool Party", "Snow Day", "Master Arcanist"};
            skinchange.DisplayName = sid[skinchange.CurrentValue];
            skinchange.OnValueChange +=
                delegate(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs changeArgs)
                {
                    sender.DisplayName = sid[changeArgs.NewValue];
                };
            Game.OnUpdate += Game_OnTick;
            Gapcloser.OnGapcloser += Gapcloser_OnGapCloser;
            Interrupter.OnInterruptableSpell += Interruptererer;
            Game.OnWndProc += Game_OnWndProc;
            Drawing.OnDraw += OnDraw;
        }

        private static void Game_OnTick(EventArgs args)
        {
            Killsteal();
            SkinChange();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                Combo();
            var target = TargetSelector.GetTarget(1200f, DamageType.Magical);
            if (target == null) return;
            if (ComboMenu["usecomboq"].Cast<CheckBox>().CurrentValue
                && Q.IsReady())
            {
                CastQ(target);
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
                Harass();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
                LaneJungleClearA.LaneClear();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
                LastHitA.LastHitB();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
                Flee();
        }

        private static
            void Gapcloser_OnGapCloser
            (AIHeroClient sender, Gapcloser.GapcloserEventArgs gapcloser)
        {
            if (!MiscMenu["gapw"].Cast<CheckBox>().CurrentValue) return;
            if (!(ObjectManager.Player.Distance(gapcloser.Sender, true) <
                  W.Range*W.Range) || !sender.IsValidTarget()) return;
            W.Cast(gapcloser.Sender);
            W.Cast(gapcloser.Sender);
        }

        private static
            void Interruptererer
            (Obj_AI_Base sender,
                Interrupter.InterruptableSpellEventArgs args)
        {
            var intTarget = TargetSelector.GetTarget(W.Range, DamageType.Magical);
            {
                if (W.IsReady() && sender.IsValidTarget(W.Range) &&
                    MiscMenu["int"].Cast<CheckBox>().CurrentValue)
                    W.Cast(intTarget.ServerPosition);
            }
        }

        private static void OnDraw(EventArgs args)
        {
            if (Ziggs.IsDead) return;
            if (DrawMenu["drawq"].Cast<CheckBox>().CurrentValue && Q.IsLearned)
            {
                Circle.Draw(Color.Goldenrod, Q.Range, Ziggs.Position);
                Circle.Draw(Color.Blue, _q2.Range, Ziggs.Position);
                Circle.Draw(Color.Tomato, _q3.Range, Ziggs.Position);
            }
            {
                if (DrawMenu["drawe"].Cast<CheckBox>().CurrentValue && E.IsLearned)
                {
                    Circle.Draw(Color.MediumVioletRed, E.Range, Player.Instance.Position);
                }
                if (DrawMenu["draww"].Cast<CheckBox>().CurrentValue && W.IsLearned)
                {
                    Circle.Draw(Color.DarkRed, W.Range, Player.Instance.Position);
                }
                if (DrawMenu["drawaa"].Cast<CheckBox>().CurrentValue)
                {
                    Circle.Draw(Color.DimGray, 550, Ziggs.Position);
                }
            }
        }

        private static void Harass()
        {
            var target = TargetSelector.GetTarget(1000, DamageType.Magical);
            var qpredvalue = Q.GetPrediction(target).HitChance >= PredQ();

            if (SelectedHero != null)
            {
                target = SelectedHero;
            }

            if (target == null || !target.IsValid())
            {
                return;
            }

            Orbwalker.OrbwalkTo(MousePos);
            if (Orbwalker.IsAutoAttacking && HarassMenu["waitAA"].Cast<CheckBox>().CurrentValue)
                return;
            if (!HarassMenu["useQHarass"].Cast<CheckBox>().CurrentValue || !Q.IsReady() || !qpredvalue) return;
            if (!(target.Distance(Ziggs) <= Q.Range)) return;
            var predQ = Q.GetPrediction(target).CastPosition;
            Q.Cast(predQ);
        }

        private static
            void Game_OnWndProc
            (WndEventArgs
                args)
        {
            if (args.Msg != (uint) WindowMessages.LeftButtonDown)
            {
                return;
            }
            SelectedHero =
                EntityManager.Heroes.Enemies
                    .FindAll(hero => hero.IsValidTarget() && hero.Distance(Game.CursorPos, true) < 39999)
                    .OrderBy(h => h.Distance(Game.CursorPos, true)).FirstOrDefault();
        }

        private static
            void Combo
            ()
        {
            var target = TargetSelector.GetTarget(1550, DamageType.Magical);
            if (target == null || !target.IsValid())
            {
                return;
            }

            if (Orbwalker.IsAutoAttacking && HarassMenu["waitAA"].Cast<CheckBox>().CurrentValue) return;

            if (ComboMenu["usecomboe"].Cast<CheckBox>().CurrentValue && E.IsReady() &&
                E.GetPrediction(target).HitChance >= PredE())
            {
                var predE = E.GetPrediction(target).CastPosition;
                E.Cast(predE);
            }
            if (ComboMenu["usecombow"].Cast<CheckBox>().CurrentValue && W.IsReady())
            {
            }
            var wpred = W.GetPrediction(target);
            if (wpred.HitChance > PredW()) return;
            if (W.IsInRange(wpred.UnitPosition) &&
                target.HealthPercent >= ComboMenu["wslider"].Cast<Slider>().CurrentValue &&
                ObjectManager.Player.ServerPosition.Distance(wpred.UnitPosition) > W.Range - 250 &&
                wpred.UnitPosition.Distance(ObjectManager.Player.ServerPosition) >
                target.Distance(ObjectManager.Player))
            {
                var pp =
                    ObjectManager.Player.ServerPosition.To2D()
                        .Extend(wpred.UnitPosition.To2D(), W.Range)
                        .To3D();
                W.Cast(pp);
                _useSecondWTime = Environment.TickCount;
            }
            if (ComboMenu["usecombor"].Cast<CheckBox>().CurrentValue)
                if (R.IsReady())
                {
                    var predR = R.GetPrediction(target).CastPosition;
                    if (target.CountEnemiesInRange(R.Width) >= ComboMenu["rslider"].Cast<Slider>().CurrentValue)
                        R.Cast(predR);
                }

            if (!MiscMenu["peel"].Cast<CheckBox>().CurrentValue) return;
            foreach (var pos in from enemy in ObjectManager.Get<Obj_AI_Base>()
                where
                    enemy.IsValidTarget() &&
                    enemy.Distance(ObjectManager.Player) <=
                    enemy.BoundingRadius + enemy.AttackRange + ObjectManager.Player.BoundingRadius &&
                    enemy.IsMelee
                let direction =
                    (enemy.ServerPosition.To2D() - ObjectManager.Player.ServerPosition.To2D()).Normalized()
                let pos = ObjectManager.Player.ServerPosition.To2D()
                select pos + Math.Min(200, Math.Max(50, enemy.Distance(ObjectManager.Player)/2))*direction)
            {
                W.Cast(pos.To3D());
                _useSecondWTime = Environment.TickCount;
            }
        }

        private static void CastQ(Obj_AI_Base target)
        {
            if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) ||
                !ComboMenu["usecomboq"].Cast<CheckBox>().CurrentValue) return;
            PredictionResult prediction;

            if (Q.IsInRange(target))
            {
                prediction = Q.GetPrediction(target);
                Q.Cast(prediction.CastPosition);
            }
            else if (_q2.IsInRange(target))
            {
                prediction = _q2.GetPrediction(target);
                _q2.Cast(prediction.CastPosition);
            }
            else if (_q3.IsInRange(target))
            {
                prediction = _q3.GetPrediction(target);
                _q3.Cast(prediction.CastPosition);
            }
            else
            {
                return;
            }

            if (prediction.HitChance < HitChance.High) return;
            if (ObjectManager.Player.ServerPosition.Distance(prediction.CastPosition) <= Q.Range + Q.Width)
            {
                Vector3 p;
                if (ObjectManager.Player.ServerPosition.Distance(prediction.CastPosition) > 300)
                {
                    p = prediction.CastPosition -
                        100*
                        (prediction.CastPosition.To2D() - ObjectManager.Player.ServerPosition.To2D()).Normalized()
                            .To3D();
                }
                else
                {
                    p = prediction.CastPosition;
                }

                Q.Cast(p);
            }
            else if (ObjectManager.Player.ServerPosition.Distance(prediction.CastPosition) <=
                     (Q.Range + _q2.Range)/2)
            {
                var p = ObjectManager.Player.ServerPosition.To2D()
                    .Extend(prediction.CastPosition.To2D(), Q.Range - 100);

                if (!CheckQCollision(target, prediction.UnitPosition, p.To3D()))
                {
                    Q.Cast(p.To3D());
                }
            }
            else
            {
                var p = ObjectManager.Player.ServerPosition.To2D() +
                        Q.Range*
                        (prediction.CastPosition.To2D() - ObjectManager.Player.ServerPosition.To2D()).Normalized
                            ();

                if (!CheckQCollision(target, prediction.UnitPosition, p.To3D()))
                {
                    Q.Cast(p.To3D());
                }
            }
        }

        private static bool CheckQCollision(Obj_AI_Base target, Vector3 targetPosition, Vector3 castPosition)
        {
            var direction = (castPosition.To2D() - ObjectManager.Player.ServerPosition.To2D()).Normalized();
            var firstBouncePosition = castPosition.To2D();
            var secondBouncePosition = firstBouncePosition +
                                       direction*0.4f*
                                       ObjectManager.Player.ServerPosition.To2D().Distance(firstBouncePosition);
            var thirdBouncePosition = secondBouncePosition +
                                      direction*0.6f*firstBouncePosition.Distance(secondBouncePosition);

            if (thirdBouncePosition.Distance(targetPosition.To2D()) < Q.Width + target.BoundingRadius)
            {
                if ((from minion in ObjectManager.Get<Obj_AI_Minion>()
                    where minion.IsValidTarget(3000)
                    let predictedPos = _q2.GetPrediction(minion)
                    where predictedPos.UnitPosition.To2D().Distance(secondBouncePosition) <
                          _q2.Width + minion.BoundingRadius
                    select minion).Any())
                {
                    return true;
                }
            }

            if (secondBouncePosition.Distance(targetPosition.To2D()) < Q.Width + target.BoundingRadius ||
                thirdBouncePosition.Distance(targetPosition.To2D()) < Q.Width + target.BoundingRadius)
            {
                return (from minion in ObjectManager.Get<Obj_AI_Minion>()
                    where minion.IsValidTarget(3000)
                    let predictedPos = Q.GetPrediction(minion)
                    where
                        predictedPos.UnitPosition.To2D().Distance(firstBouncePosition) < Q.Width + minion.BoundingRadius
                    select minion).Any();
            }

            return true;
        }

        private static
            void Killsteal
            ()
        {
            if (!MiscMenu["ksq"].Cast<CheckBox>().CurrentValue || !Q.IsReady()) return;
            foreach (
                var qtarget in
                    EntityManager.Heroes.Enemies.Where(
                        hero => hero.IsValidTarget(Q.Range) && !hero.IsDead && !hero.IsZombie))
            {
                if (!(Ziggs.GetSpellDamage(qtarget, SpellSlot.Q) >= qtarget.Health)) continue;
                var qkspred = Q.GetPrediction(qtarget).CastPosition;
                var qks2Pred = _q2.GetPrediction(qtarget).CastPosition;
                var qks3Pred = _q3.GetPrediction(qtarget).CastPosition;
                {
                    if (Q.IsInRange(qtarget))
                    {
                        Q.Cast(qkspred);
                    }
                    else if (_q2.IsInRange(qtarget))
                    {
                        _q2.Cast(qks2Pred);
                    }
                    else if (_q3.IsInRange(qtarget))
                    {
                        _q3.Cast(qks3Pred);
                    }
                }
                {
                }
            }
        }

        private static
            void Flee
            ()
        {
            if (!FleeMenu["fleew"].Cast<CheckBox>().CurrentValue) return;
            Orbwalker.MoveTo(Game.CursorPos);
            W.Cast(Ziggs.ServerPosition);
        }

        private static
            HitChance PredQ
            ()
        {
            var mode = PredMenu["hQ"].DisplayName;
            switch (mode)
            {
                case "Low (Fast Casting)":
                    return HitChance.Low;
                case "Medium":
                    return HitChance.Medium;
                case "High (Slow Casting)":
                    return HitChance.High;
            }
            return HitChance.Medium;
        }

        private static
            HitChance PredE
            ()
        {
            var mode = PredMenu["hE"].DisplayName;
            switch (mode)
            {
                case "Low (Fast Casting)":
                    return HitChance.Low;
                case "Medium":
                    return HitChance.Medium;
                case "High (Slow Casting)":
                    return HitChance.High;
            }
            return HitChance.Medium;
        }

        private static
            HitChance PredW
            ()
        {
            var mode = PredMenu["hW"].DisplayName;
            switch (mode)
            {
                case "Low (Fast Casting)":
                    return HitChance.Low;
                case "Medium":
                    return HitChance.Medium;
                case "High (Slow Casting)":
                    return HitChance.High;
            }
            return HitChance.Medium;
        }

        private static void SkinChange()
        {
            var style = SkinMenu["sID"].DisplayName;
            switch (style)
            {
                case "Default":
                    Player.SetSkinId(0);
                    break;
                case "Mad Scientist":
                    Player.SetSkinId(1);
                    break;
                case "Major":
                    Player.SetSkinId(2);
                    break;
                case "Pool Party":
                    Player.SetSkinId(3);
                    break;
                case "Snow Day":
                    Player.SetSkinId(4);
                    break;
                case "Master Arcanist":
                    Player.SetSkinId(5);
                    break;
            }
        }
    }
}
