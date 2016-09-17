using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;

namespace KaPoppy
{
    static class Extensions
    {
        public static void CreateStringMenu(this Menu menu, string[] list, string slidername, int defaultvalue = 0, int minvalue = 0)
        {
            var maxvalue = minvalue + list.Length - 1;
            if (defaultvalue < minvalue || defaultvalue > maxvalue)
                defaultvalue = minvalue;

            var a = menu.Add(slidername.ToString(), new Slider(list[defaultvalue - minvalue], defaultvalue, minvalue, maxvalue));
            a.OnValueChange += delegate
            (ValueBase<int> sender, ValueBase<int>.ValueChangeArgs Args)
            {
                sender.DisplayName = list[Args.NewValue - minvalue];
            };
        }
        public static bool IsWall(this Vector2 pos)
        {
            if (!pos.IsValid())
                return false;

            var ipos = pos.ToNavMeshCell().CollFlags;
            if (ipos.HasFlag(CollisionFlags.Wall) || ipos.HasFlag(CollisionFlags.Building))
            {
                return true;
            }
            return false;
        }
        public static bool IsWall(this Vector3 pos)
        {
            if (!pos.IsValid())
                return false;

            var ipos = pos.ToNavMeshCell().CollFlags;
            if (ipos.HasFlag(CollisionFlags.Wall) || ipos.HasFlag(CollisionFlags.Building))
            {
                return true;
            }
            return false;
        }
        public static float GetDamage(this Spell.SpellBase spell, Obj_AI_Base target)
        {
            return spell.Slot.GetDamage(target);
        }
        public static float GetDamage(this SpellSlot slot, Obj_AI_Base target)
        {
            var spellLevel = Player.Instance.Spellbook.GetSpell(slot).Level;
            float damage = 0;
            if (spellLevel == 0)
            {
                return 0;
            }
            spellLevel--;

            switch (slot)
            {
                case SpellSlot.Q:
                    damage = new float[] { 30, 65, 100, 135, 170 }[spellLevel] + 0.65f * Player.Instance.TotalAttackDamage + (0.06f * target.MaxHealth);
                    break;

                case SpellSlot.E:
                    damage = new float[] { 50, 70, 90, 110, 130 }[spellLevel] + 0.5f * Player.Instance.TotalAttackDamage;
                    break;

                case SpellSlot.R:
                    damage = new float[] { 200, 300, 400 }[spellLevel] + 0.9f * Player.Instance.TotalAttackDamage;
                    break;
            }
            return Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical, damage) - 30;
        }
        public static float GetComboDamage(this AIHeroClient target)
        {
            float damage = 0;
            damage += Player.Instance.GetAutoAttackDamage(target);
            if (Settings.ComboSettings.UseQ && Lib.Q.IsReady())
            {
                damage += Lib.Q.GetDamage(target);
            }
            if (Settings.ComboSettings.UseE && Lib.E.IsReady())
            {
                if (Settings.ComboSettings.UseEStun && Lib.CanStun(target))
                    damage += Lib.E.GetDamage(target);

                damage += Lib.E.GetDamage(target);
            }
            if (Settings.ComboSettings.UseR && Lib.R.IsReady())
            {
                damage += Lib.R.GetDamage(target);
            }
            if (ItemsSettings.UseHydra("Combo") && ItemManager.HasHydra())
            {
                damage += Player.Instance.GetItemDamage(target, ItemId.Ravenous_Hydra_Melee_Only);
            }
            return damage;
        }
    }
}
