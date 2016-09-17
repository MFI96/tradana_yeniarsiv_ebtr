using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using Settings = Yorick.Config.Misc.MiscMenu;

namespace Yorick.Modes
{
    public sealed class PermaActive : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return true;
        }

        public override void Execute()
        {
            #region KS

            if (Settings.Igniteks && HasIgnite && SpellManager.Ignite.IsReady())
            {
                var igniteKs = EntityManager.Heroes.Enemies.FirstOrDefault(e => SpellManager.Ignite.IsInRange(e) && !e.IsDead && !e.IsZombie && !e.IsInvulnerable && e.TotalShieldHealth() < Utility.IgniteDmg(e));
                if (igniteKs != null)
                {
                    SpellManager.Ignite.Cast(igniteKs);
                    return;
                }
            }

            if (Settings.Kse && E.IsReady())
            {
                var targete =
                    EntityManager.Heroes.Enemies.FirstOrDefault(
                        a =>
                            a.IsValidTarget(E.Range) &&
                            a.TotalShieldHealth() <= Player.Instance.GetSpellDamage(a, SpellSlot.E) &&
                            !a.IsZombie);

                if (targete != null)
                {
                    E.Cast(targete);
                    return;
                }
            }

            

            if (Settings.Ksw && W.IsReady())
            {
                var targetw =
                    EntityManager.Heroes.Enemies.FirstOrDefault(
                        a =>
                            a.IsValidTarget(W.Range) &&
                            a.TotalShieldHealth() <= Player.Instance.GetSpellDamage(a, SpellSlot.W) &&
                            !a.IsZombie);

                if (targetw != null)
                {
                    W.Cast(targetw);
                    return;
                }
            }

            if (Settings.Ksq && Q.IsReady())
            {
                var targetq =
                    EntityManager.Heroes.Enemies.FirstOrDefault(
                        a =>
                            a.IsValidTarget(Player.Instance.GetAutoAttackRange() + 75) &&
                            a.TotalShieldHealth() <= Player.Instance.GetSpellDamage(a, SpellSlot.Q) &&
                            !a.IsZombie);

                if (targetq != null &&
                    targetq.Distance(Player.Instance.ServerPosition) <= Player.Instance.GetAutoAttackRange())
                {
                    Q.Cast();
                    Player.IssueOrder(GameObjectOrder.AutoAttack, targetq);
                    return;
                }
            }

            

            #endregion

            #region Potion

            if (Settings.EnablePotion && !Player.Instance.IsInShopRange() &&
                Player.Instance.HealthPercent <= Settings.MinHPPotion && !PotionRunning())
            {
                if (Item.HasItem(Utility.HealthPotion.Id) && Item.CanUseItem(Utility.HealthPotion.Id))
                {
                    Utility.HealthPotion.Cast();
                    return;
                }
                if (Item.HasItem(Utility.HuntersPotion.Id) && Item.CanUseItem(Utility.HuntersPotion.Id))
                {
                    Utility.HuntersPotion.Cast();
                    return;
                }
                if (Item.HasItem(Utility.TotalBiscuit.Id) && Item.CanUseItem(Utility.TotalBiscuit.Id))
                {
                    Utility.TotalBiscuit.Cast();
                    return;
                }
                if (Item.HasItem(Utility.RefillablePotion.Id) && Item.CanUseItem(Utility.RefillablePotion.Id))
                {
                    Utility.RefillablePotion.Cast();
                    return;
                }
                if (Item.HasItem(Utility.CorruptingPotion.Id) && Item.CanUseItem(Utility.CorruptingPotion.Id))
                {
                    Utility.CorruptingPotion.Cast();
                    return;
                }
            }

            if (Settings.EnablePotion && !Player.Instance.IsInShopRange() &&
                Player.Instance.ManaPercent <= Settings.MinMPPotion && !PotionRunning())
            {
                if (Item.HasItem(Utility.CorruptingPotion.Id) && Item.CanUseItem(Utility.CorruptingPotion.Id))
                {
                    Utility.CorruptingPotion.Cast();
                    return;
                }
            }

            #endregion

            #region Smite

            if (HasSmite)
            {
                //Red Smite Combo
                if (Config.Smite.SmiteMenu.SmiteCombo && Smite.Name.Equals("s5_summonersmiteduel") &&
                    Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) && Smite.IsReady())
                {
                    foreach (
                        var smiteTarget in
                            EntityManager.Heroes.Enemies
                                .Where(h => h.IsValidTarget(Smite.Range))
                                .Where(h => h.HealthPercent <= Config.Smite.SmiteMenu.RedSmitePercent)
                                .OrderByDescending(TargetSelector.GetPriority))
                    {
                        Smite.Cast(smiteTarget);
                        return;
                    }
                }

                // Blue Smite KS
                if (Config.Smite.SmiteMenu.SmiteEnemies && Smite.Name.Equals("s5_summonersmiteplayerganker") &&
                    Smite.IsReady())
                {
                    var smiteKs =
                        EntityManager.Heroes.Enemies.FirstOrDefault(
                            e =>
                                Smite.IsInRange(e) && !e.IsDead && e.Health > 0 && !e.IsInvulnerable && e.IsVisible &&
                                e.TotalShieldHealth() < Utility.SmiteDmgHero(e));
                    if (smiteKs != null)
                    {
                        Smite.Cast(smiteKs);
                        return;
                    }
                }

                // Smite Monsters
                if (!Config.Smite.SmiteMenu.SmiteToggle || !Smite.IsReady()) return;
                {
                    var monsters2 =
                        EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.ServerPosition, Smite.Range)
                            .Where(
                                e =>
                                    !e.IsDead && e.Health > 0 && Utility.MonstersNames.Contains(e.BaseSkinName) &&
                                    !e.IsInvulnerable && e.IsVisible && e.Health <= Utility.SmiteDmgMonster(e));
                    foreach (
                        var n in
                            monsters2.Where(
                                n => Config.Smite.SmiteMenu.MainMenu[n.BaseSkinName].Cast<CheckBox>().CurrentValue))
                    {
                        Smite.Cast(n);
                        return;
                    }
                }
            }

            #endregion

            
        }
    }
}