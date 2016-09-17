namespace KappaUtility.Items
{
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;

    using Common;

    internal class AutoQSS
    {
        public static Spell.Active Cleanse;

        protected static readonly Item Mercurial_Scimitar = new Item(ItemId.Mercurial_Scimitar);

        protected static readonly Item Quicksilver_Sash = new Item(ItemId.Quicksilver_Sash);

        public static readonly Item Dervish_Blade = new Item(ItemId.Dervish_Blade);

        public static readonly Item Mikaels_Crucible = new Item(ItemId.Mikaels_Crucible);

        protected static bool loaded = false;

        public static Menu QssMenu { get; private set; }

        internal static void OnLoad()
        {
            QssMenu = Load.UtliMenu.AddSubMenu("AutoQSS");
            QssMenu.AddGroupLabel("AutoQSS Ayarları");
            QssMenu.Checkbox("enable", "Aktif");
            QssMenu.Checkbox("Mercurial", "Cıva Yatağan Kullan");
            QssMenu.Checkbox("Quicksilver", "Kullan Cıvalı Kuşak");
            QssMenu.Checkbox("Dervish_Blade", "Derviş Kılıcı Kullan");
            QssMenu.Checkbox("Mikaels_Crucible", "Mikael'in Kazanı Kullan");
            if (Player.Spells.FirstOrDefault(o => o.SData.Name.Contains("SummonerBoost")) != null)
            {
                QssMenu.Checkbox("Cleanse", "Arındırıcı Büyüler Kullan");
                Cleanse = new Spell.Active(Player.Instance.GetSpellSlotFromName("SummonerBoost"));
            }

            QssMenu.AddSeparator();
            QssMenu.AddGroupLabel("QSS Kurtarıcı:");
            QssMenu.Checkbox("blind", "Körse?");
            QssMenu.Checkbox("charm", "Çekciliğe bürünmüşse(ahri)?");
            QssMenu.Checkbox("disarm", "Etkisizhale gelmişse?");
            QssMenu.Checkbox("fear", "Korkmuşsa?");
            QssMenu.Checkbox("frenzy", "Donmuşsa?");
            QssMenu.Checkbox("silence", "Susturulmuşsa?");
            QssMenu.Checkbox("snare", "Tuzağa Düşmüşse?");
            QssMenu.Checkbox("sleep", "Uyutulmuşsa?");
            QssMenu.Checkbox("stun", "Sabitlenmişse?");
            QssMenu.Checkbox("supperss", "Önlenmişse?");
            QssMenu.Checkbox("slow", "Yavaşlatılmışsa?");
            QssMenu.Checkbox("knockup", "Havaya Kaldırılmışsa?");
            QssMenu.Checkbox("knockback", "Use On Knock Backs?");
            QssMenu.Checkbox("nearsight", "Yakındaysa(tehlike durumunda?");
            QssMenu.Checkbox("root", "Use On Roots?");
            QssMenu.Checkbox("tunt", "Alay Ediliyorsa?");
            QssMenu.Checkbox("poly", "Use On Polymorph?");
            QssMenu.Checkbox("poison", "Zehirlenmişse?");

            QssMenu.AddSeparator();
            QssMenu.AddGroupLabel("Ults Ayarları::");
            QssMenu.Checkbox("liss", "Lissandra Ult?");
            QssMenu.Checkbox("naut", "Nautilus Ult?");
            QssMenu.Checkbox("zed", "Zed Ult?");
            QssMenu.Checkbox("vlad", "Vlad Ult?");
            QssMenu.Checkbox("fizz", "Fizz Ult?");
            QssMenu.Checkbox("fiora", "Fiora Ult?");
            QssMenu.AddSeparator();
            QssMenu.Slider("hp", "Sadece canım şunun altında ise [{0}%]", 30);
            QssMenu.Slider("human", "İnsancıl Gecikme [{0}]", 150, 0, 1500);
            QssMenu.Slider("Rene", "[{0}] Daire Ykaınındaki düşmanlar", 1, 0, 5);
            QssMenu.Slider("enemydetect", "Düşmanları tespit etme mesafesi [{0}]", 1000, 0, 2000);
            loaded = true;

            Obj_AI_Base.OnBuffGain += OnBuffGain;
        }

        private static void OnBuffGain(Obj_AI_Base sender, Obj_AI_BaseBuffGainEventArgs args)
        {
            if (!loaded)
            {
                return;
            }

            if (QssMenu.GetCheckbox("enable"))
            {
                if (sender.IsMe)
                {
                    var debuff = (QssMenu.GetCheckbox("charm") && (args.Buff.Type == BuffType.Charm || Player.Instance.HasBuffOfType(BuffType.Charm)))
                                 || (QssMenu.GetCheckbox("tunt")
                                     && (args.Buff.Type == BuffType.Taunt || Player.Instance.HasBuffOfType(BuffType.Taunt)))
                                 || (QssMenu.GetCheckbox("stun") && (args.Buff.Type == BuffType.Stun || Player.Instance.HasBuffOfType(BuffType.Stun)))
                                 || (QssMenu.GetCheckbox("fear") && (args.Buff.Type == BuffType.Fear || Player.Instance.HasBuffOfType(BuffType.Fear)))
                                 || (QssMenu.GetCheckbox("silence")
                                     && (args.Buff.Type == BuffType.Silence || Player.Instance.HasBuffOfType(BuffType.Silence)))
                                 || (QssMenu.GetCheckbox("snare")
                                     && (args.Buff.Type == BuffType.Snare || Player.Instance.HasBuffOfType(BuffType.Snare)))
                                 || (QssMenu.GetCheckbox("supperss")
                                     && (args.Buff.Type == BuffType.Suppression || Player.Instance.HasBuffOfType(BuffType.Suppression)))
                                 || (QssMenu.GetCheckbox("sleep")
                                     && (args.Buff.Type == BuffType.Sleep || Player.Instance.HasBuffOfType(BuffType.Sleep)))
                                 || (QssMenu.GetCheckbox("poly")
                                     && (args.Buff.Type == BuffType.Polymorph || Player.Instance.HasBuffOfType(BuffType.Polymorph)))
                                 || (QssMenu.GetCheckbox("frenzy")
                                     && (args.Buff.Type == BuffType.Frenzy || Player.Instance.HasBuffOfType(BuffType.Frenzy)))
                                 || (QssMenu.GetCheckbox("disarm")
                                     && (args.Buff.Type == BuffType.Disarm || Player.Instance.HasBuffOfType(BuffType.Disarm)))
                                 || (QssMenu.GetCheckbox("nearsight")
                                     && (args.Buff.Type == BuffType.NearSight || Player.Instance.HasBuffOfType(BuffType.NearSight)))
                                 || (QssMenu.GetCheckbox("knockback")
                                     && (args.Buff.Type == BuffType.Knockback || Player.Instance.HasBuffOfType(BuffType.Knockback)))
                                 || (QssMenu.GetCheckbox("knockup")
                                     && (args.Buff.Type == BuffType.Knockup || Player.Instance.HasBuffOfType(BuffType.Knockup)))
                                 || (QssMenu.GetCheckbox("slow") && (args.Buff.Type == BuffType.Slow || Player.Instance.HasBuffOfType(BuffType.Slow)))
                                 || (QssMenu.GetCheckbox("poison")
                                     && (args.Buff.Type == BuffType.Poison || Player.Instance.HasBuffOfType(BuffType.Poison)))
                                 || (QssMenu.GetCheckbox("blind")
                                     && (args.Buff.Type == BuffType.Blind || Player.Instance.HasBuffOfType(BuffType.Blind)))
                                 || (QssMenu.GetCheckbox("zed") && args.Buff.Name == "zedrtargetmark")
                                 || (QssMenu.GetCheckbox("vlad") && args.Buff.Name == "vladimirhemoplaguedebuff")
                                 || (QssMenu.GetCheckbox("liss") && args.Buff.Name == "LissandraREnemy2")
                                 || (QssMenu.GetCheckbox("fizz") && args.Buff.Name == "fizzmarinerdoombomb")
                                 || (QssMenu.GetCheckbox("naut") && args.Buff.Name == "nautilusgrandlinetarget")
                                 || (QssMenu.GetCheckbox("fiora") && args.Buff.Name == "fiorarmark");
                    var enemys = QssMenu.GetSlider("Rene");
                    var hp = QssMenu.GetSlider("hp");
                    var enemysrange = QssMenu.GetSlider("enemydetect");
                    var countenemies = Helpers.CountEnemies(enemysrange);
                    var delay = QssMenu.GetSlider("human");
                    if (debuff && Player.Instance.HealthPercent <= hp && countenemies >= enemys)
                    {
                        Core.DelayAction(QssCast, delay);
                    }
                }
            }
        }

        public static void QssCast()
        {
            if (Quicksilver_Sash.IsOwned() && Quicksilver_Sash.IsReady() && QssMenu.GetCheckbox("Quicksilver"))
            {
                Quicksilver_Sash.Cast();
            }

            if (Mercurial_Scimitar.IsOwned() && Mercurial_Scimitar.IsReady() && QssMenu.GetCheckbox("Mercurial"))
            {
                Mercurial_Scimitar.Cast();
            }

            if (Dervish_Blade.IsOwned() && Dervish_Blade.IsReady() && QssMenu.GetCheckbox("Dervish_Blade"))
            {
                Dervish_Blade.Cast();
            }

            if (Cleanse != null)
            {
                if (QssMenu.GetCheckbox("Cleanse") && Cleanse.IsReady())
                {
                    Cleanse.Cast();
                }
            }
        }
    }
}