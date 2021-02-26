﻿using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.MenuNativo;
using System;
using System.Collections.Generic;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Personaggio;

using System.Drawing;
using System.Linq;
using TheLastPlanet.Shared;
using System.Threading.Tasks;
using TheLastPlanet.Client.Core;

namespace TheLastPlanet.Client.Manager
{
	static class ManagerMenu
	{
		private static bool SpawnaNelVeicolo = false;
		private static bool CancellaVecchioVeh = false;
		private static Vehicle VeicoloSalvato;

		public static async void AdminMenu(UserGroup group_level)
		{
			UIMenu AdminMenu = new UIMenu("Admin menu", "Il menu di chi comanda!", new PointF(1439, 50));
			HUD.MenuPool.Add(AdminMenu);
			UIMenu MenuPlayers = AdminMenu.AddSubMenu("Gestione Giocatori", "~r~Attenzione!!~w~ - Qui non solo potrai gestire i giocatori ma anche i loro personaggi (soldi, lavoro, inventario, armi).\n~o~FAI ATTENZIONE!~w~");
			UIMenu MenuVehicles = AdminMenu.AddSubMenu("Menu Veicoli");
			UIMenu Oggetti = AdminMenu.AddSubMenu("Menu Oggetti");
			UIMenu MenuArmi = AdminMenu.AddSubMenu("Menu Armi");
			UIMenu Meteo = AdminMenu.AddSubMenu("Cambia Meteo");
			UIMenu Orario = AdminMenu.AddSubMenu("Cambia Ora del Server");

			#region Players
			MenuPlayers.OnMenuStateChanged += async (oldmenu, newmenu, state) =>
			{
				if (state == MenuState.Opened)
				{
					MenuPlayers.Clear();
					/*// COMMENTARE PER TESTARE SU ME STESSO 
					if (Client.Instance.GetPlayers.Count() == 1)
					{
						UIMenuItem nessuno = new UIMenuItem("Non ci sono player oltre te!");
						_menu.AddItem(nessuno);
						return;
					}
					*/
					foreach (var p in Client.Instance.GetPlayers)
					{
						//if (p == Cache.Player) continue; // COMMENTARE PER TESTARE SU ME STESSO 
						var player = Funzioni.GetPlayerCharFromPlayerId(p.Handle);
						string charscount;
						if (player.char_data.Count == 1)
							charscount = "1 personaggio";
						else
							charscount = player.char_data.Count + " personaggi";

						UIMenu Giocatore = MenuPlayers.AddSubMenu(p.Name, charscount);

						UIMenuItem Teletrasportami = new UIMenuItem("Teletrasportati alla sua posizione");
						UIMenuItem Teletrasportalo = new UIMenuItem("Teletrasporta il player alla tua posizione");
						UIMenuItem Specta = new UIMenuItem("Specta Player");
						Giocatore.AddItem(Teletrasportami);
						Giocatore.AddItem(Teletrasportalo);
						Giocatore.AddItem(Specta);

						Giocatore.OnItemSelect += async (menu, item, index) =>
						{
							if (item == Teletrasportami)
								Cache.PlayerPed.Position = p.Character.Position;
							else if (item == Teletrasportalo)
								BaseScript.TriggerServerEvent("lprp:manager:TeletrasportaDaMe", p.ServerId, Cache.Char.posizione.ToVector3());
							else if (item == Specta)
							{
								if (p == Cache.Player) return;
								Cache.PlayerPed.SetDecor("AdminSpecta", p.Handle);
								RequestCollisionAtCoord(p.Character.Position.X, p.Character.Position.Y, p.Character.Position.Z);
								NetworkSetInSpectatorMode(true, p.Character.Handle);
								Client.Instance.AddTick(SpectatorMode);
							}
						};

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
						"100 anni (Perma-ban)",
					};
						UIMenu Ban = Giocatore.AddSubMenu("~r~Banna Player~w~");

						UIMenuItem motivazioneBan = new UIMenuItem("Motivazione", "UNA VOLTA SPECIFICATA LA MOTIVAZIONE.. VERRA' MOSTRATA QUI!!");
						UIMenuListItem TempoBan = new UIMenuListItem("Tempo di Ban", tempiban, 0, "NB: UNA VOLTA CONFERMATO IL BAN, IL TEMPO ~h~~r~NON~w~ SI PUO' CAMBIARE");
						UIMenuItem Banna = new UIMenuItem("Banna", "NB:~r~ IL BAN E' UNA TUA RESPONABILITA', DATO CHE IL TUO NOME VERRA' INSERITO NELLA MOTIVAZIONE~W~!", Color.FromArgb(40, 195, 16, 13), Color.FromArgb(170, 165, 10, 7));
						UIMenuCheckboxItem temp = new UIMenuCheckboxItem("Temporaneo", UIMenuCheckboxStyle.Tick, false, "Temporaneo?");
						Ban.AddItem(motivazioneBan);
						Ban.AddItem(TempoBan);
						Ban.AddItem(temp);
						Ban.AddItem(Banna);

						Ban.OnItemSelect += async (menu, item, index) =>
						{
							if (item == motivazioneBan)
							{
								Motivazione = await HUD.GetUserInput("Motivazione del Ban", "", 175);
								motivazioneBan.Description = Motivazione;
								motivazioneBan.SetRightLabel(Motivazione.Length > 15 ? Motivazione.Substring(0, 15) + "..." : Motivazione);
								menu.UpdateDescription();
							}
							else if (item == Banna)
								BaseScript.TriggerServerEvent("lprp:bannaPlayer", p.ServerId, Motivazione, temp.Checked, TempoDiBan.Ticks, Cache.Player.ServerId);
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
								case "100 anni (Perma-ban)":
									TempoDiBan = ora.AddYears(100);
									break;
							}
						};
						#endregion

						#region Kick
						string motivazionekick = "";
						UIMenu Kick = Giocatore.AddSubMenu("~y~Kicka Player~w~");
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
								BaseScript.TriggerServerEvent("lprp:kickPlayer", p.ServerId, motivazionekick, Cache.Player.ServerId);
						};
						#endregion

						UIMenu Personaggi = Giocatore.AddSubMenu("~b~Gestione Personaggi~w~", charscount);


						foreach (Shared.Char_data chars in player.char_data)
						{
							UIMenu Character = Personaggi.AddSubMenu(chars.info.firstname + " " + chars.info.lastname);

							UIMenu DatiPersonali = Character.AddSubMenu("Dati Personali", "Nome, cognome, lavoro, gangs");
							UIMenu Inventario = Character.AddSubMenu("Inventario");
							UIMenu Armi = Character.AddSubMenu("Armi");
							UIMenu Finanze = Character.AddSubMenu("Finanze");
							UIMenu VeicoliPersonali = Character.AddSubMenu("Veicoli Personali");
							UIMenu Immobili = Character.AddSubMenu("Proprietà Possedute");
							UIMenu DatiGenerici = Character.AddSubMenu("Dati Generici", "Fedina Penale, Multe..");

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
								for (int i = 0; i < chars.inventory.Count; i++)
								{
									Inventory item = chars.inventory[i];
									if (item.amount > 0)
									{
										UIMenu newItemMenu = Inventario.AddSubMenu(ConfigShared.SharedConfig.Main.Generici.ItemList[item.item].label, "[Quantità: " + item.amount.ToString() + "] " + ConfigShared.SharedConfig.Main.Generici.ItemList[item.item].description);
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
													BaseScript.TriggerServerEvent("lprp:addIntenvoryItemtochar", p.ServerId, chars.id, item.item, quantita);
												else
													HUD.ShowNotification("Quantità non valida!", NotificationColor.Red, true);
											}
											else if (mitem == rem)
											{
												int quantita = Convert.ToInt32(await HUD.GetUserInput("Quantità", "1", 2));
												if (quantita < 99 && quantita > 0)
													BaseScript.TriggerServerEvent("lprp:removeIntenvoryItemtochar", p.ServerId, chars.id, item.item, quantita);
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
									BaseScript.TriggerServerEvent("lprp:addIntenvoryItemtochar", p.ServerId, chars.id, oggetto, quantita);
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
				}
			};

			#endregion

			#region Veicoli
			UIMenuItem NomeVeh = new UIMenuItem("Nome veicolo da spawnare");
			MenuVehicles.AddItem(NomeVeh);
			UIMenu VehOptions = MenuVehicles.AddSubMenu("Opzioni di spawn");

			UIMenuCheckboxItem spawn = new UIMenuCheckboxItem("Spawna nel veicolo", UIMenuCheckboxStyle.Tick, SpawnaNelVeicolo, "");
			UIMenuCheckboxItem deletepreviousveh = new UIMenuCheckboxItem("Cancella vecchio veicolo", UIMenuCheckboxStyle.Tick, CancellaVecchioVeh, "");
			VehOptions.AddItem(spawn);
			VehOptions.AddItem(deletepreviousveh);

			NomeVeh.Activated += async (menu, item) =>
			{
				string input = await HUD.GetUserInput("Modello del veicolo", "", 15);
				if (UpdateOnscreenKeyboard() == 3) return;
				if (!input.All(o => char.IsDigit(o)) && !IsModelValid(Funzioni.HashUint(input)))
				{
					HUD.ShowNotification("Modello errato!");
					return;
				}
				else if (!IsModelValid((uint)Convert.ToInt32(input)))
				{
					HUD.ShowNotification("Modello errato!");
					return;
				}
				if (CancellaVecchioVeh)
					if (VeicoloSalvato != null)
						VeicoloSalvato.Delete();
				if (SpawnaNelVeicolo)
				{
					VeicoloSalvato = await Funzioni.SpawnVehicle(input, Cache.Char.posizione.ToVector3(), Cache.Char.posizione.W);
					if (VeicoloSalvato.Model.IsHelicopter || VeicoloSalvato.Model.IsPlane)
						SetHeliBladesFullSpeed(VeicoloSalvato.Handle);
				}
				else
					VeicoloSalvato = await Funzioni.SpawnVehicleNoPlayerInside(input, GetOffsetFromEntityInWorldCoords(PlayerPedId(), 0, 5f, 0), Cache.PlayerPed.Heading);
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

			#region Oggetti
			UIMenuItem oggettoDaSpawnare = new UIMenuItem("Oggetto da Spawnare");
			Oggetti.AddItem(oggettoDaSpawnare);
			Oggetti.OnItemSelect += async (menu, item, index) =>
			{
				string oggettino = "";
				if (item == oggettoDaSpawnare)
				{
					oggettino = await HUD.GetUserInput("Inserisci oggetto o il suo HASH", "", 50);
					if (UpdateOnscreenKeyboard() == 3) return;
					if (!oggettino.All(o => char.IsDigit(o)) && !IsModelValid(Funzioni.HashUint(oggettino)))
					{
						HUD.ShowNotification("Modello errato!");
						return;
					}
					else if (!IsModelValid((uint)Convert.ToInt32(oggettino)))
					{
						HUD.ShowNotification("Modello errato!");
						return;
					}
					Prop obj = await World.CreateProp(oggettino.All(o => char.IsDigit(o)) ? new Model(Convert.ToInt32(oggettino)) : new Model(oggettino), GetOffsetFromEntityInWorldCoords(PlayerPedId(), 0, 5f, 0), true, true);
				}
			};
			#endregion

			#region Meteo
			UIMenu metei = Meteo.AddSubMenu("Seleziona Meteo");
			UIMenuCheckboxItem blackout = new UIMenuCheckboxItem("BlackOut Generale", UIMenuCheckboxStyle.Tick, TimeWeather.Meteo.BlackOut, "BlackOut di tutte le luci in mappa");
			UIMenuCheckboxItem dinamico = new UIMenuCheckboxItem("Meteo Dinamico", UIMenuCheckboxStyle.Tick, Shared.ConfigShared.SharedConfig.Main.Meteo.ss_enable_dynamic_weather, "NB: Sperimentale! Potrebbe non funzionare!\nAttiva o disattiva meteo dinamico, se disattivato.. il meteo resterà fisso!");
			Meteo.AddItem(blackout);
			Meteo.AddItem(dinamico);
			UIMenuItem Soleggiato = new UIMenuItem("Super Soleggiato");
			UIMenuItem CSgombro = new UIMenuItem("Cielo Sgombro");
			UIMenuItem Nuvoloso = new UIMenuItem("Nuvoloso");
			UIMenuItem Smog = new UIMenuItem("Smog");
			UIMenuItem Nebbioso = new UIMenuItem("Nebbioso");
			UIMenuItem Nuvoloso2 = new UIMenuItem("Nuvoloso");
			UIMenuItem Piovoso = new UIMenuItem("Piovoso");
			UIMenuItem Tempesta = new UIMenuItem("Tempestoso");
			UIMenuItem Sereno = new UIMenuItem("Sereno");
			UIMenuItem Neutrale = new UIMenuItem("Neutrale");
			UIMenuItem Nevoso = new UIMenuItem("Nevoso");
			UIMenuItem Bufera = new UIMenuItem("Bufera di neve");
			UIMenuItem NNebbia = new UIMenuItem("Nevoso con Nebbia");
			UIMenuItem Natalizio = new UIMenuItem("Natalizio");
			UIMenuItem Halloween = new UIMenuItem("Halloween");

			metei.AddItem(Soleggiato);
			metei.AddItem(CSgombro);
			metei.AddItem(Nuvoloso);
			metei.AddItem(Smog);
			metei.AddItem(Nebbioso);
			metei.AddItem(Nuvoloso2);
			metei.AddItem(Piovoso);
			metei.AddItem(Tempesta);
			metei.AddItem(Sereno);
			metei.AddItem(Neutrale);
			metei.AddItem(Nevoso);
			metei.AddItem(Bufera);
			metei.AddItem(NNebbia);
			metei.AddItem(Natalizio);
			metei.AddItem(Halloween);
			Meteo.OnCheckboxChange += async (menu, item, _checked) =>
			{
				if (item == blackout)
				{
					BaseScript.TriggerServerEvent("changeWeatherWithParams", TimeWeather.Meteo.CurrentWeather, _checked, false);
					HUD.ShowNotification("Blackout ~b~" + (_checked ? "attivato" : "disattivato") + "~w~.");
				}
				else if (item == dinamico)
				{
					BaseScript.TriggerServerEvent("changeWeatherDynamic", _checked);
					HUD.ShowNotification("Meteo dinamico ~b~" + (_checked ? "attivato" : "disattivato") + "~w~.");
				}
			};
			metei.OnItemSelect += async (menu, item, index) =>
			{
				BaseScript.TriggerServerEvent("changeWeatherWithParams", index, TimeWeather.Meteo.BlackOut, false);
				string m = "";
				switch (index)
				{
					case 0:
						m = "Super Soleggiato";
						break;
					case 1:
						m = "Cielo Sgombro";
						break;
					case 2:
						m = "Nuvoloso";
						break;
					case 3:
						m = "Smog";
						break;
					case 4:
						m = "Nebbioso";
						break;
					case 5:
						m = "Nuvoloso";
						break;
					case 6:
						m = "Piovoso";
						break;
					case 7:
						m = "Tempestoso";
						break;
					case 8:
						m = "Sereno";
						break;
					case 9:
						m = "Neutrale";
						break;
					case 10:
						m = "Nevoso";
						break;
					case 11:
						m = "Bufera di neve";
						break;
					case 12:
						m = "Nevoso con Nebbia";
						break;
					case 13:
						m = "Natalizio";
						break;
					case 14:
						m = "Halloween";
						break;
					default:
						m = "Sconosciuto?";
						break;
				}

				HUD.ShowNotification("Meteo in transizione verso ~b~" + m + "~w~.");
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
					secondOfDay = (6 * 3600);
				else if (item == Pomeriggio)
					secondOfDay = (12 * 3600);
				else if (item == Sera)
					secondOfDay = (18 * 3600);
				else if (item == Notte)
					secondOfDay = (21 * 3600);
				BaseScript.TriggerServerEvent("UpdateFromCommandTime", secondOfDay);
			};
			#endregion

			if ((int)group_level < 2)
			{
				Meteo.ParentItem.Enabled = false;
				Meteo.ParentItem.Description = "NON HAI I PERMESSI NECESSARI";
				Meteo.ParentItem.SetRightBadge(BadgeStyle.Lock);
				Orario.ParentItem.Enabled = false;
				Orario.ParentItem.Description = "NON HAI I PERMESSI NECESSARI";
				Orario.ParentItem.SetRightBadge(BadgeStyle.Lock);
			}
			else
			{
				Meteo.ParentItem.Description = "ATTENZIONE! QUESTI CAMBIAMENTI SI APPLICANO A TUTTI I GIOCATORI!";
				Orario.ParentItem.Description = "ATTENZIONE! QUESTI CAMBIAMENTI SI APPLICANO A TUTTI I GIOCATORI!";
			}
			if ((int)group_level < 5)
			{
				Oggetti.ParentItem.Enabled = false;
				Oggetti.ParentItem.Description = "NON HAI I PERMESSI NECESSARI";
			}
			AdminMenu.Visible = true;
		}

		private static async Task SpectatorMode()
		{
			if (Cache.PlayerPed.HasDecor("AdminSpecta") && NetworkIsInSpectatorMode())
			{
				Game.DisableControlThisFrame(0, Control.Context);
				HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per smettere di spectare");
				if (Input.IsControlJustPressed(Control.Context))
				{
					Player p = new Player(Cache.PlayerPed.GetDecor<int>("AdminSpecta"));
					NetworkSetInSpectatorMode(false, p.Character.Model);
					Cache.PlayerPed.SetDecor("AdminSpecta", 0);
					Client.Instance.RemoveTick(SpectatorMode);
				}
			}
			else
				NetworkSetOverrideSpectatorMode(false);
			await Task.FromResult(0);
		}
	}
}
