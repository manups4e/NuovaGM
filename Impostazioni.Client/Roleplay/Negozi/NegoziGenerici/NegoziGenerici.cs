using System.Collections.Generic;
using CitizenFX.Core;

namespace Impostazioni.Client.Configurazione.Negozi.Generici
{
    public class ConfigNegoziGenerici
    {
        public List<Vector3> tfs { get; set; }
        public List<Vector3> rq { get; set; }
        public List<Vector3> ltd { get; set; }
        public List<Vector3> armerie { get; set; }
        public OggettiDaVendere OggettiDaVendere { get; set; }
    }

    public class OggettiDaVendere
    {
        public List<OggettoVendita> shared { get; set; }
        public List<OggettoVendita> tfs { get; set; }
        public List<OggettoVendita> rq { get; set; }
        public List<OggettoVendita> ltd { get; set; }
    }

    public class OggettoVendita
    {
        public string oggetto { get; set; }
        public int prezzo{ get; set; }
    }

}