namespace TheLastPlanet.Client.MODALITA.ROLEPLAY.Proprietà.Hotel
{
    public class Hotel
    {
        public string Name;
        public Vector3 Coords;
        public PrezzoHotel Prezzi = new PrezzoHotel();
    }

    public class PrezzoHotel
    {
        public int StanzaPiccola;
        public int StanzaMedia;
        public int Appartamento;
    }
}
