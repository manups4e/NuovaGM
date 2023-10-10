using System;
using System.Threading.Tasks;

namespace TheLastPlanet.Server.Jobs.Generici.Rimozione
{
    static class TowingServer
    {
        public static void Init()
        {
            //Server.Instance.AddTick(AggiornaVeicoli);
            Server.Instance.AddEventHandler("lprp:AggiornaVeicoliRimossi", new Action<dynamic>(AggiornaVeicoliRimossi));
        }

        private static void AggiornaVeicoliRimossi(dynamic data)
        {

        }

        public static async Task AggiornaVeicoli()
        {
        }
    }
}
