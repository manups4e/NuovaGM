using System.Collections.Generic;

namespace Settings.Client.Configurazione.Negozi.Barbieri
{
    public class ConfigBarberShops
    {
        public HeadBarbers Female { get; set; }
        public HeadBarbers Male { get; set; }
    }

    public class HeadBarbers
    {
        public Hair Hair = new Hair();
        public List<BarberStyle> Makeup = new List<BarberStyle>();
        public List<BarberStyle> Eyebrows = new List<BarberStyle>();
        public List<BarberStyle> Beard = new List<BarberStyle>();
        public List<BarberStyle> Lipstick = new List<BarberStyle>();
    }

    public class BarberStyle
    {
        public string Name;
        public string Description;
        public int var;
        public int price;
    }

    public class Hair
    {
        public List<BarberStyle> kuts = new();
        public List<BarberStyle> osheas = new();
        public List<BarberStyle> hawick = new();
        public List<BarberStyle> beach = new();
        public List<BarberStyle> mulet = new();
    }
}