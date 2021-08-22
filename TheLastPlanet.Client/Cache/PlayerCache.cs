using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using TheLastPlanet.Client.Core.PlayerChar;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.Internal.Events;
using TheLastPlanet.Shared.PlayerChar;
using TheLastPlanet.Shared.Snowflakes;

namespace TheLastPlanet.Client.Cache
{
	public static class PlayerCache
	{
		public static ClientId MyPlayer { get; private set; }
		public static Char_data CurrentChar => MyPlayer.User.CurrentChar;
		public static List<ClientId> GiocatoriOnline = new();
		public static ModalitaServer ModalitàAttuale = ModalitaServer.Lobby;

		public static async Task InitPlayer()
		{
			var pippo = await Client.Instance.Events.Get<Tuple<Snowflake, BasePlayerShared>>("lprp:setupUser");
			MyPlayer = new ClientId
			{
				Handle = Game.Player.ServerId,
				Id = pippo.Item1,
				User = new User(pippo.Item2)
			};
			Client.Instance.AddTick(TickStatus);
			await Task.FromResult(0);

		}

		public static async Task Loaded()
		{
			while (MyPlayer == null || MyPlayer != null && !MyPlayer.Ready) await BaseScript.Delay(0);
		}

		public static async Task TickStatus()
		{
			await Loaded();
			await BaseScript.Delay(200);

			#region Posizione

			MyPlayer.Posizione = new Position(MyPlayer.Ped.Position, MyPlayer.Ped.Rotation);

			if (!MyPlayer.User.Status.Spawned) return;
			if (MyPlayer.User.Status.Istanza.Stanziato) return;
			MyPlayer.Posizione = new Position(MyPlayer.Ped.Position, MyPlayer.Ped.Rotation);

			#endregion

			#region Check Veicolo
 			MyPlayer.User.Status.RolePlayStates.InVeicolo = MyPlayer.Ped.IsInVehicle();
			#endregion

			#region Check Pausa
			MyPlayer.User.Status.PlayerStates.InPausa = Game.IsPaused || HUD.MenuPool.PauseMenus.Any(x => x.Visible);
			#endregion

			await Task.FromResult(0);
		}
	}
}