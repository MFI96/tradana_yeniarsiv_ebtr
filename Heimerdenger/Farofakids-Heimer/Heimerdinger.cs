using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Enumerations;
using Farofakids_Heimer.Properties;
using SharpDX;
using Color = System.Drawing.Color;
using SettingsOnDraw = Farofakids_Heimer.Heimerdinger.Config.Modes.Draw;
using SettingsCombo = Farofakids_Heimer.Heimerdinger.Config.Modes.Combo;
using SettingsHarass = Farofakids_Heimer.Heimerdinger.Config.Modes.Harass;
using SettingsLaneClear = Farofakids_Heimer.Heimerdinger.Config.Modes.LaneClear;
using SettingsLastHit = Farofakids_Heimer.Heimerdinger.Config.Modes.LastHit;
using SettingsMisc = Farofakids_Heimer.Heimerdinger.Config.Modes.Misc;

namespace Farofakids_Heimer
{
    class Heimerdinger
    {
        public static void Initialize()
        {
            Config.Initialize();
            SpellManager.Initialize();
            ModeManager.Initialize();
            DamageIndicator.Initialize(SpellDamage.GetTotalDamage);
            EventsManager.Initialize();

            Drawing.OnDraw += OnDraw;
        }

        private static void OnDraw(EventArgs args)
        {
            if (SettingsOnDraw.DrawQ && SettingsOnDraw.DrawReady ? SpellManager.Q.IsReady() : SettingsOnDraw.DrawQ)
            {
                Circle.Draw(SettingsOnDraw.QColor, SpellManager.Q.Range, 1f, Player.Instance);
            }

            if (SettingsOnDraw.DrawW && SettingsOnDraw.DrawReady ? SpellManager.W.IsReady() : SettingsOnDraw.DrawW)
            {
                Circle.Draw(SettingsOnDraw.WColor, SpellManager.W.Range, 1f, Player.Instance);
            }

            if (SettingsOnDraw.DrawE && SettingsOnDraw.DrawReady ? SpellManager.E.IsReady() : SettingsOnDraw.DrawE)
            {
                Circle.Draw(SettingsOnDraw.EColor, SpellManager.E.Range, 1f, Player.Instance);
            }

            if (SettingsOnDraw.DrawR && SettingsOnDraw.DrawReady ? SpellManager.R.IsReady() : SettingsOnDraw.DrawR)
            {
                Circle.Draw(SettingsOnDraw.RColor, SpellManager.R.Range, 1f, Player.Instance);
            }
        }

        public class ColorConfig
        {
            public Slider RedSlider { get; set; }
            public Slider BlueSlider { get; set; }
            public Slider GreenSlider { get; set; }
            private ColorPickerControl ColorPicker { get; set; }

            public string Id { get; private set; }
            private static Menu _menu;

            public ColorConfig(Menu menu, string id, Color color, string GropuLabelName)
            {
                Id = id;
                _menu = menu;
                Init(color, GropuLabelName);
            }

            public void Init(Color color, string name)
            {
                RedSlider = new Slider("Red", color.R, 0, 255);
                GreenSlider = new Slider("Green", color.B, 0, 255);
                BlueSlider = new Slider("Blue", color.G, 0, 255);
                ColorPicker = new ColorPickerControl(Id + "ColorDisplay", color);

                _menu.AddGroupLabel(name);

                _menu.Add(Id + "ColorDisplay", ColorPicker);
                _menu.Add(Id + "Red", RedSlider);
                _menu.Add(Id + "Green", GreenSlider);
                _menu.Add(Id + "Blue", BlueSlider);

                RedSlider.OnValueChange += OnValueChange;
                GreenSlider.OnValueChange += OnValueChange;
                BlueSlider.OnValueChange += OnValueChange;

                ColorPicker.SetColor(Color.FromArgb(GetValue(ColorBytes.Red), GetValue(ColorBytes.Green), GetValue(ColorBytes.Blue)));
            }

            private void OnValueChange(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
            {
                if (sender.DisplayName == RedSlider.DisplayName)
                {
                    ColorPicker.SetColor(Color.FromArgb(sender.CurrentValue, ColorPicker.CurrentValue.G, ColorPicker.CurrentValue.B));
                }
                if (sender.DisplayName == GreenSlider.DisplayName)
                {
                    ColorPicker.SetColor(Color.FromArgb(ColorPicker.CurrentValue.R, sender.CurrentValue, ColorPicker.CurrentValue.B));
                }
                if (sender.DisplayName == BlueSlider.DisplayName)
                {
                    ColorPicker.SetColor(Color.FromArgb(ColorPicker.CurrentValue.R, ColorPicker.CurrentValue.G, sender.CurrentValue));
                }
            }

            public ColorBGRA GetSharpColor()
            {                  //RED,GREEN,BLUE,AA
                return new ColorBGRA(GetValue(ColorBytes.Red), GetValue(ColorBytes.Green), GetValue(ColorBytes.Blue), 255);
            }

            public Color GetSystemColor()
            {
                return Color.FromArgb(GetValue(ColorBytes.Red), GetValue(ColorBytes.Green), GetValue(ColorBytes.Blue));
            }

            public byte GetValue(ColorBytes color)
            {
                switch (color)
                {
                    case ColorBytes.Red:
                        return Convert.ToByte(RedSlider.CurrentValue);
                    case ColorBytes.Blue:
                        return Convert.ToByte(BlueSlider.CurrentValue);
                    case ColorBytes.Green:
                        return Convert.ToByte(GreenSlider.CurrentValue);
                }
                return 255;
            }

            private class ColorPickerControl : ValueBase<Color>
            {
                private readonly string _name;
                private Vector2 _offset;
                private Color SelectedColor { get; set; }

                private EloBuddy.SDK.Rendering.Sprite _colorPickerSprite;
                private EloBuddy.SDK.Rendering.Sprite _colorOverlaySprite;
                private TextureLoader _textureLoader;

                public override string VisibleName { get { return _name; } }
                public override Vector2 Offset { get { return _offset; } }

                public ColorPickerControl(string uId, Color defaultValue)
                    : base(uId, "", 52)
                {
                    _name = "";
                    Init(defaultValue);
                }

                private static Bitmap ContructColorOverlaySprite()
                {
                    var bitmap = new Bitmap(30, 30);
                    for (int x = 0; x < 30; x++)
                    {
                        for (int y = 0; y < 30; y++)
                        {
                            bitmap.SetPixel(x, y, Color.White);
                        }
                    }
                    return bitmap;
                }

                public void SetColor(Color color)
                {
                    SelectedColor = color;
                }
                private void Init(Color color)
                {
                    _offset = new Vector2(0, 10);
                    _textureLoader = new TextureLoader();
                    _colorPickerSprite = new EloBuddy.SDK.Rendering.Sprite(_textureLoader.Load("ColorPickerSprite", Resources.ColorPickerSprite));
                    _colorOverlaySprite = new EloBuddy.SDK.Rendering.Sprite(_textureLoader.Load("ColorOverlaySprite", ContructColorOverlaySprite()));
                    SelectedColor = color;
                }

                public override Color CurrentValue { get { return SelectedColor; } }

                public override bool Draw()
                {
                    var rect = new SharpDX.Rectangle((int)MainMenu.Position.X + 160, (int)MainMenu.Position.Y + 95 + 50, 750, 380);
                    if (MainMenu.IsVisible && IsVisible && rect.IsInside(Position))
                    {
                        _colorPickerSprite.Draw(new Vector2(Position.X + 522, Position.Y - 34));
                        _colorOverlaySprite.Color = SelectedColor;
                        _colorOverlaySprite.Draw(new Vector2(Position.X + 522 + 1, Position.Y - 34 + 1));
                        return true;
                    }
                    return false;
                }

                public override Dictionary<string, object> Serialize()
                {
                    return base.Serialize();
                }
            }

            public enum ColorBytes
            {
                Red, Green, Blue
            }
        }

        #region Champion
        public static class Config
        {
            private static readonly string MenuName = "Farofakids " + Player.Instance.ChampionName;

            private static readonly Menu Menu;

            static Config()
            {
                Menu = MainMenu.AddMenu(MenuName, MenuName.ToLower());
                Menu.AddGroupLabel("Farofakids " + Player.Instance.ChampionName);
                Menu.AddGroupLabel("the original base this script is made by MarioGK");
                Menu.AddGroupLabel("https://github.com/mariogk");
                Menu.AddGroupLabel("Çeviri-TRAdana Güncellemede Bildirin");
                Modes.Initialize();
            }

            public static void Initialize()
            {
            }

            public static class Modes
            {
                private static readonly Menu SpellsMenu, FarmMenu, MiscMenu, DrawMenu;

                static Modes()
                {
                    SpellsMenu = Menu.AddSubMenu("SPELLS");
                    Combo.Initialize();
                    Harass.Initialize();

                    FarmMenu = Menu.AddSubMenu("FARM");
                    LaneClear.Initialize();
                    LastHit.Initialize();

                    MiscMenu = Menu.AddSubMenu("MISC");
                    Misc.Initialize();

                    DrawMenu = Menu.AddSubMenu("DRAWINGS");
                    Draw.Initialize();
                }

                public static void Initialize()
                {
                }

                public static class Combo
                {
                    private static readonly CheckBox _useQ;
                    private static readonly CheckBox _useW;
                    private static readonly CheckBox _useE;
                    private static readonly CheckBox _useR;

                    public static bool UseQ
                    {
                        get { return _useQ.CurrentValue; }
                    }

                    public static bool UseW
                    {
                        get { return _useW.CurrentValue; }
                    }

                    public static bool UseE
                    {
                        get { return _useE.CurrentValue; }
                    }

                    public static bool UseR
                    {
                        get { return _useR.CurrentValue; }
                    }

                    static Combo()
                    {
                        // Initialize the menu values
                        SpellsMenu.AddGroupLabel("Kombo Büyüleri:");
                        _useQ = SpellsMenu.Add("comboQ", new CheckBox("Komboda Q Kullan"));
                        _useW = SpellsMenu.Add("comboW", new CheckBox("Komboda W Kullan"));
                        _useE = SpellsMenu.Add("comboE", new CheckBox("Komboda E Kullan"));
                        _useR = SpellsMenu.Add("comboR", new CheckBox("Komboda R Kullan"));
                    }

                    public static void Initialize()
                    {
                    }
                }

                public static class Harass
                {
                    private static readonly CheckBox _useQ;
                    private static readonly CheckBox _useW;
                    private static readonly CheckBox _useE;
                    private static readonly Slider _manaHarass;

                    public static bool UseQ
                    {
                        get { return _useQ.CurrentValue; }
                    }

                    public static bool UseW
                    {
                        get { return _useW.CurrentValue; }
                    }

                    public static bool UseE
                    {
                        get { return _useE.CurrentValue; }
                    }

                    public static int ManaHarass
                    {
                        get { return _manaHarass.CurrentValue; }
                    }

                    static Harass()
                    {
                        SpellsMenu.AddGroupLabel("Dürtme Ayarları:");
                        _useQ = SpellsMenu.Add("harassQ", new CheckBox("Dürtmede Q Kullan"));
                        _useW = SpellsMenu.Add("harassW", new CheckBox("Dürtmede W Kullan"));
                        _useE = SpellsMenu.Add("harassE", new CheckBox("Dürtmede E Kullan"));
                        SpellsMenu.AddGroupLabel("Dürtme Ayarları:");
                        _manaHarass = SpellsMenu.Add("harassMana", new Slider("Dürtme modu için en az mana", 50));
                    }

                    public static void Initialize()
                    {
                    }
                }

                public static class LaneClear
                {
                    private static readonly CheckBox _useQ;
                    private static readonly CheckBox _useW;
                    private static readonly Slider _laneMana;
                    private static readonly Slider _xCount;

                    public static bool UseQ
                    {
                        get { return _useQ.CurrentValue; }
                    }

                    public static bool UseW
                    {
                        get { return _useW.CurrentValue; }
                    }

                    public static int LaneMana
                    {
                        get { return _laneMana.CurrentValue; }
                    }

                    public static int XCount
                    {
                        get { return _xCount.CurrentValue; }
                    }

                    static LaneClear()
                    {
                        FarmMenu.AddGroupLabel("LaneTemizleme Büyüleri:");
                        _useQ = FarmMenu.Add("laneclearQ", new CheckBox("LaneTemizlemede Q Kullan"));
                        _useW = FarmMenu.Add("laneclearW", new CheckBox("LaneTemizlemede W Kullan"));
                        FarmMenu.AddGroupLabel("LaneTemizleme Ayarları:");
                        _laneMana = FarmMenu.Add("laneMana", new Slider("Temizleme için gereken mana", 80));
                        _xCount = FarmMenu.Add("xCount", new Slider("en az minyon", 3, 1, 6));
                    }

                    public static void Initialize()
                    {
                    }
                }

                public static class LastHit
                {
                    private static readonly CheckBox _useW;
                    private static readonly Slider _lastMana;

                    public static bool UseW
                    {
                        get { return _useW.CurrentValue; }
                    }

                    public static int LastMana
                    {
                        get { return _lastMana.CurrentValue; }
                    }

                    static LastHit()
                    {
                        FarmMenu.AddGroupLabel("Son Vuruuş Büyüleri:");
                        _useW = FarmMenu.Add("lasthitW", new CheckBox("SonVuruşta W Kullan"));
                        FarmMenu.AddGroupLabel("Son Vuruş Ayarları:");
                        _lastMana = FarmMenu.Add("lastMana", new Slider("Son Vuruş İçin En az mana", 90));
                    }

                    public static void Initialize()
                    {
                    }
                }

                public static class Misc
                {
                    private static readonly CheckBox _interruptSpell;
                    private static readonly CheckBox _antiGapCloserSpell;

                    public static bool InterruptSpell
                    {
                        get { return _interruptSpell.CurrentValue; }
                    }

                    public static bool AntiGapCloser
                    {
                        get { return _antiGapCloserSpell.CurrentValue; }
                    }

                    static Misc()
                    {
                        // Initialize the menu values
                        MiscMenu.AddGroupLabel("Ek Ayarlar");
                        _interruptSpell = MiscMenu.Add("interruptX", new CheckBox("interrupt Büyüleri için E Kullan"));
                        _antiGapCloserSpell = MiscMenu.Add("gapcloserX", new CheckBox("antigapcloser Büyüleri için E Kullan"));
                    }

                    public static void Initialize()
                    {
                    }
                }

                public static class Draw
                {
                    private static readonly CheckBox _drawReady;
                    private static readonly CheckBox _drawHealth;
                    private static readonly CheckBox _drawPercent;
                    private static readonly CheckBox _drawQ;
                    private static readonly CheckBox _drawW;
                    private static readonly CheckBox _drawE;
                    private static readonly CheckBox _drawR;
                    //Color Config
                    private static readonly ColorConfig _qColor;
                    private static readonly ColorConfig _wColor;
                    private static readonly ColorConfig _eColor;
                    private static readonly ColorConfig _rColor;
                    private static readonly ColorConfig _healthColor;

                    //CheckBoxes
                    public static bool DrawReady
                    {
                        get { return _drawReady.CurrentValue; }
                    }

                    public static bool DrawHealth
                    {
                        get { return _drawHealth.CurrentValue; }
                    }

                    public static bool DrawPercent
                    {
                        get { return _drawPercent.CurrentValue; }
                    }

                    public static bool DrawQ
                    {
                        get { return _drawQ.CurrentValue; }
                    }

                    public static bool DrawW
                    {
                        get { return _drawW.CurrentValue; }
                    }

                    public static bool DrawE
                    {
                        get { return _drawE.CurrentValue; }
                    }

                    public static bool DrawR
                    {
                        get { return _drawR.CurrentValue; }
                    }
                    //Colors
                    public static Color HealthColor
                    {
                        get { return _healthColor.GetSystemColor(); }
                    }

                    public static SharpDX.Color QColor
                    {
                        get { return _qColor.GetSharpColor(); }
                    }

                    public static SharpDX.Color WColor
                    {
                        get { return _wColor.GetSharpColor(); }
                    }

                    public static SharpDX.Color EColor
                    {
                        get { return _eColor.GetSharpColor(); }
                    }
                    public static SharpDX.Color RColor
                    {
                        get { return _rColor.GetSharpColor(); }
                    }

                    static Draw()
                    {
                        DrawMenu.AddGroupLabel("Büyü Göstergesi :");
                        _drawReady = DrawMenu.Add("drawOnlyWhenReady", new CheckBox("Büyü sadece hazırsa göster ?"));
                        _drawHealth = DrawMenu.Add("damageIndicatorDraw", new CheckBox("Hasar tespiti göster ?"));
                        _drawPercent = DrawMenu.Add("percentageIndicatorDraw", new CheckBox("Hasarı Yüzde olarak göster ?"));
                        DrawMenu.AddSeparator(1);
                        _drawQ = DrawMenu.Add("qDraw", new CheckBox("Göster Q Menzili ?"));
                        _drawW = DrawMenu.Add("wDraw", new CheckBox("Göster W Menzili ?"));
                        _drawE = DrawMenu.Add("eDraw", new CheckBox("Göster E Menzili ?"));
                        _drawR = DrawMenu.Add("rDraw", new CheckBox("Göster R Menzili ?"));

                        _healthColor = new ColorConfig(DrawMenu, "healthColorConfig", Color.Orange, "Renkli Hasar Tespiti:");
                        _qColor = new ColorConfig(DrawMenu, "qColorConfig", Color.Blue, "Renk Q:");
                        _wColor = new ColorConfig(DrawMenu, "wColorConfig", Color.Red, "Renk W:");
                        _eColor = new ColorConfig(DrawMenu, "eColorConfig", Color.DeepPink, "Renk E:");
                        _rColor = new ColorConfig(DrawMenu, "rColorConfig", Color.Yellow, "Renk R:");
                    }

                    public static void Initialize()
                    {
                    }
                }
            }
        }

        public static class ModeManager
        {
            private static List<ModeBase> Modes { get; set; }

            static ModeManager()
            {
                Modes = new List<ModeBase>();

                Modes.AddRange(new ModeBase[]
            {
                new PermaActive(),
                new Combo(),
                new Harass(),
                new LaneClear(),
                new JungleClear(),
                new LastHit(),
                new Flee()
            });

                Game.OnTick += OnTick;
            }

            public static void Initialize()
            {
            }

            private static void OnTick(EventArgs args)
            {
                Modes.ForEach(mode =>
                {
                    try
                    {
                        if (mode.ShouldBeExecuted())
                        {
                            mode.Execute();
                        }
                    }
                    catch (Exception e)
                    {
                       // Logger.Log(LogLevel.Error, "Error in mode '{0}'\n{1}", mode.GetType().Name, e);
                    }
                });
            }
        }

        internal static class EventsManager
        {
            public static void Initialize()
            {
                Gapcloser.OnGapcloser += Gapcloser_OnGapcloser;
                Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
            }

            private static void Gapcloser_OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
            {
                if (!sender.IsEnemy) return;

                if (SettingsMisc.InterruptSpell && sender.IsValidTarget(SpellManager.E.Range))
                {
                    SpellManager.E.Cast(sender);
                }
            }

            private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs e)
            {
                if (!sender.IsEnemy) return;

                if (SettingsMisc.AntiGapCloser && e.DangerLevel == DangerLevel.High)
                {
                    SpellManager.E.Cast(sender);
                }
            }
        }
        #endregion Champion

        #region Modes
        public sealed class Combo : ModeBase
        {
            public override bool ShouldBeExecuted()
            {
                return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
            }

            public override void Execute()
            {
                var target = TargetSelector.GetTarget(W.Range, DamageType.Magical);
                if (target == null) return;
                var qtarget = TargetSelector.GetTarget(600, DamageType.Magical);
                if (qtarget == null)
                    return;

                if (E.IsReady() && SettingsCombo.UseE && target.IsValidTarget())
                {
                    var pred = E.GetPrediction(target);
                    if (pred.HitChance >= HitChance.High)
                    {
                        E.Cast(target);
                    }
                }

                if (W.IsReady() && SettingsCombo.UseW && W.IsInRange(target))
                {
                    var pred = W.GetPrediction(target);
                    if (pred.HitChance >= HitChance.Medium)
                    {
                        W.Cast(target);
                    }
                }

                if (Q.IsReady() && qtarget.IsValidTarget(650) && SettingsCombo.UseQ)
                {
                    Q.Cast(Player.Instance.Position.Extend(target.Position, +300).To3D());
                }

                if (R.IsReady() && SettingsCombo.UseR)
                {
                    R.Cast();
                }
            }
        }

        public sealed class Flee : ModeBase
        {
            public override bool ShouldBeExecuted()
            {
                return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee);
            }

            public override void Execute()
            {
                var target = TargetSelector.GetTarget(E.Range, DamageType.Magical);
                if (target.IsValidTarget(E.Range))
                {
                    var pred = E.GetPrediction(target);
                    if (pred.HitChance >= HitChance.Medium)
                    {
                        E.Cast(pred.CastPosition);
                    }
                }
            }
        }

        public sealed class Harass : ModeBase
        {
            public override bool ShouldBeExecuted()
            {
                return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
            }

            public override void Execute()
            {
                var target = TargetSelector.GetTarget(W.Range, DamageType.Magical);
                if (target == null) return;
                var qtarget = TargetSelector.GetTarget(600, DamageType.Magical);
                if (qtarget == null)
                    return;

                if (E.IsReady() && SettingsHarass.UseE && target.IsValidTarget() &&
                SettingsHarass.ManaHarass <= Player.Instance.ManaPercent)
                {
                    var pred = E.GetPrediction(target);
                    if (pred.HitChance >= HitChance.High)
                    {
                        E.Cast(target);
                    }
                }

                if (W.IsReady() && SettingsHarass.UseW && W.IsInRange(target) &&
                SettingsHarass.ManaHarass <= Player.Instance.ManaPercent)
                {
                    var pred = W.GetPrediction(target);
                    if (pred.HitChance >= HitChance.Medium)
                    {
                        W.Cast(target);
                    }
                }

                if (Q.IsReady() && qtarget.IsValidTarget(650) && SettingsHarass.UseQ &&
                SettingsHarass.ManaHarass <= Player.Instance.ManaPercent)
                {
                    Q.Cast(Player.Instance.Position.Extend(target.Position, +300).To3D());
                }

            }
        }

        public sealed class JungleClear : ModeBase
        {
            public override bool ShouldBeExecuted()
            {
                return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear);
            }

            public override void Execute()
            {
                var jgminion =
                    EntityManager.MinionsAndMonsters.GetJungleMonsters()
                        .OrderByDescending(j => j.Health)
                        .FirstOrDefault(j => j.IsValidTarget(Q.Range));

                if (jgminion == null) return;

                if (W.IsReady() && SettingsLaneClear.UseW && SettingsLaneClear.LaneMana <= Player.Instance.ManaPercent)
                {
                    W.Cast(Player.Instance.Position.Extend(jgminion.Position, W.Range).To3D());
                }

            }
        }

        public sealed class LaneClear : ModeBase
        {
            public override bool ShouldBeExecuted()
            {
                return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear);
            }

            public override void Execute()
            {
                var minions =
                    EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                        Player.Instance.ServerPosition, W.Range, false).ToArray();
                if (minions.Length == 0) return;

                var farmLocation = EntityManager.MinionsAndMonsters.GetLineFarmLocation(minions, Q.Width, (int)Q.Range);
                if (farmLocation.HitNumber >= SettingsLaneClear.XCount && SettingsLaneClear.UseQ && SettingsLaneClear.LaneMana <= Player.Instance.ManaPercent)
                {
                    Q.Cast(farmLocation.CastPosition);
                }
                if (farmLocation.HitNumber >= SettingsLaneClear.XCount && SettingsLaneClear.UseQ && SettingsLaneClear.LaneMana <= Player.Instance.ManaPercent)
                {
                    W.Cast(farmLocation.CastPosition);
                }
            }
        }

        public sealed class LastHit : ModeBase
        {
            public override bool ShouldBeExecuted()
            {
                return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit);
            }

            public override void Execute()
            {
                var minion =
                    EntityManager.MinionsAndMonsters.GetLaneMinions()
                        .OrderByDescending(m => m.Health)
                        .FirstOrDefault(m => m.IsValidTarget(Q.Range));

                if (minion == null) return;

                if (W.IsReady() && SettingsLastHit.UseW && SettingsLastHit.LastMana <= Player.Instance.ManaPercent)
                {
                    W.Cast(Player.Instance.Position.Extend(minion.Position, W.Range).To3D());
                }

            }
        }

        public sealed class PermaActive : ModeBase
        {
            public override bool ShouldBeExecuted()
            {
                return true;
            }

            public override void Execute()
            {

            }
        }

        public abstract class ModeBase
        {
            protected static Spell.Skillshot Q
            {
                get { return SpellManager.Q; }
            }
            protected static Spell.Skillshot W
            {
                get { return SpellManager.W; }
            }
            protected static Spell.Skillshot E
            {
                get { return SpellManager.E; }
            }
            protected static Spell.Active R
            {
                get { return SpellManager.R; }
            }

            public abstract bool ShouldBeExecuted();

            public abstract void Execute();
        }

        #endregion Modes

    }
}
