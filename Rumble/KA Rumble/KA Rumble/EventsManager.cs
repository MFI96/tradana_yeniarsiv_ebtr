using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;

namespace KA_Rumble
{
    internal static class EventsManager
    {
        public static void Initialize()
        {
            Gapcloser.OnGapcloser += Gapcloser_OnGapcloser;
        }

        private static void Gapcloser_OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (!sender.IsEnemy) return;

            if (sender.IsValidTarget(SpellManager.E.Range) && Player.Instance.Mana < 60)
            {
                SpellManager.E.Cast(sender);
            }
        }
    }
}
