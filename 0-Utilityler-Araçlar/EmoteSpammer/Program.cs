using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace EmoteSpammer
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Loading.OnLoadingComplete += delegate
            {
                // Initialize menu
                var menu = MainMenu.AddMenu("EmoteSpammer", "emoteSpammer", "EmoteSpammer - Huuuuuuuu");

                menu.AddGroupLabel("Bilgi");
                menu.AddLabel("Bu programlama sizin yürürken spam yapmanızı yani arka arkaya bir hareket yapmanızı sağlar.");
                menu.AddLabel("Sen titrek istiyorsan değer göstergesini Arttır");

                menu.AddSeparator();
                menu.AddGroupLabel("Ayarlar");
                var spamKey = new KeyBind("Spam Tuşu", false, KeyBind.BindTypes.HoldActive, 'A', 'U');
                menu.Add("key", spamKey);
                var emoteTypeBox = new ComboBox("Emote Tipi", 3, Emote.Joke.ToString(), Emote.Taunt.ToString(), Emote.Dance.ToString(), Emote.Laugh.ToString());
                menu.Add("type", emoteTypeBox);
                var delaySlider = new Slider("Spam Gecikmesi", 75, 50, 150);
                menu.Add("delay", delaySlider);

                // Helpers
                var lastSpam = 0;

                Game.OnUpdate += delegate
                {
                    // Key active
                    if (spamKey.CurrentValue)
                    {
                        if (Core.GameTickCount - lastSpam >= delaySlider.CurrentValue)
                        {
                            // Update last spam
                            lastSpam = Core.GameTickCount;

                            // Do the spamming
                            Player.DoEmote((Emote) Enum.Parse(typeof (Emote), emoteTypeBox.SelectedText));

                            // Instantly move after
                            Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos, false);
                        }
                    }
                };
            };
        }
    }
}
