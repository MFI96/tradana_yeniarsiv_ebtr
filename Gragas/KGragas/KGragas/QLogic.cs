using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace KGragas
{
    public sealed class QLogic
    {
        public static bool CastedQ;
        public static bool ShouldBeExecuted()
        {
            return true;
        }

        public static void Execute()
        {
            if (CastedQ)
            {
                if (Player.Instance.Spellbook.GetSpell(SpellSlot.Q).ToggleState == 2 || Player.Instance.Spellbook.GetSpell(SpellSlot.Q).ToggleState == 1)
                {
                    Program.Q.Cast(Player.Instance);
                    CastedQ = false;
                }
                else
                {
                    CastedQ = false;
                }
            }
        }
    }





}

