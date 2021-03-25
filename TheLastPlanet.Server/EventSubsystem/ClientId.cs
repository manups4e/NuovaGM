using System;
using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using TheLastPlanet.Server.Core;
using TheLastPlanet.Server.Core.PlayerChar;
using TheLastPlanet.Shared.Internal.Events;
using TheLastPlanet.Shared.Snowflakes;

namespace TheLastPlanet.Server.Internal.Events
{
    public class ClientId : ISource
    {
        public static readonly ClientId Global = new(-1);

        public Snowflake Id { get; set; }
        public int Handle { get; set; }
        public string[] Identifiers { get; set; }
        public Player Player => Server.PlayerList.Count > 0 ?
            Server.PlayerList.FirstOrDefault(x => x.Key == Handle.ToString()).Value.Player :
            Server.Instance.GetPlayers.FirstOrDefault(x => x.Handle == Handle.ToString());

        public User User => Server.PlayerList.Count > 0 ? 
            Server.PlayerList.FirstOrDefault(x => x.Key == Handle.ToString()).Value :
            Server.Instance.GetPlayers.FirstOrDefault(x => x.Handle == Handle.ToString()).GetCurrentChar();

        
        public ClientId(Snowflake id)
        {
            Player owner = Server.PlayerList.Count > 0 ?
            Server.PlayerList.FirstOrDefault(x => x.Key == Handle.ToString()).Value.Player :
            Server.Instance.GetPlayers.FirstOrDefault(x => x.Handle == Handle.ToString());

            if (owner != null)
            {
                Id = id;
                Handle = Convert.ToInt32(owner.Handle);
                Identifiers = owner.Identifiers.ToArray();
            }
            else
            {
                throw new Exception($"Could not find runtime client: {id}");
            }
        }

        public ClientId(int handle)
        {
            Handle = handle;

            var holder = new List<string>();

            for (var index = 0; index < API.GetNumPlayerIdentifiers(handle.ToString()); index++)
            {
                holder.Add(API.GetPlayerIdentifier(handle.ToString(), index));
            }

            Id = Funzioni.GetUserFromPlayerId(handle) != null? Funzioni.GetUserFromPlayerId(handle).PlayerID : Snowflake.Empty;
            Identifiers = holder.ToArray();
        }

        public ClientId(Snowflake id, int handle, string[] identifiers)
        {
            Id = id;
            Handle = handle;
            Identifiers = identifiers;
        }

        public override string ToString()
        {
            return $"{(Id != Snowflake.Empty ? Id.ToString() : Handle.ToString())} ({API.GetPlayerName(Handle.ToString())})";
        }

        public bool Compare(string[] identifiers)
        {
            return identifiers.Any(self => Identifiers.Contains(self));
        }

        public bool Compare(Player player)
        {
            return Compare(player.GetCurrentChar().Identifiers.ToArray());
        }

        public bool Compare(ClientId client)
        {
            return client.Handle == Handle;
        }

        public static explicit operator ClientId(string netId)
        {
            if (int.TryParse(netId.Replace("net:", string.Empty), out int handle))
            {
                return new ClientId(handle);
            }

            throw new Exception($"Could not parse net id: {netId}");
        }

        public static explicit operator ClientId(int handle) => new(handle);
    }
}