using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu.Values;

using Settings = Bard.Config.Modes.Misc;

namespace Bard.Modes
{
    public sealed class PermaActive : ModeBase
    {

        private static AIHeroClient BardPitt
        {
            get { return ObjectManager.Player; }
        }
        public override bool ShouldBeExecuted()
        {
            return true;
        }

        public override void Execute()
        {
            #region Potion

            if (Settings.EnablePotion && !Player.Instance.IsInShopRange() && Player.Instance.HealthPercent <= Settings.MinHPPotion && !PotionRunning())
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

            if (Settings.EnablePotion && !Player.Instance.IsInShopRange() && Player.Instance.ManaPercent <= Settings.MinMPPotion && !PotionRunning())
            {
                if (Item.HasItem(Utility.CorruptingPotion.Id) && Item.CanUseItem(Utility.CorruptingPotion.Id))
                {
                    Utility.CorruptingPotion.Cast();
                    return;
                }
            }

            #endregion

            #region W Logic

            if (W.IsReady())
            {
                if (BardPitt.IsRecalling() || BardPitt.IsInShopRange())
                {
                    return;
                }

                var ally = EntityManager.Heroes.Allies.Where(a => a.IsValidTarget(W.Range) && a.HealthPercent <= Settings.WHeal).OrderBy(a => a.Health).FirstOrDefault();
                if (Settings.UseW && ally != null && BardPitt.ManaPercent >= Settings.WMana && !ally.IsRecalling() && !ally.IsInShopRange())
                {
                    var prediction = W.GetPrediction(ally);
                    W.Cast(prediction.UnitPosition);
                    return;
                }

                if (Settings.UseW && BardPitt.HealthPercent <= Settings.WHeal && BardPitt.ManaPercent > Settings.WMana)
                {
                    W.Cast(BardPitt);
                    return;
                }
            }

            #endregion

            //Q KS

            if (Settings.UseQKS && Q.IsReady())
            {
                foreach (var predictionQ in from target in EntityManager.Heroes.Enemies.Where(
                    hero =>
                        hero.IsValidTarget(Q.Range) && !hero.IsDead && !hero.IsZombie && hero.HealthPercent <= 25) let predictionQ = Q.GetPrediction(target) where target.Health + target.TotalShieldHealth() < ObjectManager.Player.GetSpellDamage(target, SpellSlot.Q) where predictionQ.HitChance >= HitChance.High select predictionQ)
                {
                    Q.Cast(predictionQ.CastPosition);
                    return;
                }
            }

            //Ignite KS
            if (Settings.IgniteKS && HasIgnite && SpellManager.Ignite.IsReady())
            {
                var igniteKs = EntityManager.Heroes.Enemies.FirstOrDefault(e => SpellManager.Ignite.IsInRange(e) && !e.IsDead && e.Health > 0 && !e.IsInvulnerable && e.IsVisible && e.TotalShieldHealth() < Utility.IgniteDmg(e));
                if (igniteKs != null)
                {
                    SpellManager.Ignite.Cast(igniteKs);
                    return;
                }
            }
            
            #region Smite

            if (HasSmite)
            {
                //Red Smite Combo
                if (Config.Smite.SmiteMenu.SmiteCombo && Smite.Name.Equals("s5_summonersmiteduel") && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) && Smite.IsReady())
                {
                    foreach (
                        var smiteTarget in
                            EntityManager.Heroes.Enemies
                                .Where(h => h.IsValidTarget(Smite.Range)).Where(h => h.HealthPercent <= Config.Smite.SmiteMenu.RedSmitePercent).OrderByDescending(TargetSelector.GetPriority))
                    {
                        Smite.Cast(smiteTarget);
                        return;
                    }
                }

                // Blue Smite KS
                if (Config.Smite.SmiteMenu.SmiteEnemies && Smite.Name.Equals("s5_summonersmiteplayerganker") && Smite.IsReady())
                {
                    var smiteKs = EntityManager.Heroes.Enemies.FirstOrDefault(e => Smite.IsInRange(e) && !e.IsDead && e.Health > 0 && !e.IsInvulnerable && e.IsVisible && e.TotalShieldHealth() < Utility.SmiteDmgHero(e));
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
                            .Where(e => !e.IsDead && e.Health > 0 && Utility.MonstersNames.Contains(e.BaseSkinName) && !e.IsInvulnerable && e.IsVisible && e.Health <= Utility.SmiteDmgMonster(e));
                    foreach (var n in monsters2.Where(n => Config.Smite.SmiteMenu.MainMenu[n.BaseSkinName].Cast<CheckBox>().CurrentValue))
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