using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using Color = System.Drawing.Color;

namespace KGragas
{
 /*   internal class Insec
    {
        public static bool InsecActive;
        public static AIHeroClient EnemyTarget;

        public static AIHeroClient InsecTarget
        {
            get { return EnemyTarget; }
        }

        private static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }
        public static void Init()
        {

            Game.OnTick += delegate { InsecActive = false; };
                var Q = Program.Q;
                var W = Program.W;
                var E = Program.E;
                var R = Program.R;
                var alvo = TargetSelector.GetTarget(Program.Q.Range, DamageType.Magical);
                var Rminions = EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(m => m.IsValidTarget(Program.R.Range) && (DamageLib.RCalc(m) > m.Health));
                var predPos = Prediction.Position.PredictLinearMissile(alvo, Program.R.Range, Program.R.Width, Program.R.CastDelay, Program.R.Speed, int.MaxValue, null, false);
                if (!alvo.IsValid()) return;

                if (R.IsReady() && (alvo.Distance(_Player.Position) < 900) && (InsecActive = true) && Program.Misc["Ult_" + alvo.BaseSkinName].Cast<CheckBox>().CurrentValue)
                {
                    R.Cast(predPos.CastPosition + 200);

                }
                if (R.IsReady() && (alvo.Distance(_Player.Position) < 1100) && E.IsReady() && (InsecActive = true) && Program.Misc["Ult_" + alvo.BaseSkinName].Cast<CheckBox>().CurrentValue)
                {
                    E.Cast(alvo);
                    R.Cast(predPos.CastPosition + 200);


                }
        
        
        }



    }*/
}
