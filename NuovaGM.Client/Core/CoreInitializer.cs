using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.Core
{
	static class CoreInitializer
	{
		public static async Task LogInInitializer()
		{
			Main.Init();
			Ingresso.LogIn.Init();
			Utility.HUD.HUD.Init();
			Utility.Eventi.Init();
			Discord.Init();
			TimeWeather.Meteo.Init();
			TimeWeather.Orario.Init();
			await Task.FromResult(0);
		}

	}
}
