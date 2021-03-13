﻿using CitizenFX.Core;
using Logger;
using TheLastPlanet.Server.Discord.GuildData;
using TheLastPlanet.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheLastPlanet.Server.Discord
{
	internal static class BotDiscordHandler
	{
		//private static string serverUrl = "http://45.14.185.37:1337";
		private static string serverUrl = "http://localhost:1337";
		public static Guild TheLastServer;
		public static void Init() { ConnessioneAlBot(); }

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
				TheLastServer = risposta.content.DeserializeFromJson<Guild>();
				Log.Printa(LogType.Info, $"Connesso a {TheLastServer.name}, totale membri {TheLastServer.member_count}");
			}
		}

		public static async Task InviaAlBot(object data) { await Request.Http(serverUrl, "GET", data.SerializeToJson()); }

		public static async Task<RequestResponse> InviaAlBotERicevi(object data) { return await Request.Http(serverUrl, "GET", data.SerializeToJson()); }

		public static async Task<IngressoResponse> DoesPlayerHaveRole(string discordId, List<string> Ruoli, List<string> tokens)
		{
			RequestResponse response = await InviaAlBotERicevi(new { tipo = "RichiestaRuoloPlayer", RichiestaInterna = new { IdMember = discordId, Ruoli, Tokens = tokens.SerializeToJson() } });

			return response.status == System.Net.HttpStatusCode.OK ? response.content.DeserializeFromJson<IngressoResponse>() : new IngressoResponse() { permesso = false };
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