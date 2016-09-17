using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using GuTenTak.Tristana;
using SharpDX;
using EloBuddy.SDK.Constants;
using color = System.Drawing.Color;
using System.Collections.Generic;
using SharpDX.Direct3D9;

namespace GuTenTak.Tristana
{
    internal class Program
    {
        public const string ChampionName = "Tristana";
        public static Menu Menu, ModesMenu1, ModesMenu2, ModesMenu3, DrawMenu;
        public static int SkinBase;
        public static Item Youmuu = new Item(ItemId.Youmuus_Ghostblade);
        public static Item Botrk = new Item(ItemId.Blade_of_the_Ruined_King);
        public static Item Cutlass = new Item(ItemId.Bilgewater_Cutlass);
        public static Item Qss = new Item(ItemId.Quicksilver_Sash);
        public static Item Simitar = new Item(ItemId.Mercurial_Scimitar);
        public static Item hextech = new Item(ItemId.Hextech_Gunblade, 700);
        private static Vector3? LastHarassPos { get; set; }
        private static readonly Vector2 Offset = new Vector2(1, 0);
        private static bool HasEBuff(Obj_AI_Base target)
        {
            return target.HasBuff("TristanaECharge");
        }

        public static List<AIHeroClient> CloseEnemies(float range = 1500, Vector3 from = default(Vector3))
        {
            return EntityManager.Heroes.Enemies.Where(e => e.IsValidTarget(range, false, from)).ToList();
        }

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

        public static Spell.Active Q;
        public static Spell.Skillshot W;
        public static Spell.Targeted E;
        public static Spell.Targeted R;

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Game_OnStart;
        }


        static void Game_OnStart(EventArgs args)
        {
            Game.OnUpdate += Game_OnUpdate;
            Game.OnTick += OnTick;
            Obj_AI_Base.OnBuffGain += Common.OnBuffGain;
            GameObject.OnCreate += OnCreate;
            Gapcloser.OnGapcloser += Common.Gapcloser_OnGapCloser;
            Orbwalker.OnPreAttack += OnPreAttack;
            Drawing.OnDraw += Game_OnDraw;
            SkinBase = Player.Instance.SkinId;
            try
            {
                if (ChampionName != PlayerInstance.BaseSkinName)
                {
                    return;
                }

                Q = new Spell.Active(SpellSlot.Q, (uint)_Player.AttackRange + 50);
                W = new Spell.Skillshot(SpellSlot.W, 900, SkillShotType.Circular, (int)0.5, 1400, 250);
                E = new Spell.Targeted(SpellSlot.E, (uint)_Player.AttackRange + 50);
                R = new Spell.Targeted(SpellSlot.R, (uint)_Player.AttackRange + 50);



                Bootstrap.Init(null);
                Chat.Print("GuTenTak Addon basariyla yuklendi", Color.Green);


                Menu = MainMenu.AddMenu("GuTenTak Tristana", "Tristana");
                Menu.AddSeparator();
                Menu.AddLabel("GuTenTak Tristana Addon");

                var Enemies = EntityManager.Heroes.Enemies.Where(a => !a.IsMe).OrderBy(a => a.BaseSkinName);
                ModesMenu1 = Menu.AddSubMenu("Menu", "Modes1Tristana");
                ModesMenu1.AddSeparator();
                ModesMenu1.AddLabel("Combo Ayarları");
                ModesMenu1.Add("ComboQ", new CheckBox("Q Kullan", true));
                ModesMenu1.Add("ComboE", new CheckBox("E Kullan", true));
                ModesMenu1.Add("ComboEF", new CheckBox("Kombo modunda E ile düşmanı zorla", true));
                ModesMenu1.Add("ManualR", new KeyBind("Yarı otomatik R", false, KeyBind.BindTypes.HoldActive, 'T'));

                ModesMenu1.AddSeparator();
                ModesMenu1.AddLabel("E List");
                foreach (var Enemy in EntityManager.Heroes.Enemies)
                {
                    ModesMenu1.Add(Enemy.ChampionName, new CheckBox("E Kullan " + Enemy.ChampionName, true));
                }
                ModesMenu1.AddSeparator();
                ModesMenu1.AddLabel("Dürtme Ayarları");
                ModesMenu1.Add("HarassEF", new CheckBox("Dürtme modunda E ile zorla", true));
                ModesMenu1.Add("HarassQ", new CheckBox("Q Kullan", true));
                ModesMenu1.Add("HarassE", new CheckBox("E Kullan", true));
                ModesMenu1.Add("ManaHE", new Slider("Gereken Mana %", 60));
                ModesMenu1.AddSeparator();
                ModesMenu1.AddLabel("Kill Çalma Ayarları");
                ModesMenu1.Add("KS", new CheckBox("Kill Çalma Kullan", true));
                ModesMenu1.Add("KR", new CheckBox("R Kullan", true));
                ModesMenu1.Add("KER", new CheckBox("E + R Kullan", true));

                ModesMenu2 = Menu.AddSubMenu("Farm", "Modes2Tristana");
                ModesMenu2.AddLabel("Lane Clear Ayarları");
                ModesMenu2.AddSeparator();
                ModesMenu2.Add("FarmEF", new CheckBox("Laneclear modunda E ile zorlama", true));
                ModesMenu2.Add("FarmQ", new CheckBox("Q Kullan", true));
                ModesMenu2.Add("FarmE", new CheckBox("E Kullan", true));
                ModesMenu2.Add("ManaLE", new Slider("Mana %", 40));
                ModesMenu2.AddSeparator();
                ModesMenu2.AddLabel("Jungle Clear Ayarları");
                ModesMenu2.Add("JungleEF", new CheckBox("JungleClear E Forced Target", true));
                ModesMenu2.Add("JungleQ", new CheckBox("Use Q on JungleClear", true));
                ModesMenu2.Add("JungleE", new CheckBox("Use E on JungleClear", true));
                ModesMenu2.Add("ManaJE", new Slider("Mana %", 40));

                ModesMenu3 = Menu.AddSubMenu("Misc", "Modes3Tristana");
                ModesMenu3.Add("AntiGapW", new CheckBox("W Kullan şunun içni Anti-Gapcloser", true));
                ModesMenu3.Add("AntiGapR", new CheckBox("R Kullan şunun içni Anti-Gapcloser", false));
                ModesMenu3.Add("AntiGapKR", new CheckBox("R Kullan şunun içni Anti-Gapcloser (Khazix & Rengar)", true));
                ModesMenu3.Add("FleeW", new CheckBox("Kaçma modunda W", false));

                ModesMenu3.AddSeparator();
                ModesMenu3.AddLabel("Skin Hack");
                ModesMenu3.Add("skinhack", new CheckBox("Kostüm Hilesi Aktif", false));
                ModesMenu3.Add("skinId", new ComboBox("Skin Mode", 0, "Default", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10"));

                ModesMenu3.AddSeparator();
                ModesMenu3.AddLabel("İtemleri Kullanma");
                ModesMenu3.Add("useYoumuu", new CheckBox("Kullan Youmuu", true));
                ModesMenu3.Add("usehextech", new CheckBox("Kullan Hextech", true));
                ModesMenu3.Add("useBotrk", new CheckBox("Kullan Botrk & Cutlass", true));
                ModesMenu3.Add("useQss", new CheckBox("Kullan QuickSilver", true));
                ModesMenu3.Add("minHPBotrk", new Slider("Mahvolmuş için can %", 80));
                ModesMenu3.Add("enemyMinHPBotrk", new Slider("Mahvolmuş için düşman canı %", 80));

                ModesMenu3.AddLabel("QSS Ayarları");
                ModesMenu3.Add("Qssmode", new ComboBox(" ", 0, "Auto", "Combo"));
                ModesMenu3.Add("Stun", new CheckBox("Sabitleme", true));
                ModesMenu3.Add("Blind", new CheckBox("Kör", true));
                ModesMenu3.Add("Charm", new CheckBox("Çekicilik(Ahri)", true));
                ModesMenu3.Add("Suppression", new CheckBox("Önleme,Durdurma", true));
                ModesMenu3.Add("Polymorph", new CheckBox("Polymorph", true));
                ModesMenu3.Add("Fear", new CheckBox("Korku", true));
                ModesMenu3.Add("Taunt", new CheckBox("Tuzak", true));
                ModesMenu3.Add("Silence", new CheckBox("Sessiz", false));
                ModesMenu3.Add("QssDelay", new Slider("QSS Gecikmesi", 250, 0, 1000));

                ModesMenu3.AddLabel("QSS Ult Ayarları");
                ModesMenu3.Add("ZedUlt", new CheckBox("Zed R", true));
                ModesMenu3.Add("VladUlt", new CheckBox("Vladimir R", true));
                ModesMenu3.Add("FizzUlt", new CheckBox("Fizz R", true));
                ModesMenu3.Add("MordUlt", new CheckBox("Mordekaiser R", true));
                ModesMenu3.Add("PoppyUlt", new CheckBox("Poppy R", true));
                ModesMenu3.Add("QssUltDelay", new Slider("Ulti için QSS gecikmesi", 250, 0, 1000));

                DrawMenu = Menu.AddSubMenu("Draws", "DrawTristana");
                DrawMenu.Add("drawA", new CheckBox(" Göster AA menzili", true));
                DrawMenu.Add("drawW", new CheckBox(" Göster W", true));
                DrawMenu.Add("drawE", new CheckBox(" Göster E Yükü", true));
            }

            catch (Exception e)
            {

            }

        }

        private static void Game_OnDraw(EventArgs args)
        {
            try
            {
                if (DrawMenu["drawW"].Cast<CheckBox>().CurrentValue)
                {
                    if (W.IsReady() && W.IsLearned)
                    {
                        Circle.Draw(Color.White, W.Range, Player.Instance.Position);
                    }
                }
                if (DrawMenu["drawA"].Cast<CheckBox>().CurrentValue)
                {
                    Circle.Draw(Color.LightGreen, PlayerInstance.AttackRange + 50, Player.Instance.Position);
                }
                if (DrawMenu["drawE"].Cast<CheckBox>().CurrentValue)
                {   // Thanks Ban# El :P //
                    var target = EntityManager.Heroes.Enemies.Find(
                        e => e.HasBuff("TristanaECharge") && e.IsValidTarget(2000));
                    if (!target.IsValidTarget())
                    {
                        return;
                    }


                    if (LastHarassPos == null)
                    {
                        LastHarassPos = _Player.ServerPosition;
                    }

                    var x = target.HPBarPosition.X + 45;
                    var y = target.HPBarPosition.Y - 25;

                    if (E.Level > 0)
                    {
                        if (HasEBuff(target)) //Credits to lizzaran 
                        {
                            int stacks = target.GetBuffCount("TristanaECharge");
                            if (stacks > -1)
                            {
                                for (var i = 0; 4 > i; i++)
                                {
                                    
                                    Drawing.DrawLine(x + i * 20, y, x + i * 20 + 5, y, 15, i > stacks ? color.DarkGray : color.LightSeaGreen);
                                }
                            }
                        }
                    }
                 }
            }
            catch (Exception e)
            {

            }
        }

        static void Game_OnUpdate(EventArgs args)
        {
            try
            {
                var keybind = ModesMenu1["ManualR"].Cast<KeyBind>().CurrentValue;

                if (keybind)
                {
                    var Target = TargetSelector.GetTarget(R.Range, DamageType.Physical);
                    if (Target == null) return;
                    if (!Target.IsValid()) return;
                    R.Cast(Target);
                }

                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                {
                    Common.Combo();
                    Common.ItemUsage();
                }
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
                {
                    Common.Harass();
                }

                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
                {

                    Common.LaneClear();

                }

                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
                {
                    Common.JungleClear();
                }

                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
                {
                    //Common.LastHit();

                }
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
                {
                    Common.Flee();

                }
            }
            catch (Exception e)
            {

            }
        }

        public static void OnTick(EventArgs args)
        {
            Common.KillSteal();
            Common.Skinhack();
        }

        private static void OnCreate(GameObject sender, EventArgs args)
        {
            var Rengar = EntityManager.Heroes.Enemies.Find(R => R.ChampionName.Equals("Rengar"));
            var khazix = EntityManager.Heroes.Enemies.Find(Kz => Kz.ChampionName.Equals("Khazix"));

            if (ModesMenu3["AntiGapKR"].Cast<CheckBox>().CurrentValue && R.IsReady())
            {
                if (Rengar != null)
                {
                    if (sender.Name == "Rengar_LeapSound.troy" && sender.Position.Distance(Player.Instance) < R.Range)
                        R.Cast(Rengar);
                }
                if (khazix != null)
                {
                    if (sender.Name == "Khazix_Base_E_Tar.troy" && sender.Position.Distance(Player.Instance) <= 500)
                        R.Cast(khazix);
                }
            }
        }

        private static void OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                if (ModesMenu1["ComboEF"].Cast<CheckBox>().CurrentValue)
                {
                    var forcedtarget = CloseEnemies(Q.Range).Find
                        (a => a.HasBuff("TristanaECharge"));

                    Player.IssueOrder(GameObjectOrder.AttackUnit, forcedtarget, true);
                }
            }
        }

    }
}
