using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using Logger;
using TheLastPlanet.Client.Core.PlayerChar;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Shared.SistemaEventi;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.Lavori
{
	static class JobPanelManager
	{
		public static void Init()
		{
			ClientSession.Instance.SistemaEventi.Attach("lprp:job:employee:hired", new AsyncEventCallback(async meta =>
			{
				var seed = meta.Find<int>(0);
				User user = Funzioni.GetPlayerCharFromServerId(seed);
				HUD.ShowNotification($"Sei stato assunto come {meta.Find<string>(2)}!");
				Enum.TryParse<Employment>(meta.Find<string>(1), out var employment);
				return null;
			}));
		}
	}
}
