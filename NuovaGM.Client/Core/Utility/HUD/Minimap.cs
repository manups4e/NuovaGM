using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;
using TheLastPlanet.Client.Personale;
using CitizenFX.Core.UI;

namespace TheLastPlanet.Client.Core.Utility.HUD
{
	internal static class Minimap
	{
		//public static Scaleform minimap = new Scaleform("MINIMAP");
		public static void Init() { Client.Instance.AddTick(MinimapDrawing); }

		public static async Task MinimapDrawing()
		{
			Ped p = Cache.Cache.MyPlayer.Ped;

			// SE NON STO NASCONDENDO L'HUD (cinematica)
			if (!Main.ImpostazioniClient.ModCinema)
			{
				if (Main.ImpostazioniClient.MiniMappaAttiva)
				{
					if (!IsRadarEnabled()) Screen.Hud.IsRadarVisible = true;

					switch (Main.ImpostazioniClient.DimensioniMinimappa)
					{
						// se ho settato la minimappa piccina
						case 0:
						{
							if (IsBigmapActive())              // se attualmente la minimappa è ingrandita
								SetBigmapActive(false, false); // riduciamola

							break;
						}
						// altrimenti
						case 1:
						{
							if (!IsBigmapActive())            // se è piccina
								SetBigmapActive(true, false); // ingrandiscila

							break;
						}
					}

					switch (Cache.Cache.MyPlayer.Character.StatiPlayer.InVeicolo)
					{
						//se non sono su un veicolo e non ho il menu di pausa attivo.
						case false when !IsPauseMenuActive():
							DisableRadarThisFrame(); // lascia la minimappa attiva, ma nasconda la mappa se non sono in un veicolo

							break;
						case true when Main.ImpostazioniClient.MiniMappaInAuto:
						{
							Vehicle veh = p.CurrentVehicle;

							if (veh == null) return;
							if (veh.Model.IsBicycle || IsThisModelAJetski((uint)veh.Model.Hash) || veh.Model.IsQuadbike || !veh.IsEngineRunning) DisableRadarThisFrame();

							break;
						}
						case true:
							DisableRadarThisFrame();

							break;
					}
				}
				else
				{
					if (IsRadarEnabled()) Screen.Hud.IsRadarVisible = false;
				}
			}
			else
			{
				if (IsRadarEnabled()) Screen.Hud.IsRadarVisible = false;
			}

			await Task.FromResult(0);
		}
	}
}