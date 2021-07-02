using System;
using TheLastPlanet.Shared.Internal.Events.Attributes;

namespace TheLastPlanet.Shared
{
	[Serialization]
	public partial class StationDiBenzina
	{
		public string identifier { get; set; }
		public string Name { get; set; }
		public int businessid { get; set; } = 1;
		public DateTime lastpaidrent { get; set; }
		public string ownerchar { get; set; }
		public int stationindex { get; set; }
		public string stationname { get; set; }
		public int cashwaiting { get; set; }
		public int fuel { get; set; }
		public int fuelprice { get; set; }
		public DateTime lastmanaged { get; set; }
		public DateTime lastlogin { get; set; }
		public string deliverallow { get; set; }
		public string thanksmessage { get; set; }
		public int delivertype { get; set; }
		public StationDiBenzina() { }
		public StationDiBenzina(dynamic data)
		{
			identifier = data.identifier;
			Name = data.Name;
			businessid = data.businessid;
			lastpaidrent = data.lastpaidrent;
			ownerchar = data.ownerchar;
			stationindex = data.stationindex;
			stationname = data.stationname;
			cashwaiting = data.cashwaiting;
			fuel = data.fuel;
			fuelprice = data.fuelprice;
			lastmanaged = data.lastmanaged;
			lastlogin = data.lastlogin;
			deliverallow = data.deliverallow;
			thanksmessage = data.thanksmessage;
			delivertype = data.delivertype;
		}
		public StationDiBenzina(string id, string stnm, int bid, DateTime lpd, string ownch, int stid, string statnm, int cashwait, int fuel, int fuelp, DateTime lastmana, DateTime lastlog, string deliveral, string thanks, int type)
		{
			identifier = id;
			Name = stnm;
			businessid = bid;
			lastpaidrent = lpd;
			ownerchar = ownch;
			stationindex = stid;
			stationname = statnm;
			cashwaiting = cashwait;
			this.fuel = fuel;
			fuelprice = fuelp;
			lastmanaged = lastmana;
			lastlogin = lastlog;
			deliverallow = deliveral;
			thanksmessage = thanks;
			delivertype = type;
		}
	}
}
