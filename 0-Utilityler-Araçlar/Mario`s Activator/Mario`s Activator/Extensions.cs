using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using Mario_s_Lib;

namespace Mario_s_Activator
{
    public static class Extensions
    {


        public static bool HasCC(this Obj_AI_Base target)
        {
            if (target.HasBuffOfType(BuffType.Stun) && MyMenu.CleansersMenu.GetCheckBoxValue("cc" + "Stun"))
            {
                return true;
            }
            if (target.HasBuffOfType(BuffType.Blind) && MyMenu.CleansersMenu.GetCheckBoxValue("cc" + "Blind"))
            {
                return true;
            }
            if (target.HasBuffOfType(BuffType.Slow) && MyMenu.CleansersMenu.GetCheckBoxValue("cc" + "Slow"))
            {
                return true;
            }
            if (target.HasBuffOfType(BuffType.Snare) && MyMenu.CleansersMenu.GetCheckBoxValue("cc" + "Snare"))
            {
                return true;
            }
            if (target.HasBuffOfType(BuffType.Flee) && MyMenu.CleansersMenu.GetCheckBoxValue("cc" + "Flee"))
            {
                return true;
            }
            if (target.HasBuffOfType(BuffType.Suppression) && MyMenu.CleansersMenu.GetCheckBoxValue("cc" + "Supression"))
            {
                return true;
            }
            if (target.HasBuffOfType(BuffType.Taunt) && MyMenu.CleansersMenu.GetCheckBoxValue("cc" + "Taunt"))
            {
                return true;
            }
            if (target.HasBuffOfType(BuffType.Charm) && MyMenu.CleansersMenu.GetCheckBoxValue("cc" + "Charm"))
            {
                return true;
            }
            if (target.HasBuffOfType(BuffType.Polymorph) && MyMenu.CleansersMenu.GetCheckBoxValue("cc" + "Polymorph"))
            {
                return true;
            }
            if (target.HasBuff("zedulttargetmark") && MyMenu.CleansersMenu.GetCheckBoxValue("cc" + "ZedR"))
            {
                return true;
            }
            if (target.HasBuff("vladimirhemoplague") && MyMenu.CleansersMenu.GetCheckBoxValue("cc" + "VladmirR"))
            {
                return true;
            }
            if (target.HasBuff("mordekaiserchildrenofthegrave") && MyMenu.CleansersMenu.GetCheckBoxValue("cc" + "MordekaiserR"))
            {
                return true;
            }
            if (target.HasBuff("zedulttargetmark") && MyMenu.CleansersMenu.GetCheckBoxValue("cc" + "ZedR"))
            {
                return true;
            }
            if (target.HasBuff("fiorarmark") && MyMenu.CleansersMenu.GetCheckBoxValue("cc" + "FioraR"))
            {
                return true;
            }
            if (target.HasBuff("itemdusknightfall") && MyMenu.CleansersMenu.GetCheckBoxValue("cc" + "Dusk"))
            {
                return true;
            }
            var KalistaEBuff = Player.Instance.Buffs.FirstOrDefault(b => b.DisplayName == "KalistaExpungeMarker" && b.IsValid);
            if (KalistaEBuff != null && MyMenu.CleansersMenu.GetCheckBoxValue("cc" + "KalistaE"))
            {
                var enemyKalista = EntityManager.Heroes.Enemies.FirstOrDefault(e => e.Hero == Champion.Kalista);
                if (enemyKalista != null)
                {
                    var eLevel = enemyKalista.Spellbook.GetSpell(SpellSlot.E).Level;
                    Chat.Print(GetRendDamage(Player.Instance, -1, KalistaEBuff, eLevel));
                    Chat.Print(eLevel);
                    if (Player.Instance.IsRendKillable(KalistaEBuff, eLevel))
                    {
                        Chat.Print("Cleanse");
                        return true;
                    }
                }
            }
            return false;
        }

        private static readonly float[] RawRendDamage = { 20, 30, 40, 50, 60 };
        private static readonly float[] RawRendDamageMultiplier = { 0.6f, 0.6f, 0.6f, 0.6f, 0.6f };
        private static readonly float[] RawRendDamagePerSpear = { 10, 14, 19, 25, 32 };
        private static readonly float[] RawRendDamagePerSpearMultiplier = { 0.2f, 0.225f, 0.25f, 0.275f, 0.3f };

        private static bool IsRendKillable(this Obj_AI_Base target, BuffInstance buff, int ELevel)
        {
            var totalHealth = target.TotalShieldHealth();

            var hero = target as AIHeroClient;
            if (hero != null)
            {
                if (hero.HasUndyingBuff()) return false;

                // Take into account Blitzcranks passive
                if (hero.ChampionName == "Blitzcrank" && !target.HasBuff("BlitzcrankManaBarrierCD") && !target.HasBuff("ManaBarrier"))
                {
                    totalHealth += target.Mana / 2;
                }
            }

            return GetRendDamage(target, -1, buff, ELevel) + 100f > totalHealth;
        }

        private static float GetRendDamage(Obj_AI_Base target, int customStacks, BuffInstance rendBuff, int ELevel)
        {
            return Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical, GetRawRendDamage(customStacks, rendBuff, ELevel) - 10) *(Player.Instance.HasBuff("SummonerExhaustSlow") ? 0.6f : 1);
        }

        private static float GetRawRendDamage(int customStacks, BuffInstance rendBuff, int ELevel)
        {
            var stacks = (customStacks > -1 ? customStacks : rendBuff?.Count ?? 0) - 1;
            if (stacks > -1)
            {
                var index = ELevel - 1;
                return RawRendDamage[index] + (stacks + 3) * RawRendDamagePerSpear[index] +
                       Player.Instance.TotalAttackDamage * (RawRendDamageMultiplier[index] + (stacks + 3) * RawRendDamagePerSpearMultiplier[index]);
            }

            return 0;
        }
    }
}
