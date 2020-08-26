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
			ConnessioneAlBot();
		}

		private static async void ConnessioneAlBot()
		{
			RequestResponse risposta = await InviaAlBotERicevi(new { tipo = "ConnessioneAlServer" });
			await BaseScript.Delay(0);
			while (risposta.status != System.Net.HttpStatusCode.OK)
			{
				await BaseScript.Delay(0);
				risposta = await InviaAlBotERicevi(new { tipo = "ConnessioneAlServer" });
				await BaseScript.Delay(5000);
			}
			if (risposta.status == System.Net.HttpStatusCode.OK) 
			{
				TheLastServer = JsonConvert.DeserializeObject<Guild>(risposta.content);
				Log.Printa(LogType.Info, $"Connesso a {TheLastServer.name}, totale membri {TheLastServer.member_count}");
			}
		}

		public static async Task InviaAlBot(object data)
		{
			await new Request().Http("http://45.14.185.37:1337", "GET", JsonConvert.SerializeObject(data));
		}

		public static async Task<RequestResponse> InviaAlBotERicevi(object data)
		{
			return await new Request().Http("http://45.14.185.37:1337", "GET", JsonConvert.SerializeObject(data));
		}

		public static async Task<bool> DoesPlayerHaveRole(string discordId, List<string> Ruoli)
		{
			RequestResponse response = await InviaAlBotERicevi(new { tipo = "RichiestaRuoloPlayer", RichiestaInterna = new { IdMember = discordId, Ruoli} });
			return JsonConvert.DeserializeObject<bool>(response.content); 
		}

	}
}
