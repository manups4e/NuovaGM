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
		internal static bool _inVeh;
		private static bool _inPausa;


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
            InternalGameEvents.OnPlayerEnteredVehicle += OnPlayerEnteredVehicle;
		}

        private static void OnPlayerEnteredVehicle(Player player, Vehicle vehicle)
        {
			if (player.Handle == MyPlayer.Player.Handle)
			{
				MyPlayer.User.Status.PlayerStates.InVeicolo = true;
				_inVeh = true;
			}
		}

		public static async Task Loaded()
		{
			while (MyPlayer == null || MyPlayer != null && !MyPlayer.Ready) await BaseScript.Delay(0);
		}

		public static async Task TickStatus()
		{
			await Loaded();

			#region Posizione

			// TODO: non salvare position nel db se siamo in un interior
			MyPlayer.Posizione = new Position(MyPlayer.Ped.Position, MyPlayer.Ped.Rotation);

			#endregion

			#region Check Veicolo

			if(_inVeh)
            {
                if (!MyPlayer.Ped.IsInVehicle())
                {
					MyPlayer.User.Status.PlayerStates.InVeicolo = false;
					_inVeh = false;
				}
			}

			#endregion

			#region Check Pausa

			//|| HUD.MenuPool.IsAnyPauseMenuOpen
			if (!_inPausa)
			{
				if (Game.IsPaused)
				{
					_inPausa = true;
					MyPlayer.User.Status.PlayerStates.InPausa = true;
				}
			}
            else
            {
				if (!Game.IsPaused)
				{
					_inPausa = false;
					MyPlayer.User.Status.PlayerStates.InPausa = false;
				}
			}

			#endregion

			await Task.FromResult(0);
		}
	}
}