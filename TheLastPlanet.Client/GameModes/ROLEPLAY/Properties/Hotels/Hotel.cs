namespace TheLastPlanet.Client.GameMode.ROLEPLAY.Properties.Hotel
{
    public class Hotel
    {
        public string Name;
        public Vector3 Coords;
        public HotelPrices Prices = new HotelPrices();
    }

    public class HotelPrices
    {
        public int SmallRoom;
        public int MidRoom;
        public int Apartment;
    }
}
