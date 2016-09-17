using System;
using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using Volibear;

namespace Volibear
{
    internal class Smitez
    {
        public static double TotalDamage = 0;
        public static AIHeroClient Player
        {
            get { return ObjectManager.Player; }
        }

        public static Spell.Targeted SmiteSpell;

        public static Obj_AI_Base Monster;


        public static readonly string[] SmiteableUnits =
        {
            "SRU_Red", "SRU_Blue", "SRU_Dragon", "SRU_Baron",
            "SRU_Gromp", "SRU_Murkwolf", "SRU_Razorbeak",
            "SRU_Krug", "Sru_Crab"
        };

        private static readonly int[] SmiteRed = {3715, 1415, 1414, 1413, 1412};
        private static readonly int[] SmiteBlue = {3706, 1403, 1402, 1401, 1400};

        public static void Smitemethod()
        {
            Volibear.SmiteMenu = Volibear.VoliMenu.AddSubMenu("Smite", "Smite");
            Volibear.SmiteMenu.AddSeparator();
            Volibear.SmiteMenu.Add("smiteActive",
                new KeyBind("Smite Active (toggle)", true, KeyBind.BindTypes.PressToggle, 'H'));
            Volibear.SmiteMenu.AddSeparator();
            Volibear.SmiteMenu.Add("useSlowSmite", new CheckBox("KS with Blue Smite"));
            Volibear.SmiteMenu.Add("comboWithDuelSmite", new CheckBox("Combo with Red Smite"));
            Volibear.SmiteMenu.AddSeparator();
            Volibear.SmiteMenu.AddGroupLabel("Camps");
            Volibear.SmiteMenu.AddLabel("Epics");
            Volibear.SmiteMenu.Add("SRU_Baron", new CheckBox("Baron"));
            Volibear.SmiteMenu.Add("SRU_Dragon", new CheckBox("Dragon"));
            Volibear.SmiteMenu.AddLabel("Buffs");
            Volibear.SmiteMenu.Add("SRU_Blue", new CheckBox("Blue"));
            Volibear.SmiteMenu.Add("SRU_Red", new CheckBox("Red"));
            Volibear.SmiteMenu.AddLabel("Small Camps");
            Volibear.SmiteMenu.Add("SRU_Gromp", new CheckBox("Gromp", false));
            Volibear.SmiteMenu.Add("SRU_Murkwolf", new CheckBox("Murkwolf", false));
            Volibear.SmiteMenu.Add("SRU_Krug", new CheckBox("Krug", false));
            Volibear.SmiteMenu.Add("SRU_Razorbeak", new CheckBox("Razerbeak", false));
            Volibear.SmiteMenu.Add("Sru_Crab", new CheckBox("Skuttles", false));

            Game.OnUpdate += SmiteEvent;
        }

        public static void SetSmiteSlot()
        {
            SpellSlot smiteSlot;
            if (SmiteBlue.Any(x => Player.InventoryItems.FirstOrDefault(a => a.Id == (ItemId) x) != null))
                smiteSlot = Player.GetSpellSlotFromName("s5_summonersmiteplayerganker");
            else if (
                SmiteRed.Any(
                    x => Player.InventoryItems.FirstOrDefault(a => a.Id == (ItemId) x) != null))
                smiteSlot = Player.GetSpellSlotFromName("s5_summonersmiteduel");
            else
                smiteSlot = Player.GetSpellSlotFromName("summonersmite");
            SmiteSpell = new Spell.Targeted(smiteSlot, 500);
        }

        public static int GetSmiteDamage()
        {
            var level = Player.Level;
            int[] smitedamage =
            {
                20*level + 370,
                30*level + 330,
                40*level + 240,
                50*level + 100
            };
            return smitedamage.Max();
        }

        private static void SmiteEvent(EventArgs args)
        {
            SetSmiteSlot();
            if (!SmiteSpell.IsReady() || Player.IsDead) return;
            if (Volibear.SmiteMenu["smiteActive"].Cast<KeyBind>().CurrentValue)
            {
                var unit =
                    EntityManager.MinionsAndMonsters.Monsters
                        .Where(
                            a =>
                                SmiteableUnits.Contains(a.BaseSkinName) && a.Health < GetSmiteDamage() &&
                                Volibear.SmiteMenu[a.BaseSkinName].Cast<CheckBox>().CurrentValue)
                        .OrderByDescending(a => a.MaxHealth)
                        .FirstOrDefault();

                if (unit != null)
                {
                    SmiteSpell.Cast(unit);
                    return;
                }
            }
            if (Volibear.SmiteMenu["useSlowSmite"].Cast<CheckBox>().CurrentValue &&
                SmiteSpell.Handle.Name == "s5_summonersmiteplayerganker")
            {
                foreach (
                    var target in
                        EntityManager.Heroes.Enemies
                            .Where(h => h.IsValidTarget(SmiteSpell.Range) && h.Health <= 20 + 8*Player.Level))
                {
                    SmiteSpell.Cast(target);
                    return;
                }
            }
            if (Volibear.SmiteMenu["comboWithDuelSmite"].Cast<CheckBox>().CurrentValue &&
                SmiteSpell.Handle.Name == "s5_summonersmiteduel" &&
                Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                foreach (
                    var target in
                        EntityManager.Heroes.Enemies
                            .Where(h => h.IsValidTarget(SmiteSpell.Range)).OrderByDescending(TargetSelector.GetPriority)
                    )
                {
                    SmiteSpell.Cast(target);
                    return;
                }
            }
        }
    }
}