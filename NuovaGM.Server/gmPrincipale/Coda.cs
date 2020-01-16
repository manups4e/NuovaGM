﻿using CitizenFX.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace NuovaGM.Server.gmPrincipale
{
	static class Coda
	{
		private static string notWhitelisted;
		private static bool EnableAntiSpam;
		public static int PlayersToStartRocade;
		private static int WaitingTime = 10;
		public static int onlinePlayers = 0;
		private static Dictionary<string, bool> inConnection = new Dictionary<string, bool>();
		private static bool puoentrare = false;

		public static async void Init()
		{
			Server.GetInstance.RegisterEventHandler("playerConnecting", new Action<Player, string, dynamic, dynamic>(PlayerConnect));
			Server.GetInstance.RegisterEventHandler("playerDropped", new Action<Player, string>(PlayerDropped));
		}

		public static async void PlayerConnect([FromSource] Player player, string name, dynamic kickReason, dynamic deferrals)
		{
			notWhitelisted = ConfigServer.Conf.Main.notWhitelisted;
			EnableAntiSpam = ConfigServer.Conf.Main.EnableAntiSpam;
			PlayersToStartRocade = ConfigServer.Conf.Main.PlayersToStartRocade;
			string nm = player.Name;
			string st = License.GetLicense(player, Identifier.Discord);
			DateTime time = DateTime.Now;
			try
			{
				Log.Printa(LogType.Info, player.Name + " si sta connettendo");
				BaseScript.TriggerEvent("lprp:serverLog", time.ToString("dd/MM/yyyy, HH:mm:ss") + " " + player.Name + " si sta connettendo");
				deferrals.defer();
				await BaseScript.Delay(0);
				//deferrals.update($"Shield 2.0 Controllo credenziali per il Player {player.Name}...");
				string ControlloDiscord = "{\"$schema\": \"http://adaptivecards.io/schemas/adaptive-card.json\",\"type\": \"AdaptiveCard\",\"version\": \"1.0\",\"body\": [{\"type\": \"TextBlock\",\"text\": \"Shield 2.0 Controllo credenziali per il Player " + player.Name + "...\"}],\"backgroundImage\": {\"url\": \"https://s5.gifyu.com/images/Blue_Sky_and_Clouds_Timelapse_0892__Videvo.gif\",\"horizontalAlignment\": \"Center\"},\"minHeight\": \"360px\",\"verticalContentAlignment\": \"Bottom\"}";
				deferrals.presentCard(ControlloDiscord);
				await BaseScript.Delay(2000);
				if (License.GetLicense(player, Identifier.Discord) != "discord:")
				{
					string ControlloLicenza = "{\"$schema\": \"http://adaptivecards.io/schemas/adaptive-card.json\",\"type\": \"AdaptiveCard\",\"version\": \"1.0\",\"body\": [{\"type\": \"TextBlock\",\"text\": \"Shield 2.0 Controllo credenziali per il Player " + player.Name + "...\"}],\"backgroundImage\": {\"url\": \"https://s5.gifyu.com/images/Blue_Sky_and_Clouds_Timelapse_0892__Videvo.gif\",\"horizontalAlignment\": \"Center\"},\"minHeight\": \"360px\",\"verticalContentAlignment\": \"Bottom\"}";
					deferrals.presentCard(ControlloLicenza);
					await BaseScript.Delay(2000);
					if (License.GetLicense(player, Identifier.License) != "license:")
					{
						string ControlloConnessione = "{\"$schema\": \"http://adaptivecards.io/schemas/adaptive-card.json\",\"type\": \"AdaptiveCard\",\"version\": \"1.0\",\"body\": [{\"type\": \"TextBlock\",\"text\": \"Shield 2.0 Controllo credenziali per il Player " + player.Name + "...\"}],\"backgroundImage\": {\"url\": \"https://s5.gifyu.com/images/Blue_Sky_and_Clouds_Timelapse_0892__Videvo.gif\",\"horizontalAlignment\": \"Center\"},\"minHeight\": \"360px\",\"verticalContentAlignment\": \"Bottom\"}";
						deferrals.presentCard(ControlloConnessione);
						await BaseScript.Delay(2000);
						if (DiscordWhitelist.ConnessoADiscord)
						{
//							deferrals.update($"Shield 2.0 Controllo Ban per il Player {player.Name}...");
							string ControlloBan = "{\"$schema\": \"http://adaptivecards.io/schemas/adaptive-card.json\",\"type\": \"AdaptiveCard\",\"version\": \"1.0\",\"body\": [{\"type\": \"TextBlock\",\"text\": \"Shield 2.0 Controllo Ban per il Player " + player.Name + "...\"}],\"backgroundImage\": {\"url\": \"https://s5.gifyu.com/images/Blue_Sky_and_Clouds_Timelapse_0892__Videvo.gif\",\"horizontalAlignment\": \"Center\"},\"minHeight\": \"360px\",\"verticalContentAlignment\": \"Bottom\"}";
							deferrals.presentCard(ControlloBan);
							await BaseScript.Delay(2000);
							dynamic result = await Server.GetInstance.Query("SELECT * FROM bans");
							await BaseScript.Delay(2000);
							if (result.Count > 0)
							{
								for (int i = 0; i < result.Count; i++)
								{
									if (License.GetLicense(player, Identifier.Discord) == result[i].discord)
									{
										DateTime ban = Convert.ToDateTime(result[i].DataFine);
										if (ban.Year == DateTime.MaxValue.Year || ban.Year >= DateTime.Now.AddYears(99).Year)
										{
											deferrals.done("SHIELD 2.0 Sistema Ban:\nSei stato PERMA BANNATO da questo server!\nMOTIVAZIONE: " + result[i].Motivazione + "\nBANNATO DA: " + result[i].Banner + ",\nDATA FINE: MAI, IL BAN E' PERMANENTE!.");
											return;
										}
										else if (ban.Year < DateTime.MaxValue.Year && ban.Year > DateTime.Now.Year ||
											ban.Month > DateTime.Now.Month && ban.Year < DateTime.Now.Year ||
											ban.Day > DateTime.Now.Day && ban.Month < DateTime.Now.Month ||
											ban.Hour > DateTime.Now.Hour && ban.Day < DateTime.Now.Day && ban.Month < DateTime.Now.Month ||
											ban.Minute > DateTime.Now.Minute && ban.Hour < DateTime.Now.Hour && ban.Day < DateTime.Now.Day && ban.Month < DateTime.Now.Month)
										{
											deferrals.done("SHIELD 2.0 Sistema Ban:\nSei stato BANNATO da questo server!\nMOTIVAZIONE: " + result[i].Motivazione + "\nBANNATO DA: " + result[i].Banner + ",\nDATA FINE: " + Convert.ToDateTime(result[i].DataFine).ToString("dd/MM/yyyy, HH:mm:ss") + ".\nSE PENSI DI AVER BISOGNO DI DARE / RICEVERE SPIEGAZIONI RAGGIUNGICI SU DISCORD (https://discord.gg/n4ep9Fq)");
											return;
										}
										else
										{
											int timer = 15;
											deferrals.update($"SHIELD 2.0 Sistema Ban:\nSEI STATO SBANNATO DOPO AVER CONTRAVVENUTO ALLE REGOLE DI BUON GIOCO DEL NOSTRO SERVER\nCOMPORTATI MEGLIO!!!\n Attendi {timer} secondi per entrare nel server.");
											while (timer > 0)
											{
												if (timer != 1)
													deferrals.update($"SHIELD 2.0 Sistema Ban:\nSEI STATO SBANNATO DOPO AVER CONTRAVVENUTO ALLE REGOLE DI BUON GIOCO DEL NOSTRO SERVER\nCOMPORTATI MEGLIO!!!\n Attendi {timer} secondi per entrare nel server.");
												else
													deferrals.update($"SHIELD 2.0 Sistema Ban:\nSEI STATO SBANNATO DOPO AVER CONTRAVVENUTO ALLE REGOLE DI BUON GIOCO DEL NOSTRO SERVER\nCOMPORTATI MEGLIO!!!\n Attendi {timer} secondo per entrare nel server.");
												timer--;
												await BaseScript.Delay(1000);
											}
											await Server.GetInstance.Execute($"DELETE FROM bans WHERE discord = @discord", new { discord = License.GetLicense(player, Identifier.Discord) });
										}
									}
								}
							}
							await BaseScript.Delay(1000);
							puoentrare = await DiscordWhitelist.DoesPlayerHaveRole(player.Identifiers["discord"], ConfigServer.Conf.Main.RuoloWhitelistato);
							await BaseScript.Delay(1000);

							#region PER LA WHITELIST VIA DATABASE DECOMMENTARE QUI DENTRO
							// PER LA WHITELIST VIA DATABASE!
							/*
							dynamic WhiteResult = await Server.GetInstance.Query("SELECT * FROM whitelist")
							for (int i = 0; i < WhiteResult.Count; i++)
							{
								if (DB.GetLicense(player,Identifier.Discord) == WhiteResult[i].identifier)
								{
									puoentrare = true;
								}
							}
							*/
							#endregion

							if (puoentrare)
							{
								if (EnableAntiSpam)
								{
									Log.Printa(LogType.Info, "WHITELIST: Sistema Anti-Spam all'ingresso attivo per il player " + player.Name);
									BaseScript.TriggerEvent("lprp:serverLog", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + " -- WHITELIST: Sistema Anti-Spam all'ingresso attivo per il player " + player.Name);
									for (int i = 0; i < WaitingTime; i++)
									{
										deferrals.update(ConfigServer.Conf.Main.NomeServer + " Shield 2.0 sistema di protezione Anti-Spam\nattendi " + (WaitingTime - i).ToString() + " secondi e sarai connesso automaticamente");
										await BaseScript.Delay(1000);
									}
									Log.Printa(LogType.Info, "WHITELIST: Sistema Anti-Spam all'ingresso disattivato per il player " + player.Name);
									BaseScript.TriggerEvent("lprp:serverLog", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + " -- WHITELIST: Sistema Anti-Spam all'ingresso disattivato per il player " + player.Name);
								}
								deferrals.done();
							}
							else
							{
								Log.Printa(LogType.Warning, "Il Player " + nm + ", con DiscordId: " + st + ", ha tentato di accedere da bannato oppure senza essere whitelistato nel Server!");
								deferrals.done(notWhitelisted);
								return;
							}
						}
						else
						{
							deferrals.done("ERRORE: La connessione ai server proprietari discord non sta funzionando come dovrebbe.\nRiprova a connetterti tra qualche minuto.");
							return;
						}
					}
					else
					{
						Log.Printa(LogType.Warning, "Licenza SocialClub non trovata per il player: " + nm);
						deferrals.done("ERRORE: Impossibile trovare la licenza del SocialClub, Connessione rifiutata.");
						return;
					}
				}
				else
				{
					deferrals.done("ERRORE: DiscordID non trovato, Connessione rifiutata.\nPer giocare sul nostro server devi avere Discord sempre attivo!\nAssicurati di avere Discord avviato prima di aprire FiveM.");
					Log.Printa(LogType.Warning, "Il Player: " + nm + " ha tentato di accedere senza discord attivo");
					return;
				}
			}
			catch (Exception ex)
			{
				Log.Printa(LogType.Fatal, "ERRORE NELL'ACCESSO AL SERVER CONTROLLARE SCRIPT CODA: " + ex);
				Log.Printa(LogType.Fatal, ex.StackTrace);
				deferrals.done("ERRORE NELL'ACCESSO AL SERVER, CONTATTA GLI AMMINISTRATORI SUL NOSTRO CANALE DISCORD\nINVITO: https://discord.gg/n4ep9Fq");
				return;
			}
		}

		public static async void PlayerDropped([FromSource] Player player, string reason)
		{
			string name = player.Name;
			if (ServerEntrance.PlayerList.ContainsKey(player.Handle))
			{
				var ped = ServerEntrance.PlayerList[player.Handle];
				if (ped.status.spawned)
				{
					await Server.GetInstance.Execute("UPDATE `users` SET `Name` = @name, `group` = @gr, `group_level` = @level, `playTime` = @time, `char_current` = @current, `char_data` = @data WHERE `discord` = @id", new
					{
						name = player.Name,
						gr = ped.group,
						level = ped.group_level,
						time = ped.playTime,
						current = ped.char_current,
						data = JsonConvert.SerializeObject(ped.char_data),
						id = ped.identifiers.discord
					});
					Log.Printa(LogType.Info, "Salvato personaggio: '" + ped.FullName + "' appartenente a '" + name + "' all'uscita dal gioco -- Discord:" + ped.identifiers.discord);
					BaseScript.TriggerEvent(DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + " Salvato personaggio: '" + ped.FullName + "' appartenente a '" + name + "' all'uscita dal gioco -- Discord:" + ped.identifiers.discord);
					ServerEntrance.PlayerList.Remove(player.Handle);
				}
				else
				{
					Log.Printa(LogType.Info, "Il Player'" + name + "' - " + ped.identifiers.discord + " è uscito dal server senza selezionare un personaggio");
					BaseScript.TriggerEvent(DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + " Il Player'" + name + "' - " + ped.identifiers.discord + " è uscito dal server senza selezionare un personaggio");
					ServerEntrance.PlayerList.Remove(player.Handle);
				}
			}
			BaseScript.TriggerClientEvent("lprp:aggiornaPlayers", JsonConvert.SerializeObject(ServerEntrance.PlayerList));
			await Task.FromResult(0);
		}
	}
}
