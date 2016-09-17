using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Rendering;
using SharpDX;

namespace Black_Swan_Akali
{
    class Akali
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            // Validate Champion
            if (Player.Instance.Hero != Champion.Akali) return;

            // Initialize Classes
            MenuDesigner.Initialize();
            ModeController.Initialize();

            // Events
            Game.OnUpdate += OnUpdate;
            Orbwalker.OnPostAttack += OnPostAttack;
            Gapcloser.OnGapcloser += OnGapcloser;
            Drawing.OnDraw += OnDraw;
        }

        private static void OnUpdate(EventArgs args)
        {
            if (Return.UseRKs)
            {
                if (!Spells.R.IsReady()) return;

                foreach (AIHeroClient target in EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(Spells.R.Range) && !x.HasBuffOfType(BuffType.Invulnerability)))
                {
                    if ((Player.Instance.GetSpellDamage(target, SpellSlot.R)) > target.TotalShieldHealth() + 5)
                        Spells.R.Cast(target);
                }
            }

            if (Return.UseQKs)
            {
                if (!Spells.Q.IsReady()) return;

                foreach (AIHeroClient target in EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(Spells.Q.Range) && !x.HasBuffOfType(BuffType.Invulnerability)))
                {
                    if ((Player.Instance.GetSpellDamage(target, SpellSlot.Q)) > target.TotalShieldHealth() + 5)
                        Spells.Q.Cast(target);
                }
            }
        }

        private static void OnPostAttack(AttackableUnit target, EventArgs args)
        {
            if (ModeController.OrbCombo && Return.UseECombo && Spells.E.IsReady())
            {
                var t = TargetSelector.GetTarget(Spells.E.Range, DamageType.Magical);

                if (t != null)
                    Spells.E.Cast();
            }
        }

        private static void OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (!sender.IsEnemy) return;

            if (Return.UseRGapclose && Spells.R.IsReady() && sender.IsValidTarget(Spells.R.Range))
                Spells.R.Cast(sender);
        }

        private static void OnDraw(EventArgs args)
        {
            if (Return.DrawQRange && Spells.Q.IsReady())
                Circle.Draw(Color.Aqua, Spells.Q.Range, 1, Player.Instance.Position);

            if (Return.DrawRRange && Spells.R.IsReady())
                Circle.Draw(Color.PaleVioletRed, Spells.R.Range, 1, Player.Instance.Position);
        }
    }
}
