﻿namespace KappaUtility.Summoners
{
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Menu.Values;

    internal class Flash : Spells
    {
        public static Spell.Skillshot F;

        internal static void FOnLoad()
        {
            if (Player.Spells.FirstOrDefault(o => o.SData.Name.Contains("SummonerFlash")) != null)
            {
                SummMenu.AddGroupLabel("Flash Ayarları");
                SummMenu.Add("extend", new CheckBox("Gelişmiş flash maks mesafe"));
                SummMenu.Add("wall", new CheckBox("Duvara vuracaksa atma(duvardan atlayamıyacaksa diyor"));
                SummMenu.AddSeparator();

                F = new Spell.Skillshot(Player.Instance.GetSpellSlotFromName("SummonerFlash"), 450, SkillShotType.Circular);
                Spellbook.OnCastSpell += Spellbook_OnCastSpell;
            }
        }

        private static void Spellbook_OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            if (sender.Owner.IsMe && args.Slot == F.Slot)
            {
                var wall = SummMenu["wall"].Cast<CheckBox>().CurrentValue
                           && (NavMesh.GetCollisionFlags(Player.Instance.ServerPosition.Extend(Game.CursorPos, F.Range).To3D()) == CollisionFlags.Wall
                               || NavMesh.GetCollisionFlags(Player.Instance.ServerPosition.Extend(Game.CursorPos, F.Range).To3D()) == CollisionFlags.Building);
                var extend = SummMenu["extend"].Cast<CheckBox>().CurrentValue;

                if (extend)
                {
                    if (Player.Instance.Distance(args.EndPosition) < 450)
                    {
                        F.Cast(Player.Instance.Position.Extend(Game.CursorPos, 450).To3D());
                    }
                }

                if (wall)
                {
                    args.Process = false;
                }
            }
        }
    }
}