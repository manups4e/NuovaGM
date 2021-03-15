using System;
using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core;
using Newtonsoft.Json;

#if CLIENT
using TheLastPlanet.Client.Core.PlayerChar;
#elif SERVER
using TheLastPlanet.Server.Core;
using TheLastPlanet.Server.Core.PlayerChar;
#endif

namespace TheLastPlanet.Shared.PlayerChar
{
	public class BasePlayerShared
	{
		public Snowflake.Snowflake Id;
		public int UserID;
		public string group;
		public UserGroup group_level;
		public uint char_current;
		public long playTime;
		public DateTime lastConnection;
		public Status status = new Status();
		public string discord;
		public string license;
		public Identifiers identifiers = new();
		[JsonIgnore]public PlayerStateBags StatiPlayer;

		public List<Char_data> Characters = new List<Char_data>();

		
		[JsonIgnore]
		internal string char_data
		{
			get => Characters.SerializeToJson(includeEverything: true);
			set => Characters = value.DeserializeFromJson<List<Char_data>>(includeEverything: true);
		}

		public BasePlayerShared()
		{
#if SERVER
			LoadIdentifiers();

#endif
		}

#if SERVER
		private async void LoadIdentifiers()
		{
			while (discord == null) await BaseScript.Delay(0);
			Player player = Server.ServerSession.Instance.GetPlayers.ToList().FirstOrDefault(x => x.Identifiers["discord"] == discord);
			identifiers.Steam = player.GetLicense(Identifier.Steam);
			identifiers.License = player.GetLicense(Identifier.License);
			identifiers.Discord = player.GetLicense(Identifier.Discord);
			identifiers.Fivem = player.GetLicense(Identifier.Fivem);
			identifiers.Ip = player.GetLicense(Identifier.Ip);
            StatiPlayer = new PlayerStateBags(player);
		}
#endif

	}

	public class Identifiers
	{
		public string Steam;
		public string License;
		public string Discord;
		public string Fivem;
		public string Ip;
	}

	public class Status
	{
		public bool Connected = true;
		public bool Spawned = false;
	}
}