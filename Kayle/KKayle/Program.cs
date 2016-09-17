using System;
using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using Color = System.Drawing.Color;

namespace KKayle
{
    internal class Program
    {
        public const string ChampionName = "Kayle";
       
        public static Menu Menu, DrawMenu, ComboMenu, HarassMenu, FarmMenu, HealMenu, UltMenu;

        public static Spell.Targeted Q;
        public static Spell.Targeted W;
        public static Spell.Active E;
        public static Spell.Targeted R;
        private static Spell.Targeted Ignite;


        public static AIHeroClient PlayerInstance
        {
            get { return Player.Instance; }
        }
        public static float HealthPercent()
        {
            return (PlayerInstance.Health / PlayerInstance.MaxHealth) * 100;
        }


        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }
        private static bool Spell1(string s)
        {
            return Player.Spells.FirstOrDefault(o => o.SData.Name.Contains(s)) != null;
        }

        public static bool OnDamage = false;


        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Game_OnStart;
            Drawing.OnDraw += Game_OnDraw;
            Game.OnUpdate += Game_OnUpdate;
            Game.OnTick += Game_OnTick;
            Obj_AI_Base.OnProcessSpellCast += DamageC;

        }


        //-----------------//
        // Start Game-----//
        // Game On Start-//
        static void Game_OnStart(EventArgs args)
        {
            
            try
            {
                if (ChampionName != PlayerInstance.BaseSkinName)
                {
                    return;
                }

                Bootstrap.Init(null);

                Chat.Print("KKayle Addon Loading Success");
                Q = new Spell.Targeted(SpellSlot.Q, 650);
                    Q.CastDelay = 5;
                W = new Spell.Targeted(SpellSlot.W, 900);
                E = new Spell.Active(SpellSlot.E, 650);
                R = new Spell.Targeted(SpellSlot.R, 900);
                if (Spell1("ignite"))   
                {
                    Ignite = new Spell.Targeted(ObjectManager.Player.GetSpellSlotFromName("summonerdot"), 600);
                }

                Menu = MainMenu.AddMenu("KKayle", "kayle");
                Menu.AddSeparator();
                Menu.AddLabel("Criado por Bruno105");
                // Combo Menu
                ComboMenu = Menu.AddSubMenu("Combo", "ComboKayle");
                ComboMenu.Add("ComboW", new CheckBox("W Kullan", true));
                ComboMenu.Add("useIgnite", new CheckBox("Tutuştur Kullan", false));

                // Harass Menu
                HarassMenu = Menu.AddSubMenu("Harass", "HarassKayle");
                HarassMenu.Add("HarassQ", new CheckBox("Q Kullan", true));
                HarassMenu.Add("HarassW", new CheckBox("W Kullan", false));
                HarassMenu.Add("HarassE", new CheckBox("E Kullan", true));
                HarassMenu.Add("ManaH", new Slider("Manam şundan azsa kullanma  <=", 30));

                //Farm Menu
                FarmMenu = Menu.AddSubMenu("Farm", "FarmKayle");
                FarmMenu.Add("ManaF", new Slider("Manam şundan azsa büyü kullanma  <=", 40));
                FarmMenu.Add("FarmQ", new CheckBox("Q Kullan", true));
                FarmMenu.Add("FarmE", new CheckBox("E KUllan", true));
                FarmMenu.Add("MinionE", new Slider("E için gereken minyon sayısı >=", 3, 1, 5));
                FarmMenu.AddSeparator();
                FarmMenu.AddLabel("Last Hit");
                FarmMenu.Add("LastQ", new CheckBox("Q ile son vuruş", true));
               // FarmMenu.Add("LastE", new CheckBox("Use E to Last Hit", true));
               

                // Heal Menu
                var allies = EntityManager.Heroes.Allies.Where(a => !a.IsMe).OrderBy(a => a.BaseSkinName);
                HealMenu = Menu.AddSubMenu("Heal", "HealKayle");
                HealMenu.Add("autoW", new CheckBox("W otomatik kullan", true));
                HealMenu.Add("HealSelf", new Slider("W kendine kullan % HP", 50));
                HealMenu.Add("HealAlly", new Slider("Dostlara Kullan % HP", 50));
                foreach (var a in allies)
                {
                    HealMenu.Add("autoHeal_" + a.BaseSkinName, new CheckBox("Dostlara can  " + a.BaseSkinName));
                }
                

                //--------------//
                //---Ultmate---//
                //------------//

                var ally = EntityManager.Heroes.Allies.Where(a => !a.IsMe).OrderBy(a => a.BaseSkinName);
                UltMenu = Menu.AddSubMenu("Ultimate", "UltKayle");
                UltMenu.Add("autoR", new CheckBox("Otomatik Ulti ", true));
                UltMenu.Add("UltSelf", new Slider("Ulti kendine kullan % HP", 20));
                UltMenu.Add("UltAlly", new Slider("Dostlara kullan  % HP", 20));
                foreach (var a in ally)
                {
                    UltMenu.Add("autoUlt_" + a.BaseSkinName, new CheckBox("Ulti Kullan " + a.BaseSkinName));
                }


                //------------//
                //-Draw Menu-//
                //----------//
                DrawMenu = Menu.AddSubMenu("Draws", "DrawKayle");
                // DrawMenu.Add("drawDisable", new CheckBox("Desabilidatar todos os Draw", false));
                DrawMenu.Add("drawAA", new CheckBox("Devredışı Göster AA", true));
                DrawMenu.Add("drawQ", new CheckBox("Devredışı Göster Q", true));
                DrawMenu.Add("drawW", new CheckBox("Devredışı Göster W", true));
                DrawMenu.Add("drawE", new CheckBox("Devredışı Göster E", true));
            }
            catch (Exception e)
            {
                Chat.Print("KKayle: Exception occured while Initializing Addon. Error: " + e.Message);
            }

            }
        
         
        // ------------//
        // Game OnDraw//
        // --------- //

        public static void Game_OnDraw(EventArgs args)
        {
      
            if (DrawMenu["drawAA"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = Color.White, Radius = _Player.GetAutoAttackRange(), BorderWidth = 2f }.Draw(_Player.Position);
            }
            if (DrawMenu["drawQ"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = Color.Aqua, Radius = 650, BorderWidth = 2f }.Draw(_Player.Position);
            }
            if (DrawMenu["drawW"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = Color.Green, Radius = 900, BorderWidth = 2f }.Draw(_Player.Position);
            }
            if (DrawMenu["drawE"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = Color.Red, Radius = _Player.GetAutoAttackRange() + 400, BorderWidth = 2f }.Draw(_Player.Position);
            }


        }

        //-----//
       // Heal //
      // -----//
       

        //-------------//
        //--Ultimate--//
        //-----------//
        
    

        // ------------//
        // Game On Update//
        // ------------//

        public static void Game_OnUpdate(EventArgs args)
        {

            

            //-------------//
            //----Modes----//
            //-------------//
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
               ModeManager.Combo();
            }

            if ((Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))) 
            {
               ModeManager.Harass();

            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                ModeManager.LaneClear();

            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                ModeManager.JungleClear();

            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                ModeManager.LastHit();

            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                ModeManager.Flee();

            }


            }


        public static void Game_OnTick(EventArgs args)
        {
            ModeManager.AutoHeal();
            ModeManager.AutoUlt();


        }
       

        public static void DamageC(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        
        {
           /* if (_Player.IsRecalling()) return;
            var target = args.Target as Obj_AI_Base;
            if (!target.IsAlly || sender.IsAlly || target == null)
            {

                return;
            }
            if (sender.IsEnemy && target.IsAlly && !target.IsMinion)
            {
                OnDamage = true;

            }

            if (target.IsUnderTurret())
            {
                OnDamage = true;
            }*/

        }
        static void KInterrupter(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            if (args.DangerLevel == DangerLevel.High && sender.IsEnemy && sender is AIHeroClient && sender.Distance(_Player) < R.Range && R.IsReady())
            {
                R.Cast(Player.Instance);
            }

        }

        }

    }

