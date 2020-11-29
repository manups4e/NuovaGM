using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Server.Veicoli
{
	static class BagagliaioServer
	{
		public static ConcurrentDictionary<string, string> BagagliaiGenerici = new ConcurrentDictionary<string, string>();
		public static void Init()
		{
		//	Server.Instance.AddEventHandler("lprp:bagagliaio:getTrunksContents", new Action<Player, string>(GestisciBagagliaio));
		}

	}
}
