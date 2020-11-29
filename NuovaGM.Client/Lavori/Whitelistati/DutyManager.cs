using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Lavori.Whitelistati.Medici;
using TheLastPlanet.Client.Lavori.Whitelistati.Polizia;
using System;
using System.Threading.Tasks;

namespace TheLastPlanet.Client.Lavori.Whitelistati
{
	static class DutyManager
	{
		private static bool Polizia = false;
		private static bool Medici = false;
		public static void Init()
		{
//			Client.Instance.AddEventHandler("lprp:onPlayerSpawn", new Action(Spawnato));
		}
	}
}
