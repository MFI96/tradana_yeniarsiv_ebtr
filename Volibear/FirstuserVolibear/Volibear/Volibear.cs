using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Events;

namespace Volibear
{
    internal class Volibear
    {

        public static Spell.Targeted W;
        public static Spell.Active Q, E, R;
        public static Menu VoliMenu, ComboMenu, HarassMenu, LaneMenu, MiscMenu, JungleMenu, KSMenu, DrawMenu, SmiteMenu;
      
        public static AIHeroClient Player
        {
            get { return ObjectManager.Player; }
        }

        public static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Player.Hero != Champion.Volibear) return;
            Chat.Print("Volibear Yukledi");
            VoliMenu = MainMenu.AddMenu("Volibear", "Volibear");
            VoliMenu.AddGroupLabel("Çılgın Voli!");
            ComboMenu = VoliMenu.AddSubMenu("Combo");
            ComboMenu.AddGroupLabel("Combo Ayarları");
            ComboMenu.Add("UseQ", new CheckBox("Kullan Q"));
            ComboMenu.Add("UseW", new CheckBox("Kullan W"));
            ComboMenu.Add("UseE", new CheckBox("Kullan E"));
            ComboMenu.Add("UseR", new CheckBox("Kullan R"));
            ComboMenu.Add("UseItems", new CheckBox("Kullan İtemler"));
            ComboMenu.Add("Wcount", new Slider("W için düşmanın canı", 100, 0, 100));
            ComboMenu.Add("Rcount", new Slider("Ulti için gereken düşman sayısı", 2, 1, 5));


            HarassMenu = VoliMenu.AddSubMenu("Harass");
            HarassMenu.AddGroupLabel("Dürtme Ayarları");
            HarassMenu.Add("Ehrs", new CheckBox("E Kullan"));


            LaneMenu = VoliMenu.AddSubMenu("Farm");
            LaneMenu.AddGroupLabel("Lanetemizleme Ayarları");
            LaneMenu.Add("laneQ", new CheckBox("Kullan Q"));
            LaneMenu.Add("laneW", new CheckBox("Kullan W"));
            LaneMenu.Add("laneE", new CheckBox("Kullan E"));
            LaneMenu.Add("LCM", new Slider("Mana %", 30, 0, 100));


            JungleMenu = VoliMenu.AddSubMenu("Jungle");
            JungleMenu.AddGroupLabel("Ormantemizleme Ayarları");
            JungleMenu.Add("JungleQ", new CheckBox("Kullan Q"));
            JungleMenu.Add("JungleW", new CheckBox("Kullan W"));
            JungleMenu.Add("JungleE", new CheckBox("Kullan E"));
            JungleMenu.Add("JCM", new Slider("Mana %", 30, 0, 100));



            MiscMenu = VoliMenu.AddSubMenu("Misc");
            MiscMenu.AddGroupLabel("Ek Ayarlar");
            MiscMenu.Add("gapcloserW", new CheckBox("Anti-GapCloser W"));

            KSMenu = VoliMenu.AddSubMenu("ks");
            KSMenu.AddGroupLabel("Kill Çalma Ayarları");
            KSMenu.Add("ksW", new CheckBox("W ile çal"));
            KSMenu.Add("ksE", new CheckBox("E ile çal"));

            DrawMenu = VoliMenu.AddSubMenu("Drawings");
            DrawMenu.AddGroupLabel("Gösterge Ayarları");
            DrawMenu.Add("DrawWE", new CheckBox("W ve E göster"));
            DrawMenu.Add("smitestatus1", new CheckBox("Çarp durumunu göster"));



            Q = new Spell.Active(SpellSlot.Q, 750);
            W = new Spell.Targeted(SpellSlot.W, 395);
            E = new Spell.Active(SpellSlot.E, 415);
            R = new Spell.Active(SpellSlot.R, (uint)Player.GetAutoAttackRange());

            Game.OnUpdate += OnUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
            Orbwalker.OnPreAttack += Orbwalker_OnPreAttack;
            Gapcloser.OnGapcloser += Gapcloser_OnGapcloser;
        }


        private static void OnUpdate(EventArgs args)
        

            {
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) Combo();
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)) Harass();
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)) LaneClear();
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)) JungleClear();


            KillSteal();
            Smitez.Smitemethod();
        }




        static void Orbwalker_OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (ComboMenu["UseR"].Cast<CheckBox>().CurrentValue && R.IsReady() && args.Target.IsEnemy &&
                args.Target.IsValid &&
                Player.CountEnemiesInRange(300) >= ComboMenu["Rcount"].Cast<Slider>().CurrentValue)

            {
                R.Cast();
            }
        }


        private static void Drawing_OnDraw(EventArgs args)
        {
            if (DrawMenu["DrawWE"].Cast<CheckBox>().CurrentValue)
            {
                Drawing.DrawCircle(Player.Position, E.Range, System.Drawing.Color.CadetBlue);
            }
            var heropos = Drawing.WorldToScreen(ObjectManager.Player.Position);
            var green = System.Drawing.Color.LimeGreen;
            var red = System.Drawing.Color.IndianRed;
            if (DrawMenu["smitestatus1"].Cast<CheckBox>().CurrentValue)
            {
                Drawing.DrawText(heropos.X - 40, heropos.Y + 20, System.Drawing.Color.FloralWhite, "Smite:");
                Drawing.DrawText(heropos.X + 10, heropos.Y + 20,
                    SmiteMenu["smiteActive"].Cast<KeyBind>().CurrentValue ? System.Drawing.Color.LimeGreen : System.Drawing.Color.Red,
                    SmiteMenu["smiteActive"].Cast<KeyBind>().CurrentValue ? "On" : "Off");

            }
        }

        private static void KillSteal()
        {
            var enemyForKs = EntityManager.Heroes.Enemies.FirstOrDefault(h => W.IsReady() && WDamage(h) > h.Health);
            if (enemyForKs != null && W.IsReady() && KSMenu["ksW"].Cast<CheckBox>().CurrentValue)
            {
                W.Cast(enemyForKs);
            }

            var kSableE =
                EntityManager.Heroes.Enemies.FindAll(
                    champ =>
                        champ.IsValidTarget() &&
                        (champ.Health <= ObjectManager.Player.GetSpellDamage(champ, SpellSlot.E)) &&
                        champ.Distance(ObjectManager.Player) < E.Range);
            if (kSableE.Any())
            {
                E.Cast(kSableE.FirstOrDefault());
            }
        }

        public static double WDamage(Obj_AI_Base target)
        {
            return (new double[] { 80, 125, 170, 215, 260 }[W.Level - 1] +
                    ((Player.MaxHealth - (498.48f + (86f * (Player.Level - 1f)))) * 0.15f)) *
                   ((target.MaxHealth - target.Health) / target.MaxHealth + 1);
        }

        private static void Combo()
        {
            var target = TargetSelector.GetTarget(1490, DamageType.Physical);
            if (target == null || target.IsZombie) return;


            if (Player.Distance(target) <= Q.Range && Q.IsReady() && (ComboMenu["UseQ"].Cast<CheckBox>().CurrentValue))
            {
                Q.Cast();
            }
            if (Player.Distance(target) <= E.Range && E.IsReady() && (ComboMenu["UseE"].Cast<CheckBox>().CurrentValue))
            {
                E.Cast();
            }

            var T = TargetSelector.GetTarget(W.Range, DamageType.Physical);
            var health = target.Health;
            var maxhealth = target.MaxHealth;
            float wcount = ComboMenu["Wcount"].Cast<Slider>().CurrentValue;
            if (health < ((maxhealth * wcount) / 100))
            {
                if (ComboMenu["UseW"].Cast<CheckBox>().CurrentValue && W.IsReady())
                {
                    W.Cast(T);
                }

                if (ComboMenu["UseItems"].Cast<CheckBox>().CurrentValue)
                {
                    UseItems(target);
                }
            }
        }

        private static void Gapcloser_OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            var T = TargetSelector.GetTarget(W.Range, DamageType.Physical);
            if (Player.IsDead || !sender.IsEnemy || !sender.IsValidTarget(W.Range) || !W.IsReady() || !MiscMenu["gapcloserW"].Cast<CheckBox>().CurrentValue) return;

            W.Cast(T);
        }



        internal static void UseItems(Obj_AI_Base target)
        {
            var KhazixServerPosition = Player.ServerPosition.To2D();
            var targetServerPosition = target.ServerPosition.To2D();

            if (Item.CanUseItem(ItemId.Ravenous_Hydra_Melee_Only) && 400 > Player.Distance(target))
            {
                Item.UseItem(ItemId.Ravenous_Hydra_Melee_Only);
            }
            if (Item.CanUseItem(ItemId.Tiamat_Melee_Only) && 400 > Player.Distance(target))
            {
                Item.UseItem(ItemId.Tiamat_Melee_Only);
            }
            if (Item.CanUseItem(ItemId.Titanic_Hydra) && 400 > Player.Distance(target))
            {
                Item.UseItem(ItemId.Titanic_Hydra);
            }
            if (Item.CanUseItem(ItemId.Blade_of_the_Ruined_King) && 550 > Player.Distance(target))
            {
                Item.UseItem(ItemId.Blade_of_the_Ruined_King);
            }
            if (Item.CanUseItem(ItemId.Youmuus_Ghostblade) && Player.GetAutoAttackRange() > Player.Distance(target))
            {
                Item.UseItem(ItemId.Youmuus_Ghostblade);
            }
            if (Item.CanUseItem(ItemId.Bilgewater_Cutlass) && 550 > Player.Distance(target))
            {
                Item.UseItem(ItemId.Bilgewater_Cutlass);
            }
        }

        private static void Harass()
        {
            var target = TargetSelector.GetTarget(E.Range, DamageType.Physical);

            if (target == null)
            {
                return;
            }

            if (HarassMenu["Ehrs"].Cast<CheckBox>().CurrentValue && Player.Distance(target) <= E.Range && E.IsReady())
            {
                E.Cast();
            }
        }

        private static void JungleClear()
        {
            var Mob = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.ServerPosition, E.Range).OrderBy(x => x.MaxHealth).ToList();

            if (Player.ManaPercent > JungleMenu["JCM"].Cast<Slider>().CurrentValue)
            {
                if (JungleMenu["JungleQ"].Cast<CheckBox>().CurrentValue && Q.IsReady())
                {
                    foreach (var minion in Mob)
                    {
                        if (minion.IsValidTarget())
                        {
                            Q.Cast();
                        }
                    }
                }

                if (JungleMenu["JungleW"].Cast<CheckBox>().CurrentValue && W.IsReady())
                {
                    foreach (var minion in Mob)
                    {
                        if (minion.IsValidTarget())
                        {
                            W.Cast(minion);
                        }
                    }
                }
                if (JungleMenu["JungleE"].Cast<CheckBox>().CurrentValue && E.IsReady())
                {
                    foreach (var minion in Mob)
                    {
                        if (minion.IsValidTarget())
                        {
                            E.Cast();
                        }
                    }
                }
            }
        }

        public static bool HasSpell(string s)
        {
            return EloBuddy.Player.Spells.FirstOrDefault(o => o.SData.Name.Contains(s)) != null;
        }

        private static void LaneClear()
        {
            var allMinions = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.ServerPosition, Q.Range);

            if (Player.ManaPercent > LaneMenu["LCM"].Cast<Slider>().CurrentValue)
            {
                if (LaneMenu["LaneW"].Cast<CheckBox>().CurrentValue && Q.IsReady())
                {
                    foreach (var minion in allMinions)
                    {
                        if (minion.IsValidTarget())
                        {
                            W.Cast(minion);
                        }
                    }
                }

                if (LaneMenu["LaneE"].Cast<CheckBox>().CurrentValue && E.IsReady())
                {
                    foreach (var minion in allMinions)
                    {
                        if (minion.IsValidTarget())
                        {
                            E.Cast();
                        }
                    }
                }
            }
        }
    }
}