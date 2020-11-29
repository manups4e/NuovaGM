using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Server.Lavori.Generici.Rimozione
{
	static class RimozioneServer
	{
		public static void Init()
		{
			Server.Instance.AddTick(AggiornaVeicoli);
			Server.Instance.AddEventHandler("lprp:AggiornaVeicoliRimossi", new Action<dynamic>(AggiornaVeicoliRimossi));
		}

		private static void AggiornaVeicoliRimossi(dynamic data)
		{

		}

		public static async Task AggiornaVeicoli()
		{
		}
	}
}
