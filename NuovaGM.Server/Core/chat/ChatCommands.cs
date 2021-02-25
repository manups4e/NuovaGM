using CitizenFX.Core;
using Logger;
using Newtonsoft.Json;
using TheLastPlanet.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;
using System.Text.RegularExpressions;

namespace TheLastPlanet.Server.Core
{
	static class ChatEvents
	{
		public static void Init()
		{
			Server.Instance.AddCommand("ooc", new Action<Player, List<string>, string>(Ooc), UserGroup.User, new ChatSuggestion("Scrivi in chat ~y~fuori dal personaggio~w~", new SuggestionParam[1] { new SuggestionParam("Testo", "Il testo da inserire") }));
			Server.Instance.AddCommand("pol", new Action<Player, List<string>, string>(Pol), UserGroup.User, new ChatSuggestion("Scrivi in chat con i tuoi colleghi ~y~poliziotti~w~", new SuggestionParam[1] { new SuggestionParam("Testo", "Il testo da inserire") }));
			Server.Instance.AddCommand("pil", new Action<Player, List<string>, string>(Pil), UserGroup.User, new ChatSuggestion("Scrivi in chat con i tuoi colleghi ~y~piloti~w~", new SuggestionParam[1] { new SuggestionParam("Testo", "Il testo da inserire") }));
			Server.Instance.AddCommand("med", new Action<Player, List<string>, string>(Med), UserGroup.User, new ChatSuggestion("Scrivi in chat con i tuoi colleghi ~y~poliziotti~w~", new SuggestionParam[1] { new SuggestionParam("Testo", "Il testo da inserire") }));
			Server.Instance.AddCommand("mec", new Action<Player, List<string>, string>(Mec), UserGroup.User, new ChatSuggestion("Scrivi in chat con i tuoi colleghi ~y~poliziotti~w~", new SuggestionParam[1] { new SuggestionParam("Testo", "Il testo da inserire") }));
			Server.Instance.AddCommand("me", new Action<Player, List<string>, string>(Me), UserGroup.User, new ChatSuggestion("Descrivi i tuoi stati d'animo e personali", new SuggestionParam[1] { new SuggestionParam("Testo", "Il testo da inserire") }));
			Server.Instance.AddCommand("do", new Action<Player, List<string>, string>(Do), UserGroup.User, new ChatSuggestion("Descrivi le tue azioni personali e interpersonali", new SuggestionParam[1] { new SuggestionParam("Testo", "Il testo da inserire") }));
			Server.Instance.AddCommand("giveitem", new Action<Player, List<string>, string>(GiveItem), UserGroup.Moderatore, new ChatSuggestion("Dai un oggetto a un player", new SuggestionParam[3] { new SuggestionParam("ID Player", "Il Server ID del player"), new SuggestionParam("Oggetto", "L'oggetto da dare al player"), new SuggestionParam("Quantità", "Quantità dell'oggetto da dare") }));
			Server.Instance.AddCommand("removeitem", new Action<Player, List<string>, string>(RemoveItem), UserGroup.Moderatore, new ChatSuggestion("Togli un oggetto a un player", new SuggestionParam[3] { new SuggestionParam("ID Player", "Il Server ID del player"), new SuggestionParam("Oggetto", "L'oggetto da togliere al player"), new SuggestionParam("Quantità", "Quantità dell'oggetto da togliere") }));
			Server.Instance.AddCommand("giveweapon", new Action<Player, List<string>, string>(GiveWeapon), UserGroup.Moderatore, new ChatSuggestion("Dai un'arma a un player", new SuggestionParam[3] { new SuggestionParam("ID Player", "Il Server ID del player"), new SuggestionParam("Arma", "L'arma da dare al player [es. weapon_pistol]"), new SuggestionParam("Quantità", "Quantità di munizioni da dare") }));
			Server.Instance.AddCommand("removeweapon", new Action<Player, List<string>, string>(RemoveWeapon), UserGroup.Moderatore, new ChatSuggestion("Togli un'arma a un player", new SuggestionParam[2] { new SuggestionParam("ID Player", "Il Server ID del player"), new SuggestionParam("Arma", "L'arma da togliere al player [es. weapon_pistol]")}));
			Server.Instance.AddCommand("givemoney", new Action<Player, List<string>, string>(GiveMoney), UserGroup.Moderatore, new ChatSuggestion("Dai soldi nel portafoglio ad un player", new SuggestionParam[2] { new SuggestionParam("ID Player", "Il Server ID del player"), new SuggestionParam("Quantità", "Quanti soldi vuoi dargli?")}));
			Server.Instance.AddCommand("givebank", new Action<Player, List<string>, string>(GiveBank), UserGroup.Moderatore, new ChatSuggestion("Dai soldi in banca ad un player", new SuggestionParam[2] { new SuggestionParam("ID Player", "Il Server ID del player"), new SuggestionParam("Quantità", "Quanti soldi vuoi dargli?") }));
			Server.Instance.AddCommand("givedirty", new Action<Player, List<string>, string>(GiveDirty), UserGroup.Moderatore, new ChatSuggestion("Dai soldi sporchi ad un player", new SuggestionParam[2] { new SuggestionParam("ID Player", "Il Server ID del player"), new SuggestionParam("Quantità", "Quanti soldi vuoi dargli?") }));
			Server.Instance.AddCommand("removemoney", new Action<Player, List<string>, string>(RemoveMoney), UserGroup.Moderatore, new ChatSuggestion("Rimuovi soldi nel portafoglio ad un player", new SuggestionParam[2] { new SuggestionParam("ID Player", "Il Server ID del player"), new SuggestionParam("Quantità", "Quanti soldi vuoi togliere?") }));
			Server.Instance.AddCommand("removebank", new Action<Player, List<string>, string>(RemoveBank), UserGroup.Moderatore, new ChatSuggestion("Rimuovi soldi in banca ad un player", new SuggestionParam[2] { new SuggestionParam("ID Player", "Il Server ID del player"), new SuggestionParam("Quantità", "Quanti soldi vuoi togliere?") }));
			Server.Instance.AddCommand("removedirty", new Action<Player, List<string>, string>(RemoveDirty), UserGroup.Moderatore, new ChatSuggestion("Rimuovi soldi sporchi ad un player", new SuggestionParam[2] { new SuggestionParam("ID Player", "Il Server ID del player"), new SuggestionParam("Quantità", "Quanti soldi vuoi togliere?") }));
			Server.Instance.AddCommand("setmoney", new Action<Player, List<string>, string>(SetFinances), UserGroup.Moderatore, new ChatSuggestion("Modifica definitivamente un account monetario del player", new SuggestionParam[3] { new SuggestionParam("ID Player", "Il Server ID del player"), new SuggestionParam("Account", "cash = soldi, bank = banca, dirty = sporchi"), new SuggestionParam("Quantità", "Attenzione, se ho 10 e metto 1, la quantità diventa 1") }));
			Server.Instance.AddCommand("annuncio", new Action<Player, List<string>, string>(Annuncio), UserGroup.Moderatore, new ChatSuggestion("Annuncio a tutti i giocatori", new SuggestionParam[1] { new SuggestionParam("Annuncio", "Messaggio da far leggere a tutti") }));
			Server.Instance.AddCommand("revive", new Action<Player, List<string>, string>(Revive), UserGroup.Moderatore, new ChatSuggestion("Rianima un giocatore", new SuggestionParam[1] { new SuggestionParam("ID Player", "[Opzionale] Il Server ID del player, se non inserisci niente rianimi te stesso") }));
			Server.Instance.AddCommand("setgroup", new Action<Player, List<string>, string>(SetGroup), UserGroup.Admin, new ChatSuggestion("Cambia gruppo al player", new SuggestionParam[2] { new SuggestionParam("ID Player", "Il Server ID del player"), new SuggestionParam("Id Gruppo", "0 = User, 1 = Helper, 2 = Moderatore, 3 = Admin, 4 = Founder, 5 = Sviluppatore") }));
			Server.Instance.AddCommand("tp", new Action<Player, List<string>, string>(Teleport), UserGroup.Moderatore, new ChatSuggestion("Teletrasportati alle coordinate", new SuggestionParam[3] { new SuggestionParam("X", ""), new SuggestionParam("Y", ""), new SuggestionParam("Z", "") }));
			Server.Instance.AddCommand("suicidati", new Action<Player, List<string>, string>(Muori), UserGroup.Moderatore, new ChatSuggestion("Uccide il tuo personaggio"));
			Server.Instance.AddCommand("car", new Action<Player, List<string>, string>(SpawnVehicle), UserGroup.Moderatore, new ChatSuggestion("Spawna un'auto e ti ci porta dentro", new SuggestionParam[1] { new SuggestionParam("Modello", "Il modello del veicolo da spawnare") }));
			Server.Instance.AddCommand("dv", new Action<Player, List<string>, string>(Dv), UserGroup.Moderatore, new ChatSuggestion("Elimina il veicolo corrente o quello a cui guardi"));
			Server.Instance.AddCommand("salvatutti", new Action<Player, List<string>, string>(Salvatutti), UserGroup.Moderatore, new ChatSuggestion("Salva tutti i giocatori subito"));
			Server.Instance.AddCommand("sviluppatore", new Action<Player, List<string>, string>(Sviluppatore), UserGroup.Sviluppatore, new ChatSuggestion("Attiva le funzioni dello sviluppatore", new SuggestionParam[1] { new SuggestionParam("Accensione", "On/Off") }));
			Server.Instance.AddCommand("setjob", new Action<Player, List<string>, string>(SetJob), UserGroup.Moderatore, new ChatSuggestion("Cambia lavoro ad un player", new SuggestionParam[3] { new SuggestionParam("ID Player", "Il Server ID del player"), new SuggestionParam("Lavoro", "Il lavoro da attivare"), new SuggestionParam("Grado", "Il grado lavorativo") }));
			Server.Instance.AddCommand("setgang", new Action<Player, List<string>, string>(SetGang), UserGroup.Moderatore, new ChatSuggestion("Cambia gang ad un giocatore", new SuggestionParam[3] { new SuggestionParam("ID Player", "Il Server ID del player"), new SuggestionParam("Gang", "La gang da settare"), new SuggestionParam("Grado", "Il grado della gang") }));
			Server.Instance.AddCommand("cambiaora", new Action<Player, List<string>, string>(Time), UserGroup.Admin, new ChatSuggestion("Cambia ora nel server", new SuggestionParam[2] { new SuggestionParam("Ore", ""), new SuggestionParam("Minuti", "") }));
			Server.Instance.AddCommand("bloccatempo", new Action<Player, List<string>, string>(FreezeTime), UserGroup.Admin, new ChatSuggestion("Frezza il tempo e non cambia piu ora", new SuggestionParam[1] { new SuggestionParam("Blocca / sblocca", "Si/True/Vero/1 - No/False/Falso/0")}));
			Server.Instance.AddCommand("setmeteo", new Action<Player, List<string>, string>(Weather), UserGroup.Admin, new ChatSuggestion("Cambia il meteo in gioco", new SuggestionParam[1] { new SuggestionParam("Meteo", "Inserisci il numero")}));
			Server.Instance.AddCommand("dailicenza", new Action<Player, List<string>, string>(DaiLicenza), UserGroup.Moderatore, new ChatSuggestion("Dai una licenza ad un player", new SuggestionParam[2] { new SuggestionParam("ID Player", "Il Server ID del player"), new SuggestionParam("Licenza", "La licenza da dare")}));
			Server.Instance.AddCommand("rimuovilicenza", new Action<Player, List<string>, string>(RimuoviLicenza), UserGroup.Moderatore, new ChatSuggestion("Togli una licenza ad un player", new SuggestionParam[2] { new SuggestionParam("ID Player", "Il Server ID del player"), new SuggestionParam("Licenza", "La licenza da togliere")}));

			//			Server.Instance.AddCommand("nome comando", new Action<Player, List<string>, string>(funzione comando), false, new ChatSuggestion("", new SuggestionParam[] { new SuggestionParam() }));

		}

		// GESTIONE CHAT
		public static async void Ooc(Player sender, List<string> args, string rawCommand)
		{
			try
			{
				if (args.Count > 0)
				{
					string noCom = rawCommand.Substring(5);

					string filtro = $"({Server.Impostazioni.Main.BadWords.Keys.Aggregate((i, j) => i + "|" + j)})";
					Regex filter = new Regex(filtro, RegexOptions.IgnoreCase);
					MatchCollection matches = filter.Matches(noCom);
					foreach (Match m in matches)
						noCom = filter.Replace(noCom, Server.Impostazioni.Main.BadWords[m.Value], 1);

					BaseScript.TriggerClientEvent("chat:addMessage", new { color = new[] { 0, 255, 153 }, multiline = true, args = new[] { "[FUORI RP] | " + sender.Name, noCom } });
				}
			}
			catch(Exception e)
			{
				Log.Printa(LogType.Error, e.ToString());
			}
		}

		public static void Pol(Player sender, List<string> args, string rawCommand)
		{
			var user = Funzioni.GetUserFromPlayerId(sender.Handle);
			if (user.CurrentChar.job.name.ToLower() == "polizia")
				Server.PlayerList.Values.Where(x => x.CurrentChar.job.name.ToLower() == "polizia").ToList().ForEach(x => x.p.TriggerEvent("chat:addMessage", new { color = new[] { 244, 65, 125 }, multiline = true, args = new[] { "[POLIZIA] | " + user.FullName, rawCommand.Substring(5) } }));
			else user.showNotification("Non puoi usare questo comando!");
		}

		public static void Pil(Player sender, List<string> args, string rawCommand)
		{
			var user = Funzioni.GetUserFromPlayerId(sender.Handle);
			if (user.CurrentChar.job.name.ToLower() == "pilota")
				Server.PlayerList.Values.Where(x => x.CurrentChar.job.name.ToLower() == "pilota").ToList().ForEach(x => x.p.TriggerEvent("chat:addMessage", new { color = new[] { 244, 223, 66 }, multiline = true, args = new[] { "[PILOTI] | " + user.FullName, rawCommand.Substring(5) } }));
			else user.showNotification("Non puoi usare questo comando!");
		}

		public static void Med(Player sender, List<string> args, string rawCommand)
		{
			var user = Funzioni.GetUserFromPlayerId(sender.Handle);
			if (user.CurrentChar.job.name.ToLower() == "medico")
				Server.PlayerList.Values.Where(x => x.CurrentChar.job.name.ToLower() == "medico").ToList().ForEach(x => x.p.TriggerEvent("chat:addMessage", new { color = new[] { 88, 154, 202 }, multiline = true, args = new[] { "[MEDICI] | " + user.FullName, rawCommand.Substring(5) } }));
			else user.showNotification("Non puoi usare questo comando!");
		}

		public static void Mec(Player sender, List<string> args, string rawCommand)
		{
			var user = Funzioni.GetUserFromPlayerId(sender.Handle);
			if (user.CurrentChar.job.name.ToLower() == "meccanico")
				Server.PlayerList.Values.Where(x => x.CurrentChar.job.name.ToLower() == "meccanico").ToList().ForEach(x => x.p.TriggerEvent("chat:addMessage", new { color = new[] { 102, 102, 255 }, multiline = true, args = new[] { "[MECCANICI] | " + user.FullName, rawCommand.Substring(5) } }));
			else user.showNotification("Non puoi usare questo comando!");
		}

		public static void Me(Player sender, List<string> args, string rawCommand)
		{
			BaseScript.TriggerClientEvent("lprp:triggerProximityDisplay", sender, "[ME]: ", rawCommand.Substring(4), 0, 255, 153);
		}

		public static void Do(Player sender, List<string> args, string rawCommand)
		{
			BaseScript.TriggerClientEvent("lprp:triggerProximityDisplay", sender, "[DO]: ", rawCommand.Substring(4), 0, 255, 153);
		}

		// FINE CHAT
		// GESTIONE INVENTARIO
		public static void GiveItem(Player sender, List<string> args, string rawCommand)
		{
			try
			{
				if (Server.PlayerList.ContainsKey(args[0]))
				{
					User player = Funzioni.GetUserFromPlayerId(args[0]);
					string item = "" + args[1];
					player.addInventoryItem(item, Convert.ToInt32(args[2]), Shared.ConfigShared.SharedConfig.Main.Generici.ItemList[item].peso);
				}
				else
					sender.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO giveitem] = ", "Il player con ID \"" + args[0] + "\" non è online!" }, color = new[] { 255, 0, 0 } });
			}
			catch
			{
				sender.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO giveitem] = ", "Errore nei parametri!" }, color = new[] { 255, 0, 0 } });
			}
		}

		public static void RemoveItem(Player sender, List<string> args, string rawCommand)
		{
			try
			{
				if (Server.PlayerList.ContainsKey(args[0]))
				{
					User player = Funzioni.GetUserFromPlayerId(args[0]);
					player.removeInventoryItem(args[1], Convert.ToInt32(args[2]));
				}
				else
					sender.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO removeitem] = ", "Il player con ID" + args[0] + " non è online!" }, color = new[] { 255, 0, 0 } });
			}
			catch
			{
				sender.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO removeitem] = ", "Errore nei parametri!" }, color = new[] { 255, 0, 0 } });
			}
		}

		public static void GiveWeapon(Player sender, List<string> args, string rawCommand)
		{
			try
			{
				if (Server.PlayerList.ContainsKey(args[0]))
				{
					User player = Funzioni.GetUserFromPlayerId(args[0]);
					player.addWeapon(args[1].ToUpper(), Convert.ToInt32(args[2]));
				}
				else
					sender.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO giveweapon] = ", "Il player con ID" + args[0] + " non è online!" }, color = new[] { 255, 0, 0 } });
			}
			catch
			{
				sender.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO giveweapon] = ", "Errore nei parametri!" }, color = new[] { 255, 0, 0 } });
			}
		}

		public static void RemoveWeapon(Player sender, List<string> args, string rawCommand)
		{
			try
			{
				if (Server.PlayerList.ContainsKey(args[0]))
				{
					User player = Funzioni.GetUserFromPlayerId(args[0]);
					player.removeWeapon(args[1].ToUpper());
				}
				else
					sender.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO removeweapon] = ", "Il player con ID" + args[0] + " non è online!" }, color = new[] { 255, 0, 0 } });
			}
			catch
			{
				sender.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO removeweapon] = ", "Errore nei parametri!" }, color = new[] { 255, 0, 0 } });
			}
		}
		// FINE GESTIONE INVENTARIO
		// GESTIONE DELLE FINANZE
		public static void GiveMoney(Player sender, List<string> args, string rawCommand)
		{
			if (Server.PlayerList.ContainsKey(args[0]))
				Eventi.GiveMoney(Server.Instance.GetPlayers[Convert.ToInt32(args[0])], Convert.ToInt32(args[1]));
			else
				sender.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO givemoney] = ", "Il player con ID" + args[0] + " non è online!" }, color = new[] { 255, 0, 0 } });
		}

		public static void GiveBank(Player sender, List<string> args, string rawCommand)
		{
			if (Server.PlayerList.ContainsKey(args[0]))
				Eventi.GiveBank(Server.Instance.GetPlayers[Convert.ToInt32(args[0])], Convert.ToInt32(args[1]));
			else
				sender.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO givebank] = ", "Il player con ID" + args[0] + " non è online!" }, color = new[] { 255, 0, 0 } });
		}

		public static void GiveDirty(Player sender, List<string> args, string rawCommand)
		{
			if (Server.PlayerList.ContainsKey(args[0]))
				Eventi.GiveDirty(Server.Instance.GetPlayers[Convert.ToInt32(args[0])], Convert.ToInt32(args[1]));
			else
				sender.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO givedirty] = ", "Il player con ID" + args[0] + " non è online!" }, color = new[] { 255, 0, 0 } });
		}

		public static void RemoveMoney(Player sender, List<string> args, string rawCommand)
		{
			if (Server.PlayerList.ContainsKey(args[0]))
				Eventi.RemoveMoney(Server.Instance.GetPlayers[Convert.ToInt32(args[0])], Convert.ToInt32(args[1]));
			else
				sender.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO givedirty] = ", "Il player con ID" + args[0] + " non è online!" }, color = new[] { 255, 0, 0 } });
		}

		public static void RemoveBank(Player sender, List<string> args, string rawCommand)
		{
			if (Server.PlayerList.ContainsKey(args[0]))
				Eventi.RemoveBank(Server.Instance.GetPlayers[Convert.ToInt32(args[0])], Convert.ToInt32(args[1]));
			else
				sender.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO givedirty] = ", "Il player con ID" + args[0] + " non è online!" }, color = new[] { 255, 0, 0 } });
		}

		public static void RemoveDirty(Player sender, List<string> args, string rawCommand)
		{
			if (Server.PlayerList.ContainsKey(args[0]))
				Eventi.RemoveDirty(Server.Instance.GetPlayers[Convert.ToInt32(args[0])], Convert.ToInt32(args[1]));
			else
				sender.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO givedirty] = ", "Il player con ID" + args[0] + " non è online!" }, color = new[] { 255, 0, 0 } });
		}

		public static void SetFinances(Player sender, List<string> args, string rawCommand)
		{
			if (Server.PlayerList.ContainsKey(args[0]))
			{
				var player = Funzioni.GetUserFromPlayerId(args[0]);
				if (args[1] == "soldi")
				{
					player.Money -= player.Money;
					player.Money += Convert.ToInt32(args[2]);
				}
				else if (args[1] == "banca")
				{
					player.Bank -= player.Bank;
					player.Bank += Convert.ToInt32(args[2]);
				}
				else if (args[1] == "sporchi")
				{
					player.DirtyMoney -= player.DirtyMoney;
					player.DirtyMoney += Convert.ToInt32(args[2]);
				}
				else
					sender.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO setmoney] = ", "L'account monetario '" + args[1] + "' non esiste!" }, color = new[] { 255, 0, 0 } });
			}
			else
				sender.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO setmoney] = ", "Il player con ID" + args[0] + " non è online!" }, color = new[] { 255, 0, 0 } });
		}

		// FINE GESTIONE FINANZE
		// ANNUNCIO PLAYERS
		public static void Annuncio(Player sender, List<string> args, string rawCommand)
		{
			BaseScript.TriggerClientEvent("lprp:announce", rawCommand.Replace("annuncio", string.Empty));
		}
		// FINE ANNUNCIO

		// REVIVE
		public static void Revive(Player sender, List<string> args, string rawCommand)
		{
			var now = DateTime.Now;
			if (args != null && args.Count > 0)
			{
				if (GetPlayerName(args[0]) != ".")
				{
					Player p = Server.Instance.GetPlayers[Convert.ToInt32(args[0])];
					Log.Printa(LogType.Info, "Comandi: " + sender.Name + " ha usato il comando revive su " + GetPlayerName(args[0]));
					BaseScript.TriggerEvent("lprp:serverlog", now.ToString("dd/MM/yyyy, HH:mm:ss") + " -- Comandi: " + sender.Name + " ha usato il comando revive su " + GetPlayerName(args[0]));
					BaseScript.TriggerClientEvent(p, "lprp:reviveChar");
				}
			}
			else
				sender.TriggerEvent("lprp:reviveChar");
		}
		// FINE REVIVE

		// SETGROUP
		public static async void SetGroup(Player sender, List<string> args, string rawCommand)
		{
			await BaseScript.Delay(0);
			var now = DateTime.Now;
			if (Convert.ToInt32(args[0]) > 0)
			{
				string group = "normal";
				int group_level = 0;
				Player ricevitore = Funzioni.GetPlayerFromId(args[0]);
				if (ricevitore.Name.Length > 0)
				{
					Log.Printa(LogType.Info, "Comandi: " + sender.Name + " ha usato il comando setgroup su " + ricevitore.Name);
					BaseScript.TriggerEvent("lprp:serverlog", now.ToString("dd/MM/yyyy, HH:mm:ss") + " -- Comandi: " + sender.Name + " ha usato il comando setgroup su " + ricevitore.Name);
					if (args[1] == "normal")
					{
						group = "normal";
						group_level = (int)UserGroup.User;
					}
					else if (args[1] == "helper")
					{
						group = "helper";
						group_level = (int)UserGroup.Helper;
					}
					else if (args[1] == "mod")
					{
						group = "moderatore";
						group_level = (int)UserGroup.Moderatore;
					}
					else if (args[1] == "admin")
					{
						group = "admin";
						group_level = (int)UserGroup.Admin;
					}
					else if (args[1] == "founder")
					{
						group = "founder";
						group_level = (int)UserGroup.Founder;
					}
					else if (args[1] == "dev")
					{
						group = "dev";
						group_level = (int)UserGroup.Sviluppatore;
					}
					await Server.Instance.Execute("UPDATE `users` SET `group` = @gruppo,  `group_level` = @groupL WHERE `discord` = @disc", new
					{
						gruppo = group,
						groupL = group_level,
						disc = License.GetLicense(ricevitore, Identifier.Discord)
					});
					var user = Funzioni.GetUserFromPlayerId(ricevitore.Handle);
					user.group = group;
					user.group_level = (UserGroup)group_level;
					Log.Printa(LogType.Info, $"Il player {ricevitore.Name} e' stato settato come gruppo {group}");
					BaseScript.TriggerEvent("lprp:serverLog", now.ToString("dd/MM/yyyy, HH:mm:ss") + $" --  Il player {ricevitore.Name} e' stato settato come gruppo {group}");
				}
				else
					Log.Printa(LogType.Error, "Il player con ID" + args[0] + " non è online!");
			}
			else
				Log.Printa(LogType.Error, "errore nel comando setgroup..riprova");
		}
		// FINE SETGROUP

		public static void Teleport(Player sender, List<string> args, string rawCommand)
		{
			float x = 0;
			float y = 0;
			float z = 0;
			if (float.TryParse(args[0], out x) && float.TryParse(args[1], out y) && float.TryParse(args[2], out z))
				sender.TriggerEvent("lprp:teleportCoords", args[0], args[1], args[2]);
			else
				sender.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO tp] = ", "Errore coordinate non valide, riprova!" }, color = new[] { 255, 0, 0 } });
		}

		public static void Muori(Player sender, List<string> args, string rawCommand)
		{
			sender.TriggerEvent("lprp:death");
		}

		public static void SpawnVehicle(Player sender, List<string> args, string rawCommand)
		{
			sender.TriggerEvent("lprp:spawnVehicle", args[0]);
		}

		public static void Dv(Player sender, List<string> args, string rawCommand)
		{
			sender.TriggerEvent("lprp:deleteVehicle");
		}

		public static void Delgun(Player sender, List<string> args, string rawCommand)
		{
			sender.TriggerEvent("lprp:ObjectDeleteGun", args[0]);
		}

		private static async void Salvatutti(Player sender, List<string> args, string rawCommand)
		{
			try
			{
				var now = DateTime.Now;
				foreach (var player in Server.PlayerList)
				{
					if (player.Value.status.spawned)
					{
						BaseScript.TriggerClientEvent(Funzioni.GetPlayerFromId(player.Key), "lprp:mostrasalvataggio");
						await Funzioni.SalvaPersonaggio(Funzioni.GetPlayerFromId(player.Key));
						Log.Printa(LogType.Info, "Salvato personaggio: '" + player.Value.FullName + "' appartenente a '" + Funzioni.GetPlayerFromId(player.Key).Name + "' - " + player.Value.identifiers.discord);
						BaseScript.TriggerEvent(DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + " Salvato personaggio: '" + player.Value.FullName + "' appartenente a '" + Funzioni.GetPlayerFromId(player.Key).Name + "' - " + player.Value.identifiers.discord);
						await Task.FromResult(0);
					}
				}
				BaseScript.TriggerClientEvent("lprp:aggiornaPlayers", Server.PlayerList.Serialize());
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Fatal, "" + ex);
			}
		}

		public static void Sviluppatore(Player sender, List<string> args, string rawCommand)
		{
			if (args[0].ToLower() == "on")
				sender.TriggerEvent("lprp:sviluppatoreOn", true);
			else if (args[0].ToLower() == "off")
				sender.TriggerEvent("lprp:sviluppatoreOn", false);
			else
				sender.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO sviluppatore] = ", "Errore argomento non valido, riprova!" }, color = new[] { 255, 0, 0 } });
		}

		public static void SetJob(Player sender, List<string> args, string rawCommand)
		{
			Player p = Funzioni.GetPlayerFromId(args[0]);
			if (Server.PlayerList.ContainsKey(p.Handle))
			{
				User pers = Funzioni.GetUserFromPlayerId(p.Handle);
				if (pers.status.spawned)
					pers.SetJob(args[1], Convert.ToInt32(args[2]));
				else
					sender.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO setjob] = ", "Errore il player non ha selezionato un personaggio, riprova!" }, color = new[] { 255, 0, 0 } });
			}
			else
				sender.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO setjob] = ", "Errore id player non trovato, riprova!" }, color = new[] { 255, 0, 0 } });
		}

		public static void SetGang(Player sender, List<string> args, string rawCommand)
		{
			Player p = Funzioni.GetPlayerFromId(args[0]);
			if (Server.PlayerList.ContainsKey(p.Handle))
			{
				User pers = Funzioni.GetUserFromPlayerId(p.Handle);
				if (pers.status.spawned)
					pers.SetGang(args[1], Convert.ToInt32(args[2]));
				else
					sender.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO setgang] = ", "Errore il player non ha selezionato un personaggio, riprova!" }, color = new[] { 255, 0, 0 } });
			}
			else
				sender.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO setgang] = ", "Errore id player non trovato, riprova!" }, color = new[] { 255, 0, 0 } });
		}

		public static void Time(Player sender, List<string> args, string rawCommand)
		{
			int Tempo = 0;
			int h = 0;
			int m = 0;
			if (args != null && args.Count > 0 && args.Count < 3)
			{
				if (args.Count == 2)
				{
					Tempo = ((Convert.ToInt32(args[0]) < 24 ? Convert.ToInt32(args[0]) : 0) * 3600) + ((Convert.ToInt32(args[1]) < 59 ? Convert.ToInt32(args[1]) : 0) * 60) + 0;
					h = (int)Math.Floor(Tempo / 3600f);
					m = (int)Math.Floor((Tempo - (h * 3600)) / 60f);
				}
				if (args.Count == 1)
				{
					Tempo = ((Convert.ToInt32(args[0]) < 24 ? Convert.ToInt32(args[0]) : 0) * 3600) + 0 + 0;
					h = (int)Math.Floor(Tempo / 3600f);
					m = 0;
				}
			}

			if (sender.Handle == "0")
			{
				if (args != null && args.Count > 0 && args.Count < 3)
				{
					BaseScript.TriggerEvent("UpdateFromCommandTime", Tempo);
					Log.Printa(LogType.Info, $"Time -- Orario server impostato alle ore {h}:{m}");
				}
				else
					Log.Printa(LogType.Error, $"Time -- Devi impostare almeno l'ora");
			}
			else
			{
				if (args != null && args.Count > 0 && args.Count < 3)
				{
					BaseScript.TriggerEvent("UpdateFromCommandTime", Tempo);
					sender.TriggerEvent("lprp:ShowNotification", $"Orario server impostato alle ~y~{h}:{m}~w~");
				}
				else
					sender.TriggerEvent("lprp:ShowNotification", $"ERRORE! Devi impostare almeno l'orario!");
			}
		}

		public static void FreezeTime(Player sender, List<string> args, string rawCommand)
		{
			int h = (int)Math.Floor(TimeWeather.Orario.secondOfDay / 3600f);
			int m = (int)Math.Floor((TimeWeather.Orario.secondOfDay - (h * 3600)) / 60f);
			bool freeze = false;
			if (args[0].ToLower() == "true" || args[0].ToLower() == "1" || args[0].ToLower() == "vero" || args[0].ToLower() == "si")
				freeze = true;
			if (sender.Handle == "0")
			{
				BaseScript.TriggerEvent("freezeTime", freeze);
				if (freeze)
					Log.Printa(LogType.Info, $"Orario di gioco bloccato alle ore {h}:{m}");
				else
					Log.Printa(LogType.Info, $"Orario di gioco sbloccato dalle ore {h}:{m}");
			}
			else
			{
				BaseScript.TriggerEvent("freezeTime", freeze);
				if (freeze)
					sender.TriggerEvent("lprp:ShowNotification", $"Orario di gioco bloccato alle ore ~b~{h}:{m}~w~");
				else
					sender.TriggerEvent("lprp:ShowNotification", $"Orario di gioco sbloccato dlle ore ~b~{h}:{m}~w~");
			}
		}

		public static void Weather(Player sender, List<string> args, string rawCommand)
		{
			if (sender.Handle == "0")
			{
				if (args.Count > 1 || Convert.ToInt32(args[0]) > 14 || !(args[0] as string).All(o => char.IsDigit(o)))
				{
					Log.Printa(LogType.Error, "/weather <weathertype>\nCurrent Weather: " + TimeWeather.Meteo.currentWeather + "\nErrore weather, argomenti disponibili: 0 = EXTRASUNNY, 1 =  CLEAR, 2 = CLOUDS, 3 = SMOG, 4 = FOGGY, 5 = OVERCAST, 6 = RAIN, 7 = THUNDERSTORM, 8 = CLEARING, 9 = NEUTRAL, 10 = SNOW, 11 =  BLIZZARD, 12 = SNOWLIGHT, 13 = XMAS, 14 = HALLOWEEN");
					return;
				}
				else
				{
					TimeWeather.Meteo.currentWeather = Convert.ToInt32(args[0]);
					Log.Printa(LogType.Debug, TimeWeather.Meteo.currentWeather + "");
					TimeWeather.Meteo.weatherTimer = ConfigShared.SharedConfig.Main.Meteo.ss_weather_timer * 60;
					BaseScript.TriggerEvent("changeWeather", false);
				}
			}
			else
			{
				if (args.Count < 1 || Convert.ToInt32(args[0]) > 14 || !(args[0] as string).All(o => char.IsDigit(o)))
					sender.TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO weather] = ", "Errore weather, argomenti disponibili: ~n~0 = EXTRASUNNY, 1 = CLEAR, 2 = CLOUDS, 3 = SMOG, 4 = FOGGY, 5 = OVERCAST 6 = RAIN, 7 = THUNDERSTORM, 8 = CLEARING, ~n~9 = NEUTRAL, 10 = SNOW, 11 = BLIZZARD, 12 = SNOWLIGHT, 13 = XMAS, 14 = HALLOWEEN!" }, color = new[] { 255, 0, 0 } });
				else
				{
					TimeWeather.Meteo.currentWeather = Convert.ToInt32(args[0]);
					TimeWeather.Meteo.weatherTimer = ConfigShared.SharedConfig.Main.Meteo.ss_weather_timer * 60;
					BaseScript.TriggerEvent("changeWeather", false);
					string meteo = "";
					int a = Convert.ToInt32(args[0]);
					switch (a)
					{
						case 0:
							meteo = "Super Soleggiato";
							break;
						case 1:
							meteo = "Cielo Sgombro";
							break;
						case 2:
							meteo = "Nuvoloso";
							break;
						case 3:
							meteo = "Smog";
							break;
						case 4:
							meteo = "Nebbioso";
							break;
						case 5:
							meteo = "Nuvoloso";
							break;
						case 6:
							meteo = "Piovoso";
							break;
						case 7:
							meteo = "Tempestoso";
							break;
						case 8:
							meteo = "Sereno";
							break;
						case 9:
							meteo = "Neutrale";
							break;
						case 10:
							meteo = "Nevoso";
							break;
						case 11:
							meteo = "Bufera di neve";
							break;
						case 12:
							meteo = "Nevoso con Nebbia";
							break;
						case 13:
							meteo = "Natalizio";
							break;
						case 14:
							meteo = "Halloween";
							break;
						default:
							meteo = "Sconosciuto?";
							break;
					}
					sender.TriggerEvent("lprp:ShowNotification", "Meteo modificato in ~b~" + meteo + "~w~");
				}
			}
		}

		private static void DaiLicenza(Player sender, List<string> args, string rawCommand)
		{
			if (sender.Handle == "0")
				Log.Printa(LogType.Error, $"Comando permesso solo in game");
			else
			{
				if (!string.IsNullOrEmpty(args[0] as string))
				{
					if (!string.IsNullOrEmpty(args[1] as string))
					{
						User pers = Funzioni.GetUserFromPlayerId(args[0]);
						pers.giveLicense(args[1], GetPlayerName("" + sender));
					}
					else
						sender.TriggerEvent("lprp:ShowNotification", "Nessuna licenza specificata!!");
				}
				else
					sender.TriggerEvent("lprp:ShowNotification", "Nessun id specificato!!");
			}

		}

		private static void RimuoviLicenza(Player sender, List<string> args, string rawCommand)
		{
			if (sender.Handle == "0")
				Log.Printa(LogType.Error, $"Comando permesso solo in game");
			else
			{
				if (!string.IsNullOrEmpty(args[0] as string))
				{
					if (!string.IsNullOrEmpty(args[1] as string))
					{
						User pers = Funzioni.GetUserFromPlayerId(args[0]);
						pers.removeLicense(args[1]);
					}
					else
						sender.TriggerEvent("lprp:ShowNotification", "Nessuna licenza specificata!!");
				}
				else
					sender.TriggerEvent("lprp:ShowNotification", "Nessun id specificato!!");

			}
		}
	}
}
