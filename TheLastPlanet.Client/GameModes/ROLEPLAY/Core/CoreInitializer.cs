using System.Threading.Tasks;

namespace TheLastPlanet.Client.GameMode.ROLEPLAY.Core
{
    static class CoreInitializer
    {
        public static async Task LogInInitializer()
        {
            LogIn.LogIn.Init();
            Main.Init();
            RoleplayEvents.Init();
            TimeWeather.WeatherClient.Init();
            TimeWeather.TimeClient.Init();
            await Task.FromResult(0);
        }

        public static async Task LogInStop()
        {
            LogIn.LogIn.Stop();
            //Eventi.Stop();
            TimeWeather.WeatherClient.Stop();
            TimeWeather.TimeClient.Stop();
            await Task.FromResult(0);
        }
    }
}
