using System.Collections.Generic;
using System.Linq;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using YasuoBuddy.EvadePlus.SkillshotTypes;

namespace YasuoBuddy.EvadePlus
{
    internal class EvadeMenu
    {
        public static Menu MainMenu { get; private set; }
        public static Menu SkillshotMenu { get; private set; }
        public static Menu SpellMenu { get; private set; }
        public static Menu DrawMenu { get; private set; }
        public static Menu ControlsMenu { get; private set; }

        public static readonly Dictionary<string, EvadeSkillshot> MenuSkillshots =
            new Dictionary<string, EvadeSkillshot>();

        public static void CreateMenu()
        {
            if (MainMenu != null)
            {
                return;
            }

            MainMenu = EloBuddy.SDK.Menu.MainMenu.AddMenu("NewYasuo Evade", "YassEvade");

            // Set up main menu
            MainMenu.AddGroupLabel("Geral");
            MainMenu.Add("fowDetection", new CheckBox("Enable FOW detection"));
            MainMenu.Add("processSpellDetection", new CheckBox("Enable Process Spell Detection"));
            MainMenu.Add("limitDetectionRange", new CheckBox("Limit Spell Detection Range"));
            MainMenu.Add("recalculatePosition", new CheckBox("Recalculate position/Recalcular posicao"));
            MainMenu.Add("moveToInitialPosition", new CheckBox("Back to current position/Voltar para posicão atual.", false));
            MainMenu.Add("serverTimeBuffer", new Slider("Server Time Buffer/Tempo de Servidor", 0));
            MainMenu.AddSeparator();
            MainMenu.AddSeparator();

            MainMenu.AddGroupLabel("Humanizer");
            MainMenu.Add("skillshotActivationDelay", new Slider("Skillshot Delay", 0, 0, 400));
            MainMenu.AddSeparator(10);
            MainMenu.Add("extraEvadeRange", new Slider("Extra Evade", 15, 0, 100));
            MainMenu.Add("randomizeExtraEvadeRange", new CheckBox("Randomize Extra Evade/Extra Evade Aleatorio", false));

            // Set up skillshot menu
            var heroes = EntityManager.Heroes.Enemies;
            var heroNames = heroes.Select(obj => obj.ChampionName).ToArray();
            var skillshots =
                SkillshotDatabase.Database.Where(s => heroNames.Contains(s.SpellData.ChampionName)).ToList();
            skillshots.AddRange(
                SkillshotDatabase.Database.Where(
                    s =>
                        s.SpellData.ChampionName == "AllChampions" &&
                        heroes.Any(obj => obj.Spellbook.Spells.Select(c => c.Name).Contains(s.SpellData.SpellName))));

            SkillshotMenu = MainMenu.AddSubMenu("Skillshots");
            SkillshotMenu.AddLabel(string.Format("Skillshots total: {0}", skillshots.Count));
            SkillshotMenu.AddSeparator();

            foreach (var c in skillshots)
            {
                var skillshotString = c.ToString().ToLower();

                if (MenuSkillshots.ContainsKey(skillshotString))
                    continue;

                MenuSkillshots.Add(skillshotString, c);

                SkillshotMenu.AddGroupLabel(c.DisplayText);
                SkillshotMenu.Add(skillshotString + "/enable", new CheckBox("Dodge/Esquivar"));
                SkillshotMenu.Add(skillshotString + "/draw", new CheckBox("Draw"));
                if (c is LinearMissileSkillshot)
                {
                    SkillshotMenu.Add(skillshotString + "/w", new CheckBox("Dodge With W/Esquivar com o W"));
                }
                SkillshotMenu.Add(skillshotString + "/e", new CheckBox("Dodge With E/Esquivar com o E"));
                var dangerous = new CheckBox("Dangerous/Perigoso", c.SpellData.IsDangerous);
                dangerous.OnValueChange += delegate(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
                {
                    GetSkillshot(sender.SerializationId).SpellData.IsDangerous = args.NewValue;
                };
                SkillshotMenu.Add(skillshotString + "/dangerous", dangerous);

                var dangerValue = new Slider("Level of Danger/Nivel de Perigo", c.SpellData.DangerValue, 1, 5);
                dangerValue.OnValueChange += delegate(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
                {
                    GetSkillshot(sender.SerializationId).SpellData.DangerValue = args.NewValue;
                };
                SkillshotMenu.Add(skillshotString + "/dangervalue", dangerValue);

                SkillshotMenu.AddSeparator();
            }

            // Set up spell menu
            SpellMenu = MainMenu.AddSubMenu("Spell");
            SpellMenu.AddGroupLabel("Flash");
            SpellMenu.Add("flash", new Slider("Danger Value", 5, 0, 5));

            // Set up draw menu
            DrawMenu = MainMenu.AddSubMenu("Draw");
            DrawMenu.AddGroupLabel("Evade Draw");
            DrawMenu.Add("disableAllDrawings", new CheckBox("Disable/Desativar Drawings", true));
            DrawMenu.Add("drawEvadePoint", new CheckBox("Draw Evade Point", false));
            DrawMenu.Add("drawEvadeStatus", new CheckBox("Draw Evade Status", false));
            DrawMenu.Add("drawDangerPolygon", new CheckBox("Draw Danger Polygon", false));
            DrawMenu.AddSeparator();
            DrawMenu.Add("drawPath", new CheckBox("Draw Autpathing Path", false));

            // Set up controls menu
            ControlsMenu = MainMenu.AddSubMenu("Controle");
            ControlsMenu.AddGroupLabel("Controle");
            ControlsMenu.Add("enableEvade", new KeyBind("Enable Evade", true, KeyBind.BindTypes.PressToggle));
            ControlsMenu.Add("dodgeOnlyDangerous", new KeyBind("Dodge Only Dangerous", false, KeyBind.BindTypes.HoldActive));

            TargetedSpells.SpellDetectorWindwaller.Init();
        }

        private static EvadeSkillshot GetSkillshot(string s)
        {
            return MenuSkillshots[s.ToLower().Split('/')[0]];
        }

        public static bool IsSkillshotEnabled(EvadeSkillshot skillshot)
        {
            var valueBase = SkillshotMenu[skillshot + "/enable"];
            return valueBase != null && valueBase.Cast<CheckBox>().CurrentValue;
        }

        public static bool IsSkillshotW(EvadeSkillshot skillshot)
        {
            var valueBase = SkillshotMenu[skillshot + "/w"];
            return valueBase != null && valueBase.Cast<CheckBox>().CurrentValue;
        }

        public static bool IsSkillshotE(EvadeSkillshot skillshot)
        {
            var valueBase = SkillshotMenu[skillshot + "/e"];
            return valueBase != null && valueBase.Cast<CheckBox>().CurrentValue;
        }

        public static bool IsSkillshotDrawingEnabled(EvadeSkillshot skillshot)
        {
            var valueBase = SkillshotMenu[skillshot + "/draw"];
            return valueBase != null && valueBase.Cast<CheckBox>().CurrentValue;
        }
    }
}