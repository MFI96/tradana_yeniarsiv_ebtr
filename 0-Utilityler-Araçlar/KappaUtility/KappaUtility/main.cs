using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KappaUtility
{
    using EloBuddy.SDK.Events;

    internal class main
    {
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Load.Loading_OnLoadingComplete;
        }
    }
}