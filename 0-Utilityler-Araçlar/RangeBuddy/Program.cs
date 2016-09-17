namespace RangeBuddy
{
    using System;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;
    using EloBuddy.SDK.Rendering;

    using SharpDX;

    internal class Program
    {
        private const float ExpRange = 1350f;

        private static readonly float TrtRange = 875f + ObjectManager.Player.BoundingRadius;

        private static Menu rMenu, eMenu, aMenu, tMenu, sMenu;

        private static Obj_AI_Base currentTurret = ObjectManager.Player;

        private static bool turretIsAttackingMe;

        private static readonly SpellSlot[] Spells = { SpellSlot.Q, SpellSlot.W, SpellSlot.E, SpellSlot.R };

        private static void DrawAttack()
        {
            var drawSelf = aMenu["a.s"].Cast<CheckBox>().CurrentValue;
            var drawAlly = aMenu["a.a"].Cast<CheckBox>().CurrentValue;
            var drawEnemy = aMenu["a.e"].Cast<CheckBox>().CurrentValue;
            var thickness = rMenu["cT"].Cast<Slider>().CurrentValue;

            if (!drawAlly && !drawEnemy && !drawSelf)
            {
                return;
            }
            foreach (var hero in
                EntityManager.Heroes.AllHeroes.Where(hero => !hero.IsDead && hero.IsVisible)
                    .Where(
                        hero =>
                        (hero.IsAlly && drawAlly || hero.IsMe && drawSelf || hero.IsEnemy && drawEnemy)
                        && !(hero.IsMe && !drawSelf)))
            {
                var radius = hero.BoundingRadius + hero.AttackRange
                             + (hero.IsAlly
                                    ? EntityManager.Heroes.Enemies.Select(e => e.BoundingRadius)
                                          .DefaultIfEmpty(0)
                                          .Average()
                                    : ObjectManager.Player.BoundingRadius);
                if (hero.VisibleOnScreen)
                {
                    new Circle(Color.Gold, radius, thickness).Draw(hero.Position);
                }
            }
        }

        private static void DrawExperience()
        {
            var drawSelf = eMenu["e.s"].Cast<CheckBox>().CurrentValue;
            var drawAlly = eMenu["e.a"].Cast<CheckBox>().CurrentValue;
            var drawEnemy = eMenu["e.e"].Cast<CheckBox>().CurrentValue;
            var thickness = rMenu["cT"].Cast<Slider>().CurrentValue;

            if (!drawAlly && !drawEnemy && !drawSelf)
            {
                return;
            }
            foreach (var hero in
                EntityManager.Heroes.AllHeroes.Where(hero => !hero.IsDead && hero.IsVisible)
                    .Where(
                        hero =>
                        (hero.IsAlly && drawAlly || hero.IsMe && drawSelf || hero.IsEnemy && drawEnemy)
                        && !(hero.IsMe && !drawSelf) && hero.VisibleOnScreen))
            {
                new Circle(Color.Gray, ExpRange, thickness).Draw(hero.Position);
            }
        }

        private static void DrawTurret()
        {
            var drawAlly = tMenu["t.a"].Cast<CheckBox>().CurrentValue;
            var drawEnemy = tMenu["t.e"].Cast<CheckBox>().CurrentValue;
            var thickness = rMenu["cT"].Cast<Slider>().CurrentValue;
            if (!drawAlly && !drawEnemy)
            {
                return;
            }
            foreach (var entry in EntityManager.Turrets.AllTurrets)
            {
                var turret = entry;
                if (turret == null || !turret.IsValid || turret.IsDead)
                {
                    return;
                }

                var distToTurret = ObjectManager.Player.ServerPosition.Distance(turret.Position);
                if (!(distToTurret < TrtRange + 1500))
                {
                    continue;
                }
                ColorBGRA color = Color.HotPink;
                if (drawAlly && turret.IsAlly)
                {
                    color = Color.Green;
                }
                if (drawEnemy && turret.IsEnemy)
                {
                    color = Color.Yellow;
                    if (distToTurret <= TrtRange)
                    {
                        color = Color.Orange;
                    }
                    if (currentTurret.NetworkId == turret.NetworkId && turretIsAttackingMe)
                    {
                        color = Color.Red;
                    }
                }
                if (color != (ColorBGRA)Color.HotPink)
                {
                    new Circle(color, TrtRange, thickness).Draw(turret.Position);
                }
            }
        }

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoad;
        }

        private static void OnDrawingDraw(EventArgs args)
        {
            DrawExperience();
            DrawTurret();
            DrawAttack();
            DrawSkills();
        }

        private static void DrawSkills()
        {
            var thickness = rMenu["cT"].Cast<Slider>().CurrentValue;
            foreach (
                var spell in
                    Spells.Where(
                        spell =>
                        ObjectManager.Player.Spellbook.GetSpell(spell).IsReady
                        && sMenu["s." + spell.ToString()[spell.ToString().Length - 1].ToString().ToLower()]
                               .Cast<CheckBox>().CurrentValue))
            {
                new Circle(Color.Aquamarine, ObjectManager.Player.Spellbook.GetSpell(spell).SData.CastRange, thickness)
                    .Draw(ObjectManager.Player.Position);
            }
        }

        private static void OnLoad(EventArgs args)
        {
            rMenu = MainMenu.AddMenu("RangeBuddy", "rMenu");
            rMenu.Add("cT", new Slider("Daire Kalınlığı", 2, 1, 10));

            eMenu = rMenu.AddSubMenu("Deneyim(Tecrübe)");
            eMenu.Add("e.s", new CheckBox("Kendini göster", false));
            eMenu.Add("e.a", new CheckBox("Dostları göster", false));
            eMenu.Add("e.e", new CheckBox("Düşmanları göster", false));

            aMenu = rMenu.AddSubMenu("Otomatik Saldırı");
            aMenu.Add("a.s", new CheckBox("Kendini göster", false));
            aMenu.Add("a.a", new CheckBox("Dostları göster", false));
            aMenu.Add("a.e", new CheckBox("Düşmanları göster", false));

            tMenu = rMenu.AddSubMenu("Kule");
            tMenu.Add("t.a", new CheckBox("Dostları göster", false));
            tMenu.Add("t.e", new CheckBox("Düşmanları göster", false));

            sMenu = rMenu.AddSubMenu("Büyüler");
            sMenu.Add("s.q", new CheckBox("Göster Q Menzili", false));
            sMenu.Add("s.w", new CheckBox("Göster W Menzili", false));
            sMenu.Add("s.e", new CheckBox("Göster E Menzili", false));
            sMenu.Add("s.r", new CheckBox("Göster R Menzili", false));

            //CacheTurrets();
            Game.OnTick += OnTick;
            Obj_AI_Base.OnBasicAttack += OnTurretAttack;
            Drawing.OnDraw += OnDrawingDraw;
        }

        private static void OnTick(EventArgs args)
        {
            if (currentTurret.Distance(ObjectManager.Player) <= TrtRange
                || currentTurret.IsAlly && !currentTurret.IsDead)
            {
                return;
            }
            turretIsAttackingMe = false;
        }

        private static void OnTurretAttack(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!(sender.IsStructure()) || !args.Target.IsMe)
            {
                return;
            }
            turretIsAttackingMe = true;
            currentTurret = sender;
        }
    }
}