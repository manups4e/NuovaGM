﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheLastPlanet.Client.GameMode.ROLEPLAY.Banking;

namespace TheLastPlanet.Client.PlayerList
{
    internal static class FivemPlayerlist
    {
        static int maxPlayers = 0;
        public static void Init()
        {
            Client.Instance.AddTick(DisplayController);
        }

        public static void MostraMoney()
        {
            N_0x170f541e1cadd1de(true);
            SetMultiplayerWalletCash();
            SetMultiplayerBankCash();
            N_0x170f541e1cadd1de(false);
        }

        public static void NascondiMoney()
        {
            RemoveMultiplayerWalletCash();
            RemoveMultiplayerBankCash();
        }

        //TODO: this should be replaced by the ScaleformUI one to show the players online

        /// <summary>
        /// Manages the display and page setup of the playerlist.
        /// </summary>
        /// <returns></returns>
        private static async Task DisplayController()
        {
            if (Input.IsControlJustPressed(Control.MultiplayerInfo) && !MenuHandler.IsAnyMenuOpen && !IsPedRunningMobilePhoneTask(PlayerPedId()))
            {
                ScaleformUI.Main.PlayerListInstance.PlayerRows.Clear();
                int num = await EventDispatcher.Get<int>("tlg:fs:getMaxPlayers", PlayerCache.ActualMode);
                List<PlayerSlot> list = await EventDispatcher.Get<List<PlayerSlot>>("tlg:fs:getPlayers", PlayerCache.ActualMode);
                if (PlayerCache.ActualMode == ServerMode.Roleplay)
                {
                    if (BankingClient.interfaceOpen)
                        return;
                }

                foreach (PlayerSlot p in list)
                {
                    Ped ped = p.ServerId == PlayerCache.MyPlayer.Handle ? PlayerCache.MyPlayer.Ped : Functions.GetPlayerClientFromServerId(p.ServerId)?.Ped;
                    System.Tuple<int, string> mug = await Functions.GetPedMugshotAsync(ped);
                    if (ScaleformUI.Main.PlayerListInstance.PlayerRows.Any<PlayerRow>(x => x.ServerId == p.ServerId)) continue;
                    PlayerRow row = new PlayerRow()
                    {
                        Color = p.Color,
                        CrewLabelText = p.CrewLabelText,
                        FriendType = p.FriendType,
                        IconOverlayText = p.IconOverlayText,
                        JobPointsDisplayType = (ScoreDisplayType)p.JobPointsDisplayType,
                        JobPointsText = p.JobPointsText,
                        Name = p.Name,
                        RightIcon = (ScoreRightIconType)p.RightIcon,
                        RightText = p.RightText,
                        ServerId = p.ServerId,
                        TextureString = mug.Item2
                    };
                    ScaleformUI.Main.PlayerListInstance.AddRow(row);
                    UnregisterPedheadshot(mug.Item1);
                }
                ScaleformUI.Main.PlayerListInstance.SetTitle($"Modalità {PlayerCache.ActualMode} (online {num})", $"{(ScaleformUI.Main.PlayerListInstance.CurrentPage + 1)} / {ScaleformUI.Main.PlayerListInstance.MaxPages}", 2);
                ScaleformUI.Main.PlayerListInstance.CurrentPage++;
            }
            if (ScaleformUI.Main.PlayerListInstance.Enabled)
            {
                if (!Screen.Hud.IsComponentActive(HudComponent.MpCash)) MostraMoney();
                if (ScaleformUI.Main.PlayerListInstance.PlayerRows.Count > 0)
                {
                    foreach (PlayerRow p in ScaleformUI.Main.PlayerListInstance.PlayerRows)
                    {
                        int player = GetPlayerFromServerId(p.ServerId);
                        int index = ScaleformUI.Main.PlayerListInstance.PlayerRows.IndexOf(p);
                        if (NetworkIsPlayerTalking(player) || MumbleIsPlayerTalking(player))
                            ScaleformUI.Main.PlayerListInstance.SetIcon(index, ScoreRightIconType.ACTIVE_HEADSET, "");
                        else
                            ScaleformUI.Main.PlayerListInstance.SetIcon(index, p.RightIcon, p.RightText);
                    }
                }
            }
            else
            {
                if (Screen.Hud.IsComponentActive(HudComponent.MpCash)) NascondiMoney();
            }
        }
        /*
				private static async Task LoadScale()
				{
					if (scale != null)
					{
						for (int i = 0; i < maxClients * 2; i++) scale.CallFunction("SET_DATA_SLOT_EMPTY", i);
						scale.Dispose();
					}

					scale = null;
					while (!HasScaleformMovieLoaded(RequestScaleformMovie("MP_MM_CARD_FREEMODE"))) await BaseScript.Delay(0);
					scale = new Scaleform("MP_MM_CARD_FREEMODE");
					string titleIcon = "2";
					string titleLeftText = "The Last Galaxy - " + PlayerCache.ModalitàAttuale.ToString();
					string titleRightText = $"Players {NetworkGetNumConnectedPlayers()}/{maxClients}";
					scale.CallFunction("SET_TITLE", titleLeftText, titleRightText, titleIcon);
					await UpdateScale();
					scale.CallFunction("DISPLAY_VIEW");
				}


				/// <summary>
				/// Updates the scaleform settings.
				/// </summary>
				/// <returns></returns>
				private static async Task UpdateScale()
				{
					List<PlayerRow> rows = new List<PlayerRow>();
					for (int x = 0; x < 128; x++) // cleaning up in case of a reload, this frees up all ped headshot handles :)
						UnregisterPedheadshot(x);
					int amount = 0;

					var players = EventDispatcher.Get<List<PlayerRow>>("tlg:fs:getPlayersRows");

					foreach (Player p in Client.Instance.GetPlayers.ToList())
					{
						if (IsRowSupposedToShow(amount))
						{
							if (playerConfigs.ContainsKey(p.ServerId))
							{
								PlayerRow row = new()
								{
									color = 111,
									crewLabelText = playerConfigs[p.ServerId].crewName,
									friendType = ' ',
									iconOverlayText = "",
									jobPointsDisplayType = playerConfigs[p.ServerId].showJobPointsIcon ? PlayerRow.DisplayType.ICON : playerConfigs[p.ServerId].jobPoints >= 0 ? PlayerRow.DisplayType.NUMBER_ONLY : PlayerRow.DisplayType.NONE,
									jobPointsText = playerConfigs[p.ServerId].jobPoints >= 0 ? playerConfigs[p.ServerId].jobPoints.ToString() : "",
									name = p.Name.Replace("<", "").Replace(">", "").Replace("^", "").Replace("~", "").Trim(),
									rightIcon = (int)PlayerRow.RightIconType.RANK_FREEMODE,
									rightText = $"{p.ServerId}",
									serverId = p.ServerId
								};
								row.textureString = (await Funzioni.GetPedMugshotAsync(p.Character)).Item2;
								rows.Add(row);
							}
							else
							{
								PlayerRow row = new()
								{
									color = 111,
									crewLabelText = "",
									friendType = ' ',
									iconOverlayText = "",
									jobPointsDisplayType = PlayerRow.DisplayType.NUMBER_ONLY,
									jobPointsText = "",
									name = p.Name.Replace("<", "").Replace(">", "").Replace("^", "").Replace("~", "").Trim(),
									rightIcon = (int)PlayerRow.RightIconType.RANK_FREEMODE,
									rightText = $"{p.ServerId}",
									serverId = p.ServerId
								};
								row.textureString = (await Funzioni.GetPedMugshotAsync(p.Character)).Item2;
								rows.Add(row);
							}
						}

						amount++;
					}

					rows.Sort((row1, row2) => row1.serverId.CompareTo(row2.serverId));
					for (int i = 0; i < maxClients * 2; i++) scale.CallFunction("SET_DATA_SLOT_EMPTY", i);
					int index = 0;

					foreach (PlayerRow row in rows)
					{
						if (row.crewLabelText != "")
							scale.CallFunction("SET_DATA_SLOT", index, row.rightText, row.name, row.color, row.rightIcon, row.iconOverlayText, row.jobPointsText, $"..+{row.crewLabelText}", (int)row.jobPointsDisplayType, row.textureString, row.textureString, row.friendType);
						else
							scale.CallFunction("SET_DATA_SLOT", index, row.rightText, row.name, row.color, row.rightIcon, row.iconOverlayText, row.jobPointsText, "", (int)row.jobPointsDisplayType, row.textureString, row.textureString, row.friendType);
						index++;
					}
				}
		*/
    }
}