using System.Collections.Generic;

namespace Impostazioni.Client.Configurazione.Lavori.Generici
{
    public class Tassisti
    {
        public Vector3 PosAccettazione { get; set; }
        public Vector3 PosRitiroVeicolo { get; set; }
        public Vector3 PosDepositoVeicolo { get; set; }
        public Vector4 PosSpawnVeicolo { get; set; }
        public float PrezzoModifier { get; set; }
        public float pickupRange { get; set; }
        public List<Vector3> jobCoords { get; set; }
    }
}