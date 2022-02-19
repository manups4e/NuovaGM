using System.Threading.Tasks;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Core
{
    static class CoreInitializer
    {
        public static async Task LogInInitializer()
        {
            LogIn.LogIn.Init();
            Main.Init();
            EventiRoleplay.Init();
            TimeWeather.MeteoClient.Init();
            TimeWeather.OrarioClient.Init();
            await Task.FromResult(0);
        }

        public static async Task LogInStop()
        {
            LogIn.LogIn.Stop();
            //Eventi.Stop();
            TimeWeather.MeteoClient.Stop();
            TimeWeather.OrarioClient.Stop();
            await Task.FromResult(0);
        }
    }
}
