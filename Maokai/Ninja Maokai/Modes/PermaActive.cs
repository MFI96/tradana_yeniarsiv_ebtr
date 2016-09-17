using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu.Values;
using Settings = Maokai.Config.Misc.MiscMenu;

namespace Maokai.Modes
{
    public sealed class PermaActive : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return true;
        }

        public override void Execute()
        {
            if (Player.Instance.HasBuff("MaokaiDrain3"))
            {
                var targetsR = TargetSelector.GetTarget(R.Range, DamageType.Magical);
                var toggleState = R.Handle.ToggleState;
                var noenemies = Player.Instance.CountEnemiesInRange(R.Range) == 0;

                if (targetsR != null && Settings.RKS &&
                    targetsR.Health <
                    Player.Instance.GetSpellDamage(targetsR, SpellSlot.R) +
                    Player.Instance.CalculateDamageOnUnit(targetsR, DamageType.Magical, R.Handle.Ammo) ||
                    Settings.TurnoffR && noenemies)
                {
                    if (toggleState == 2)
                    {
                        R.Cast();
                        return;
                    }
                }
            }

            #region KS Q

            if (Settings.QKS && Q.IsReady())
            {
                var qks =
                    EntityManager.Heroes.Enemies.FirstOrDefault(
                        z => z.IsValidTarget(SpellManager.Q.Range) && !z.IsDead && !z.IsZombie &&
                             z.Health < Player.Instance.GetSpellDamage(z, SpellSlot.Q));
                if (qks != null)
                {
                    var qpred = SpellManager.Q.GetPrediction(qks);
                    if (qpred.HitChance >= HitChance.High)
                    {
                        SpellManager.Q.Cast(qpred.CastPosition);
                        return;
                    }
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
                    var smiteslow =
                        EntityManager.Heroes.Enemies.FirstOrDefault(
                            a =>
                                a.IsValidTarget(Smite.Range) && !a.IsDead &&
                                a.Health <= Utility.SmiteDmgHero(a) + Player.Instance.GetAutoAttackDamage(a) &&
                                a.Distance(Player.Instance.ServerPosition) > Player.Instance.GetAutoAttackRange());

                    if (smiteslow != null && Config.Smite.SmiteMenu.BlueSlow)
                    {
                        Smite.Cast(smiteslow);
                        return;
                    }

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