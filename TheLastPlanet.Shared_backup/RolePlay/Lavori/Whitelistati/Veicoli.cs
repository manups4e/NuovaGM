using CitizenFX.Core;
using TheLastPlanet.Shared.Internal.Events.Attributes;

namespace TheLastPlanet.Shared
{
	public class VeicoloLavorativoEAffitto
	{
		public Vehicle veicolo { get; set; }
		public string Proprietario{ get; set; }

		public VeicoloLavorativoEAffitto(Vehicle veh, string proprietario)
		{
			veicolo = veh;
			Proprietario = proprietario;
		}
	}

	public class VeicoloPol
	{
		public string Plate { get; set; }
		public int Model { get; set; }
		public int Handle { get; set; }

		public VeicoloPol(string plate, int model, int handle)
		{
			Plate = plate;
			Model = model;
			Handle = handle;
		}
	}
}