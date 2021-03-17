using CitizenFX.Core;

namespace TheLastPlanet.Shared
{
	public class VeicoloLavorativoEAffitto
	{
		public Vehicle veicolo;
		public string Proprietario;

		public VeicoloLavorativoEAffitto(Vehicle veh, string proprietario)
		{
			veicolo = veh;
			Proprietario = proprietario;
		}
	}

	public class VeicoloPol
	{
		public string Plate;
		public int Model;
		public int Handle;

		public VeicoloPol(string plate, int model, int handle)
		{
			Plate = plate;
			Model = model;
			Handle = handle;
		}
	}
}
