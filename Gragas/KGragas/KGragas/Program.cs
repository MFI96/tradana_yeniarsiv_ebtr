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
using SharpDX;

namespace KGragas
{
    internal class Program
    {
        public const string ChampionName = "Gragas";
        public static Menu Menu, ModesMenu1, ModesMenu2, DrawMenu, Misc;
        public static AIHeroClient PlayerInstance
        {
            get { return Player.Instance; }
        }
        private static float HealthPercent()
        {
            return (PlayerInstance.Health / PlayerInstance.MaxHealth) * 100;
        }

        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }



        public static Spell.Skillshot Q;
        public static Spell.Active W;
        public static Spell.Skillshot E;
        public static Spell.Skillshot R;
        public static Spell.Targeted Ignite;
        public static bool CastedQ;
        public static Vector3 insecpos, eqpos, movingawaypos;
        public static Vector3 teste;



        static void Main(string[] args)
        {

            Loading.OnLoadingComplete += Game_OnStart;
            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Game_OnDraw;
            GameObject.OnCreate += Game_ObjectCreate;
            //GameObject.OnDelete += Game_OnDelete;
            //Orbwalker.OnPostAttack += Reset;
            Game.OnTick += Game_OnTick;
            Interrupter.OnInterruptableSpell += KInterrupter;
            Gapcloser.OnGapcloser += KGapCloser;
        }



        static void Game_OnStart(EventArgs args)
        {

            try
            {
                if (ChampionName != PlayerInstance.BaseSkinName)
                {
                    return;
                }

                Bootstrap.Init(null);
                Chat.Print("KGragas Addon'u Basariyla Yuklendi", Color.Green);




                Q = new Spell.Skillshot(SpellSlot.Q, 775, SkillShotType.Circular, 1, 1000, 110);
                Q.AllowedCollisionCount = int.MaxValue;
                W = new Spell.Active(SpellSlot.W);
                E = new Spell.Skillshot(SpellSlot.E, 675, SkillShotType.Linear, 0, 1000, 50);
                R = new Spell.Skillshot(SpellSlot.R, 1100, SkillShotType.Circular, 1, 1000, 700);
                R.AllowedCollisionCount = int.MaxValue;




                Menu = MainMenu.AddMenu("KGragas", "gragas");
                Menu.AddSeparator();
                Menu.AddLabel("Criado por Bruno105");
                Menu.AddLabel("Çeviri Tradana-Güncellemede bildirin");


                //------------//
                //-Mode Menu-//
                //-----------//

                var Enemies = EntityManager.Heroes.Enemies.Where(a => !a.IsMe).OrderBy(a => a.BaseSkinName);
                ModesMenu1 = Menu.AddSubMenu("Combo/Dürtme/KS", "Modes1Gragas");
                ModesMenu1.AddSeparator();
                ModesMenu1.AddLabel("Kombo Ayarları");
                ModesMenu1.Add("ComboQ", new CheckBox("Komboda Q Kullan", true));
                ModesMenu1.Add("ComboW", new CheckBox("Komboda W Kullan", true));
                ModesMenu1.Add("ComboE", new CheckBox("Komboda E Kullan", true));
                ModesMenu1.Add("ComboR", new CheckBox("Komboda R Kullan", false));
                ModesMenu1.AddLabel("R Sadece:");
                foreach (var a in Enemies)
                {
                    ModesMenu1.Add("Ult_" + a.BaseSkinName, new CheckBox(a.BaseSkinName));
                }
                // ModesMenu1.Add("MinR", new Slider("Use R if min Champs on R range:", 2, 1, 5));
                ModesMenu1.AddSeparator();
                ModesMenu1.AddLabel("Dürtme Ayarları");
                ModesMenu1.Add("ManaH", new Slider("Manam Şundan azsa büyü kullanma <=", 40));
                ModesMenu1.Add("HarassQ", new CheckBox("Dürtmede Q Kullan", true));
                ModesMenu1.Add("HarassW", new CheckBox("Dürtmede W Kullan", true));
                ModesMenu1.Add("HarassE", new CheckBox("Dürtmede E Kullan", true));
                ModesMenu1.AddSeparator();
                ModesMenu1.AddLabel("Kill Çalma Ayarları");
                ModesMenu1.Add("KQ", new CheckBox("Kill Çalmak için Q Kullan", true));
                ModesMenu1.Add("KE", new CheckBox("Kill Çalmak için E Kullan", true));
                ModesMenu1.Add("KR", new CheckBox("Kill Çalmak için R Kullan", true));

                ModesMenu2 = Menu.AddSubMenu("Lane/Orman/SonV", "Modes2Gragas");
                ModesMenu2.AddLabel("Sonvuruş Ayarları");
                ModesMenu2.Add("ManaL", new Slider("Manam Şundan azsa büyü kullanma <= ", 40));
                ModesMenu2.Add("LastQ", new CheckBox("Sonvuruş için Q Kullan", true));
                ModesMenu2.Add("LastW", new CheckBox("Sonvuruş için W Kullan", true));
                ModesMenu2.Add("LastE", new CheckBox("Sonvuruş için E Kullan", true));
                ModesMenu2.AddLabel("LaneTemizleme Ayarları");
                ModesMenu2.Add("ManaF", new Slider("Manam Şundan azsa büyü kullanma <=", 40));
                ModesMenu2.Add("FarmQ", new CheckBox("LaneTemizlemede Q Kullan", true));
                ModesMenu2.Add("FarmW", new CheckBox("LaneTemizlemede W Kullan", true));
                ModesMenu2.Add("FarmE", new CheckBox("LaneTemizlemede E Kullan", true));
                ModesMenu2.Add("MinionQ", new Slider("Q Kullanmak için gereken minyon sayısı :", 3, 1, 5));
                ModesMenu2.AddLabel("Orman Temizleme Ayarları");
                ModesMenu2.Add("ManaJ", new Slider("Manam Şundan azsa büyü kullanma <=", 40));
                ModesMenu2.Add("JungQ", new CheckBox("Ormanda Q Kullan", true));
                ModesMenu2.Add("JungW", new CheckBox("Ormanda W Kullan", true));
                ModesMenu2.Add("JungE", new CheckBox("Ormanda E Kullan", true));



                //------------//
                //-Draw Menu-//
                //----------//
                DrawMenu = Menu.AddSubMenu("Gösterge", "DrawKassadin");
                DrawMenu.Add("drawAA", new CheckBox("AA menzilini göster", true));
                DrawMenu.Add("drawQ", new CheckBox(" Q menzilini göster", true));
                DrawMenu.Add("drawE", new CheckBox(" E menzilini göster", true));
                DrawMenu.Add("drawR", new CheckBox(" R menzilini göster", true));
                //------------//
                //-Misc Menu-//
                //----------//
                Misc = Menu.AddSubMenu("EkMenü", "Misc");
                Misc.Add("useEGapCloser", new CheckBox("GapCloser E Kullan ", true));
                Misc.Add("useRGapCloser", new CheckBox("GapCloser R Kullan ", true));
                Misc.Add("useEInterrupter", new CheckBox("Interrupt E Kullan ", true));
                Misc.Add("useRInterrupter", new CheckBox("Interrupt R Kullan ", true));
                Misc.Add("Key", new KeyBind("İnsec Tuşu", false,KeyBind.BindTypes.HoldActive, (uint) 'A'));

            }

            catch (Exception e)
            {
                Chat.Print("KGragas: Exception occured while Initializing Addon. Error: " + e.Message);

            }

        }
        static void Game_OnDraw(EventArgs args)
        {



            if (DrawMenu["drawAA"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = Color.White, Radius = _Player.GetAutoAttackRange(), BorderWidth = 2f }.Draw(_Player.Position);
            }
            if (DrawMenu["drawQ"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = Color.Aqua, Radius = 775, BorderWidth = 2f }.Draw(_Player.Position);
            }
            if (DrawMenu["drawE"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = Color.Red, Radius = 675, BorderWidth = 2f }.Draw(_Player.Position);
            }
            if (DrawMenu["drawR"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = Color.Blue, Radius = 1100, BorderWidth = 2f }.Draw(_Player.Position);
            }

        }

        static void Game_OnUpdate(EventArgs args)
        {
            if (Misc["Key"].Cast<KeyBind>().CurrentValue)
            {
                ModesManager.Insec();


            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                ModesManager.Combo();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                ModesManager.Harass();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {

                ModesManager.LaneClear();

            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {

                ModesManager.JungleClear();
            }



            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                ModesManager.LastHit();

            }








        }

        static void Game_OnTick(EventArgs args)
        {

            ModesManager.KillSteal();

        }



        private static void Game_ObjectCreate(GameObject sender, EventArgs args)
        {
            if (sender.Name == ("Gragas_Base_Q_Ally.Troy"))
            {
                if (Player.Instance.Spellbook.GetSpell(SpellSlot.Q).ToggleState == 2 || Player.Instance.Spellbook.GetSpell(SpellSlot.Q).ToggleState == 1)
                {
                    Program.Q.Cast(Player.Instance);
                    CastedQ = false;
                }
                else
                {
                    CastedQ = false;
                }


            }


        }
        static void KInterrupter(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {

            if (args.DangerLevel == DangerLevel.High && sender.IsEnemy && sender is AIHeroClient && sender.Distance(_Player) < E.Range && E.IsReady() && Misc["useEInterrupter"].Cast<CheckBox>().CurrentValue)
            {
                E.Cast(sender);
            }
            if (args.DangerLevel == DangerLevel.High && sender.IsEnemy && sender is AIHeroClient && sender.Distance(_Player) < R.Range && R.IsReady() && Misc["useRInterrupter"].Cast<CheckBox>().CurrentValue)
            {
                R.Cast(sender);
            }

        }
        static void KGapCloser(Obj_AI_Base sender, Gapcloser.GapcloserEventArgs args)
        {


            if (sender.IsEnemy && sender is AIHeroClient && sender.Distance(_Player) < E.Range && E.IsReady() && Misc["useEGapCloser"].Cast<CheckBox>().CurrentValue)
            {
                E.Cast(sender);
            }
            if (sender.IsEnemy && sender is AIHeroClient && sender.Distance(_Player) < R.Range && R.IsReady() && Misc["useRGapCloser"].Cast<CheckBox>().CurrentValue)
            {
                R.Cast(sender);

            }
        }



        }
    }

