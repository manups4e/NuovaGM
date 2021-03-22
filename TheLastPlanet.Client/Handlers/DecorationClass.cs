using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheLastPlanet.Client.SessionCache;

namespace TheLastPlanet.Client
{
	internal static class DecorationClass
	{
		public static async void DichiaraDecor()
		{
			/* DECOR GENERICI */
			EntityDecoration.RegisterProperty("NuovaGM2019fighissimo!yeah!", DecorationType.Int);
			EntityDecoration.RegisterProperty("Testdecor", DecorationType.Int);
			/* DECOR VEICOLI */
			EntityDecoration.RegisterProperty("lprp_fuel", DecorationType.Float);
			EntityDecoration.RegisterProperty("VeicoloPolizia", DecorationType.Int);
			EntityDecoration.RegisterProperty("VeicoloMedici", DecorationType.Int);
			EntityDecoration.RegisterProperty("VeicoloRimozione", DecorationType.Int);
			EntityDecoration.RegisterProperty("VeicoloPersonale", DecorationType.Int);
			/* DECOR PICKUP */
			EntityDecoration.RegisterProperty("PickupOggetto", DecorationType.Int);
			EntityDecoration.RegisterProperty("PickupAccount", DecorationType.Int);
			EntityDecoration.RegisterProperty("PickupArma", DecorationType.Int);
			/* DECOR LOCK */
			EntityDecoration.LockProperties();

			await Cache.Loaded();
			ClientSession.Instance.AddTick(GestionePlayersDecors.GestioneDecors);
			await Task.FromResult(0);
		}
	}
}