using CitizenFX.Core;
using Logger;
using TheLastPlanet.Server.Core;
using TheLastPlanet.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using Impostazioni.Shared.Configurazione.Generici;
using static CitizenFX.Core.Native.API;
using TheLastPlanet.Server.Core.PlayerChar;

namespace TheLastPlanet.Server.Businesses
{
	internal static class PompeDiBenzinaServer
	{
		private static int stationCapacity = 1500;
		private static float fuelUnownedBuyPrice = 0.9f;
		private static int rentprice = Server.Impostazioni.Main.RentPricePompeDiBenzina;

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
			List<StationDiBenzina> playerstations = new();
			dynamic result = await Server.Instance.Query($"SELECT * FROM `businesses` WHERE businessid = {1}");
			await BaseScript.Delay(0);
			if (result.Count > 0)
				for (int i = 0; i < result.Count; i++)
					playerstations.Add(new StationDiBenzina(result[i]));
			else
				Server.Logger.Error( "BusinessServer.cs - Errore a prendere le stazioni dal database");
			BaseScript.TriggerClientEvent("lprp:businesses:setstations", ConfigShared.SharedConfig.Main.Veicoli.gasstations.ToJson(), playerstations.ToJson());
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
							BaseScript.TriggerClientEvent(Server.Instance.GetPlayers[Convert.ToInt32(p.source)], "lprp:ShowNotification", "La tua stazione di rifornimento è stata ripresa per mancato pagamento.");
						}
						else
						{
							p.Bank -= rentprice;
							await Server.Instance.Execute($"UPDATE businesses SET lastpaidrent = @now  WHERE stationindex = @idx", new { now = timenow, idx = result[i].stationindex });
							BaseScript.TriggerClientEvent(Server.Instance.GetPlayers[Convert.ToInt32(p.source)], "lprp:ShowNotification", "{0} è stato pagato automaticamente per l'affitto della stazione.", rentprice);
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
				Server.Logger.Error( "lprp:businesses:checkcanmanage: errore a ottenere le info stazione o gli identifiers non combaciano");
				p.TriggerEvent("lprp:businesses:checkcanmanage", false);
			}
		}

		public static async void GetStationCash([FromSource] Player p, int sidx)
		{
			User user = Funzioni.GetUserFromPlayerId(p.Handle);
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
					p.TriggerEvent("lprp:ShowNotification", "Non ci sono fondi disponibili per questa stazione.");
				}
			}
			else
			{
				Server.Logger.Error( "lprp:businesses:getstationcash: errore nell'ottenere dati stazione o identifier non combacianti");
			}
		}

		public static async void ChangeStation([FromSource] Player p, string stationname, string thanksmessage, int fuelCost, int Manageid, int Deltype, string Deliverylist)
		{
			string name = stationname;
			string thanks = thanksmessage;
			int fuelcost = (int)fuelCost;
			int manageid = (int)Manageid;
			DateTime lastmanaged = DateTime.Now;
			int deltype = (int)Deltype;
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
				p.TriggerEvent("lprp:ShowNotification", "Le impostazioni della tua stazione sono state aggiornate.");
			}
		}

		public static async void SellStation([FromSource] Player p, string sellname, int Manageid)
		{
			string name = sellname;
			int manageid = (int)Manageid;

			if (name != null)
				foreach (Player a in Server.Instance.GetPlayers.ToList())
				{
					User user = Funzioni.GetUserFromPlayerId(a.Handle);

					if (user.FullName == name)
					{
						await Server.Instance.Execute($"UPDATE `businesses` SET `identifier` = @id, ownerchar = @owner WHERE stationindex = @idx", new { id = user.Identifiers.Discord, owner = user.FullName, idx = manageid });
						SendStationsUpdate();
						p.TriggerEvent("lprp:businesses:sellstation", true, name);
					}
					else
					{
						p.TriggerEvent("lprp:businesses:sellstation", false, "La persona a cui tenti di vendere la stazione non esiste o non è online.");
					}
				}
			else
				p.TriggerEvent("lprp:businesses:sellstation", false, "La persona a cui tenti di vendere la stazione non esiste o non è online.");
		}

		public static async void DepositFuel([FromSource] Player p, int Index, int fuelfortank)
		{
			User user = Funzioni.GetUserFromPlayerId(p.Handle);
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
								p.TriggerEvent("lprp:fuel:depositfuel", false, "Questa stazione accetta consegne solo da personale con contratto.");
							}
						}
						else
						{
							p.TriggerEvent("lprp:fuel:depositfuel", false, "Questa stazione non ha abbastanza denaro per pagarti.");
						}
					}
					else
					{
						p.TriggerEvent("lprp:fuel:depositfuel", false, "Questa stazione è gia al massimo della capienza di carburante.");
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
							Server.Logger.Info( $"Il personaggio {user.FullName} del player {GetPlayerName(p.Handle)} è stato pagato ${pay} per una consegna di carburante (stazione non posseduta).");
							BaseScript.TriggerEvent("lprp:serverlog", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + " -- Il personaggio {0} del player {1} è stato pagato {2}$ per una consegna di carburante (stazione non posseduta).", user.FullName, GetPlayerName(p.Handle), pay);
						}

						p.TriggerEvent("lprp:fuel:depositfuelnotowned", true, (tankerfuel - overflow).ToString(), stationfuel.ToString(), pay.ToString(), overflow.ToString());
					}
					else
					{
						p.TriggerEvent("lprp:fuel:depositfuelnotowned", false, "Questa stazione è già alla massima capienza.");
					}
				}
			}
			else
			{
				Server.Logger.Error( "BusinessServer.cs:373 - lprp:businesses:depositfuel, errore caricamento index e fuel");
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
				User user = Funzioni.GetUserFromPlayerId(p.Handle);
				List<GasStation> stations = ConfigShared.SharedConfig.Main.Veicoli.gasstations;
				GasStation station = stations[sidx];
				int bankmoney = user.Bank;
				dynamic result = await Server.Instance.Query($"SELECT * FROM `businesses` WHERE `stationindex` = @idx", new { idx = sidx });
				await BaseScript.Delay(0);

				if (result[0].identifier != "" && result[0].ownerchar != "" && (result[0].identifier != null && result[0].ownerchar != null))
				{
					p.TriggerEvent("lprp:businesses:purchasestation", false, "Questa stazione è già posseduta.");
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
						Server.Logger.Info( $"Il personaggio {user.FullName} del player {GetPlayerName(p.Handle)} ha pagato {sellprice} per una Stazione di Rifornimento.");
						BaseScript.TriggerEvent("lprp:serverlog", now.ToString("dd/MM/yyyy, HH:mm:ss") + " -- Il personaggio {0} del player {1} ha pagato {2} per una Stazione di Rifornimento.", user.FullName, GetPlayerName(p.Handle), sellprice);
						SendStationsUpdate();
						p.TriggerEvent("lprp:businesses:purchasestation", true, "", sidx, sellprice);
					}
					else
					{
						p.TriggerEvent("lprp:businesses:purchasestation", false, "Non hai abbastanza soldi in banca per coprire il costo.");
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

		#region COMANDI ADMIN

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
				p.TriggerEvent("lprp:ShowNotification", "Aggiunti soldi alla stazione. Nuovo saldo: {0}", newm.ToString());
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
				p.TriggerEvent("lprp:ShowNotification", "Carburante aggiunto. Nuovo livello settato a {0}", newf.ToString());
			}
		}

		#endregion

		public static async void SResetManage([FromSource] Player p, int closest)
		{
			int index = closest;

			if (index > 0)
			{
				await Server.Instance.Execute($"UPDATE `businesses` SET `lastmanaged` = @last WHERE `stationindex` = @idx", new { last = DateTime.MinValue.ToString(), idx = index });
				p.TriggerEvent("lprp:ShowNotification", "Data di gestione resettata.");
			}
		}

		public static async void AddStationFunds([FromSource] Player p, int manageid, int amount)
		{
			User user = Funzioni.GetUserFromPlayerId(p.Handle);
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
							p.TriggerEvent("lprp:businesses:stationfundschange", false, "Non puoi depositare soldi se il tuo saldo è maggiore di $50,000. Porta un po' di denaro in banca!");
						}
						else
						{
							user.Money -= amount;
							Server.Logger.Info( $"Il personaggio {user.FullName} del player {GetPlayerName(p.Handle)} ha depositato ${amount} nella sua stazione di rifornimento.");
							BaseScript.TriggerEvent("lprp:serverlog", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + " -- Il personaggio {0} del player {1} ha depositato {2}$ nella sua stazione di rifornimento.", user.FullName, GetPlayerName(p.Handle), amount.ToString());
							await Server.Instance.Execute($"UPDATE `businesses` SET `cashwaiting` = @cash WHERE `stationindex` = @idx", new { cash = smoney, idx = manageid });
							p.TriggerEvent("lprp:businesses:stationfundschange", true, smoney.ToString());
						}
					}
					else
					{
						p.TriggerEvent("lprp:businesses:stationfundschange", false, "Non hai abbastanza soldi da depositare nella stazione.");
					}
				}
		}

		public static async void RemStationFunds([FromSource] Player p, int manageid, int amount)
		{
			User user = Funzioni.GetUserFromPlayerId(p.Handle);
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
						Server.Logger.Info( $"Il personaggio {user.FullName} del player {GetPlayerName(p.Handle)} has ritirato ${amount} dalla sua stazione di rifornimento");
						BaseScript.TriggerEvent("lprp:serverlog", DateTime.Now.ToString("dd/MM/yyyy, HH:mm:ss") + " -- Il personaggio {0} del player {1} has ritirato {2}$ dalla sua stazione di rifornimento", user.FullName, GetPlayerName(p.Handle), amount.ToString());
						await Server.Instance.Execute($"UPDATE businesses SET cashwaiting = @cash  WHERE stationindex = @idx", new { cash = samount, idx = manageid });
						p.TriggerEvent("lprp:businesses:stationfundschange", true, samount.ToString());
					}
					else
					{
						p.TriggerEvent("lprp:businesses:stationfundschange", false, "La stazione non ha fondi a sufficienza.");
					}
				}
		}
	}
}