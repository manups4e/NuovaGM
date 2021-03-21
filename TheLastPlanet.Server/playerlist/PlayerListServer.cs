﻿using CitizenFX.Core;
using TheLastPlanet.Server;
using System;
using System.Collections.Concurrent;
using System.Linq;
using static CitizenFX.Core.Native.API;

namespace FivemPlayerlistServer
{
	public static class FPLServer
	{

		private static ConcurrentDictionary<int, dynamic[]> list = new ConcurrentDictionary<int, dynamic[]>();
		public static void Init()
		{
			ServerSession.Instance.AddEventHandler("lprp:fs:getMaxPlayers", new Action<Player>(ReturnMaxPlayers));
			ServerSession.Instance.RegisterExport("setPlayerRowConfig", new Action<string, string, string, string>(SetPlayerConfig2));
			ServerSession.Instance.AddEventHandler("lprp:fs:setPlayerRowConfig", new Action<int, string, int, bool>(SetPlayerConfig));
		}

		private static async void ReturnMaxPlayers([FromSource] Player source)
		{
			await BaseScript.Delay(0);
			source.TriggerEvent("lprp:fs:setMaxPlayers", int.Parse(GetConvar("sv_maxClients", "30").ToString()));

			foreach (Player p in ServerSession.Instance.GetPlayers.ToList())
				if (list.ContainsKey(int.Parse(p.Handle)))
				{
					dynamic[] listItem = list[int.Parse(p.Handle)];
					dynamic p1 = listItem[0];
					dynamic p2 = listItem[1];
					dynamic p3 = listItem[2];
					dynamic p4 = listItem[3];
					source.TriggerEvent("lprp:fs:setPlayerRowConfig", p1, p2, p3, p4);
					await BaseScript.Delay(1);
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