using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using Newtonsoft.Json;
using NuovaGM.Shared;
using Logger;
using NuovaGM.Client.Proprietà.Appartamenti.Case;

namespace NuovaGM.Client.Proprietà
{
	static class Manager
	{
		private static ConfigProprieta Proprietà;
		public static void Init()
		{
			Proprietà = Client.Impostazioni.Proprieta;
		}

		public static async Task MarkerFuori()
		{
			foreach (var app in Proprietà.Appartamenti)
			{
				if (Game.PlayerPed.IsInRangeOf(app.Value.MarkerEntrata, 1.375f))
				{
					HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per ~y~entrare o citofonare~w~.");
					if (Input.IsControlJustPressed(Control.Context) && !HUD.MenuPool.IsAnyMenuOpen)
						AppartamentiClient.EntraMenu(app); // da fare e agg. controllo se è casa mia o no per il menu
				}
				if (Game.PlayerPed.IsInRangeOf(app.Value.MarkerGarageEsterno, 3f))
				{
					if (Game.PlayerPed.IsInVehicle())
					{
						// controllo se il veicolo è mio
						//CambiaCamGarage() // ed entra
						//else errore non è il tuo veicolo
					}
					else
					{
						// codice per entrare a piedi
					}
				}
			}
			foreach (var gar in Proprietà.Garages.Garages)
			{
				if(Game.PlayerPed.IsInRangeOf(gar.Value.SpawnDentro, 1.5f))
				{
					if (Game.PlayerPed.IsOnFoot)
					{
						// ENTRARE NEI GARAGES
					}
				}
			}
		}

		public static async Task MarkerDentro()
		{
			if (Game.Player.GetPlayerData().Istanza.Stanziato)
			{
				if (Proprietà.Appartamenti.ContainsKey(Game.Player.GetPlayerData().Istanza.Instance))
				{
					var app = Proprietà.Appartamenti[Game.Player.GetPlayerData().Istanza.Instance];
					if (Game.PlayerPed.IsInRangeOf(app.MarkerUscita, 1.375f))
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per ~y~uscire~w~.");
						if (Input.IsControlJustPressed(Control.Context) && !HUD.MenuPool.IsAnyMenuOpen)
							AppartamentiClient.EsciMenu(app);
					}
					if (Game.PlayerPed.IsInRangeOf(app.MarkerGarageInterno, 1.375f))
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per ~y~uscire~w~.");
						if (Input.IsControlJustPressed(Control.Context) && !HUD.MenuPool.IsAnyMenuOpen)
							AppartamentiClient.EsciMenu(app, inGarage: true);
					}
				}
			}
		}


	}
}
