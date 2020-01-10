using CitizenFX.Core;
using Newtonsoft.Json;
using NuovaGM.Server.gmPrincipale;
using NuovaGM.Shared;
using System;
using System.Collections.Generic;
using static CitizenFX.Core.Native.API;

namespace NuovaGM.Server.Businesses
{
	static class PompeDiBenzinaServer
	{
		static int stationCapacity = 1500;
		static float fuelUnownedBuyPrice = 0.9f;
		static int rentprice = ConfigServer.Conf.Main.RentPricePompeDiBenzina;

		public static void Init()
		{
			Server.GetInstance.RegisterEventHandler("lprp:caricaStazioniGasServer", new Action(SendStationsUpdate));
			Server.GetInstance.RegisterEventHandler("lprp:businesses:checkcanmanage", new Action<Player, int>(CheckCanManage));
			Server.GetInstance.RegisterEventHandler("lprp:businesses:getstationcash", new Action<Player, int>(GetStationCash));
			Server.GetInstance.RegisterEventHandler("lprp:businesses:changestation", new Action<Player, string, string, int, int, int, string>(ChangeStation));
			Server.GetInstance.RegisterEventHandler("lprp:businesses:sellstation", new Action<Player, string, int>(SellStation));
			Server.GetInstance.RegisterEventHandler("lprp:businesses:depositfuel", new Action<Player, int, int>(DepositFuel));
			Server.GetInstance.RegisterEventHandler("lprp:businesses:checkfuelforstation", new Action<Player, int>(CheckFuelStation));
			Server.GetInstance.RegisterEventHandler("lprp:businesses:removefuelfromstation", new Action<Player, int, int>(RemoveFuelStation));
			Server.GetInstance.RegisterEventHandler("lprp:businesses:purchasestation", new Action<Player, int>(PurchaseStation));
			Server.GetInstance.RegisterEventHandler("lprp:businesses:addmoneytostation", new Action<Player, int, int>(AddMoneyToStation));
			Server.GetInstance.RegisterEventHandler("lprp:businesses:saddfuel", new Action<Player, int, int>(SAddFuel));
			Server.GetInstance.RegisterEventHandler("lprp:businesses:saddmoney", new Action<Player, int, int>(SAddMoney));
			Server.GetInstance.RegisterEventHandler("lprp:businesses:sresetmanage", new Action<Player, int>(SResetManage));
			Server.GetInstance.RegisterEventHandler("lprp:businesses:addstationfunds", new Action<Player, int, int>(AddStationFunds));
			Server.GetInstance.RegisterEventHandler("lprp:businesses:remstationfunds", new Action<Player, int, int>(RemStationFunds));
		}

		public static async void SendStationsUpdate()
		{
			List<GasStation> stations = ConfigShared.SharedConfig.Main.Veicoli.gasstations;
			List<StationDiBenzina> playerstations = new List<StationDiBenzina>();

			dynamic result = await Server.GetInstance.Query($"SELECT * FROM `businesses` WHERE businessid = {1}");
			if (result.Count > 0)
				for (int i = 0; i < result.Count; i++)
					playerstations.Add(new StationDiBenzina(result[i]));
			else
				Log.Printa(LogType.Error, "BusinessServer.cs - Errore a prendere le stazioni dal database");
			List<dynamic> lista = new List<dynamic>() { stations, playerstations };
			BaseScript.TriggerClientEvent("lprp:businesses:setstations", JsonConvert.SerializeObject(lista));
		}

		public static async void checkRent(User p)
		{
			dynamic result = await Server.GetInstance.Query($"SELECT lastpaidrent, ownerchar, stationindex FROM businesses WHERE ownerchar = @charname", new { charname = p.FullName });
			if (result.Count > 0)
			{
				for (int i = 0; i < result.Count; i++)
				{
					DateTime lastpaidrent = Convert.ToDateTime(result[i].lastpaidrent);
					DateTime rentlimit = lastpaidrent.AddDays(7); // affitto ogni 7 giorni (modificabile)
					DateTime timenow = DateTime.Now;
					if (rentlimit < timenow)
					{
						await Server.GetInstance.Execute($"UPDATE `businesses` SET lastlogin = @now WHERE ownerchar = @charname AND stationindex = @index",
							new { now = timenow, charname = p.FullName, index = result[i].stationindex });
						if (p.Bank < rentprice)
						{
							await Server.GetInstance.Execute($"UPDATE businesses SET identifier = '', ownerchar = '', stationname = 'Stazione di Rifornimento', thanksmessage = 'Grazie per il tuo lavoro!', cashwaiting = 0, fuelprice = 2, lastmanaged = 0, delivertype = 1, deliverallow = '', lastlogin = 0, lastpaidrent = 0 WHERE stationindex = @index", new { index = result[i].stationindex });
							BaseScript.TriggerClientEvent(Server.GetInstance.GetPlayers[Convert.ToInt32(p.source)], "lprp:ShowNotification", "La tua stazione di rifornimento è stata ripresa per mancato pagamento.");
						}
						else
						{
							p.Bank -= rentprice;
							await Server.GetInstance.Execute($"UPDATE businesses SET lastpaidrent = @now  WHERE stationindex = @idx",
								new { now = timenow, idx = result[i].stationindex });
							BaseScript.TriggerClientEvent(Server.GetInstance.GetPlayers[Convert.ToInt32(p.source)], "lprp:ShowNotification", "{0} è stato pagato automaticamente per l'affitto della stazione.", rentprice);
						}
					}
				}
			}
		}

		public static async void CheckCanManage([FromSource] Player p, int sidx)
		{
			dynamic result = await Server.GetInstance.Query($"SELECT `lastmanaged`, `cashwaiting`, `ownerchar` FROM `businesses` WHERE `stationindex` = @idx AND `identifier` = @ident", new { idx = sidx, ident = License.GetLicense(p, Identifier.Discord) });
			if (result != null)
			{
				DateTime lastmanaged = Convert.ToDateTime(result[0].lastmanaged);
				DateTime time = DateTime.Now;
				bool canmanage = (lastmanaged.AddDays(1) < time);
				string managetime = "";
				string owner = result[0].ownerchar;
				if (!canmanage)
					managetime = lastmanaged.ToString("HH:mm");
				BaseScript.TriggerClientEvent(p, "lprp:businesses:checkcanmanage", canmanage, sidx, managetime, result[0].cashwaiting);
			}
			else
			{
				Log.Printa(LogType.Error, "lprp:businesses:checkcanmanage: errore a ottenere le info stazione o gli identifiers non combaciano");
				BaseScript.TriggerClientEvent(p, "lprp:businesses:checkcanmanage", false);
			}
		}

		public static async void GetStationCash([FromSource] Player p, int sidx)
		{
			User user = ServerEntrance.PlayerList[p.Handle];
			dynamic result = await Server.GetInstance.Query($"SELECT `lastmanaged`, `cashwaiting` FROM `businesses` WHERE `stationindex` = @idx AND `identifier` = @id", new { idx = sidx, id = License.GetLicense(p, Identifier.Discord) });
			if (result.Count > 0)
			{
				int payout = (int)Math.Ceiling(result[0].cashwaiting);
				if (payout > 0)
				{
					user.Bank += payout;
					await Server.GetInstance.Execute($"UPDATE `businesses` SET `cashwaiting` = {0} WHERE `stationindex` = @idx AND `identifier` = @id", new
					{
						idx = sidx,
						id = License.GetLicense(p, Identifier.Discord)
					});
					BaseScript.TriggerClientEvent(p, "lprp:businesses:getstationcash", payout);
				}
				else
					p.TriggerEvent("lprp:ShowNotification", "Non ci sono fondi disponibili per questa stazione.");
			}
			else
				Log.Printa(LogType.Error, "lprp:businesses:getstationcash: errore nell'ottenere dati stazione o identifier non combacianti");
		}

		public static async void ChangeStation([FromSource]Player p, string stationname, string thanksmessage, int fuelCost, int Manageid, int Deltype, string Deliverylist)
		{
			string name = stationname;
			string thanks = thanksmessage;
			int fuelcost = (int)fuelCost;
			int manageid = (int)Manageid;
			DateTime lastmanaged = DateTime.Now;
			int deltype = (int)Deltype;
			string dellist = Deliverylist;
			string[] deliverylist = null;
			if (deltype == 3)
				deliverylist = dellist.Split(';');

			if (name != null && fuelcost > 0 && manageid != 0)
			{
				await Server.GetInstance.Execute($"UPDATE `businesses` SET `stationname` = @stName, `fuelprice` = @price, `thanksmessage` = @thx, `lastmanaged` = @last, `delivertype` = @deliver, `deliverallow` = @allow WHERE `stationindex` = @idx AND `identifier` = @id", new
				{
					stName = name,
					price = fuelcost,
					thx = thanks,
					last = lastmanaged.ToString("yyyy-MM-dd HH:mm:ss"),
					deliver = deltype,
					allow = JsonConvert.SerializeObject(deliverylist),
					idx = manageid,
					id = License.GetLicense(p, Identifier.Discord)
				});
				SendStationsUpdate();
				BaseScript.TriggerClientEvent(p, "lprp:ShowNotification", "Le impostazioni della tua stazione sono state aggiornate.");
			}
		}

		public static async void SellStation([FromSource] Player p, string sellname, int Manageid)
		{
			string name = sellname;
			int manageid = (int)Manageid;
			if (name != null)
			{
				foreach (Player a in Server.GetInstance.GetPlayers)
				{
					if (ServerEntrance.PlayerList[a.Handle].FullName == name)
					{
						await Server.GetInstance.Execute($"UPDATE `businesses` SET `identifier` = @id, ownerchar = @owner WHERE stationindex = @idx", new
						{
							id = ServerEntrance.PlayerList[a.Handle].identifiers.discord,
							owner = ServerEntrance.PlayerList[a.Handle].FullName,
							idx = manageid
						});
						SendStationsUpdate();
						BaseScript.TriggerClientEvent(p, "lprp:businesses:sellstation", true, name);
					}
					else
						BaseScript.TriggerClientEvent(p, "lprp:businesses:sellstation", false, "La persona a cui tenti di vendere la stazione non esiste o non è online.");
				}
			}
			else
				BaseScript.TriggerClientEvent(p, "lprp:businesses:sellstation", false, "La persona a cui tenti di vendere la stazione non esiste o non è online.");
		}

		public static async void DepositFuel([FromSource]Player p, int Index, int fuelfortank)
		{
			User user = ServerEntrance.PlayerList[p.Handle];
			int index = Index;
			int tankerfuel = fuelfortank;
			if (index > 0 && tankerfuel > 0)
			{
				List<GasStation> stations = ConfigShared.SharedConfig.Main.Veicoli.gasstations;

				dynamic result = await Server.GetInstance.Query($"SELECT * FROM `businesses` WHERE `stationindex` = @idx", new { idx = index });
				if (result[0].identifier != "" && result[0].identifier != null && result[0].ownerchar != "" && result[0].ownerchar != null)
				{
					int stationfuel = result[0].fuel;
					int maxfuel = stationCapacity;
					int overflow = 0;
					if (stationfuel < stationCapacity)
					{
						if ((stationfuel + tankerfuel) <= stationCapacity)
						{
							stationfuel += tankerfuel;
						}
						else
						{
							overflow = (stationCapacity - stationfuel) - tankerfuel;
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
								if (user.identifiers.discord == result[0].identifier && user.FullName == result[0].ownerchar)
								{
									allowdelivery = true;
								}
							}
							else if (deltype == 2)
							{
								allowdelivery = true;
							}
							else if (deltype == 3)
							{
								string[] allowList = JsonConvert.DeserializeObject<string[]>(result[0].deliveryallow);
								foreach (string s in allowList)
								{
									if (user.FullName == s)
									{
										allowdelivery = true;
										break;
									}
								}
							}
							if (allowdelivery)
							{
								int newcash = stationcash - payout;
								await Server.GetInstance.Execute($"UPDATE `businesses` SET `fuel` = @fuel, `cashwaiting` = @cash WHERE `stationindex` = @idx", new
								{
									fuel = stationfuel,
									cash = newcash,
									idx = index
								});
								user.Bank += payout;
								BaseScript.TriggerClientEvent(p, "lprp:fuel:depositfuel", true, (tankerfuel - overflow).ToString(), stationfuel.ToString(), overflow.ToString());
							}
							else
								BaseScript.TriggerClientEvent(p, "lprp:fuel:depositfuel", false, "Questa stazione accetta consegne solo da personale con contratto.");
						}
						else
							BaseScript.TriggerClientEvent(p, "lprp:fuel:depositfuel", false, "Questa stazione non ha abbastanza denaro per pagarti.");
					}
					else
						BaseScript.TriggerClientEvent(p, "lprp:fuel:depositfuel", false, "Questa stazione è gia al massimo della capienza di carburante.");
				}
				else
				{
					
					int stationfuel = result[0].fuel;
					int maxfuel = stationCapacity;
					int overflow = 0;
					if (stationfuel < stationCapacity)
					{
						if ((stationfuel + tankerfuel) <= stationCapacity)
							stationfuel += tankerfuel;
						else
						{
							overflow = tankerfuel - (stationCapacity - stationfuel);
							stationfuel = stationCapacity;
						}
						await Server.GetInstance.Execute($"UPDATE `businesses` SET `fuel` = @fuel WHERE `stationindex` = @idx", new
						{
							fuel = stationfuel,
							idx = index
						});
						int pay = (int)Math.Ceiling((tankerfuel - overflow) * fuelUnownedBuyPrice);
						if (pay > 0)
						{
							user.Bank += pay;
							Log.Printa(LogType.Info, $"Il personaggio {user.FullName} del player {GetPlayerName(p.Handle)} è stato pagato ${pay} per una consegna di carburante (stazione non posseduta).");
							BaseScript.TriggerEvent("lprp:serverlog", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + " -- Il personaggio {0} del player {1} è stato pagato {2}$ per una consegna di carburante (stazione non posseduta).", user.FullName, GetPlayerName(p.Handle), pay);
						}
						BaseScript.TriggerClientEvent(p, "lprp:fuel:depositfuelnotowned", true, (tankerfuel - overflow).ToString(), stationfuel.ToString(), pay.ToString(), overflow.ToString());
					}
					else
						BaseScript.TriggerClientEvent(p, "lprp:fuel:depositfuelnotowned", false, "Questa stazione è già alla massima capienza.");
				}
			}
			else
				Log.Printa(LogType.Error, "BusinessServer.cs:373 - lprp:businesses:depositfuel, errore caricamento index e fuel");
		}

		public static async void CheckFuelStation([FromSource] Player p, int index)
		{
			if (index > 0)
			{
				dynamic result = await Server.GetInstance.Query($"SELECT `fuel`, `fuelprice` FROM `businesses` WHERE `stationindex` = @idx", new { idx = index });
				BaseScript.TriggerClientEvent(p, "lprp:fuel:checkfuelforstation", result[0].fuel, result[0].fuelprice);
			}
		}

		public static async void RemoveFuelStation([FromSource]Player p, int stationindex, int addedfuel)
		{
			dynamic result = await Server.GetInstance.Query($"SELECT `fuel` FROM `businesses` WHERE `stationindex` = @idx", new { idx = stationindex });
			int fuel = result[0].fuel;
			fuel -= addedfuel;
			await Server.GetInstance.Execute($"UPDATE `businesses` SET `fuel` = @fu WHERE `stationindex` = @idx", new { fu = fuel, idx = stationindex });
		}

		public static async void PurchaseStation([FromSource] Player p, int sidx)
		{
			if (sidx > 0)
			{
				User user = ServerEntrance.PlayerList[p.Handle];
				List<GasStation> stations = ConfigShared.SharedConfig.Main.Veicoli.gasstations;
				GasStation station = stations[sidx];
				int bankmoney = user.Bank;
				dynamic result = await Server.GetInstance.Query($"SELECT * FROM `businesses` WHERE `stationindex` = @idx", new { idx = sidx });
				if ((result[0].identifier != "" && result[0].ownerchar != "") && (result[0].identifier != null && result[0].ownerchar != null))
					BaseScript.TriggerClientEvent(p, "lprp:businesses:purchasestation", false, "Questa stazione è già posseduta.");
				else
				{
					int sellprice = station.sellprice;
					DateTime now = DateTime.Now;
					if (bankmoney >= sellprice)
					{
						await Server.GetInstance.Execute($"UPDATE `businesses` SET `identifier`= @id, `ownerchar`= @owner, `Name`= @name, `stationname`= @stname, `lastpaidrent` = @last WHERE `stationindex`= {sidx}", new
						{
							id = user.identifiers.discord,
							owner = user.FullName,
							name = p.Name,
							stname = "Una Pompa di Benzina",
							last = now.ToString("yyyy-MM-dd HH:mm:ss"),
							idx = sidx
						});
						user.Bank -= sellprice;
						Log.Printa(LogType.Info, $"Il personaggio {user.FullName} del player {GetPlayerName(p.Handle)} ha pagato {sellprice} per una Stazione di Rifornimento.");
						BaseScript.TriggerEvent("lprp:serverlog", now.ToString("dd/MM/yyyy, HH:mm:ss") + " -- Il personaggio {0} del player {1} ha pagato {2} per una Stazione di Rifornimento.", user.FullName, GetPlayerName(p.Handle), sellprice);
						SendStationsUpdate();
						BaseScript.TriggerClientEvent(p, "lprp:businesses:purchasestation", true, "", sidx, sellprice);
					}
					else
						BaseScript.TriggerClientEvent(p, "lprp:businesses:purchasestation", false, "Non hai abbastanza solti in banca per coprire il costo.");
				}
			}
		}

		public static async void AddMoneyToStation([FromSource] Player p, int stationindex, int Amount)
		{
			int sidx = stationindex;
			int amount = Amount;
			int oldamount = 0;
			int newamount = 0;

			dynamic result = await Server.GetInstance.Query($"SELECT `cashwaiting` FROM `businesses` WHERE `stationindex` = @idx", new { idx = sidx });
			oldamount = result[0].cashwaiting;
			newamount = oldamount + amount;
			await Server.GetInstance.Execute($"UPDATE `businesses` SET `cashwaiting` = @cash WHERE stationindex = @idx", new { cash = newamount, idx = sidx });
		}


		#region COMANDI ADMIN
		public static async void SAddMoney([FromSource] Player p, int closest, int Amount)
		{
			int index = closest;
			int amount = Amount;
			if (index > 0 && Amount > -1)
			{
				dynamic result = await Server.GetInstance.Query($"SELECT `cashwaiting` FROM `businesses` WHERE `stationindex` = @idx", new { idx = index });
				int oldm = result[0].cashwaiting;
				int newm = oldm + amount;
				await Server.GetInstance.Execute($"UPDATE `businesses` SET `cashwaiting` = @cash WHERE `stationindex` = @idx ", new { cash = newm, idx = index });
				BaseScript.TriggerClientEvent(p, "lprp:ShowNotification", "Aggiunti soldi alla stazione. Nuovo saldo: {0}", newm.ToString());
			}
		}

		public static async void SAddFuel([FromSource] Player p, int closest, int Amount)
		{
			int index = closest;
			int amount = Amount;
			if (index > 0 && Amount > -1)
			{
				dynamic result = await Server.GetInstance.Query($"SELECT `fuel` FROM `businesses` WHERE `stationindex` = @idx", new { idx = index });
				int oldf = result[0].fuel;
				int newf = oldf + amount;
				await Server.GetInstance.Execute($"UPDATE `businesses` SET `fuel` = @fuel WHERE `stationindex` = @idx", new
				{
					fuel = newf,
					idx = index
				});
				BaseScript.TriggerClientEvent(p, "lprp:ShowNotification", "Carburante aggiunto. Nuovo livello settato a {0}", newf.ToString());
			}
		}
		#endregion

		public static async void SResetManage([FromSource] Player p, int closest)
		{
			int index = closest;
			if (index > 0)
			{
				await Server.GetInstance.Execute($"UPDATE `businesses` SET `lastmanaged` = @last WHERE `stationindex` = @idx", new
				{
					last = DateTime.MinValue.ToString(),
					idx = index
				});
				BaseScript.TriggerClientEvent(p, "lprp:ShowNotification", "Data di gestione resettata.");
			}
		}

		public static async void AddStationFunds([FromSource] Player p, int manageid, int amount)
		{
			User user = ServerEntrance.PlayerList[p.Handle];
			int money = user.Money;
			if (money >= amount)
			{
				dynamic result = await Server.GetInstance.Query($"SELECT `cashwaiting`, `ownerchar` FROM `businesses` WHERE `stationindex` = @idx", new { idx = manageid });
				if (result.Count > 0)
				{
					if (result[0].ownerchar.ToLower() == user.FullName.ToLower())
					{
						int smoney = (int)result[0].cashwaiting + amount;
						if (smoney > 50000)
							BaseScript.TriggerClientEvent(p, "lprp:businesses:stationfundschange", false, "Non puoi depositare soldi se il tuo saldo è maggiore di ~b~$50,000~w~. Porta un po' di denaro in banca!");
						else
						{
							user.Money -= amount;
							Log.Printa(LogType.Info, $"Il personaggio {user.FullName} del player {GetPlayerName(p.Handle)} ha depositato ${amount} nella sua stazione di rifornimento.");
							BaseScript.TriggerEvent("lprp:serverlog", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + " -- Il personaggio {0} del player {1} ha depositato {2}$ nella sua stazione di rifornimento.", user.FullName, GetPlayerName(p.Handle), amount.ToString());
							await Server.GetInstance.Execute($"UPDATE `businesses` SET `cashwaiting` = @cash WHERE `stationindex` = @idx", new
							{
								cash = smoney,
								idx = manageid
							});
							BaseScript.TriggerClientEvent(p, "lprp:businesses:stationfundschange", true, smoney.ToString());
						}
					}
					else
						BaseScript.TriggerClientEvent(p, "lprp:businesses:stationfundschange", false, "Non hai abbastanza soldi per coprire questa transazione.");
				}
			}
		}

		public static async void RemStationFunds([FromSource] Player p, int manageid, int amount)
		{
			User user = ServerEntrance.PlayerList[p.Handle];
			dynamic result = await Server.GetInstance.Query($"SELECT `cashwaiting`, `ownerchar` FROM `businesses` WHERE `stationindex` = @idx", new { idx = manageid });
			if (result.Count > 0)
			{
				if (result[0].ownerchar.ToLower() == user.FullName.ToLower())
				{
					int samount = (int)result[0].cashwaiting;
					if (samount >= amount)
					{
						samount = (int)result[0].cashwaiting - amount;
						user.Money += amount;
						Log.Printa(LogType.Info, $"Il personaggio {user.FullName} del player {GetPlayerName(p.Handle)} has ritirato ${amount} dalla sua stazione di rifornimento");
						BaseScript.TriggerEvent("lprp:serverlog", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + " -- Il personaggio {0} del player {1} has ritirato {2}$ dalla sua stazione di rifornimento", user.FullName, GetPlayerName(p.Handle), amount.ToString());
						await Server.GetInstance.Execute($"UPDATE businesses SET cashwaiting = @cash  WHERE stationindex = @idx", new
						{
							cash = samount,
							idx = manageid
						});
						BaseScript.TriggerClientEvent(p, "lprp:businesses:stationfundschange", true, samount.ToString());
					}
					else
						BaseScript.TriggerClientEvent(p, "lprp:businesses:stationfundschange", false, "Non hai abbastanza soldi per ritirare quella cifra.");
				}
			}
		}
	}
}
