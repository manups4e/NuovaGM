using CitizenFX.Core;
using TheLastPlanet.Client.Core.CharCreation;
using TheLastPlanet.Client.Core;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Shared;
using System;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.Core
{
	static class Discord
	{
		public static void Init()
		{
			SetDiscordAppId(Client.Impostazioni.Main.DiscordAppId);
			SetDiscordRichPresenceAsset(Client.Impostazioni.Main.DiscordRichPresenceAsset);
			SetDiscordRichPresenceAssetText("Discord.gg/n4ep9Fq");
			Client.Instance.AddTick(RichPresence);
		}

		private static async Task RichPresence()
		{
			SetDiscordAppId(Client.Impostazioni.Main.DiscordAppId);
			SetDiscordRichPresenceAsset(Client.Impostazioni.Main.DiscordRichPresenceAsset);
			Vector3 PedCoords = Cache.Char == null ? Cache.PlayerPed.Position : Cache.Char.posizione.ToVector3();
			uint StreetName = 0;
			uint StreetAngolo = 0;
			GetStreetNameAtCoord(PedCoords.X, PedCoords.Y, PedCoords.Z, ref StreetName, ref StreetAngolo);
			string NomeVia = GetStreetNameFromHashKey(StreetName);
			string NomeAngolo = GetStreetNameFromHashKey(StreetAngolo);
			if (!Main.spawned)
			{
				if (!Ingresso.LogIn.guiEnabled)
				{
					if (Creator.Creazione.Visible || Creator.Apparel.Visible || Creator.Apparenze.Visible || Creator.Dettagli.Visible || Creator.Genitori.Visible || Creator.Info.Visible)
						SetRichPresence("Sta creando un personaggio");
				}
				else SetRichPresence("Sta selezionando un personaggio");
			}
			else
			{
				if (Cache.PlayerPed.IsOnFoot && !Cache.PlayerPed.IsInWater && !Lavori.Generici.Pescatore.PescatoreClient.Pescando && !Lavori.Generici.Cacciatore.CacciatoreClient.StaCacciando)
				{
					if (Cache.PlayerPed.IsSprinting)
					{
						if (StreetAngolo != 0)
							SetRichPresence("Sta correndo in " + NomeVia + " angolo " + NomeAngolo);
						else
							SetRichPresence("Sta correndo in " + NomeVia);
					}
					else if (Cache.PlayerPed.IsRunning)
					{
						if (StreetAngolo != 0)
							SetRichPresence("E' di fretta in " + NomeVia + " angolo " + NomeAngolo);
						else
							SetRichPresence("E' di fretta in " + NomeVia);
					}
					else if (Cache.PlayerPed.IsWalking)
					{
						if (StreetAngolo != 0)
							SetRichPresence("Sta passeggiando in " + NomeVia + " angolo " + NomeAngolo);
						else
							SetRichPresence("Sta passeggiando in " + NomeVia);
					}
					else if (IsPedStill(PlayerPedId()))
					{
						if (StreetAngolo != 0)
							SetRichPresence("E' fermo a piedi in " + NomeVia + " angolo " + NomeAngolo);
						else
							SetRichPresence("E' fermo a piedi in " + NomeVia);
					}
				}
				else if (Cache.PlayerPed.IsInVehicle() && !Cache.PlayerPed.IsInHeli && !Cache.PlayerPed.IsInPlane && !Cache.PlayerPed.IsOnFoot && !Cache.PlayerPed.IsInSub && !Cache.PlayerPed.IsInBoat)
				{
					float KMH = (float)Math.Round(Cache.PlayerPed.CurrentVehicle.Speed * 3.6, 2);
					string VehName = Cache.PlayerPed.CurrentVehicle.LocalizedName;
					if (KMH > 50)
					{
						if (StreetAngolo != 0)
							SetRichPresence("Sfreccia a tutta velocità in " + NomeVia + " angolo " + NomeAngolo + ", Veicolo: " + VehName);
						else
							SetRichPresence("Sfreccia a tutta velocità in " + NomeVia + ", Veicolo: " + VehName);
					}
					else if (KMH <= 50 && KMH > 0)
					{
						if (StreetAngolo != 0)
							SetRichPresence("Alla guida in " + NomeVia + " angolo " + NomeAngolo + ", Veicolo: " + VehName);
						else
							SetRichPresence("Alla guida in " + NomeVia + ", Veicolo: " + VehName);
					}
					else if (KMH == 0)
					{
						if (StreetAngolo != 0)
							SetRichPresence("Parcheggiato in " + NomeVia + " angolo " + NomeAngolo + ", Veicolo: " + VehName);
						else
							SetRichPresence("Parcheggiato in " + NomeVia + ", Veicolo: " + VehName);
					}
				}
				else if (Cache.PlayerPed.IsInHeli || Cache.PlayerPed.IsInPlane)
				{
					string VehName = Cache.PlayerPed.CurrentVehicle.LocalizedName;
					if (Cache.PlayerPed.CurrentVehicle.IsInAir || GetEntityHeightAboveGround(Cache.PlayerPed.CurrentVehicle.Handle) > 2f)
					{
						if (StreetAngolo != 0)
							SetRichPresence("Sta sorvolando " + NomeVia + " angolo " + NomeAngolo + ", Veicolo: " + VehName);
						else
							SetRichPresence("Atterrato in " + NomeVia + ", Veicolo: " + VehName);
					}
				}
				else if (Cache.PlayerPed.IsSwimming)
					SetRichPresence("Sta nuotando");
				else if (Cache.PlayerPed.IsSwimmingUnderWater || Cache.PlayerPed.IsDiving)
					SetRichPresence("Sta nuotando sott'acqua");
				else if (Cache.PlayerPed.IsInBoat && Cache.PlayerPed.CurrentVehicle.IsInWater)
				{
					SetRichPresence("Sta navigando in barca: " + Cache.PlayerPed.CurrentVehicle.LocalizedName);
				}
				else if (Cache.PlayerPed.IsInSub && Cache.PlayerPed.CurrentVehicle.IsInWater)
					SetRichPresence("Sta esplorando i fondali in un sottomarino");
				else if (Cache.PlayerPed.IsAiming || Cache.PlayerPed.IsAimingFromCover || Cache.PlayerPed.IsShooting && !Lavori.Generici.Pescatore.PescatoreClient.Pescando && !Lavori.Generici.Cacciatore.CacciatoreClient.StaCacciando)
					SetRichPresence("E' in uno scontro a fuoco");
				else if (Cache.Player.GetPlayerData().StatiPlayer.Ammanettato)
					SetRichPresence("Legato o ammanettato");
				else if (Main.IsDead)
					SetRichPresence("Sta morendo");
				else if (Cache.PlayerPed.IsDoingDriveBy)
					SetRichPresence("In uno scontro a fuoco da veicolo");
				else if (Cache.PlayerPed.IsInParachuteFreeFall || Cache.PlayerPed.ParachuteState == ParachuteState.FreeFalling)
					SetRichPresence("Fa paracadutismo");
				else if (IsPedStill(PlayerPedId()) || (Cache.PlayerPed.IsInVehicle() && Cache.PlayerPed.CurrentVehicle.Speed == 0) && (int)Math.Floor(GetTimeSinceLastInput(0) / 1000f) > (int)Math.Floor(Client.Impostazioni.Main.AFKCheckTime / 2f))
					SetRichPresence("AFK in gioco");
				else if (Cache.Player.GetPlayerData().StatiPlayer.InPausa)
					SetRichPresence("In Pausa");
				else if (Lavori.Generici.Pescatore.PescatoreClient.Pescando)
					SetRichPresence("Sta pescando");
				else if (Lavori.Generici.Cacciatore.CacciatoreClient.StaCacciando)
					SetRichPresence("Sta cacciando");
			}
			await BaseScript.Delay(1000);
		}
	}
}
