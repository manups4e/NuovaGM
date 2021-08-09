using System;
using CitizenFX.Core;
using Logger;
using TheLastPlanet.Shared.Internal.Events.Attributes;

namespace TheLastPlanet.Shared
{

    public class BaseStateBag<T>
    {
        private Player _player;
        private Entity _entity;

        public string Name { get; set; }
        public bool Replicated { get; set; }
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

        public BaseStateBag(Player player, string name, bool replicated)
        {
            _player = player;
            Name = name;
            Replicated = replicated;
            Value = default;
        }
        public BaseStateBag(Entity entity, string name, bool replicated)
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
        private BaseStateBag<bool> _adminSpecta;
        private BaseStateBag<bool> _inPausa;
        private BaseStateBag<ModalitaServer> _modalita;
        
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
            _adminSpecta = new BaseStateBag<bool>(player, _name+":AdminSpecta", true);
            _inPausa = new BaseStateBag<bool>(player, _name+":InPausa", true);
            _modalita = new BaseStateBag<ModalitaServer>(player, _name+":Modalita", true);
            AdminSpecta = false;
            InPausa = false;
        }
    }

    public class RPStates : BaseBag
    {
        private BaseStateBag<bool> _svenuto;
        private BaseStateBag<bool> _ammanettato;
        private BaseStateBag<bool> _inCasa;
        private BaseStateBag<bool> _inServizio;
        private BaseStateBag<bool> _finDiVita;
        private BaseStateBag<bool> _inVeicolo;

        public bool Svenuto
        {
            get => _svenuto.Value;
            set => _svenuto.Value = value;
        }

        public bool Ammanettato
        {
            get => _ammanettato.Value;
            set => _ammanettato.Value = value;
        }

        public bool InCasa
        {
            get => _inCasa.Value;
            set => _inCasa.Value = value;
        }

        public bool InServizio
        {
            get => _inServizio.Value;
            set => _inServizio.Value = value;
        }

        public bool FinDiVita
        {
            get => _finDiVita.Value;
            set => _finDiVita.Value = value;
        }
        
        public bool InVeicolo
        {
            get => _inVeicolo.Value;
            set => _inVeicolo.Value = value;
        }

        public RPStates() { }

        public RPStates(Player player, string name) : base(player, name)
        {
            _svenuto = new BaseStateBag<bool>(player, _name+":Svenuto", true);
            _ammanettato = new BaseStateBag<bool>(player, _name+":Ammanettato", true);
            _inCasa = new BaseStateBag<bool>(player, _name+":InCasa", true);
            _inServizio = new BaseStateBag<bool>(player, _name+":InServizio", true);
            _finDiVita = new BaseStateBag<bool>(player, _name+":FinDiVita", true);
            _inVeicolo = new BaseStateBag<bool>(player, _name+":InVeicolo", true);

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
	    private BaseStateBag<bool> _stanziato;
	    private BaseStateBag<int> _serverIdProprietario;
	    private BaseStateBag<bool> _isOwner;
	    private BaseStateBag<string> _instance;
        public InstanceBags() { }
        public InstanceBags(Player player, string name) : base(player, name)
        {
	        _stanziato = new BaseStateBag<bool>(player, _name+":Stanziato", true);
	        _serverIdProprietario = new BaseStateBag<int>(player, _name+":ServerIdProprietario", true);
	        _isOwner = new BaseStateBag<bool>(player, _name+":IsOwner", true);
	        _instance = new BaseStateBag<string>(player, _name+":Istanza", true);
        }

     	public bool Stanziato
        {
	        get => _stanziato.Value;
	        set => _stanziato.Value = value;
        }
		public int ServerIdProprietario
		{
			get => _serverIdProprietario.Value;
			set => _serverIdProprietario.Value = value;
		}
		public bool IsProprietario
		{
			get => _isOwner.Value;
			set => _isOwner.Value = value;
		}

		public string Instance
		{
			get => _instance.Value;
			set => _instance.Value = value;
		}

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
		}

		/// <summary>
		/// Cambia Istanza con una nuova (es. casa e garage)
		/// </summary>
		/// <param name="instance">Specifica quale istanza</param>
		public void CambiaIstanza(string instance)
		{
			if (Stanziato)
				if (Instance != instance)
					Instance = instance;
		}

		/// <summary>
		/// Cambia Proprietario dell'istanza
		/// </summary>
		/// <param name="netId">networkId del proprietario</param>
		public void CambiaIstanza(int netId)
		{
			if (Stanziato)
				if (ServerIdProprietario != netId)
					ServerIdProprietario = netId;
		}
    }
}