using CitizenFX.Core;
using TheLastPlanet.Server.Core;
using System;

namespace TheLastPlanet.Server.manager
{
	static class ServerManager
	{
		public static void Init()
		{
			Server.Instance.AddEventHandler("lprp:manager:TeletrasportaDaMe", new Action<int, Vector3>(TippaDaMe));
		}

		private static void TippaDaMe(int source, Vector3 coords)
		{
			Funzioni.GetPlayerFromId(source).TriggerEvent("lprp:manager:TeletrasportaDaMe", coords);
		}
	}
}