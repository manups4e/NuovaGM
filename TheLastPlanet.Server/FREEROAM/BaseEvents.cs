using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheLastPlanet.Server.Core;

namespace TheLastPlanet.Server
{
	static class BaseEvents
	{
		private static List<string> killedMessages = new List<string>()
		{
			"ha ucciso",
			"ha distrutto",
			"ha cancellato",
			"ha macellato",
			"ha annichilito",
			"ha dissolto",
			"ha polverizzato",
			"ha devastato",
			"ha giustiziato",
			"ha atomizzato",
			"ha assassinato",
			"ha annientato",
			"ha appiattito",
			"ha rottamato",
		};

		public static void Init()
		{
			Server.Instance.AddEventHandler("lprp:onPlayerKilled", new Action<Player, string, int, Vector3>(OnPlayerKilled));
		}

		private static void OnPlayerKilled([FromSource] Player player, string tipo, int killer, Vector3 victimCoords)
		{
			string morte = "";
			Player Killer = Funzioni.GetPlayerFromId(killer);
			switch (tipo)
			{
				case "suicidio":
					morte = $"{player.Name} si è suicidato.";
					break;
				case "omicidio":
					morte = $"{Killer.Name} {killedMessages[new Random().Next(killedMessages.Count - 1)]} {player.Name}";
					break;
				case "generico":
					morte = $"{player.Name} è morto.";
					break;
			}
			Server.Instance.GetPlayers.ToList().ForEach(x => x.TriggerEvent("lpop:ShowNotification", morte));
		}
	}
}
