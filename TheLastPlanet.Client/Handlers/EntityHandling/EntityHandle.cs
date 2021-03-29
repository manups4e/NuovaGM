using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using Logger;
using TheLastPlanet.Client.Core.PlayerChar;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Handlers.Animations;
using TheLastPlanet.Shared;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.Handlers.EntityHandling
{
	public enum TypeSpawn
	{
		Network,
		Local
	}

	public class EntityHandle
	{
		public int Handle { get; set; }
		public Position Position { get; set; }
		public Model Model { get; set; }
		public TypeSpawn TypeSpawn { get; set; }
		public EntityHandle() { }
		public EntityHandle(Position pos, TypeSpawn type) 
		{
			Position = pos;
			TypeSpawn = type;
		}

		public void Delete() { }
	}

	#region Ped
	public class PedHandle : EntityHandle
	{
		public Ped Ped { get; set; }
		public AnimationQueue AnimQueue;
		public PedHandle() { }
		public PedHandle(string model, Position pos, TypeSpawn type) : base(pos, type)
		{
			Model = new Model(model);
		}

		public PedHandle(PedHash model, Position pos, TypeSpawn type) : base(pos, type)
		{
			Model = new Model(model);
		}

		public PedHandle(Model model, Position pos, TypeSpawn type) : base(pos, type)
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

		public VehicleHandle() { }
		public VehicleHandle(string model, Position pos, TypeSpawn type) : base(pos, type)
		{
			Model = new Model(model);
		}

		public VehicleHandle(PedHash model, Position pos, TypeSpawn type) : base(pos, type)
		{
			Model = new Model(model);
		}

		public VehicleHandle(Model model, Position pos, TypeSpawn type) : base(pos, type)
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
		public PropHandle() { }
		public PropHandle(string model, Position pos, TypeSpawn type, bool placeOnGround = true, bool dynamic = true) : base(pos, type)
		{
			Model = new Model(model);
			_placeOnGround = placeOnGround;
			_dynamic = dynamic;
		}

		public PropHandle(PedHash model, Position pos, TypeSpawn type, bool placeOnGround = true, bool dynamic = true) : base(pos, type)
		{
			Model = new Model(model);
			_placeOnGround = placeOnGround;
			_dynamic = dynamic;
		}

		public PropHandle(Model model, Position pos, TypeSpawn type, bool placeOnGround = true, bool dynamic = true) : base(pos, type)
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
				Prop = await Funzioni.SpawnLocalProp(Model.Hash, Position.ToVector3, true, true);
			else
				Prop = await Funzioni.CreateProp(Model.Hash, Position.ToVector3, Vector3.Zero);
		}

	}
	#endregion
}
