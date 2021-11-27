using System.Linq;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using TheLastPlanet.Client.Core.Utility.HUD;
using ScaleformUI;
using System.Threading.Tasks;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Shared;
using System;
using TheLastPlanet.Client.Cache;

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
				var client = Funzioni.GetClientIdFromServerId(userId);
				if (client.User is null || !client.User.Status.Spawned) return;
				if (!value.Stanziato)
				{
					if (NetworkIsPlayerConcealed(client.Player.Handle))
						NetworkConcealPlayer(client.Player.Handle, false, false);
					return;
				}
				if (value.Instance != string.Empty)
				{
					if (value.ServerIdProprietario != 0 || PlayerCache.MyPlayer.User.Status.Istanza.ServerIdProprietario != 0)
					{
						if (value.ServerIdProprietario != PlayerCache.MyPlayer.Player.ServerId && PlayerCache.MyPlayer.User.Status.Istanza.ServerIdProprietario != client.Handle)
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

        private static int checkTimer1 = 0;

		public static async Task GestioneDecors()
		{
			/*
			if (Cache.PlayerCache.GiocatoriOnline.Count is 1) return;
			if (Game.GameTime - checkTimer1 > 250)
            {
                foreach (var client in Cache.PlayerCache.GiocatoriOnline.Where(x=>x.Handle != Cache.PlayerCache.MyPlayer.Handle))
				{
					if (client == Cache.PlayerCache.MyPlayer) return;

					if (client.User is null || !client.User.Status.Spawned) continue;
					if (!client.User.Status.Istanza.Stanziato)
					{
						if (NetworkIsPlayerConcealed(client.Player.Handle))
							NetworkConcealPlayer(client.Player.Handle, false, false);
						continue;
					}

					if (client.User.Status.Istanza.Instance != string.Empty)
					{
						if (client.User.Status.Istanza.ServerIdProprietario != 0 || Cache.PlayerCache.MyPlayer.User.Status.Istanza.ServerIdProprietario != 0)
						{
							if (client.User.Status.Istanza.ServerIdProprietario != Cache.PlayerCache.MyPlayer.Player.ServerId && Cache.PlayerCache.MyPlayer.User.Status.Istanza.ServerIdProprietario != client.Handle)
							{
								if (!NetworkIsPlayerConcealed(client.Player.Handle)) NetworkConcealPlayer(client.Player.Handle, true, true);
							}
							else
							{
								if (client.User.Status.Istanza.ServerIdProprietario == Cache.PlayerCache.MyPlayer.Player.ServerId || Cache.PlayerCache.MyPlayer.Player.ServerId == client.User.Status.Istanza.ServerIdProprietario)
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

					checkTimer1 = Game.GameTime;
				}
            }
			*/
		}
	}
}