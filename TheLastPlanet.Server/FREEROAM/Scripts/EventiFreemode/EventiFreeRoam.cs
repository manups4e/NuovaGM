using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using TheLastPlanet.Shared;
using TheLastPlanet.Shared.Internal.Events;

namespace TheLastPlanet.Server.FREEROAM.Scripts.EventiFreemode
{
    internal class EventiFreeRoam
    {
        public static void Init()
        {
			Server.Instance.Events.Mount("tlg:freeroam:finishCharServer", new Action<ClientId, FreeRoamChar>(FinishChar));
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
	}
}
