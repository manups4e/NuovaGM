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
	static class Minimap
	{
		//public static Scaleform minimap = new Scaleform("MINIMAP");
		public static void Init()
		{
			Client.Instance.AddTick(MinimapDrawing);
		}

		public static async Task MinimapDrawing()
		{
			Ped p = Game.PlayerPed;
			// SE NON STO NASCONDENDO L'HUD (cinematica)
			if (!EventiPersonalMenu.DoHideHud)
			{
				if (Main.ImpostazioniClient.MiniMappaAttiva)
				{
					if (!IsRadarEnabled())
						Screen.Hud.IsRadarVisible = true;

					if (Main.ImpostazioniClient.DimensioniMinimappa == 0) // se ho settato la minimappa piccina
					{
						if (IsBigmapActive()) // se attualmente la minimappa è ingrandita
						{
							SetBigmapActive(false, false); // riduciamola
						}
					}
					else if (Main.ImpostazioniClient.DimensioniMinimappa == 1) // altrimenti
					{
						if (!IsBigmapActive()) // se è piccina
							SetBigmapActive(true, false); // ingrandiscila
					}

					//se non sono su un veicolo e non ho il menu di pausa attivo.
					if (!p.IsInVehicle() && (!IsPauseMenuActive()))
						DisableRadarThisFrame(); // lascia la minimappa attiva, ma nasconda la mappa se non sono in un veicolo
					if (p.IsInVehicle())
					{
						if (Main.ImpostazioniClient.MiniMappaInAuto)
						{
							Vehicle veh = p.CurrentVehicle;
							if (veh.Model.IsBicycle || IsThisModelAJetski((uint)veh.Model.Hash) || veh.Model.IsQuadbike || !veh.IsEngineRunning)
								DisableRadarThisFrame();
						}
						else
							DisableRadarThisFrame();
					}
				}
				else
				{
					if(IsRadarEnabled())
						Screen.Hud.IsRadarVisible = false;
				}
			}

			await Task.FromResult(0);
		}
	}
}
