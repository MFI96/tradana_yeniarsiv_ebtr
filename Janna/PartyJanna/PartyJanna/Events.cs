using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using SharpDX;
using System.Collections.Generic;
using System.Linq;
using Settings = PartyJanna.Config.Settings.AutoShield;
using Settings2 = PartyJanna.Config.Settings.AntiGapcloser;
using Settings3 = PartyJanna.Config.Settings.Interrupter;

namespace PartyJanna
{
    public static class Events
    {
        public static List<AIHeroClient> PriorAllyOrder { get; private set; }
        public static List<AIHeroClient> HpAllyOrder { get; private set; }
        public static int HighestPriority { get; private set; }
        public static float LowestHP { get; private set; }

        static Events()
        {
            Obj_AI_Base.OnBasicAttack += OnBasicAttack;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            Gapcloser.OnGapcloser += OnGapcloser;
            Interrupter.OnInterruptableSpell += OnInterruptableSpell;
        }

        public static void Initialize() { }

        public static bool PathIsInSpellRange(Vector3[] pathway, GameObjectProcessSpellCastEventArgs spell, float spellrange)
        {
            if (pathway != null)
            {
                foreach (var v in pathway)
                {
                    if (v.IsInRange(spell.End, spellrange))
                    { return true; }
                }
            }

            return false;
        }

        private static void OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (!sender.IsEnemy || !Settings2.AntiGap)
            { return; }

            foreach (var ally in EntityManager.Heroes.Allies)
            {
                if (e.End.Distance(ally) <= 300 && SpellManager.Q.IsInRange(sender.Position))
                {
                    SpellManager.Q.Cast(sender.Position);
                }
            }
        }

        private static void OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs e)
        {
            if (!sender.IsEnemy)
            { return; }

            if (e.DangerLevel == DangerLevel.High)
            {
                if (Settings3.RInterruptDangerous && SpellManager.R.IsReady() && SpellManager.R.IsInRange(sender) && Player.Instance.Mana >= 100)
                {
                    SpellManager.R.Cast();
                }
                else
                {
                    if (Settings3.QInterruptDangerous && SpellManager.Q.IsReady() && SpellManager.Q.IsInRange(sender))
                    {
                        SpellManager.Q.Cast(sender.Position);
                    }
                }
            }
            else
            {
                if (Settings3.QInterrupt && SpellManager.Q.IsReady() && SpellManager.Q.IsInRange(sender))
                {
                    SpellManager.Q.Cast(sender.Position);
                }
            }
        }

        public static void OnBasicAttack(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            PriorAllyOrder = new List<AIHeroClient>();

            HpAllyOrder = new List<AIHeroClient>();

            HighestPriority = 0;

            LowestHP = int.MaxValue;

            if (sender.IsEnemy && sender.IsMinion)
            {
                foreach (var ally in EntityManager.Heroes.Allies.Where(ally => ally.CountEnemiesInRange(1000) == 0))
                {
                    if (args.Target == ally)
                    {
                        SpellManager.E.Cast(sender);
                    }
                }
            }

            if (sender.IsAlly && sender.IsRanged && !sender.IsMinion && Settings.BoostAD)
            {
                foreach (var enemy in EntityManager.Heroes.Enemies)
                {
                    if (args.Target == enemy)
                    {
                        SpellManager.E.Cast(sender);
                    }
                }
            }

            if (sender.IsEnemy)
            {
                if (!sender.IsMinion)
                {
                    if (Settings.PriorMode == 0)
                    {
                        foreach (var ally in EntityManager.Heroes.Allies)
                        {
                            if (ally.Health <= LowestHP)
                            {
                                LowestHP = ally.Health;
                                HpAllyOrder.Insert(0, ally);
                            }
                            else
                            {
                                HpAllyOrder.Add(ally);
                            }
                        }

                        foreach (var ally in HpAllyOrder.Where(ally => Player.Instance.IsInRange(ally, SpellManager.E.Range)))
                        {
                            if (args.Target == ally)
                            {
                                SpellManager.E.Cast(ally);
                            }
                        }
                    }
                    else
                    {
                        foreach (var slider in Settings.Sliders)
                        {
                            if (slider.CurrentValue >= HighestPriority)
                            {
                                HighestPriority = slider.CurrentValue;

                                foreach (var ally in Settings.Heros)
                                {
                                    if (slider.VisibleName.Contains(ally.ChampionName))
                                    {
                                        PriorAllyOrder.Insert(0, ally);
                                    }
                                }
                            }
                            else
                            {
                                foreach (var ally in Settings.Heros)
                                {
                                    if (slider.VisibleName.Contains(ally.ChampionName))
                                    {
                                        PriorAllyOrder.Add(ally);
                                    }
                                }
                            }
                        }

                        foreach (var ally in PriorAllyOrder.Where(ally => Player.Instance.IsInRange(ally, SpellManager.E.Range)))
                        {
                            if (args.Target == ally)
                            {
                                SpellManager.E.Cast(ally);
                            }
                        }
                    }
                }
            }
        }

        public static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsEnemy)
            { return; }

            PriorAllyOrder = new List<AIHeroClient>();

            HpAllyOrder = new List<AIHeroClient>();

            HighestPriority = 0;

            LowestHP = int.MaxValue;

            if (Settings.PriorMode == 1)
            {
                foreach (var slider in Settings.Sliders)
                {
                    if (slider.CurrentValue >= HighestPriority)
                    {
                        HighestPriority = slider.CurrentValue;

                        foreach (var ally in Settings.Heros)
                        {
                            if (slider.VisibleName.Contains(ally.ChampionName))
                            {
                                PriorAllyOrder.Insert(0, ally);
                            }
                        }
                    }
                    else
                    {
                        foreach (var ally in Settings.Heros)
                        {
                            if (slider.VisibleName.Contains(ally.ChampionName))
                            {
                                PriorAllyOrder.Add(ally);
                            }
                        }
                    }
                }

                foreach (var ally in PriorAllyOrder.Where(ally => Player.Instance.IsInRange(ally, SpellManager.E.Range)))
                {
                    if (ally.IsInRange(args.End, 200) || PathIsInSpellRange(ally.RealPath(), args, 200))
                    {
                        SpellManager.E.Cast(ally);
                    }
                }

            }
            else
            {
                foreach (var ally in EntityManager.Heroes.Allies)
                {
                    if (ally.Health <= LowestHP)
                    {
                        LowestHP = ally.Health;
                        HpAllyOrder.Insert(0, ally);
                    }
                    else
                    {
                        HpAllyOrder.Add(ally);
                    }
                }

                foreach (var ally in HpAllyOrder.Where(ally => Player.Instance.IsInRange(ally, SpellManager.E.Range)))
                {
                    if (ally.IsInRange(args.End, args.SData.CastRadius) || PathIsInSpellRange(ally.RealPath(), args, 200))
                    {
                        SpellManager.E.Cast(ally);
                    }
                }
            }
        }
    }
}
