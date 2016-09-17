using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using Moon_Walk_Evade.EvadeSpells;
using Moon_Walk_Evade.Skillshots;

namespace Moon_Walk_Evade
{
    internal static class MenuExtension
    {
        public static void AddStringList(this Menu m, string uniqueId, string displayName, string[] values, int defaultValue)
        {
            var mode = m.Add(uniqueId, new Slider(displayName, defaultValue, 0, values.Length - 1));
            mode.DisplayName = displayName + ": " + values[mode.CurrentValue];
            mode.OnValueChange += delegate (ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
            {
                sender.DisplayName = displayName + ": " + values[args.NewValue];
            };
        }
    }
    internal class EvadeMenu
    {
        public static Menu MainMenu { get; private set; }
        public static Menu SkillshotMenu { get; private set; }
        public static Menu SpellMenu { get; private set; }
        public static Menu DrawMenu { get; private set; }
        public static Menu HotkeysMenu { get; private set; }

        public static Menu CollisionMenu { get; private set; }

        public static readonly Dictionary<string, EvadeSkillshot> MenuSkillshots = new Dictionary<string, EvadeSkillshot>();
        public static readonly List<EvadeSpellData> MenuEvadeSpells = new List<EvadeSpellData>();

        public static void CreateMenu()
        {
            if (MainMenu != null)
            {
                return;
            }

            MainMenu = EloBuddy.SDK.Menu.MainMenu.AddMenu("MoonWalkEvade", "MoonWalkEvade");

            // Set up main menu
            MainMenu.AddGroupLabel("Genel Ayarlar");
            MainMenu.Add("evadeMode", new ComboBox("Kaçış Modu", 0, "Akıllı - Moon Walk Stili", "Hızlı - EvadePlus Stili"));
            MainMenu.AddSeparator();

            MainMenu.Add("fowDetection", new CheckBox("Görülmeyen yerden büyülerden kaç"));
            MainMenu.Add("processSpellDetection", new CheckBox("Hızlı büyü Tespit etmeyi Aktif Et"));
            MainMenu.Add("limitDetectionRange", new CheckBox("Büyü Tespit Etme Menzilini Sınırla"));
            MainMenu.Add("recalculatePosition", new CheckBox("Kaçış Pozisyonunu yeniden Hesapla", false));
            MainMenu.Add("moveToInitialPosition", new CheckBox("Kaçarken İstenen Pozisyona Doğru Yürü", false));
            MainMenu.AddSeparator();

            MainMenu.Add("minComfortDist", new Slider("Düşmanlardan en iyi pozisyona doğru ilerle(değiştirmeyin)", 550, 0, 1000));
            MainMenu.AddLabel("Mümkünse");
            MainMenu.AddSeparator(10);
            MainMenu.Add("ignoreComfort", new Slider("X kadar düşman varsa bu pozisyonu hesaplama(değiştirmeyin)", 1, 1, 5));
            MainMenu.AddSeparator();

            MainMenu.AddGroupLabel("İnsancıl Ayar");
            MainMenu.Add("skillshotActivationDelay", new Slider("Tepki Süresi", 0, 0, 400));
            MainMenu.AddSeparator();

            MainMenu.Add("extraEvadeRange", new Slider("Ekstra Kaçma Mesafesi", 0, 0, 300));
            MainMenu.Add("randomizeExtraEvadeRange", new CheckBox("Karışık Ekstra Menzil", false));

            var heroes = Program.DeveloperMode ? EntityManager.Heroes.AllHeroes : EntityManager.Heroes.Enemies;
            var heroNames = heroes.Select(obj => obj.ChampionName).ToArray();
            var skillshots =
                SkillshotDatabase.Database.Where(s => heroNames.Contains(s.OwnSpellData.ChampionName)).ToList();
            skillshots.AddRange(
                SkillshotDatabase.Database.Where(
                    s =>
                        s.OwnSpellData.ChampionName == "AllChampions" &&
                        heroes.Any(obj => obj.Spellbook.Spells.Select(c => c.Name).Contains(s.OwnSpellData.SpellName))));
            var evadeSpells =
                EvadeSpellDatabase.Spells.Where(s => Player.Instance.ChampionName.Contains(s.ChampionName)).ToList();
            evadeSpells.AddRange(EvadeSpellDatabase.Spells.Where(s => s.ChampionName == "AllChampions"));


            SkillshotMenu = MainMenu.AddSubMenu("Büyüler");

            foreach (var c in skillshots)
            {
                var skillshotString = c.ToString().ToLower();

                if (MenuSkillshots.ContainsKey(skillshotString))
                    continue;

                MenuSkillshots.Add(skillshotString, c);

                SkillshotMenu.AddGroupLabel(c.DisplayText);
                SkillshotMenu.Add(skillshotString + "/enable", new CheckBox("Büyülerden Kaçma", c.OwnSpellData.EnabledByDefault));
                SkillshotMenu.Add(skillshotString + "/draw", new CheckBox("Göster"));

                var dangerous = new CheckBox("Tehlike", c.OwnSpellData.IsDangerous);
                dangerous.OnValueChange += delegate (ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
                {
                    GetSkillshot(sender.SerializationId).OwnSpellData.IsDangerous = args.NewValue;
                };
                SkillshotMenu.Add(skillshotString + "/dangerous", dangerous);

                var dangerValue = new Slider("Tehlike Seviyesi", c.OwnSpellData.DangerValue, 1, 5);
                dangerValue.OnValueChange += delegate (ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
                {
                    GetSkillshot(sender.SerializationId).OwnSpellData.DangerValue = args.NewValue;
                };
                SkillshotMenu.Add(skillshotString + "/dangervalue", dangerValue);

                SkillshotMenu.AddSeparator();
            }

            // Set up spell menu
            SpellMenu = MainMenu.AddSubMenu("Büyülerden Kaç");

            foreach (var e in evadeSpells)
            {
                var evadeSpellString = e.SpellName;

                if (MenuEvadeSpells.Any(x => x.SpellName == evadeSpellString))
                    continue;

                MenuEvadeSpells.Add(e);

                SpellMenu.AddGroupLabel(evadeSpellString);
                SpellMenu.Add(evadeSpellString + "/enable", new CheckBox("Kullan " + (!e.isItem ? e.Slot.ToString() : "")));

                var dangerValueSlider = new Slider("Tehlike Seviyesi", e.DangerValue, 1, 5);
                dangerValueSlider.OnValueChange += delegate (ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
                {
                    MenuEvadeSpells.First(x =>
                        x.SpellName.Contains(sender.SerializationId.Split('/')[0])).DangerValue = args.NewValue;
                };
                SpellMenu.Add(evadeSpellString + "/dangervalue", dangerValueSlider);

                SpellMenu.AddSeparator();
            }


            DrawMenu = MainMenu.AddSubMenu("Göstergeler,Çizimler");
            DrawMenu.Add("disableAllDrawings", new CheckBox("Tüm Göstergeler Devredışı", false));
            DrawMenu.Add("drawEvadePoint", new CheckBox("Kaçılacak noktayı Göster"));
            DrawMenu.Add("drawEvadeStatus", new CheckBox("Kaçma Durumunu Göster"));
            DrawMenu.Add("drawSkillshots", new CheckBox("Büyülerin gittiği yeri"));
            DrawMenu.Add("drawDangerPolygon", new CheckBox("Tehlikeli Alanı Göster"));


            HotkeysMenu = MainMenu.AddSubMenu("Tuşlar");
            HotkeysMenu.Add("enableEvade", new KeyBind("Kaçma Aktif", true, KeyBind.BindTypes.PressToggle, 'M'));
            HotkeysMenu.Add("dodgeOnlyDangerousH", new KeyBind("Sadece Tehlikede Kaç (Basılı Tut)", false, KeyBind.BindTypes.HoldActive));
            HotkeysMenu.Add("dodgeOnlyDangerousT", new KeyBind("Sadece Tehlikede Kaç (Tuş)", false, KeyBind.BindTypes.PressToggle));
            HotkeysMenu.AddSeparator();
            HotkeysMenu.Add("debugMode", new KeyBind("Hata Ayıklama Modu", false, KeyBind.BindTypes.PressToggle));
            HotkeysMenu.Add("debugModeIntervall", new Slider("Hata Ayıklama Süresi Aralığı", 1000, 0, 5000));
            HotkeysMenu.AddStringList("debugMissile", "Seçilen Büyüler", SkillshotDatabase.Database.Select(x => x.OwnSpellData.SpellName).ToArray(), 0);
            HotkeysMenu.Add("isProjectile", new CheckBox("Mermi?"));

            CollisionMenu = MainMenu.AddSubMenu("Önüne birşey gelmesini ayarla(Collision)");
            CollisionMenu.Add("minion", new CheckBox("Minyonun önüne geçmesini Hesapla"));
            CollisionMenu.Add("yasuoWall", new CheckBox("Yasuo Duvaırını Hesapla"));
            CollisionMenu.Add("useProj", new CheckBox("Büyü Öngörümü Kullan", false));
        }

        private static EvadeSkillshot GetSkillshot(string s)
        {
            return MenuSkillshots[s.ToLower().Split('/')[0]];
        }

        public static bool IsSkillshotEnabled(EvadeSkillshot skillshot)
        {
            var valueBase = SkillshotMenu[skillshot + "/enable"];
            return (valueBase != null && valueBase.Cast<CheckBox>().CurrentValue) ||
                HotkeysMenu["debugMode"].Cast<KeyBind>().CurrentValue;
        }

        public static bool IsSkillshotDrawingEnabled(EvadeSkillshot skillshot)
        {
            var valueBase = SkillshotMenu[skillshot + "/draw"];
            return (valueBase != null && valueBase.Cast<CheckBox>().CurrentValue) ||
                HotkeysMenu["debugMode"].Cast<KeyBind>().CurrentValue;
        }
    }
}