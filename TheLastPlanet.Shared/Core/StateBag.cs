using CitizenFX.Core;
using System;
using FxEvents.Shared.Attributes;

namespace TheLastPlanet.Shared
{
    public class EntityStateBag<T>
    {
        private readonly Entity _entity;
        public string Name { get; set; }
        public bool Replicated { get; set; } = true;
        internal T _value;
        public T State
        {
            get
            {
                _value = _entity.GetState<T>(Name);
                if (_value == null) return default;
                return _value;
            }
            set
            {
                _entity.SetState(Name, value, Replicated);
                _value = value;
            }
        }

        public EntityStateBag(Entity entity, string name, bool replicated = true)
        {
            _entity = entity;
            Name = name;
            Replicated = replicated;
        }
    }

    public class PlayerStateBag<T>
    {
        private Player _player;

        public string Name { get; set; }
        public bool Replicated { get; set; } = true;
        internal T _value;
        public T State
        {
            get
            {
                _value = _player.GetState<T>(Name);
                if (_value == null) return default;
                return _value;
            }
            set
            {
                _player.SetState(Name, value, true);
                _value = value;
            }
        }

        public PlayerStateBag(Player player, string name, bool replicated = true)
        {
            _player = player;
            Name = name;
            Replicated = replicated;
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
        internal readonly PlayerStateBag<bool> _spawned;
        internal readonly PlayerStateBag<bool> _adminSpecta;
        internal readonly PlayerStateBag<bool> _inPausa;
        internal readonly PlayerStateBag<ModalitaServer> _modalita;
        internal readonly PlayerStateBag<bool> _wanted;
        internal readonly PlayerStateBag<bool> _inVeicolo;
        internal readonly PlayerStateBag<bool> _passive;

        public bool Spawned
        {
            get => _spawned.State;
            set => _spawned.State = value;
        }
        public bool InVeicolo
        {
            get => _inVeicolo.State;
            set => _inVeicolo.State = value;
        }

        public bool Wanted
        {
            get => _wanted.State;
            set => _wanted.State = value;
        }

        public bool AdminSpecta
        {
            get => _adminSpecta.State;
            set => _adminSpecta.State = value;
        }
        public bool InPausa
        {
            get => _inPausa.State;
            set => _inPausa.State = value;
        }

        public bool ModalitaPassiva
        {
            get => _passive.State;
            set => _passive.State = value;
        }

        public ModalitaServer Modalita
        {
            get => _modalita.State;
            set
            {
#if SERVER
                _modalita.State = value;
#elif CLIENT
                EventDispatcher.Send("tlg:addPlayerToBucket", value);
#endif
            }
        }

        public PlayerStates() { }

        public PlayerStates(Player player, string name) : base(player, name)
        {
            _spawned = new(player, _name + ":Spawned", true);
            _adminSpecta = new(player, _name + ":AdminSpecta", true);
            _inPausa = new(player, _name + ":InPausa", true);
            _modalita = new(player, _name + ":Modalita", true);
            _wanted = new(player, _name + ":WantedAttivo", true);
            _inVeicolo = new(player, _name + ":InVeicolo", true);
            _passive = new(player, _name + ":ModPassiva", true);
        }
    }

    public class FreeRoamStates : BaseBag
    {
        public FreeRoamStates() {
        }
        public FreeRoamStates(Player player, string name) : base(player, name)
        {
        }
    }

    public class RPStates : BaseBag
    {
        internal readonly PlayerStateBag<bool> _svenuto;
        internal readonly PlayerStateBag<bool> _ammanettato;
        internal readonly PlayerStateBag<bool> _inCasa;
        internal readonly PlayerStateBag<bool> _inServizio;
        internal readonly PlayerStateBag<bool> _finDiVita;

        public bool Svenuto
        {
            get => _svenuto.State;
            set => _svenuto.State = value;
        }

        public bool Ammanettato
        {
            get => _ammanettato.State;
            set => _ammanettato.State = value;
        }

        public bool InCasa
        {
            get => _inCasa.State;
            set => _inCasa.State = value;
        }

        public bool InServizio
        {
            get => _inServizio.State;
            set => _inServizio.State = value;
        }

        public bool FinDiVita
        {
            get => _finDiVita.State;
            set => _finDiVita.State = value;
        }

        public RPStates() {
        }

        public RPStates(Player player, string name) : base(player, name)
        {
            _svenuto = new (player, _name + ":Svenuto", true);
            _ammanettato = new (player, _name + ":Ammanettato", true);
            _inCasa = new (player, _name + ":InCasa", true);
            _inServizio = new (player, _name + ":InServizio", true);
            _finDiVita = new (player, _name + ":FinDiVita", true);
        }
    }

    public class InstanceBags : BaseBag
    {
        internal readonly PlayerStateBag<InstanceBag> _instanceBag;
        public InstanceBags() { }
        public InstanceBags(Player player, string name) : base(player, name)
        {
            _instanceBag = new PlayerStateBag<InstanceBag>(player, _name);
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
            _instanceBag.State = bag;
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
            _instanceBag.State = bag;
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
            _instanceBag.State = bag;
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
            _instanceBag.State = bag;
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
                    _instanceBag.State = bag;
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
                    _instanceBag.State = bag;
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