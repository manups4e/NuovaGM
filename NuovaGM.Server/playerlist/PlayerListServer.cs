using CitizenFX.Core;
using NuovaGM.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using static CitizenFX.Core.Native.API;

namespace FivemPlayerlistServer
{
	public static class FPLServer
	{

		private static Dictionary<int, dynamic[]> list = new Dictionary<int, dynamic[]>();
		public static void Init()
		{
			Server.Instance.RegisterEventHandler("lprp:fs:getMaxPlayers", new Action<Player>(ReturnMaxPlayers));
			Server.Instance.RegisterExport("setPlayerRowConfig", new Action<string, string, string, string>(SetPlayerConfig2));
			Server.Instance.RegisterEventHandler("lprp:fs:setPlayerRowConfig", new Action<int, string, int, bool>(SetPlayerConfig));
		}

		private static async void ReturnMaxPlayers([FromSource] Player source)
		{
			source.TriggerEvent("lprp:fs:setMaxPlayers", int.Parse(GetConvar("sv_maxClients", "30").ToString()));
			foreach (Player p in Server.Instance.GetPlayers.ToList())
			{
				if (list.ContainsKey(int.Parse(p.Handle)))
				{
					var listItem = list[int.Parse(p.Handle)];
					var p1 = listItem[0];
					var p2 = listItem[1];
					var p3 = listItem[2];
					var p4 = listItem[3];
					source.TriggerEvent("lprp:fs:setPlayerRowConfig", p1, p2, p3, p4);
					await BaseScript.Delay(1);
				}
			}
		}

		private static void SetPlayerConfig2(string playerServerId, string crewName, string jobPoints, string showJobPointsIcon)
		{
			SetPlayerConfig(int.Parse(playerServerId), crewName, int.Parse(jobPoints ?? "-1"), bool.Parse(showJobPointsIcon ?? "false"));
		}
		private static void SetPlayerConfig(int playerServerId, string crewName, int jobPoints, bool showJobPointsIcon)
		{
			if (playerServerId > 0)
			{
				list[playerServerId] = new dynamic[4] { playerServerId, crewName ?? "", jobPoints != null ? jobPoints : -1, showJobPointsIcon != null ? showJobPointsIcon : false };
				BaseScript.TriggerClientEvent("lprp:fs:setPlayerConfig", playerServerId, crewName ?? "", jobPoints != null ? jobPoints : -1,
					showJobPointsIcon != null ? showJobPointsIcon : false);
			}
		}

	}
}
