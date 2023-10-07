using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
#if SERVER
using TheLastPlanet.Server.Core.Buckets;
#endif

namespace TheLastPlanet.Shared
{
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
