using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.NativeUI;
using TheLastPlanet.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Impostazioni.Client.Configurazione.Lavori.WhiteList;
using Logger;
using TheLastPlanet.Shared.Veicoli;
using TheLastPlanet.Client.Core;
using TheLastPlanet.Client.SessionCache;

namespace TheLastPlanet.Client.RolePlay.Lavori.Whitelistati.VenditoreAuto
{
	// void func_747(int iParam0) in carmod_shop.c
	internal static class CarDealer
	{
		private static ConfigVenditoriAuto carDealer;
		private static Vehicle PreviewVeh;

		public static void Init()
		{
			carDealer = Client.Impostazioni.RolePlay.Lavori.VenditoriAuto;
			Client.Instance.AddEventHandler("lprp:onPlayerSpawn", new Action(Spawnato));
			Client.Instance.AddEventHandler("lprp:cardealer:catalogoAlcuni", new Action<bool, List<int>>(CatalogoAlcuni));
			Client.Instance.AddEventHandler("lprp:cardealer:cambiaVehCatalogo", new Action<bool, string>(CambiaVehCatalogo));
		}

		public static void Stop()
		{
			carDealer = null;
			Client.Instance.RemoveEventHandler("lprp:onPlayerSpawn", new Action(Spawnato));
			Client.Instance.RemoveEventHandler("lprp:cardealer:catalogoAlcuni", new Action<bool, List<int>>(CatalogoAlcuni));
			Client.Instance.RemoveEventHandler("lprp:cardealer:cambiaVehCatalogo", new Action<bool, string>(CambiaVehCatalogo));
		}

		private static void Spawnato()
		{
			Blip vend = World.CreateBlip(carDealer.Config.MenuVendita.ToVector3);
			vend.Sprite = BlipSprite.PersonalVehicleCar;
			vend.Color = BlipColor.Green;
			vend.IsShortRange = true;
			vend.Name = "Concessionaria";
		}

		public static async Task Markers()
		{
			Ped p = Cache.MyPlayer.Ped;

			if (Cache.MyPlayer.User.CurrentChar.Job.Name.ToLower() == "cardealer")
				// verrà sostiuito con il sedersi alla scrivania e mostrare al cliente
				if (p.IsInRangeOf(carDealer.Config.MenuVendita.ToVector3, 1.375f))
				{
					HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per aprire il menu del venditore");
					if (Input.IsControlJustPressed(Control.Context) && !HUD.MenuPool.IsAnyMenuOpen) MenuVenditore();
				}

			if (Cache.MyPlayer.User.CurrentChar.Job.Grade > 1)
				// verrà sostiuito con il sedersi alla scrivania 
				if (p.IsInRangeOf(carDealer.Config.BossActions.ToVector3, 1.375f))
				{
					HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per aprire il menu boss");
					if (Input.IsControlJustPressed(Control.Context) && !HUD.MenuPool.IsAnyMenuOpen) MenuBoss();
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
			List<Player> players = new();
			HUD.MenuPool.OnMenuStateChanged += async (_oldmenu, _newmenu, _state) =>
			{
				if (_newmenu != mostraCatalogo) return;
				_newmenu.Clear();
				players.Clear();
				players = Funzioni.GetPlayersInArea(Cache.MyPlayer.User.Posizione.ToVector3, 3f);
				List<string> texts = players.Select(x => x.GetPlayerData().FullName).ToList();
				string txt = "";
				foreach (string t in texts) txt = t + "~n~";
				UIMenu mostraCatalogoAlcuni = mostraCatalogo.AddSubMenu("Mostra a scelta");

				if (players.Count == 0)
				{
					mostraCatalogoAlcuni.ParentItem.Enabled = false;
					mostraCatalogoAlcuni.ParentItem.Description = "Non hai persone vicino!";
				}

				mostraCatalogo.OnMenuStateChanged += async (_oldsubmenu, _newsubmenu, _substate) =>
				{
					if (_substate != MenuState.ChangeForward) return;
					_newsubmenu.Clear();
					List<int> persone = new List<int>();

					foreach (Player p in players)
					{
						UIMenuCheckboxItem persona = new(p.GetPlayerData().FullName, false);
						persona.CheckboxEvent += (_item, _activated) =>
						{
							if (!_activated) return;
							if (!persone.Contains(p.ServerId))
								persone.Add(p.ServerId);
							else if (persone.Contains(p.ServerId)) persone.Remove(p.ServerId);
						};
					}

					UIMenuItem mostra = new UIMenuItem("Mostra a selezionati", "", Colors.GreenDark, Colors.GreenLight);
					_newsubmenu.AddItem(mostra);
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

			if (Cache.MyPlayer.User.CurrentChar.Job.Grade < 2)
			{
				riacquista.ParentItem.Enabled = false;
				riacquista.ParentItem.Description = "Solo i capi possono acquistare i veicoli usati!";
			}

			menuVenditore.Visible = true;
		}

		private static async void MenuBoss() { }

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
			string SelectedVeh = "";

			foreach (KeyValuePair<string, List<VeicoloCatalogoVenditore>> p in carDealer.Catalogo.Keys.OrderBy(k => k).ToDictionary(k => k, T1 => carDealer.Catalogo[T1]))
			{
				UIMenu sezione = catalogo.AddSubMenu(p.Key);
				sezione.InstructionalButtons.Add(new InstructionalButton(Control.ParachuteBrakeLeft, "Apri/Chiudi veicolo"));
				List<VeicoloCatalogoVenditore> vehs = new();

				foreach (VeicoloCatalogoVenditore i in p.Value.OrderBy(x => x.price))
				{
					UIMenu vah = sezione.AddSubMenu(Game.GetGXTEntry(i.name), i.description);
					vah.ParentItem.SetRightLabel("~g~$" + i.price);
					vehs.Add(i);
					UIMenu colore1 = vah.AddSubMenu("Colore primario");
					UIMenu colore2 = vah.AddSubMenu("Colore secondario");

					for (int l = 0; l < Enum.GetValues(typeof(VehicleColor)).Length; l++)
					{
						UIMenuItem colo = new UIMenuItem(Funzioni.GetVehColorLabel(l));
						colore1.AddItem(colo);
						colore2.AddItem(colo);
					}

					colore1.OnIndexChange += async (menu, index) => PreviewVeh.Mods.PrimaryColor = (VehicleColor)index;
					colore2.OnIndexChange += async (menu, index) => PreviewVeh.Mods.SecondaryColor = (VehicleColor)index;
					UIMenu prendi = vah.AddSubMenu("Prendi");
					prendi.OnMenuStateChanged += async (_oldsubmenu, _newsubmenu, _substate) =>
					{
						if (_newsubmenu != prendi) return;
						_newsubmenu.Clear();

						if (Cache.MyPlayer.User.CurrentChar.Proprietà.Any(x => Client.Impostazioni.RolePlay.Proprieta.Garages.Garages.ContainsKey(x) || Client.Impostazioni.RolePlay.Proprieta.Appartamenti.ContainsKey(x)))
						{
							foreach (KeyValuePair<string, ConfigCase> pro in Client.Impostazioni.RolePlay.Proprieta.Appartamenti)
								if (pro.Value.GarageIncluso)
									foreach (UIMenuItem c in from a in Cache.MyPlayer.User.CurrentChar.Proprietà where a == pro.Key select new UIMenuItem(pro.Value.Label))
									{
										c.SetRightLabel("" + Cache.MyPlayer.User.CurrentChar.Veicoli.Where(x => x.Garage.Garage == pro.Key).ToList().Count + "/" + Client.Impostazioni.RolePlay.Proprieta.Appartamenti[pro.Key].VehCapacity);
										prendi.AddItem(c);
										c.Activated += async (_menu_, _item_) =>
										{
											string s1 = Funzioni.GetRandomString(2);
											await BaseScript.Delay(100);
											string s2 = Funzioni.GetRandomString(2);
											string plate = s1 + " " + Funzioni.GetRandomInt(001, 999).ToString("000") + s2;
											PreviewVeh.Mods.LicensePlate = plate;
											VehProp prop = await PreviewVeh.GetVehicleProperties();
											OwnedVehicle veicolo = new OwnedVehicle(PreviewVeh, plate, new VehicleData(Cache.MyPlayer.User.CurrentChar.Info.insurance, prop, false), new VehGarage(true, pro.Key, Cache.MyPlayer.User.CurrentChar.Veicoli.Where(x => x.Garage.Garage == pro.Key).ToList().Count), "Normale");
											BaseScript.TriggerServerEvent("lprp:cardealer:vendiVehAMe", veicolo.ToJson(settings: JsonHelper.IgnoreJsonIgnoreAttributes));
											HUD.MenuPool.CloseAllMenus();
											HUD.ShowNotification($"Hai comprato il veicolo: ~y~{veicolo.DatiVeicolo.props.Name}~w~ al prezzo di ~g~${prendi.ParentMenu.ParentItem.RightLabel}~w~.");
											Screen.Fading.FadeOut(800);
											await BaseScript.Delay(1000);
											World.RenderingCamera = null;
											cam.Delete();
											Screen.Fading.FadeIn(800);
										};
									}
						}
						else
						{
							UIMenuItem no = new UIMenuItem("Non hai appartamenti o garage!!");
							prendi.AddItem(no);
						}
					};
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
				sezione.OnItemSelect += async (menu, item, index) => SelectedVeh = vehs[index].name;
				HUD.MenuPool.OnMenuStateChanged += async delegate(UIMenu oldmenu, UIMenu newmenu, MenuState state)
				{
					switch (state)
					{
						case MenuState.Opened when newmenu == sezione:
							Client.Instance.AddTick(RuotaVeh);

							break;
						case MenuState.Closed when oldmenu == sezione:
						{
							await BaseScript.Delay(100);

							if (catalogo.Visible)
							{
								Client.Instance.RemoveTick(RuotaVeh);
								PreviewVeh.Delete();
								cam.PointAt(new Vector3(228.9409f, -989.8207f, -99.99992f));
							}

							break;
						}
					}
				};
			}

			catalogo.OnMenuStateChanged += async (oldmenu, newmenu, state) =>
			{
				if (state != MenuState.Closed || oldmenu != catalogo) return;
				await BaseScript.Delay(100);

				if (HUD.MenuPool.IsAnyMenuOpen) return;
				Screen.Fading.FadeOut(800);
				await BaseScript.Delay(1000);
				World.RenderingCamera = null;
				cam.Delete();
				Screen.Fading.FadeIn(800);
			};
			Screen.Fading.FadeIn(800);
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

				foreach (KeyValuePair<string, List<VeicoloCatalogoVenditore>> p in carDealer.Catalogo.Keys.OrderBy(k => k).ToDictionary(k => k, T1 => carDealer.Catalogo[T1]))
				{
					UIMenu sezione = catalogo.AddSubMenu(p.Key);
					sezione.InstructionalButtons.Add(new InstructionalButton(Control.ParachuteBrakeLeft, "Apri/Chiudi veicolo"));
					List<VeicoloCatalogoVenditore> vehs = new List<VeicoloCatalogoVenditore>();

					foreach (VeicoloCatalogoVenditore i in p.Value.OrderBy(x => x.price))
					{
						UIMenu vah = sezione.AddSubMenu(Game.GetGXTEntry(i.name), i.description);
						vah.ParentItem.SetRightLabel("~g~$" + i.price);
						vehs.Add(i);

						foreach (int pl in players)
						{
							Player user = Client.Instance.GetPlayers.ToList().FirstOrDefault(x => x.ServerId == pl);
							UIMenu player = vah.AddSubMenu(user.GetPlayerData().FullName);

							if (user.GetPlayerData().CurrentChar.Proprietà.Any(x => Client.Impostazioni.RolePlay.Proprieta.Garages.Garages.ContainsKey(x) || (Client.Impostazioni.RolePlay.Proprieta.Appartamenti.GroupBy(l => l.Value.GarageIncluso == true) as Dictionary<string, ConfigCase>).ContainsKey(x)))
							{
								List<string> prop = new();

								foreach (string gar in user.GetPlayerData().CurrentChar.Proprietà)
								{
									if (Client.Impostazioni.RolePlay.Proprieta.Garages.Garages.ContainsKey(gar))
									{
										UIMenuItem posto = new(Client.Impostazioni.RolePlay.Proprieta.Garages.Garages[gar].Label);
										player.AddItem(posto);
									}
									else if (Client.Impostazioni.RolePlay.Proprieta.Appartamenti.ContainsKey(gar) && Client.Impostazioni.RolePlay.Proprieta.Appartamenti[gar].GarageIncluso)
									{
										UIMenuItem posto = new(Client.Impostazioni.RolePlay.Proprieta.Appartamenti[gar].Label);
										player.AddItem(posto);
									}

									player.OnItemSelect += (menu, item, index) => BaseScript.TriggerServerEvent("lprp:carDealer:vendi", user.ServerId, item.Text);
								}
							}
							else
							{
								player.ParentItem.Enabled = false;
								player.ParentItem.Description = "Questa persona non ha proprietà con garage!";
							}
						}
					}

					sezione.OnIndexChange += (menu, index) => BaseScript.TriggerServerEvent("lprp:cardealer:cambiaVehCatalogo", players, vehs[index].name);
					sezione.OnMenuStateChanged += (oldmenu, newmenu, state) =>
					{
						if (state != MenuState.ChangeBackward || oldmenu != sezione) return;
						Client.Instance.RemoveTick(RuotaVeh);
						PreviewVeh.Delete();
						cam.PointAt(new Vector3(228.9409f, -989.8207f, -99.99992f));
					};
				}

				catalogo.OnMenuStateChanged += async (oldmenu, newmenu, state) =>
				{
					if (state != MenuState.Closed || oldmenu != catalogo) return;
					await BaseScript.Delay(100);

					if (HUD.MenuPool.IsAnyMenuOpen) return;
					Screen.Fading.FadeOut(800);
					await BaseScript.Delay(1000);
					World.RenderingCamera = null;
					cam.Delete();
					Screen.Fading.FadeIn(800);
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
					if (PreviewVeh.Model.IsCar)
					{
						PreviewVeh.IsPositionFrozen = false;

						foreach (VehicleDoor d in PreviewVeh.Doors.GetAll())
						{
							d.CanBeBroken = false;
							if (d.IsOpen)
								d.Close();
							else
								d.Open();
							await BaseScript.Delay(500);
						}

						PreviewVeh.IsPositionFrozen = true;
					}
			}
		}
	}
}