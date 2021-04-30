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
			LogIn.LogIn.Init();
			Main.Init();
			HUD.Init();
			Eventi.Init();
			Discord.Init();
			TimeWeather.Meteo.Init();
			TimeWeather.Orario.Init();
			await Task.FromResult(0);
		}

	}
}
