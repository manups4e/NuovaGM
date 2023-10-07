using System.Collections.Generic;

namespace Settings.Client.Configurazione.Negozi.Abiti
{
    public class ConfigClotheShops
    {
        public Apparel Female { get; set; }
        public Apparel Male { get; set; }
    }

    public class Apparel
    {
        public List<Suit> BincoSuit { get; set; }
        public List<Suit> DiscSuit { get; set; }
        public List<Suit> SubSuit { get; set; }
        public List<Suit> PonsSuit { get; set; }
        public List<Single> BincoShoes { get; set; }
        public List<Single> DiscShoes { get; set; }
        public List<Single> SubShoes { get; set; }
        public List<Single> PonsShoes { get; set; }
        public List<Single> BincoPants { get; set; }
        public List<Single> DiscPants { get; set; }
        public List<Single> SubPants { get; set; }
        public List<Single> PonsPants { get; set; }
        public List<Single> Glasses { get; set; }
        public Accessories Accessories { get; set; }
    }

}