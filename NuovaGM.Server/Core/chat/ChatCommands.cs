﻿using CitizenFX.Core;
using Logger;
using Newtonsoft.Json;
using TheLastPlanet.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Server.Core
{
	static class ChatEvents
	{
		public static void Init()
		{
			Server.Instance.AddCommand("ooc", new Action<int, List<dynamic>, string>(Ooc), false);
			Server.Instance.AddCommand("pol", new Action<int, List<dynamic>, string>(Pol), false);
			Server.Instance.AddCommand("pil", new Action<int, List<dynamic>, string>(Pil), false);
			Server.Instance.AddCommand("med", new Action<int, List<dynamic>, string>(Med), false);
			Server.Instance.AddCommand("mec", new Action<int, List<dynamic>, string>(Mec), false);
			Server.Instance.AddCommand("me", new Action<int, List<dynamic>, string>(Me), false);
			Server.Instance.AddCommand("do", new Action<int, List<dynamic>, string>(Do), false);
			Server.Instance.AddCommand("giveitem", new Action<int, List<dynamic>, string>(GiveItem), false);
			Server.Instance.AddCommand("removeitem", new Action<int, List<dynamic>, string>(RemoveItem), false);
			Server.Instance.AddCommand("giveweapon", new Action<int, List<dynamic>, string>(GiveWeapon), false);
			Server.Instance.AddCommand("removeweapon", new Action<int, List<dynamic>, string>(RemoveWeapon), false);
			Server.Instance.AddCommand("givemoney", new Action<int, List<dynamic>, string>(GiveMoney), false);
			Server.Instance.AddCommand("givebank", new Action<int, List<dynamic>, string>(GiveBank), false);
			Server.Instance.AddCommand("givedirty", new Action<int, List<dynamic>, string>(GiveDirty), false);
			Server.Instance.AddCommand("removemoney", new Action<int, List<dynamic>, string>(RemoveMoney), false);
			Server.Instance.AddCommand("removebank", new Action<int, List<dynamic>, string>(RemoveBank), false);
			Server.Instance.AddCommand("removedirty", new Action<int, List<dynamic>, string>(RemoveDirty), false);
			Server.Instance.AddCommand("setmoney", new Action<int, List<dynamic>, string>(SetFinances), false);
			Server.Instance.AddCommand("annuncio", new Action<int, List<dynamic>, string>(Annuncio), false);
			Server.Instance.AddCommand("revive", new Action<int, List<dynamic>, string>(Revive), false);
			Server.Instance.AddCommand("setgroup", new Action<int, List<dynamic>, string>(SetGroup), false);
			Server.Instance.AddCommand("tp", new Action<int, List<dynamic>, string>(Teleport), false);
			Server.Instance.AddCommand("suicidati", new Action<int, List<dynamic>, string>(Muori), false);
			Server.Instance.AddCommand("car", new Action<int, List<dynamic>, string>(SpawnVehicle), false);
			Server.Instance.AddCommand("dv", new Action<int, List<dynamic>, string>(Dv), false);
			Server.Instance.AddCommand("salvatutti", new Action<int, List<dynamic>, string>(Salvatutti), false);
			Server.Instance.AddCommand("sviluppatore", new Action<int, List<dynamic>, string>(Sviluppatore), false);
			Server.Instance.AddCommand("setjob", new Action<int, List<dynamic>, string>(SetJob), false);
			Server.Instance.AddCommand("setgang", new Action<int, List<dynamic>, string>(SetGang), false);
			Server.Instance.AddCommand("cambiaora", new Action<int, List<dynamic>, string>(Time), false);
			Server.Instance.AddCommand("bloccatempo", new Action<int, List<dynamic>, string>(FreezeTime), false);
			Server.Instance.AddCommand("setmeteo", new Action<int, List<dynamic>, string>(Weather), false);
			Server.Instance.AddCommand("dailicenza", new Action<int, List<dynamic>, string>(DaiLicenza), false);
			Server.Instance.AddCommand("rimuovilicenza", new Action<int, List<dynamic>, string>(RimuoviLicenza), false);


			//			Server.Instance.AddCommand("nome comando", new Action<int, List<dynamic>, string>(funzione comando), false);

		}

		// GESTIONE CHAT
		public static void Ooc(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatServer.commands["ooc"]))
					BaseScript.TriggerClientEvent("chat:addMessage", new { color = new[] { 0, 255, 153 }, multiline = true, args = new[] { "[FUORI RP] | " + GetPlayerName(sender.ToString()), rawCommand.Substring(4) } });
				ChatServer.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
			else
				Log.Printa(LogType.Error, "Questo comando funziona SOLO in gioco");
		}

		public static void Pol(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatServer.commands["pol"]))
				{
					var user = Funzioni.GetUserFromPlayerId(sender.ToString());
					if (user.CurrentChar.job.name.ToLower() == "polizia")
						Server.PlayerList.Values.Where(x => x.CurrentChar.job.name.ToLower() == "polizia").ToList().ForEach(x => x.p.TriggerEvent("chat:addMessage", new { color = new[] { 244, 65, 125 }, multiline = true, args = new[] { "[POLIZIA] | " + user.FullName, rawCommand.Substring(4) } }));
					else user.showNotification("Non puoi usare questo comando!");
				}
				ChatServer.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
			else
				Log.Printa(LogType.Error, "Questo comando funziona SOLO in gioco");
		}

		public static void Pil(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatServer.commands["pil"]))
				{
					var user = Funzioni.GetUserFromPlayerId(sender.ToString());
					if (user.CurrentChar.job.name.ToLower() == "pilota")
						Server.PlayerList.Values.Where(x => x.CurrentChar.job.name.ToLower() == "pilota").ToList().ForEach(x => x.p.TriggerEvent("chat:addMessage", new { color = new[] { 244, 223, 66 }, multiline = true, args = new[] { "[PILOTI] | " + user.FullName, rawCommand.Substring(4) } }));
					else user.showNotification("Non puoi usare questo comando!");
				}
				ChatServer.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
			else
				Log.Printa(LogType.Error, "Questo comando funziona SOLO in gioco");
		}

		public static void Med(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatServer.commands["med"]))
				{
					var user = Funzioni.GetUserFromPlayerId(sender.ToString());
					if (user.CurrentChar.job.name.ToLower() == "medico")
						Server.PlayerList.Values.Where(x => x.CurrentChar.job.name.ToLower() == "medico").ToList().ForEach(x => x.p.TriggerEvent("chat:addMessage", new { color = new[] { 88, 154, 202 }, multiline = true, args = new[] { "[MEDICI] | " + user.FullName, rawCommand.Substring(4) } }));
					else user.showNotification("Non puoi usare questo comando!");
				}
				ChatServer.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
			else
				Log.Printa(LogType.Error, "Questo comando funziona SOLO in gioco");
		}

		public static void Mec(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatServer.commands["mec"]))
				{
					var user = Funzioni.GetUserFromPlayerId(sender.ToString());
					if (user.CurrentChar.job.name.ToLower() == "meccanico")
						Server.PlayerList.Values.Where(x => x.CurrentChar.job.name.ToLower() == "meccanico").ToList().ForEach(x => x.p.TriggerEvent("chat:addMessage", new { color = new[] { 102, 102, 255 }, multiline = true, args = new[] { "[MECCANICI] | " + user.FullName, rawCommand.Substring(4) } }));
					else user.showNotification("Non puoi usare questo comando!");
				}
				ChatServer.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
			else
				Log.Printa(LogType.Error, "Questo comando funziona SOLO in gioco");
		}

		public static void Me(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatServer.commands["me"]))
					BaseScript.TriggerClientEvent("lprp:triggerProximityDisplay", sender, "[ME]: ", rawCommand.Substring(3), 0, 255, 153);
				ChatServer.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
			else
				Log.Printa(LogType.Error, "Questo comando funziona SOLO in gioco");
		}

		public static void Do(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatServer.commands["do"]))
					BaseScript.TriggerClientEvent("lprp:triggerProximityDisplay", sender, "[DO]: ", rawCommand.Substring(3), 0, 255, 153);
				ChatServer.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
			else
				Log.Printa(LogType.Error, "Questo comando funziona SOLO in gioco");
		}

		// FINE CHAT
		// GESTIONE INVENTARIO
		public static void GiveItem(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				try
				{
					if (Funzioni.IsPlayerAndHasPermission(sender, ChatServer.commands["giveitem"]))
					{
						if (Server.PlayerList.ContainsKey(args[0]))
						{
							User player = Funzioni.GetUserFromPlayerId(args[0]);
							string item = "" + args[1];
							player.addInventoryItem(item, Convert.ToInt32(args[2]), Shared.ConfigShared.SharedConfig.Main.Generici.ItemList[item].peso);
						}
						else
							Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO giveitem] = ", "Il player con ID \"" + args[0] + "\" non è online!" }, color = new[] { 255, 0, 0 } });
					}
				}
				catch
				{
					Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO giveitem] = ", "Errore nei parametri!" }, color = new[] { 255, 0, 0 } });
				}
				ChatServer.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
		}

		public static void RemoveItem(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				try
				{
					if (Funzioni.IsPlayerAndHasPermission(sender, ChatServer.commands["removeitem"]))
					{
						if (Server.PlayerList.ContainsKey(args[0]))
						{
							User player = Funzioni.GetUserFromPlayerId(args[0]);
							player.removeInventoryItem(args[1], Convert.ToInt32(args[2]));
						}
						else
							Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO removeitem] = ", "Il player con ID" + args[0] + " non è online!" }, color = new[] { 255, 0, 0 } });
					}
				}
				catch
				{
					Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO removeitem] = ", "Errore nei parametri!" }, color = new[] { 255, 0, 0 } });
				}
				ChatServer.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
		}

		public static void GiveWeapon(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				try
				{
					if (Funzioni.IsPlayerAndHasPermission(sender, ChatServer.commands["giveweapon"]))
					{
						if (Server.PlayerList.ContainsKey(args[0]))
						{
							User player = Funzioni.GetUserFromPlayerId(args[0]);
							player.addWeapon(args[1].ToUpper(), Convert.ToInt32(args[2]));
						}
						else
							Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO giveweapon] = ", "Il player con ID" + args[0] + " non è online!" }, color = new[] { 255, 0, 0 } });
					}
				}
				catch
				{
					Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO giveweapon] = ", "Errore nei parametri!" }, color = new[] { 255, 0, 0 } });
				}
				ChatServer.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
		}

		public static void RemoveWeapon(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				try
				{
					if (Funzioni.IsPlayerAndHasPermission(sender, ChatServer.commands["removeweapon"]))
					{
						if (Server.PlayerList.ContainsKey(args[0]))
						{
							User player = Funzioni.GetUserFromPlayerId(args[0]);
							player.removeWeapon(args[1].ToUpper());
						}
						else
						{
							Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO removeweapon] = ", "Il player con ID" + args[0] + " non è online!" }, color = new[] { 255, 0, 0 } });
						}
					}
				}
				catch
				{
					Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO removeweapon] = ", "Errore nei parametri!" }, color = new[] { 255, 0, 0 } });
				}
				ChatServer.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
		}
		// FINE GESTIONE INVENTARIO
		// GESTIONE DELLE FINANZE
		public static void GiveMoney(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatServer.commands["givemoney"]))
				{
					if (Server.PlayerList.ContainsKey(args[0]))
						Eventi.GiveMoney(Server.Instance.GetPlayers[Convert.ToInt32(args[0])], Convert.ToInt32(args[1]));
					else
						Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO givemoney] = ", "Il player con ID" + args[0] + " non è online!" }, color = new[] { 255, 0, 0 } });
				}
				ChatServer.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
		}
		
		public static void GiveBank(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatServer.commands["givebank"]))
				{
					if (Server.PlayerList.ContainsKey(args[0]))
						Eventi.GiveBank(Server.Instance.GetPlayers[Convert.ToInt32(args[0])], Convert.ToInt32(args[1]));
					else
						Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO givebank] = ", "Il player con ID" + args[0] + " non è online!" }, color = new[] { 255, 0, 0 } });
				}
				ChatServer.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
		}

		public static void GiveDirty(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatServer.commands["givedirty"]))
				{
					if (Server.PlayerList.ContainsKey(args[0]))
						Eventi.GiveDirty(Server.Instance.GetPlayers[Convert.ToInt32(args[0])], Convert.ToInt32(args[1]));
					else
						Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO givedirty] = ", "Il player con ID" + args[0] + " non è online!" }, color = new[] { 255, 0, 0 } });
				}
				ChatServer.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
		}
		
		public static void RemoveMoney(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatServer.commands["removemoney"]))
				{
					if (Server.PlayerList.ContainsKey(args[0]))
						Eventi.RemoveMoney(Server.Instance.GetPlayers[Convert.ToInt32(args[0])], Convert.ToInt32(args[1]));
					else
						Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO givedirty] = ", "Il player con ID" + args[0] + " non è online!" }, color = new[] { 255, 0, 0 } });
				}
				ChatServer.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
		}

		public static void RemoveBank(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatServer.commands["removebank"]))
				{
					if (Server.PlayerList.ContainsKey(args[0]))
						Eventi.RemoveBank(Server.Instance.GetPlayers[Convert.ToInt32(args[0])], Convert.ToInt32(args[1]));
					else
						Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO givedirty] = ", "Il player con ID" + args[0] + " non è online!" }, color = new[] { 255, 0, 0 } });
				}
				ChatServer.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
		}

		public static void RemoveDirty(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatServer.commands["removedirty"]))
				{
					if (Server.PlayerList.ContainsKey(args[0]))
						Eventi.RemoveDirty(Server.Instance.GetPlayers[Convert.ToInt32(args[0])], Convert.ToInt32(args[1]));
					else
						Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO givedirty] = ", "Il player con ID" + args[0] + " non è online!" }, color = new[] { 255, 0, 0 } });
				}
				ChatServer.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
		}

		public static void SetFinances(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatServer.commands["setmoney"]))
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
							Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO setmoney] = ", "L'account monetario '" + args[1] + "' non esiste!" }, color = new[] { 255, 0, 0 } });
					}
					else
						Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO setmoney] = ", "Il player con ID" + args[0] + " non è online!" }, color = new[] { 255, 0, 0 } });
				}
				ChatServer.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
		}

		// FINE GESTIONE FINANZE
		// ANNUNCIO PLAYERS
		public static void Annuncio(int sender, List<dynamic> args, string rawCommand)
		{
			if (Funzioni.IsPlayerAndHasPermission(sender, ChatServer.commands["annuncio"]) || sender == 0)
				BaseScript.TriggerClientEvent("lprp:announce", rawCommand.Replace("annuncio", string.Empty));
			if (sender != 0)
				ChatServer.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
		}
		// FINE ANNUNCIO

		// REVIVE
		public static void Revive(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatServer.commands["revive"]))
				{
					var now = DateTime.Now;
					if (args != null && args.Count > 0)
					{
						if (GetPlayerName(args[0]) != ".")
						{
							Player p = Server.Instance.GetPlayers[Convert.ToInt32(args[0])];
							Log.Printa(LogType.Info, "Comandi: " + GetPlayerName(sender.ToString()) + " ha usato il comando revive su " + GetPlayerName(args[0]));
							BaseScript.TriggerEvent("lprp:serverlog", now.ToString("dd/MM/yyyy, HH:mm:ss") + " -- Comandi: " + GetPlayerName(sender.ToString()) + " ha usato il comando revive su " + GetPlayerName(args[0]));
							BaseScript.TriggerClientEvent(p, "lprp:reviveChar");
						}
					}
					else
						Funzioni.GetPlayerFromId(sender).TriggerEvent("lprp:reviveChar");
				}
				ChatServer.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
		}
		// FINE REVIVE

		// SETGROUP
		public static async void SetGroup(int sender, List<dynamic> args, string rawCommand)
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
					if (sender != 0)
					{
						Log.Printa(LogType.Info, "Comandi: " + GetPlayerName(sender.ToString()) + " ha usato il comando setgroup su " + ricevitore.Name);
						BaseScript.TriggerEvent("lprp:serverlog", now.ToString("dd/MM/yyyy, HH:mm:ss") + " -- Comandi: " + GetPlayerName(sender.ToString()) + " ha usato il comando setgroup su " + ricevitore.Name);
					}
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
				{
					if (sender != 0)
						Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO setgroup] = ", "Il player con ID" + args[0] + " non è online!" }, color = new[] { 255, 0, 0 } });
					else
						Log.Printa(LogType.Error, "Il player con ID" + args[0] + " non è online!");
				}
			}
			else
			{
				if (sender != 0)
					Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO setgroup] = ", "errore nel comando setgroup.. riprova" }, color = new[] { 255, 0, 0 } });
				else
					Log.Printa(LogType.Error, "errore nel comando setgroup..riprova");
			}
			if (sender != 0)
				ChatServer.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
		}
		// FINE SETGROUP

		public static void Teleport(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatServer.commands["tp"]))
				{
					float x = 0;
					float y = 0;
					float z = 0;
					if (float.TryParse(args[0], out x) && float.TryParse(args[1], out y) && float.TryParse(args[2], out z))
					{
						Funzioni.GetPlayerFromId(sender).TriggerEvent("lprp:teleportCoords", args[0], args[1], args[2]);
					}
					else
						Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO tp] = ", "Errore coordinate non valide, riprova!" }, color = new[] { 255, 0, 0 } });
				}
				ChatServer.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
		}

		public static void Muori(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatServer.commands["suicidati"]))
					Funzioni.GetPlayerFromId(sender).TriggerEvent("lprp:death");
				ChatServer.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
		}

		public static void SpawnVehicle(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatServer.commands["car"]))
					Funzioni.GetPlayerFromId(sender).TriggerEvent("lprp:spawnVehicle", args[0]);
				ChatServer.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
		}

		public static void Dv(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatServer.commands["dv"]))
					Funzioni.GetPlayerFromId(sender).TriggerEvent("lprp:deleteVehicle");
				ChatServer.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
		}

		public static void Delgun(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatServer.commands["delgun"]))
					Funzioni.GetPlayerFromId(sender).TriggerEvent("lprp:ObjectDeleteGun", args[0]);
				ChatServer.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
		}

		private static async void Salvatutti(int sender, List<dynamic> args, string rawCommand)
		{
			await BaseScript.Delay(0);
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
			if(sender!=0)
				ChatServer.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
		}

		public static void Sviluppatore(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatServer.commands["sviluppatore"]))
				{
					Player send = Funzioni.GetPlayerFromId(sender);
					if (args[0] == "on")
					{
						send.TriggerEvent("lprp:sviluppatoreOn", true);
					}
					else if (args[0] == "off")
					{
						send.TriggerEvent("lprp:sviluppatoreOn", false);
					}
					else
					{
						Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO sviluppatore] = ", "Errore argomento non valido, riprova!" }, color = new[] { 255, 0, 0 } });
					}
				}
				ChatServer.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
		}

		public static void SetJob(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatServer.commands["setjob"]))
				{
					Player p = Funzioni.GetPlayerFromId(args[0]);
					if (Server.PlayerList.ContainsKey(p.Handle))
					{
						User pers = Funzioni.GetUserFromPlayerId(p.Handle);
						if (pers.status.spawned)
							pers.SetJob(args[1], Convert.ToInt32(args[2]));
						else
							Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO setjob] = ", "Errore il player non ha selezionato un personaggio, riprova!" }, color = new[] { 255, 0, 0 } });
					}
					else
						Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO setjob] = ", "Errore id player non trovato, riprova!" }, color = new[] { 255, 0, 0 } });
				}
				ChatServer.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
		}

		public static void SetGang(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatServer.commands["setgang"]))
				{
					Player p = Funzioni.GetPlayerFromId(args[0]);
					if (Server.PlayerList.ContainsKey(p.Handle))
					{
						User pers = Funzioni.GetUserFromPlayerId(p.Handle);
						if (pers.status.spawned)
							pers.SetGang(args[1], Convert.ToInt32(args[2]));
						else
							Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO setgang] = ", "Errore il player non ha selezionato un personaggio, riprova!" }, color = new[] { 255, 0, 0 } });
					}
					else
						Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO setgang] = ", "Errore id player non trovato, riprova!" }, color = new[] { 255, 0, 0 } });
				}
				ChatServer.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
		}

		public static void Time(int sender, List<dynamic> args, string rawCommand)
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

			if (sender == 0)
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
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatServer.commands["cambiaora"]))
				{
					if (args != null && args.Count > 0 && args.Count < 3)
					{
						BaseScript.TriggerEvent("UpdateFromCommandTime", Tempo);
						Funzioni.GetPlayerFromId(sender).TriggerEvent("lprp:ShowNotification", $"Orario server impostato alle ~y~{h}:{m}~w~");
					}
					else
						Funzioni.GetPlayerFromId(sender).TriggerEvent("lprp:ShowNotification", $"ERRORE! Devi impostare almeno l'orario!");
				}
			}
		}

		public static void FreezeTime(int sender, List<dynamic> args, string rawCommand)
		{
			int h = (int)Math.Floor(TimeWeather.Orario.secondOfDay / 3600f);
			int m = (int)Math.Floor((TimeWeather.Orario.secondOfDay - (h * 3600)) / 60f);
			bool freeze = false;
			if (args[0] == "true" || args[0] == "1")
				freeze = true;
			if (sender == 0)
			{
				BaseScript.TriggerEvent("freezeTime", freeze);
				if (freeze)
					Log.Printa(LogType.Info, $"Orario di gioco bloccato alle ore {h}:{m}");
				else
					Log.Printa(LogType.Info, $"Orario di gioco sbloccato dalle ore {h}:{m}");
			}
			else
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatServer.commands["bloccatempo"]))
				{
					BaseScript.TriggerEvent("freezeTime", freeze);
					if (freeze)
						Funzioni.GetPlayerFromId(sender).TriggerEvent("lprp:ShowNotification", $"Orario di gioco bloccato alle ore ~b~{h}:{m}~w~");
					else
						Funzioni.GetPlayerFromId(sender).TriggerEvent("lprp:ShowNotification", $"Orario di gioco sbloccato dlle ore ~b~{h}:{m}~w~");
				}
			}
		}

		public static void Weather(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender == 0)
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
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatServer.commands["setmeteo"]))
				{
					if (args.Count < 1 || Convert.ToInt32(args[0]) > 14 || !(args[0] as string).All(o => char.IsDigit(o)))
						Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO weather] = ", "Errore weather, argomenti disponibili: ~n~0 = EXTRASUNNY, 1 = CLEAR, 2 = CLOUDS, 3 = SMOG, 4 = FOGGY, 5 = OVERCAST 6 = RAIN, 7 = THUNDERSTORM, 8 = CLEARING, ~n~9 = NEUTRAL, 10 = SNOW, 11 = BLIZZARD, 12 = SNOWLIGHT, 13 = XMAS, 14 = HALLOWEEN!" }, color = new[] { 255, 0, 0 } });
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
						Funzioni.GetPlayerFromId(sender).TriggerEvent("lprp:ShowNotification", "Meteo modificato in ~b~" + meteo + "~w~");
					}
				}
			}
		}

		private static void DaiLicenza(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender == 0)
				Log.Printa(LogType.Error, $"Comando permesso solo in game");
			else
			{
				if (!string.IsNullOrEmpty(args[0] as string))
				{
					if (!string.IsNullOrEmpty(args[1] as string))
					{
						User pers = Funzioni.GetUserFromPlayerId(args[0]);
						pers.giveLicense(args[1], GetPlayerName(""+sender));
					}
					else
						Funzioni.GetPlayerFromId(sender).TriggerEvent("lprp:ShowNotification", "Nessuna licenza specificata!!");
				}
				else
					Funzioni.GetPlayerFromId(sender).TriggerEvent("lprp:ShowNotification", "Nessun id specificato!!");
			}

		}

		private static void RimuoviLicenza(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender == 0)
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
						Funzioni.GetPlayerFromId(sender).TriggerEvent("lprp:ShowNotification", "Nessuna licenza specificata!!");
				}
				else
					Funzioni.GetPlayerFromId(sender).TriggerEvent("lprp:ShowNotification", "Nessun id specificato!!");

			}
		}
	}
}
