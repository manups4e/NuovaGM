﻿using System;
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
		public int UserID;
		public string group;
		public UserGroup group_level;
		public long playTime;
		public DateTime lastConnection;
		public Status status = new();
		[JsonIgnore] public Player Player;
		[JsonIgnore] private string discord;
		[JsonIgnore] private string license;
		public Identifiers identifiers = new();
		[JsonIgnore]public PlayerStateBags StatiPlayer;

		public List<Char_data> Characters = new();
		[JsonIgnore]
		private Char_data _current;
		public Char_data CurrentChar
		{
			get => _current;
			set => _current = value;
		}


		[JsonIgnore]
		internal string char_data
		{
			get => Characters.ToJson(settings: JsonHelper.IgnoreJsonIgnoreAttributes);
			set => Characters = value.FromJson<List<Char_data>>(settings: JsonHelper.IgnoreJsonIgnoreAttributes);
		}

		public BasePlayerShared()
		{
		}
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