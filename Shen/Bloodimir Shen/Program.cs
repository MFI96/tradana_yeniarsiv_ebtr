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
using extend = EloBuddy.SDK.Extensions;

namespace Bloodimir_Shen
{
    internal static class Program
    {
        private static readonly AIHeroClient Shen = ObjectManager.Player;
        public static Spell.Targeted R, Exhaust;
        public static Spell.Active W;
        public static Spell.Skillshot E, Q;
        public static Spell.Skillshot Flash;
        private static Item _randuin;
        public static Menu ShenMenu;
        private static Menu _comboMenu;
        private static Menu _ultMenu;
        public static Menu MiscMenu;
        private static Menu _eMenu;
        public static Menu DrawMenu;
        public static Item Hydra;
        public static Item Tiamat;
        public static Item Titan;
        private static Menu _skinMenu;
        private static int[] _abilitySequence;
        public static List<Obj_AI_Turret> Turrets = new List<Obj_AI_Turret>();
        public static Vector3 ShenBlade, ShenBladeCast;
        public static float BladeCevre = 335f;
        public static int QOff = 0, WOff = 0, EOff = 0, ROff = 0;

        private static Vector3 MousePos
        {
            get { return Game.CursorPos; }
        }

        public static Vector3 PosEflash(Vector3 fetarget)
        {
            return fetarget + (Shen.Position - fetarget)/2;
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
            if (Player.Instance.ChampionName != "Shen")
                return;
            Bootstrap.Init(null);
            Tiamat = new Item((int) ItemId.Tiamat_Melee_Only, Player.Instance.GetAutoAttackRange());
            Hydra = new Item((int) ItemId.Ravenous_Hydra_Melee_Only, Player.Instance.GetAutoAttackRange());
            Titan = new Item((int) ItemId.Titanic_Hydra, Player.Instance.GetAutoAttackRange());
            Q = new Spell.Skillshot(SpellSlot.Q, 2000, SkillShotType.Linear, 500, 2500, 150);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Skillshot(SpellSlot.E, 610, SkillShotType.Linear, 500, 1600, 50);
            R = new Spell.Targeted(SpellSlot.R, 31000);
            Exhaust = new Spell.Targeted(ObjectManager.Player.GetSpellSlotFromName("summonerexhaust"), 650);
            var flashSlot = Shen.GetSpellSlotFromName("summonerflash");
            Flash = new Spell.Skillshot(flashSlot, 32767, SkillShotType.Linear);
            _randuin = new Item((int) ItemId.Randuins_Omen);
            _abilitySequence = new[] {1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2};

            ShenMenu = MainMenu.AddMenu("BloodimirShen", "bloodimirshen");
            ShenMenu.AddGroupLabel("Bloodimir Shen");
            ShenMenu.AddSeparator();
            ShenMenu.AddLabel("Bloodimir Shen Reworked v2.0.1.1");

            _comboMenu = ShenMenu.AddSubMenu("Combo", "sbtw");
            _comboMenu.AddGroupLabel("Kombo Ayarları");
            _comboMenu.AddSeparator();
            _comboMenu.Add("usecomboq", new CheckBox("Q Kullan"));
            _comboMenu.Add("autow", new CheckBox("Otomatik W"));
            _comboMenu.Add("usecomboe", new CheckBox("E Kullan"));

            _skinMenu = ShenMenu.AddSubMenu("Skin Değiştirici", "skin");
            _skinMenu.AddGroupLabel("İstediğin Görünümü Seç");

            var skinchange = _skinMenu.Add("sID", new Slider("Skin", 5, 0, 6));
            var sid = new[]
            {
                "Default", "Frozen", "Yellow Jacket", "Surgeon", "Blood Moon", "Warlord", "TPA"
            };
            skinchange.DisplayName = sid[skinchange.CurrentValue];
            skinchange.OnValueChange +=
                delegate(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs changeArgs)
                {
                    sender.DisplayName = sid[changeArgs.NewValue];
                };

            _eMenu = ShenMenu.AddSubMenu("Taunt", "etaunt");
            _eMenu.AddGroupLabel("E Ayarları");
            _eMenu.Add("eslider", new Slider("E için en az hedef", 1, 0, 5));
            _eMenu.Add("fleee", new CheckBox("Kaçarken E Kullan"));
            _eMenu.AddSeparator();
            foreach (var obj in ObjectManager.Get<AIHeroClient>().Where(obj => obj.Team != Shen.Team))
            {
                _eMenu.Add("taunt" + obj.ChampionName.ToLower(), new CheckBox("Alay et " + obj.ChampionName));
            }
            _eMenu.Add("flashe", new KeyBind("Flash E", false, KeyBind.BindTypes.HoldActive, 'Y'));
            _eMenu.Add("e", new KeyBind("E", false, KeyBind.BindTypes.HoldActive, 'E'));

            _ultMenu = ShenMenu.AddSubMenu("ULT", "ultmenu");
            _ultMenu.AddGroupLabel("ULT");
            _ultMenu.AddSeparator();
            _ultMenu.Add("autoult", new CheckBox("Otomatik Ulti Kullanma Tuşu"));
            _ultMenu.Add("rslider", new Slider("Ulti kullanmak için gereken can yüzde", 20));
            _ultMenu.AddSeparator();
            _ultMenu.Add("ult", new KeyBind("ULT", false, KeyBind.BindTypes.HoldActive, 'R'));
            _ultMenu.AddSeparator();
            foreach (var obj in ObjectManager.Get<AIHeroClient>().Where(obj => obj.Team == Shen.Team))
            {
                _ultMenu.Add("ult" + obj.ChampionName.ToLower(), new CheckBox("Ult" + obj.ChampionName));
            }

            MiscMenu = ShenMenu.AddSubMenu("Misc", "misc");
            MiscMenu.AddGroupLabel("Ek");
            MiscMenu.AddSeparator();
            MiscMenu.Add("LCQ", new CheckBox("Akıllı Lanetemizleme"));
            MiscMenu.Add("inte", new CheckBox("Interrupt Büyüleri"));
            MiscMenu.Add("TUT", new CheckBox("Kule Altında otomatik tut"));
            MiscMenu.AddSeparator();
            MiscMenu.Add("support", new CheckBox("Support Modu", false));
            MiscMenu.Add("useexhaust", new CheckBox("Bitkinlik Kullan"));
            MiscMenu.Add("randuin", new CheckBox("Kullan Randuin"));
            MiscMenu.Add("lvlup", new CheckBox("Otomatik Büyü Yükseltme", false));
            MiscMenu.AddSeparator();
            foreach (var source in ObjectManager.Get<AIHeroClient>().Where(a => a.IsEnemy))
            {
                MiscMenu.Add(source.ChampionName + "exhaust",
                    new CheckBox("Bitkinlik Kullanılacak şampiyonlar " + source.ChampionName, false));
            }


            DrawMenu = ShenMenu.AddSubMenu("Drawings", "drawings");
            DrawMenu.AddGroupLabel("Göstergeler");
            DrawMenu.AddSeparator();
            DrawMenu.Add("drawb", new CheckBox("Göster Kılıç"));
            DrawMenu.Add("drawe", new CheckBox("Göster E"));
            DrawMenu.Add("drawfe", new CheckBox("Göster FlashE"));


            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
            Game.OnUpdate += OnUpdate;
            Drawing.OnDraw += OnDraw;
            AttackableUnit.OnDamage += OnDamage;
            Orbwalker.OnPreAttack += Orbwalker_OnPreAttack;
            Orbwalker.OnPostAttack += Orbwalker_OnPostAttack;
            Obj_AI_Base.OnProcessSpellCast += Game_ProcessSpell;
            Core.DelayAction(FlashE, 1);
        }

        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender,
            Interrupter.InterruptableSpellEventArgs args)
        {
            if (E.IsReady() && sender.IsValidTarget(E.Range) && MiscMenu["inte"].Cast<CheckBox>().CurrentValue)
                E.Cast(sender);
        }

        private static void OnDraw(EventArgs args)
        {
            if (Shen.IsDead) return;
            if (DrawMenu["drawb"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(Color.Red, BladeCevre, ShenBlade);
            }
            if (DrawMenu["drawe"].Cast<CheckBox>().CurrentValue && R.IsLearned)
            {
                Circle.Draw(Color.LightBlue, E.Range, Player.Instance.Position);
            }
            if (DrawMenu["drawfe"].Cast<CheckBox>().CurrentValue && E.IsLearned && Flash.IsReady() && E.IsReady())
            {
                Circle.Draw(Color.DarkBlue, E.Range + 425, Player.Instance.Position);
            }
            {
                DrawAllyHealths();
            }
            {
                Danger();
            }
        }

        private static
            void Flee
            ()
        {
            if (!_eMenu["fleee"].Cast<CheckBox>().CurrentValue) return;
            Orbwalker.MoveTo(Game.CursorPos);
            E.Cast(MousePos);
        }

        private static void HighestAuthority()
        {
            var autoult = _ultMenu["autoult"].Cast<CheckBox>().CurrentValue;
            if (!autoult) return;
            if (Shen.CountEnemiesInRange(800) < 1 && Shen.HealthPercent >= 25)
                foreach (var ally in EntityManager.Heroes.Allies.Where(
                    x =>
                        _ultMenu["ult" + x.ChampionName].Cast<CheckBox>().CurrentValue && x.IsValidTarget(R.Range) &&
                        x.HealthPercent < 9)) if (R.IsReady() && ally.CountEnemiesInRange(650) >= 1)
                    R.Cast(ally);
        }

        private static void OnUpdate(EventArgs args)
        {
            if (_eMenu["flashe"].Cast<KeyBind>().CurrentValue)
            {
                FlashE();
            }
            SkinChange();
            if (MiscMenu["randuin"].Cast<CheckBox>().CurrentValue) RanduinU();
            HighestAuthority();
            if (MiscMenu["lvlup"].Cast<CheckBox>().CurrentValue) LevelUpSpells();
            {
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                    Combo();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                LaneClear();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                Flee();
            }
            if (_eMenu["e"].Cast<KeyBind>().CurrentValue)
            {
                Ee();
            }
            if (_ultMenu["ult"].Cast<KeyBind>().CurrentValue)
            {
                Ult();
            }
        }

        private static void Orbwalker_OnPostAttack(AttackableUnit target, EventArgs args)
        {
            if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) &&
                (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) &&
                 (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit)))) return;
            var e = target as Obj_AI_Base;
            if (target == null) return;
            if (e.IsValidTarget() && Hydra.IsReady())
            {
                Hydra.Cast();
            }
            if (e.IsValidTarget() && Tiamat.IsReady())
            {
                Tiamat.Cast();
            }
            if (e.IsValidTarget() && Titan.IsReady())
            {
                Titan.Cast();
            }
        }

        public static Geometry.Polygon GetPoly(Vector3 pos, float range, float width)
        {
            var poss = Shen.ServerPosition.Extend(pos, range);
            var direction = (poss.Normalized() - Shen.ServerPosition.To2D()).Normalized();

            var pos1 = (Shen.ServerPosition.To2D() - direction.Perpendicular()*width/2f).To3D();

            var pos2 =
                (poss.Normalized() + (poss.Normalized() - Shen.ServerPosition.To2D()).Normalized() +
                 direction.Perpendicular()*width/2f).To3D();

            var pos3 = (Shen.ServerPosition.To2D() + direction.Perpendicular()*width/2f).To3D();

            var pos4 =
                (poss.Normalized() + (poss.Normalized() - Shen.ServerPosition.To2D()).Normalized() -
                 direction.Perpendicular()*width/2f).To3D();
            var poly = new Geometry.Polygon();
            poly.Add(pos1);
            poly.Add(pos3);
            poly.Add(pos2);
            poly.Add(pos4);
            return poly;
        }

        private static void Game_ProcessSpell(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe && (args.SData.Name == "ShenQ" || args.SData.Name == "ShenR"))
            {
                ShenBladeCast = args.End;
            }
        }

        private static void HandleQ(Obj_AI_Base target)
        {
            var poly = GetPoly(ShenBlade, Shen.Distance(ShenBlade), 150);
            {
                if (((target.Distance(Shen) < 176) ||
                     (target.Distance(ShenBlade) < 450 && poly.IsInside(target.Position)) ||
                     Shen.Distance(target) < Player.Instance.GetAutoAttackRange(target)))
                {
                    Q.Cast(target);
                }
            }
        }

        private static void RanduinU()
        {
            if (!_randuin.IsReady() || !_randuin.IsOwned()) return;
            var randuin = MiscMenu["randuin"].Cast<CheckBox>().CurrentValue;
            if (randuin && Shen.HealthPercent <= 15 && Shen.CountEnemiesInRange(_randuin.Range) >= 1 ||
                Shen.CountEnemiesInRange(_randuin.Range) >= 2)
            {
                _randuin.Cast();
            }
        }

        private static void LevelUpSpells()
        {
            var qL = Shen.Spellbook.GetSpell(SpellSlot.Q).Level + QOff;
            var wL = Shen.Spellbook.GetSpell(SpellSlot.W).Level + WOff;
            var eL = Shen.Spellbook.GetSpell(SpellSlot.E).Level + EOff;
            var rL = Shen.Spellbook.GetSpell(SpellSlot.R).Level + ROff;
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

        private static void Orbwalker_OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) &&
                !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit) &&
                !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)) return;
            var t = target as Obj_AI_Minion;
            if (t == null) return;
            {
                if (MiscMenu["support"].Cast<CheckBox>().CurrentValue)
                    args.Process = false;
            }
        }

        private static void FlashE()
        {
            Player.IssueOrder(GameObjectOrder.MoveTo, MousePos);
            var fetarget = TargetSelector.GetTarget(1025, DamageType.Magical);
            if (fetarget == null) return;
            var xpos = fetarget.Position.Extend(fetarget, E.Range);
            var predepos = E.GetPrediction(fetarget).CastPosition;
            {
                if (!E.IsReady() || !Flash.IsReady() && fetarget.Distance(Shen) > 1025)  return;
                if (fetarget.IsValidTarget(1025))
                {
                    Flash.Cast((Vector3) xpos);
                    E.Cast(predepos);
                }
            }
        }

        private static void Ee()
        {
            Player.IssueOrder(GameObjectOrder.MoveTo, MousePos);
            var etarget = TargetSelector.GetTarget(600, DamageType.Magical);
            if (etarget == null) return;
            var predepos = E.GetPrediction(etarget).CastPosition;
            if (!_eMenu["e"].Cast<KeyBind>().CurrentValue) return;
            if (!E.IsReady()) return;
            if (etarget.IsValidTarget(600))
            {
                E.Cast(predepos);
            }
        }

        private static void Ult()
        {
            var autoult = _ultMenu["autoult"].Cast<CheckBox>().CurrentValue;
            var rslider = _ultMenu["rslider"].Cast<Slider>().CurrentValue;
            if (!autoult || (!_ultMenu["ult"].Cast<KeyBind>().CurrentValue)) return;
            foreach (var ally in EntityManager.Heroes.Allies.Where(
                x =>
                    _ultMenu["ult" + x.ChampionName].Cast<CheckBox>().CurrentValue && x.IsValidTarget(R.Range) &&
                    x.HealthPercent < rslider))
                if (R.IsReady() && ally.CountEnemiesInRange(600) >= 1)
                    R.Cast(ally);
        }

        private static void DrawAllyHealths()
        {
            {
                float i = 0;
                foreach (
                    var hero in EntityManager.Heroes.Allies.Where(hero => hero.IsAlly && !hero.IsMe && !hero.IsDead))
                {
                    var champion = hero.ChampionName;
                    if (champion.Length > 12)
                    {
                        champion = champion.Remove(7) + "..";
                    }
                    var percent = (int) (hero.Health/hero.MaxHealth*100);
                    var color = System.Drawing.Color.Red;
                    if (percent > 25)
                    {
                        color = System.Drawing.Color.Orange;
                    }   
                    if (percent > 50)
                    {
                        color = System.Drawing.Color.Yellow;
                    }
                    if (percent > 75)
                    {
                        color = System.Drawing.Color.LimeGreen;
                    }
                    Drawing.DrawText(
                        Drawing.Width*0.8f, Drawing.Height*0.1f + i, color, " (" + champion + ") ");
                    Drawing.DrawText(
                        Drawing.Width*0.9f, Drawing.Height*0.1f + i, color,
                        ((int) hero.Health) + " (" + percent + "%)");
                    i += 20f;
                }
            }
        }

        private static void Danger()
        {
            var rslider = _ultMenu["rslider"].Cast<Slider>().CurrentValue;
            foreach (
                var ally in
                    EntityManager.Heroes.Allies.Where(
                        x => x.IsValidTarget(R.Range) && x.HealthPercent < rslider)
                )
            {
                const float i = 0;
                {
                    var champion = ally.ChampionName;
                    if (champion.Length > 12)
                    {
                        champion = champion.Remove(7) + "..";
                    }
                    var percent = (int) (ally.Health/ally.MaxHealth*100);
                    var color = System.Drawing.Color.Red;
                    if (percent > 25)
                    {
                        color = System.Drawing.Color.Orange;
                    }
                    if (percent > 50)
                    {
                        color = System.Drawing.Color.Yellow;
                    }
                    if (percent > 75)
                    {
                        color = System.Drawing.Color.LimeGreen;
                    }
                    if (ally.CountEnemiesInRange(850) >= 1 && R.IsReady())
                    {
                        Drawing.DrawText(
                            Drawing.Width*0.4f, Drawing.Height*0.4f + i, color, " (" + champion + ")"
                                                                                + "-   (IS DYING - PRESS R TO AUTO ULT)");
                    }
                }
            }
        }

        private static Obj_AI_Base GetEnemy(float range, GameObjectType t)
        {
            switch (t)
            {
                case GameObjectType.AIHeroClient:
                    return EntityManager.Heroes.Enemies.OrderBy(a => a.Health).FirstOrDefault(
                        a => a.Distance(Player.Instance) < range && !a.IsDead && !a.IsInvulnerable);
                default:
                    return EntityManager.MinionsAndMonsters.EnemyMinions.OrderBy(a => a.Health).FirstOrDefault(
                        a =>
                            a.Distance(Player.Instance) < range && !a.IsDead && !a.IsInvulnerable &&
                            Shen.GetSpellDamage(a, SpellSlot.Q) >= a.Health);
            }
        }

        private static
            void LaneClear()
        {

            var qcheck = MiscMenu["LCQ"].Cast<CheckBox>().CurrentValue;
            var qready = Q.IsReady();
            if (!qcheck || !qready) return;
            {
                Q.Cast(Shen);
            }
        }

        private static
            void Combo()
        {
            var target = TargetSelector.GetTarget(750, DamageType.Magical);
           
            if (_comboMenu["autow"].Cast<CheckBox>().CurrentValue && W.IsReady())
            {               
                if (target.Distance(Shen) < 400 && Shen.CountAlliesInRange(650) > 1)
                {
                    W.Cast();
                }
                if (!E.IsReady() && target.Distance(Shen) < 350)
                {
                    W.Cast();
                }

               
            }
            if (Q.IsReady())
            {
                var qtarget = GetEnemy(Q.Range, GameObjectType.AIHeroClient);
                if (qtarget.IsValidTarget(Q.Range))
                {
                    HandleQ(qtarget);

            }
            if (!E.IsReady()) return;
            var eTarget = TargetSelector.GetTarget(900, DamageType.Magical);
            if (eTarget.IsValidTarget(E.Range))
                if (_eMenu["taunt" + eTarget.ChampionName].Cast<CheckBox>().CurrentValue)
                    if (eTarget.CountEnemiesInRange(E.Width) <= 1)
                        if (E.GetPrediction(eTarget).HitChance >= HitChance.High)
                        {
                            E.Cast(eTarget);
                        }
                        else if (eTarget.CountEnemiesInRange(E.Width) >=
                                 _eMenu["eslider"].Cast<Slider>().CurrentValue)
                        {
                            E.Cast(eTarget.ServerPosition);
                        }
            }
        }

        private static void OnDamage(AttackableUnit sender, AttackableUnitDamageEventArgs args)
        {
            var valcheck = MiscMenu["TUT"].Cast<CheckBox>().CurrentValue;
            var t = EntityManager.Heroes.AllHeroes.FirstOrDefault(h => h.NetworkId == args.Source.NetworkId);
            var s = EntityManager.Heroes.Enemies.FirstOrDefault(h => h.NetworkId == args.Target.NetworkId);
            if (valcheck && t != null && s != null &&
                (t.IsMe &&
                 ObjectManager.Get<Obj_AI_Turret>()
                     .FirstOrDefault(x => x.Distance(t) < 750 && x.Distance(s) < 750 && x.IsAlly) != null))
            {
                {
                    E.Cast(s);
                }
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
                case "Frozen":
                    Player.SetSkinId(1);
                    break;
                case "Yellow Jacket":
                    Player.SetSkinId(2);
                    break;
                case "Surgeon":
                    Player.SetSkinId(3);
                    break;
                case "Blood Moon":
                    Player.SetSkinId(4);
                    break;
                case "Warlord":
                    Player.SetSkinId(5);
                    break;
                case "TPA":
                    Player.SetSkinId(6);
                    break;
            }
        }
    }
}
