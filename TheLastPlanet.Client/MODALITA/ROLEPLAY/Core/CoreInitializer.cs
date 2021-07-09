using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.RolePlay.Core.LogIn;

namespace TheLastPlanet.Client.RolePlay.Core
{
	static class CoreInitializer
	{
		public static async Task LogInInitializer()
		{
			RolePlay.LogIn.LogIn.Init();
			Main.Init();
			Eventi.Init();
			TimeWeather.Meteo.Init();
			TimeWeather.Orario.Init();
			await Task.FromResult(0);
		}

		public static async Task LogInStop()
		{
			RolePlay.LogIn.LogIn.Stop();
			Main.Stop();
			//Eventi.Stop();
			TimeWeather.Meteo.Stop();
			TimeWeather.Orario.Stop();
			await Task.FromResult(0);
		}
	}
}
