using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;
using Color = System.Drawing.Color;
using KillSteal = AddonTemplate.KillSteal;

namespace AddonTemplate
{
    public static class DamageHelper
    {
        private const int BarWidth = 105;
        private const float LineThickness = 10;
        private static readonly Vector2 BarOffset = new Vector2(2.5f, 9);

        public static void Initialize()
        {
            Drawing.OnEndScene += OnEndScene;
        }

        private static void OnEndScene(EventArgs args)
        {
            if (Config.Modes.Draw.DamageIndicator)
            {
                foreach (var unit in EntityManager.Heroes.Enemies.Where(u => u.IsValidTarget() && u.IsHPBarRendered))
                {
                    var damage = GetEDamage(unit);
                    if (damage <= 0)
                    {
                        continue;
                    }
                    var damagePercentage = ((unit.TotalShieldHealth() - damage) > 0 ? (unit.TotalShieldHealth() - damage) : 0) /
                                            (unit.MaxHealth + unit.AllShield + unit.AttackShield + unit.MagicShield);
                    var currentHealthPercentage = unit.TotalShieldHealth() / (unit.MaxHealth + unit.AllShield + unit.AttackShield + unit.MagicShield);
                
                    var startPoint = new Vector2((int)(unit.HPBarPosition.X + BarOffset.X + damagePercentage * BarWidth), (int)(unit.HPBarPosition.Y + BarOffset.Y));
                    var endPoint = new Vector2((int)(unit.HPBarPosition.X + BarOffset.X + currentHealthPercentage * BarWidth) + 0.5f, (int)(unit.HPBarPosition.Y + BarOffset.Y));

                    Drawing.DrawLine(startPoint, endPoint, LineThickness, Color.FromArgb(170, Color.Green));
                }
            }
        }

        public static int getEStacks(this Obj_AI_Base target)
        {
            var buffs =
                ObjectManager.Get<Obj_GeneralParticleEmitter>().Where(b => b.Position.Distance(target.ServerPosition) < 150 && b.Name.Contains("twitch_poison_counter_"));
            if (buffs.Any())
            {
                string str = buffs.ElementAt(0).Name;
                return str["twitch_poison_counter_0".Length] - 48; // 48 : ascii to number
            }
            return 0;
        }

        public static float GetEDamage(Obj_AI_Base target)
        {
            var bc = getEStacks(target);
            if (bc == 0)
                return 0;
            var total = Player.Instance.CalculateDamageOnUnit(target, DamageType.True,
                (float)(new[] { 20, 35, 50, 65, 80 }[SpellManager.E.Level - 1] + 
                (new[] { 15, 20, 25, 30, 35 }[SpellManager.E.Level - 1] * getEStacks(target) +
                0.2 * Player.Instance.FlatMagicDamageMod +
                0.25 * Player.Instance.FlatPhysicalDamageMod)));
            return total;
        }
    }
}
