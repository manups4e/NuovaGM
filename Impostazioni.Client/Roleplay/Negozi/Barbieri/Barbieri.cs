using System.Collections.Generic;

namespace Impostazioni.Client.Configurazione.Negozi.Barbieri
{
    public class ConfigNegoziBarbieri
    {
	    public BarbieriTesta Femmina { get; set; }
		public BarbieriTesta Maschio { get; set; }
    }
    
    public class BarbieriTesta
    {
        public Capelli capelli = new Capelli();
        public List<Capigliature> trucco = new List<Capigliature>();
        public List<Capigliature> sopr = new List<Capigliature>();
        public List<Capigliature> barba = new List<Capigliature>();
        public List<Capigliature> ross = new List<Capigliature>();
    }

    public class Capigliature
    {
	    public string Name;
	    public string Description;
	    public int var;
	    public int price;
    }
    
    public class Capelli
    {
	    public List<Capigliature> kuts = new List<Capigliature>();
	    public List<Capigliature> osheas = new List<Capigliature>();
	    public List<Capigliature> hawick = new List<Capigliature>();
	    public List<Capigliature> beach = new List<Capigliature>();
	    public List<Capigliature> mulet = new List<Capigliature>();
    }
}