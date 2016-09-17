using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using SharpDX;

namespace UBKennen
{
    internal class Events
    {
        public static void Gapcloser_OnGapCloser(AIHeroClient who, Gapcloser.GapcloserEventArgs e)
        {
            if (Config.MiscMenu["useEAG"].Cast<CheckBox>().CurrentValue 
                && who.IsEnemy 
                && e.End.Distance(ObjectManager.Player) < 200)
            {
                Spells.E.Cast();
            }

            if (Config.MiscMenu["useWAG"].Cast<CheckBox>().CurrentValue 
                && who.IsEnemy 
                && e.End.Distance(ObjectManager.Player) < 200)
            {
                Spells.W.Cast();
            }

            if (Config.MiscMenu["useQAG"].Cast<CheckBox>().CurrentValue 
                && who.IsEnemy 
                && e.End.Distance(ObjectManager.Player) < 200)
            {
                Spells.Q.Cast(e.End);
            }
         }
    }
}
