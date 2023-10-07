﻿using System;
using System.Linq;
using Newtonsoft.Json;
using TheLastPlanet.Shared.PlayerChar;
using FxEvents.Shared.Snowflakes;
using FxEvents.Shared.EventSubsystem;
#if CLIENT
using TheLastPlanet.Client.Core.PlayerChar;
#elif SERVER
using TheLastPlanet.Server.Core;
using TheLastPlanet.Server.Core.PlayerChar;
using TheLastPlanet.Server;
#endif

namespace TheLastPlanet.Shared
{

    public class PlayerClient : ISource
    {
        public Snowflake Id { get; set; }
        public int Handle { get; set; }
        public User User { get; set; }
        public Identifiers Identifiers => User.Identifiers;

        internal Status Status { get; set; }

        public PlayerClient()
        {
        }

        public override string ToString()
        {
            return $"{(Id != Snowflake.Empty ? Id.ToString() : Handle.ToString())} ({Player.Name})";
        }


        public bool Compare(Identifiers identifier)
        {
            return Identifiers == identifier;
        }

        public static explicit operator PlayerClient(string netId)
        {
            if (int.TryParse(netId.Replace("net:", string.Empty), out int handle))
            {
                return new PlayerClient(handle);
            }

            throw new Exception($"Could not parse net id: {netId}");
        }

        public bool Compare(PlayerClient client)
        {
            return client.Handle == Handle;
        }

#if CLIENT
        
        [JsonIgnore]
        internal Position Posizione { get; set; }
        
        [JsonIgnore]
        internal bool Ready => User != null;
        
        [JsonIgnore]
        internal Player Player { get => new(API.GetPlayerFromServerId(Handle)); }
        private Ped _ped;
        
        [JsonIgnore]
        internal Ped Ped
        {
            get
            {
                var handle = GetPlayerPed(GetPlayerFromServerId(Handle));
                if (_ped is null || _ped.Handle != handle)
                    _ped = new Ped(handle);
                return _ped;
            }
        }

        public PlayerClient(Tuple<Snowflake, BasePlayerShared> value)
        {
            Id = value.Item1;
            Handle = Game.Player.ServerId;
            User = new(value.Item2);
            Status = new(Player);
        }

        public PlayerClient(Snowflake id)
        {
            Player owner = Game.Player;
            if (owner != null)
            {
                Id = id;
                Handle = owner.ServerId;
                LoadUser();
                Status = new(Player);
            }
            else
            {
                throw new Exception($"Could not find runtime client: {id}");
            }
        }

        public PlayerClient(int handle)
        {
            Handle = handle;
            Status = new(Player);
            LoadUser();
        }

        public async void LoadUser()
        {
            var bps = await EventDispatcher.Get<BasePlayerShared>("tlg:GetUserFromServerId", Handle);
            User = new User(bps);
            Id = User != null ? User.PlayerID : Snowflake.Empty;
        }

#elif SERVER

        [JsonIgnore]
        internal Player Player { get => Server.Server.Instance.GetPlayers[Handle]; }


        [JsonIgnore]
        internal Ped Ped { get => Player.Character; }

        public static readonly PlayerClient Global = new(-1);

        public PlayerClient(Snowflake id)
        {
            Player owner = Server.Server.Instance.GetPlayers.FirstOrDefault(x => x.Handle == Handle.ToString());
            if (owner != null)
            {
                Id = id;
                Handle = Convert.ToInt32(owner.Handle);
                User = Server.Server.Instance.Clients.FirstOrDefault(x => x.Handle == Handle)?.User;
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
            User = Server.Server.Instance.Clients.FirstOrDefault(x => x.Handle == Handle)?.User;
            Id = User != null ? User.PlayerID : Snowflake.Empty;
            Status = new(Player);
        }

        public PlayerClient(User user)
        {
            Handle = Convert.ToInt32(user.Player.Handle);
            User = user;
            Id = user.PlayerID;
            Status = new(Player);
        }

        public bool Compare(Player player)
        {
            return Compare(player.GetCurrentChar().Identifiers);
        }
#endif
        public static explicit operator PlayerClient(int handle) => new(handle);
    }

    public class Status
    {
        public PlayerStates PlayerStates { get; set; }
        public RPStates RolePlayStates { get; set; }
        public InstanceBags Instance { get; set; }
        public FreeRoamStates FreeRoamStates { get; set; }

        public Status(Player player)
        {
            PlayerStates = new(player, "PlayerStates");
            RolePlayStates = new(player, "RolePlayStates");
            FreeRoamStates = new(player, "FreeRoamStates");
            Instance = new(player, "PlayerInstance");
        }

        public void Clear()
        {
            PlayerStates.Mode = ServerMode.Lobby;
            PlayerStates.Spawned = false;
            PlayerStates.InVehicle = false;
            PlayerStates.Paused = false;
            PlayerStates.AdminSpectating = false;
            PlayerStates.Wanted = false;
            RolePlayStates.InHome = false;
            RolePlayStates.Fainted = false;
            RolePlayStates.OnDuty = false;
            RolePlayStates.Cuffed = false;
            RolePlayStates.Dying = false;
            Instance.RimuoviIstanza();
        }

        public void Load()
        {
            PlayerStates.Mode = PlayerStates._mode.State;
            PlayerStates.Spawned = PlayerStates._spawned.State;
            PlayerStates.InVehicle = PlayerStates._inVeh.State;
            PlayerStates.Paused = PlayerStates._paused.State;
            PlayerStates.AdminSpectating = PlayerStates._adminSpectating.State;
            PlayerStates.Wanted = PlayerStates._wanted.State;
            RolePlayStates.InHome = RolePlayStates._inHome.State;
            RolePlayStates.Fainted = RolePlayStates._fainted.State;
            RolePlayStates.OnDuty = RolePlayStates._onDuty.State;
            RolePlayStates.Cuffed = RolePlayStates._cuffed.State;
            RolePlayStates.Dying = RolePlayStates._dying.State;
            InstanceBag p = Instance._instanceBag.State;
            Instance.Instanced = p.Instanced;
            Instance.ServerIdOwner = p.ServerIdOwner;
            Instance.IsOwner = p.IsOwner;
            Instance.Instance = p.Instance;
        }
    }
}