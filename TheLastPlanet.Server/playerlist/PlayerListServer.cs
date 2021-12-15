using CitizenFX.Core;
using TheLastPlanet.Server;
using System;
using System.Collections.Concurrent;
using System.Linq;
using static CitizenFX.Core.Native.API;
using TheLastPlanet.Shared.Internal.Events;
using TheLastPlanet.Server.Core.Buckets;
using TheLastPlanet.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FivemPlayerlistServer
{
	public static class PlayerListServer
	{

		private static ConcurrentDictionary<int, dynamic[]> list = new ConcurrentDictionary<int, dynamic[]>();
		public static void Init()
		{
		
			Server.Instance.Events.Mount("tlg:fs:getMaxPlayers", new Func<ClientId, ModalitaServer, Task<int>>(ReturnMaxPlayers));
			Server.Instance.Events.Mount("tlg:fs:getPlayers", new Func<ClientId, ModalitaServer, Task<List<PlayerSlot>>>(ReturnPlayers));
			Server.Instance.RegisterExport("setPlayerRowConfig", new Action<string, string, string, string>(SetPlayerConfig2));
			Server.Instance.Events.Mount("tlg:fs:setPlayerRowConfig", new Action<int, string, int, bool>(SetPlayerConfig));
		}

		private static async Task<int> ReturnMaxPlayers(ClientId source, ModalitaServer mod)
		{
			await BaseScript.Delay(0);
            switch (mod)
            {
				case ModalitaServer.Lobby:
					return Server.Instance.GetPlayers.Count();
				case ModalitaServer.FreeRoam:
					return BucketsHandler.FreeRoam.GetTotalPlayers();
				case ModalitaServer.Roleplay:
					return BucketsHandler.RolePlay.GetTotalPlayers();
				case ModalitaServer.Gare:
					return BucketsHandler.Gare.GetTotalPlayers();
				case ModalitaServer.Minigiochi:
					return BucketsHandler.Minigiochi.GetTotalPlayers();
				default:
					return 0;
			}
		}
		private static async Task<List<PlayerSlot>> ReturnPlayers(ClientId source, ModalitaServer mod)
        {
			List<PlayerSlot> result = new();
			foreach (var client in Server.Instance.Clients)
			{
                PlayerSlot slot = new()
                {
                    CrewLabelText = String.Empty,
                    FriendType = ' ',
                    IconOverlayText = String.Empty,
                    JobPointsDisplayType = SlotScoreDisplayType.NONE,
                    JobPointsText = String.Empty,
                    Name = client.Player.Name.Replace("<", "").Replace(">", "").Replace("^", "").Replace("~", "").Trim(),
                    ServerId = client.Handle
                };

                switch (client.User.Status.PlayerStates.Modalita)
                {
					case ModalitaServer.Lobby:
						slot.RightIcon = SlotScoreRightIconType.NONE;
						slot.RightText = $"Lobby";
						slot.Color = 3;
					break;
					case ModalitaServer.FreeRoam:
						slot.RightIcon = SlotScoreRightIconType.RANK_FREEMODE;
						slot.RightText = $"{client.User.FreeRoamChar.Level}";
						slot.Color = 21;
						break;
					case ModalitaServer.Roleplay:
						slot.RightIcon = SlotScoreRightIconType.NONE;
						slot.RightText = $"ID {client.Handle}";
						slot.Color = 116;
						break;
						//case ModalitaServer.Gare:
						//case ModalitaServer.Minigiochi:
				}
				result.Add(slot);
            }

            return mod switch
            {
                ModalitaServer.Lobby => result,
                ModalitaServer.FreeRoam => result.Where(x => x.Color == 21).ToList(),
                ModalitaServer.Roleplay => result.Where(x => x.Color == 116).ToList(),
                _ => result,
            };
        }
		private static void SetPlayerConfig2(string playerServerId, string crewName, string jobPoints, string showJobPointsIcon)
		{
			SetPlayerConfig(int.Parse(playerServerId), crewName, int.Parse(jobPoints ?? "-1"), bool.Parse(showJobPointsIcon ?? "false"));
		}
		private static void SetPlayerConfig(int playerServerId, string crewName, int jobPoints, bool showJobPointsIcon)
		{
			if (playerServerId > 0)
			{
				list[playerServerId] = new dynamic[4] { playerServerId, crewName ?? "", jobPoints, showJobPointsIcon };
				BaseScript.TriggerClientEvent("tlg:fs:setPlayerConfig", playerServerId, crewName ?? "", jobPoints,
					showJobPointsIcon);
			}
		}
	}
}
