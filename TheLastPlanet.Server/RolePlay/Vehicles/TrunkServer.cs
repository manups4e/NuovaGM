using System.Collections.Concurrent;

namespace TheLastPlanet.Server.Vehicles
{
    static class TrunkServer
    {
        public static ConcurrentDictionary<string, string> GenericTrunks = new ConcurrentDictionary<string, string>();
        public static void Init()
        {
            //	Server.Instance.AddEventHandler("lprp:bagagliaio:getTrunksContents", new Action<Player, string>(GestisciBagagliaio));
        }

    }
}
