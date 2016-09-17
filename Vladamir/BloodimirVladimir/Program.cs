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

namespace BloodimirVladimir
{
    internal class Program
    {
        public static Spell.Active W, E;
        public static Spell.Skillshot R, Flash;
        public static Spell.Targeted Ignite, Q;
        public static Item Zhonia;
        public static Menu VladMenu, ComboMenu, DrawMenu, SkinMenu, MiscMenu, LaneClear, HarassMenu, LastHit;
        public static AIHeroClient Vlad = ObjectManager.Player;

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
            if (Player.Instance.ChampionName != "Vladimir")
                return;
            Bootstrap.Init(null);
            Q = new Spell.Targeted(SpellSlot.Q, 600);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Active(SpellSlot.E, 610);
            R = new Spell.Skillshot(SpellSlot.R, 700, SkillShotType.Circular, 250, 1200, 150);
            if (HasSpell("summonerdot"))
                Ignite = new Spell.Targeted(ObjectManager.Player.GetSpellSlotFromName("summonerdot"), 600);
            Zhonia = new Item((int) ItemId.Zhonyas_Hourglass);
            var flashSlot = Vlad.GetSpellSlotFromName("summonerflash");
            Flash = new Spell.Skillshot(flashSlot, 32767, SkillShotType.Linear);

            VladMenu = MainMenu.AddMenu("Bloodimir", "bloodimir");
            VladMenu.AddGroupLabel("Bloodimir.Bloodimir");
            VladMenu.AddSeparator();
            VladMenu.AddLabel("Bloodimir c what i did there? version 1.0.5.2");

            ComboMenu = VladMenu.AddSubMenu("Combo", "sbtw");
            ComboMenu.AddGroupLabel("Kombo Ayarları");
            ComboMenu.AddSeparator();
            ComboMenu.Add("usecomboq", new CheckBox("Q Kullan"));
            ComboMenu.Add("usecomboe", new CheckBox("E Kullan"));
            ComboMenu.Add("usecombor", new CheckBox("R Kullan"));
            ComboMenu.Add("useignite", new CheckBox("Tutuştur Kullan"));
            ComboMenu.AddSeparator();
            ComboMenu.Add("rslider", new Slider("R için gerekli kişi sayısı", 2, 0, 5));
            DrawMenu = VladMenu.AddSubMenu("Gösterge", "drawings");
            DrawMenu.AddGroupLabel("Gösterge");
            DrawMenu.AddSeparator();
            DrawMenu.Add("drawq", new CheckBox("Göster Q Menzil"));
            DrawMenu.Add("drawe", new CheckBox("Göster E Menzil"));
            DrawMenu.Add("drawr", new CheckBox("Göster R Menzil"));
            DrawMenu.Add("drawaa", new CheckBox("Göster AA Menzil"));

            LaneClear = VladMenu.AddSubMenu("Lane Clear", "laneclear");
            LaneClear.AddGroupLabel("Lane Clear Ayarları");
            LaneClear.Add("LCE", new CheckBox("E Kullan"));
            LaneClear.Add("LCQ", new CheckBox("Q Kullan"));

            LastHit = VladMenu.AddSubMenu("Last Hit", "lasthit");
            LastHit.AddGroupLabel("Son VURUŞ Ayarları");
            LastHit.Add("LHQ", new CheckBox("Kullan Q"));

            HarassMenu = VladMenu.AddSubMenu("Harass Menu", "harass");
            HarassMenu.AddGroupLabel("Dürtme Settings");
            HarassMenu.Add("hq", new CheckBox("Dürtme Q"));
            HarassMenu.Add("he", new CheckBox("Dürtme E"));
            HarassMenu.Add("autoh", new CheckBox("Otomatik Dürtme"));
            HarassMenu.Add("autohq", new CheckBox("Dürtmede otomatik Q Kullan"));
            HarassMenu.Add("autohe", new CheckBox("Dürtmede otomatik E Kullan"));


            MiscMenu = VladMenu.AddSubMenu("Misc Menu", "miscmenu");
            MiscMenu.AddGroupLabel("Ek");
            MiscMenu.AddSeparator();
            MiscMenu.Add("ksq", new CheckBox("KS için Q"));
            MiscMenu.Add("kse", new CheckBox("KS için E"));
            MiscMenu.Add("zhonias", new CheckBox("Zhonya Kullan"));
            MiscMenu.Add("zhealth", new Slider("Canım Şundan az ise Zhonya bas %", 8));
            MiscMenu.AddSeparator();
            MiscMenu.Add("gapcloserw", new CheckBox("Anti Gapcloser W"));
            MiscMenu.Add("gapcloserhp", new Slider("Gapcloser W Health %", 25));
            MiscMenu.AddSeparator();

            SkinMenu = VladMenu.AddSubMenu("Skin Changer", "skin");
            SkinMenu.AddGroupLabel("İstediğin Görünümü Seç");

            var skinchange = SkinMenu.Add("sID", new Slider("Skin", 5, 0, 7));
            var sid = new[]
            {"Default", "Count", "Marquius", "Nosferatu", "Vandal", "Blood Lord", "Soulstealer", "Academy"};
            skinchange.DisplayName = sid[skinchange.CurrentValue];
            skinchange.OnValueChange +=
                delegate(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs changeArgs)
                {
                    sender.DisplayName = sid[changeArgs.NewValue];
                };

            Game.OnUpdate += Tick;
            Drawing.OnDraw += OnDraw;
            Gapcloser.OnGapcloser += Gapcloser_OnGapCloser;
        }

        private static void OnDraw(EventArgs args)
        {
            if (!Vlad.IsDead)
            {
                if (DrawMenu["drawq"].Cast<CheckBox>().CurrentValue && Q.IsLearned)
                {
                    Circle.Draw(Color.Red, Q.Range, Player.Instance.Position);
                }
                {
                    if (DrawMenu["drawe"].Cast<CheckBox>().CurrentValue && E.IsLearned)
                    {
                        Circle.Draw(Color.DarkGreen, E.Range, Player.Instance.Position);
                    }
                    if (DrawMenu["drawr"].Cast<CheckBox>().CurrentValue && R.IsLearned)
                    {
                        Circle.Draw(Color.DarkMagenta, R.Range, Player.Instance.Position);
                    }
                }
                if (DrawMenu["drawaa"].Cast<CheckBox>().CurrentValue)
                {
                    Circle.Draw(Color.DarkCyan, 518, Player.Instance.Position);
                }
            }
        }

        private static
            void Gapcloser_OnGapCloser
            (AIHeroClient sender, Gapcloser.GapcloserEventArgs gapcloser)
        {
            if (!gapcloser.Sender.IsEnemy || (!MiscMenu["gapcloserw"].Cast<CheckBox>().CurrentValue))
                return;
            if (ObjectManager.Player.Distance(gapcloser.Sender, true) <
                W.Range && sender.IsValidTarget() && W.IsReady() &&
                Vlad.HealthPercent <= MiscMenu["gapcloserhp"].Cast<Slider>().CurrentValue)
            {
                W.Cast();
            }
        }

        public static void Flee()
        {
            Orbwalker.MoveTo(Game.CursorPos);
            W.Cast();
        }

        private static void Zhonya()
        {
            var zhoniaon = MiscMenu["zhonias"].Cast<CheckBox>().CurrentValue;
            var zhealth = MiscMenu["zhealth"].Cast<Slider>().CurrentValue;

            if (zhoniaon && Zhonia.IsReady() && Zhonia.IsOwned())
            {
                if (Vlad.HealthPercent <= zhealth)
                {
                    Zhonia.Cast();
                }
            }
        }

        private static void Tick(EventArgs args)
        {
            Killsteal();
            SkinChange();
            Zhonya();
            if (HarassMenu["autoh"].Cast<CheckBox>().CurrentValue) AutoHarass();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                Flee();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Combo.VladCombo();
                Rincombo(ComboMenu["usecombor"].Cast<CheckBox>().CurrentValue);
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) ||
                Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                LaneClearA.LaneClear();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
                Harass();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                LastHitA.LastHitB();
            }
            else if (!ComboMenu["useignite"].Cast<CheckBox>().CurrentValue ||
                     !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) return;
            foreach (
                var source in
                    ObjectManager.Get<AIHeroClient>()
                        .Where(
                            a =>
                                a.IsEnemy && a.IsValidTarget(Ignite.Range) &&
                                a.Health < 50 + 20*Vlad.Level - (a.HPRegenRate/5*3)))
            {
                Ignite.Cast(source);
                return;
            }
        }

        public static Obj_AI_Base GetEnemy(float range, GameObjectType t)
        {
            switch (t)
            {
                case GameObjectType.AIHeroClient:
                    return EntityManager.Heroes.Enemies.OrderBy(a => a.Health).FirstOrDefault(
                        a => a.Distance(Player.Instance) < range && !a.IsDead && !a.IsInvulnerable);
                default:
                    return EntityManager.MinionsAndMonsters.EnemyMinions.OrderBy(a => a.Health).FirstOrDefault(
                        a => a.Distance(Player.Instance) < range && !a.IsDead && !a.IsInvulnerable);
            }
        }

        public static void Harass()
        {
            Orbwalker.OrbwalkTo(MousePos);
            if (HarassMenu["he"].Cast<CheckBox>().CurrentValue)
            {
                var enemy = TargetSelector.GetTarget(E.Range, DamageType.Magical);

                if (enemy != null)
                    E.Cast();
            }
            if (HarassMenu["hq"].Cast<CheckBox>().CurrentValue)
            {
                var enemy = (AIHeroClient) GetEnemy(Q.Range, GameObjectType.AIHeroClient);

                if (enemy != null)
                    Q.Cast(enemy);
            }
        }

        public static void AutoHarass()
        {
            if (HarassMenu["autohe"].Cast<CheckBox>().CurrentValue)
            {
                var enemy = TargetSelector.GetTarget(E.Range, DamageType.Magical);

                if (enemy != null)
                    E.Cast();
            }
            if (HarassMenu["autohq"].Cast<CheckBox>().CurrentValue)
            {
                var enemy = (AIHeroClient) GetEnemy(Q.Range, GameObjectType.AIHeroClient);

                if (enemy != null)
                    Q.Cast(enemy);
            }
        }

        public static void Rincombo(bool useR)
        {
            foreach (
                var qtarget in
                    EntityManager.Heroes.Enemies.Where(
                        hero => hero.IsValidTarget(Q.Range) && !hero.IsDead && !hero.IsZombie))
            {
                if (Vlad.GetSpellDamage(qtarget, SpellSlot.Q) >= qtarget.Health ||
                    (Vlad.GetSpellDamage(qtarget, SpellSlot.E) >= qtarget.Health))
                    return;
                  var rtarget = TargetSelector.GetTarget(1250, DamageType.Magical);
                if (ComboMenu["usecombor"].Cast<CheckBox>().CurrentValue)
                    if (useR && R.IsReady() &&
                        rtarget.CountEnemiesInRange(R.Width) >= ComboMenu["rslider"].Cast<Slider>().CurrentValue)
                    {

                        R.Cast(rtarget.ServerPosition);
                    }
            }
        }

        private static void Killsteal()
        {
            var enemy = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            if (MiscMenu["ksq"].Cast<CheckBox>().CurrentValue && Q.IsReady())
            {
                foreach (
                    var qtarget in
                        EntityManager.Heroes.Enemies.Where(
                            hero => hero.IsValidTarget(Q.Range) && !hero.IsDead && !hero.IsZombie))
                {
                    if (Vlad.GetSpellDamage(qtarget, SpellSlot.Q) >= qtarget.Health && qtarget.Distance(Vlad) <= Q.Range)
                    {
                        Q.Cast(enemy);
                    }
                    if (MiscMenu["kse"].Cast<CheckBox>().CurrentValue && Q.IsReady())
                    {
                        foreach (
                            var etarget in
                                EntityManager.Heroes.Enemies.Where(
                                    hero => hero.IsValidTarget(E.Range) && !hero.IsDead && !hero.IsZombie))
                        {
                            if (Vlad.GetSpellDamage(etarget, SpellSlot.E) >= etarget.Health &&
                                etarget.Distance(Vlad) <= E.Range)
                            {
                                E.Cast();
                            }
                        }
                    }
                    {
                    }
                }
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
                case "Count":
                    Player.SetSkinId(1);
                    break;
                case "Marquius":
                    Player.SetSkinId(2);
                    break;
                case "Nosferatu":
                    Player.SetSkinId(3);
                    break;
                case "Vandal":
                    Player.SetSkinId(4);
                    break;
                case "Blood Lord":
                    Player.SetSkinId(5);
                    break;
                case "Soulstealer":
                    Player.SetSkinId(6);
                    break;
                case "Academy":
                    Player.SetSkinId(7);
                    break;
            }
        }
    }
}