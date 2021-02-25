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
		public static PlayerChar Player;
		public static void AddPlayer(string jsonData) => Player = jsonData.Deserialize<PlayerChar>();
		public static void UpdatePedId() => Player.PlayerPed = new Ped(PlayerPedId());


	}
}
