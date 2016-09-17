using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Rendering;
using SharpDX;
using Settings = Yorick.Config.Draw.DrawMenu;


namespace Yorick
{
    internal class Events
    {
        static Events()
        {
            Drawing.OnDraw += OnDraw;
            Orbwalker.OnPostAttack += Orbwalker_OnPostAttack;
        }


        private static void Orbwalker_OnPostAttack(AttackableUnit target, EventArgs args)
        {
            if (Config.Combo.ComboMenu.UseQ && SpellManager.Q.IsReady() &&
                Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                if (target != null)
                {
                    SpellManager.Q.Cast();
                    Orbwalker.ResetAutoAttack();
                    return;
                }
            }

            if (Config.JungleClear.JungleClearMenu.UseQ && SpellManager.Q.IsReady() &&
                Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear) &&
                Player.Instance.ManaPercent >= Config.JungleClear.JungleClearMenu.ManaQ)
            {
                var qtarget =
                    EntityManager.MinionsAndMonsters.GetJungleMonsters().OrderByDescending(a => a.MaxHealth)
                        .FirstOrDefault(
                            a =>
                                a.IsValidTarget(Player.Instance.GetAutoAttackRange() + 75) &&
                                a.Health > Player.Instance.GetSpellDamage(a, SpellSlot.Q));
                if (qtarget != null)
                {
                    SpellManager.Q.Cast();
                    Orbwalker.ResetAutoAttack();
                    return;
                }
            }

            if (Config.LaneClear.LaneClearMenu.UseQ && SpellManager.Q.IsReady() &&
                Player.Instance.ManaPercent >= Config.LaneClear.LaneClearMenu.ManaQ &&
                Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                var targetq = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                    Player.Instance.Position, Player.Instance.GetAutoAttackRange() + 75)
                    .OrderByDescending(a => a.MaxHealth)
                    .FirstOrDefault(a => a.Health >= Player.Instance.GetSpellDamage(a, SpellSlot.Q));
                if (targetq != null)
                {
                    SpellManager.Q.Cast();
                    Orbwalker.ResetAutoAttack();
                }
            }
        }

        private static void OnDraw(EventArgs args)
        {
            if (Settings.DrawW && SpellManager.W.IsLearned)
            {
                Circle.Draw(Color.Green, SpellManager.W.Range, Player.Instance.Position);
            }

            if (Settings.DrawE && SpellManager.E.IsLearned)
            {
                Circle.Draw(Color.Green, SpellManager.E.Range, Player.Instance.Position);
            }

            if (Settings.DrawR && SpellManager.R.IsLearned)
            {
                Circle.Draw(Color.Green, SpellManager.R.Range, Player.Instance.Position);
            }

            if (SpellManager.HasSmite())
            {
                if (Settings.DrawSmite && Config.Smite.SmiteMenu.SmiteToggle
                    || Settings.DrawSmite && Config.Smite.SmiteMenu.SmiteCombo
                    || Settings.DrawSmite && Config.Smite.SmiteMenu.SmiteEnemies)
                {
                    Circle.Draw(Color.Blue, SpellManager.Smite.Range, Player.Instance.Position);
                }
            }
        }

        public static void Initialize()
        {
        }
    }
}