using TheLastPlanet.Client.Core.Utility;

namespace TheLastPlanet.Client
{
    internal static class GestionePlayersDecors
    {

        public static void Init()
        {
            Client.Instance.StateBagsHandler.OnRoleplayStateBagChange += OnRoleplayStateBagChange;
            Client.Instance.StateBagsHandler.OnInstanceBagChange += OnInstanceBagChange;

        }

        private static async void OnInstanceBagChange(int userId, InstanceBag value)
        {
            await PlayerCache.Loaded();
            if (userId != PlayerCache.MyPlayer.Handle)
            {
                var client = Funzioni.GetPlayerClientFromServerId(userId);
                if (client == null || client.User == null || !client.Status.PlayerStates.Spawned) return;
                if (!value.Stanziato)
                {
                    if (NetworkIsPlayerConcealed(client.Player.Handle))
                        NetworkConcealPlayer(client.Player.Handle, false, false);
                    return;
                }
                if (value.Instance != string.Empty)
                {
                    if (value.ServerIdProprietario != 0 || PlayerCache.MyPlayer.Status.Istanza.ServerIdProprietario != 0)
                    {
                        if (value.ServerIdProprietario != PlayerCache.MyPlayer.Player.ServerId && PlayerCache.MyPlayer.Status.Istanza.ServerIdProprietario != client.Handle)
                        {
                            if (!NetworkIsPlayerConcealed(client.Player.Handle))
                                NetworkConcealPlayer(client.Player.Handle, true, true);
                        }
                        else
                        {
                            if (value.ServerIdProprietario == PlayerCache.MyPlayer.Player.ServerId || PlayerCache.MyPlayer.Player.ServerId == value.ServerIdProprietario)
                                if (NetworkIsPlayerConcealed(client.Player.Handle))
                                    NetworkConcealPlayer(client.Player.Handle, false, false);
                        }
                    }
                    else if (NetworkIsPlayerConcealed(client.Player.Handle))
                    {
                        NetworkConcealPlayer(client.Player.Handle, false, false);
                    }
                }
                else if (!NetworkIsPlayerConcealed(client.Player.Handle))
                {
                    NetworkConcealPlayer(client.Player.Handle, true, true);
                }
            }
        }

        private static void OnRoleplayStateBagChange(int userId, string type, bool value)
        {
        }
    }
}