﻿namespace KappaUtility.Trackers
{
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    using Color = System.Drawing.Color;

    internal class GanksDetector
    {
        private static readonly AIHeroClient _Hero;

        private static float DrawDuration;

        private static float EnterTime;

        private static float Allypingtimer;

        private static float Enemypingtimer;

        public static Menu GankMenu { get; private set; }

        protected static bool loaded = false;

        internal static void OnLoad()
        {
            GankMenu = Load.UtliMenu.AddSubMenu("GanksDetector");
            GankMenu.AddGroupLabel("Gank Uyarıcı");
            GankMenu.Add("enable", new CheckBox("Aktif", false));
            GankMenu.Add("ping", new CheckBox("Şampiyonlara ping at", false));
            GankMenu.Add("cd", new Slider("Bekleme Süresi (Saniye)", 15));
            GankMenu.Add("range", new Slider("Tespit etme menzili", 2500, 500, 6500));
            GankMenu.AddSeparator();
            GankMenu.AddGroupLabel("Şunları tespit et -");
            GankMenu.AddGroupLabel("Dostlar:");
            foreach (var hero in ObjectManager.Get<AIHeroClient>())
            {
                var cb = new CheckBox(hero.BaseSkinName) { CurrentValue = true };
                if (hero.Team == Player.Instance.Team && !hero.IsMe)
                {
                    GankMenu.Add("AllyGank" + hero.BaseSkinName, cb);
                }
            }

            GankMenu.AddGroupLabel("Düşmanlar:");
            foreach (var hero in ObjectManager.Get<AIHeroClient>())
            {
                var cb = new CheckBox(hero.BaseSkinName) { CurrentValue = true };
                if (hero.Team != Player.Instance.Team)
                {
                    GankMenu.Add("EnemyGank" + hero.BaseSkinName, cb);
                }
            }
            loaded = true;
        }

        internal static void OnEndScene()
        {
            if (!loaded)
            {
                return;
            }

            if (!GankMenu["enable"].Cast<CheckBox>().CurrentValue)
            {
                return;
            }

            var range = GankMenu["range"].Cast<Slider>().CurrentValue;
            var cd = GankMenu["cd"].Cast<Slider>().CurrentValue;
            var heros =
                EntityManager.Heroes.AllHeroes.Where(
                    x =>
                    x.IsInRange(Player.Instance.Position, range) && !x.IsDead && !x.IsInvulnerable && Detect(x) && !x.IsMe
                    && Game.Time - DrawDuration < cd);
            foreach (var hero in heros.Where(hero => hero != null))
            {
                var c = hero.IsAlly ? Color.FromArgb(125, 0, 255, 0) : Color.FromArgb(125, 255, 0, 0);
                Drawing.DrawLine(Drawing.WorldToMinimap(Player.Instance.Position), Drawing.WorldToMinimap(hero.Position), 5, c);
            }
        }

        internal static void OnUpdate()
        {
            if (!loaded)
            {
                return;
            }

            if (Player.Instance.IsDead || !GankMenu["enable"].Cast<CheckBox>().CurrentValue)
            {
                return;
            }

            var range = GankMenu["range"].Cast<Slider>().CurrentValue;
            var cd = GankMenu["cd"].Cast<Slider>().CurrentValue;
            var heros =
                EntityManager.Heroes.AllHeroes.Where(x => x.IsInRange(Player.Instance.Position, range) && !x.IsDead && !x.IsInvulnerable && !x.IsMe);
            foreach (var hero in
                heros.Where(hero => hero != null && Detect(hero) && hero.IsInRange(Player.Instance.Position, range)))
            {
                if (Game.Time - EnterTime > cd)
                {
                    DrawDuration = Game.Time;
                    if (hero.IsAlly)
                    {
                        if (GankMenu["ping"].Cast<CheckBox>().CurrentValue)
                        {
                            if (Game.Time - Allypingtimer > cd)
                            {
                                TacticalMap.ShowPing(PingCategory.OnMyWay, hero, true);
                            }
                        }

                        Allypingtimer = Game.Time;
                    }

                    if (hero.IsEnemy)
                    {
                        if (GankMenu["ping"].Cast<CheckBox>().CurrentValue)
                        {
                            if (Game.Time - Enemypingtimer > cd)
                            {
                                TacticalMap.ShowPing(PingCategory.Danger, hero, true);
                            }
                        }

                        Enemypingtimer = Game.Time;
                    }
                }

                EnterTime = Game.Time;
            }
        }

        internal static void OnDraw()
        {
            if (!loaded)
            {
                return;
            }
            if (!GankMenu["enable"].Cast<CheckBox>().CurrentValue)
            {
                return;
            }

            var range = GankMenu["range"].Cast<Slider>().CurrentValue;
            var cd = GankMenu["cd"].Cast<Slider>().CurrentValue;
            var heros =
                EntityManager.Heroes.AllHeroes.Where(
                    x =>
                    x.IsInRange(Player.Instance.Position, range) && !x.IsDead && !x.IsInvulnerable && Detect(x) && Game.Time - DrawDuration < cd
                    && !x.IsMe);
            foreach (var hero in heros.Where(hero => hero != null))
            {
                var c = hero.IsAlly ? Color.FromArgb(125, 0, 255, 0) : Color.FromArgb(125, 255, 0, 0);
                Drawing.DrawLine(Drawing.WorldToScreen(Player.Instance.Position), Drawing.WorldToScreen(hero.Position), 5, c);
                Drawing.DrawText(
                    Drawing.WorldToScreen(Player.Instance.ServerPosition.To2D().Extend(hero, 300f).To3D()),
                    Color.White,
                    hero.ChampionName,
                    40);
            }
        }

        internal static bool Detect(AIHeroClient hero)
        {
            if (!GankMenu["enable"].Cast<CheckBox>().CurrentValue || hero.IsMe)
            {
                return false;
            }

            return hero.IsEnemy
                       ? GankMenu["EnemyGank" + hero.BaseSkinName].Cast<CheckBox>().CurrentValue
                       : GankMenu["AllyGank" + hero.BaseSkinName].Cast<CheckBox>().CurrentValue;
        }
    }
}