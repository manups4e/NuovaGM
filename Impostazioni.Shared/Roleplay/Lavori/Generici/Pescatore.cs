using System.Collections.Generic;

namespace Impostazioni.Shared.Roleplay.Lavori.Generici
{
    public class Pescatori
    {
        public bool TempoPescaDinamico { get; set; }
        public int TempoFisso { get; set; }
        public int PrezzoVenditaPesce { get; set; }
        public int PrezzoVenditaAltro { get; set; }
        public List<Vector3> LuoghiVendita { get; set; }
        public Vector4 SpawnBarca { get; set; }
        public List<string> Barche { get; set; }
        public PesciPescati Pesci { get; set; }
    }

    public class PesciPescati
    {
        public List<string> facile = new List<string>();
        public List<string> medio = new List<string>();
        public List<string> avanzato = new List<string>();
    }

}