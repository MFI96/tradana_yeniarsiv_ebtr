using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;

namespace Volibear
{
    class Program
    {      
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Volibear.Loading_OnLoadingComplete;
        }
    }
}
