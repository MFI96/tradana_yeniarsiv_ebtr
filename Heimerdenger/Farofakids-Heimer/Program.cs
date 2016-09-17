using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK.Events;

namespace Farofakids_Heimer
{
    class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            try
            {
                switch (ObjectManager.Player.Hero)
                {
                    case Champion.Heimerdinger:
                       Heimerdinger.Initialize();
                        break;
                }
            }
            catch (Exception exp)
            {
                Console.Write(exp);
            }
        }
    }
}
