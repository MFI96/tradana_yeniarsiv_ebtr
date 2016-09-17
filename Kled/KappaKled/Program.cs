using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace KappaKled
{
    class Program
    {
        public static AIHeroClient user = Player.Instance;
        public static Menu MenuIni, AutoMenu, ComboMenu, HarassMenu, LaneClearMenu, KillStealMenu;
        
        public static Spell.Skillshot Q;
        public static Spell.Skillshot E;

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if(user == null || user.Hero != Champion.Kled) return;
            
            Q = new Spell.Skillshot(SpellSlot.Q, 750, SkillShotType.Linear, 250, 3000, 40) {AllowedCollisionCount = 0};
            E = new Spell.Skillshot(SpellSlot.E, 550, SkillShotType.Linear, 250, 950, 100) { AllowedCollisionCount = int.MaxValue };

            MenuIni = MainMenu.AddMenu("KappaKled", "KappaKled");
            AutoMenu = MenuIni.AddSubMenu("Auto");
            ComboMenu = MenuIni.AddSubMenu("Combo");
            HarassMenu = MenuIni.AddSubMenu("Harass");
            LaneClearMenu = MenuIni.AddSubMenu("LaneClear");
            KillStealMenu = MenuIni.AddSubMenu("KillSteal");

            AutoMenu.Add("GapQkled", new CheckBox("Rakibe Yaklaşma/Uzaklaşma için Q (Kled)"));
            AutoMenu.Add("GapQskaarl", new CheckBox("Rakibe Yaklaşma/Uzaklaşma için Q (Skaarl)"));

            ComboMenu.Add("Q", new CheckBox("Kullan Q"));
            ComboMenu.Add("selectedQ", new CheckBox("Qyu sadece seçilen hedefe Kullan (Skaarl)", false));
            ComboMenu.Add("E", new CheckBox("Kullan E"));
            ComboMenu.Add("E2mouse", new CheckBox("E2'yi mouse'a doğru Kullan", false));
            ComboMenu.Add("E2ally", new CheckBox("E2 Dostlara doğru Kullan"));

            HarassMenu.Add("Q", new CheckBox("Kullan Q"));
            HarassMenu.Add("E", new CheckBox("Kullan E"));


            KillStealMenu.Add("Q", new CheckBox("Kullan Q"));
            KillStealMenu.Add("E", new CheckBox("Kullan E"));

            State.Init();
            Modes.Base.Init();
            Game.OnTick += Game_OnTick;
            //Drawing.OnDraw += Drawing_OnDraw;
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            //new Geometry.Polygon.Rectangle(user.ServerPosition, user.ServerPosition.Extend(Game.CursorPos, Q.Range).To3D(), Q.Width).Draw(System.Drawing.Color.AliceBlue);
        }

        private static void Game_OnTick(EventArgs args)
        {
            Q = State.MyCurrentState(State.Current.Kled) ? new Spell.Skillshot(SpellSlot.Q, 700, SkillShotType.Cone, 250, 1600, 90) { AllowedCollisionCount = int.MaxValue } : new Spell.Skillshot(SpellSlot.Q, 750, SkillShotType.Linear, 250, 3000, 40) { AllowedCollisionCount = 0 };
        }
    }
}
