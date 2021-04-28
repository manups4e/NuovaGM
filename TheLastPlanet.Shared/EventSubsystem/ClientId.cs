using System;
using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
#if SERVER
using TheLastPlanet.Server.Core;
using TheLastPlanet.Server.Core.PlayerChar;
using TheLastPlanet.Server;
#elif CLIENT
using TheLastPlanet.Client.Core.PlayerChar;
#endif
using TheLastPlanet.Shared.Internal.Events;
using TheLastPlanet.Shared.PlayerChar;
using TheLastPlanet.Shared.Snowflakes;

namespace TheLastPlanet.Shared.Internal.Events
{
    public class ClientId : ISource
    {

        public Snowflake Id { get; set; }
        public int Handle { get; set; }
		[JsonIgnore]
        public Player Player { get; set; }

#if SERVER
        [JsonIgnore]
        public Ped Ped => Player.Character;
        public static readonly ClientId Global = new(-1);
#elif CLIENT
        [JsonIgnore]
        public Ped Ped { get; set; }
        
        [JsonIgnore]
        public Position Posizione => User.Posizione;
        public void UpdatePedId() => Ped = new Ped(API.PlayerPedId());
        public bool Ready => Player != null && Ped != null && User != null;
#endif

        public User User { get; set; }
        public Identifiers Identifiers => User.Identifiers;
        public ClientId() { }
#if CLIENT
        public ClientId(Tuple<Snowflake, User> value)
		{
            Id = value.Item1;
            Handle = Game.Player.ServerId;
            Player = Game.Player;
            Ped = new Ped(API.PlayerPedId());
			User = new(value.Item2);
        }
#endif
        public ClientId(Snowflake id)
        {
#if SERVER
            Player owner = Server.Server.Instance.GetPlayers.FirstOrDefault(x => x.Handle == Handle.ToString());
#elif CLIENT
            Player owner = Game.Player;
#endif
            if (owner != null)
            {
                Id = id;
#if CLIENT
                Handle = owner.ServerId;
#elif SERVER
                Handle = Convert.ToInt32(owner.Handle);
                LoadUser();
#endif
            }
            else
            {
                throw new Exception($"Could not find runtime client: {id}");
            }
        }

#if SERVER
        public ClientId(int handle)
        {
            Handle = handle;
            Player = Server.Server.Instance.GetPlayers.FirstOrDefault(x => x.Handle == Handle.ToString());
            if (handle > 0)
                LoadUser();
            Id = User != null ? User.PlayerID : Snowflake.Empty;
        }
#endif

#if SERVER
        public ClientId(User user)
        {
            Handle = Convert.ToInt32(user.Player.Handle);

            Player = user.Player;
            User = user;
            Id = user.PlayerID;
        }
#endif

        public ClientId(Snowflake id, int handle, string[] identifiers)
        {
            Id = id;
            Handle = handle;
            LoadUser();
        }

        public override string ToString()
        {
            return $"{(Id != Snowflake.Empty ? Id.ToString() : Handle.ToString())} ({Player.Name})";
        }

#if SERVER
        public bool Compare(Identifiers identifier)
        {
            return Identifiers == identifier;
        }

        public bool Compare(Player player)
        {
            return Compare(player.GetCurrentChar().Identifiers);
        }

        public static explicit operator ClientId(string netId)
        {
            if (int.TryParse(netId.Replace("net:", string.Empty), out int handle))
            {
                return new ClientId(handle);
            }

            throw new Exception($"Could not parse net id: {netId}");
        }
#endif

        public bool Compare(ClientId client)
        {
            return client.Handle == Handle;
        }

        public async void LoadUser()
		{
            ClientId res;
#if SERVER
             res = Server.Server.Instance.Clients.FirstOrDefault(x => x.Handle == Handle);
#elif CLIENT
            res = Client.Client.Instance.Clients.FirstOrDefault(x => x.Handle == Handle);
#endif
            if (res != null)
                User = res.User;
#if SERVER
            else
            {
	            Snowflake newone = SnowflakeGenerator.Instance.Next();
                const string procedure = "call IngressoPlayer(@disc, @lice, @name, @snow)";
                User = new(Player, await MySQL.QuerySingleAsync<BasePlayerShared>(procedure, new
                {
                    disc = Convert.ToInt64(Player.GetLicense(Identifier.Discord)),
                    lice = Player.GetLicense(Identifier.License),
                    name = Player.Name,
                    snow = newone.ToInt64()
                }));
            }
#endif
        }

#if SERVER
        public static explicit operator ClientId(int handle) => new(handle);
#endif

    }
}