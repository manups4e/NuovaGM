using System.Collections.Generic;
using CitizenFX.Core;
using TheLastPlanet.Shared.Internal.Events.Attributes;

namespace Impostazioni.Shared.Configurazione.Generici
{
    [Serialization]
    public partial class GasStation
    {
        public Vector3 pos;
        public List<Vector3> pumps = new List<Vector3>();
        public Vector3 ppos;
        public int sellprice;

        public GasStation()
        {
        }

        public GasStation(dynamic data)
        {
            pos = new Vector3((float) data["pos"][0].Value, (float) data["pos"][1].Value, (float) data["pos"][2].Value);
            ppos = new Vector3((float) data["ppos"][0].Value, (float) data["ppos"][1].Value,
                (float) data["ppos"][2].Value);
            for (int i = 0; i < data["pumps"].Count; i++)
                pumps.Add(new Vector3((float) data["pumps"][i][0].Value, (float) data["pumps"][i][1].Value,
                    (float) data["pumps"][i][2].Value));
            sellprice = (int) data["sellprice"].Value;
        }
    }
}