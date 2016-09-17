using System.Linq;
using EloBuddy;
using EloBuddy.SDK;


// Using the config like this makes your life easier, trust me
using Settings = Farofakids_Galio.Config.Modes.Combo;

namespace Farofakids_Galio.Modes
{
    public sealed class Combo : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()  
        {
            var target = TargetSelector.GetTarget(800, DamageType.Magical);
            if (target != null) 
            {
                if (Program.rActive /*|| justR*/)
                {
                    return;
                }

                if (Settings.UseE && E.IsReady() && target.IsValidTarget(E.Range))
                {
                    var pred = E.GetPrediction(target);
                    E.Cast(pred.CastPosition);
                }
                if (Settings.UseQ && Q.IsReady() && target.IsValidTarget(Q.Range))
                {
                    var pred = Q.GetPrediction(target);
                    Q.Cast(pred.CastPosition);
                }

                if (Settings.UseW && W.IsReady() && target.IsValidTarget(W.Range) /*&& Program.SafeWCast(target)*/)
                {
                    /*var pred = W.GetPrediction(target);
                    W.Cast(pred.CastPosition);*/
                }
                if (Settings.UseR && R.IsReady() && Player.Instance.CountEnemiesInRange(R.Range) >= Settings.UseRmin)
                {
                    R.Cast();
                    if (W.IsReady())
                        W.Cast(Player.Instance);
                }

            }
        }

    }
}
