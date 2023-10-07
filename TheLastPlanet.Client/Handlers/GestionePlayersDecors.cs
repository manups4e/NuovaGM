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
                if (!value.Instanced)
                {
                    if (NetworkIsPlayerConcealed(client.Player.Handle))
                        NetworkConcealPlayer(client.Player.Handle, false, false);
                    return;
                }
                if (value.Instance != string.Empty)
                {
                    if (value.ServerIdOwner != 0 || PlayerCache.MyPlayer.Status.Instance.ServerIdOwner != 0)
                    {
                        if (value.ServerIdOwner != PlayerCache.MyPlayer.Player.ServerId && PlayerCache.MyPlayer.Status.Instance.ServerIdOwner != client.Handle)
                        {
                            if (!NetworkIsPlayerConcealed(client.Player.Handle))
                                NetworkConcealPlayer(client.Player.Handle, true, true);
                        }
                        else
                        {
                            if (value.ServerIdOwner == PlayerCache.MyPlayer.Player.ServerId || PlayerCache.MyPlayer.Player.ServerId == value.ServerIdOwner)
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