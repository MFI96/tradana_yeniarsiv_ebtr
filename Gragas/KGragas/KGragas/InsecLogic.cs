using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Utils;
using System.Linq;
using SharpDX;

namespace KGragas
{
    internal class InsecLogic
    {
        public static void Insec()
        {
            var Player = Program.PlayerInstance;
            var R = Program.R;
            var  insecpos = Program.insecpos;
            var mov = Program.movingawaypos;
            var eqpos = Program.eqpos;
           var alvo = TargetSelector.GetTarget(Program.R.Range, DamageType.Magical);

           eqpos = Player.Position.Extend(alvo,R.Range + 300).To3D();
           insecpos = Player.Position.Extend(alvo.Position, Player.Distance(alvo) + 200).To3D();
           mov = Player.Position.Extend(alvo.Position, Player.Distance(alvo) + 300).To3D();

           if (Program.Misc["Key"].Cast<KeyBind>().CurrentValue)
           {
           Orbwalker.OrbwalkTo(Game.CursorPos);   

               if (alvo.IsFacing(Player) == false && alvo.IsMoving & (R.IsInRange(insecpos) && alvo.Distance(insecpos) < 300))
                   R.Cast(mov); 

               if (R.IsInRange(insecpos) && alvo.Distance(insecpos) < 300 && alvo.IsFacing(Player) && alvo.IsMoving)
                   R.Cast(eqpos);

               else if (R.IsInRange(insecpos) && alvo.Distance(insecpos) < 300)
                   R.Cast(insecpos);

           }



        }






    }
}
