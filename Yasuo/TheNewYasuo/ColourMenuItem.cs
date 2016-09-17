using System.Linq;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;

namespace YasuoBuddy
{
    static class ColourMenuItem
    {
        private static readonly Color[] Colours =
        {
            Color.BlueViolet, Color.Aqua, Color.MidnightBlue, Color.Gold,
            Color.LimeGreen, Color.Violet, Color.WhiteSmoke, Color.DarkRed
        };
        private static readonly string[] ColoursName =
        {
            "BlueViolet/AzulVioleta", "Aqua/Agua", "MidnightBlue/AzulEscuro", "Gold/Ouro", "LimeGreen/VerdeLimao", "Violet/Violeta", "White/Branco", "DarkRed/VermelhoEscuro"
        };

        public static void AddColourItem(this Menu menu, string uniqueId, int defaultColour = 0)
        {
            var a = menu.Add(uniqueId, new Slider("Colour/Cor: ", defaultColour, 0, Colours.Count() - 1));
            a.DisplayName = "Colour/Cor: " + ColoursName[a.CurrentValue];
            a.OnValueChange += delegate { a.DisplayName = "Colour/Cor: " + ColoursName[a.CurrentValue]; };
        }

        public static Color GetColour(this Menu m, string id)
        {
            var number = m[id].Cast<Slider>();
            if (number != null)
            {
                return Colours[number.CurrentValue];
            }
            return Color.AliceBlue;
        }
    }
}