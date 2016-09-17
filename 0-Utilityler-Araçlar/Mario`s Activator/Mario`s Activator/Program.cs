using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using Version = System.Version;

namespace Mario_s_Activator
{
    internal class Program
    {
        #region VersionChecker
        public static bool VersionChecked { get; internal set; }
        public static bool ActivatorLoaded { get; internal set; }

        private static Version LocalVersion => Assembly.GetExecutingAssembly().GetName().Version;

        private static string VersionUrl => "https://raw.githubusercontent.com/mariogk/ItsMeMario/master/Mario%60s%20Activator/Activator.version";


        private static void DoWithResponse(WebRequest request, Action<HttpWebResponse> responseAction)
        {
            Action wrapperAction = () =>
            {
                request.BeginGetResponse(iar =>
                {
                    var response = (HttpWebResponse)((HttpWebRequest)iar.AsyncState).EndGetResponse(iar);
                    responseAction(response);
                }, request);
            };
            wrapperAction.BeginInvoke(iar =>
            {
                var action = (Action)iar.AsyncState;
                action.EndInvoke(iar);
            }, wrapperAction);
        }

        public static void CheckVersion()
        {
            DoWithResponse(WebRequest.Create(VersionUrl), response =>
            {
                var stream = response.GetResponseStream();
                if (stream != default(Stream))
                {
                    var internetVersion = new Version(new StreamReader(stream).ReadToEnd());
                    if (internetVersion != LocalVersion)
                    {
                        Chat.Print("New version found("+internetVersion+") of Mario`s Activator please update it.(To load it anyway type /load)", Color.DarkCyan);
                    }
                    else
                    {
                        VersionChecked = true;
                    }
                }
                else
                {
                    Chat.Print("An error happened while trying to check your version, try again.");
                }
            });
        }
        #endregion VersionChecker
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            ActivatorLoaded = false;
            CheckVersion();
            Game.OnTick += Game_OnTick;
            Chat.OnInput += Chat_OnInput;
        }

        private static void Chat_OnInput(ChatInputEventArgs args)
        {
            if (args.Input.Contains("/load"))
            {
                args.Process = false;
                Core.DelayAction(() => Chat.Print("Loading now", Color.BlueViolet), 200);
                Core.DelayAction(() => VersionChecked = true, 400);
            }
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (VersionChecked)
            {
                if (!ActivatorLoaded)
                {
                    Activator.Init();
                    ActivatorLoaded = true;
                }
                if (ActivatorLoaded)
                {
                    Game.OnTick -= Game_OnTick;
                }
            }
        }
    }
}
