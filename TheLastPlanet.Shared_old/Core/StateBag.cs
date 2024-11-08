﻿using System;
using CitizenFX.Core;
using Impostazioni.Shared.Core;
using Logger;
using TheLastPlanet.Shared.Internal.Events.Attributes;

namespace TheLastPlanet.Shared
{

    public class BaseStateBag<T>
    {
        private Player _player;
        private Entity _entity;

        public string Name { get; set; }
        public bool Replicated { get; set; } = true;
        public T Value
        {
            get => _player != null ? _player.GetState<T>(Name) : _entity.GetState<T>(Name);
            set
            {
                if (_player != null)
                    _player.SetState(Name, value, true);
                else
                    _entity.SetState(Name, value, Replicated);
            }
        }

        public BaseStateBag(Player player, string name, bool replicated = true)
        {
            _player = player;
            Name = name;
            Replicated = replicated;
            Value = default;
        }
        public BaseStateBag(Entity entity, string name, bool replicated = true)
        {
            _entity = entity;
            Name = name;
            Replicated = replicated;
            Value = default;
        }
    }

    public class BaseBag
    {
        [Ignore] internal Player player;

        internal string _name;

        public BaseBag(Player pl, string name)
        {
            player = pl;
            _name = name;
        }
        public BaseBag() { }
    }

    public class PlayerStates : BaseBag
    {
        private readonly BaseStateBag<bool> _adminSpecta;
        private readonly BaseStateBag<bool> _inPausa;
        private readonly BaseStateBag<ModalitaServer> _modalita;
        private readonly BaseStateBag<bool> _wanted;

        public bool Wanted
        {
            get => _wanted.Value;
            set => _wanted.Value = value;
        }

        public bool AdminSpecta
        {
            get => _adminSpecta.Value;
            set => _adminSpecta.Value = value;
        }
        public bool InPausa
        {
            get => _inPausa.Value;
            set => _inPausa.Value = value;
        }

        public ModalitaServer Modalita
        {
            get => _modalita.Value;
            set
            {
#if SERVER
                _modalita.Value = value;
#elif CLIENT
                Client.Client.Instance.Events.Send("tlg:addPlayerToBucket", value);
#endif
            }
        }
        public PlayerStates() { }
        public PlayerStates(Player player, string name) : base(player, name)
        {
            _adminSpecta = new BaseStateBag<bool>(player, _name + ":AdminSpecta", true);
            _inPausa = new BaseStateBag<bool>(player, _name + ":InPausa", true);
            _modalita = new BaseStateBag<ModalitaServer>(player, _name + ":Modalita", true);
            _wanted = new BaseStateBag<bool>(player, _name + ":WantedAttivo", true);
        }
    }

    public class RPStates : BaseBag
    {
        private readonly BaseStateBag<bool> _svenuto;
        private readonly BaseStateBag<bool> _ammanettato;
        private readonly BaseStateBag<bool> _inCasa;
        private readonly BaseStateBag<bool> _inServizio;
        private readonly BaseStateBag<bool> _finDiVita;
        private readonly BaseStateBag<bool> _inVeicolo;

        private bool svenuto;
        private bool ammanettato;
        private bool inCasa;
        private bool inServizio;
        private bool finDiVita;
        private bool inVeicolo;

        public bool Svenuto
        {
            get => svenuto;
            set => svenuto = value;
        }

        public bool Ammanettato
        {
            get => ammanettato;
            set => ammanettato = value;
        }

        public bool InCasa
        {
            get => inCasa;
            set => inCasa = value;
        }

        public bool InServizio
        {
            get => inServizio;
            set => inServizio = value;
        }

        public bool FinDiVita
        {
            get => finDiVita;
            set => finDiVita = value;
        }

        public bool InVeicolo
        {
            get => inVeicolo;
            set => inVeicolo = value;
        }
        
        public RPStates() { }

        public RPStates(Player player, string name) : base(player, name)
        {
            _svenuto = new BaseStateBag<bool>(player, _name + ":Svenuto", true);
            _ammanettato = new BaseStateBag<bool>(player, _name + ":Ammanettato", true);
            _inCasa = new BaseStateBag<bool>(player, _name + ":InCasa", true);
            _inServizio = new BaseStateBag<bool>(player, _name + ":InServizio", true);
            _finDiVita = new BaseStateBag<bool>(player, _name + ":FinDiVita", true);
            _inVeicolo = new BaseStateBag<bool>(player, _name + ":InVeicolo", true);

            Svenuto = false;
            Ammanettato = false;
            InCasa = false;
            InServizio = false;
            FinDiVita = false;
            InVeicolo = false;
        }
    }

    public class InstanceBags : BaseBag
    {
        private readonly BaseStateBag<InstanceBag> _instanceBag;
        public InstanceBags() { }
        public InstanceBags(Player player, string name) : base(player, name)
        {
            _instanceBag = new BaseStateBag<InstanceBag>(player, _name);
            RimuoviIstanza();
        }

        public bool Stanziato { get; set; }
        public int ServerIdProprietario { get; set; }
        public bool IsProprietario { get; set; }
        public string? Instance { get; set; }

        /// <summary>
        /// Istanza generica
        /// </summary>
        public void Istanzia()
        {
            Stanziato = true;
#if CLIENT
            ServerIdProprietario = Game.Player.ServerId;
#elif SERVER
			ServerIdProprietario = Convert.ToInt32(player.Handle);
#endif
            IsProprietario = true;
            Instance = string.Empty;
            var bag = new InstanceBag(Stanziato, ServerIdProprietario, IsProprietario, Instance);
            _instanceBag.Value = bag;
        }

        /// <summary>
        /// Istanza generica specificando quale Istanza
        /// </summary>
        public void Istanzia(string instance)
        {
            Stanziato = true;
#if CLIENT
            ServerIdProprietario = Game.Player.ServerId;
#elif SERVER
			ServerIdProprietario = Convert.ToInt32(player.Handle);
#endif
            IsProprietario = true;
            this.Instance = instance;
            var bag = new InstanceBag(Stanziato, ServerIdProprietario, IsProprietario, Instance);
            _instanceBag.Value = bag;
        }

        /// <summary>
        /// Istanza specifica
        /// </summary>
        public void Istanzia(int ServerId, string instance)
        {
            Stanziato = true;
            ServerIdProprietario = ServerId;
            IsProprietario = true;
            this.Instance = instance;
            var bag = new InstanceBag(Stanziato, ServerIdProprietario, IsProprietario, Instance);
            _instanceBag.Value = bag;
        }

        /// <summary>
        /// Rimuovi da istanza
        /// </summary>
        public void RimuoviIstanza()
        {
            Stanziato = false;
            ServerIdProprietario = 0;
            IsProprietario = false;
            Instance = string.Empty;
            var bag = new InstanceBag(Stanziato, ServerIdProprietario, IsProprietario, Instance);
            _instanceBag.Value = bag;
        }

        /// <summary>
        /// Cambia Istanza con una nuova (es. casa e garage)
        /// </summary>
        /// <param name="instance">Specifica quale istanza</param>
        public void CambiaIstanza(string instance)
        {
            if (Stanziato)
            {
                if (Instance != instance)
                {
                    Instance = instance;
                    var bag = new InstanceBag(Stanziato, ServerIdProprietario, IsProprietario, Instance);
                    _instanceBag.Value = bag;
                }
            }
        }

        /// <summary>
        /// Cambia Proprietario dell'istanza
        /// </summary>
        /// <param name="netId">networkId del proprietario</param>
        public void CambiaIstanza(int netId)
        {
            if (Stanziato)
            {
                if (ServerIdProprietario != netId)
                {
                    ServerIdProprietario = netId; var bag = new InstanceBag(Stanziato, ServerIdProprietario, IsProprietario, Instance);
                    _instanceBag.Value = bag;
                }
            }

        }
    }


    [Serialization]
    public partial class InstanceBag
    {
        public bool Stanziato { get; set; }
        public int ServerIdProprietario { get; set; }
        public bool IsProprietario { get; set; }
        public string? Instance { get; set; }

        public InstanceBag(bool stanziato, int serverId, bool proprietario, string instance)
        {
            Stanziato = stanziato;
            ServerIdProprietario = serverId;
            IsProprietario = proprietario;
            Instance = instance;
        }
    }
}