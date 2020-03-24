using CitizenFX.Core;
using Newtonsoft.Json;
using NuovaGM.Client.gmPrincipale.Utility;
using NuovaGM.Client.gmPrincipale.Utility.HUD;
using NuovaGM.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace NuovaGM.Client.Businesses
{
	class PompeDiBenzinaClient
	{
		static string identifier = "";
		static bool interactWait = false;
		static List<GasStation> stations = new List<GasStation>();
		static List<StationDiBenzina> playerstations = new List<StationDiBenzina>();

		public static void Init()
		{
			Client.GetInstance.RegisterEventHandler("lprp:businesses:setstations", new Action<string>(SetStations));
			Client.GetInstance.RegisterEventHandler("lprp:businesses:checkcanmanage", new Action<bool, int, string, int>(CheckCanManage));
			Client.GetInstance.RegisterEventHandler("lprp:businesses:getstationcash", new Action<int>(GetStationCash));
			Client.GetInstance.RegisterEventHandler("lprp:businesses:sellstation", new Action<bool, string, string>(SellStation));
			Client.GetInstance.RegisterEventHandler("lprp:businesses:purchasestation", new Action<bool, string, int, int>(PurchaseStation));
			Client.GetInstance.RegisterEventHandler("lprp:businesses:stationfundschange", new Action<bool, string>(StationFundsChange));
			Client.GetInstance.RegisterNuiEventHandler("lprp:businesses:manage", new Action<dynamic>(Manage));
			Client.GetInstance.RegisterNuiEventHandler("lprp:businesses:sellstation", new Action<dynamic>(SellStation));
			Client.GetInstance.RegisterNuiEventHandler("lprp:businesses:notification", new Action<dynamic>(Notification));
			Client.GetInstance.RegisterNuiEventHandler("menuclosed", new Action<dynamic>(MenuClosed));
			Client.GetInstance.RegisterNuiEventHandler("lprp:businesses:addstationfunds", new Action<dynamic>(AddStationFunds));
			Client.GetInstance.RegisterNuiEventHandler("lprp:businesses:remstationfunds", new Action<dynamic>(RemStationFunds));
		}

		public static StationDiBenzina GetStationInfo(int index)
		{
			foreach (StationDiBenzina s in playerstations)
				if (s.stationindex == index)
					return s;
			return null;
		}

		public StationDiBenzina GetPlayerStationsNearCoords(Vector3 pos)
		{
			int mstation = 0;
			for (int i = 0; i < stations.Count; i++)
			{
				float dist = World.GetDistance(Game.PlayerPed.Position, pos);
				if (dist < 50f)
				{
					mstation = i;
					break;
				}
			}
			return mstation > 0 ? playerstations[mstation] : null;
		}

		public static void SetStations(string lista)
		{
			List<dynamic> newlist = JsonConvert.DeserializeObject<List<dynamic>>(lista);
			if (stations.Count > 0)
				stations.Clear();
			if (playerstations.Count > 0)
				playerstations.Clear();
			for (int i = 0; i < newlist[0].Count; i++)
				stations.Add(new GasStation(newlist[0][i]));
			for (int i = 0; i < newlist[1].Count; i++)
				playerstations.Add(new StationDiBenzina((string)newlist[1][i]["identifier"].Value, (string)newlist[1][i]["Name"], (int)newlist[1][i]["businessid"].Value, Convert.ToDateTime(newlist[1][i]["lastpaidrent"].Value), (string)newlist[1][i]["ownerchar"].Value, (int)newlist[1][i]["stationindex"].Value, (string)newlist[1][i]["stationname"].Value, (int)newlist[1][i]["cashwaiting"].Value, (int)newlist[1][i]["fuel"].Value, (int)newlist[1][i]["fuelprice"].Value, Convert.ToDateTime(newlist[1][i]["lastmanaged"].Value), Convert.ToDateTime(newlist[1][i]["lastlogin"].Value), (string)newlist[1][i]["deliverallow"].Value, (string)newlist[1][i]["thanksmessage"].Value, (int)newlist[1][i]["delivertype"].Value));
		}

		public static void CheckCanManage(bool canmanage, int manageid, string managetime, int funds)
		{
			if (canmanage)
			{
				StationDiBenzina station = playerstations[manageid];
				string pfmtstr = "";
				string[] allow = station.deliverallow.Split(';');
				if (allow.Length > 0)
					foreach (string s in allow)
						pfmtstr += " " + s;
				SetNuiFocus(true, true);
				string a = "{\"showManager\":\"true\",\"manageid\":\"" + manageid + "\",\"stationname\":\"" + station.stationname + "\",\"thanksmessage\":\"" + station.thanksmessage + "\",\"fuelcost\":\"" + station.fuelprice + "\",\"deltype\":\"" + station.delivertype + "\",\"funds\":\"" + funds + "\",\"deliverylist\":\"" + pfmtstr + "\"}";
				SendNuiMessage(a);

			}
			else
				HUD.ShowNotification("Puoi gestire la stazione una volta ogni 24 ore.~n~Torna domani alle ore ~r~" + managetime + "~w~.", NotificationColor.RedDifferent);
			interactWait = false;
		}

		public static void GetStationCash(int payout)
		{
			HUD.ShowNotification("Sei stato pagato ~b~" + payout + "$~w~ da questa stazione.");
			interactWait = false;
		}

		public static void SellStation(bool success, string msg, string name)
		{
			if (success)
			{
				string a = "{\"closeManager\":\"true\"}";
				SendNuiMessage(a);
				SetNuiFocus(false, false);
				HUD.ShowNotification("La tua Stazione è stata venduta a ~b~" + name + "~w~.", NotificationColor.GreenLight);
			}
			else
			{
				HUD.ShowNotification(msg, NotificationColor.Red);
			}
		}

		public static void PurchaseStation(bool success, string msg, int sidx, int sellprice)
		{
			if (success)
			{
				HUD.ShowNotification("Congratulazioni nell'acquisto della tua nuova Stazione!\n~b~ " + sellprice + "$~w~ sono stati spesi.", NotificationColor.GreenLight);
				BaseScript.TriggerServerEvent("lprp:businesses:checkcanmanage", sidx);
			}
			else
			{
				HUD.ShowNotification(msg, NotificationColor.Red);
				interactWait = false;
			}
		}

		public static void StationFundsChange(bool success, string msg)
		{
			if (success)
			{
				string a = "{\"setFunds\":\"true\",\"stationmoney\":\"" + msg + "\"}";
				SendNuiMessage(a);
			}
			else
			{
				HUD.ShowNotification(msg);
				string a = "{\"setFunds\":\"true\",\"text\":\"" + msg + "\"}";
				SendNuiMessage(a);
			}
		}

		private static void Manage(dynamic data)
		{
			string name = data.stationname;
			int fuelcost = Convert.ToInt32(data.fuelcost);
			int manageid = Convert.ToInt32(data.manageid);
			string thks = data.thanksmessage;
			int deltype = Convert.ToInt32(data.deltype);
			string deliverylist = data.deliverylist;

			Client.Printa(LogType.Debug, name);
			Client.Printa(LogType.Debug, thks);

			BaseScript.TriggerServerEvent("lprp:businesses:changestation", name, thks, fuelcost, manageid, deltype, deliverylist);
			SetNuiFocus(false, false);
		}


		private static void SellStation(dynamic data)
		{
			string sellname = data.sellname;
			int manageid = Convert.ToInt32(data.manageid);
			BaseScript.TriggerServerEvent("lprp:businesses:sellstation", sellname, manageid);
		}

		private static void Notification(dynamic data)
		{
			HUD.ShowNotification(data.text);
		}

		private static void MenuClosed(dynamic data)
		{
			SetNuiFocus(false, false);
		}

		private static void AddStationFunds(dynamic data)
		{
			int amount = Convert.ToInt32(data.amount);
			int manageid = Convert.ToInt32(data.manageid);
			BaseScript.TriggerServerEvent("lprp:businesses:addstationfunds", manageid, amount);
		}


		private static void RemStationFunds(dynamic data)
		{
			int amount = Convert.ToInt32(data.amount);
			int manageid = Convert.ToInt32(data.manageid);
			BaseScript.TriggerServerEvent("lprp:businesses:remstationfunds", manageid, amount);
		}

		public static async Task BusinessesPumps()
		{
			for (int i = 0; i < stations.Count; i++)
			{ 
				float dist = World.GetDistance(Game.PlayerPed.Position, stations[i].ppos.ToVector3());
				if (dist < 80)
				{
					StationDiBenzina stationinfo = GetStationInfo(i + 1);
					World.DrawMarker(MarkerType.VerticalCylinder, new Vector3(stations[i].ppos[0], stations[i].ppos[1], stations[i].ppos[2] - 1.00001f), new Vector3(0), new Vector3(0), new Vector3(1.1f, 1.1f, 1.3f), System.Drawing.Color.FromArgb(170, 0, 255, 0));
					if (dist < 1.3f)
					{
						if (stationinfo.ownerchar.ToLower() == Eventi.Player.FullName.ToLower())
						{
							HUD.ShowHelp("Premi ~INPUT_CONTEXT~ per gestire la stazione");
							if (!interactWait)
							{
								if (Game.IsControlJustPressed(0, Control.Context) || Game.IsDisabledControlJustPressed(0, Control.Context))
								{
									interactWait = true;
									BaseScript.TriggerServerEvent("lprp:businesses:checkcanmanage", i + 1);
								}
							}
						}
						else if (stationinfo.ownerchar == "")
						{
							HUD.ShowHelp("Questa stazione è in vendita a ~g~" + stations[i].sellprice.ToString() + "$~w~.\nPremi ~b~~INPUT_CONTEXT~~w~ per comprarla.");
							if (!interactWait)
							{
								if (Game.IsControlJustPressed(0, Control.Context) || Game.IsDisabledControlJustPressed(0, Control.Context))
								{
									interactWait = true;
									BaseScript.TriggerServerEvent("lprp:businesses:purchasestation", i + 1);
								}
							}
						}
					}
					Scaleform info = new Scaleform("mp_mission_name_freemode");
					while (!info.IsLoaded) await BaseScript.Delay(0);
					if (stationinfo.ownerchar == null || stationinfo.ownerchar == "")
						info.CallFunction("SET_MISSION_INFO", stationinfo.stationname, "\nProprietario: Nessuno", "", "", "", "", "", "", "", "");
					else
						info.CallFunction("SET_MISSION_INFO", stationinfo.stationname, "\nProprietario: " + stationinfo.ownerchar, "", "", "", "", "", "", "", "");
					info.Render3D(stations[i].ppos.ToVector3(), GetGameplayCamRot(0), new Vector3(2.0f, 2.0f, 2.0f));
				}
			}
			await Task.FromResult(0);
		}
	}
}