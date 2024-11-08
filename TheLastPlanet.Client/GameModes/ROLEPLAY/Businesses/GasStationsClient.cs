﻿using Settings.Shared.Config.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace TheLastPlanet.Client.GameMode.ROLEPLAY.Businesses
{
    internal class GasStationsClient
    {
        private static bool _interactWait = false;
        private static List<GasStation> _stations = new List<GasStation>();
        private static List<GasStations> _playerstations = new();
        private static Scaleform _info = new Scaleform("mp_mission_name_freemode");

        public static void Init()
        {
            AccessingEvents.OnRoleplaySpawn += Spawned;
            AccessingEvents.OnRoleplayLeave += onPlayerLeft;
        }
        public static void Spawned(PlayerClient client)
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

        public static void onPlayerLeft(PlayerClient client)
        {
            Client.Instance.RemoveEventHandler("lprp:businesses:setstations", new Action<string, string>(SetStations));
            Client.Instance.RemoveEventHandler("lprp:businesses:checkcanmanage", new Action<bool, int, string, int>(CheckCanManage));
            Client.Instance.RemoveEventHandler("lprp:businesses:getstationcash", new Action<int>(GetStationCash));
            Client.Instance.RemoveEventHandler("lprp:businesses:sellstation", new Action<bool, string, string>(SellStation));
            Client.Instance.RemoveEventHandler("lprp:businesses:purchasestation", new Action<bool, string, int, int>(PurchaseStation));
            Client.Instance.RemoveEventHandler("lprp:businesses:stationfundschange", new Action<bool, string>(StationFundsChange));
        }

        private static GasStations GetStationInfo(int index)
        {
            foreach (GasStations s in _playerstations)
                if (s.Stationindex == index)
                    return s;

            return null;
        }

        private GasStations GetPlayerstationsNearCoords(Vector3 pos)
        {
            int mstation = 0;

            for (int i = 0; i < _stations.Count; i++)
            {
                float dist = Cache.PlayerCache.MyPlayer.Position.Distance(pos);

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
            _playerstations = stazioniPlayer.FromJson<List<GasStations>>();
        }

        private static void CheckCanManage(bool canmanage, int manageid, string managetime, int funds)
        {
            if (canmanage)
            {
                GasStations station = _playerstations[manageid];
                string pfmtstr = "";
                string[] allow = station.Deliverallow.Split(';');
                if (allow.Length > 0) pfmtstr = allow.Aggregate(pfmtstr, (current, s) => current + " " + s);
                Client.Instance.NuiManager.SetFocus(true, true);
                Client.Instance.NuiManager.SendMessage(new
                {
                    showManager = true,
                    manageid,
                    station.Stationname,
                    station.Thanksmessage,
                    fuelcost = station.Fuelprice,
                    deltype = station.Delivertype,
                    funds,
                    deliverylist = pfmtstr
                });
            }
            else
            {
                HUD.ShowNotification("You can handle the Station only once every 24 hours.~n~Come back tomorrow after ~r~" + managetime + "~w~.", ColoreNotifica.RedDifferent);
            }

            _interactWait = false;
        }

        private static void GetStationCash(int payout)
        {
            HUD.ShowNotification("You were paid ~b~" + payout + "$~w~ from this station.");
            _interactWait = false;
        }

        private static void SellStation(bool success, string msg, string name)
        {
            if (success)
            {
                Client.Instance.NuiManager.SendMessage(new { closeManager = true });
                Client.Instance.NuiManager.SetFocus(false, false);
                HUD.ShowNotification("Your station has been sold to ~b~" + name + "~w~.", ColoreNotifica.GreenLight);
            }
            else
            {
                HUD.ShowNotification(msg, ColoreNotifica.Red);
            }
        }

        private static void PurchaseStation(bool success, string msg, int sidx, int sellprice)
        {
            if (success)
            {
                HUD.ShowNotification($"Congratulations for buying your new station!\n~b~ {sellprice}~w~ have been spent.", ColoreNotifica.GreenLight);
                BaseScript.TriggerServerEvent("lprp:businesses:checkcanmanage", sidx);
            }
            else
            {
                HUD.ShowNotification(msg, ColoreNotifica.Red);
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
                float dist = Cache.PlayerCache.MyPlayer.Position.Distance(_stations[i].ppos);

                if (!(dist < 80)) continue;
                GasStations stationinfo = GetStationInfo(i + 1);
                World.DrawMarker(MarkerType.VerticalCylinder, new Vector3(_stations[i].ppos[0], _stations[i].ppos[1], _stations[i].ppos[2] - 1.00001f), new Vector3(0), new Vector3(0), new Vector3(1.1f, 1.1f, 1.3f), System.Drawing.Color.FromArgb(170, 0, 255, 0));

                if (dist < 1.3f)
                    if (!Cache.PlayerCache.MyPlayer.Status.PlayerStates.InVehicle)
                    {
                        if (string.Equals(stationinfo.Ownerchar, Cache.PlayerCache.MyPlayer.User.FullName, StringComparison.CurrentCultureIgnoreCase))
                        {
                            HUD.ShowHelp("Press ~INPUT_CONTEXT~ to manage your station");

                            if (!_interactWait)
                                if (Input.IsControlJustPressed(Control.Context) || Input.IsDisabledControlJustPressed(Control.Context))
                                {
                                    _interactWait = true;
                                    BaseScript.TriggerServerEvent("lprp:businesses:checkcanmanage", i + 1);
                                }
                        }
                        else if (stationinfo.Ownerchar == "")
                        {
                            HUD.ShowHelp("This station is on sell for ~g~" + _stations[i].sellprice.ToString() + "$~w~.\nPress ~b~~INPUT_CONTEXT~~w~ to buy it.");

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
                if (string.IsNullOrEmpty(stationinfo.Ownerchar))
                    _info.CallFunction("SET_MISSION_INFO", stationinfo.Stationname, "\nOwner: NONE", "", "", "", "", "", "", "", "");
                else
                    _info.CallFunction("SET_MISSION_INFO", stationinfo.Stationname, "\nOwner: " + stationinfo.Ownerchar, "", "", "", "", "", "", "", "");
                _info.Render3D(_stations[i].ppos, GetGameplayCamRot(0), new Vector3(2.0f, 2.0f, 2.0f));
            }

            await Task.FromResult(0);
        }
    }
}