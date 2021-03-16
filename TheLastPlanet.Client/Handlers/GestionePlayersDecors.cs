using System.Linq;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using TheLastPlanet.Client.Core.Utility.HUD;
using TheLastPlanet.Client.MenuNativo;
using System.Threading.Tasks;
using TheLastPlanet.Client.Core.Utility;
using TheLastPlanet.Shared;
using TheLastPlanet.Client.Core;

namespace TheLastPlanet.Client
{
	internal static class GestionePlayersDecors
	{
		public static async Task GestioneDecors()
		{
			await SessionCache.Cache.Loaded();
			if (ClientSession.Instance.GetPlayers.ToList().Count > 0)
				foreach (var player in ClientSession.Instance.GetPlayers)
					if (player.GetPlayerData() != null)
					{
						if (player.GetPlayerData().StatiPlayer.Istanza.Stanziato)
						{
							if (player.GetPlayerData().StatiPlayer.Istanza.Stanziato && player != SessionCache.Cache.MyPlayer.Player)
							{
								if (player.GetPlayerData().StatiPlayer.Istanza.Instance != string.Empty)
								{
									if (player.GetPlayerData().StatiPlayer.Istanza.ServerIdProprietario != 0 || SessionCache.Cache.MyPlayer.User.StatiPlayer.Istanza.ServerIdProprietario != 0)
									{
										if (player.GetPlayerData().StatiPlayer.Istanza.ServerIdProprietario != SessionCache.Cache.MyPlayer.Player.ServerId && SessionCache.Cache.MyPlayer.User.StatiPlayer.Istanza.ServerIdProprietario != player.ServerId)
										{
											if (!NetworkIsPlayerConcealed(player.Handle)) NetworkConcealPlayer(player.Handle, true, true);
										}
										else
										{
											if (player.GetPlayerData().StatiPlayer.Istanza.ServerIdProprietario == SessionCache.Cache.MyPlayer.Player.ServerId || SessionCache.Cache.MyPlayer.Player.ServerId == player.GetPlayerData().StatiPlayer.Istanza.ServerIdProprietario)
												if (NetworkIsPlayerConcealed(player.Handle))
													NetworkConcealPlayer(player.Handle, false, false);
										}
									}
									else
									{
										if (NetworkIsPlayerConcealed(player.Handle)) NetworkConcealPlayer(player.Handle, false, false);
									}
								}
								else
								{
									if (!NetworkIsPlayerConcealed(player.Handle)) NetworkConcealPlayer(player.Handle, true, true);
								}
							}
							else
							{
								if (NetworkIsPlayerConcealed(player.Handle)) NetworkConcealPlayer(player.Handle, false, false);
							}
						}

						if (!player.GetPlayerData().StatiPlayer.InPausa) continue;
						if (player.Character.IsInRangeOf(SessionCache.Cache.MyPlayer.User.posizione.ToVector3(), 30)) HUD.DrawText3D(player.Character.Bones[Bone.SKEL_Head].Position + new Vector3(0, 0, 0.85f), Colors.White, "IN PAUSA");
					}
		}
	}
}