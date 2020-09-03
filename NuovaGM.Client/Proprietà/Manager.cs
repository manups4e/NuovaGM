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
			Log.Printa(LogType.Debug, JsonConvert.SerializeObject(Proprietà.Garages));
			Log.Printa(LogType.Debug, JsonConvert.SerializeObject(Proprietà.Appartamenti));
			Client.Instance.AddTick(MarkerBlipHandler);
		}

		public static async Task MarkerBlipHandler()
		{
			foreach (var app in Proprietà.Appartamenti.LowEnd) 
			{
				if (Game.PlayerPed.IsInRangeOf(app.Value.MarkerEntrata, 1.375f))
				{
					HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per ~y~entrare o citofonare~w~.");
					if (Input.IsControlJustPressed(Control.Context) && !HUD.MenuPool.IsAnyMenuOpen())
						AppartamentiMain.EntraMenu(app); // da fare e agg. controllo se è casa mia o no per il menu
				}

				if (Game.PlayerPed.IsInRangeOf(app.Value.MarkerUscita, 1.375f))
				{
					HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per ~y~uscire~w~.");
					if (Input.IsControlJustPressed(Control.Context) && !HUD.MenuPool.IsAnyMenuOpen())
						AppartamentiMain.EsciMenu(app); // da fare e agg. controllo se è casa mia o no per il menu e per il garage
				}

				if (Game.PlayerPed.IsInRangeOf(app.Value.GarageMarker, 3f))
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
		}
	}
}
