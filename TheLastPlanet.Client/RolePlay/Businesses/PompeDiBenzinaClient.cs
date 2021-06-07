﻿using CitizenFX.Core;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Impostazioni.Shared.Configurazione.Generici;
using static CitizenFX.Core.Native.API;
using TheLastPlanet.Client.Core;
using TheLastPlanet.Client.SessionCache;

namespace TheLastPlanet.Client.RolePlay.Businesses
{
	internal class PompeDiBenzinaClient
	{
		private static bool _interactWait = false;
		private static List<GasStation> _stations = new List<GasStation>();
		private static List<StationDiBenzina> _playerstations = new();
		private static Scaleform _info = new Scaleform("mp_mission_name_freemode");

		public static void Init()
		{
			Client.Instance.AddEventHandler("lprp:businesses:setstations", new Action<string, string>(SetStations));
			Client.Instance.AddEventHandler("lprp:businesses:checkcanmanage", new Action<bool, int, string, int>(CheckCanManage));
			Client.Instance.AddEventHandler("lprp:businesses:getstationcash", new Action<int>(GetStationCash));
			Client.Instance.AddEventHandler("lprp:businesses:sellstation", new Action<bool, string, string>(SellStation));
			Client.Instance.AddEventHandler("lprp:businesses:purchasestation", new Action<bool, string, int, int>(PurchaseStation));
			Client.Instance.AddEventHandler("lprp:businesses:stationfundschange", new Action<bool, string>(StationFundsChange));
			Client.Instance.NuiManager.RegisterCallback("lprp:businesses:manage", new Action<IDictionary<string, object>>(Manage));
			Client.Instance.NuiManager.RegisterCallback("lprp:businesses:sellstation", new Action<IDictionary<string, object>>(SellStation));
			Client.Instance.NuiManager.RegisterCallback("lprp:businesses:notification", new Action<IDictionary<string, object>>(Notification));
			Client.Instance.NuiManager.RegisterCallback("menuclosed", new Action(MenuClosed));
			Client.Instance.NuiManager.RegisterCallback("lprp:businesses:addstationfunds", new Action<IDictionary<string, object>>(AddStationFunds));
			Client.Instance.NuiManager.RegisterCallback("lprp:businesses:remstationfunds", new Action<IDictionary<string, object>>(RemStationFunds));
		}

		public static void Stop()
		{
			Client.Instance.RemoveEventHandler("lprp:businesses:setstations", new Action<string, string>(SetStations));
			Client.Instance.RemoveEventHandler("lprp:businesses:checkcanmanage", new Action<bool, int, string, int>(CheckCanManage));
			Client.Instance.RemoveEventHandler("lprp:businesses:getstationcash", new Action<int>(GetStationCash));
			Client.Instance.RemoveEventHandler("lprp:businesses:sellstation", new Action<bool, string, string>(SellStation));
			Client.Instance.RemoveEventHandler("lprp:businesses:purchasestation", new Action<bool, string, int, int>(PurchaseStation));
			Client.Instance.RemoveEventHandler("lprp:businesses:stationfundschange", new Action<bool, string>(StationFundsChange));
		}

		private static StationDiBenzina GetStationInfo(int index)
		{
			foreach (StationDiBenzina s in _playerstations)
				if (s.stationindex == index)
					return s;

			return null;
		}

		private StationDiBenzina GetPlayerstationsNearCoords(Vector3 pos)
		{
			int mstation = 0;

			for (int i = 0; i < _stations.Count; i++)
			{
				float dist = Cache.MyPlayer.User.Posizione.Distance(pos);

				if (dist < 50f)
				{
					mstation = i;

					break;
				}
			}

			return mstation > 0 ? _playerstations[mstation] : null;
		}

		private static void SetStations(string pompeBenza, string stazioniPlayer)
		{
			if (_stations.Count > 0) _stations.Clear();
			if (_playerstations.Count > 0) _playerstations.Clear();
			_stations = pompeBenza.FromJson<List<GasStation>>();
			_playerstations = stazioniPlayer.FromJson<List<StationDiBenzina>>();
		}

		private static void CheckCanManage(bool canmanage, int manageid, string managetime, int funds)
		{
			if (canmanage)
			{
				StationDiBenzina station = _playerstations[manageid];
				string pfmtstr = "";
				string[] allow = station.deliverallow.Split(';');
				if (allow.Length > 0) pfmtstr = allow.Aggregate(pfmtstr, (current, s) => current + " " + s);
				Client.Instance.NuiManager.SetFocus(true, true);
				Client.Instance.NuiManager.SendMessage(new
				{
					showManager = true,
					manageid,
					station.stationname,
					station.thanksmessage,
					fuelcost = station.fuelprice,
					deltype = station.delivertype,
					funds,
					deliverylist = pfmtstr
				});
			}
			else
			{
				HUD.ShowNotification("Puoi gestire la stazione una volta ogni 24 ore.~n~Torna domani alle ore ~r~" + managetime + "~w~.", NotificationColor.RedDifferent);
			}

			_interactWait = false;
		}

		private static void GetStationCash(int payout)
		{
			HUD.ShowNotification("Sei stato pagato ~b~" + payout + "$~w~ da questa stazione.");
			_interactWait = false;
		}

		private static void SellStation(bool success, string msg, string name)
		{
			if (success)
			{
				Client.Instance.NuiManager.SendMessage(new { closeManager = true });
				Client.Instance.NuiManager.SetFocus(false, false);
				HUD.ShowNotification("La tua Stazione è stata venduta a ~b~" + name + "~w~.", NotificationColor.GreenLight);
			}
			else
			{
				HUD.ShowNotification(msg, NotificationColor.Red);
			}
		}

		private static void PurchaseStation(bool success, string msg, int sidx, int sellprice)
		{
			if (success)
			{
				HUD.ShowNotification($"Congratulazioni nell'acquisto della tua nuova Stazione!\n~b~ {sellprice}~w~ sono stati spesi.", NotificationColor.GreenLight);
				BaseScript.TriggerServerEvent("lprp:businesses:checkcanmanage", sidx);
			}
			else
			{
				HUD.ShowNotification(msg, NotificationColor.Red);
				_interactWait = false;
			}
		}

		private static void StationFundsChange(bool success, string msg)
		{
			object a;
			if (success)
				a = new { setFunds = true, stationmoney = msg };
			else
				//HUD.ShowNotification(msg);
				a = new { setStatus = true, text = msg };
			Client.Instance.NuiManager.SendMessage(a);
		}

		private static void Manage(IDictionary<string, object> data)
		{
			string name = data["stationname"] as string;
			int fuelcost = Convert.ToInt32(data["fuelcost"]);
			int manageid = Convert.ToInt32(data["manageid"]);
			string thks = data["thanksmessage"] as string;
			int deltype = Convert.ToInt32(data["deltype"]);
			string deliverylist = data["deliverylist"] as string;
			BaseScript.TriggerServerEvent("lprp:businesses:changestation", name, thks, fuelcost, manageid, deltype, deliverylist);
			Client.Instance.NuiManager.SetFocus(false, false);
		}

		private static void SellStation(IDictionary<string, object> data)
		{
			string sellname = data["sellname"] as string;
			int manageid = Convert.ToInt32(data["manageid"]);
			BaseScript.TriggerServerEvent("lprp:businesses:sellstation", sellname, manageid);
		}

		private static void Notification(IDictionary<string, object> data)
		{
			HUD.ShowNotification(data["text"] as string);
		}

		private static void MenuClosed()
		{
			Client.Instance.NuiManager.SetFocus(false, false);
		}

		private static void AddStationFunds(IDictionary<string, object> data)
		{
			int amount = Convert.ToInt32(data["amount"]);
			int manageid = Convert.ToInt32(data["manageid"]);
			BaseScript.TriggerServerEvent("lprp:businesses:addstationfunds", manageid, amount);
		}

		private static void RemStationFunds(IDictionary<string, object> data)
		{
			int amount = Convert.ToInt32(data["amount"]);
			int manageid = Convert.ToInt32(data["manageid"]);
			BaseScript.TriggerServerEvent("lprp:businesses:remstationfunds", manageid, amount);
		}

		public static async Task BusinessesPumps()
		{
			for (int i = 0; i < _stations.Count; i++)
			{
				float dist = Cache.MyPlayer.User.Posizione.Distance(_stations[i].ppos);

				if (!(dist < 80)) continue;
				StationDiBenzina stationinfo = GetStationInfo(i + 1);
				World.DrawMarker(MarkerType.VerticalCylinder, new Vector3(_stations[i].ppos[0], _stations[i].ppos[1], _stations[i].ppos[2] - 1.00001f), new Vector3(0), new Vector3(0), new Vector3(1.1f, 1.1f, 1.3f), System.Drawing.Color.FromArgb(170, 0, 255, 0));

				if (dist < 1.3f)
					if (!Cache.MyPlayer.User.StatiPlayer.InVeicolo)
					{
						if (string.Equals(stationinfo.ownerchar, Cache.MyPlayer.User.FullName, StringComparison.CurrentCultureIgnoreCase))
						{
							HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per gestire la stazione");

							if (!_interactWait)
								if (Input.IsControlJustPressed(Control.Context) || Input.IsDisabledControlJustPressed(Control.Context))
								{
									_interactWait = true;
									BaseScript.TriggerServerEvent("lprp:businesses:checkcanmanage", i + 1);
								}
						}
						else if (stationinfo.ownerchar == "")
						{
							HUD.ShowHelp("Questa stazione è in vendita a ~g~" + _stations[i].sellprice.ToString() + "$~w~.\nPremi ~b~~INPUT_CONTEXT~~w~ per comprarla.");

							if (!_interactWait)
								if (Input.IsControlJustPressed(Control.Context) || Input.IsDisabledControlJustPressed(Control.Context))
								{
									_interactWait = true;
									BaseScript.TriggerServerEvent("lprp:businesses:purchasestation", i + 1);
								}
						}
					}

				_info = new Scaleform("mp_mission_name_freemode");
				while (!_info.IsLoaded) await BaseScript.Delay(10);
				if (string.IsNullOrEmpty(stationinfo.ownerchar))
					_info.CallFunction("SET_MISSION_INFO", stationinfo.stationname, "\nProprietario: Nessuno", "", "", "", "", "", "", "", "");
				else
					_info.CallFunction("SET_MISSION_INFO", stationinfo.stationname, "\nProprietario: " + stationinfo.ownerchar, "", "", "", "", "", "", "", "");
				_info.Render3D(_stations[i].ppos, GetGameplayCamRot(0), new Vector3(2.0f, 2.0f, 2.0f));
			}

			await Task.FromResult(0);
		}
	}
}