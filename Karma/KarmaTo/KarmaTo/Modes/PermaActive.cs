using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Constants;
using EloBuddy.SDK.Events;
using Settings = KarmaTo.Config.Modes.PermaActive;

namespace KarmaTo.Modes
{
    public sealed class PermaActive : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return true;
        }

        public override void Execute()
        {

        }
        public void onSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!Settings.autoShieldSpell)
                return;
            if (sender.Team != Utils.getPlayer().Team && args.Target != null && sender is AIHeroClient && args.Target is AIHeroClient)
            {
                SpellManager.castE((Obj_AI_Base)args.Target, false);
            }
        }

        public void OnBasicAttack(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!Settings.autoShieldTurret)
                return;

            if (sender is Obj_AI_Turret)
            {
                if (sender.IsEnemy && args.Target != null &&
                    args.Target is AIHeroClient)
                {
                    SpellManager.castE((Obj_AI_Base)args.Target, false);
                }
            }
        }
        //AntiGapCloser
        public void OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (Settings.antiGapCloser)
            {
                if (sender.Team == Utils.getPlayer().Team && sender.IsValidTarget(E.Range))
                {
                    E.Cast(sender);
                }
            }
        }
    }
}
