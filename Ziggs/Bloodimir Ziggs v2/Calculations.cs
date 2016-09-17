using EloBuddy;
using EloBuddy.SDK;

namespace Bloodimir_Ziggs_v2
{
    internal class Calculations
    {
        private static AIHeroClient Ziggs
        {
            get { return ObjectManager.Player; }
        }

        public static float Qcalc(Obj_AI_Base target)
        {
            return Ziggs.CalculateDamageOnUnit(target, DamageType.Magical,
                new float[] {0, 75, 120, 165, 210, 255}[Program.Q.Level] +
                0.65f*Ziggs.FlatMagicDamageMod);
        }
        public static float Rcalc(Obj_AI_Base target)
        {
            return Ziggs.CalculateDamageOnUnit(target, DamageType.Magical,
                new float[] {0, 200, 300, 400}[Program.R.Level] +
                0.72f*Ziggs.FlatMagicDamageMod);
        }

        public static float Passivecalc(Obj_AI_Base target)
        {
            return Ziggs.CalculateDamageOnUnit(target, DamageType.Magical,
                new float[] {20, 24, 28, 32, 36, 40, 44, 48, 56, 64, 72, 80, 88, 100, 112, 124, 136, 148, 160}[
                    Ziggs.Level] +
                0.38f*Ziggs.FlatMagicDamageMod);
        }
    }
}