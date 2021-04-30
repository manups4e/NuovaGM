using System.Collections.Concurrent;

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
