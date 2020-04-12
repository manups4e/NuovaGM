using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuovaGM.Client
{
	static class DecorationClass
	{
		public static async Task DichiaraDecor()
		{
			EntityDecoration.RegisterProperty("NuovaGM2019fighissimo!yeah!", DecorationType.Int);
			EntityDecoration.RegisterProperty("lprp_fuel", DecorationType.Float);
			EntityDecoration.RegisterProperty("VeicoloPolizia", DecorationType.Int);
			EntityDecoration.RegisterProperty("VeicoloMedici", DecorationType.Int);
			EntityDecoration.RegisterProperty("VeicoloRimozione", DecorationType.Int);
			EntityDecoration.RegisterProperty("VeicoloPersonale", DecorationType.Int);
			EntityDecoration.RegisterProperty("PickupOggetto", DecorationType.Int);
			EntityDecoration.RegisterProperty("PickupAccount", DecorationType.Int);
			EntityDecoration.RegisterProperty("PickupArma", DecorationType.Int);
			EntityDecoration.LockProperties();
			await Task.FromResult(0);
		}
	}
}
