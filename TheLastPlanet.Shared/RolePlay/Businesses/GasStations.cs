using System;

namespace TheLastPlanet.Shared
{

    public class GasStations
    {
        public string Identifier { get; set; }
        public string Name { get; set; }
        public int Businessid { get; set; } = 1;
        public DateTime Lastpaidrent { get; set; }
        public string Ownerchar { get; set; }
        public int Stationindex { get; set; }
        public string Stationname { get; set; }
        public int Cashwaiting { get; set; }
        public int Fuel { get; set; }
        public int Fuelprice { get; set; }
        public DateTime Lastmanaged { get; set; }
        public DateTime Lastlogin { get; set; }
        public string Deliverallow { get; set; }
        public string Thanksmessage { get; set; }
        public int Delivertype { get; set; }
        public GasStations() { }
        public GasStations(dynamic data)
        {
            Identifier = data.identifier;
            Name = data.Name;
            Businessid = data.businessid;
            Lastpaidrent = data.lastpaidrent;
            Ownerchar = data.ownerchar;
            Stationindex = data.stationindex;
            Stationname = data.stationname;
            Cashwaiting = data.cashwaiting;
            Fuel = data.fuel;
            Fuelprice = data.fuelprice;
            Lastmanaged = data.lastmanaged;
            Lastlogin = data.lastlogin;
            Deliverallow = data.deliverallow;
            Thanksmessage = data.thanksmessage;
            Delivertype = data.delivertype;
        }
        public GasStations(string id, string stnm, int bid, DateTime lpd, string ownch, int stid, string statnm, int cashwait, int fuel, int fuelp, DateTime lastmana, DateTime lastlog, string deliveral, string thanks, int type)
        {
            Identifier = id;
            Name = stnm;
            Businessid = bid;
            Lastpaidrent = lpd;
            Ownerchar = ownch;
            Stationindex = stid;
            Stationname = statnm;
            Cashwaiting = cashwait;
            Fuel = fuel;
            Fuelprice = fuelp;
            Lastmanaged = lastmana;
            Lastlogin = lastlog;
            Deliverallow = deliveral;
            Thanksmessage = thanks;
            Delivertype = type;
        }
    }
}
