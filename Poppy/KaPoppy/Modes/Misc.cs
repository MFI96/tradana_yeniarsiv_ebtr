using EloBuddy;
using EloBuddy.SDK;
using KaPoppy;

namespace Modes
{
    using System;
    using System.Linq;
    using DashSpells;
    using Menu = Settings.MiscSettings;
    class Misc : Helper
    {
        public static void Execute()
        {
            if (Menu.SemiAutoR)
            {
                var selectedtgt = TargetSelector.SelectedTarget ?? TargetSelector.GetTarget(Lib.R.MaximumRange - 250, DamageType.Physical);
                if (selectedtgt != null && selectedtgt.IsValidTarget(Lib.R.MaximumRange - 250) && !selectedtgt.IsDead && Lib.R.IsReady())
                {
                    if (selectedtgt.IsValidTarget(Lib.R.MaximumRange - 250))
                    {
                        if (!Lib.R.IsCharging)
                            Lib.R.StartCharging();
                        else
                        {
                            var pred = Lib.R.GetPrediction(selectedtgt);
                            if (pred.HitChance >= EloBuddy.SDK.Enumerations.HitChance.High)
                            {
                                Lib.R.Cast(pred.CastPosition);
                            }
                            else if (Lib.R.Range == Lib.R.MaximumRange)
                                Lib.R.Cast(pred.CastPosition);
                        }
                    }
                }
            }
            var target = TargetSelector.GetTarget(Lib.R.MaximumRange, DamageType.Physical);
            if (target == null) return;

            var QDMG = Lib.Q.GetDamage(target);
            var EDMG = Lib.E.GetDamage(target);
            var RDMG = Lib.R.GetDamage(target);
            if (Menu.UseQ && QDMG > target.Health + target.AttackShield && Lib.Q.IsReady() && target.IsValidTarget(Lib.Q.Range) &&
                Lib.Q.GetPrediction(target).HitChance >= EloBuddy.SDK.Enumerations.HitChance.Medium)
            {
                Lib.Q.Cast(Lib.Q.GetPrediction(target).CastPosition);
            }
            else if (Menu.UseE && EDMG > target.Health + target.AttackShield && Lib.E.IsReady() && target.IsValidTarget(Lib.E.Range))
            {
                Lib.E.Cast(target);
            }
            else if (Menu.UseQ && Menu.UseE && QDMG + EDMG > target.Health + target.AttackShield && target.IsValidTarget(Lib.E.Range) &&
                Lib.Q.IsReady() && Lib.E.IsReady())
            {
                Lib.E.Cast(target);
            }
            else if (Menu.UseR && RDMG > target.Health + target.AttackShield && Lib.R.IsReady() && target.IsValidTarget(Lib.R.MaximumRange - 250))
            {
                if (!Lib.R.IsCharging)
                {
                    Lib.R.StartCharging();
                }
                else
                {
                    if (Lib.R.Range == Lib.R.MaximumRange)
                    {
                        Lib.R.Cast(target);
                    }
                    else if (target.IsValidTarget(Lib.R.Range))
                    {
                        var pred = Lib.R.GetPrediction(target);
                        if (pred.HitChance >= EloBuddy.SDK.Enumerations.HitChance.High)
                        {
                            Lib.R.Cast(pred.CastPosition);
                        }
                    }
                }
            }
        }

        internal static void AntiRengar(GameObject sender, EventArgs args)
        {
            return;
            if (sender.IsAlly || Lib.W.IsReady()) return;
            var rengo = EntityManager.Heroes.Enemies.Where(x => x.Hero == Champion.Rengar);
            if (rengo.Count() > 0)
            {
                if (Menu.AntiRengo)
                {
                    if (sender.Name == "Rengar_LeapSound.troy")
                    {
                        if (rengo.First().IsValidTarget(Lib.W.Range))
                        {
                            Lib.W.Cast();
                        }
                    }
                }
            }
        }

        internal static void SpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!Lib.W.IsReady() || !Menu.AntiGapcloser) return;
            if (sender == null || !sender.IsEnemy || sender.IsMinion || (sender is AIHeroClient)) return;
            var enemy = (AIHeroClient)sender;

            if (Lib.W.IsInRange(args.End) || Lib.W.IsInRange(args.Start))
            {
                foreach (var dash in DashSpells.Dashes)
                {
                    if (enemy.Hero == dash.champ)
                    {
                        if (dash.spellname == string.Empty)
                        {
                            if (args.Slot == dash.spellKey)
                            {
                                if (Menu.WEnabled(dash.champ, dash.spellKey))
                                {
                                    Lib.W.Cast();
                                }
                            }
                        }
                        else
                        {
                            if (args.SData.Name == dash.spellname)
                            {
                                if (Menu.WEnabled(dash.champ, dash.spellname))
                                {
                                    Lib.W.Cast();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}




