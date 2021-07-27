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
		private static bool _inVeh;
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
			Client.Instance.AddTick(TickStatiPlayer);
			await Task.FromResult(0);

		}

		public static async Task Loaded()
		{
			while (MyPlayer == null || MyPlayer != null && !MyPlayer.Ready) await BaseScript.Delay(0);
		}

		public static async Task TickStatiPlayer()
		{
			await Loaded();
			await BaseScript.Delay(200);

			#region Posizione

			MyPlayer.Posizione = new Position(MyPlayer.Ped.Position, MyPlayer.Ped.Rotation);

			if (!MyPlayer.User.status.Spawned) return;
			if (MyPlayer.User.StatiPlayer.Istanza.Stanziato) return;
			MyPlayer.User.Posizione = new Position(MyPlayer.Ped.Position, MyPlayer.Ped.Rotation);

			#endregion

			#region Check Veicolo

			if (MyPlayer.Ped.IsInVehicle())
			{
				if (!_inVeh)
				{
					MyPlayer.User.StatiPlayer.RolePlayStates.InVeicolo = true;
					_inVeh = true;
				}
			}
			else
			{
				if (_inVeh)
				{
					MyPlayer.User.StatiPlayer.RolePlayStates.InVeicolo = false;
					_inVeh = false;
				}
			}

			#endregion

			#region Check Pausa

			if (Game.IsPaused || HUD.MenuPool.PauseMenus.Any(x => x.Visible))
			{
				if (!_inPausa)
				{
					_inPausa = true;
					MyPlayer.User.StatiPlayer.PlayerStates.InPausa = true;
				}
			}
			else
			{
				if (_inPausa)
				{
					_inPausa = false;
					MyPlayer.User.StatiPlayer.PlayerStates.InPausa = false;
				}
			}

			#endregion

			await Task.FromResult(0);
		}
	}
}