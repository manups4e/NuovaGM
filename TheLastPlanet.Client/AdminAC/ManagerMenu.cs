using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using TheLastPlanet.Client.Core.PlayerChar;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using ScaleformUI;
using TheLastPlanet.Shared;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.AdminAC
{
	internal static class ManagerMenu
	{
		private static bool SpawnaNelVeicolo = false;
		private static bool CancellaVecchioVeh = false;
		private static Vehicle VeicoloSalvato;

		public static async void AdminMenu(UserGroup group_level)
		{
			UIMenu AdminMenu = new("Admin menu", "Il menu di chi comanda!", new PointF(950, 50));
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
				if (state != MenuState.Opened) return;
				MenuPlayers.Clear();

				/*// COMMENTARE PER TESTARE SU ME STESSO 
					if (Client.Instance.GetPlayers.Count() == 1)
					{
						UIMenuItem nessuno = new UIMenuItem("Non ci sono player oltre te!");
						_menu.AddItem(nessuno);
						return;
					}
					*/
				foreach (Player p in Client.Instance.GetPlayers)
				{
					//if (p == Cache.Player) continue; // COMMENTARE PER TESTARE SU ME STESSO 
					User player = Funzioni.GetPlayerCharFromPlayerId(p.Handle);
					string charscount;
					if (player.Characters.Count == 1)
						charscount = "1 personaggio";
					else
						charscount = player.Characters.Count + " personaggi";
					UIMenu Giocatore = MenuPlayers.AddSubMenu(p.Name, charscount);
					UIMenuItem Teletrasportami = new("Teletrasportati alla sua posizione");
					UIMenuItem Teletrasportalo = new("Teletrasporta il player alla tua posizione");
					UIMenuItem Specta = new("Specta Player");
					Giocatore.AddItem(Teletrasportami);
					Giocatore.AddItem(Teletrasportalo);
					Giocatore.AddItem(Specta);
					Giocatore.OnItemSelect += async (menu, item, index) =>
					{
						if (item == Teletrasportami)
						{
							Cache.PlayerCache.MyPlayer.Ped.Position = p.Character.Position;
						}
						else if (item == Teletrasportalo)
						{
							Client.Instance.Events.Send("manager:TeletrasportaDaMe", p.ServerId);
						}
						else if (item == Specta)
						{
							if (p == Cache.PlayerCache.MyPlayer.Player) return;
							Cache.PlayerCache.MyPlayer.Ped.SetDecor("AdminSpecta", p.Handle);
							RequestCollisionAtCoord(p.Character.Position.X, p.Character.Position.Y, p.Character.Position.Z);
							NetworkSetInSpectatorMode(true, p.Character.Handle);
							Client.Instance.AddTick(SpectatorMode);
						}
					};

					#region Ban Player

					DateTime TempoDiBan = DateTime.Now.AddMinutes(10);
					string Motivazione = "";
					List<dynamic> tempiban = new()
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
						"100 anni (Perma-ban)"
					};
					UIMenu Ban = Giocatore.AddSubMenu("~r~Banna Player~w~");
					UIMenuItem motivazioneBan = new("Motivazione", "UNA VOLTA SPECIFICATA LA MOTIVAZIONE.. VERRA' MOSTRATA QUI!!");
					UIMenuListItem TempoBan = new("Tempo di Ban", tempiban, 0, "NB: UNA VOLTA CONFERMATO IL BAN, IL TEMPO ~h~~r~NON~w~ SI PUO' CAMBIARE");
					UIMenuItem Banna = new("Banna", "NB:~r~ IL BAN E' UNA TUA RESPONABILITA', DATO CHE IL TUO NOME VERRA' INSERITO NELLA MOTIVAZIONE~W~!", HudColor.HUD_COLOUR_REDDARK, HudColor.HUD_COLOUR_RED);
					UIMenuCheckboxItem temp = new("Temporaneo", UIMenuCheckboxStyle.Tick, false, "Temporaneo?");
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
						{
							BaseScript.TriggerServerEvent("lprp:bannaPlayer", p.ServerId, Motivazione, temp.Checked, TempoDiBan.Ticks, Cache.PlayerCache.MyPlayer.Player.ServerId);
						}

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
					UIMenuItem motivazioneKick = new("Motivazione");
					UIMenuItem Kicka = new("Kicka fuori dal server", "NB: ATTENZIONE! IL TUO NOME SARA' INSERITO ALLA FINE DELLA MOTIVAZIONE!");
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
						{
							BaseScript.TriggerServerEvent("lprp:kickPlayer", p.ServerId, motivazionekick, Cache.PlayerCache.MyPlayer.Player.ServerId);
						}
					};

					#endregion

					UIMenu Personaggi = Giocatore.AddSubMenu("~b~Gestione Personaggi~w~", charscount);

					foreach (Char_data chars in player.Characters)
					{
						UIMenu Character = Personaggi.AddSubMenu(chars.Info.firstname + " " + chars.Info.lastname);
						UIMenu DatiPersonali = Character.AddSubMenu("Dati Personali", "Nome, cognome, lavoro, gangs");
						UIMenu Inventario = Character.AddSubMenu("Inventario");
						UIMenu Armi = Character.AddSubMenu("Armi");
						UIMenu Finanze = Character.AddSubMenu("Finanze");
						UIMenu VeicoliPersonali = Character.AddSubMenu("Veicoli Personali");
						UIMenu Immobili = Character.AddSubMenu("Proprietà Possedute");
						UIMenu DatiGenerici = Character.AddSubMenu("Dati Generici", "Fedina Penale, Multe..");

						#region Dati Personali

						UIMenuItem nomeCognome = new("Nome e Cognome");
						nomeCognome.SetRightLabel(chars.Info.firstname + " " + chars.Info.lastname);
						UIMenuItem dDN = new("Data di Nascita");
						dDN.SetRightLabel(chars.Info.dateOfBirth);
						UIMenuItem sesso = new("Sesso");
						sesso.SetRightLabel(chars.Skin.sex);
						UIMenuItem altezza = new("Altezza");
						altezza.SetRightLabel(chars.Info.height + "cm");
						UIMenuItem job = new("Occupazione Attuale");
						job.SetRightLabel(chars.Job.Name);
						UIMenuItem telefono = new("N° di Telefono");
						telefono.SetRightLabel("" + chars.Info.phoneNumber);
						UIMenuItem assicurazione = new("N° di Assicurazione");
						assicurazione.SetRightLabel("" + chars.Info.insurance);
						DatiPersonali.AddItem(nomeCognome);
						DatiPersonali.AddItem(dDN);
						DatiPersonali.AddItem(sesso);
						DatiPersonali.AddItem(altezza);
						DatiPersonali.AddItem(job);
						DatiPersonali.AddItem(telefono);
						DatiPersonali.AddItem(assicurazione);

						#endregion

						#region Inventario

						UIMenuItem addItem = new("Aggiungi un oggetto all'inventario", "Dovrai inserire nome dell'oggetto e poi la sua quantità");
						Inventario.AddItem(addItem);

						if (chars.Inventory.Count > 0)
							foreach (Inventory item in chars.Inventory)
							{
								if (item.Amount <= 0) continue;
								UIMenu newItemMenu = Inventario.AddSubMenu(ConfigShared.SharedConfig.Main.Generici.ItemList[item.Item].label, "[Quantità: " + item.Amount.ToString() + "] " + ConfigShared.SharedConfig.Main.Generici.ItemList[item.Item].description);
								UIMenuItem add = new("Aggiungi", "Quanti ne ~y~aggiungiamo~w~?");
								UIMenuItem rem = new("Rimuovi", "Quanti ne ~y~rimuoviamo~w~?");
								newItemMenu.AddItem(add);
								newItemMenu.AddItem(rem);
								Inventory item1 = item;
								newItemMenu.OnItemSelect += async (menu, mitem, index) =>
								{
									if (mitem == add)
									{
										int quantita = Convert.ToInt32(await HUD.GetUserInput("Quantità", "1", 2));
										if (quantita < 99 && quantita > 0)
											BaseScript.TriggerServerEvent("lprp:addIntenvoryItemtochar", p.ServerId, chars.CharID, item1.Item, quantita);
										else
											HUD.ShowNotification("Quantità non valida!", NotificationColor.Red, true);
									}
									else if (mitem == rem)
									{
										int quantita = Convert.ToInt32(await HUD.GetUserInput("Quantità", "1", 2));
										if (quantita < 99 && quantita > 0)
											BaseScript.TriggerServerEvent("lprp:removeIntenvoryItemtochar", p.ServerId, chars.CharID, item1.Item, quantita);
										else
											HUD.ShowNotification("Quantità non valida!", NotificationColor.Red, true);
									}

									menu.ParentMenu.RefreshIndex();
								};
							}

						addItem.Activated += async (menu, item) =>
						{
							string oggetto = await HUD.GetUserInput("Nome dell'oggetto", "", 10);
							int quantita = Convert.ToInt32(await HUD.GetUserInput("Quantità", "1", 2));
							if (quantita < 99 && quantita > 0)
								BaseScript.TriggerServerEvent("lprp:addIntenvoryItemtochar", p.ServerId, chars.CharID, oggetto, quantita);
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
			};

			#endregion

			#region Veicoli

			UIMenuItem NomeVeh = new("Nome veicolo da spawnare");
			MenuVehicles.AddItem(NomeVeh);
			UIMenu VehOptions = MenuVehicles.AddSubMenu("Opzioni di spawn");
			UIMenuCheckboxItem spawn = new("Spawna nel veicolo", UIMenuCheckboxStyle.Tick, SpawnaNelVeicolo, "");
			UIMenuCheckboxItem deletepreviousveh = new("Cancella vecchio veicolo", UIMenuCheckboxStyle.Tick, CancellaVecchioVeh, "");
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
					VeicoloSalvato = await Funzioni.SpawnVehicle(input, Cache.PlayerCache.MyPlayer.Posizione.ToVector3, Cache.PlayerCache.MyPlayer.Posizione.Heading);
					if (VeicoloSalvato.Model.IsHelicopter || VeicoloSalvato.Model.IsPlane) SetHeliBladesFullSpeed(VeicoloSalvato.Handle);
				}
				else
				{
					VeicoloSalvato = await Funzioni.SpawnVehicleNoPlayerInside(input, GetOffsetFromEntityInWorldCoords(PlayerPedId(), 0, 5f, 0), Cache.PlayerCache.MyPlayer.Ped.Heading);
				}

				VeicoloSalvato.DirtLevel = 0;
				VeicoloSalvato.NeedsToBeHotwired = false;
				VeicoloSalvato.MarkAsNoLongerNeeded();
			};
			VehOptions.OnCheckboxChange += (menu, item, activated) =>
			{
				if (item == spawn)
					SpawnaNelVeicolo = activated;
				else if (item == deletepreviousveh) CancellaVecchioVeh = activated;
			};

			#endregion

			#region Oggetti

			UIMenuItem oggettoDaSpawnare = new("Oggetto da Spawnare");
			Oggetti.AddItem(oggettoDaSpawnare);
			Oggetti.OnItemSelect += async (menu, item, index) =>
			{
				string oggettino = "";

				if (item != oggettoDaSpawnare) return;
				oggettino = await HUD.GetUserInput("Inserisci oggetto o il suo HASH", "", 50);

				if (UpdateOnscreenKeyboard() == 3) return;

				if (!oggettino.All(o => char.IsDigit(o)) && !IsModelValid(Funzioni.HashUint(oggettino)))
				{
					HUD.ShowNotification("Modello errato!");

					return;
				}

				if (!IsModelValid((uint)Convert.ToInt32(oggettino)))
				{
					HUD.ShowNotification("Modello errato!");

					return;
				}

				//Prop obj = await World.CreateProp(oggettino.All(o => char.IsDigit(o)) ? new Model(Convert.ToInt32(oggettino)) : new Model(oggettino), GetOffsetFromEntityInWorldCoords(PlayerPedId(), 0, 5f, 0), true, true);
				Prop obj = await Funzioni.CreateProp(oggettino.All(o => char.IsDigit(o)) ? new Model(Convert.ToInt32(oggettino)) : new Model(oggettino), GetOffsetFromEntityInWorldCoords(PlayerPedId(), 0, 5f, 0), new Vector3(0, 0, Cache.PlayerCache.MyPlayer.Ped.Heading - 180f), true);
			};

			#endregion

			#region Meteo

			UIMenu metei = Meteo.AddSubMenu("Seleziona Meteo");
			UIMenuCheckboxItem blackout = new("BlackOut Generale", UIMenuCheckboxStyle.Tick, MODALITA.ROLEPLAY.TimeWeather.Meteo.BlackOut, "BlackOut di tutte le luci in mappa");
			UIMenuCheckboxItem dinamico = new("Meteo Dinamico", UIMenuCheckboxStyle.Tick, ConfigShared.SharedConfig.Main.Meteo.ss_enable_dynamic_weather, "NB: Sperimentale! Potrebbe non funzionare!\nAttiva o disattiva meteo dinamico, se disattivato.. il meteo resterà fisso!");
			Meteo.AddItem(blackout);
			Meteo.AddItem(dinamico);
			UIMenuItem Soleggiato = new("Super Soleggiato");
			UIMenuItem CSgombro = new("Cielo Sgombro");
			UIMenuItem Nuvoloso = new("Nuvoloso");
			UIMenuItem Smog = new("Smog");
			UIMenuItem Nebbioso = new("Nebbioso");
			UIMenuItem Nuvoloso2 = new("Nuvoloso");
			UIMenuItem Piovoso = new("Piovoso");
			UIMenuItem Tempesta = new("Tempestoso");
			UIMenuItem Sereno = new("Sereno");
			UIMenuItem Neutrale = new("Neutrale");
			UIMenuItem Nevoso = new("Nevoso");
			UIMenuItem Bufera = new("Bufera di neve");
			UIMenuItem NNebbia = new("Nevoso con Nebbia");
			UIMenuItem Natalizio = new("Natalizio");
			UIMenuItem Halloween = new("Halloween");
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
					BaseScript.TriggerServerEvent("changeWeatherWithParams", MODALITA.ROLEPLAY.TimeWeather.Meteo.CurrentWeather, _checked, false);
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
				BaseScript.TriggerServerEvent("changeWeatherWithParams", index, MODALITA.ROLEPLAY.TimeWeather.Meteo.BlackOut, false);
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

			UIMenuItem Mattino = new("Mattino", "Ore 6:00");
			UIMenuItem Pomeriggio = new("Pomeriggio", "Ore 12:00");
			UIMenuItem Sera = new("Sera", "Ore 18:00");
			UIMenuItem Notte = new("Notte", "Ore 21:00");
			Orario.AddItem(Mattino);
			Orario.AddItem(Pomeriggio);
			Orario.AddItem(Sera);
			Orario.AddItem(Notte);
			Orario.OnItemSelect += async (menu, item, index) =>
			{
				int secondOfDay = 0;
				if (item == Mattino)
					secondOfDay = 6 * 3600;
				else if (item == Pomeriggio)
					secondOfDay = 12 * 3600;
				else if (item == Sera)
					secondOfDay = 18 * 3600;
				else if (item == Notte) secondOfDay = 21 * 3600;
				BaseScript.TriggerServerEvent("UpdateFromCommandTime", secondOfDay);
			};

			#endregion

			if ((int)group_level < 2)
			{
				Meteo.ParentItem.Enabled = false;
				Meteo.ParentItem.Description = "NON HAI I PERMESSI NECESSARI";
				Meteo.ParentItem.SetRightBadge(BadgeIcon.LOCK);
				Orario.ParentItem.Enabled = false;
				Orario.ParentItem.Description = "NON HAI I PERMESSI NECESSARI";
				Orario.ParentItem.SetRightBadge(BadgeIcon.LOCK);
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
			if (Cache.PlayerCache.MyPlayer.Ped.HasDecor("AdminSpecta") && NetworkIsInSpectatorMode())
			{
				Game.DisableControlThisFrame(0, Control.Context);
				HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per smettere di spectare");

				if (Input.IsControlJustPressed(Control.Context))
				{
					Player p = new(Cache.PlayerCache.MyPlayer.Ped.GetDecor<int>("AdminSpecta"));
					NetworkSetInSpectatorMode(false, p.Character.Model);
					Cache.PlayerCache.MyPlayer.Ped.SetDecor("AdminSpecta", 0);
					Client.Instance.RemoveTick(SpectatorMode);
				}
			}
			else
			{
				NetworkSetOverrideSpectatorMode(false);
			}

			await Task.FromResult(0);
		}
	}
}