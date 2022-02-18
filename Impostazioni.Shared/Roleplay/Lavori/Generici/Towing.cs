using System.Collections.Generic;

namespace Impostazioni.Shared.Roleplay.Lavori.Generici
{
    public class Towing
    {
        public Vector3 InizioLavoro { get; set; }
        public List<Vector3> PuntiDespawn { get; set; }
        public List<string> VeicoliDaRimorchiare { get; set; }
        public List<Vector4> SpawnVeicoli { get; set; }
    }
}