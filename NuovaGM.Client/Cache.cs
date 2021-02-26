using CitizenFX.Core;
using TheLastPlanet.Client.Core.Personaggio;
using TheLastPlanet.Shared;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client
{
	public static class Cache
	{
		public static PlayerChar Char { get; private set; }
		public static Ped PlayerPed { get; private set; }
		public static Player Player { get; set; }

		static Cache()
		{
			UpdatePedId();
			UpdatePlayerId();
		}

		public static void AddPlayer(string jsonData) => Char = jsonData.Deserialize<PlayerChar>();
		public static void UpdatePedId() => PlayerPed = new Ped(PlayerPedId());
		public static void UpdatePlayerId() => Player = Game.Player;
	}
}