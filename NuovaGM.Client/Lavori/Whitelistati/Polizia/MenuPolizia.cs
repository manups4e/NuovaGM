using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

namespace TheLastPlanet.Client.Lavori.Whitelistati.Polizia
{
	public static class MenuPolizia
	{
		#region MenuSpogliatoio

		public static async void CloakRoomMenu()
		{
			AbitiLav PilotaMaschio = new AbitiLav() { Abiti = new ComponentDrawables(-1, 0, -1, 96, 41, -1, 24, 40, 15, 0, 0, 54), TextureVestiti = new ComponentDrawables(-1, 0, -1, 0, 0, -1, 0, 0, 0, 0, 0, 0), Accessori = new PropIndices(47, -1, -1, -1, -1, -1, -1, -1, -1), TexturesAccessori = new PropIndices(0, -1, -1, -1, -1, -1, -1, -1, -1) };
			AbitiLav PilotaFemmina = new AbitiLav() { Abiti = new ComponentDrawables(-1, 0, -1, 25, 0, -1, 24, 0, 13, 0, 0, 1), TextureVestiti = new ComponentDrawables(-1, 0, -1, 0, 1, -1, 0, 0, 0, 0, 0, 4), Accessori = new PropIndices(-1, -1, -1, -1, -1, -1, -1, -1, -1), TexturesAccessori = new PropIndices(-1, -1, -1, -1, -1, -1, -1, -1, -1) };
			UIMenu Spogliatoio = new UIMenu("Spogliatoio Polizia", "Cambiati ed entra/esci dal servizio");
			HUD.MenuPool.Add(Spogliatoio);

			#region PER TESTARE GLI OUTFIT GENERICI (TUTTI)

			/*
						foreach (Test p in PoliziaMainClient.Testicolo)
						{
							UIMenuItem abito = new UIMenuItem("Numero " + PoliziaMainClient.Testicolo.IndexOf(p));
							Spogliatoio.AddItem(abito);
						}

						Spogliatoio.OnIndexChange += async (menu, index) =>
						{
							for (int i = 0; i < 12; i++)
							{
								if (i != 2)
									SetPedComponentVariation(PlayerPedId(), i, PoliziaMainClient.Testicolo[index].ComponentDrawables[i], PoliziaMainClient.Testicolo[index].ComponentTextures[i], 2);
								if (i<9)
									if (PoliziaMainClient.Testicolo[index].PropIndices[i] == -1) 
										ClearPedProp(PlayerPedId(), i);
									else
										SetPedPropIndex(PlayerPedId(), i, PoliziaMainClient.Testicolo[index].PropIndices[i], PoliziaMainClient.Testicolo[index].PropTextures[i], true);
							}
							await Task.FromResult(0);
						};
			*/

			#endregion

			UIMenuItem Uniforme = new UIMenuItem("");
			UIMenuItem Giubbotto = new UIMenuItem("");
			UIMenuItem Pilota = new UIMenuItem("");

			switch (Cache.Char.StatiPlayer.InServizio)
			{
				case false when !PoliziaMainClient.InServizioDaPilota:
					Uniforme = new UIMenuItem("Indossa l'uniforme", "Se ti cambi entri automaticamente in servizio!");
					Pilota = new UIMenuItem("Indossa la tuta da Pilota", "Oggi lo piloti tu l'elicottero della liberta!");

					break;
				case false when PoliziaMainClient.InServizioDaPilota:
					Uniforme = new UIMenuItem("Indossa l'uniforme", "Se ti cambi entri automaticamente in servizio!");
					Pilota = new UIMenuItem("Rimuovi la tuta da Pilota", "Se ti cambi esci automaticamente dal servizio e le armi prese alla polizia verranno restituite!");

					break;
				case true when !PoliziaMainClient.InServizioDaPilota:
					Uniforme = new UIMenuItem("Rimuovi l'uniforme", "Se ti cambi esci automaticamente dal servizio e le armi prese alla polizia verranno restituite!");
					Pilota = new UIMenuItem("Indossa la tuta da Pilota", "Oggi lo piloti tu l'elicottero della liberta!");

					break;
				default:
				{
					if (Cache.Char.StatiPlayer.InServizio || PoliziaMainClient.InServizioDaPilota)
					{
						Giubbotto = Cache.PlayerPed.Armor < 1 ? new UIMenuItem("Indossa il Giubbotto Anti-Proiettile", "Potrebbe salvarti la vita") : new UIMenuItem("Rimuovi il Giubbotto Anti-Proiettile", "Speriamo sia stato utile");
						Spogliatoio.AddItem(Giubbotto);
					}

					break;
				}
			}

			Spogliatoio.AddItem(Uniforme);
			Spogliatoio.AddItem(Pilota);
			Spogliatoio.OnItemSelect += async (menu, item, index) =>
			{
				Screen.Fading.FadeOut(800);
				await BaseScript.Delay(1000);
				HUD.MenuPool.CloseAllMenus();
				NetworkFadeOutEntity(PlayerPedId(), true, false);

				if (item == Uniforme)
				{
					if (!Cache.Char.StatiPlayer.InServizio)
					{
						foreach (KeyValuePair<string, JobGrade> Grado in Client.Impostazioni.Lavori.Polizia.Gradi.Where(Grado => Cache.Char.CurrentChar.job.name == "Polizia").Where(Grado => Grado.Value.Id == Cache.Char.CurrentChar.job.grade))
							switch (Cache.Char.CurrentChar.skin.sex)
							{
								case "Maschio":
									CambiaVestito(Grado.Value.Vestiti.Maschio);

									break;
								case "Femmina":
									CambiaVestito(Grado.Value.Vestiti.Femmina);

									break;
							}

						Cache.Char.StatiPlayer.InServizio = true;
					}
					else
					{
						await Funzioni.UpdateDress(Cache.Char.CurrentChar.dressing);
						Cache.Char.StatiPlayer.InServizio = false;
					}
				}
				else if (item == Pilota)
				{
					switch (Cache.Char.CurrentChar.skin.sex)
					{
						case "Maschio":
							CambiaVestito(new AbitiLav() { Abiti = new ComponentDrawables(-1, 0, -1, 96, 41, -1, 24, 40, 15, 0, 0, 54), TextureVestiti = new ComponentDrawables(-1, 0, -1, 0, 0, -1, 0, 0, 0, 0, 0, 0), Accessori = new PropIndices(47, -1, -1, -1, -1, -1, -1, -1, -1), TexturesAccessori = new PropIndices(-1, -1, -1, -1, -1, -1, -1, -1, -1) });

							break;
						case "Femmina":
							CambiaVestito(new AbitiLav() { Abiti = new ComponentDrawables(-1, 0, -1, 111, 42, -1, 24, 24, 3, 0, 0, 47), TextureVestiti = new ComponentDrawables(-1, 0, -1, 0, 0, -1, 0, 0, 0, 0, 0, 0), Accessori = new PropIndices(46, 0, 0, 0, 0, 0, 0, 0, 0), TexturesAccessori = new PropIndices(0, -1, -1, -1, -1, -1, -1, -1, -1) });

							break;
					}
				}
				else if (item == Giubbotto)
				{
					if (Cache.PlayerPed.Armor < 1)
					{
						SetPedComponentVariation(PlayerPedId(), 9, 4, 1, 2);
						Cache.PlayerPed.Armor = 30;
					}
					else
					{
						SetPedComponentVariation(PlayerPedId(), 9, 0, 1, 2);
						Cache.PlayerPed.Armor = 0;
					}
				}

				NetworkFadeInEntity(PlayerPedId(), true);
				await BaseScript.Delay(500);
				Screen.Fading.FadeIn(800);
				menu.RefreshIndex();
				await Task.FromResult(0);
			};
			Spogliatoio.Visible = true;
		}

		public static async void CambiaVestito(AbitiLav dress)
		{
			int id = PlayerPedId();
			SetPedComponentVariation(id, (int)DrawableIndexes.Faccia, dress.Abiti.Faccia, dress.TextureVestiti.Faccia, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Maschera, dress.Abiti.Maschera, dress.TextureVestiti.Maschera, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Torso, dress.Abiti.Torso, dress.TextureVestiti.Torso, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Pantaloni, dress.Abiti.Pantaloni, dress.TextureVestiti.Pantaloni, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Borsa_Paracadute, dress.Abiti.Borsa_Paracadute, dress.TextureVestiti.Borsa_Paracadute, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Scarpe, dress.Abiti.Scarpe, dress.TextureVestiti.Scarpe, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Accessori, dress.Abiti.Accessori, dress.TextureVestiti.Accessori, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Sottomaglia, dress.Abiti.Sottomaglia, dress.TextureVestiti.Sottomaglia, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Kevlar, dress.Abiti.Kevlar, dress.TextureVestiti.Kevlar, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Badge, dress.Abiti.Badge, dress.TextureVestiti.Badge, 2);
			SetPedComponentVariation(id, (int)DrawableIndexes.Torso_2, dress.Abiti.Torso_2, dress.TextureVestiti.Torso_2, 2);
			SetPedPropIndex(id, (int)PropIndexes.Cappelli_Maschere, dress.Accessori.Cappelli_Maschere, dress.TexturesAccessori.Cappelli_Maschere, true);
			SetPedPropIndex(id, (int)PropIndexes.Orecchie, dress.Accessori.Orecchie, dress.TexturesAccessori.Orecchie, true);
			SetPedPropIndex(id, (int)PropIndexes.Occhiali_Occhi, dress.Accessori.Occhiali_Occhi, dress.TexturesAccessori.Occhiali_Occhi, true);
			SetPedPropIndex(id, (int)PropIndexes.Unk_3, dress.Accessori.Unk_3, dress.TexturesAccessori.Unk_3, true);
			SetPedPropIndex(id, (int)PropIndexes.Unk_4, dress.Accessori.Unk_4, dress.TexturesAccessori.Unk_4, true);
			SetPedPropIndex(id, (int)PropIndexes.Unk_5, dress.Accessori.Unk_5, dress.TexturesAccessori.Unk_5, true);
			SetPedPropIndex(id, (int)PropIndexes.Orologi, dress.Accessori.Orologi, dress.TexturesAccessori.Orologi, true);
			SetPedPropIndex(id, (int)PropIndexes.Bracciali, dress.Accessori.Bracciali, dress.TexturesAccessori.Bracciali, true);
			SetPedPropIndex(id, (int)PropIndexes.Unk_8, dress.Accessori.Unk_8, dress.TexturesAccessori.Unk_8, true);
		}

		#endregion

		#region MenuF6

		private static UIMenu MenuPoliziaPrincipale = new UIMenu("", "");
		private static UIMenu InterazioneCivile = new UIMenu("", "");
		private static UIMenu InterazioneVeicolo = new UIMenu("", "");
		private static UIMenu ControlliPersonaRemoto = new UIMenu("", "");
		private static UIMenu ControlliVeicoloRemoto = new UIMenu("", "");
		private static UIMenu Oggetti = new UIMenu("", "");
		private static uint StreetA = 0;
		private static uint StreetB = 0;
		private static UIMenu CercaPers = new UIMenu("", "");
		private static UIMenuItem Pos = new UIMenuItem("");

		public static async void MainMenu()
		{
			string nome = "";
			string cognome = "";
			string numero = "";
			MenuPoliziaPrincipale = new UIMenu("Menu Polizia", "IO SONO LA LEGGE!", new Point(50, 200));
			HUD.MenuPool.Add(MenuPoliziaPrincipale);
			InterazioneCivile = MenuPoliziaPrincipale.AddSubMenu("Interazioni col Cittadino", "Mi faccia vedere i dati!");
			InterazioneVeicolo = MenuPoliziaPrincipale.AddSubMenu("Interazioni col Veicolo", "Mi faccia controllare il veicolo!");
			ControlliPersonaRemoto = MenuPoliziaPrincipale.AddSubMenu("S.O.C.P.", "Sistema Online di Controllo Persone!");
			ControlliVeicoloRemoto = MenuPoliziaPrincipale.AddSubMenu("S.O.C.T.", "Sistema Online di Controllo Veicoli!");
			Oggetti = HUD.MenuPool.AddSubMenu(MenuPoliziaPrincipale, "Posa a terra un oggetto", "Abbiamo anche le bande chiodate!");

			#region CIVILE

			UIMenu DatiPlayer = InterazioneCivile.AddSubMenu("Carta d'identità", "Controllino");
			UIMenu Perquisizione = InterazioneCivile.AddSubMenu("Perquisisci", "Controllino");
			UIMenuItem ammanetta = new UIMenuItem("Ammanetta / Smanetta"); // CON ANIMAZIONE -- testare
			UIMenuItem accompagna = new UIMenuItem("Accompagna");          // SE RIESCO FACENDO ANCHE CAMMINARE IL PED DELLA PERSONA -- testare
			UIMenu FedinaPenale = InterazioneCivile.AddSubMenu("Controllo Fedina");
			UIMenuItem mettiVeicolo = new UIMenuItem("Fai sedere nel veicolo");
			UIMenuItem togliVeicolo = new UIMenuItem("Fai uscire dal veicolo");              // FAI USCIRE FAI ENTRARE CON ANIMAZIONE -- testare
			UIMenu multa = InterazioneCivile.AddSubMenu("Fai una Multa");                    // CREARE SISTEMA MULTE e aggiungere multa personalizzata 
			UIMenu fatture = InterazioneCivile.AddSubMenu("Controllo pagamenti in sospeso"); // SALVATAGGIO FATTURE IN DB
			UIMenuItem incarcera = new UIMenuItem("Incarcera");                              //DEVI ESSERE VICINO ALLA CELLA!
			UIMenu Licenze = InterazioneCivile.AddSubMenu("Controlla Licenze");              // controllo licenze e patenti
			InterazioneCivile.AddItem(ammanetta);
			InterazioneCivile.AddItem(accompagna);
			InterazioneCivile.AddItem(mettiVeicolo);
			InterazioneCivile.AddItem(togliVeicolo);
			InterazioneCivile.AddItem(incarcera);
			HUD.MenuPool.OnMenuStateChanged += async (_oldMenu, _newMenu, state) =>
			{
				if (_newMenu == DatiPlayer)
				{
					_newMenu.Clear();

					if (Client.Instance.GetPlayers.ToList().Count > 1)
					{
						Tuple<Player, float> Player_Distance = Funzioni.GetClosestPlayer();
						Ped ClosestPed = Player_Distance.Item1.Character;
						int playerServerId = GetPlayerServerId(Player_Distance.Item1.Handle);
						PlayerChar player = Funzioni.GetPlayerCharFromServerId(playerServerId);
						float distance = Player_Distance.Item2;

						if (distance < 3f && ClosestPed != null)
						{
							UIMenuItem nomeCognome = new UIMenuItem("Nome e Cognome");
							nomeCognome.SetRightLabel(player.FullName);
							UIMenuItem dDN = new UIMenuItem("Data di Nascita");
							dDN.SetRightLabel(player.DoB);
							UIMenuItem sesso = new UIMenuItem("Sesso");
							sesso.SetRightLabel(Cache.Char.CurrentChar.skin.sex);
							UIMenuItem altezza = new UIMenuItem("Altezza");
							altezza.SetRightLabel(Cache.Char.CurrentChar.info.height + "cm");
							UIMenuItem job = new UIMenuItem("Occupazione Attuale");
							job.SetRightLabel(Cache.Char.CurrentChar.job.name);
							UIMenuItem telefono = new UIMenuItem("N° di Telefono");
							telefono.SetRightLabel("" + Cache.Char.CurrentChar.info.phoneNumber);
							UIMenuItem assicurazione = new UIMenuItem("N° di Assicurazione");
							assicurazione.SetRightLabel("" + Cache.Char.CurrentChar.info.insurance);
							UIMenuItem nomePlayer = new UIMenuItem("Nome Player", "~r~ATTENZIONE!!~w~ - Da usare solo in caso di necessità~n~Un uso sbagliato verrà considerato metagame!");
							nomePlayer.SetRightLabel(Player_Distance.Item1.Name);
							DatiPlayer.AddItem(nomeCognome);
							DatiPlayer.AddItem(dDN);
							DatiPlayer.AddItem(sesso);
							DatiPlayer.AddItem(altezza);
							DatiPlayer.AddItem(job);
							DatiPlayer.AddItem(telefono);
							DatiPlayer.AddItem(assicurazione);
							DatiPlayer.AddItem(nomePlayer);
						}
						else
						{
							UIMenuItem noPlayers = new UIMenuItem("Non ci sono Player nelle vicinanze!");
							DatiPlayer.AddItem(noPlayers);
						}
					}
					else
					{
						UIMenuItem noPlayers = new UIMenuItem("Non ci sono altri Player nel server!");
						DatiPlayer.AddItem(noPlayers);
					}
				}
				else if (_newMenu == Perquisizione)
				{
					if (_newMenu.MenuItems.Count > 0) _newMenu.Clear();

					if (Client.Instance.GetPlayers.ToList().Count() > 1)
					{
						Tuple<Player, float> Player_Distance = Funzioni.GetClosestPlayer();
						Ped ClosestPed = Player_Distance.Item1.Character;
						int playerServerId = GetPlayerServerId(Player_Distance.Item1.Handle);
						PlayerChar player = Funzioni.GetPlayerCharFromServerId(playerServerId);
						float distance = Player_Distance.Item2;

						if (distance < 3f)
						{
							List<Inventory> inv = Cache.Char.Inventory;

							if (inv.Count > 0)
							{
								foreach (Inventory it in inv)
									if (it.amount > 0)
									{
										UIMenuItem oggetto = new UIMenuItem(it.item);
										oggetto.SetRightLabel($"Quantità: {it.amount}");
										_newMenu.AddItem(oggetto);
										oggetto.Activated += async (_menu, item) =>
										{
											BaseScript.TriggerServerEvent("lprp:polizia:confisca", it.item, it.amount);
										};
									}
							}
							else
							{
								Perquisizione.AddItem(new UIMenuItem("Questa persona non ha oggetti con se!"));
							}
						}
						else
						{
							Perquisizione.AddItem(new UIMenuItem("Non ci sono Player nelle vicinanze!"));
						}
					}
					else
					{
						Perquisizione.AddItem(new UIMenuItem("Non ci sono Players oltre te"));
					}
				}
				//else if(_newMenu == FedinaPenale)
				//else if(_newMenu == multa)
				//else if(_newMenu == fatture)
				else if (_newMenu == CercaPers)
				{
					Dictionary<string, PlayerChar> players = await Funzioni.GetAllPlayersAndTheirData();
					_newMenu.Clear();

					foreach (KeyValuePair<string, PlayerChar> pers in players.OrderBy(x => x.Key))
					{
						foreach (Char_data data in pers.Value.char_data)
							if (!string.IsNullOrEmpty(nome) && data.info.firstname.Contains(nome) || !string.IsNullOrEmpty(cognome) && data.info.lastname.Contains(cognome) || numero != "" && numero != null && data.info.phoneNumber.ToString().Contains(numero))
							{
								int source = 0;
								foreach (Player p in Client.Instance.GetPlayers.ToList().Where(p => p.Name == pers.Key)) source = p.ServerId;
								UIMenu Personaggio = CercaPers.AddSubMenu(data.info.firstname + " " + data.info.lastname + " [" + pers.Key + "]");
								UIMenuItem NomeCognome = new UIMenuItem("Nome:", "Il suo Nome");
								NomeCognome.SetRightLabel(data.info.firstname + " " + data.info.lastname);
								Personaggio.AddItem(NomeCognome);
								UIMenuItem DDN = new UIMenuItem("Data di Nascita:", "La sua data di nascita");
								DDN.SetRightLabel(data.info.dateOfBirth);
								Personaggio.AddItem(DDN);
								//UIMenuItem Altez = new UIMenuItem("Altezza:");
								UIMenuItem job = new UIMenuItem("Lavoro: ", "Il suo lavoro");
								job.SetRightLabel(data.job.name);
								Personaggio.AddItem(job);
								//UIMenuItem gang = new UIMenuItem("Gang: ", "Le affiliazioni");
								UIMenuItem bank = new UIMenuItem("Banca: ", "I soldi in banca");
								bank.SetRightLabel("$" + data.finance.bank);
								Personaggio.AddItem(bank);
								Pos = new UIMenuItem("Ultima Posizione conosciuta");
								if (source != 0)
									GetStreetNameAtCoord(Client.Instance.GetPlayers[source].Character.Position.X, Client.Instance.GetPlayers[source].Character.Position.Y, Client.Instance.GetPlayers[source].Character.Position.Z, ref StreetA, ref StreetB);
								else
									GetStreetNameAtCoord(data.location.position.X, data.location.position.Y, data.location.position.Z, ref StreetA, ref StreetB);
								Pos.Description = GetStreetNameFromHashKey(StreetA);
								if (StreetB != 0) Pos.Description = Pos.Description + ", angolo " + GetStreetNameFromHashKey(StreetB);
								Personaggio.AddItem(Pos);
								UIMenu fedinaPenale = Personaggio.AddSubMenu("Fedina Penale");
								UIMenu Fatture = Personaggio.AddSubMenu("Multe / Insoluti");
							}
					}
				}
				else if (_newMenu == MenuPoliziaPrincipale && state == MenuState.Opened)
				{
					Client.Instance.AddTick(ControlloMenu);
				}
				else if (state == MenuState.Closed && _oldMenu == MenuPoliziaPrincipale)
				{
					Client.Instance.RemoveTick(ControlloMenu);
				}

				;
			};
			InterazioneCivile.OnItemSelect += async (menu, item, index) =>
			{
				if (Client.Instance.GetPlayers.ToList().Count() > 1)
				{
					Tuple<Player, float> Player_Distance = Funzioni.GetClosestPlayer();
					Ped ClosestPed = Player_Distance.Item1.Character;
					int playerServerId = Player_Distance.Item1.ServerId;
					PlayerChar player = Funzioni.GetPlayerCharFromServerId(playerServerId);
					float distance = Player_Distance.Item2;

					if (distance < 3f && ClosestPed != null)
						switch (item)
						{
							case UIMenuItem i when i == ammanetta:
								BaseScript.TriggerServerEvent("lprp:polizia:ammanetta_smanetta", playerServerId);

								break;
							case UIMenuItem i when i == accompagna:
								if (player.StatiPlayer.Ammanettato) // rifare client-->server-->client
									BaseScript.TriggerServerEvent("lprp:polizia:accompagna", playerServerId, Cache.PlayerPed.NetworkId);
								else
									HUD.ShowNotification("Non è ammanettato!!");

								break;
							case UIMenuItem i when i == mettiVeicolo:
								if (player.StatiPlayer.Ammanettato) // rifare client-->server-->client
									BaseScript.TriggerServerEvent("lprp:polizia:mettiVeicolo", playerServerId);
								else
									HUD.ShowNotification("Non è ammanettato!!");

								break;
							case UIMenuItem i when i == togliVeicolo: // rifare client-->server-->client
								if (player.StatiPlayer.Ammanettato)
									BaseScript.TriggerServerEvent("lprp:polizia:esciVeicolo", playerServerId);
								else
									HUD.ShowNotification("Non è ammanettato!!");

								break;
							case UIMenuItem i when i == incarcera:
								break;
						}
					else
						HUD.ShowNotification("Nessuno trovato vicino a te..~n~Avvicinati!", NotificationColor.Red, true);
				}
				else
				{
					HUD.ShowNotification("Non ci sono altri giocatori!", NotificationColor.Red, true);
				}
			};

			#endregion

			#region VEICOLO

			UIMenu Controllo = InterazioneVeicolo.AddSubMenu("Controlla stato veicolo");
			UIMenuItem PickLock = new UIMenuItem("Apri veicolo chiuso", "Qualcuno lo ha chiuso?");
			UIMenuItem requisizione = new UIMenuItem("Requisisci Veicolo");
			InterazioneVeicolo.AddItem(PickLock);
			InterazioneVeicolo.AddItem(requisizione);
			InterazioneVeicolo.OnItemSelect += async (menu, item, index) =>
			{
				switch (item)
				{
					case UIMenuItem n when n == PickLock:
						// check veicolo di un player locked
						RequestAnimDict("anim@amb@clubhouse@tutorial@bkr_tut_ig3@");
						while (!HasAnimDictLoaded("anim@amb@clubhouse@tutorial@bkr_tut_ig3@")) await BaseScript.Delay(0);
						Cache.PlayerPed.Task.PlayAnimation("anim@amb@clubhouse@tutorial@bkr_tut_ig3@", "machinic_loop_mechandplayer", 8f, -1, AnimationFlags.Loop);
						await BaseScript.Delay(5000);
						// veicolo aperto qui da ora
						Cache.PlayerPed.Task.ClearAll();

						break;
					case UIMenuItem n when n == requisizione:
						TaskStartScenarioInPlace(PlayerPedId(), "CODE_HUMAN_MEDIC_TIME_OF_DEATH", 0, true);
						await BaseScript.Delay(5000);
						// veicolo eliminato e riportato in deposito...
						// oppure marchiato come non necessario (MarkAsNoLongerNeeded) e poi despawnato dopo un po'
						// oppure si chiama azienda di rimozione auto
						Cache.PlayerPed.Task.ClearAll();

						break;
				}
			};

			#endregion

			#region Ricerca

			UIMenuItem cercaNome = new UIMenuItem("Cerca per Nome");
			UIMenuItem cercaCognome = new UIMenuItem("Cerca per Cognome");
			UIMenuItem cercaNTelefono = new UIMenuItem("Cerca per N° di telefono");
			// apre un menu a parte (per dinamicità)
			// controllo tutti i player in ServerList (mandandoli al client ogni minuto / o piu tempo) controllando ogni personaggio di ogni player
			// e segnalo tutti quelli con quel nome / cognome. se selezionato mostro i suoi dati.
			ControlliPersonaRemoto.AddItem(cercaNome);
			ControlliPersonaRemoto.AddItem(cercaCognome);
			ControlliPersonaRemoto.AddItem(cercaNTelefono);
			CercaPers = ControlliPersonaRemoto.AddSubMenu("Effettua ricerca");
			UIMenuItem cercaModello = new UIMenuItem("Cerca per Modello");
			UIMenuItem cercaTarga = new UIMenuItem("Cerca per Targa");
			// COME SOPRA
			ControlliVeicoloRemoto.AddItem(cercaModello);
			ControlliVeicoloRemoto.AddItem(cercaTarga);
			ControlliPersonaRemoto.OnItemSelect += async (menu, item, index) =>
			{
				if (item == cercaNome)
				{
					nome = await HUD.GetUserInput("Inserisci il nome da cercare", "", 30);
					if (!nome.Any(char.IsDigit))
						item.SetRightLabel(nome);
					else
						HUD.ShowNotification("Non puoi ricercare un nome che contiene cifre!", NotificationColor.Red, true);
				}
				else if (item == cercaCognome)
				{
					cognome = await HUD.GetUserInput("Inserisci il cognome da cercare", "", 30);
					if (!cognome.Any(char.IsDigit))
						item.SetRightLabel(cognome);
					else
						HUD.ShowNotification("Non puoi ricercare un cognome che contiene cifre!", NotificationColor.Red, true);
				}
				else if (item == cercaNTelefono)
				{
					numero = await HUD.GetUserInput("Inserisci il numero da cercare", "", 30);
					if (numero.All(char.IsDigit))
						item.SetRightLabel(numero);
					else
						HUD.ShowNotification("Non puoi ricercare un numero che contiene lettere!", NotificationColor.Red, true);
				}
			};

			#endregion

			#region OGGETTI

			#endregion

			MenuPoliziaPrincipale.Visible = true;
		}

		#endregion

		#region MenuSpawnVeicoli

		///////////////////////// ELICOTTERI
		private static Vehicle PreviewHeli = new Vehicle(0);
		private static Camera HeliCam = new Camera(0);

		public static async void HeliMenu(StazioniDiPolizia Stazione, SpawnerSpawn Punto)
		{
			LoadInterior(GetInteriorAtCoords(-1267.0f, -3013.135f, -48.5f));
			RequestCollisionAtCoord(-1267.0f, -3013.135f, -48.5f);
			RequestAdditionalCollisionAtCoord(-1267.0f, -3013.135f, -48.5f);
			HeliCam = new Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", true)) { Position = new Vector3(-1268.174f, -2999.561f, -44.215f), IsActive = true };
			await BaseScript.Delay(1000);
			UIMenu MenuElicotteri = new UIMenu("Elicotteri Polizia", "Pattuglia le strade con stile!");
			HUD.MenuPool.Add(MenuElicotteri);

			foreach (Autorizzati t in Stazione.ElicotteriAutorizzati)
			{
				UIMenuItem veh = new UIMenuItem(t.Nome);
				MenuElicotteri.AddItem(veh);
			}

			MenuElicotteri.OnIndexChange += async (menu, index) =>
			{
				PreviewHeli = await Funzioni.SpawnLocalVehicle(Stazione.ElicotteriAutorizzati[index].Model, new Vector3(-1267.0f, -3013.135f, -48.490f), Punto.SpawnPoints[0].Heading);
				PreviewHeli.IsCollisionEnabled = false;
				PreviewHeli.IsPersistent = true;
				PreviewHeli.PlaceOnGround();
				PreviewHeli.IsPositionFrozen = true;
				if (PreviewHeli.Model.Hash == 353883353) SetVehicleLivery(PreviewHeli.Handle, 0); // per il polmav della pula.. funziona solo per i veicoli d'emergenza!
				PreviewHeli.LockStatus = VehicleLockStatus.Locked;
				SetHeliBladesFullSpeed(PreviewHeli.Handle);
				PreviewHeli.IsInvincible = true;
				PreviewHeli.IsEngineRunning = true;
				PreviewHeli.IsDriveable = false;
				if (HeliCam.IsActive && PreviewHeli.Exists()) HeliCam.PointAt(PreviewHeli);
			};
			MenuElicotteri.OnItemSelect += async (menu, item, index) =>
			{
				Screen.Fading.FadeOut(800);
				await BaseScript.Delay(1000);
				HeliCam.IsActive = false;
				RenderScriptCams(false, false, 0, false, false);

				foreach (SpawnPoints t in Punto.SpawnPoints)
					if (!Funzioni.IsSpawnPointClear(t.Coords, 2f))
					{
						continue;
					}
					else if (Funzioni.IsSpawnPointClear(t.Coords, 2f))
					{
						PoliziaMainClient.ElicotteroAttuale = await Funzioni.SpawnVehicle(Stazione.ElicotteriAutorizzati[index].Model, t.Coords, t.Heading);

						break;
					}
					else
					{
						PoliziaMainClient.ElicotteroAttuale = await Funzioni.SpawnVehicle(Stazione.ElicotteriAutorizzati[index].Model, Punto.SpawnPoints[0].Coords, Punto.SpawnPoints[0].Heading);

						break;
					}

				Cache.PlayerPed.CurrentVehicle.SetVehicleFuelLevel(100f);
				Cache.PlayerPed.CurrentVehicle.IsDriveable = true;
				Cache.PlayerPed.CurrentVehicle.Mods.LicensePlate = Funzioni.GetRandomInt(99) + "POL" + Funzioni.GetRandomInt(999);
				if (Cache.PlayerPed.CurrentVehicle.Model.Hash == 353883353) SetVehicleLivery(Cache.PlayerPed.CurrentVehicle.Handle, 0);
				Cache.PlayerPed.CurrentVehicle.SetDecor("VeicoloPolizia", Funzioni.GetRandomInt(100));
				VeicoloPol veh = new VeicoloPol(Cache.PlayerPed.CurrentVehicle.Mods.LicensePlate, Cache.PlayerPed.CurrentVehicle.Model.Hash, Cache.PlayerPed.CurrentVehicle.Handle);
				BaseScript.TriggerServerEvent("lprp:polizia:AggiungiVehPolizia", veh.Serialize());
				HUD.MenuPool.CloseAllMenus();
				PreviewHeli.MarkAsNoLongerNeeded();
				PreviewHeli.Delete();
			};
			HUD.MenuPool.OnMenuStateChanged += async (_oldmenu, _newmenu, _state) =>
			{
				if (_newmenu == MenuElicotteri)
				{
					if (_state == MenuState.Opened)
					{
						PreviewHeli = await Funzioni.SpawnLocalVehicle(Stazione.ElicotteriAutorizzati[0].Model, new Vector3(-1267.0f, -3013.135f, -48.490f), 0);
						PreviewHeli.IsCollisionEnabled = false;
						PreviewHeli.IsPersistent = true;
						PreviewHeli.PlaceOnGround();
						PreviewHeli.IsPositionFrozen = true;
						if (PreviewHeli.Model.Hash == 353883353) SetVehicleLivery(PreviewHeli.Handle, 0); // 0 per pula, 1 per medici.. funziona solo per i veicoli d'emergenza!
						PreviewHeli.LockStatus = VehicleLockStatus.Locked;
						PreviewHeli.IsInvincible = true;
						PreviewHeli.IsEngineRunning = true;
						PreviewHeli.IsDriveable = false;
						Client.Instance.AddTick(Heading);
						HeliCam.PointAt(PreviewHeli);
						if (GetInteriorFromEntity(PreviewHeli.Handle) != 0) SetFocusEntity(PreviewHeli.Handle);
						while (!HasCollisionLoadedAroundEntity(PreviewHeli.Handle)) await BaseScript.Delay(1000);
						RenderScriptCams(true, false, 0, false, false);
						Screen.Fading.FadeIn(800);
					}
				}
				else if (_state == MenuState.Closed && _oldmenu == MenuElicotteri)
				{
					Screen.Fading.FadeOut(800);
					await BaseScript.Delay(1000);
					Client.Instance.RemoveTick(Heading);
					HeliCam.IsActive = false;
					RenderScriptCams(false, false, 0, false, false);
					ClearFocus();
					await BaseScript.Delay(1000);
					Screen.Fading.FadeIn(800);
					PreviewHeli.MarkAsNoLongerNeeded();
					PreviewHeli.Delete();
				}
			};
			MenuElicotteri.Visible = true;
		}

		#endregion

		#region MenuVeicoliGarageSperimentale

		private static List<Vehicle> veicoliParcheggio = new List<Vehicle>();
		private static StazioniDiPolizia StazioneAttuale = new StazioniDiPolizia();
		private static int LivelloGarage = 0;
		private static List<Vector4> parcheggi = new List<Vector4>()
		{
			new Vector4(224.500f, -998.695f, -99.6f, 225.0f),
			new Vector4(224.500f, -994.630f, -99.6f, 225.0f),
			new Vector4(224.500f, -990.255f, -99.6f, 225.0f),
			new Vector4(224.500f, -986.628f, -99.6f, 225.0f),
			new Vector4(224.500f, -982.496f, -99.6f, 225.0f),
			new Vector4(232.500f, -982.496f, -99.6f, 135.0f),
			new Vector4(232.500f, -986.628f, -99.6f, 135.0f),
			new Vector4(232.500f, -990.255f, -99.6f, 135.0f),
			new Vector4(232.500f, -994.630f, -99.6f, 135.0f),
			new Vector4(232.500f, -998.695f, -99.6f, 135.0f)
		};
		private static SpawnerSpawn PuntoAttuale = new SpawnerSpawn();
		private static bool InGarage = false;

		public static async void VehicleMenuNuovo(StazioniDiPolizia Stazione, SpawnerSpawn Punto)
		{
			Cache.Char.StatiPlayer.Istanza.Istanzia("SceltaVeicoliPolizia");
			StazioneAttuale = Stazione;
			PuntoAttuale = Punto;
			Cache.PlayerPed.Position = new Vector3(236.349f, -1005.013f, -100f);
			Cache.PlayerPed.Heading = 85.162f;
			InGarage = true;

			if (Stazione.VeicoliAutorizzati.Count(o => o.GradiAutorizzati[0] == -1 || o.GradiAutorizzati.Contains(Cache.Char.CurrentChar.job.grade)) <= 10)
				for (int i = 0; i < Stazione.VeicoliAutorizzati.Count(o => o.GradiAutorizzati[0] == -1 || o.GradiAutorizzati.Contains(Cache.Char.CurrentChar.job.grade)); i++)
				{
					veicoliParcheggio.Add(await Funzioni.SpawnLocalVehicle(Stazione.VeicoliAutorizzati[i].Model, new Vector3(parcheggi[i].X, parcheggi[i].Y, parcheggi[i].Z), parcheggi[i].W));
					veicoliParcheggio[i].PlaceOnGround();
					veicoliParcheggio[i].IsPersistent = true;
					veicoliParcheggio[i].LockStatus = VehicleLockStatus.Unlocked;
					veicoliParcheggio[i].IsInvincible = true;
					veicoliParcheggio[i].IsCollisionEnabled = true;
					veicoliParcheggio[i].IsEngineRunning = false;
					veicoliParcheggio[i].IsDriveable = false;
					veicoliParcheggio[i].IsSirenActive = true;
					veicoliParcheggio[i].IsSirenSilent = true;
					veicoliParcheggio[i].SetDecor("VeicoloPolizia", Funzioni.GetRandomInt(100));
				}
			else
				await GarageConPiuVeicoli(Stazione.VeicoliAutorizzati, LivelloGarage);

			await BaseScript.Delay(1000);
			Screen.Fading.FadeIn(800);
			Client.Instance.AddTick(ControlloGarageNew);
		}

		private static async Task GarageConPiuVeicoli(List<Autorizzati> autorizzati, int livelloGarage)
		{
			foreach (Vehicle veh in veicoliParcheggio) veh.Delete();
			veicoliParcheggio.Clear();
			int totale = autorizzati.Count(o => o.GradiAutorizzati[0] == -1 || o.GradiAutorizzati.Contains(Cache.Char.CurrentChar.job.grade));
			int LivelloGarageAttuali = totale - livelloGarage * 10 > livelloGarage * 10 ? 10 : totale - livelloGarage * 10;

			for (int i = 0; i < LivelloGarageAttuali; i++)
			{
				veicoliParcheggio.Add(await Funzioni.SpawnLocalVehicle(autorizzati[i + livelloGarage * 10].Model, new Vector3(parcheggi[i].X, parcheggi[i].Y, parcheggi[i].Z), parcheggi[i].W));
				veicoliParcheggio[i].PlaceOnGround();
				veicoliParcheggio[i].IsPersistent = true;
				veicoliParcheggio[i].LockStatus = VehicleLockStatus.Unlocked;
				veicoliParcheggio[i].IsInvincible = true;
				veicoliParcheggio[i].IsCollisionEnabled = true;
				veicoliParcheggio[i].IsEngineRunning = false;
				veicoliParcheggio[i].IsDriveable = false;
				veicoliParcheggio[i].IsSirenActive = true;
				veicoliParcheggio[i].IsSirenSilent = true;
				veicoliParcheggio[i].SetDecor("VeicoloPolizia", Funzioni.GetRandomInt(100));
			}
		}

		#endregion

		#region MenuArmeria

		#endregion

		#region ControlliETask

		private static async Task ControlloGarageNew()
		{
			Ped p = Cache.PlayerPed;

			if (Cache.Char.StatiPlayer.Istanza.Stanziato)
				if (InGarage)
				{
					if (p.IsInRangeOf(new Vector3(240.317f, -1004.901f, -99f), 3f))
					{
						HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per cambiare piano");
						if (Input.IsControlJustPressed(Control.Context)) MenuPiano();
					}

					if (Cache.Char.StatiPlayer.InVeicolo)
						if (p.CurrentVehicle.HasDecor("VeicoloPolizia"))
						{
							HUD.ShowHelp("Per selezionare questo veicolo~n~~y~Accendi il motore~w~ e ~y~accelera~w~.");

							if (Input.IsControlJustPressed(Control.VehicleAccelerate) && p.CurrentVehicle.IsEngineRunning)
							{
								Screen.Fading.FadeOut(800);
								await BaseScript.Delay(1000);
								int model = p.CurrentVehicle.Model.Hash;
								foreach (Vehicle vehicle in veicoliParcheggio) vehicle.Delete();
								veicoliParcheggio.Clear();

								for (int i = 0; i < PuntoAttuale.SpawnPoints.Count; i++)
									if (!Funzioni.IsSpawnPointClear(PuntoAttuale.SpawnPoints[i].Coords, 2f))
									{
										continue;
									}
									else if (Funzioni.IsSpawnPointClear(PuntoAttuale.SpawnPoints[i].Coords, 2f))
									{
										PoliziaMainClient.VeicoloAttuale = await Funzioni.SpawnVehicle(model, PuntoAttuale.SpawnPoints[i].Coords, PuntoAttuale.SpawnPoints[i].Heading);

										break;
									}
									else
									{
										PoliziaMainClient.VeicoloAttuale = await Funzioni.SpawnVehicle(model, PuntoAttuale.SpawnPoints[0].Coords, PuntoAttuale.SpawnPoints[0].Heading);

										break;
									}

								p.CurrentVehicle.SetVehicleFuelLevel(100f);
								p.CurrentVehicle.IsEngineRunning = true;
								p.CurrentVehicle.IsDriveable = true;
								p.CurrentVehicle.Mods.LicensePlate = Funzioni.GetRandomInt(99) + "POL" + Funzioni.GetRandomInt(999);
								p.CurrentVehicle.SetDecor("VeicoloPolizia", Funzioni.GetRandomInt(100));
								VeicoloPol veh = new VeicoloPol(p.CurrentVehicle.Mods.LicensePlate, p.CurrentVehicle.Model.Hash, p.CurrentVehicle.Handle);
								BaseScript.TriggerServerEvent("lprp:polizia:AggiungiVehPolizia", veh.Serialize());
								InGarage = false;
								StazioneAttuale = null;
								PuntoAttuale = null;
								veicoliParcheggio.Clear();
								Cache.Char.StatiPlayer.Istanza.RimuoviIstanza();
								await BaseScript.Delay(1000);
								Screen.Fading.FadeIn(800);
								Client.Instance.RemoveTick(ControlloGarageNew);
							}
						}
				}
		}

		private static async void MenuPiano()
		{
			UIMenu Ascensore = new UIMenu("Seleziona Piano", "Sali o scendi?");
			HUD.MenuPool.Add(Ascensore);
			UIMenuItem esci = new UIMenuItem("Esci dal Garage");
			Ascensore.AddItem(esci);
			int conto = StazioneAttuale.VeicoliAutorizzati.Count(o => o.GradiAutorizzati[0] == -1 || o.GradiAutorizzati.Contains(Cache.Char.CurrentChar.job.grade));
			int piani = 1;
			for (int i = 1; i < conto + 1; i++)
				if (i % 10 == 0)
					piani++;

			for (int i = 0; i < piani; i++)
			{
				UIMenuItem piano = new UIMenuItem($"{i + 1}° piano");
				Ascensore.AddItem(piano);
				if (i == LivelloGarage) piano.SetRightBadge(BadgeStyle.Car);
			}

			Ascensore.OnItemSelect += async (menu, item, index) =>
			{
				if (item.RightBadge == BadgeStyle.Car)
				{
					HUD.ShowNotification("Questo è il garage attuale!!", true);
				}
				else
				{
					HUD.MenuPool.CloseAllMenus();
					Screen.Fading.FadeOut(800);
					await BaseScript.Delay(1000);

					if (item == esci)
					{
						Cache.PlayerPed.Position = StazioneAttuale.Veicoli[StazioneAttuale.Veicoli.IndexOf(PuntoAttuale)].SpawnerMenu;
						InGarage = false;
						StazioneAttuale = null;
						PuntoAttuale = null;
						Cache.Char.StatiPlayer.Istanza.RimuoviIstanza();
						veicoliParcheggio.Clear();
						Client.Instance.RemoveTick(ControlloGarageNew);
					}
					else
					{
						LivelloGarage = index - 1;
						await GarageConPiuVeicoli(StazioneAttuale.VeicoliAutorizzati, LivelloGarage);
					}

					await BaseScript.Delay(1000);
					Screen.Fading.FadeIn(800);
				}
			};
			Ascensore.Visible = true;
		}

		private static async Task ControlloMenu()
		{
			Ped p = Cache.PlayerPed;

			if (Cache.Char.StatiPlayer.InVeicolo)
			{
				if (p.CurrentVehicle.Driver == Cache.PlayerPed && p.CurrentVehicle.Speed < 2 || p.CurrentVehicle.GetPedOnSeat(VehicleSeat.Passenger) == Cache.PlayerPed)
				{
					if (InterazioneCivile.ParentItem.Enabled)
					{
						InterazioneCivile.ParentItem.Enabled = false;
						InterazioneCivile.ParentItem.SetRightBadge(BadgeStyle.Lock);
						InterazioneCivile.ParentItem.Description = InterazioneCivile.ParentItem.Description + " - ~r~NON~w~ disponibile dentro un veicolo";
					}

					if (InterazioneVeicolo.ParentItem.Enabled)
					{
						InterazioneVeicolo.ParentItem.Enabled = false;
						InterazioneVeicolo.ParentItem.SetRightBadge(BadgeStyle.Lock);
						InterazioneVeicolo.ParentItem.Description = InterazioneVeicolo.ParentItem.Description + " - ~r~NON~w~ disponibile dentro un veicolo";
					}

					if (Oggetti.ParentItem.Enabled)
					{
						Oggetti.ParentItem.Enabled = false;
						Oggetti.ParentItem.SetRightBadge(BadgeStyle.Lock);
						Oggetti.ParentItem.Description = Oggetti.ParentItem.Description + " - ~r~NON~w~ disponibile dentro un veicolo";
					}

					if (!ControlliPersonaRemoto.ParentItem.Enabled)
					{
						ControlliPersonaRemoto.ParentItem.Enabled = true;
						ControlliPersonaRemoto.ParentItem.SetRightBadge(BadgeStyle.None);
						ControlliPersonaRemoto.ParentItem.Description = "Sistema Online di Controllo Persone!";
					}

					if (!ControlliVeicoloRemoto.ParentItem.Enabled)
					{
						ControlliVeicoloRemoto.ParentItem.Enabled = true;
						ControlliVeicoloRemoto.ParentItem.SetRightBadge(BadgeStyle.None);
						ControlliVeicoloRemoto.ParentItem.Description = "Sistema Online di Controllo Veicoli!";
					}
				}
			}
			else
			{
				if (!InterazioneCivile.ParentItem.Enabled)
				{
					InterazioneCivile.ParentItem.Enabled = true;
					InterazioneCivile.ParentItem.SetRightBadge(BadgeStyle.None);
					InterazioneCivile.ParentItem.Description = "Mi faccia vedere i dati!";
				}

				if (!InterazioneVeicolo.ParentItem.Enabled)
				{
					InterazioneVeicolo.ParentItem.Enabled = true;
					InterazioneVeicolo.ParentItem.SetRightBadge(BadgeStyle.None);
					InterazioneVeicolo.ParentItem.Description = "Mi faccia controllare il veicolo!";
				}

				if (!Oggetti.ParentItem.Enabled)
				{
					Oggetti.ParentItem.Enabled = true;
					Oggetti.ParentItem.SetRightBadge(BadgeStyle.None);
					Oggetti.ParentItem.Description = "Abbiamo anche le bande chiodate!";
				}

				if (ControlliPersonaRemoto.ParentItem.Enabled)
				{
					ControlliPersonaRemoto.ParentItem.Enabled = false;
					ControlliPersonaRemoto.ParentItem.SetRightBadge(BadgeStyle.Lock);
					ControlliPersonaRemoto.ParentItem.Description = ControlliPersonaRemoto.ParentItem.Description + " - ~r~NON~w~ disponibile fuori da un veicolo della polizia o lontano da un computer!";
				}

				if (ControlliVeicoloRemoto.ParentItem.Enabled)
				{
					ControlliVeicoloRemoto.ParentItem.Enabled = false;
					ControlliVeicoloRemoto.ParentItem.SetRightBadge(BadgeStyle.Lock);
					ControlliVeicoloRemoto.ParentItem.Description = ControlliVeicoloRemoto.ParentItem.Description + " - ~r~NON~w~ disponibile fuori da un veicolo della polizia o lontano da un computer!";
				}
			}

			if (Client.Instance.GetPlayers.ToList().Count() > 1)
			{
				Tuple<Player, float> Player_Distance = Funzioni.GetClosestPlayer();
				float distance = Player_Distance.Item2;

				if (distance < 3)
				{
					if (!InterazioneCivile.ParentItem.Enabled)
					{
						InterazioneCivile.ParentItem.Enabled = true;
						InterazioneCivile.ParentItem.SetRightBadge(BadgeStyle.None);
						InterazioneCivile.ParentItem.Description = "Mi faccia vedere i dati!";
					}
				}
				else
				{
					if (InterazioneCivile.ParentItem.Enabled)
					{
						InterazioneCivile.ParentItem.Enabled = false;
						InterazioneCivile.ParentItem.SetRightBadge(BadgeStyle.Lock);
						InterazioneCivile.ParentItem.Description += "- ~r~NON~w~ ci sono player vicini";
						if (InterazioneCivile.Visible) InterazioneCivile.Visible = false;
					}
				}
			}
			else
			{
				if (InterazioneCivile.ParentItem.Enabled)
				{
					InterazioneCivile.ParentItem.Enabled = false;
					InterazioneCivile.ParentItem.SetRightBadge(BadgeStyle.Lock);
					InterazioneCivile.ParentItem.Description += "- ~r~NON~w~ ci sono player vicini";
					if (InterazioneCivile.Visible) InterazioneCivile.Visible = false;
				}
			}

			MenuPoliziaPrincipale.UpdateDescription();
			await BaseScript.Delay(250);
		}

		private static async Task Heading()
		{
			if (PreviewHeli.Exists())
			{
				RequestCollisionAtCoord(-1267.0f, -3013.135f, -48.5f);
				PreviewHeli.Heading += 1;
				if (!PreviewHeli.IsEngineRunning) SetHeliBladesFullSpeed(PreviewHeli.Handle);
			}

			await Task.FromResult(0);
		}

		#endregion
	}
}