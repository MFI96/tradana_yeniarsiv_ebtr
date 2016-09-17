using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using Color = System.Drawing.Color;

namespace LookSharp
{
    abstract class PluginBase : Program
    {
        protected Menu PluginMenu, ModeMenu, MiscMenu, DrawMenu;
        protected Spell.SpellBase Q, W, E, Q2, W2, E2, R;
        
        //protected bool isMelee { get { return Hero.AttackRange < 300; } }
        protected float[] CD = new float[6], CDtemp = new float[6]; // Qmelee to Erange

        protected PluginBase()
        {
            PluginMenu = MainMenu.AddMenu(Hero.ChampionName, Hero.ChampionName);
            PluginMenu.AddGroupLabel("Information");
            PluginMenu.AddLabel("Made by Lookaside");
            PluginMenu.AddLabel("Please upvote in forums!");
            
            Chat.Print("LookSharp => " + Hero.ChampionName + " Loaded!");
            Game.OnUpdate += OnUpdate;
            Drawing.OnDraw += OnDraw;
        }
        private void OnUpdate(EventArgs args)
        {
            if (!Hero.IsDead && !Shop.IsOpen)
            {
                Plugin.OnTick();
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) Plugin.Combo();
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)) Plugin.Harass();
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)) Plugin.LaneClear();
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)) Plugin.JungleClear();
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit)) Plugin.LastHit();
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee)) Plugin.Flee();
            }
        }

        protected void OnDraw(EventArgs args)
        {
            if (!Hero.IsDead)
            {
                Plugin.OnPaint();
            }
            //Drawing.DrawText(100, 100, Color.Red, CD[0].ToString("0.0"));
        }
        public virtual void OnTick() { }
        public virtual void OnPaint() { }
        public virtual void Combo() { }
        public virtual void Harass() { }
        public virtual void LaneClear() { }
        public virtual void JungleClear() { }
        public virtual void LastHit() { }
        public virtual void Flee() { }

        // helper methods
        public void UpdateCooldowns(bool isMelee)
        {
            if (isMelee)
            {
                CDtemp[0] = Hero.Spellbook.GetSpell(SpellSlot.Q).CooldownExpires;
                CDtemp[1] = Hero.Spellbook.GetSpell(SpellSlot.W).CooldownExpires;
                CDtemp[2] = Hero.Spellbook.GetSpell(SpellSlot.E).CooldownExpires;
            }
            else
            {
                CDtemp[3] = Hero.Spellbook.GetSpell(SpellSlot.Q).CooldownExpires;
                CDtemp[4] = Hero.Spellbook.GetSpell(SpellSlot.W).CooldownExpires;
                CDtemp[5] = Hero.Spellbook.GetSpell(SpellSlot.E).CooldownExpires;
            }
            for (int i = 0; i < 6; ++i)
            {
                CD[i] = CDtemp[i] - Game.Time < 0 ? 0 : CDtemp[i] - Game.Time;
            }
        }

        public Vector3 extend(Vector3 position, Vector3 target, float distance, bool towards) // towards/away from target
        {
            if (towards)
            {
                return position + Vector3.Normalize(target - position) * distance;
            }
            else
            {
                return position + Vector3.Normalize(position - target) * distance;
            }
            
        }

        public bool IsValidTarget(AIHeroClient target)
        {
            return target.IsValid && target.IsEnemy && !target.IsDead && !target.IsInvulnerable && !target.IsZombie;
        }
    }
}
