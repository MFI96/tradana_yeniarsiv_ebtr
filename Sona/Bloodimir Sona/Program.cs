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

namespace Bloodimir_Sona
{
    internal class Program
    {
        public static Spell.Active Q;
        public static Spell.Active W;
        public static Spell.Active E;
        public static Spell.Skillshot R;
        public static Spell.Targeted Exhaust;
        public static AIHeroClient Sona = ObjectManager.Player;
        public static Item FrostQueen;

        public static Menu SonaMenu,
            ComboMenu,
            LaneJungleClear,
            SkinMenu,
            MiscMenu,
            HarassMenu,
            DrawMenu,
            FleeMenu;

        public static AIHeroClient SelectedHero { get; set; }

        private static Vector3 MousePos
        {
            get { return Game.CursorPos; }
        }

        public static bool HasSpell(string s)
        {
            return Player.Spells.FirstOrDefault(o => o.SData.Name.Contains(s)) != null;
        }

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoaded;
        }

        private static void OnLoaded(EventArgs args)
        {
            if (Player.Instance.ChampionName != "Sona")
                return;
            Bootstrap.Init(null);
            Q = new Spell.Active(SpellSlot.Q, 850);
            W = new Spell.Active(SpellSlot.W, 1000);
            E = new Spell.Active(SpellSlot.E, 350);
            R = new Spell.Skillshot(SpellSlot.R, 1000, SkillShotType.Circular, 250, 2400, 140);
            FrostQueen = new Item(3092, 850f);
            Exhaust = new Spell.Targeted(ObjectManager.Player.GetSpellSlotFromName("summonerexhaust"), 650);

            SonaMenu = MainMenu.AddMenu("BloodimirSona", "bloodimirsona");
            SonaMenu.AddGroupLabel("Bloodimir Sona v1.0.0.0");
            SonaMenu.AddSeparator();
            SonaMenu.AddLabel("Bloodimir Sona v1.0.0.0");

            ComboMenu = SonaMenu.AddSubMenu("Combo", "sbtw");
            ComboMenu.AddGroupLabel("Kombo Ayarları");
            ComboMenu.AddSeparator();
            ComboMenu.Add("usecomboq", new CheckBox("Q Kullan"));
            ComboMenu.Add("usecombor", new CheckBox("R Kullan"));
            ComboMenu.Add("comboOnlyExhaust", new CheckBox("Use Exhaust (Combo Only)"));
            ComboMenu.Add("useitems", new CheckBox("İtemleri Kullan"));
            ComboMenu.AddSeparator();
            ComboMenu.Add("rslider", new Slider("R için gerekli kişi sayısı", 1, 0, 5));
            ComboMenu.Add("waitAA", new CheckBox("AA tamamlanmasını bekle", false));

            HarassMenu = SonaMenu.AddSubMenu("Dürtme", "Harass");
            HarassMenu.Add("useQHarass", new CheckBox("Q Kullan"));
            HarassMenu.Add("waitAA", new CheckBox("AA tamamlanmasını bekle", false));

            LaneJungleClear = SonaMenu.AddSubMenu("Lane Jungle Clear", "lanejungleclear");
            LaneJungleClear.AddGroupLabel("Lane Jungle Clear Ayarları");
            LaneJungleClear.Add("LCQ", new CheckBox("Q Kullan"));

            DrawMenu = SonaMenu.AddSubMenu("Gösterge", "drawings");
            DrawMenu.AddGroupLabel("Gösterge");
            DrawMenu.AddSeparator();
            DrawMenu.Add("drawq", new CheckBox("Göster Q"));
            DrawMenu.Add("draww", new CheckBox("Göster W"));
            DrawMenu.Add("drawe", new CheckBox("Göster E"));
            DrawMenu.Add("drawr", new CheckBox("Göster R"));
            DrawMenu.Add("drawaa", new CheckBox("Göster AA"));

            MiscMenu = SonaMenu.AddSubMenu("Misc Menu", "miscmenu");
            MiscMenu.AddGroupLabel("Ek");
            MiscMenu.AddSeparator();
            MiscMenu.Add("ks", new CheckBox("KS"));
            MiscMenu.Add("int", new CheckBox("Interrupt Büyüleri"));
            MiscMenu.Add("support", new CheckBox("Destek Modu", false));
            MiscMenu.Add("HPLowAllies", new CheckBox("W kullan % Dostların canı şu kadar", false));
            MiscMenu.Add("wslider", new Slider("W kullanmak için dostların canları şundan az", 60));
            MiscMenu.Add("useexhaust", new CheckBox("Bitkinlik Kullan"));
            foreach (var source in ObjectManager.Get<AIHeroClient>().Where(a => a.IsEnemy))
            {
                MiscMenu.Add(source.ChampionName + "exhaust",
                    new CheckBox("Bitkinlik " + source.ChampionName, false));
            }

            FleeMenu = SonaMenu.AddSubMenu("Flee", "Flee");
            FleeMenu.Add("fleee", new CheckBox("Kullan E"));

            SkinMenu = SonaMenu.AddSubMenu("Skin Changer", "skin");
            SkinMenu.AddGroupLabel("İstediğiniz Görünümü Seçin");

            var skinchange = SkinMenu.Add("sID", new Slider("Skin", 3, 0, 5));
            var sid = new[] {"Default", "Muse", "Pentakill", "Silent Night", "Guqin", "Arcade"};
            skinchange.DisplayName = sid[skinchange.CurrentValue];
            skinchange.OnValueChange +=
                delegate(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs changeArgs)
                {
                    sender.DisplayName = sid[changeArgs.NewValue];
                };
            Game.OnUpdate += Game_OnTick;
            Interrupter.OnInterruptableSpell += Interruptererer;
            Orbwalker.OnPreAttack += Orbwalker_OnPreAttack;
            Game.OnWndProc += Game_OnWndProc;
            Drawing.OnDraw += OnDraw;
        }

        private static void Game_OnTick(EventArgs args)
        {
            Killsteal();
            SkinChange();
            AutoW();
            Passive();
            var target = TargetSelector.GetTarget(1200, DamageType.Magical);
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                Combo();
            if (target != null)
            {
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
                    Harass();
                else if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
                    LaneJungleClearA.LaneClear();
                else if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
                {
                }
                else if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
                    Flee();
                {
                            if (!MiscMenu["useexhaust"].Cast<CheckBox>().CurrentValue ||
                                ComboMenu["comboOnlyExhaust"].Cast<CheckBox>().CurrentValue &&
                                !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                                return;
                            foreach (
                                var enemy in
                                    ObjectManager.Get<AIHeroClient>()
                                        .Where(a => a.IsEnemy && a.IsValidTarget(Exhaust.Range))
                                        .Where(
                                            enemy =>
                                                MiscMenu[enemy.ChampionName + "exhaust"].Cast<CheckBox>().CurrentValue))
                            {
                                if (enemy.IsFacing(Sona))
                                {
                                    if (!(Sona.HealthPercent < 50)) continue;
                                    Exhaust.Cast(enemy);
                                    return;
                                }
                                if (!(enemy.HealthPercent < 50)) continue;
                                Exhaust.Cast(enemy);
                                return;
                            } } }
                        }
                    

        private static
            void Interruptererer
            (Obj_AI_Base sender,
                Interrupter.InterruptableSpellEventArgs args)
        {
            var intTarget = TargetSelector.GetTarget(R.Range, DamageType.Magical);
            {
                if (R.IsReady() && sender.IsValidTarget(R.Range) &&
                    MiscMenu["int"].Cast<CheckBox>().CurrentValue)
                    R.Cast(intTarget.ServerPosition);
            }
        }

        private static void OnDraw(EventArgs args)
        {
            if (!Sona.IsDead)
            {
                if (DrawMenu["drawq"].Cast<CheckBox>().CurrentValue && Q.IsLearned)
                {
                    Circle.Draw(Color.Blue, Q.Range, Player.Instance.Position);
                }
                {
                    if (DrawMenu["drawe"].Cast<CheckBox>().CurrentValue && E.IsLearned)
                    {
                        Circle.Draw(Color.MediumVioletRed, E.Range, Player.Instance.Position);
                    }
                    if (DrawMenu["draww"].Cast<CheckBox>().CurrentValue && W.IsLearned)
                    {
                        Circle.Draw(Color.Green, W.Range, Player.Instance.Position);
                    }
                    if (DrawMenu["drawr"].Cast<CheckBox>().CurrentValue && W.IsLearned)
                    {
                        Circle.Draw(Color.Yellow, R.Range, Player.Instance.Position);
                    }
                    if (DrawMenu["drawaa"].Cast<CheckBox>().CurrentValue)
                    {
                        Circle.Draw(Color.DimGray, 550, Player.Instance.Position);
                    }
                }
            }
        }

        private static void Orbwalker_OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) ||
                Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit) ||
                (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass) ||
                 Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)))
            {
                var t = target as Obj_AI_Minion;
                if (t != null)
                {
                    {
                        if (MiscMenu["support"].Cast<CheckBox>().CurrentValue)
                            args.Process = false;
                    }
                }
            }
        }

        public static
            void Harass
            ()
        {
            var target = TargetSelector.GetTarget(650, DamageType.Magical);
            Orbwalker.OrbwalkTo(MousePos);
            if (HarassMenu["useQHarass"].Cast<CheckBox>().CurrentValue && Q.IsReady())
            {
                if (Q.IsInRange(target))
                {
                    Q.Cast();
                }
            }
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

        public static
            void Combo
            ()
        {
            var target = TargetSelector.GetTarget(1000, DamageType.Magical);
            if (target == null || !target.IsValid())
            {
                return;
            }

            if (Orbwalker.IsAutoAttacking && ComboMenu["waitAA"].Cast<CheckBox>().CurrentValue)
                return;
            if (ComboMenu["usecomboq"].Cast<CheckBox>().CurrentValue)
                if (Q.IsReady() && Sona.CountEnemiesInRange(Q.Range) >= 1)
                {
                    Q.Cast();
                }
            if (ComboMenu["usecombor"].Cast<CheckBox>().CurrentValue)
                if (R.IsReady())
                {
                    var predR = R.GetPrediction(target).CastPosition;
                    if (target.CountEnemiesInRange(R.Width) >= ComboMenu["rslider"].Cast<Slider>().CurrentValue)
                        R.Cast(predR);
                }
            if (ComboMenu["useitems"].Cast<CheckBox>().CurrentValue)
            {
                if (FrostQueen.IsOwned() && FrostQueen.IsReady())
                    FrostQueen.Cast();
            }
        }

        public static void UseESmart(Obj_AI_Base target)
        {
            try
            {
                if (target.Path.Length == 0 || !target.IsMoving)
                    return;
                var nextEnemPath = target.Path[0].To2D();
                var dist = Sona.Position.To2D().Distance(target.Position.To2D());
                var distToNext = nextEnemPath.Distance(Sona.Position.To2D());
                if (distToNext <= dist)
                    return;
                var msDif = Sona.MoveSpeed - target.MoveSpeed;
                if (msDif <= 0 && !target.IsInAutoAttackRange(target))
                    E.Cast();

                var reachIn = dist/msDif;
                if (reachIn > 4)
                    E.Cast();
            }
            catch
            {
            }
        }

        private static int GetPassiveCount()
        {
            foreach (var buff in Sona.Buffs)
                if (buff.Name == "sonapassivecount") return buff.Count;
            return 0;
        }

        private static void Passive()
        {
            var unit = TargetSelector.GetTarget(550, DamageType.Magical);
            if ((Q.IsReady() && GetPassiveCount() == 2 && unit.Distance(Sona) <= 550))
            {
                if (Q.IsReady()) Q.Cast();
                Player.IssueOrder(GameObjectOrder.AttackUnit, unit);
            }
        }

        private static void AutoW()
        {
            var healAllies = MiscMenu["HPLowAllies"].Cast<CheckBox>().CurrentValue;
            var healHealthPercent = MiscMenu["wslider"].Cast<Slider>().CurrentValue;

            if (healAllies)
            {
                var ally =
                    EntityManager.Heroes.Allies.Where(
                        x => x.IsValidTarget(W.Range) && x.HealthPercent < healHealthPercent)
                        .FirstOrDefault();

                if (ally != null)
                {
                    W.Cast();
                }
            }
        }

        private static
            void Killsteal
            ()
        {
            if (MiscMenu["ks"].Cast<CheckBox>().CurrentValue && Q.IsReady())
            {
                try
                {
                    foreach (
                        var qtarget in
                            EntityManager.Heroes.Enemies.Where(
                                hero => hero.IsValidTarget(Q.Range) && !hero.IsDead && !hero.IsZombie))
                    {
                        if (Sona.GetSpellDamage(qtarget, SpellSlot.Q) >= qtarget.Health)
                        {
                            {
                                if (Q.IsInRange(qtarget) && (Q.IsReady() || GetPassiveCount() == 2))
                                {
                                    Q.Cast();
                                    Player.IssueOrder(GameObjectOrder.AttackUnit, qtarget);
                                }
                            }
                            {
                            }
                        }
                    }
                }
                catch
                {
                }
            }
        }

        public static
            void Flee
            ()
        {
            if (FleeMenu["fleee"].Cast<CheckBox>().CurrentValue)
            {
                E.Cast();
                Orbwalker.MoveTo(Game.CursorPos);
            }
        }

        private static void SkinChange()
        {
            var style = SkinMenu["sID"].DisplayName;
            switch (style)
            {
                case "Default":
                    Player.SetSkinId(0);
                    break;
                case "Muse":
                    Player.SetSkinId(1);
                    break;
                case "Pentakill":
                    Player.SetSkinId(2);
                    break;
                case "Silent Night":
                    Player.SetSkinId(3);
                    break;
                case "Guqin":
                    Player.SetSkinId(4);
                    break;
                case "Arcade":
                    Player.SetSkinId(5);
                    break;
            }
        }
    }
}
