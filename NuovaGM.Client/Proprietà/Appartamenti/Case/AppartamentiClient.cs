using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuovaGM.Client.MenuNativo;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Client.gmPrincipale.Utility;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using CitizenFX.Core.UI;
using Logger;
using NuovaGM.Shared;

namespace NuovaGM.Client.Proprietà.Appartamenti.Case
{
	static class AppartamentiClient
	{
		public static void Init()
		{
			Client.Instance.AddEventHandler("lprp:richiestaDiEntrare", new Action<int, string>(Richiesta));
			Client.Instance.AddEventHandler("lprp:citofono:puoiEntrare", new Action<int, string>(PuoiEntrare));
		}

		public static async void EntraMenu(KeyValuePair<string, ConfigCase> app)
		{
			if (Game.PlayerPed.IsVisible)
				NetworkFadeOutEntity(PlayerPedId(), true, false);
			var dummycam = World.CreateCamera(GameplayCamera.Position, GameplayCamera.Rotation, GameplayCamera.FieldOfView);
			World.RenderingCamera = dummycam;
			var cam = World.CreateCamera(app.Value.TelecameraFuori.pos, new Vector3(0), GameplayCamera.FieldOfView);
			cam.PointAt(app.Value.TelecameraFuori.guarda);
			RenderScriptCams(true, true, 1500, true, false);
			dummycam.InterpTo(cam, 1500, 1, 1);
			UIMenu casa = new UIMenu(app.Value.Label, "Appartamenti");
			casa.Title.Scale = 0.9f;
			HUD.MenuPool.Add(casa);
			UIMenu Citofona = casa.AddSubMenu("Citofona ai residenti");
			Citofona.Title.Scale = 0.9f;
			UIMenuItem entra;

			if (Game.Player.GetPlayerData().CurrentChar.Proprietà.Contains(app.Key))
			{
				entra = new UIMenuItem("Entra in casa");
				casa.AddItem(entra);
				entra.Activated += async (_submenu, _subitem) =>
				{
					Game.Player.GetPlayerData().Istanza.Istanzia(app.Key);
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
					Game.PlayerPed.Position = app.Value.SpawnDentro;
					while (!HasCollisionLoadedAroundEntity(PlayerPedId())) await BaseScript.Delay(1000);
					await BaseScript.Delay(2000);
					Screen.Fading.FadeIn(500);
					NetworkFadeInEntity(PlayerPedId(), true);
				};
			}
			Citofona.OnMenuOpen += async (_menu) =>
			{
				_menu.Clear();
				List<Player> gioc = new List<Player>();
				foreach(var p in Client.Instance.GetPlayers.ToList())
				{
					if (p == Game.Player) continue;
					var pl = p.GetPlayerData();
					if (pl.Istanza.Stanziato)
					{
						if (pl.Istanza.IsProprietario)
						{
							if (pl.Istanza.Instance == app.Key)
							{
								gioc.Add(p);
							}
						}
					}
				}
				if(gioc.Count > 0)
				{
					foreach (var p in gioc.ToList())
					{
						var pl = p.GetPlayerData();
						UIMenuItem it = new UIMenuItem(pl.FullName);
						_menu.AddItem(it);
						it.Activated += (_submenu, _subitem) =>
						{
							Game.PlaySound("DOOR_BUZZ", "MP_PLAYER_APARTMENT");
							BaseScript.TriggerServerEvent("lprp:citofonaAlPlayer", p.ServerId, app.Serialize()); // params: personaincasa.serverid, fromsource chi suona
							HUD.MenuPool.CloseAllMenus();
						};
					}
				}
				else
				{
					_menu.AddItem(new UIMenuItem("Non ci sono persone in casa al momento!"));
				}
			};
			casa.OnMenuClose += async (_menu) =>
			{
				await BaseScript.Delay(100);
				if (HUD.MenuPool.IsAnyMenuOpen()) return;
				if(cam.IsActive)
					RenderScriptCams(false, true, 1500, true, false);
				dummycam.Delete();
				cam.Delete();
				NetworkFadeInEntity(PlayerPedId(), true);
			};
			while (dummycam.IsInterpolating) await BaseScript.Delay(0);
			while (cam.IsInterpolating) await BaseScript.Delay(0);
			casa.Visible = true;
		}
		public static async void EsciMenu(ConfigCase app, bool inGarage = false, bool inTetto = false)
		{
			UIMenu esci = new UIMenu(app.Label, "Appartamenti");
			HUD.MenuPool.Add(esci);
			esci.Title.Scale = 0.9f;
			UIMenuItem escisci = new UIMenuItem("Esci dall'appartamento");
			esci.AddItem(escisci);
			UIMenuItem garage = new UIMenuItem("", "");
			UIMenuItem tetto = new UIMenuItem("", "");
			UIMenuItem casa = new UIMenuItem("", "");
			if(inGarage || inTetto)
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
				if (Game.PlayerPed.IsVisible)
					NetworkFadeOutEntity(PlayerPedId(), true, false);
				Screen.Fading.FadeOut(500);
				while (!Screen.Fading.IsFadedOut) await BaseScript.Delay(0);
				if (_item == escisci)
				{
					Funzioni.Teleport(PlayerPedId(), app.SpawnFuori);
					Game.Player.GetPlayerData().Istanza.RimuoviIstanza();
				}
				else if (_item == casa)
				{
					Funzioni.Teleport(PlayerPedId(), app.SpawnDentro);
				}
				else if (_item == garage)
				{
					Funzioni.Teleport(PlayerPedId(), app.SpawnGarageAPiediDentro);
				}
				else if (_item == tetto)
				{
					Funzioni.Teleport(PlayerPedId(), app.SpawnTetto);
					Game.Player.GetPlayerData().Istanza.RimuoviIstanza();
				}
				await BaseScript.Delay(2000);
				Screen.Fading.FadeIn(500);
				NetworkFadeInEntity(PlayerPedId(), true);
			};
			esci.Visible = true;
		}

		static string nome;
		static string appa;
		static int serverIdRic;
		static int tempo;
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
			KeyValuePair<string, ConfigCase> app = appartamento.Deserialize<KeyValuePair<string, ConfigCase>>();
			var InCasa = Client.Instance.GetPlayers.ToList().FirstOrDefault(x => x.ServerId == serverIdInCasa);
			if(InCasa != null)
			{
				if(Game.PlayerPed.IsInRangeOf(app.Value.MarkerEntrata, 3f))
				{
					if(!Game.Player.GetPlayerData().Istanza.Stanziato)
					{
						Game.Player.GetPlayerData().Istanza.Istanzia(InCasa.Character.NetworkId, app.Key);
						Funzioni.Teleport(PlayerPedId(), app.Value.SpawnDentro);
					}
				}
			}
		}

	}
}
