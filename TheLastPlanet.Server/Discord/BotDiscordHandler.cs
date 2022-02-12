using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheLastPlanet.Server.Discord.GuildData;
using TheLastPlanet.Shared;

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
                Server.Logger.Warning("Connessione al bot fallita, controlla che il bot sia attivo..");
                risposta = await InviaAlBotERicevi(new { tipo = "ConnessioneAlServer" });
                await BaseScript.Delay(5000);
            }

            if (risposta.status == System.Net.HttpStatusCode.OK)
            {
                try
                {
                    TheLastServer = risposta.content.FromJson<Guild>();
                    Server.Logger.Info($"Connesso a {TheLastServer.name}, totale membri {TheLastServer.member_count}");
                }
                catch (Exception e)
                {
                    Server.Logger.Error(e.ToString());
                }
            }
        }

        public static async Task InviaAlBot(object data) { await Server.Instance.WebRequest.Http(serverUrl, "GET", data.ToJson()); }

        public static async Task<RequestResponse> InviaAlBotERicevi(object data) { return await Server.Instance.WebRequest.Http(serverUrl, "GET", data.ToJson()); }

        public static async Task<IngressoResponse> DoesPlayerHaveRole(string discordId, List<string> Ruoli, List<string> tokens)
        {
            RequestResponse response = await InviaAlBotERicevi(new { tipo = "RichiestaRuoloPlayer", RichiestaInterna = new { IdMember = discordId, Ruoli, Tokens = tokens.ToJson() } });

            return response.status == System.Net.HttpStatusCode.OK ? response.content.FromJson<IngressoResponse>() : new IngressoResponse() { permesso = false };
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