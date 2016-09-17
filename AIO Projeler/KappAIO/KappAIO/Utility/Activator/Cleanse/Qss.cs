using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Spells;
using KappAIO.Common;
using static KappAIO.Utility.Activator.Database;

namespace KappAIO.Utility.Activator.Cleanse
{
    internal class Qss
    {
        private static readonly List<Item> SelfQss = new List<Item> { Quicksilver_Sash, Mercurial_Scimitar };

        private static readonly List<Item> AllyQss = new List<Item> { Mikaels };

        private static readonly List<BuffType> BuffsToQss = new List<BuffType>
        {
            BuffType.Blind, BuffType.Charm, BuffType.Fear, BuffType.Flee, BuffType.Knockback, BuffType.Knockup, BuffType.NearSight,
            BuffType.Poison, BuffType.Polymorph, BuffType.Sleep, BuffType.Slow, BuffType.Snare, BuffType.Silence, BuffType.Stun,
            BuffType.Suppression, BuffType.Taunt
        };

        private static Menu Clean;

        public static void Init()
        {
            try
            {
                Clean = Load.MenuIni.AddSubMenu("Qss");
                Clean.CreateCheckBox("ally", "Qss Allies");
                if (Player.Instance.Hero == Champion.Gangplank)
                {
                    Clean.AddSeparator(0);
                    Clean.AddGroupLabel("Spells");
                    Clean.CreateCheckBox("W", "Use Gangplank W");
                }
                Clean.AddSeparator(0);
                Clean.AddGroupLabel("Items");
                Clean.CreateCheckBox("Cleanse", "Use Summoner Cleanse");
                SelfQss.ForEach(i => Clean.CreateCheckBox(i.Id.ToString(), "Use " + i.ItemInfo.Name));
                AllyQss.ForEach(i => Clean.CreateCheckBox(i.Id.ToString(), "Use " + i.ItemInfo.Name));
                Clean.AddSeparator(0);
                Clean.AddGroupLabel("Buffs To Qss");
                BuffsToQss.ForEach(b => Clean.CreateCheckBox(b.ToString(), "Use On " + b));

                Game.OnTick += Game_OnTick;
            }
            catch (Exception ex)
            {
                Logger.Send("Activator Qss Error While Init", ex, Logger.LogLevel.Error);
            }
        }

        private static void Game_OnTick(EventArgs args)
        {
            try
            {
                if (Player.Instance.IsDead)return;

                foreach (var ally in EntityManager.Heroes.Allies.Where(a => a.IsKillable() && a.Buffs.Any(b => BuffsToQss.Contains(b.Type) && Clean.CheckBoxValue(b.Type.ToString()))))
                {
                    if (Player.Instance.Hero == Champion.Gangplank && ally.IsMe && Player.GetSpell(SpellSlot.W).IsReady && Player.GetSpell(SpellSlot.W).IsLearned && Clean.CheckBoxValue("W"))
                    {
                        Player.CastSpell(SpellSlot.W);
                        return;
                    }
                    CastQss(ally);
                    return;
                }
            }
            catch (Exception ex)
            {
                Logger.Send("Activator Qss Error At Game_OnTick", ex, Logger.LogLevel.Error);
            }
        }

        private static void CastQss(Obj_AI_Base target)
        {
            try
            {
                foreach (var i in SelfQss.Where(a => a.ItemReady(Clean)))
                {
                    if (target.IsMe)
                    {
                        i.Cast();
                        return;
                    }
                }
                foreach (var i in AllyQss.Where(a => a.ItemReady(Clean)))
                {
                    if (target.IsMe || (target.IsAlly && !target.IsMe && Clean.CheckBoxValue("ally")))
                    {
                        i.Cast(target);
                        return;
                    }
                }

                if (target.IsMe && SummonerSpells.Cleanse.IsReady() && Clean.CheckBoxValue("Cleanse"))
                {
                    SummonerSpells.Cleanse.Cast();
                }
            }
            catch (Exception ex)
            {
                Logger.Send("Activator Qss Error At CastQss", ex, Logger.LogLevel.Error);
            }
        }
    }
}
