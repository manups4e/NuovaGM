﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using TheLastPlanet.Client.Core;

namespace TheLastPlanet.Client
{
	static class DecorationClass
	{
		public static async Task DichiaraDecor()
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
			Client.Instance.AddTick(GestionePlayersDecors.GestioneDecors);
			await Task.FromResult(0);
		}
	}
}
