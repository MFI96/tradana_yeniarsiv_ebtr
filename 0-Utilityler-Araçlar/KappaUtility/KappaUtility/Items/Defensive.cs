namespace KappaUtility.Items
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;

    using Common;

    internal class Defensive
    {
        public static readonly Item Zhonyas = new Item(ItemId.Zhonyas_Hourglass);

        public static readonly Item Seraph = new Item(ItemId.Seraphs_Embrace);

        public static readonly Item FOTM = new Item(ItemId.Face_of_the_Mountain, 600);

        public static readonly Item Solari = new Item(ItemId.Locket_of_the_Iron_Solari, 600);

        public static readonly Item Randuin = new Item(ItemId.Randuins_Omen, 500f);

        public static int Seraphh => DefMenu.GetSlider("Seraphh");

        public static int Solarih => DefMenu.GetSlider("Solarih");

        public static int FaceOfTheMountainh => DefMenu.GetSlider("FaceOfTheMountainh");

        public static int Zhonyash => DefMenu.GetSlider("Zhonyash");

        public static int Seraphn => DefMenu.GetSlider("Seraphn");

        public static int Solarin => DefMenu.GetSlider("Solarin");

        public static int FaceOfTheMountainn => DefMenu.GetSlider("FaceOfTheMountainn");

        public static int Zhonyasn => DefMenu.GetSlider("Zhonyasn");

        public static bool Seraphc => DefMenu.GetCheckbox("Seraph") && Seraph.IsOwned(Player.Instance) && Seraph.IsReady();

        public static bool Solaric => DefMenu.GetCheckbox("Solari") && Solari.IsOwned(Player.Instance) && Solari.IsReady();

        public static bool FaceOfTheMountainc => DefMenu.GetCheckbox("FaceOfTheMountain") && FOTM.IsOwned(Player.Instance) && FOTM.IsReady();

        public static bool Zhonyasc => DefMenu.GetCheckbox("Zhonyas") && Zhonyas.IsOwned(Player.Instance) && Zhonyas.IsReady();

        public static Menu DefMenu { get; private set; }

        protected static bool loaded = false;

        internal static void OnLoad()
        {
            DefMenu = Load.UtliMenu.AddSubMenu("Defence Items");
            DefMenu.AddGroupLabel("Savunma Ayarları");
            DefMenu.Checkbox("Zhonyas", "Kullan Zhonyas");
            DefMenu.Slider("Zhonyash", "Kullan Zhonyas canım şundan az [{0}%]", 35);
            DefMenu.Slider("Zhonyasn", "Kullan Zhonyas eğer daha fazla zarar geliyorsa [{0}%]", 50);
            DefMenu.AddSeparator();
            DefMenu.Checkbox("Seraph", "Kullan Seraph");
            DefMenu.Slider("Seraphh", "Kullan Seraph Canım şundan az [{0}%]", 45);
            DefMenu.Slider("Seraphn", "Kullan Seraph eğer daha fazla zarar geliyorsa [{0}%]", 45);
            DefMenu.AddSeparator();
            DefMenu.Checkbox("FaceOfTheMountain", "Kullan Dağın Sureti");
            DefMenu.Slider("FaceOfTheMountainh", "Kullan Dağın sureti için canım şundan az [{0}%]", 50);
            DefMenu.Slider("FaceOfTheMountainn", "Kullan Dağın eğer daha fazla zarar geliyorsa [{0}%]", 50);
            DefMenu.AddSeparator();
            DefMenu.Checkbox("Solari", "Kullan Solari");
            DefMenu.Slider("Solarih", "Kullan Solari can şundan azsa [{0}%]", 30);
            DefMenu.Slider("Solarin", "Kullan Solari eğer canımı şundan aşağı düşürecek hasar geliyorsa [{0}%]", 35);
            DefMenu.AddSeparator();
            DefMenu.Checkbox("Randuin", "Kullan Randuin (Omen)");
            DefMenu.Slider("Randuinh", "Kullan Randuin şu kadar düşmanda", 2, 1, 5);
            DefMenu.AddSeparator();
            DefMenu.AddGroupLabel("Zhonya tehlike seviyesi");
            DefMenu.Checkbox("ZhonyasD", "Tehlikeli büyülerde kullanma");
            DamageHandler.OnLoad();
            Zhonya.OnLoad();
            loaded = true;
        }

        internal static void Items()
        {
            if (!loaded)
            {
                return;
            }

            if (Randuin.IsReady() && Randuin.IsOwned(Player.Instance) && Helpers.CountEnemies((int)Randuin.Range) >= DefMenu.GetSlider("Randuinh")
                && DefMenu.GetCheckbox("Randuin"))
            {
                Randuin.Cast();
            }
        }

        public static void defcast(Obj_AI_Base caster, Obj_AI_Base target, Obj_AI_Base enemy, float dmg)
        {
            var damagepercent = (dmg / target.TotalShieldHealth()) * 100;
            var death = damagepercent >= target.HealthPercent || dmg >= target.TotalShieldHealth();

            if (target.IsValidTarget(Defensive.FOTM.Range) && Defensive.FaceOfTheMountainc)
            {
                if (Defensive.FaceOfTheMountainh >= target.HealthPercent || death || damagepercent >= Defensive.FaceOfTheMountainn)
                {
                    Defensive.FOTM.Cast(target);
                }
            }

            if (target.IsValidTarget(Defensive.Solari.Range) && Defensive.Solaric)
            {
                if (Defensive.Solarih >= target.HealthPercent || death || damagepercent >= Defensive.Solarin)
                {
                    Defensive.Solari.Cast();
                }
            }

            if (target.IsMe)
            {
                if (Defensive.Seraphc)
                {
                    if (Defensive.Seraphh >= target.HealthPercent || death || damagepercent >= Defensive.Seraphn)
                    {
                        Defensive.Seraph.Cast();
                    }
                }

                if (Defensive.Zhonyasc)
                {
                    if (Defensive.Zhonyash >= target.HealthPercent || death || damagepercent >= Defensive.Zhonyasn)
                    {
                        Defensive.Zhonyas.Cast();
                    }
                }
            }
        }
    }
}