namespace KappaUtility.Misc
{
    using EloBuddy;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    internal class SkinHax
    {
        public static Menu SkinMenu { get; private set; }

        internal static void OnLoad()
        {
            SkinMenu = Load.UtliMenu.AddSubMenu("Skin Hax");
            SkinMenu.AddGroupLabel("Kostüm Ayarları");
            SkinMenu.Add(Player.Instance.ChampionName + "skin", new CheckBox("Aktif", false));
            SkinMenu.Add(Player.Instance.ChampionName + "skins", new Slider("Kostüm Seç", 0, 0, 15)).OnValueChange += delegate { Hax(); };

            SkinMenu.AddLabel("Can be buggy When your Champion Model Changes in Game.");
            Obj_AI_Base.OnUpdateModel += Obj_AI_Base_OnUpdateModel;
        }

        private static void Obj_AI_Base_OnUpdateModel(Obj_AI_Base sender, UpdateModelEventArgs args)
        {
            if (sender.IsMe
                && (args.Model != Player.Instance.Model || args.SkinId != SkinMenu[Player.Instance.ChampionName + "skins"].Cast<Slider>().CurrentValue))
            {
                args.Process = false;
            }
        }

        public static void Hax()
        {
            if (SkinMenu[Player.Instance.ChampionName + "skin"].Cast<CheckBox>().CurrentValue
                && Player.Instance.SkinId != SkinMenu[Player.Instance.ChampionName + "skins"].Cast<Slider>().CurrentValue)
            {
                Player.Instance.SetSkinId(SkinMenu[Player.Instance.ChampionName + "skins"].Cast<Slider>().CurrentValue);
            }
        }
    }
}