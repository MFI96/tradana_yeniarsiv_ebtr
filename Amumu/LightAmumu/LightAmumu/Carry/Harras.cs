using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using System.Collections.Generic;
using System.Linq;

namespace LightAmumu.Carry
{
    public sealed class Harras : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
        }

        public override void Execute()
        {
            foreach (var target in EntityManager.Heroes.Enemies.Where(target => target.IsValidTarget(E.Range)))
            {
                E.Cast();
            }
        }
    }
}