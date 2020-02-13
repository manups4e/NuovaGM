using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.Lavori.Whitelistati.Medici;
using NuovaGM.Client.Lavori.Whitelistati.Polizia;
using System;
using System.Threading.Tasks;

namespace NuovaGM.Client.Lavori.Whitelistati
{
	static class DutyManager
	{
		private static bool Polizia = false;
		private static bool Medici = false;
		public static void Init()
		{
			Client.GetInstance.RegisterEventHandler("lprp:onPlayerSpawn", new Action(Spawnato));
		}

		private static async void Spawnato()
		{
			Client.GetInstance.RegisterTickHandler(ControlloLavori);
		}

		public static async Task ControlloLavori()
		{
		}
	}
}
