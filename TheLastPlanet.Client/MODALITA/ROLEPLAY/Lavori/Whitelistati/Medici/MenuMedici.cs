using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using static CitizenFX.Core.Native.API;
using TheLastPlanet.Client.Core;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using ScaleformUI;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Veicoli;
using Newtonsoft.Json;
using TheLastPlanet.Shared;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Lavori.Whitelistati.Medici
{
	internal static class MenuMedici
	{
		#region Spogliatoio

		public static async void MenuSpogliatoio()
		{
			UIMenu spogliatoio = new("Spogliatoio", "Entra / esci in servizio");
			HUD.MenuPool.Add(spogliatoio);
			UIMenuItem cambio;
			cambio = !Cache.PlayerCache.MyPlayer.User.Status.RolePlayStates.InServizio ? new UIMenuItem("Entra in Servizio", "Hai fatto un giuramento.") : new UIMenuItem("Esci dal Servizio", "Smetti di lavorare");
			spogliatoio.AddItem(cambio);
			cambio.Activated += async (item, index) =>
			{
				Screen.Fading.FadeOut(800);
				await BaseScript.Delay(1000);
				HUD.MenuPool.CloseAllMenus();
				NetworkFadeOutEntity(PlayerPedId(), true, false);

				if (!Cache.PlayerCache.MyPlayer.User.Status.RolePlayStates.InServizio)
				{
					foreach (KeyValuePair<string, JobGrade> Grado in Client.Impostazioni.RolePlay.Lavori.Medici.Gradi.Where(Grado => Grado.Value.Id == Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Grade))
						switch (Cache.PlayerCache.MyPlayer.User.CurrentChar.Skin.sex)
						{
							case "Maschio":
								CambiaVestito(Grado.Value.Vestiti.Maschio);

								break;
							case "Femmina":
								CambiaVestito(Grado.Value.Vestiti.Femmina);

								break;
						}

					Cache.PlayerCache.MyPlayer.User.Status.RolePlayStates.InServizio = true;
				}
				else
				{
					Cache.PlayerCache.MyPlayer.User.Status.RolePlayStates.InServizio = false;
					Funzioni.UpdateDress(PlayerPedId(), Cache.PlayerCache.MyPlayer.User.CurrentChar.Dressing);
				}

				await BaseScript.Delay(500);
				Screen.Fading.FadeIn(800);
				NetworkFadeInEntity(PlayerPedId(), true);
			};
			spogliatoio.Visible = true;
		}

		public static async void CambiaVestito(AbitiLav dress)
		{
			int id = PlayerPedId();
			SetPedComponentVariation(id, (int)DrawableIndexes.Faccia, dress.Abiti.Faccia, dress.TextureVestiti.Faccia, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Maschera, dress.Abiti.Maschera, dress.TextureVestiti.Maschera, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Torso, dress.Abiti.Torso, dress.TextureVestiti.Torso, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Pantaloni, dress.Abiti.Pantaloni, dress.TextureVestiti.Pantaloni, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Borsa_Paracadute, dress.Abiti.Borsa_Paracadute, dress.TextureVestiti.Borsa_Paracadute, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Scarpe, dress.Abiti.Scarpe, dress.TextureVestiti.Scarpe, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Accessori, dress.Abiti.Accessori, dress.TextureVestiti.Accessori, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Sottomaglia, dress.Abiti.Sottomaglia, dress.TextureVestiti.Sottomaglia, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Kevlar, dress.Abiti.Kevlar, dress.TextureVestiti.Kevlar, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Badge, dress.Abiti.Badge, dress.TextureVestiti.Badge, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Torso_2, dress.Abiti.Torso_2, dress.TextureVestiti.Torso_2, 2);
			SetPedPropIndex(id, (int)PropIndexes.Cappelli_Maschere, dress.Accessori.Cappelli_Maschere, dress.TexturesAccessori.Cappelli_Maschere, true);
			SetPedPropIndex(id, (int)PropIndexes.Orecchie, dress.Accessori.Orecchie, dress.TexturesAccessori.Orecchie, true);
			SetPedPropIndex(id, (int)PropIndexes.Occhiali_Occhi, dress.Accessori.Occhiali_Occhi, dress.TexturesAccessori.Occhiali_Occhi, true);
			SetPedPropIndex(id, (int)PropIndexes.Unk_3, dress.Accessori.Unk_3, dress.TexturesAccessori.Unk_3, true);
			SetPedPropIndex(id, (int)PropIndexes.Unk_4, dress.Accessori.Unk_4, dress.TexturesAccessori.Unk_4, true);
			SetPedPropIndex(id, (int)PropIndexes.Unk_5, dress.Accessori.Unk_5, dress.TexturesAccessori.Unk_5, true);
			SetPedPropIndex(id, (int)PropIndexes.Orologi, dress.Accessori.Orologi, dress.TexturesAccessori.Orologi, true);
			SetPedPropIndex(id, (int)PropIndexes.Bracciali, dress.Accessori.Bracciali, dress.TexturesAccessori.Bracciali, true);
			SetPedPropIndex(id, (int)PropIndexes.Unk_8, dress.Accessori.Unk_8, dress.TexturesAccessori.Unk_8, true);
		}

		#endregion

		#region Farmacia

		public static async void MenuFarmacia()
		{
			UIMenu farmacia = new("Farmacia e Medicinali", "Con prescrizione o senza?");
			HUD.MenuPool.Add(farmacia);
			farmacia.Visible = true;
		}

		#endregion

		#region MenuF6

		public static async void InteractionMenu()
		{
			UIMenu MenuMedico = new("Menu Medico", "Salviamo qualche vita!");
			HUD.MenuPool.Add(MenuMedico);
			UIMenuItem controlloFerite = new("Controlla ferite", "Dove fa male?");
			UIMenuItem rianima = new("Tenta rianimazione", "Attenzione: potrebbe fallire!");
			MenuMedico.AddItem(controlloFerite);
			MenuMedico.AddItem(rianima);
			MenuMedico.Visible = true;
		}

		#endregion

		#region MenuVeicoli

		private static List<Vehicle> veicoliParcheggio = new();
		private static Ospedale StazioneAttuale = new();
		private static int LivelloGarage = 0;
		private static List<Vector4> parcheggi = new()
		{
			new(224.500f, -998.695f, -99.6f, 225.0f),
			new(224.500f, -994.630f, -99.6f, 225.0f),
			new(224.500f, -990.255f, -99.6f, 225.0f),
			new(224.500f, -986.628f, -99.6f, 225.0f),
			new(224.500f, -982.496f, -99.6f, 225.0f),
			new(232.500f, -982.496f, -99.6f, 135.0f),
			new(232.500f, -986.628f, -99.6f, 135.0f),
			new(232.500f, -990.255f, -99.6f, 135.0f),
			new(232.500f, -994.630f, -99.6f, 135.0f),
			new(232.500f, -998.695f, -99.6f, 135.0f)
		};
		private static SpawnerSpawn PuntoAttuale = new();
		private static bool InGarage = false;

		public static async void VehicleMenuNuovo(Ospedale Stazione, SpawnerSpawn Punto)
		{
			Cache.PlayerCache.MyPlayer.User.Status.Istanza.Istanzia("SceltaVeicoliMedici");
			StazioneAttuale = Stazione;
			PuntoAttuale = Punto;
			Cache.PlayerCache.MyPlayer.Ped.Position = new Vector3(236.349f, -1005.013f, -100f);
			Cache.PlayerCache.MyPlayer.Ped.Heading = 85.162f;
			InGarage = true;

			if (Stazione.VeicoliAutorizzati.Count(o => o.GradiAutorizzati[0] == -1 || o.GradiAutorizzati.Contains(Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Grade)) <= 10)
				for (int i = 0; i < Stazione.VeicoliAutorizzati.Count(o => o.GradiAutorizzati[0] == -1 || o.GradiAutorizzati.Contains(Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Grade)); i++)
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
			else
				await GarageConPiuVeicoli(Stazione.VeicoliAutorizzati, LivelloGarage);

			await BaseScript.Delay(1000);
			Screen.Fading.FadeIn(800);
			Client.Instance.AddTick(ControlloGarage);
		}

		private static async Task GarageConPiuVeicoli(List<Autorizzati> autorizzati, int livelloGarage)
		{
			foreach (Vehicle veh in veicoliParcheggio) veh.Delete();
			veicoliParcheggio.Clear();
			int totale = autorizzati.Count(o => o.GradiAutorizzati[0] == -1 || o.GradiAutorizzati.Contains(Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Grade));
			int LivelloGarageAttuali = totale - livelloGarage * 10 > livelloGarage * 10 ? 10 : totale - livelloGarage * 10;

			for (int i = 0; i < LivelloGarageAttuali; i++)
			{
				veicoliParcheggio.Add(await Funzioni.SpawnLocalVehicle(autorizzati[i + livelloGarage * 10].Model, new Vector3(parcheggi[i].X, parcheggi[i].Y, parcheggi[i].Z), parcheggi[i].W));
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
			Ped p = Cache.PlayerCache.MyPlayer.Ped;

			if (Cache.PlayerCache.MyPlayer.User.Status.Istanza.Stanziato)
				if (InGarage)
				{
					if (p.IsInRangeOf(new Vector3(240.317f, -1004.901f, -99f), 3f))
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per cambiare piano");
						if (Input.IsControlJustPressed(Control.Context)) MenuPiano();
					}

					if (Cache.PlayerCache.MyPlayer.User.Status.PlayerStates.InVeicolo)
						if (p.CurrentVehicle.HasDecor("VeicoloMedici"))
						{
							HUD.ShowHelp("Per selezionare questo veicolo e uscire~n~~y~Accendi il motore~w~ e ~y~accelera~w~.");

							if (Input.IsControlJustPressed(Control.VehicleAccelerate) && p.CurrentVehicle.IsEngineRunning)
							{
								Screen.Fading.FadeOut(800);
								await BaseScript.Delay(1000);
								int model = p.CurrentVehicle.Model.Hash;
								foreach (Vehicle vehicle in veicoliParcheggio) vehicle.Delete();
								veicoliParcheggio.Clear();

								for (int i = 0; i < PuntoAttuale.SpawnPoints.Count; i++)
									if (!Funzioni.IsSpawnPointClear(PuntoAttuale.SpawnPoints[i].Coords.ToVector3, 2f))
									{
										continue;
									}
									else if (Funzioni.IsSpawnPointClear(PuntoAttuale.SpawnPoints[i].Coords.ToVector3, 2f))
									{
										MediciMainClient.VeicoloAttuale = await Funzioni.SpawnVehicle(model, PuntoAttuale.SpawnPoints[i].Coords.ToVector3, PuntoAttuale.SpawnPoints[i].Heading);

										break;
									}
									else
									{
										MediciMainClient.VeicoloAttuale = await Funzioni.SpawnVehicle(model, PuntoAttuale.SpawnPoints[0].Coords.ToVector3, PuntoAttuale.SpawnPoints[0].Heading);

										break;
									}

								p.CurrentVehicle.SetVehicleFuelLevel(100f);
								p.CurrentVehicle.IsEngineRunning = true;
								p.CurrentVehicle.IsDriveable = true;
								p.CurrentVehicle.Mods.LicensePlate = Funzioni.GetRandomInt(99) + "MED" + Funzioni.GetRandomInt(999);
								p.CurrentVehicle.SetDecor("VeicoloMedici", Funzioni.GetRandomInt(100));
								VeicoloPol veh = new(p.CurrentVehicle.Mods.LicensePlate, p.CurrentVehicle.Model.Hash, p.CurrentVehicle.Handle);
								BaseScript.TriggerServerEvent("lprp:polizia:AggiungiVehMedici", veh.ToJson());
								InGarage = false;
								StazioneAttuale = null;
								PuntoAttuale = null;
								veicoliParcheggio.Clear();
								Cache.PlayerCache.MyPlayer.User.Status.Istanza.RimuoviIstanza();
								await BaseScript.Delay(1000);
								Screen.Fading.FadeIn(800);
								Client.Instance.RemoveTick(ControlloGarage);
							}
						}
				}
		}

		private static async void MenuPiano()
		{
			UIMenu Ascensore = new("Seleziona Piano", "Sali o scendi?");
			HUD.MenuPool.Add(Ascensore);
			UIMenuItem esci = new("Esci dal Garage");
			Ascensore.AddItem(esci);
			int conto = StazioneAttuale.VeicoliAutorizzati.Count(o => o.GradiAutorizzati[0] == -1 || o.GradiAutorizzati.Contains(Cache.PlayerCache.MyPlayer.User.CurrentChar.Job.Grade));
			int piani = 1;
			for (int i = 1; i < conto + 1; i++)
				if (i % 10 == 0)
					piani++;

			for (int i = 0; i < piani; i++)
			{
				UIMenuItem piano = new($"{i + 1}° piano");
				Ascensore.AddItem(piano);
				if (i == LivelloGarage) piano.SetRightBadge(BadgeIcon.CAR);
			}

			Ascensore.OnItemSelect += async (menu, item, index) =>
			{
				if (item.RightBadge == BadgeIcon.CAR)
				{
					HUD.ShowNotification("Questo è il garage attuale!!", true);
				}
				else
				{
					HUD.MenuPool.CloseAllMenus();
					Screen.Fading.FadeOut(800);
					await BaseScript.Delay(1000);

					if (item == esci)
					{
						Cache.PlayerCache.MyPlayer.Ped.Position = StazioneAttuale.Veicoli[StazioneAttuale.Veicoli.IndexOf(PuntoAttuale)].SpawnerMenu.ToVector3;
						InGarage = false;
						StazioneAttuale = null;
						PuntoAttuale = null;
						Cache.PlayerCache.MyPlayer.User.Status.Istanza.RimuoviIstanza();
						veicoliParcheggio.Clear();
						Client.Instance.RemoveTick(ControlloGarage);
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

		private static Vehicle PreviewHeli = new(0);
		private static Camera HeliCam = new(0);

		public static async void HeliMenu(Ospedale Stazione, SpawnerSpawn Punto)
		{
			LoadInterior(GetInteriorAtCoords(-1267.0f, -3013.135f, -48.5f));
			RequestCollisionAtCoord(-1267.0f, -3013.135f, -48.5f);
			RequestAdditionalCollisionAtCoord(-1267.0f, -3013.135f, -48.5f);
			HeliCam = new Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", true));
			HeliCam.Position = new Vector3(-1268.174f, -2999.561f, -44.215f);
			HeliCam.IsActive = true;
			await BaseScript.Delay(1000);
			UIMenu MenuElicotteri = new("Elicotteri Medici", "Cura le strade con stile!");
			HUD.MenuPool.Add(MenuElicotteri);

			for (int i = 0; i < Stazione.ElicotteriAutorizzati.Count; i++)
			{
				UIMenuItem veh = new(Stazione.ElicotteriAutorizzati[i].Nome);
				MenuElicotteri.AddItem(veh);
			}

			MenuElicotteri.OnIndexChange += async (menu, index) =>
			{
				PreviewHeli = await Funzioni.SpawnLocalVehicle(Stazione.ElicotteriAutorizzati[index].Model, new Vector3(-1267.0f, -3013.135f, -48.490f), Punto.SpawnPoints[0].Heading);
				PreviewHeli.IsCollisionEnabled = false;
				PreviewHeli.IsPersistent = true;
				PreviewHeli.PlaceOnGround();
				PreviewHeli.IsPositionFrozen = true;
				if (PreviewHeli.Model.Hash == 353883353) SetVehicleLivery(PreviewHeli.Handle, 1);
				PreviewHeli.LockStatus = VehicleLockStatus.Locked;
				SetHeliBladesFullSpeed(PreviewHeli.Handle);
				PreviewHeli.IsInvincible = true;
				PreviewHeli.IsEngineRunning = true;
				PreviewHeli.IsDriveable = false;
				if (HeliCam.IsActive && PreviewHeli.Exists()) HeliCam.PointAt(PreviewHeli);
			};
			MenuElicotteri.OnItemSelect += async (menu, item, index) =>
			{
				Screen.Fading.FadeOut(800);
				await BaseScript.Delay(1000);
				HeliCam.IsActive = false;
				RenderScriptCams(false, false, 0, false, false);

				foreach (SpawnPoints t in Punto.SpawnPoints)
					if (!Funzioni.IsSpawnPointClear(t.Coords.ToVector3, 2f))
					{
						continue;
					}
					else if (Funzioni.IsSpawnPointClear(t.Coords.ToVector3, 2f))
					{
						MediciMainClient.ElicotteroAttuale = await Funzioni.SpawnVehicle(Stazione.ElicotteriAutorizzati[index].Model, t.Coords.ToVector3, t.Heading);

						break;
					}
					else
					{
						MediciMainClient.ElicotteroAttuale = await Funzioni.SpawnVehicle(Stazione.ElicotteriAutorizzati[index].Model, Punto.SpawnPoints[0].Coords.ToVector3, Punto.SpawnPoints[0].Heading);

						break;
					}

				Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.SetVehicleFuelLevel(100f);
				Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.IsDriveable = true;
				Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Mods.LicensePlate = Funzioni.GetRandomInt(99) + "MED" + Funzioni.GetRandomInt(999);
				Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.SetDecor("VeicoloMedici", Funzioni.GetRandomInt(100));
				if (Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Model.Hash == 353883353) SetVehicleLivery(Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Handle, 1);
				VeicoloPol veh = new(Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Mods.LicensePlate, Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Model.Hash, Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Handle);
				BaseScript.TriggerServerEvent("lprp:polizia:AggiungiVehMedici", veh.ToJson());
				HUD.MenuPool.CloseAllMenus();
				PreviewHeli.MarkAsNoLongerNeeded();
				PreviewHeli.Delete();
			};
			HUD.MenuPool.OnMenuStateChanged += async (oldmenu, newmenu, state) =>
			{
				if (state == MenuState.Opened && newmenu == MenuElicotteri)
				{
					PreviewHeli = await Funzioni.SpawnLocalVehicle(Stazione.ElicotteriAutorizzati[0].Model, new Vector3(-1267.0f, -3013.135f, -48.490f), 0);
					PreviewHeli.IsCollisionEnabled = false;
					PreviewHeli.IsPersistent = true;
					PreviewHeli.PlaceOnGround();
					PreviewHeli.IsPositionFrozen = true;
					if (PreviewHeli.Model.Hash == 353883353) SetVehicleLivery(PreviewHeli.Handle, 1); // 0 per pula, 1 per medici.. funziona solo per i veicoli d'emergenza!
					PreviewHeli.LockStatus = VehicleLockStatus.Locked;
					PreviewHeli.IsInvincible = true;
					PreviewHeli.IsEngineRunning = true;
					PreviewHeli.IsDriveable = false;
					Client.Instance.AddTick(Heading);
					HeliCam.PointAt(PreviewHeli);
					if (GetInteriorFromEntity(PreviewHeli.Handle) != 0) SetFocusEntity(PreviewHeli.Handle);
					while (!HasCollisionLoadedAroundEntity(PreviewHeli.Handle)) await BaseScript.Delay(1000);
					RenderScriptCams(true, false, 0, false, false);
					Screen.Fading.FadeIn(800);
				}
				else if (state == MenuState.Closed && oldmenu == MenuElicotteri)
				{
					Screen.Fading.FadeOut(800);
					await BaseScript.Delay(1000);
					Client.Instance.RemoveTick(Heading);
					HeliCam.IsActive = false;
					RenderScriptCams(false, false, 0, false, false);
					ClearFocus();
					await BaseScript.Delay(1000);
					Screen.Fading.FadeIn(800);
					PreviewHeli.MarkAsNoLongerNeeded();
					PreviewHeli.Delete();
				}
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
				if (!PreviewHeli.IsEngineRunning) SetHeliBladesFullSpeed(PreviewHeli.Handle);
			}
		}
	}
}