using CitizenFX.Core;
using NuovaGM.Client.gmPrincipale;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace NuovaGM.Client.ListaPlayers
{
	static class FivemPlayerlist
	{
		private static int maxClients = -1;
		private static bool ScaleSetup = false;
		private static int currentPage = 0;
		static Scaleform scale;
		private static int maxPages = 1;
		public struct PlayerRowConfig
		{
			public string crewName;
			public int jobPoints;
			public bool showJobPointsIcon;
		}
		private static Dictionary<int, PlayerRowConfig> playerConfigs = new Dictionary<int, PlayerRowConfig>();

		/// <summary>
		/// Constructor
		/// </summary>
		public static void Init()
		{
			BaseScript.TriggerServerEvent("lprp:fs:getMaxPlayers");
			Client.Instance.AddTick(ShowScoreboard);
			Client.Instance.AddTick(DisplayController);
			Client.Instance.AddTick(BackupTimer);
			Client.Instance.AddEventHandler("lprp:fs:setMaxPlayers", new Action<int>(SetMaxPlayers));
			Client.Instance.AddEventHandler("lprp:fs:setPlayerConfig", new Action<int, string, int, bool>(SetPlayerConfig));
		}


		/// <summary>
		/// Set the config for the specified player.
		/// </summary>
		/// <param name="playerServerId"></param>
		/// <param name="crewname"></param>
		/// <param name="jobpoints"></param>
		/// <param name="showJPicon"></param>
		private static async void SetPlayerConfig(int playerServerId, string crewname, int jobpoints, bool showJPicon)
		{
			var cfg = new PlayerRowConfig()
			{
				crewName = crewname ?? "",
				jobPoints = jobpoints,
				showJobPointsIcon = showJPicon
			};
			playerConfigs[playerServerId] = cfg;
			if (currentPage > -1)
			{
				await LoadScale();
			}
		}


		/// <summary>
		/// Used to close the page if the regular timer fails to close it for some odd reason.
		/// </summary>
		/// <returns></returns>
		private static async Task BackupTimer()
		{
			var timer = GetGameTimer();
			var oldPage = currentPage;
			while (GetGameTimer() - timer < 8000 && currentPage > 0 && currentPage == oldPage)
				await BaseScript.Delay(0);
			if (oldPage == currentPage)
				currentPage = 0;
		}

		/// <summary>
		/// Updates the max pages to disaplay based on the player count.
		/// </summary>
		private static void UpdateMaxPages()
		{
			maxPages = (int)Math.Ceiling((double)Client.Instance.GetPlayers.ToList().Count() / 16.0);
		}

		/// <summary>
		/// Manages the display and page setup of the playerlist.
		/// </summary>
		/// <returns></returns>
		private static async Task DisplayController()
		{
			if (Input.IsControlJustPressed(Control.MultiplayerInfo) && !HUD.MenuPool.IsAnyMenuOpen() && !Banking.BankingClient.InterfacciaAperta && !IsPedRunningMobilePhoneTask(PlayerPedId()))
			{
				UpdateMaxPages();
				if (ScaleSetup)
				{
					currentPage++;
					if (currentPage > maxPages)
					{
						currentPage = 0;
					}
					await LoadScale();
					var timer = GetGameTimer();
					bool nextPage = false;
					while (GetGameTimer() - timer < 5000)
					{
						await BaseScript.Delay(1);
						if (Input.IsControlJustPressed(Control.MultiplayerInfo))
						{
							nextPage = true;
							break;
						}
					}
					if (nextPage)
					{
						UpdateMaxPages();
						if (currentPage < maxPages)
						{
							currentPage++;
							await LoadScale();
						}
						else
						{
							currentPage = 0;
						}
					}
					else
					{
						currentPage = 0;
					}
				}
			}
		}

		/// <summary>
		/// Updates the max players (triggered from server event)
		/// </summary>
		/// <param name="count"></param>
		private static void SetMaxPlayers(int count)
		{
			maxClients = count;
		}

		/// <summary>
		/// Shows the scoreboard.
		/// </summary>
		/// <returns></returns>
		private static async Task ShowScoreboard()
		{
			if (maxClients != -1 && Main.spawned)
			{
				if (!ScaleSetup)
				{
					await LoadScale();
					ScaleSetup = true;
				}
				if (currentPage > 0)
				{
					float safezone = GetSafeZoneSize();
					float change = (safezone - 0.89f) / 0.11f;
					float x = 50f;
					x -= change * 78f;
					float y = 50f;
					y -= change * 50f;

					var width = 400f;
					var height = 490f;
					if (scale != null)
					{
						if (scale.IsLoaded)
						{
							scale.Render2DScreenSpace(new System.Drawing.PointF(x, y), new System.Drawing.PointF(width, height));
						}
					}
				}
			}
		}

		/// <summary>
		/// Loads the scaleform.
		/// </summary>
		/// <returns></returns>
		private static async Task LoadScale()
		{
			if (scale != null)
			{
				for (var i = 0; i < maxClients * 2; i++)
				{
					scale.CallFunction("SET_DATA_SLOT_EMPTY", i);
				}
				scale.Dispose();
			}
			scale = null;
			while (!HasScaleformMovieLoaded(RequestScaleformMovie("MP_MM_CARD_FREEMODE")))
			{
				await BaseScript.Delay(0);
			}
			scale = new Scaleform("MP_MM_CARD_FREEMODE");
			var titleIcon = "2";
			var titleLeftText = Client.Impostazioni.Main.NomeServer;
			var titleRightText = $"Players {NetworkGetNumConnectedPlayers()}/{maxClients}";
			scale.CallFunction("SET_TITLE", titleLeftText, titleRightText, titleIcon);
			await UpdateScale();
			scale.CallFunction("DISPLAY_VIEW");
		}

		/// <summary>
		/// Struct used for the player info row options.
		/// </summary>
		struct PlayerRow
		{
			public int serverId;
			public string name;
			public string rightText;
			public int color;
			public string iconOverlayText;
			public string jobPointsText;
			public string crewLabelText;
			public enum DisplayType
			{
				NUMBER_ONLY = 0,
				ICON = 1,
				NONE = 2
			};
			public DisplayType jobPointsDisplayType;
			public enum RightIconType
			{
				NONE = 0,
				INACTIVE_HEADSET = 48,
				MUTED_HEADSET = 49,
				ACTIVE_HEADSET = 47,
				RANK_FREEMODE = 65,
				KICK = 64,
				LOBBY_DRIVER = 79,
				LOBBY_CODRIVER = 80,
				SPECTATOR = 66,
				BOUNTY = 115,
				DEAD = 116,
				DPAD_GANG_CEO = 121,
				DPAD_GANG_BIKER = 122,
				DPAD_DOWN_TARGET = 123
			};
			public int rightIcon;
			public string textureString;
			public char friendType;
		}

		/// <summary>
		/// Returns the ped headshot string used for the image of the ped for each row.
		/// </summary>
		/// <param name="ped"></param>
		/// <returns></returns>
		private static async Task<string> GetHeadshotImage(int ped)
		{
			var headshotHandle = RegisterPedheadshot(ped);
			while (!IsPedheadshotReady(headshotHandle))
				await BaseScript.Delay(0);
			return GetPedheadshotTxdString(headshotHandle) ?? "";
		}

		/// <summary>
		/// Updates the scaleform settings.
		/// </summary>
		/// <returns></returns>
		private static async Task UpdateScale()
		{
			List<PlayerRow> rows = new List<PlayerRow>();

			for (var x = 0; x < 128; x++) // cleaning up in case of a reload, this frees up all ped headshot handles :)
				UnregisterPedheadshot(x);

			var amount = 0;
			foreach (Player p in Client.Instance.GetPlayers.ToList())
			{
				if (IsRowSupposedToShow(amount))
				{
					if (playerConfigs.ContainsKey(p.ServerId))
					{
						PlayerRow row = new PlayerRow()
						{
							color = 111,
							crewLabelText = playerConfigs[p.ServerId].crewName,
							friendType = ' ',
							iconOverlayText = "",
							jobPointsDisplayType = playerConfigs[p.ServerId].showJobPointsIcon ? PlayerRow.DisplayType.ICON :
								(playerConfigs[p.ServerId].jobPoints >= 0 ? PlayerRow.DisplayType.NUMBER_ONLY : PlayerRow.DisplayType.NONE),
							jobPointsText = playerConfigs[p.ServerId].jobPoints >= 0 ? playerConfigs[p.ServerId].jobPoints.ToString() : "",
							name = p.Name.Replace("<", "").Replace(">", "").Replace("^", "").Replace("~", "").Trim(),
							rightIcon = (int)PlayerRow.RightIconType.RANK_FREEMODE,
							rightText = $"{p.ServerId}",
							serverId = p.ServerId,
						};
						row.textureString = await GetHeadshotImage(GetPlayerPed(p.Handle));
						rows.Add(row);
					}
					else
					{
						PlayerRow row = new PlayerRow()
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
							serverId = p.ServerId,
						};
						row.textureString = await GetHeadshotImage(GetPlayerPed(p.Handle));
						rows.Add(row);
					}
				}
				amount++;
			}
			rows.Sort((row1, row2) => row1.serverId.CompareTo(row2.serverId));
			for (var i = 0; i < maxClients * 2; i++)
			{
				scale.CallFunction("SET_DATA_SLOT_EMPTY", i);
			}
			var index = 0;
			foreach (PlayerRow row in rows)
			{
				if (row.crewLabelText != "")
				{
					scale.CallFunction("SET_DATA_SLOT", index, row.rightText, row.name, row.color, row.rightIcon, row.iconOverlayText, row.jobPointsText,
						$"..+{row.crewLabelText}", (int)row.jobPointsDisplayType, row.textureString, row.textureString, row.friendType);
				}
				else
				{
					scale.CallFunction("SET_DATA_SLOT", index, row.rightText, row.name, row.color, row.rightIcon, row.iconOverlayText, row.jobPointsText,
						"", (int)row.jobPointsDisplayType, row.textureString, row.textureString, row.friendType);
				}
				index++;
			}
		}

		/// <summary>
		/// Used to check if the row from the loop is supposed to be displayed based on the current page view.
		/// </summary>
		/// <param name="row"></param>
		/// <returns></returns>
		private static bool IsRowSupposedToShow(int row)
		{
			if (currentPage > 0)
			{
				var max = currentPage * 16;
				var min = (currentPage * 16) - 16;
				if (row >= min && row < max)
				{
					return true;
				}
				return false;
			}
			return false;
		}
	}
}
