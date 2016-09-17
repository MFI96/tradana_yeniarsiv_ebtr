using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using Color = System.Drawing.Color;
using SharpDX;


namespace TimeBreakerEkko
{

    class Program
    {
        public static Spell.Skillshot Q;
        public static Spell.Skillshot W;
        public static Spell.Skillshot E;
        public static Spell.Active R;
        static Spell.Targeted Smite = null;
        public static Menu Menu, FarmingMenu, MiscMenu, DrawMenu, HarassMenu, ComboMenu, SmiteMenu, Skin;
        static Item Healthpot;
        static Item Manapot;
        static Item HuntersPotion;
        static Item CorruptionPotion;
        static Item RefillablePotion;
        public static SpellSlot SmiteSlot = SpellSlot.Unknown;
        public static SpellSlot IgniteSlot = SpellSlot.Unknown;
        private static readonly int[] SmitePurple = { 3713, 3726, 3725, 3726, 3723 };
        private static readonly int[] SmiteGrey = { 3711, 3722, 3721, 3720, 3719 };
        private static readonly int[] SmiteRed = { 3715, 3718, 3717, 3716, 3714 };
        private static readonly int[] SmiteBlue = { 3706, 3710, 3709, 3708, 3707 };
        public static Obj_GeneralParticleEmitter EkkoUlt { get; set; }

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        private static string Smitetype
        {
            get
            {
                if (SmiteBlue.Any(i => Item.HasItem(i)))
                    return "s5_summonersmiteplayerganker";

                if (SmiteRed.Any(i => Item.HasItem(i)))
                    return "s5_summonersmiteduel";

                if (SmiteGrey.Any(i => Item.HasItem(i)))
                    return "s5_summonersmitequick";

                if (SmitePurple.Any(i => Item.HasItem(i)))
                    return "itemsmiteaoe";

                return "summonersmite";
            }
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.ChampionName != "Ekko")
                return;


            Bootstrap.Init(null);
            SpellDataInst smite = _Player.Spellbook.Spells.Where(spell => spell.Name.Contains("smite")).Any() ? _Player.Spellbook.Spells.Where(spell => spell.Name.Contains("smite")).First() : null;
            if (smite != null)
            {
                Smite = new Spell.Targeted(smite.Slot, 500);
            }
            Healthpot = new Item(2003, 0);
            Manapot = new Item(2004, 0);
            RefillablePotion = new Item(2031, 0);
            HuntersPotion = new Item(2032, 0);
            CorruptionPotion = new Item(2033, 0);
            uint level = (uint)Player.Instance.Level;
            Q = new Spell.Skillshot(SpellSlot.Q, 750, SkillShotType.Linear, 250, 2200, 60);
            W = new Spell.Skillshot(SpellSlot.W, 1620, SkillShotType.Circular, 500, 1000, 500);
            E = new Spell.Skillshot(SpellSlot.E, 400, SkillShotType.Linear, 250, int.MaxValue, 1);
            R = new Spell.Active(SpellSlot.R, 400);
            EkkoUlt = ObjectManager.Get<Obj_GeneralParticleEmitter>().FirstOrDefault(x => x.Name.Equals("Ekko_Base_R_TrailEnd.troy"));          

            Menu = MainMenu.AddMenu("Time Breaker Ekko", "ekko");
            
            ComboMenu = Menu.AddSubMenu("Kombo Ayarları","ComboSettings");            
            ComboMenu.AddLabel("Kombo Ayarları");
            ComboMenu.Add("QCombo", new CheckBox("Kullan Q"));
            ComboMenu.Add("WCombo", new CheckBox("Kullan W"));
            ComboMenu.Add("ECombo", new CheckBox("Kullan E"));
            ComboMenu.AddLabel("R Ayarları");
            ComboMenu.Add("SafeR", new CheckBox("Öleceksem R Kullan"));
            ComboMenu.Add("SafeRHP", new Slider("Can < %", 10, 1, 50));
            ComboMenu.Add("AutoR", new CheckBox("Otomatik R için Düşman Sayısı", false));
            ComboMenu.Add("AutoRCount", new Slider("Düşman Sayısı >=", 3, 1, 5));
            ComboMenu.Add("BaseR", new CheckBox("Laneden Üsse dön", false));

            HarassMenu = Menu.AddSubMenu("Dürtme Ayarları", "HarassSettings");
            HarassMenu.Add("QHarass", new CheckBox("Kullan Q"));
            HarassMenu.Add("QHarassMana", new Slider("Manam > %", 45, 0, 100));
            HarassMenu.Add("WHarass", new CheckBox("Kullan W"));
            HarassMenu.Add("WHarassMana", new Slider("Manam > %", 45, 0, 100));
            HarassMenu.Add("EHarass", new CheckBox("Kullan E"));
            HarassMenu.Add("EHarassMana", new Slider("Manam > %", 45, 0, 100));

            FarmingMenu = Menu.AddSubMenu("LaneTemizleme", "FarmSettings");
            FarmingMenu.AddLabel("LaneTemizleme Ayarları");
            FarmingMenu.Add("QLaneClear", new CheckBox("Kullan Q"));
            FarmingMenu.Add("QlaneclearMana", new Slider("Mana > %", 45, 0, 100));
            FarmingMenu.Add("WLaneClear", new CheckBox("Kullan W", false));
            FarmingMenu.Add("WlaneclearMana", new Slider("Manam > %", 35, 0, 100));
            FarmingMenu.Add("ELaneClear", new CheckBox("E Kullan"));
            FarmingMenu.Add("ElaneclearMana", new Slider("Manam > %", 60, 0, 100));

            FarmingMenu.AddLabel("Orman Temizleme Ayarları");
            FarmingMenu.Add("QJungleClear", new CheckBox("Q Kullan"));
            FarmingMenu.Add("QJungleClearMana", new Slider("Manam > %", 30, 0, 100));
            FarmingMenu.Add("WJungleClear", new CheckBox("W Kullan"));
            FarmingMenu.Add("WJungleClearMana", new Slider("Manam > %", 30, 0, 100));
            FarmingMenu.Add("EJungleClear", new CheckBox("E Kullan"));
            FarmingMenu.Add("EJungleClearMana", new Slider("Manam > %", 30, 0, 100));

            FarmingMenu.AddLabel("Son Vuruş Ayarları");
            FarmingMenu.Add("Qlasthit", new CheckBox("Q Kullan"));
            FarmingMenu.Add("QlasthitMana", new Slider("Mana > %", 35, 0, 100));

            Skin = Menu.AddSubMenu("Skin Değiştirici", "SkinChanger");
            Skin.Add("checkSkin", new CheckBox("Aktif"));
            Skin.Add("skin.Id", new Slider("SkinNumarası", 1, 0, 2));

            SetSmiteSlot();
            if (SmiteSlot != SpellSlot.Unknown)
            {
                SmiteMenu = Menu.AddSubMenu("Çarp Kullanımı", "SmiteUsage");
                SmiteMenu.Add("SmiteEnemy", new CheckBox("Düşmana kombo yaparken Kullan!"));               
                SmiteMenu.AddLabel("Çarp Kullan");
                SmiteMenu.Add("Use Smite?", new CheckBox("Çarp Kullan"));
                SmiteMenu.Add("Red?", new CheckBox("Kırmızı"));
                SmiteMenu.Add("Blue?", new CheckBox("Mavi"));
                SmiteMenu.Add("Dragon?", new CheckBox("Ejder"));
                SmiteMenu.Add("Baron?", new CheckBox("Baron"));
            }

            MiscMenu = Menu.AddSubMenu("Ek Ayarlar", "Misc");
            MiscMenu.AddLabel("Otomatik");
            MiscMenu.Add("Auto Ignite", new CheckBox("Otomatik Tutuştur"));
            MiscMenu.Add("FleeE", new CheckBox("Kaçarken E Kullan"));         
            MiscMenu.Add("autoW", new CheckBox("W için düşman say"));
            MiscMenu.Add("autoWCount", new Slider("Düşman Sayısı ", 3, 1, 5));
            MiscMenu.Add("autoWStunned", new CheckBox("Otomatik W ile stunla"));
            MiscMenu.Add("Interrupter", new CheckBox("Interrupt da  W Kullan"));
            MiscMenu.Add("Gapcloser", new CheckBox("Gapclose da Q-W Kullan"));

            MiscMenu.AddLabel("Kill Çalma");
            MiscMenu.Add("Qkill", new CheckBox("Kill Çalmada Q Kullan"));
            MiscMenu.Add("Ekill", new CheckBox("Kill Çalmada E Kullan"));
            MiscMenu.Add("Rkill", new CheckBox("Kill Çalmada R Kullan"));

            MiscMenu.AddLabel("Akti Edici");
            MiscMenu.Add("useHP", new CheckBox("Can İksiri Aktif"));           
            MiscMenu.Add("useHPV", new Slider("Canım < %", 45, 0, 100));
            MiscMenu.Add("useMana", new CheckBox("Mana İksiri Aktif"));
            MiscMenu.Add("useManaV", new Slider("Manam < %", 45, 0, 100));
            MiscMenu.Add("useCrystal", new CheckBox("Tüketilebilir İksirler"));
            MiscMenu.Add("useCrystalHPV", new Slider("Canım < %", 65, 0, 100));
            MiscMenu.Add("useCrystalManaV", new Slider("Manam < %", 65, 0, 100));

            DrawMenu = Menu.AddSubMenu("Gösterge Ayarları", "Drawings");
            DrawMenu.Add("drawQ", new CheckBox("Göster Q Menzili"));
            DrawMenu.Add("drawW", new CheckBox("Göster W Menzili"));
            DrawMenu.Add("drawE", new CheckBox("Göster E Menzili"));
            DrawMenu.Add("drawR", new CheckBox("Göster R Menzili"));

            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += Drawing_OnDraw;
            GameObject.OnCreate += GameObject_OnCreate;
            GameObject.OnDelete += GameObject_OnDelete;
            Dash.OnDash += Unit_OnDash;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
            Gapcloser.OnGapcloser += Gapcloser_OnGapCloser;

            Chat.Print("Time Breaker Ekko", System.Drawing.Color.ForestGreen);
            Chat.Print("v0.0.0.1", System.Drawing.Color.AliceBlue);
            Chat.Print("centilmen50", System.Drawing.Color.Red);
        }
        private static void SetSmiteSlot()
        {
            foreach (
                var spell in
                    _Player.Spellbook.Spells.Where(
                        spell => string.Equals(spell.Name, Smitetype, StringComparison.CurrentCultureIgnoreCase)))
            {
                SmiteSlot = spell.Slot;
            }
        }

        #region Player Damage
        public static float getComboDamageNoUlt(AIHeroClient hero)
        {
            double damage = 0;
            if (E.IsReady())
            {
                damage += _Player.GetSpellDamage(_Player, SpellSlot.E);
            }
            if (Q.IsReady())
            {
                damage += _Player.GetSpellDamage(_Player, SpellSlot.Q) * 3;
            }
            if (W.IsReady())
            {
                damage += (float)_Player.GetSpellDamage(_Player, SpellSlot.Q);
            }
            return (float)damage;
        }

        public static float getComboDamageUlt(AIHeroClient hero)
        {
            double damage = 0;
            if (R.IsReady())
            {
                damage += _Player.GetSpellDamage(_Player, SpellSlot.R);
            }
            if (E.IsReady())
            {
                damage += _Player.GetSpellDamage(_Player, SpellSlot.E);
            }
            if (Q.IsReady())
            {
                damage += _Player.GetSpellDamage(_Player, SpellSlot.Q) * 3;
            }
            if (W.IsReady())
            {
                damage += (float)_Player.GetSpellDamage(_Player, SpellSlot.Q);
            }
            return (float)damage;
        }
        #endregion

        private static void Game_OnTick(EventArgs args)
        {
            var HPpot = MiscMenu["useHP"].Cast<CheckBox>().CurrentValue;
            var Mpot = MiscMenu["useMana"].Cast<CheckBox>().CurrentValue;
            var Crystal = MiscMenu["useCrystal"].Cast<CheckBox>().CurrentValue;
            var HPv = MiscMenu["useHPv"].Cast<Slider>().CurrentValue;
            var Manav = MiscMenu["useManav"].Cast<Slider>().CurrentValue;
            var CrystalHPv = MiscMenu["useCrystalHPv"].Cast<Slider>().CurrentValue;
            var CrystalManav = MiscMenu["useCrystalManav"].Cast<Slider>().CurrentValue;
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            var igntarget = TargetSelector.GetTarget(600, DamageType.True);
            var t = TargetSelector.GetTarget(Q.Range, DamageType.Magical);

            if (_Player.SkinId != Skin["skin.Id"].Cast<Slider>().CurrentValue)
            {
                if (checkSkin())
                {
                    Player.SetSkinId(SkinId());
                }
            }

            if (Smite != null)
            {
                if (Smite.IsReady() && SmiteMenu["Use Smite?"].Cast<CheckBox>().CurrentValue)
                {
                    Obj_AI_Minion Mob = EntityManager.MinionsAndMonsters.GetJungleMonsters(_Player.Position, Smite.Range).FirstOrDefault();

                    if (Mob != default(Obj_AI_Minion))
                    {
                        bool kill = GetSmiteDamage() >= Mob.Health;

                        if (kill)
                        {
                            if ((Mob.Name.Contains("SRU_Dragon") || Mob.Name.Contains("SRU_Baron"))) Smite.Cast(Mob);
                            else if (Mob.Name.StartsWith("SRU_Red") && SmiteMenu["Red?"].Cast<CheckBox>().CurrentValue) Smite.Cast(Mob);
                            else if (Mob.Name.StartsWith("SRU_Blue") && SmiteMenu["Blue?"].Cast<CheckBox>().CurrentValue) Smite.Cast(Mob);
                        }
                    }
                }
            }

            if (HPpot && Player.Instance.HealthPercent < HPv)
            {
                if (Item.HasItem(Healthpot.Id) && Item.CanUseItem(Healthpot.Id) && !Player.HasBuff("RegenerationPotion"))
                {
                    Healthpot.Cast();
                }
                else if (Item.HasItem(RefillablePotion.Id) && Item.CanUseItem(RefillablePotion.Id) && !Player.HasBuff("RegenerationPotion") && !Player.HasBuff("FlaskOfCrystalWater") && !Player.HasBuff("ItemCrystalFlask"))
                {
                    RefillablePotion.Cast();
                }
            }

            if (Mpot && Player.Instance.ManaPercent < Manav)
            {
                if (Item.HasItem(Manapot.Id) && Item.CanUseItem(Manapot.Id) && !Player.HasBuff("FlaskOfCrystalWater") && !Player.HasBuff("ItemCrystalFlask"))
                {
                    Manapot.Cast();
                }
            }

                if (Crystal && Player.Instance.HealthPercent < CrystalHPv || Crystal && Player.Instance.ManaPercent < CrystalManav)
            {
                
                if (Item.HasItem(CorruptionPotion.Id) && Item.CanUseItem(CorruptionPotion.Id) && !Player.HasBuff("RegenerationPotion") && !Player.HasBuff("FlaskOfCrystalWater") && !Player.HasBuff("ItemCrystalFlask") && !Player.HasBuff("ItemDarkCrystalFlaskJungle"))
                {
                    CorruptionPotion.Cast();
                }
                else if (Item.HasItem(HuntersPotion.Id) && Item.CanUseItem(HuntersPotion.Id) && !Player.HasBuff("RegenerationPotion") && !Player.HasBuff("FlaskOfCrystalWater") && !Player.HasBuff("ItemCrystalFlask") && !Player.HasBuff("ItemCrystalFlaskJungle"))
                {
                    HuntersPotion.Cast();
                }

            }

            if(R.IsReady() && ComboMenu["SafeR"].Cast<CheckBox>().CurrentValue && _Player.HealthPercent <= ComboMenu["SafeRHP"].Cast<Slider>().CurrentValue)
            {
                if (_Player.IsUnderTurret())
                {
                    R.Cast();
                }
                foreach (var enemie in EntityManager.Heroes.Enemies)
                {
                    if(_Player.Distance(enemie) < 1200 && !enemie.IsDead)
                    {
                        R.Cast();
                    }
                }
                
            }


            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Combo();
                SmiteOnTarget(t);
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
            KillSteal();
        }

        public static void GameObject_OnCreate(GameObject sender, EventArgs args)
        {

            var particle = sender as Obj_GeneralParticleEmitter;
            if (particle != null)
            {
                if (particle.Name.Equals("Ekko_Base_R_TrailEnd.troy"))
                {
                    EkkoUlt = particle;
                }
            }
        }


        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe) return;
            double ShouldUseOn = ShouldUse(args.SData.Name);
            if (sender.Team != ObjectManager.Player.Team && ShouldUseOn >= 0f && sender.IsValidTarget(Q.Range))
            {

                if (MiscMenu["Interrupter"].Cast<CheckBox>().CurrentValue && W.IsReady() && _Player.Distance(sender) <= W.Range)
                {
                    if(W.MinimumHitChance >= HitChance.High)
                    {
                        W.Cast(sender);
                    }
                }

            }

            if (ComboMenu["SafeR"].Cast<CheckBox>().CurrentValue && R.IsReady())
            {
                if (_Player.HealthPercent <= ComboMenu["SafeRHP"].Cast<Slider>().CurrentValue)
                {
                    if (sender.IsEnemy && args.Target.IsMe)
                    {
                        if (_Player.CountEnemiesInRange(1300) > 1)
                        {
                            if (_Player.CountAlliesInRange(1300) >= 1 + 1)
                            {
                                R.Cast();
                                return;
                            }
                            if (_Player.CountAlliesInRange(1300) == 0 + 1)
                            {
                                if (sender.GetAutoAttackDamage(_Player) >= _Player.Health)
                                {
                                    R.Cast();
                                    return;
                                }
                            }
                        }
                        else if (sender.GetAutoAttackDamage(_Player) >= _Player.Health)
                        {
                            R.Cast();
                            return;
                        }
                    }
                }

            }


            if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Harass)
            {
                if (HarassMenu["QHarass"].Cast<CheckBox>().CurrentValue && sender.IsEnemy && args.Target.IsMe && Q.IsReady() && _Player.Distance(sender) <= Q.Range)
                {
                    if(Q.MinimumHitChance >= HitChance.High)
                    {
                        Q.Cast(sender);
                    }
                }
                Harass();
            }
        }

        public static int SkinId()
        {
            return Skin["skin.Id"].Cast<Slider>().CurrentValue;
        }
        public static bool checkSkin()
        {
            return Skin["checkSkin"].Cast<CheckBox>().CurrentValue;
        }


        static void Unit_OnDash(Obj_AI_Base sender, Dash.DashEventArgs args)
        {
            var useQ = ComboMenu["QCombo"].Cast<CheckBox>().CurrentValue;
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);

            if (!sender.IsEnemy) return;

            if (sender.NetworkId == target.NetworkId)
            {

                if (useQ && Q.IsReady() && args.EndPos.Distance(_Player) <= Q.Range)
                {

                    var delay = (int)(args.EndTick - Game.Time - 250 - 0.1f);
                    if (delay > 0)
                    {
                        Core.DelayAction(() => Q.Cast(args.EndPos), delay * 1000);
                    }
                    else
                    {
                        Q.Cast(args.EndPos);
                    }
                }
            }
        }

        public static void Gapcloser_OnGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (sender.IsMe) return;
            if (MiscMenu["Gapcloser"].Cast<CheckBox>().CurrentValue && Q.IsReady() && _Player.Distance(sender) < Q.Range)
            {
                Q.Cast(sender);
            }

            if (MiscMenu["Gapcloser"].Cast<CheckBox>().CurrentValue && W.IsReady() && _Player.Distance(sender) < Q.Range)
            {
                W.Cast(_Player.ServerPosition);
            }
        }

        public static void GameObject_OnDelete(GameObject sender, EventArgs args)
        {
            var particle = sender as Obj_GeneralParticleEmitter;
            if (particle != null)
            {
                if (particle.Name.Equals("Ekko_Base_R_TrailEnd.troy"))
                {
                    EkkoUlt = null;
                }
            }
        }

        public static double ShouldUse(string SpellName)
        {
            if (SpellName == "KatarinaR")
                return 0;
            if (SpellName == "AlZaharNetherGrasp")
                return 0;
            if (SpellName == "GalioIdolOfDurand")
                return 0;
            if (SpellName == "LuxMaliceCannon")
                return 0;
            if (SpellName == "MissFortuneBulletTime")
                return 0;
            if (SpellName == "CaitlynPiltoverPeacemaker")
                return 0;
            if (SpellName == "EzrealTrueshotBarrage")
                return 0;
            if (SpellName == "InfiniteDuress")
                return 0;
            if (SpellName == "VelkozR")
                return 0;
            if (SpellName == "XerathLocusOfPower2")
                return 0;
            if (SpellName == "Drain")
                return 0;
            if (SpellName == "Crowstorm")
                return 0;
            if (SpellName == "ReapTheWhirlwind")
                return 0;
            if (SpellName == "FallenOne")
                return 0;
            if (SpellName == "JudicatorIntervention")
                return 0;
            if (SpellName == "KennenShurikenStorm")
                return 0;
            if (SpellName == "LucianR")
                return 0;
            if (SpellName == "SoulShackles")
                return 0;
            if (SpellName == "NamiQ")
                return 0;
            if (SpellName == "AbsoluteZero")
                return 0;
            if (SpellName == "Pantheon_GrandSkyfall_Jump")
                return 0;
            if (SpellName == "RivenMartyr")
                return 0;
            if (SpellName == "RivenTriCleave_03")
                return 0;
            if (SpellName == "RunePrison")
                return 0;
            if (SpellName == "SkarnerImpale")
                return 0;
            if (SpellName == "UndyingRage")
                return 0;
            if (SpellName == "VarusQ")
                return 0;
            if (SpellName == "MonkeyKingSpinToWin")
                return 0;
            if (SpellName == "YasuoRKnockUpComboW")
                return 0;
            if (SpellName == "ZacE")
                return 0;
            if (SpellName == "ZacR")
                return 0;
            if (SpellName == "UrgotSwap2")
                return 0;
            return -1;
        }

        private static void SmiteOnTarget(AIHeroClient t)
        {
            var range = 700f;
            var use = SmiteMenu["SmiteEnemy"].Cast<CheckBox>().CurrentValue;
            var itemCheck = SmiteBlue.Any(i => Item.HasItem(i)) || SmiteRed.Any(i => Item.HasItem(i));
            if (itemCheck && use &&
                _Player.Spellbook.CanUseSpell(SmiteSlot) == SpellState.Ready &&
                t.Distance(_Player.Position) < range)
            {
                _Player.Spellbook.CastSpell(SmiteSlot, t);
            }
        }
        
        private static void Combo()
        {
            var useQ = ComboMenu["QCombo"].Cast<CheckBox>().CurrentValue;
            var useW = ComboMenu["WCombo"].Cast<CheckBox>().CurrentValue;
            var useE = ComboMenu["ECombo"].Cast<CheckBox>().CurrentValue;
            var useR = ComboMenu["SafeR"].Cast<CheckBox>().CurrentValue;

            if (useR && R.IsReady())
            {
                RLogic();
            }

            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            if (target.IsValidTarget())
            {

                #region Sort W combo mode
                if (useW && W.IsReady())
                {
                    WLogic();
                }
                #endregion

                #region Sort E combo mode
                if (useE && E.IsReady() && !W.IsReady())
                {
                    ELogic();
                }
                #endregion

                #region Sort Q combo mode
                if (useQ && Q.IsReady() && !W.IsReady())
                {
                    QLogic();
                }
                #endregion

            }
        }
        private static void KillSteal()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            var useQ = MiscMenu["Qkill"].Cast<CheckBox>().CurrentValue;

            if (Q.IsReady() && useQ && target.IsValidTarget(Q.Range) && !target.IsZombie && target.Health <= _Player.GetSpellDamage(target, SpellSlot.Q))
            {
                Q.Cast(target);
            }
        }
      
        private static void Harass()
        {
            var targetH = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            var useQ = HarassMenu["QHarass"].Cast<CheckBox>().CurrentValue;
            var useW = HarassMenu["WHarass"].Cast<CheckBox>().CurrentValue;
            var useE = HarassMenu["EHarass"].Cast<CheckBox>().CurrentValue;

            if (useQ && Q.IsReady() && _Player.Distance(targetH) <= Q.Range)
            {
                Q.Cast(targetH);
            }

            if (useE && E.IsReady() && targetH.GetBuffCount("EkkoStacks") == 2 && _Player.Distance(targetH) <= E.Range + 450)
            {
                E.Cast(targetH.ServerPosition);
            }

        }
        private static void JungleClear()
        {
            var useQ = FarmingMenu["QJungleClear"].Cast<CheckBox>().CurrentValue;
            var useQMana = FarmingMenu["QJungleClearMana"].Cast<Slider>().CurrentValue;
            var useW = FarmingMenu["WJungleClear"].Cast<CheckBox>().CurrentValue;
            var useWMana = FarmingMenu["WJungleClearMana"].Cast<Slider>().CurrentValue;
            var useE = FarmingMenu["EJungleClear"].Cast<CheckBox>().CurrentValue;
            var useEMana = FarmingMenu["EJungleClearMana"].Cast<Slider>().CurrentValue;
            foreach (var monster in EntityManager.MinionsAndMonsters.Monsters)
            {
                if (_Player.Distance(monster) <= W.Range && useW && Player.Instance.ManaPercent > useWMana)
                {
                    W.Cast(monster);
                }
                if (_Player.Distance(monster) <= Q.Range && useQ && Player.Instance.ManaPercent > useQMana)
                {
                    Q.Cast(monster);
                }
                if (_Player.Distance(monster) <= E.Range && useE && Player.Instance.ManaPercent > useEMana)
                {
                    E.Cast(monster);
                }
                
            }
        }
        private static void LaneClear()
        {
            var useQ = FarmingMenu["QLaneClear"].Cast<CheckBox>().CurrentValue;
            var useW = FarmingMenu["WLaneClear"].Cast<CheckBox>().CurrentValue;
            var useE = FarmingMenu["ELaneClear"].Cast<CheckBox>().CurrentValue;
            var Qmana = FarmingMenu["QlaneclearMana"].Cast<Slider>().CurrentValue;
            var Wmana = FarmingMenu["WlaneclearMana"].Cast<Slider>().CurrentValue;
            var EHP = FarmingMenu["ElaneclearMana"].Cast<Slider>().CurrentValue;
            var minions = ObjectManager.Get<Obj_AI_Base>().OrderBy(m => m.Health).Where(m => m.IsMinion && m.IsEnemy && !m.IsDead);
            foreach (var minion in minions)
            {
                if (useQ && Q.IsReady() && minion.IsValidTarget(Q.Range) && Player.Instance.ManaPercent > Qmana && minions.Count() >= 3)
                {
                    Q.Cast(minion);
                }
                if (useW && W.IsReady() && Player.Instance.ManaPercent > Wmana && minion.IsValidTarget(W.Range) && minions.Count() >= 3)
                {
                    W.Cast(minion);
                }
                if (useE && E.IsReady() && Player.Instance.HealthPercent > EHP)
                {
                    E.Cast(minion);
                }
            }
        }

        static float GetSmiteDamage()
        {
            float damage = new float();

            if (_Player.Level < 10) damage = 360 + (_Player.Level - 1) * 30;

            else if (_Player.Level < 15) damage = 280 + (_Player.Level - 1) * 40;

            else if (_Player.Level < 19) damage = 150 + (_Player.Level - 1) * 50;

            return damage;
        }

        private static void LastHit()
        {
            
        }

        public static void QLogic()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);

            if (_Player.CountEnemiesInRange(1300) > 1)
            {
                if (_Player.CountAlliesInRange(1300) >= 1 + 1)
                {
                    if (target.CountAlliesInRange(Q.Width) >= 1)
                    {
                        if (target.GetBuffCount("EkkoStacks") == 2)
                        {
                            if(Q.MinimumHitChance >= HitChance.High)
                            {
                                Q.Cast(target);
                            }                         
                            return;
                        }
                        if (target.GetBuffCount("EkkoStacks") < 2)
                        {
                            Q.Cast(target);
                            return;
                        }
                        return;
                    }
                    if (target.CountAlliesInRange(Q.Width) == 0)
                    {
                        if (Q.MinimumHitChance >= HitChance.High)
                        {
                            Q.Cast(target);
                        }
                        return;
                    }
                    return;
                }
                if (_Player.CountAlliesInRange(1300) == 0 + 1)
                {
                    if (target.CountAlliesInRange(Q.Width) >= 1)
                    {
                        Q.Cast(target);
                        return;
                    }
                    if (target.CountAlliesInRange(Q.Width) == 0)
                    {
                        if (Q.MinimumHitChance >= HitChance.High)
                        {
                            Q.Cast(target);
                        }
                        return;
                    }
                    return;
                }
                return;
            }

            if (_Player.CountEnemiesInRange(1300) == 1)
            {

                    Q.Cast(target);
                
                return;
            }
            return;
        }

        public static void WLogic()
        {
            var target = TargetSelector.GetTarget(E.Range + 450 + 200, DamageType.Magical);

            if (_Player.CountEnemiesInRange(1300) > 1)
            {
                if (target.CountAlliesInRange(W.Width) >= 1)
                {
                    if (_Player.HealthPercent <= target.HealthPercent)
                    {
                        if (_Player.CountEnemiesInRange(360) >= 1)
                        {
                            if (target.Distance(_Player) <= 360)
                            {
                                W.Cast(_Player.ServerPosition);
                                return;
                            }
                            if (target.Distance(_Player) > 360)
                            {
                                W.Cast(target);
                                return;
                            }
                            return;
                        }
                        if (_Player.CountEnemiesInRange(360) == 0)
                        {
                            W.Cast(target);
                            return;
                        }
                        return;
                    }

                    if (_Player.HealthPercent > target.HealthPercent)
                    {
                        if (_Player.HealthPercent >= 50)
                        {
                            if (target.GetBuffCount("EkkoStacks") == 2)
                            {
                                
                                    W.Cast(target);
                                
                                return;
                            }
                            if (target.GetBuffCount("EkkoStacks") < 2)
                            {
                                W.Cast(target);
                                return;
                            }
                            return;
                        }
                        if (_Player.HealthPercent < 50)
                        {
                            if (_Player.CountEnemiesInRange(360) >= 1)
                            {
                                if (target.Distance(_Player) <= 360)
                                {
                                    W.Cast(_Player.ServerPosition);
                                    return;
                                }
                                if (target.Distance(_Player) > 360)
                                {
                                    W.Cast(target);
                                    return;
                                }
                                return;
                            }
                            if (_Player.CountEnemiesInRange(360) == 0)
                            {
                                W.Cast(target);
                                return;
                            }
                            return;
                        }
                        return;
                    }
                    return;
                }

                if (target.CountAlliesInRange(W.Width) == 0)
                {
                    if (_Player.CountEnemiesInRange(360) >= 1)
                    {
                        if (target.Distance(_Player) <= 360)
                        {
                            W.Cast(_Player.ServerPosition);
                            return;
                        }
                        if (target.Distance(_Player) > 360)
                        {
                            
                                W.Cast(target);
                            
                            return;
                        }
                        return;
                    }
                    if (_Player.CountEnemiesInRange(360) == 0)
                    {
                        
                            W.Cast(target);
                        
                        return;
                    }
                    return;
                }
                return;
            }

            if (_Player.CountEnemiesInRange(1300) == 1)
            {
                
                    W.Cast(target);
                
                return;
            }
            return;
        }

        public static void ELogic()
        {
            var target = TargetSelector.GetTarget(E.Range + 425, DamageType.Magical);

            if (_Player.Distance(target) > 260)
            {
                if (_Player.HealthPercent >= 50 || R.IsReady())
                {
                    if (target.GetBuffCount("EkkoStacks") == 2)
                    {
                        E.Cast(target.ServerPosition);
                        return;
                    }
                    if (target.GetBuffCount("EkkoStacks") < 2)
                    {
                        E.Cast(target.ServerPosition);
                        return;
                    }
                    return;
                }
                if (_Player.HealthPercent < 50 && !R.IsReady())
                {
                    if (target.GetBuffCount("EkkoStacks") == 2)
                    {
                        E.Cast(target.ServerPosition);
                        return;
                    }
                    return;
                }
                return;
            }
            return;
        }

        public static void RLogic()
        {
            var EnemiesCDash = EntityManager.Heroes.Enemies.Count(x => x.IsValidTarget() && !x.IsDead && x.Distance(EkkoUlt.Position) > 385 && x.Distance(EkkoUlt.Position) < 800 && getComboDamageNoUlt(x) > x.Health);
            var EnemiesCNoDash = EntityManager.Heroes.Enemies.Count(x => x.IsValidTarget() && !x.IsDead && x.Distance(EkkoUlt.Position) < 385 && getComboDamageUlt(x) > x.Health);
            var CountEnemiesIn800 = EntityManager.Heroes.Enemies.Count(x => x.IsValidTarget() && !x.IsDead && x.Distance(EkkoUlt.Position) < 800);
            var CountAlliesIn1000 = EntityManager.Heroes.Allies.Count(x => x.IsValidTarget() && !x.IsDead && x.Distance(EkkoUlt.Position) < 1000);
            var CountEnemiesIn1100 = EntityManager.Heroes.Enemies.Count(x => x.IsValidTarget() && !x.IsDead && x.Distance(EkkoUlt.Position) < 1100);
            var CountAlliesIn1300 = EntityManager.Heroes.Allies.Count(x => x.IsValidTarget() && !x.IsDead && x.Distance(EkkoUlt.Position) < 1300);

            var target = TargetSelector.GetTarget(850, DamageType.Magical);
            if (_Player.CountEnemiesInRange(850) == 0 || getComboDamageNoUlt(target) < target.Health)
            {
                if (EnemiesCNoDash >= 1 && CountEnemiesIn800 <= 2)
                {
                    R.Cast();
                }
                if (EnemiesCNoDash >= 1 && CountEnemiesIn800 > 2 && CountAlliesIn1000 >= CountEnemiesIn800)
                {
                    R.Cast();
                }

                if (EnemiesCDash >= 1 && CountEnemiesIn1100 <= 2 && E.IsReady())
                {
                    R.Cast();
                }
                if (EnemiesCDash >= 1 && CountEnemiesIn1100 > 2 && CountAlliesIn1300 >= CountEnemiesIn1100 && E.IsReady())
                {
                    R.Cast();
                }
            }

        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (DrawMenu["drawQ"].Cast<CheckBox>().CurrentValue)
            {
                Drawing.DrawCircle(_Player.Position, Q.Range, Color.Red);
            }
            if (DrawMenu["drawW"].Cast<CheckBox>().CurrentValue)
            {
                Drawing.DrawCircle(_Player.Position, W.Range, Color.Red);
            }
            if (DrawMenu["drawE"].Cast<CheckBox>().CurrentValue)
            {
                Drawing.DrawCircle(_Player.Position, E.Range, Color.Red);
            }
            if (DrawMenu["drawR"].Cast<CheckBox>().CurrentValue)
            {
                Drawing.DrawCircle(EkkoUlt.Position, R.Range, Color.Red);
            }
        }
    }
}