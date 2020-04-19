﻿using System;
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
		static UITimerBarItem timerVeicolo = new UITimerBarItem("Veicolo da rimorchiare");
		static int TempoRimozione;
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


			Blip Rim = World.CreateBlip(Rimozione.InizioLavoro.ToVector3());
			Rim.Sprite = BlipSprite.TowTruck;
			Rim.Name = "Soccorso Stradale";

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
						Client.Printa(LogType.Debug, "valore = " + GetEntityAttachedToTowTruck(VeicoloLavorativo.Handle));
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
			await BaseScript.Delay(/*new Random().Next(60000)*/10000);
			puntoDiSpawn = Rimozione.SpawnVeicoli[Funzioni.GetRandomInt(Rimozione.SpawnVeicoli.Count - 1)].ToVector4();
			while (Funzioni.GetVehiclesInArea(new Vector3(puntoDiSpawn.X, puntoDiSpawn.Y, puntoDiSpawn.Z), 3f).ToList().FirstOrDefault(x => x.HasDecor("VeicoloRimozione")) != null)
			{
				await BaseScript.Delay(0);
				puntoDiSpawn = Rimozione.SpawnVeicoli[Funzioni.GetRandomInt(Rimozione.SpawnVeicoli.Count - 1)].ToVector4();
			}

			Client.Printa(LogType.Debug, "Punto di Spawn = " + puntoDiSpawn.ToString());
			if (VeicoloDaRimuovere == null)
			{
				uint streename = 0;
				uint crossing = 0;
				GetStreetNameAtCoord(puntoDiSpawn.X, puntoDiSpawn.Y, puntoDiSpawn.Z, ref streename, ref crossing);
				string str = GetStreetNameFromHashKey(streename);
				string veicolo = Rimozione.VeicoliDaRimorchiare[Funzioni.GetRandomInt(Rimozione.VeicoliDaRimorchiare.Count - 1)];
				RequestCollisionAtCoord(puntoDiSpawn.X, puntoDiSpawn.Y, puntoDiSpawn.Z);
				HUD.ShowAdvancedNotification("Veicolo", "Da rimuovere", $"Veicolo da rimuovere in {str}", "CHAR_CALL911", IconType.DollarIcon);
				if (World.GetDistance(new Vector3(puntoDiSpawn.X, puntoDiSpawn.Y, puntoDiSpawn.Z), Game.PlayerPed.Position) < 1000)
					TempoRimozione = Funzioni.GetRandomInt(60, 120);
				else
					TempoRimozione = Funzioni.GetRandomInt(120, 240);
				HUD.TimerBarPool.Add(timerVeicolo);
				Client.Instance.AddTick(TimerVeicolo);
				while (World.GetDistance(Game.PlayerPed.Position, new Vector3(puntoDiSpawn.X, puntoDiSpawn.Y, puntoDiSpawn.Z)) > 100 && TempoRimozione > 0) await BaseScript.Delay(0);
				if (TempoRimozione > 0)
				{
					VeicoloDaRimuovere = await Funzioni.SpawnVehicleNoPlayerInside(veicolo, new Vector3(puntoDiSpawn.X, puntoDiSpawn.Y, puntoDiSpawn.Z), puntoDiSpawn.W);
					VeicoloDaRimuovere.IsPersistent = true;
					VeicoloDaRimuovere.PlaceOnGround();
					VeicoloDaRimuovere.PreviouslyOwnedByPlayer = true;
					VeicoloDaRimuovere.Repair();
					VeicoloDaRimuovere.LockStatus = VehicleLockStatus.Locked;
					VeicoloDaRimuovere.SetDecor("VeicoloRimozione", VeicoloDaRimuovere.Handle);
					BlipVeicoloDaRimuovere = VeicoloDaRimuovere.AttachBlip();
					BlipVeicoloDaRimuovere.Sprite = BlipSprite.TowTruck;
					BlipVeicoloDaRimuovere.Name = "Veicolo da Rimorchiare";
					HUD.ShowAdvancedNotification("Veicolo", "Da rimuovere", $"Il veicolo da rimuovere e' un modello {VeicoloDaRimuovere.LocalizedName} con targa {VeicoloDaRimuovere.Mods.LicensePlate}", "CHAR_CALL911", IconType.DollarIcon);
				}
				while (World.GetDistance(Game.PlayerPed.Position, VeicoloDaRimuovere.Position) > 20 && TempoRimozione > 0) await BaseScript.Delay(0);
				if (GetEntityAttachedToTowTruck(VeicoloLavorativo.Handle) == 0)
					HUD.ShowNotification("~INPUT_VEH_MOVE_UD~ per controllare il gancio.\n~INPUT_VEH_ROOF~ (tieni premuto) per sgangiare il veicolo");
				while (GetEntityAttachedToTowTruck(VeicoloLavorativo.Handle) != VeicoloDaRimuovere.Handle) await BaseScript.Delay(0);
				PuntoDiConsegna = World.CreateBlip(Rimozione.PuntiDespawn[Funzioni.GetRandomInt(Rimozione.PuntiDespawn.Count - 1)].ToVector3());
				PuntoDiConsegna.ShowRoute = true;
				while (World.GetDistance(VeicoloDaRimuovere.Position, PuntoDiConsegna.Position) < 4) await BaseScript.Delay(0);
				HUD.ShowNotification("Premi ~INPUT_CONTEXT~ per depositare il veicolo");
				if (Input.IsControlJustPressed(Control.Context))
				{
					VeicoloDaRimuovere.Delete();
					VeicoloDaRimuovere = null;
					BlipVeicoloDaRimuovere.Delete();
					BlipVeicoloDaRimuovere = null;
					PuntoDiConsegna.Delete();
					PuntoDiConsegna = null;
				}
			}
			await Task.FromResult(0);
		}

		private static async Task TimerVeicolo()
		{
			while (TempoRimozione >= 0)
			{
				timerVeicolo.TextTimerBar.Caption = TempoRimozione > 59 ? $"{(int)Math.Floor(TempoRimozione / 60f)}:{TempoRimozione - (int)Math.Floor(TempoRimozione / 60f) * 60}" : $"{TempoRimozione}";
				await BaseScript.Delay(1000);
				TempoRimozione--;
				if (GetEntityAttachedToTowTruck(VeicoloLavorativo.Handle) != 0 && GetEntityAttachedToTowTruck(VeicoloLavorativo.Handle) == VeicoloDaRimuovere.Handle)
				{
					HUD.TimerBarPool.Remove(timerVeicolo);
					Client.Instance.RemoveTick(TimerVeicolo);
					break; 
				}
			}
			if (TempoRimozione == 0 && GetEntityAttachedToTowTruck(VeicoloLavorativo.Handle) == 0 && World.GetDistance(Game.PlayerPed.Position, VeicoloDaRimuovere.Position) > 50)
			{
				HUD.ShowNotification("Il veicolo da rimuovere se n'è andato!!", NotificationColor.Red, true);
				VeicoloDaRimuovere.Delete();
				VeicoloDaRimuovere = null;
				BlipVeicoloDaRimuovere.Delete();
				BlipVeicoloDaRimuovere = null;
				Client.Instance.RemoveTick(TimerVeicolo);
			}
			else Client.Instance.RemoveTick(TimerVeicolo);
		}
	}
}
 