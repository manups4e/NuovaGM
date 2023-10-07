using System.Collections.Generic;

namespace Settings.Server.Configurazione.Coda
{
    public class ConfigQueue
    {
        public Dictionary<string, string> Messages { get; set; }
        public List<string> Permissions { get; set; }
        public bool Whitelistonly { get; set; }
        public int LoadTime { get; set; }
        public int GraceTime { get; set; }
        public int QueueGraceTime { get; set; }
        public bool AddCountAfterServerName { get; set; }
        public bool AllowSymbols { get; set; }
        public bool StateChangeMessages { get; set; }
    }

}