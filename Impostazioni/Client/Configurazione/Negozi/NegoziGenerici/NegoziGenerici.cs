using System.Collections.Generic;
using CitizenFX.Core;

namespace Impostazioni.Client.Configurazione.Negozi.Generici
{
    public class ConfigNegoziGenerici
    {
        public List<Vector3> tfs = new List<Vector3>()
        {
            new(373.875f, 325.896f, 102.566f),
            new(2557.458f, 382.282f, 107.622f),
            new(-3038.939f, 585.954f, 6.908f),
            new(-3241.927f, 1001.462f, 11.830f),
            new(547.431f, 2671.710f, 41.156f),
            new(1961.464f, 3740.672f, 31.343f),
            new(2678.916f, 3280.671f, 54.241f),
            new(1729.216f, 6414.131f, 34.037f),
        };

        public List<Vector3> rq = new List<Vector3>()
        {
            new(1135.808f, -982.281f, 45.415f),
            new(-1222.915f, -906.983f, 11.326f),
            new(-1487.553f, -379.107f, 39.163f),
            new(-2968.243f, 390.910f, 14.043f),
            new(1166.024f, 2708.930f, 37.157f),
            new(1392.562f, 3604.684f, 33.980f),
        };

        public List<Vector3> ltd = new List<Vector3>()
        {
            new(-48.519f, -1757.514f, 28.421f),
            new(1163.373f, -323.801f, 68.205f),
            new(-707.501f, -914.260f, 18.215f),
            new(-1820.523f, 792.518f, 137.118f),
            new(1698.388f, 4924.404f, 41.06f),
        };

        public List<Vector3> armerie = new List<Vector3>()
        {
            new(-662.1f, -935.3f, 20.8f),
            new(810.2f, -2157.3f, 28.6f),
            new(1693.4f, 3759.5f, 33.7f),
            new(-330.2f, 6083.8f, 30.4f),
            new(252.3f, -50.0f, 68.9f),
            new(22.0f, -1107.2f, 28.8f),
            new(2567.6f, 294.3f, 107.7f),
            new(-1117.5f, 2698.6f, 17.5f),
            new(842.4f, -1033.4f, 27.1f),
            new(-1306.2f, -394.0f, 35.6f),
        };

        public OggettiDaVendere OggettiDaVendere = new OggettiDaVendere()
        {
            shared = new List<OggettoVendita>()
            {
                new() {oggetto = "hamburger", prezzo = 3},
                new() {oggetto = "acqua", prezzo = 2},
                new() {oggetto = "cannadapescabase", prezzo = 1200},
            }
        };
    }

    public class OggettiDaVendere
    {
        public List<OggettoVendita> shared = new List<OggettoVendita>();
        public List<OggettoVendita> tfs = new List<OggettoVendita>();
        public List<OggettoVendita> rq = new List<OggettoVendita>();
        public List<OggettoVendita> ltd = new List<OggettoVendita>();
    }

    public class OggettoVendita
    {
        public string oggetto;
        public int prezzo;
    }

}