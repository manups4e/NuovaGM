using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScaleformUI;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.Core.Utility;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using CitizenFX.Core.UI;
using Logger;
using TheLastPlanet.Shared;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Veicoli;
using TheLastPlanet.Client.Core;
using TheLastPlanet.Client.Core.PlayerChar;
using TheLastPlanet.Shared.Veicoli;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Proprietà.Appartamenti.Case
{
	internal static class AppartamentiClient
	{
		public static List<Vehicle> VeicoliParcheggio = new List<Vehicle>();

		public static void Init()
		{
			Client.Instance.AddEventHandler("lprp:richiestaDiEntrare", new Action<int, string>(Richiesta));
			Client.Instance.AddEventHandler("lprp:citofono:puoiEntrare", new Action<int, string>(PuoiEntrare));
			Client.Instance.AddEventHandler("lprp:entraGarageConProprietario", new Action<Vector3>(EntraGarageConProprietario));
			Client.Instance.AddEventHandler("lprp:housedealer:caricaImmobiliDaDB", new Action<string, string>(CaricaCaseDaDb));
		}

		public static void Stop()
		{
			Client.Instance.RemoveEventHandler("lprp:richiestaDiEntrare", new Action<int, string>(Richiesta));
			Client.Instance.RemoveEventHandler("lprp:citofono:puoiEntrare", new Action<int, string>(PuoiEntrare));
			Client.Instance.RemoveEventHandler("lprp:entraGarageConProprietario", new Action<Vector3>(EntraGarageConProprietario));
			Client.Instance.RemoveEventHandler("lprp:housedealer:caricaImmobiliDaDB", new Action<string, string>(CaricaCaseDaDb));
			Client.Impostazioni.RolePlay.Proprieta.Appartamenti.Clear();
			Client.Impostazioni.RolePlay.Proprieta.Garages.Garages.Clear();
		}

		private static async void CaricaCaseDaDb(string JsonCase, string jsonGarage)
		{
			Dictionary<string, string> aparts = JsonCase.FromJson<Dictionary<string, string>>();
			Dictionary<string, string> garages = jsonGarage.FromJson<Dictionary<string, string>>();
			foreach (KeyValuePair<string, string> a in aparts) Client.Impostazioni.RolePlay.Proprieta.Appartamenti.Add(a.Key, a.Value.FromJson<ConfigCase>());
			foreach (KeyValuePair<string, string> a in garages) Client.Impostazioni.RolePlay.Proprieta.Garages.Garages.Add(a.Key, a.Value.FromJson<Garages>());
		}

		public static async void EntraMenu(KeyValuePair<string, ConfigCase> app)
		{
			Camera dummycam = World.CreateCamera(GameplayCamera.Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
			World.RenderingCamera = dummycam;
			Camera cam = World.CreateCamera(app.Value.TelecameraFuori.pos.ToVector3, new Vector3(0), GameplayCamera.FieldOfView);
			cam.PointAt(app.Value.TelecameraFuori.guarda.ToVector3);
			RenderScriptCams(true, true, 1500, true, false);
			dummycam.InterpTo(cam, 1500, 1, 1);
			UIMenu casa = new UIMenu(app.Value.Label, "Appartamenti");
			HUD.MenuPool.ControlDisablingEnabled = true;
			HUD.MenuPool.Add(casa);
			UIMenu Citofona = casa.AddSubMenu("Citofona ai residenti");
			UIMenuItem entra;

			if (Cache.PlayerCache.MyPlayer.User.CurrentChar.Proprietà.Contains(app.Key))
			{
				entra = new UIMenuItem("Entra in casa");
				casa.AddItem(entra);
				entra.Activated += async (_submenu, _subitem) =>
				{
					Cache.PlayerCache.MyPlayer.User.Status.Istanza.Istanzia(app.Key);
					Screen.Fading.FadeOut(500);
					while (!Screen.Fading.IsFadedOut) await BaseScript.Delay(0);
					HUD.MenuPool.CloseAllMenus();

					while (cam.IsActive && cam.Exists() && cam != null)
					{
						RenderScriptCams(false, false, 1500, true, false);
						World.RenderingCamera = null;
						cam.IsActive = false;
						cam.Delete();
					}

					RequestCollisionAtCoord(app.Value.SpawnDentro.X, app.Value.SpawnDentro.Y, app.Value.SpawnDentro.Z);
					Cache.PlayerCache.MyPlayer.Ped.Position = app.Value.SpawnDentro.ToVector3;
					while (!HasCollisionLoadedAroundEntity(PlayerPedId())) await BaseScript.Delay(1000);
					await BaseScript.Delay(2000);
					Screen.Fading.FadeIn(500);
					NetworkFadeInEntity(PlayerPedId(), true);
				};
			}

			HUD.MenuPool.OnMenuStateChanged += async (a, _menu, c) =>
			{
				switch (c)
				{
					case MenuState.ChangeForward when _menu == Citofona:
					{
						_menu.Clear();
						List<Player> gioc = (from p in Client.Instance.GetPlayers.ToList() where p != Cache.PlayerCache.MyPlayer.Player let pl = p.GetPlayerData() where pl.Status.Istanza.Stanziato where pl.Status.Istanza.IsProprietario where pl.Status.Istanza.Instance == app.Key select p).ToList();

						if (gioc.Count > 0)
							foreach (Player p in gioc.ToList())
							{
								User pl = p.GetPlayerData();
								UIMenuItem it = new UIMenuItem(pl.FullName);
								_menu.AddItem(it);
								it.Activated += (_submenu, _subitem) =>
								{
									Game.PlaySound("DOOR_BUZZ", "MP_PLAYER_APARTMENT");
									BaseScript.TriggerServerEvent("lprp:citofonaAlPlayer", p.ServerId, app.ToJson()); // params: personaincasa.serverid, fromsource chi suona
									HUD.MenuPool.CloseAllMenus();
								};
							}
						else
							_menu.AddItem(new UIMenuItem("Non ci sono persone in casa al momento!"));

						break;
					}
					case MenuState.ChangeBackward when _menu == casa:
					{
						await BaseScript.Delay(100);

						if (HUD.MenuPool.IsAnyMenuOpen) return;
						if (cam.IsActive) RenderScriptCams(false, true, 1500, true, false);
						dummycam.Delete();
						cam.Delete();
						HUD.MenuPool.ControlDisablingEnabled = true;

						break;
					}
				}
			};
			while (dummycam.IsInterpolating) await BaseScript.Delay(0);
			while (cam.IsInterpolating) await BaseScript.Delay(0);
			casa.Visible = true;
		}

		public static async void EsciMenu(ConfigCase app, bool inGarage = false, bool inTetto = false)
		{
			UIMenu esci = new UIMenu(app.Label, "Appartamenti");
			HUD.MenuPool.Add(esci);
			UIMenuItem escisci = new UIMenuItem("Esci dall'appartamento");
			esci.AddItem(escisci);
			UIMenuItem garage = new UIMenuItem("", "");
			UIMenuItem tetto = new UIMenuItem("", "");
			UIMenuItem casa = new UIMenuItem("", "");

			if (inGarage || inTetto)
			{
				casa = new UIMenuItem("Entra in casa");
				esci.AddItem(casa);
			}

			if (app.GarageIncluso && !inGarage)
			{
				garage = new UIMenuItem("Vai al garage");
				esci.AddItem(garage);
			}

			if (app.TettoIncluso && !inTetto)
			{
				tetto = new UIMenuItem("Vai sul tetto");
				esci.AddItem(tetto);
			}

			esci.OnItemSelect += async (_menu, _item, _index) =>
			{
				HUD.MenuPool.CloseAllMenus();
				if (Cache.PlayerCache.MyPlayer.Ped.IsVisible) NetworkFadeOutEntity(PlayerPedId(), true, false);
				Screen.Fading.FadeOut(500);
				while (!Screen.Fading.IsFadedOut) await BaseScript.Delay(0);

				if (_item == escisci)
				{
					Funzioni.Teleport(app.SpawnFuori.ToVector3);
					Cache.PlayerCache.MyPlayer.User.Status.Istanza.RimuoviIstanza();
				}
				else if (_item == casa)
				{
					Funzioni.Teleport(app.SpawnDentro.ToVector3);
				}
				else if (_item == garage)
				{
					ClearPedTasksImmediately(Cache.PlayerCache.MyPlayer.Ped.Handle);
					Cache.PlayerCache.MyPlayer.Ped.IsPositionFrozen = true;
					if (Cache.PlayerCache.MyPlayer.Ped.IsVisible) NetworkFadeOutEntity(PlayerPedId(), true, false);
					DoScreenFadeOut(500);
					while (!IsScreenFadedOut()) await BaseScript.Delay(0);
					RequestCollisionAtCoord(app.SpawnGarageAPiediDentro.X, app.SpawnGarageAPiediDentro.Y, app.SpawnGarageAPiediDentro.Z);
					NewLoadSceneStart(app.SpawnGarageAPiediDentro.X, app.SpawnGarageAPiediDentro.Y, app.SpawnGarageAPiediDentro.Z, app.SpawnGarageAPiediDentro.X, app.SpawnGarageAPiediDentro.Y, app.SpawnGarageAPiediDentro.Z, 50f, 0);
					int tempTimer = GetGameTimer();

					// Wait for the new scene to be loaded.
					while (IsNetworkLoadingScene())
					{
						// If this takes longer than 1 second, just abort. It's not worth waiting that long.
						if (GetGameTimer() - tempTimer > 1000)
						{
							Client.Logger.Debug( "Waiting for the scene to load is taking too long (more than 1s). Breaking from wait loop.");

							break;
						}

						await BaseScript.Delay(0);
					}

					SetEntityCoords(PlayerPedId(), app.SpawnGarageAPiediDentro.X, app.SpawnGarageAPiediDentro.Y, app.SpawnGarageAPiediDentro.Z, false, false, false, false);
					tempTimer = GetGameTimer();

					// Wait for the collision to be loaded around the entity in this new location.
					while (!HasCollisionLoadedAroundEntity(Cache.PlayerCache.MyPlayer.Ped.Handle))
					{
						// If this takes too long, then just abort, it's not worth waiting that long since we haven't found the real ground coord yet anyway.
						if (GetGameTimer() - tempTimer > 1000)
						{
							Client.Logger.Debug( "Waiting for the collision is taking too long (more than 1s). Breaking from wait loop.");

							break;
						}

						await BaseScript.Delay(0);
					}

					foreach (OwnedVehicle veh in Cache.PlayerCache.MyPlayer.User.CurrentChar.Veicoli)
						if (veh.Garage.Garage == Cache.PlayerCache.MyPlayer.User.Status.Istanza.Instance)
							if (veh.Garage.InGarage)
							{
								Vehicle veic = await Funzioni.SpawnLocalVehicle(veh.DatiVeicolo.props.Model, new Vector3(Client.Impostazioni.RolePlay.Proprieta.Garages.LowEnd.PosVehs[veh.Garage.Posto].X, Client.Impostazioni.RolePlay.Proprieta.Garages.LowEnd.PosVehs[veh.Garage.Posto].Y, Client.Impostazioni.RolePlay.Proprieta.Garages.LowEnd.PosVehs[veh.Garage.Posto].Z), Client.Impostazioni.RolePlay.Proprieta.Garages.LowEnd.PosVehs[veh.Garage.Posto].Heading);
								await veic.SetVehicleProperties(veh.DatiVeicolo.props);
								VeicoliParcheggio.Add(veic);
							}

					NetworkFadeInEntity(Cache.PlayerCache.MyPlayer.Ped.Handle, true);
					Cache.PlayerCache.MyPlayer.Ped.IsPositionFrozen = false;
					DoScreenFadeIn(500);
					SetGameplayCamRelativePitch(0.0f, 1.0f);
					Client.Instance.AddTick(Garage);
				}
				else if (_item == tetto)
				{
					Funzioni.Teleport(app.SpawnTetto.ToVector3);
					Cache.PlayerCache.MyPlayer.User.Status.Istanza.RimuoviIstanza();
				}

				await BaseScript.Delay(2000);
				Screen.Fading.FadeIn(500);
				NetworkFadeInEntity(PlayerPedId(), true);
			};
			esci.Visible = true;
		}

		private static string nome;
		private static string appa;
		private static int serverIdRic;
		private static int tempo;

		public static void Richiesta(int serverIdRichiedente, string app)
		{
			Game.PlaySound("DOOR_BUZZ", "MP_PLAYER_APARTMENT");
			nome = Client.Instance.GetPlayers.ToList().FirstOrDefault(x => x.ServerId == serverIdRichiedente).GetPlayerData().FullName;
			appa = app;
			serverIdRic = serverIdRichiedente;
			tempo = GetGameTimer();
			Client.Instance.AddTick(AccRif);
		}

		private static async Task AccRif()
		{
			HUD.ShowHelp($"{nome} ti ha citofonato.\n~INPUT_VEH_EXIT~ per accettare");

			if (GetGameTimer() - tempo < 30000)
			{
				if (Input.IsControlJustPressed(Control.VehicleExit))
				{
					BaseScript.TriggerServerEvent("lprp:citofono:puoEntrare", serverIdRic, appa);
					Client.Instance.RemoveTick(AccRif);
					nome = null;
					appa = null;
					serverIdRic = 0;
					tempo = 0;
				}
			}
			else
			{
				Client.Instance.RemoveTick(AccRif);
				nome = null;
				appa = null;
				serverIdRic = 0;
				tempo = 0;
			}
		}

		public static void PuoiEntrare(int serverIdInCasa, string appartamento)
		{
			KeyValuePair<string, ConfigCase> app = appartamento.FromJson<KeyValuePair<string, ConfigCase>>();
			Player InCasa = Client.Instance.GetPlayers.ToList().FirstOrDefault(x => x.ServerId == serverIdInCasa);

			if (InCasa == null) return;
			if (!Cache.PlayerCache.MyPlayer.Ped.IsInRangeOf(app.Value.MarkerEntrata.ToVector3, 3f)) return;
			if (Cache.PlayerCache.MyPlayer.User.Status.Istanza.Stanziato) return;
			Cache.PlayerCache.MyPlayer.User.Status.Istanza.Istanzia(InCasa.ServerId, app.Key);
			Funzioni.Teleport(app.Value.SpawnDentro.ToVector3);
		}

		public static async Task Garage()
		{
			if (Cache.PlayerCache.MyPlayer.Ped.IsInRangeOf(Client.Impostazioni.RolePlay.Proprieta.Garages.LowEnd.ModifyMarker.ToVector3, 1.375f))
			{
				// gestire
			}

			if (Cache.PlayerCache.MyPlayer.Ped.IsInRangeOf(Client.Impostazioni.RolePlay.Proprieta.Garages.MidEnd4.ModifyMarker.ToVector3, 1.375f))
			{
				// gestire
			}

			if (Cache.PlayerCache.MyPlayer.Ped.IsInRangeOf(Client.Impostazioni.RolePlay.Proprieta.Garages.MidEnd6.ModifyMarker.ToVector3, 1.375f))
			{
				// gestire
			}

			if (Cache.PlayerCache.MyPlayer.Ped.IsInRangeOf(Client.Impostazioni.RolePlay.Proprieta.Garages.HighEnd.ModifyMarker.ToVector3, 1.375f))
			{
				// gestire
			}

			if (Cache.PlayerCache.MyPlayer.User.Status.RolePlayStates.InVeicolo)
			{
				HUD.ShowHelp("Per selezionare questo veicolo e uscire~n~~y~Accendi il motore~w~ e ~y~accelera~w~.");

				if (Input.IsControlJustPressed(Control.VehicleAccelerate) && Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.IsEngineRunning)
				{
					Screen.Fading.FadeOut(800);
					await BaseScript.Delay(1000);
					string plate = Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Mods.LicensePlate;
					foreach (Vehicle vehicle in VeicoliParcheggio) vehicle.Delete();
					VeicoliParcheggio.Clear();
					Vector4 exit = Vector4.Zero;
					if (Client.Impostazioni.RolePlay.Proprieta.Appartamenti.ContainsKey(Cache.PlayerCache.MyPlayer.User.Status.Istanza.Instance))
						exit = Client.Impostazioni.RolePlay.Proprieta.Appartamenti[Cache.PlayerCache.MyPlayer.User.Status.Istanza.Instance].SpawnGarageInVehFuori.ToVector4;
					else
						exit = Client.Impostazioni.RolePlay.Proprieta.Garages.Garages[Cache.PlayerCache.MyPlayer.User.Status.Istanza.Instance].SpawnFuori.ToVector4;
					int tempo = GetGameTimer();
					Vector3 newPos = exit.ToVector3();
					float Head = exit.W;

					while (!Funzioni.IsSpawnPointClear(exit.ToVector3(), 2f))
					{
						if (GetGameTimer() - tempo > 5000)
						{
							Client.Logger.Debug( "Punto di spawn fuori dal garage occupato, trovato nuovo punto");

							break;
						}

						await BaseScript.Delay(0);
					}

					if (!Funzioni.IsSpawnPointClear(exit.ToVector3(), 2f)) GetClosestVehicleNodeWithHeading(exit.X, exit.Y, exit.Z, ref newPos, ref Head, 1, 3, 0);
					Vehicle vehi = await Funzioni.SpawnVehicle(Cache.PlayerCache.MyPlayer.User.CurrentChar.Veicoli.FirstOrDefault(x => x.Targa == plate).DatiVeicolo.props.Model, newPos, Head);
					await vehi.SetVehicleProperties(Cache.PlayerCache.MyPlayer.User.CurrentChar.Veicoli.FirstOrDefault(x => x.Targa == plate).DatiVeicolo.props);
					Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.IsEngineRunning = true;
					Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.IsDriveable = true;
					BaseScript.TriggerServerEvent("lprp:vehInGarage", plate, false);
					Cache.PlayerCache.MyPlayer.User.Status.Istanza.RimuoviIstanza();
					await BaseScript.Delay(1000);
					Screen.Fading.FadeIn(800);
					Client.Instance.RemoveTick(Garage);
				}
			}
		}

		private static async void EntraGarageConProprietario(Vector3 pos)
		{
			if (Cache.PlayerCache.MyPlayer.Ped.IsVisible) NetworkFadeOutEntity(Cache.PlayerCache.MyPlayer.Ped.CurrentVehicle.Handle, true, false);
			Screen.Fading.FadeOut(500);
			await BaseScript.Delay(1000);
			RequestCollisionAtCoord(pos.X, pos.Y, pos.Z);
			NewLoadSceneStart(pos.X, pos.Y, pos.Z, pos.X, pos.Y, pos.Z, 50f, 0);
			int tempTimer = GetGameTimer();

			// Wait for the new scene to be loaded.
			while (IsNetworkLoadingScene())
			{
				// If this takes longer than 1 second, just abort. It's not worth waiting that long.
				if (GetGameTimer() - tempTimer > 1000)
				{
					Client.Logger.Debug( "Waiting for the scene to load is taking too long (more than 1s). Breaking from wait loop.");

					break;
				}

				await BaseScript.Delay(0);
			}

			SetEntityCoords(PlayerPedId(), pos.X, pos.Y, pos.Z, false, false, false, false);
			tempTimer = GetGameTimer();

			// Wait for the collision to be loaded around the entity in this new location.
			while (!HasCollisionLoadedAroundEntity(Cache.PlayerCache.MyPlayer.Ped.Handle))
			{
				// If this takes too long, then just abort, it's not worth waiting that long since we haven't found the real ground coord yet anyway.
				if (GetGameTimer() - tempTimer > 1000)
				{
					Client.Logger.Debug( "Waiting for the collision is taking too long (more than 1s). Breaking from wait loop.");

					break;
				}

				await BaseScript.Delay(0);
			}

			foreach (OwnedVehicle veh in Cache.PlayerCache.MyPlayer.User.CurrentChar.Veicoli)
				if (veh.Garage.Garage == Cache.PlayerCache.MyPlayer.User.Status.Istanza.Instance)
					if (veh.Garage.InGarage)
					{
						Vehicle veic = await Funzioni.SpawnLocalVehicle(veh.DatiVeicolo.props.Model, new Vector3(Client.Impostazioni.RolePlay.Proprieta.Garages.LowEnd.PosVehs[veh.Garage.Posto].X, Client.Impostazioni.RolePlay.Proprieta.Garages.LowEnd.PosVehs[veh.Garage.Posto].Y, Client.Impostazioni.RolePlay.Proprieta.Garages.LowEnd.PosVehs[veh.Garage.Posto].Z), Client.Impostazioni.RolePlay.Proprieta.Garages.LowEnd.PosVehs[veh.Garage.Posto].Heading);
						await veic.SetVehicleProperties(veh.DatiVeicolo.props);
						VeicoliParcheggio.Add(veic);
					}

			NetworkFadeInEntity(Cache.PlayerCache.MyPlayer.Ped.Handle, true);
			Cache.PlayerCache.MyPlayer.Ped.IsPositionFrozen = false;
			DoScreenFadeIn(500);
			SetGameplayCamRelativePitch(0.0f, 1.0f);
			Client.Instance.AddTick(Garage);
		}
	}
}