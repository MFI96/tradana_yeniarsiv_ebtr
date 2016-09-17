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


namespace Perfect_Udyr
{

    class Program
    {
        public static Spell.Active Q;
        public static Spell.Active W;
        public static Spell.Active E;
        public static Spell.Active R;
        static Spell.Targeted Smite = null;
        public static Menu Menu, FarmingMenu, MiscMenu, DrawMenu, HarassMenu, ComboMenu, SmiteMenu, UpdateMenu, JungleMenu;
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
            if (Player.Instance.ChampionName != "Udyr")
                return;

            SpellDataInst smite = _Player.Spellbook.Spells.Where(spell => spell.Name.Contains("smite")).Any() ? _Player.Spellbook.Spells.Where(spell => spell.Name.Contains("smite")).First() : null;
            if (smite != null)
            {
                Smite = new Spell.Targeted(smite.Slot, 500);
            }
            Bootstrap.Init(null);

            Healthpot = new Item(2003, 0);
            Manapot = new Item(2004, 0);
            RefillablePotion = new Item(2031, 0);
            HuntersPotion = new Item(2032, 0);
            CorruptionPotion = new Item(2033, 0);
            uint level = (uint)Player.Instance.Level;
            Q = new Spell.Active(SpellSlot.Q, 250);
            W = new Spell.Active(SpellSlot.W, 250);
            E = new Spell.Active(SpellSlot.E, 250);
            R = new Spell.Active(SpellSlot.R, 500);

            Menu = MainMenu.AddMenu("Perfect Udyr", "perfectudyr");
            Menu.AddLabel("Perrrrrrrrrfect Ass");
            Menu.AddSeparator();

            

            ComboMenu = Menu.AddSubMenu("Combo Settings","ComboSettings");            
            ComboMenu.AddLabel("Combo Ayarları");
            ComboMenu.Add("QCombo", new CheckBox("Kullan Q"));
            ComboMenu.Add("WCombo", new CheckBox("Kullan W"));
            ComboMenu.Add("ECombo", new CheckBox("Kullan E"));
            ComboMenu.Add("RCombo", new CheckBox("Kullan R"));
            ComboMenu.Add("useTiamat", new CheckBox("Kullan İtemleri"));
            var Style = ComboMenu.Add("combostyle", new Slider("Kombo Stili", 0, 0, 1));
            Style.OnValueChange += delegate
            {
                Style.DisplayName = "Combo Style: " + new[] { "Tiger Combo", "Phoenix Combo" }[Style.CurrentValue];
            };
            Style.DisplayName = "Combo Style: " + new[] { "Tiger Combo", "Phoenix Combo" }[Style.CurrentValue];

            HarassMenu = Menu.AddSubMenu("Harass Settings", "HarassSettings");
            HarassMenu.AddLabel("None.");

            FarmingMenu = Menu.AddSubMenu("Lane Clear", "FarmSettings");

            FarmingMenu.AddLabel("Lane Temizleme");
            FarmingMenu.Add("QLaneClear", new CheckBox("Q Kullan"));
            FarmingMenu.Add("QlaneclearMana", new Slider("Mana < %", 45, 0, 100));
            FarmingMenu.Add("WLaneClear", new CheckBox("W Kullan"));
            FarmingMenu.Add("WlaneclearMana", new Slider("Mana < %", 35, 0, 100));
            FarmingMenu.Add("WlaneclearHealth", new Slider("Canım < %", 60, 0, 100));
            FarmingMenu.Add("RLaneClear", new CheckBox("R Kullan"));
            FarmingMenu.Add("RlaneclearMana", new Slider("Mana < %", 60, 0, 100));
            FarmingMenu.Add("RlaneclearCount", new Slider("Minyon Say > ", 3, 1, 10));

            FarmingMenu.AddLabel("Son Vuruş Ayarları");
            FarmingMenu.Add("Qlasthit", new CheckBox("Q Kullan"));
            FarmingMenu.Add("QlasthitMana", new Slider("Mana < %", 35, 0, 100));
            FarmingMenu.Add("Wlasthit", new CheckBox("W Kullan"));
            FarmingMenu.Add("WlasthitMana", new Slider("Mana < %", 35, 0, 100));
            FarmingMenu.Add("WlasthitHealth", new Slider("Canım < %", 60, 0, 100));


            JungleMenu = Menu.AddSubMenu("Jungle Clear", "JungSettings");

            JungleMenu.AddLabel("Orman Temizleme");
            JungleMenu.Add("Qjungle", new CheckBox("Q Kullan"));
            JungleMenu.Add("QjungleMana", new Slider("Mana < %", 30, 0, 100));
            JungleMenu.Add("Wjungle", new CheckBox("W Kullan"));
            JungleMenu.Add("WjungleMana", new Slider("Mana < %", 30, 0, 100));
            JungleMenu.Add("WjungleHealth", new Slider("Canım < %", 90, 0, 100));
            JungleMenu.Add("Ejungle", new CheckBox("E Kullan"));
            JungleMenu.Add("EjungleMana", new Slider("Mana < %", 30, 0, 100));
            JungleMenu.Add("Rjungle", new CheckBox("R Kullan"));
            JungleMenu.Add("RjungleMana", new Slider("Mana < %", 30, 0, 100));
            var JungleStyle = JungleMenu.Add("jungstyle", new Slider("Kombo Stili", 0, 0, 3));
            JungleStyle.OnValueChange += delegate
            {
                JungleStyle.DisplayName = "Combo Style: " + new[] { "Q-E", "Q-W", "R-E", "R-W" }[JungleStyle.CurrentValue];
            };
            JungleStyle.DisplayName = "Combo Style: " + new[] { "Q-E", "Q-W", "R-E", "R-W" }[JungleStyle.CurrentValue];

            


            SetSmiteSlot();
            if (SmiteSlot != SpellSlot.Unknown)
            {
                SmiteMenu = Menu.AddSubMenu("Smite Usage", "SmiteUsage");
                SmiteMenu.Add("SmiteEnemy", new CheckBox("Çarp Kullan Düşmana!"));               
                SmiteMenu.AddLabel("Çarp Kullan");
                SmiteMenu.Add("Use Smite?", new CheckBox("Kullan Çarp"));
                SmiteMenu.Add("Red?", new CheckBox("Kırmızı"));
                SmiteMenu.Add("Blue?", new CheckBox("Mavi"));
                SmiteMenu.Add("Dragon?", new CheckBox("Ejder"));
                SmiteMenu.Add("Baron?", new CheckBox("Baron"));
                SmiteMenu.Add("Small?", new CheckBox("Küçük Kamplar"));
            }


            MiscMenu = Menu.AddSubMenu("More Settings", "Misc");

            MiscMenu.AddLabel("Otomatik");
            MiscMenu.Add("Auto Ignite", new CheckBox("Tutuştur Kullan"));
            MiscMenu.Add("autoQ", new CheckBox("Kaçarken Q Kullan"));         
            MiscMenu.AddSeparator();
            MiscMenu.AddLabel("İtemler");
            MiscMenu.AddSeparator();
            MiscMenu.AddLabel("Mahvolmuş ve pala ayarları");
            MiscMenu.Add("botrkHP", new Slider("Benim Canım < %", 60, 0, 100));
            MiscMenu.Add("botrkenemyHP", new Slider("Düşmanın Canı < %", 60, 0, 100));

            MiscMenu.AddLabel("Kill Çalma");
            MiscMenu.Add("Qkills", new CheckBox("Q Kullan"));
            MiscMenu.Add("Ekills", new CheckBox("E Kullan"));

            MiscMenu.AddLabel("Activator");
            MiscMenu.Add("useHP", new CheckBox("Can potu Kullan"));           
            MiscMenu.Add("useHPV", new Slider("Canım < %", 45, 0, 100));
            MiscMenu.Add("useMana", new CheckBox("Mana potu Kullan"));
            MiscMenu.Add("useManaV", new Slider("Mana < %", 45, 0, 100));
            MiscMenu.Add("useCrystal", new CheckBox("Avcı Potu"));
            MiscMenu.Add("useCrystalHPV", new Slider("Canım  < %", 65, 0, 100));
            MiscMenu.Add("useCrystalManaV", new Slider("Manam < %", 65, 0, 100));

            DrawMenu = Menu.AddSubMenu("Draw Settings", "Drawings");
            DrawMenu.Add("drawAA", new CheckBox("Göster AA Menzili"));
            DrawMenu.Add("drawR", new CheckBox("Göster R Menzili"));

            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += Drawing_OnDraw;

            Chat.Print("Perfect Addon", System.Drawing.Color.Red);
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
        


    private static void Game_OnTick(EventArgs args)
        {
            var HPpot = MiscMenu["useHP"].Cast<CheckBox>().CurrentValue;
            var Mpot = MiscMenu["useMana"].Cast<CheckBox>().CurrentValue;
            var Crystal = MiscMenu["useCrystal"].Cast<CheckBox>().CurrentValue;
            var HPv = MiscMenu["useHPv"].Cast<Slider>().CurrentValue;
            var Manav = MiscMenu["useManav"].Cast<Slider>().CurrentValue;
            var CrystalHPv = MiscMenu["useCrystalHPv"].Cast<Slider>().CurrentValue;
            var CrystalManav = MiscMenu["useCrystalManav"].Cast<Slider>().CurrentValue;
            var useItem = ComboMenu["useTiamat"].Cast<CheckBox>().CurrentValue;
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            var igntarget = TargetSelector.GetTarget(600, DamageType.True);
            var t = TargetSelector.GetTarget(600, DamageType.Magical);

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
                if (Item.HasItem(RefillablePotion.Id) && Item.CanUseItem(RefillablePotion.Id) && !Player.HasBuff("RegenerationPotion") && !Player.HasBuff("FlaskOfCrystalWater") && !Player.HasBuff("ItemCrystalFlask"))
                {
                    RefillablePotion.Cast();
                }
                else if (Item.HasItem(CorruptionPotion.Id) && Item.CanUseItem(CorruptionPotion.Id) && !Player.HasBuff("RegenerationPotion") && !Player.HasBuff("FlaskOfCrystalWater") && !Player.HasBuff("ItemCrystalFlask") && !Player.HasBuff("ItemDarkCrystalFlaskJungle"))
                {
                    CorruptionPotion.Cast();
                }
                else if (Item.HasItem(HuntersPotion.Id) && Item.CanUseItem(HuntersPotion.Id) && !Player.HasBuff("RegenerationPotion") && !Player.HasBuff("FlaskOfCrystalWater") && !Player.HasBuff("ItemCrystalFlask") && !Player.HasBuff("ItemCrystalFlaskJungle"))
                {
                    HuntersPotion.Cast();
                }

            }

            if (useItem && target.IsValidTarget(400) && !target.IsDead && !target.IsZombie && target.HealthPercent < 100)
            {
                HandleItems();
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
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                Flee();
            }
            KillSteal();
            
        }
        public static double SpellToInterrupt(string SpellName)
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
        private static void Flee()
        {
            E.Cast();
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
            var target = TargetSelector.GetTarget(_Player.AttackRange + 800, DamageType.Physical);
            var useQ = ComboMenu["QCombo"].Cast<CheckBox>().CurrentValue;
            var useW = ComboMenu["WCombo"].Cast<CheckBox>().CurrentValue;
            var useE = ComboMenu["ECombo"].Cast<CheckBox>().CurrentValue;
            var useR = ComboMenu["RCombo"].Cast<CheckBox>().CurrentValue;
            var useItem = ComboMenu["useTiamat"].Cast<CheckBox>().CurrentValue;
            var style = ComboMenu["combostyle"].Cast<Slider>().CurrentValue;
            switch (style)
            {
                case 0:
                    if (E.IsReady() && _Player.Distance(target) > E.Range)
                    {
                        E.Cast();
                    }
                    else if (E.IsReady() && _Player.Distance(target) <= E.Range && !target.HasBuff("udyrbearstuncheck"))
                    {
                        E.Cast();
                    }
                    if (useW && W.IsReady() && _Player.Distance(target) <= W.Range && target.HasBuff("udyrbearstuncheck"))
                    {
                        W.Cast();
                    }
                    if (useR && R.IsReady() && !HavePhoenixAoe && _Player.Distance(target) <= R.Range && target.HasBuff("udyrbearstuncheck"))
                    {
                        R.Cast();
                    }
                    if (useQ && Q.IsReady() && _Player.Distance(target) <= Q.Range && target.HasBuff("udyrbearstuncheck"))
                    {
                        Q.Cast();
                    }
                    
                    break;
                case 1:
                    if (E.IsReady() && _Player.Distance(target) > E.Range)
                    {
                        E.Cast();
                    }
                    else if (E.IsReady() && _Player.Distance(target) <= E.Range && !target.HasBuff("udyrbearstuncheck"))
                    {
                        E.Cast();
                    }
                    if (useW && W.IsReady() && _Player.Distance(target) <= W.Range && target.HasBuff("udyrbearstuncheck"))
                    {
                        W.Cast();
                    }

                    if (useQ && Q.IsReady() && _Player.Distance(target) <= Q.Range && target.HasBuff("udyrbearstuncheck"))
                    {
                        Q.Cast();
                    }
                    if (useR && R.IsReady() && !HavePhoenixAoe && _Player.Distance(target) <= R.Range && target.HasBuff("udyrbearstuncheck"))
                    {
                        R.Cast();
                    }
                    
                    break;
                default:
                    if (E.IsReady() && _Player.Distance(target) > E.Range)
                    {
                        E.Cast();
                    }
                    else if (E.IsReady() && _Player.Distance(target) <= E.Range && !target.HasBuff("udyrbearstuncheck"))
                    {
                        E.Cast();
                    }
                    if (useW && W.IsReady() && _Player.Distance(target) <= W.Range && target.HasBuff("udyrbearstuncheck"))
                    {
                        W.Cast();
                    }
                    if (useR && R.IsReady() && !HavePhoenixAoe && _Player.Distance(target) <= R.Range && target.HasBuff("udyrbearstuncheck"))
                    {
                        R.Cast();
                    }
                    if (useQ && Q.IsReady() && _Player.Distance(target) <= Q.Range && target.HasBuff("udyrbearstuncheck"))
                    {
                        Q.Cast();
                    }
                    

                    break;
            }
            if (useItem && target.IsValidTarget(400) && !target.IsDead && !target.IsZombie)
            {
                HandleItems();
            }

        }
        private static void KillSteal()
        {
           
        }

        internal static void HandleItems()
        {
            var botrktarget = TargetSelector.GetTarget(550, DamageType.Physical);
            var youmutarget = TargetSelector.GetTarget(800, DamageType.Physical);
            var useItem = ComboMenu["useTiamat"].Cast<CheckBox>().CurrentValue;
            var useBotrkHP = MiscMenu["botrkHP"].Cast<Slider>().CurrentValue;
            var useBotrkEnemyHP = MiscMenu["botrkenemyHP"].Cast<Slider>().CurrentValue;
            //HYDRA
            if (useItem && Item.HasItem(3077) && Item.CanUseItem(3077))
                Item.UseItem(3077);

            //TİAMAT
            if (useItem && Item.HasItem(3074) && Item.CanUseItem(3074))
                Item.UseItem(3074);

            //NEW ITEM
            if (useItem && Item.HasItem(3748) && Item.CanUseItem(3748))
                Item.UseItem(3748);

            //BİLGEWATER CUTLASS
            if (useItem && Item.HasItem(3144) && Item.CanUseItem(3144) && botrktarget.HealthPercent <= useBotrkEnemyHP && _Player.HealthPercent <= useBotrkHP)
                Item.UseItem(3144, botrktarget);

            //BOTRK
            if (useItem && Item.HasItem(3153) && Item.CanUseItem(3153) && botrktarget.HealthPercent <= useBotrkEnemyHP && _Player.HealthPercent <= useBotrkHP)
                Item.UseItem(3153, botrktarget);

            //YOUMU
            if (useItem && Item.HasItem(3142) && Item.CanUseItem(3142) && youmutarget.IsValidTarget(800))
                Item.UseItem(3142);
        }

        private static void Harass()
        {

        }
        private static void LaneClear()
        {
            var useQ = FarmingMenu["QLaneClear"].Cast<CheckBox>().CurrentValue;
            var useW = FarmingMenu["WLaneClear"].Cast<CheckBox>().CurrentValue;
            var useR = FarmingMenu["RLaneClear"].Cast<CheckBox>().CurrentValue;
            var Qmana = FarmingMenu["QlaneclearMana"].Cast<Slider>().CurrentValue;
            var Wmana = FarmingMenu["WlaneclearMana"].Cast<Slider>().CurrentValue;
            var Rmana = FarmingMenu["RlaneclearMana"].Cast<Slider>().CurrentValue;
            var Whealth = FarmingMenu["WlaneclearHealth"].Cast<Slider>().CurrentValue;
            var RCount = FarmingMenu["RlaneclearCount"].Cast<Slider>().CurrentValue;
            var minions = ObjectManager.Get<Obj_AI_Base>().OrderBy(m => m.Health).Where(m => m.IsMinion && m.IsEnemy && !m.IsDead);
            foreach (var minion in minions)
            {
                if (useR && !isInRStance && R.IsReady() && _Player.ManaPercent >= Rmana && (_Player.HealthPercent > Whealth || !useW))
                {
                    if (minions.Count() >= RCount)
                    {
                        R.Cast();
                    }                      
                }
                if (useQ && !isInQStance && Q.IsReady() && _Player.ManaPercent >= Qmana && (_Player.HealthPercent > Whealth || !useW))
                {
                    Q.Cast();
                }
                else if (useW && !isInWStance && W.IsReady() && _Player.ManaPercent >= Wmana && _Player.HealthPercent <= Whealth)
                {
                    W.Cast();
                }
            }
        }
        private static void JungleClear()
        {
            var useQ = JungleMenu["Qjungle"].Cast<CheckBox>().CurrentValue;
            var useQMana = JungleMenu["QjungleMana"].Cast<Slider>().CurrentValue;
            var useW = JungleMenu["Wjungle"].Cast<CheckBox>().CurrentValue;
            var useWMana = JungleMenu["WjungleMana"].Cast<Slider>().CurrentValue;
            var useE = JungleMenu["Ejungle"].Cast<CheckBox>().CurrentValue;
            var useEMana = JungleMenu["EjungleMana"].Cast<Slider>().CurrentValue;
            var useR = JungleMenu["Rjungle"].Cast<CheckBox>().CurrentValue;
            var useRMana = JungleMenu["RjungleMana"].Cast<Slider>().CurrentValue;
            var useWHealth = JungleMenu["WjungleHealth"].Cast<Slider>().CurrentValue;
            var JungleStyle = JungleMenu["jungstyle"].Cast<Slider>().CurrentValue;
            foreach (var monster in EntityManager.MinionsAndMonsters.Monsters)
            {
                switch (JungleStyle)
                {
                    case 0:
                        if (useQ && Q.IsReady() && _Player.Distance(monster) < _Player.AttackRange && _Player.ManaPercent >= useQMana)
                        {
                            Q.Cast();
                        }
                        else if (useE && E.IsReady() && _Player.Distance(monster) < _Player.AttackRange && _Player.Mana > useEMana && _Player.ManaPercent >= useEMana && !monster.HasBuff("udyrbearstuncheck"))
                        {
                            E.Cast();
                        }
                        HandleItems();
                        break;
                    case 1:
                        if (useW && W.IsReady() && _Player.Distance(monster) < _Player.AttackRange && _Player.ManaPercent >= useWMana && _Player.HealthPercent <= useWHealth)
                        {
                            W.Cast();
                        }
                        else if (useQ && Q.IsReady() && _Player.Distance(monster) < _Player.AttackRange && _Player.ManaPercent >= useQMana)
                        {
                            Q.Cast();
                        }
                        
                        HandleItems();
                        break;
                    case 2:
                        if (useR && R.IsReady() && _Player.Distance(monster) < _Player.AttackRange && _Player.ManaPercent >= useRMana)
                        {
                            R.Cast();
                        }
                        else if (useE && E.IsReady() && _Player.Distance(monster) < _Player.AttackRange && _Player.Mana > useEMana && _Player.ManaPercent >= useEMana && !monster.HasBuff("udyrbearstuncheck"))
                        {
                            E.Cast();
                        }
                        HandleItems();
                        break;
                    case 3:
                       
                        if (useW && W.IsReady() && _Player.Distance(monster) < _Player.AttackRange && _Player.ManaPercent >= useWMana && _Player.HealthPercent <= useWHealth)
                        {
                            W.Cast();
                        }
                        else if (useR && R.IsReady() && _Player.Distance(monster) < _Player.AttackRange && _Player.ManaPercent >= useRMana)
                        {
                            R.Cast();
                        }
                        HandleItems();
                        break; 
                }
            }                   
                          
        }
        private static void LastHit()
        {
            var useQ = FarmingMenu["Qlasthit"].Cast<CheckBox>().CurrentValue;
            var useW = FarmingMenu["Wlasthit"].Cast<CheckBox>().CurrentValue;
            var Qmana = FarmingMenu["QlasthitMana"].Cast<Slider>().CurrentValue;
            var Wmana = FarmingMenu["WlasthitMana"].Cast<Slider>().CurrentValue;
            var WHealth = FarmingMenu["WlasthitHealth"].Cast<Slider>().CurrentValue;
            var minions = ObjectManager.Get<Obj_AI_Base>().OrderBy(m => m.Health).Where(m => m.IsMinion && m.IsEnemy && !m.IsDead);
            foreach (var minion in minions)
            {
                if (useQ && !isInQStance && Q.IsReady() && minion.Health < _Player.GetSpellDamage(minion, SpellSlot.Q) * 0.9 && _Player.ManaPercent >= Qmana && (_Player.HealthPercent > WHealth || !useW))
                {
                    Q.Cast();
                }

                else if (useW && !isInWStance && W.IsReady() && _Player.ManaPercent >= Wmana && _Player.HealthPercent <= WHealth)
                {
                    W.Cast();
                }
            }
        }
        public static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base unit, GameObjectProcessSpellCastEventArgs args)
        {

            double InterruptOn = SpellToInterrupt(args.SData.Name);
            if (unit.Team != ObjectManager.Player.Team && InterruptOn >= 0f && unit.IsValidTarget(800))
            {

                if ( E.IsReady() &&  _Player.Distance(unit) <= 800)
                {
                    if (!isInEStance)
                    {
                        E.Cast();
                    }
                    else
                        return;

                }

            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (DrawMenu["drawR"].Cast<CheckBox>().CurrentValue)
            {
                Drawing.DrawCircle(_Player.Position, R.Range, System.Drawing.Color.Red);
            }
            if (ComboMenu["combostyle"].Cast<CheckBox>().CurrentValue)
            {
                var enez = Drawing.WorldToScreen(_Player.Position);
                    Drawing.DrawText(enez[0] - 20,enez[1],System.Drawing.Color.OrangeRed,"sda" + ComboMenu["combostyle"].Cast<Slider>().CurrentValue);
                
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
        private static bool isInQStance
        {
            get { return ObjectManager.Player.HasBuff("UdyrTigerStance"); }
        }

        private static bool isInWStance
        {
            get { return ObjectManager.Player.HasBuff("UdyrTurtleStance"); }
        }

        private static bool isInEStance
        {
            get { return ObjectManager.Player.HasBuff("UdyrBearStance"); }
        }

        private static bool isInRStance
        {
            get { return ObjectManager.Player.HasBuff("UdyrPhoenixStance"); }
        }

        private static bool HavePhoenixAoe
        {
            get { return ObjectManager.Player.HasBuff("UdyrPhoenixActivation"); }
        }
    }
}