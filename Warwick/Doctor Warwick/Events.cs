using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using SharpDX;
using System;
using System.Linq;
using SettingsDrawing = Warwick.Config.DrawingMenu;
using SettingsMana = Warwick.Config.ManaManagerMenu;
using SettingsMisc = Warwick.Config.MiscMenu;
using SettingsModes = Warwick.Config.ModesMenu;

namespace Warwick
{
    public static class Events
    {

        static Item Youmuu;
        private static float PlayerMana
        {
            get { return Player.Instance.ManaPercent; }
        }

        static Events()
        {
            Youmuu = new Item(ItemId.Youmuus_Ghostblade);
            Interrupter.OnInterruptableSpell += InterrupterOnInterruptableSpell;
            Orbwalker.OnPreAttack += OrbwalkerOnPreAttack;
            Obj_AI_Base.OnBuffGain += ObjAiBaseOnBuffGain;
            Drawing.OnDraw += OnDraw;
        }

        private static void ObjAiBaseOnBuffGain(Obj_AI_Base sender, Obj_AI_BaseBuffGainEventArgs args)
        {
            // Cast Challenging Smite in Ult
            if (SettingsModes.Combo.UseSmite && sender.IsEnemy && sender is AIHeroClient && SpellManager.HasChallengingSmite() && args.Buff.Name.Equals("suppression") &&
                args.Buff.SourceName.Equals("Warwick") && SettingsModes.Combo.UseSmite &&
                Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                SpellManager.Smite.Cast(sender);
                Debug.WriteChat("Casting Smite in combo with ult.");
            }
        }

        private static void OrbwalkerOnPreAttack(AttackableUnit target, EventArgs args)
        {
            if (SettingsModes.Combo.UseItems && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) && CanUseItem(ItemId.Youmuus_Ghostblade))
            {
                Youmuu.Cast();
            }
            if (target is AIHeroClient && SettingsModes.Combo.UseSmite && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) &&
                (SpellManager.HasChillingSmite() || SpellManager.HasChallengingSmite()) && SpellManager.Smite.IsReady())
            {
                var enemy = (AIHeroClient)target;
                if (enemy.IsValidTarget(SpellManager.Smite.Range))
                {
                    SpellManager.Smite.Cast(enemy);
                }
            }
            // No sense in checking if W is off cooldown
            if (!SpellManager.W.IsReady())
            {
                return;
            }
            // Check if we should use W to attack heroes
            if ((SettingsModes.Combo.UseW && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) ||
                (SettingsModes.Harass.UseW && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)) ||
                (Orbwalker.LaneClearAttackChamps && SettingsModes.LaneClear.UseW &&
                 Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)))
            {
                if (target is AIHeroClient && PlayerMana >= SettingsMana.MinWMana)
                {
                    SpellManager.W.Cast();
                    Debug.WriteChat("Casting W, because attacking enemy hero in Combo or Harras or LaneClear.");
                    return;
                }
            }
            // Check if we should use W to attack minions/monsters/turrets
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) ||
                Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                if (target is Obj_AI_Minion && PlayerMana >= SettingsMana.MinQMana)
                {
                    if (SettingsModes.JungleClear.UseW && target.Team == GameObjectTeam.Neutral)
                    {
                        SpellManager.W.Cast();
                        Debug.WriteChat("Casting W, because attacking monster in JungleClear");
                    }
                    else if (SettingsModes.LaneClear.UseW && target.IsEnemy)
                    {
                        SpellManager.W.Cast();
                        Debug.WriteChat("Casting W, because attacking minion in LaneClear");
                    }
                }
            }
        }

        private static void InterrupterOnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs interruptableSpellEventArgs)
        {
            if (SettingsMisc.InterruptR && sender.IsEnemy && sender.IsValidTarget(SpellManager.R.Range) &&
                SpellManager.R.IsReady())
            {
                SpellManager.R.Cast(sender);
                Debug.WriteChat("Interrupting spell from {0} with R.", ((AIHeroClient)sender).ChampionName);
            }
        }

        public static void Initialize()
        {

        }

        private static void OnDraw(EventArgs args)
        {
            if (SettingsDrawing.DrawQ)
            {
                if (!(SettingsDrawing.DrawOnlyReady && !SpellManager.Q.IsReady()))
                {
                    Circle.Draw(Color.LightBlue, SpellManager.Q.Range, Player.Instance.Position);
                }
            }
            if (SettingsDrawing.DrawE)
            {
                if (!(SettingsDrawing.DrawOnlyReady && !SpellManager.E.IsReady()))
                {
                    Circle.Draw(Color.Orange, SpellManager.ERange(), Player.Instance.Position);
                }
            }
            if (SettingsDrawing.DrawR)
            {
                if (!(SettingsDrawing.DrawOnlyReady && !SpellManager.R.IsReady()))
                {
                    Circle.Draw(Color.Yellow, SpellManager.R.Range, Player.Instance.Position);
                }
            }
            if (SettingsDrawing.DrawIgnite && SpellManager.HasIgnite())
            {
                if (!(SettingsDrawing.DrawOnlyReady && !SpellManager.Ignite.IsReady()))
                {
                    Circle.Draw(Color.Red, SpellManager.Ignite.Range, Player.Instance.Position);
                }
            }
            if (SettingsDrawing.DrawSmite && SpellManager.HasSmite())
            {
                if (!(SettingsDrawing.DrawOnlyReady && !SpellManager.Smite.IsReady()))
                {
                    Circle.Draw(Color.White, SpellManager.Smite.Range, Player.Instance.Position);
                }
            }
            if (SettingsDrawing.DrawLasthittable)
            {
                if (!(SettingsDrawing.DrawOnlyReady && !SpellManager.Q.IsReady()))
                {
                    var minions =
                        EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                            Player.Instance.Position, 1000.0f)
                            .Where(e => e.IsValidTarget() && e.Health < Damages.QDamage(e));
                    foreach (var m in minions)
                    {
                        Circle.Draw(Color.Red, m.BoundingRadius, m.Position);
                    }
                }
            }
        }

        private static bool CanUseItem(ItemId id)
        {
            return Item.HasItem(id) && Item.CanUseItem(id);
        }
    }
}
