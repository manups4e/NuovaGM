using System.Collections.Generic;

namespace Impostazioni.Server.Configurazione.Coda
{
    public class ConfigCoda
    {
        public Dictionary<string, string> messages { get; set; }
        public List<string> permessi { get; set; }
        public bool whitelistonly { get; set; }
        public int loadTime { get; set; }
        public int graceTime { get; set; }
        public int queueGraceTime { get; set; }
        public bool contoCodaFineNomeServer { get; set; }
        public bool allowSymbols { get; set; }
        public bool stateChangeMessages { get; set; }
    }

}