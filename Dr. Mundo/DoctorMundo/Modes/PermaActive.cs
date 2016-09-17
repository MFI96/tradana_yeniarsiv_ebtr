using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu.Values;
using Settings = DrMundo.Config.MiscMenu;
using SettingsHarass = DrMundo.Config.ModesMenu.Harass;
using SettingsCombo = DrMundo.Config.ModesMenu.Combo;
using SettingsPrediction = DrMundo.Config.PredictionMenu;
using SettingsHealth = DrMundo.Config.HealthManagerMenu;

namespace DrMundo.Modes
{
    public sealed class PermaActive : ModeBase
    {
        static Item HealthPotion;
        static Item CorruptingPotion;
        static Item RefillablePotion;
        static Item HuntersPotion;
        static Item TotalBiscuit;

        static PermaActive()
        {
            HealthPotion = new Item(2003, 0);
            TotalBiscuit = new Item(2010, 0);
            CorruptingPotion = new Item(2033, 0);
            RefillablePotion = new Item(2031, 0);
            HuntersPotion = new Item(2032, 0);
        }

        public override bool ShouldBeExecuted()
        {
            return !Player.Instance.IsDead;
        }

        public override void Execute()
        {
            // Auto-toggle W off
            if (Settings.AutoWOff && WActive)
            {
                if (PlayerHealth < SettingsHealth.MinWHealth)
                {
                    Debug.WriteChat("Health below {0}%, turning W off", "" + SettingsHealth.MinWHealth);
                    W.Cast();
                }
                else
                {
                    var enemy =
                        EntityManager.Heroes.Enemies.FirstOrDefault(
                            e => !e.IsDead && e.Health > 0 && e.IsVisible && e.Distance(_PlayerPos) < 600);
                    var minion = EntityManager.MinionsAndMonsters.CombinedAttackable.FirstOrDefault(
                            e => !e.IsDead && e.Health > 0 && e.IsVisible && e.Distance(_PlayerPos) < 600);
                    if (enemy == null && minion == null)
                    {
                        Debug.WriteChat("No enemies around - turning W off.");
                        W.Cast();
                    }
                }
            }

            // Auto Q Harass
            if (SettingsHarass.AutoQ && Q.IsReady() && PlayerHealth >= SettingsHarass.MinAutoQHealth && !_Player.IsRecalling() && !PlayerIsUnderEnemyTurret() /* Don't harass under enemy turrets */ && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) /* Don't Harass in Combo, it can mess it up */)
            {
                
                var enemies = EntityManager.Heroes.Enemies.Where(
                    e => e.IsValidTarget(SettingsCombo.MaxQDistance)).OrderBy(e => _Player.Distance(e));
                foreach (var enemy in enemies)
                {
                    if (
                        DrMundo.Config.ModesMenu.MenuModes["blacklist" + enemy.ChampionName].Cast<CheckBox>()
                            .CurrentValue)
                    {
                        continue;
                    }
                    var pred = Q.GetPrediction(enemy);
                    if (pred.HitChance >= SettingsPrediction.MinQHCAutoHarass)
                    {
                        Q.Cast(pred.CastPosition);
                        Debug.WriteChat("Casting Q in Auto Harass, Target: {0}, HitChance: {1}", enemy.ChampionName, pred.HitChance.ToString());
                        break;
                    }
                }
            }

            // KillSteal
            if (Settings.KsQ && Q.IsReady())
            {
                var enemies =
                    EntityManager.Heroes.Enemies.Where(
                        e => e.IsValidTarget(Q.Range) && e.TotalShieldHealth() < Damages.QDamage(e));
                foreach (var enemy in enemies)
                {
                    if (!enemy.HasBuffOfType(BuffType.SpellImmunity) && !enemy.HasBuffOfType(BuffType.SpellShield))
                    {
                        var pred = Q.GetPrediction(enemy);
                        if (pred.HitChance >= SettingsPrediction.MinQHCKillSteal)
                        {
                            Q.Cast(enemy);
                            Debug.WriteChat("Casting Q in KS on {0}, Enemy HP: {1}", "" + enemy.ChampionName,
                                "" + enemy.Health);
                            break;
                        }
                    }
                }
            }
            if (Settings.KsIgnite && HasIgnite && Ignite.IsReady())
            {
                var enemy =
                    EntityManager.Heroes.Enemies.FirstOrDefault(
                        e => e.IsValidTarget(Ignite.Range) && e.TotalShieldHealth() < Damages.IgniteDmg(e));
                if (enemy != null)
                {
                    Ignite.Cast(enemy);
                    Debug.WriteChat("Casting Ignite in KS on {0}, Enemy HP: {1}", "" + enemy.ChampionName, "" + enemy.Health);
                }
            }

            // Automatic ult usage
            if (Settings.AutoR && R.IsReady() && !Player.Instance.IsRecalling() && !Player.Instance.IsInShopRange())
            {
                var enemiesAround = EntityManager.Heroes.Enemies.Count(e => e.Distance(Player.Instance) < 1500 && e.IsValidTarget());
                if (enemiesAround >= Settings.AutoRMinEnemies && Player.Instance.HealthPercent <= Settings.AutoRMinHP)
                {
                    R.Cast();
                    Debug.WriteChat("AutoCasting R, Enemies around: {0}, Current HP: {1}", "" + enemiesAround, "" + ((int)PlayerHealth));
                }
            }

            // Potion manager
            if (Settings.Potion && !Player.Instance.IsInShopRange() && Player.Instance.HealthPercent <= Settings.potionMinHP && !(Player.Instance.HasBuff("RegenerationPotion") || Player.Instance.HasBuff("ItemCrystalFlaskJungle") || Player.Instance.HasBuff("ItemMiniRegenPotion") || Player.Instance.HasBuff("ItemCrystalFlask") || Player.Instance.HasBuff("ItemDarkCrystalFlask")))
            {
                if (Item.HasItem(HealthPotion.Id) && Item.CanUseItem(HealthPotion.Id))
                {
                    Debug.WriteChat("Using HealthPotion because below {0}% HP - have {1}% HP", String.Format("{0}", Settings.potionMinHP), String.Format("{0:##.##}", Player.Instance.HealthPercent));
                    HealthPotion.Cast();
                    return;
                }
                if (Item.HasItem(HuntersPotion.Id) && Item.CanUseItem(HuntersPotion.Id))
                {
                    Debug.WriteChat("Using HuntersPotion because below {0}% HP - have {1}% HP", String.Format("{0}", Settings.potionMinHP), String.Format("{0:##.##}", Player.Instance.HealthPercent));
                    HealthPotion.Cast();
                    return;
                }
                if (Item.HasItem(TotalBiscuit.Id) && Item.CanUseItem(TotalBiscuit.Id))
                {
                    Debug.WriteChat("Using TotalBiscuitOfRejuvenation because below {0}% HP - have {1}% HP", String.Format("{0}", Settings.potionMinHP), String.Format("{0:##.##}", Player.Instance.HealthPercent));
                    TotalBiscuit.Cast();
                    return;
                }
                if (Item.HasItem(RefillablePotion.Id) && Item.CanUseItem(RefillablePotion.Id))
                {
                    Debug.WriteChat("Using RefillablePotion because below {0}% HP - have {1}% HP", String.Format("{0}", Settings.potionMinHP), String.Format("{0:##.##}", Player.Instance.HealthPercent));
                    RefillablePotion.Cast();
                    return;
                }
                if (Item.HasItem(CorruptingPotion.Id) && Item.CanUseItem(CorruptingPotion.Id))
                {
                    Debug.WriteChat("Using CorruptingPotion because below {0}% HP - have {1}% HP", String.Format("{0}", Settings.potionMinHP), String.Format("{0:##.##}", Player.Instance.HealthPercent));
                    CorruptingPotion.Cast();
                    return;
                }
            }
        }
    }
}
