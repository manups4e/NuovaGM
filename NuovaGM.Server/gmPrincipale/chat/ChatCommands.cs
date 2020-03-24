using CitizenFX.Core;
using Newtonsoft.Json;
using NuovaGM.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace NuovaGM.Server.gmPrincipale
{
	static class ChatEvents
	{
		public static void Init()
		{
			Server.GetInstance.AddCommand("ooc", new Action<int, List<dynamic>, string>(Ooc), false);
			Server.GetInstance.AddCommand("pol", new Action<int, List<dynamic>, string>(Pol), false);
			Server.GetInstance.AddCommand("pil", new Action<int, List<dynamic>, string>(Pil), false);
			Server.GetInstance.AddCommand("med", new Action<int, List<dynamic>, string>(Med), false);
			Server.GetInstance.AddCommand("mec", new Action<int, List<dynamic>, string>(Mec), false);
			Server.GetInstance.AddCommand("me", new Action<int, List<dynamic>, string>(Me), false);
			Server.GetInstance.AddCommand("do", new Action<int, List<dynamic>, string>(Do), false);
			Server.GetInstance.AddCommand("giveitem", new Action<int, List<dynamic>, string>(GiveItem), false);
			Server.GetInstance.AddCommand("removeitem", new Action<int, List<dynamic>, string>(RemoveItem), false);
			Server.GetInstance.AddCommand("giveweapon", new Action<int, List<dynamic>, string>(GiveWeapon), false);
			Server.GetInstance.AddCommand("removeweapon", new Action<int, List<dynamic>, string>(RemoveWeapon), false);
			Server.GetInstance.AddCommand("givemoney", new Action<int, List<dynamic>, string>(GiveMoney), false);
			Server.GetInstance.AddCommand("givebank", new Action<int, List<dynamic>, string>(GiveBank), false);
			Server.GetInstance.AddCommand("givedirty", new Action<int, List<dynamic>, string>(GiveDirty), false);
			Server.GetInstance.AddCommand("removemoney", new Action<int, List<dynamic>, string>(RemoveMoney), false);
			Server.GetInstance.AddCommand("removebank", new Action<int, List<dynamic>, string>(RemoveBank), false);
			Server.GetInstance.AddCommand("removedirty", new Action<int, List<dynamic>, string>(RemoveDirty), false);
			Server.GetInstance.AddCommand("setmoney", new Action<int, List<dynamic>, string>(SetFinances), false);
			Server.GetInstance.AddCommand("annuncio", new Action<int, List<dynamic>, string>(Annuncio), false);
			Server.GetInstance.AddCommand("revive", new Action<int, List<dynamic>, string>(Revive), false);
			Server.GetInstance.AddCommand("setgroup", new Action<int, List<dynamic>, string>(SetGroup), false);
			Server.GetInstance.AddCommand("tp", new Action<int, List<dynamic>, string>(Teleport), false);
			Server.GetInstance.AddCommand("suicidati", new Action<int, List<dynamic>, string>(Muori), false);
			Server.GetInstance.AddCommand("car", new Action<int, List<dynamic>, string>(SpawnVehicle), false);
			Server.GetInstance.AddCommand("dv", new Action<int, List<dynamic>, string>(Dv), false);
			Server.GetInstance.AddCommand("salvatutti", new Action<int, List<dynamic>, string>(Salvatutti), false);
			Server.GetInstance.AddCommand("sviluppatore", new Action<int, List<dynamic>, string>(Sviluppatore), false);
			Server.GetInstance.AddCommand("setjob", new Action<int, List<dynamic>, string>(SetJob), false);
			Server.GetInstance.AddCommand("setgang", new Action<int, List<dynamic>, string>(SetGang), false);
			Server.GetInstance.AddCommand("cambiaora", new Action<int, List<dynamic>, string>(Time), false);
			Server.GetInstance.AddCommand("bloccatempo", new Action<int, List<dynamic>, string>(FreezeTime), false);
			Server.GetInstance.AddCommand("setmeteo", new Action<int, List<dynamic>, string>(Weather), false);
			Server.GetInstance.AddCommand("dailicenza", new Action<int, List<dynamic>, string>(DaiLicenza), false);
			Server.GetInstance.AddCommand("rimuovilicenza", new Action<int, List<dynamic>, string>(RimuoviLicenza), false);


			//			Server.GetInstance.AddCommand("nome comando", new Action<int, List<dynamic>, string>(funzione comando), false);

		}

		// GESTIONE CHAT
		public static void Ooc(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatMain.commands["ooc"]))
				{
					string msg = "";
					for (int i = 0; i < args.Count; i++)
					{
						msg = msg + " " + args[i];
					}
					BaseScript.TriggerClientEvent("chat:addMessage", new { color = new[] { 0, 255, 153 }, multiline = true, args = new[] { "[FUORI RP] | " + GetPlayerName(sender.ToString()), msg } });
				}
				ChatMain.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
			else
				Log.Printa(LogType.Error, "Questo comando funziona SOLO in gioco");
		}

		public static void Pol(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatMain.commands["pol"]))
				{
					if (ServerEntrance.PlayerList["" + sender].CurrentChar.job.name.ToLower() == "polizia")
					{
						string msg = "";
						for (int i = 0; i < args.Count; i++)
							msg = msg + " " + args[i];
						BaseScript.TriggerClientEvent("chat:addMessage", new { color = new[] { 244, 65, 125 }, multiline = true, args = new[] { "[POLIZIA] | " + ServerEntrance.PlayerList[sender.ToString()].FullName, msg } });
					}
				}
				ChatMain.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
			else
				Log.Printa(LogType.Error, "Questo comando funziona SOLO in gioco");
		}

		public static void Pil(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatMain.commands["pil"]))
				{
					if (ServerEntrance.PlayerList["" + sender].CurrentChar.job.name.ToLower() == "pilota")
					{
						string msg = "";
						for (int i = 0; i < args.Count; i++)
							msg = msg + " " + args[i];
						BaseScript.TriggerClientEvent("chat:addMessage", new { color = new[] { 244, 223, 66 }, multiline = true, args = new[] { "[PILOTI] | " + ServerEntrance.PlayerList[sender.ToString()].FullName, msg } });
					}
				}
				ChatMain.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
			else
				Log.Printa(LogType.Error, "Questo comando funziona SOLO in gioco");
		}

		public static void Med(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatMain.commands["med"]))
				{
					if (ServerEntrance.PlayerList["" + sender].CurrentChar.job.name.ToLower() == "medico")
					{
						string msg = "";
						for (int i = 0; i < args.Count; i++)
							msg = msg + " " + args[i];
						BaseScript.TriggerClientEvent("chat:addMessage", new { color = new[] { 88, 154, 202 }, multiline = true, args = new[] { "[MEDICI] | " + ServerEntrance.PlayerList[sender.ToString()].FullName, msg } });
					}
				}
				ChatMain.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
			else
				Log.Printa(LogType.Error, "Questo comando funziona SOLO in gioco");
		}

		public static void Mec(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatMain.commands["mec"]))
				{
					if (ServerEntrance.PlayerList["" + sender].CurrentChar.job.name.ToLower() == "meccanico")
					{
						string msg = "";
						for (int i = 0; i < args.Count; i++)
							msg = msg + " " + args[i];
						BaseScript.TriggerClientEvent("chat:addMessage", new { color = new[] { 102, 102, 255 }, multiline = true, args = new[] { "[MECCANICI] | " + ServerEntrance.PlayerList[sender.ToString()].FullName, msg } });
					}
				}
				ChatMain.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
			else
				Log.Printa(LogType.Error, "Questo comando funziona SOLO in gioco");
		}

		public static void Me(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatMain.commands["me"]))
				{
					string msg = "";
					for (int i = 0; i < args.Count; i++)
						msg = msg + " " + args[i];
					BaseScript.TriggerClientEvent("lprp:triggerProximityDisplay", sender, "[ME]: ", msg, 0, 255, 153);
				}
				ChatMain.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
			else
				Log.Printa(LogType.Error, "Questo comando funziona SOLO in gioco");
		}

		public static void Do(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatMain.commands["do"]))
				{
					string msg = "";
					for (int i = 0; i < args.Count; i++)
						msg = msg + " " + args[i];
					BaseScript.TriggerClientEvent("lprp:triggerProximityDisplay", sender, "[DO]: ", msg, 0, 255, 153);
				}
				ChatMain.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
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
					if (Funzioni.IsPlayerAndHasPermission(sender, ChatMain.commands["giveitem"]))
					{
						if (ServerEntrance.PlayerList.ContainsKey(args[0]))
						{
							User player = ServerEntrance.PlayerList[args[0]];
							string item = "" + args[1];
							player.addInventoryItem(item, Convert.ToInt32(args[2]), Shared.SharedScript.ItemList[item].peso);
						}
						else
							Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO giveitem] = ", "Il player con ID \"" + args[0] + "\" non è online!" }, color = new[] { 255, 0, 0 } });
					}
				}
				catch
				{
					Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO giveitem] = ", "Errore nei parametri!" }, color = new[] { 255, 0, 0 } });

				}
				ChatMain.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
		}

		public static void RemoveItem(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				try
				{
					if (Funzioni.IsPlayerAndHasPermission(sender, ChatMain.commands["removeitem"]))
					{
						if (ServerEntrance.PlayerList.ContainsKey(args[0]))
						{
							User player = ServerEntrance.PlayerList[args[0]];
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
				ChatMain.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
		}

		public static void GiveWeapon(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				try
				{
					if (Funzioni.IsPlayerAndHasPermission(sender, ChatMain.commands["giveweapon"]))
					{
						if (ServerEntrance.PlayerList.ContainsKey(args[0]))
						{
							User player = ServerEntrance.PlayerList[args[0]];
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
				ChatMain.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
		}

		public static void RemoveWeapon(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				try
				{
					if (Funzioni.IsPlayerAndHasPermission(sender, ChatMain.commands["removeweapon"]))
					{
						if (ServerEntrance.PlayerList.ContainsKey(args[0]))
						{
							User player = ServerEntrance.PlayerList[args[0]];
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
				ChatMain.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
		}
		// FINE GESTIONE INVENTARIO
		// GESTIONE DELLE FINANZE
		public static void GiveMoney(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatMain.commands["givemoney"]))
				{
					if (ServerEntrance.PlayerList.ContainsKey(args[0]))
						Eventi.GiveMoney(Server.GetInstance.GetPlayers[Convert.ToInt32(args[0])], Convert.ToInt32(args[1]));
					else
						Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO givemoney] = ", "Il player con ID" + args[0] + " non è online!" }, color = new[] { 255, 0, 0 } });
				}
				ChatMain.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
		}
		
		public static void GiveBank(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatMain.commands["givebank"]))
				{
					if (ServerEntrance.PlayerList.ContainsKey(args[0]))
						Eventi.GiveBank(Server.GetInstance.GetPlayers[Convert.ToInt32(args[0])], Convert.ToInt32(args[1]));
					else
						Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO givebank] = ", "Il player con ID" + args[0] + " non è online!" }, color = new[] { 255, 0, 0 } });
				}
				ChatMain.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
		}

		public static void GiveDirty(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatMain.commands["givedirty"]))
				{
					if (ServerEntrance.PlayerList.ContainsKey(args[0]))
						Eventi.GiveDirty(Server.GetInstance.GetPlayers[Convert.ToInt32(args[0])], Convert.ToInt32(args[1]));
					else
						Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO givedirty] = ", "Il player con ID" + args[0] + " non è online!" }, color = new[] { 255, 0, 0 } });
				}
				ChatMain.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
		}
		
		public static void RemoveMoney(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatMain.commands["removemoney"]))
				{
					if (ServerEntrance.PlayerList.ContainsKey(args[0]))
						Eventi.RemoveMoney(Server.GetInstance.GetPlayers[Convert.ToInt32(args[0])], Convert.ToInt32(args[1]));
					else
						Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO givedirty] = ", "Il player con ID" + args[0] + " non è online!" }, color = new[] { 255, 0, 0 } });
				}
				ChatMain.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
		}

		public static void RemoveBank(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatMain.commands["removebank"]))
				{
					if (ServerEntrance.PlayerList.ContainsKey(args[0]))
						Eventi.RemoveBank(Server.GetInstance.GetPlayers[Convert.ToInt32(args[0])], Convert.ToInt32(args[1]));
					else
						Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO givedirty] = ", "Il player con ID" + args[0] + " non è online!" }, color = new[] { 255, 0, 0 } });
				}
				ChatMain.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
		}

		public static void RemoveDirty(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatMain.commands["removedirty"]))
				{
					if (ServerEntrance.PlayerList.ContainsKey(args[0]))
						Eventi.RemoveDirty(Server.GetInstance.GetPlayers[Convert.ToInt32(args[0])], Convert.ToInt32(args[1]));
					else
						Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO givedirty] = ", "Il player con ID" + args[0] + " non è online!" }, color = new[] { 255, 0, 0 } });
				}
				ChatMain.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
		}

		public static void SetFinances(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatMain.commands["setmoney"]))
				{
					if (ServerEntrance.PlayerList.ContainsKey(args[0]))
					{
						var player = ServerEntrance.PlayerList[args[0]];
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
						{
							Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO setmoney] = ", "L'account monetario '" + args[1] + "' non esiste!" }, color = new[] { 255, 0, 0 } });
						}
					}
					else
					{
						Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO setmoney] = ", "Il player con ID" + args[0] + " non è online!" }, color = new[] { 255, 0, 0 } });
					}
				}
				ChatMain.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
		}

		// FINE GESTIONE FINANZE
		// ANNUNCIO PLAYERS
		public static void Annuncio(int sender, List<dynamic> args, string rawCommand)
		{
			if (Funzioni.IsPlayerAndHasPermission(sender, ChatMain.commands["annuncio"]) || sender == 0)
			{
				string msg = "";
				for (int i = 0; i < args.Count; i++)
					msg = msg + " " + args[i];
				BaseScript.TriggerClientEvent("lprp:announce", msg);
			}
			if (sender != 0)
				ChatMain.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
		}
		// FINE ANNUNCIO

		// REVIVE
		public static void Revive(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatMain.commands["revive"]))
				{
					var now = DateTime.Now;
					if (args != null && args.Count > 0)
					{
						if (GetPlayerName(args[0]) != ".")
						{
							Player p = Server.GetInstance.GetPlayers[Convert.ToInt32(args[0])];
							Log.Printa(LogType.Info, "Comandi: " + GetPlayerName(sender.ToString()) + " ha usato il comando revive su " + GetPlayerName(args[0]));
							BaseScript.TriggerEvent("lprp:serverlog", now.ToString("dd/MM/yyyy, HH:mm:ss") + " -- Comandi: " + GetPlayerName(sender.ToString()) + " ha usato il comando revive su " + GetPlayerName(args[0]));
							BaseScript.TriggerClientEvent(p, "lprp:reviveChar");
						}
					}
					else
					{
						Funzioni.GetPlayerFromId(sender).TriggerEvent("lprp:reviveChar");
					}
				}
				ChatMain.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
		}
		// FINE REVIVE

		// SETGROUP
		public static async void SetGroup(int sender, List<dynamic> args, string rawCommand)
		{
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
						group_level = 0;
					}
					else if (args[1] == "helper")
					{
						group = "helper";
						group_level = 1;
					}
					else if (args[1] == "mod")
					{
						group = "moderatore";
						group_level = 2;
					}
					else if (args[1] == "admin")
					{
						group = "admin";
						group_level = 3;
					}
					else if (args[1] == "founder")
					{
						group = "founder";
						group_level = 4;
					}
					else if (args[1] == "dev")
					{
						group = "dev";
						group_level = 5;
					}
					await Server.GetInstance.Execute("UPDATE `users` SET `group` = @gruppo,  `group_level` = @groupL WHERE `discord` = @disc", new
					{
						gruppo = group,
						groupL = group_level,
						disc = License.GetLicense(ricevitore, Identifier.Discord)
					});
					ServerEntrance.PlayerList[ricevitore.Handle].group = group;
					ServerEntrance.PlayerList[ricevitore.Handle].group_level = group_level;
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
				ChatMain.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
		}
		// FINE SETGROUP

		public static void Teleport(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatMain.commands["tp"]))
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
				ChatMain.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
		}

		public static void Muori(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatMain.commands["suicidati"]))
					Funzioni.GetPlayerFromId(sender).TriggerEvent("lprp:death");
				ChatMain.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
		}

		public static void SpawnVehicle(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatMain.commands["car"]))
					Funzioni.GetPlayerFromId(sender).TriggerEvent("lprp:spawnVehicle", args[0]);
				ChatMain.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
		}

		public static void Dv(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatMain.commands["dv"]))
					Funzioni.GetPlayerFromId(sender).TriggerEvent("lprp:deleteVehicle");
				ChatMain.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
		}

		public static void Delgun(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatMain.commands["delgun"]))
					Funzioni.GetPlayerFromId(sender).TriggerEvent("lprp:ObjectDeleteGun", args[0]);
				ChatMain.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
		}

		public static async void Salvatutti(int sender, List<dynamic> args, string rawCommand)
		{
			try
			{
				var now = DateTime.Now;
				foreach (var player in ServerEntrance.PlayerList)
				{
					if (player.Value.status.spawned)
					{
						BaseScript.TriggerClientEvent(Funzioni.GetPlayerFromId(player.Key), "lprp:mostrasalvataggio");
						Funzioni.SalvaPersonaggio(Funzioni.GetPlayerFromId(player.Key));
						Log.Printa(LogType.Info, "Salvato personaggio: '" + player.Value.FullName + "' appartenente a '" + Funzioni.GetPlayerFromId(player.Key).Name + "' - " + player.Value.identifiers.discord);
						BaseScript.TriggerEvent(DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + " Salvato personaggio: '" + player.Value.FullName + "' appartenente a '" + Funzioni.GetPlayerFromId(player.Key).Name + "' - " + player.Value.identifiers.discord);
						await Task.FromResult(0);
					}
				}
				BaseScript.TriggerClientEvent("lprp:aggiornaPlayers", JsonConvert.SerializeObject(ServerEntrance.PlayerList));
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Fatal, "" + ex);
			}
			if(sender!=0)
				ChatMain.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
		}

		public static void Sviluppatore(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatMain.commands["sviluppatore"]))
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
				ChatMain.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
		}

		public static void SetJob(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatMain.commands["setjob"]))
				{
					Player p = Funzioni.GetPlayerFromId(args[0]);
					if (ServerEntrance.PlayerList.ContainsKey(p.Handle))
					{
						User pers = ServerEntrance.PlayerList[p.Handle];
						if (pers.status.spawned)
							pers.SetJob(args[1], Convert.ToInt32(args[2]));
						else
							Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO setjob] = ", "Errore il player non ha selezionato un personaggio, riprova!" }, color = new[] { 255, 0, 0 } });
					}
					else
						Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO setjob] = ", "Errore id player non trovato, riprova!" }, color = new[] { 255, 0, 0 } });
				}
				ChatMain.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
			}
		}

		public static void SetGang(int sender, List<dynamic> args, string rawCommand)
		{
			if (sender != 0)
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatMain.commands["setgang"]))
				{
					Player p = Funzioni.GetPlayerFromId(args[0]);
					if (ServerEntrance.PlayerList.ContainsKey(p.Handle))
					{
						User pers = ServerEntrance.PlayerList[p.Handle];
						if (pers.status.spawned)
							pers.SetGang(args[1], Convert.ToInt32(args[2]));
						else
							Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO setgang] = ", "Errore il player non ha selezionato un personaggio, riprova!" }, color = new[] { 255, 0, 0 } });
					}
					else
						Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO setgang] = ", "Errore id player non trovato, riprova!" }, color = new[] { 255, 0, 0 } });
				}
				ChatMain.chatCommandEntered(Funzioni.GetPlayerFromId(sender), rawCommand);
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
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatMain.commands["cambiaora"]))
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
			int h = (int)Math.Floor(Meteo_New.Orario.secondOfDay / 3600f);
			int m = (int)Math.Floor((Meteo_New.Orario.secondOfDay - (h * 3600)) / 60f);
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
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatMain.commands["bloccatempo"]))
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
				bool validWeatherType = false;
				if (args.Count > 1 || Convert.ToInt32(args[0]) > 14 || !(args[0] as string).All(o => char.IsDigit(o)))
				{
					Log.Printa(LogType.Error, "/weather <weathertype>\nCurrent Weather: " + Meteo_New.Meteo.currentWeather);
					return;
				}
				else
				{
					var tableKeys = Shared.ConfigShared.SharedConfig.Main.Meteo.ss_weather_Transition.Keys;
					foreach (var p in tableKeys)
						if (p == Convert.ToInt32(args[0]))
							validWeatherType = true;
					if (validWeatherType)
					{
						Meteo_New.Meteo.currentWeather = Convert.ToInt32(args[0]);
						Meteo_New.Meteo.weatherTimer = Shared.ConfigShared.SharedConfig.Main.Meteo.ss_weather_timer * 60;
						BaseScript.TriggerEvent("changeWeather", false);
					}
					else
						Log.Printa(LogType.Error, "Errore weather, argomenti disponibili: 0 = EXTRASUNNY, 1 =  CLEAR, 2 = CLOUDS, 3 = SMOG, 4 = FOGGY, 5 = OVERCAST, 6 = RAIN, 7 = THUNDERSTORM, 8 = CLEARING, 9 = NEUTRAL, 10 = SNOW, 11 =  BLIZZARD, 12 = SNOWLIGHT, 13 = XMAS, 14 = HALLOWEEN");
				}
			}
			else
			{
				if (Funzioni.IsPlayerAndHasPermission(sender, ChatMain.commands["setmeteo"]))
				{
					bool validWeatherType = false;
					if (args.Count < 1 || Convert.ToInt32(args[0]) > 14 || !(args[0] as string).All(o => char.IsDigit(o)))
						Funzioni.GetPlayerFromId(sender).TriggerEvent("chat:addMessage", new { args = new[] { "[COMANDO weather] = ", "Errore weather, argomenti disponibili: ~n~0 = EXTRASUNNY, 1 = CLEAR, 2 = CLOUDS, 3 = SMOG, 4 = FOGGY, 5 = OVERCAST 6 = RAIN, 7 = THUNDERSTORM, 8 = CLEARING, ~n~9 = NEUTRAL, 10 = SNOW, 11 = BLIZZARD, 12 = SNOWLIGHT, 13 = XMAS, 14 = HALLOWEEN!" }, color = new[] { 255, 0, 0 } });
					else
					{
						var tableKeys = Shared.ConfigShared.SharedConfig.Main.Meteo.ss_weather_Transition.Keys;
						foreach (var p in tableKeys)
							if (p == Convert.ToInt32(args[0]))
								validWeatherType = true;
						if (validWeatherType)
						{
							Meteo_New.Meteo.currentWeather = Convert.ToInt32(args[0]);
							Meteo_New.Meteo.weatherTimer = Shared.ConfigShared.SharedConfig.Main.Meteo.ss_weather_timer * 60;
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
		}

		private static async void DaiLicenza(int sender, List<dynamic> args, string rawCommand)
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

		private static async void RimuoviLicenza(int sender, List<dynamic> args, string rawCommand)
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
