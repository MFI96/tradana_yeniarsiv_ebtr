using System;
using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;

namespace KappAIO.Champions
{
    public abstract class Base
    {
        public static AIHeroClient user = Player.Instance;
        public static string MenuName = "KappAIO " + user.ChampionName;
        public static readonly List<Spell.SpellBase> SpellList = new List<Spell.SpellBase>();
        public static Menu MenuIni, AutoMenu, ComboMenu, HarassMenu, JungleClearMenu, LaneClearMenu, KillStealMenu, DrawMenu;
        public abstract void Active();
        public abstract void Combo();
        public abstract void Flee();
        public abstract void Harass();
        public abstract void LastHit();
        public abstract void LaneClear();
        public abstract void JungleClear();
        public abstract void KillSteal();
        public abstract void Draw();

        protected Base()
        {
            Game.OnTick += this.Game_OnTick;
            Drawing.OnEndScene += this.Drawing_OnEndScene;
        }

        public virtual void Drawing_OnEndScene(EventArgs args)
        {
            this.Draw();
        }

        public virtual void Game_OnTick(EventArgs args)
        {
            if (user.IsDead)
                return;

            var activemode = Orbwalker.ActiveModesFlags;
            this.Active();
            this.KillSteal();
            if (activemode.HasFlag(Orbwalker.ActiveModes.Combo)) this.Combo();
            if (activemode.HasFlag(Orbwalker.ActiveModes.Harass)) this.Harass();
            if (activemode.HasFlag(Orbwalker.ActiveModes.LaneClear)) this.LaneClear();
            if (activemode.HasFlag(Orbwalker.ActiveModes.LastHit)) this.LastHit();
            if (activemode.HasFlag(Orbwalker.ActiveModes.JungleClear)) this.JungleClear();
            if (activemode.HasFlag(Orbwalker.ActiveModes.Flee)) this.Flee();
        }
    }
}
