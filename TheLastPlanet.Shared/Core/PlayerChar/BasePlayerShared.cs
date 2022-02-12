using System;
using System.Collections.Generic;
using CitizenFX.Core;
using Newtonsoft.Json;
using TheLastPlanet.Shared.Internal.Events.Attributes;
using TheLastPlanet.Shared.Snowflakes;

#if CLIENT
using TheLastPlanet.Client.Core.PlayerChar;
#elif SERVER
using TheLastPlanet.Server.Core;
using TheLastPlanet.Server.Core.PlayerChar;
#endif

namespace TheLastPlanet.Shared.PlayerChar
{
	[Serialization]
	public partial class BasePlayerShared
	{
		public int ID { get; set; }

		[Ignore][JsonIgnore]
		private ulong UserID
		{
			set => PlayerID = Snowflake.Parse(value);
		}

		public string? Name { get; set; }
		public Snowflake PlayerID { get; set; }
		public string? group{ get; set; }
		public UserGroup group_level{ get; set; }
		public long playTime{ get; set; }
		[Ignore] [JsonIgnore] 
		public Status Status { get; set; }

		[Ignore][JsonIgnore] public Player Player;
		public Identifiers Identifiers { get; set; }
		
		public List<Char_data> Characters { get; set; } = new();
		[Ignore][JsonIgnore]
		private Char_data _current;
		public Char_data CurrentChar
		{
			get => _current;
			set => _current = value;
		}

		[Ignore]
		private FreeRoamChar _freeRoamChar;
		public FreeRoamChar FreeRoamChar
		{
			get => _freeRoamChar;
			set => _freeRoamChar = value;

		}
		public List<PlayerScore> PlayerScores { get; set; } = new();


		[Ignore][JsonIgnore]
		internal string char_data
		{
			get => Characters.ToJson(settings: JsonHelper.IgnoreJsonIgnoreAttributes);
			set => Characters = value.FromJson<List<Char_data>>(settings: JsonHelper.IgnoreJsonIgnoreAttributes);
		}
	}

	[Serialization]
	public partial class Identifiers
	{
		public string? Steam { get; set; }
		public string? License{ get; set; }
		public string? Discord{ get; set; }
		public string? Fivem{ get; set; }
		public string? Ip{ get; set; }

		public string?[] ToArray() => new string?[] { Steam, Discord, License, Fivem, Ip };
	}

	public class Status
	{
		private BaseStateBag<bool> _spawned;

		public bool Spawned { get => _spawned.Value; set => _spawned.Value = value; }
		public PlayerStates PlayerStates;
		public RPStates RolePlayStates;
		public InstanceBags Istanza;
		public FreeRoamStates FreeRoamStates;

		public Status(Player player)
		{
			_spawned = new(player, "spawned", true);
			PlayerStates = new(player, "PlayerStates");
			RolePlayStates = new(player, "RolePlayStates");
			FreeRoamStates = new(player, "FreeRoamStates");
			Istanza = new(player, "PlayerInstance");
		}
	}
}