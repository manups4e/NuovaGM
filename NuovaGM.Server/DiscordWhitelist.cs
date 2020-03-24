using CitizenFX.Core;
using Newtonsoft.Json;
using NuovaGM.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NuovaGM.Server
{
	static class DiscordWhitelist
	{
		private static string GuildId;
		private static string DiscordToken = "Bot ";
		private static string sputtanabot = "https://discordapp.com/api/webhooks/686884834337095732/sXSQKCYdh6xTc__QJ8tE3RnWlfdDvBwyTC5N-GbC6SY4zDA7hViYW5kBGZpuRTWrijE3";
		public static bool ConnessoADiscord = false;

		public static async void Init()
		{
			while (ConfigServer.Conf.Main.DiscordToken == null) await BaseScript.Delay(0);
			GuildId = ConfigServer.Conf.Main.GuildId;
			DiscordToken += ConfigServer.Conf.Main.DiscordToken;
			Server.GetInstance.RegisterTickHandler(connessioneDiscord);
		}

		public async static Task connessioneDiscord()
		{
			RequestResponse connessione = await DiscordConnection("guilds/" + GuildId);
			if (connessione.status != System.Net.HttpStatusCode.OK)
			{
				Log.Printa(LogType.Warning, "Errore nel contattare i server Discord, controlla la configurazione e assicurati che sia tutto corretto. Errore: " + connessione.status);
				ConnessoADiscord = false;
			}
			while (connessione.status != System.Net.HttpStatusCode.OK)
			{
				Log.Printa(LogType.Warning, "Nuovo tentativo di riconnessione ai server Discord in corso (ogni 5 secondi)");
				await BaseScript.Delay(5000);
				connessione = await DiscordConnection("guilds/" + GuildId);
			}
			dynamic data = JsonConvert.DeserializeObject<DiscordGuildResponse>(connessione.content);
			Log.Printa(LogType.Info, "Connesso correttamente al Server \"" + data.name + "\" [codice: " + data.id + "]");
			ConnessoADiscord = true;
			await BaseScript.Delay(300000);
		}

		public static async void SendWebhookMessageCoda(Dictionary<string, string> idents, string name, string hook, string Flag, string Info)
		{
			string Webhook = hook;
			var IdentList = idents;
			string SteamHex = IdentList["steam"];
			long SteamDec = Convert.ToInt64(SteamHex, 16);
			string License = IdentList["license"];
			string DiscordIdent = IdentList["discord"];
			string FiveM = IdentList["fivem"];

			// Json builder
			JsonEmbeds jsonbuilder = new JsonEmbeds();
			jsonbuilder.Embeds = new List<Embed>();
			// Base builder
			Embed data = new Embed();
			data.Fields = new List<Field>();
			// Base data (Name, Flag, Info etc.)
			data.Username = "Sputtanatore"; data.Title = Flag; data.Description = Info;

			// Field data
			// Name field
			Field Name = new Field();
			Name.Name = "Nome"; Name.Value = name; Name.Inline = true;

			// Id field
			Field Id = new Field();
			Id.Name = "Server Id"; Id.Value = "nessuno"; Id.Inline = true;

			// Base ident field
			Field Ident = new Field();
			Ident.Name = "Identifiers"; Ident.Value = "Questi sono i dati del player rilevati"; Ident.Inline = false;

			// Steam field
			Field Steam = new Field();
			Steam.Name = "Steam"; Steam.Value = "[" + SteamHex + "](http://steamcommunity.com/profiles/" + SteamDec + ")"; Steam.Inline = true;

			// License field
			Field Rockstar = new Field();
			Rockstar.Name = "License"; Rockstar.Value = License.Replace(License.Substring(License.Length - 20, 20), "********************"); Rockstar.Inline = true;

			// Discord field
			Field Discord = new Field();
			Discord.Name = "Discord"; Discord.Value = "Id: " + DiscordIdent + "\n Tag: <@" + DiscordIdent + ">"; Discord.Inline = true;

			// FiveM field
			Field Fivem = new Field();
			Fivem.Name = "FiveM"; Fivem.Value = "Id: " + FiveM + ""; Fivem.Inline = true;

			// Insert data 
			data.Fields.Add(Name);
			data.Fields.Add(Id);
			data.Fields.Add(Ident);
			data.Fields.Add(Steam);
			data.Fields.Add(Rockstar);
			data.Fields.Add(Discord);
			data.Fields.Add(Fivem);
			jsonbuilder.Embeds.Add(data);

			var userData = JsonConvert.SerializeObject(jsonbuilder);
			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers.Add("Content-Type", "application/json");

			if (Webhook != "none")
			{
				Request request = new Request();
				RequestResponse result = await request.Http(Webhook, "POST", userData, headers);
			}
		}

		public static async void SendWebhookMessage(Player Source, string hook, string Flag, string Info)
		{
			string Webhook = hook;
			var IdentList = Source.Identifiers;
			string SteamHex = Source.Identifiers["steam"];
			long SteamDec = Convert.ToInt64(SteamHex, 16);
			string License = Source.Identifiers["license"];
			string DiscordIdent = Source.Identifiers["discord"];
			string FiveM = Source.Identifiers["fivem"];

			// Json builder
			JsonEmbeds jsonbuilder = new JsonEmbeds();
			jsonbuilder.Embeds = new List<Embed>();
			// Base builder
			Embed data = new Embed();
			data.Fields = new List<Field>();
			// Base data (Name, Flag, Info etc.)
			data.Username = "Sputtanatore"; data.Title = Flag; data.Description = Info;

			// Field data
			// Name field
			Field Name = new Field();
			Name.Name = "Nome"; Name.Value = Source.Name; Name.Inline = true;

			// Id field
			Field Id = new Field();
			Id.Name = "Server Id"; Id.Value = Source.Handle; Id.Inline = true;

			// Base ident field
			Field Ident = new Field();
			Ident.Name = "Identifiers"; Ident.Value = "Questi sono i dati rilevati"; Ident.Inline = false;

			// Steam field
			Field Steam = new Field();
			Steam.Name = "Steam"; Steam.Value = "[" + SteamHex + "](http://steamcommunity.com/profiles/" + SteamDec + ")"; Steam.Inline = true;

			// License field
			Field Rockstar = new Field();
			Rockstar.Name = "License"; Rockstar.Value = License.Replace(License.Substring(License.Length-8, 8), "********"); Rockstar.Inline = true;

			// Discord field
			Field Discord = new Field();
			if (Discord != null)
			{
				Discord.Name = "Discord"; Discord.Value = "Id: " + DiscordIdent + "\n Tag: <@" + DiscordIdent + ">"; Discord.Inline = true;
			}

			// Insert data 
			data.Fields.Add(Name);
			data.Fields.Add(Id);
			data.Fields.Add(Ident);
			data.Fields.Add(Steam);
			data.Fields.Add(Rockstar);
			data.Fields.Add(Discord);
			jsonbuilder.Embeds.Add(data);

			var userData = JsonConvert.SerializeObject(jsonbuilder);
			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers.Add("Content-Type", "application/json");

			if (Webhook != "none")
			{
				Request request = new Request();
				RequestResponse result = await request.Http(Webhook, "POST", userData, headers);
			}
		}

		public static async Task<RequestResponse> DiscordConnection(string endpoint, string method = "GET", string jsondata = "")
		{
			Request r = new Request();
			RequestResponse response = await r.Http("https://discordapp.com/api/" + endpoint, method, jsondata, new Dictionary<string, string> { ["Content-Type"] = "application/json", ["Authorization"] = DiscordToken }).ConfigureAwait(true);
			return response;
		}

		public static async Task<bool> DoesPlayerHaveRole(string discordId, string role)
		{
			string endpoint = "guilds/" + GuildId + "/members/" + discordId;
			RequestResponse member = await DiscordConnection(endpoint);
			if (member.status != System.Net.HttpStatusCode.OK)
				return false;
			else
			{
				DiscordMemberResponse data = JsonConvert.DeserializeObject<DiscordMemberResponse>(member.content);
				foreach (var r in data.roles)
					if (r == role)
						return true;
				return false;
			}
		}
	}

	internal class DiscordGuildResponse
	{
		public int mfa_level;
		public string application_id;
		public List<dynamic> features;
		public int afk_timeout;
		public string system_channel_id;
		public int default_message_notifications;
		public bool widget_enabled;
		public string afk_channel_id;
		public dynamic premium_subscription_count;
		public int explicit_content_filter;
		public dynamic max_presences;
		public string id;
		public string icon;
		public string preferred_locale;
		public int verification_level;
		public string name;
		public List<GuildRoles> roles = new List<GuildRoles>();
		public string widget_channel_id;
		public string description;
		public string embed_channel_id;
		public int system_channel_flags;
		public string banner;
		public int premium_tier;
		public string splash;
		public int max_members;
		public List<string> emojis = new List<string>();
		public bool embed_enabled;
		public string region;
		public string vanity_url_code;
		public string owner_id;
	}

	internal class GuildRoles
	{
		public bool hoist;
		public string name;
		public bool mentionale;
		public int color;
		public int position;
		public string id;
		public bool managed;
		public long permissions;
	}

	internal class DiscordMemberResponse
	{
		public string nick;
		public DiscordUser user;
		public List<string> roles = new List<string>();
		public string premium_since;
		public bool deaf;
		public bool mute;
		public DateTime joined_at;
	}
	internal class DiscordUser
	{
		public string username;
		public string discriminator;
		public string id;
		public string avatar;
	}

	public class JsonEmbeds
	{
		[JsonProperty("embeds")]
		public List<Embed> Embeds { get; set; }
	}

	public class Embed
	{
		[JsonProperty("username")]
		public string Username { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("fields")]
		public List<Field> Fields { get; set; }
	}

	public class Field
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("value")]
		public string Value { get; set; }

		[JsonProperty("inline")]
		public bool Inline { get; set; }
	}
}
