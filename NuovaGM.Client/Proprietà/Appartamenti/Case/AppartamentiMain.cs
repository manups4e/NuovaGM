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

namespace NuovaGM.Client.Proprietà.Appartamenti.Case
{
	static class AppartamentiMain
	{
		public static void Init()
		{
			Client.Instance.AddEventHandler("lprp:richiestaDiEntrare", new Action<int, string>(Richiesta));
			Client.Instance.AddEventHandler("lprp:citofono:puoiEntrare", new Action<int, string>(PuoiEntrare));
		}

		public static async void EntraMenu(KeyValuePair<string, ConfigCase> app)
		{
			if (Game.PlayerPed.IsVisible)
			{
				//NetworkFadeOutEntity(PlayerPedId(), true, false);
				SetEntityLocallyInvisible(PlayerPedId());
			}
			var cam = World.CreateCamera(app.Value.TelecameraFuori.pos, new Vector3(0), 45f);
			cam.PointAt(app.Value.TelecameraFuori.guarda);
			RenderScriptCams(true, true, 1500, true, false);
			UIMenu casa = new UIMenu(app.Value.Label, "Appartamenti");
			casa.Title.Scale = 0.8f;
			HUD.MenuPool.Add(casa);
			UIMenu Citofona = HUD.MenuPool.AddSubMenu(casa, "Citofona ai residenti");
			UIMenuItem entra;
			if (Game.Player.GetPlayerData().CurrentChar.Proprietà.Contains(app.Key))
			{
				entra = new UIMenuItem("Entra in casa");
				entra.Activated += (_submenu, _subitem) =>
				{
					Game.Player.GetPlayerData().Istanza.Istanzia(app.Key);
					Funzioni.Teleport(PlayerPedId(), app.Value.SpawnDentro);
				};
			}
			Citofona.OnMenuOpen += async (_menu) =>
			{
				foreach(var p in Client.Instance.GetPlayers.ToList())
				{
					var pl = p.GetPlayerData();
					if (pl.Istanza.Stanziato)
					{
						if (pl.Istanza.IsProprietario)
						{
							if (pl.Istanza.Instance == app.Key)
							{
								UIMenuItem it = new UIMenuItem(pl.FullName);
								_menu.AddItem(it);
								it.Activated += (_submenu, _subitem) =>
								{ 
									Game.PlaySound("DOOR_BUZZ", "MP_PLAYER_APARTMENT");
									BaseScript.TriggerServerEvent("lprp:citofonaAlPlayer", p.ServerId, JsonConvert.SerializeObject(app)); // params: personaincasa.serverid, fromsource chi suona
								};
							}
						}
					}
				}
			};
			casa.Visible = true;
			casa.OnMenuClose += (_menu) =>
			{
				RenderScriptCams(false, true, 1500, true, false);
				NetworkFadeInEntity(PlayerPedId(), true);
			};
		}
		public static async void EsciMenu(KeyValuePair<string, ConfigCase> app)
		{

		}

		public static async void Richiesta(int serverIdRichiedente, string app)
		{
			int tempo = GetGameTimer();
			Game.PlaySound("DOOR_BUZZ", "MP_PLAYER_APARTMENT");
			while (GetGameTimer() - tempo > 30000)
			{
				await BaseScript.Delay(0);
				HUD.ShowHelp($"{Client.Instance.GetPlayers.ToList().FirstOrDefault(x => x.ServerId == serverIdRichiedente).GetPlayerData().FullName} ti ha citofonato.\n" +
					$"~INPUT_VEH_EXIT~ per accettare", 1);
				if (Input.IsControlJustPressed(Control.VehicleExit))
				{
					BaseScript.TriggerServerEvent("lprp:citofono:puoiEntrare", serverIdRichiedente, app);
					break;
				}					
			}
		}

		public static void PuoiEntrare(int serverIdInCasa, string appartamento)
		{
			KeyValuePair<string, ConfigCase> app = JsonConvert.DeserializeObject<KeyValuePair<string, ConfigCase>>(appartamento);
			var InCasa = Client.Instance.GetPlayers.ToList().FirstOrDefault(x => x.ServerId == serverIdInCasa);
			if(InCasa != null)
			{
				if(Game.PlayerPed.IsInRangeOf(app.Value.MarkerEntrata, 3f))
				{
					if(!Game.Player.GetPlayerData().Istanza.Stanziato)
					{
						Game.Player.GetPlayerData().Istanza.Istanzia(InCasa.Character.NetworkId, app.Key);
					}
				}
			}
		}

	}
}
