using System.Collections.Generic;

namespace Impostazioni.Client.Configurazione.Negozi.Abiti
{
    public class ConfigNegoziAbiti
    {
	    public Abiti Femmina { get; set; }
	    public Abiti Maschio { get; set; }
    }
    
    public class Abiti
    {
        public List<Completo> BincoVest { get; set; }
		public List<Completo> DiscVest { get; set; }
        public List<Completo> SubVest { get; set; }
        public List<Completo> PonsVest { get; set; }
        public List<Singolo> BincoScarpe { get; set; }
        public List<Singolo> DiscScarpe { get; set; }
        public List<Singolo> SubScarpe { get; set; }
        public List<Singolo> PonsScarpe { get; set; }
        public List<Singolo> BincoPant { get; set; }
        public List<Singolo> DiscPant { get; set; }
        public List<Singolo> SubPant { get; set; }
        public List<Singolo> PonsPant { get; set; }
        public List<Singolo> Occhiali { get; set; }
        public Accessori Accessori { get; set; }
		public Abiti() { }
    }

}