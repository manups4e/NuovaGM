using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using static CitizenFX.Core.Native.API;
using NuovaGM.Client.MenuNativo;

using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Shared;

namespace NuovaGM.Client.Lavori.Generici.Rimozione
{
	public class RimozioneClient
	{
		static Towing Rimozione;
		static Vehicle VeicoloLavorativo;
		static Vehicle VeicoloDaRimuovere;
		static Blip BlipVeicoloDaRimuovere;
		static Blip PuntoDiConsegna;
		static Vector4 puntoDiSpawn;
		static Vector3 puntoDiDespawn;

		public static void Init()
		{
			Rimozione = Client.Impostazioni.Lavori.Generici.Rimozione;
			RequestAnimDict("oddjobs@towing");
			Client.Printa(LogType.Debug, Newtonsoft.Json.JsonConvert.SerializeObject(Rimozione));
			Client.Instance.AddTick(InizioLavoro);

			//IsVehicleAttachedToTowTruck(int towtruck, int vehicle);
			//GetEntityAttachedToTowTruck(int towtruck);
			//SetVehicleSiren(int towtruck, bool toggle); // quando stai agganciando / trasportando
			//DetachVehicleFromTowTruck
			//animazioni
			//"oddjobs@towingcome_here", "come_here_idle_a"
			//"oddjobs@towing", "Start_Engine_Loop"
			//"oddjobs@towing", "Start_Engine_Exit"
			//"oddjobs@towingpleadingidle_b", "idle_d"

			/*
			func_130(Local_2996[0].f_6, &uLocal_3042, &uLocal_3044);
			SET_FORCE_HD_VEHICLE(Local_2996[0].f_6, 1);
			SET_VEHICLE_TYRES_CAN_BURST(Local_2996[0].f_6, 0);
			SET_ENTITY_LOAD_COLLISION_FLAG(Local_2996[0].f_6, 1, 1);
			SET_VEHICLE_HAS_STRONG_AXLES(Local_2996[0].f_6, 1);
			return 1;
			*/

			// cercare "hint" per le telecamere.. fiiiiiigo

		}


		public static async Task InizioLavoro()
		{
			if (Game.Player.GetPlayerData().CurrentChar.job.name != "Rimozione forzata")
			{
				if (World.GetDistance(Game.PlayerPed.Position, Rimozione.InizioLavoro.ToVector3()) < 50)
					World.DrawMarker(MarkerType.TruckSymbol, Rimozione.InizioLavoro.ToVector3(), new Vector3(0), new Vector3(0), new Vector3(2.5f, 2.5f, 2.5f), Colors.Brown, true, false, true);
				if (World.GetDistance(Game.PlayerPed.Position, Rimozione.InizioLavoro.ToVector3()) < 1.375)
				{
					HUD.ShowHelp("Vuoi lavorare nel magico mondo del ~y~soccorso stradale~w~?\nPremi ~INPUT_CONTEXT~ per accettare un contratto lavorativo!");
					if (Input.IsControlJustPressed(Control.Context))
					{
						Game.Player.GetPlayerData().CurrentChar.job.name = "Rimozione forzata";
						Game.Player.GetPlayerData().CurrentChar.job.grade = 0;
						VeicoloLavorativo = await Funzioni.SpawnVehicle("towtruck2", new Vector3(401.55f, -1631.309f, 29.3f), 140);
						//VeicoloLavorativo.SetDecor("VeicoloLavorativo", );
						VeicoloLavorativo.PlaceOnGround();
						VeicoloLavorativo.PreviouslyOwnedByPlayer = true;
						VeicoloLavorativo.Repair();
						Client.Instance.AddTick(LavoroRimozioneForzata);
						Client.Instance.AddTick(ControlloRimozione);
						Client.Instance.RemoveTick(InizioLavoro);

					}
				}
			}
			await Task.FromResult(0);
		}

		public static async Task ControlloRimozione()
		{
			if (VeicoloLavorativo != null)
			{
				if (Game.PlayerPed.LastVehicle == VeicoloLavorativo)
				{
					if (World.GetDistance(Game.PlayerPed.Position, VeicoloLavorativo.Position) > 60)
						HUD.ShowNotification("~r~Attenzione~w~!! Ti stai allontanando troppo dal tuo veicolo lavorativo!!", true);
					if (World.GetDistance(Game.PlayerPed.Position, VeicoloLavorativo.Position) > 100)
					{
						HUD.ShowNotification("Ti sei allontanato troppo dal tuo veicolo, il veicolo è stato riportato in azienda e hai perso il lavoro!", NotificationColor.Red, true);
						Game.Player.GetPlayerData().CurrentChar.job.name = "Disoccupato";
						Game.Player.GetPlayerData().CurrentChar.job.grade = 0;
						VeicoloLavorativo.Delete();
						VeicoloLavorativo = null;
						Client.Instance.RemoveTick(LavoroRimozioneForzata);
						Client.Instance.RemoveTick(ControlloRimozione);
						Client.Instance.AddTick(InizioLavoro);
					}
				}
			}
		}



		public static async Task LavoroRimozioneForzata()
		{
			await BaseScript.Delay(new Random().Next(60000));
			puntoDiSpawn = Rimozione.SpawnVeicoli[Funzioni.GetRandomInt(Rimozione.SpawnVeicoli.Count - 1)].ToVector4();

			if (VeicoloDaRimuovere == null)
			{
				uint streename = 0;
				uint crossing = 0;
				GetStreetNameAtCoord(puntoDiSpawn.X, puntoDiSpawn.Y, puntoDiSpawn.Z, ref streename, ref crossing);
				string str = GetStreetNameFromHashKey(streename);
				string veicolo = Rimozione.VeicoliDaRimorchiare[Funzioni.GetRandomInt(Rimozione.VeicoliDaRimorchiare.Count - 1)];
				RequestCollisionAtCoord(puntoDiSpawn.X, puntoDiSpawn.Y, puntoDiSpawn.Z);
				VeicoloDaRimuovere = await Funzioni.SpawnVehicleNoPlayerInside(veicolo, new Vector3(puntoDiSpawn.X, puntoDiSpawn.Y, puntoDiSpawn.Z), puntoDiSpawn.W);
				VeicoloDaRimuovere.PlaceOnGround();
				VeicoloDaRimuovere.PreviouslyOwnedByPlayer = true;
				VeicoloDaRimuovere.Repair();
				VeicoloDaRimuovere.LockStatus = VehicleLockStatus.Locked;
				HUD.ShowAdvancedNotification("Veicolo", "Da rimuovere", $"Veicolo da rimuovere in {str}", "CHAR_CALL911", IconType.DollarIcon);
			}
			await Task.FromResult(0);
		}

		/*
		puntoDiSpawn = Rimozione.SpawnVeicoli[Funzioni.GetRandomInt(Rimozione.SpawnVeicoli.Count - 1)].ToVector4();
		while (Funzioni.GetVehiclesInArea(new Vector3(puntoDiSpawn.X, puntoDiSpawn.Y, puntoDiSpawn.Z), 2.5f).ToList().FirstOrDefault(x => x.HasDecor("VeicoloRimozione")) != null)
		puntoDiSpawn = Rimozione.SpawnVeicoli[Funzioni.GetRandomInt(Rimozione.SpawnVeicoli.Count - 1)].ToVector4();
		Client.Printa(LogType.Debug, "Punto di Spawn = " + puntoDiSpawn.ToString());
		*/
		/*
		VeicoloDaRimuovere = await Funzioni.SpawnVehicleNoPlayerInside(Rimozione.VeicoliDaRimorchiare[Funzioni.GetRandomInt(Rimozione.VeicoliDaRimorchiare.Count - 1)], new Vector3(puntoDiSpawn.X, puntoDiSpawn.Y, puntoDiSpawn.Z), puntoDiSpawn.W);
		Client.Printa(LogType.Debug, "Targa = " + VeicoloDaRimuovere.Mods.LicensePlate);
		VeicoloDaRimuovere.LockStatus = VehicleLockStatus.CannotBeTriedToEnter;
		VeicoloDaRimuovere.IsPersistent = true;
		VeicoloDaRimuovere.PlaceOnGround();
		VeicoloDaRimuovere.SetDecor("VeicoloRimozione", VeicoloDaRimuovere.Handle);
		*/




	}
}
 