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

namespace Evelynn
{
    internal static class Program
    {
        public static Spell.Active Q, W;
        public static Spell.Targeted E;
        private static Spell.Skillshot _r;
        private static Menu _eveMenu;
        public static Menu ComboMenu;
        private static Menu _drawMenu;
        private static Menu _skinMenu;
        private static Menu _miscMenu;
        public static Menu LaneJungleClear, LastHitMenu;
        private static readonly AIHeroClient Eve = ObjectManager.Player;

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoaded;
        }

        private static void OnLoaded(EventArgs args)
        {
            if (Player.Instance.ChampionName != "Evelynn")
                return;
            Bootstrap.Init(null);
            Q = new Spell.Active(SpellSlot.Q, 475);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Targeted(SpellSlot.E, 225);
            _r = new Spell.Skillshot(SpellSlot.R, 900, SkillShotType.Circular, 250, 1200, 150);

            _eveMenu = MainMenu.AddMenu("BloodimirEve", "bloodimireve");
            _eveMenu.AddGroupLabel("Bloodimir.Evelynn");
            _eveMenu.AddSeparator();
            _eveMenu.AddLabel("Bloodimir Evelynn V1.0.1.0 - çeviri tradana");

            ComboMenu = _eveMenu.AddSubMenu("Combo", "sbtw");
            ComboMenu.AddGroupLabel("Kombo Ayarları");
            ComboMenu.AddSeparator();
            ComboMenu.Add("usecomboq", new CheckBox("Q Kullan"));
            ComboMenu.Add("usecombow", new CheckBox("W Kullan"));
            ComboMenu.Add("usecomboe", new CheckBox("E Kullan"));
            ComboMenu.Add("usecombor", new CheckBox("R Kullan"));
            ComboMenu.AddSeparator();
            ComboMenu.Add("rslider", new Slider("Minimum people for R", 1, 0, 5));

            _drawMenu = _eveMenu.AddSubMenu("Drawings", "drawings");
            _drawMenu.AddGroupLabel("Gösterge");
            _drawMenu.AddSeparator();
            _drawMenu.Add("drawq", new CheckBox("Göster Q"));
            _drawMenu.Add("drawr", new CheckBox("Göster R"));
            _drawMenu.Add("drawe", new CheckBox("Göster R"));

            LaneJungleClear = _eveMenu.AddSubMenu("Lane Jungle Clear", "lanejungleclear");
            LaneJungleClear.AddGroupLabel("Lane Jungle Clear Ayarları");
            LaneJungleClear.Add("LCE", new CheckBox("E Kullan"));
            LaneJungleClear.Add("LCQ", new CheckBox("Q Kullan"));

            LastHitMenu = _eveMenu.AddSubMenu("Last Hit", "lasthit");
            LastHitMenu.AddGroupLabel("Last Hit Ayarları");
            LastHitMenu.Add("LHQ", new CheckBox("Q Kullan"));

            _miscMenu = _eveMenu.AddSubMenu("Misc Menu", "miscmenu");
            _miscMenu.AddGroupLabel("KS");
            _miscMenu.AddSeparator();
            _miscMenu.Add("kse", new CheckBox("KS için E"));
            _miscMenu.AddSeparator();
            _miscMenu.Add("ksq", new CheckBox("KS için Q"));
            _miscMenu.Add("asw", new CheckBox("Otomatik W"));

            _skinMenu = _eveMenu.AddSubMenu("Skin Changer", "skin");
            _skinMenu.AddGroupLabel("İstediğiniz Görünümü Seçin");

            var skinchange = _skinMenu.Add("sID", new Slider("Skin", 2, 0, 4));
            var sid = new[] {"Default", "Shadow", "Masquerade", "Tango", "Safecracker"};
            skinchange.DisplayName = sid[skinchange.CurrentValue];
            skinchange.OnValueChange +=
                delegate(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs changeArgs)
                {
                    sender.DisplayName = sid[changeArgs.NewValue];
                };

            Game.OnUpdate += Tick;
            Drawing.OnDraw += OnDraw;
        }

        private static void OnDraw(EventArgs args)
        {
            if (Eve.IsDead) return;
            if (_drawMenu["drawq"].Cast<CheckBox>().CurrentValue && Q.IsLearned)
            {
                Circle.Draw(Color.DarkBlue, Q.Range, Player.Instance.Position);
            }
            if (_drawMenu["drawr"].Cast<CheckBox>().CurrentValue && _r.IsLearned)
            {
                Circle.Draw(Color.Red, _r.Range, Player.Instance.Position);
            }
            if (_drawMenu["drawe"].Cast<CheckBox>().CurrentValue && E.IsLearned)
            {
                Circle.Draw(Color.Green, E.Range, Player.Instance.Position);
            }
        }

        private static void Flee()
        {
            Orbwalker.MoveTo(Game.CursorPos);
            W.Cast();
        }

        private static void AutoW()
        {
            var useW = _miscMenu["asw"].Cast<CheckBox>().CurrentValue;

            if (Player.HasBuffOfType(BuffType.Slow) || Eve.CountEnemiesInRange(550) >= 3 && useW)
            {
                W.Cast();
            }
        }

        private static void Tick(EventArgs args)
        {
            Killsteal();
            SkinChange();
            AutoW();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                Flee();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Combo.EveCombo();
                Rincombo(ComboMenu["usecombor"].Cast<CheckBox>().CurrentValue);
            }
            {
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
                {
                    LaneJungleClearA.LaneClearB();
                }
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
                {
                    LaneJungleClearA.JungleClearB();
                }
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
                {
                    LastHitA.LastHitB();
                }
            }
        }

        private static void Rincombo(bool useR)
        {
            var rtarget = TargetSelector.GetTarget(_r.Range, DamageType.Magical);
            if (!ComboMenu["usecombor"].Cast<CheckBox>().CurrentValue) return;
            if (!useR || !_r.IsReady() ||
                rtarget.CountEnemiesInRange(_r.Width) < ComboMenu["rslider"].Cast<Slider>().CurrentValue) return;
            _r.Cast(rtarget.ServerPosition);
        }

        private static void Killsteal()
        {
            if (!_miscMenu["ksq"].Cast<CheckBox>().CurrentValue || !Q.IsReady()) return;
            try
            {
                foreach (
                    var qtarget in
                        EntityManager.Heroes.Enemies.Where(
                            hero =>
                                hero.IsValidTarget(Q.Range) && !hero.IsDead && !hero.IsZombie))
                {
                    if (Eve.GetSpellDamage(qtarget, SpellSlot.Q) >= qtarget.Health)
                    {
                        Q.Cast();
                    }
                    if (!_miscMenu["kse"].Cast<CheckBox>().CurrentValue || !E.IsReady()) continue;
                    try
                    {
                        foreach (var etarget in EntityManager.Heroes.Enemies.Where(
                            hero =>
                                hero.IsValidTarget(E.Range) && !hero.IsDead && !hero.IsZombie)
                            .Where(etarget => Eve.GetSpellDamage(etarget, SpellSlot.E) >= etarget.Health))
                        {
                            E.Cast(etarget);
                        }
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
        }

        private static void SkinChange()
        {
            var style = _skinMenu["sID"].DisplayName;
            switch (style)
            {
                case "Default":
                    Player.SetSkinId(0);
                    break;
                case "Shadow":
                    Player.SetSkinId(1);
                    break;
                case "Masquerade":
                    Player.SetSkinId(2);
                    break;
                case "Tango":
                    Player.SetSkinId(3);
                    break;
                case "Safecracker":
                    Player.SetSkinId(4);
                    break;
            }
        }
    }
}
