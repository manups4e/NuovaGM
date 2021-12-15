using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Core;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.LogIn;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Core
{
	static class CoreInitializer
	{
		public static async Task LogInInitializer()
		{
			LogIn.LogIn.Init();
			Main.Init();
			EventiRoleplay.Init();
			TimeWeather.Meteo.Init();
			TimeWeather.Orario.Init();
			await Task.FromResult(0);
		}

		public static async Task LogInStop()
		{
			LogIn.LogIn.Stop();
			Main.Stop();
			//Eventi.Stop();
			TimeWeather.Meteo.Stop();
			TimeWeather.Orario.Stop();
			await Task.FromResult(0);
		}
	}
}
