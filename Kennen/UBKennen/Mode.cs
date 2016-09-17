using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Constants;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;


namespace UBKennen
{
    class Mode
    {
        //Combo
        public static void Combo()
        {
            var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Magical);
            if (Config.ComboMenu["useQCombo"].Cast<CheckBox>().CurrentValue
                && Spells.Q.IsReady()
                && !Player.Instance.IsDashing())
            {
                if (target != null && target.IsValidTarget())
                {
                    Spells.Q.Cast(target);
                }
            }

            if (Config.ComboMenu["useWCombo"].Cast<CheckBox>().CurrentValue
                 && Spells.W.IsReady())
            {
                var intTarget = TargetSelector.GetTarget(Spells.W.Range, DamageType.Magical);
                if (intTarget.CountEnemiesInRange(950) >= Config.ComboMenu["WhitCombo"].Cast<Slider>().CurrentValue
                    && intTarget.HasBuff("kennenmarkofstorm"))
                {
                    Spells.W.Cast();
                }
            }
            if (Config.ComboMenu["useECombo"].Cast<CheckBox>().CurrentValue
                 && Spells.E.IsReady() && target.CountEnemiesInRange((Player.Instance.MoveSpeed) * 2) > 0)
            {
                Spells.E.Cast();
            }

            if (Config.ComboMenu["useRCombo"].Cast<CheckBox>().CurrentValue
                      && Spells.R.IsReady())
            {
                var rtarget = TargetSelector.GetTarget(Spells.R.Range, DamageType.Magical);
                if (rtarget.CountEnemiesInRange(450) >= Config.ComboMenu["RhitCombo"].Cast<Slider>().CurrentValue)
                {
                    Spells.R.Cast();
                }
            }
        }
        //Harass
        public static void Harass()
        {
            if (Config.HarassMenu["useQ"].Cast<CheckBox>().CurrentValue
                && Spells.Q.IsReady()
                && !Player.Instance.IsDashing())
            {
                var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Physical);
                if (target != null && target.IsValidTarget())
                {
                    Spells.Q.Cast(target);
                }
            }
            var intTarget = TargetSelector.GetTarget(Spells.W.Range, DamageType.Magical);
            if (Config.HarassMenu["useW"].Cast<CheckBox>().CurrentValue
                && Player.Instance.Mana > Config.HarassMenu["HrEnergyManager"].Cast<Slider>().CurrentValue
                && Spells.W.IsReady()
                && intTarget.HasBuff("kennenmarkofstorm") && intTarget.CountEnemiesInRange(950) != null)
            {
                Spells.W.Cast();
            }
        }
        //LaneClear
        public static void LaneClear()
        {
            var minion = ObjectManager.Get<Obj_AI_Minion>().Where(x => x.IsEnemy && x.IsMinion && x.IsValidTarget(Spells.Q.Range)).OrderBy(x => x.Health).FirstOrDefault();
            if (minion != null) return;
            if (Config.LaneClear["useQLc"].Cast<CheckBox>().CurrentValue
              && Player.Instance.Mana > Config.LaneClear["EnergyManager"].Cast<Slider>().CurrentValue
              && Spells.Q.IsReady()) return;
            {
                Spells.Q.Cast(minion);
            }

            var wminion = ObjectManager.Get<Obj_AI_Minion>().Where(x => x.IsEnemy && x.IsMinion && x.IsValidTarget(Spells.W.Range)).OrderBy(x => x.Health).FirstOrDefault();
            if (Config.LaneClear["useWLc"].Cast<CheckBox>().CurrentValue
                && Player.Instance.Mana > Config.LaneClear["EnergyManager"].Cast<Slider>().CurrentValue
                && Spells.W.IsReady()
                && wminion.HasBuff("kennenmarkofstorm") && wminion.CountEnemiesInRange(950) >= Config.LaneClear["WhitLc"].Cast<Slider>().CurrentValue)
            {
                Spells.W.Cast();
            }

            if (Config.LaneClear["useELc"].Cast<CheckBox>().CurrentValue
               && Player.Instance.Mana > Config.LaneClear["EnergyManager"].Cast<Slider>().CurrentValue)
            {
                if (Spells.E.IsReady())
                {
                    Spells.E.Cast();
                }
            }
        }
        //Lasthit
        public enum AttackSpell
        { Q, W }
        private static Obj_AI_Base MinionQLh(GameObjectType type, AttackSpell spell)
        {
            return EntityManager.MinionsAndMonsters.EnemyMinions.OrderBy(a => a.Health).FirstOrDefault
                (a => a.IsEnemy
                && a.Type == type
                && a.Distance(Kennen) <= Spells.Q.Range
                && !a.IsDead
                && !a.IsInvulnerable
                && a.IsValidTarget(Spells.Q.Range)
                && a.Health <= Damages.QDamage(a));
        }

        private static Obj_AI_Base MinionWlh(GameObjectType type, AttackSpell spell)
        {
            return EntityManager.MinionsAndMonsters.EnemyMinions.OrderBy(a => a.Health).FirstOrDefault
                (a => a.IsEnemy
                && a.Type == type
                && a.Distance(Kennen) <= Spells.W.Range
                && !a.IsDead
                && !a.IsInvulnerable
                && a.IsValidTarget(Spells.W.Range)
                && a.Health <= Damages.WDamage(a));
        }

        public static void Lasthit()
        {
            if (Config.LasthitMenu["useQLh"].Cast<CheckBox>().CurrentValue
              || Spells.Q.IsReady()) return;
            {
                var qminion = (Obj_AI_Minion)MinionQLh(GameObjectType.obj_AI_Minion, AttackSpell.Q);
                if (qminion != null)
                {
                    Spells.Q.Cast(qminion.ServerPosition);
                }

                if (Config.LasthitMenu["useWLh"].Cast<CheckBox>().CurrentValue
                 || Spells.W.IsReady()) return;
                {
                    var wminion = (Obj_AI_Minion)MinionWlh(GameObjectType.obj_AI_Minion, AttackSpell.W);
                    if (wminion != null)
                    {
                        if (wminion.HasBuff("kennenmarkofstorm"))
                        {
                            Spells.W.Cast();
                        }
                    }
                }
            }
        }
        //JungleClear
        public static void JungleClear()
        {
            var monster = ObjectManager.Get<Obj_AI_Minion>().Where(x => x.IsMonster && x.IsValidTarget(Spells.Q.Range)).OrderBy(x => x.Health).FirstOrDefault();
            if (monster == null || !monster.IsValid) return;
            if (Orbwalker.IsAutoAttacking) return;
            Orbwalker.ForcedTarget = null;
            if (Config.JungleClear["useQJc"].Cast<CheckBox>().CurrentValue
                && Player.Instance.Mana > Config.JungleClear["JcEnergyManager"].Cast<Slider>().CurrentValue
                && Spells.Q.IsReady())
            {
                Spells.Q.Cast(monster);
            }

            var wmonster = ObjectManager.Get<Obj_AI_Minion>().Where(x => x.IsMonster && x.IsValidTarget(Spells.W.Range)).OrderBy(x => x.Health).FirstOrDefault();
            if (wmonster == null || !wmonster.IsValid) return;
            if (Orbwalker.IsAutoAttacking) return;
            Orbwalker.ForcedTarget = null;
            if (Config.JungleClear["useWJc"].Cast<CheckBox>().CurrentValue
                && Player.Instance.ManaPercent > Config.JungleClear["JcEnergyManager"].Cast<Slider>().CurrentValue
                && Spells.W.IsReady()
                && wmonster.HasBuff("kennenmarkofstorm") && wmonster.CountEnemiesInRange(800) >= Config.JungleClear["WhitJc"].Cast<Slider>().CurrentValue)
            {
                if (Player.Instance.Mana > Config.JungleClear["JcEnergyManager"].Cast<Slider>().CurrentValue)
                {
                    Spells.W.Cast();
                }
            }

            if (Config.JungleClear["useEJc"].Cast<CheckBox>().CurrentValue
               && Player.Instance.Mana > Config.JungleClear["JcEnergyManager"].Cast<Slider>().CurrentValue)
            {
                if (Spells.E.IsReady())
                {
                    Spells.E.Cast();
                }
            }
        }
        public static readonly AIHeroClient Kennen = ObjectManager.Player;
        //KillSteal
        public static void Killsteal()
        {
            if (Config.ComboMenu["useIg"].Cast<CheckBox>().CurrentValue && Spells.ignite != null)
            {
                var target = EntityManager.Heroes.Enemies.FirstOrDefault(
                        t =>
                            t.IsValidTarget(Spells.ignite.Range) &&
                            t.Health <= Player.Instance.GetSpellDamage(t, Spells.ignite.Slot));

                if (target != null && Spells.ignite.IsReady())
                {
                    Spells.ignite.Cast(target);
                }
            }
            if (Config.MiscMenu["useQKS"].Cast<CheckBox>().CurrentValue && Spells.Q.IsReady())
            {
                var target = TargetSelector.GetTarget(EntityManager.Heroes.Enemies.Where(t => t != null
                    && t.IsValidTarget()
                    && Spells.Q.IsInRange(t)
                    && t.Health <= Damages.QDamage(t)), DamageType.Magical);

                if (target != null)
                {
                    var pred = Spells.Q.GetPrediction(target);
                    {
                        Spells.Q.Cast(pred.CastPosition);
                    }
                }
            }
            if (Config.MiscMenu["useWKS"].Cast<CheckBox>().CurrentValue && Spells.W.IsReady())
            {
                var target = TargetSelector.GetTarget(EntityManager.Heroes.Enemies.Where(t => t != null
                    && t.IsValidTarget()
                    && t.HasBuff("kennenmarkofstorm")
                    && Spells.W.IsInRange(t)
                    && t.Health <= Damages.WDamage(t)), DamageType.Magical);

                if (target != null)
                {
                    Spells.W.Cast();
                }
            }
        }
        public static void Useheal()
        {

            if (Config.ComboMenu["useheal"].Cast<CheckBox>().CurrentValue
             && Player.Instance.HealthPercent < Config.ComboMenu["manageheal"].Cast<Slider>().CurrentValue
             && ObjectManager.Player.CountEnemiesInRange(900) >= 1
             && Spells.heal.IsReady())
            {
                Spells.heal.Cast();
            }
            foreach (
                var ally in EntityManager.Heroes.Allies.Where(a => !a.IsDead))
            {
                if (Config.ComboMenu["usehealally"].Cast<CheckBox>().CurrentValue && ally.CountEnemiesInRange(800) >= 1
                    && ObjectManager.Player.Position.Distance(ally) < 800
                    && ally.HealthPercent <= Config.ComboMenu["managehealally"].Cast<Slider>().CurrentValue
                    && Spells.heal.IsReady())
                {
                    Spells.heal.Cast();
                }
            }
        }
    }
}
