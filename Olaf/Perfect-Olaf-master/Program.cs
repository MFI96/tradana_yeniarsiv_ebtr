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


namespace Perfect_Olaf
{
    internal class OlafAxe
    {
        public GameObject Object { get; set; }
        public float NetworkId { get; set; }
        public Vector3 AxePos { get; set; }
        public double ExpireTime { get; set; }
    }

    class Program
    {
        public static Spell.Skillshot Q;
        public static Spell.Skillshot Q2;
        public static Spell.Active W;
        public static Spell.Targeted E;
        public static Spell.Active R;
        public static Menu Menu, SkillMenu, FarmingMenu, MiscMenu, DrawMenu, HarassMenu, ComboMenu, SmiteMenu, UpdateMenu;
        static Spell.Targeted Smite = null;
        private static readonly OlafAxe olafAxe = new OlafAxe();
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
            if (Player.Instance.ChampionName != "Olaf")
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
            Q = new Spell.Skillshot(SpellSlot.Q, 1000, SkillShotType.Linear, 250, 1550, 75)
            {
                AllowedCollisionCount = int.MaxValue, MinimumHitChance = HitChance.High
            };
            Q2 = new Spell.Skillshot(SpellSlot.Q, 900, SkillShotType.Linear, 250, 1550, 75)
            {
                AllowedCollisionCount = int.MaxValue, MinimumHitChance = HitChance.High
            };
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Targeted(SpellSlot.E, 325);
            R = new Spell.Active(SpellSlot.R);

            Menu = MainMenu.AddMenu("Perfect Olaf", "perfectolaf");
            
            ComboMenu = Menu.AddSubMenu("Combo Settings","ComboSettings");            
            ComboMenu.AddLabel("Kombo Ayarları");
            ComboMenu.Add("QCombo", new CheckBox("Kullan Q"));
            ComboMenu.Add("WCombo", new CheckBox("Kullan W"));
            ComboMenu.Add("ECombo", new CheckBox("Kullan E"));
            ComboMenu.Add("RCombo", new CheckBox("Kullan R"));
            ComboMenu.Add("useTiamat", new CheckBox("Kullan İtemler"));

            HarassMenu = Menu.AddSubMenu("Harass Settings", "HarassSettings");
            HarassMenu.AddLabel("Dürtme Ayarları");
            HarassMenu.Add("QHarass", new CheckBox("Kullan Q"));
            HarassMenu.Add("WHarass", new CheckBox("Kullan W"));
            HarassMenu.Add("EHarass", new CheckBox("Kullan E"));

            FarmingMenu = Menu.AddSubMenu("Lane Clear", "FarmSettings");

            FarmingMenu.AddLabel("Lane Temizleme");
            FarmingMenu.Add("QLaneClear", new CheckBox("Q Kullan"));
            FarmingMenu.Add("QlaneclearMana", new Slider("Mana < %", 45, 0, 100));
            FarmingMenu.Add("WLaneClear", new CheckBox("W Kullan"));
            FarmingMenu.Add("WlaneclearMana", new Slider("Mana < %", 45, 0, 100));
            FarmingMenu.Add("ELaneClear", new CheckBox("E Kullan"));
            FarmingMenu.Add("ElaneclearHP", new Slider("Canım < %", 10, 0, 100));

            FarmingMenu.AddLabel("Orman Temizleme");
            FarmingMenu.Add("Qjungle", new CheckBox("Q Kullan"));
            FarmingMenu.Add("QjungleMana", new Slider("Mana < %", 45, 0, 100));
            FarmingMenu.Add("Wjungle", new CheckBox("W Kullan"));
            FarmingMenu.Add("WjungleMana", new Slider("Mana < %", 45, 0, 100));
            FarmingMenu.Add("Ejungle", new CheckBox("E Kullan"));
            FarmingMenu.Add("EjungleHP", new Slider("Canım < %", 25, 0, 100));

            FarmingMenu.AddLabel("SonVuruş Ayarları");
            FarmingMenu.Add("Qlasthit", new CheckBox("Q Kullan"));
            FarmingMenu.Add("Elasthit", new CheckBox("E Kullan"));
            FarmingMenu.Add("QlasthitMana", new Slider("Mana < %", 45, 0, 100));

            SetSmiteSlot();
            if (SmiteSlot != SpellSlot.Unknown)
            {
                SmiteMenu = Menu.AddSubMenu("Smite Usage", "SmiteUsage");
                SmiteMenu.AddLabel("Çarp Kullanımı");
                SmiteMenu.Add("Use Smite?", new CheckBox("Çarp Kullan"));
                SmiteMenu.Add("SmiteEnemy", new CheckBox("Komboda düşmana çarp kullan!"));
                SmiteMenu.Add("Red?", new CheckBox("Kırmızı"));
                SmiteMenu.Add("Blue?", new CheckBox("Mavi"));
                SmiteMenu.Add("Dragon?", new CheckBox("Ejder"));
                SmiteMenu.Add("Baron?", new CheckBox("Baron"));
            }
            

            MiscMenu = Menu.AddSubMenu("More Settings", "Misc");

            MiscMenu.AddLabel("Otomatik Ayarlar");
            MiscMenu.Add("Auto Ignite", new CheckBox("Otomatik Tutuştur"));
            MiscMenu.Add("autoQ", new CheckBox("Kaçarken otomatik Q Kullan"));         
            MiscMenu.Add("autoR", new CheckBox("Tehlikeli durumlarda Otomatik R",false));
            MiscMenu.Add("autoEenemyHP", new Slider("Düşman Canı < %", 45, 0, 100));
            MiscMenu.AddSeparator();
            MiscMenu.AddLabel("İtemler");
            MiscMenu.AddLabel("Mahvolmuş ve Bilge Palası");
            MiscMenu.Add("botrkHP", new Slider("Canım < %", 60, 0, 100));
            MiscMenu.Add("botrkenemyHP", new Slider("Düşmanın canı < %", 60, 0, 100));

            MiscMenu.AddLabel("KillÇalma");
            MiscMenu.Add("Qkill", new CheckBox("Q Kullan"));
            MiscMenu.Add("Ekill", new CheckBox("E Kullan"));

            MiscMenu.AddLabel("Aktivator");
            MiscMenu.Add("useHP", new CheckBox("Can İksiri Kullan"));           
            MiscMenu.Add("useHPV", new Slider("Canım < %", 45, 0, 100));
            MiscMenu.Add("useMana", new CheckBox("Mana iksiri kullan"));
            MiscMenu.Add("useManaV", new Slider("Mana < %", 45, 0, 100));
            MiscMenu.Add("useCrystal", new CheckBox("Dolduralabilir iksir kullan"));
            MiscMenu.Add("useCrystalHPV", new Slider("Canım < %", 45, 0, 100));
            MiscMenu.Add("useCrystalManaV", new Slider("Mana < %", 45, 0, 100));

            DrawMenu = Menu.AddSubMenu("Draw Settings", "Drawings");
            DrawMenu.Add("drawAA", new CheckBox("Göster AA Menzili"));
            DrawMenu.Add("drawQ", new CheckBox("Göster Q"));
            DrawMenu.Add("drawQpos", new CheckBox("Göster Q Pozisyonu"));
            DrawMenu.Add("drawE", new CheckBox("Göster E"));

            UpdateMenu = Menu.AddSubMenu("Last Update Logs", "Updates");
            UpdateMenu.AddLabel("V0.1.7.0");
            UpdateMenu.AddLabel("-Q Prediction UPDATE! Please Change Prediction Settings");
            UpdateMenu.AddLabel("Prediction");
            UpdateMenu.AddLabel("  Algorithm");
            UpdateMenu.AddLabel("    Hitchance = 4");
            UpdateMenu.AddLabel("  Collision");
            UpdateMenu.AddLabel("    Extra Hitbox Radius = 40");

            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += Drawing_OnDraw;
            GameObject.OnCreate += GameObject_OnCreate;
            GameObject.OnDelete += GameObject_OnDelete;

            Chat.Print("Perrrrrrrrrfect Addon made by Centilmen50, Cevirmen tradana", System.Drawing.Color.Red);
        }
        private static void GameObject_OnCreate(GameObject obj, EventArgs args)
        {
            if (obj.Name == "olaf_axe_totem_team_id_green.troy")
            {
                olafAxe.Object = obj;
                olafAxe.ExpireTime = Game.Time + 8;
                olafAxe.NetworkId = obj.NetworkId;
                olafAxe.AxePos = obj.Position;
            }
        }
        private static void GameObject_OnDelete(GameObject obj, EventArgs args)
        {
            if (obj.Name == "olaf_axe_totem_team_id_green.troy")
            {
                olafAxe.Object = null;
            }
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
            var t = TargetSelector.GetTarget(600, DamageType.True);

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
            KillSteal();
            autoR();            
        }
        private static void Combo()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            var useQ = ComboMenu["QCombo"].Cast<CheckBox>().CurrentValue;
            var useW = ComboMenu["WCombo"].Cast<CheckBox>().CurrentValue;
            var useE = ComboMenu["ECombo"].Cast<CheckBox>().CurrentValue;
            var useR = ComboMenu["RCombo"].Cast<CheckBox>().CurrentValue;
            var useItem = ComboMenu["useTiamat"].Cast<CheckBox>().CurrentValue;

            if (useQ && Q.IsReady() && Q.GetPrediction(target).HitChance >= HitChance.Medium && !target.IsDead && !target.IsZombie && target.IsFacing(_Player))
            {
                Q.Cast(target);               
            }
            else if (useQ && Q.IsReady() && Q.GetPrediction(target).HitChance >= HitChance.Medium && !target.IsDead && !target.IsZombie && !target.IsFacing(_Player))
            {
                Q.Cast(target);
            }
            if (W.IsReady() && useW && target.IsValidTarget(300) && !target.IsDead && !target.IsZombie)
            {
                W.Cast();
            }
            if (E.IsReady() && useE && target.IsValidTarget(E.Range) && !target.IsDead && !target.IsZombie)
            {
                E.Cast(target);
            }          
            if (useItem && !target.IsDead && !target.IsZombie)
            {
                HandleItems();
            }
            if (useR && R.IsReady() && target.IsValidTarget(800) && !target.IsDead && !target.IsZombie)
            {
                R.Cast();
            }


        }
        private static void KillSteal()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            var useQ = MiscMenu["Qkill"].Cast<CheckBox>().CurrentValue;
            var useE = MiscMenu["Ekill"].Cast<CheckBox>().CurrentValue;

            if (Q.IsReady() && useQ && target.IsValidTarget(Q.Range) && !target.IsZombie && target.Health <= _Player.GetSpellDamage(target, SpellSlot.Q))
            {
                Q.Cast(target);
            }
            if (E.IsReady() && useE && target.IsValidTarget(E.Range) && !target.IsZombie && target.Health <= _Player.GetSpellDamage(target, SpellSlot.E))
            {
                E.Cast(target);
            }
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

        internal static void HandleItems()
        {
            var botrktarget = TargetSelector.GetTarget(550, DamageType.Physical);
            var youmutarget = TargetSelector.GetTarget(800, DamageType.Physical);
            var target = TargetSelector.GetTarget(600, DamageType.Physical);
            var useItem = ComboMenu["useTiamat"].Cast<CheckBox>().CurrentValue;
            var useBotrkHP = MiscMenu["botrkHP"].Cast<Slider>().CurrentValue;
            var useBotrkEnemyHP = MiscMenu["botrkenemyHP"].Cast<Slider>().CurrentValue;
            //HYDRA
            if (useItem && Item.HasItem(3077) && Item.CanUseItem(3077) && target.IsValidTarget(400))
                Item.UseItem(3077);

            //TİAMAT
            if (useItem && Item.HasItem(3074) && Item.CanUseItem(3074) && target.IsValidTarget(400))
                Item.UseItem(3074);

            //NEW ITEM
            if (useItem && Item.HasItem(3748) && Item.CanUseItem(3748) && target.IsValidTarget(_Player.AttackRange))
                Item.UseItem(3748);

            //BİLGEWATER CUTLASS
            if (useItem && Item.HasItem(3144) && Item.CanUseItem(3144) && botrktarget.HealthPercent <= useBotrkEnemyHP && _Player.HealthPercent <= useBotrkHP && botrktarget.IsValidTarget(550))
                Item.UseItem(3144, botrktarget);

            //BOTRK
            if (useItem && Item.HasItem(3153) && Item.CanUseItem(3153) && botrktarget.HealthPercent <= useBotrkEnemyHP && _Player.HealthPercent <= useBotrkHP && botrktarget.IsValidTarget(550))
                Item.UseItem(3153, botrktarget);

            //YOUMU
            if (useItem && Item.HasItem(3142) && Item.CanUseItem(3142) && youmutarget.IsValidTarget(800))
                Item.UseItem(3142);
        }

        private static void Harass()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            var useQ = HarassMenu["QHarass"].Cast<CheckBox>().CurrentValue;
            var useW = HarassMenu["WHarass"].Cast<CheckBox>().CurrentValue;
            var useE = HarassMenu["EHarass"].Cast<CheckBox>().CurrentValue;

            if (Q.IsReady() && useQ && target.IsValidTarget(Q.Range) && !target.IsDead && !target.IsZombie)
            {
                Q.Cast(target);
            }
            if (W.IsReady() && useW && target.IsValidTarget(_Player.AttackRange) && !target.IsDead && !target.IsZombie)
            {
                W.Cast();
            }
            if (E.IsReady() && useE && target.IsValidTarget(E.Range) && !target.IsDead && !target.IsZombie)
            {
                E.Cast(target);
            }

        }
        private static void LaneClear()
        {
            var useQ = FarmingMenu["QLaneClear"].Cast<CheckBox>().CurrentValue;
            var useW = FarmingMenu["WLaneClear"].Cast<CheckBox>().CurrentValue;
            var useE = FarmingMenu["ELaneClear"].Cast<CheckBox>().CurrentValue;
            var Qmana = FarmingMenu["QlaneclearMana"].Cast<Slider>().CurrentValue;
            var Wmana = FarmingMenu["WlaneclearMana"].Cast<Slider>().CurrentValue;
            var EHP = FarmingMenu["ElaneclearHP"].Cast<Slider>().CurrentValue;
            var minions = ObjectManager.Get<Obj_AI_Base>().OrderBy(m => m.Health).Where(m => m.IsMinion && m.IsEnemy && !m.IsDead);
            foreach (var minion in minions)
            {
                if (useQ && Q.IsReady() && !minion.IsValidTarget(_Player.AttackRange) && minion.IsValidTarget(Q.Range) && Player.Instance.ManaPercent > Qmana && minion.Health <= _Player.GetSpellDamage(minion, SpellSlot.Q) && minions.Count() > 1)
                {
                    Q.Cast(minion);
                }
                if (useW && W.IsReady() && Player.Instance.ManaPercent > Wmana && minion.IsValidTarget(_Player.AttackRange) && Player.Instance.HealthPercent < 35)
                {
                    W.Cast();
                }
                if (useE && E.IsReady() && Player.Instance.HealthPercent > EHP && minion.Health <= _Player.GetSpellDamage(minion, SpellSlot.E) && !minion.IsValidTarget(_Player.AttackRange))
                {
                    E.Cast(minion);
                }
            }
        }
        private static void JungleClear()
        {
            var useQ = FarmingMenu["Qjungle"].Cast<CheckBox>().CurrentValue;
            var useQMana = FarmingMenu["QjungleMana"].Cast<Slider>().CurrentValue;
            var useW = FarmingMenu["Wjungle"].Cast<CheckBox>().CurrentValue;
            var useWMana = FarmingMenu["WjungleMana"].Cast<Slider>().CurrentValue;
            var useE = FarmingMenu["Ejungle"].Cast<CheckBox>().CurrentValue;
            var useEHP = FarmingMenu["EjungleHP"].Cast<Slider>().CurrentValue;
            foreach (var monster in EntityManager.MinionsAndMonsters.Monsters)
            {
                if (useQ && Q.IsReady() && Player.Instance.ManaPercent > useQMana)
                {
                    Q.Cast(monster);
                }
                if (useW && W.IsReady() && Player.Instance.ManaPercent > useWMana)
                {
                    W.Cast();
                }
                if (useE && E.IsReady() && Player.Instance.HealthPercent > useEHP)
                {
                    E.Cast(monster);
                }

                HandleItems();
            }
        }
        private static void LastHit()
        {
            var useQ = FarmingMenu["Qlasthit"].Cast<CheckBox>().CurrentValue;
            var useE = FarmingMenu["Elasthit"].Cast<CheckBox>().CurrentValue;
            var mana = FarmingMenu["QlasthitMana"].Cast<Slider>().CurrentValue;
            var minions = ObjectManager.Get<Obj_AI_Base>().OrderBy(m => m.Health).Where(m => m.IsMinion && m.IsEnemy && !m.IsDead);
            foreach (var minion in minions)
            {
                if (useQ && Q.IsReady() && !minion.IsValidTarget(E.Range) && minion.IsValidTarget(Q.Range) && Player.Instance.ManaPercent > mana && minion.Health <= _Player.GetSpellDamage(minion, SpellSlot.Q))
                {
                    Q.Cast(minion);
                }
                if (useE && E.IsReady() && minion.Health <= _Player.GetSpellDamage(minion, SpellSlot.E))
                {
                    E.Cast(minion);
                }
            }
        }
        private static void autoR()
        {
            var autoR = MiscMenu["autoR"].Cast<CheckBox>().CurrentValue;

            if (autoR && R.IsReady() && _Player.HasBuffOfType(BuffType.Stun)
            || _Player.HasBuffOfType(BuffType.Fear) 
            || _Player.HasBuffOfType(BuffType.Charm) 
            || _Player.HasBuffOfType(BuffType.Silence) 
            || _Player.HasBuffOfType(BuffType.Snare) 
            || _Player.HasBuffOfType(BuffType.Taunt)
            || _Player.HasBuffOfType(BuffType.Suppression)
            || _Player.HasBuffOfType(BuffType.Sleep)
            || _Player.HasBuffOfType(BuffType.Polymorph)
            || _Player.HasBuffOfType(BuffType.Frenzy)
            || _Player.HasBuffOfType(BuffType.Disarm)
            || _Player.HasBuffOfType(BuffType.NearSight)
            || _Player.HasBuffOfType(BuffType.Blind))
            {
                R.Cast();
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
        private static void Drawing_OnDraw(EventArgs args)
        {
            var drawAxePosition = DrawMenu["drawQpos"].Cast<CheckBox>().CurrentValue;

            if (drawAxePosition && olafAxe.Object != null)
            {
                new Circle() { Color = Color.Green, BorderWidth = 6, Radius = 85 }.Draw(olafAxe.Object.Position);
            }
            if (DrawMenu["drawQ"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = Color.Red, BorderWidth = 1, Radius = Q.Range }.Draw(_Player.Position);
            }
            if (DrawMenu["drawE"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = Color.Red, BorderWidth = 1, Radius = E.Range }.Draw(_Player.Position);
            }
        }
    }
}