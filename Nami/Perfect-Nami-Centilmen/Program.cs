using System;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using Color = System.Drawing.Color;
using Version = System.Version;
using System.Net;
using System.Text.RegularExpressions;

namespace PerfectNami
{
    static class Program
    {
        public static AIHeroClient _Player { get { return ObjectManager.Player; } }

        public static Item Bilgewater, Randuin, QSS, Glory, FOTMountain, Mikael, IronSolari;
        public static Menu menu,ComboMenu,HarassMenu,AutoMenu,DrawMenu;
        public static AIHeroClient Target = null;
        public static List<string> DodgeSpells = new List<string>() { "LuxMaliceCannon", "LuxMaliceCannonMis", "EzrealtrueShotBarrage", "KatarinaR", "YasuoDashWrapper", "ViR", "NamiR", "ThreshQ", "xerathrmissilewrapper", "yasuoq3w", "UFSlash" };
        public static readonly Spell.Skillshot Q = new Spell.Skillshot(SpellSlot.Q, 875, EloBuddy.SDK.Enumerations.SkillShotType.Circular, 1, int.MaxValue, 150)
        {
            MinimumHitChance = EloBuddy.SDK.Enumerations.HitChance.Medium
        };
        public static readonly Spell.Targeted W = new Spell.Targeted(SpellSlot.W, 725);
        public static readonly Spell.Targeted E = new Spell.Targeted(SpellSlot.E, 800);
        public static readonly Spell.Skillshot R = new Spell.Skillshot(SpellSlot.R, 2750, EloBuddy.SDK.Enumerations.SkillShotType.Linear, 250, 500, 160);

        static void Main(string[] args) { Loading.OnLoadingComplete += OnLoadingComplete; }

        static void OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.ChampionName != "Nami")
            {
            return;
            }

            Bilgewater = new Item(3144, 550);
            Randuin = new Item(3143, 500);
            Glory = new Item(3800);
            QSS = new Item(3140);
            FOTMountain = new Item(3401);
            Mikael = new Item(3222, 750);
            IronSolari = new Item(3190, 600);

            menu = MainMenu.AddMenu("Perfect Nami", "PerfectNami");
            ComboMenu = menu.AddSubMenu("Kombo Ayarları", "ComboMenu");
            ComboMenu.Add("ComboUseQ", new CheckBox("Kullan Q"));
            ComboMenu.Add("ComboUseW", new CheckBox("Kullan W"));
            ComboMenu.Add("ComboUseE", new CheckBox("Kullan E"));
            ComboMenu.Add("ComboUseR", new CheckBox("Kullan R"));

            HarassMenu = menu.AddSubMenu("Dürtme Ayarları", "HarassMenu");
            HarassMenu.Add("HarassUseQ", new CheckBox("Kullan Q"));
            HarassMenu.Add("HarassUseW", new CheckBox("Kullan W"));
            HarassMenu.Add("HarassUseE", new CheckBox("Kullan E"));

            AutoMenu = menu.AddSubMenu("Otomatik Ayarları", "AutoMenu");
            AutoMenu.Add("AutoW", new CheckBox("W Kullan can için)"));
            AutoMenu.Add("AutoWV", new Slider("Dostların Canı < % ", 50, 1, 100));
            AutoMenu.Add("ManaToW", new Slider("Manam <  %", 30, 1, 100));
            AutoMenu.Add("AutoR", new CheckBox("Otomatik R"));
            AutoMenu.Add("AutoRCount", new Slider("otomatik R için say >= ", 3, 1, 5));
            AutoMenu.Add("useItems", new CheckBox("İtemleri Kullan"));
            AutoMenu.AddLabel("Mikail, Dağın Sureti, Glory, Randuin, Solarinin Broşu");
            AutoMenu.Add("AutoQInterrupt", new CheckBox("Otomatik Q Interrupt"));
            AutoMenu.AddLabel("e.g Katarina R");
            AutoMenu.Add("gapcloser", new CheckBox("Otomatik Q Gapcloser"));
            AutoMenu.Add("interrupter", new CheckBox("Otomatik Q Interrupter"));

            DrawMenu = menu.AddSubMenu("Göstergeler", "DrawMenu");
            DrawMenu.Add("DrawAA", new CheckBox("Göster AA"));
            DrawMenu.Add("DrawQ", new CheckBox("Göster Q"));
            DrawMenu.Add("DrawW", new CheckBox("Göster W"));
            DrawMenu.Add("DrawE", new CheckBox("Göster E"));
            DrawMenu.Add("DrawR", new CheckBox("Göster R"));

            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += Drawing_OnDraw;
            AIHeroClient.OnProcessSpellCast += AIHeroClient_OnProcessSpellCast;
            Gapcloser.OnGapcloser += Gapcloser_OnGapCloser;
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
        }


        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender,
            Interrupter.InterruptableSpellEventArgs e)
        {
            if (AutoMenu["interrupter"].Cast<CheckBox>().CurrentValue && sender.IsEnemy &&
                e.DangerLevel == EloBuddy.SDK.Enumerations.DangerLevel.High && sender.IsValidTarget(900))
            {
                Q.Cast(sender);
            }
        }

        public static void Gapcloser_OnGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (AutoMenu["gapcloser"].Cast<CheckBox>().CurrentValue && sender.IsEnemy &&
                e.End.Distance(_Player) < 200)
            {
                Q.Cast(e.End);
            }
        }

        static void AIHeroClient_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (DodgeSpells.Any(el => el == args.SData.Name))
            {
                if (Q.IsReady() && Q.IsInRange(sender))
                {
                    Q.Cast(sender);
                }
                   
            }
        }
        
    
        //----------------------------------------------Drawing_OnDraw----------------------------------------

        static void Drawing_OnDraw(EventArgs args)
        {
            if (!_Player.IsDead)
            {
                if (DrawMenu["DrawQ"].Cast<CheckBox>().CurrentValue)
                {
                    Drawing.DrawCircle(_Player.Position, Q.Range, Q.IsReady() ? Color.Gold : Color.Red);
                }
                if (DrawMenu["DrawW"].Cast<CheckBox>().CurrentValue)
                { 
                        Drawing.DrawCircle(_Player.Position, W.Range, W.IsReady() ? Color.Gold : Color.Red);
                }
                if (DrawMenu["DrawE"].Cast<CheckBox>().CurrentValue)
                { 
                        Drawing.DrawCircle(_Player.Position, E.Range, E.IsReady() ? Color.Gold : Color.Red);
                }
                if (DrawMenu["DrawR"].Cast<CheckBox>().CurrentValue)
                { 
                        Drawing.DrawCircle(_Player.Position, R.Range, R.IsReady() ? Color.Gold : Color.Red);
                }
            }
            return;
        }

        //-------------------------------------------Game_OnTick----------------------------------------------

        static void Game_OnTick(EventArgs args)
        {
            if (_Player.IsDead) { return; }
            var useItems = AutoMenu["useItems"].Cast<CheckBox>().CurrentValue;
            if (_Player.CountEnemiesInRange(1000) > 0)
            {
                foreach (AIHeroClient enemy in EntityManager.Heroes.Enemies)
                {
                    foreach (AIHeroClient ally in EntityManager.Heroes.Allies)
                    {
                        if (ally.IsFacing(enemy) && ally.HealthPercent <= 30 && _Player.Distance(ally) <= 750)
                        {
                            if (useItems && FOTMountain.IsReady())
                            {
                                FOTMountain.Cast(ally);
                            }


                            if ((useItems && ally.HasBuffOfType(BuffType.Charm) || ally.HasBuffOfType(BuffType.Fear) || ally.HasBuffOfType(BuffType.Poison) || ally.HasBuffOfType(BuffType.Polymorph) || ally.HasBuffOfType(BuffType.Silence) || ally.HasBuffOfType(BuffType.Sleep) || ally.HasBuffOfType(BuffType.Slow) || ally.HasBuffOfType(BuffType.Snare) || ally.HasBuffOfType(BuffType.Stun) || ally.HasBuffOfType(BuffType.Taunt)) && Mikael.IsReady())
                            {
                                Mikael.Cast(ally);
                            }
                            
                        }

                        if(ally.HasBuffOfType(BuffType.Slow))
                        {
                            E.Cast(ally);
                        }

                        if (ally.IsFacing(enemy) && ally.HealthPercent <= 30 && _Player.Distance(ally) <= 600)
                        {
                            if (useItems && IronSolari.IsReady())
                            {
                                IronSolari.Cast();
                            }
                        }
                    }
                }
            }

            if (!_Player.HasBuff("recall"))
            {
                foreach (AIHeroClient allys in EntityManager.Heroes.Allies)
                {
                    if (W.IsReady() && allys != _Player && EntityManager.Heroes.Allies.Where(ally => ally.HealthPercent <= AutoMenu["AutoWV"].Cast<Slider>().CurrentValue && W.IsInRange(ally)).Any() && _Player.ManaPercent >= AutoMenu["ManaToW"].Cast<Slider>().CurrentValue)
                    {                       
                            W.Cast(allys);
                    }
                }
            }


            

                    
                    
                   
                    //-------------------------------------------------Harass-------------------------------------------

                    if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
                    {
                        Target = TargetSelector.GetTarget(875, DamageType.Magical);
                var HarassUseQ = HarassMenu["HarassUseQ"].Cast<CheckBox>().CurrentValue;
                var HarassUseW = HarassMenu["HarassUseW"].Cast<CheckBox>().CurrentValue;
                var HarassUseE = HarassMenu["HarassUseE"].Cast<CheckBox>().CurrentValue;
                if (HarassUseQ && Q.IsReady() && Target.IsValidTarget(Q.Range - 35))
                        {
                            Q.Cast(Target);
                        }
                            

                        if (HarassUseW && W.IsReady() && Target.IsValidTarget(W.Range))
                        {
                            W.Cast(Target);
                        }
                        foreach (AIHeroClient enemy in EntityManager.Heroes.Enemies)
                        {
                            foreach (AIHeroClient ally in EntityManager.Heroes.Allies)
                            {
                                if (HarassUseE && E.IsReady() && _Player.Distance(enemy) <= 1000 && _Player.Distance(ally) <= 725)
                                {
                                    E.Cast(ally);
                                }
                            }
                        }
                              
                    }

                    //---------------------------------------------------Combo--------------------------------------------

                    if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                    {
                var ComboUseQ = ComboMenu["ComboUseQ"].Cast<CheckBox>().CurrentValue;
                var ComboUseW = ComboMenu["ComboUseW"].Cast<CheckBox>().CurrentValue;
                var ComboUseE = ComboMenu["ComboUseE"].Cast<CheckBox>().CurrentValue;
                var ComboUseR = ComboMenu["ComboUseR"].Cast<CheckBox>().CurrentValue;
                Target = TargetSelector.GetTarget(875, DamageType.Magical);
                        foreach (AIHeroClient enemy in EntityManager.Heroes.Enemies)
                        {
                            foreach (AIHeroClient ally in EntityManager.Heroes.Allies)
                            {
                                if (ComboUseE && E.IsReady() && ally != _Player && _Player.Distance(enemy) <= 1000 && _Player.Distance(ally) <= 725)
                                {
                                    E.Cast(ally);
                                }
                        var AutoRCount = AutoMenu["AutoRCount"].Cast<Slider>().CurrentValue;
                        if (ComboUseR && enemy.IsFacing(ally) && _Player.CountEnemiesInRange(2000) > AutoRCount)
                                {
                                    R.Cast(enemy);
                                }
                            }

                        }
                        if (useItems && QSS.IsReady() && (_Player.HasBuffOfType(BuffType.Charm) || _Player.HasBuffOfType(BuffType.Blind) || _Player.HasBuffOfType(BuffType.Fear) || _Player.HasBuffOfType(BuffType.Polymorph) || _Player.HasBuffOfType(BuffType.Silence) || _Player.HasBuffOfType(BuffType.Sleep) || _Player.HasBuffOfType(BuffType.Snare) || _Player.HasBuffOfType(BuffType.Stun) || _Player.HasBuffOfType(BuffType.Suppression) || _Player.HasBuffOfType(BuffType.Taunt)))
                        {
                            QSS.Cast();
                        }                      
                        if (ComboUseQ && Q.IsReady() && Target.IsValidTarget(Q.Range))
                            Q.Cast(Target);

                        if (ComboUseW && W.IsReady() && Target.IsValidTarget(W.Range))
                            W.Cast(Target);

                        if (useItems && Target.IsValidTarget(Bilgewater.Range) && Bilgewater.IsReady())
                            Bilgewater.Cast(Target);

                        if (useItems && Target.IsValidTarget(Randuin.Range) && Randuin.IsReady())
                            Randuin.Cast();

                    }


                }
            

        
    }
}
