using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CitizenFX.Core;
using Logger;
using Newtonsoft.Json;
using TheLastPlanet.Client.Core.PlayerChar;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.PlayerChar;
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
		public static Dictionary<string, PlayerChar> GiocatoriOnline = new();

		public static async Task InitPlayer()
		{
			UpdatePedId();
			UpdatePlayerId();
			Client.Instance.AddTick(TickStatiPlayer);
			Char = await Client.Instance.Eventi.Request<PlayerChar>("lprp:setupUser");
			await Task.FromResult(0);
		}

		public static async Task Loading()
		{
			while (true)
			{
				if (Char != null && PlayerPed != null && Player != null) break;
				await BaseScript.Delay(100);
			}
		}

		public static async Task AddPlayer(string Data)
		{
			try
			{
				Char = Data.DeserializeFromJson<PlayerChar>();
				Log.Printa(LogType.Debug, Char.SerializeToJson());

				//BasePlayerShared = Data.DeserializeByte<BasePlayerShared>();
				//Dictionary<string, string> test = Data.DeserializeByte<Dictionary<string, string>>();
				//Log.Printa(LogType.Debug, test.SerializeToJson());
			}
			catch (Exception e)
			{
				Log.Printa(LogType.Error, e.ToString());
			}
		}

		public static void UpdatePedId()
		{
			PlayerPed = new Ped(PlayerPedId());
		}

		public static void UpdatePlayerId()
		{
			Player = Game.Player;
		}

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