using System;
using System.Threading.Tasks;

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
