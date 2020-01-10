using CitizenFX.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NuovaGM.Server
{
	static class DiscordWhitelist
	{
		private static string GuildId;
		private static string DiscordToken = "Bot ";
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
}
