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
			if (Client.Instance.GetPlayers.ToList().Count > 0)
				foreach (Player player in Client.Instance.GetPlayers)
					if (player.GetPlayerData() != null)
					{
						if (player.GetPlayerData().StatiPlayer.Istanza.Stanziato)
						{
							if (player.GetPlayerData().StatiPlayer.Istanza.Stanziato && player != Cache.Player)
							{
								if (player.GetPlayerData().StatiPlayer.Istanza.Instance != string.Empty)
								{
									if (player.GetPlayerData().StatiPlayer.Istanza.ServerIdProprietario != 0 || Cache.Char.StatiPlayer.Istanza.ServerIdProprietario != 0)
									{
										if (player.GetPlayerData().StatiPlayer.Istanza.ServerIdProprietario != Cache.Player.ServerId && Cache.Char.StatiPlayer.Istanza.ServerIdProprietario != player.ServerId)
										{
											if (!NetworkIsPlayerConcealed(player.Handle)) NetworkConcealPlayer(player.Handle, true, true);
										}
										else
										{
											if (player.GetPlayerData().StatiPlayer.Istanza.ServerIdProprietario == Cache.Player.ServerId || Cache.Player.ServerId == player.GetPlayerData().StatiPlayer.Istanza.ServerIdProprietario)
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
						if (player.Character.IsInRangeOf(Cache.Char.posizione.ToVector3(), 30)) HUD.DrawText3D(player.Character.Bones[Bone.SKEL_Head].Position + new Vector3(0, 0, 0.85f), Colors.White, "IN PAUSA");
					}
		}
	}
}