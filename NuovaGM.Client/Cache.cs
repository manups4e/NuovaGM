using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using TheLastPlanet.Client.Core.Personaggio;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Shared;
using static CitizenFX.Core.Native.API;

namespace TheLastPlanet.Client
{
	public static class Cache
	{
		private static bool _inVeh;
		private static bool _inPausa;
		public static PlayerChar Char { get; private set; }
		public static Ped PlayerPed { get; private set; }
		public static Player Player { get; private set; }

		static Cache()
		{
			UpdatePedId();
			UpdatePlayerId();
			Client.Instance.AddTick(TickStatiPlayer);
		}

		public static void AddPlayer(string jsonData) { Char = jsonData.Deserialize<PlayerChar>(); }

		public static void UpdatePedId() { PlayerPed = new Ped(PlayerPedId()); }

		public static void UpdatePlayerId() { Player = Game.Player; }

		private static int _checkTimer1;

		public static async Task TickStatiPlayer()
		{
			while (Char == null) await BaseScript.Delay(0);
			await BaseScript.Delay(20);

			#region Check Veicolo

			if (PlayerPed.IsInVehicle())
			{
				if (!_inVeh)
				{
					Char.StatiPlayer.InVeicolo = true;
					_inVeh = true;
				}
			}
			else
			{
				if (_inVeh)
				{
					Char.StatiPlayer.InVeicolo = false;
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
					Char.StatiPlayer.InPausa = true;
				}
			}
			else
			{
				if (_inPausa)
				{
					_inPausa = false;
					Char.StatiPlayer.InPausa = false;
				}
			}

			#endregion

			await Task.FromResult(0);
		}
	}
}