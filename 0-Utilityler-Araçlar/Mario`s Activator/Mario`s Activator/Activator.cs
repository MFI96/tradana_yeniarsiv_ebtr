using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using Mario_s_Lib;
using Mario_s_Lib.DataBases;
using static Mario_s_Activator.SummonerSpells;
using static Mario_s_Activator.MyMenu;

namespace Mario_s_Activator
{
    internal class Activator
    {
        public static bool CanPost;

        public static void Init()
        {
            InitializeSummonerSpells();
            Game.OnTick += Game_OnTick;
            Orbwalker.OnPostAttack += Orbwalker_OnPostAttack;
            Game.OnUpdate += Game_OnUpdate;
            InitializeMenu();
            Drawings.InitializeDrawings();
            Cleanser.Init();
        }

        private static int TickCount;
        private static void Game_OnTick(EventArgs args)
        {
            if(TickCount > Environment.TickCount) return;

            OffensiveOnTick();
            ConsumablesOnTick();
            IgniteOnTick();
            CastPoroThrower();
            Revealer.OnTick();

            if (SettingsMenu.GetCheckBoxValue("dev"))
            {
                foreach (var a in EntityManager.Heroes.Allies.Where(a => a.IsInDanger(80)))
                {
                    Chat.Print(a.ChampionName + " On danger");
                }
            }

            TickCount = Environment.TickCount + SettingsMenu.GetSliderValue("tickLimiter");
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            DefensiveOnTick();
            SmiteOnTick();
            HealOnTick();
            BarrierOnTick();
            ProtectorOnTick();
            Cleanser.CleanseOnTick();
        }

        private static void OffensiveOnTick()
        {
            var offItem =
                Offensive.OffensiveItems.FirstOrDefault(i => i.IsReady() && i.IsOwned() && OffensiveMenu.GetCheckBoxValue("check" + (int) i.Id));

            if (offItem != null)
            {
                if ((offItem.Id == ItemId.Tiamat || offItem.Id == ItemId.Ravenous_Hydra) && CanPost)
                {
                    var killMinion =
                        EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(
                            m => m.IsValidTarget(offItem.Range) && m.Health <= Player.Instance.GetAutoAttackDamage(m) * 0.6f);

                    var countMinion = EntityManager.MinionsAndMonsters.EnemyMinions.Count(m => m.IsValidTarget(offItem.Range));

                    var targetTiamat = TargetSelector.GetTarget(offItem.Range, DamageType.Physical);

                    if (targetTiamat != null && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                    {
                        offItem.Cast();
                        return;
                    }

                    if ((killMinion != null || countMinion >= 3) && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
                    {
                        offItem.Cast();
                        return;
                    }
                }

                if (offItem.Id == ItemId.Titanic_Hydra && CanPost)
                {
                    var killMinionT =
                        EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(
                            m => m.IsValidTarget(offItem.Range) && m.Health <= 40 + Player.Instance.MaxHealth * 0.1f);

                    var countMinionT = EntityManager.MinionsAndMonsters.EnemyMinions.Count(m => m.IsValidTarget(offItem.Range));

                    var targetTitanic = TargetSelector.GetTarget(offItem.Range, DamageType.Physical);


                    if (targetTitanic != null && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                    {
                        offItem.Cast();
                        return;
                    }

                    if ((killMinionT != null || countMinionT >= 3) && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
                    {
                        offItem.Cast();
                        return;
                    }
                    return;
                }


                if (SettingsMenu.GetCheckBoxValue("comboUseItems")
                    ? Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)
                    : offItem.IsReady())
                {
                    var target = TargetSelector.GetTarget(offItem.Range, DamageType.Mixed);
                    if (target != null && offItem.IsReady())
                    {
                        offItem.Cast(target);
                    }
                }
            }
        }

        private static void ConsumablesOnTick()
        {
            if (Player.Instance.IsInShopRange() || Player.Instance.IsRecalling()) return;

            var itemConsumable =
                Consumables.ComsumableItems.FirstOrDefault(
                    i => i.IsReady() && i.IsOwned() && ConsumablesMenu.GetCheckBoxValue("check" + (int) i.Id));

            if (itemConsumable != null)
            {
                switch (itemConsumable.Id)
                {
                    case ItemId.Elixir_of_Sorcery:
                        if (Player.Instance.HasBuff("ElixirOfSorcery")) return;
                        if (Player.Instance.CountEnemiesInRange(1000) >= 1 &&
                            Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                        {
                            itemConsumable.Cast();
                        }
                        return;
                    case ItemId.Elixir_of_Wrath:
                        if (Player.Instance.HasBuff("ElixirOfWrath")) return;
                        if (Player.Instance.CountEnemiesInRange(Player.Instance.GetAutoAttackRange() + 250) >= 1 &&
                            Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                        {
                            itemConsumable.Cast();
                        }
                        return;
                    case ItemId.Elixir_of_Iron:
                        if (Player.Instance.HasBuff("ElixirOfIron")) return;
                        if (Player.Instance.CountEnemiesInRange(1000) >= 1 &&
                            Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                        {
                            itemConsumable.Cast();
                        }
                        return;
                    case ItemId.Health_Potion:
                        if (Player.Instance.HasBuff("RegenerationPotion")) return;
                        if (Player.Instance.HealthPercent <= ConsumablesMenu.GetSliderValue("slider" + (int)itemConsumable.Id + "health") &&
                            Player.Instance.Health + 250 <= Player.Instance.MaxHealth)
                        {
                            itemConsumable.Cast();
                        }
                        return;
                    case ItemId.Total_Biscuit_of_Rejuvenation:
                        if (Player.Instance.HasBuff("ItemMiniRegenPotion")) return;
                        if (Player.Instance.HealthPercent <= ConsumablesMenu.GetSliderValue("slider" + (int)itemConsumable.Id + "health") ||
                            Player.Instance.ManaPercent <= ConsumablesMenu.GetSliderValue("slider" + (int) itemConsumable.Id + "mana") &&
                            Player.Instance.Health + 250 <= Player.Instance.MaxHealth &&
                            Player.Instance.Mana + 150 <= Player.Instance.MaxMana)
                        {
                            itemConsumable.Cast();
                        }
                        return;
                    case ItemId.Hunters_Potion:
                        if (Player.Instance.HasBuff("ItemCrystalFlaskJungle")) return;
                        if (Player.Instance.HealthPercent <= ConsumablesMenu.GetSliderValue("slider" + (int)itemConsumable.Id + "health") ||
                            Player.Instance.ManaPercent <= ConsumablesMenu.GetSliderValue("slider" + (int) itemConsumable.Id + "mana") &&
                            Player.Instance.Health + 250 <= Player.Instance.MaxHealth &&
                            Player.Instance.Mana + 150 <= Player.Instance.MaxMana)
                        {
                            itemConsumable.Cast();
                        }
                        return;
                    case ItemId.Corrupting_Potion:
                        if (Player.Instance.HasBuff("ItemDarkCrystalFlask")) return;
                        if (Player.Instance.HealthPercent <= ConsumablesMenu.GetSliderValue("slider" + (int)itemConsumable.Id + "health") ||
                            Player.Instance.ManaPercent <= ConsumablesMenu.GetSliderValue("slider" + (int) itemConsumable.Id + "mana") &&
                            Player.Instance.Health + 250 <= Player.Instance.MaxHealth &&
                            Player.Instance.Mana + 150 <= Player.Instance.MaxMana)
                        {
                            itemConsumable.Cast();
                        }
                        return;
                    case ItemId.Refillable_Potion:
                        if(Player.Instance.HasBuff("ItemCrystalFlask"))
                        if (Player.Instance.HealthPercent <= ConsumablesMenu.GetSliderValue("slider" + (int)itemConsumable.Id + "health") &&
                            Player.Instance.Health + 250 <= Player.Instance.MaxHealth)
                        {
                            itemConsumable.Cast();
                        }
                        return;
                }
            }
        }

        private static Spell.Targeted _protectSpell;

        private static void ProtectorOnTick()
        {
            var champS = ProtectSpells.Spells.FirstOrDefault(s => s.Champ == Player.Instance.Hero);
            if (champS != null)
            {
                if (!ProtectMenu.GetCheckBoxValue("checkProtector")) return;

                var spell = Player.GetSpell(champS.Slot);
                if (spell != null && ProtectMenu.GetCheckBoxValue("canUseSpell" + spell.Slot))
                {
                    var range = spell.SData.CastRadius <= 0 ? spell.SData.CastRadius : spell.SData.CastRangeDisplayOverride;

                    _protectSpell = new Spell.Targeted(spell.Slot, (uint) range);

                    var ally =
                        EntityManager.Heroes.Allies.FirstOrDefault(
                            a =>
                                a.IsValidTarget(_protectSpell.Range) && ProtectMenu.GetCheckBoxValue("canUseSpellOn" + a.ChampionName) &&
                                a.IsInDanger(ProtectMenu.GetSliderValue("protectallyhealth")));

                    if (ally != null)
                    {
                        _protectSpell?.Cast(ally);
                    }

                }
            }
        }

        private static void DefensiveOnTick()
        {
            var defItem =
                Defensive.DefensiveItems.FirstOrDefault(
                    i => i.IsReady() && DefensiveMenu.GetCheckBoxValue("check" + (int) i.Id));
            if (defItem != null)
            {
                if (Player.Instance.IsInDanger(DefensiveMenu.GetSliderValue("slider" + (int) defItem.Id)))
                {
                    switch (defItem.Id)
                    {
                        case ItemId.Randuins_Omen:
                            if (Player.Instance.CountEnemiesInRange(defItem.Range) >=
                                DefensiveMenu.GetSliderValue("slider" + (int) defItem.Id))
                            {
                                defItem.Cast();
                            }
                            return;
                        case ItemId.Ohmwrecker:
                            var towerAAingAlly = EntityManager.Heroes.Allies.FirstOrDefault(a => a.IsValid && a.ReceivingTurretAttack());
                            if (towerAAingAlly != null)
                            {
                                defItem.Cast();
                            }
                            return;
                        case ItemId.Face_of_the_Mountain:
                            var ally =
                                EntityManager.Heroes.Allies.OrderBy(a => a.Health)
                                    .FirstOrDefault(
                                        a =>
                                            a.IsInDanger(DefensiveMenu.GetSliderValue("slider" + (int) defItem.Id + "ally")) &&
                                            a.IsValidTarget(defItem.Range));
                            if (ally != null)
                            {
                                defItem.Cast(ally);
                            }

                            if (Player.Instance.IsInDanger(DefensiveMenu.GetSliderValue("slider" + (int) defItem.Id)))
                            {
                                defItem.Cast(Player.Instance);
                            }
                            return;
                    }

                    defItem.Cast();
                }
            }
        }

        #region SummonerOnTick

        private static void SmiteOnTick()
        {
            if (!PlayerHasSmite || !Smite.IsReady() || Smite == null || SummonerMenu.GetKeyBindValue("smiteKeybind")) return;

            Obj_AI_Base GetJungleMinion;

            var comboBoxValue = SummonerMenu.Get<ComboBox>("comboBox").CurrentValue;
            var sliderSafeDMG = SummonerMenu.GetSliderValue("sliderDMGSmite");

            switch (comboBoxValue)
            {
                case 0:
                    GetJungleMinion =
                        EntityManager.MinionsAndMonsters.GetJungleMonsters()
                            .FirstOrDefault(
                                m =>
                                    MonsterSmiteables.Contains(m.BaseSkinName) && m.IsValidTarget(Smite.Range) &&
                                    Prediction.Health.GetPrediction(m, Game.Ping) <= SmiteDamage() - sliderSafeDMG &&
                                    SummonerMenu.GetCheckBoxValue("monster" + m.BaseSkinName));
                    break;
                case 1:
                    GetJungleMinion =
                        EntityManager.MinionsAndMonsters.GetJungleMonsters()
                            .FirstOrDefault(
                                m =>
                                    MonsterSmiteables.Contains(m.BaseSkinName) && m.IsValidTarget(Smite.Range) &&
                                    m.Health <= SmiteDamage() - sliderSafeDMG &&
                                    SummonerMenu.GetCheckBoxValue("monster" + m.BaseSkinName));
                    break;
                default:
                    GetJungleMinion = null;
                    break;
            }


            if (GetJungleMinion != null)
            {
                Smite.Cast(GetJungleMinion);
            }

            if (!SummonerMenu.GetCheckBoxValue("smiteUseOnChampions")) return;

            var keepSmite = SummonerMenu.GetSliderValue("smiteKeep");

            var smiteGanker = Player.Spells.FirstOrDefault(s => s.Name.ToLower().Contains("playerganker"));
            if(Smite.Handle.Ammo < keepSmite)return;

            if (smiteGanker != null)
            {
                var target =
                    EntityManager.Heroes.Enemies.FirstOrDefault(
                        e =>
                            Prediction.Health.GetPrediction(e, Game.Ping) - 5 <= SmiteKSDamage() && e.IsValidTarget(Smite.Range));

                if (target != null)
                {
                    Smite.Cast(target);
                }
            }

            var smiteDuel = Player.Spells.FirstOrDefault(s => s.Name.ToLower().Contains("duel"));

            if (smiteDuel != null)
            {
                var target = TargetSelector.GetTarget(Smite.Range, DamageType.Mixed);

                if (target != null && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) && target.HealthPercent <= 60 &&
                    target.IsInAutoAttackRange(Player.Instance))
                {
                    Smite.Cast(target);
                }
            }
        }

        private static void IgniteOnTick()
        {
            if (PlayerHasIgnite && SummonerMenu.GetCheckBoxValue("check" + "ignite"))
            {
                var target = TargetSelector.GetTarget(Ignite.Range, DamageType.Mixed);
                if (target != null && Ignite.IsReady() && !target.IsInRange(Player.Instance, SummonerMenu.GetSliderValue("minimunRangeIgnite")))
                {
                    var predictedHealth = Prediction.Health.GetPrediction(target, Game.Ping);
                    if (predictedHealth <= GetTotalDamage(target) + IgniteDamage() && predictedHealth > IgniteDamage())
                    {
                        Ignite.Cast(target);
                    }
                }
            }
        }

        private static void BarrierOnTick()
        {
            if (PlayerHasBarrier && SummonerMenu.GetCheckBoxValue("check" + "barrier") && Player.Instance.IsInDanger( SummonerMenu.GetSliderValue("slider" + "barrier")))
            {
                Barrier.Cast();
            }
        }

        private static void HealOnTick()
        {
            if (PlayerHasHeal && SummonerMenu.GetCheckBoxValue("check" + "heal"))
            {
                var ally =
                    EntityManager.Heroes.Allies.OrderBy(a => a.Health)
                        .FirstOrDefault(
                            a => a.IsValidTarget(Heal.Range) && !a.IsMe && a.IsInDanger( SummonerMenu.GetSliderValue("slider" + "heal" + "ally")));
                if (ally != null)
                {
                    Heal.Cast();
                }
                if (Player.Instance.IsInDanger(SummonerMenu.GetSliderValue("slider" + "heal" + "me")))
                {
                    Heal.Cast();
                }
            }
        }

        private static void CastPoroThrower()
        {
            if (!PlayerHasPoroThrower) return;

            if (!PoroThrower.IsReady() || !SummonerMenu.GetCheckBoxValue("check" + "snowball") ||
                PoroThrower.Name.ToLower().Contains("snowballfollowupcast")) return;

            var targetPoro = TargetSelector.GetTarget(PoroThrower.Range, DamageType.True);
            if (targetPoro != null && targetPoro.IsValid)
            {
                var tpp = PoroThrower.GetPrediction(targetPoro);
                if (tpp.HitChancePercent >= 85)
                    PoroThrower.Cast(tpp.CastPosition);
            }
        }

        #endregion SummonerOnTick


        private static void Orbwalker_OnPostAttack(AttackableUnit target, EventArgs args)
        {
            CanPost = true;
            Core.DelayAction(() => CanPost = false, 90);
        }
    }
}
