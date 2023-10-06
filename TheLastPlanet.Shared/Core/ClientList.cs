using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
#if SERVER
using TheLastPlanet.Server.Core.Buckets;
#endif

namespace TheLastPlanet.Shared
{
    public class GarageData
    {
        /// <summary>
        /// The unique ID of this teleport.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the teleport category.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The vehicle limit for the garage.
        /// </summary>
        public int VehLimit { get; set; }

        /// <summary>
        /// The minimum permission level to enter this garage.
        /// </summary>
        public int MinPermLevel { get; set; }

        /// <summary>
        /// The spawn locations for the ped.
        /// </summary>
        public List<SpawnLocs> EntranceSpawnsPed { get; set; }

        /// <summary>
        /// The spawn locations for the vehicle.
        /// </summary>
        public List<SpawnLocs> EnteranceSpawnsVeh { get; set; }

        /// <summary>
        /// The spawn locations for the ped.
        /// </summary>
        public List<SpawnLocs> ExitSpawnsPed { get; set; }

        /// <summary>
        /// The spawn locations for the vehicle.
        /// </summary>
        public List<SpawnLocs> ExitSpawnsVeh { get; set; }

        /// <summary>
        /// The entrances for the ped.
        /// </summary>
        public List<CoordsRadius> EntranceMarkersPed { get; set; }

        /// <summary>
        /// The entrances for the vehicle.
        /// </summary>
        public List<CoordsRadius> EntranceMarkersVeh { get; set; }

        /// <summary>
        /// The exits for the ped.
        /// </summary>
        public List<CoordsRadius> ExitMarkersPed { get; set; }

        /// <summary>
        /// The entrances for the vehicle.
        /// </summary>
        public List<CoordsRadius> ExitMarkersVeh { get; set; }

        /// <summary>
        /// The no park zones.
        /// </summary>
        public List<NoParkZones> NoParkZones { get; set; }

        public GarageData() { }
    }

    public class CoordsRadius
    {
        /// <summary>
        /// The coords of the teleport.
        /// </summary>
        public Vector3 Coords { get; set; }

        /// <summary>
        /// The radius to check for the player to trigger.
        /// </summary>
        public int RadiusToCheck { get; set; }
        public CoordsRadius() { }
    }

    public class SpawnLocs
    {
        /// <summary>
        /// The coords of the teleport.
        /// </summary>
        public Vector3 Coords { get; set; }

        /// <summary>
        /// The heading of the teleport.
        /// </summary>
        public float Heading { get; set; }
        public SpawnLocs() { }
    }

    public class NoParkZones
    {
        /// <summary>
        /// The start vector of the box.
        /// </summary>
        public Vector3 Start { get; set; }

        /// <summary>
        /// The end vector of the box.
        /// </summary>
        public Vector3 End { get; set; }

        /// <summary>
        /// The width of the box.
        /// </summary>
        public float Width { get; set; }
        public NoParkZones() { }
    }

    public interface IClientList : IEnumerable<PlayerClient>
    {
        void RequestPlayerList();

        void ReceivedPlayerList(IList<object> players);

        Task WaitRequested();
    }

    public class ClientList : IClientList
    {
        private List<PlayerClient> clientList;
        private bool updating = false;
        public ClientList()
        {
            RequestPlayerList();
        }

        public IEnumerator<PlayerClient> GetEnumerator()
        {
            /*
            if (clientList == null)
            {
#if CLIENT
                RequestPlayerList(PlayerCache.ModalitàAttuale);
#elif SERVER
                RequestPlayerList(ModalitaServer.UNKNOWN);
#endif
            }
            */

            foreach (PlayerClient player in clientList)
            {
                yield return player;
            }
        }

        public async void RequestPlayerList()
        {
            updating = true;
#if CLIENT
            clientList = await EventDispatcher.Get<List<PlayerClient>>("tlg:getClients");
#elif SERVER
            clientList = Server.Server.Instance.Clients;
#endif
            updating = false;
        }

        public void ReceivedPlayerList(IList<object> players)
        {

        }
        public async Task WaitRequested()
        {
            if (updating) while (updating) await BaseScript.Delay(0);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (PlayerClient player in clientList)
            {
                yield return player;
            }
        }

        public void Add(PlayerClient pl)
        {
            clientList.Add(pl);
        }

    }
}
