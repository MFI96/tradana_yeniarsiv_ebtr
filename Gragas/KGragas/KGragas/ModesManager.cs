using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Utils;
using System.Linq;


namespace KGragas
{


    internal class ModesManager
    {


        public static void Combo()
        {
            var Q = Program.Q;
            var W = Program.W;
            var E = Program.E;
            var R = Program.R;
            var alvo = TargetSelector.GetTarget(R.Range, DamageType.Magical);
            var predPosQ = Prediction.Position.PredictLinearMissile(alvo, Q.Range, Q.Width, Q.CastDelay, Q.Speed, int.MaxValue, null, false);
            var predPos = Prediction.Position.PredictLinearMissile(alvo, R.Range, R.Width, R.CastDelay, R.Speed, int.MaxValue, null, false);
            var predPosE = Prediction.Position.PredictLinearMissile(alvo, E.Radius, E.Width, E.CastDelay, E.Speed, 0, null, true);
            if (!alvo.IsValid()) return;

            if (Q.IsReady() && alvo.IsValidTarget(Q.Range) && Program.ModesMenu1["ComboQ"].Cast<CheckBox>().CurrentValue)
            {
                Q.Cast(alvo);
                Program.CastedQ = true;
            }

            if (W.IsReady() && alvo.IsValidTarget(E.Range) && Program.ModesMenu1["ComboW"].Cast<CheckBox>().CurrentValue)
            {
                W.Cast();

            }
            if (E.IsReady() && alvo.IsValidTarget(E.Range) && Program.ModesMenu1["ComboE"].Cast<CheckBox>().CurrentValue)
            {
                E.Cast(alvo);


            }
            if (R.IsReady() && alvo.IsValidTarget(R.Range) && !(Program.ModesMenu1["ComboR"].Cast<CheckBox>().CurrentValue) && Program.ModesMenu1["Ult_" + alvo.BaseSkinName].Cast<CheckBox>().CurrentValue)//&& !(Q.IsInRange(alvo)))
            {
                R.Cast(predPos.CastPosition + 100);

            }
            if (E.IsReady() && alvo.IsValidTarget(R.Range) && !(Q.IsInRange(alvo)))
            {
                E.Cast(predPosE.CastPosition);

            }

            /* var Player = Program.PlayerInstance;
             var R = Program.R;
             var insecpos = Program.insecpos;
            var mov = Program.movingawaypos;
             var eqpos = Program.eqpos;
             var alvo = TargetSelector.GetTarget(Program.R.Range, DamageType.Magical);
             eqpos = Player.Position.Extend(alvo.Position, Player.Distance(alvo)).To3D();
             insecpos = Player.Position.Extend(alvo.Position, Player.Distance(alvo) + 200).To3D();
             mov = Player.Position.Extend(alvo.Position, Player.Distance(alvo) + 350).To3D();
             eqpos = Player.Position.Extend(alvo.Position, Player.Distance(alvo) + 200).To3D();
             if (R.IsReady())
             {


            
                 if (insecpos.Distance(Player.Position) < R.Range - 20)
                 {
                     if (Player.Distance(insecpos.Extend(alvo.Position.To2D(), 900 - alvo.Distance(alvo))) < Program.E.Range && alvo.IsFacing(Player))
                     {
                         R.Cast(insecpos);
                     }
                 }
             }*/




        }

        public static void Harass()
        {

            var Q = Program.Q;
            var W = Program.W;
            var E = Program.E;
            var R = Program.R;
            var alvo = TargetSelector.GetTarget(Program.R.Range, DamageType.Magical);
            if (!alvo.IsValid()) return;
            if ((Program._Player.ManaPercent <= Program.ModesMenu1["ManaH"].Cast<Slider>().CurrentValue))
            {
                return;
            }
            if (Q.IsReady() && alvo.IsValidTarget(Q.Range) && Program.ModesMenu1["HarassQ"].Cast<CheckBox>().CurrentValue)
            {
                Q.Cast(alvo);
                Program.CastedQ = true;
            }

            if (W.IsReady() && alvo.IsValidTarget(E.Range) && Program.ModesMenu1["HarassW"].Cast<CheckBox>().CurrentValue)
            {
                W.Cast();

            }
            if (E.IsReady() && alvo.IsValidTarget(E.Range) && Program.ModesMenu1["HarassE"].Cast<CheckBox>().CurrentValue)
            {
                E.Cast(alvo);


            }
        }

        public static void LaneClear()
        {
            var Q = Program.Q;
            var W = Program.W;
            var E = Program.E;
            var R = Program.R;
            var minions = EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(m => m.IsValidTarget(Q.Range));
            var minion = EntityManager.MinionsAndMonsters.EnemyMinions.Where(t => t.IsInRange(Player.Instance.Position, Q.Range) && !t.IsDead && t.IsValid && !t.IsInvulnerable).Count();
            if (minions == null) return;
            if ((Program._Player.ManaPercent <= Program.ModesMenu2["ManaF"].Cast<Slider>().CurrentValue))
            {
                return;
            }

            if (Q.IsReady() && Program.Q.IsInRange(minions) && Program.ModesMenu2["FarmQ"].Cast<CheckBox>().CurrentValue && (minion >= Program.ModesMenu2["MinionQ"].Cast<Slider>().CurrentValue))
            {

                Q.Cast(Q.GetPrediction(minions).CastPosition);

                Program.CastedQ = true;

            }

            if (W.IsReady() && E.IsInRange(minions) && Program.ModesMenu2["FarmW"].Cast<CheckBox>().CurrentValue)
            {
                W.Cast();

            }
            if (E.IsReady() && Program.E.IsInRange(minions) && Program.ModesMenu2["FarmE"].Cast<CheckBox>().CurrentValue)
            {
                E.Cast(minions);

            }

        }

        public static void LastHit()
        {
            var Q = Program.Q;
            var W = Program.W;
            var E = Program.E;
            var R = Program.R;
            var qminions = EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(m => m.IsValidTarget(Program.Q.Range) && (DamageLib.QCalc(m) > m.Health));
            var eminions = EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(m => m.IsValidTarget(Program.E.Range) && (DamageLib.ECalc(m) > m.Health));
            var minion = EntityManager.MinionsAndMonsters.EnemyMinions.Where(t => t.IsInRange(Player.Instance.Position, Program.E.Range) && !t.IsDead && t.IsValid && !t.IsInvulnerable).Count();
            if (qminions == null) return;
            var prediction = Program.Q.GetPrediction(qminions);
            if (Q.IsReady() && Program.Q.IsInRange(qminions) && Program.ModesMenu2["LastQ"].Cast<CheckBox>().CurrentValue && qminions.Health < DamageLib.QCalc(qminions))

                Q.Cast(Q.GetPrediction(qminions).CastPosition);

            Program.CastedQ = true;

            if (Program.E.IsReady() && Program.E.IsInRange(eminions) && Program.ModesMenu2["LastE"].Cast<CheckBox>().CurrentValue && eminions.Health < DamageLib.ECalc(eminions))
            {
                E.Cast(eminions);
            }



        }

        public static void JungleClear()
        {
            var Q = Program.Q;
            var W = Program.W;
            var E = Program.E;
            var R = Program.R;

            var jungleMonsters = EntityManager.MinionsAndMonsters.GetJungleMonsters().OrderByDescending(j => j.Health).FirstOrDefault(j => j.IsValidTarget(Program.Q.Range));
            var minioon = EntityManager.MinionsAndMonsters.EnemyMinions.Where(t => t.IsInRange(Player.Instance.Position, Program.E.Range) && !t.IsDead && t.IsValid && !t.IsInvulnerable).Count();
            if (jungleMonsters == null) return;
            if ((Program._Player.ManaPercent <= Program.ModesMenu2["ManaJ"].Cast<Slider>().CurrentValue))
            {
                return;
            }
            if (Q.IsReady() && Q.IsInRange(jungleMonsters) && Program.ModesMenu2["JungQ"].Cast<CheckBox>().CurrentValue)

                Q.Cast(Q.GetPrediction(jungleMonsters).CastPosition);

            Program.CastedQ = true;


            if (W.IsReady() && E.IsInRange(jungleMonsters) && Program.ModesMenu2["JungW"].Cast<CheckBox>().CurrentValue)
            {
                W.Cast();

            }
            if (E.IsReady() && E.IsInRange(jungleMonsters) && Program.ModesMenu2["JungE"].Cast<CheckBox>().CurrentValue)
            {
                E.Cast(jungleMonsters);

            }
        }



        public static void KillSteal()
        {
            var Q = Program.Q;
            var W = Program.W;
            var E = Program.E;
            var R = Program.R;


            foreach (var enemy in EntityManager.Heroes.Enemies.Where(a => !a.IsDead && !a.IsZombie && a.Health > 0))
            {
                if (enemy.IsValidTarget(R.Range) && enemy.HealthPercent <= 40)
                {

                    if (DamageLib.ECalc(enemy) + DamageLib.ECalc(enemy) + DamageLib.RCalc(enemy) >= enemy.Health)
                    {
                        if (Program.ModesMenu1["KQ"].Cast<CheckBox>().CurrentValue && (DamageLib.ECalc(enemy) >= enemy.Health) && Q.IsInRange(enemy) && Q.IsReady())
                        {
                            Q.Cast(Q.GetPrediction(enemy).CastPosition);

                            Program.CastedQ = true;
                        }
                        if (Program.ModesMenu1["KE"].Cast<CheckBox>().CurrentValue && (DamageLib.ECalc(enemy) >= enemy.Health) && E.IsInRange(enemy) && E.IsReady())
                        { R.Cast(enemy); }
                        if (Program.ModesMenu1["KR"].Cast<CheckBox>().CurrentValue && (DamageLib.RCalc(enemy) >= enemy.Health) && R.IsInRange(enemy) && R.IsReady())
                        { R.Cast(enemy); }
                    }

                }
            }

        }


        public static void Insec()
        {
            var Player = Program.PlayerInstance;
            var R = Program.R;
            var Q = Program.Q;
            var E = Program.E;
            var insecpos = Program.insecpos;
            var mov = Program.movingawaypos;
            var eqpos = Program.eqpos;
            var alvo = TargetSelector.GetTarget(Program.R.Range, DamageType.Magical);
            Orbwalker.MoveTo(Game.CursorPos);

            eqpos = Player.Position.Extend(alvo, R.Range + 300).To3D();
            insecpos = Player.Position.Extend(alvo.Position, Player.Distance(alvo) + 200).To3D();
            mov = Player.Position.Extend(alvo.Position, Player.Distance(alvo) + 300).To3D();

            if (R.IsReady() && !(alvo == null))
            {
                if (alvo.IsFacing(Player) == false && alvo.IsMoving & (R.IsInRange(insecpos) && alvo.Distance(insecpos) < 300))
                    R.Cast(mov);

                if (R.IsInRange(insecpos) && alvo.Distance(insecpos) < 300 && alvo.IsFacing(Player) && alvo.IsMoving)
                    R.Cast(eqpos);

                else if (R.IsInRange(insecpos) && alvo.Distance(insecpos) < 300)
                    R.Cast(insecpos);

                if (Q.IsReady() && alvo.IsValidTarget(Q.Range))
                {
                    Q.Cast(alvo);
                    Program.CastedQ = true;
                }

                if (E.IsReady() && alvo.IsValidTarget(E.Range))
                {
                    E.Cast(alvo);


                }







            }






        }



    }
}
    

