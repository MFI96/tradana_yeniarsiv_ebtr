using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using Settings = Zac.Config.JungleClear.JungleClearMenu;

namespace Zac.Modes
{
    public sealed class JungleClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear);
        }

        public override void Execute()
        {
            if (Q.IsReady() && Settings.UseQ)
            {
                var minionsQ =
                    EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.Position, Q.Range)
                        .OrderByDescending(a => a.MaxHealth)
                        .FirstOrDefault();


                if (minionsQ != null && minionsQ.IsValid)
                {
                    Q.Cast(minionsQ);
                    return;
                }
            }

            if (W.IsReady() && Settings.UseW)
            {
                var minionsW =
                    EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.Position, W.Range - 75)
                        .OrderByDescending(a => a.MaxHealth)
                        .FirstOrDefault();

                if (minionsW != null && minionsW.IsValid)
                {
                    W.Cast();
                }
            }
            //if (!Events.ChannelingE)
            //{
            //    var blob = ObjectManager.Get<Obj_AI_Base>().Where(a => a.IsValid && a.Name == "BlobDrop" && a.Team == Player.Instance.Team && a.Distance(Player.Instance.Position) < W.Range).OrderBy(a => a.Distance(Player.Instance.Position)).FirstOrDefault();
            //    if (blob != null && Orbwalker.CanMove)
            //    {
            //        Player.IssueOrder(GameObjectOrder.MoveTo, blob.Position);
            //        return;
            //    }
            //}
        }
    }
}