using KappAIO.Common;
using KappAIO.Common.KappaEvade;

namespace KappAIO.Utility
{
    internal class Load
    {
        public static void Init()
        {
            KappaEvade.Init();
            Activator.Load.Init();
            Events.Init();
            //Tracker.Ganks.Init();
        }
    }
}
