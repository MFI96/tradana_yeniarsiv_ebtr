using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;

namespace KenchUnbenched
{
    static class KenchUnbenched
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        public static Menu Menu, ComboMenu, HarassMenu, FarmingMenu, DrawMenu, KillStealMenu, SaveMenu;

        public static Spell.Skillshot QSpell;
        public static Spell.Targeted WSpellSwallow;
        public static Spell.Skillshot WSpellSpit;
        public static Spell.Active ESpell;

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.Hero != Champion.TahmKench) return;
			
			QSpell = new Spell.Skillshot(SpellSlot.Q, 800, SkillShotType.Linear, 100, 2000, 75);
			WSpellSwallow = new Spell.Targeted(SpellSlot.W, 250);
			WSpellSpit = new Spell.Skillshot(SpellSlot.W, 900, SkillShotType.Linear, 100, 900, 75);
			ESpell = new Spell.Active(SpellSlot.E);

            Menu = MainMenu.AddMenu("Kench Unbenched", "kbswag");
            Menu.AddGroupLabel("Kench Unbenched");

            ComboMenu = Menu.AddSubMenu("Combo Menu", "combomenuKench");
            ComboMenu.AddGroupLabel("Combo Ayarları");
            ComboMenu.Add("Combo.Q", new CheckBox("Kullan Q"));
            ComboMenu.Add("Combo.QOnlyStun", new CheckBox("Q sadece stunluysa / AA dışındaysa"));
            ComboMenu.Add("Combo.W.Enemy", new CheckBox("W düşmana"));
            ComboMenu.Add("Combo.W.Minion", new CheckBox("W ile minyon fırlat"));
            ComboMenu.Add("Combo.E", new CheckBox("Kullan E"));

            HarassMenu = Menu.AddSubMenu("Harass Menu", "harassmenuKench");
            HarassMenu.AddGroupLabel("Dürtme Ayarları");
            HarassMenu.Add("Harass.Q", new CheckBox("Kullan Q"));
            HarassMenu.Add("Harass.W.Enemy", new CheckBox("W düşmana Kullan"));
            HarassMenu.Add("Harass.W.Minion", new CheckBox("W ile minyon fırlat"));
            HarassMenu.Add("Harass.E", new CheckBox("Kullan E"));

            FarmingMenu = Menu.AddSubMenu("Farm Menu", "farmmenuKench");
            FarmingMenu.AddGroupLabel("Farm Ayarları");
            FarmingMenu.AddLabel("Son Vuruş Ayarları");
            FarmingMenu.Add("LastHit.Q", new CheckBox("Kullan Q"));
            FarmingMenu.AddLabel("Lanetemizleme Ayarları");
            FarmingMenu.Add("WaveClear.Q", new CheckBox("Kullan Q"));
            FarmingMenu.AddLabel("Orman Ayarları");
            FarmingMenu.Add("Jungle.Q", new CheckBox("Kullan Q"));

            KenchSaver.Initialize();

            KillStealMenu = Menu.AddSubMenu("KillSteal Menu");
            KillStealMenu.AddGroupLabel("Kill Çalma Ayarları");
            KillStealMenu.Add("KillSteal.Q", new CheckBox("Kullan Q"));
            KillStealMenu.Add("KillSteal.W.Swallow", new CheckBox("W ile yut"));
            KillStealMenu.Add("KillSteal.W.Spit", new CheckBox("W ile yutup fırlat"));


            DrawMenu = Menu.AddSubMenu("Draw Menu", "drawMenuKench");
            DrawMenu.AddGroupLabel("Gösterge Ayarları");
            DrawMenu.Add("Draw.Q", new CheckBox("Göster Q"));
            DrawMenu.AddColourItem("Draw.Q.Colour");
            DrawMenu.AddSeparator();
            DrawMenu.Add("Draw.W", new CheckBox("Göster W"));
            DrawMenu.AddColourItem("Draw.W.Colour");
            DrawMenu.AddSeparator();
            DrawMenu.Add("Draw.E", new CheckBox("Göster E"));
            DrawMenu.AddColourItem("Draw.E.Colour");
            DrawMenu.AddSeparator();
            DrawMenu.AddLabel("Gösterge Kapatma");
            DrawMenu.AddColourItem("Draw.OFF");

            Drawing.OnDraw += Drawing_OnDraw;
            Obj_AI_Base.OnProcessSpellCast += KenchCheckManager.Obj_AI_Base_OnProcessSpellCast;
            Game.OnTick += Game_OnTick;
        }

        private static void Game_OnTick(EventArgs args)
        {
            StateHandler.KillSteal();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                StateHandler.Combo();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                StateHandler.Harass();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                StateHandler.LastHit();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                StateHandler.JungleClear();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                StateHandler.WaveClear();
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (DrawMenu["Draw.Q"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(QSpell.IsReady() ? DrawMenu.GetColour("Draw.Q.Colour") : DrawMenu.GetColour("Draw.OFF"), QSpell.Range, Player.Instance.Position);
            }
            if (DrawMenu["Draw.W"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(WSpellSwallow.IsReady() ? DrawMenu.GetColour("Draw.W.Colour") : DrawMenu.GetColour("Draw.OFF"), WSpellSwallow.Range, Player.Instance.Position);
                Circle.Draw(WSpellSpit.IsReady() ? DrawMenu.GetColour("Draw.W.Colour") : DrawMenu.GetColour("Draw.OFF"), WSpellSpit.Range, Player.Instance.Position);
            }
            if (DrawMenu["Draw.E"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(ESpell.IsReady() ? DrawMenu.GetColour("Draw.E.Colour") : DrawMenu.GetColour("Draw.OFF"), ESpell.Range, Player.Instance.Position);
            }
        }

    }
}
