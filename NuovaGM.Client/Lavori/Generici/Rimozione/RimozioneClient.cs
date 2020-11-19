using CitizenFX.Core;
using CitizenFX.Core.UI;
using Logger;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Client.MenuNativo;
using NuovaGM.Client.Veicoli;
using NuovaGM.Shared;
using System;
using System.Linq;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;


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
		static UITimerBarItem timerVeicolo = new UITimerBarItem("Veicolo da rimorchiare");
		static int TempoRimozione;
		static bool distwarn = false;

		public static void Init()
		{
			Rimozione = Client.Impostazioni.Lavori.Generici.Rimozione;
			//RequestAnimDict("oddjobs@towing");

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


			Blip Rim = World.CreateBlip(Rimozione.InizioLavoro);
			Rim.Sprite = BlipSprite.TowTruck;
			Rim.Name = "Soccorso Stradale";
			Rim.IsShortRange = true;
			SetBlipDisplay(Rim.Handle, 4);
		}


		public static async Task InizioLavoro()
		{
			Ped p = Game.PlayerPed;
			if (Game.Player.GetPlayerData().CurrentChar.job.name != "Rimozione forzata")
			{
				if (p.IsInRangeOf(Rimozione.InizioLavoro, 50))
					World.DrawMarker(MarkerType.TruckSymbol, Rimozione.InizioLavoro, new Vector3(0), new Vector3(0), new Vector3(2.5f, 2.5f, 2.5f), Colors.Brown, true, false, true);
				if (p.IsInRangeOf(Rimozione.InizioLavoro, 1.375f))
				{
					HUD.ShowHelp("Vuoi lavorare nel magico mondo del ~y~soccorso stradale~w~?\nPremi ~INPUT_CONTEXT~ per accettare un contratto lavorativo!");
					if (Input.IsControlJustPressed(Control.Context))
					{
						Screen.Fading.FadeOut(800);
						await BaseScript.Delay(1000);
						Game.Player.GetPlayerData().CurrentChar.job.name = "Rimozione forzata";
						Game.Player.GetPlayerData().CurrentChar.job.grade = 0;
						VeicoloLavorativo = await Funzioni.SpawnVehicle("towtruck", new Vector3(401.55f, -1631.309f, 29.3f), 140);
						VeicoloLavorativo.SetDecor("VeicoloLavorativo", VeicoloLavorativo.Handle);
						VeicoloLavorativo.PlaceOnGround();
						VeicoloLavorativo.PreviouslyOwnedByPlayer = true;
						VeicoloLavorativo.Repair();
						VeicoloLavorativo.SetVehicleFuelLevel(100f);
						Log.Printa(LogType.Debug, "valore = " + GetEntityAttachedToTowTruck(VeicoloLavorativo.Handle));
						Client.Instance.AddTick(LavoroRimozioneForzata);
						Client.Instance.AddTick(ControlloRimozione);
						Client.Instance.RemoveTick(InizioLavoro);
						Screen.Fading.FadeIn(800);
					}
				}
			}
			await Task.FromResult(0);
		}

		public static async Task ControlloRimozione()
		{
			if (VeicoloLavorativo != null)
			{
				float dist = Vector3.Distance(Game.Player.GetPlayerData().posizione.ToVector3(), VeicoloLavorativo.Position);
				if (dist > 48 && dist < 80)
					Screen.ShowSubtitle("~r~Attenzione~w~!! Ti stai allontanando troppo dal tuo veicolo lavorativo!!", 1);
				if (dist > 80)
				{
					Client.Instance.RemoveTick(LavoroRimozioneForzata);
					Client.Instance.RemoveTick(ControlloRimozione);
					Client.Instance.AddTick(InizioLavoro);
					if (HUD.TimerBarPool.TimerBars.Contains(timerVeicolo))
						HUD.TimerBarPool.Remove(timerVeicolo);
					HUD.ShowNotification("Ti sei allontanato troppo dal tuo veicolo, il veicolo è stato riportato in azienda e hai perso il lavoro!", NotificationColor.Red, true);
					Game.Player.GetPlayerData().CurrentChar.job.name = "Disoccupato";
					Game.Player.GetPlayerData().CurrentChar.job.grade = 0;
					if (VeicoloLavorativo != null && VeicoloLavorativo.Exists())
					{
						VeicoloLavorativo.Delete();
						VeicoloLavorativo = null;
					}
					if (BlipVeicoloDaRimuovere != null && BlipVeicoloDaRimuovere.Exists())
					{
						BlipVeicoloDaRimuovere.Delete();
						BlipVeicoloDaRimuovere = null;
					}
					if (PuntoDiConsegna != null && PuntoDiConsegna.Exists())
					{
						PuntoDiConsegna.Delete();
						PuntoDiConsegna = null;
					}
					if (VeicoloDaRimuovere != null && VeicoloDaRimuovere.Exists())
					{
						VeicoloDaRimuovere.Delete();
						VeicoloDaRimuovere = null;
					}
				}
			}
		}

		public static async Task LavoroRimozioneForzata()
		{
			if (VeicoloDaRimuovere == null && VeicoloDaRimuovere.Exists())
			{
				await BaseScript.Delay(10000);
				puntoDiSpawn = Rimozione.SpawnVeicoli[Funzioni.GetRandomInt(Rimozione.SpawnVeicoli.Count - 1)];
				while (Funzioni.GetVehiclesInArea(new Vector3(puntoDiSpawn.X, puntoDiSpawn.Y, puntoDiSpawn.Z), 3f).ToList().FirstOrDefault(x => x.HasDecor("VeicoloRimozione")) != null)
				{
					await BaseScript.Delay(0);
					puntoDiSpawn = Rimozione.SpawnVeicoli[Funzioni.GetRandomInt(Rimozione.SpawnVeicoli.Count - 1)];
				}

				// DEBUG
				Log.Printa(LogType.Debug, "Punto di Spawn = " + puntoDiSpawn.ToString());

				uint streename = 0;
				uint crossing = 0;
				GetStreetNameAtCoord(puntoDiSpawn.X, puntoDiSpawn.Y, puntoDiSpawn.Z, ref streename, ref crossing);
				string str = GetStreetNameFromHashKey(streename);
				string veicolo = Rimozione.VeicoliDaRimorchiare[Funzioni.GetRandomInt(Rimozione.VeicoliDaRimorchiare.Count - 1)];
				RequestCollisionAtCoord(puntoDiSpawn.X, puntoDiSpawn.Y, puntoDiSpawn.Z);
				HUD.ShowAdvancedNotification("Veicolo", "Da rimuovere", $"Veicolo da rimuovere in {str}", "CHAR_CALL911", IconType.DollarIcon);
				BlipVeicoloDaRimuovere = World.CreateBlip(new Vector3(puntoDiSpawn.X, puntoDiSpawn.Y, puntoDiSpawn.Z));
				BlipVeicoloDaRimuovere.Sprite = BlipSprite.PersonalVehicleCar;
				BlipVeicoloDaRimuovere.Color = BlipColor.Red;
				BlipVeicoloDaRimuovere.Name = "Veicolo da Rimorchiare";
				if (Vector3.Distance(new Vector3(puntoDiSpawn.X, puntoDiSpawn.Y, puntoDiSpawn.Z), Game.Player.GetPlayerData().posizione.ToVector3()) < 1000)
					TempoRimozione = Funzioni.GetRandomInt(60, 120);
				else
					TempoRimozione = Funzioni.GetRandomInt(120, 240);
				HUD.TimerBarPool.Add(timerVeicolo);
				Client.Instance.AddTick(TimerVeicolo);
				while (Vector3.Distance(Game.Player.GetPlayerData().posizione.ToVector3(), new Vector3(puntoDiSpawn.X, puntoDiSpawn.Y, puntoDiSpawn.Z)) > 200 && TempoRimozione > 0)
				{
					if (VeicoloLavorativo == null) return;
					await BaseScript.Delay(0);
				}
				if (TempoRimozione > 0)
				{
					VeicoloDaRimuovere = await Funzioni.SpawnVehicleNoPlayerInside(veicolo, new Vector3(puntoDiSpawn.X, puntoDiSpawn.Y, puntoDiSpawn.Z), puntoDiSpawn.W);
					while (VeicoloDaRimuovere == null) await BaseScript.Delay(0);
					VeicoloDaRimuovere.IsPersistent = true;
					VeicoloDaRimuovere.PlaceOnGround();
					VeicoloDaRimuovere.PreviouslyOwnedByPlayer = true;
					VeicoloDaRimuovere.Repair();
					VeicoloDaRimuovere.LockStatus = VehicleLockStatus.Locked;
					VeicoloDaRimuovere.SetDecor("VeicoloRimozione", VeicoloDaRimuovere.Handle);
					if (BlipVeicoloDaRimuovere.Exists()) BlipVeicoloDaRimuovere.Delete();
					BlipVeicoloDaRimuovere = VeicoloDaRimuovere.AttachBlip();
					BlipVeicoloDaRimuovere.Sprite = BlipSprite.PersonalVehicleCar;
					BlipVeicoloDaRimuovere.Color = BlipColor.Red;
					BlipVeicoloDaRimuovere.Name = "Veicolo da Rimorchiare";
					HUD.ShowAdvancedNotification("Veicolo", "Da rimuovere", $"Il veicolo da rimuovere e' un modello ~y~{VeicoloDaRimuovere.LocalizedName}~w~ con targa ~y~{VeicoloDaRimuovere.Mods.LicensePlate}~w~", "CHAR_CALL911", IconType.DollarIcon);
				}
			}
			if (VeicoloLavorativo == null) return;
			while (Vector3.Distance(Game.Player.GetPlayerData().posizione.ToVector3(), VeicoloDaRimuovere.Position) > 20 && TempoRimozione > 0 && VeicoloDaRimuovere != null) await BaseScript.Delay(0);
			if (!IsVehicleAttachedToTowTruck(VeicoloLavorativo.Handle, VeicoloLavorativo.Handle) && Game.PlayerPed.IsInRangeOf(VeicoloDaRimuovere.Position, 10))
				HUD.ShowHelp("~INPUT_VEH_MOVE_UD~ per controllare il gancio.\n~INPUT_VEH_ROOF~ (tieni premuto) per sgangiare il veicolo");
			if (GetEntityAttachedToTowTruck(VeicoloLavorativo.Handle) != 0 && GetEntityAttachedToTowTruck(VeicoloLavorativo.Handle) != VeicoloDaRimuovere.Handle)
				HUD.ShowHelp("Hai agganciato il veicolo sbagliato!");
			//while (GetEntityAttachedToTowTruck(VeicoloLavorativo.Handle) != VeicoloDaRimuovere.Handle && Vector3.Distance(VeicoloDaRimuovere.Position, PuntoDiConsegna.Position) > 25) await BaseScript.Delay(0);
			if (GetEntityAttachedToTowTruck(VeicoloLavorativo.Handle) == VeicoloDaRimuovere.Handle)
			{
				if (PuntoDiConsegna == null)
				{
					SetVehicleSiren(VeicoloLavorativo.Handle, true);
					SetForceHdVehicle(VeicoloDaRimuovere.Handle, true);
					VeicoloDaRimuovere.CanTiresBurst = false;
					SetEntityLoadCollisionFlag(VeicoloDaRimuovere.Handle, true);
					VeicoloDaRimuovere.IsAxlesStrong = true;
					PuntoDiConsegna = World.CreateBlip(Rimozione.PuntiDespawn[Funzioni.GetRandomInt(Rimozione.PuntiDespawn.Count - 1)]);
					PuntoDiConsegna.ShowRoute = true;
				}
			}
			else
			{
				if (PuntoDiConsegna != null && PuntoDiConsegna.Exists() && !VeicoloDaRimuovere.IsInRangeOf(PuntoDiConsegna.Position, 25))
				{
					PuntoDiConsegna.Delete();
					PuntoDiConsegna = null;
				}
			}
			if (PuntoDiConsegna != null && VeicoloDaRimuovere.IsInRangeOf(PuntoDiConsegna.Position, 25))
			{
				if (IsVehicleAttachedToTowTruck(VeicoloLavorativo.Handle, VeicoloDaRimuovere.Handle))
					HUD.DrawText3D(PuntoDiConsegna.Position, Colors.WhiteSmoke, "Sgancia qui il veicolo per depositarlo!");
				else
				{
					var money = 200 + VeicoloDaRimuovere.BodyHealth / 10;
					BaseScript.TriggerServerEvent("lprp:givebank", money);
					BlipVeicoloDaRimuovere.Delete();
					BlipVeicoloDaRimuovere = null;
					PuntoDiConsegna.Delete();
					PuntoDiConsegna = null;
					await BaseScript.Delay(2000);
					VeicoloDaRimuovere.Delete();
					VeicoloDaRimuovere = null;
				}
			}
		}

		private static async Task TimerVeicolo()
		{
			while (TempoRimozione > 0)
			{
				string tempo = TempoRimozione > 59 ? TempoRimozione - (int)Math.Floor(TempoRimozione / 60f) * 60 < 10 ? $"{(int)Math.Floor(TempoRimozione / 60f)}:0{TempoRimozione - (int)Math.Floor(TempoRimozione / 60f) * 60}" : $"{(int)Math.Floor(TempoRimozione / 60f)}:{TempoRimozione - (int)Math.Floor(TempoRimozione / 60f) * 60}" : TempoRimozione > 9 ? $"{TempoRimozione}" : $"0{TempoRimozione}";
				timerVeicolo.TextTimerBar.Caption = tempo;
				await BaseScript.Delay(1000);
				TempoRimozione--;
				if (VeicoloLavorativo != null && VeicoloLavorativo.Exists() && GetEntityAttachedToTowTruck(VeicoloLavorativo.Handle) != 0 && GetEntityAttachedToTowTruck(VeicoloLavorativo.Handle) == VeicoloDaRimuovere.Handle)
				{
					HUD.TimerBarPool.Remove(timerVeicolo);
					Client.Instance.RemoveTick(TimerVeicolo);
					break;
				}
				if ((VeicoloLavorativo != null && !VeicoloLavorativo.Exists()) || VeicoloLavorativo == null)
				{
					Client.Instance.RemoveTick(TimerVeicolo);
					return;
				}
				await BaseScript.Delay(0);
			}
			if (TempoRimozione == 0)
			{
				if (GetEntityAttachedToTowTruck(VeicoloLavorativo.Handle) == 0)
				{
					HUD.TimerBarPool.Remove(timerVeicolo);
					timerVeicolo = null;
					HUD.ShowNotification("Il veicolo da rimuovere se n'è andato!!", NotificationColor.Red, true);
					if (VeicoloDaRimuovere != null)
					{
						VeicoloDaRimuovere.IsPersistent = false;
						VeicoloDaRimuovere.PreviouslyOwnedByPlayer = false;
						VeicoloDaRimuovere.Delete();
						VeicoloDaRimuovere = null;
					}
					BlipVeicoloDaRimuovere.Delete();
					BlipVeicoloDaRimuovere = null;
					Client.Instance.RemoveTick(TimerVeicolo);
				}
			}
		}
	}
}
