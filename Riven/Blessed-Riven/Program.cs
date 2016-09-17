using System;
using System.Linq;
using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using Color = System.Drawing.Color;
using EloBuddy.SDK.Constants;
using SharpDX;
using EloBuddy.SDK.Rendering;

namespace Blessed_Riven
{
    class Program
    {
        public static Spell.Active Q, W, E, R1;
        public static Spell.Skillshot R;
 
        
        public static bool EnableR;
        public static int LastCastQ;
        public static int LastCastW;
        private static bool ssfl;
        public static int QCount;
        public static Menu Menu, FarmingMenu, MiscMenu, DrawMenu, HarassMenu, ComboMenu, DelayMenu;
        static Item Healthpot;

        public static SpellSlot IgniteSlot = SpellSlot.Unknown;

        private static Spell.Targeted _ignite;

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;

        }

        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (_Player.ChampionName != "Riven") return;

            Q = new Spell.Active(SpellSlot.Q, 300);
            W = new Spell.Active(SpellSlot.W, (uint) (70 + _Player.BoundingRadius +
                                              (_Player.HasBuff("RivenFengShuiEngine") ? 195 : 120)));
            E = new Spell.Active(SpellSlot.E, 325);
            R = new Spell.Skillshot(SpellSlot.R, 900, SkillShotType.Cone, 250, 1600, 45)
            {
                AllowedCollisionCount = int.MaxValue
            };
            R1 = new Spell.Active(SpellSlot.R);


            Healthpot = new Item(2003, 0);
            _ignite = new Spell.Targeted(ObjectManager.Player.GetSpellSlotFromName("summonerdot"), 600);

            Chat.Print("Blessed Riven Yuklendi.", Color.Brown);
            Menu = MainMenu.AddMenu("Blessed Riven", "BlessedRiven");

            ComboMenu = Menu.AddSubMenu("Combo Settings", "ComboSettings");
            ComboMenu.AddLabel("Kombo Ayarları");
            ComboMenu.Add("QCombo", new CheckBox("Kullan Q"));
            ComboMenu.Add("WCombo", new CheckBox("Kullan W"));
            ComboMenu.Add("ECombo", new CheckBox("Kullan E"));
            ComboMenu.Add("RCombo", new CheckBox("Kullan R"));
            ComboMenu.Add("R2Combo", new CheckBox("Düşman ölecekse R2 kullan)"));
            ComboMenu.AddLabel("Burst = Hedefi seç tuşa bas");
            ComboMenu.AddLabel("Flash kullanılabilirse burst aktif olur");
            ComboMenu.AddLabel("Flashız performans vermez");
            ComboMenu.Add("ForcedR", new KeyBind("Zorla R", true, KeyBind.BindTypes.PressToggle, 'U'));
            ComboMenu.Add("useTiamat", new CheckBox("İtemleri Kullan"));
            ComboMenu.AddLabel("R Ayarları");
            ComboMenu.Add("RCantKill", new CheckBox("Komboyla ölmiyecekse", false));
            ComboMenu.Add("REnemyCount", new Slider("Düşman say >= ", 0, 0, 4));

            HarassMenu = Menu.AddSubMenu("Harass Settings", "HarassSettings");
            HarassMenu.AddLabel("Dürtme Ayarları");
            HarassMenu.Add("QHarass", new CheckBox("Kullan Q"));
            HarassMenu.Add("WHarass", new CheckBox("Kullan W"));
            HarassMenu.Add("EHarass", new CheckBox("Kullan E"));
            var Style = HarassMenu.Add("harassstyle", new Slider("Dürtme Stili", 0, 0, 2));
            Style.OnValueChange += delegate
            {
                Style.DisplayName = "Harass Style: " + new[] { "Q,Q,W,Q and E back", "E,H,Q3,W", "E,H,AA,Q,W" }[Style.CurrentValue];
            };
            Style.DisplayName = "Harass Style: " + new[] { "Q,Q,W,Q and E back", "E,H,Q3,W", "E,H,AA,Q,W" }[Style.CurrentValue];

            FarmingMenu = Menu.AddSubMenu("Clear Settings", "FarmSettings");
            FarmingMenu.AddLabel("Lane Temizleme");
            FarmingMenu.Add("QLaneClear", new CheckBox("Q Kullan"));
            FarmingMenu.Add("WLaneClear", new CheckBox("W Kullan"));
            FarmingMenu.Add("ELaneClear", new CheckBox("E Kullan"));

            FarmingMenu.AddLabel("Orman Temizleme");
            FarmingMenu.Add("QJungleClear", new CheckBox("Q Kullan"));
            FarmingMenu.Add("WJungleClear", new CheckBox("W Kullan"));
            FarmingMenu.Add("EJungleClear", new CheckBox("E Kullan"));

            FarmingMenu.AddLabel("Son Vuruş");
            FarmingMenu.Add("Qlasthit", new CheckBox("Q Kullan"));
            FarmingMenu.Add("Wlasthit", new CheckBox("W Kullan"));

            MiscMenu = Menu.AddSubMenu("More Settings", "Misc");
            MiscMenu.AddLabel("Otomatik");
            MiscMenu.Add("UseShield", new CheckBox("Kalkan(E)"));
            MiscMenu.Add("AutoIgnite", new CheckBox("Tutuştur"));
            MiscMenu.Add("AutoQSS", new CheckBox("Otomatik QSS"));
            MiscMenu.Add("AutoW", new CheckBox("Otomatik W"));
            MiscMenu.Add("WInterrupt", new CheckBox("W Interrupt"));
            MiscMenu.AddLabel("Q canlı tut");
            MiscMenu.Add("Alive.Q", new CheckBox("Q canlı tut(Q ile kaç)"));
            MiscMenu.AddLabel("Activator");
            MiscMenu.Add("useHP", new CheckBox("Can potu"));
            MiscMenu.Add("useHPV", new Slider("canım şundan az < %", 45, 0, 100));

            DelayMenu = Menu.AddSubMenu("Delay Settings(Humanizer)", "Delay");
            DelayMenu.Add("useHumanizer", new CheckBox("İnsancıl Ayar Kullan?", false));
            DelayMenu.Add("spell1a1b", new Slider("Q1,Q2 Gecikmesi(ms)", 261, 100, 400));
            DelayMenu.Add("spell1c", new Slider("Q3 Gecikmesi(ms)", 353, 100, 400));
            DelayMenu.Add("spell2", new Slider("W Gecikmesi(ms)", 120, 100, 400));
            DelayMenu.Add("spell4a", new Slider("R Gecikmesi(ms)", 0, 0, 400));
            DelayMenu.Add("spell4b", new Slider("R2 Gecikmesi(ms)", 100, 50, 400));

            DrawMenu = Menu.AddSubMenu("Draw Settings", "Drawings");
            DrawMenu.Add("drawCombo", new CheckBox("Kombo Menzilini Göster", false));

            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += Drawing_OnDraw;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
            Obj_AI_Base.OnSpellCast += Obj_AI_Base_OnSpellCast;
            Obj_AI_Base.OnPlayAnimation += Obj_AI_Base_OnPlayAnimation;
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;

        }

        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs e)
        {
            if (sender.IsEnemy && W.IsReady() && sender.IsValidTarget() && !sender.IsZombie && MiscMenu["WInterrupt"].Cast<CheckBox>().CurrentValue)
            {
                if (sender.IsValidTarget(125 + _Player.BoundingRadius + sender.BoundingRadius)) W.Cast();
            }
        }

        private static void DoQSS()
        {
            if (!MiscMenu["AutoQSS"].Cast<CheckBox>().CurrentValue) return;

            if (Item.HasItem(3139) && Item.CanUseItem(3139) && ObjectManager.Player.CountEnemiesInRange(1800) > 0)
            {
                Core.DelayAction(() => Item.UseItem(3139), 1);
            }

            if (Item.HasItem(3140) && Item.CanUseItem(3140) && ObjectManager.Player.CountEnemiesInRange(1800) > 0)
            {
                Core.DelayAction(() => Item.UseItem(3140), 1);
            }
        }
        
        private static void Game_OnTick(EventArgs args)
        {
            var HPpot = MiscMenu["useHP"].Cast<CheckBox>().CurrentValue;
            var HPv = MiscMenu["useHPv"].Cast<Slider>().CurrentValue;

            if (LastCastQ + 3600 < Environment.TickCount)
            {
                QCount = 0;
            }
            if (MiscMenu["Alive.Q"].Cast<CheckBox>().CurrentValue && !_Player.IsRecalling() && QCount < 3 && QCount > 0 && LastCastQ + 3480 < Environment.TickCount && _Player.HasBuff("RivenTriCleaveBuff") && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Player.CastSpell(SpellSlot.Q,
                    Orbwalker.LastTarget != null && Orbwalker.LastAutoAttack - Environment.TickCount < 3000
                        ? Orbwalker.LastTarget.Position
                        : Game.CursorPos);
                return;
            }
            foreach (AIHeroClient enemy in EntityManager.Heroes.Enemies)
            {     
                if (HPpot && _Player.HealthPercent < HPv && _Player.Distance(enemy) < 2000)
                {
                    if (Item.HasItem(Healthpot.Id) && Item.CanUseItem(Healthpot.Id) && !Player.HasBuff("RegenerationPotion"))
                    {
                        Healthpot.Cast();
                    }
                }
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Combo();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                Harass();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                LaneClear();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                LastHit();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                JungleClear();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                Flee();
            }
            Auto();
        }

        private static void Auto()
        {
            var w = TargetSelector.GetTarget(W.Range, DamageType.Physical);
            if (w == null) return;

            if (w.IsValidTarget(W.Range) && MiscMenu["AutoW"].Cast<CheckBox>().CurrentValue)
            {
                W.Cast();
            }
            if (_Player.HasBuffOfType(BuffType.Stun) || _Player.HasBuffOfType(BuffType.Taunt) || _Player.HasBuffOfType(BuffType.Polymorph) || _Player.HasBuffOfType(BuffType.Frenzy) || _Player.HasBuffOfType(BuffType.Fear) || _Player.HasBuffOfType(BuffType.Snare) || _Player.HasBuffOfType(BuffType.Suppression))
            {
                DoQSS();
            }
            if (MiscMenu["AutoIgnite"].Cast<CheckBox>().CurrentValue)
            {
                if (!_ignite.IsReady() || _Player.IsDead) return;
                foreach (
                    var source in
                        EntityManager.Heroes.Enemies
                            .Where(
                                a => a.IsValidTarget(_ignite.Range) &&
                                    a.Health < 50 + 20 * _Player.Level - (a.HPRegenRate / 5 * 3)))
                {
                    _ignite.Cast(source);
                    return;
                }
            }
        }
  
        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsEnemy || sender.Type == _Player.Type || MiscMenu["UseShield"].Cast<CheckBox>().CurrentValue)
            {
                var epos = _Player.ServerPosition +
                           (_Player.ServerPosition - sender.ServerPosition).Normalized() * 300;

                if (!(_Player.Distance(sender.ServerPosition) <= args.SData.CastRange)) return;
                switch (args.SData.TargettingType)
                {
                    case SpellDataTargetType.Unit:

                        if (args.Target.NetworkId == _Player.NetworkId)
                        {
                            if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.LastHit &&
                                !args.SData.Name.Contains("NasusW"))
                            {
                                if (E.IsReady()) Player.CastSpell(SpellSlot.E, epos);
                            }
                        }

                        break;
                    case SpellDataTargetType.SelfAoe:

                        if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.LastHit)
                        {
                            if (E.IsReady()) Player.CastSpell(SpellSlot.E, epos);
                        }

                        break;
                }
                if (args.SData.Name.Contains("IreliaEquilibriumStrike"))
                {
                    if (args.Target.NetworkId == _Player.NetworkId)
                    {
                        if (W.IsReady() && W.IsInRange(sender)) W.Cast();
                        else if (E.IsReady()) Player.CastSpell(SpellSlot.E, epos);
                    }
                }
                if (args.SData.Name.Contains("TalonCutthroat"))
                {
                    if (args.Target.NetworkId == _Player.NetworkId)
                    {
                        if (W.IsReady()) W.Cast();
                    }
                }
                if (args.SData.Name.Contains("RenektonPreExecute"))
                {
                    if (args.Target.NetworkId == _Player.NetworkId)
                    {
                        if (W.IsReady()) W.Cast();
                    }
                }
                if (args.SData.Name.Contains("GarenRPreCast"))
                {
                    if (args.Target.NetworkId == _Player.NetworkId)
                    {
                        if (E.IsReady()) Player.CastSpell(SpellSlot.E, epos);
                    }
                }
                if (args.SData.Name.Contains("GarenQAttack"))
                {
                    if (args.Target.NetworkId == _Player.NetworkId)
                    {
                        if (E.IsReady()) Player.CastSpell(SpellSlot.E, Game.CursorPos);
                    }
                }
                if (args.SData.Name.Contains("XenZhaoThrust3"))
                {
                    if (args.Target.NetworkId == _Player.NetworkId)
                    {
                        if (W.IsReady()) W.Cast();
                    }
                }
                if (args.SData.Name.Contains("RengarQ"))
                {
                    if (args.Target.NetworkId == _Player.NetworkId)
                    {
                        if (E.IsReady()) Player.CastSpell(SpellSlot.E, Game.CursorPos);
                    }
                }
                if (args.SData.Name.Contains("RengarPassiveBuffDash"))
                {
                    if (args.Target.NetworkId == _Player.NetworkId)
                    {
                        if (E.IsReady()) Player.CastSpell(SpellSlot.E, Game.CursorPos);
                    }
                }
                if (args.SData.Name.Contains("RengarPassiveBuffDashAADummy"))
                {
                    if (args.Target.NetworkId == _Player.NetworkId)
                    {
                        if (E.IsReady()) Player.CastSpell(SpellSlot.E, Game.CursorPos);
                    }
                }
                if (args.SData.Name.Contains("TwitchEParticle"))
                {
                    if (args.Target.NetworkId == _Player.NetworkId)
                    {
                        if (E.IsReady()) Player.CastSpell(SpellSlot.E, Game.CursorPos);
                    }
                }
                if (args.SData.Name.Contains("FizzPiercingStrike"))
                {
                    if (args.Target.NetworkId == _Player.NetworkId)
                    {
                        if (E.IsReady()) Player.CastSpell(SpellSlot.E, Game.CursorPos);
                    }
                }
                if (args.SData.Name.Contains("HungeringStrike"))
                {
                    if (args.Target.NetworkId == _Player.NetworkId)
                    {
                        if (E.IsReady()) Player.CastSpell(SpellSlot.E, Game.CursorPos);
                    }
                }
                if (args.SData.Name.Contains("YasuoDash"))
                {
                    if (args.Target.NetworkId == _Player.NetworkId)
                    {
                        if (E.IsReady()) Player.CastSpell(SpellSlot.E, Game.CursorPos);
                    }
                }
                if (args.SData.Name.Contains("KatarinaRTrigger"))
                {
                    if (args.Target.NetworkId == _Player.NetworkId)
                    {
                        if (W.IsReady() && W.IsInRange(sender)) W.Cast();
                        else if (E.IsReady()) Player.CastSpell(SpellSlot.E, Game.CursorPos);
                    }
                }
                if (args.SData.Name.Contains("YasuoDash"))
                {
                    if (args.Target.NetworkId == _Player.NetworkId)
                    {
                        if (E.IsReady()) Player.CastSpell(SpellSlot.E, Game.CursorPos);
                    }
                }
                if (args.SData.Name.Contains("KatarinaE"))
                {
                    if (args.Target.NetworkId == _Player.NetworkId)
                    {
                        if (W.IsReady()) W.Cast();
                    }
                }
                if (args.SData.Name.Contains("MonkeyKingQAttack"))
                {
                    if (args.Target.NetworkId == _Player.NetworkId)
                    {
                        if (E.IsReady()) Player.CastSpell(SpellSlot.E, Game.CursorPos);
                    }
                }
                if (args.SData.Name.Contains("MonkeyKingSpinToWin"))
                {
                    if (args.Target.NetworkId == _Player.NetworkId)
                    {
                        if (E.IsReady()) Player.CastSpell(SpellSlot.E, Game.CursorPos);
                        else if (W.IsReady()) W.Cast();
                    }
                }
                if (args.SData.Name.Contains("MonkeyKingQAttack"))
                {
                    if (args.Target.NetworkId == _Player.NetworkId)
                    {
                        if (E.IsReady()) Player.CastSpell(SpellSlot.E, Game.CursorPos);
                    }
                }
                if (args.SData.Name.Contains("MonkeyKingQAttack"))
                {
                    if (args.Target.NetworkId == _Player.NetworkId)
                    {
                        if (E.IsReady()) Player.CastSpell(SpellSlot.E, Game.CursorPos);
                    }
                }
                if (args.SData.Name.Contains("MonkeyKingQAttack"))
                {
                    if (args.Target.NetworkId == _Player.NetworkId)
                    {
                        if (E.IsReady()) Player.CastSpell(SpellSlot.E, Game.CursorPos);
                    }
                }
            }
            if (!sender.IsMe) return;

            if (args.SData.Name.ToLower().Contains(W.Name.ToLower()))
            {
                LastCastW = Environment.TickCount;
                return;
            }
            if (args.Target is Obj_AI_Turret || args.Target is Obj_Barracks || args.Target is Obj_BarracksDampener ||
                args.Target is Obj_Building)
                if (args.Target.IsValid && args.Target != null && Q.IsReady() && FarmingMenu["QLaneClear"].Cast<CheckBox>().CurrentValue &&
                    Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
                    Player.CastSpell(SpellSlot.Q, (Obj_AI_Base)args.Target);
            AIHeroClient client = args.Target as AIHeroClient;

            if (args.SData.Name.ToLower().Contains(Q.Name.ToLower()))
            {
                LastCastQ = Environment.TickCount;
                if (!MiscMenu["Alive.Q"].Cast<CheckBox>().CurrentValue) return;
                Core.DelayAction(() =>
                {
                    if (!_Player.IsRecalling() && QCount <= 2)
                    {
                        Player.CastSpell(SpellSlot.Q,
                            Orbwalker.LastTarget != null && Orbwalker.LastAutoAttack - Environment.TickCount < 3000
                                ? Orbwalker.LastTarget.Position
                                : Game.CursorPos);
                    }
                }, 3480);
                return;
            }
        }

        private static void ForceItem()
        {
            if (Item.HasItem(3074) && Item.CanUseItem(3074))
            {
                Item.UseItem(3074);
                return;
            }
            else if (Item.HasItem(3077) && Item.CanUseItem(3077))
            {
                Item.UseItem(3077);
                return;
            }
        }

        private static void Obj_AI_Base_OnPlayAnimation(Obj_AI_Base sender, GameObjectPlayAnimationEventArgs args)
        {
            if (!sender.IsMe) return;
            var t = 0;
            switch (args.Animation)
            {
                case "Spell1a":
                    if (DelayMenu["useHumanizer"].Cast<CheckBox>().CurrentValue)
                    {
                        t = DelayMenu["spell1a1b"].Cast<Slider>().CurrentValue;
                        QCount = 1;
                    }
                    else
                    {
                        t = 221;
                        QCount = 1;
                    }                                
                    break;
                case "Spell1b":
                    if (DelayMenu["useHumanizer"].Cast<CheckBox>().CurrentValue)
                    {
                        t = DelayMenu["spell1a1b"].Cast<Slider>().CurrentValue;
                        QCount = 2;
                    }
                    else
                    {
                        t = 221;
                        QCount = 2;
                    }
                    break;
                case "Spell1c":
                    if (DelayMenu["useHumanizer"].Cast<CheckBox>().CurrentValue)
                    {
                        t = DelayMenu["spell1c"].Cast<Slider>().CurrentValue;
                        QCount = 0;
                    }
                    else
                    {
                        t = 303;
                        QCount = 0;
                    }
                    break;
                case "Spell2":
                    if (DelayMenu["useHumanizer"].Cast<CheckBox>().CurrentValue)
                    {
                        t = DelayMenu["spell2"].Cast<Slider>().CurrentValue;
                    }
                    else
                    {
                        t = 110;
                    }
                    break;
                case "Spell3":
                    break;
                case "Spell4a":
                    if (DelayMenu["useHumanizer"].Cast<CheckBox>().CurrentValue)
                    {
                        t = DelayMenu["spell4a"].Cast<Slider>().CurrentValue;
                    }
                    else
                    {
                        t = 0;
                    }
                    break;
                case "Spell4b":
                    if (DelayMenu["useHumanizer"].Cast<CheckBox>().CurrentValue)
                    {
                        t = DelayMenu["spell4b"].Cast<Slider>().CurrentValue;
                    }
                    else
                    {
                        t = 100;
                    }
                    break;
            }
            if (t != 0 && ((Orbwalker.ActiveModesFlags != Orbwalker.ActiveModes.None)))
            {
                Orbwalker.ResetAutoAttack();
                Core.DelayAction(CancelAnimation, t - Game.Ping);
            }

        }

        private static void CancelAnimation()
        {
            Player.DoEmote(Emote.Dance);
            Orbwalker.ResetAutoAttack();
        }

        private static void Obj_AI_Base_OnSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe) return;
            var target = args.Target as Obj_AI_Base;
            var targetC = args.Target as AIHeroClient;

            // Hydra
            if (args.SData.Name.ToLower().Contains("itemtiamatcleave"))
            {
                Orbwalker.ResetAutoAttack();
                if (W.IsReady())
                {
                    var target2 = TargetSelector.GetTarget(W.Range, DamageType.Physical);
                    if (target2 != null || Orbwalker.ActiveModesFlags != Orbwalker.ActiveModes.None)
                    {
                        Player.CastSpell(SpellSlot.W);
                    }
                }
                return;
            }

            //W
            if (args.SData.Name.ToLower().Contains(W.Name.ToLower()))
            {
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                {
                    if (_Player.HasBuff("RivenFengShuiEngine") && R.IsReady() &&
                        ComboMenu["R2Combo"].Cast<CheckBox>().CurrentValue)
                    {
                        ssfl = true;
                        var target2 = TargetSelector.GetTarget(R.Range, DamageType.Physical);
                        if (target2 != null &&
                            (target2.Distance(Player.Instance) < W.Range &&
                             target2.Health >
                             _Player.CalculateDamageOnUnit(target2, DamageType.Physical, Damage.WDamage()) ||
                             target2.Distance(Player.Instance) > W.Range) &&
                            _Player.CalculateDamageOnUnit(target2, DamageType.Physical,
                                Damage.RDamage(target2) + Damage.WDamage()) > target2.Health)
                        {
                            R.Cast(target2);
                        }
                    }
                }

                target = (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) ||
                          Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
                    ? TargetSelector.GetTarget(E.Range + W.Range, DamageType.Physical)
                    : (Obj_AI_Base)Orbwalker.LastTarget;
                if (Q.IsReady() && Orbwalker.ActiveModesFlags != Orbwalker.ActiveModes.None && target != null)
                {
                    Player.CastSpell(SpellSlot.Q, target.Position);
                    return;
                }
                return;
            }

            //E
            if (args.SData.Name.ToLower().Contains(E.Name.ToLower()))
            {
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                {
                    if (_Player.HasBuff("RivenFengShuiEngine") && R.IsReady() &&
                        ComboMenu["R2Combo"].Cast<CheckBox>().CurrentValue)
                    {
                        ssfl = true;
                        var target2 = TargetSelector.GetTarget(R.Range, DamageType.Physical);
                        if (target2 != null &&
                            _Player.CalculateDamageOnUnit(target2, DamageType.Physical,
                                (Damage.RDamage(target2))) > target2.Health)
                        {
                            R.Cast(target2);
                            return;
                        }
                    }
                    if ((EnableR == true) && R.IsReady() &&
                        !_Player.HasBuff("RivenFengShuiEngine") &&
                        ComboMenu["RCombo"].Cast<CheckBox>().CurrentValue)
                    {
                        ssfl = false;
                        ForceR();
                        Player.CastSpell(SpellSlot.R);
                    }
                    target = TargetSelector.GetTarget(W.Range, DamageType.Physical);
                    if (target != null && _Player.Distance(target) < W.Range)
                    {
                        Player.CastSpell(SpellSlot.W);
                        return;
                    }
                }
            }

            //Q
            if (args.SData.Name.ToLower().Contains(Q.Name.ToLower()))
            {
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                {
                    if (_Player.HasBuff("RivenFengShuiEngine") && R.IsReady() &&
                        ComboMenu["R2Combo"].Cast<CheckBox>().CurrentValue)
                    {
                        var target2 = TargetSelector.GetTarget(R.Range, DamageType.Physical);
                        if (target2 != null &&
                            (target2.Distance(Player.Instance) < 300 &&
                             target2.Health >
                             _Player.CalculateDamageOnUnit(target2, DamageType.Physical, Damage.QDamage()) ||
                             target2.Distance(Player.Instance) > 300) &&
                            _Player.CalculateDamageOnUnit(target2, DamageType.Physical,
                                Damage.RDamage(target2) + Damage.QDamage()) > target2.Health)
                        {
                            R.Cast(target2);
                        }
                    }
                }
                return;
            }

            if (args.SData.IsAutoAttack() && target != null)
            {
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
                {
                    JungleAfterAa(target);
                }

                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit) && target.IsMinion())
                {
                    LastHitAfterAa(target);
                }

                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) && target.IsMinion())
                {
                    LaneClearAfterAa(target);
                }
            }
            if (args.SData.IsAutoAttack() && targetC != null)
            {
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                {
                    ComboAfterAa(targetC);
                }

                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
                {
                    HarassAfterAa(targetC);
                }
            }

            }

        private static float getComboDamage(Obj_AI_Base enemy)
        {
            if (enemy != null)
            {
                float damage = 0;
                float passivenhan;
                if (_Player.Level >= 18)
                {
                    passivenhan = 0.5f;
                }
                else if (_Player.Level >= 15)
                {
                    passivenhan = 0.45f;
                }
                else if (_Player.Level >= 12)
                {
                    passivenhan = 0.4f;
                }
                else if (_Player.Level >= 9)
                {
                    passivenhan = 0.35f;
                }
                else if (_Player.Level >= 6)
                {
                    passivenhan = 0.3f;
                }
                else if (_Player.Level >= 3)
                {
                    passivenhan = 0.25f;
                }
                else
                {
                    passivenhan = 0.2f;
                }
                if (Item.HasItem(3074)) damage = damage + _Player.GetAutoAttackDamage(enemy) * 0.7f;
                if (W.IsReady()) damage = damage + Damage.WDamage();
                if (Q.IsReady())
                {
                    var qnhan = 4 - QCount;
                    damage = damage + Damage.QDamage() * qnhan +
                             _Player.GetAutoAttackDamage(enemy) * qnhan * (1 + passivenhan);
                }
                damage = damage + _Player.GetAutoAttackDamage(enemy) * (1 + passivenhan);
                if (R.IsReady())
                {
                    return damage * 1.2f + Damage.RDamage(enemy);
                }

                return damage;
            }
            return 0;
        }

        public static void ComboAfterAa(AIHeroClient target)
        {
            try
            {
                if (_Player.HasBuff("RivenFengShuiEngine") && R.IsReady() &&
                    ComboMenu["R2Combo"].Cast<CheckBox>().CurrentValue)
                {
                    if (_Player.CalculateDamageOnUnit(target, DamageType.Physical, Damage.RDamage(target)) + _Player.GetAutoAttackDamage(target, true) > target.Health)
                    {
                        ssfl = true;
                        R.Cast(target);
                        return;
                    }
                }
                if (ComboMenu["WCombo"].Cast<CheckBox>().CurrentValue && W.IsReady() &&
                    W.IsInRange(target))
                {
                    Core.DelayAction(ForceItem, 50);
                    Player.CastSpell(SpellSlot.W);
                    return;
                }
                if (ComboMenu["QCombo"].Cast<CheckBox>().CurrentValue && Q.IsReady())
                {
                    Player.CastSpell(SpellSlot.Q, target.Position);
                    return;
                }
                Core.DelayAction(ForceItem, 50);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        
        }

        public static void HarassAfterAa(AIHeroClient target)
        {
            
                if (HarassMenu["WHarass"].Cast<CheckBox>().CurrentValue && W.IsReady() &&
                    W.IsInRange(target))
                {
                Core.DelayAction(ForceItem, 50);
                Player.CastSpell(SpellSlot.W);
                return;
            }
                if (HarassMenu["QHarass"].Cast<CheckBox>().CurrentValue && Q.IsReady())
                {
                    Player.CastSpell(SpellSlot.Q, target.Position);
                return;
            }
            Core.DelayAction(ForceItem, 50);


        }

        public static void LastHitAfterAa(Obj_AI_Base target)
        {
            
                var unitHp = target.Health - _Player.GetAutoAttackDamage(target, true);
                if (unitHp > 0)
                {
                    if (FarmingMenu["QLastHit"].Cast<CheckBox>().CurrentValue && Q.IsReady() &&
                        _Player.CalculateDamageOnUnit(target, DamageType.Physical, Damage.QDamage()) >
                        unitHp)
                    {
                        Player.CastSpell(SpellSlot.Q, target.Position);
                        return;
                    }
                    if (FarmingMenu["WLastHit"].Cast<CheckBox>().CurrentValue && W.IsReady() &&
                        W.IsInRange(target) &&
                        _Player.CalculateDamageOnUnit(target, DamageType.Physical, Damage.WDamage()) >
                        unitHp)
                    {
                        Player.CastSpell(SpellSlot.W);
                    }
                }
            
        }

        public static void LaneClearAfterAa(Obj_AI_Base target)
        {
            try
            { 
                var unitHp = target.Health - _Player.GetAutoAttackDamage(target, true);
                if (unitHp > 0)
                {
                    if (FarmingMenu["QLaneClear"].Cast<CheckBox>().CurrentValue && Q.IsReady())
                    {
                        Player.CastSpell(SpellSlot.Q, target.Position);
                        return;
                    }
                    if (FarmingMenu["WLaneClear"].Cast<CheckBox>().CurrentValue && W.IsReady() &&
                        W.IsInRange(target))
                    {
                        Player.CastSpell(SpellSlot.W);
                        return;
                    }
                }
                else
                {
                    List<Obj_AI_Minion> minions = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                        _Player.Position, Q.Range).Where(a => a.NetworkId != target.NetworkId).ToList();
                    if (FarmingMenu["QLaneClear"].Cast<CheckBox>().CurrentValue && Q.IsReady() && minions.Any())
                    {
                        Player.CastSpell(SpellSlot.Q, minions[0].Position);
                        return;
                    }
                    minions = minions.Where(a => a.Distance(Player.Instance) < W.Range).ToList();
                    if (FarmingMenu["WLaneClear"].Cast<CheckBox>().CurrentValue && W.IsReady() &&
                        W.IsInRange(target) && minions.Any())
                    {
                        Player.CastSpell(SpellSlot.W);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
        }

        public static void JungleAfterAa(Obj_AI_Base target)
        {
            
            {
                if (FarmingMenu["WJungleClear"].Cast<CheckBox>().CurrentValue && W.IsReady() &&
                    W.IsInRange(target))
                {
                    Core.DelayAction(ForceItem, 50);
                    Player.CastSpell(SpellSlot.W);
                    return;
                }
                if (FarmingMenu["QJungleClear"].Cast<CheckBox>().CurrentValue && Q.IsReady())
                {
                    Player.CastSpell(SpellSlot.Q, target.Position);
                    return;
                }
                Core.DelayAction(ForceItem, 50);
            }
        }

        private static void Combo()
        {
            
            if (Orbwalker.IsAutoAttacking) return;
            var target = TargetSelector.GetTarget(E.Range + W.Range + 200, DamageType.Physical);
            if (target == null) return;
            var useQ = ComboMenu["QCombo"].Cast<CheckBox>().CurrentValue;
            var useW = ComboMenu["WCombo"].Cast<CheckBox>().CurrentValue;
            var useE = ComboMenu["ECombo"].Cast<CheckBox>().CurrentValue;
            var useR = ComboMenu["RCombo"].Cast<CheckBox>().CurrentValue;
            var useItem = ComboMenu["useTiamat"].Cast<CheckBox>().CurrentValue;
            EnableR = false;
            try
            { 
                if (R.IsReady() && _Player.HasBuff("RivenFengShuiEngine") &&
                     ComboMenu["R2Combo"].Cast<CheckBox>().CurrentValue)
            {
                if (EntityManager.Heroes.Enemies.Where(
                        enemy =>
                            enemy.IsValidTarget(R.Range) &&
                            enemy.Health <
                            _Player.CalculateDamageOnUnit(enemy, DamageType.Physical,
                                Damage.RDamage(enemy))).Any(enemy => R.Cast(enemy)))
                {
                    ssfl = true;
                    return;
                }
            }

            if (ComboMenu["RCombo"].Cast<CheckBox>().CurrentValue && R.IsReady() && !_Player.HasBuff("RivenFengShuiEngine"))
            {
                if ((ComboMenu["RCantKill"].Cast<CheckBox>().CurrentValue &&
                    target.Health > Damage.ComboDamage(target, true)
                    && target.Health < Damage.ComboDamage(target)
                    && target.Health > _Player.GetAutoAttackDamage(target, true) * 2) ||
                    (ComboMenu["REnemyCount"].Cast<Slider>().CurrentValue > 0 &&
                    _Player.CountEnemiesInRange(600) >= ComboMenu["REnemyCount"].Cast<Slider>().CurrentValue) || IsRActive)
                {
                    ssfl = false;
                    EnableR = true;
                        ForceR();
                }
                if (ComboMenu["ForcedR"].Cast<KeyBind>().CurrentValue)
                {
                    ssfl = false;
                    EnableR = true;
                        ForceR();
                }
            }

            if (ComboMenu["ECombo"].Cast<CheckBox>().CurrentValue && target.Distance(Player.Instance) > W.Range && E.IsReady())
            {
                if (Item.HasItem(3142) && Item.CanUseItem(3142))
                {
                    Item.UseItem(3142);
                }
                Player.CastSpell(SpellSlot.E, target.Position);
                return;
            }

                if (ComboMenu["ECombo"].Cast<CheckBox>().CurrentValue && target.Distance(Player.Instance) < W.Range && E.IsReady())
                {
                    Player.CastSpell(SpellSlot.E, Game.CursorPos);
                    return;
                }

                if (ComboMenu["WCombo"].Cast<CheckBox>().CurrentValue &&
                target.Distance(Player.Instance) <= W.Range && W.IsReady())
            {
                    Core.DelayAction(ForceItem, 50);
                    Player.CastSpell(SpellSlot.W);
                    return;
                
            }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void Flee()
        {         
            var x = _Player.Position.Extend(Game.CursorPos, 300);
            if (Q.IsReady() && !_Player.IsDashing()) Player.CastSpell(SpellSlot.Q, Game.CursorPos);
            if (E.IsReady() && !_Player.IsDashing()) Player.CastSpell(SpellSlot.E, x.To3D());

        }

        public static void Harass()
        {
            if (Orbwalker.IsAutoAttacking) return;

            var target = TargetSelector.GetTarget(E.Range + W.Range, DamageType.Physical);

             {
                if (target == null) return;

                if (HarassMenu["EHarass"].Cast<CheckBox>().CurrentValue &&
                    (target.Distance(Player.Instance) > W.Range &&
                     target.Distance(Player.Instance) < E.Range + W.Range ||
                     IsRActive && R.IsReady() &&
                     target.Distance(Player.Instance) < E.Range + 265 + _Player.BoundingRadius) &&
                    E.IsReady())
                {
                    Player.CastSpell(SpellSlot.E, target.Position);
                    return;
                }

                if (HarassMenu["WHarass"].Cast<CheckBox>().CurrentValue &&
                    target.Distance(Player.Instance) <= W.Range && W.IsReady())
                {
                    ForceItem();
                    Player.CastSpell(SpellSlot.W);
                    return;
                }
            }
        }

        private static void JungleClear()
        {
            var minion =
                 EntityManager.MinionsAndMonsters.Monsters.OrderByDescending(a => a.MaxHealth)
                     .FirstOrDefault(a => a.Distance(Player.Instance) < _Player.GetAutoAttackRange(a));

            {
                if (minion == null) return;

                if (FarmingMenu["QJungleClear"].Cast<CheckBox>().CurrentValue && Q.IsReady() &&
                       minion.Health <=
                       _Player.CalculateDamageOnUnit(minion, DamageType.Physical, Damage.QDamage()))
                {
                    Player.CastSpell(SpellSlot.Q, minion.Position);
                    return;
                }

                if (FarmingMenu["EJungleClear"].Cast<CheckBox>().CurrentValue && (!W.IsReady() && !Q.IsReady() || _Player.HealthPercent < 20) && E.IsReady() &&
                    LastCastW + 750 < Environment.TickCount)
                {
                    Player.CastSpell(SpellSlot.E, minion.Position);
                }
            }
        }

        public static void LaneClear()
        {
            try
            {
                Orbwalker.ForcedTarget = null;
                {
                    if (Orbwalker.IsAutoAttacking || LastCastQ + 260 > Environment.TickCount) return;
                    foreach (
                        var minion in EntityManager.MinionsAndMonsters.EnemyMinions.Where(a => a.IsValidTarget(W.Range)))
                    {
                        if (FarmingMenu["QLaneClear"].Cast<CheckBox>().CurrentValue && Q.IsReady() &&
                            minion.Health <=
                            _Player.CalculateDamageOnUnit(minion, DamageType.Physical, Damage.QDamage()))
                        {
                            Player.CastSpell(SpellSlot.Q, minion.Position);
                            return;
                        }
                        if (FarmingMenu["WLaneClear"].Cast<CheckBox>().CurrentValue && W.IsReady() &&
                            minion.Health <=
                            _Player.CalculateDamageOnUnit(minion, DamageType.Physical, Damage.WDamage()))
                        {
                            Player.CastSpell(SpellSlot.W);
                            return;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.Write(ex);
            }

        }
                 
        public static void LastHit()
        {
            Orbwalker.ForcedTarget = null;
            {
                if (Orbwalker.IsAutoAttacking) return;

                foreach (
                    var minion in EntityManager.MinionsAndMonsters.EnemyMinions.Where(a => a.IsValidTarget(W.Range)))
                {
                    if (FarmingMenu["QLastHit"].Cast<CheckBox>().CurrentValue && Q.IsReady() &&
                        minion.Health <=
                        _Player.CalculateDamageOnUnit(minion, DamageType.Physical, Damage.QDamage()))
                    {
                        Player.CastSpell(SpellSlot.Q, minion.Position);
                        return;
                    }
                    if (FarmingMenu["WLastHit"].Cast<CheckBox>().CurrentValue && W.IsReady() &&
                        minion.Health <=
                        _Player.CalculateDamageOnUnit(minion, DamageType.Physical, Damage.WDamage()))
                    {
                        Player.CastSpell(SpellSlot.W);
                        return;
                    }
                }
            }

        }

        public static bool IsRActive
        {
            get
            {
                return ComboMenu["ForcedR"].Cast<KeyBind>().CurrentValue &&
                       ComboMenu["RCombo"].Cast<CheckBox>().CurrentValue;
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (_Player.IsDead)
                return;
            if (DrawMenu["drawCombo"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(E.IsReady() ? SharpDX.Color.LimeGreen : SharpDX.Color.IndianRed, 250 + _Player.AttackRange + 70, ObjectManager.Player.Position);
            }
        }

        private static void ForceR()
        {
            ssfl = (R.IsReady() && R.Name == "RivenFengShuiEngine" && R1.Cast());
            Core.DelayAction(() => ssfl = true, 500);
        }

    }
}