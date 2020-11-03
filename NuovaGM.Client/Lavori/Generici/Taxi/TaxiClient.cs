using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Client.MenuNativo;
using NuovaGM.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuovaGM.Client.Veicoli;

namespace NuovaGM.Client.Lavori.Generici.Taxi
{
	static class TaxiClient
	{
		private static Tassisti taxi;
		private static Vehicle VeicoloServizio;
		private static List<Ped> Passeggeri;
		public static void Init()
		{
			taxi = Client.Impostazioni.Lavori.Generici.Tassista;
			// provvisorio
			Client.Instance.AddTick(Markers);
		}

		public static async Task Markers()
		{
			if(Game.PlayerPed.IsInRangeOf(taxi.PosAccettazione, 100))
			{
				World.DrawMarker(MarkerType.ChevronUpx2, taxi.PosAccettazione, Vector3.Zero, Vector3.Zero, new Vector3(1.5f), Colors.Yellow, rotateY: true);
				if (Game.PlayerPed.IsInRangeOf(taxi.PosAccettazione, 1.375f))
				{
					if(Game.Player.GetPlayerData().CurrentChar.job.name.ToLower() != "taxi")
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per accettare il lavoro da tassista.");
						if (Input.IsControlJustPressed(Control.Context)) 
						{
							// entra in servizio.. setta il lavoro.. insomma ci siamo capiti
						}
					}
					else
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per smettere di lavorare.");
						if (Input.IsControlJustPressed(Control.Context))
						{
							// esci dal servizio.. torna senza lavoro.. insomma ci siamo capiti
						}
					}
				}
			}
			if (Game.Player.GetPlayerData().CurrentChar.job.name.ToLower() == "taxi")
			{
				if (Game.PlayerPed.IsInRangeOf(taxi.PosRitiroVeicolo, 100))
				{
					if (VeicoloServizio == null || (VeicoloServizio != null && !VeicoloServizio.Exists() || VeicoloServizio.IsDead))
					{
						World.DrawMarker(MarkerType.CarSymbol, taxi.PosRitiroVeicolo, Vector3.Zero, Vector3.Zero, new Vector3(1.5f), Colors.Yellow, rotateY: true);
						if (Game.PlayerPed.IsInRangeOf(taxi.PosRitiroVeicolo, 1.375f))
						{
							HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per prendere il tuo veicolo di servizio.");
							if (Input.IsControlJustPressed(Control.Context))
							{
								if (Game.PlayerPed.IsVisible)
									NetworkFadeOutEntity(Game.PlayerPed.Handle, true, false);
								Screen.Fading.FadeOut(800);
								await BaseScript.Delay(1000);
								VeicoloServizio = await Funzioni.SpawnVehicle("taxi", taxi.PosSpawnVeicolo.ToVector3(), taxi.PosSpawnVeicolo.W);
								VeicoloServizio.SetVehicleFuelLevel(100f);
								VeicoloServizio.Mods.LicensePlate = "TAXI_" + Funzioni.GetRandomInt(000, 999).ToString("000");
								VeicoloServizio.IsEngineRunning = true;
								VeicoloServizio.IsDriveable = true;
								NetworkFadeInEntity(PlayerPedId(), true);
								Screen.Fading.FadeIn(500);
							}
						}
					}
				}
				if (Game.PlayerPed.IsInRangeOf(taxi.PosDepositoVeicolo, 100))
				{
					if (VeicoloServizio != null && !VeicoloServizio.IsDead && VeicoloServizio.Exists())
					{
						World.DrawMarker(MarkerType.CarSymbol, taxi.PosDepositoVeicolo, Vector3.Zero, Vector3.Zero, new Vector3(1.5f), Colors.Red, rotateY: true);
						if (Game.PlayerPed.IsInRangeOf(taxi.PosDepositoVeicolo, 1.375f))
						{
							HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per parcheggiare il veicolo di servizio.");
							if (Input.IsControlJustPressed(Control.Context))
							{
								if (Game.PlayerPed.CurrentVehicle.IsVisible)
									NetworkFadeOutEntity(Game.PlayerPed.CurrentVehicle.Handle, true, false);
								Screen.Fading.FadeOut(800);
								await BaseScript.Delay(1000);
								VeicoloServizio.Delete();
								VeicoloServizio = null;
								Game.PlayerPed.Position = taxi.PosRitiroVeicolo;
								NetworkFadeInEntity(PlayerPedId(), true);
								Screen.Fading.FadeIn(500);
							}
						}
					}
				}
			}
		}
	}
}
