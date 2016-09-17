using EloBuddy;
using EloBuddy.SDK;

namespace PartyJanna.Modes
{
    public sealed class LaneClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear);
        }

        public override void Execute()
        {
            foreach (Obj_AI_Minion enemyMinion in EntityManager.MinionsAndMonsters.GetLaneMinions())
            {
                if (enemyMinion.IsInRange(Player.Instance, Q.Range))
                {
                    Q.Cast(enemyMinion);
                }
            }
        }
    }
}
