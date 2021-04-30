using System;
using TheLastPlanet.Client.Core.PlayerChar;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;

namespace TheLastPlanet.Client.RolePlay.Lavori
{
	static class JobPanelManager
	{
		public static void Init()
		{
			Client.Instance.Eventi.Mount("lprp:job:employee:hired", new Action<int, string, string> ((seed, job, emp) =>
			{
				User user = Funzioni.GetPlayerCharFromServerId(seed);
				HUD.ShowNotification($"Sei stato assunto come {emp}!");
				Enum.TryParse<Employment>(job, out var employment);
			}));
		}
	}
}
