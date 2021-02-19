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
			Ped playerPed = new Ped(PlayerPedId());
			Player player = Game.Player;
			SetDiscordAppId(Client.Impostazioni.Main.DiscordAppId);
			SetDiscordRichPresenceAsset(Client.Impostazioni.Main.DiscordRichPresenceAsset);
			Vector3 PedCoords = Eventi.Player == null ? playerPed.Position : Eventi.Player.posizione.ToVector3();
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
				if (playerPed.IsOnFoot && !playerPed.IsInWater && !Lavori.Generici.Pescatore.PescatoreClient.Pescando && !Lavori.Generici.Cacciatore.CacciatoreClient.StaCacciando)
				{
					if (playerPed.IsSprinting)
					{
						if (StreetAngolo != 0)
							SetRichPresence("Sta correndo in " + NomeVia + " angolo " + NomeAngolo);
						else
							SetRichPresence("Sta correndo in " + NomeVia);
					}
					else if (playerPed.IsRunning)
					{
						if (StreetAngolo != 0)
							SetRichPresence("E' di fretta in " + NomeVia + " angolo " + NomeAngolo);
						else
							SetRichPresence("E' di fretta in " + NomeVia);
					}
					else if (playerPed.IsWalking)
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
				else if (playerPed.IsInVehicle() && !playerPed.IsInHeli && !playerPed.IsInPlane && !playerPed.IsOnFoot && !playerPed.IsInSub && !playerPed.IsInBoat)
				{
					float KMH = (float)Math.Round(playerPed.CurrentVehicle.Speed * 3.6, 2);
					string VehName = playerPed.CurrentVehicle.LocalizedName;
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
				else if (playerPed.IsInHeli || playerPed.IsInPlane)
				{
					string VehName = playerPed.CurrentVehicle.LocalizedName;
					if (playerPed.CurrentVehicle.IsInAir || GetEntityHeightAboveGround(playerPed.CurrentVehicle.Handle) > 2f)
					{
						if (StreetAngolo != 0)
							SetRichPresence("Sta sorvolando " + NomeVia + " angolo " + NomeAngolo + ", Veicolo: " + VehName);
						else
							SetRichPresence("Atterrato in " + NomeVia + ", Veicolo: " + VehName);
					}
				}
				else if (playerPed.IsSwimming)
					SetRichPresence("Sta nuotando");
				else if (playerPed.IsSwimmingUnderWater || playerPed.IsDiving)
					SetRichPresence("Sta nuotando sott'acqua");
				else if (playerPed.IsInBoat && playerPed.CurrentVehicle.IsInWater)
				{
					SetRichPresence("Sta navigando in barca: " + playerPed.CurrentVehicle.LocalizedName);
				}
				else if (playerPed.IsInSub && playerPed.CurrentVehicle.IsInWater)
					SetRichPresence("Sta esplorando i fondali in un sottomarino");
				else if (playerPed.IsAiming || playerPed.IsAimingFromCover || playerPed.IsShooting && !Lavori.Generici.Pescatore.PescatoreClient.Pescando && !Lavori.Generici.Cacciatore.CacciatoreClient.StaCacciando)
					SetRichPresence("E' in uno scontro a fuoco");
				else if (player.GetPlayerData().StatiPlayer.Ammanettato)
					SetRichPresence("Legato o ammanettato");
				else if (Main.IsDead)
					SetRichPresence("Sta morendo");
				else if (playerPed.IsDoingDriveBy)
					SetRichPresence("In uno scontro a fuoco da veicolo");
				else if (playerPed.IsInParachuteFreeFall || playerPed.ParachuteState == ParachuteState.FreeFalling)
					SetRichPresence("Fa paracadutismo");
				else if (IsPedStill(PlayerPedId()) || (playerPed.IsInVehicle() && playerPed.CurrentVehicle.Speed == 0) && (int)Math.Floor(GetTimeSinceLastInput(0) / 1000f) > (int)Math.Floor(Client.Impostazioni.Main.AFKCheckTime / 2f))
					SetRichPresence("AFK in gioco");
				else if (player.GetPlayerData().StatiPlayer.InPausa)
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
