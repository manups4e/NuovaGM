using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using static CitizenFX.Core.Native.API;
using TheLastPlanet.Client.Core;
using TheLastPlanet.Client.Core.Personaggio;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.MenuNativo;
using TheLastPlanet.Client.Veicoli;
using Newtonsoft.Json;
using TheLastPlanet.Shared;

namespace TheLastPlanet.Client.Lavori.Generici.Pescatore
{
	// NON E PIU UN LAVORO... LIBERO PER TUTTI.. CREARE PUNTI DI AFFITTO BARCHE PER CHI LE VUOLE..
	// CREARE PUNTI GENERICI DI VENDITA DEL PESCE, E PUNTI GENERICI DI ATTACCO / STACCO BARCHE PER CHI LE POSSIEDE

	internal static class PescatoreClient
	{
		private static Pescatori PuntiPesca;
		private static string scenario = "WORLD_HUMAN_STAND_FISHING";
		private static string AnimDict = "amb@world_human_stand_fishing@base";
		private static bool LavoroAccettato = false;
		private static Vehicle lastVehicle;
		public static bool Pescando = false;
		public static bool CannaInMano = false;
		private static int TipoCanna = -1;
		private static Prop CannaDaPesca;
		private static bool mostrablip = false;
		private static List<string> PerVendereIlPesce = new List<string>()
		{
			"branzino",
			"sgombro",
			"sogliola",
			"orata",
			"tonno",
			"salmone",
			"merluzzo",
			"pescespada",
			"squalo",
			"fruttidimare",
			"carpa",
			"luccio",
			"persico",
			"pescegattocomune",
			"pescegattopunteggiato",
			"spigola",
			"trota",
			"ghiozzo",
			"lucioperca",
			"alborella",
			"carassio",
			"carassiodorato",
			"cheppia",
			"rovella",
			"spinarello",
			"storionecobice",
			"storionecomune",
			"storioneladano"
		};
		private static List<Blip> venditaPesceBlip = new List<Blip>();

		private static Vector3 LuogoDiPesca = new Vector3(0);
		// oggetti: canna da pesca, esche, pesci, frutti di mare magari, gamberi.. crostacei
		// considerare spogliatoio (obbligatorio / opzionale)

		// N_0xc54a08c85ae4d410 mentre peschi (0.0 normale, 1.0 acqua liscia, 3.0 acqua mossa)
		// benson per portare il pesce

		public static async void Init()
		{
			PuntiPesca = Client.Impostazioni.Lavori.Generici.Pescatore;
			ConfigShared.SharedConfig.Main.Generici.ItemList["cannadapescabase"].Usa += async (item, index) =>
			{
				RequestAnimDict(AnimDict);
				RequestAnimDict("amb@code_human_wander_drinking@beer@male@base");
				HUD.MenuPool.CloseAllMenus();
				CannaDaPesca = new Prop(CreateObject((int)item.prop, 1729.73f, 6403.90f, 34.56f, true, true, true));
				AttachEntityToEntity(CannaDaPesca.Handle, PlayerPedId(), GetPedBoneIndex(PlayerPedId(), /*60309*/ 57005), 0.10f, 0, -0.001f, 80.0f, 150.0f, 200.0f, false, false, false, false, 1, true);
				TaskPlayAnim(PlayerPedId(), "amb@code_human_wander_drinking@beer@male@base", "static", 3.5f, -8, -1, 49, 0, false, false, false);
				CannaInMano = true;
				TipoCanna = 0;
				Client.Instance.AddTick(Pesca);
				RemoveAnimDict("amb@code_human_wander_drinking@beer@male@base");
			};
			ConfigShared.SharedConfig.Main.Generici.ItemList["cannadapescamedia"].Usa += async (item, index) =>
			{
				RequestAnimDict(AnimDict);
				RequestAnimDict("amb@code_human_wander_drinking@beer@male@base");
				HUD.MenuPool.CloseAllMenus();
				CannaDaPesca = new Prop(CreateObject((int)item.prop, 1729.73f, 6403.90f, 34.56f, true, true, true));
				AttachEntityToEntity(CannaDaPesca.Handle, PlayerPedId(), GetPedBoneIndex(PlayerPedId(), /*60309*/ 57005), 0.10f, 0, -0.001f, 80.0f, 150.0f, 200.0f, false, false, false, false, 1, true);
				TaskPlayAnim(PlayerPedId(), "amb@code_human_wander_drinking@beer@male@base", "static", 3.5f, -8, -1, 49, 0, false, false, false);
				CannaInMano = true;
				TipoCanna = 1;
				Client.Instance.AddTick(Pesca);
				RemoveAnimDict("amb@code_human_wander_drinking@beer@male@base");
			};
			ConfigShared.SharedConfig.Main.Generici.ItemList["cannadapescaavanzata"].Usa += async (item, index) =>
			{
				RequestAnimDict(AnimDict);
				RequestAnimDict("amb@code_human_wander_drinking@beer@male@base");
				HUD.MenuPool.CloseAllMenus();
				CannaDaPesca = new Prop(CreateObject((int)item.prop, 1729.73f, 6403.90f, 34.56f, true, true, true));
				AttachEntityToEntity(CannaDaPesca.Handle, PlayerPedId(), GetPedBoneIndex(PlayerPedId(), /*60309*/ 57005), 0.10f, 0, -0.001f, 80.0f, 150.0f, 200.0f, false, false, false, false, 1, true);
				TaskPlayAnim(PlayerPedId(), "amb@code_human_wander_drinking@beer@male@base", "static", 3.5f, -8, -1, 49, 0, false, false, false);
				CannaInMano = true;
				TipoCanna = 2;
				Client.Instance.AddTick(Pesca);
				RemoveAnimDict("amb@code_human_wander_drinking@beer@male@base");
			};
		}

		public static async Task ControlloPesca()
		{
			Ped p = Cache.PlayerPed;

			if (Main.spawned)
			{
				if (PerVendereIlPesce.Any(o => Cache.Char.getInventoryItem(o).Item1))
				{
					if (!mostrablip)
					{
						foreach (Vector3 punto in PuntiPesca.LuoghiVendita)
						{
							Blip puntovendita = new Blip(AddBlipForCoord(punto[0], punto[1], punto[2]))
							{
								Sprite = BlipSprite.TowTruck,
								Name = "Vendita pesce",
								Color = BlipColor.MichaelBlue,
								Scale = 0.8f,
								IsShortRange = true
							};
							SetBlipDisplay(puntovendita.Handle, 4);
							venditaPesceBlip.Add(puntovendita);
						}

						mostrablip = true;
					}

					foreach (Vector3 punto in PuntiPesca.LuoghiVendita.Where(punto => p.IsInRangeOf(punto, 80)))
					{
						World.DrawMarker(MarkerType.DollarSign, punto, new Vector3(0), new Vector3(0), new Vector3(2.0f, 2.0f, 2.0f), Colors.DarkSeaGreen, false, false, true);

						if (!p.IsInRangeOf(punto, 2)) continue;
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per vendere il pesce che possiedi");
						if (Input.IsControlJustPressed(Control.Context) && !HUD.MenuPool.IsAnyMenuOpen) ApriMenuVenditaPesce();
					}
				}
				else
				{
					if (mostrablip)
					{
						foreach (Blip blip in venditaPesceBlip.Where(blip => blip.Exists())) blip.Delete();
						venditaPesceBlip.Clear();
						mostrablip = false;
					}
				}

				/*			if (Vector3.Distance(Cache.Char.posizione.ToVector3(), PuntiPesca.AffittoBarca) < 2f && !Cache.Char.StatiPlayer.InVeicolo)
							{
								HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per scegliere una ~b~barca~w~.");
								if (Input.IsControlJustPressed(Control.Context))
								{
									MenuBarche();
								}
							}
				*/
			}
		}

		private static async Task ApriMenuVenditaPesce()
		{
			DateTime oggi = new DateTime();

			if (oggi.DayOfWeek == DayOfWeek.Monday || oggi.DayOfWeek == DayOfWeek.Wednesday || oggi.DayOfWeek == DayOfWeek.Friday)
			{
				UIMenu venditaPesce = new UIMenu("Vendita pesce fresco", "Vendi qui e guadagna di più");
				HUD.MenuPool.Add(venditaPesce);
				List<Inventory> inventario = Cache.Char.CurrentChar.inventory;

				foreach (Inventory inv in inventario)
				{
					foreach (List<dynamic> amountino in from s in PerVendereIlPesce where inv.item == s select new List<dynamic>())
					{
						for (int j = 0; j < inv.amount; j++) amountino.Add((j + 1).ToString());
						UIMenuListItem pesce = new UIMenuListItem(ConfigShared.SharedConfig.Main.Generici.ItemList[inv.item].label, amountino, 0, ConfigShared.SharedConfig.Main.Generici.ItemList[inv.item].description);
						venditaPesce.AddItem(pesce);
						pesce.OnListSelected += async (item, index) =>
						{
							string quantita = item.Items[item.Index].ToString();
							int perc = 0;
							if (Convert.ToInt32(quantita) > 9 && Convert.ToInt32(quantita) < 20)
								perc = 2;
							else if (Convert.ToInt32(quantita) > 19 && Convert.ToInt32(quantita) < 30)
								perc = 4;
							else if (Convert.ToInt32(quantita) > 29 && Convert.ToInt32(quantita) < 40)
								perc = 6;
							else if (Convert.ToInt32(quantita) > 39 && Convert.ToInt32(quantita) < 50)
								perc = 8;
							else if (Convert.ToInt32(quantita) > 49 && Convert.ToInt32(quantita) < 60)
								perc = 10;
							else if (Convert.ToInt32(quantita) > 59 && Convert.ToInt32(quantita) < 70)
								perc = 12;
							else if (Convert.ToInt32(quantita) > 69 && Convert.ToInt32(quantita) < 80)
								perc = 14;
							else if (Convert.ToInt32(quantita) > 79 && Convert.ToInt32(quantita) < 90)
								perc = 16;
							else if (Convert.ToInt32(quantita) > 89 && Convert.ToInt32(quantita) < 100)
								perc = 18;
							else if (Convert.ToInt32(quantita) > 99) perc = 20;
							int valoreAggiunto = ConfigShared.SharedConfig.Main.Generici.ItemList[inv.item].sellPrice + ConfigShared.SharedConfig.Main.Generici.ItemList[inv.item].sellPrice * (perc + (int)Math.Round(Cache.Char.CurrentChar.statistiche.FISHING / 10)) / 100;
							BaseScript.TriggerServerEvent("lprp:removeIntenvoryItem", inv.item, Convert.ToInt32(quantita));
							BaseScript.TriggerServerEvent("lprp:givemoney", valoreAggiunto * Convert.ToInt32(quantita));
						};
					}
				}

				venditaPesce.Visible = true;
			}
			else
			{
				HUD.ShowNotification("Il mercato Ittico è chiuso oggi torna quando siamo aperti!!");
			}
		}

		public static async Task Pesca()
		{
			if (CannaInMano)
			{
				Game.DisableControlThisFrame(0, Control.FrontendX);
				Game.DisableControlThisFrame(0, Control.FrontendY);
				if (!Pescando) HUD.ShowHelp("Premi ~INPUT_FRONTEND_X~ per iniziare a pescare.~n~Premi ~INPUT_FRONTEND_Y~ per posare la canna da pesca");

				if (Input.IsDisabledControlJustPressed(Control.FrontendX))
				{
					float altezza = 0;

					if (GetWaterHeightNoWaves(Cache.Char.posizione.ToVector3().X, Cache.Char.posizione.ToVector3().Y, Cache.Char.posizione.ToVector3().Z, ref altezza))
					{
						Cache.PlayerPed.IsPositionFrozen = true;
						SetEnableHandcuffs(PlayerPedId(), true);
						CannaDaPesca.Detach();
						AttachEntityToEntity(CannaDaPesca.Handle, PlayerPedId(), GetPedBoneIndex(PlayerPedId(), 60309), 0, 0, 0, 0, 0, 0, false, false, false, false, 2, true);
						TaskPlayAnim(PlayerPedId(), AnimDict, "base", 8.0f, -8, -1, 34, 0, false, false, false);
						Pescando = true;
					}
					else
					{
						HUD.ShowNotification("Qui non puoi pescare.. prova ad entrare in acqua!", NotificationColor.Red, true);
					}
				}

				if (Input.IsDisabledControlJustPressed(Control.FrontendY))
				{
					CannaDaPesca.Delete();
					Client.Instance.RemoveTick(Pesca);
					Cache.PlayerPed.Task.ClearAll();
					CannaInMano = false;
					TipoCanna = -1;
				}
			}

			if (Pescando)
			{
				if (PuntiPesca.TempoPescaDinamico)
					await BaseScript.Delay(Funzioni.GetRandomInt(30000, 120000));
				else
					await BaseScript.Delay(PuntiPesca.TempoFisso * 1000);
				await ControlliEPesca();
			}
		}

		private static async Task ControlliEPesca()
		{
			int TocchiTotali = Funzioni.GetRandomInt(20, 40);
			int tocchiEffettuati = 0;
			int contogenerico = 0;
			int contomax = 0;

			if (TipoCanna != -1)
			{
				switch (TipoCanna)
				{
					case 0:
						TocchiTotali = Funzioni.GetRandomInt(20, 40);
						contomax = 1500;

						break;
					case 1:
						TocchiTotali = Funzioni.GetRandomInt(30, 50);
						contomax = 1000;

						break;
					default:
					{
						if (TipoCanna == 0)
						{
							TocchiTotali = Funzioni.GetRandomInt(40, 60);
							contomax = 800;
						}

						break;
					}
				}

				while (tocchiEffettuati < TocchiTotali)
				{
					await BaseScript.Delay(0);
					Game.DisableControlThisFrame(0, Control.Attack);
					Game.DisableAllControlsThisFrame(1);

					if (Game.CurrentInputMode == InputMode.GamePad)
					{
						HUD.ShowHelp("Ha abboccato qualcosa!! Gira velocemente ~INPUT_LOOK_UD~ per pescarla!");
						if (Input.IsDisabledControlJustPressed(Control.LookUpOnly) || Input.IsControlJustPressed(Control.LookDownOnly)) tocchiEffettuati += 1;
					}
					else
					{
						HUD.ShowHelp("Ha abboccato qualcosa!! Premi velocemente ~INPUT_ATTACK~ per pescarla!");
						if (Input.IsDisabledControlJustPressed(Control.Attack)) tocchiEffettuati += 1;
					}

					contogenerico += 1;

					if (contogenerico > contomax) break;
				}

				if (Funzioni.GetRandomInt(0, 100) < 90 && contogenerico < contomax)
				{
					string pesce = "";

					switch (TipoCanna)
					{
						case 0:
							pesce = PuntiPesca.Pesci.facile[Funzioni.GetRandomInt(0, PuntiPesca.Pesci.facile.Count - 1)];

							break;
						case 1:
							pesce = PuntiPesca.Pesci.medio[Funzioni.GetRandomInt(0, PuntiPesca.Pesci.medio.Count - 1)];

							break;
						case 2:
							pesce = PuntiPesca.Pesci.avanzato[Funzioni.GetRandomInt(0, PuntiPesca.Pesci.avanzato.Count - 1)];

							break;
					}

					float peso = Funzioni.GetRandomFloat(0f, ConfigShared.SharedConfig.Main.Generici.ItemList[pesce].peso);
					HUD.ShowNotification($"Hai pescato un bell'esemplare di {ConfigShared.SharedConfig.Main.Generici.ItemList[pesce].label}, dal peso di {peso}Kg");
					BaseScript.TriggerServerEvent("lprp:addIntenvoryItem", pesce, 1, peso);
				}
				else
				{
					HUD.ShowNotification("Il pesce è scappato! Andrà meglio la prossima volta..", true);
				}

				Pescando = false;
				Cache.PlayerPed.Task.ClearAll();
				CannaDaPesca.Detach();
				AttachEntityToEntity(CannaDaPesca.Handle, PlayerPedId(), GetPedBoneIndex(PlayerPedId(), /*60309*/ 57005), 0.10f, 0, -0.001f, 80.0f, 150.0f, 200.0f, false, false, false, false, 1, true);
				TaskPlayAnim(PlayerPedId(), "amb@code_human_wander_drinking@beer@male@base", "static", 3.5f, -8, -1, 49, 0, false, false, false);
				Cache.PlayerPed.IsPositionFrozen = false;
				SetEnableHandcuffs(PlayerPedId(), false);
				await Task.FromResult(0);
			}
		}

		private static async void MenuBarche()
		{
			UIMenu Barche = new UIMenu("Pescatore", "Scegli la barca", new System.Drawing.PointF(50, 50));
			HUD.MenuPool.Add(Barche);
			foreach (UIMenuItem boat in PuntiPesca.Barche.Select(barca => new UIMenuItem(GetLabelText(barca), "~y~Se sei in compagnia dei tuoi amici~w~ potete usare una barca sola insieme e risparmiare nell'affitto!"))) Barche.AddItem(boat);
			Vehicle veh = new Vehicle(0);
			Barche.OnIndexChange += async (menu, index) =>
			{
				if (veh.Exists()) veh.Delete();
				veh = await Funzioni.SpawnLocalVehicle(PuntiPesca.Barche[index], new Vector3(PuntiPesca.SpawnBarca[0], PuntiPesca.SpawnBarca[1], PuntiPesca.SpawnBarca[2]), PuntiPesca.SpawnBarca[3]);
			};
			Barche.OnItemSelect += async (menu, item, index) =>
			{
				HUD.MenuPool.CloseAllMenus();
				if (veh.Exists()) veh.Delete();
				Vehicle newveh = await Funzioni.SpawnVehicleNoPlayerInside(PuntiPesca.Barche[index], new Vector3(PuntiPesca.SpawnBarca[0], PuntiPesca.SpawnBarca[1], PuntiPesca.SpawnBarca[2]), PuntiPesca.SpawnBarca[3]);
				VeicoloLavorativoEAffitto vehlav = new VeicoloLavorativoEAffitto(newveh, Cache.Char.FullName);
				BaseScript.TriggerServerEvent("lprp:registraVeicoloLavorativoENon", vehlav.Serialize());
			};
			Barche.Visible = true;
		}
	}
}