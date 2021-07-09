using System.Threading.Tasks;
using CitizenFX.Core;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Handlers.Animations;
using TheLastPlanet.Shared;

namespace TheLastPlanet.Client.Handlers.EntityHandling
{
	public enum TypeSpawn
	{
		Network,
		Local
	}
	
	#region Entity
	public abstract class EntityHandle
	{
		public int Handle { get; set; }
		public Position Position { get; set; }
		public Model Model { get; set; }
		public TypeSpawn TypeSpawn { get; set; }
		public ModalitaServer ModalitaSpawn { get; set; }

		protected EntityHandle()
		{
		}

		protected EntityHandle(Position pos, TypeSpawn type, ModalitaServer modalita)
		{
			Position = pos;
			TypeSpawn = type;
			ModalitaSpawn = modalita;
		}

		protected virtual void Delete()
		{
		}
	}
#endregion
	
	#region Ped

	public class PedHandle : EntityHandle
	{
		public Ped Ped { get; set; }
		public AnimationQueue AnimQueue;

		public PedHandle()
		{
		}

		public PedHandle(string model, Position pos, TypeSpawn type, ModalitaServer modalita) : base(pos, type, modalita)
		{
			Model = new Model(model);
		}

		public PedHandle(PedHash model, Position pos, TypeSpawn type, ModalitaServer modalita) : base(pos, type, modalita)
		{
			Model = new Model(model);
		}

		public PedHandle(Model model, Position pos, TypeSpawn type, ModalitaServer modalita) : base(pos, type, modalita)
		{
			Model = model;
		}

		public async Task<Ped> SpawnResult()
		{
			Ped result;
			if (TypeSpawn == TypeSpawn.Local)
				result = await Funzioni.CreatePedLocally(Model.Hash, Position.ToVector3, Position.Heading);
			else
				result = await Funzioni.SpawnPed(Model.Hash, Position);

			return result;
		}

		public async Task Spawn()
		{
			if (TypeSpawn == TypeSpawn.Local)
				Ped = await Funzioni.CreatePedLocally(Model.Hash, Position.ToVector3, Position.Heading);
			else
				Ped = await Funzioni.SpawnPed(Model.Hash, Position);
			AnimQueue = new AnimationQueue(Ped.Handle);
		}
	}

	#endregion

	#region Vehicle

	public class VehicleHandle : EntityHandle
	{
		public Vehicle Vehicle { get; set; }

		public VehicleHandle()
		{
		}

		public VehicleHandle(string model, Position pos, TypeSpawn type, ModalitaServer modalita) : base(pos, type, modalita)
		{
			Model = new Model(model);
		}

		public VehicleHandle(PedHash model, Position pos, TypeSpawn type, ModalitaServer modalita) : base(pos, type, modalita)
		{
			Model = new Model(model);
		}

		public VehicleHandle(Model model, Position pos, TypeSpawn type, ModalitaServer modalita) : base(pos, type, modalita)
		{
			Model = model;
		}

		public async Task<Vehicle> SpawnResult()
		{
			Vehicle result;
			if (TypeSpawn == TypeSpawn.Local)
				result = await Funzioni.SpawnLocalVehicle(Model.Hash, Position.ToVector3, Position.Heading);
			else
				result = await Funzioni.SpawnVehicleNoPlayerInside(Model.Hash, Position.ToVector3, Position.Heading);

			return result;
		}

		public async Task Spawn()
		{
			if (TypeSpawn == TypeSpawn.Local)
				Vehicle = await Funzioni.SpawnLocalVehicle(Model.Hash, Position.ToVector3, Position.Heading);
			else
				Vehicle = await Funzioni.SpawnVehicleNoPlayerInside(Model.Hash, Position.ToVector3, Position.Heading);
		}
	}

	#endregion

	#region Prop

	public class PropHandle : EntityHandle
	{
		public Prop Prop { get; set; }
		private bool _placeOnGround;
		private bool _dynamic;

		public PropHandle()
		{
		}

		public PropHandle(string model, Position pos, TypeSpawn type, ModalitaServer modalita, bool placeOnGround = true, bool dynamic = true) : base(pos, type, modalita)
		{
			Model = new Model(model);
			_placeOnGround = placeOnGround;
			_dynamic = dynamic;
		}

		public PropHandle(PedHash model, Position pos, TypeSpawn type, ModalitaServer modalita, bool placeOnGround = true, bool dynamic = true) : base(pos, type, modalita)
		{
			Model = new Model(model);
			_placeOnGround = placeOnGround;
			_dynamic = dynamic;
		}

		public PropHandle(Model model, Position pos, TypeSpawn type, ModalitaServer modalita, bool placeOnGround = true, bool dynamic = true) : base(pos, type, modalita)
		{
			Model = model;
			_placeOnGround = placeOnGround;
			_dynamic = dynamic;
		}

		public async Task<Prop> SpawnResult()
		{
			Prop result;
			if (TypeSpawn == TypeSpawn.Local)
				result = await Funzioni.SpawnLocalProp(Model.Hash, Position.ToVector3, _dynamic, _placeOnGround);
			else
				result = await Funzioni.CreateProp(Model.Hash, Position.ToVector3, Vector3.Zero);

			return result;
		}

		public async Task Spawn()
		{
			if (TypeSpawn == TypeSpawn.Local)
				Prop = await Funzioni.SpawnLocalProp(Model.Hash, Position.ToVector3, _dynamic, _placeOnGround);
			else
				Prop = await Funzioni.CreateProp(Model.Hash, Position.ToVector3, Vector3.Zero);
		}
	}

	#endregion
}