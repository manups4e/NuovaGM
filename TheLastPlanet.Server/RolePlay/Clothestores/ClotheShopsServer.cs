using System;
using TheLastPlanet.Server.Core;

namespace TheLastPlanet.Server.Clothestores
{
    internal static class ClotheShopsServer
    {
        public static void Init()
        {
            Server.Instance.AddEventHandler("lprp:abiti:compra", new Action<Player, int, int>(Compra));
            Server.Instance.AddEventHandler("lprp:barbiere:compra", new Action<Player, int, int>(CompraBrb));
        }

        private static void Compra([FromSource] Player p, int price, int num)
        {
            PlayerClient client = Functions.GetClientFromPlayerId(int.Parse(p.Handle));
            if (client != null)
            {
                if (num == 1)
                    client.User.Money -= price;
                else if (num == 2)
                    client.User.Bank -= price;
                Server.Logger.Info($"The character {client.User.FullName}, belonging to {p.Name} spent {price} in a clothing store");
            }
        }

        private static void CompraBrb([FromSource] Player p, int price, int num)
        {
            PlayerClient client = Functions.GetClientFromPlayerId(int.Parse(p.Handle));
            if (client != null)
            {
                if (num == 1)
                    client.User.Money -= price;
                else if (num == 2)
                    client.User.Bank -= price;
                Server.Logger.Info($"The character {client.User.FullName}, belonging to {p.Name} spent {price} in a barber shop");
            }
        }
    }
}