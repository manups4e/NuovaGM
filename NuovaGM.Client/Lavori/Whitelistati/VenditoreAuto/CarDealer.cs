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
using System.Linq.Expressions;
using System.Threading.Tasks;
using Logger;
using NuovaGM.Shared.Veicoli;

namespace NuovaGM.Client.Lavori.Whitelistati.VenditoreAuto
{
	// void func_747(int iParam0) in carmod_shop.c
	static class CarDealer
	{
		private static ConfigVenditoriAuto carDealer;
		private static Vehicle PreviewVeh;
		public static void Init()
		{
			carDealer = Client.Impostazioni.Lavori.VenditoriAuto;
			Client.Instance.AddEventHandler("lprp:onPlayerSpawn", new Action(Spawnato));
			Client.Instance.AddEventHandler("lprp:cardealer:catalogoAlcuni", new Action<bool, List<int>>(CatalogoAlcuni));
			Client.Instance.AddEventHandler("lprp:cardealer:cambiaVehCatalogo", new Action<bool, string>(CambiaVehCatalogo));
		}

		private static void Spawnato()
		{
			Blip vend = World.CreateBlip(carDealer.Config.MenuVendita);
			vend.Sprite = BlipSprite.PersonalVehicleCar;
			vend.Color = BlipColor.Green;
			vend.IsShortRange = true;
			vend.Name = "Concessionaria";
		}


		public static async Task Markers()
		{
			if(Game.Player.GetPlayerData().CurrentChar.job.name.ToLower() == "cardealer")
			{
				// verrà sostiuito con il sedersi alla scrivania e mostrare al cliente
				if(Game.PlayerPed.IsInRangeOf(carDealer.Config.MenuVendita, 1.375f))
				{
					HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per aprire il menu del venditore");
					if (Input.IsControlJustPressed(Control.Context) && !HUD.MenuPool.IsAnyMenuOpen())
						MenuVenditore();
				}
			}
			if(Game.Player.GetPlayerData().CurrentChar.job.grade > 1)
			{
				// verrà sostiuito con il sedersi alla scrivania 
				if(Game.PlayerPed.IsInRangeOf(carDealer.Config.BossActions, 1.375f))
				{
					HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per aprire il menu boss");
					if (Input.IsControlJustPressed(Control.Context) && !HUD.MenuPool.IsAnyMenuOpen())
						MenuBoss();
				}
			}
		}


		private static async void MenuVenditore()
		{
			UIMenu menuVenditore = new UIMenu("Menu Venditore", "Tutto l'occorrente a portata di click");
			HUD.MenuPool.Add(menuVenditore);

			// la fattura sarà automatica all'acquisto da parte dell'acquirente (magari non ci sarà fattura ma acquisto automatico)
			// non sarà disponibile per l'affitto che partirà dalla consegna del veicolo

			UIMenuItem catalogoPriv = new UIMenuItem("Guarda catalogo", "Solo per te");
			menuVenditore.AddItem(catalogoPriv);
			catalogoPriv.Activated += async (menu, item) => 
			{
				Screen.Fading.FadeOut(800);
				await BaseScript.Delay(1000);
				MostraAMe();
			};
			UIMenu mostraCatalogo = menuVenditore.AddSubMenu("Mostra catalogo", "Scegli a chi");
			mostraCatalogo.OnMenuOpen += async (_menu) =>
			{
				_menu.Clear();
				List<Player> players = Funzioni.GetPlayersInArea(Game.PlayerPed.Position, 3f);
				List<string> texts = players.Select(x => x.GetPlayerData().FullName).ToList();
				string txt = "";
				foreach (var t in texts) txt = t + "~n~";
				UIMenu mostraCatalogoAlcuni = mostraCatalogo.AddSubMenu("Mostra a scelta");
				if (players.Count == 0)
				{
					mostraCatalogoAlcuni.ParentItem.Enabled = false;
					mostraCatalogoAlcuni.ParentItem.Description = "Non hai persone vicino!";
				}
				mostraCatalogoAlcuni.OnMenuOpen += async (_submenu) =>
				{
					_submenu.Clear();
					List<int> persone = new List<int>();
					foreach(var p in players)
					{
						UIMenuCheckboxItem persona = new UIMenuCheckboxItem(p.GetPlayerData().FullName, false);
						persona.CheckboxEvent += (_item, _activated) =>
						{
							if (_activated)
								if (!persone.Contains(p.ServerId)) persone.Add(p.ServerId);
							else
								if (persone.Contains(p.ServerId)) persone.Remove(p.ServerId);
						};
					}
					UIMenuItem mostra = new UIMenuItem("Mostra a selezionati", "", Colors.GreenDark, Colors.GreenLight);
					_submenu.AddItem(mostra);
					mostra.Activated += (menu, item) =>
					{
						if (persone.Count == 0)
						{
							HUD.ShowNotification("Non hai selezionato nessuno!");
							return;
						}
						BaseScript.TriggerEvent("lprp:cardealer:attivaCatalogoAlcuni", persone);
					};
				};
			};
			UIMenu riacquista = menuVenditore.AddSubMenu("Acquista veicolo usato");
			if(Game.Player.GetPlayerData().CurrentChar.job.grade < 2)
			{
				riacquista.ParentItem.Enabled = false;
				riacquista.ParentItem.Description = "Solo i capi possono acquistare i veicoli usati!";
			}
			menuVenditore.Visible = true;
		}
		private static async void MenuBoss()
		{

		}

		private static async void MostraAMe()
		{
			HUD.MenuPool.CloseAllMenus();
			LoadInterior(146433);
			SetInteriorActive(146433, true);
			RequestCollisionAtCoord(230.2893f, -996.1444f, -96.08697f);
			Camera cam = World.CreateCamera(new Vector3(230.2893f, -996.1444f, -96.08697f), Vector3.Zero, 45f);
			World.RenderingCamera = cam;
			cam.Position = new Vector3(230.2893f, -996.1444f, -98.08697f);
			cam.PointAt(new Vector3(228.9409f, -989.8207f, -99.99992f));

			UIMenu catalogo = new UIMenu("Catalogo concessionaria", "Il tuo catalogo di fiducia");
			HUD.MenuPool.Add(catalogo);
			Dictionary<string, List<VeicoloCatalogoVenditore>> Catalogo = new Dictionary<string, List<VeicoloCatalogoVenditore>>();
			foreach(var p in carDealer.Catalogo.Keys.OrderBy(k => k).ToDictionary(k => k, T1 => carDealer.Catalogo[T1]))
			{
				UIMenu sezione = catalogo.AddSubMenu(p.Key);
				sezione.AddInstructionalButton(new InstructionalButton(Control.ParachuteBrakeLeft, "Apri/Chiudi veicolo"));
				List<VeicoloCatalogoVenditore> vehs = new List<VeicoloCatalogoVenditore>();
				foreach(var i in p.Value.OrderBy(x => x.price))
				{
					UIMenu vah = sezione.AddSubMenu(Game.GetGXTEntry(i.name), i.description);
					vah.ParentItem.SetRightLabel("~g~$" + i.price);
					vehs.Add(i);
					UIMenu colore1 = vah.AddSubMenu("Colore primario");
					UIMenu colore2 = vah.AddSubMenu("Colore secondario");

					for (int l=0; l<Enum.GetValues(typeof(VehicleColor)).Length; l++)
					{
						UIMenuItem colo = new UIMenuItem(Funzioni.GetVehColorLabel(l));
						colore1.AddItem(colo);
						colore2.AddItem(colo);
					}


				}
				sezione.OnIndexChange += async (menu, index) =>
				{
					PreviewVeh = await Funzioni.SpawnLocalVehicle(vehs[index].name, new Vector3(228.9409f, -989.8207f, -99.99992f), -180f);
					cam.PointAt(PreviewVeh);
					PreviewVeh.IsEngineRunning = true;
					PreviewVeh.AreLightsOn = true;
					PreviewVeh.IsInteriorLightOn = true;
					PreviewVeh.IsPositionFrozen = true;
					PreviewVeh.IsLeftIndicatorLightOn = true;
					PreviewVeh.IsRightIndicatorLightOn = true;
				};
				sezione.OnItemSelect += async (menu, item, index) =>
				{

					var veh = vehs[index].name;
					if (Game.Player.GetPlayerData().CurrentChar.Proprietà.Any(x => Client.Impostazioni.Proprieta.Garages.Garages.ContainsKey(x) || (Client.Impostazioni.Proprieta.Appartamenti.GroupBy(l => l.Value.GarageIncluso == true) as Dictionary<string, ConfigCase>).ContainsKey(x)))
					{
						string plate = Funzioni.GetRandomString(2) + " " + Funzioni.GetRandomInt(999) + Funzioni.GetRandomString(2);
						OwnedVehicle veicolo = new OwnedVehicle(PreviewVeh, plate, new VehicleData(Game.Player.GetPlayerData().CurrentChar.info.insurance, await PreviewVeh.GetVehicleProperties(), false),true, "Normale");
						BaseScript.TriggerServerEvent("lprp:cardealer:vendiVehAMe", veicolo.Serialize());
					}
				};
				sezione.OnMenuClose += async (menu) =>
				{
					await BaseScript.Delay(100);
					if (catalogo.Visible)
					{
						Client.Instance.RemoveTick(RuotaVeh);
						PreviewVeh.Delete();
						cam.PointAt(new Vector3(228.9409f, -989.8207f, -99.99992f));
					}
				};
			}

			catalogo.OnMenuClose += async (menu) =>
			{
				await BaseScript.Delay(100);
				if (!HUD.MenuPool.IsAnyMenuOpen())
				{
					Screen.Fading.FadeOut(800);
					await BaseScript.Delay(1000);
					World.RenderingCamera = null;
					cam.Delete();
					Screen.Fading.FadeIn(800);
				}
			};
			Screen.Fading.FadeIn(800);
			Client.Instance.AddTick(RuotaVeh);
			catalogo.Visible = true;
		}

		private static async void CatalogoAlcuni(bool venditore, List<int> players)
		{
			HUD.MenuPool.CloseAllMenus();
			LoadInterior(146433);
			SetInteriorActive(146433, true);
			RequestCollisionAtCoord(230.2893f, -996.1444f, -96.08697f);
			Camera cam = World.CreateCamera(new Vector3(230.2893f, -996.1444f, -96.08697f), Vector3.Zero, 45f);
			World.RenderingCamera = cam;
			cam.Position = new Vector3(230.2893f, -996.1444f, -98.08697f);
			cam.PointAt(new Vector3(228.9409f, -989.8207f, -99.99992f));
			if (venditore)
			{
				UIMenu catalogo = new UIMenu("Catalogo concessionaria", "Il tuo catalogo di fiducia");
				HUD.MenuPool.Add(catalogo);
				Dictionary<string, List<VeicoloCatalogoVenditore>> Catalogo = new Dictionary<string, List<VeicoloCatalogoVenditore>>();
				foreach (var p in carDealer.Catalogo.Keys.OrderBy(k => k).ToDictionary(k => k, T1 => carDealer.Catalogo[T1]))
				{
					UIMenu sezione = catalogo.AddSubMenu(p.Key);
					sezione.AddInstructionalButton(new InstructionalButton(Control.ParachuteBrakeLeft, "Apri/Chiudi veicolo"));
					List<VeicoloCatalogoVenditore> vehs = new List<VeicoloCatalogoVenditore>();
					foreach (var i in p.Value.OrderBy(x => x.price))
					{
						UIMenu vah = sezione.AddSubMenu(Game.GetGXTEntry(i.name), i.description);
						vah.ParentItem.SetRightLabel("~g~$" + i.price);
						vehs.Add(i);
						foreach(var pl in players)
						{
							var user = Client.Instance.GetPlayers.ToList().FirstOrDefault(x => x.ServerId == pl);
							UIMenu player = vah.AddSubMenu(user.GetPlayerData().FullName);
							if (user.GetPlayerData().CurrentChar.Proprietà.Any(x => Client.Impostazioni.Proprieta.Garages.Garages.ContainsKey(x) || (Client.Impostazioni.Proprieta.Appartamenti.GroupBy(l => l.Value.GarageIncluso == true) as Dictionary<string, ConfigCase>).ContainsKey(x)))
							{
								List<string> prop = new List<string>();
								foreach(var gar in user.GetPlayerData().CurrentChar.Proprietà)
								{
									if (Client.Impostazioni.Proprieta.Garages.Garages.ContainsKey(gar)) 
									{
										UIMenuItem posto = new UIMenuItem(Client.Impostazioni.Proprieta.Garages.Garages[gar].Label);
										player.AddItem(posto);
									}
									else if (Client.Impostazioni.Proprieta.Appartamenti.ContainsKey(gar) && Client.Impostazioni.Proprieta.Appartamenti[gar].GarageIncluso)
									{
										UIMenuItem posto = new UIMenuItem(Client.Impostazioni.Proprieta.Appartamenti[gar].Label);
										player.AddItem(posto);
									}
									player.OnItemSelect += async (menu, item, index) =>
									{
										BaseScript.TriggerServerEvent("lprp:carDealer:vendi", user.ServerId, item.Text); // fare controllo lato server sulla proprietà (se è un garage o un appartamento)
									};
								}
							}
							else
							{
								player.ParentItem.Enabled = false;
								player.ParentItem.Description = "Questa persona non ha proprietà con garage!";
							}
						}
					}
					sezione.OnIndexChange += async (menu, index) =>
					{
						BaseScript.TriggerServerEvent("lprp:cardealer:cambiaVehCatalogo", players, vehs[index].name);
					};
					sezione.OnMenuClose += async (menu) =>
					{
						Client.Instance.RemoveTick(RuotaVeh);
						PreviewVeh.Delete();
						cam.PointAt(new Vector3(228.9409f, -989.8207f, -99.99992f));
					};
				}

				catalogo.OnMenuClose += async (menu) =>
				{
					await BaseScript.Delay(100);
					if (!HUD.MenuPool.IsAnyMenuOpen())
					{
						Screen.Fading.FadeOut(800);
						await BaseScript.Delay(1000);
						World.RenderingCamera = null;
						cam.Delete();
						Screen.Fading.FadeIn(800);
					}
				};
				catalogo.Visible = true;
			}
			Client.Instance.AddTick(RuotaVeh);
			Screen.Fading.FadeIn(800);
		}

		private static async void CambiaVehCatalogo(bool venditore, string name)
		{
			PreviewVeh = await Funzioni.SpawnLocalVehicle(name, new Vector3(228.9409f, -989.8207f, -99.99992f), -180f);
			PreviewVeh.IsEngineRunning = true;
			PreviewVeh.AreLightsOn = true;
			PreviewVeh.IsInteriorLightOn = true;
			PreviewVeh.IsPositionFrozen = true;
			PreviewVeh.IsLeftIndicatorLightOn = true;
			PreviewVeh.IsRightIndicatorLightOn = true;
		}


		private static async Task RuotaVeh()
		{
			if (PreviewVeh != null && PreviewVeh.Exists())
			{
				Game.DisableControlThisFrame(0, Control.ParachuteBrakeLeft);
				PreviewVeh.Heading += 0.2f;
				if (Input.IsDisabledControlJustPressed(Control.ParachuteBrakeLeft))
				{
					if (PreviewVeh.Model.IsCar)
					{
						PreviewVeh.IsPositionFrozen = false;
						foreach (var d in PreviewVeh.Doors.GetAll())
						{
							d.CanBeBroken = false;
							if (d.IsOpen) d.Close();
							else d.Open();
							await BaseScript.Delay(500);
						}
						PreviewVeh.IsPositionFrozen = true;
					}
				}
			}
		}
	}
}
