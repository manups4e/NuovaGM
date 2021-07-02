using CitizenFX.Core;
using TheLastPlanet.Shared.Internal.Events.Attributes;

namespace TheLastPlanet.Shared
{
	[Serialization]
	public partial class VeicoloLavorativoEAffitto
	{
		public Vehicle veicolo;
		public string Proprietario;

		public VeicoloLavorativoEAffitto(Vehicle veh, string proprietario)
		{
			veicolo = veh;
			Proprietario = proprietario;
		}
	}

	[Serialization]
	public partial class VeicoloPol
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