using System;
using System.Drawing;
using AlchemistSinged;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;

namespace AutoCannon
{
    // Created by Counter
    internal class Program
    {
        // Grab Player Attributes
        public static AIHeroClient Champion { get { return Player.Instance; } }

        // Skills
        public static Spell.Skillshot Throw;

        public static void Main()
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        public static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Game.MapId != GameMapId.HowlingAbyss) return;

            // Initialize classes
            MenuManager.Initialize();

            // Setting variable
            SpellDataInst sumspell = null;
            if (Player.GetSpell(SpellSlot.Summoner1).Name == "summonersnowball")
                sumspell = Player.GetSpell(SpellSlot.Summoner1);
            else if (Player.GetSpell(SpellSlot.Summoner2).Name == "summonersnowball")
                sumspell = Player.GetSpell(SpellSlot.Summoner2);
            if (Player.GetSpell(SpellSlot.Summoner1).Name == "summonerporothrow")
                sumspell = Player.GetSpell(SpellSlot.Summoner1);
            else if (Player.GetSpell(SpellSlot.Summoner2).Name == "summonerporothrow")
                sumspell = Player.GetSpell(SpellSlot.Summoner2);

            if (sumspell != null)
            {
                switch (sumspell.Name)
                {
                    case "summonersnowball":
                        MenuManager.SettingsMenu.Add("markrange", new Slider("SS: KarTopu Menzili - 0 ise kapat", 1600, 0, 1600));
                        break;
                    case "summonerporothrow":
                        MenuManager.SettingsMenu.Add("pororange", new Slider("SS: Poro Kralı Menzili - 0 ise kapat", 2500, 0, 2500));
                        break;
                }

                var range = sumspell.Name == "summonersnowball"
                    ? MenuManager.SettingsMenu["markrange"].Cast<Slider>().CurrentValue
                    : MenuManager.SettingsMenu["pororange"].Cast<Slider>().CurrentValue;
                Throw = new Spell.Skillshot(sumspell.Slot, (uint) range, SkillShotType.Linear)
                {
                    MinimumHitChance = HitChance.High,
                    AllowedCollisionCount = 0
                };

                Game.OnTick += Game_OnTick;
                Drawing.OnDraw += Drawing_OnDraw;
            }
        }

        public static void Game_OnTick(EventArgs args)
        {
            // Mark Calculations
            if (!Throw.IsOnCooldown && Throw.Name != "snowballfollowupcast" && Throw.Name != "porothrowfollowupcast")
            {
                // Calculate Damage
                var damage = Throw.Name == "summonersnowball" ? 10 + 5*Champion.Level : 20 + 10*Champion.Level;

                // Cast
                var kstarget = TargetManager.GetChampionTarget(Throw.Range, DamageType.True, false, true, damage);
                if (kstarget != null)
                    Throw.Cast(kstarget);
                else
                {
                    var target = TargetManager.GetChampionTarget(Throw.Range, DamageType.True, false, true);
                    if (target != null)
                        Throw.Cast(target);
                }
            }
        }

        public static void Drawing_OnDraw(EventArgs args)
        {
            Drawing.DrawCircle(Champion.Position, Throw.Range, Color.CadetBlue);
        }
    }
}