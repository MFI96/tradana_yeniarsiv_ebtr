namespace Darius
{
    // The Dank Memes Master

    using System;
    using System.Globalization;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;
    using EloBuddy.SDK.Rendering;

    using SharpDX;

    internal class KappaDarius
    {
        public const string ChampName = "Darius";

        public static int passiveCounter;

        public static readonly Item Hydra = new Item(ItemId.Ravenous_Hydra_Melee_Only, 250f);

        public static readonly Item Titanic = new Item(ItemId.Titanic_Hydra, Player.Instance.GetAutoAttackRange());

        public static readonly Item Timat = new Item(ItemId.Tiamat_Melee_Only, 250f);

        public static readonly Item Cutlass = new Item((int)ItemId.Bilgewater_Cutlass, 550);

        public static readonly Item Botrk = new Item((int)ItemId.Blade_of_the_Ruined_King, 550);

        public static readonly Item Youmuu = new Item((int)ItemId.Youmuus_Ghostblade);

        public static Menu QMenu { get; private set; }

        public static Menu WMenu { get; private set; }

        public static Menu EMenu { get; private set; }

        public static Menu RMenu { get; private set; }

        public static Menu ManaMenu { get; private set; }

        public static Menu ItemsMenu { get; private set; }

        public static Menu KillStealMenu { get; private set; }

        public static Menu DrawMenu { get; private set; }

        private static Menu menuIni;

        public static Spell.Active Q { get; private set; }

        public static Spell.Active W { get; private set; }

        public static Spell.Skillshot E { get; private set; }

        public static Spell.Targeted R { get; private set; }

        public static Spell.Targeted Ignite;

        public static HpBarIndicator Hpi = new HpBarIndicator();

        public static void Execute()
        {
            if (Player.Instance.ChampionName != ChampName)
            {
                return;
            }

            menuIni = MainMenu.AddMenu("KappaDarius", "KappaDarius");
            menuIni.AddGroupLabel("Darius The Dank Memes Master!");
            menuIni.AddGroupLabel("Çeviri tradana");
            menuIni.AddGroupLabel("Genel Ayarlar");
            menuIni.Add("Items", new CheckBox("İtemleri Kullan?"));
            menuIni.Add("Combo", new CheckBox("Kombo Kullan?"));
            menuIni.Add("Harass", new CheckBox("Dürtme KUllan?"));
            menuIni.Add("Clear", new CheckBox("QSS Kullan?"));
            menuIni.Add("Drawings", new CheckBox("Göstergeler Aç?"));
            menuIni.Add("KillSteal", new CheckBox("Killçal?"));

            QMenu = menuIni.AddSubMenu("Q Settings");
            QMenu.AddGroupLabel("Q Ayarları");
            QMenu.Add("Combo", new CheckBox("Kombo'da Q"));
            QMenu.Add("Harass", new CheckBox("Dürtmede Q"));
            QMenu.AddGroupLabel("Lanetemizleme Ayarı Q için");
            QMenu.Add("Clear", new CheckBox("Q İle Lanetemizleme"));
            QMenu.Add("Qlc", new Slider("Q için en az minyon >=", 3, 1, 10));
            QMenu.AddSeparator();
            QMenu.AddGroupLabel("Ek Ayarlar");
            QMenu.Add("QE", new CheckBox("Her zaman Qdan Önce E", false));
            QMenu.Add("Stick", new CheckBox("Q atılacak Hedefe Doğru Yürü"));
            QMenu.Add("QAA", new CheckBox("Diğer düz vuruşu beklerken Q Kullan", false));
            QMenu.Add("range", new CheckBox("Düşman düzvuruş menzilindeyken Q atma", false));
            QMenu.Add("Flee", new CheckBox("Kaçarken Q (Hedef Takip Etmeyi Yok Say)"));
            QMenu.Add("QFlee", new Slider("Kaçarken Q için canım şundan az %", 90, 0, 100));
            QMenu.Add("Qaoe", new CheckBox("Alan Hasarı için Otomatik Q"));
            QMenu.Add("Qhit", new Slider("Q kaç kişiye vuracaksa >=", 3, 1, 5));

            WMenu = menuIni.AddSubMenu("W Settings");
            WMenu.AddGroupLabel("W Ayarları");
            WMenu.Add("Combo", new CheckBox("Komboda W"));
            WMenu.Add("Harass", new CheckBox("Dürtmede W"));
            WMenu.Add("Clear", new CheckBox("Lanetemizlemede W"));
            WMenu.AddGroupLabel("Ek Ayarlar");
            WMenu.Add("AAr", new CheckBox("W ile Düzvuruş Resetle"));

            EMenu = menuIni.AddSubMenu("E Settings");
            EMenu.AddGroupLabel("E Ayarları");
            EMenu.Add("Combo", new CheckBox("Komboda E"));
            EMenu.Add("Harass", new CheckBox("Dürtmede E"));
            EMenu.AddGroupLabel("Ek Ayarları");
            EMenu.Add("Interrupt", new CheckBox("Tehlikeli yeteneği bozmak için E kullan"));

            RMenu = menuIni.AddSubMenu("R Settings");
            RMenu.AddGroupLabel("R Ayarları");
            RMenu.Add("Combo", new CheckBox("R ile Komboyu bitir(hedefi mahvet)"));
            RMenu.Add("stack", new CheckBox("R kullanmak için Yük(kanama)", false));
            RMenu.Add("count", new Slider("R için Yük Say >=", 5, 0, 5));
            RMenu.Add("SaveR", new CheckBox("Eğer hedef düzvuruşla ölecek mesafedeyse R kullanma", false));
            RMenu.Add("SR", new Slider("Eğer hedef şu kadar düzvuruşla ölecekse R Kullanma X", 1, 0, 6));
            RMenu.Add("semiR", new KeyBind("Yarı Otomatik R", false, KeyBind.BindTypes.HoldActive));

            KillStealMenu = menuIni.AddSubMenu("KillSteal");
            KillStealMenu.AddGroupLabel("KillÇalma Ayarları");
            KillStealMenu.Add("Rks", new CheckBox("R İle çal"));

            if (Player.Spells.FirstOrDefault(o => o.SData.Name.Contains("SummonerDot")) != null)
            {
                KillStealMenu.Add("IGP", new CheckBox("Tutuştur+Kanamayla öldür"));
                KillStealMenu.Add("IG", new CheckBox("Sadece Tutuştur", false));
                KillStealMenu.AddLabel("Tutuştur ve Kanamanın hasarını hesaplayarak hareket et");
                Ignite = new Spell.Targeted(ObjectManager.Player.GetSpellSlotFromName("summonerdot"), 600);
            }

            ManaMenu = menuIni.AddSubMenu("Mana Manager");
            ManaMenu.AddGroupLabel("Dürtme");
            ManaMenu.Add("harassmana", new Slider("En az mana %", 75, 0, 100));
            ManaMenu.AddGroupLabel("Lanetemizleme");
            ManaMenu.Add("lanemana", new Slider("En az mana %", 60, 0, 100));

            ItemsMenu = menuIni.AddSubMenu("Items");
            ItemsMenu.AddGroupLabel("İtem Ayarları");
            ItemsMenu.Add("Hydra", new CheckBox("Kullan Hydra / Timat / Haşmetli Hydra"));
            ItemsMenu.Add("useGhostblade", new CheckBox("Kullan Youmuu'nun kılıcı"));
            ItemsMenu.Add("UseBOTRK", new CheckBox("Mahvolmuş Kılıç Kullan"));
            ItemsMenu.Add("UseBilge", new CheckBox("BilgeWater Palası Kullan"));
            ItemsMenu.AddSeparator();
            ItemsMenu.Add("eL", new Slider("Kullanmak için düşmanın canı", 65, 0, 100));
            ItemsMenu.Add("oL", new Slider("Kullanmak için benim canım", 65, 0, 100));

            DrawMenu = menuIni.AddSubMenu("Drawings");
            DrawMenu.AddGroupLabel("Gösterge Ayarları");
            DrawMenu.Add("Q", new CheckBox("Göster Q"));
            DrawMenu.Add("W", new CheckBox("Göster W"));
            DrawMenu.Add("E", new CheckBox("Göster E"));
            DrawMenu.Add("R", new CheckBox("Göster R"));
            DrawMenu.AddSeparator();
            DrawMenu.AddGroupLabel("Ulti Göstergesi");
            DrawMenu.Add("DrawD", new CheckBox("T hasarını Göster"));
            DrawMenu.Add("Killable", new CheckBox("Ölecek hedefi Göster"));
            DrawMenu.Add("Stacks", new CheckBox("Pasif Yükü Göster"));
            DrawMenu.Add("PPx", new Slider("Pasif Yük Pozisyonu X", 100, 0, 150));
            DrawMenu.Add("PPy", new Slider("Pasid Yük Pozisyonu Y", 100, 0, 150));
            DrawMenu.Add("RHealth", new CheckBox("R den sonraki canı göster"));
            DrawMenu.Add("RHx", new Slider("Rden sonra Can Pozisyonu", 135, 0, 150));

            Q = new Spell.Active(SpellSlot.Q, 400);
            W = new Spell.Active(SpellSlot.W, 300);
            E = new Spell.Skillshot(SpellSlot.E, 550, SkillShotType.Cone, 250, 666, 100);
            R = new Spell.Targeted(SpellSlot.R, 460);

            Game.OnUpdate += OnUpdate;
            Drawing.OnDraw += OnDraw;
            Drawing.OnEndScene += OnEndScene;
            Orbwalker.OnPostAttack += OnAfterAttack;
            Obj_AI_Base.OnSpellCast += Obj_AI_Base_OnSpellCast;
            Interrupter.OnInterruptableSpell += OnInterruptableTarget;
        }

        private static void OnInterruptableTarget(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs arg)
        {
            if (!EMenu.Get<CheckBox>("Interrupt").CurrentValue || sender == null)
            {
                return;
            }

            var pred = E.GetPrediction(sender);
            if (E.IsReady() && sender.IsValidTarget(E.Range) && sender.IsEnemy && !sender.IsDead)
            {
                E.Cast(pred.CastPosition);
            }
        }

        private static void Obj_AI_Base_OnSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (args.SData.Name.ToLower().Contains("itemtiamatcleave") && sender.IsMe)
            {
                Orbwalker.ResetAutoAttack();
            }
        }

        internal static void OnAfterAttack(AttackableUnit unit, EventArgs args)
        {
            if (!(unit is AIHeroClient))
            {
                return;
            }
            var target = TargetSelector.GetTarget(W.Range, DamageType.True);
            var hero = (AIHeroClient)unit;
            var Wcombo = WMenu["Combo"].Cast<CheckBox>().CurrentValue;
            if (hero == null || !hero.IsValid || hero.Type != GameObjectType.AIHeroClient)
            {
                return;
            }

            var flags = Orbwalker.ActiveModesFlags;

            if (target != null)
            {
                if (flags.HasFlag(Orbwalker.ActiveModes.Combo))
                {
                    if (Wcombo)
                    {
                        if (WMenu["AAr"].Cast<CheckBox>().CurrentValue)
                        {
                            if (W.Cast())
                            {
                                Orbwalker.ResetAutoAttack();
                                Player.IssueOrder(GameObjectOrder.AttackUnit, target);
                            }
                        }
                    }

                    if (ItemsMenu["Hydra"].Cast<CheckBox>().CurrentValue && Hydra.IsReady() && Hydra.IsOwned(Player.Instance)
                        && target.IsValidTarget(Hydra.Range))
                    {
                        Hydra.Cast();
                    }

                    if (ItemsMenu["Hydra"].Cast<CheckBox>().CurrentValue && Timat.IsReady() && Timat.IsOwned(Player.Instance)
                        && target.IsValidTarget(Timat.Range))
                    {
                        Timat.Cast();
                    }

                    if (ItemsMenu["Hydra"].Cast<CheckBox>().CurrentValue && Titanic.IsReady() && Titanic.IsOwned(Player.Instance)
                        && target.IsValidTarget(Titanic.Range))
                    {
                        if (Titanic.Cast())
                        {
                            Orbwalker.ResetAutoAttack();
                            Player.IssueOrder(GameObjectOrder.AttackUnit, target);
                        }
                    }
                    if (QMenu["Combo"].Cast<CheckBox>().CurrentValue && (QMenu["QAA"].Cast<CheckBox>().CurrentValue && Q.IsReady()) && !W.IsReady())
                    {
                        Q.Cast();
                    }
                }
            }
        }

        private static void OnUpdate(EventArgs args)
        {
            var lanemana = ManaMenu["lanemana"].Cast<Slider>().CurrentValue;
            var harassmana = ManaMenu["harassmana"].Cast<Slider>().CurrentValue;
            if (Player.Instance.IsDead || MenuGUI.IsChatOpen || Player.Instance.IsRecalling())
            {
                return;
            }

            var flags = Orbwalker.ActiveModesFlags;
            if (flags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Combo();
            }

            if (flags.HasFlag(Orbwalker.ActiveModes.LaneClear) && Player.Instance.ManaPercent >= lanemana)
            {
                Clear();
            }

            if (flags.HasFlag(Orbwalker.ActiveModes.Harass) && Player.Instance.ManaPercent >= harassmana)
            {
                Harass();
            }

            if (flags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                Flee();
            }

            if (menuIni["KillSteal"].Cast<CheckBox>().CurrentValue)
            {
                KillSteal();
            }

            if (QMenu["Qaoe"].Cast<CheckBox>().CurrentValue
                && Player.Instance.CountEnemiesInRange(Q.Range) >= QMenu["Qhit"].Cast<Slider>().CurrentValue)
            {
                Q.Cast();
            }
        }

        private static void Combo()
        {
            QCast();
            WCast();
            ECast();
            RCast();

            if (menuIni["Items"].Cast<CheckBox>().CurrentValue)
            {
                Items();
            }
        }

        private static void Harass()
        {
            QCast();
            WCast();
            ECast();
        }

        private static void Clear()
        {
            var Qclear = QMenu["Clear"].Cast<CheckBox>().CurrentValue && Q.IsReady();
            var Wclear = WMenu["Clear"].Cast<CheckBox>().CurrentValue && W.IsReady();

            var allMinions = EntityManager.MinionsAndMonsters.Get(
                EntityManager.MinionsAndMonsters.EntityType.Minion,
                EntityManager.UnitTeam.Enemy,
                ObjectManager.Player.Position,
                Q.Range,
                false);
            if (allMinions == null)
            {
                return;
            }

            foreach (var minion in allMinions)
            {
                if (Qclear)
                {
                    allMinions.Any();
                    {
                        var fl = EntityManager.MinionsAndMonsters.GetLineFarmLocation(allMinions, 100, (int)Q.Range);
                        if (fl.HitNumber >= QMenu["Qlc"].Cast<Slider>().CurrentValue)
                        {
                            Q.Cast();
                        }
                    }
                }

                if (Wclear)
                {
                    if (minion.IsInAutoAttackRange(Player.Instance) && minion.IsValidTarget() && W.IsReady()
                        && Player.Instance.Distance(minion.ServerPosition) <= 225f
                        && Player.Instance.GetSpellDamage(minion, SpellSlot.W) + Player.Instance.GetAutoAttackDamage(minion)
                        >= minion.TotalShieldHealth())
                    {
                        W.Cast();
                        Player.IssueOrder(GameObjectOrder.AttackUnit, minion);
                    }
                }
            }
        }

        private static void Flee()
        {
            var hp = QMenu["QFlee"].Cast<Slider>().CurrentValue;
            var Qflee = QMenu["Flee"].Cast<CheckBox>().CurrentValue && Q.IsReady();
            if (Qflee && Player.Instance.HealthPercent < hp)
            {
                if (Player.Instance.CountEnemiesInRange(Q.Range) >= 1)
                {
                    Q.Cast();
                }
            }
        }

        private static void KillSteal()
        {
            var SaveR = RMenu["SaveR"].Cast<CheckBox>().CurrentValue;
            var SR = RMenu["SR"].Cast<Slider>().CurrentValue;
            var Rks = KillStealMenu["Rks"].Cast<CheckBox>().CurrentValue && R.IsReady();
            var target =
                ObjectManager.Get<AIHeroClient>()
                    .FirstOrDefault(
                        enemy =>
                        enemy.IsEnemy && enemy.IsValidTarget(1000) && !enemy.IsDead && !enemy.HasBuff("kindredrnodeathbuff")
                        && !enemy.HasBuff("JudicatorIntervention") && !enemy.HasBuff("ChronoShift") && !enemy.HasBuff("UndyingRage")
                        && !enemy.IsInvulnerable && !enemy.IsZombie && !enemy.HasUndyingBuff() && !enemy.HasBuff("AatroxPassiveActivate")
                        && !enemy.HasBuff("rebirthcooldown"));

            if (target != null)
            {
                if (Rks)
                {
                    if (SaveR && ObjectManager.Player.GetAutoAttackDamage(target) * SR > target.TotalShieldHealth()
                        && target.IsValidTarget(ObjectManager.Player.GetAutoAttackRange()))
                    {
                        return;
                    }

                    var pred = E.GetPrediction(target);
                    // Credits cancerous
                    passiveCounter = target.GetBuffCount("DariusHemo") <= 0 ? 0 : target.GetBuffCount("DariusHemo");
                    if (RDmg(target, passiveCounter) >= target.Health + PassiveDmg(target, 1))
                    {
                        if (target.IsValidTarget(R.Range))
                        {
                            R.Cast(target);
                        }

                        if (!target.IsValidTarget(R.Range) && target.IsValidTarget(E.Range)
                            && Player.Instance.Mana >= (R.Handle.SData.Mana + E.Handle.SData.Mana))
                        {
                            E.Cast(pred.CastPosition);
                        }
                    }

                    if (target.IsValidTarget(R.Range) && target.TotalShieldHealth() < Player.Instance.GetSpellDamage(target, SpellSlot.R))
                    {
                        if (!target.IsValidTarget(R.Range) && target.IsValidTarget(E.Range)
                            && Player.Instance.Mana >= (R.Handle.SData.Mana + E.Handle.SData.Mana))
                        {
                            E.Cast(pred.CastPosition);
                        }

                        R.Cast(target);
                    }
                }

                if (Rks && target.IsValidTarget(R.Range))
                {
                    return;
                }

                if (Player.Spells.FirstOrDefault(o => o.SData.Name.Contains("SummonerDot")) != null)
                {
                    var IG = KillStealMenu["IG"].Cast<CheckBox>().CurrentValue;
                    var IGP = KillStealMenu["IGP"].Cast<CheckBox>().CurrentValue;
                    if (IGP && target.IsValidTarget(Ignite.Range)
                        && Player.Instance.GetSummonerSpellDamage(target, DamageLibrary.SummonerSpells.Ignite)
                        + PassiveDmg(target, target.GetBuffCount("DariusHemo")) > target.TotalShieldHealth() + target.HPRegenRate)
                    {
                        if (PassiveDmg(target, target.GetBuffCount("DariusHemo")) < target.TotalShieldHealth() + (target.HPRegenRate * 4)
                            && !target.IsValidTarget(Player.Instance.GetAutoAttackRange()))
                        {
                            Ignite.Cast(target);
                        }

                        if (PassiveDmg(target, target.GetBuffCount("DariusHemo")) < target.TotalShieldHealth())
                        {
                            if (target.TotalShieldHealth() > Player.Instance.TotalShieldHealth())
                            {
                                Ignite.Cast(target);
                            }
                        }
                    }

                    if (IG && target.IsValidTarget(Ignite.Range)
                        && Player.Instance.GetSummonerSpellDamage(target, DamageLibrary.SummonerSpells.Ignite)
                        > target.TotalShieldHealth() + (target.HPRegenRate * 4) && !target.IsValidTarget(Player.Instance.GetAutoAttackRange()))
                    {
                        Ignite.Cast(target);
                    }
                }
            }
        }

        private static void QCast()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            var Qrange = QMenu["range"].Cast<CheckBox>().CurrentValue;
            var Qcombo = QMenu["Combo"].Cast<CheckBox>().CurrentValue && Q.IsReady();
            var Qharass = QMenu["Harass"].Cast<CheckBox>().CurrentValue && Q.IsReady();

            var flags = Orbwalker.ActiveModesFlags;

            if (target != null)
            {
                if (QMenu["QAA"].Cast<CheckBox>().CurrentValue && Q.IsReady() && Player.Instance.IsInAutoAttackRange(target)
                    && Player.Instance.CanAttack)
                {
                    return;
                }

                if (flags.HasFlag(Orbwalker.ActiveModes.Combo))
                {
                    if (Qcombo && target.IsValidTarget(Q.Range))
                    {
                        if (Qrange)
                        {
                            if (!Player.Instance.IsInAutoAttackRange(target))
                            {
                                Q.Cast();
                            }
                        }

                        if (!Qrange)
                        {
                            Q.Cast();
                        }

                        if (Player.Instance.GetSpellDamage(target, SpellSlot.Q) > target.TotalShieldHealth())
                        {
                            Q.Cast();
                        }
                    }
                }

                if (flags.HasFlag(Orbwalker.ActiveModes.Harass))
                {
                    if (Qharass && target.IsValidTarget(Q.Range))
                    {
                        if (flags.HasFlag(Orbwalker.ActiveModes.Harass))
                        {
                            if (!target.IsInAutoAttackRange(Player.Instance))
                            {
                                Q.Cast();
                            }
                        }
                    }
                }

                if (QMenu["Stick"].Cast<CheckBox>().CurrentValue)
                {
                    if (Player.Instance.HasBuff("RumbleDangerZone") && target.IsValidTarget(Q.Range) && !target.IsUnderEnemyturret()
                        && !target.IsUnderHisturret())
                    {
                        Player.IssueOrder(GameObjectOrder.MoveTo, target.Position);
                    }
                }
            }
        }

        private static void WCast()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            var Wcombo = WMenu["Combo"].Cast<CheckBox>().CurrentValue && W.IsReady();
            var Wharass = WMenu["Harass"].Cast<CheckBox>().CurrentValue && W.IsReady();

            var flags = Orbwalker.ActiveModesFlags;

            if (target != null)
            {
                if (flags.HasFlag(Orbwalker.ActiveModes.Combo))
                {
                    if (Wcombo)
                    {
                        if (!WMenu["AAr"].Cast<CheckBox>().CurrentValue)
                        {
                            if (Player.Instance.IsInAutoAttackRange(target) && W.IsReady())
                            {
                                Player.IssueOrder(GameObjectOrder.AttackUnit, target);
                                W.Cast();
                            }

                            if (Player.Instance.HasBuff("DariusNoxianTacticsActive"))
                            {
                                Player.IssueOrder(GameObjectOrder.AttackUnit, target);
                            }
                        }
                    }
                }

                if (flags.HasFlag(Orbwalker.ActiveModes.Harass))
                {
                    if (Wharass)
                    {
                        if (target.IsValidTarget(W.Range))
                        {
                            Player.IssueOrder(GameObjectOrder.AttackUnit, target);
                            W.Cast();
                        }
                    }
                }
            }
        }

        private static void ECast()
        {
            var target = TargetSelector.GetTarget(E.Range, DamageType.Physical);
            var Qcombo = QMenu["Combo"].Cast<CheckBox>().CurrentValue && Q.IsReady();
            var QE = QMenu["QE"].Cast<CheckBox>().CurrentValue;
            var Ecombo = EMenu["Combo"].Cast<CheckBox>().CurrentValue && E.IsReady();
            var Eharass = EMenu["Harass"].Cast<CheckBox>().CurrentValue && E.IsReady();

            var flags = Orbwalker.ActiveModesFlags;

            if (target != null)
            {
                if (flags.HasFlag(Orbwalker.ActiveModes.Combo))
                {
                    if (Ecombo)
                    {
                        if (QE && Q.IsReady())
                        {
                            return;
                        }

                        if (target.IsValidTarget(E.Range))
                        {
                            if (Q.IsReady() && target.IsValidTarget(Q.Range) && Qcombo
                                || !Q.IsReady() && target.IsInRange(Player.Instance, Player.Instance.GetAutoAttackRange()))
                            {
                                return;
                            }

                            var pred = E.GetPrediction(target);
                            E.Cast(pred.CastPosition);
                        }
                    }
                }

                if (flags.HasFlag(Orbwalker.ActiveModes.Harass))
                {
                    if (Eharass)
                    {
                        if (target.IsValidTarget(E.Range)
                            && ((Q.IsReady() && !target.IsValidTarget(Q.Range)) || (!Q.IsReady() && target.IsInAutoAttackRange(Player.Instance))))
                        {
                            var pred = E.GetPrediction(target);
                            E.Cast(pred.CastPosition);
                        }
                    }
                }
            }
        }

        private static void RCast()
        {
            var SaveR = RMenu["SaveR"].Cast<CheckBox>().CurrentValue;
            var SR = RMenu["SR"].Cast<Slider>().CurrentValue;
            var buffcount = RMenu["count"].Cast<Slider>().CurrentValue;
            var rt = TargetSelector.GetTarget(R.Range, DamageType.True);
            var Rcombo = RMenu["Combo"].Cast<CheckBox>().CurrentValue && R.IsReady();
            var Rstack = RMenu["stack"].Cast<CheckBox>().CurrentValue && R.IsReady();
            var SemiR = RMenu["semiR"].Cast<KeyBind>().CurrentValue && R.IsReady();
            var SemiRtarget =
                EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(R.Range) && x != null)
                    .OrderByDescending(x => RDmg(x, passiveCounter))
                    .FirstOrDefault();
            if (rt != null)
            {
                if (!rt.HasBuff("kindredrnodeathbuff") && !rt.HasBuff("JudicatorIntervention") && !rt.HasBuff("ChronoShift")
                    && !rt.HasBuff("UndyingRage") && !rt.IsInvulnerable && !rt.IsZombie && !rt.HasUndyingBuff()
                    && !rt.HasBuff("AatroxPassiveActivate") && !rt.HasBuff("rebirthcooldown"))
                {
                    if (SaveR && ObjectManager.Player.GetAutoAttackDamage(rt) * SR > rt.TotalShieldHealth()
                        && rt.IsValidTarget(ObjectManager.Player.GetAutoAttackRange()))
                    {
                        return;
                    }

                    if (SemiR)
                    {
                        R.Cast(SemiRtarget);
                    }

                    if (Rcombo)
                    {
                        if (rt.IsValidTarget(R.Range) && Player.Instance.GetSpellDamage(rt, SpellSlot.R) > rt.TotalShieldHealth())
                        {
                            R.Cast(rt);
                        }
                    }

                    if (Rstack)
                    {
                        if (rt.IsValidTarget(R.Range) && rt.GetBuffCount("DariusHemo") >= buffcount)
                        {
                            R.Cast(rt);
                        }
                    }
                }
            }
        }

        private static void Items()
        {
            var target = TargetSelector.GetTarget(W.Range, DamageType.Physical);
            if (target == null || !target.IsValidTarget())
            {
                return;
            }

            if (Botrk.IsReady() && Botrk.IsOwned(Player.Instance) && target.IsValidTarget(Botrk.Range)
                && target.HealthPercent <= ItemsMenu["eL"].Cast<Slider>().CurrentValue && ItemsMenu["UseBOTRK"].Cast<CheckBox>().CurrentValue)
            {
                Botrk.Cast(target);
            }

            if (Botrk.IsReady() && Botrk.IsOwned(Player.Instance) && target.IsValidTarget(Botrk.Range)
                && target.HealthPercent <= ItemsMenu["oL"].Cast<Slider>().CurrentValue && ItemsMenu["UseBOTRK"].Cast<CheckBox>().CurrentValue)
            {
                Botrk.Cast(target);
            }

            if (Cutlass.IsReady() && Cutlass.IsOwned(Player.Instance) && target.IsValidTarget(Cutlass.Range)
                && target.HealthPercent <= ItemsMenu["eL"].Cast<Slider>().CurrentValue && ItemsMenu["UseBilge"].Cast<CheckBox>().CurrentValue)
            {
                Cutlass.Cast(target);
            }

            if (Youmuu.IsReady() && Youmuu.IsOwned(Player.Instance) && target.IsValidTarget(Q.Range)
                && ItemsMenu["useGhostblade"].Cast<CheckBox>().CurrentValue)
            {
                Youmuu.Cast();
            }
        }

        // Credits cancerous
        public static float RDmg(Obj_AI_Base unit, int stackcount)
        {
            var bonus = stackcount * (new[] { 20, 20, 40, 60 }[R.Level] + (0.15 * Player.Instance.FlatPhysicalDamageMod));

            return
                (float)
                (bonus
                 + Player.Instance.CalculateDamageOnUnit(
                     unit,
                     DamageType.True,
                     new[] { 100, 100, 200, 300 }[R.Level] + (float)(0.75 * Player.Instance.FlatPhysicalDamageMod)));
        }

        public static float PassiveDmg(Obj_AI_Base unit, int stackcount)
        {
            if (stackcount < 1)
            {
                stackcount = 1;
            }

            return Player.Instance.CalculateDamageOnUnit(
                unit,
                DamageType.Physical,
                (9 + Player.Instance.Level) + (float)(0.3 * Player.Instance.FlatPhysicalDamageMod)) * stackcount;
        }

        private static void OnDraw(EventArgs args)
        {
            if (!menuIni.Get<CheckBox>("Drawings").CurrentValue)
            {
                return;
            }

            if (DrawMenu.Get<CheckBox>("Q").CurrentValue && Q.IsLearned)
            {
                if (Q.IsReady())
                {
                    Circle.Draw(Color.OrangeRed, Q.Range, ObjectManager.Player.Position);
                }

                if (!Q.IsReady())
                {
                    Circle.Draw(Color.DarkRed, Q.Range, ObjectManager.Player.Position);
                }
            }

            if (DrawMenu.Get<CheckBox>("W").CurrentValue && W.IsLearned)
            {
                if (W.IsReady())
                {
                    Circle.Draw(Color.OrangeRed, W.Range, ObjectManager.Player.Position);
                }

                if (!W.IsReady())
                {
                    Circle.Draw(Color.DarkRed, W.Range, ObjectManager.Player.Position);
                }
            }

            if (DrawMenu.Get<CheckBox>("E").CurrentValue && E.IsLearned)
            {
                if (E.IsReady())
                {
                    Circle.Draw(Color.OrangeRed, E.Range, ObjectManager.Player.Position);
                }

                if (!E.IsReady())
                {
                    Circle.Draw(Color.DarkRed, E.Range, ObjectManager.Player.Position);
                }
            }

            if (DrawMenu.Get<CheckBox>("R").CurrentValue && R.IsLearned)
            {
                if (R.IsReady())
                {
                    Circle.Draw(Color.OrangeRed, R.Range, ObjectManager.Player.Position);
                }

                if (!R.IsReady())
                {
                    Circle.Draw(Color.DarkRed, R.Range, ObjectManager.Player.Position);
                }
            }
        }

        private static void OnEndScene(EventArgs args)
        {
            if (menuIni["Drawings"].Cast<CheckBox>().CurrentValue)
            {
                foreach (var enemy in EntityManager.Heroes.Enemies.Where(e => e.IsValidTarget() && e.IsHPBarRendered))
                {
                    if (DrawMenu["DrawD"].Cast<CheckBox>().CurrentValue && enemy.IsVisible)
                    {
                        Hpi.unit = enemy;
                        Hpi.drawDmg((int)RDmg(enemy, passiveCounter), System.Drawing.Color.Goldenrod);
                    }
                    var hpPos = enemy.HPBarPosition;
                    if (DrawMenu["Killable"].Cast<CheckBox>().CurrentValue && enemy.IsVisible)
                    {
                        if (RDmg(enemy, passiveCounter) > enemy.TotalShieldHealth())
                        {
                            Drawing.DrawText(hpPos.X + 135f, hpPos.Y, System.Drawing.Color.FromArgb(255, 106, 106), "DUNK = DEAD", 2);
                        }
                    }

                    if (DrawMenu["Stacks"].Cast<CheckBox>().CurrentValue)
                    {
                        if (enemy.GetBuffCount("DariusHemo") > 0 && enemy.IsVisible)
                        {
                            var endTime = Math.Max(0, enemy.GetBuff("DariusHemo").EndTime - Game.Time);
                            Drawing.DrawText(Drawing.WorldToScreen(enemy.Position) - new Vector2(DrawMenu["PPx"].Cast<Slider>().CurrentValue, DrawMenu["PPy"].Cast<Slider>().CurrentValue),
                                System.Drawing.Color.FromArgb(255, 106, 106),
                                (int)enemy.GetBuffCount("DariusHemo") + " Stacks " + Convert.ToString((int)endTime, CultureInfo.InvariantCulture),
                                2);
                        }
                    }

                    if (DrawMenu["RHealth"].Cast<CheckBox>().CurrentValue && enemy.IsVisible)
                    {
                        Drawing.DrawText(
                            hpPos.X + DrawMenu["RHx"].Cast<Slider>().CurrentValue,
                            hpPos.Y - 20f,
                            System.Drawing.Color.FromArgb(255, 106, 106),
                            Convert.ToString((int)(enemy.TotalShieldHealth() - RDmg(enemy, passiveCounter)), CultureInfo.CurrentCulture),
                            2);
                    }
                }
            }
        }
    }
}