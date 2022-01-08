using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.Internal.Events;

namespace TheLastPlanet.Server.FREEROAM.Scripts.EventiFreemode
{
    internal class EventiFreeRoam
    {
        public static void Init()
        {
			Server.Instance.Events.Mount("tlg:freeroam:finishCharServer", new Action<ClientId, FreeRoamChar>(FinishChar));
			Server.Instance.Events.Mount("tlg:freeroam:salvapersonaggio", new Action<ClientId>(SalvaPersonaggio));
			Server.Instance.Events.Mount("tlg:casino:getVehModel", new Func<ClientId, Task<string>>(ReturnCasinoPriceModelForPlayer));
		}

		public static void SalvaPersonaggio(ClientId client)
        {
			client.User.FreeRoamChar.Posizione = client.Ped.Position.ToPosition();
			API.SetResourceKvpNoSync($"freeroam:player_{client.User.Identifiers.Discord}:char_model", BitConverter.ToString(client.User.FreeRoamChar.ToBytes()));
		}

		public static void FinishChar(ClientId client, FreeRoamChar data)
		{
			try
			{
				FreeRoamChar Char = data;
				client.User.FreeRoamChar = Char;
				API.SetResourceKvpNoSync($"freeroam:player_{client.User.Identifiers.Discord}:char_model", BitConverter.ToString(Char.ToBytes()));
				//API.DeleteResourceKvpNoSync($"freeroam:player_{client.User.Identifiers.Discord}:char_model");
			}
			catch (Exception e)
			{
				Server.Logger.Error($"{e.Message}");
			}
		}
		public static async Task<string> ReturnCasinoPriceModelForPlayer(ClientId client)
		{
			// per il momento usiamo una prototipo poi vediamo...
			return "zentorno";
		}

		public static async void SpawnEventVehicles(Dictionary<Vector4, uint> vehicles)
		{

		}

	}
}
