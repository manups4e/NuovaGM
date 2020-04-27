using CitizenFX.Core;
using Logger;
using Newtonsoft.Json;
using NuovaGM.Server.Discord.GuildData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NuovaGM.Server.Discord
{
	static class BotDiscordHandler
	{
		public static Guild TheLastServer;
		public static void Init()
		{
			Server.Instance.AddTick(ConnessioneAlBot);
		}

		private static async Task ConnessioneAlBot()
		{
			RequestResponse risposta = await InviaAlBotERicevi(new { tipo = "ConnessioneAlServer" });
			while (risposta.status != System.Net.HttpStatusCode.OK)
			{
				Log.Printa(LogType.Warning, "Connessione al server discord non riuscita, nuovo tentativo ogni 5 secondi...");
				await BaseScript.Delay(5000);
				risposta = await InviaAlBotERicevi(new { tipo = "ConnessioneAlServer" });
			}
			if (risposta.status == System.Net.HttpStatusCode.OK) 
			{
				TheLastServer = JsonConvert.DeserializeObject<Guild>(risposta.content);
				Log.Printa(LogType.Info, $"Connesso a {TheLastServer.name}, totale membri {TheLastServer.member_count}");
			}
			await BaseScript.Delay(300000);
		}

		public static async Task InviaAlBot(object data)
		{
			await new Request().Http("localhost:1337", "GET", JsonConvert.SerializeObject(data));
		}

		public static async Task<RequestResponse> InviaAlBotERicevi(object data)
		{
			return await new Request().Http("localhost:1337", "GET", JsonConvert.SerializeObject(data));
		}

		public static async Task<bool> DoesPlayerHaveRole(string discordId, string rolename)
		{
			RequestResponse response = await InviaAlBotERicevi(new { tipo = "RichiestaRuoloPlayer", RichiestaInterna = new { IdMember = discordId, IdRole = rolename } });
			bool ruolo = JsonConvert.DeserializeObject<bool>(response.content);
			return ruolo;
		}

	}
}
