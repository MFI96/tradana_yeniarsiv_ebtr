using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using System.Linq;
using Settings = Vi.Config.Modes.Combo;
using Settings2 = Vi.Config.Modes.JungleClear;
using Settings3 = Vi.Config.Draw.DrawMenu;

namespace Vi
{
    class Events
    {

        static Events()
        {
            Orbwalker.OnPostAttack += OnAfterAttack;
            Drawing.OnDraw += OnDraw;
        }

        public static void OnAfterAttack(AttackableUnit target, EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) && SpellManager.E.IsReady() && Settings.UseE)
            {
                if (target != null)
                {
                    SpellManager.E.Cast();
                    return;
                }

            }

            if (Vi.Config.Modes.LaneClear.UseE && SpellManager.E.IsReady() && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) && Player.Instance.ManaPercent >= Vi.Config.Modes.LaneClear.MinMana)
            {
                var MinionsE = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy).Where(a => a.IsInRange(Player.Instance.ServerPosition, SpellManager.E2.Range));
                var Efarm = EntityManager.MinionsAndMonsters.GetLineFarmLocation(MinionsE, 100, (int)SpellManager.E2.Range);
                if (Efarm.HitNumber > 2 && Efarm.CastPosition.IsValid())
                {
                    SpellManager.E.Cast();
                    return;
                }
            }

            if (Vi.Config.Modes.JungleClear.UseE && SpellManager.E.IsReady() && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear) && Player.Instance.ManaPercent >= Vi.Config.Modes.JungleClear.MinMana)
            {
                var MinionsE = EntityManager.MinionsAndMonsters.GetJungleMonsters().Where(a => a.IsInRange(Player.Instance.ServerPosition, SpellManager.E2.Range));
                var Efarm = EntityManager.MinionsAndMonsters.GetLineFarmLocation(MinionsE, 100, (int)SpellManager.E2.Range);

                if (Efarm.HitNumber > 1 && Efarm.CastPosition.IsValid())
                {
                    SpellManager.E.Cast();
                    return;
                }

                var dragon = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.ServerPosition, SpellManager.E2.Range)
                        .Where(e => SmiteDamage.IMportantMonsters.Contains(e.BaseSkinName) && e.IsVisible && e.Health > Player.Instance.GetAutoAttackDamage(e) + 100);
                var dragonFarm = EntityManager.MinionsAndMonsters.GetLineFarmLocation(dragon, 100, (int)SpellManager.E2.Range);

                if (dragonFarm.HitNumber > 0)
                {
                    SpellManager.E.Cast();
                    return;
                }
            }           
        }

        private static void OnDraw(EventArgs args)
        {
            if (Settings3.DrawQ && SpellManager.Q.IsLearned)
            {
                Circle.Draw(Color.Green, 725, Player.Instance.Position);
            }

            if (Settings3.DrawW && SpellManager.W.IsLearned)
            {
                Circle.Draw(Color.Red, SpellManager.W.Range, Player.Instance.Position);
            }

            if (Settings3.DrawE && SpellManager.E.IsLearned)
            {
                Circle.Draw(Color.LightBlue, SpellManager.E.Range, Player.Instance.Position);
            }

            if (Settings3.DrawR && SpellManager.R.IsLearned)
            {
                Circle.Draw(Color.DarkBlue, SpellManager.R.Range, Player.Instance.Position);
            }

            if (SpellManager.HasSmite())
            {
                if (Settings3.DrawSmite && Config.Smite.SmiteMenu.SmiteToggle
                    || Settings3.DrawSmite && Config.Smite.SmiteMenu.SmiteCombo
                    || Settings3.DrawSmite && Config.Smite.SmiteMenu.SmiteEnemies)
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
