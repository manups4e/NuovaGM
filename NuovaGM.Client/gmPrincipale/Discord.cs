using CitizenFX.Core;
using NuovaGM.Client.gmPrincipale.MenuGm;
using NuovaGM.Client.gmPrincipale.Utility;
using System;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace NuovaGM.Client.gmPrincipale
{
	static class Discord
	{
		public static void Init()
		{
			SetDiscordAppId(ConfigClient.Conf.Main.DiscordAppId);
			SetDiscordRichPresenceAsset(ConfigClient.Conf.Main.DiscordRichPresenceAsset);
			SetDiscordRichPresenceAssetText("Discord.gg/n4ep9Fq");
			Client.GetInstance.RegisterTickHandler(RichPresence);
		}

		private static async Task RichPresence()
		{
			SetDiscordAppId(ConfigClient.Conf.Main.DiscordAppId);
			SetDiscordRichPresenceAsset(ConfigClient.Conf.Main.DiscordRichPresenceAsset);
			Vector3 PedCoords = Game.PlayerPed.Position;
			uint StreetName = 0;
			uint StreetAngolo = 0;
			GetStreetNameAtCoord(PedCoords.X, PedCoords.Y, PedCoords.Z, ref StreetName, ref StreetAngolo);
			string NomeVia = GetStreetNameFromHashKey(StreetName);
			string NomeAngolo = GetStreetNameFromHashKey(StreetAngolo);
			if (!Main.spawned)
			{
				if (Menus.CharSelection.Visible)
					SetRichPresence("Sta selezionando un personaggio");
				else if (Menus.Creazione.Visible || Menus.Apparel.Visible || Menus.Apparenze.Visible || Menus.Dettagli.Visible || Menus.Genitori.Visible || Menus.Info.Visible)
					SetRichPresence("Sta creando un personaggio");
			}
			else
			{
				if (Game.PlayerPed.IsOnFoot && !Game.PlayerPed.IsInWater)
				{
					if (Game.PlayerPed.IsSprinting)
					{
						if (StreetAngolo != 0)
							SetRichPresence("Sta correndo in " + NomeVia + " angolo " + NomeAngolo);
						else
							SetRichPresence("Sta correndo in " + NomeVia);
					}
					else if (Game.PlayerPed.IsRunning)
					{
						if (StreetAngolo != 0)
							SetRichPresence("E' di fretta in " + NomeVia + " angolo " + NomeAngolo);
						else
							SetRichPresence("E' di fretta in " + NomeVia);
					}
					else if (Game.PlayerPed.IsWalking)
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
				else if (Game.PlayerPed.IsInVehicle() && !Game.PlayerPed.IsInHeli && !Game.PlayerPed.IsInPlane && !Game.PlayerPed.IsOnFoot && !Game.PlayerPed.IsInSub && !Game.PlayerPed.IsInBoat)
				{
					float KMH = (float)Math.Round(Game.PlayerPed.CurrentVehicle.Speed * 3.6, 2);
					string VehName = Game.PlayerPed.CurrentVehicle.LocalizedName;
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
				else if (Game.PlayerPed.IsInHeli || Game.PlayerPed.IsInPlane)
				{
					string VehName = Game.PlayerPed.CurrentVehicle.LocalizedName;
					if (Game.PlayerPed.CurrentVehicle.IsInAir || GetEntityHeightAboveGround(Game.PlayerPed.CurrentVehicle.Handle) > 2f)
					{
						if (StreetAngolo != 0)
							SetRichPresence("Sta sorvolando " + NomeVia + " angolo " + NomeAngolo + ", Veicolo: " + VehName);
						else
							SetRichPresence("Atterrato in " + NomeVia + ", Veicolo: " + VehName);
					}
				}
				else if (Game.PlayerPed.IsSwimming)
					SetRichPresence("Sta nuotando");
				else if (Game.PlayerPed.IsSwimmingUnderWater || Game.PlayerPed.IsDiving)
					SetRichPresence("Sta nuotando sott'acqua");
				else if (Game.PlayerPed.IsInBoat && Game.PlayerPed.CurrentVehicle.IsInWater)
				{
					SetRichPresence("Sta navigando in barca: " + Game.PlayerPed.CurrentVehicle.LocalizedName);
				}
				else if (Game.PlayerPed.IsInSub && Game.PlayerPed.CurrentVehicle.IsInWater)
					SetRichPresence("Sta esplorando i fondali in un sottomarino");
				else if (Game.PlayerPed.IsAiming || Game.PlayerPed.IsAimingFromCover || Game.PlayerPed.IsShooting)
					SetRichPresence("E' in uno scontro a fuoco");
				else if (Game.PlayerPed.IsCuffed)
					SetRichPresence("Legato o ammanettato");
				else if (Main.IsDead)
					SetRichPresence("Sta morendo");
				else if (Game.PlayerPed.IsDoingDriveBy)
					SetRichPresence("In uno scontro a fuoco da veicolo");
				else if (Game.PlayerPed.IsInParachuteFreeFall || Game.PlayerPed.ParachuteState == ParachuteState.FreeFalling)
					SetRichPresence("Fa paracadutismo");
				else if (IsPedStill(PlayerPedId()) || (Game.PlayerPed.IsInVehicle() && Game.PlayerPed.CurrentVehicle.Speed == 0) && Main.AFKTime < (int)Math.Round(Main.SecondsBeforeKick / 2f))
					SetRichPresence("AFK in gioco");
				else if (Game.IsPaused)
					SetRichPresence("In Pausa");
				else if (Lavori.Generici.Pescatore.PescatoreClient.Pescando)
					SetRichPresence("Sta pescando");
			}
			await BaseScript.Delay(1000);
		}
	}
}
