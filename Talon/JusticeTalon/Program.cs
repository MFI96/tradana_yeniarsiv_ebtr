using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using Mario_sGangplank.Ultilities;
using SharpDX;
using Settings = JusticeTalon.Config.Modes.Combo;

namespace JusticeTalon
{
    public static class Program
    {
        // Change this line to the champion you want to make the addon for,
        // watch out for the case being correct!
        public const string ChampName = "Talon";

        public static void Main(string[] args)
        {
            // Wait till the loading screen has passed
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            // Verify the champion we made this addon for
            if (Player.Instance.ChampionName != ChampName)
            {
                // Champion is not the one we made this addon for,
                // therefore we return
                return;
            }

            // Initialize the classes that we need
            Config.Initialize();
            SpellManager.Initialize();
            ModeManager.Initialize();
            DamageIndicator.Init();

            // Listen to events we need
            Drawing.OnDraw += OnDraw;
            Orbwalker.OnPostAttack += Orbwalker_Post;
        }

        /*private static void Orbwalker_Pre(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) ||
                Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) ||
                Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear) ||
                Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                ModeManager.Useitems();
                Orbwalker.ResetAutoAttack();
            }
        kk}*/ 
        private static void Orbwalker_Post(AttackableUnit target, EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                if (Config.Modes.Combo.UseR && SpellManager.R.IsReady())
                {
                    SpellManager.R.Cast();
                    Orbwalker.ResetAutoAttack();
                }
                else if (Config.Modes.Combo.UseQ && SpellManager.Q.IsReady())
                {
                    SpellManager.Q.Cast();
                    Orbwalker.ResetAutoAttack();
                }
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                if (Config.Modes.JungleClear.UseQ && SpellManager.Q.IsReady())
                {
                    SpellManager.Q.Cast();
                    Orbwalker.ResetAutoAttack();
                }
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                if (Config.Modes.JungleClear.UseQ && SpellManager.Q.IsReady())
                {
                    SpellManager.Q.Cast();
                    Orbwalker.ResetAutoAttack();
                }
            }
        }


        private static void OnDraw(EventArgs args)
        {
            if (Config.Modes.Skilldraws.drawE)
            {
                Circle.Draw(Color.AntiqueWhite, SpellManager.E.Range, Player.Instance.Position);
            }
            // Draw range circles of our spells
            //Circle.Draw(Color.Red, SpellManager.Q.Range, Player.Instance.Position);
            // TODO: Uncomment if you want those enabled aswell, but remember to enable them
            // TODO: in the SpellManager aswell, otherwise you will get a NullReferenceException
           // Circle.Draw(Color.Green, SpellManager.W.Range, Player.Instance.Position);
            //Circle.Draw(Color.Yellow, SpellManager.E.Range, Player.Instance.Position);
            //Circle.Draw(Color.Red, SpellManager.R.Range, Player.Instance.Position);
        }
    }
}