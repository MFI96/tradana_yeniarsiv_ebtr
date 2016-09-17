using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace KarmaTo
{
    public static class SpellManager
    {
        public static Spell.Skillshot Q { get; private set; }
        public static Spell.Targeted W { get; private set; }
        public static Spell.Targeted E { get; private set; }
        public static Spell.Active R { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 950, SkillShotType.Linear, 250, 1500, 100);
            W = new Spell.Targeted(SpellSlot.W, 675);
            E = new Spell.Targeted(SpellSlot.E, 800);
            R = new Spell.Active(SpellSlot.R);
        }

        public static void castR()
        {
            if (R.IsReady())
            {
                R.Cast();
            }
        }

        public static void castE(Obj_AI_Base target, bool useR)
        {
            if (E.IsReady() && target.IsValidTarget(E.Range))
            {
                if (useR)
                    castR();
                E.Cast(target);
            }
        }

        public static void castQ(Obj_AI_Base target, bool useR, double predictionHitChance)
        {
            if (Q.IsReady() && target.IsValidTarget(Q.Range))
            {
                var pred = Q.GetPrediction(target);
                //No collisions we cast it directly
                if (pred.HitChancePercent >= predictionHitChance && !pred.Collision)
                {
                    if (useR)
                        castR();
                    Q.Cast(pred.CastPosition);
                }
                //Collisions : Damn we need to use something called the brain ...
                else
                {
                    var collision = new Obj_AI_Base();
                    if (pred.CollisionObjects.Length != 0)
                    {
                        collision = pred.CollisionObjects[0];
                        if (collision.IsValidTarget() && target.Distance(collision.Position) < Q.Width)
                        {
                            if (useR)
                                castR();
                            Q.Cast(pred.CastPosition);
                            return;
                        }
                    }
                }
            }
        }

        public static void Initialize()
        {
            // Let the static initializer do the job, this way we avoid multiple init calls aswell
        }
    }
}
