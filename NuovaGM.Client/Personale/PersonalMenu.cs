using CitizenFX.Core;
using CitizenFX.Core.UI;
using Newtonsoft.Json;
using TheLastPlanet.Client.Giostre;
using TheLastPlanet.Client.Core.Status;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.MenuNativo;
using TheLastPlanet.Client.Veicoli;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;
using TheLastPlanet.Client.Core;
using TheLastPlanet.Shared;
using Logger;
using TheLastPlanet.Client.MenuNativo.PauseMenu;
using System.Drawing;

namespace TheLastPlanet.Client.Personale
{
	static class PersonalMenu
	{
		static MenuPool pool = HUD.MenuPool;
		static UIMenuItem fa = new UIMenuItem("Fame");
		static UIMenuItem se = new UIMenuItem("Sete");
		static UIMenuItem st = new UIMenuItem("Stanchezza");
		static UIMenuItem ma = new UIMenuItem("Stato Clinico");

		public static List<dynamic> personaggi = new List<dynamic>();
		public static float interactionDistance = 3.5f;
		public static int lockDistance = 25;
		public static string itemgps = "nessuno";
		public static bool salvato = false;
		public static bool acceso = false;
		public static bool chiuso = false;
		public static bool attivo = false;
		public static bool GranaCinema = false;
		static bool aperto = false;
		private static float fuelint = 0;
		public static List<dynamic> gps = new List<dynamic>()
		{
		   "Nessuno",
		   "Centro per l'Impiego",
		   "Stazione di polizia",
		   "Ospedale",
		   "Banca",
		   "Negozio",
		   "Negozio d'armi",
		   "Negozio di animali",
		   "Concessionario",
		   "Parcheggio Affitto Auto",
		   "Negozio di Abbigliamento",
		   "Barbiere",
		   "Autoscuola",
		};
		static List<dynamic> finestrini = new List<dynamic>(){
			"Anteriore Sinistro",
			"Anteriore Destro",
			"Posteriore Sinistro",
			"Posteriore Destro",
		};
		static List<dynamic> portiere = new List<dynamic>(){
			"Anteriore Sinistra",
			"Anteriore Destra",
			"Posteriore Sinistra",
			"Posteriore Destra",
			"Cofano",
			"Bagagliaio"
		};
		static List<dynamic> chiusure = new List<dynamic>(){
			"Chiusa",
			"Aperta"
		};
		public static Blip b;
		public static bool blipFound = false;

		public static async void menuPersonal()
		{
			var pos = new System.Drawing.Point(50, 50);
			UIMenu PersonalMenu = new UIMenu("Menu Personale", "~g~A portata di mano~w~", pos);
			pool.Add(PersonalMenu);

			#region GPS Veloce

			UIMenuListItem gpsItem = new UIMenuListItem("GPS Veloce", gps, 0);
			PersonalMenu.AddItem(gpsItem);
			PersonalMenu.OnListSelect += async (menu, _item, _itemIndex) =>
			{
				if (_item == gpsItem)
				{

					int var;
					switch (_item.Items[_item.Index])
					{
						case "Centro per l'Impiego":
							var = 407;
							break;
						case "Stazione di polizia":
							var = 60;
							break;
						case "Ospedale":
							var = 80;
							break;
						case "Banca":
							var = 108;
							break;
						case "Negozio":
							var = 52;
							break;
						case "Negozio d'armi":
							var = 154;
							break;
						case "Negozio di animali":
							var = 463;
							break;
						case "Concessionario":
							var = 225;
							break;
						case "Parcheggio Affitto Auto":
							var = 50;
							break;
						case "Negozio di Abbigliamento":
							var = 73;
							break;
						case "Barbiere":
							var = 71;
							break;
						case "Autoscuola":
							var = 408;
							break;
						default:
							var = 0;
							break;
					}
					if (_item.Items[_item.Index] == "Nessuno")
					{
						try
						{
							if (b.Exists())
							{
								SetWaypointOff();
								b.ShowRoute = false;
								Client.Instance.RemoveTick(routeColor);
								HUD.ShowNotification("GPS: Destinazione rimossa.", true);
								return;
							}
							else
								HUD.ShowNotification("GPS: Nessuna destinazione impostata!!", true);
						}
						catch(Exception e)
						{
							Log.Printa(LogType.Debug, e.ToString() + e.StackTrace);
							HUD.ShowNotification("Nessuna destinazione impostata!!", true);
						}
					}
					else
					{
						Blip[] test = World.GetAllBlips((BlipSprite)var);
						HUD.ShowNotification("GPS: Calcolo..");
						await BaseScript.Delay(1000);
						b = test.ToList().OrderBy(x => Vector3.Distance(x.Position, Game.Player.GetPlayerData().posizione.ToVector3())).FirstOrDefault();
						HUD.ShowNotification("GPS: Calcolo..").Hide();
						if (b != null)
						{
							b.ShowRoute = true;
							HUD.ShowNotification($"Destinazione più vicina impostata per {(_item as UIMenuListItem).Items[(_item as UIMenuListItem).Index]}.");
							Client.Instance.AddTick(routeColor);
						}
						else
							HUD.ShowNotification("Destinazione non trovata!");
					}
				}
			};
			#endregion

			#region Controlli Veicolo
			UIMenu vehContr = pool.AddSubMenu(PersonalMenu, "Controlli Veicolo", "Controlla il tuo veicolo con questi comandi rapidi", pos);
			UIMenuItem fuel = new UIMenuItem("Carburante veicolo salvato", "Facciamo il pieno?");
			UIMenuCheckboxItem salva = new UIMenuCheckboxItem("Salva Veicolo", salvato, "Salva qui il tuo veicolo!");
			UIMenuCheckboxItem chiudi = new UIMenuCheckboxItem("Serratura", chiuso, "Apri o chiudi il tuo veicolo (Salvalo prima!)");
			UIMenuListItem fin = new UIMenuListItem("Alza/Abbassa finestrini", finestrini, 0, "Alza e abbassa i finestrini del veicolo");
			UIMenuListItem port = new UIMenuListItem("Apri/Chiudi Portiere", portiere, 0, "Apri e chiudi le portiere della tua macchina");
			UIMenuCheckboxItem motore = new UIMenuCheckboxItem("Accensione Remota", EventiPersonalMenu.saveVehicle!=null? EventiPersonalMenu.saveVehicle.IsEngineRunning:false, "Accensione del motore remota!");

			vehContr.AddItem(fuel);
			vehContr.AddItem(salva);
			vehContr.AddItem(chiudi);
			vehContr.AddItem(fin);
			vehContr.AddItem(port);
			vehContr.AddItem(motore);
			if (!Game.PlayerPed.IsInVehicle())
			{
				chiudi.Enabled = false;
				fin.Enabled = false;
				port.Enabled = false;
				motore.Enabled = false;
			}
			vehContr.OnCheckboxChange += async (_menu, _item, _checked) =>
			{
				if (_item == chiudi)
					EventiPersonalMenu.Lock(_checked);
				else if (_item == salva)
				{
					if (_checked && Game.PlayerPed.IsInVehicle())
					{
						EventiPersonalMenu.Save(_checked);
						if (_checked)
						{
							chiudi.Enabled = true;
							fin.Enabled = true;
							port.Enabled = true;
							motore.Enabled = true;
							fuel.SetRightLabel(fuelint + "%");
						}
						else
						{
							chiudi.Enabled = false;
							fin.Enabled = false;
							port.Enabled = false;
							motore.Enabled = false;
							fuel.SetRightLabel("nessun veicolo salvato");
						}
					}
					else if (!_checked)
					{
						EventiPersonalMenu.Save(_checked);
						chiudi.Enabled = false;
						fin.Enabled = false;
						port.Enabled = false;
						motore.Enabled = false;
						fuel.SetRightLabel("nessun veicolo salvato");
					}
					else
						HUD.ShowNotification("Devi essere in un veicolo per attivare la funzione di salvataggio", true);
				}
				else if (_item == motore)
					EventiPersonalMenu.motore(_checked);
			};

			vehContr.OnListSelect += (_menu, _listItem, _itemIndex) =>
			{
				if (_listItem == fin)
					EventiPersonalMenu.Finestrini((_listItem as UIMenuListItem).Items[(_listItem as UIMenuListItem).Index].ToString());
				else if (_listItem == port)
					EventiPersonalMenu.Portiere((_listItem as UIMenuListItem).Items[(_listItem as UIMenuListItem).Index].ToString());
			};

			#endregion

			#region Personaggio e dati personali

			UIMenu persMenu = pool.AddSubMenu(PersonalMenu, "Personaggio Attuale");
			UIMenu datiPers = pool.AddSubMenu(persMenu, "Dati Personaggio", "Imparati");
			datiPers.OnMenuOpen += async (_menu) =>
			{
				datiPers.Clear();
				UIMenuItem name = new UIMenuItem("Nome: ", "Il suo nome");
				UIMenuItem dob = new UIMenuItem("Data di Nascita: ", "La sua Data di nascita");
				UIMenuItem alt = new UIMenuItem("Altezza: ", "La sua Altezza");
				UIMenuItem nTel = new UIMenuItem("N° Telefono: ", "Segnatelo se vuoi");
				UIMenuItem nAss = new UIMenuItem("N° Assicurazione: ", "Segnatelo se vuoi");
				UIMenuItem job = new UIMenuItem("Lavoro: ", "Il suo lavoro");
				UIMenuItem gang = new UIMenuItem("Gang: ", "Le affiliazioni");
				UIMenuItem bank = new UIMenuItem("Banca: ", "I soldi in banca");
				name.SetRightLabel(Game.Player.GetPlayerData().FullName);
				dob.SetRightLabel(Game.Player.GetPlayerData().DOB);
				alt.SetRightLabel("" + Game.Player.GetPlayerData().CurrentChar.info.height);
				nTel.SetRightLabel("" + Game.Player.GetPlayerData().CurrentChar.info.phoneNumber);
				nAss.SetRightLabel("" + Game.Player.GetPlayerData().CurrentChar.info.insurance);
				//
				//job.Label = SharedScript.Jobs[Game.Player.GetPlayerData().CurrentChar.job.name].label;
				//gang.Label = SharedScript.Gangs[Game.Player.GetPlayerData().CurrentChar.job.name].label;
				//
				job.SetRightLabel(Game.Player.GetPlayerData().CurrentChar.job.name);
				gang.SetRightLabel(Game.Player.GetPlayerData().CurrentChar.gang.name);
				bank.SetRightLabel("~g~$" + Game.Player.GetPlayerData().Bank);
				datiPers.AddItem(name);
				datiPers.AddItem(dob);
				datiPers.AddItem(alt);
				datiPers.AddItem(nTel);
				datiPers.AddItem(nAss);
				datiPers.AddItem(job);
				datiPers.AddItem(gang);
				UIMenu money = datiPers.AddSubMenu("Soldi: ", "I suoi soldi");
				money.ParentItem.SetRightLabel("~g~$" + Game.Player.GetPlayerData().Money);
				money.ParentItem.SetRightBadge(UIMenuItem.BadgeStyle.ArrowRight);
				datiPers.AddItem(bank);
				UIMenu dirty = datiPers.AddSubMenu("Soldi Sporchi: ", "I soldi sporchi");
				dirty.ParentItem.SetRightLabel("~r~$" + Game.Player.GetPlayerData().DirtyMoney);
				dirty.ParentItem.SetRightBadge(UIMenuItem.BadgeStyle.ArrowRight);

				UIMenu daiMoney = money.AddSubMenu("Dai a qualcuno", "A chi?");
				UIMenu daiDirty = dirty.AddSubMenu("Dai a qualcuno", "A chi?");
				daiMoney.ParentItem.SetRightBadge(UIMenuItem.BadgeStyle.ArrowRight);
				daiMoney.ParentItem.SetRightBadge(UIMenuItem.BadgeStyle.ArrowRight);

				UIMenuItem getta = new UIMenuItem("Butta via", "perché?");

				money.AddItem(getta);
				dirty.AddItem(getta);

				money.OnItemSelect += async (menu, item, index) =>
				{
					if (item == getta)
					{
						int amount = -1;
						do
						{
							await BaseScript.Delay(0);
							string am = await HUD.GetUserInput("Quanto vuoi buttare?", "0", 10);
							if (string.IsNullOrEmpty(am))
								break;
							amount = Convert.ToInt32(am);
							if (amount < 1 || amount > Game.Player.GetPlayerData().Money)
								HUD.ShowNotification("Quantità non valida!", NotificationColor.Red, true);
						}
						while (amount <= 1);
						if (amount == -1) return;
						Game.PlayerPed.Task.PlayAnimation("weapons@first_person@aim_rng@generic@projectile@sticky_bomb@", "plant_floor");
						BaseScript.TriggerServerEvent("lprp:removeAccountWithPickup", "soldi", amount);
						await BaseScript.Delay(1000);
						menu.GoBack();
						datiPers.RefreshIndex();
					}
				};
				dirty.OnItemSelect += async (menu, item, index) =>
				{
					if (item == getta)
					{
						int amount = -1;
						do
						{
							await BaseScript.Delay(0);
							string am = await HUD.GetUserInput("Quanto vuoi buttare?", "0", 10);
							if (string.IsNullOrEmpty(am))
								break;
							amount = Convert.ToInt32(am);
							if (amount < 1 || amount > Game.Player.GetPlayerData().DirtyMoney)
								HUD.ShowNotification("Quantità non valida!", NotificationColor.Red, true);
						}
						while (amount < 1);
						if (amount == -1) return;
						Game.PlayerPed.Task.PlayAnimation("weapons@first_person@aim_rng@generic@projectile@sticky_bomb@", "plant_floor");
						BaseScript.TriggerServerEvent("lprp:removeAccountWithPickup", "soldi_sporchi", amount);
						await BaseScript.Delay(1000);
						menu.GoBack();
						datiPers.RefreshIndex();
					}
				};
			};
			UIMenu salute = pool.AddSubMenu(persMenu, "Salute", "Fame, sete..", pos);
			fa.SetRightLabel("" + Math.Round(StatsNeeds.nee.fame, 2) + "%");
			se.SetRightLabel("" + Math.Round(StatsNeeds.nee.sete, 2) + "%");
			st.SetRightLabel("" + Math.Round(StatsNeeds.nee.stanchezza, 2) + "%");
			if (StatsNeeds.nee.malattia)
			{
				ma.SetRightLabel("Malato");
			}
			else
			{
				ma.SetRightLabel("In Salute");
			}

			salute.AddItem(fa);
			salute.AddItem(se);
			salute.AddItem(st);
			salute.AddItem(ma);

			salute.OnMenuOpen += (menu) =>
			{
				Client.Instance.AddTick(AggiornaSalute);
			};
			salute.OnMenuClose += (menu) =>
			{
				Client.Instance.RemoveTick(AggiornaSalute);
			};

			UIMenu Inventory = pool.AddSubMenu(persMenu, "Inventario Personale", "Tasche", pos);
			Inventory.OnMenuOpen += async (menu) =>
			{
				Inventory.Clear();
				int rimozione = 5;
				var inv = Game.Player.GetPlayerData().Inventory;
				if (inv.Count > 0)
				{
					foreach (var it in inv)
					{
						if (it.amount > 0)
						{
							UIMenu newItemMenu = pool.AddSubMenu(Inventory, ConfigShared.SharedConfig.Main.Generici.ItemList[it.item].label, "[Quantità: " + it.amount.ToString() + "] " + ConfigShared.SharedConfig.Main.Generici.ItemList[it.item].description);
							if (ConfigShared.SharedConfig.Main.Generici.ItemList[it.item].use.use)
							{
								UIMenuItem useButton = new UIMenuItem(ConfigShared.SharedConfig.Main.Generici.ItemList[it.item].use.label, ConfigShared.SharedConfig.Main.Generici.ItemList[it.item].use.description, Colors.GreenLight, Colors.Green);
								newItemMenu.AddItem(useButton);
								newItemMenu.OnItemSelect += (_menu, _item, _index) =>
								{
									/*
									 * BaseScript.TriggerServerEvent("lprp:useItem", ConfigShared.SharedConfig.Main.Generici.ItemList[it.item]);
									 *///DA GESTIRE 
									if (_item == useButton)
										ConfigShared.SharedConfig.Main.Generici.ItemList[it.item].UsaOggettoEvent(1);
								};
							}
							if (ConfigShared.SharedConfig.Main.Generici.ItemList[it.item].give.give)
							{
								List<dynamic> amountino = new List<dynamic>();
								for (int j = 0; j < it.amount; j++)
								{
									amountino.Add((j + 1).ToString());
								}

								UIMenu giveButton = newItemMenu.AddSubMenu(ConfigShared.SharedConfig.Main.Generici.ItemList[it.item].give.label, ConfigShared.SharedConfig.Main.Generici.ItemList[it.item].give.description);
								giveButton.ParentItem.HighlightColor = Colors.Cyan;
								giveButton.ParentItem.HighlightedTextColor = Colors.DarkCyan;
								List<int> playerId = new List<int>();
								var players = Funzioni.GetPlayersInArea(Game.Player.GetPlayerData().posizione.ToVector3(), 3f);
								if (players.Count > 0)
								{
									foreach (var player in players)
									{
										UIMenuListItem playerItem = new UIMenuListItem(Funzioni.GetPlayerCharFromServerId(player.ServerId).FullName, amountino, 0, "Scegli la quantità e procedi..");
										playerId.Add(player.ServerId);
										giveButton.AddItem(playerItem);
										Game.PlayerPed.Task.PlayAnimation("mp_common", "givetake1_a");
									}
									giveButton.OnListSelect += async (_menu, _listItem, _index) =>
									{
										BaseScript.TriggerServerEvent("lprp:giveInventoryItemToPlayer", playerId[_index], it.item, int.Parse(amountino[_listItem.Index]));
									};
								}
								else
									giveButton.AddItem(new UIMenuItem("Nessun player nelle vicinanze.", "Trova qualcuno!!"));
							}
							if (ConfigShared.SharedConfig.Main.Generici.ItemList[it.item].drop.drop)
							{
								List<dynamic> amountino = new List<dynamic>();
								for (int j = 0; j < it.amount; j++)
								{
									amountino.Add((j + 1).ToString());
								}

								UIMenuListItem dropButton = new UIMenuListItem(ConfigShared.SharedConfig.Main.Generici.ItemList[it.item].drop.label, amountino, 0, ConfigShared.SharedConfig.Main.Generici.ItemList[it.item].drop.description, Colors.RedLight, Colors.Red);
								newItemMenu.AddItem(dropButton);
								newItemMenu.OnListSelect += async (_menu, _listItem, Index) =>
								{
									if (_listItem == dropButton)
									{
										Game.PlayerPed.Task.PlayAnimation("weapons@first_person@aim_rng@generic@projectile@sticky_bomb@", "plant_floor");
										BaseScript.TriggerServerEvent("lprp:removeInventoryItemWithPickup", it.item, int.Parse(amountino[_listItem.Index]));
										await BaseScript.Delay(1000);
//										Inventory.RemoveItemAt(Inventory.MenuItems.IndexOf(newItemMenu.ParentItem));
										_menu.GoBack();
										Inventory.RefreshIndex();
									}
								};
							}
						}
					}
				}
				else
					Inventory.AddItem(new UIMenuItem("Non hai niente nell'inventario.", "Datti da fare!!", Colors.RedDark, Colors.Red));
			};
			UIMenu weapMenu = pool.AddSubMenu(persMenu, "Inventario Armi", "Le tue armi", pos);
			weapMenu.OnMenuOpen += async (menu) =>
			{
				weapMenu.Clear();
				if (Game.Player.GetPlayerData().getCharWeapons(Game.Player.GetPlayerData().char_current).Count > 0)
				{
					for (int i = 0; i < Game.Player.GetPlayerData().getCharWeapons(Game.Player.GetPlayerData().char_current).Count; i++)
					{
						Weapons armi = Game.Player.GetPlayerData().getCharWeapons(Game.Player.GetPlayerData().char_current)[i];
						UIMenu arma = pool.AddSubMenu(weapMenu, GetLabelText(Funzioni.GetWeaponLabel((uint)GetHashKey(armi.name))), "Munizioni: " + armi.ammo);
						if (armi.components.Count > 0)
						{
							UIMenu componenti = pool.AddSubMenu(arma, "Componenti " + GetLabelText(Funzioni.GetWeaponLabel((uint)GetHashKey(armi.name))), "Scegli qui quali abilitare tra quelli che hai!");
							for (int j = 0; j < armi.components.Count; j++)
							{
								Components comp = armi.components[j];
								bool attivo = comp.active;
								if (GetLabelText(Funzioni.GetWeaponLabel((uint)GetHashKey(comp.name))) != "Caricatore standard")
								{
									UIMenuCheckboxItem componente = new UIMenuCheckboxItem(GetLabelText(Funzioni.GetWeaponLabel((uint)GetHashKey(comp.name))), comp.active, "Attiva/Disattiva questo componente!");
									componenti.AddItem(componente);
									componenti.OnCheckboxChange += (_menu, _item, _checked) =>
									{
										if (_item.Text == GetLabelText(Funzioni.GetWeaponLabel((uint)GetHashKey(comp.name))))
										{
											uint componentHash = (uint)GetHashKey(comp.name);
											List<Weapons> armiAgg = new List<Weapons>();
											List<Components> weaponComponents = new List<Components> { new Components(comp.name, comp.active) };
											armiAgg.Add(new Weapons(armi.name, armi.ammo, weaponComponents, armi.tint));
											Game.Player.GetPlayerData().CurrentChar.weapons = armiAgg;
											BaseScript.TriggerServerEvent("lprp:updateCurChar", "weapons", armiAgg.Serialize());
											if (_checked)
											{
												GiveWeaponComponentToPed(PlayerPedId(), (uint)GetHashKey(armi.name), componentHash);
												HUD.ShowNotification("~y~" + GetLabelText(Funzioni.GetWeaponLabel((uint)GetHashKey(comp.name))) + " Attivato/a.", true);
											}
											else if (!_checked)
											{
												RemoveWeaponComponentFromPed(PlayerPedId(), (uint)GetHashKey(armi.name), componentHash);
												HUD.ShowNotification("~y~" + GetLabelText(Funzioni.GetWeaponLabel((uint)GetHashKey(comp.name))) + " Disattivato/a.");
											}
										}
									};
								}
							}
						}
						/* CODICE PER DARE UN ARMA E SI AGGIUNGERE ANCHE LE MUNIZIONI
						UIMenuItem giveButton = new UIMenuItem($"Dai {GetLabelText(Funzioni.GetWeaponLabel((uint)GetHashKey(armi.name)))}", "", Colors.Cyan, Colors.DarkCyan);
						arma.AddItem(giveButton);
						*/
						UIMenuItem dropButton = new UIMenuItem($"Gettare {GetLabelText(Funzioni.GetWeaponLabel((uint)GetHashKey(armi.name)))}", "", Colors.RedLight, Colors.Red);
						arma.AddItem(dropButton);
						dropButton.Activated += async (_menu, item) =>
						{
							Game.PlayerPed.Task.PlayAnimation("weapons@first_person@aim_rng@generic@projectile@sticky_bomb@", "plant_floor");
							BaseScript.TriggerServerEvent("lprp:removeWeaponWithPickup", armi.name);
							await BaseScript.Delay(1000);
//							weapMenu.RemoveItemAt(weapMenu.MenuItems.IndexOf(arma.ParentItem));
							_menu.GoBack();
							weapMenu.RefreshIndex();
						};
					}
				}
				else
				{
					UIMenuItem noItemsButton = new UIMenuItem("Non hai niente nell'inventario.", "Datti da fare!!", Colors.RedDark, Colors.Red);
					weapMenu.AddItem(noItemsButton);
				}
			};
			#endregion

			#region Lavoro
			/*
			UIMenu WorkMenu = pool.AddSubMenu(PersonalMenu, "Menu Lavoro", "Menu dei Lavori", pos);
			for (int i = 0; i < 20; i++)
			{
				WorkMenu.AddItem(new UIMenuItem("Fregati!"));
			}
			*/
			#endregion
			#region animazioni e stile
			List<dynamic> umori = new List<dynamic>() { "Determinato", "Triste", "Depresso", "Annoiato", "Impaziente", "Timido", "Lunatico", "Stressato", "Pigro" };
			List<dynamic> attegg = new List<dynamic>() { "Fiero", "Cattivo", "Gangster", "Freddo", "Vuoto Dentro", "Borioso", "Perso", "Intimidatorio", "Ricco", "Aggressivo", "Imponente", "Esibizionista" };
			List<dynamic> donn = new List<dynamic>() { "Arrogante", "Di Classe", "Fragile", "Femme Fatale", "Assorbente senza Ali", "Triste", "Rebelle", "Sexy", "Con la ciccia ai glutei", "Fiera" };

			UIMenu ANeStil = pool.AddSubMenu(PersonalMenu, "Animazioni e Stile");
			UIMenu umore = pool.AddSubMenu(ANeStil, "Stile Camminata", "Le emozioni contano", pos);
			UIMenuListItem Item1 = new UIMenuListItem("Umori", umori, 0, "Come si sente il tuo personaggio oggi?");
			UIMenuListItem Item2 = new UIMenuListItem("Stile", attegg, 0, "Che atteggiamento ha il tuo personaggio?");
			UIMenuListItem Item3 = new UIMenuListItem("Femminili", donn, 0, "Perche a noi importa del gentil sesso!");
			UIMenuItem Item4 = new UIMenuItem("~r~Reset", "Perche atteggiarti quando puoi camminare normalmente?", Colors.RedDark, Colors.RedLight);
			umore.AddItem(Item1);
			umore.AddItem(Item2);
			umore.AddItem(Item3);
			umore.AddItem(Item4);
			umore.OnListSelect += (_menu, _listItem, _itemIndex) =>
			{
				if (_listItem == Item1 && !Death.ferito)
				{
					string ActiveItem = (_listItem as UIMenuListItem).Items[(_listItem as UIMenuListItem).Index].ToString();

					if (ActiveItem == "Determinato")
					{
						Game.PlayerPed.MovementAnimationSet = "move_m@brave";
					}
					else if (ActiveItem == "Triste")
					{
						Game.PlayerPed.MovementAnimationSet = "move_m@sad@a";
					}
					else if (ActiveItem == "Depresso")
					{
						Game.PlayerPed.MovementAnimationSet = "move_m@depressed@a";
					}
					else if (ActiveItem == "Annoiato")
					{
						Game.PlayerPed.MovementAnimationSet = "move_characters@michael@fire";
					}
					else if (ActiveItem == "Impaziente")
					{
						Game.PlayerPed.MovementAnimationSet = "move_m@quick";
					}
					else if (ActiveItem == "Timido")
					{
						Game.PlayerPed.MovementAnimationSet = "move_m@confident";
					}
					else if (ActiveItem == "Lunatico")
					{
						Game.PlayerPed.MovementAnimationSet = "move_m@hurry@a";
					}
					else if (ActiveItem == "Stressato")
					{
						Game.PlayerPed.MovementAnimationSet = "move_m@hurry@a";
					}
					else if (ActiveItem == "Pigro")
					{
						Game.PlayerPed.MovementAnimationSet = "move_characters@jimmy@slow@";
					}
				}
				else if (_listItem == Item2 && !Death.ferito)
				{
					string atteg = (_listItem as UIMenuListItem).Items[(_listItem as UIMenuListItem).Index].ToString();
					if (atteg == "Fiero")
					{
						Game.PlayerPed.MovementAnimationSet = "move_m@business@a";
					}
					else if (atteg == "Cattivo")
					{
						Game.PlayerPed.MovementAnimationSet = "move_m@casual@d";
					}
					else if (atteg == "Gangster")
					{
						Game.PlayerPed.MovementAnimationSet = "move_m@casual@e";
					}
					else if (atteg == "Freddo")
					{
						Game.PlayerPed.MovementAnimationSet = "move_m@buzzed";
					}
					else if (atteg == "Vuoto Dentro")
					{
						Game.PlayerPed.MovementAnimationSet = "move_m@depressed@b";
					}
					else if (atteg == "Borioso")
					{
						Game.PlayerPed.MovementAnimationSet = "move_m@sassy";
					}
					else if (atteg == "Perso")
					{
						Game.PlayerPed.MovementAnimationSet = "move_m@hobo@b";
					}
					else if (atteg == "Intimidatorio")
					{
						Game.PlayerPed.MovementAnimationSet = "move_m@intimidation@1h";
					}
					else if (atteg == "Ricco")
					{
						Game.PlayerPed.MovementAnimationSet = "move_m@money";
					}
					else if (atteg == "Aggressivo")
					{
						Game.PlayerPed.MovementAnimationSet = "move_m@fire";
					}
					else if (atteg == "Imponente")
					{
						Game.PlayerPed.MovementAnimationSet = "move_m@gangster@generic";
					}
					else if (atteg == "Esibizionista")
					{
						Game.PlayerPed.MovementAnimationSet = "move_m@swagger";
					}
				}
				else if (_listItem == Item3 && !Death.ferito)
				{
					string donna = (_listItem as UIMenuListItem).Items[(_listItem as UIMenuListItem).Index].ToString();
					if (donna == "Arrogante")
					{
						Game.PlayerPed.MovementAnimationSet = "move_f@arrogant@a";
					}
					else if (donna == "Di Classe")
					{
						Game.PlayerPed.MovementAnimationSet = "move_characters@amanda@bag";
					}
					else if (donna == "Fragile")
					{
						Game.PlayerPed.MovementAnimationSet = "move_f@femme@";
					}
					else if (donna == "Femme Fatale")
					{
						Game.PlayerPed.MovementAnimationSet = "move_f@gangster@ng";
					}
					else if (donna == "Assorbente senza Ali")
					{
						Game.PlayerPed.MovementAnimationSet = "move_f@heels@c";
					}
					else if (donna == "Triste")
					{
						Game.PlayerPed.MovementAnimationSet = "move_f@sad@a";
					}
					else if (donna == "Ribelle")
					{
						Game.PlayerPed.MovementAnimationSet = "move_f@sassy";
					}
					else if (donna == "Sexy")
					{
						Game.PlayerPed.MovementAnimationSet = "move_f@sexy@a";
					}
					else if (donna == "Con la ciccia ai glutei")
					{
						Game.PlayerPed.MovementAnimationSet = "move_f@tough_guy@";
					}
					else if (donna == "Fiera")
					{
						Game.PlayerPed.MovementAnimationSet = "move_f@tool_belt@a";
					}
				}
				else if (_listItem == Item4 && !Death.ferito)
				{
					Game.PlayerPed.MovementAnimationSet = null;
				}
				else
				{
					HUD.ShowNotification("Sei ferito! Non puoi fare quest'azione al momento!", NotificationColor.Red, true);
				}
			};

			UIMenu animMenu = pool.AddSubMenu(ANeStil, "Menu Animazioni", "~g~Quando il RolePlay diventa anche divertente", pos);
			UIMenu animMenu1 = pool.AddSubMenu(animMenu, "Festa", "~g~Per divertirci");

			UIMenuItem item1 = new UIMenuItem("Suonare", "Che bella musica!");
			UIMenuItem item2 = new UIMenuItem("Dj", "Che bella musica!");
			UIMenuItem item3 = new UIMenuItem("Bere una bibita", "Ancora meglio se in compagnia!");
			UIMenuItem item4 = new UIMenuItem("Bere una birra", "Ancora meglio con una bella partita!");
			UIMenuItem item5 = new UIMenuItem("Air Guitar", "Per chi, una vera, non può permettersela!");
			UIMenuItem item6 = new UIMenuItem("Air Shagging", "Questo è divertirsi!");
			UIMenuItem item7 = new UIMenuItem("Rock 'n Roll", "L'anima del Metal vive in te!");
			UIMenuItem item8 = new UIMenuItem("Fumare una canna", "Per calmare i nervi!");
			UIMenuItem item9 = new UIMenuItem("Ubriaco", "Vacci piano!");

			animMenu1.OnItemSelect += (_menu, _item, _index) =>
			{
				if (!Game.PlayerPed.IsInVehicle() && Game.PlayerPed.IsAlive)
				{
					if (_item == item1)
					{
						Game.PlayerPed.Task.StartScenario("WORLD_HUMAN_MUSICIAN", Game.Player.GetPlayerData().posizione.ToVector3());
					}
					else if (_item == item2)
					{
						Game.PlayerPed.Task.PlayAnimation("anim@mp_player_intcelebrationmale@dj", "dj");
					}
					else if (_item == item3)
					{
						Game.PlayerPed.Task.StartScenario("WORLD_HUMAN_DRINKING", Game.Player.GetPlayerData().posizione.ToVector3());
					}
					else if (_item == item4)
					{
						Game.PlayerPed.Task.StartScenario("WORLD_HUMAN_PARTYING", Game.Player.GetPlayerData().posizione.ToVector3());
					}
					else if (_item == item5)
					{
						Game.PlayerPed.Task.PlayAnimation("anim@mp_player_intcelebrationmale@air_guitar", "air_guitar");
					}
					else if (_item == item6)
					{
						Game.PlayerPed.Task.PlayAnimation("anim@mp_player_intcelebrationfemale@air_shagging", "air_shagging");
					}
					else if (_item == item7)
					{
						Game.PlayerPed.Task.PlayAnimation("mp_player_int_upperrock", "mp_player_int_rock");
					}
					else if (_item == item8)
					{
						Game.PlayerPed.Task.StartScenario("WORLD_HUMAN_SMOKING_POT", Game.Player.GetPlayerData().posizione.ToVector3());
					}
					else if (_item == item9)
					{
						Game.PlayerPed.Task.PlayAnimation("amb@world_human_bum_standing@drunk@idle_a", "idle_a");
					}
				}
				else
				{
					HUD.ShowNotification("Non puoi usare quest'animazione adesso!!", NotificationColor.Red);
				}
			};

			animMenu1.AddItem(item1);
			animMenu1.AddItem(item2);
			animMenu1.AddItem(item3);
			animMenu1.AddItem(item4);
			animMenu1.AddItem(item5);
			animMenu1.AddItem(item6);
			animMenu1.AddItem(item7);
			animMenu1.AddItem(item8);
			animMenu1.AddItem(item9);

			UIMenu animMenu2 = pool.AddSubMenu(animMenu, "Saluti", "~g~Fagli sapere che li stai chiamando", pos);

			UIMenuItem item10 = new UIMenuItem("Salutare", "Fai ciao con la manina!");
			UIMenuItem item11 = new UIMenuItem("Saluto tra Amici", "Lo ami quasi come un cugino!");
			UIMenuItem item12 = new UIMenuItem("Saluto in Gang", "Lo ami come se fossi te stesso!");
			UIMenuItem item13 = new UIMenuItem("Saluto Militare", "AAAAAAAAAATTENTI!");

			animMenu2.AddItem(item10);
			animMenu2.AddItem(item11);
			animMenu2.AddItem(item12);
			animMenu2.AddItem(item13);

			animMenu2.OnItemSelect += (_menu, _item, _index) =>
			{
				if (!Game.PlayerPed.IsInVehicle() && Game.PlayerPed.IsAlive)
				{
					if (_item == item10)
					{
						Game.PlayerPed.Task.PlayAnimation("gestures@m@standing@casual", "gesture_hello");
					}
					else if (_item == item11)
					{
						Game.PlayerPed.Task.PlayAnimation("mp_ped_interaction", "handshake_guy_a");
					}
					else if (_item == item12)
					{
						Game.PlayerPed.Task.PlayAnimation("mp_ped_interaction", "hugs_guy_a");
					}
					else if (_item == item13)
					{
						Game.PlayerPed.Task.PlayAnimation("mp_player_int_uppersalute", "mp_player_int_salute");
					}
				}
				else
				{
					HUD.ShowNotification("Non puoi usare quest'animazione adesso!!", NotificationColor.Red);
				}
			};



			UIMenu animMenu3 = pool.AddSubMenu(animMenu, "Lavori", "~g~Il RolePlay non è solo un gioco!", pos);

			UIMenuItem item14 = new UIMenuItem("Arrendersi alla polizia", "Prima che sparino!");
			UIMenuItem item15 = new UIMenuItem("Pescatore", "Per rilassarsi al porto!");
			UIMenuItem item16 = new UIMenuItem("Agricoltore - Vendemmia", "Per chi ama la natura!");
			UIMenuItem item17 = new UIMenuItem("Meccanico - Ripara il veicolo", "Porta con te lo straccetto per le mani!");
			UIMenuItem item18 = new UIMenuItem("Meccanico - Ripara il motore", "Che mal di schiena!");
			UIMenuItem item19 = new UIMenuItem("Polizia - Gestire il Traffico", "Usa il Fischietto!");
			UIMenuItem item20 = new UIMenuItem("Polizia - Indagare", "Fammi vedere meglio...");
			UIMenuItem item21 = new UIMenuItem("Binocolo", "Per vederci chiaro!");
			UIMenuItem item22 = new UIMenuItem("Medico - Controlla il paziente", "Potrà ancora suonare il piano?!");
			UIMenuItem item23 = new UIMenuItem("Taxi - Parlare al cliente", "Conversare aiuta a rilassarsi!");
			UIMenuItem item24 = new UIMenuItem("Taxi - Dare il conto", "Ecco a lei il resto!");
			UIMenuItem item25 = new UIMenuItem("Drogheria - dare la roba", "Ecco a lei Signore..");
			UIMenuItem item26 = new UIMenuItem("Fotografo", "Immortala i momenti migliori!");
			UIMenuItem item27 = new UIMenuItem("Annotare", "Spero non sia la lista della spesa!");
			UIMenuItem item28 = new UIMenuItem("Martellare", "Che male al dito!");
			UIMenuItem item29 = new UIMenuItem("Chiedere l'elemosina", "Siamo tutti poveri..");
			UIMenuItem item30 = new UIMenuItem("Fare la Statua", "Fermo... fermo...");
			//UIMenuItem item31 = new UIMenuItem();

			animMenu3.AddItem(item14);
			animMenu3.AddItem(item15);
			animMenu3.AddItem(item16);
			animMenu3.AddItem(item17);
			animMenu3.AddItem(item18);
			animMenu3.AddItem(item19);
			animMenu3.AddItem(item20);
			animMenu3.AddItem(item21);
			animMenu3.AddItem(item22);
			animMenu3.AddItem(item23);
			animMenu3.AddItem(item24);
			animMenu3.AddItem(item25);
			animMenu3.AddItem(item26);
			animMenu3.AddItem(item27);
			animMenu3.AddItem(item28);
			animMenu3.AddItem(item29);
			animMenu3.AddItem(item30);

			animMenu3.OnItemSelect += (_menu, _item, _index) =>
			{
				if (!Game.PlayerPed.IsInVehicle() && Game.PlayerPed.IsAlive)
				{
					if (_item == item14)
					{
						Game.PlayerPed.Task.PlayAnimation("random@arrests@busted", "idle_c");
					}
					else if (_item == item15)
					{
						Game.PlayerPed.Task.StartScenario("world_human_stand_fishing", Game.Player.GetPlayerData().posizione.ToVector3());
					}
					else if (_item == item16)
					{
						Game.PlayerPed.Task.StartScenario("world_human_gardener_plant", Game.Player.GetPlayerData().posizione.ToVector3());
					}
					else if (_item == item17)
					{
						Game.PlayerPed.Task.StartScenario("world_human_vehicle_mechanic", Game.Player.GetPlayerData().posizione.ToVector3());
					}
					else if (_item == item18)
					{
						Game.PlayerPed.Task.PlayAnimation("mini@repair", "fixing_a_ped");
					}
					else if (_item == item19)
					{
						Game.PlayerPed.Task.StartScenario("WORLD_HUMAN_CAR_PARK_ATTENDANT", Game.Player.GetPlayerData().posizione.ToVector3());
					}
					else if (_item == item20)
					{
						Game.PlayerPed.Task.PlayAnimation("amb@code_human_police_investigate@idle_b", "idle_f");
					}
					else if (_item == item21)
					{
						Game.PlayerPed.Task.StartScenario("WORLD_HUMAN_BINOCULARS", Game.Player.GetPlayerData().posizione.ToVector3());
					}
					else if (_item == item22)
					{
						Game.PlayerPed.Task.StartScenario("CODE_HUMAN_MEDIC_KNEEL", Game.Player.GetPlayerData().posizione.ToVector3());
					}
					else if (_item == item23)
					{
						Game.PlayerPed.Task.PlayAnimation("oddjobs@taxi@driver", "leanover_idle");
					}
					else if (_item == item24)
					{
						Game.PlayerPed.Task.PlayAnimation("oddjobs@taxi@cyi", "std_hand_off_ps_passenger");
					}
					else if (_item == item25)
					{
						Game.PlayerPed.Task.PlayAnimation("mp_am_hold_up", "purchase_beerbox_shopkeeper");
					}
					else if (_item == item26)
					{
						Game.PlayerPed.Task.StartScenario("WORLD_HUMAN_PAPARAZZI", Game.Player.GetPlayerData().posizione.ToVector3());
					}
					else if (_item == item27)
					{
						Game.PlayerPed.Task.StartScenario("WORLD_HUMAN_CLIPBOARD", Game.Player.GetPlayerData().posizione.ToVector3());
					}
					else if (_item == item28)
					{
						Game.PlayerPed.Task.StartScenario("WORLD_HUMAN_HAMMERING", Game.Player.GetPlayerData().posizione.ToVector3());
					}
					else if (_item == item29)
					{
						Game.PlayerPed.Task.StartScenario("WORLD_HUMAN_BUM_FREEWAY", Game.Player.GetPlayerData().posizione.ToVector3());
					}
					else if (_item == item30)
					{
						Game.PlayerPed.Task.StartScenario("WORLD_HUMAN_HUMAN_STATUE", Game.Player.GetPlayerData().posizione.ToVector3());
					}
				}
				else
				{
					HUD.ShowNotification("Non puoi usare quest'animazione adesso!!", NotificationColor.Red);
				}
			};


			UIMenu animMenu4 = pool.AddSubMenu(animMenu, "Umore", "~g~Cosa vuoi dirgli col corpo?", pos);

			UIMenuItem item31 = new UIMenuItem("Congratularsi", "Meglio di Cosi!");
			UIMenuItem item32 = new UIMenuItem("Super", "Pollice in SU!");
			UIMenuItem item33 = new UIMenuItem("Indicare", "TU!");
			UIMenuItem item34 = new UIMenuItem("Che vuoi?", "");
			UIMenuItem item35 = new UIMenuItem("Lo sapevo, cazzo!", "");
			UIMenuItem item36 = new UIMenuItem("Facepalm", "");
			UIMenuItem item37 = new UIMenuItem("Calmati", "Meglio ragionarci");
			UIMenuItem item38 = new UIMenuItem("Spavento", "");
			UIMenuItem item39 = new UIMenuItem("Sottomesso", "E porta rispetto!");
			UIMenuItem item40 = new UIMenuItem("Preparati", "Un po' di riscaldamento");
			UIMenuItem item41 = new UIMenuItem("Non è possibile!", "Non ci credo!");
			UIMenuItem item42 = new UIMenuItem("Abbracciare", "Un po' di affetto");
			UIMenuItem item43 = new UIMenuItem("Dito Medio", "Il meglio per mandare a quel paese con dolcezza e decisione!");
			UIMenuItem item44 = new UIMenuItem("Segaiolo", "Per far capire che hai molto tempo libero!");
			UIMenuItem item45 = new UIMenuItem("Suicidio", "Spaventa i tuoi amici e familiari!");
			//UIMenuItem item46 = new UIMenuItem();

			animMenu4.AddItem(item31);
			animMenu4.AddItem(item32);
			animMenu4.AddItem(item33);
			animMenu4.AddItem(item34);
			animMenu4.AddItem(item35);
			animMenu4.AddItem(item36);
			animMenu4.AddItem(item37);
			animMenu4.AddItem(item38);
			animMenu4.AddItem(item39);
			animMenu4.AddItem(item40);
			animMenu4.AddItem(item41);
			animMenu4.AddItem(item42);
			animMenu4.AddItem(item43);
			animMenu4.AddItem(item44);
			animMenu4.AddItem(item45);

			animMenu4.OnItemSelect += (_menu, _item, _index) =>
			{
				if (!Game.PlayerPed.IsInVehicle() && Game.PlayerPed.IsAlive)
				{
					if (_item == item31)
					{
						Game.PlayerPed.Task.StartScenario("WORLD_HUMAN_CHEERING", Game.Player.GetPlayerData().posizione.ToVector3());
					}
					else if (_item == item32)
					{
						Game.PlayerPed.Task.PlayAnimation("mp_action", "thanks_male_06");
					}
					else if (_item == item33)
					{
						Game.PlayerPed.Task.PlayAnimation("gestures@m@standing@casual", "gesture_point");
					}
					else if (_item == item34)
					{
						Game.PlayerPed.Task.PlayAnimation("gestures@m@standing@casual", "gesture_bring_it_on");
					}
					else if (_item == item35)
					{
						Game.PlayerPed.Task.PlayAnimation("anim@am_hold_up@male", "shoplift_high");
					}
					else if (_item == item36)
					{
						Game.PlayerPed.Task.PlayAnimation("anim@mp_player_intcelebrationmale@face_palm", "face_palm");
					}
					else if (_item == item37)
					{
						Game.PlayerPed.Task.PlayAnimation("gestures@m@standing@casual", "gesture_easy_now");
					}
					else if (_item == item38)
					{
						Game.PlayerPed.Task.PlayAnimation("oddjobs@assassinate@multi@", "react_big_variations_a");
					}
					else if (_item == item39)
					{
						Game.PlayerPed.Task.PlayAnimation("amb@code_human_cower_stand@male@react_cowering", "base_right");
					}
					else if (_item == item40)
					{
						Game.PlayerPed.Task.PlayAnimation("anim@deathmatch_intros@unarmed", "intro_male_unarmed_e");
					}
					else if (_item == item41)
					{
						Game.PlayerPed.Task.PlayAnimation("gestures@m@standing@casual", "gesture_damn");
					}
					else if (_item == item42)
					{
						Game.PlayerPed.Task.PlayAnimation("mp_ped_interaction", "kisses_guy_a");
					}
					else if (_item == item43)
					{
						Game.PlayerPed.Task.PlayAnimation("mp_player_int_upperfinger", "mp_player_int_finger_01_enter");
					}
					else if (_item == item44)
					{
						Game.PlayerPed.Task.PlayAnimation("mp_player_int_upperwank", "mp_player_int_wank_01");
					}
					else if (_item == item45)
					{
						Game.PlayerPed.Task.PlayAnimation("mp_suicide", "pistol");
					}
				}
				else
				{
					HUD.ShowNotification("Non puoi usare quest'animazione adesso!!", NotificationColor.Red);
				}
			};

			UIMenu animMenu5 = pool.AddSubMenu(animMenu, "Sports", "~g~Tenersi in forma è importante..", pos);
			UIMenuItem item46 = new UIMenuItem("Mostra i tuoi muscoli", "Le donne ti vorranno e gli uomini vorranno essere te!");
			UIMenuItem item47 = new UIMenuItem("Fare i pesi", "Per bicipiti da urlo!");
			UIMenuItem item48 = new UIMenuItem("Fare le flessioni", "oh yeah!");
			UIMenuItem item49 = new UIMenuItem("Fare gli addominali", "Scolpito nella roccia");
			UIMenuItem item50 = new UIMenuItem("Yoga", "Per chi si depila l'inguine");


			animMenu5.AddItem(item46);
			animMenu5.AddItem(item47);
			animMenu5.AddItem(item48);
			animMenu5.AddItem(item49);
			animMenu5.AddItem(item50);

			animMenu5.OnItemSelect += (_menu, _item, _index) =>
			{
				if (!Game.PlayerPed.IsInVehicle() && Game.PlayerPed.IsAlive)
				{
					if (_item == item46)
					{
						Game.PlayerPed.Task.PlayAnimation("amb@world_human_muscle_flex@arms_at_side@base", "base");
					}
					else if (_item == item47)
					{
						Game.PlayerPed.Task.PlayAnimation("amb@world_human_muscle_free_weights@male@barbell@base", "base");
					}
					else if (_item == item48)
					{
						Game.PlayerPed.Task.PlayAnimation("amb@world_human_push_ups@male@base", "base");
					}
					else if (_item == item49)
					{
						Game.PlayerPed.Task.PlayAnimation("amb@world_human_sit_ups@male@base", "base");
					}
					else if (_item == item50)
					{
						Game.PlayerPed.Task.PlayAnimation("amb@world_human_yoga@male@base", "base_a");
					}
				}
				else
				{
					HUD.ShowNotification("Non puoi usare quest'animazione adesso!!", NotificationColor.Red);
				}
			};


			UIMenu animMenu6 = pool.AddSubMenu(animMenu, "Varie", "~g~Per la vita quotidiana", pos);

			UIMenuItem item51 = new UIMenuItem("Bere un caffè", "Non Dormire!");
			UIMenuItem item52 = new UIMenuItem("Sedersi", "Il tuo sedere ringrazia!");
			UIMenuItem item53 = new UIMenuItem("Sedersi per terra", "Ci sono le panchine eh!");
			UIMenuItem item54 = new UIMenuItem("Appoggiati al muro", "Come un tamarro!");
			UIMenuItem item55 = new UIMenuItem("Prendere il sole", "Hai preso la crema?");
			UIMenuItem item56 = new UIMenuItem("Sdraiato a pancia in giu", "Occhio a chi arriva da dietro..");
			UIMenuItem item57 = new UIMenuItem("Pulisci", "Sguattero!");
			UIMenuItem item58 = new UIMenuItem("Fai un selfie", "Il popolo di Instagram ringrazia");

			animMenu6.AddItem(item51);
			animMenu6.AddItem(item52);
			animMenu6.AddItem(item53);
			animMenu6.AddItem(item54);
			animMenu6.AddItem(item55);
			animMenu6.AddItem(item56);
			animMenu6.AddItem(item57);
			animMenu6.AddItem(item58);

			animMenu6.OnItemSelect += (_menu, _item, _index) =>
			{
				if (!Game.PlayerPed.IsInVehicle() && Game.PlayerPed.IsAlive)
				{
					if (_item == item51)
					{
						Game.PlayerPed.Task.PlayAnimation("amb@world_human_aa_coffee@idle_a", "idle_a");
					}
					else if (_item == item52)
					{
						Game.PlayerPed.Task.PlayAnimation("anim@heists@prison_heistunfinished_biztarget_idle", "target_idle");
					}
					else if (_item == item53)
					{
						Game.PlayerPed.Task.StartScenario("WORLD_HUMAN_PICNIC", Game.Player.GetPlayerData().posizione.ToVector3());
					}
					else if (_item == item54)
					{
						Game.PlayerPed.Task.StartScenario("world_human_leaning", Game.Player.GetPlayerData().posizione.ToVector3());
					}
					else if (_item == item55)
					{
						Game.PlayerPed.Task.StartScenario("WORLD_HUMAN_SUNBATHE_BACK", Game.Player.GetPlayerData().posizione.ToVector3());
					}
					else if (_item == item56)
					{
						Game.PlayerPed.Task.StartScenario("WORLD_HUMAN_SUNBATHE", Game.Player.GetPlayerData().posizione.ToVector3());
					}
					else if (_item == item57)
					{
						Game.PlayerPed.Task.StartScenario("world_human_maid_clean", Game.Player.GetPlayerData().posizione.ToVector3());
					}
					else if (_item == item58)
					{
						Game.PlayerPed.Task.StartScenario("world_human_tourist_mobile", Game.Player.GetPlayerData().posizione.ToVector3());
					}
				}
				else
				{
					HUD.ShowNotification("Non puoi usare quest'animazione adesso!!", NotificationColor.Red);
				}
			};

			UIMenu animMenu7 = pool.AddSubMenu(animMenu, "Hard", "~g~La vita può prendere delle pieghe inaspettate nel RolePlay", pos);
			UIMenuItem item59 = new UIMenuItem("L'uomo si fa fare un b******* in auto", "Meglio di Cosi!");
			UIMenuItem item60 = new UIMenuItem("La donna fa un b******* in auto", "Pollice in SU! ..e non solo quello ;)");
			UIMenuItem item61 = new UIMenuItem("Uomo sesso in auto", "Grande!");
			UIMenuItem item62 = new UIMenuItem("Donna sesso in auto", "Complimentoni!");
			UIMenuItem item63 = new UIMenuItem("Panterona?", "");
			UIMenuItem item64 = new UIMenuItem("Fare l'affascinante!", "");
			UIMenuItem item65 = new UIMenuItem("Quanto sono bella/o??", "");
			UIMenuItem item66 = new UIMenuItem("Mostra il seno", "");
			UIMenuItem item67 = new UIMenuItem("Strip Tease 1", "");
			UIMenuItem item68 = new UIMenuItem("Strip Tease 2", "");
			UIMenuItem item69 = new UIMenuItem("Strip Tease 3(A terra)", "");

			animMenu7.AddItem(item59);
			animMenu7.AddItem(item60);
			animMenu7.AddItem(item61);
			animMenu7.AddItem(item62);
			animMenu7.AddItem(item63);
			animMenu7.AddItem(item64);
			animMenu7.AddItem(item65);
			animMenu7.AddItem(item66);
			animMenu7.AddItem(item67);
			animMenu7.AddItem(item68);
			animMenu7.AddItem(item69);

			animMenu7.OnItemSelect += (_menu, _item, _index) =>
			{
				if (!Game.PlayerPed.IsInVehicle() && Game.PlayerPed.IsAlive)
				{
					if (_item == item59)
					{
						Game.PlayerPed.Task.PlayAnimation("oddjobs@towing", "m_blow_job_loop");
					}
					else if (_item == item60)
					{
						Game.PlayerPed.Task.PlayAnimation("oddjobs@towing", "f_blow_job_loop");
					}
					else if (_item == item61)
					{
						Game.PlayerPed.Task.PlayAnimation("mini@prostitutes@sexlow_veh", "low_car_sex_loop_player");
					}
					else if (_item == item62)
					{
						Game.PlayerPed.Task.PlayAnimation("mini@prostitutes@sexlow_veh", "low_car_sex_loop_female");
					}
					else if (_item == item63)
					{
						Game.PlayerPed.Task.PlayAnimation("mp_player_int_uppergrab_crotch", "mp_player_int_grab_crotch");
					}
					else if (_item == item64)
					{
						Game.PlayerPed.Task.PlayAnimation("mini@strip_club@idles@stripper", "stripper_idle_02");
					}
					else if (_item == item65)
					{
						Game.PlayerPed.Task.StartScenario("WORLD_HUMAN_PROSTITUTE_HIGH_CLASS", Game.Player.GetPlayerData().posizione.ToVector3());
					}
					else if (_item == item66)
					{
						Game.PlayerPed.Task.PlayAnimation("mini@strip_club@backroom@", "stripper_b_backroom_idle_b");
					}
					else if (_item == item67)
					{
						Game.PlayerPed.Task.PlayAnimation("mini@strip_club@lap_dance@ld_girl_a_song_a_p1", "ld_girl_a_song_a_p1_f");
					}
					else if (_item == item68)
					{
						Game.PlayerPed.Task.PlayAnimation("mini@strip_club@private_dance@part2", "priv_dance_p2");
					}
					else if (_item == item69)
					{
						Game.PlayerPed.Task.PlayAnimation("mini@strip_club@private_dance@part3", "priv_dance_p3");
					}
				}
				else
				{
					HUD.ShowNotification("Non puoi usare quest'animazione adesso!!", NotificationColor.Red);
				}
			};

			UIMenu animMenu8 = pool.AddSubMenu(animMenu, "Nuove", "~g~Perche ci siamo evoluti~w~", pos);

			UIMenuItem item70 = new UIMenuItem("Facepalm", "La mamma degli idioti è sempre incinta");
			UIMenuItem item71 = new UIMenuItem("Incrocia le braccia", "Serio!");
			UIMenuItem item72 = new UIMenuItem("Dannazione", "");
			UIMenuItem item73 = new UIMenuItem("Fallimento", "");
			UIMenuItem item74 = new UIMenuItem("Incrocia le braccia2", "Serio!");
			UIMenuItem item75 = new UIMenuItem("batti le mani sarcastico", "");
			UIMenuItem item76 = new UIMenuItem("Tieni la folla", "calmi calmi");
			UIMenuItem item77 = new UIMenuItem("Tieni la folla2", "Ho detto calmi");

			animMenu7.OnItemSelect += (_menu, _item, _index) =>
			{
				if (!Game.PlayerPed.IsInVehicle() && Game.PlayerPed.IsAlive)
				{
					if (_item == item70)
					{
						Game.PlayerPed.Task.PlayAnimation("anim@mp_player_intupperface_palm", "idle_a");
					}
					else if (_item == item71)
					{
						Game.PlayerPed.Task.PlayAnimation("oddjobs@assassinate@construction@", "unarmed_fold_arms");
					}
					else if (_item == item72)
					{
						Game.PlayerPed.Task.PlayAnimation("gestures@m@standing@casual", "gesture_damn");
					}
					else if (_item == item73)
					{
						Game.PlayerPed.Task.PlayAnimation("random@car_thief@agitated@idle_a", "agitated_idle_a");
					}
					else if (_item == item74)
					{
						Game.PlayerPed.Task.PlayAnimation("oddjobs@assassinate@construction@", "idle_a");
					}
					else if (_item == item75)
					{
						Game.PlayerPed.Task.PlayAnimation("anim@mp_player_intcelebrationmale@slow_clap", "slow_clap");
					}
					else if (_item == item76)
					{
						Game.PlayerPed.Task.PlayAnimation("amb@code_human_police_crowd_control@idle_a", "idle_a");
					}
					else if (_item == item77)
					{
						Game.PlayerPed.Task.PlayAnimation("amb@code_human_police_crowd_control@idle_b", "idle_d");
					}
				}
				else
				{
					HUD.ShowNotification("Non puoi usare quest'animazione adesso!!", NotificationColor.Red);
				}
			};

			animMenu8.AddItem(item70);
			animMenu8.AddItem(item71);
			animMenu8.AddItem(item72);
			animMenu8.AddItem(item73);
			animMenu8.AddItem(item74);
			animMenu8.AddItem(item75);
			animMenu8.AddItem(item76);
			animMenu8.AddItem(item77);

			#endregion

			#region Gangs
			UIMenu bandeCriminali = pool.AddSubMenu(PersonalMenu, "Bande criminali", "Fonda e gestisti la tua banda criminale!");
			if (Game.Player.GetPlayerData().CurrentChar.gang.name == "Incensurato")
			{
				UIMenuItem diventaBoss = new UIMenuItem("Diventa Boss di una Banda!", "Baciamo le mani.");
				List<dynamic> lavoro = new List<dynamic>() { "No", "Si" };
				UIMenuListItem incerca = new UIMenuListItem("In cerca di \"Lavoro\"", lavoro, 0, GetLabelText("PIM_MAGH0D"));
				bandeCriminali.AddItem(diventaBoss);
				bandeCriminali.AddItem(incerca);
				// GB_BECOMEB = You are now the CEO of ~a~~s~
				// GB_GOON_OPEN = Hold ~INPUT_VEH_EXIT~to open the door for your Boss

				diventaBoss.Activated += async (menu, item) =>
				{
					if (Game.Player.GetPlayerData().Bank > 5000)
					{
						if (Main.GangsAttive.Count < 3)
						{
							string gname = await HUD.GetUserInput("Nome della Banda", "", 15);
							pool.CloseAllMenus();
							BigMessageThread.MessageInstance.ShowSimpleShard("Boss", $"Sei diventato il Boss della banda ~o~{gname}~w~.");
							Game.PlaySound("Boss_Message_Orange", "GTAO_Boss_Goons_FM_Soundset");
							BaseScript.TriggerServerEvent("lprp:updateCurChar", "gang", (new Gang(gname, 5)).Serialize());
							Main.GangsAttive.Add(new Gang(gname, Main.GangsAttive.Count + 1));
						}
						else
							HUD.ShowNotification("Ci sono già troppe Bande Criminali attive in sessione.~n~Riprova in un altro momento.", NotificationColor.Red, true);
					}
					else
						HUD.ShowNotification("Non possiedi abbastanza fondi bancari per diventare un Boss!", NotificationColor.Red, true);
				};
			}
			else
			{
				if (Game.Player.GetPlayerData().CurrentChar.gang.grade > 4)
				{
					UIMenu assumi = pool.AddSubMenu(bandeCriminali, "Assumi membri");
					UIMenu gestione = pool.AddSubMenu(bandeCriminali, "Gestione banda");
					UIMenu abilitàBoss = pool.AddSubMenu(bandeCriminali, "Abilità Boss");
					UIMenuItem ritirati = new UIMenuItem("Ritirati", "Attenzione.. non potrai fondare una banda prima di altre 6 ore!");
					bandeCriminali.AddItem(ritirati);
					ritirati.Activated += (menu, item) =>
					{
						pool.CloseAllMenus();
						Main.GangsAttive.Remove(Game.Player.GetPlayerData().CurrentChar.gang);
						BigMessageThread.MessageInstance.ShowSimpleShard("Ritirato", $"Non sei più il boss della banda ~o~{Game.Player.GetPlayerData().CurrentChar.gang.name}~w~.");
						Game.PlaySound("Boss_Message_Orange", "GTAO_Boss_Goons_FM_Soundset");
						BaseScript.TriggerServerEvent("lprp:updateCurChar", "gang", (new Gang("Incensurato", 0)).Serialize());
					};
				}
				else
				{
					UIMenuItem ritirati = new UIMenuItem("Ritirati", "Smetti di lavorare per il tuo boss attuale");
					bandeCriminali.AddItem(ritirati);
					ritirati.Activated += (menu, item) =>
					{
						pool.CloseAllMenus();
						BigMessageThread.MessageInstance.ShowSimpleShard("Ritirato", $"Non fai più parte della banda ~o~{Game.Player.GetPlayerData().CurrentChar.gang.name}~w~.");
						Game.PlaySound("Boss_Message_Orange", "GTAO_Boss_Goons_FM_Soundset");
						BaseScript.TriggerServerEvent("lprp:updateCurChar", "gang", (new Gang("Incensurato", 0)).Serialize());
					};
				}
			}
			#endregion
			#region Suicidio
			UIMenuListItem suicidio = new UIMenuListItem("Suicidati", new List<dynamic>() { "Medicine", "Pistola" }, 0, "~r~ATTENZIONE~w~, il suicidio assistito è un azzardo RP molto pericoloso.\nIl tempo di respawn sara' molto meno del solito e perderai tutto!");
			PersonalMenu.AddItem(suicidio);

			suicidio.OnListSelected += async (item, index) =>
			{
				RequestAnimDict("mp_suicide");
				while (!HasAnimDictLoaded("mp_suicide")) await BaseScript.Delay(0);

				string var = item.Items[index] as string;
				switch (var) 
				{
					case "Medicine":
						if (Game.Player.GetPlayerData().CurrentChar.skin.sex == "Maschio")
							Game.PlayerPed.Task.PlayAnimation("mp_suicide", "pill");
						else
							Game.PlayerPed.Task.PlayAnimation("mp_suicide", "pill_fp");
						break;
					case "Pistola":
						Game.PlayerPed.Weapons.Give(WeaponHash.Pistol, 1, true, true);
						string anim = "";
						if (Game.Player.GetPlayerData().CurrentChar.skin.sex == "Maschio")
							anim = "PISTOL";
						else
							anim = "PISTOL_FP";
						TaskPlayAnim(PlayerPedId(), "MP_SUICIDE", anim, 8f, -8f, -1, 270540800, 0, false, false, false);
						while (GetEntityAnimCurrentTime(PlayerPedId(), "MP_SUICIDE", anim) < 0.99f) await BaseScript.Delay(0);
						Game.PlayerPed.Weapons.Remove(WeaponHash.Pistol);
						HUD.MenuPool.CloseAllMenus();
						break;
				}


			};

			#endregion
			PersonalMenu.OnMenuClose += (_menu) => aperto = false;
			PersonalMenu.Visible = true;
		}

		private static async Task AggiornaSalute()
		{
			if (Game.Player.GetPlayerData().CurrentChar.needs.fame > 30f)
				fa.SetRightLabel("~y~" + Math.Round(Game.Player.GetPlayerData().CurrentChar.needs.fame, 2) + "%");
			else if (Game.Player.GetPlayerData().CurrentChar.needs.fame > 60f)
				fa.SetRightLabel("~o~" + Math.Round(Game.Player.GetPlayerData().CurrentChar.needs.fame, 2) + "%");
			else if (Game.Player.GetPlayerData().CurrentChar.needs.fame > 90f)
				fa.SetRightLabel("~r~" + Math.Round(Game.Player.GetPlayerData().CurrentChar.needs.fame, 2) + "%");
			else
				fa.SetRightLabel("~g~" + Math.Round(Game.Player.GetPlayerData().CurrentChar.needs.fame, 2) + "%");

			if (Game.Player.GetPlayerData().CurrentChar.needs.sete > 30f)
				se.SetRightLabel("~y~" + Math.Round(Game.Player.GetPlayerData().CurrentChar.needs.sete, 2) + "%");
			else if (Game.Player.GetPlayerData().CurrentChar.needs.sete > 60f)
				se.SetRightLabel("~o~" + Math.Round(Game.Player.GetPlayerData().CurrentChar.needs.sete, 2) + "%");
			else if (Game.Player.GetPlayerData().CurrentChar.needs.sete > 90f)
				se.SetRightLabel("~r~" + Math.Round(Game.Player.GetPlayerData().CurrentChar.needs.sete, 2) + "%");
			else
				se.SetRightLabel("~g~" + Math.Round(Game.Player.GetPlayerData().CurrentChar.needs.sete, 2) + "%");

			if (Game.Player.GetPlayerData().CurrentChar.needs.stanchezza > 30f)
				st.SetRightLabel("~y~" + Math.Round(Game.Player.GetPlayerData().CurrentChar.needs.stanchezza, 2) + "%");
			else if (Game.Player.GetPlayerData().CurrentChar.needs.stanchezza > 60f)
				st.SetRightLabel("~o~" + Math.Round(Game.Player.GetPlayerData().CurrentChar.needs.stanchezza, 2) + "%");
			else if (Game.Player.GetPlayerData().CurrentChar.needs.stanchezza > 90f)
				st.SetRightLabel("~r~" + Math.Round(Game.Player.GetPlayerData().CurrentChar.needs.stanchezza, 2) + "%");
			else
				st.SetRightLabel("~g~" + Math.Round(Game.Player.GetPlayerData().CurrentChar.needs.stanchezza, 2) + "%");

			if (Game.Player.GetPlayerData().CurrentChar.needs.malattia)
				ma.SetRightLabel("~r~In malattia");
			else
				ma.SetRightLabel("~g~In Salute");

			await BaseScript.Delay(3000);
		}

		public static async Task attiva()
		{
			if (EventiPersonalMenu.saveVehicle != null)
				fuelint = (int)Math.Floor(FuelClient.vehicleFuelLevel(EventiPersonalMenu.saveVehicle) / 65f * 100);
			if (Input.IsControlPressed(Control.InteractionMenu) && !HUD.MenuPool.IsAnyMenuOpen)
			{
				bool tasto = await Input.WaitForKeyRelease(Control.InteractionMenu);
				if (tasto && !aperto)
				{
					if (!MontagneRusse.SonoSeduto && RuotaPanoramica.GiroFinito)
					{
						menuPersonal();
						aperto = true;
					}
					else
						HUD.ShowNotification("Non puoi aprire il menu sulle giostre!", NotificationColor.Red, true);
				}
			}
			await Task.FromResult(0);
		}


		public enum RouteColor
		{
			Red = 1,
			Green = 2,
			Blue = 3,
			Yellow = 5,
		}
		public static async Task routeColor()
		{

			if (Vector3.Distance(Game.Player.GetPlayerData().posizione.ToVector3(), b.Position) > 5000f)
				SetBlipRouteColour(b.Handle, (int)RouteColor.Red);
			else if (Vector3.Distance(Game.Player.GetPlayerData().posizione.ToVector3(), b.Position) < 5000f && Vector3.Distance(Game.Player.GetPlayerData().posizione.ToVector3(), b.Position) > 4500f)
				SetBlipRouteColour(b.Handle, (int)RouteColor.Blue);
			else if (Vector3.Distance(Game.Player.GetPlayerData().posizione.ToVector3(), b.Position) < 4500f && Vector3.Distance(Game.Player.GetPlayerData().posizione.ToVector3(), b.Position) > 2500f)
				SetBlipRouteColour(b.Handle, (int)RouteColor.Yellow);
			else if (Vector3.Distance(Game.Player.GetPlayerData().posizione.ToVector3(), b.Position) < 2500f && Vector3.Distance(Game.Player.GetPlayerData().posizione.ToVector3(), b.Position) > 1500f)
				SetBlipRouteColour(b.Handle, (int)RouteColor.Yellow);
			else if (Vector3.Distance(Game.Player.GetPlayerData().posizione.ToVector3(), b.Position) < 1500f)
				SetBlipRouteColour(b.Handle, (int)RouteColor.Green);

			if (Vector3.Distance(Game.Player.GetPlayerData().posizione.ToVector3(), b.Position) < 20)
			{
				HUD.ShowNotification("GPS: Sei arrivato a ~b~Destinazione~w~!", NotificationColor.GreenDark, true);
				b.ShowRoute = false;
				Client.Instance.RemoveTick(routeColor);
			}
			await Task.FromResult(0);
		}

		public static string getStat(string name)
		{
			int val = 0;
			StatGetInt((uint)Game.GenerateHash(name), ref val, -1);
			return val.ToString();
		}
	}
}
