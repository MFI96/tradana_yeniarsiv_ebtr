using EloBuddy.SDK.Events;

namespace PandaTeemoReborn
{
    /// <summary>
    /// PandaTeemoReborn is the first addon I have made based off Hellsing's and Mario's Template.
    /// </summary>
    internal class Program
    {
        private static void Main()
        {
            Loading.OnLoadingComplete += EventManager.Loading_OnLoadingComplete;
        }
    }
}
