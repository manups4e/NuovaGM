﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using static CitizenFX.Core.Native.API;
using NuovaGM.Client.gmPrincipale;
using NuovaGM.Client.gmPrincipale.Personaggio;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Client.MenuNativo;
using NuovaGM.Client.Veicoli;
using NuovaGM.Shared;
using Newtonsoft.Json;

namespace NuovaGM.Client.Lavori.Whitelistati.Medici
{
	static class MenuMedici
	{

		#region Spogliatoio
		public static async void MenuSpogliatoio()
		{
			UIMenu spogliatoio = new UIMenu("Spogliatoio", "Entra / esci in servizio");
			HUD.MenuPool.Add(spogliatoio);
			UIMenuItem cambio;
			if (!Eventi.Player.InServizio)
				cambio = new UIMenuItem("Entra in Servizio", "Hai fatto un giuramento.");
			else
				cambio = new UIMenuItem("Esci dal Servizio", "Smetti di lavorare");
			spogliatoio.AddItem(cambio);

			cambio.Activated += async (item, index) =>
			{
				Screen.Fading.FadeOut(800);
				await BaseScript.Delay(1000);
				HUD.MenuPool.CloseAllMenus();
				Eventi.Player.Stanziato = true;
				if (!Eventi.Player.InServizio) 
				{
					foreach (var Grado in ConfigClient.Conf.Lavori.Medici.Gradi)
					{
						if (Grado.Value.Id == Eventi.Player.CurrentChar.job.grade)
						{
							switch (Eventi.Player.CurrentChar.skin.sex)
							{
								case "Maschio":
									CambiaVestito(Grado.Value.Vestiti.Maschio);
									break;
								case "Femmina":
									CambiaVestito(Grado.Value.Vestiti.Femmina);
									break;
							}
						}
					}
					Eventi.Player.InServizio = true;
				}
				else
				{
					Eventi.Player.InServizio = false;
					await Funzioni.UpdateDress(Eventi.Player.CurrentChar.dressing);
				}
				await BaseScript.Delay(500);
				Screen.Fading.FadeIn(800);
				Eventi.Player.Stanziato = false;
			};
			spogliatoio.Visible = true;
		}

		public static async void CambiaVestito(AbitiLav dress)
		{
			SetPedComponentVariation(PlayerPedId(), (int)DrawableIndexes.Faccia, dress.Abiti.Faccia, dress.TextureVestiti.Faccia, 2);
			SetPedComponentVariation(PlayerPedId(), (int)DrawableIndexes.Maschera, dress.Abiti.Maschera, dress.TextureVestiti.Maschera, 2);
			SetPedComponentVariation(PlayerPedId(), (int)DrawableIndexes.Torso, dress.Abiti.Torso, dress.TextureVestiti.Torso, 2);
			SetPedComponentVariation(PlayerPedId(), (int)DrawableIndexes.Pantaloni, dress.Abiti.Pantaloni, dress.TextureVestiti.Pantaloni, 2);
			SetPedComponentVariation(PlayerPedId(), (int)DrawableIndexes.Borsa_Paracadute, dress.Abiti.Borsa_Paracadute, dress.TextureVestiti.Borsa_Paracadute, 2);
			SetPedComponentVariation(PlayerPedId(), (int)DrawableIndexes.Scarpe, dress.Abiti.Scarpe, dress.TextureVestiti.Scarpe, 2);
			SetPedComponentVariation(PlayerPedId(), (int)DrawableIndexes.Accessori, dress.Abiti.Accessori, dress.TextureVestiti.Accessori, 2);
			SetPedComponentVariation(PlayerPedId(), (int)DrawableIndexes.Sottomaglia, dress.Abiti.Sottomaglia, dress.TextureVestiti.Sottomaglia, 2);
			SetPedComponentVariation(PlayerPedId(), (int)DrawableIndexes.Kevlar, dress.Abiti.Kevlar, dress.TextureVestiti.Kevlar, 2);
			SetPedComponentVariation(PlayerPedId(), (int)DrawableIndexes.Badge, dress.Abiti.Badge, dress.TextureVestiti.Badge, 2);
			SetPedComponentVariation(PlayerPedId(), (int)DrawableIndexes.Torso_2, dress.Abiti.Torso_2, dress.TextureVestiti.Torso_2, 2);
			SetPedPropIndex(PlayerPedId(), (int)PropIndexes.Cappelli_Maschere, dress.Accessori.Cappelli_Maschere, dress.TexturesAccessori.Cappelli_Maschere, true);
			SetPedPropIndex(PlayerPedId(), (int)PropIndexes.Orecchie, dress.Accessori.Orecchie, dress.TexturesAccessori.Orecchie, true);
			SetPedPropIndex(PlayerPedId(), (int)PropIndexes.Occhiali_Occhi, dress.Accessori.Occhiali_Occhi, dress.TexturesAccessori.Occhiali_Occhi, true);
			SetPedPropIndex(PlayerPedId(), (int)PropIndexes.Unk_3, dress.Accessori.Unk_3, dress.TexturesAccessori.Unk_3, true);
			SetPedPropIndex(PlayerPedId(), (int)PropIndexes.Unk_4, dress.Accessori.Unk_4, dress.TexturesAccessori.Unk_4, true);
			SetPedPropIndex(PlayerPedId(), (int)PropIndexes.Unk_5, dress.Accessori.Unk_5, dress.TexturesAccessori.Unk_5, true);
			SetPedPropIndex(PlayerPedId(), (int)PropIndexes.Orologi, dress.Accessori.Orologi, dress.TexturesAccessori.Orologi, true);
			SetPedPropIndex(PlayerPedId(), (int)PropIndexes.Bracciali, dress.Accessori.Bracciali, dress.TexturesAccessori.Bracciali, true);
			SetPedPropIndex(PlayerPedId(), (int)PropIndexes.Unk_8, dress.Accessori.Unk_8, dress.TexturesAccessori.Unk_8, true);
		}

		#endregion

		#region Farmacia
		public static async void MenuFarmacia()
		{
			UIMenu farmacia = new UIMenu("Farmacia e Medicinali", "Con prescrizione o senza?");
			HUD.MenuPool.Add(farmacia);

			farmacia.Visible = true;
		}
		#endregion

		#region MenuF6
		public static async void InteractionMenu()
		{

		}
		#endregion

		#region MenuVeicoli
		private static List<Vehicle> veicoliParcheggio = new List<Vehicle>();
		private static Ospedale StazioneAttuale = new Ospedale();
		private static int LivelloGarage = 0;
		private static List<Vector4> parcheggi = new List<Vector4>()
		{
			new Vector4(224.500f, -998.695f, -99.6f, 225.0f),
			new Vector4(224.500f, -994.630f, -99.6f, 225.0f),
			new Vector4(224.500f, -990.255f, -99.6f, 225.0f),
			new Vector4(224.500f, -986.628f, -99.6f, 225.0f),
			new Vector4(224.500f, -982.496f, -99.6f, 225.0f),
			new Vector4(232.500f, -982.496f, -99.6f, 135.0f),
			new Vector4(232.500f, -986.628f, -99.6f, 135.0f),
			new Vector4(232.500f, -990.255f, -99.6f, 135.0f),
			new Vector4(232.500f, -994.630f, -99.6f, 135.0f),
			new Vector4(232.500f, -998.695f, -99.6f, 135.0f),
		};
		private static SpawnerSpawn PuntoAttuale = new SpawnerSpawn();
		private static bool InGarage = false;
		public static async void VehicleMenuNuovo(Ospedale Stazione, SpawnerSpawn Punto)
		{
			Eventi.Player.Stanziato = true;
			StazioneAttuale = Stazione;
			PuntoAttuale = Punto;
			Game.PlayerPed.Position = new Vector3(236.349f, -1005.013f, -100f);
			Game.PlayerPed.Heading = 85.162f;
			InGarage = true;
			if (Stazione.VeicoliAutorizzati.Count(o => o.GradiAutorizzati[0] == -1 || o.GradiAutorizzati.Contains(Eventi.Player.CurrentChar.job.grade)) <= 10)
			{
				for (int i = 0; i < Stazione.VeicoliAutorizzati.Count(o => o.GradiAutorizzati[0] == -1 || o.GradiAutorizzati.Contains(Eventi.Player.CurrentChar.job.grade)); i++)
				{
					veicoliParcheggio.Add(await Funzioni.SpawnLocalVehicle(Stazione.VeicoliAutorizzati[i].Model, new Vector3(parcheggi[i].X, parcheggi[i].Y, parcheggi[i].Z), parcheggi[i].W));
					veicoliParcheggio[i].PlaceOnGround();
					veicoliParcheggio[i].IsPersistent = true;
					veicoliParcheggio[i].LockStatus = VehicleLockStatus.Unlocked;
					veicoliParcheggio[i].IsInvincible = true;
					veicoliParcheggio[i].IsCollisionEnabled = true;
					veicoliParcheggio[i].IsEngineRunning = false;
					veicoliParcheggio[i].IsDriveable = false;
					veicoliParcheggio[i].IsSirenActive = true;
					veicoliParcheggio[i].IsSirenSilent = true;
					veicoliParcheggio[i].SetDecor("VeicoloMedici", Funzioni.GetRandomInt(100));
				}
			}
			else
			{
				await GarageConPiuVeicoli(Stazione.VeicoliAutorizzati, LivelloGarage);
			}
			await BaseScript.Delay(1000);
			Screen.Fading.FadeIn(800);
			Client.GetInstance.RegisterTickHandler(ControlloGarage);
		}
		private static async Task GarageConPiuVeicoli(List<Autorizzati> autorizzati, int livelloGarage)
		{
			foreach (var veh in veicoliParcheggio) veh.Delete();
			veicoliParcheggio.Clear();
			int totale = autorizzati.Count(o => o.GradiAutorizzati[0] == -1 || o.GradiAutorizzati.Contains(Eventi.Player.CurrentChar.job.grade));
			int LivelloGarageAttuali = totale - livelloGarage * 10 > livelloGarage * 10 ? 10 : (totale - (livelloGarage * 10));
			for (int i = 0; i < LivelloGarageAttuali; i++)
			{
				veicoliParcheggio.Add(await Funzioni.SpawnLocalVehicle(autorizzati[i + (livelloGarage * 10)].Model, new Vector3(parcheggi[i].X, parcheggi[i].Y, parcheggi[i].Z), parcheggi[i].W));
				veicoliParcheggio[i].PlaceOnGround();
				veicoliParcheggio[i].IsPersistent = true;
				veicoliParcheggio[i].LockStatus = VehicleLockStatus.Unlocked;
				veicoliParcheggio[i].IsInvincible = true;
				veicoliParcheggio[i].IsCollisionEnabled = true;
				veicoliParcheggio[i].IsEngineRunning = false;
				veicoliParcheggio[i].IsDriveable = false;
				veicoliParcheggio[i].IsSirenActive = true;
				veicoliParcheggio[i].IsSirenSilent = true;
				veicoliParcheggio[i].SetDecor("VeicoloMedici", Funzioni.GetRandomInt(100));
			}
		}

		private static async Task ControlloGarage()
		{
			if (Eventi.Player.Stanziato)
			{
				if (InGarage)
				{
					if (World.GetDistance(Game.PlayerPed.Position, new Vector3(240.317f, -1004.901f, -99f)) < 3f)
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per cambiare piano");
						if (Game.IsControlJustPressed(0, Control.Context))
						{
							MenuPiano();
						}
					}
					if (Game.PlayerPed.IsInVehicle())
					{
						if (Game.PlayerPed.CurrentVehicle.HasDecor("VeicoloMedici"))
						{
							HUD.ShowHelp("Per selezionare questo veicolo~n~~y~Accendi il motore~w~ e ~y~accelera~w~.");
							if (Game.IsControlJustPressed(0, Control.VehicleAccelerate) && Game.PlayerPed.CurrentVehicle.IsEngineRunning == true)
							{
								Screen.Fading.FadeOut(800);
								await BaseScript.Delay(1000);
								int model = Game.PlayerPed.CurrentVehicle.Model.Hash;
								foreach (var vehicle in veicoliParcheggio) vehicle.Delete();
								veicoliParcheggio.Clear();
								for (int i = 0; i < PuntoAttuale.SpawnPoints.Count; i++)
								{
									if (!Funzioni.IsSpawnPointClear(PuntoAttuale.SpawnPoints[i].Coords.ToVector3(), 2f))
										continue;
									else if (Funzioni.IsSpawnPointClear(PuntoAttuale.SpawnPoints[i].Coords.ToVector3(), 2f))
									{
										MediciMainClient.VeicoloAttuale = await Funzioni.SpawnVehicle(model, PuntoAttuale.SpawnPoints[i].Coords.ToVector3(), PuntoAttuale.SpawnPoints[i].Heading);
										break;
									}
									else
									{
										MediciMainClient.VeicoloAttuale = await Funzioni.SpawnVehicle(model, PuntoAttuale.SpawnPoints[0].Coords.ToVector3(), PuntoAttuale.SpawnPoints[0].Heading);
										break;
									}
								}
								Game.PlayerPed.CurrentVehicle.SetVehicleFuelLevel(100f);
								Game.PlayerPed.CurrentVehicle.IsEngineRunning = true;
								Game.PlayerPed.CurrentVehicle.IsDriveable = true;
								Game.PlayerPed.CurrentVehicle.Mods.LicensePlate = Funzioni.GetRandomInt(99) + "MED" + Funzioni.GetRandomInt(999);
								Game.PlayerPed.CurrentVehicle.SetDecor("VeicoloMedici", Funzioni.GetRandomInt(100));
								VeicoloPol veh = new VeicoloPol(Game.PlayerPed.CurrentVehicle.Mods.LicensePlate, Game.PlayerPed.CurrentVehicle.Model.Hash, Game.PlayerPed.CurrentVehicle.Handle);
								BaseScript.TriggerServerEvent("lprp:polizia:AggiungiVehMedici", JsonConvert.SerializeObject(veh));
								InGarage = false;
								StazioneAttuale = null;
								PuntoAttuale = null;
								veicoliParcheggio.Clear();
								Eventi.Player.Stanziato = false;
								await BaseScript.Delay(1000);
								Screen.Fading.FadeIn(800);
								Client.GetInstance.DeregisterTickHandler(ControlloGarage);
							}
						}
					}
				}
			}
		}

		private static async void MenuPiano()
		{
			UIMenu Ascensore = new UIMenu("Seleziona Piano", "Sali o scendi?");
			HUD.MenuPool.Add(Ascensore);
			UIMenuItem esci = new UIMenuItem("Esci dal Garage");
			Ascensore.AddItem(esci);
			int conto = StazioneAttuale.VeicoliAutorizzati.Count(o => o.GradiAutorizzati[0] == -1 || o.GradiAutorizzati.Contains(Eventi.Player.CurrentChar.job.grade));
			int piani = 1;
			for (int i = 1; i < conto + 1; i++)
			{
				if (i % 10 == 0)
				{
					piani++;
				}
			}
			for (int i = 0; i < piani; i++)
			{
				UIMenuItem piano = new UIMenuItem($"{i + 1}° piano");
				Ascensore.AddItem(piano);
				if (i == LivelloGarage)
					piano.SetRightBadge(UIMenuItem.BadgeStyle.Car);
			}
			Ascensore.OnItemSelect += async (menu, item, index) =>
			{

				if (item.RightBadge == UIMenuItem.BadgeStyle.Car)
					HUD.ShowNotification("Questo è il garage attuale!!", true);
				else
				{
					HUD.MenuPool.CloseAllMenus();
					Screen.Fading.FadeOut(800);
					await BaseScript.Delay(1000);
					if (item == esci)
					{
						Game.PlayerPed.Position = StazioneAttuale.Veicoli[StazioneAttuale.Veicoli.IndexOf(PuntoAttuale)].SpawnerMenu.ToVector3();
						InGarage = false;
						StazioneAttuale = null;
						PuntoAttuale = null;
						Eventi.Player.Stanziato = false;
						veicoliParcheggio.Clear();
						Client.GetInstance.DeregisterTickHandler(ControlloGarage);
					}
					else
					{
						LivelloGarage = index - 1;
						await GarageConPiuVeicoli(StazioneAttuale.VeicoliAutorizzati, LivelloGarage);
					}
					await BaseScript.Delay(1000);
					Screen.Fading.FadeIn(800);
				}
			};
			Ascensore.Visible = true;
		}
		#endregion
		
		#region MenuElicotteri
		static Vehicle PreviewHeli = new Vehicle(0);
		static Camera HeliCam = new Camera(0);
		public static async void HeliMenu(Ospedale Stazione, SpawnerSpawn Punto)
		{
			LoadInterior(GetInteriorAtCoords(-1267.0f, -3013.135f, -48.5f));
			RequestCollisionAtCoord(-1267.0f, -3013.135f, -48.5f);
			RequestAdditionalCollisionAtCoord(-1267.0f, -3013.135f, -48.5f);
			HeliCam = new Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", true));
			HeliCam.Position = new Vector3(-1268.174f, -2999.561f, -44.215f);
			HeliCam.IsActive = true;
			await BaseScript.Delay(1000);
			UIMenu MenuElicotteri = new UIMenu("Elicotteri Medici", "Cura le strade con stile!");
			HUD.MenuPool.Add(MenuElicotteri);

			MenuElicotteri.OnMenuOpen += async (menu) =>
			{
				PreviewHeli = await Funzioni.SpawnLocalVehicle(Stazione.ElicotteriAutorizzati[0].Model, new Vector3(-1267.0f, -3013.135f, -48.490f), 0);
				PreviewHeli.IsCollisionEnabled = false;
				PreviewHeli.IsPersistent = true;
				PreviewHeli.PlaceOnGround();
				PreviewHeli.IsPositionFrozen = true;
				if (PreviewHeli.Model.Hash == 353883353)
					SetVehicleLivery(PreviewHeli.Handle, 1); // 0 per pula, 1 per medici.. funziona solo per i veicoli d'emergenza!
				PreviewHeli.LockStatus = VehicleLockStatus.Locked;
				PreviewHeli.IsInvincible = true;
				PreviewHeli.IsEngineRunning = true;
				PreviewHeli.IsDriveable = false;
				Client.GetInstance.RegisterTickHandler(Heading);
				HeliCam.PointAt(PreviewHeli);
				while (!HasCollisionLoadedAroundEntity(PreviewHeli.Handle)) await BaseScript.Delay(1000);
				RenderScriptCams(true, false, 0, false, false);
				Screen.Fading.FadeIn(800);
			};


			for (int i = 0; i < Stazione.ElicotteriAutorizzati.Count; i++)
			{
				UIMenuItem veh = new UIMenuItem(Stazione.ElicotteriAutorizzati[i].Nome);
				MenuElicotteri.AddItem(veh);
			}

			MenuElicotteri.OnIndexChange += async (menu, index) =>
			{
				PreviewHeli = await Funzioni.SpawnLocalVehicle(Stazione.ElicotteriAutorizzati[index].Model, new Vector3(-1267.0f, -3013.135f, -48.490f), Punto.SpawnPoints[0].Heading);
				PreviewHeli.IsCollisionEnabled = false;
				PreviewHeli.IsPersistent = true;
				PreviewHeli.PlaceOnGround();
				PreviewHeli.IsPositionFrozen = true;
				if (PreviewHeli.Model.Hash == 353883353)
					SetVehicleLivery(PreviewHeli.Handle, 1);
				PreviewHeli.LockStatus = VehicleLockStatus.Locked;
				SetHeliBladesFullSpeed(PreviewHeli.Handle);
				PreviewHeli.IsInvincible = true;
				PreviewHeli.IsEngineRunning = true;
				PreviewHeli.IsDriveable = false;
				if (HeliCam.IsActive && PreviewHeli.Exists())
				{
					HeliCam.PointAt(PreviewHeli);
				}
			};

			MenuElicotteri.OnItemSelect += async (menu, item, index) =>
			{
				Screen.Fading.FadeOut(800);
				await BaseScript.Delay(1000);
				HeliCam.IsActive = false;
				RenderScriptCams(false, false, 0, false, false);
				for (int i = 0; i < Punto.SpawnPoints.Count; i++)
				{
					if (!Funzioni.IsSpawnPointClear(Punto.SpawnPoints[i].Coords.ToVector3(), 2f))
					{
						continue;
					}
					else if (Funzioni.IsSpawnPointClear(Punto.SpawnPoints[i].Coords.ToVector3(), 2f))
					{
						MediciMainClient.ElicotteroAttuale = await Funzioni.SpawnVehicle(Stazione.ElicotteriAutorizzati[index].Model, Punto.SpawnPoints[i].Coords.ToVector3(), Punto.SpawnPoints[i].Heading);
						break;
					}
					else
					{
						MediciMainClient.ElicotteroAttuale = await Funzioni.SpawnVehicle(Stazione.ElicotteriAutorizzati[index].Model, Punto.SpawnPoints[0].Coords.ToVector3(), Punto.SpawnPoints[0].Heading);
						break;
					}
				}
				Game.PlayerPed.CurrentVehicle.SetVehicleFuelLevel(100f);
				Game.PlayerPed.CurrentVehicle.IsDriveable = true;
				Game.PlayerPed.CurrentVehicle.Mods.LicensePlate = Funzioni.GetRandomInt(99) + "MED" + Funzioni.GetRandomInt(999);
				Game.PlayerPed.CurrentVehicle.SetDecor("VeicoloMedici", Funzioni.GetRandomInt(100));
				if (Game.PlayerPed.CurrentVehicle.Model.Hash == 353883353)
					SetVehicleLivery(Game.PlayerPed.CurrentVehicle.Handle, 1);
				VeicoloPol veh = new VeicoloPol(Game.PlayerPed.CurrentVehicle.Mods.LicensePlate, Game.PlayerPed.CurrentVehicle.Model.Hash, Game.PlayerPed.CurrentVehicle.Handle);
				BaseScript.TriggerServerEvent("lprp:polizia:AggiungiVehMedici", JsonConvert.SerializeObject(veh));
				HUD.MenuPool.CloseAllMenus();
				PreviewHeli.MarkAsNoLongerNeeded();
				PreviewHeli.Delete();
			};

			MenuElicotteri.OnMenuClose += async (menu) =>
			{
				Screen.Fading.FadeOut(800);
				await BaseScript.Delay(1000);
				Client.GetInstance.DeregisterTickHandler(Heading);
				HeliCam.IsActive = false;
				RenderScriptCams(false, false, 0, false, false);
				await BaseScript.Delay(1000);
				Screen.Fading.FadeIn(800);
				PreviewHeli.MarkAsNoLongerNeeded();
				PreviewHeli.Delete();
			};
			MenuElicotteri.Visible = true;
		}
		#endregion


		private static async Task Heading()
		{
			if (PreviewHeli.Exists())
			{
				RequestCollisionAtCoord(-1267.0f, -3013.135f, -48.5f);
				PreviewHeli.Heading += 1;
				if (!PreviewHeli.IsEngineRunning)
				{
					SetHeliBladesFullSpeed(PreviewHeli.Handle);
				}
			}
		}

	}
}