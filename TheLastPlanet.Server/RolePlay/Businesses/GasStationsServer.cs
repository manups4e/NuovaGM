using Settings.Shared.Config.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using TheLastPlanet.Server.Core;
using TheLastPlanet.Server.Core.PlayerChar;

namespace TheLastPlanet.Server.Businesses
{
    internal static class GasStationsServer
    {
        private static int stationCapacity = 1500;
        private static float fuelUnownedBuyPrice = 0.9f;
        private static int rentprice = Server.Impostazioni.Main.RentPriceGasPumps;

        public static void Init()
        {
            Server.Instance.AddEventHandler("lprp:caricaStazioniGasServer", new Action(SendStationsUpdate));
            Server.Instance.AddEventHandler("lprp:businesses:checkcanmanage", new Action<Player, int>(CheckCanManage));
            Server.Instance.AddEventHandler("lprp:businesses:getstationcash", new Action<Player, int>(GetStationCash));
            Server.Instance.AddEventHandler("lprp:businesses:changestation", new Action<Player, string, string, int, int, int, string>(ChangeStation));
            Server.Instance.AddEventHandler("lprp:businesses:sellstation", new Action<Player, string, int>(SellStation));
            Server.Instance.AddEventHandler("lprp:businesses:depositfuel", new Action<Player, int, int>(DepositFuel));
            Server.Instance.AddEventHandler("lprp:businesses:checkfuelforstation", new Action<Player, int>(CheckFuelStation));
            Server.Instance.AddEventHandler("lprp:businesses:removefuelfromstation", new Action<Player, int, int>(RemoveFuelStation));
            Server.Instance.AddEventHandler("lprp:businesses:purchasestation", new Action<Player, int>(PurchaseStation));
            Server.Instance.AddEventHandler("lprp:businesses:addmoneytostation", new Action<Player, int, int>(AddMoneyToStation));
            Server.Instance.AddEventHandler("lprp:businesses:saddfuel", new Action<Player, int, int>(SAddFuel));
            Server.Instance.AddEventHandler("lprp:businesses:saddmoney", new Action<Player, int, int>(SAddMoney));
            Server.Instance.AddEventHandler("lprp:businesses:sresetmanage", new Action<Player, int>(SResetManage));
            Server.Instance.AddEventHandler("lprp:businesses:addstationfunds", new Action<Player, int, int>(AddStationFunds));
            Server.Instance.AddEventHandler("lprp:businesses:remstationfunds", new Action<Player, int, int>(RemStationFunds));
        }

        public static async void SendStationsUpdate()
        {
            List<GasStations> playerstations = new();
            dynamic result = await Server.Instance.Query($"SELECT * FROM `businesses` WHERE businessid = {1}");
            await BaseScript.Delay(0);
            if (result.Count > 0)
                for (int i = 0; i < result.Count; i++)
                    playerstations.Add(new GasStations(result[i]));
            else
                Server.Logger.Error("BusinessServer.cs - Error getting stations from database");
            BaseScript.TriggerClientEvent("lprp:businesses:setstations", ConfigShared.SharedConfig.Main.Vehicles.gasstations.ToJson(), playerstations.ToJson());
        }

        public static async void checkRent(User p)
        {
            dynamic result = await Server.Instance.Query($"SELECT lastpaidrent, ownerchar, stationindex FROM businesses WHERE ownerchar = @charname", new { charname = p.FullName });
            await BaseScript.Delay(0);

            if (result.Count > 0)
                for (int i = 0; i < result.Count; i++)
                {
                    DateTime lastpaidrent = Convert.ToDateTime(result[i].lastpaidrent);
                    DateTime rentlimit = lastpaidrent.AddDays(7); // affitto ogni 7 giorni (modificabile)
                    DateTime timenow = DateTime.Now;

                    if (rentlimit < timenow)
                    {
                        await Server.Instance.Execute($"UPDATE `businesses` SET lastlogin = @now WHERE ownerchar = @charname AND stationindex = @index", new { now = timenow, charname = p.FullName, index = result[i].stationindex });

                        if (p.Bank < rentprice)
                        {
                            await Server.Instance.Execute($"UPDATE businesses SET identifier = '', ownerchar = '', stationname = 'Stazione di Rifornimento', thanksmessage = 'Grazie per il tuo lavoro!', cashwaiting = 0, fuelprice = 2, lastmanaged = 0, delivertype = 1, deliverallow = '', lastlogin = 0, lastpaidrent = 0 WHERE stationindex = @index", new { index = result[i].stationindex });
                            BaseScript.TriggerClientEvent(Server.Instance.GetPlayers[Convert.ToInt32(p.source)], "tlg:ShowNotification", "La tua stazione di rifornimento è stata ripresa per mancato pagamento.");
                        }
                        else
                        {
                            p.Bank -= rentprice;
                            await Server.Instance.Execute($"UPDATE businesses SET lastpaidrent = @now  WHERE stationindex = @idx", new { now = timenow, idx = result[i].stationindex });
                            BaseScript.TriggerClientEvent(Server.Instance.GetPlayers[Convert.ToInt32(p.source)], "tlg:ShowNotification", "{0} è stato pagato automaticamente per l'affitto della stazione.", rentprice);
                        }
                    }
                }
        }

        public static async void CheckCanManage([FromSource] Player p, int sidx)
        {
            dynamic result = await Server.Instance.Query($"SELECT `lastmanaged`, `cashwaiting`, `ownerchar` FROM `businesses` WHERE `stationindex` = @idx AND `identifier` = @ident", new { idx = sidx, ident = p.GetCurrentChar().Identifiers.Discord });
            await BaseScript.Delay(0);

            if (result != null)
            {
                DateTime lastmanaged = Convert.ToDateTime(result[0].lastmanaged);
                DateTime time = DateTime.Now;
                bool canmanage = lastmanaged.AddDays(1) < time;
                string managetime = "";
                string owner = result[0].ownerchar;
                if (!canmanage) managetime = lastmanaged.ToString("HH:mm");
                p.TriggerEvent("lprp:businesses:checkcanmanage", canmanage, sidx, managetime, result[0].cashwaiting);
            }
            else
            {
                Server.Logger.Error("lprp:businesses:checkcanmanage: errore a ottenere le info stazione o gli identifiers non combaciano");
                p.TriggerEvent("lprp:businesses:checkcanmanage", false);
            }
        }

        public static async void GetStationCash([FromSource] Player p, int sidx)
        {
            User user = Functions.GetUserFromPlayerId(p.Handle);
            dynamic result = await Server.Instance.Query($"SELECT `lastmanaged`, `cashwaiting` FROM `businesses` WHERE `stationindex` = @idx AND `identifier` = @id", new { idx = sidx, id = p.GetCurrentChar().Identifiers.Discord });
            await BaseScript.Delay(0);

            if (result.Count > 0)
            {
                int payout = (int)Math.Ceiling(result[0].cashwaiting);

                if (payout > 0)
                {
                    user.Bank += payout;
                    await Server.Instance.Execute($"UPDATE `businesses` SET `cashwaiting` = {0} WHERE `stationindex` = @idx AND `identifier` = @id", new { idx = sidx, id = p.GetCurrentChar().Identifiers.Discord });
                    p.TriggerEvent("lprp:businesses:getstationcash", payout);
                }
                else
                {
                    p.TriggerEvent("tlg:ShowNotification", "There are no funds available for this station.");
                }
            }
            else
            {
                Server.Logger.Error("lprp:businesses:getstationcash: error in obtaining mismatching station data or identifiers");
            }
        }

        public static async void ChangeStation([FromSource] Player p, string stationname, string thanksmessage, int fuelCost, int Manageid, int Deltype, string Deliverylist)
        {
            string name = stationname;
            string thanks = thanksmessage;
            int fuelcost = fuelCost;
            int manageid = Manageid;
            DateTime lastmanaged = DateTime.Now;
            int deltype = Deltype;
            string dellist = Deliverylist;
            string[] deliverylist = null;
            if (deltype == 3) deliverylist = dellist.Split(';');

            if (name != null && fuelcost > 0 && manageid != 0)
            {
                await Server.Instance.Execute($"UPDATE `businesses` SET `stationname` = @stName, `fuelprice` = @price, `thanksmessage` = @thx, `lastmanaged` = @last, `delivertype` = @deliver, `deliverallow` = @allow WHERE `stationindex` = @idx AND `identifier` = @id", new
                {
                    stName = name,
                    price = fuelcost,
                    thx = thanks,
                    last = lastmanaged.ToString("yyyy-MM-dd HH:mm:ss"),
                    deliver = deltype,
                    allow = deliverylist.ToJson(),
                    idx = manageid,
                    id = p.GetCurrentChar().Identifiers.Discord
                });
                SendStationsUpdate();
                p.TriggerEvent("tlg:ShowNotification", "Your station settings have been updated.");
            }
        }

        public static async void SellStation([FromSource] Player p, string sellname, int Manageid)
        {
            string name = sellname;
            int manageid = Manageid;

            if (name != null)
                foreach (Player a in Server.Instance.GetPlayers.ToList())
                {
                    User user = Functions.GetUserFromPlayerId(a.Handle);

                    if (user.FullName == name)
                    {
                        await Server.Instance.Execute($"UPDATE `businesses` SET `identifier` = @id, ownerchar = @owner WHERE stationindex = @idx", new { id = user.Identifiers.Discord, owner = user.FullName, idx = manageid });
                        SendStationsUpdate();
                        p.TriggerEvent("lprp:businesses:sellstation", true, name);
                    }
                    else
                    {
                        p.TriggerEvent("lprp:businesses:sellstation", false, "The person you are trying to sell the station to does not exist or is not online.");
                    }
                }
            else
                p.TriggerEvent("lprp:businesses:sellstation", false, "The person you are trying to sell the station to does not exist or is not online.");
        }

        public static async void DepositFuel([FromSource] Player p, int Index, int fuelfortank)
        {
            User user = Functions.GetUserFromPlayerId(p.Handle);
            int index = Index;
            int tankerfuel = fuelfortank;

            if (index > 0 && tankerfuel > 0)
            {
                dynamic result = await Server.Instance.Query($"SELECT * FROM `businesses` WHERE `stationindex` = @idx", new { idx = index });
                await BaseScript.Delay(0);

                if (result[0].identifier != "" && result[0].identifier != null && result[0].ownerchar != "" && result[0].ownerchar != null)
                {
                    int stationfuel = result[0].fuel;
                    int maxfuel = stationCapacity;
                    int overflow = 0;

                    if (stationfuel < stationCapacity)
                    {
                        if (stationfuel + tankerfuel <= stationCapacity)
                        {
                            stationfuel += tankerfuel;
                        }
                        else
                        {
                            overflow = stationCapacity - stationfuel - tankerfuel;
                            stationfuel = stationCapacity;
                        }

                        int deltype = result[0].delivertype;
                        bool allowdelivery = false;
                        int stationcash = result[0].cashwaiting;
                        int payout = (int)Math.Ceiling(fuelUnownedBuyPrice * (tankerfuel - overflow));

                        if (stationcash >= payout)
                        {
                            if (deltype == 1)
                            {
                                if (user.Identifiers.Discord == result[0].identifier && user.FullName == result[0].ownerchar) allowdelivery = true;
                            }
                            else if (deltype == 2)
                            {
                                allowdelivery = true;
                            }
                            else if (deltype == 3)
                            {
                                string[] allowList = (result[0].deliveryallow as string).FromJson<string[]>();

                                foreach (string s in allowList)
                                    if (user.FullName == s)
                                    {
                                        allowdelivery = true;

                                        break;
                                    }
                            }

                            if (allowdelivery)
                            {
                                int newcash = stationcash - payout;
                                await Server.Instance.Execute($"UPDATE `businesses` SET `fuel` = @fuel, `cashwaiting` = @cash WHERE `stationindex` = @idx", new { fuel = stationfuel, cash = newcash, idx = index });
                                user.Bank += payout;
                                p.TriggerEvent("lprp:fuel:depositfuel", true, (tankerfuel - overflow).ToString(), stationfuel.ToString(), overflow.ToString());
                            }
                            else
                            {
                                p.TriggerEvent("lprp:fuel:depositfuel", false, "This station only accepts deliveries from contracted personnel.");
                            }
                        }
                        else
                        {
                            p.TriggerEvent("lprp:fuel:depositfuel", false, "This station doesn't have enough money to pay you.");
                        }
                    }
                    else
                    {
                        p.TriggerEvent("lprp:fuel:depositfuel", false, "This station is already at maximum fuel capacity.");
                    }
                }
                else
                {
                    int stationfuel = result[0].fuel;
                    int maxfuel = stationCapacity;
                    int overflow = 0;

                    if (stationfuel < stationCapacity)
                    {
                        if (stationfuel + tankerfuel <= stationCapacity)
                        {
                            stationfuel += tankerfuel;
                        }
                        else
                        {
                            overflow = tankerfuel - (stationCapacity - stationfuel);
                            stationfuel = stationCapacity;
                        }

                        await Server.Instance.Execute($"UPDATE `businesses` SET `fuel` = @fuel WHERE `stationindex` = @idx", new { fuel = stationfuel, idx = index });
                        int pay = (int)Math.Ceiling((tankerfuel - overflow) * fuelUnownedBuyPrice);

                        if (pay > 0)
                        {
                            user.Bank += pay;
                            Server.Logger.Info($"The character {user.FullName} of the player {GetPlayerName(p.Handle)} was paid ${pay} for a fuel delivery (station not owned).");
                            BaseScript.TriggerEvent("lprp:serverlog", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + " -- Player {1}'s character {0} has been paid {2} $ for a fuel delivery (unowned station).", user.FullName, GetPlayerName(p.Handle), pay);
                        }

                        p.TriggerEvent("lprp:fuel:depositfuelnotowned", true, (tankerfuel - overflow).ToString(), stationfuel.ToString(), pay.ToString(), overflow.ToString());
                    }
                    else
                    {
                        p.TriggerEvent("lprp:fuel:depositfuelnotowned", false, "This station is already at maximum capacity.");
                    }
                }
            }
            else
            {
                Server.Logger.Error("BusinessServer.cs - lprp:businesses:depositfuel, index and fuel loading error");
            }
        }

        public static async void CheckFuelStation([FromSource] Player p, int index)
        {
            if (index > 0)
            {
                dynamic result = await Server.Instance.Query($"SELECT `fuel`, `fuelprice` FROM `businesses` WHERE `stationindex` = @idx", new { idx = index });
                await BaseScript.Delay(0);
                p.TriggerEvent("lprp:fuel:checkfuelforstation", result[0].fuel, result[0].fuelprice);
            }
        }

        public static async void RemoveFuelStation([FromSource] Player p, int stationindex, int addedfuel)
        {
            dynamic result = await Server.Instance.Query($"SELECT `fuel` FROM `businesses` WHERE `stationindex` = @idx", new { idx = stationindex });
            await BaseScript.Delay(0);
            int fuel = result[0].fuel;
            fuel -= addedfuel;
            await Server.Instance.Execute($"UPDATE `businesses` SET `fuel` = @fu WHERE `stationindex` = @idx", new { fu = fuel, idx = stationindex });
        }

        public static async void PurchaseStation([FromSource] Player p, int sidx)
        {
            if (sidx > 0)
            {
                User user = Functions.GetUserFromPlayerId(p.Handle);
                List<GasStation> stations = ConfigShared.SharedConfig.Main.Vehicles.gasstations;
                GasStation station = stations[sidx];
                int bankmoney = user.Bank;
                dynamic result = await Server.Instance.Query($"SELECT * FROM `businesses` WHERE `stationindex` = @idx", new { idx = sidx });
                await BaseScript.Delay(0);

                if (result[0].identifier != "" && result[0].ownerchar != "" && (result[0].identifier != null && result[0].ownerchar != null))
                {
                    p.TriggerEvent("lprp:businesses:purchasestation", false, "This station is already owned.");
                }
                else
                {
                    int sellprice = station.sellprice;
                    DateTime now = DateTime.Now;

                    if (bankmoney >= sellprice)
                    {
                        await Server.Instance.Execute($"UPDATE `businesses` SET `identifier`= @id, `ownerchar`= @owner, `Name`= @name, `stationname`= @stname, `lastpaidrent` = @last WHERE `stationindex`= {sidx}", new
                        {
                            id = user.Identifiers.Discord,
                            owner = user.FullName,
                            name = p.Name,
                            stname = "Una Pompa di Benzina",
                            last = now.ToString("yyyy-MM-dd HH:mm:ss"),
                            idx = sidx
                        });
                        user.Bank -= sellprice;
                        Server.Logger.Info($"Player character {user.FullName} {GetPlayerName(p.Handle)} paid {sellprice} for a Fuel Station.");
                        BaseScript.TriggerEvent("lprp:serverlog", now.ToString("dd/MM/yyyy, HH:mm:ss") + " -- Player {1}'s character {0} paid {2} for a Fuel Station.", user.FullName, GetPlayerName(p.Handle), sellprice);
                        SendStationsUpdate();
                        p.TriggerEvent("lprp:businesses:purchasestation", true, "", sidx, sellprice);
                    }
                    else
                    {
                        p.TriggerEvent("lprp:businesses:purchasestation", false, "You don't have enough money in the bank to cover the cost.");
                    }
                }
            }
        }

        public static async void AddMoneyToStation([FromSource] Player p, int stationindex, int Amount)
        {
            int sidx = stationindex;
            int amount = Amount;
            int oldamount = 0;
            int newamount = 0;
            dynamic result = await Server.Instance.Query($"SELECT `cashwaiting` FROM `businesses` WHERE `stationindex` = @idx", new { idx = sidx });
            await BaseScript.Delay(0);
            oldamount = result[0].cashwaiting;
            newamount = oldamount + amount;
            await Server.Instance.Execute($"UPDATE `businesses` SET `cashwaiting` = @cash WHERE stationindex = @idx", new { cash = newamount, idx = sidx });
        }

        #region commands ADMIN

        public static async void SAddMoney([FromSource] Player p, int closest, int Amount)
        {
            int index = closest;
            int amount = Amount;

            if (index > 0 && Amount > -1)
            {
                dynamic result = await Server.Instance.Query($"SELECT `cashwaiting` FROM `businesses` WHERE `stationindex` = @idx", new { idx = index });
                await BaseScript.Delay(0);
                int oldm = result[0].cashwaiting;
                int newm = oldm + amount;
                await Server.Instance.Execute($"UPDATE `businesses` SET `cashwaiting` = @cash WHERE `stationindex` = @idx ", new { cash = newm, idx = index });
                p.TriggerEvent("tlg:ShowNotification", "Added money to the station. New balance: {0}", newm.ToString());
            }
        }

        public static async void SAddFuel([FromSource] Player p, int closest, int Amount)
        {
            int index = closest;
            int amount = Amount;

            if (index > 0 && Amount > -1)
            {
                dynamic result = await Server.Instance.Query($"SELECT `fuel` FROM `businesses` WHERE `stationindex` = @idx", new { idx = index });
                await BaseScript.Delay(0);
                int oldf = result[0].fuel;
                int newf = oldf + amount;
                await Server.Instance.Execute($"UPDATE `businesses` SET `fuel` = @fuel WHERE `stationindex` = @idx", new { fuel = newf, idx = index });
                p.TriggerEvent("tlg:ShowNotification", "Fuel added. New level set to {0}", newf.ToString());
            }
        }

        #endregion

        public static async void SResetManage([FromSource] Player p, int closest)
        {
            int index = closest;

            if (index > 0)
            {
                await Server.Instance.Execute($"UPDATE `businesses` SET `lastmanaged` = @last WHERE `stationindex` = @idx", new { last = DateTime.MinValue.ToString(), idx = index });
                p.TriggerEvent("tlg:ShowNotification", "Management date reset.");
            }
        }

        public static async void AddStationFunds([FromSource] Player p, int manageid, int amount)
        {
            User user = Functions.GetUserFromPlayerId(p.Handle);
            int money = user.Money;
            dynamic result = await Server.Instance.Query($"SELECT `cashwaiting`, `ownerchar` FROM `businesses` WHERE `stationindex` = @idx", new { idx = manageid });
            await BaseScript.Delay(0);

            if (result.Count > 0)
                if (result[0].ownerchar.ToLower() == user.FullName.ToLower())
                {
                    if (money >= amount)
                    {
                        int smoney = (int)result[0].cashwaiting + amount;

                        if (smoney > 50000)
                        {
                            p.TriggerEvent("lprp:businesses:stationfundschange", false, "You cannot deposit money if your balance is greater than $50,000. Bring some money to the bank!");
                        }
                        else
                        {
                            user.Money -= amount;
                            Server.Logger.Info($"Player character {user.FullName} {GetPlayerName(p.Handle)} has deposited ${amount} at his gas station.");
                            BaseScript.TriggerEvent("lprp:serverlog", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + " -- Player {1}'s character {0} has deposited {2}$ at his gas station.", user.FullName, GetPlayerName(p.Handle), amount.ToString());
                            await Server.Instance.Execute($"UPDATE `businesses` SET `cashwaiting` = @cash WHERE `stationindex` = @idx", new { cash = smoney, idx = manageid });
                            p.TriggerEvent("lprp:businesses:stationfundschange", true, smoney.ToString());
                        }
                    }
                    else
                    {
                        p.TriggerEvent("lprp:businesses:stationfundschange", false, "You don't have enough money to deposit at the station.");
                    }
                }
        }

        public static async void RemStationFunds([FromSource] Player p, int manageid, int amount)
        {
            User user = Functions.GetUserFromPlayerId(p.Handle);
            dynamic result = await Server.Instance.Query($"SELECT `cashwaiting`, `ownerchar` FROM `businesses` WHERE `stationindex` = @idx", new { idx = manageid });
            await BaseScript.Delay(0);

            if (result.Count > 0)
                if (result[0].ownerchar.ToLower() == user.FullName.ToLower())
                {
                    int samount = (int)result[0].cashwaiting;

                    if (samount >= amount)
                    {
                        samount = (int)result[0].cashwaiting - amount;
                        user.Money += amount;
                        Server.Logger.Info($"Player character {user.FullName} {GetPlayerName(p.Handle)} has withdrawn ${amount} from his gas station");
                        BaseScript.TriggerEvent("lprp:serverlog", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + " -- Player {1}'s character {0} has withdrawn {2}$ from his gas station", user.FullName, GetPlayerName(p.Handle), amount.ToString());
                        await Server.Instance.Execute($"UPDATE businesses SET cashwaiting = @cash  WHERE stationindex = @idx", new { cash = samount, idx = manageid });
                        p.TriggerEvent("lprp:businesses:stationfundschange", true, samount.ToString());
                    }
                    else
                    {
                        p.TriggerEvent("lprp:businesses:stationfundschange", false, "The station does not have enough funds.");
                    }
                }
        }
    }
}