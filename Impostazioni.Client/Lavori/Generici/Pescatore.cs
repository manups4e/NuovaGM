using System.Collections.Generic;
using CitizenFX.Core;

namespace Impostazioni.Client.Configurazione.Lavori.Generici
{
    public class Pescatori
    {
        public bool TempoPescaDinamico = true;
        public int TempoFisso = 10;
        public int PrezzoVenditaPesce = 10;
        public int PrezzoVenditaAltro = 20;
        public List<Vector3> LuoghiVendita = new()
        {
            new Vector3(-3251.2f, 991.5f, 11.49f),
            new Vector3(3804.0f, 4443.3f, 3.0f),
            new Vector3(2517.6f, 4218.0f, 38.8f),
            new Vector3(-1.827f, 6488.367f, 31.506f),
            new Vector3(-1552.16f, -966.732f, 13.017f),
            new Vector3(-1017.513f, -1350.519f, 5.473f)
        };

        public Vector4 SpawnBarca = new Vector4(3856.264f, 4455.604f, 1.405f, 268.4614f);
        public List<string> Barche = new List<string>()
        {
            //"TUG", //-- non sono sicuro di voler dare il tug (barcone immenso)
            "DINGHY"

        };
        public PesciPescati Pesci = new PesciPescati()
        {
            facile = new List<string>() { "orata", "sogliola", "branzino" },
            medio = new List<string>() { "orata", "sogliola", "branzino", "sogliola" },
            avanzato = new List<string>() { "orata", "sogliola", "branzino", "tonno", "pescespada" },
        };
    }
    
    public class PesciPescati
    {
        public List<string> facile = new List<string>();
        public List<string> medio = new List<string>();
        public List<string> avanzato = new List<string>();
    }

}