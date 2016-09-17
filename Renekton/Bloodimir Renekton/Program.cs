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

namespace Bloodimir_Renekton
{
    internal static class Program
    {
        private const string E2BuffName = "renektonsliceanddicedelay";
        public static Spell.Active Q;
        public static Spell.Active W;
        public static Spell.Skillshot E;
        private static Spell.Active _r;
        private static Spell.Targeted _ignite;
        public static Item Hydra;
        public static Item Tiamat;
        private static Menu _renekMenu;
        public static Menu ComboMenu;
        private static Menu _skinMenu;
        private static Menu _miscMenu;
        private static Menu _drawMenu;
        private static Menu _harassMenu;
        public static Menu LaneJungleClear, LastHit;
        public static Item Bilgewater, Youmuu, Botrk;
        private static readonly AIHeroClient Renek = ObjectManager.Player;
        private static int[] _abilitySequence;
        public static int QOff = 0, WOff = 0, EOff = 0, ROff = 0;

        private static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoaded;
        }

        private static bool HasSpell(string s)
        {
            return Player.Spells.FirstOrDefault(o => o.SData.Name.Contains(s)) != null;
        }

        private static void OnLoaded(EventArgs args)
        {
            if (Player.Instance.ChampionName != "Renekton")
                return;
            Bootstrap.Init(null);
            Q = new Spell.Active(SpellSlot.Q, 225);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Skillshot(SpellSlot.E, 450, SkillShotType.Linear);
            _r = new Spell.Active(SpellSlot.R);
            Tiamat = new Item((int) ItemId.Tiamat_Melee_Only, 420);
            Hydra = new Item((int) ItemId.Ravenous_Hydra_Melee_Only, 420);
            Botrk = new Item(3153, 550f);
            Bilgewater = new Item(3144, 475f);
            Youmuu = new Item(3142, 10);
            _abilitySequence = new[] {2, 1, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2};

            _renekMenu = MainMenu.AddMenu("BloodimiRenekton", "bloodimirrenekton");
            _renekMenu.AddGroupLabel("Bloodimir.enekton");
            _renekMenu.AddSeparator();
            _renekMenu.AddLabel("BloodimiRenekton v1.0.1.0");

            ComboMenu = _renekMenu.AddSubMenu("Combo", "sbtw");
            ComboMenu.AddGroupLabel("Kombo Ayarları");
            ComboMenu.AddSeparator();
            ComboMenu.Add("usecomboq", new CheckBox("Q Kullan"));
            ComboMenu.Add("usecombow", new CheckBox("W Kullan"));
            ComboMenu.Add("usecomboe", new CheckBox("E Kullan"));
            ComboMenu.Add("useitems", new CheckBox("İtemleri Kullan"));
            ComboMenu.Add("autoult", new CheckBox("Otomatik Ulti"));
            ComboMenu.AddSeparator();
            ComboMenu.Add("rslider", new Slider("Ulti için gereken canım", 31));

            LaneJungleClear = _renekMenu.AddSubMenu("Lane Jungle Clear", "lanejungleclear");
            LaneJungleClear.AddGroupLabel("Lane Jungle Clear Ayarları");
            LaneJungleClear.Add("LCE", new CheckBox("E Kullan"));
            LaneJungleClear.Add("LCQ", new CheckBox("Q Kullan"));
            LaneJungleClear.Add("LCW", new CheckBox("W Kullan"));
            LaneJungleClear.Add("LCI", new CheckBox("İtemleri Kullan"));

            _drawMenu = _renekMenu.AddSubMenu("Drawings", "drawings");
            _drawMenu.AddGroupLabel("Gösterge");
            _drawMenu.AddSeparator();
            _drawMenu.Add("drawq", new CheckBox("Göster Q"));
            _drawMenu.Add("drawe", new CheckBox("Göster E"));

            LastHit = _renekMenu.AddSubMenu("Last Hit", "lasthit");
            LastHit.AddGroupLabel("Son Vuruş Ayarları");
            LastHit.Add("LHQ", new CheckBox("Q Kullan"));
            LastHit.Add("LHW", new CheckBox("W Kullan"));
            LastHit.Add("LHI", new CheckBox("İtemleri Kullan"));

            _harassMenu = _renekMenu.AddSubMenu("Harass Menu", "harass");
            _harassMenu.AddGroupLabel("Dürtme Ayarları");
            _harassMenu.Add("hq", new CheckBox("Dürt Q"));
            _harassMenu.Add("hw", new CheckBox("Dürt W"));
            _harassMenu.Add("hi", new CheckBox("İtemlerle Dürtme"));

            _miscMenu = _renekMenu.AddSubMenu("Misc Menu", "miscmenu");
            _miscMenu.AddGroupLabel("KS");
            _miscMenu.AddSeparator();
            _miscMenu.Add("ksq", new CheckBox("KS için Q kullan"));
            _miscMenu.AddSeparator();
            _miscMenu.Add("intw", new CheckBox("W to Interrupt"));
            _miscMenu.AddSeparator();
            _miscMenu.Add("gapclose", new CheckBox("W to Interrupt"));
            _miscMenu.Add("lvlup", new CheckBox("Büyüleri Otomatik Ver", false));

            _skinMenu = _renekMenu.AddSubMenu("Skin Changer", "skin");
            _skinMenu.AddGroupLabel("İstediğin Görünümü Seç");

            var skinchange = _skinMenu.Add("sid", new Slider("Skin", 5, 0, 7));
            var sid = new[]
            {"Classic", "Galactic", "Outback", "Bloodfury", "Rune Wars", "Scorched Earth", "Pool Party", "Prehistoric"};
            skinchange.DisplayName = sid[skinchange.CurrentValue];
            skinchange.OnValueChange +=
                delegate(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs changeArgs)
                {
                    sender.DisplayName = sid[changeArgs.NewValue];
                };

            Game.OnUpdate += Tick;
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
            Drawing.OnDraw += OnDraw;
            Orbwalker.OnPostAttack += Orbwalker_OnPostAttack;
            Gapcloser.OnGapcloser += OnGapClose;
        }

        private static void Flee()
        {
            Orbwalker.MoveTo(Game.CursorPos);
            E.Cast(Game.CursorPos);
        }

        private static void LevelUpSpells()
        {
            var qL = Renek.Spellbook.GetSpell(SpellSlot.Q).Level + QOff;
            var wL = Renek.Spellbook.GetSpell(SpellSlot.W).Level + WOff;
            var eL = Renek.Spellbook.GetSpell(SpellSlot.E).Level + EOff;
            var rL = Renek.Spellbook.GetSpell(SpellSlot.R).Level + ROff;
            if (qL + wL + eL + rL >= ObjectManager.Player.Level) return;
            int[] level = {0, 0, 0, 0};
            for (var i = 0; i < ObjectManager.Player.Level; i++)
            {
                level[_abilitySequence[i] - 1] = level[_abilitySequence[i] - 1] + 1;
            }
            if (qL < level[0]) ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.Q);
            if (wL < level[1]) ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.W);
            if (eL < level[2]) ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.E);
            if (rL < level[3]) ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.R);
        }

        private static
            void OnGapClose
            (AIHeroClient sender, Gapcloser.GapcloserEventArgs gapcloser)
        {
            if (!gapcloser.Sender.IsEnemy)
                return;
            var gapclose = _miscMenu["gapclose"].Cast<CheckBox>().CurrentValue;
            if (!gapclose)
                return;

            if (W.IsReady() && ObjectManager.Player.Distance(gapcloser.Sender, true) <
                Player.Instance.GetAutoAttackRange() && sender.IsValidTarget())
            {
                W.Cast(gapcloser.Sender);
            }
        }

        private static void OnDraw(EventArgs args)
        {
            if (Renek.IsDead) return;
            if (_drawMenu["drawq"].Cast<CheckBox>().CurrentValue && Q.IsLearned)
            {
                Circle.Draw(Color.Red, Q.Range, Player.Instance.Position);
            }
            if (_drawMenu["drawe"].Cast<CheckBox>().CurrentValue && E.IsLearned)
            {
                Circle.Draw(Color.DarkCyan, E.Range, Player.Instance.Position);
            }
        }

        private static void Tick(EventArgs args)
        {
            Killsteal();
            SkinChange();
            if (_miscMenu["lvlup"].Cast<CheckBox>().CurrentValue) LevelUpSpells();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                Flee();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Combo.RenekCombo();
                Combo.Items();
                Combo.UseE();
                AutoUlt(ComboMenu["autoult"].Cast<CheckBox>().CurrentValue);
            }
            {
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) ||
                    Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
                {
                    LaneJungleClearA.LaneClear();
                    LaneJungleClearA.Items();
                }
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
                {
                    Harass();
                }
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
                {
                    LastHitA.LastHitB();
                    LastHitA.Items();
                }
            }
        }

        private static void AutoUlt(bool useR)
        {
            var autoR = ComboMenu["autoult"].Cast<CheckBox>().CurrentValue;
            var healthAutoR = ComboMenu["rslider"].Cast<Slider>().CurrentValue;
            if (autoR && _Player.HealthPercent < healthAutoR)
            {
                _r.Cast();
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

        private static void Harass()
        {
            var target = TargetSelector.GetTarget(E.Range, DamageType.Physical);
            if (target != null && Player.Instance.Distance(target.Position) < E.Range && !Renek.HasBuff(E2BuffName) &&
                E.IsReady())
            {
                Player.CastSpell(SpellSlot.E, target.Position);
            }
            if (!Renek.HasBuff(E2BuffName)) return;
            var qtarget = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            if (qtarget.Distance(Renek.Position) <= 225 && Q.IsReady() &&
                _harassMenu["hq"].Cast<CheckBox>().CurrentValue)
                Q.Cast();
            var wtarget = TargetSelector.GetTarget(W.Range, DamageType.Physical);
            if (wtarget.Distance(Renek.Position) <= W.Range && _harassMenu["hw"].Cast<CheckBox>().CurrentValue)
                W.Cast();
            var itarget = TargetSelector.GetTarget(Hydra.Range, DamageType.Physical);
            if (itarget.Distance(Renek.Position) <= Hydra.Range && _harassMenu["hi"].Cast<CheckBox>().CurrentValue)
                Hydra.Cast();
            if (itarget.Distance(Renek.Position) <= Tiamat.Range && _harassMenu["hi"].Cast<CheckBox>().CurrentValue)
                Tiamat.Cast();
            if (!Renek.HasBuff(E2BuffName) || !E.IsReady()) return;
            if (!W.IsReady() || !Q.IsReady())
            {
                Player.CastSpell(SpellSlot.E, qtarget.Position);
            }
        }

        private static void Orbwalker_OnPostAttack(AttackableUnit target, EventArgs args)
        {
            if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) &&
                (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) &&
                 (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit)))) return;
            var e = target as AIHeroClient;
            if (!ComboMenu["usecombow"].Cast<CheckBox>().CurrentValue || !W.IsReady() || !e.IsEnemy) return;
            if (target == null) return;
            if (e.IsValidTarget() && W.IsReady())
            {
                W.Cast();
            }
            if (e.IsValidTarget() && Hydra.IsReady())
            {
                Hydra.Cast();
            }
            if (e.IsValidTarget() && Tiamat.IsReady())
            {
                Tiamat.Cast();
            }
        }

        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender,
            Interrupter.InterruptableSpellEventArgs args)
        {
            {
                if (W.IsReady() && sender.IsValidTarget(Player.Instance.GetAutoAttackRange()) &&
                    _miscMenu["intw"].Cast<CheckBox>().CurrentValue)
                    W.Cast(sender);
            }
        }

        private static void Killsteal()
        {
            if (!_miscMenu["ksq"].Cast<CheckBox>().CurrentValue || !Q.IsReady()) return;
            foreach (var qtarget in EntityManager.Heroes.Enemies.Where(
                hero => hero.IsValidTarget(Q.Range) && !hero.IsDead && !hero.IsZombie)
                .Where(qtarget => Renek.GetSpellDamage(qtarget, SpellSlot.Q) >= qtarget.Health))
            {
                Q.Cast();
            }
        }

        private static void SkinChange()
        {
            var style = _skinMenu["sid"].DisplayName;
            switch (style)
            {
                case "Classic":
                    Player.SetSkinId(0);
                    break;
                case "Galactic":
                    Player.SetSkinId(1);
                    break;
                case "Outback":
                    Player.SetSkinId(2);
                    break;
                case "Bloodfury":
                    Player.SetSkinId(3);
                    break;
                case "Rune Wars":
                    Player.SetSkinId(4);
                    break;
                case "Scorched Earth":
                    Player.SetSkinId(5);
                    break;
                case "Pool Party":
                    Player.SetSkinId(6);
                    break;
                case "Prehistoric":
                    Player.SetSkinId(7);
                    break;
            }
        }
    }
}
