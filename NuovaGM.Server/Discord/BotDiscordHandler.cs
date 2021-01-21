using CitizenFX.Core;
using Logger;
using Newtonsoft.Json;
using TheLastPlanet.Server.Discord.GuildData;
using TheLastPlanet.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core.Native;

namespace TheLastPlanet.Server.Discord
{
	static class BotDiscordHandler
	{
		//private static string serverUrl = "http://45.14.185.37:1337";
		private static string serverUrl = "http://localhost:1337";
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
				TheLastServer = risposta.content.Deserialize<Guild>();
				Log.Printa(LogType.Info, $"Connesso a {TheLastServer.name}, totale membri {TheLastServer.member_count}");
			}
		}

		public static async Task InviaAlBot(object data)
		{
			await new Request().Http(serverUrl, "GET", data.Serialize());
		}

		public static async Task<RequestResponse> InviaAlBotERicevi(object data)
		{
			return await new Request().Http(serverUrl, "GET", data.Serialize());
		}

		public static async Task<IngressoResponse> DoesPlayerHaveRole(string discordId, List<string> Ruoli, List<string> tokens)
		{
			RequestResponse response = await InviaAlBotERicevi(new { tipo = "RichiestaRuoloPlayer", RichiestaInterna = new { IdMember = discordId, Ruoli, Tokens = tokens.Serialize()} });
			if(response.status == System.Net.HttpStatusCode.OK)
				return response.content.Deserialize<IngressoResponse>(); 
			else
				return new IngressoResponse() { permesso = false };
		}
	}
	internal class IngressoResponse
	{
		public bool permesso;
        public bool bannato;
        public string motivazione;
        public string banId;
        public bool temporaneo;
        public string datafine;
        public string banner;
	}
}
