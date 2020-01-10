using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Client.MenuNativo;
using System;
using System.Collections.Generic;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Personaggio;
using NuovaGM.Shared;
using System.Drawing;

namespace NuovaGM.Client.Manager
{
	static class ManagerMenu
	{
		private static bool SpawnaNelVeicolo = false;
		private static bool CancellaVecchioVeh = false;
		private static Vehicle VeicoloSalvato;


		public static async void AdminMenu(int group_level)
		{
			UIMenu AdminMenu = new UIMenu("Admin menu", "Il menu di chi comanda!", new PointF(1439, 50));
			HUD.MenuPool.Add(AdminMenu);
			UIMenu MenuPlayers = HUD.MenuPool.AddSubMenu(AdminMenu, "Gestione Giocatori", "~r~Attenzione!!~w~ - Qui non solo potrai gestire i giocatori ma anche i loro personaggi (soldi, lavoro, inventario, armi). ~o~FAI ATTENZIONE!~w~");
			UIMenu MenuVehicles = HUD.MenuPool.AddSubMenu(AdminMenu, "Menu Veicoli");
			UIMenu MenuArmi = HUD.MenuPool.AddSubMenu(AdminMenu, "Menu Armi");
			UIMenu Meteo = HUD.MenuPool.AddSubMenu(AdminMenu, "Cambia Meteo");
			UIMenu Orario = HUD.MenuPool.AddSubMenu(AdminMenu, "Cambia Ora del Server");

			#region Players
			foreach (KeyValuePair<string, PlayerChar> player in Eventi.GiocatoriOnline)
			{
				string charscount;
				if (player.Value.char_data.Count == 1)
					charscount = "1 personaggio";
				else
					charscount = player.Value.char_data.Count + " personaggi";

				UIMenu Giocatore = HUD.MenuPool.AddSubMenu(MenuPlayers, GetPlayerName(GetPlayerFromServerId(player.Value.source)), charscount);

				#region Ban Player
				DateTime TempoDiBan = DateTime.Now.AddMinutes(10);
				string Motivazione = "";
				List<dynamic> tempiban = new List<dynamic>()
				{
					"10 min",
					"20 min",
					"30 min",
					"1 ora",
					"2 ore",
					"3 ore",
					"12 ore",
					"1 giorno",
					"2 giorni",
					"3 giorni",
					"1 settimana",
					"2 settimane",
					"3 settimane",
					"1 mese",
					"2 mesi",
					"3 mesi",
					"6 mesi",
					"1 anno",
					"Perma-Ban",
					"Tempo personalizzato"
				};
				UIMenu Ban = HUD.MenuPool.AddSubMenu(Giocatore, "~r~Banna Player~w~");

				UIMenuItem motivazioneBan = new UIMenuItem("Motivazione", "UNA VOLTA SPECIFICATA LA MOTIVAZIONE.. VERRA' MOSTRATA QUI!!");
				UIMenuListItem TempoBan = new UIMenuListItem("Tempo di Ban", tempiban, 0, "NB: UNA VOLTA CONFERMATO IL BAN, IL TEMPO ~h~~r~NON~w~ SI PUO' CAMBIARE");
				UIMenuItem Banna = new UIMenuItem("Banna", "NB:~r~ IL BAN E' UNA TUA RESPONABILITA', DATO CHE IL TUO NOME VERRA' INSERITO NELLA MOTIVAZIONE~W~!", Color.FromArgb(40, 195, 16, 13), Color.FromArgb(170, 165, 10, 7));
				Ban.AddItem(motivazioneBan);
				Ban.AddItem(TempoBan);
				Ban.AddItem(Banna);

				Ban.OnItemSelect += async (menu, item, index) =>
				{
					if (item == motivazioneBan)
					{
						Motivazione = await HUD.GetUserInput("Motivazione del Ban", "", 175);
						motivazioneBan.Description = Motivazione;
						motivazioneBan.SetRightLabel(Motivazione.Substring(0, 15) + "...");
						menu.UpdateDescription();
					}
					else if (item == Banna)
						BaseScript.TriggerServerEvent("lprp:bannaPlayer", player.Key, Motivazione, TempoDiBan.Ticks, Game.Player.ServerId);
					// string target, string motivazione, int tempodiban, string banner  - banner e target sono i serverid.. comodo eh?
				};

				TempoBan.OnListChanged += async (item, index) =>
				{
					DateTime ora = DateTime.Now;
					switch (item.Items[index])
					{
						case "10 min":
							TempoDiBan = ora.AddMinutes(10);
							break;
						case "20 min":
							TempoDiBan = ora.AddMinutes(20);
							break;
						case "30 min":
							TempoDiBan = ora.AddMinutes(30);
							break;
						case "1 ora":
							TempoDiBan = ora.AddHours(1);
							break;
						case "2 ore":
							TempoDiBan = ora.AddHours(2);
							break;
						case "3 ore":
							TempoDiBan = ora.AddHours(3);
							break;
						case "12 ore":
							TempoDiBan = ora.AddHours(12);
							break;
						case "1 giorno":
							TempoDiBan = ora.AddDays(1);
							break;
						case "2 giorni":
							TempoDiBan = ora.AddDays(2);
							break;
						case "3 giorni":
							TempoDiBan = ora.AddDays(3);
							break;
						case "1 settimana":
							TempoDiBan = ora.AddDays(7);
							break;
						case "2 settimane":
							TempoDiBan = ora.AddDays(14);
							break;
						case "3 settimane":
							TempoDiBan = ora.AddDays(21);
							break;
						case "1 mese":
							TempoDiBan = ora.AddMonths(1);
							break;
						case "2 mesi":
							TempoDiBan = ora.AddMonths(2);
							break;
						case "3 mesi":
							TempoDiBan = ora.AddMonths(3);
							break;
						case "6 mesi":
							TempoDiBan = ora.AddMonths(6);
							break;
						case "1 anno":
							TempoDiBan = ora.AddYears(1);
							break;
						case "Perma-Ban":
							TempoDiBan = ora.AddYears(100);
							break;
						case "Tempo personalizzato":
							TempoDiBan = ora.AddMinutes(1); // per ora
							break;
					}
				};
				#endregion

				#region Kick
				string motivazionekick = "";
				UIMenu Kick = HUD.MenuPool.AddSubMenu(Giocatore, "~y~Kicka Player~w~");
				UIMenuItem motivazioneKick = new UIMenuItem("Motivazione");
				UIMenuItem Kicka = new UIMenuItem("Kicka fuori dal server", "NB: ATTENZIONE! IL TUO NOME SARA' INSERITO ALLA FINE DELLA MOTIVAZIONE!");
				Kick.AddItem(motivazioneKick);
				Kick.AddItem(Kicka);

				Kick.OnItemSelect += async (menu, item, index) =>
				{
					if (item == motivazioneKick)
					{
						motivazionekick = await HUD.GetUserInput("Motivazione del kick?", "", 175);
						motivazioneKick.Description = motivazionekick;
						motivazioneKick.SetRightLabel(motivazionekick.Substring(0, 15) + "...");
					}
					else if (item == Kicka)
						BaseScript.TriggerServerEvent("lprp:kickPlayer", player.Key, motivazionekick, Game.Player.ServerId);
				};
				#endregion

				UIMenu Personaggi = HUD.MenuPool.AddSubMenu(Giocatore, "~b~Gestione Personaggi~w~", charscount);


				foreach (Shared.Char_data chars in player.Value.char_data)
				{
					UIMenu Character = HUD.MenuPool.AddSubMenu(Personaggi, chars.info.firstname + " " + chars.info.lastname);

					UIMenu DatiPersonali = HUD.MenuPool.AddSubMenu(Character, "Dati Personali", "Nome, cognome, lavoro, gangs");
					UIMenu Inventario = HUD.MenuPool.AddSubMenu(Character, "Inventario");
					UIMenu Armi = HUD.MenuPool.AddSubMenu(Character, "Armi");
					UIMenu Finanze = HUD.MenuPool.AddSubMenu(Character, "Finanze");
					UIMenu VeicoliPersonali = HUD.MenuPool.AddSubMenu(Character, "Veicoli Personali");
					UIMenu Immobili = HUD.MenuPool.AddSubMenu(Character, "Proprietà Possedute");
					UIMenu DatiGenerici = HUD.MenuPool.AddSubMenu(Character, "Dati Generici", "Fedina Penale, Multe..");

					#region Dati Personali
					UIMenuItem nomeCognome = new UIMenuItem("Nome e Cognome");
					nomeCognome.SetRightLabel(chars.info.firstname + " " + chars.info.lastname);
					UIMenuItem dDN = new UIMenuItem("Data di Nascita");
					dDN.SetRightLabel(chars.info.dateOfBirth);
					UIMenuItem sesso = new UIMenuItem("Sesso");
					sesso.SetRightLabel(chars.skin.sex);
					UIMenuItem altezza = new UIMenuItem("Altezza");
					altezza.SetRightLabel(chars.info.height + "cm");
					UIMenuItem job = new UIMenuItem("Occupazione Attuale");
					job.SetRightLabel(chars.job.name);
					UIMenuItem telefono = new UIMenuItem("N° di Telefono");
					telefono.SetRightLabel("" + chars.info.phoneNumber);
					UIMenuItem assicurazione = new UIMenuItem("N° di Assicurazione");
					assicurazione.SetRightLabel("" + chars.info.insurance);
					DatiPersonali.AddItem(nomeCognome);
					DatiPersonali.AddItem(dDN);
					DatiPersonali.AddItem(sesso);
					DatiPersonali.AddItem(altezza);
					DatiPersonali.AddItem(job);
					DatiPersonali.AddItem(telefono);
					DatiPersonali.AddItem(assicurazione);
					#endregion

					#region Inventario
					UIMenuItem addItem = new UIMenuItem("Aggiungi un oggetto all'inventario", "Dovrai inserire nome dell'oggetto e poi la sua quantità", Color.FromArgb(100, 0, 139, 139), Color.FromArgb(255, 0, 255, 255));
					Inventario.AddItem(addItem);
					if (chars.inventory.Count > 0)
					{
						for (int i=0; i < chars.inventory.Count; i++)
						{
							Inventory item = chars.inventory[i];
							if (item.amount > 0)
							{
								UIMenu newItemMenu = HUD.MenuPool.AddSubMenu(Inventario, SharedScript.ItemList[item.item].label, "[Quantità: " + item.amount.ToString() + "] " + SharedScript.ItemList[item.item].description);
								UIMenuItem add = new UIMenuItem("Aggiungi", "Quanti ne ~y~aggiungiamo~w~?", Color.FromArgb(40, 22, 242, 26), Color.FromArgb(170, 13, 195, 16));
								UIMenuItem rem = new UIMenuItem("Rimuovi", "Quanti ne ~y~rimuoviamo~w~?", Color.FromArgb(40, 195, 16, 13), Color.FromArgb(170, 165, 10, 7));
								newItemMenu.AddItem(add);
								newItemMenu.AddItem(rem);

								newItemMenu.OnItemSelect += async (menu, mitem, index) =>
								{
									if (mitem == add)
									{
										int quantita = Convert.ToInt32(await HUD.GetUserInput("Quantità", "1", 2));
										if (quantita < 99 && quantita > 0)
											BaseScript.TriggerServerEvent("lprp:addIntenvoryItemtochar", player.Key, chars.id, item.item, quantita);
										else
											HUD.ShowNotification("Quantità non valida!", NotificationColor.Red, true);
									}
									else if (mitem == rem)
									{
										int quantita = Convert.ToInt32(await HUD.GetUserInput("Quantità", "1", 2));
										if (quantita < 99 && quantita > 0)
											BaseScript.TriggerServerEvent("lprp:removeIntenvoryItemtochar", player.Key, chars.id, item.item, quantita);
										else
											HUD.ShowNotification("Quantità non valida!", NotificationColor.Red, true);
									}
									menu.ParentMenu.RefreshIndex();
								};
							}
						}
					}
					addItem.Activated += async (menu, item) =>
					{
						string oggetto = await HUD.GetUserInput("Nome dell'oggetto", "", 10);
						int quantita = Convert.ToInt32(await HUD.GetUserInput("Quantità", "1", 2));
						if (quantita < 99 && quantita > 0)
							BaseScript.TriggerServerEvent("lprp:addIntenvoryItemtochar", player.Key, chars.id, oggetto, quantita);
						else
							HUD.ShowNotification("Quantità non valida!", NotificationColor.Red, true);
						menu.RefreshIndex(); 
					};
					#endregion

					#region Armi
					#endregion

					#region Veicoli Personali
					#endregion

					#region Proprietà Possedute
					#endregion

					#region Immobili
					#endregion

					#region Dati Generici
					#endregion
				}
			}
			#endregion

			#region Veicoli
			UIMenuItem NomeVeh = new UIMenuItem("Nome veicolo da spawnare");
			MenuVehicles.AddItem(NomeVeh);
			UIMenu VehOptions = HUD.MenuPool.AddSubMenu(MenuVehicles, "Opzioni di spawn");

			UIMenuCheckboxItem spawn = new UIMenuCheckboxItem("Spawna nel veicolo", UIMenuCheckboxStyle.Tick, SpawnaNelVeicolo, "");
			UIMenuCheckboxItem deletepreviousveh = new UIMenuCheckboxItem("Cancella vecchio veicolo", UIMenuCheckboxStyle.Tick, CancellaVecchioVeh, "");
			VehOptions.AddItem(spawn);
			VehOptions.AddItem(deletepreviousveh);

			NomeVeh.Activated += async (menu, item) =>
			{
				string input = await HUD.GetUserInput("Modello del veicolo", "", 15);
				if (UpdateOnscreenKeyboard() == 3) return;
				if (!IsModelValid(Funzioni.HashUint(input)))
				{
					HUD.ShowNotification("Modello errato!");
					return;
				}
				if (CancellaVecchioVeh)
					if (VeicoloSalvato != null)
						VeicoloSalvato.Delete();
				if (SpawnaNelVeicolo) 
				{
					VeicoloSalvato = await Funzioni.SpawnVehicle(input, Game.PlayerPed.Position, Game.PlayerPed.Heading);
					if (VeicoloSalvato.Model.IsHelicopter || VeicoloSalvato.Model.IsPlane)
						SetHeliBladesFullSpeed(VeicoloSalvato.Handle);
				}
				else
					VeicoloSalvato = await Funzioni.SpawnVehicleNoPlayerInside(input, GetOffsetFromEntityInWorldCoords(PlayerPedId(), 0, 5f, 0), Game.PlayerPed.Heading);
				VeicoloSalvato.DirtLevel = 0;
				VeicoloSalvato.NeedsToBeHotwired = false;
				VeicoloSalvato.MarkAsNoLongerNeeded();	
			};

			VehOptions.OnCheckboxChange += (menu, item, activated) =>
			{
				if (item == spawn)
					SpawnaNelVeicolo = activated;
				else if (item == deletepreviousveh)
					CancellaVecchioVeh = activated;
			};
			#endregion

			#region Orario
			UIMenuItem Mattino = new UIMenuItem("Mattino", "Ore 6:00");
			UIMenuItem Pomeriggio = new UIMenuItem("Pomeriggio", "Ore 12:00");
			UIMenuItem Sera = new UIMenuItem("Sera", "Ore 18:00");
			UIMenuItem Notte = new UIMenuItem("Notte", "Ore 21:00");
			Orario.AddItem(Mattino);
			Orario.AddItem(Pomeriggio);
			Orario.AddItem(Sera);
			Orario.AddItem(Notte);
			Orario.OnItemSelect += async (menu, item, index) =>
			{
				int secondOfDay = 0;
				if (item == Mattino)
					secondOfDay = (6 * 3600) + 0 + 0;
				else if (item == Pomeriggio)
					secondOfDay = (12 * 3600) + 0 + 0;
				else if (item == Sera)
					secondOfDay = (18 * 3600) + 0 + 0;
				else if (item == Notte)
					secondOfDay = (21 * 3600) + 0 + 0;
				BaseScript.TriggerServerEvent("UpdateFromCommandTime", secondOfDay);
			};
			#endregion

			if (Eventi.Player.group_level < 2)
			{
				Meteo.ParentItem.Enabled = false;
				Meteo.ParentItem.Description = "NON HAI I PERMESSI NECESSARI";
				Meteo.ParentItem.SetRightBadge(UIMenuItem.BadgeStyle.Lock);
				Orario.ParentItem.Enabled = false;
				Orario.ParentItem.Description = "NON HAI I PERMESSI NECESSARI";
				Orario.ParentItem.SetRightBadge(UIMenuItem.BadgeStyle.Lock);
			}
			else
			{
				Meteo.ParentItem.Description = "ATTENZIONE! QUESTI CAMBIAMENTI SI APPLICANO A TUTTI I GIOCATORI!";
				Orario.ParentItem.Description = "ATTENZIONE! QUESTI CAMBIAMENTI SI APPLICANO A TUTTI I GIOCATORI!";
			}
			AdminMenu.Visible = true;
		}


	}
}
