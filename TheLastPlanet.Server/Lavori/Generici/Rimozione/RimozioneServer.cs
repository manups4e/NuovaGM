using System;
using System.Threading.Tasks;

namespace TheLastPlanet.Server.Lavori.Generici.Rimozione
{
	static class RimozioneServer
	{
		public static void Init()
		{
			ServerSession.Instance.AddTick(AggiornaVeicoli);
			ServerSession.Instance.AddEventHandler("lprp:AggiornaVeicoliRimossi", new Action<dynamic>(AggiornaVeicoliRimossi));
		}

		private static void AggiornaVeicoliRimossi(dynamic data)
		{

		}

		public static async Task AggiornaVeicoli()
		{
		}
	}
}
