using System.Linq;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.NativeUI;
using System.Threading.Tasks;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Shared;

namespace TheLastPlanet.Client
{
	internal static class GestionePlayersDecors
	{
		private static int checkTimer1 = 0;

		public static async Task GestioneDecors()
		{
			if (Cache.PlayerCache.GiocatoriOnline.Count < 2) return;

			if (Game.GameTime - checkTimer1 > 250)
				foreach (var client in Cache.PlayerCache.GiocatoriOnline.Where(x=>x.Handle != Cache.PlayerCache.MyPlayer.Handle))
				{
					if (client == Cache.PlayerCache.MyPlayer) return;

					if (client.User == null) continue;
					if (!client.ClientStateBags.Istanza.Stanziato)
					{
						if (NetworkIsPlayerConcealed(client.Player.Handle))
							NetworkConcealPlayer(client.Player.Handle, false, false);
						continue;
					}

					if (client.ClientStateBags.Istanza.Instance != string.Empty)
					{
						if (client.ClientStateBags.Istanza.ServerIdProprietario != 0 || Cache.PlayerCache.MyPlayer.User.StatiPlayer.Istanza.ServerIdProprietario != 0)
						{
							if (client.ClientStateBags.Istanza.ServerIdProprietario != Cache.PlayerCache.MyPlayer.Player.ServerId && Cache.PlayerCache.MyPlayer.User.StatiPlayer.Istanza.ServerIdProprietario != client.Handle)
							{
								if (!NetworkIsPlayerConcealed(client.Player.Handle)) NetworkConcealPlayer(client.Player.Handle, true, true);
							}
							else
							{
								if (client.ClientStateBags.Istanza.ServerIdProprietario == Cache.PlayerCache.MyPlayer.Player.ServerId || Cache.PlayerCache.MyPlayer.Player.ServerId == client.ClientStateBags.Istanza.ServerIdProprietario)
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

					/*
					if (!player.User.StatiPlayer.InPausa) continue;
					if (player.Character.IsInRangeOf(Cache.MyPlayer.User.posizione.ToVector3, 30))
						HUD.DrawText3D(player.Character.Bones[Bone.SKEL_Head].Position + new Vector3(0, 0, 0.85f), Colors.White, "IN PAUSA");
					*/
					checkTimer1 = Game.GameTime;
				}
		}
	}
}