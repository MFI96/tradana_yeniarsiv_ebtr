using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;

namespace AddonTemplate
{
    class GameEvent
    {
        public static void OnDraw(EventArgs args)
        {
            if ((Config.Modes.Draw.DrawW && !Config.Modes.Draw.OnlyRdy) || (Config.Modes.Draw.OnlyRdy && !SpellManager.W.IsOnCooldown && Config.Modes.Draw.DrawW && SpellManager.W.IsLearned))
                Circle.Draw(Color.BlueViolet, SpellManager.W.Range, Player.Instance.Position);
            if ((Config.Modes.Draw.DrawE && !Config.Modes.Draw.OnlyRdy) || (Config.Modes.Draw.OnlyRdy && !SpellManager.E.IsOnCooldown && Config.Modes.Draw.DrawE && SpellManager.E.IsLearned))
                Circle.Draw(Color.Yellow, SpellManager.E.Range, Player.Instance.Position);
        }

        public static void SkinHax_OnValueChanged(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
        {
            if (Config.Modes.Draw.UseHax)
            {
                Config.Modes.Draw._skinhax.DisplayName =
                    Config.Modes.Draw.skinName[Config.Modes.Draw._skinhax.CurrentValue];
                Player.Instance.SetSkin(Player.Instance.ChampionName, args.NewValue);
            }
        }

        public static void UseHax_OnValueChanged(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
        {
            if (!sender.CurrentValue)
            {
                Player.Instance.SetSkin(Player.Instance.ChampionName, Program.SkinBase);
            }
            else
            {
                Player.Instance.SetSkin(Player.Instance.ChampionName, Config.Modes.Draw._skinhax.CurrentValue);
            }
        }

        public static void StealthRecall_OnValueChanged(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
        {
            if (args.NewValue && SpellManager.Q.IsReady())
            {
                SpellManager.Q.Cast();
                SpellManager.Recall.Cast();
                Config.Modes.Misc._stealthRecall.CurrentValue = false;
            }
        }
    }
}
