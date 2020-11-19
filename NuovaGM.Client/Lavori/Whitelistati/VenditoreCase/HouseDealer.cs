using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Client.MenuNativo;
using NuovaGM.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logger;

namespace NuovaGM.Client.Lavori.Whitelistati.VenditoreCase
{
	static class HouseDealer
	{
		private static ConfigVenditoriCase house;
		public static void Init()
		{
			house = Client.Impostazioni.Lavori.VenditoriCase;
			Client.Instance.AddEventHandler("lprp:onPlayerSpawn", new Action(Spawnato));
		}

		private static async Task Markers()
		{
			Ped p = Game.PlayerPed;
			if (!Game.Player.GetPlayerData().Istanza.Stanziato)
			{
				World.DrawMarker(MarkerType.VerticalCylinder, house.Config.Ingresso, Vector3.Zero, Vector3.Zero, new Vector3(1.375f, 1.375f, 0.4f), Colors.Blue);
				if (p.IsInRangeOf(house.Config.Ingresso, 1.375f))
				{
					if (Game.Player.GetPlayerData().CurrentChar.job.name.ToLower() == "venditorecase")
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per entrare in ufficio");
					else
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per entrare nell'ufficio dell'agenzia immobiliare");
					if (Input.IsControlJustPressed(Control.Context))
					{
						Funzioni.Teleport(house.Config.Dentro);
						Game.Player.GetPlayerData().Istanza.Istanzia("VenditoreCase");
					}
				}
			}
			else 
			{
				if (Game.Player.GetPlayerData().Istanza.Instance == "VenditoreCase")
				{
					World.DrawMarker(MarkerType.VerticalCylinder, house.Config.Uscita, Vector3.Zero, Vector3.Zero, new Vector3(1.375f, 1.375f, 0.4f), Colors.Red);
					if (p.IsInRangeOf(house.Config.Uscita, 1.375f))
					{
						if (Game.Player.GetPlayerData().CurrentChar.job.name.ToLower() == "venditorecase")
							HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per uscire dall'ufficio");
						else
							HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per uscire dall'ufficio dell'agenzia immobiliare");
						if (Input.IsControlJustPressed(Control.Context))
						{
							Funzioni.Teleport(house.Config.Fuori);
							Game.Player.GetPlayerData().Istanza.RimuoviIstanza();
						}
					}
				}
			}


			if (Game.Player.GetPlayerData().CurrentChar.job.name.ToLower() == "venditorecase")
			{
				// verrà cambiato con il sedersi alla scrivania
				if (p.IsInRangeOf(house.Config.Actions, 1.375f))
				{
					HUD.ShowHelp("~INPUT_CONTEXT~ Apri il menu di vendita");
					if (Input.IsControlJustPressed(Control.Context) && !HUD.MenuPool.IsAnyMenuOpen)
						MenuVenditoreCase();
				}
			}
		}

		private static async void MenuVenditoreCase()
		{
			UIMenu venditore = new UIMenu("Agenzia Immobiliare", "Abbiamo la casa per tutte le esigenze!");
			HUD.MenuPool.Add(venditore);
			Dictionary<string, ConfigCase> Appartamenti = Client.Impostazioni.Proprieta.Appartamenti;
			Dictionary<string, Garages> Garages = Client.Impostazioni.Proprieta.Garages.Garages;
			UIMenu appart = venditore.AddSubMenu("Appartamenti");
			UIMenu gara = venditore.AddSubMenu("Garages");
			foreach (var app in Appartamenti.OrderBy(x=>x.Value.Price))
			{
				UIMenu appartamento = appart.AddSubMenu(app.Value.Label);
				appartamento.ParentItem.SetRightLabel("Rif. ~g~$" + app.Value.Price);
				appartamento.OnMenuOpen += async (menu) =>
				{
					menu.Clear();
					var players = Funzioni.GetPlayersInArea(Game.Player.GetPlayerData().posizione.ToVector3(), 3.5f, false);
					foreach(var p in players)
					{
						UIMenu persona = menu.AddSubMenu(p.GetPlayerData().FullName);
						UIMenuListItem mostra = new UIMenuListItem("Mostra Appartamento", new List<dynamic>() { "Nulla", "Esterno", "Interno", "Bagno", "Garage" }, 0);
						UIMenuItem affitta = new UIMenuItem("Affitta");
						UIMenuItem vendi = new UIMenuItem("Vendi");
						persona.AddItem(mostra);
						persona.AddItem(affitta);
						persona.AddItem(vendi);
						Camera cam = World.CreateCamera(Vector3.Zero, Vector3.Zero, GameplayCamera.FieldOfView);
						persona.OnMenuOpen += async (_menu) =>
						{
							cam = World.CreateCamera(Vector3.Zero, Vector3.Zero, GameplayCamera.FieldOfView);
						};
						mostra.OnListChanged += async (_item, _index) =>
						{
							switch (_index)
							{
								case 0:
									Screen.Fading.FadeOut(800);
									await BaseScript.Delay(1000);
									cam.Position = Vector3.Zero;
									cam.Rotation = Vector3.Zero;
									RenderScriptCams(false, false, 1000, false, false);
									Screen.Fading.FadeIn(500);
									break;
								case 1:
									Screen.Fading.FadeOut(500);
									await BaseScript.Delay(600);
									RequestCollisionAtCoord(app.Value.TelecameraFuori.pos.X, app.Value.TelecameraFuori.pos.Y, app.Value.TelecameraFuori.pos.Z);
									RequestAdditionalCollisionAtCoord(app.Value.TelecameraFuori.pos.X, app.Value.TelecameraFuori.pos.Y, app.Value.TelecameraFuori.pos.Z);
									NewLoadSceneStart(app.Value.TelecameraFuori.pos.X, app.Value.TelecameraFuori.pos.Y, app.Value.TelecameraFuori.pos.Z, app.Value.TelecameraFuori.pos.X, app.Value.TelecameraFuori.pos.Y, app.Value.TelecameraFuori.pos.Z, 50f, 0);
									int tempTimer0 = GetGameTimer();
									while (IsNetworkLoadingScene())
									{
										if (GetGameTimer() - tempTimer0 > 3000) break;
										await BaseScript.Delay(0);
									}
									cam.Position = app.Value.TelecameraFuori.pos;
									cam.PointAt(app.Value.TelecameraFuori.guarda);
									RenderScriptCams(true, false, 1000, false, false);
									Screen.Fading.FadeIn(500);
									break;
								case 2:
									Screen.Fading.FadeOut(500);
									await BaseScript.Delay(600);
									RequestCollisionAtCoord(app.Value.TelecameraDentro.Interno.pos.X, app.Value.TelecameraDentro.Interno.pos.Y, app.Value.TelecameraDentro.Interno.pos.Z);
									RequestAdditionalCollisionAtCoord(app.Value.TelecameraDentro.Interno.pos.X, app.Value.TelecameraDentro.Interno.pos.Y, app.Value.TelecameraDentro.Interno.pos.Z);
									NewLoadSceneStart(app.Value.TelecameraDentro.Interno.pos.X, app.Value.TelecameraDentro.Interno.pos.Y, app.Value.TelecameraDentro.Interno.pos.Z, app.Value.TelecameraDentro.Interno.pos.X, app.Value.TelecameraDentro.Interno.pos.Y, app.Value.TelecameraDentro.Interno.pos.Z, 50f, 0);
									int tempTimer1 = GetGameTimer();
									while (IsNetworkLoadingScene())
									{
										if (GetGameTimer() - tempTimer1 > 3000) break;
										await BaseScript.Delay(0);
									}
									cam.Position = Vector3.Add(app.Value.TelecameraDentro.Interno.pos, new Vector3(0, 0, 1f));
									cam.PointAt(app.Value.TelecameraDentro.Interno.guarda);
									Screen.Fading.FadeIn(500);
									break;
								case 3:
									Screen.Fading.FadeOut(500);
									await BaseScript.Delay(600);
									RequestCollisionAtCoord(app.Value.TelecameraDentro.Bagno.pos.X, app.Value.TelecameraDentro.Bagno.pos.Y, app.Value.TelecameraDentro.Bagno.pos.Z);
									RequestAdditionalCollisionAtCoord(app.Value.TelecameraDentro.Bagno.pos.X, app.Value.TelecameraDentro.Bagno.pos.Y, app.Value.TelecameraDentro.Bagno.pos.Z);
									NewLoadSceneStart(app.Value.TelecameraDentro.Bagno.pos.X, app.Value.TelecameraDentro.Bagno.pos.Y, app.Value.TelecameraDentro.Bagno.pos.Z, app.Value.TelecameraDentro.Bagno.pos.X, app.Value.TelecameraDentro.Bagno.pos.Y, app.Value.TelecameraDentro.Bagno.pos.Z, 50f, 0);
									int tempTimer2 = GetGameTimer();
									while (IsNetworkLoadingScene())
									{
										if (GetGameTimer() - tempTimer2 > 3000) break;
										await BaseScript.Delay(0);
									}
									cam.Position = Vector3.Add(app.Value.TelecameraDentro.Bagno.pos, new Vector3(0, 0, 1f));
									cam.PointAt(app.Value.TelecameraDentro.Bagno.guarda);
									RenderScriptCams(true, false, 1000, false, false);
									Screen.Fading.FadeIn(500);
									break;
								case 4:
									Screen.Fading.FadeOut(500);
									await BaseScript.Delay(600);
									RequestCollisionAtCoord(app.Value.TelecameraDentro.Garage.pos.X, app.Value.TelecameraDentro.Garage.pos.Y, app.Value.TelecameraDentro.Garage.pos.Z);
									RequestAdditionalCollisionAtCoord(app.Value.TelecameraDentro.Garage.pos.X, app.Value.TelecameraDentro.Garage.pos.Y, app.Value.TelecameraDentro.Garage.pos.Z);
									NewLoadSceneStart(app.Value.TelecameraDentro.Garage.pos.X, app.Value.TelecameraDentro.Garage.pos.Y, app.Value.TelecameraDentro.Garage.pos.Z, app.Value.TelecameraDentro.Garage.pos.X, app.Value.TelecameraDentro.Garage.pos.Y, app.Value.TelecameraDentro.Garage.pos.Z, 50f, 0);
									int tempTimer3 = GetGameTimer();
									while (IsNetworkLoadingScene())
									{
										if (GetGameTimer() - tempTimer3 > 3000) break;
										await BaseScript.Delay(0);
									}
									cam.Position = app.Value.TelecameraDentro.Garage.pos;
									cam.PointAt(app.Value.TelecameraDentro.Garage.guarda);
									RenderScriptCams(true, false, 1000, false, false);
									Screen.Fading.FadeIn(500);
									break;
							}
						};
						affitta.Activated += async (_menu, _item) =>
						{
							string res = await HUD.GetUserInput("Inserisci prezzo d'affitto", "" + app.Value.Price, 10);
							if(string.IsNullOrEmpty(res) || string.IsNullOrWhiteSpace(res))
							{
								HUD.ShowNotification("Devi inserire almeno un valore!", NotificationColor.Red, true);
								return;
							}
							int aff = Convert.ToInt32(res);
							if (aff <= 0)
							{
								HUD.ShowNotification("Devi inserire un valore positivo!", NotificationColor.Red, true);
								return;
							}
							BaseScript.TriggerServerEvent("housedealer:vendi", false, p.ServerId, app.Serialize(), aff);
						};
						vendi.Activated += async (_menu, _item) =>
						{
							string res = await HUD.GetUserInput("Inserisci prezzo di vendita", "" + app.Value.Price, 10);
							if (string.IsNullOrEmpty(res) || string.IsNullOrWhiteSpace(res))
							{
								HUD.ShowNotification("Devi inserire almeno un valore!", NotificationColor.Red, true);
								return;
							}
							int aff = Convert.ToInt32(res);
							if (aff <= 0)
							{
								HUD.ShowNotification("Devi inserire un valore positivo!", NotificationColor.Red, true);
								return;
							}
							BaseScript.TriggerServerEvent("housedealer:vendi", true, p.ServerId, app.Serialize(), aff);
						};
						persona.OnMenuClose += async (_menu) =>
						{
							if (cam.IsActive && GetRenderingCam() == cam.Handle || cam.Position != Vector3.Zero)
							{
								Screen.Fading.FadeOut(800);
								await BaseScript.Delay(1000);
								RenderScriptCams(false, false, 1000, false, false);
								cam.Delete();
								Screen.Fading.FadeIn(500);
							}
						};
					}
				};
			}
			foreach (var gar in Garages)
			{
				UIMenu garage = gara.AddSubMenu(gar.Value.Label);
			}
			venditore.Visible = true;
		}

		private static void Spawnato()
		{
			Client.Instance.AddTick(Markers);
		}
	}
}
