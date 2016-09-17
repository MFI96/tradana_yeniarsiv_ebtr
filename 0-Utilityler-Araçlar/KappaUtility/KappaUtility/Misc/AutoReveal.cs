namespace KappaUtility.Misc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;
    using EloBuddy.SDK.Rendering;

    using KappaUtility.Common;
    using KappaUtility.Summoners;

    using SharpDX;

    internal class AutoReveal
    {
        public static readonly Item Sweeping_Lens_Trinket = new Item(ItemId.Sweeping_Lens_Trinket, 600f);

        public static readonly Item Oracle_Alteration = new Item(ItemId.Oracle_Alteration, 600f);

        public static readonly Item Vision_Ward = new Item(ItemId.Vision_Ward, 600f);

        public static readonly Item Warding_Totem_Trinket = new Item(ItemId.Warding_Totem_Trinket, 600f);

        public static readonly Item Greater_Stealth_Totem_Trinket = new Item(ItemId.Greater_Stealth_Totem_Trinket, 600f);

        public static readonly Item Greater_Vision_Totem_Trinket = new Item(ItemId.Greater_Vision_Totem_Trinket, 600f);

        public static readonly Item Sightstone = new Item((int)ItemId.Sightstone, 600f);

        public static readonly Item Ruby_Sightstone = new Item((int)ItemId.Ruby_Sightstone, 600f);

        public static readonly Item Farsight_Alteration = new Item((int)ItemId.Farsight_Alteration, 1250);

        static readonly List<Stealth> SpellList = new List<Stealth>();

        public static float vaynebuff = 0f;

        public struct Stealth
        {
            public SpellSlot Slot;

            public Champion Hero;

            public String Name;
        } 

        public static Menu BushMenu { get; private set; }

        internal static void OnLoad()
        {

            SpellList.Add(new Stealth { Hero = Champion.Akali, Name = "akalismokebomb", Slot = SpellSlot.W });
            SpellList.Add(new Stealth { Hero = Champion.Shaco, Name = "deceive", Slot = SpellSlot.Q });
            SpellList.Add(new Stealth { Hero = Champion.Khazix, Name = "khazixr", Slot = SpellSlot.R });
            SpellList.Add(new Stealth { Hero = Champion.Khazix, Name = "khazixrlong", Slot = SpellSlot.R });
            SpellList.Add(new Stealth { Hero = Champion.Talon, Name = "talonshadowassault", Slot = SpellSlot.R });
            SpellList.Add(new Stealth { Hero = Champion.MonkeyKing, Name = "monkeykingdecoy", Slot = SpellSlot.W });
            SpellList.Add(new Stealth { Hero = Champion.Vayne, Name = "vaynetumble", Slot = SpellSlot.Q });
            SpellList.Add(new Stealth { Hero = Champion.Twitch, Name = "hideinshadows", Slot = SpellSlot.Q });

            BushMenu = Load.UtliMenu.AddSubMenu("Auto Revealer");
            BushMenu.AddGroupLabel("Gizleneni gösterme");
            BushMenu.Add("enable", new CheckBox("Aktif", false));
            BushMenu.Add("combo", new CheckBox("Sadece Komboda", false));
            BushMenu.AddSeparator();
            BushMenu.AddGroupLabel("Gizleneni Göster");
            BushMenu.Add("enables", new CheckBox("Aktif", false));
            BushMenu.Add("combos", new CheckBox("Sadece Komboda", false));
            BushMenu.AddGroupLabel("Şampiyon Seç:");
            foreach (var champion in SpellList)
            {
                BushMenu.Add(champion.Name, new CheckBox(champion.Hero + " - " + champion.Slot + " - " + champion.Name));
            }

            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
        }

        public static void OnTick()
        {
            var vayne =
                EntityManager.Heroes.Enemies.FirstOrDefault(
                    v => v.Hero == Champion.Vayne && v.IsEnemy && v.Buffs.Any(b => b.Name.ToLower().Contains("vayneinquisition")));

            if (vayne != null)
            {
                vaynebuff = vayne.GetBuff("VayneInquisition").EndTime;
            }
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if(!sender.IsEnemy || sender == null) return;

            if (SpellList.Any(spell => spell.Name == args.SData.Name.ToLower()))
            {
                if (BushMenu.GetCheckbox(args.SData.Name))
                {
                    if (args.SData.Name.ToLower().Contains("vaynetumble") && Game.Time > vaynebuff)
                    {
                        return;
                    }
                    
                    RevealCast(sender.ServerPosition);
                }
            }
        }

        public static void Reveal()
        {
            var enemies = EntityManager.Heroes.Enemies.Where(x => !x.IsDead && x.Distance(Player.Instance) < 1250);
            if (BushMenu["enable"].Cast<CheckBox>().CurrentValue && !Player.Instance.IsDead)
            {
                var flags = Orbwalker.ActiveModesFlags;
                foreach (var target in enemies)
                {
                    var pred = Prediction.Position.PredictUnitPosition(target, 500).To3D();

                    if (!BushMenu["combo"].Cast<CheckBox>().CurrentValue)
                    {
                        if (NavMesh.IsWallOfGrass(pred, 50))
                        {
                            WardCast(pred);
                        }
                    }

                    if (BushMenu["combo"].Cast<CheckBox>().CurrentValue && flags.HasFlag(Orbwalker.ActiveModes.Combo))
                    {
                        if (NavMesh.IsWallOfGrass(pred, 50))
                        {
                            WardCast(pred);
                        }
                    }
                }
            }
        }

        public static void RevealCast(Vector3 vector3)
        {
            if (Sweeping_Lens_Trinket.IsOwned() && Sweeping_Lens_Trinket.IsReady() && vector3.IsInRange(Player.Instance, Sweeping_Lens_Trinket.Range))
            {
                if (Sweeping_Lens_Trinket.Cast(vector3))
                {
                    return;
                }
            }

            if (Oracle_Alteration.IsOwned() && Oracle_Alteration.IsReady() && vector3.IsInRange(Player.Instance, Oracle_Alteration.Range))
            {
                if (Oracle_Alteration.Cast(vector3))
                {
                    return;
                }
            }

            if (Vision_Ward.IsOwned() && Vision_Ward.IsReady() && vector3.IsInRange(Player.Instance, Oracle_Alteration.Range))
            {
                Vision_Ward.Cast(Player.Instance.ServerPosition);
            }
        }

        public static void WardCast(Vector3 vector3)
        {
            var ward = ObjectManager.Get<Obj_AI_Minion>().Any(w => w.Name.ToLower().Contains("ward") && w.Name != "WardCorpse" && w.IsAlly && w.IsValid && w.Distance(vector3) < 500);

            var ally = EntityManager.Heroes.Allies.Any(a => a.Distance(vector3) < 100 && a.IsValidTarget());

            if (ward || ally) return;

            if (Warding_Totem_Trinket.IsOwned() && Warding_Totem_Trinket.IsReady() && Player.Instance.IsInRange(vector3, Warding_Totem_Trinket.Range))
            {
                if (Warding_Totem_Trinket.Cast(vector3))
                {
                    return;
                }
            }

            if (Greater_Stealth_Totem_Trinket.IsOwned() && Greater_Stealth_Totem_Trinket.IsReady() && Player.Instance.IsInRange(vector3, Greater_Stealth_Totem_Trinket.Range))
            {
                if (Greater_Stealth_Totem_Trinket.Cast(vector3))
                {
                    return;
                }
            }

            if (Greater_Vision_Totem_Trinket.IsOwned() && Greater_Vision_Totem_Trinket.IsReady() && Player.Instance.IsInRange(vector3, Greater_Vision_Totem_Trinket.Range))
            {
                if (Greater_Vision_Totem_Trinket.Cast(vector3))
                {
                    return;
                }
            }

            if (Sightstone.IsOwned() && Sightstone.IsReady() && Player.Instance.IsInRange(vector3, Sightstone.Range))
            {
                if (Sightstone.Cast(vector3))
                {
                    return;
                }
            }

            if (Ruby_Sightstone.IsOwned() && Ruby_Sightstone.IsReady() && Player.Instance.IsInRange(vector3, Ruby_Sightstone.Range))
            {
                if (Ruby_Sightstone.Cast(vector3))
                {
                    return;
                }
            }

            if (Farsight_Alteration.IsOwned() && Farsight_Alteration.IsReady() && Player.Instance.IsInRange(vector3, Farsight_Alteration.Range))
            {
                if (Farsight_Alteration.Cast(vector3))
                {
                    return;
                }
            }
        }
    }
}