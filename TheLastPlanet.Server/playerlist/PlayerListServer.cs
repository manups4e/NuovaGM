using CitizenFX.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheLastPlanet.Server;
using TheLastPlanet.Server.Core.Buckets;
using TheLastPlanet.Shared;


namespace FivemPlayerlistServer
{
    public static class PlayerListServer
    {

        private static ConcurrentDictionary<int, dynamic[]> list = new ConcurrentDictionary<int, dynamic[]>();
        public static void Init()
        {

            EventDispatcher.Mount("tlg:fs:getMaxPlayers", new Func<PlayerClient, ServerMode, Task<int>>(ReturnMaxPlayers));
            EventDispatcher.Mount("tlg:fs:getPlayers", new Func<PlayerClient, ServerMode, Task<List<PlayerSlot>>>(ReturnPlayers));
            Server.Instance.RegisterExport("setPlayerRowConfig", new Action<string, string, string, string>(SetPlayerConfig2));
            EventDispatcher.Mount("tlg:fs:setPlayerRowConfig", new Action<int, string, int, bool>(SetPlayerConfig));
        }

        private static async Task<int> ReturnMaxPlayers([FromSource] PlayerClient source, ServerMode mod)
        {
            await BaseScript.Delay(0);
            switch (mod)
            {
                case ServerMode.Lobby:
                    return Server.Instance.GetPlayers.Count();
                case ServerMode.FreeRoam:
                    return BucketsHandler.FreeRoam.GetTotalPlayers();
                case ServerMode.Roleplay:
                    return BucketsHandler.RolePlay.GetTotalPlayers();
                case ServerMode.Races:
                    return BucketsHandler.Gare.GetTotalPlayers();
                case ServerMode.Minigames:
                    return BucketsHandler.Minigiochi.GetTotalPlayers();
                default:
                    return 0;
            }
        }
        private static async Task<List<PlayerSlot>> ReturnPlayers([FromSource] PlayerClient source, ServerMode mod)
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

                switch (client.Status.PlayerStates.Mode)
                {
                    case ServerMode.Lobby:
                        slot.RightIcon = SlotScoreRightIconType.NONE;
                        slot.RightText = $"Lobby";
                        slot.Color = 3;
                        break;
                    case ServerMode.FreeRoam:
                        slot.RightIcon = SlotScoreRightIconType.RANK_FREEMODE;
                        slot.RightText = $"{client.User.FreeRoamChar.Level}";
                        slot.Color = 21;
                        break;
                    case ServerMode.Roleplay:
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
                ServerMode.Lobby => result,
                ServerMode.FreeRoam => result.Where(x => x.Color == 21).ToList(),
                ServerMode.Roleplay => result.Where(x => x.Color == 116).ToList(),
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
