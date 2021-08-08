using CitizenFX.Core;
using TheLastPlanet.Client.MODALITA.ROLEPLAY.Core;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.NativeUI;
using TheLastPlanet.Shared;
using System.Collections.Generic;
using System.Linq;
using Impostazioni.Client.Configurazione.Negozi.Generici;

namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Businesses
{
	internal static class NegoziBusiness
	{
		public static void Init()
		{
			/*
			Client.Instance().AddEventHandler("lprp:negozi:setstations", new Action<string, string>(SetStations));
			Client.Instance().AddEventHandler("lprp:negozi:checkcanmanage", new Action<bool, int, string, int>(CheckCanManage));
			Client.Instance().AddEventHandler("lprp:negozi:getstationcash", new Action<int>(GetStationCash));
			Client.Instance().AddEventHandler("lprp:negozi:sellstation", new Action<bool, string, string>(SellStation));
			Client.Instance().AddEventHandler("lprp:negozi:purchasestation", new Action<bool, string, int, int>(PurchaseStation));
			Client.Instance().AddEventHandler("lprp:negozi:stationfundschange", new Action<bool, string>(StationFundsChange));
			Client.Instance().AddEventHandler("lprp:negozi:manage", new Action<dynamic>(Manage));
			Client.Instance().AddEventHandler("lprp:negozi:sellstation", new Action<dynamic>(SellStation));
			Client.Instance().AddEventHandler("lprp:negozi:notification", new Action<dynamic>(Notification));
			Client.Instance().AddEventHandler("lprp:negozi:addstationfunds", new Action<dynamic>(AddStationFunds));
			Client.Instance().AddEventHandler("lprp:negozi:remstationfunds", new Action<dynamic>(RemStationFunds));
			*/
		}

		public static void Stop()
		{
		}

		public static void NegozioPubblico(string tipo)
		{
			KeyValuePair<string, string> neg = Main.Textures[tipo];
			string description = "";
			List<OggettoVendita> oggettiDaAggiungere = Client.Impostazioni.RolePlay.Negozi.NegoziGenerici.OggettiDaVendere.shared;

			switch (tipo)
			{
				case "247":
					description = "Aperti 24/7!";
					Client.Impostazioni.RolePlay.Negozi.NegoziGenerici.OggettiDaVendere.tfs.ForEach(x => oggettiDaAggiungere.Add(x));

					break;
				case "ltd":
					description = "Non è mica infinita!";
					Client.Impostazioni.RolePlay.Negozi.NegoziGenerici.OggettiDaVendere.ltd.ForEach(x => oggettiDaAggiungere.Add(x));

					break;
				case "rq":
					description = "I liquori migliori!";
					Client.Impostazioni.RolePlay.Negozi.NegoziGenerici.OggettiDaVendere.rq.ForEach(x => oggettiDaAggiungere.Add(x));

					break;
			}

			UIMenu Negozio = new UIMenu("", description, new System.Drawing.PointF(1470, 500), neg.Key, neg.Value);
			HUD.MenuPool.Add(Negozio);

			foreach (OggettoVendita ogg in oggettiDaAggiungere)
			{
				UIMenuItem oggetto = new UIMenuItem(ConfigShared.SharedConfig.Main.Generici.ItemList[ogg.oggetto].label, "");
				if (Cache.PlayerCache.MyPlayer.User.Money >= ogg.prezzo || Cache.PlayerCache.MyPlayer.User.Bank >= ogg.prezzo)
					oggetto.SetRightLabel($"~g~${ogg.prezzo}");
				else
					oggetto.SetRightLabel($"~r~${ogg.prezzo}");
				Negozio.AddItem(oggetto);
			}

			Negozio.OnItemSelect += (menu, item, index) =>
			{
				string nome = ConfigShared.SharedConfig.Main.Generici.ItemList.FirstOrDefault(x => x.Value.label == item.Text).Key;

				if (!string.IsNullOrEmpty(nome))
				{
					OggettoVendita ogg = oggettiDaAggiungere.FirstOrDefault(x => x.oggetto == nome);

					if (Cache.PlayerCache.MyPlayer.User.Money >= ogg.prezzo)
					{
						BaseScript.TriggerServerEvent("lprp:removemoney", ogg.prezzo);
						BaseScript.TriggerServerEvent("lprp:addIntenvoryItem", ogg.oggetto, 1, 1f);
					}
					else
					{
						if (Cache.PlayerCache.MyPlayer.User.Bank >= ogg.prezzo)
						{
							BaseScript.TriggerServerEvent("lprp:removebank", ogg.prezzo);
							BaseScript.TriggerServerEvent("lprp:addIntenvoryItem", ogg.oggetto, 1, 1f);
						}
						else
						{
							HUD.ShowNotification("Non hai abbastanza denaro!", NotificationColor.Red, true);
						}
					}
				}
			};
			Negozio.Visible = true;
		}

		/*
		public void SetStations(string lista, string license)
		{
			List<dynamic> newlist = new List<dynamic>();
			newlist = JsonConvert.DeserializeObject<List<dynamic>>(lista);
			identifier = license;
			if (stations.Count > 0)
				stations.Clear();
			if (playerstations.Count > 0)
				playerstations.Clear();
			for (int i = 0; i < newlist[0].Count; i++)
				stations.Add(new GasStation(newlist[0][i]));
			for (int i = 0; i < newlist[1].Count; i++)
				playerstations.Add(new StationDiBenzina((string)newlist[1][i]["identifier"].Value, (string)newlist[1][i]["Name"], (int)newlist[1][i]["businessid"].Value, Convert.ToDateTime(newlist[1][i]["lastpaidrent"].Value), (string)newlist[1][i]["ownerchar"].Value, (int)newlist[1][i]["stationindex"].Value, (string)newlist[1][i]["stationname"].Value, (int)newlist[1][i]["cashwaiting"].Value, (int)newlist[1][i]["fuel"].Value, (int)newlist[1][i]["fuelprice"].Value, Convert.ToDateTime(newlist[1][i]["lastmanaged"].Value), Convert.ToDateTime(newlist[1][i]["lastlogin"].Value), (string)newlist[1][i]["deliverallow"].Value, (string)newlist[1][i]["thanksmessage"].Value, (int)newlist[1][i]["delivertype"].Value));
		}

		public void CheckCanManage(bool canmanage, int manageid, string managetime, int funds)
		{
			if (canmanage)
			{
				StationDiBenzina station = playerstations[manageid];
				string pfmtstr = "";
				string[] allow = station.deliverallow.Split(';');
				if (allow.Length > 0)
				{
					foreach (string s in allow)
					{
						pfmtstr += " " + s;
					}
				}

				SetNuiFocus(true, true);
				string a = "{\"showManager\":\"true\",\"manageid\":\"" + manageid + "\",\"stationname\":\"" + station.stationname + "\",\"thanksmessage\":\"" + station.thanksmessage + "\",\"fuelcost\":\"" + station.fuelprice + "\",\"deltype\":\"" + station.delivertype + "\",\"funds\":\"" + funds + "\",\"deliverylist\":\"" + pfmtstr + "\"}";
				SendNuiMessage(a);

			}
			else
				Funzioni.ShowNotification("Puoi gestire la stazione una volta al giorno.\nTorna domani alle ore ~r~" + managetime + "~w~.", NotificationColor.Cyan);
			interactWait = false;
		}

		public void GetStationCash(int payout)
		{
			Funzioni.ShowNotification("Sei stato pagato ~b~" + payout + "$~w~ da questa stazione.");
			interactWait = false;
		}

		public void SellStation(bool success, string msg, string name)
		{
			if (success)
			{
				string a = "{\"closeManager\":\"true\"}";
				SendNuiMessage(a);
				SetNuiFocus(false, false);
				Funzioni.ShowNotification("La tua Stazione è stata venduta a ~b~" + name + "~w~.", NotificationColor.GreenLight);
			}
			else
				Funzioni.ShowNotification(msg, NotificationColor.Red);
		}

		public void PurchaseStation(bool success, string msg, int sidx, int sellprice)
		{
			if (success)
			{
				Funzioni.ShowNotification("Congratulazioni nell'acquisto della tua nuova Stazione!\n~b~ " + sellprice + "$~w~ sono stati prelevati dal tuo conto bancario.", NotificationColor.GreenLight);
				TriggerServerEvent("lprp:businesses:checkcanmanage", sidx);
			}
			else
			{
				Funzioni.ShowNotification(msg, NotificationColor.Red);
				interactWait = false;
			}
		}

		public void StationFundsChange(bool success, string msg)
		{
			if (success)
			{
				string a = "{\"setFunds\":\"true\",\"stationmoney\":\"" + msg + "\"}";
				SendNuiMessage(a);
			}
			else
			{
				Funzioni.ShowNotification(msg);
				string a = "{\"setFunds\":\"true\",\"text\":\"" + msg + "\"}";
				SendNuiMessage(a);
			}
		}

		static privatevoid Manage(dynamic data)
		{
			string name = data.stationname;
			int fuelcost = Convert.ToInt32(data.fuelcost);
			int manageid = Convert.ToInt32(data.manageid);
			string thks = data.thanksmessage;
			int deltype = Convert.ToInt32(data.deltype);
			string deliverylist = data.deliverylist;

			Debug.WriteLine(name);
			Debug.WriteLine(thks);

			TriggerServerEvent("lprp:businesses:changestation", name, thks, fuelcost, manageid, deltype, deliverylist);
			SetNuiFocus(false, false);
		}


		static privatevoid SellStation(dynamic data)
		{
			string sellname = data.sellname;
			int manageid = Convert.ToInt32(data.manageid);
			TriggerServerEvent("lprp:businesses:sellstation", sellname, manageid);
		}

		static privatevoid Notification(dynamic data)
		{
			Funzioni.ShowNotification(data.text);
		}

		static privatevoid MenuClosed(dynamic data)
		{
			SetNuiFocus(false, false);
		}

		static privatevoid AddStationFunds(dynamic data)
		{
			int amount = Convert.ToInt32(data.amount);
			int manageid = Convert.ToInt32(data.manageid);
			TriggerServerEvent("lprp:businesses:addstationfunds", manageid, amount);
		}


		void RemStationFunds(dynamic data)
		{
			int amount = Convert.ToInt32(data.amount);
			int manageid = Convert.ToInt32(data.manageid);
			TriggerServerEvent("lprp:businesses:remstationfunds", manageid, amount);
		}

		*/
		/*
		public async Task BusinessesPumps()
		{
			for (int i = 0; i < stations.Count; i++)
			{
				float dist = Vector3.Distance(Cache.PlayerPed.Position, new Vector3(stations[i].ppos.x, stations[i].ppos.y, stations[i].ppos.z));
				if (dist < 80)
				{
					StationDiBenzina stationinfo = GetStationInfo(i + 1);
					World.DrawMarker(MarkerType.VerticalCylinder, new Vector3(stations[i].ppos.x, stations[i].ppos.y, stations[i].ppos.z - 1.00001f), new Vector3(0), new Vector3(0), new Vector3(1.1f, 1.1f, 1.3f), System.Drawing.Color.FromArgb(170, 0, 255, 0));
					if (dist < 1.3f)
					{
						if (stationinfo.ownerchar.ToLower() == Cache.Char.FullName.ToLower())
						{
							Funzioni.ShowHelp("Premi ~INPUT_CONTEXT~ per gestire la stazione");
							if (!interactWait)
							{
								if (Input.IsControlJustPressed(Control.Context) || Input.IsDisabledControlJustPressed(Control.Context))
								{
									interactWait = true;
									TriggerServerEvent("lprp:businesses:checkcanmanage", i + 1);
								}
							}
						}
						else if (stationinfo.ownerchar == "")
						{
							Funzioni.ShowHelp("Questa stazione è in vendita a ~g~" + stations[i].sellprice.ToString() + "$~w~.\nPremi ~b~~INPUT_CONTEXT~~w~ per comprarla.");
							if (!interactWait)
							{
								if (Input.IsControlJustPressed(Control.Context) || Input.IsDisabledControlJustPressed(Control.Context))
								{
									interactWait = true;
									TriggerServerEvent("lprp:businesses:purchasestation", i + 1);
								}
							}
						}
					}
					Scaleform info = new Scaleform("mp_mission_name_freemode");
					while (!info.IsLoaded) await Delay(0);
					if (stationinfo.ownerchar == null || stationinfo.ownerchar == "")
						info.CallFunction("SET_MISSION_INFO", stationinfo.stationname, "\nProprietario: Nessuno", "", "", "", "", "", "", "", "");
					else
						info.CallFunction("SET_MISSION_INFO", stationinfo.stationname, "\nProprietario: " + stationinfo.ownerchar, "", "", "", "", "", "", "", "");
					info.Render3D(new Vector3(stations[i].ppos.x, stations[i].ppos.y, stations[i].ppos.z), GetGameplayCamRot(0), new Vector3(2.0f, 2.0f, 2.0f));
					//Funzioni.ShowHelp("~g~" + stationinfo.stationname + "\nProprietario: " + stationinfo.ownerchar);
				}
			}
		}
		*/
	}
}