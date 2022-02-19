using System;
using System.Collections.Generic;
using System.Linq;
using TheLastPlanet.Client.MODALITA.FREEROAM.Spawner;
using TheLastPlanet.Shared.Internal.Events;

namespace TheLastPlanet.Client.MODALITA.FREEROAM.Managers
{
    static class PlayerBlipsHandler
    {
        private static List<FRBlipsInfo> _fRBlipsInfos = new();
        private static Dictionary<int, Blip> playerBlips = new();

        public static void Init()
        {
            AccessingEvents.OnFreeRoamSpawn += OnPlayerJoined;
            AccessingEvents.OnFreeRoamLeave += OnPlayerLeft;
        }

        private static void OnPlayerJoined(ClientId client)
        {
            Client.Instance.Events.Mount("freeroam.UpdatePlayerBlipInfos", new Action<List<FRBlipsInfo>>(UpdateBlips));
        }

        public static void OnPlayerLeft(ClientId client)
        {
            Client.Instance.Events.Unmount("freeroam.UpdatePlayerBlipInfos");
            foreach (var bb in playerBlips) if (bb.Value.Exists()) bb.Value.Delete();
        }

        private static void UpdateBlips(List<FRBlipsInfo> info)
        {
            _fRBlipsInfos = info;
            UpdatePlayerBlips();
        }

        public static void UpdatePlayerBlips()
        {
            playerBlips.Where(x => !_fRBlipsInfos.Any(y => y.ServerId == x.Key)).ToList().ForEach(x => x.Value.Delete());
            foreach (var player in _fRBlipsInfos)
            {
                Blip blip;
                //if (player.ServerId == PlayerCache.MyPlayer.Player.ServerId) continue; // ignora il mio pg fai solo gli altri

                if (playerBlips.ContainsKey(player.ServerId))
                {
                    blip = playerBlips[player.ServerId];
                    blip.Position = player.Pos.ToVector3;
                    blip.Rotation = (int)Math.Ceiling(player.Pos.Heading);
                }
                else
                {
                    blip = World.CreateBlip(player.Pos.ToVector3);
                    SetBlipCategory(blip.Handle, 7);
                    SetBlipDisplay(blip.Handle, 6);
                    SetBlipShrink(blip.Handle, false);
                    SetBlipShowCone(blip.Handle, true);
                    ShowHeadingIndicatorOnBlip(blip.Handle, true);
                    playerBlips.Add(player.ServerId, blip);
                }

                blip.Name = player.Name;
                blip.Sprite = BlipSprite.Standard;
                if ((int)blip.Sprite != player.Sprite)
                    blip.Sprite = (BlipSprite)player.Sprite;
            }
        }
    }
}
