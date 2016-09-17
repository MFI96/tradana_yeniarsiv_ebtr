using System;
using System.Drawing;
using EloBuddy;
using EloBuddy.SDK.Events;
using OneForWeek.Draw.Notifications;
using OneForWeek.Model.Notification;
using OneForWeek.Plugin.Hero;
using OneForWeek.Util.Misc;

namespace OneForWeek
{
    class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadCompleted;
        }

        private static void OnLoadCompleted(EventArgs args)
        {
            if (ObjectManager.Player.ChampionName == "Cassiopeia")
            {
                Notification.DrawNotification(new NotificationModel(Game.Time, 20f, 1f, ObjectManager.Player.ChampionName + " injected !", Color.DeepSkyBlue));
                Notification.DrawNotification(new NotificationModel(Game.Time, 20f, 1f, "Addon by: Mr Articuno", Color.Purple));
                new Cassiopeia().Init();
                Igniter.Init();
            }
            else
            {
                Notification.DrawNotification(new NotificationModel(Game.Time, 20f, 1f, ObjectManager.Player.ChampionName + " is Not Supported", Color.Red));
            }
        }
    }
}
