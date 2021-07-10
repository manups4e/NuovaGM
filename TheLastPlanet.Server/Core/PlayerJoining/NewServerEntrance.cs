using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using Logger;
using TheLastPlanet.Server.Core.PlayerChar;
using TheLastPlanet.Server.Discord;
using TheLastPlanet.Server.Internal.Events;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.PlayerChar;
using TheLastPlanet.Shared.Snowflakes;
using TheLastPlanet.Shared.Internal.Events;
using TheLastPlanet.Server.FreeRoam.Scripts.EventiFreemode;
using TheLastPlanet.Server.Core.Buckets;
using Newtonsoft.Json;

namespace TheLastPlanet.Server.Core.PlayerJoining
{
	internal static class NewServerEntrance
	{
		private static readonly string NoWhitelist = $@"{{""$schema"": ""http://adaptivecards.io/schemas/adaptive-card.json"",""type"": ""AdaptiveCard"",""version"": ""1.0"",""body"": [{{""type"": ""ColumnSet"",""columns"": [{{""type"": ""Column"",""width"": 2,""items"": [{{""type"": ""TextBlock"",""text"": ""Non sei whitelistato nel server"",""weight"": ""Bolder"",""size"": ""Medium""}},{{""type"": ""TextBlock"",""text"": ""Non hai i permessi necessari ad accedere al server."",""isSubtle"": true,""wrap"": true}},{{""type"": ""TextBlock"",""text"": ""Siamo in fase Alpha Testing, vuoi partecipare al testing e segnalare i bugs per aiutare lo sviluppo? Inserisci i tuoi dati qui sotto ed entra nel nostro Discord! (https://discord.gg/n4ep9Fq)"",""isSubtle"": true,""wrap"": true,""size"": ""Small""}},{{""type"": ""TextBlock"",""text"": ""Il tuo nome"",""wrap"": true}},{{""type"": ""Input.Text"",""id"": ""myName"",""placeholder"": ""Scrivi qui Nome o NickName""}},{{""type"": ""TextBlock"",""text"": ""Motivazione"",""wrap"": true}},{{""type"": ""Input.Text"",""id"": ""myMotivazione"",""placeholder"": ""Scrivi qui la motivazione"",""style"": ""Text""}},{{""type"": ""TextBlock"",""text"": ""Nome Discord""}}]}},{{""type"": ""Column"",""width"": 1,""items"": [{{""type"": ""Image"",""url"": ""https://miro.medium.com/max/1000/1*OQQLQscmbtr-xxxw5GKZ3w.jpeg"",""size"": ""auto""}}]}}]}},{{""type"": ""Input.Text"",""placeholder"": ""Scrivi qui NomeDiscord#0000"",""id"": ""MyDiscordId""}}],""actions"": [{{""type"": ""Action.Submit"",""title"": ""Invia""}}]}}";
		private static readonly string ControlloLicenza = $@"{{""$schema"":""http://adaptivecards.io/schemas/adaptive-card.json"",""type"": ""AdaptiveCard"",""version"": ""1.0"",""body"": [{{""type"": ""TextBlock"",""text"": ""Raccolta Informazioni..""}}],""backgroundImage"": {{""url"": ""https://s7.gifyu.com/images/dots.gif"",""horizontalAlignment"": ""Center""}},""minHeight"": ""360px"",""verticalContentAlignment"": ""Bottom""}}";
		private static readonly string IngressoConsentito = $@"{{""$schema"":""http://adaptivecards.io/schemas/adaptive-card.json"",""type"": ""AdaptiveCard"",""version"": ""1.0"",""body"": [{{""type"": ""TextBlock"",""text"": ""Shield 2.0: Accesso consentito, attendi...""}}],""backgroundImage"": {{""url"": ""https://s7.gifyu.com/images/dots.gif"",""horizontalAlignment"": ""Center""}},""minHeight"": ""360px"",""verticalContentAlignment"": ""Bottom""}}";
		private static readonly string Errore = $@"{{""$schema"": ""http://adaptivecards.io/schemas/adaptive-card.json"",""type"": ""AdaptiveCard"",""version"": ""1.3"",""body"": [{{""type"": ""ColumnSet"",""columns"": [{{""type"": ""Column"",""width"": 2,""items"": [{{""type"": ""TextBlock"",""text"": ""Errore sconosciuto"",""weight"": ""Bolder"",""size"": ""Medium""}},{{""type"": ""TextBlock"",""text"": ""Siamo spiacenti, l'accesso al server o la comunicazione con il bot ha subito un errore imprevisto!"",""isSubtle"": true,""wrap"": true}},{{""type"": ""TextBlock"",""text"": ""Per farci perdonare, ecco qui l'immagine di un gattino su di un unicorno!"",""isSubtle"": true,""wrap"": true,""size"": ""Small""}}]}},{{""type"": ""Column"",""width"": 1,""items"": [{{""type"": ""Image"",""url"": ""https://iyanceres.files.wordpress.com/2018/02/cat-unicorn.jpg"",""size"": ""auto""}}]}}]}}]}}";

		public static void Init()
		{
			Server.Instance.AddEventHandler("playerConnecting", new Action<Player, string, CallbackDelegate, ExpandoObject>(PlayerConnecting));
			Server.Instance.AddEventHandler("playerJoining", new Action<Player, string>(PlayerJoining));
			Server.Instance.AddEventHandler("playerDropped", new Action<Player, string>(Dropped));

			// TODO: SPOSTARE IN ROLEPLAY
			Server.Instance.Events.Mount("lprp:setupUser", new Func<ClientId, Task<Tuple<Snowflake, BasePlayerShared>>>(SetupUser));
			Server.Instance.Events.Mount("lprp:RequestLoginInfo", new Func<ClientId, Task<List<LogInInfo>>>(LogInfo));
			Server.Instance.Events.Mount("lprp:anteprimaChar", new Func<ulong, Task<SkinAndDress>>(PreviewChar));
			Server.Instance.Events.Mount("lprp:Select_Char", new Func<ClientId, ulong, Task<Char_data>>(LoadChar));
			Server.Instance.Events.Mount("lprp:Select_FreeRoamChar", new Func<ClientId, int, Task<FreeRoamChar>>(LoadFreeRoamChar));

#if DEBUG
			Server.Instance.AddEventHandler("onResourceStart", new Action<string>(async (resName) =>
			{
				foreach(var p in Server.Instance.GetPlayers)
				{
					PlayerJoining(p, "");
				}
			}));
#endif
		}

		private static async void PlayerConnecting([FromSource] Player source, string playerName, dynamic denyWithReason, dynamic deferrals)
		{
			deferrals.defer();
			await BaseScript.Delay(500);

			try
			{
				deferrals.presentCard(ControlloLicenza);
				await BaseScript.Delay(1000);
				List<string> PlayerTokens = new();
				int tokensNum = GetNumPlayerTokens(source.Handle);
				for (int i = 0; i < tokensNum; i++) PlayerTokens.Add(GetPlayerToken(source.Handle, i));
				IngressoResponse puoentrare = await BotDiscordHandler.DoesPlayerHaveRole(source.GetLicense(Identifier.Discord), Server.Impostazioni.Coda.permessi, PlayerTokens);

				if (puoentrare.permesso)
				{
					if (!Server.Debug)
						if (Server.Instance.Clients.Any(x => source.Identifiers["license"] == x.Identifiers.License))
						{
							deferrals.done($"Last Planet Shield 2.0\nSiamo spiacenti.. ma pare che tu stia usando una licenza attualmente già in uso tra i giocatori online.\n" + $"Tu - Lic.: {source.Identifiers["license"].Replace(source.Identifiers["license"].Substring(20), "")}...,\n" + $"altro player - Lic.: {Server.Instance.Clients.FirstOrDefault(x => source.Identifiers["license"] == x.Identifiers.License).Identifiers.License.Replace(Server.Instance.Clients.FirstOrDefault(x => source.Identifiers["license"] == x.Identifiers.License).Identifiers.License.Substring(20), "")}..., nome: {Server.Instance.Clients.FirstOrDefault(x => source.Identifiers["license"] == x.Identifiers.License).Player.Name}\n" + $"Fai uno screenshot di questo messaggio e contatta gli amministratori del server.\n Grazie");

							return;
						}

					deferrals.presentCard(IngressoConsentito);
					await BaseScript.Delay(2000);
					deferrals.done();
				}
				else
				{
					if (puoentrare.bannato)
					{
						string banText = "Last Planet Shield 2.0.";

						if (!string.IsNullOrEmpty(puoentrare.datafine))
						{
							string datafine = "MAI";
							banText += "\nSei attualmente bannato dal server!";
							if (puoentrare.temporaneo) banText += "\nIl tuo ban è temporaneo, potrai ri-entrare dopo la data e l'orario di fine ban.";
							banText += "\n- BAN ID: " + puoentrare.banId;
							banText += "\n- Bannato da: " + puoentrare.banner;
							banText += "\n- Motivazione: " + puoentrare.motivazione;
							banText += "\n- Data di fine: " + puoentrare.datafine;
						}
						else
						{
							banText += "\nIl tuo accesso al server è stato bloccato!";
							banText += "\n\n- Motivazione: " + puoentrare.motivazione;
							banText += "\n- Bannato da: SISTEMA ANTICHEAT";
						}

						banText += "\n\nSe vuoi parlare con lo staff riguardo al tuo ban, ricorda di segnare il BAN ID (se presente, oppure fai uno screenshot dell'errore) e riferiscilo allo staff.";
						deferrals.done(banText);

						return;
					}
					else
					{
						if (Server.Impostazioni.Coda.whitelistonly)
						{
							deferrals.presentCard(NoWhitelist, new Action<dynamic>(async (var) =>
							{
								object dati = new { tipo = "RichiestaIngresso", RichiestaInterna = var, items = source.Identifiers.ToList(), nome = source.Name };
								deferrals.done("Ti ringraziamo per la tua candidatura! I nostri admin la prenderanno in considerazione e se la riterranno valida ti contatteranno!\n" + "Resta aggiornato sul mio server discord con invito https://discord.gg/n4ep9Fq!");
								await BotDiscordHandler.InviaAlBot(dati);
							}));

							return;
						}
					}
				}
			}
			catch (Exception e)
			{
				Server.Logger.Error(e.ToString());
				deferrals.presentCard(Errore);
			}
		}

		private static async void PlayerJoining([FromSource] Player source, string oldId)
		{
			try
			{
				Snowflake newone = SnowflakeGenerator.Instance.Next();
				const string procedure = "call IngressoPlayer(@disc, @lice, @name, @snow)";
				BasePlayerShared p = await MySQL.QuerySingleAsync<BasePlayerShared>(procedure, new { disc = Convert.ToInt64(source.GetLicense(Identifier.Discord)), lice = source.GetLicense(Identifier.License), name = source.Name, snow = newone.ToInt64() });
				User user = new(source, p);
				ClientId client = new(user);
				Server.Instance.Clients.Add(client);
			}
			catch (Exception e)
			{
				Server.Logger.Error(e.ToString());
			}
		}

		private static async void EntratoMaProprioSulSerio(Player player)
		{
			await Server.Instance.Execute($"UPDATE users SET last_connection = @last WHERE discord = @id", new { last = DateTime.Now, id = player.GetLicense(Identifier.Discord) });
		}

		private static async Task<Tuple<Snowflake, BasePlayerShared>> SetupUser(ClientId source)
		{
			try
			{
				source.User.StatiPlayer = new PlayerStateBags(source.Player);
				EntratoMaProprioSulSerio(source.Player);
				return new Tuple<Snowflake, BasePlayerShared>(source.Id, source.User);
			}
			catch (Exception e)
			{
				Server.Logger.Error(e.ToString());

				return default;
			}
		}

		private static async Task<List<LogInInfo>> LogInfo(ClientId client)
		{
			string query = "SELECT CharID, info, money, bank FROM personaggi WHERE UserID = @id";
			var info = await MySQL.QueryListAsync(query, new
			{
				client.User.ID
			});
			List<LogInInfo> result = new();
			foreach(var ii in info)
			{
				var p = (ii.info as string).FromJson<Info>();
				result.Add(new LogInInfo()
				{
					ID = ((long)ii.CharID).ToString(),
					firstName = p.firstname,
					lastName = p.lastname,
					dateOfBirth = p.dateOfBirth,
					Money = ii.money,
					Bank = ii.bank,
				});
			}

			return result;
		}

		private static async Task<SkinAndDress> PreviewChar(ulong id)
		{

			SkinAndDress result = new();
			string querySkin = "SELECT skin FROM personaggi WHERE CharID = @id";
			string queryDress = "SELECT dressing FROM personaggi WHERE CharID = @id";
			string queryLoc = "SELECT location FROM personaggi WHERE CharID = @id";

			string skin = await MySQL.QuerySingleAsync<string>(querySkin, new { id });
			string dress = await MySQL.QuerySingleAsync<string>(queryDress, new { id });
			string loc = await MySQL.QuerySingleAsync<string>(queryLoc, new { id });

			result.Skin = skin.FromJson<Skin>();
			result.Position = loc.FromJson<Position>();
			result.Dressing = dress.FromJson<Dressing>();
			result.Dressing.Name = string.Empty;
			result.Dressing.Description = string.Empty;

			Server.Logger.Debug(result.Dressing.ToJson());
			return result;
		}

		private static async Task<Char_data> LoadChar(ClientId source, ulong id)
		{
			string query = "SELECT * FROM personaggi WHERE CharID = @id";
			User user = Funzioni.GetClientFromPlayerId(source.Handle).User;
			Char_Metadata res = await MySQL.QuerySingleAsync<Char_Metadata>(query, new { id });
			user.CurrentChar = new Char_data()
			{
				Info = res.info.FromJson<Info>(),
				Finance = new Finance(res.money, res.bank, res.dirtyCash),
				Posizione = res.location.FromJson<Position>(),
				Job = new Job(res.job, res.job_grade),
				Gang = new Gang(res.gang, res.gang_grade),
				Skin = res.skin.FromJson<Skin>(),
				Inventory = res.inventory.FromJson<List<Inventory>>(),
				Weapons = res.weapons.FromJson<List<Weapons>>(),
				Dressing = res.dressing.FromJson<Dressing>(),
				Needs = res.needs.FromJson<Needs>(),
				Statistiche = res.statistiche.FromJson<Statistiche>()
			};
			user.CurrentChar.Dressing.Name = string.Empty;
			user.CurrentChar.Dressing.Description = string.Empty;

			return user.CurrentChar;
		}

		private static async Task<FreeRoamChar> LoadFreeRoamChar(ClientId source, int id)
		{
			string query = "SELECT * FROM freeroampersonaggi WHERE UserID = @id";
			User user = Funzioni.GetClientFromPlayerId(source.Handle).User;
			FreeRoamChar_Metadata res = await MySQL.QuerySingleAsync<FreeRoamChar_Metadata>(query, new { id });
			if(res == null)
			{
				user.FreeRoamChar = new FreeRoamChar();
				(BucketsHandler.FreeRoam.Bucket as FreeRoamBucket).AddPlayer(source);
				return user.FreeRoamChar;

			}
			user.FreeRoamChar = new()
			{
				Finance = new Finance(res.money, res.bank, 0),
				Posizione = res.location.FromJson<Position>(),
				Gang = new Gang(res.gang, res.gang_grade),
				Skin = res.skin.FromJson<Skin>(),
				Weapons = res.weapons.FromJson<List<Weapons>>(),
				Dressing = res.dressing.FromJson<Dressing>(),
				Statistiche = res.statistiche.FromJson<FreeRoamStats>(),
				Level = res.level,
				TotalXp = res.totalXp,
			};
			(BucketsHandler.FreeRoam.Bucket as FreeRoamBucket).AddPlayer(source);
			return user.FreeRoamChar;
		}


		public static async void Dropped([FromSource] Player player, string reason)
		{
			Player p = player;
			string name = p.Name;
			string handle = p.Handle;
			DateTime now = DateTime.Now;
			string text = name + " e' uscito.";
			if (reason != "")
				text = reason switch
				{
					"Timed out after 10 seconds." => name + " e' crashato.",
					"Disconnected." or "Exited."  => name + " si e' disconnesso.",
					_                             => name + " si e' disconnesso: " + reason
				};
			ClientId client = Funzioni.GetClientFromPlayerId(int.Parse(player.Handle));

			if (client != null)
			{
				User ped = client.User;
				string disc = ped.Identifiers.Discord;

				if (ped.status.Spawned)
				{
					await ped.SalvaPersonaggio();
					Server.Logger.Info("Salvato personaggio: '" + ped.FullName + "' appartenente a '" + name + "' all'uscita dal gioco -- Discord:" + disc);
				}
				else
				{
					Server.Logger.Info("Il Player '" + name + "' - " + disc + " è uscito dal server senza selezionare un personaggio");
				}

				Server.Instance.Clients.Remove(client);
			}

			Server.Logger.Info(text);
			BaseScript.TriggerClientEvent("lprp:ShowNotification", "~r~" + text);
		}
	}
}
