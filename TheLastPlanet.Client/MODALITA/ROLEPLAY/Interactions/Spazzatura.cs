﻿using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.NativeUI;
using TheLastPlanet.Client.RolePlay.Core.Status;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Shared;
using TheLastPlanet.Client.Core;

namespace TheLastPlanet.Client.RolePlay.Interactions
{
	internal static class Spazzatura
	{
		private static readonly List<ObjectHash> Cestini = new List<ObjectHash>()
		{
			ObjectHash.prop_bin_01a,
			ObjectHash.prop_bin_02a,
			ObjectHash.prop_bin_03a,
			ObjectHash.prop_bin_04a,
			ObjectHash.prop_bin_05a,
			ObjectHash.prop_bin_06a,
			ObjectHash.prop_bin_07a,
			ObjectHash.prop_bin_07b,
			ObjectHash.prop_bin_07c,
			ObjectHash.prop_bin_07d,
			ObjectHash.prop_bin_08a,
			ObjectHash.prop_bin_08open,
			ObjectHash.prop_bin_09a,
			ObjectHash.prop_bin_10a,
			ObjectHash.prop_bin_10b,
			ObjectHash.prop_bin_11a,
			ObjectHash.prop_bin_11b,
			ObjectHash.prop_bin_12a,
			ObjectHash.prop_bin_13a,
			ObjectHash.prop_bin_14a,
			ObjectHash.prop_bin_14b,
			ObjectHash.prop_bin_beach_01a,
			ObjectHash.prop_bin_beach_01d,
			ObjectHash.prop_cs_bin_01,
			ObjectHash.prop_cs_bin_01_skinned,
			ObjectHash.prop_cs_bin_02,
			ObjectHash.prop_cs_bin_03,
			ObjectHash.prop_gas_smallbin01,
			ObjectHash.prop_recyclebin_01a,
			ObjectHash.prop_recyclebin_02a,
			ObjectHash.prop_recyclebin_02b,
			ObjectHash.prop_recyclebin_02_c,
			ObjectHash.prop_recyclebin_02_d,
			ObjectHash.prop_recyclebin_03_a,
			ObjectHash.prop_recyclebin_04_a,
			ObjectHash.prop_recyclebin_04_b,
			ObjectHash.prop_recyclebin_05_a,
			ObjectHash.v_serv_tc_bin1_,
			ObjectHash.v_serv_tc_bin2_,
			ObjectHash.v_serv_tc_bin3_,
			ObjectHash.zprop_bin_01a_old
		};
		private static float TrashRange = 1.375f;
		private static Prop BinClosest;

		public static async Task ControlloSpazzatura()
		{
			BinClosest = World.GetAllProps().Where(o => Cestini.Contains((ObjectHash)(uint)o.Model.Hash)).FirstOrDefault(o => Cache.PlayerCache.MyPlayer.User.Posizione.Distance(o.Position) < TrashRange);
			await BaseScript.Delay(500);
		}

		public static async Task CestiSpazzatura()
		{
			Ped p = Cache.PlayerCache.MyPlayer.Ped;

			if (BinClosest != null && !HUD.MenuPool.IsAnyMenuOpen)
			{
				HUD.ShowHelp("Premid ~INPUT_CONTEXT~ per gettare via qualcosa.~n~Premi ~INPUT_DETONATE~ per cercare qualcosa nella spazzatura.");

				if (Input.IsControlJustPressed(Control.Context) && !HUD.MenuPool.IsAnyMenuOpen)
				{
					List<Inventory> inv = Cache.PlayerCache.MyPlayer.User.Inventory;

					if (inv.Count > 0)
					{
						UIMenu GettaMenu = new UIMenu("Getta nel Cestino", "Cosa buttiamo via?");
						HUD.MenuPool.Add(GettaMenu);

						foreach (Inventory it in inv)
							if (it.Amount > 0)
								if (ConfigShared.SharedConfig.Main.Generici.ItemList[it.Item].drop.drop)
								{
									List<dynamic> list = new List<dynamic>();
									for (int obj = 1; obj < it.Amount + 1; obj++) list.Add(obj);
									UIMenuListItem buttavia = new UIMenuListItem(it.Item, list, 0);
									GettaMenu.AddItem(buttavia);
								}

						GettaMenu.OnItemSelect += async (menu, item, index) =>
						{
							BaseScript.TriggerServerEvent("lprp:removeIntenvoryItem", item.Text, (item as UIMenuListItem).Index + 1);
							HUD.ShowNotification($"Hai gettato nella spazzatura ~y~{(item as UIMenuListItem).Index + 1}x {item.Text}~w~");
						};
						GettaMenu.Visible = true;
					}
					else
					{
						HUD.ShowNotification("Non hai oggetti nell'inventario!!", NotificationColor.Red, true);
					}
				}
				else if (Input.IsControlJustPressed(Control.Detonate) && !HUD.MenuPool.IsAnyMenuOpen)
				{
					Vector3 offset = GetOffsetFromEntityInWorldCoords(BinClosest.Handle, 0f, -0.97f, 0.05f);
					p.Task.LookAt(BinClosest);
					TaskGoStraightToCoord(PlayerPedId(), offset.X, offset.Y, offset.Z + 1, 1f, 20000, BinClosest.Heading, 0.1f);
					TaskStartScenarioInPlace(PlayerPedId(), "PROP_HUMAN_BUM_BIN", 0, true);
					await BaseScript.Delay(5000);
					int random = Funzioni.GetRandomInt(0, 100);

					switch (random)
					{
						case int n when n < 16:
							HUD.ShowNotification("Qualcuno ha gettato via dei soldi!!");
							BaseScript.TriggerServerEvent("lprp:givemoney", Funzioni.GetRandomInt(10, 50));

							break;
						case int n when n > 15 && n < 41:
							int rd = Funzioni.GetRandomInt(1, 2);

							switch (rd)
							{
								case 1:
									HUD.ShowNotification("Hai trovato un panino mezzo mangiato.. Meglio mangiarlo subito!");
									StatsNeeds.Needs["Fame"].Val -= 20;

									break;
								case 2:
									HUD.ShowNotification("Hai trovato una bottiglietta d'acqua non finita.. Meglio berla subito!");
									StatsNeeds.Needs["Sete"].Val -= 20;

									break;
							}

							break;
						case int n when n > 40 && n < 100:
							HUD.ShowNotification("Oh beh.. non hai trovato nulla..");

							break;
					}

					await BaseScript.Delay(1000);
					p.Task.ClearAll();
				}
			}
		}
	}
}