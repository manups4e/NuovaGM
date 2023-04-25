using System.Collections.Generic;
using CitizenFX.Core;
using Newtonsoft.Json;
using FxEvents.Shared.Attributes;
using FxEvents.Shared.Snowflakes;

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
        public int ID { get; set; }

        [Ignore]
        [JsonIgnore]
        private ulong UserID
        {
            set => PlayerID = Snowflake.Parse(value);
        }

        public string? Name { get; set; }
        public Snowflake PlayerID { get; set; }
        public string? group { get; set; }
        public UserGroup group_level { get; set; }
        public long playTime { get; set; }

        [Ignore] [JsonIgnore] internal Player Player;
        public Identifiers Identifiers { get; set; }

        public List<Char_data> Characters { get; set; } = new();

        private Char_data _current;
        public Char_data CurrentChar
        {
            get => _current;
            set => _current = value;
        }

        private FreeRoamChar _freeRoamChar;
        public FreeRoamChar FreeRoamChar
        {
            get => _freeRoamChar;
            set => _freeRoamChar = value;

        }
        public List<PlayerScore> PlayerScores { get; set; } = new();


        [Ignore]
        [JsonIgnore]
        internal string char_data
        {
            get => Characters.ToJson(settings: JsonHelper.IgnoreJsonIgnoreAttributes);
            set => Characters = value.FromJson<List<Char_data>>(settings: JsonHelper.IgnoreJsonIgnoreAttributes);
        }
        public BasePlayerShared() { }
    }

    
    public class Identifiers
    {
        public string? Steam { get; set; }
        public string? License { get; set; }
        public string? Discord { get; set; }
        public string? Fivem { get; set; }
        public string? Ip { get; set; }

        public string?[] ToArray() => new string?[] { Steam, Discord, License, Fivem, Ip };
    }
}