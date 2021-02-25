using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using TheLastPlanet.Client.Core.Personaggio;
using TheLastPlanet.Shared;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client.Core
{
	public static class Cache
	{

		private static Ped _ped;
		private static Player _player;

		public static Ped PlayerPed
		{
			get => _ped;
			set => _ped = value;
		}
		public static Player Player
		{
			get => _player;
			set => _player = value;
		}

		public static PlayerChar Char;
		public static void AddPlayer(string jsonData) => Char = jsonData.Deserialize<PlayerChar>();
		public static void UpdatePedId() => PlayerPed = new Ped(PlayerPedId());
		public static void UpdatePlayerId() => Player = Game.Player;


	}
}
