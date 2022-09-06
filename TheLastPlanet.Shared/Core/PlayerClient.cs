using System;
using System.Linq;
using Newtonsoft.Json;
#if SERVER
using TheLastPlanet.Server.Core;
using TheLastPlanet.Server.Core.PlayerChar;
using TheLastPlanet.Server;
#elif CLIENT
using TheLastPlanet.Client.Core.PlayerChar;
#endif
using TheLastPlanet.Shared.PlayerChar;
using FxEvents.Shared.Snowflakes;
using FxEvents.Shared.Attributes;
using FxEvents.Shared.EventSubsystem;

namespace TheLastPlanet.Shared
{
    [Serialization]
    public partial class PlayerClient : ISource
    {
        public Snowflake Id { get; set; }
        public int Handle { get; set; }
        public User User { get; set; }
        public Identifiers Identifiers => User.Identifiers;

        //[Ignore]
        //public ClientStateBags ClientStateBags { get; set; }
#if CLIENT
        [Ignore]
        [JsonIgnore]
        public Position Posizione { get; set; }
        [Ignore]
        [JsonIgnore]
        public bool Ready => User != null;
        [Ignore]
        [JsonIgnore]
        public Player Player { get => new(API.GetPlayerFromServerId(Handle)); }
        private Ped _ped;
        [Ignore]
        [JsonIgnore]
        public Ped Ped
        {
            get
            {
                var handle = GetPlayerPed(GetPlayerFromServerId(Handle));
                if (_ped is null || _ped.Handle != handle)
                    _ped = new Ped(handle);
                return _ped;
            }
        }

#elif SERVER
        [Ignore]
        [JsonIgnore]
        public Player Player { get => Server.Server.Instance.GetPlayers[Handle]; }

        [Ignore]
        [JsonIgnore]
        public Ped Ped { get => Player.Character; }

        public static readonly PlayerClient Global = new(-1);
#endif
        [Ignore]
        public Status Status { get; set; }

        public PlayerClient()
        {
        }

#if CLIENT
        public PlayerClient(Tuple<Snowflake, BasePlayerShared> value)
        {
            Id = value.Item1;
            Handle = Game.Player.ServerId;
            User = new(value.Item2);
            //ClientStateBags = new ClientStateBags(Player);
            Status = new(Player);
        }
#endif
        public PlayerClient(Snowflake id)
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
                Status = new(Player);
                //ClientStateBags = new(Player);
            }
            else
            {
                throw new Exception($"Could not find runtime client: {id}");
            }
        }

        public PlayerClient(int handle)
        {
            Handle = handle;
            //Player = Server.Server.Instance.GetPlayers.FirstOrDefault(x => x.Handle == Handle.ToString());
            if (handle > 0) LoadUser();
            Id = User != null ? User.PlayerID : Snowflake.Empty;
            //ClientStateBags = new(Player);
            Status = new(Player);
        }

#if SERVER
        public PlayerClient(User user)
        {
            Handle = Convert.ToInt32(user.Player.Handle);
            //Player = user.Player;
            User = user;
            Id = user.PlayerID;
            //ClientStateBags = new(Player);
            Status = new(Player);
        }
#endif

        public PlayerClient(Snowflake id, int handle, string[] identifiers)
        {
            Id = id;
            Handle = handle;
            LoadUser();
            //ClientStateBags = new(Player);
            Status = new(Player);
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

        public static explicit operator PlayerClient(string netId)
        {
            if (int.TryParse(netId.Replace("net:", string.Empty), out int handle))
            {
                return new PlayerClient(handle);
            }

            throw new Exception($"Could not parse net id: {netId}");
        }
#endif
        public bool Compare(PlayerClient client)
        {
            return client.Handle == Handle;
        }

        public void LoadUser()
        {
            PlayerClient res;
#if SERVER
            res = Server.Server.Instance.Clients.FirstOrDefault(x => x.Handle == Handle);
            if (res != null) User = res.User;
#elif CLIENT
            res = Client.Client.Instance.Clients.FirstOrDefault(x => x.Handle == Handle);
            User = new();
#endif
            /*
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
            */
        }

#if SERVER
        public static explicit operator PlayerClient(int handle) => new(handle);
#endif

    }

    public class Status
    {
        public PlayerStates PlayerStates { get; set; }
        public RPStates RolePlayStates { get; set; }
        public InstanceBags Istanza { get; set; }
        public FreeRoamStates FreeRoamStates { get; set; }

        public Status(Player player)
        {
            PlayerStates = new(player, "PlayerStates");
            RolePlayStates = new(player, "RolePlayStates");
            FreeRoamStates = new(player, "FreeRoamStates");
            Istanza = new(player, "PlayerInstance");
        }

        public void Clear()
        {
            PlayerStates.Modalita = ModalitaServer.Lobby;
            PlayerStates.Spawned = false;
            PlayerStates.InVeicolo = false;
            PlayerStates.InPausa = false;
            PlayerStates.AdminSpecta = false;
            PlayerStates.Wanted = false;
            RolePlayStates.InCasa = false;
            RolePlayStates.Svenuto = false;
            RolePlayStates.InServizio = false;
            RolePlayStates.Ammanettato = false;
            RolePlayStates.FinDiVita = false;
            Istanza.RimuoviIstanza();
        }

        public void Load()
        {
            PlayerStates.Modalita = PlayerStates._modalita.State;
            PlayerStates.Spawned = PlayerStates._spawned.State;
            PlayerStates.InVeicolo = PlayerStates._inVeicolo.State;
            PlayerStates.InPausa = PlayerStates._inPausa.State;
            PlayerStates.AdminSpecta = PlayerStates._adminSpecta.State;
            PlayerStates.Wanted = PlayerStates._wanted.State;
            RolePlayStates.InCasa = RolePlayStates._inCasa.State;
            RolePlayStates.Svenuto = RolePlayStates._svenuto.State;
            RolePlayStates.InServizio = RolePlayStates._inServizio.State;
            RolePlayStates.Ammanettato = RolePlayStates._ammanettato.State;
            RolePlayStates.FinDiVita = RolePlayStates._finDiVita.State;
            var p = Istanza._instanceBag.State;
            Istanza.Stanziato = p.Stanziato;
            Istanza.ServerIdProprietario = p.ServerIdProprietario;
            Istanza.IsProprietario = p.IsProprietario ;
            Istanza.Instance = p.Instance ;
        }
    }
}